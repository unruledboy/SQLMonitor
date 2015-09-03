using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Diff;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.UI
{
    public partial class UserQuery : UserControl, ICancelable
    {
        private readonly ServerInfo _server = null;
        private string _fileName = string.Empty;
        private bool _isRunning = false;
        private Thread _thread = null;

        public UserQuery()
        {
            InitializeComponent();
        }

        public UserQuery(string query, ServerInfo server)
            : this()
        {
            _server = server.Clone();
            rtbSQL.Font = Monitor.Instance.SetFont();
            Utils.SetTextBoxStyle(rtbSQL);
            rtbSQL.Text = query;
        }

        ~UserQuery()
        {
            Cancel();
        }

        public void Cancel()
        {
            try
            {
                if (_isRunning && _thread != null)
                    _thread.Abort();
                _isRunning = false;
            }
            catch (Exception)
            {
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        private void SetCommand(bool cancel)
        {
            _isRunning = cancel;
            Monitor.Instance.SetExecute(cancel);
        }

        private void StartQuery(object state)
        {
            try
            {
                SetCommand(true);
//#if (DEBUG)
//                Thread.Sleep(10000);
//#endif
                string message;
                var time = new Stopwatch();
                time.Start();
                var results = SqlHelper.QuerySet((string)state, _server, out message);
                if (results != null)
                {
                    this.Invoke(() =>
                    {
                        results.Tables.Cast<DataTable>().ForEach(t =>
                        {
                            var dataGrid = new DataGridView();
                            dataGrid.DataError += (OnQueryDataGridDataError);
                            dataGrid.Location = new Point(0, tpData.Controls.Cast<DataGridView>().Sum((c) => c.Height + 6));
                            dataGrid.Width = tpData.Width - 20;
                            dataGrid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                            dataGrid.ReadOnly = true;
                            dataGrid.AllowUserToAddRows = false;
                            dataGrid.AllowUserToDeleteRows = false;
                            dataGrid.DataSource = t;
                            tpData.Controls.Add(dataGrid);
                            for (var i = 0; i < dataGrid.Rows.Count; i++)
                            {
                                dataGrid.Rows[i].HeaderCell.Value = (i + 1).ToString();
                                dataGrid.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }
                        });
                        if (tpData.Controls.Count > 0)
                        {
                            var defaultHeight = 160;
                            var height = tpData.Height / tpData.Controls.Count;
                            if (height < defaultHeight)
                                height = defaultHeight;
                            tpData.Controls.Cast<Control>().ForEach(c => c.Height = height);
                        }
                        time.Stop();
                        rtbInfo.Text = message + "\r\n\r\nExecuted in " + time.Elapsed;
                    });
                }
            }
            catch (Exception ex)
            {
                this.Invoke(() =>
                    {
                        tcQueryResult.SelectedTab = tpInfo;
                        rtbInfo.Text = ex is ThreadAbortException ? "Query cancelled." :  ex.Message;
                    });
            }
            finally
            {
                SetCommand(false);
            }
        }

        public void Execute()
        {
            if (!_isRunning)
            {
                tpData.Controls.Clear();
                var sql = rtbSQL.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                if (string.IsNullOrEmpty(sql))
                    sql = rtbSQL.Text;
                if (!string.IsNullOrEmpty(sql))
                {
                    Settings.Instance.LastQuery = sql;
                    using (new DisposableState(this, Monitor.Instance.Commands))
                    {
                        _thread = new Thread(StartQuery);
                        _thread.Start(sql);
                    }
                }
                else
                    Monitor.Instance.ShowMessage("Please input sql to execute.");
            }
            else
            {
                if (_thread != null)
                    _thread.Abort();
                _isRunning = false;
                SetCommand(false);
            }
        }

        private void OnQueryDataGridDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void OnChangeConnectionClick(object sender, EventArgs e)
        {
            using (var dlg = new ConnectionDialog(_server))
            {
                if (dlg.ShowDialog(this.ParentForm) == DialogResult.OK)
                {
                    _server.Server = dlg.Server;
                    _server.User = dlg.UserName;
                    _server.Password = dlg.Password;
                    _server.AuthType = dlg.AuthType;
                    var page = this.Parent as TabPage;
                    var index = page.Text.IndexOf(" ");
                    page.Text = _server.Server + page.Text.Substring(index);
                }
            }
        }

        private void OnSaveScriptClick(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void SaveScript()
        {
            var page = this.Parent as TabPage;
            if (string.IsNullOrEmpty(_fileName))
            {
                _fileName = page.Text;
                Path.GetInvalidFileNameChars().ForEach(c => _fileName = _fileName.Replace(c.ToString(), string.Empty));
                Path.GetInvalidPathChars().ForEach(c => _fileName = _fileName.Replace(c.ToString(), string.Empty));
            }
            var newFile = Monitor.Instance.SaveScript(_fileName, rtbSQL.Text);
            if (!string.IsNullOrEmpty(newFile))
            {
                _fileName = newFile;
                Monitor.Instance.AddRecentObject(_server, _server.Database, _fileName, RecentObjectTypes.FilePath, string.Empty);
                page.Text = _server.Server + "." + _server.Database + " " + _fileName;
            }
        }

        private void OnOpenScriptClick(object sender, EventArgs e)
        {
            OpenScript();
        }

        private void OpenScript()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog(this.ParentForm) == DialogResult.OK)
                {
                    _fileName = dlg.FileName;
                    var text = File.ReadAllText(_fileName);
                    if (!string.IsNullOrEmpty(rtbSQL.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText))
                        Utils.SelectText(rtbSQL, text);
                    else
                        rtbSQL.Text = text;
                    Monitor.Instance.AddRecentObject(_server, _server.Database, _fileName, RecentObjectTypes.FilePath, string.Empty);
                }
            }
        }

        private void OnSqlKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    if (e.Control)
                        SaveScript();
                    break;
                case Keys.O:
                    if (e.Control)
                        OpenScript();
                    break;
                default:
                    break;
            }
        }

        private void OnCompareScriptClick(object sender, EventArgs e)
        {
            using (var dlg = new DiffResults(rtbSQL.Text))
            {
                dlg.ShowDialog(this);
            }
        }

        private void OnContentDragDrop(object sender, DragEventArgs e)
        {
            var result = Utils.GetDragDropContent(e);
            if (!string.IsNullOrEmpty(result))
                rtbSQL.Text = result;
        }

        private void OnContentDragEnter(object sender, DragEventArgs e)
        {
            Utils.HandleContentDragEnter(e);
        }
    }
}
