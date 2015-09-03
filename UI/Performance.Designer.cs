using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Xnlab.SQLMon.UI
{
    partial class Performance
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            var chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            var legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlPerformance = new System.Windows.Forms.Panel();
            this.cmdPerformanceNext = new System.Windows.Forms.Button();
            this.cmdPerformancePrevious = new System.Windows.Forms.Button();
            this.dtpPerformanceStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblPerformanceStartDate = new System.Windows.Forms.Label();
            this.cboPerformanceViewTypes = new System.Windows.Forms.ComboBox();
            this.lblPerformanceViewType = new System.Windows.Forms.Label();
            this.chkShowPerformanceHistory = new System.Windows.Forms.CheckBox();
            this.chkAutoPerformance = new System.Windows.Forms.CheckBox();
            this.lblPerformanceConnection = new System.Windows.Forms.Label();
            this.txtPerformanceConnections = new System.Windows.Forms.TextBox();
            this.lblPerformancePacket = new System.Windows.Forms.Label();
            this.txtPerformancePackets = new System.Windows.Forms.TextBox();
            this.lblPerformanceWrite = new System.Windows.Forms.Label();
            this.txtPerformanceWrite = new System.Windows.Forms.TextBox();
            this.lblPerformanceRead = new System.Windows.Forms.Label();
            this.txtPerformanceRead = new System.Windows.Forms.TextBox();
            this.lblPerformanceCPU = new System.Windows.Forms.Label();
            this.txtPerformanceCPU = new System.Windows.Forms.TextBox();
            this.llPerformanceIO = new System.Windows.Forms.LinkLabel();
            this.txtPerformanceIO = new System.Windows.Forms.TextBox();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.cmdPopDock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chPerformance)).BeginInit();
            this.pnlPerformance.SuspendLayout();
            this.SuspendLayout();
            // 
            // chPerformance
            // 
            this.chPerformance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            this.chPerformance.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chPerformance.BackSecondaryColor = System.Drawing.Color.White;
            this.chPerformance.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.Format = "hh:mm:ss";
            chartArea1.AxisX.LabelStyle.Interval = 10D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.Interval = 10D;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorTickMark.Interval = 10D;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(165)))), ((int)(((byte)(191)))), ((int)(((byte)(228)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.CursorX.Interval = 5D;
            chartArea1.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorX.SelectionColor = System.Drawing.Color.PaleGoldenrod;
            chartArea1.CursorX.AutoScroll = false;
            chartArea1.CursorY.AutoScroll = false;
            chartArea1.AxisX.ScrollBar.Enabled = false;
            chartArea1.AxisY.ScrollBar.Enabled = false;
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.AxisY.ScaleView.Zoomable = false;
            chartArea1.Name = "Default";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 98F;
            chartArea1.Position.Width = 98F;
            chartArea1.Position.X = 1F;
            chartArea1.Position.Y = 1F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chPerformance.ChartAreas.Add(chartArea1);
            this.chPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.DockedToChartArea = "Default";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Default";
            this.chPerformance.Legends.Add(legend1);
            this.chPerformance.Location = new System.Drawing.Point(0, 0);
            this.chPerformance.Name = "chPerformance";
            this.chPerformance.Size = new System.Drawing.Size(770, 407);
            this.chPerformance.TabIndex = 16;
            this.chPerformance.CursorPositionChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.OnPerformanceCursorPositionChanged);
            this.chPerformance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnPerformanceKeyDown);
            this.chPerformance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPerformanceMouseDown);
            this.chPerformance.MouseLeave += new System.EventHandler(this.OnPerformanceMouseLeave);
            this.chPerformance.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnPerformanceMouseMove);
            this.chPerformance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPerformanceMouseUp);
            // 
            // pnlPerformance
            // 
            this.pnlPerformance.Controls.Add(this.cmdPerformanceNext);
            this.pnlPerformance.Controls.Add(this.cmdPerformancePrevious);
            this.pnlPerformance.Controls.Add(this.dtpPerformanceStartDate);
            this.pnlPerformance.Controls.Add(this.lblPerformanceStartDate);
            this.pnlPerformance.Controls.Add(this.cboPerformanceViewTypes);
            this.pnlPerformance.Controls.Add(this.lblPerformanceViewType);
            this.pnlPerformance.Controls.Add(this.chkShowPerformanceHistory);
            this.pnlPerformance.Controls.Add(this.chkAutoPerformance);
            this.pnlPerformance.Controls.Add(this.lblPerformanceConnection);
            this.pnlPerformance.Controls.Add(this.txtPerformanceConnections);
            this.pnlPerformance.Controls.Add(this.lblPerformancePacket);
            this.pnlPerformance.Controls.Add(this.txtPerformancePackets);
            this.pnlPerformance.Controls.Add(this.lblPerformanceWrite);
            this.pnlPerformance.Controls.Add(this.txtPerformanceWrite);
            this.pnlPerformance.Controls.Add(this.lblPerformanceRead);
            this.pnlPerformance.Controls.Add(this.txtPerformanceRead);
            this.pnlPerformance.Controls.Add(this.lblPerformanceCPU);
            this.pnlPerformance.Controls.Add(this.txtPerformanceCPU);
            this.pnlPerformance.Controls.Add(this.llPerformanceIO);
            this.pnlPerformance.Controls.Add(this.txtPerformanceIO);
            this.pnlPerformance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPerformance.Location = new System.Drawing.Point(0, 407);
            this.pnlPerformance.Name = "pnlPerformance";
            this.pnlPerformance.Size = new System.Drawing.Size(770, 87);
            this.pnlPerformance.TabIndex = 15;
            // 
            // cmdPerformanceNext
            // 
            this.cmdPerformanceNext.Enabled = false;
            this.cmdPerformanceNext.Location = new System.Drawing.Point(651, 62);
            this.cmdPerformanceNext.Name = "cmdPerformanceNext";
            this.cmdPerformanceNext.Size = new System.Drawing.Size(28, 23);
            this.cmdPerformanceNext.TabIndex = 18;
            this.cmdPerformanceNext.Text = ">";
            this.cmdPerformanceNext.UseVisualStyleBackColor = true;
            this.cmdPerformanceNext.Click += new System.EventHandler(this.OnPerformanceNextClick);
            // 
            // cmdPerformancePrevious
            // 
            this.cmdPerformancePrevious.Enabled = false;
            this.cmdPerformancePrevious.Location = new System.Drawing.Point(623, 62);
            this.cmdPerformancePrevious.Name = "cmdPerformancePrevious";
            this.cmdPerformancePrevious.Size = new System.Drawing.Size(28, 23);
            this.cmdPerformancePrevious.TabIndex = 18;
            this.cmdPerformancePrevious.Text = "<";
            this.cmdPerformancePrevious.UseVisualStyleBackColor = true;
            this.cmdPerformancePrevious.Click += new System.EventHandler(this.OnPerformancePreviousClick);
            // 
            // dtpPerformanceStartDate
            // 
            this.dtpPerformanceStartDate.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtpPerformanceStartDate.Enabled = false;
            this.dtpPerformanceStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPerformanceStartDate.Location = new System.Drawing.Point(469, 63);
            this.dtpPerformanceStartDate.Name = "dtpPerformanceStartDate";
            this.dtpPerformanceStartDate.Size = new System.Drawing.Size(152, 21);
            this.dtpPerformanceStartDate.TabIndex = 17;
            this.dtpPerformanceStartDate.ValueChanged += new System.EventHandler(this.OnPerformanceStartDateValueChanged);
            // 
            // lblPerformanceStartDate
            // 
            this.lblPerformanceStartDate.AutoSize = true;
            this.lblPerformanceStartDate.Location = new System.Drawing.Point(392, 66);
            this.lblPerformanceStartDate.Name = "lblPerformanceStartDate";
            this.lblPerformanceStartDate.Size = new System.Drawing.Size(35, 12);
            this.lblPerformanceStartDate.TabIndex = 16;
            this.lblPerformanceStartDate.Text = "Start";
            // 
            // cboPerformanceViewTypes
            // 
            this.cboPerformanceViewTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPerformanceViewTypes.Enabled = false;
            this.cboPerformanceViewTypes.FormattingEnabled = true;
            this.cboPerformanceViewTypes.Location = new System.Drawing.Point(236, 63);
            this.cboPerformanceViewTypes.Name = "cboPerformanceViewTypes";
            this.cboPerformanceViewTypes.Size = new System.Drawing.Size(121, 20);
            this.cboPerformanceViewTypes.TabIndex = 14;
            this.cboPerformanceViewTypes.SelectedIndexChanged += new System.EventHandler(this.OnPerformanceViewTypesSelectedIndexChanged);
            // 
            // lblPerformanceViewType
            // 
            this.lblPerformanceViewType.AutoSize = true;
            this.lblPerformanceViewType.Location = new System.Drawing.Point(172, 66);
            this.lblPerformanceViewType.Name = "lblPerformanceViewType";
            this.lblPerformanceViewType.Size = new System.Drawing.Size(29, 12);
            this.lblPerformanceViewType.TabIndex = 13;
            this.lblPerformanceViewType.Text = "View";
            // 
            // chkShowPerformanceHistory
            // 
            this.chkShowPerformanceHistory.AutoSize = true;
            this.chkShowPerformanceHistory.Location = new System.Drawing.Point(77, 64);
            this.chkShowPerformanceHistory.Name = "chkShowPerformanceHistory";
            this.chkShowPerformanceHistory.Size = new System.Drawing.Size(66, 16);
            this.chkShowPerformanceHistory.TabIndex = 12;
            this.chkShowPerformanceHistory.Text = "History";
            this.chkShowPerformanceHistory.UseVisualStyleBackColor = true;
            this.chkShowPerformanceHistory.CheckedChanged += new System.EventHandler(this.OnShowPerformanceHistoryCheckedChanged);
            // 
            // chkAutoPerformance
            // 
            this.chkAutoPerformance.AutoSize = true;
            this.chkAutoPerformance.Location = new System.Drawing.Point(9, 64);
            this.chkAutoPerformance.Name = "chkAutoPerformance";
            this.chkAutoPerformance.Size = new System.Drawing.Size(48, 16);
            this.chkAutoPerformance.TabIndex = 12;
            this.chkAutoPerformance.Text = "Auto";
            this.chkAutoPerformance.UseVisualStyleBackColor = true;
            this.chkAutoPerformance.CheckedChanged += new System.EventHandler(this.OnAutoPerformanceCheckedChanged);
            // 
            // lblPerformanceConnection
            // 
            this.lblPerformanceConnection.AutoSize = true;
            this.lblPerformanceConnection.Location = new System.Drawing.Point(392, 40);
            this.lblPerformanceConnection.Name = "lblPerformanceConnection";
            this.lblPerformanceConnection.Size = new System.Drawing.Size(65, 12);
            this.lblPerformanceConnection.TabIndex = 11;
            this.lblPerformanceConnection.Text = "Connection";
            // 
            // txtPerformanceConnections
            // 
            this.txtPerformanceConnections.Location = new System.Drawing.Point(469, 35);
            this.txtPerformanceConnections.Name = "txtPerformanceConnections";
            this.txtPerformanceConnections.ReadOnly = true;
            this.txtPerformanceConnections.Size = new System.Drawing.Size(119, 21);
            this.txtPerformanceConnections.TabIndex = 10;
            // 
            // lblPerformancePacket
            // 
            this.lblPerformancePacket.AutoSize = true;
            this.lblPerformancePacket.Location = new System.Drawing.Point(392, 13);
            this.lblPerformancePacket.Name = "lblPerformancePacket";
            this.lblPerformancePacket.Size = new System.Drawing.Size(41, 12);
            this.lblPerformancePacket.TabIndex = 9;
            this.lblPerformancePacket.Text = "Packet";
            // 
            // txtPerformancePackets
            // 
            this.txtPerformancePackets.Location = new System.Drawing.Point(469, 8);
            this.txtPerformancePackets.Name = "txtPerformancePackets";
            this.txtPerformancePackets.ReadOnly = true;
            this.txtPerformancePackets.Size = new System.Drawing.Size(119, 21);
            this.txtPerformancePackets.TabIndex = 8;
            // 
            // lblPerformanceWrite
            // 
            this.lblPerformanceWrite.AutoSize = true;
            this.lblPerformanceWrite.Location = new System.Drawing.Point(172, 40);
            this.lblPerformanceWrite.Name = "lblPerformanceWrite";
            this.lblPerformanceWrite.Size = new System.Drawing.Size(35, 12);
            this.lblPerformanceWrite.TabIndex = 7;
            this.lblPerformanceWrite.Text = "Write";
            // 
            // txtPerformanceWrite
            // 
            this.txtPerformanceWrite.Location = new System.Drawing.Point(236, 35);
            this.txtPerformanceWrite.Name = "txtPerformanceWrite";
            this.txtPerformanceWrite.ReadOnly = true;
            this.txtPerformanceWrite.Size = new System.Drawing.Size(119, 21);
            this.txtPerformanceWrite.TabIndex = 6;
            // 
            // lblPerformanceRead
            // 
            this.lblPerformanceRead.AutoSize = true;
            this.lblPerformanceRead.Location = new System.Drawing.Point(172, 13);
            this.lblPerformanceRead.Name = "lblPerformanceRead";
            this.lblPerformanceRead.Size = new System.Drawing.Size(29, 12);
            this.lblPerformanceRead.TabIndex = 5;
            this.lblPerformanceRead.Text = "Read";
            // 
            // txtPerformanceRead
            // 
            this.txtPerformanceRead.Location = new System.Drawing.Point(236, 8);
            this.txtPerformanceRead.Name = "txtPerformanceRead";
            this.txtPerformanceRead.ReadOnly = true;
            this.txtPerformanceRead.Size = new System.Drawing.Size(119, 21);
            this.txtPerformanceRead.TabIndex = 4;
            // 
            // lblPerformanceCPU
            // 
            this.lblPerformanceCPU.AutoSize = true;
            this.lblPerformanceCPU.Location = new System.Drawing.Point(7, 38);
            this.lblPerformanceCPU.Name = "lblPerformanceCPU";
            this.lblPerformanceCPU.Size = new System.Drawing.Size(23, 12);
            this.lblPerformanceCPU.TabIndex = 3;
            this.lblPerformanceCPU.Text = "CPU";
            // 
            // txtPerformanceCPU
            // 
            this.txtPerformanceCPU.Location = new System.Drawing.Point(56, 37);
            this.txtPerformanceCPU.Name = "txtPerformanceCPU";
            this.txtPerformanceCPU.ReadOnly = true;
            this.txtPerformanceCPU.Size = new System.Drawing.Size(90, 21);
            this.txtPerformanceCPU.TabIndex = 2;
            // 
            // llPerformanceIO
            // 
            this.llPerformanceIO.AutoSize = true;
            this.llPerformanceIO.Location = new System.Drawing.Point(7, 13);
            this.llPerformanceIO.Name = "llPerformanceIO";
            this.llPerformanceIO.Size = new System.Drawing.Size(17, 12);
            this.llPerformanceIO.TabIndex = 1;
            this.llPerformanceIO.TabStop = true;
            this.llPerformanceIO.Text = "IO";
            this.llPerformanceIO.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnPerformanceIoLinkClicked);
            // 
            // txtPerformanceIO
            // 
            this.txtPerformanceIO.Location = new System.Drawing.Point(56, 8);
            this.txtPerformanceIO.Name = "txtPerformanceIO";
            this.txtPerformanceIO.ReadOnly = true;
            this.txtPerformanceIO.Size = new System.Drawing.Size(90, 21);
            this.txtPerformanceIO.TabIndex = 0;
            // 
            // cmdPopDock
            // 
            this.cmdPopDock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPopDock.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cmdPopDock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPopDock.Location = new System.Drawing.Point(692, 3);
            this.cmdPopDock.Name = "cmdPopDock";
            this.cmdPopDock.Size = new System.Drawing.Size(75, 23);
            this.cmdPopDock.TabIndex = 17;
            this.cmdPopDock.Text = "Popup";
            this.cmdPopDock.UseVisualStyleBackColor = true;
            this.cmdPopDock.Visible = false;
            this.cmdPopDock.Click += new System.EventHandler(this.OnPopDockClick);
            // 
            // Performance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdPopDock);
            this.Controls.Add(this.chPerformance);
            this.Controls.Add(this.pnlPerformance);
            this.Name = "Performance";
            this.Size = new System.Drawing.Size(770, 494);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.chPerformance)).EndInit();
            this.pnlPerformance.ResumeLayout(false);
            this.pnlPerformance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Chart chPerformance;
        private Panel pnlPerformance;
        private Label lblPerformanceConnection;
        private TextBox txtPerformanceConnections;
        private Label lblPerformancePacket;
        private TextBox txtPerformancePackets;
        private Label lblPerformanceWrite;
        private TextBox txtPerformanceWrite;
        private Label lblPerformanceRead;
        private TextBox txtPerformanceRead;
        private Label lblPerformanceCPU;
        private TextBox txtPerformanceCPU;
        private LinkLabel llPerformanceIO;
        private TextBox txtPerformanceIO;
        private ToolTip ttInfo;
        private Button cmdPopDock;
        private CheckBox chkAutoPerformance;
        private CheckBox chkShowPerformanceHistory;
        private Label lblPerformanceViewType;
        private ComboBox cboPerformanceViewTypes;
        private Label lblPerformanceStartDate;
        private DateTimePicker dtpPerformanceStartDate;
        private Button cmdPerformancePrevious;
        private Button cmdPerformanceNext;
    }
}
