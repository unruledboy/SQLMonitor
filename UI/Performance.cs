using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.UI
{
    public partial class Performance : UserControl
    {
        private ObjectModes _objectMode;
        private ServerInfo _server;
        private MonitorEngine _engine;
        private bool _isDocked = true;
        private int _lastPointIndex = 0;
        private List<HistoryRecord> _currentRecords = null;
        private bool _isLoading = false;

        public Performance()
        {
            InitializeComponent();

            Enum.GetValues(typeof(DateTypes)).Cast<DateTypes>().ForEach((s) => cboPerformanceViewTypes.Items.Add(s));
            cboPerformanceViewTypes.SelectedItem = DateTypes.Day;

            dtpPerformanceStartDate.Value = DateTime.Now.Date;
        }

        public void Init(ObjectModes objectMode, ServerInfo server)
        {
            _isLoading = true;
            _objectMode = objectMode;
            _server = server;
            if (_server != null)
                chkAutoPerformance.Checked = Settings.Instance.PerformanceItems.Exists(p => p.Server == _server.Server
                    && p.Database == _server.Database && p.IsServer == IsServer);
            _isLoading = false;
            StartEngine();
        }

        public void RemovePerformanceItem()
        {
            MonitorEngine.Instance.RemoveUserPerformanceItem(_server, IsServer);
        }

        private void StartEngine()
        {
            if (_engine == null)
            {
                _engine = new MonitorEngine();
                _engine.Message += OnMonitorEngineMessage;
                _engine.RequestPerformanceServer += OnMonitorEngineRequestServer;
                _engine.UpdateServerInfo += OnMonitorUpdateServerInfo;
            }
        }

        internal void ShowPopDock()
        {
            cmdPopDock.Visible = true;
        }

        private bool IsServer
        {
            get { return _objectMode == ObjectModes.Server; }
        }

        internal string Title
        {
            get
            {
                var name = History.GetKey(_server, IsServer);
                return "Performance (" + name + ")";
            }
        }

        internal ObjectModes ObjectMode
        {
            get { return _objectMode; }
        }

        internal ServerInfo Server
        {
            get { return _server; }
        }

        private void OnPerformanceCursorPositionChanged(object sender, CursorEventArgs e)
        {
            if (chPerformance.Series.Count > 0 && _currentRecords.Count > 0)
            {
                var index = _currentRecords.FindIndex(p => p.Value16.ToOADate() >= e.NewPosition);
                if (_lastPointIndex != index && index >= 0)
                {
                    _lastPointIndex = index;
                    SetPerformanceInfo();
                }
                Debug.WriteLine(e.NewPosition);
            }
        }

        private void OnMonitorEngineRequestServer(object sender, ServerInfoEventArgs e)
        {
            e.IsServer = _objectMode == ObjectModes.Server;
            e.Server = _server;
            e.Cancel = _server == null;
        }

        internal void SetInterval(string interval)
        {
            StartEngine();
            _engine.SetPerformanceInterval(interval);
        }

        private void OnMonitorEngineMessage(object sender, MessageEventArgs e)
        {
            this.Invoke(() =>
            {
                if (e.Cancel)
                {
                    _engine.DisablePerformance();
                }
                Monitor.Instance.ShowMessage(e.Message);
            });
        }

        internal void ResetPerformance()
        {
            ResetPerformance(true);
        }

        private void ResetPerformance(bool isNew)
        {
            chPerformance.Series.Clear();
            ResetPerformanceIo();
            txtPerformanceIO.Text = string.Empty;
            txtPerformanceCPU.Text = string.Empty;
            txtPerformanceRead.Text = string.Empty;
            txtPerformanceWrite.Text = string.Empty;
            txtPerformancePackets.Text = string.Empty;
            txtPerformanceConnections.Text = string.Empty;
            if (_objectMode == ObjectModes.Databases)
            {
                llPerformanceIO.Text = "Stall";
                lblPerformanceCPU.Text = "Since";
                ttInfo.SetToolTip(txtPerformanceCPU, "Calculation since last startup");
                lblPerformanceRead.Text = "DB read";
                ttInfo.SetToolTip(txtPerformanceRead, "Number of reads / Total reads");
                lblPerformanceWrite.Text = "DB write";
                ttInfo.SetToolTip(txtPerformanceWrite, "Number of writes / Total writes");
                lblPerformancePacket.Text = "Log read";
                ttInfo.SetToolTip(txtPerformancePackets, "Number of reads / Total reads");
                lblPerformanceConnection.Text = "Log write";
                ttInfo.SetToolTip(txtPerformanceConnections, "Number of writes / Total writes");
            }
            else if (_objectMode == ObjectModes.Server)
            {
                llPerformanceIO.Text = "IO";
                ttInfo.SetToolTip(txtPerformanceIO, "Total IO time / Current IO %");
                lblPerformanceCPU.Text = "CPU";
                ttInfo.SetToolTip(txtPerformanceCPU, "Total CPU time / Current CPU %");
                lblPerformanceRead.Text = "Read";
                ttInfo.SetToolTip(txtPerformanceRead, "Total number of reads since last startup/ Recent number of reads");
                lblPerformanceWrite.Text = "Write";
                ttInfo.SetToolTip(txtPerformanceWrite, "Total number of writes since last startup/ Recent number of writes");
                lblPerformancePacket.Text = "Packet";
                ttInfo.SetToolTip(txtPerformancePackets, "Total number of packets received / Total number of packets sent");
                lblPerformanceConnection.Text = "Connection";
                ttInfo.SetToolTip(txtPerformanceConnections, "Total connection count / Recent connection count");
            }
            if (isNew)
                chkShowPerformanceHistory.Checked = false;
            _lastPointIndex = 0;
            _currentRecords = new List<HistoryRecord>();
        }

        private void ResetPerformanceIo()
        {
            txtPerformanceIO.BackColor = SystemColors.Control;
            ttInfo.SetToolTip(txtPerformanceIO, "DB stall / Log stall");
            llPerformanceIO.Enabled = false;
        }

        private Series AddPerformanceSerie(string name, Color color)
        {
            var serie = new Series(name);
            serie.ChartArea = "Default";
            serie.BorderWidth = 1;
            serie.Color = color;
            serie.ChartType = SeriesChartType.Line;
            serie.ShadowOffset = 1;
            serie.XValueType = ChartValueType.Time;
            //serie.LabelFormat = "HH:MM:SS";
            chPerformance.Series.Add(serie);
            return serie;
        }

        private void AddPerformanceValue(DateTime timeStamp, Series serie, long value)
        {
            var current = timeStamp.ToOADate();

            if (current < chPerformance.ChartAreas[0].AxisX.Minimum)
                serie.Points.Clear();

            serie.Points.AddXY(current, value);
            //Serie.Points[Serie.Points.Count - 1].LabelFormat = "HH:MM:SS";

            if (!chkShowPerformanceHistory.Checked)
            {
                var removeBefore = timeStamp.AddSeconds(-60).ToOADate();
                while (serie.Points[0].XValue < removeBefore)
                {
                    serie.Points.RemoveAt(0);
                }
                chPerformance.ChartAreas[0].AxisX.Minimum = serie.Points[0].XValue;
                chPerformance.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(serie.Points[0].XValue).AddMinutes(1).ToOADate();
            }

            chPerformance.Invalidate();
        }

        private void AddPerformanceHistoryRecord(PerformanceRecord record, DateTime date, DateTime min, DateTime max)
        {
            if (_objectMode == ObjectModes.Server)
            {
                var cpuBusyCurrent = record.Value1;
                var ioBusyCurrent = record.Value2;
                var currentRead = record.Value3;
                var currentWrite = record.Value4;
                var packetsReceivedCurrent = record.Value5;
                var packetsSentCurrent = record.Value6;
                var connectionsCurrent = record.Value7;
                Series cpu;
                Series io;
                Series read;
                Series write;
                Series packetsSent;
                Series packetsReceived;
                Series connections;
                if (chPerformance.Series.Count == 0)
                {
                    chPerformance.ChartAreas[0].AxisX.Minimum = min.ToOADate();
                    chPerformance.ChartAreas[0].AxisX.Maximum = max.ToOADate();

                    cpu = AddPerformanceSerie("CPU", Color.Green);
                    io = AddPerformanceSerie("IO", Color.Blue);
                    read = AddPerformanceSerie("Read", Color.Red);
                    write = AddPerformanceSerie("Write", Color.Salmon);
                    packetsReceived = AddPerformanceSerie("Packet Received", Color.Tan);
                    packetsSent = AddPerformanceSerie("Packet Sent", Color.Pink);
                    connections = AddPerformanceSerie("Connections", Color.Yellow);
                }
                else
                {
                    cpu = chPerformance.Series[0];
                    io = chPerformance.Series[1];
                    read = chPerformance.Series[2];
                    write = chPerformance.Series[3];
                    packetsReceived = chPerformance.Series[4];
                    packetsSent = chPerformance.Series[5];
                    connections = chPerformance.Series[6];
                }
                AddPerformanceValue(date, cpu, cpuBusyCurrent);
                AddPerformanceValue(date, io, ioBusyCurrent);
                AddPerformanceValue(date, read, currentRead);
                AddPerformanceValue(date, write, currentWrite);
                AddPerformanceValue(date, packetsReceived, packetsReceivedCurrent);
                AddPerformanceValue(date, packetsSent, packetsSentCurrent);
                AddPerformanceValue(date, connections, connectionsCurrent);
            }
            else
            {
                var dbCurrentNumberReads = record.Value5;
                var dbCurrentNumberWrites = record.Value6;

                var logCurrentNumberReads = record.Value11;
                var logCurrentNumberWrites = record.Value12;

                Series dbReads;
                Series dbWrites;
                Series logReads;
                Series logWrites;
                if (chPerformance.Series.Count == 0)
                {
                    chPerformance.ChartAreas[0].AxisX.Minimum = min.ToOADate();
                    chPerformance.ChartAreas[0].AxisX.Maximum = max.ToOADate();

                    dbReads = AddPerformanceSerie("DB Read", Color.Red);
                    dbWrites = AddPerformanceSerie("DB Write", Color.Salmon);
                    logReads = AddPerformanceSerie("Log Read", Color.Green);
                    logWrites = AddPerformanceSerie("Log Write", Color.Blue);
                }
                else
                {
                    dbReads = chPerformance.Series[0];
                    dbWrites = chPerformance.Series[1];
                    logReads = chPerformance.Series[2];
                    logWrites = chPerformance.Series[3];
                }
                AddPerformanceValue(date, dbReads, dbCurrentNumberReads);
                AddPerformanceValue(date, dbWrites, dbCurrentNumberWrites);
                AddPerformanceValue(date, logReads, logCurrentNumberReads);
                AddPerformanceValue(date, logWrites, logCurrentNumberWrites);
            }
        }

        private void OnMonitorUpdateServerInfo(object sender, PerformanceRecordEventArgs e)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    if (!chkShowPerformanceHistory.Checked)
                    {
                        try
                        {
                            var record = e.Data;
                            var now = DateTime.Now;
                            var historyRecord = new HistoryRecord(record) { Date = now.ToString(), Value16 = now };
                            _currentRecords.Add(historyRecord);
                            AddPerformanceHistoryRecord(record, now, now, now.AddMinutes(1));
                            SetPerformanceInfo(record);
                        }
                        catch (Exception ex)
                        {
                            _engine.DisablePerformance();
                            Monitor.Instance.ShowMessage(ex.Message);
                        }
                    }
                });
            }
        }

        private void SetPerformanceInfo(PerformanceRecord record)
        {
            _isLoading = true;
            if (_objectMode == ObjectModes.Server)
            {
                var cpuBusyCurrent = record.Value1;
                var ioBusyCurrent = record.Value2;
                var currentRead = record.Value3;
                var currentWrite = record.Value4;
                var connectionsCurrent = record.Value7;
                var ioBusyTotal = record.Value8;
                txtPerformanceIO.Text = string.Format("{0} / {1}", ioBusyTotal, ioBusyCurrent);

                var cpuBusyTotal = record.Value9;
                txtPerformanceCPU.Text = string.Format("{0} / {1}", cpuBusyTotal, cpuBusyCurrent);

                var totalRead = record.Value10;
                txtPerformanceRead.Text = string.Format("{0} / {1}", totalRead, currentRead);

                var totalWrite = record.Value11;
                txtPerformanceWrite.Text = string.Format("{0} / {1}", totalWrite, currentWrite);

                var packetsReceivedTotal = record.Value12;
                var packetsSentTotal = record.Value13;
                txtPerformancePackets.Text = string.Format("{0} / {1}", packetsReceivedTotal, packetsSentTotal);

                var connectionsTotal = record.Value14;
                txtPerformanceConnections.Text = string.Format("{0} / {1}", connectionsTotal, connectionsCurrent);
            }
            else
            {
                var dbIsStall = record.Value13;
                var dbNumberReads = record.Value1;
                var dbBytesRead = record.Value2;
                var dbNumberWrites = record.Value3;
                var dbBytesWritten = record.Value4;
                var dbStartDate = record.Value16;
                var dbFileCount = record.Value15;

                var logIsStall = record.Value14;
                var logNumberReads = record.Value7;
                var logBytesRead = record.Value8;
                var logNumberWrites = record.Value9;
                var logBytesWritten = record.Value10;

                txtPerformanceCPU.Text = dbStartDate.ToString();
                txtPerformanceIO.Text = string.Format("{0} / {1}", dbIsStall, logIsStall);
                ResetPerformanceIo();
                if (dbIsStall >= QueryEngine.DbStallThreshold
                    || logIsStall >= QueryEngine.DbStallThreshold)
                {
                    txtPerformanceIO.BackColor = Color.Red;
                    ttInfo.SetToolTip(txtPerformanceIO, "Potential performance bottleneck due to hard disk IO delay, check using database analysis.");
                    llPerformanceIO.Enabled = true;
                }
                txtPerformanceRead.Text = string.Format("{0} / {1}", dbNumberReads, Utils.FormatSize(dbBytesRead));
                txtPerformanceWrite.Text = string.Format("{0} / {1}", dbNumberWrites, Utils.FormatSize(dbBytesWritten));
                txtPerformancePackets.Text = string.Format("{0} / {1}", logNumberReads, Utils.FormatSize(logBytesRead));
                txtPerformanceConnections.Text = string.Format("{0} / {1}", logNumberWrites, Utils.FormatSize(logBytesWritten));
            }
            if (record.Value16 > DateTime.MinValue)
                dtpPerformanceStartDate.Value = record.Value16;
            _isLoading = false;
        }

        private void OnPerformanceIoLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Monitor.Instance.ShowAnalysis(AnalysisTypes.Performance);
        }

        internal void GetPerformanceData()
        {
            _engine.CheckPerformance();
        }

        private void OnPopDockClick(object sender, EventArgs e)
        {
            SetPopDock();
        }

        internal void SetPopDock()
        {
            if (_isDocked)
            {
                var dlg = new PerformanceDialog { Name = Title, Text = Title };
                dlg.Controls.Add(this);
                dlg.Show();
                Monitor.Instance.RemoveCurrentTab();
                cmdPopDock.Text = "Dock";
                dlg.BringToFront();
            }
            else
            {
                var parent = this.Parent as PerformanceDialog;
                Monitor.Instance.AddPerformance(this);
                parent.Close();
                cmdPopDock.Text = "Popup";
            }
            _isDocked = !_isDocked;
        }

        private void OnShowPerformanceHistoryCheckedChanged(object sender, EventArgs e)
        {
            var isChecked = chkShowPerformanceHistory.Checked;
            ResetPerformance(false);
            cboPerformanceViewTypes.Enabled = isChecked;
            dtpPerformanceStartDate.Enabled = isChecked;
            cmdPerformancePrevious.Enabled = isChecked;
            cmdPerformanceNext.Enabled = isChecked;
            var area = chPerformance.ChartAreas[0];
            if (isChecked)
            {
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Enabled = false;
                area.CursorX.AxisType = AxisType.Primary;
                area.InnerPlotPosition.Auto = true;
                //pnlCurrentPoint.Visible = true;
                ShowHistory();
            }
            else
            {
                area.AxisX.MajorGrid.Enabled = true;
                area.AxisX.LabelStyle.Enabled = true;
                area.InnerPlotPosition.Auto = false;
                //pnlCurrentPoint.Visible = false;
            }
        }

        private void OnAutoPerformanceCheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
            {
                if (chkAutoPerformance.Checked)
                    Settings.Instance.AddPerformanceItem(_server, IsServer);
                else
                    Settings.Instance.RemovePerformanceItem(_server, IsServer);
            }
        }

        private void ShowHistory()
        {
            if (_server != null)
            {
                ResetPerformance(false);
                _currentRecords = History.GetRecords(_server, IsServer, (DateTypes)cboPerformanceViewTypes.SelectedItem, dtpPerformanceStartDate.Value);
                Debug.WriteLine("count:" + _currentRecords.Count);
                if (_currentRecords.Count > 0)
                {
                    var min = _currentRecords.Min(r => Convert.ToDateTime(r.Date));
                    var max = _currentRecords.Max(r => Convert.ToDateTime(r.Date));
                    _currentRecords.ForEach(r =>
                        {
                            r.Value16 = Convert.ToDateTime(r.Date);
                            AddPerformanceHistoryRecord(r, r.Value16, min, max);
                        });
                }
            }
        }

        private void OnPerformanceStartDateValueChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
                ShowHistory();
        }

        private void OnPerformancePreviousClick(object sender, EventArgs e)
        {
            MoveLeft();
        }

        private void MoveLeft()
        {
            dtpPerformanceStartDate.Value = dtpPerformanceStartDate.Value.AddDays(-1);
            //if (HasData && lastPointIndex > 0)
            //{
            //    lastPointIndex--;
            //    SetPerformanceInfo();
            //}
        }

        private void OnPerformanceNextClick(object sender, EventArgs e)
        {
            MoveRight();
        }

        private void MoveRight()
        {
            dtpPerformanceStartDate.Value = dtpPerformanceStartDate.Value.AddDays(1);
            //if (HasData && lastPointIndex < chPerformance.Series[0].Points.Count - 1)
            //{
            //    lastPointIndex++;
            //    SetPerformanceInfo();
            //}
        }

        private void SetPerformanceInfo()
        {
            if (_lastPointIndex < _currentRecords.Count)
                SetPerformanceInfo(_currentRecords[_lastPointIndex]);
        }

        private void OnPerformanceMouseDown(object sender, MouseEventArgs e)
        {
            //isMouseMoving = true;
        }

        private void OnPerformanceMouseUp(object sender, MouseEventArgs e)
        {
            //isMouseMoving = false;
            //SetPerformanceInfo(e);
        }

        private void OnPerformanceMouseMove(object sender, MouseEventArgs e)
        {
            //if (isMouseMoving)
            //{
            //    SetPerformanceInfo(e);
            //}
        }

        private bool HasData
        {
            get { return chPerformance.Series.Count > 0 && chPerformance.Series[0].Points.Count > 0; }
        }

        //private void SetPerformanceInfo(MouseEventArgs e)
        //{
        //    if (HasData)
        //    {
        //        var series = chPerformance.Series[0];
        //        var pixelPerPoint = (chPerformance.Width - chPerformance.ChartAreas[0].Position.Width) / series.Points.Count;
        //        var index = Convert.ToInt32((e.X - chPerformance.ChartAreas[0].Position.Width) / pixelPerPoint);
        //        if (lastPointIndex != index && index >= 0 && index < series.Points.Count)
        //        {
        //            lastPointIndex = index;
        //            //pnlCurrentPoint.Location = new Point(e.X, 0);
        //            SetPerformanceInfo();
        //        }
        //    }
        //}

        private void OnPerformanceMouseLeave(object sender, EventArgs e)
        {
            //isMouseMoving = false;
        }

        private void OnPerformanceKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Right:
                    MoveRight();
                    break;
                default:
                    break;
            }
        }

        private void OnPerformanceViewTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHistory();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
        }

    }
}