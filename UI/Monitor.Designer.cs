using System.ComponentModel;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using Xnlab.SQLMon.Controls.OutlookGrid;

namespace Xnlab.SQLMon.UI
{
    partial class Monitor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsCommands = new System.Windows.Forms.ToolStrip();
            this.tbNewConnection = new System.Windows.Forms.ToolStripSplitButton();
            this.tmNewQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tcbRefreshActivitiesIntervals = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tssRuntime = new System.Windows.Forms.ToolStripSeparator();
            this.tbProjectHomepage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbRunManagementStudio = new System.Windows.Forms.ToolStripButton();
            this.tbRecentObjects = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsRecentObjects = new System.Windows.Forms.ToolStripSeparator();
            this.tmClearRecentObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvProcesses = new System.Windows.Forms.DataGridView();
            this.dtcActivitiesSPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesHostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesHostProcess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesProgramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesCPU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesPhysicalIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesLoginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesLastRequestStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesLastRequestEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesLoginName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcActivitiesEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rtbProcessSQL = new ICSharpCode.TextEditor.TextEditorControl();
            this.cmsActivityScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmiSaveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpSummary = new System.Windows.Forms.TabPage();
            this.txtServerInstanceName = new System.Windows.Forms.TextBox();
            this.lblServerInstanceName = new System.Windows.Forms.Label();
            this.txtServerProcessID = new System.Windows.Forms.TextBox();
            this.lblServerProcessID = new System.Windows.Forms.Label();
            this.txtServerInstallationTime = new System.Windows.Forms.TextBox();
            this.lblServerInstallationTime = new System.Windows.Forms.Label();
            this.txtServerStartTime = new System.Windows.Forms.TextBox();
            this.lblServerStartTime = new System.Windows.Forms.Label();
            this.lklObjects = new System.Windows.Forms.LinkLabel();
            this.lblObjectCount = new System.Windows.Forms.Label();
            this.lklConnections = new System.Windows.Forms.LinkLabel();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblConnectionCount = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tpObjects = new System.Windows.Forms.TabPage();
            this.scObjects = new System.Windows.Forms.SplitContainer();
            this.dgvObjects = new System.Windows.Forms.DataGridView();
            this.State = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModifyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlObjectScript = new System.Windows.Forms.Panel();
            this.pnlSearchCommands = new System.Windows.Forms.Panel();
            this.cmdSearchObjectNext = new System.Windows.Forms.Button();
            this.cmdSearchObjectPrevious = new System.Windows.Forms.Button();
            this.cmdCompareObjectVersion = new System.Windows.Forms.Button();
            this.cboObjectScriptVersions = new System.Windows.Forms.ComboBox();
            this.lblObjectScriptVersion = new System.Windows.Forms.Label();
            this.rtbObjectScript = new ICSharpCode.TextEditor.TextEditorControl();
            this.cmsObjectScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmiSaveScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tiCompareScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tpActivities = new System.Windows.Forms.TabPage();
            this.tpPerformance = new System.Windows.Forms.TabPage();
            this.tpAnalysis = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.cboAnalysisTypes = new System.Windows.Forms.ComboBox();
            this.lblAnalysisType = new System.Windows.Forms.Label();
            this.rtbAnalysisSQL = new ICSharpCode.TextEditor.TextEditorControl();
            this.cmsAnalysisScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tiAnalysisScriptSaveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tpAlerts = new System.Windows.Forms.TabPage();
            this.chkEnableAlert = new System.Windows.Forms.CheckBox();
            this.cmdNewMonitorItem = new System.Windows.Forms.Button();
            this.cboAlertTitle = new System.Windows.Forms.ComboBox();
            this.lblAlertTitle = new System.Windows.Forms.Label();
            this.lblAlertMetrict = new System.Windows.Forms.Label();
            this.cboAlertMethods = new System.Windows.Forms.ComboBox();
            this.lblAlertMethod = new System.Windows.Forms.Label();
            this.cmdEditMonitorItem = new System.Windows.Forms.Button();
            this.cboAlertConnections = new System.Windows.Forms.ComboBox();
            this.cmdDeleteMonitorItem = new System.Windows.Forms.Button();
            this.dgvMonitorItems = new System.Windows.Forms.DataGridView();
            this.cboAlertCondictionValues = new System.Windows.Forms.ComboBox();
            this.txtAlertTarget = new System.Windows.Forms.TextBox();
            this.cboAlertConditionTypes = new System.Windows.Forms.ComboBox();
            this.cmdSaveMonitorItem = new System.Windows.Forms.Button();
            this.lblAlert = new System.Windows.Forms.Label();
            this.cboAlertTypes = new System.Windows.Forms.ComboBox();
            this.tpHistories = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.dgvHistories = new System.Windows.Forms.DataGridView();
            this.rtbHistoryDetail = new ICSharpCode.TextEditor.TextEditorControl();
            this.tpOptions = new System.Windows.Forms.TabPage();
            this.gbAnalysisOptions = new System.Windows.Forms.GroupBox();
            this.lblFreeCPURatio = new System.Windows.Forms.Label();
            this.cboFreeCPURatios = new System.Windows.Forms.ComboBox();
            this.lblFreeMemoryRatio = new System.Windows.Forms.Label();
            this.cboFreeMemoryRatios = new System.Windows.Forms.ComboBox();
            this.lblTableDataIndexSpaceRatio = new System.Windows.Forms.Label();
            this.cboTableDataIndexSpaceRatios = new System.Windows.Forms.ComboBox();
            this.lblDatabaseDataLogSpaceRatio = new System.Windows.Forms.Label();
            this.cboDatabaseDataLogSpaceRatios = new System.Windows.Forms.ComboBox();
            this.lblDatabaseDiskFreeSpaceRatio = new System.Windows.Forms.Label();
            this.cboDatabaseDiskFreeSpaceRatios = new System.Windows.Forms.ComboBox();
            this.gbAlertTemplate = new System.Windows.Forms.GroupBox();
            this.cmdSetDefaultAlertTemplate = new System.Windows.Forms.Button();
            this.rtbAlertTemplate = new System.Windows.Forms.RichTextBox();
            this.gbAlertMethod = new System.Windows.Forms.GroupBox();
            this.lblAlertMailReceivers = new System.Windows.Forms.Label();
            this.txtAlertMailReceiver = new System.Windows.Forms.TextBox();
            this.cboAlertMailServers = new System.Windows.Forms.ComboBox();
            this.lblAlertMailServer = new System.Windows.Forms.Label();
            this.txtAlertMailPassword = new System.Windows.Forms.TextBox();
            this.lblAlertUserName = new System.Windows.Forms.Label();
            this.lblAlertMailPassword = new System.Windows.Forms.Label();
            this.txtAlertMailUser = new System.Windows.Forms.TextBox();
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.lblPerformanceIntervalSeconds = new System.Windows.Forms.Label();
            this.lblPerformanceInterval = new System.Windows.Forms.Label();
            this.cboPerformanceIntervals = new System.Windows.Forms.ComboBox();
            this.lblMonitorRefreshIntervalSeconds = new System.Windows.Forms.Label();
            this.lblConnectionTimeoutSeconds = new System.Windows.Forms.Label();
            this.chkLogHistory = new System.Windows.Forms.CheckBox();
            this.chkAutoWordWrap = new System.Windows.Forms.CheckBox();
            this.cmdChooseFont = new System.Windows.Forms.Button();
            this.lblFont = new System.Windows.Forms.Label();
            this.lblConnectionTimeout = new System.Windows.Forms.Label();
            this.lblMonitorRefreshInterval = new System.Windows.Forms.Label();
            this.cboConnectionTimeouts = new System.Windows.Forms.ComboBox();
            this.cboMonitorRefreshIntervals = new System.Windows.Forms.ComboBox();
            this.cmsObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmiRegisterServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiEditServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiRemoveServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiTruncateObject = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiShrinkDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiCheckDB = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiTruncateTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiCleanTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiTableIndexDefrag = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiSetDatabaseState = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiDetachDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiAttachDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiBackupDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiRestoreDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiShowPerformance = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiSearchDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiObjectDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiSetVersionControl = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiOpenTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiNewQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.il16 = new System.Windows.Forms.ImageList(this.components);
            this.epHint = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmsObjectList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmiFindObjectReferences = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvServerHealth = new OutlookGrid();
            this.dtcHealthCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcHealthName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dtcHealthCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcHealthReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcHealthDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtcHealthObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pgPerformance = new Performance();
            this.dgvAnalysis = new OutlookGrid();
            this.tsCommands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).BeginInit();
            this.cmsActivityScript.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpSummary.SuspendLayout();
            this.tpObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scObjects)).BeginInit();
            this.scObjects.Panel1.SuspendLayout();
            this.scObjects.Panel2.SuspendLayout();
            this.scObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).BeginInit();
            this.pnlObjectScript.SuspendLayout();
            this.pnlSearchCommands.SuspendLayout();
            this.cmsObjectScript.SuspendLayout();
            this.tpActivities.SuspendLayout();
            this.tpPerformance.SuspendLayout();
            this.tpAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.cmsAnalysisScript.SuspendLayout();
            this.tpAlerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorItems)).BeginInit();
            this.tpHistories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistories)).BeginInit();
            this.tpOptions.SuspendLayout();
            this.gbAnalysisOptions.SuspendLayout();
            this.gbAlertTemplate.SuspendLayout();
            this.gbAlertMethod.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.cmsObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).BeginInit();
            this.cmsObjectList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerHealth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalysis)).BeginInit();
            this.SuspendLayout();
            // 
            // tsCommands
            // 
            this.tsCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNewConnection,
            this.tbRefresh,
            this.tcbRefreshActivitiesIntervals,
            this.toolStripSeparator1,
            this.tssRuntime,
            this.tbProjectHomepage,
            this.toolStripSeparator2,
            this.tbRunManagementStudio,
            this.tbRecentObjects});
            this.tsCommands.Location = new System.Drawing.Point(0, 0);
            this.tsCommands.Name = "tsCommands";
            this.tsCommands.Size = new System.Drawing.Size(851, 25);
            this.tsCommands.TabIndex = 0;
            this.tsCommands.Text = "toolStrip1";
            // 
            // tbNewConnection
            // 
            this.tbNewConnection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmNewQuery});
            this.tbNewConnection.Image = global::Xnlab.SQLMon.Properties.Resources.New2;
            this.tbNewConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNewConnection.Name = "tbNewConnection";
            this.tbNewConnection.Size = new System.Drawing.Size(66, 22);
            this.tbNewConnection.Text = "New";
            this.tbNewConnection.ToolTipText = "New Connection";
            this.tbNewConnection.ButtonClick += new System.EventHandler(this.OnRegisterServerClick);
            // 
            // tmNewQuery
            // 
            this.tmNewQuery.Image = global::Xnlab.SQLMon.Properties.Resources.Edit2;
            this.tmNewQuery.Name = "tmNewQuery";
            this.tmNewQuery.Size = new System.Drawing.Size(141, 22);
            this.tmNewQuery.Text = "New Query";
            this.tmNewQuery.Click += new System.EventHandler(this.OnNewQueryClick);
            // 
            // tbRefresh
            // 
            this.tbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tbRefresh.Image")));
            this.tbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRefresh.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.tbRefresh.Name = "tbRefresh";
            this.tbRefresh.Size = new System.Drawing.Size(72, 22);
            this.tbRefresh.Text = "Refresh";
            this.tbRefresh.Click += new System.EventHandler(this.OnRefreshClick);
            // 
            // tcbRefreshActivitiesIntervals
            // 
            this.tcbRefreshActivitiesIntervals.Name = "tcbRefreshActivitiesIntervals";
            this.tcbRefreshActivitiesIntervals.Size = new System.Drawing.Size(75, 25);
            this.tcbRefreshActivitiesIntervals.TextChanged += new System.EventHandler(this.OnRefreshActivitiesIntervalsTextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tssRuntime
            // 
            this.tssRuntime.Name = "tssRuntime";
            this.tssRuntime.Size = new System.Drawing.Size(6, 25);
            this.tssRuntime.Visible = false;
            // 
            // tbProjectHomepage
            // 
            this.tbProjectHomepage.Image = ((System.Drawing.Image)(resources.GetObject("tbProjectHomepage.Image")));
            this.tbProjectHomepage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbProjectHomepage.Name = "tbProjectHomepage";
            this.tbProjectHomepage.Size = new System.Drawing.Size(78, 22);
            this.tbProjectHomepage.Text = "SQLMon";
            this.tbProjectHomepage.ToolTipText = "SQL Mon Project Home";
            this.tbProjectHomepage.Click += new System.EventHandler(this.OnProjectHomepageClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbRunManagementStudio
            // 
            this.tbRunManagementStudio.Image = ((System.Drawing.Image)(resources.GetObject("tbRunManagementStudio.Image")));
            this.tbRunManagementStudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRunManagementStudio.Name = "tbRunManagementStudio";
            this.tbRunManagementStudio.Size = new System.Drawing.Size(146, 22);
            this.tbRunManagementStudio.Text = "Management Studio";
            // 
            // tbRecentObjects
            // 
            this.tbRecentObjects.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRecentObjects,
            this.tmClearRecentObjects});
            this.tbRecentObjects.Image = global::Xnlab.SQLMon.Properties.Resources.History2;
            this.tbRecentObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRecentObjects.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.tbRecentObjects.Name = "tbRecentObjects";
            this.tbRecentObjects.Size = new System.Drawing.Size(124, 22);
            this.tbRecentObjects.Text = "Recent Objects";
            // 
            // tsRecentObjects
            // 
            this.tsRecentObjects.Name = "tsRecentObjects";
            this.tsRecentObjects.Size = new System.Drawing.Size(103, 6);
            // 
            // tmClearRecentObjects
            // 
            this.tmClearRecentObjects.Image = global::Xnlab.SQLMon.Properties.Resources.Cross2;
            this.tmClearRecentObjects.Name = "tmClearRecentObjects";
            this.tmClearRecentObjects.Size = new System.Drawing.Size(106, 22);
            this.tmClearRecentObjects.Text = "&Clear";
            this.tmClearRecentObjects.Click += new System.EventHandler(this.OnClearRecentObjectsClick);
            // 
            // dgvProcesses
            // 
            this.dgvProcesses.AllowUserToAddRows = false;
            this.dgvProcesses.AllowUserToDeleteRows = false;
            this.dgvProcesses.AllowUserToResizeRows = false;
            this.dgvProcesses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcesses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dtcActivitiesSPID,
            this.dtcActivitiesHostName,
            this.dtcActivitiesHostProcess,
            this.dtcActivitiesProgramName,
            this.dtcActivitiesDB,
            this.dtcActivitiesCPU,
            this.dtcActivitiesPhysicalIO,
            this.dtcActivitiesLoginTime,
            this.dtcActivitiesLastRequestStart,
            this.dtcActivitiesLastRequestEnd,
            this.dtcActivitiesStatus,
            this.dtcActivitiesCommand,
            this.dtcActivitiesPercent,
            this.dtcActivitiesLoginName,
            this.dtcActivitiesEnabled});
            this.dgvProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcesses.Location = new System.Drawing.Point(0, 0);
            this.dgvProcesses.Name = "dgvProcesses";
            this.dgvProcesses.ReadOnly = true;
            this.dgvProcesses.RowHeadersVisible = false;
            this.dgvProcesses.RowTemplate.Height = 24;
            this.dgvProcesses.ShowRowErrors = false;
            this.dgvProcesses.Size = new System.Drawing.Size(594, 366);
            this.dgvProcesses.TabIndex = 1;
            this.dgvProcesses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnProcessesCellDoubleClick);
            this.dgvProcesses.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.OnProcessesCellFormating);
            this.dgvProcesses.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnProcessesRowEnter);
            // 
            // dtcActivitiesSPID
            // 
            this.dtcActivitiesSPID.DataPropertyName = "spid";
            this.dtcActivitiesSPID.FillWeight = 40F;
            this.dtcActivitiesSPID.HeaderText = "Id";
            this.dtcActivitiesSPID.Name = "dtcActivitiesSPID";
            this.dtcActivitiesSPID.ReadOnly = true;
            this.dtcActivitiesSPID.Width = 40;
            // 
            // dtcActivitiesHostName
            // 
            this.dtcActivitiesHostName.DataPropertyName = "hostname";
            this.dtcActivitiesHostName.FillWeight = 85F;
            this.dtcActivitiesHostName.HeaderText = "Host Name";
            this.dtcActivitiesHostName.Name = "dtcActivitiesHostName";
            this.dtcActivitiesHostName.ReadOnly = true;
            this.dtcActivitiesHostName.Width = 85;
            // 
            // dtcActivitiesHostProcess
            // 
            this.dtcActivitiesHostProcess.DataPropertyName = "hostprocess";
            this.dtcActivitiesHostProcess.HeaderText = "Host Process";
            this.dtcActivitiesHostProcess.Name = "dtcActivitiesHostProcess";
            this.dtcActivitiesHostProcess.ReadOnly = true;
            // 
            // dtcActivitiesProgramName
            // 
            this.dtcActivitiesProgramName.DataPropertyName = "program_name";
            this.dtcActivitiesProgramName.HeaderText = "Program";
            this.dtcActivitiesProgramName.Name = "dtcActivitiesProgramName";
            this.dtcActivitiesProgramName.ReadOnly = true;
            // 
            // dtcActivitiesDB
            // 
            this.dtcActivitiesDB.DataPropertyName = "db";
            this.dtcActivitiesDB.HeaderText = "DB";
            this.dtcActivitiesDB.Name = "dtcActivitiesDB";
            this.dtcActivitiesDB.ReadOnly = true;
            // 
            // dtcActivitiesCPU
            // 
            this.dtcActivitiesCPU.DataPropertyName = "cpu";
            this.dtcActivitiesCPU.FillWeight = 60F;
            this.dtcActivitiesCPU.HeaderText = "CPU(ms)";
            this.dtcActivitiesCPU.Name = "dtcActivitiesCPU";
            this.dtcActivitiesCPU.ReadOnly = true;
            this.dtcActivitiesCPU.Width = 60;
            // 
            // dtcActivitiesPhysicalIO
            // 
            this.dtcActivitiesPhysicalIO.DataPropertyName = "physical_io";
            this.dtcActivitiesPhysicalIO.FillWeight = 45F;
            this.dtcActivitiesPhysicalIO.HeaderText = "IO";
            this.dtcActivitiesPhysicalIO.Name = "dtcActivitiesPhysicalIO";
            this.dtcActivitiesPhysicalIO.ReadOnly = true;
            this.dtcActivitiesPhysicalIO.Width = 45;
            // 
            // dtcActivitiesLoginTime
            // 
            this.dtcActivitiesLoginTime.DataPropertyName = "login_time";
            this.dtcActivitiesLoginTime.HeaderText = "Login Time";
            this.dtcActivitiesLoginTime.Name = "dtcActivitiesLoginTime";
            this.dtcActivitiesLoginTime.ReadOnly = true;
            this.dtcActivitiesLoginTime.Visible = false;
            // 
            // dtcActivitiesLastRequestStart
            // 
            this.dtcActivitiesLastRequestStart.DataPropertyName = "last_batch_begin";
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.dtcActivitiesLastRequestStart.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtcActivitiesLastRequestStart.FillWeight = 110F;
            this.dtcActivitiesLastRequestStart.HeaderText = "Request Start";
            this.dtcActivitiesLastRequestStart.Name = "dtcActivitiesLastRequestStart";
            this.dtcActivitiesLastRequestStart.ReadOnly = true;
            this.dtcActivitiesLastRequestStart.Width = 110;
            // 
            // dtcActivitiesLastRequestEnd
            // 
            this.dtcActivitiesLastRequestEnd.DataPropertyName = "last_batch_end";
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = null;
            this.dtcActivitiesLastRequestEnd.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtcActivitiesLastRequestEnd.FillWeight = 110F;
            this.dtcActivitiesLastRequestEnd.HeaderText = "Request End";
            this.dtcActivitiesLastRequestEnd.Name = "dtcActivitiesLastRequestEnd";
            this.dtcActivitiesLastRequestEnd.ReadOnly = true;
            this.dtcActivitiesLastRequestEnd.Width = 110;
            // 
            // dtcActivitiesStatus
            // 
            this.dtcActivitiesStatus.DataPropertyName = "status";
            this.dtcActivitiesStatus.FillWeight = 70F;
            this.dtcActivitiesStatus.HeaderText = "Status";
            this.dtcActivitiesStatus.Name = "dtcActivitiesStatus";
            this.dtcActivitiesStatus.ReadOnly = true;
            this.dtcActivitiesStatus.Width = 70;
            // 
            // dtcActivitiesCommand
            // 
            this.dtcActivitiesCommand.DataPropertyName = "cmd";
            this.dtcActivitiesCommand.HeaderText = "Command";
            this.dtcActivitiesCommand.Name = "dtcActivitiesCommand";
            this.dtcActivitiesCommand.ReadOnly = true;
            this.dtcActivitiesCommand.Visible = false;
            // 
            // dtcActivitiesPercent
            // 
            this.dtcActivitiesPercent.DataPropertyName = "percent_complete";
            this.dtcActivitiesPercent.HeaderText = "Percent";
            this.dtcActivitiesPercent.Name = "dtcActivitiesPercent";
            this.dtcActivitiesPercent.ReadOnly = true;
            // 
            // dtcActivitiesLoginName
            // 
            this.dtcActivitiesLoginName.DataPropertyName = "loginname";
            this.dtcActivitiesLoginName.FillWeight = 60F;
            this.dtcActivitiesLoginName.HeaderText = "User";
            this.dtcActivitiesLoginName.Name = "dtcActivitiesLoginName";
            this.dtcActivitiesLoginName.ReadOnly = true;
            this.dtcActivitiesLoginName.Width = 60;
            // 
            // dtcActivitiesEnabled
            // 
            this.dtcActivitiesEnabled.DataPropertyName = "Enabled";
            this.dtcActivitiesEnabled.HeaderText = "Enabled";
            this.dtcActivitiesEnabled.Name = "dtcActivitiesEnabled";
            this.dtcActivitiesEnabled.ReadOnly = true;
            this.dtcActivitiesEnabled.Visible = false;
            // 
            // rtbProcessSQL
            // 
            this.rtbProcessSQL.AutoScroll = true;
            this.rtbProcessSQL.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbProcessSQL.ContextMenuStrip = this.cmsActivityScript;
            this.rtbProcessSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbProcessSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbProcessSQL.IsReadOnly = false;
            this.rtbProcessSQL.Location = new System.Drawing.Point(0, 0);
            this.rtbProcessSQL.Name = "rtbProcessSQL";
            this.rtbProcessSQL.Size = new System.Drawing.Size(594, 176);
            this.rtbProcessSQL.TabIndex = 2;
            // 
            // cmsActivityScript
            // 
            this.cmsActivityScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiSaveToFile});
            this.cmsActivityScript.Name = "cmsQuery";
            this.cmsActivityScript.Size = new System.Drawing.Size(143, 26);
            // 
            // tmiSaveToFile
            // 
            this.tmiSaveToFile.Name = "tmiSaveToFile";
            this.tmiSaveToFile.Size = new System.Drawing.Size(142, 22);
            this.tmiSaveToFile.Text = "&Save to File";
            this.tmiSaveToFile.Click += new System.EventHandler(this.OnActivityScriptSaveToFileClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvProcesses);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbProcessSQL);
            this.splitContainer1.Size = new System.Drawing.Size(594, 546);
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 3;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpSummary);
            this.tcMain.Controls.Add(this.tpObjects);
            this.tcMain.Controls.Add(this.tpActivities);
            this.tcMain.Controls.Add(this.tpPerformance);
            this.tcMain.Controls.Add(this.tpAnalysis);
            this.tcMain.Controls.Add(this.tpAlerts);
            this.tcMain.Controls.Add(this.tpHistories);
            this.tcMain.Controls.Add(this.tpOptions);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(608, 578);
            this.tcMain.TabIndex = 4;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.OnMainSelectedIndexChanged);
            this.tcMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMainMouseDoubleClick);
            this.tcMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMainMouseUp);
            // 
            // tpSummary
            // 
            this.tpSummary.Controls.Add(this.dgvServerHealth);
            this.tpSummary.Controls.Add(this.txtServerInstanceName);
            this.tpSummary.Controls.Add(this.lblServerInstanceName);
            this.tpSummary.Controls.Add(this.txtServerProcessID);
            this.tpSummary.Controls.Add(this.lblServerProcessID);
            this.tpSummary.Controls.Add(this.txtServerInstallationTime);
            this.tpSummary.Controls.Add(this.lblServerInstallationTime);
            this.tpSummary.Controls.Add(this.txtServerStartTime);
            this.tpSummary.Controls.Add(this.lblServerStartTime);
            this.tpSummary.Controls.Add(this.lklObjects);
            this.tpSummary.Controls.Add(this.lblObjectCount);
            this.tpSummary.Controls.Add(this.lklConnections);
            this.tpSummary.Controls.Add(this.txtVersion);
            this.tpSummary.Controls.Add(this.lblConnectionCount);
            this.tpSummary.Controls.Add(this.lblVersion);
            this.tpSummary.Location = new System.Drawing.Point(4, 22);
            this.tpSummary.Name = "tpSummary";
            this.tpSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tpSummary.Size = new System.Drawing.Size(600, 552);
            this.tpSummary.TabIndex = 0;
            this.tpSummary.Text = "Dashboard";
            this.tpSummary.UseVisualStyleBackColor = true;
            // 
            // txtServerInstanceName
            // 
            this.txtServerInstanceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerInstanceName.Location = new System.Drawing.Point(414, 76);
            this.txtServerInstanceName.Name = "txtServerInstanceName";
            this.txtServerInstanceName.ReadOnly = true;
            this.txtServerInstanceName.Size = new System.Drawing.Size(180, 21);
            this.txtServerInstanceName.TabIndex = 10;
            // 
            // lblServerInstanceName
            // 
            this.lblServerInstanceName.AutoSize = true;
            this.lblServerInstanceName.Location = new System.Drawing.Point(341, 79);
            this.lblServerInstanceName.Name = "lblServerInstanceName";
            this.lblServerInstanceName.Size = new System.Drawing.Size(59, 12);
            this.lblServerInstanceName.TabIndex = 9;
            this.lblServerInstanceName.Text = "Instance:";
            // 
            // txtServerProcessID
            // 
            this.txtServerProcessID.Location = new System.Drawing.Point(414, 107);
            this.txtServerProcessID.Name = "txtServerProcessID";
            this.txtServerProcessID.ReadOnly = true;
            this.txtServerProcessID.Size = new System.Drawing.Size(101, 21);
            this.txtServerProcessID.TabIndex = 8;
            // 
            // lblServerProcessID
            // 
            this.lblServerProcessID.AutoSize = true;
            this.lblServerProcessID.Location = new System.Drawing.Point(341, 109);
            this.lblServerProcessID.Name = "lblServerProcessID";
            this.lblServerProcessID.Size = new System.Drawing.Size(71, 12);
            this.lblServerProcessID.TabIndex = 7;
            this.lblServerProcessID.Text = "Process ID:";
            // 
            // txtServerInstallationTime
            // 
            this.txtServerInstallationTime.Location = new System.Drawing.Point(90, 76);
            this.txtServerInstallationTime.Name = "txtServerInstallationTime";
            this.txtServerInstallationTime.ReadOnly = true;
            this.txtServerInstallationTime.Size = new System.Drawing.Size(212, 21);
            this.txtServerInstallationTime.TabIndex = 6;
            // 
            // lblServerInstallationTime
            // 
            this.lblServerInstallationTime.AutoSize = true;
            this.lblServerInstallationTime.Location = new System.Drawing.Point(8, 79);
            this.lblServerInstallationTime.Name = "lblServerInstallationTime";
            this.lblServerInstallationTime.Size = new System.Drawing.Size(65, 12);
            this.lblServerInstallationTime.TabIndex = 5;
            this.lblServerInstallationTime.Text = "Installed:";
            // 
            // txtServerStartTime
            // 
            this.txtServerStartTime.Location = new System.Drawing.Point(90, 105);
            this.txtServerStartTime.Name = "txtServerStartTime";
            this.txtServerStartTime.ReadOnly = true;
            this.txtServerStartTime.Size = new System.Drawing.Size(212, 21);
            this.txtServerStartTime.TabIndex = 6;
            // 
            // lblServerStartTime
            // 
            this.lblServerStartTime.AutoSize = true;
            this.lblServerStartTime.Location = new System.Drawing.Point(8, 107);
            this.lblServerStartTime.Name = "lblServerStartTime";
            this.lblServerStartTime.Size = new System.Drawing.Size(53, 12);
            this.lblServerStartTime.TabIndex = 5;
            this.lblServerStartTime.Text = "Started:";
            // 
            // lklObjects
            // 
            this.lklObjects.AutoSize = true;
            this.lklObjects.Location = new System.Drawing.Point(341, 139);
            this.lklObjects.Name = "lklObjects";
            this.lklObjects.Size = new System.Drawing.Size(53, 12);
            this.lklObjects.TabIndex = 4;
            this.lklObjects.TabStop = true;
            this.lklObjects.Text = "Objects:";
            this.lklObjects.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnObjectsLinkClicked);
            // 
            // lblObjectCount
            // 
            this.lblObjectCount.AutoSize = true;
            this.lblObjectCount.Location = new System.Drawing.Point(412, 139);
            this.lblObjectCount.Name = "lblObjectCount";
            this.lblObjectCount.Size = new System.Drawing.Size(11, 12);
            this.lblObjectCount.TabIndex = 3;
            this.lblObjectCount.Text = "0";
            // 
            // lklConnections
            // 
            this.lklConnections.AutoSize = true;
            this.lklConnections.Location = new System.Drawing.Point(8, 139);
            this.lklConnections.Name = "lklConnections";
            this.lklConnections.Size = new System.Drawing.Size(77, 12);
            this.lklConnections.TabIndex = 2;
            this.lklConnections.TabStop = true;
            this.lklConnections.Text = "Connections:";
            this.lklConnections.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnConnectionsLinkClicked);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(90, 13);
            this.txtVersion.Multiline = true;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtVersion.Size = new System.Drawing.Size(504, 55);
            this.txtVersion.TabIndex = 1;
            // 
            // lblConnectionCount
            // 
            this.lblConnectionCount.AutoSize = true;
            this.lblConnectionCount.Location = new System.Drawing.Point(88, 139);
            this.lblConnectionCount.Name = "lblConnectionCount";
            this.lblConnectionCount.Size = new System.Drawing.Size(11, 12);
            this.lblConnectionCount.TabIndex = 0;
            this.lblConnectionCount.Text = "0";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(8, 16);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(53, 12);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Version:";
            // 
            // tpObjects
            // 
            this.tpObjects.Controls.Add(this.scObjects);
            this.tpObjects.Location = new System.Drawing.Point(4, 22);
            this.tpObjects.Name = "tpObjects";
            this.tpObjects.Padding = new System.Windows.Forms.Padding(3);
            this.tpObjects.Size = new System.Drawing.Size(600, 552);
            this.tpObjects.TabIndex = 3;
            this.tpObjects.Text = "Objects";
            this.tpObjects.UseVisualStyleBackColor = true;
            // 
            // scObjects
            // 
            this.scObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scObjects.Location = new System.Drawing.Point(3, 3);
            this.scObjects.Name = "scObjects";
            this.scObjects.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scObjects.Panel1
            // 
            this.scObjects.Panel1.Controls.Add(this.dgvObjects);
            // 
            // scObjects.Panel2
            // 
            this.scObjects.Panel2.Controls.Add(this.pnlObjectScript);
            this.scObjects.Size = new System.Drawing.Size(594, 546);
            this.scObjects.SplitterDistance = 308;
            this.scObjects.TabIndex = 17;
            this.scObjects.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnObjectsSpliterMoved);
            // 
            // dgvObjects
            // 
            this.dgvObjects.AllowUserToAddRows = false;
            this.dgvObjects.AllowUserToDeleteRows = false;
            this.dgvObjects.AllowUserToResizeRows = false;
            this.dgvObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.State,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.Count,
            this.CreateDate,
            this.ModifyDate,
            this.Path,
            this.Value,
            this.TypeName});
            this.dgvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvObjects.Location = new System.Drawing.Point(0, 0);
            this.dgvObjects.Name = "dgvObjects";
            this.dgvObjects.ReadOnly = true;
            this.dgvObjects.RowHeadersVisible = false;
            this.dgvObjects.RowTemplate.Height = 24;
            this.dgvObjects.ShowRowErrors = false;
            this.dgvObjects.Size = new System.Drawing.Size(594, 308);
            this.dgvObjects.TabIndex = 4;
            this.dgvObjects.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.OnObjectsCellFormatting);
            this.dgvObjects.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnObjectsRowEnter);
            // 
            // State
            // 
            this.State.FillWeight = 20F;
            this.State.HeaderText = "";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Width = 20;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn13.FillWeight = 200F;
            this.dataGridViewTextBoxColumn13.HeaderText = "Name";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 200;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "SpaceUsed";
            this.dataGridViewTextBoxColumn14.HeaderText = "Space";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // Count
            // 
            this.Count.DataPropertyName = "Count";
            this.Count.HeaderText = "Count";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // CreateDate
            // 
            this.CreateDate.DataPropertyName = "CreateDate";
            this.CreateDate.HeaderText = "Create Date";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            // 
            // ModifyDate
            // 
            this.ModifyDate.DataPropertyName = "ModifyDate";
            this.ModifyDate.HeaderText = "Modify Date";
            this.ModifyDate.Name = "ModifyDate";
            this.ModifyDate.ReadOnly = true;
            // 
            // Path
            // 
            this.Path.DataPropertyName = "Path";
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Width = 500;
            // 
            // Value
            // 
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Visible = false;
            // 
            // TypeName
            // 
            this.TypeName.DataPropertyName = "Type";
            this.TypeName.HeaderText = "Type";
            this.TypeName.Name = "TypeName";
            this.TypeName.ReadOnly = true;
            this.TypeName.Visible = false;
            // 
            // pnlObjectScript
            // 
            this.pnlObjectScript.Controls.Add(this.pnlSearchCommands);
            this.pnlObjectScript.Controls.Add(this.cmdCompareObjectVersion);
            this.pnlObjectScript.Controls.Add(this.cboObjectScriptVersions);
            this.pnlObjectScript.Controls.Add(this.lblObjectScriptVersion);
            this.pnlObjectScript.Controls.Add(this.rtbObjectScript);
            this.pnlObjectScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlObjectScript.Location = new System.Drawing.Point(0, 0);
            this.pnlObjectScript.Name = "pnlObjectScript";
            this.pnlObjectScript.Size = new System.Drawing.Size(594, 234);
            this.pnlObjectScript.TabIndex = 15;
            // 
            // pnlSearchCommands
            // 
            this.pnlSearchCommands.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlSearchCommands.Controls.Add(this.cmdSearchObjectNext);
            this.pnlSearchCommands.Controls.Add(this.cmdSearchObjectPrevious);
            this.pnlSearchCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSearchCommands.Location = new System.Drawing.Point(0, 203);
            this.pnlSearchCommands.Name = "pnlSearchCommands";
            this.pnlSearchCommands.Size = new System.Drawing.Size(594, 31);
            this.pnlSearchCommands.TabIndex = 4;
            // 
            // cmdSearchObjectNext
            // 
            this.cmdSearchObjectNext.Location = new System.Drawing.Point(85, 5);
            this.cmdSearchObjectNext.Name = "cmdSearchObjectNext";
            this.cmdSearchObjectNext.Size = new System.Drawing.Size(75, 23);
            this.cmdSearchObjectNext.TabIndex = 4;
            this.cmdSearchObjectNext.Text = "&Next";
            this.cmdSearchObjectNext.UseVisualStyleBackColor = true;
            this.cmdSearchObjectNext.Click += new System.EventHandler(this.OnSearchObjectNextClick);
            // 
            // cmdSearchObjectPrevious
            // 
            this.cmdSearchObjectPrevious.Enabled = false;
            this.cmdSearchObjectPrevious.Location = new System.Drawing.Point(4, 4);
            this.cmdSearchObjectPrevious.Name = "cmdSearchObjectPrevious";
            this.cmdSearchObjectPrevious.Size = new System.Drawing.Size(75, 23);
            this.cmdSearchObjectPrevious.TabIndex = 3;
            this.cmdSearchObjectPrevious.Text = "&Previous";
            this.cmdSearchObjectPrevious.UseVisualStyleBackColor = true;
            this.cmdSearchObjectPrevious.Click += new System.EventHandler(this.OnSearchObjectPreviousClick);
            // 
            // cmdCompareObjectVersion
            // 
            this.cmdCompareObjectVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCompareObjectVersion.Location = new System.Drawing.Point(514, 4);
            this.cmdCompareObjectVersion.Name = "cmdCompareObjectVersion";
            this.cmdCompareObjectVersion.Size = new System.Drawing.Size(75, 23);
            this.cmdCompareObjectVersion.TabIndex = 2;
            this.cmdCompareObjectVersion.Text = "&Compare";
            this.cmdCompareObjectVersion.UseVisualStyleBackColor = true;
            this.cmdCompareObjectVersion.Click += new System.EventHandler(this.OnCompareObjectVersionClick);
            // 
            // cboObjectScriptVersions
            // 
            this.cboObjectScriptVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboObjectScriptVersions.DisplayMember = "Key";
            this.cboObjectScriptVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObjectScriptVersions.FormattingEnabled = true;
            this.cboObjectScriptVersions.Location = new System.Drawing.Point(76, 4);
            this.cboObjectScriptVersions.Name = "cboObjectScriptVersions";
            this.cboObjectScriptVersions.Size = new System.Drawing.Size(432, 20);
            this.cboObjectScriptVersions.TabIndex = 1;
            this.cboObjectScriptVersions.ValueMember = "Value";
            this.cboObjectScriptVersions.SelectedIndexChanged += new System.EventHandler(this.OnObjectScriptVersionsSelectedIndexChanged);
            // 
            // lblObjectScriptVersion
            // 
            this.lblObjectScriptVersion.AutoSize = true;
            this.lblObjectScriptVersion.Location = new System.Drawing.Point(4, 6);
            this.lblObjectScriptVersion.Name = "lblObjectScriptVersion";
            this.lblObjectScriptVersion.Size = new System.Drawing.Size(53, 12);
            this.lblObjectScriptVersion.TabIndex = 0;
            this.lblObjectScriptVersion.Text = "&Version:";
            // 
            // rtbObjectScript
            // 
            this.rtbObjectScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbObjectScript.AutoScroll = true;
            this.rtbObjectScript.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbObjectScript.ContextMenuStrip = this.cmsObjectScript;
            this.rtbObjectScript.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbObjectScript.IsReadOnly = false;
            this.rtbObjectScript.Location = new System.Drawing.Point(3, 33);
            this.rtbObjectScript.Name = "rtbObjectScript";
            this.rtbObjectScript.Size = new System.Drawing.Size(588, 163);
            this.rtbObjectScript.TabIndex = 5;
            // 
            // cmsObjectScript
            // 
            this.cmsObjectScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiSaveScript,
            this.tiCompareScript});
            this.cmsObjectScript.Name = "cmsQuery";
            this.cmsObjectScript.Size = new System.Drawing.Size(143, 48);
            // 
            // tmiSaveScript
            // 
            this.tmiSaveScript.Name = "tmiSaveScript";
            this.tmiSaveScript.Size = new System.Drawing.Size(142, 22);
            this.tmiSaveScript.Text = "&Save to File";
            this.tmiSaveScript.Click += new System.EventHandler(this.OnSaveScriptClick);
            // 
            // tiCompareScript
            // 
            this.tiCompareScript.Name = "tiCompareScript";
            this.tiCompareScript.Size = new System.Drawing.Size(142, 22);
            this.tiCompareScript.Text = "&Compare...";
            this.tiCompareScript.Click += new System.EventHandler(this.OnCompareScriptClick);
            // 
            // tpActivities
            // 
            this.tpActivities.Controls.Add(this.splitContainer1);
            this.tpActivities.Location = new System.Drawing.Point(4, 22);
            this.tpActivities.Name = "tpActivities";
            this.tpActivities.Padding = new System.Windows.Forms.Padding(3);
            this.tpActivities.Size = new System.Drawing.Size(600, 552);
            this.tpActivities.TabIndex = 1;
            this.tpActivities.Text = "Activities";
            this.tpActivities.UseVisualStyleBackColor = true;
            // 
            // tpPerformance
            // 
            this.tpPerformance.Controls.Add(this.pgPerformance);
            this.tpPerformance.Location = new System.Drawing.Point(4, 22);
            this.tpPerformance.Name = "tpPerformance";
            this.tpPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tpPerformance.Size = new System.Drawing.Size(600, 552);
            this.tpPerformance.TabIndex = 9;
            this.tpPerformance.Text = "Performance";
            this.tpPerformance.UseVisualStyleBackColor = true;
            // 
            // tpAnalysis
            // 
            this.tpAnalysis.Controls.Add(this.splitContainer4);
            this.tpAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tpAnalysis.Name = "tpAnalysis";
            this.tpAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tpAnalysis.Size = new System.Drawing.Size(600, 552);
            this.tpAnalysis.TabIndex = 6;
            this.tpAnalysis.Text = "Analysis";
            this.tpAnalysis.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.cboAnalysisTypes);
            this.splitContainer4.Panel1.Controls.Add(this.dgvAnalysis);
            this.splitContainer4.Panel1.Controls.Add(this.lblAnalysisType);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.rtbAnalysisSQL);
            this.splitContainer4.Size = new System.Drawing.Size(594, 546);
            this.splitContainer4.SplitterDistance = 369;
            this.splitContainer4.TabIndex = 3;
            // 
            // cboAnalysisTypes
            // 
            this.cboAnalysisTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAnalysisTypes.FormattingEnabled = true;
            this.cboAnalysisTypes.Location = new System.Drawing.Point(67, 4);
            this.cboAnalysisTypes.Name = "cboAnalysisTypes";
            this.cboAnalysisTypes.Size = new System.Drawing.Size(394, 20);
            this.cboAnalysisTypes.TabIndex = 0;
            this.cboAnalysisTypes.SelectedIndexChanged += new System.EventHandler(this.OnAnalysisTypesSelectedIndexChanged);
            // 
            // lblAnalysisType
            // 
            this.lblAnalysisType.AutoSize = true;
            this.lblAnalysisType.Location = new System.Drawing.Point(3, 6);
            this.lblAnalysisType.Name = "lblAnalysisType";
            this.lblAnalysisType.Size = new System.Drawing.Size(35, 12);
            this.lblAnalysisType.TabIndex = 1;
            this.lblAnalysisType.Text = "&Type:";
            // 
            // rtbAnalysisSQL
            // 
            this.rtbAnalysisSQL.AutoScroll = true;
            this.rtbAnalysisSQL.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbAnalysisSQL.ContextMenuStrip = this.cmsAnalysisScript;
            this.rtbAnalysisSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbAnalysisSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAnalysisSQL.IsReadOnly = false;
            this.rtbAnalysisSQL.Location = new System.Drawing.Point(0, 0);
            this.rtbAnalysisSQL.Name = "rtbAnalysisSQL";
            this.rtbAnalysisSQL.Size = new System.Drawing.Size(594, 173);
            this.rtbAnalysisSQL.TabIndex = 3;
            // 
            // cmsAnalysisScript
            // 
            this.cmsAnalysisScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiAnalysisScriptSaveToFile});
            this.cmsAnalysisScript.Name = "cmsQuery";
            this.cmsAnalysisScript.Size = new System.Drawing.Size(143, 26);
            // 
            // tiAnalysisScriptSaveToFile
            // 
            this.tiAnalysisScriptSaveToFile.Name = "tiAnalysisScriptSaveToFile";
            this.tiAnalysisScriptSaveToFile.Size = new System.Drawing.Size(142, 22);
            this.tiAnalysisScriptSaveToFile.Text = "&Save to File";
            this.tiAnalysisScriptSaveToFile.Click += new System.EventHandler(this.OnAnalysisScriptSaveToFileClick);
            // 
            // tpAlerts
            // 
            this.tpAlerts.Controls.Add(this.chkEnableAlert);
            this.tpAlerts.Controls.Add(this.cmdNewMonitorItem);
            this.tpAlerts.Controls.Add(this.cboAlertTitle);
            this.tpAlerts.Controls.Add(this.lblAlertTitle);
            this.tpAlerts.Controls.Add(this.lblAlertMetrict);
            this.tpAlerts.Controls.Add(this.cboAlertMethods);
            this.tpAlerts.Controls.Add(this.lblAlertMethod);
            this.tpAlerts.Controls.Add(this.cmdEditMonitorItem);
            this.tpAlerts.Controls.Add(this.cboAlertConnections);
            this.tpAlerts.Controls.Add(this.cmdDeleteMonitorItem);
            this.tpAlerts.Controls.Add(this.dgvMonitorItems);
            this.tpAlerts.Controls.Add(this.cboAlertCondictionValues);
            this.tpAlerts.Controls.Add(this.txtAlertTarget);
            this.tpAlerts.Controls.Add(this.cboAlertConditionTypes);
            this.tpAlerts.Controls.Add(this.cmdSaveMonitorItem);
            this.tpAlerts.Controls.Add(this.lblAlert);
            this.tpAlerts.Controls.Add(this.cboAlertTypes);
            this.tpAlerts.Location = new System.Drawing.Point(4, 22);
            this.tpAlerts.Name = "tpAlerts";
            this.tpAlerts.Padding = new System.Windows.Forms.Padding(3);
            this.tpAlerts.Size = new System.Drawing.Size(600, 552);
            this.tpAlerts.TabIndex = 5;
            this.tpAlerts.Text = "Alerts";
            this.tpAlerts.UseVisualStyleBackColor = true;
            // 
            // chkEnableAlert
            // 
            this.chkEnableAlert.AutoSize = true;
            this.chkEnableAlert.Checked = true;
            this.chkEnableAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableAlert.Location = new System.Drawing.Point(474, 12);
            this.chkEnableAlert.Name = "chkEnableAlert";
            this.chkEnableAlert.Size = new System.Drawing.Size(60, 16);
            this.chkEnableAlert.TabIndex = 18;
            this.chkEnableAlert.Text = "Enable";
            this.chkEnableAlert.UseVisualStyleBackColor = true;
            // 
            // cmdNewMonitorItem
            // 
            this.cmdNewMonitorItem.Location = new System.Drawing.Point(86, 209);
            this.cmdNewMonitorItem.Name = "cmdNewMonitorItem";
            this.cmdNewMonitorItem.Size = new System.Drawing.Size(111, 23);
            this.cmdNewMonitorItem.TabIndex = 17;
            this.cmdNewMonitorItem.Text = "&New";
            this.cmdNewMonitorItem.UseVisualStyleBackColor = true;
            this.cmdNewMonitorItem.Click += new System.EventHandler(this.OnNewMonitorItemClick);
            // 
            // cboAlertTitle
            // 
            this.cboAlertTitle.FormattingEnabled = true;
            this.cboAlertTitle.Location = new System.Drawing.Point(86, 144);
            this.cboAlertTitle.Name = "cboAlertTitle";
            this.cboAlertTitle.Size = new System.Drawing.Size(173, 20);
            this.cboAlertTitle.TabIndex = 16;
            // 
            // lblAlertTitle
            // 
            this.lblAlertTitle.AutoSize = true;
            this.lblAlertTitle.Location = new System.Drawing.Point(6, 147);
            this.lblAlertTitle.Name = "lblAlertTitle";
            this.lblAlertTitle.Size = new System.Drawing.Size(41, 12);
            this.lblAlertTitle.TabIndex = 15;
            this.lblAlertTitle.Text = "&Title:";
            // 
            // lblAlertMetrict
            // 
            this.lblAlertMetrict.AutoSize = true;
            this.lblAlertMetrict.Location = new System.Drawing.Point(462, 112);
            this.lblAlertMetrict.Name = "lblAlertMetrict";
            this.lblAlertMetrict.Size = new System.Drawing.Size(0, 12);
            this.lblAlertMetrict.TabIndex = 14;
            // 
            // cboAlertMethods
            // 
            this.cboAlertMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlertMethods.FormattingEnabled = true;
            this.cboAlertMethods.Location = new System.Drawing.Point(86, 175);
            this.cboAlertMethods.Name = "cboAlertMethods";
            this.cboAlertMethods.Size = new System.Drawing.Size(173, 20);
            this.cboAlertMethods.TabIndex = 13;
            // 
            // lblAlertMethod
            // 
            this.lblAlertMethod.AutoSize = true;
            this.lblAlertMethod.Location = new System.Drawing.Point(6, 178);
            this.lblAlertMethod.Name = "lblAlertMethod";
            this.lblAlertMethod.Size = new System.Drawing.Size(29, 12);
            this.lblAlertMethod.TabIndex = 12;
            this.lblAlertMethod.Text = "&Via:";
            // 
            // cmdEditMonitorItem
            // 
            this.cmdEditMonitorItem.Location = new System.Drawing.Point(439, 209);
            this.cmdEditMonitorItem.Name = "cmdEditMonitorItem";
            this.cmdEditMonitorItem.Size = new System.Drawing.Size(111, 23);
            this.cmdEditMonitorItem.TabIndex = 11;
            this.cmdEditMonitorItem.Text = "&Edit";
            this.cmdEditMonitorItem.UseVisualStyleBackColor = true;
            this.cmdEditMonitorItem.Click += new System.EventHandler(this.OnEditMonitorItemClick);
            // 
            // cboAlertConnections
            // 
            this.cboAlertConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlertConnections.FormattingEnabled = true;
            this.cboAlertConnections.Location = new System.Drawing.Point(86, 9);
            this.cboAlertConnections.Name = "cboAlertConnections";
            this.cboAlertConnections.Size = new System.Drawing.Size(173, 20);
            this.cboAlertConnections.TabIndex = 10;
            this.cboAlertConnections.DropDown += new System.EventHandler(this.OnAlertConnectionsDropDown);
            // 
            // cmdDeleteMonitorItem
            // 
            this.cmdDeleteMonitorItem.Location = new System.Drawing.Point(322, 209);
            this.cmdDeleteMonitorItem.Name = "cmdDeleteMonitorItem";
            this.cmdDeleteMonitorItem.Size = new System.Drawing.Size(111, 23);
            this.cmdDeleteMonitorItem.TabIndex = 9;
            this.cmdDeleteMonitorItem.Text = "&Remove";
            this.cmdDeleteMonitorItem.UseVisualStyleBackColor = true;
            this.cmdDeleteMonitorItem.Click += new System.EventHandler(this.OnDeleteMonitorItemClick);
            // 
            // dgvMonitorItems
            // 
            this.dgvMonitorItems.AllowUserToAddRows = false;
            this.dgvMonitorItems.AllowUserToDeleteRows = false;
            this.dgvMonitorItems.AllowUserToResizeRows = false;
            this.dgvMonitorItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitorItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMonitorItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitorItems.Location = new System.Drawing.Point(7, 242);
            this.dgvMonitorItems.Name = "dgvMonitorItems";
            this.dgvMonitorItems.ReadOnly = true;
            this.dgvMonitorItems.RowHeadersVisible = false;
            this.dgvMonitorItems.RowTemplate.Height = 24;
            this.dgvMonitorItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMonitorItems.Size = new System.Drawing.Size(590, 302);
            this.dgvMonitorItems.TabIndex = 8;
            this.dgvMonitorItems.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnMonitorItemsCellMouseDoubleClick);
            // 
            // cboAlertCondictionValues
            // 
            this.cboAlertCondictionValues.FormattingEnabled = true;
            this.cboAlertCondictionValues.Location = new System.Drawing.Point(280, 109);
            this.cboAlertCondictionValues.Name = "cboAlertCondictionValues";
            this.cboAlertCondictionValues.Size = new System.Drawing.Size(173, 20);
            this.cboAlertCondictionValues.TabIndex = 7;
            // 
            // txtAlertTarget
            // 
            this.txtAlertTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlertTarget.Enabled = false;
            this.txtAlertTarget.Location = new System.Drawing.Point(86, 41);
            this.txtAlertTarget.Multiline = true;
            this.txtAlertTarget.Name = "txtAlertTarget";
            this.txtAlertTarget.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAlertTarget.Size = new System.Drawing.Size(506, 53);
            this.txtAlertTarget.TabIndex = 6;
            // 
            // cboAlertConditionTypes
            // 
            this.cboAlertConditionTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlertConditionTypes.FormattingEnabled = true;
            this.cboAlertConditionTypes.Location = new System.Drawing.Point(86, 109);
            this.cboAlertConditionTypes.Name = "cboAlertConditionTypes";
            this.cboAlertConditionTypes.Size = new System.Drawing.Size(173, 20);
            this.cboAlertConditionTypes.TabIndex = 5;
            this.cboAlertConditionTypes.SelectedIndexChanged += new System.EventHandler(this.OnCondictionTypesSelectedIndexChanged);
            // 
            // cmdSaveMonitorItem
            // 
            this.cmdSaveMonitorItem.Location = new System.Drawing.Point(205, 209);
            this.cmdSaveMonitorItem.Name = "cmdSaveMonitorItem";
            this.cmdSaveMonitorItem.Size = new System.Drawing.Size(111, 23);
            this.cmdSaveMonitorItem.TabIndex = 4;
            this.cmdSaveMonitorItem.Text = "&Save";
            this.cmdSaveMonitorItem.UseVisualStyleBackColor = true;
            this.cmdSaveMonitorItem.Click += new System.EventHandler(this.OnSaveMonitorItemClick);
            // 
            // lblAlert
            // 
            this.lblAlert.AutoSize = true;
            this.lblAlert.Location = new System.Drawing.Point(5, 9);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(71, 12);
            this.lblAlert.TabIndex = 3;
            this.lblAlert.Text = "&Alert when:";
            // 
            // cboAlertTypes
            // 
            this.cboAlertTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlertTypes.FormattingEnabled = true;
            this.cboAlertTypes.Location = new System.Drawing.Point(280, 9);
            this.cboAlertTypes.Name = "cboAlertTypes";
            this.cboAlertTypes.Size = new System.Drawing.Size(173, 20);
            this.cboAlertTypes.TabIndex = 2;
            this.cboAlertTypes.SelectedIndexChanged += new System.EventHandler(this.OnAlertTypesSelectedIndexChanged);
            // 
            // tpHistories
            // 
            this.tpHistories.Controls.Add(this.splitContainer5);
            this.tpHistories.Location = new System.Drawing.Point(4, 22);
            this.tpHistories.Name = "tpHistories";
            this.tpHistories.Padding = new System.Windows.Forms.Padding(3);
            this.tpHistories.Size = new System.Drawing.Size(600, 552);
            this.tpHistories.TabIndex = 8;
            this.tpHistories.Text = "Histories";
            this.tpHistories.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.dgvHistories);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.rtbHistoryDetail);
            this.splitContainer5.Size = new System.Drawing.Size(594, 546);
            this.splitContainer5.SplitterDistance = 273;
            this.splitContainer5.TabIndex = 10;
            // 
            // dgvHistories
            // 
            this.dgvHistories.AllowUserToAddRows = false;
            this.dgvHistories.AllowUserToDeleteRows = false;
            this.dgvHistories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHistories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistories.Location = new System.Drawing.Point(0, 0);
            this.dgvHistories.Name = "dgvHistories";
            this.dgvHistories.ReadOnly = true;
            this.dgvHistories.RowHeadersVisible = false;
            this.dgvHistories.RowTemplate.Height = 24;
            this.dgvHistories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistories.Size = new System.Drawing.Size(594, 273);
            this.dgvHistories.TabIndex = 9;
            this.dgvHistories.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnHistoriesRowEnter);
            // 
            // rtbHistoryDetail
            // 
            this.rtbHistoryDetail.AutoScroll = true;
            this.rtbHistoryDetail.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbHistoryDetail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbHistoryDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHistoryDetail.IsReadOnly = false;
            this.rtbHistoryDetail.Location = new System.Drawing.Point(0, 0);
            this.rtbHistoryDetail.Name = "rtbHistoryDetail";
            this.rtbHistoryDetail.Size = new System.Drawing.Size(594, 269);
            this.rtbHistoryDetail.TabIndex = 4;
            // 
            // tpOptions
            // 
            this.tpOptions.Controls.Add(this.gbAnalysisOptions);
            this.tpOptions.Controls.Add(this.gbAlertTemplate);
            this.tpOptions.Controls.Add(this.gbAlertMethod);
            this.tpOptions.Controls.Add(this.gbBasic);
            this.tpOptions.Location = new System.Drawing.Point(4, 22);
            this.tpOptions.Name = "tpOptions";
            this.tpOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tpOptions.Size = new System.Drawing.Size(600, 552);
            this.tpOptions.TabIndex = 7;
            this.tpOptions.Text = "Options";
            this.tpOptions.UseVisualStyleBackColor = true;
            // 
            // gbAnalysisOptions
            // 
            this.gbAnalysisOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAnalysisOptions.Controls.Add(this.lblFreeCPURatio);
            this.gbAnalysisOptions.Controls.Add(this.cboFreeCPURatios);
            this.gbAnalysisOptions.Controls.Add(this.lblFreeMemoryRatio);
            this.gbAnalysisOptions.Controls.Add(this.cboFreeMemoryRatios);
            this.gbAnalysisOptions.Controls.Add(this.lblTableDataIndexSpaceRatio);
            this.gbAnalysisOptions.Controls.Add(this.cboTableDataIndexSpaceRatios);
            this.gbAnalysisOptions.Controls.Add(this.lblDatabaseDataLogSpaceRatio);
            this.gbAnalysisOptions.Controls.Add(this.cboDatabaseDataLogSpaceRatios);
            this.gbAnalysisOptions.Controls.Add(this.lblDatabaseDiskFreeSpaceRatio);
            this.gbAnalysisOptions.Controls.Add(this.cboDatabaseDiskFreeSpaceRatios);
            this.gbAnalysisOptions.Location = new System.Drawing.Point(9, 141);
            this.gbAnalysisOptions.Name = "gbAnalysisOptions";
            this.gbAnalysisOptions.Size = new System.Drawing.Size(583, 113);
            this.gbAnalysisOptions.TabIndex = 18;
            this.gbAnalysisOptions.TabStop = false;
            this.gbAnalysisOptions.Text = "Analysis";
            // 
            // lblFreeCPURatio
            // 
            this.lblFreeCPURatio.AutoSize = true;
            this.lblFreeCPURatio.Location = new System.Drawing.Point(15, 81);
            this.lblFreeCPURatio.Name = "lblFreeCPURatio";
            this.lblFreeCPURatio.Size = new System.Drawing.Size(89, 12);
            this.lblFreeCPURatio.TabIndex = 17;
            this.lblFreeCPURatio.Text = "Free CPU Ratio";
            // 
            // cboFreeCPURatios
            // 
            this.cboFreeCPURatios.FormattingEnabled = true;
            this.cboFreeCPURatios.Location = new System.Drawing.Point(122, 78);
            this.cboFreeCPURatios.MaxLength = 5;
            this.cboFreeCPURatios.Name = "cboFreeCPURatios";
            this.cboFreeCPURatios.Size = new System.Drawing.Size(104, 20);
            this.cboFreeCPURatios.TabIndex = 18;
            this.cboFreeCPURatios.TextChanged += new System.EventHandler(this.OnFreeCpuRatiosTextChanged);
            // 
            // lblFreeMemoryRatio
            // 
            this.lblFreeMemoryRatio.AutoSize = true;
            this.lblFreeMemoryRatio.Location = new System.Drawing.Point(295, 55);
            this.lblFreeMemoryRatio.Name = "lblFreeMemoryRatio";
            this.lblFreeMemoryRatio.Size = new System.Drawing.Size(107, 12);
            this.lblFreeMemoryRatio.TabIndex = 15;
            this.lblFreeMemoryRatio.Text = "Free Memory Ratio";
            // 
            // cboFreeMemoryRatios
            // 
            this.cboFreeMemoryRatios.FormattingEnabled = true;
            this.cboFreeMemoryRatios.Location = new System.Drawing.Point(418, 52);
            this.cboFreeMemoryRatios.MaxLength = 5;
            this.cboFreeMemoryRatios.Name = "cboFreeMemoryRatios";
            this.cboFreeMemoryRatios.Size = new System.Drawing.Size(104, 20);
            this.cboFreeMemoryRatios.TabIndex = 16;
            this.cboFreeMemoryRatios.TextChanged += new System.EventHandler(this.OnFreeMemoryRatiosTextChanged);
            // 
            // lblTableDataIndexSpaceRatio
            // 
            this.lblTableDataIndexSpaceRatio.AutoSize = true;
            this.lblTableDataIndexSpaceRatio.Location = new System.Drawing.Point(15, 52);
            this.lblTableDataIndexSpaceRatio.Name = "lblTableDataIndexSpaceRatio";
            this.lblTableDataIndexSpaceRatio.Size = new System.Drawing.Size(107, 12);
            this.lblTableDataIndexSpaceRatio.TabIndex = 13;
            this.lblTableDataIndexSpaceRatio.Text = "Table Index Ratio";
            // 
            // cboTableDataIndexSpaceRatios
            // 
            this.cboTableDataIndexSpaceRatios.FormattingEnabled = true;
            this.cboTableDataIndexSpaceRatios.Location = new System.Drawing.Point(122, 49);
            this.cboTableDataIndexSpaceRatios.MaxLength = 5;
            this.cboTableDataIndexSpaceRatios.Name = "cboTableDataIndexSpaceRatios";
            this.cboTableDataIndexSpaceRatios.Size = new System.Drawing.Size(104, 20);
            this.cboTableDataIndexSpaceRatios.TabIndex = 14;
            this.cboTableDataIndexSpaceRatios.TextChanged += new System.EventHandler(this.OnTableDataIndexSpaceRatiosTextChanged);
            // 
            // lblDatabaseDataLogSpaceRatio
            // 
            this.lblDatabaseDataLogSpaceRatio.AutoSize = true;
            this.lblDatabaseDataLogSpaceRatio.Location = new System.Drawing.Point(295, 26);
            this.lblDatabaseDataLogSpaceRatio.Name = "lblDatabaseDataLogSpaceRatio";
            this.lblDatabaseDataLogSpaceRatio.Size = new System.Drawing.Size(113, 12);
            this.lblDatabaseDataLogSpaceRatio.TabIndex = 11;
            this.lblDatabaseDataLogSpaceRatio.Text = "DB Log Space Ratio";
            // 
            // cboDatabaseDataLogSpaceRatios
            // 
            this.cboDatabaseDataLogSpaceRatios.FormattingEnabled = true;
            this.cboDatabaseDataLogSpaceRatios.Location = new System.Drawing.Point(418, 23);
            this.cboDatabaseDataLogSpaceRatios.MaxLength = 5;
            this.cboDatabaseDataLogSpaceRatios.Name = "cboDatabaseDataLogSpaceRatios";
            this.cboDatabaseDataLogSpaceRatios.Size = new System.Drawing.Size(104, 20);
            this.cboDatabaseDataLogSpaceRatios.TabIndex = 12;
            this.cboDatabaseDataLogSpaceRatios.TextChanged += new System.EventHandler(this.OnDatabaseDataLogSpaceRatiosTextChanged);
            // 
            // lblDatabaseDiskFreeSpaceRatio
            // 
            this.lblDatabaseDiskFreeSpaceRatio.AutoSize = true;
            this.lblDatabaseDiskFreeSpaceRatio.Location = new System.Drawing.Point(15, 23);
            this.lblDatabaseDiskFreeSpaceRatio.Name = "lblDatabaseDiskFreeSpaceRatio";
            this.lblDatabaseDiskFreeSpaceRatio.Size = new System.Drawing.Size(95, 12);
            this.lblDatabaseDiskFreeSpaceRatio.TabIndex = 9;
            this.lblDatabaseDiskFreeSpaceRatio.Text = "Free Disk Ratio";
            // 
            // cboDatabaseDiskFreeSpaceRatios
            // 
            this.cboDatabaseDiskFreeSpaceRatios.FormattingEnabled = true;
            this.cboDatabaseDiskFreeSpaceRatios.Location = new System.Drawing.Point(122, 20);
            this.cboDatabaseDiskFreeSpaceRatios.MaxLength = 5;
            this.cboDatabaseDiskFreeSpaceRatios.Name = "cboDatabaseDiskFreeSpaceRatios";
            this.cboDatabaseDiskFreeSpaceRatios.Size = new System.Drawing.Size(104, 20);
            this.cboDatabaseDiskFreeSpaceRatios.TabIndex = 10;
            this.cboDatabaseDiskFreeSpaceRatios.TextChanged += new System.EventHandler(this.OnDatabaseDiskFreeSpaceRatiosTextChanged);
            // 
            // gbAlertTemplate
            // 
            this.gbAlertTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAlertTemplate.Controls.Add(this.cmdSetDefaultAlertTemplate);
            this.gbAlertTemplate.Controls.Add(this.rtbAlertTemplate);
            this.gbAlertTemplate.Location = new System.Drawing.Point(9, 370);
            this.gbAlertTemplate.Name = "gbAlertTemplate";
            this.gbAlertTemplate.Size = new System.Drawing.Size(583, 163);
            this.gbAlertTemplate.TabIndex = 17;
            this.gbAlertTemplate.TabStop = false;
            this.gbAlertTemplate.Text = "Alert Template";
            // 
            // cmdSetDefaultAlertTemplate
            // 
            this.cmdSetDefaultAlertTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSetDefaultAlertTemplate.Location = new System.Drawing.Point(7, 126);
            this.cmdSetDefaultAlertTemplate.Name = "cmdSetDefaultAlertTemplate";
            this.cmdSetDefaultAlertTemplate.Size = new System.Drawing.Size(184, 23);
            this.cmdSetDefaultAlertTemplate.TabIndex = 7;
            this.cmdSetDefaultAlertTemplate.Text = "Set &Default";
            this.cmdSetDefaultAlertTemplate.UseVisualStyleBackColor = true;
            this.cmdSetDefaultAlertTemplate.Click += new System.EventHandler(this.OnSetDefaultAlertTemplateClick);
            // 
            // rtbAlertTemplate
            // 
            this.rtbAlertTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbAlertTemplate.Location = new System.Drawing.Point(7, 22);
            this.rtbAlertTemplate.Name = "rtbAlertTemplate";
            this.rtbAlertTemplate.Size = new System.Drawing.Size(570, 98);
            this.rtbAlertTemplate.TabIndex = 0;
            this.rtbAlertTemplate.Text = "";
            // 
            // gbAlertMethod
            // 
            this.gbAlertMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAlertMethod.Controls.Add(this.lblAlertMailReceivers);
            this.gbAlertMethod.Controls.Add(this.txtAlertMailReceiver);
            this.gbAlertMethod.Controls.Add(this.cboAlertMailServers);
            this.gbAlertMethod.Controls.Add(this.lblAlertMailServer);
            this.gbAlertMethod.Controls.Add(this.txtAlertMailPassword);
            this.gbAlertMethod.Controls.Add(this.lblAlertUserName);
            this.gbAlertMethod.Controls.Add(this.lblAlertMailPassword);
            this.gbAlertMethod.Controls.Add(this.txtAlertMailUser);
            this.gbAlertMethod.Location = new System.Drawing.Point(9, 260);
            this.gbAlertMethod.Name = "gbAlertMethod";
            this.gbAlertMethod.Size = new System.Drawing.Size(583, 100);
            this.gbAlertMethod.TabIndex = 16;
            this.gbAlertMethod.TabStop = false;
            this.gbAlertMethod.Text = "Alert Method";
            // 
            // lblAlertMailReceivers
            // 
            this.lblAlertMailReceivers.AutoSize = true;
            this.lblAlertMailReceivers.Location = new System.Drawing.Point(295, 31);
            this.lblAlertMailReceivers.Name = "lblAlertMailReceivers";
            this.lblAlertMailReceivers.Size = new System.Drawing.Size(59, 12);
            this.lblAlertMailReceivers.TabIndex = 15;
            this.lblAlertMailReceivers.Text = "&Receiver:";
            // 
            // txtAlertMailReceiver
            // 
            this.txtAlertMailReceiver.Location = new System.Drawing.Point(393, 28);
            this.txtAlertMailReceiver.Name = "txtAlertMailReceiver";
            this.txtAlertMailReceiver.Size = new System.Drawing.Size(138, 21);
            this.txtAlertMailReceiver.TabIndex = 16;
            // 
            // cboAlertMailServers
            // 
            this.cboAlertMailServers.FormattingEnabled = true;
            this.cboAlertMailServers.Location = new System.Drawing.Point(120, 30);
            this.cboAlertMailServers.Name = "cboAlertMailServers";
            this.cboAlertMailServers.Size = new System.Drawing.Size(138, 20);
            this.cboAlertMailServers.TabIndex = 10;
            // 
            // lblAlertMailServer
            // 
            this.lblAlertMailServer.AutoSize = true;
            this.lblAlertMailServer.Location = new System.Drawing.Point(15, 33);
            this.lblAlertMailServer.Name = "lblAlertMailServer";
            this.lblAlertMailServer.Size = new System.Drawing.Size(77, 12);
            this.lblAlertMailServer.TabIndex = 9;
            this.lblAlertMailServer.Text = "Mail &Server:";
            // 
            // txtAlertMailPassword
            // 
            this.txtAlertMailPassword.Location = new System.Drawing.Point(393, 62);
            this.txtAlertMailPassword.Name = "txtAlertMailPassword";
            this.txtAlertMailPassword.PasswordChar = '*';
            this.txtAlertMailPassword.Size = new System.Drawing.Size(138, 21);
            this.txtAlertMailPassword.TabIndex = 14;
            // 
            // lblAlertUserName
            // 
            this.lblAlertUserName.AutoSize = true;
            this.lblAlertUserName.Location = new System.Drawing.Point(15, 65);
            this.lblAlertUserName.Name = "lblAlertUserName";
            this.lblAlertUserName.Size = new System.Drawing.Size(71, 12);
            this.lblAlertUserName.TabIndex = 11;
            this.lblAlertUserName.Text = "Login &User:";
            // 
            // lblAlertMailPassword
            // 
            this.lblAlertMailPassword.AutoSize = true;
            this.lblAlertMailPassword.Location = new System.Drawing.Point(295, 65);
            this.lblAlertMailPassword.Name = "lblAlertMailPassword";
            this.lblAlertMailPassword.Size = new System.Drawing.Size(95, 12);
            this.lblAlertMailPassword.TabIndex = 13;
            this.lblAlertMailPassword.Text = "Login &Password:";
            // 
            // txtAlertMailUser
            // 
            this.txtAlertMailUser.Location = new System.Drawing.Point(120, 62);
            this.txtAlertMailUser.Name = "txtAlertMailUser";
            this.txtAlertMailUser.Size = new System.Drawing.Size(138, 21);
            this.txtAlertMailUser.TabIndex = 12;
            // 
            // gbBasic
            // 
            this.gbBasic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBasic.Controls.Add(this.lblPerformanceIntervalSeconds);
            this.gbBasic.Controls.Add(this.lblPerformanceInterval);
            this.gbBasic.Controls.Add(this.cboPerformanceIntervals);
            this.gbBasic.Controls.Add(this.lblMonitorRefreshIntervalSeconds);
            this.gbBasic.Controls.Add(this.lblConnectionTimeoutSeconds);
            this.gbBasic.Controls.Add(this.chkLogHistory);
            this.gbBasic.Controls.Add(this.chkAutoWordWrap);
            this.gbBasic.Controls.Add(this.cmdChooseFont);
            this.gbBasic.Controls.Add(this.lblFont);
            this.gbBasic.Controls.Add(this.lblConnectionTimeout);
            this.gbBasic.Controls.Add(this.lblMonitorRefreshInterval);
            this.gbBasic.Controls.Add(this.cboConnectionTimeouts);
            this.gbBasic.Controls.Add(this.cboMonitorRefreshIntervals);
            this.gbBasic.Location = new System.Drawing.Point(9, 6);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(583, 122);
            this.gbBasic.TabIndex = 15;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "Basic";
            // 
            // lblPerformanceIntervalSeconds
            // 
            this.lblPerformanceIntervalSeconds.AutoSize = true;
            this.lblPerformanceIntervalSeconds.Location = new System.Drawing.Point(232, 95);
            this.lblPerformanceIntervalSeconds.Name = "lblPerformanceIntervalSeconds";
            this.lblPerformanceIntervalSeconds.Size = new System.Drawing.Size(29, 12);
            this.lblPerformanceIntervalSeconds.TabIndex = 13;
            this.lblPerformanceIntervalSeconds.Text = "Secs";
            // 
            // lblPerformanceInterval
            // 
            this.lblPerformanceInterval.AutoSize = true;
            this.lblPerformanceInterval.Location = new System.Drawing.Point(15, 92);
            this.lblPerformanceInterval.Name = "lblPerformanceInterval";
            this.lblPerformanceInterval.Size = new System.Drawing.Size(89, 12);
            this.lblPerformanceInterval.TabIndex = 11;
            this.lblPerformanceInterval.Text = "&Perf Interval:";
            // 
            // cboPerformanceIntervals
            // 
            this.cboPerformanceIntervals.FormattingEnabled = true;
            this.cboPerformanceIntervals.Location = new System.Drawing.Point(122, 89);
            this.cboPerformanceIntervals.MaxLength = 5;
            this.cboPerformanceIntervals.Name = "cboPerformanceIntervals";
            this.cboPerformanceIntervals.Size = new System.Drawing.Size(104, 20);
            this.cboPerformanceIntervals.TabIndex = 12;
            this.cboPerformanceIntervals.TextChanged += new System.EventHandler(this.OnPerformanceIntervalsTextChanged);
            // 
            // lblMonitorRefreshIntervalSeconds
            // 
            this.lblMonitorRefreshIntervalSeconds.AutoSize = true;
            this.lblMonitorRefreshIntervalSeconds.Location = new System.Drawing.Point(232, 61);
            this.lblMonitorRefreshIntervalSeconds.Name = "lblMonitorRefreshIntervalSeconds";
            this.lblMonitorRefreshIntervalSeconds.Size = new System.Drawing.Size(29, 12);
            this.lblMonitorRefreshIntervalSeconds.TabIndex = 10;
            this.lblMonitorRefreshIntervalSeconds.Text = "Secs";
            // 
            // lblConnectionTimeoutSeconds
            // 
            this.lblConnectionTimeoutSeconds.AutoSize = true;
            this.lblConnectionTimeoutSeconds.Location = new System.Drawing.Point(528, 61);
            this.lblConnectionTimeoutSeconds.Name = "lblConnectionTimeoutSeconds";
            this.lblConnectionTimeoutSeconds.Size = new System.Drawing.Size(29, 12);
            this.lblConnectionTimeoutSeconds.TabIndex = 10;
            this.lblConnectionTimeoutSeconds.Text = "Secs";
            // 
            // chkLogHistory
            // 
            this.chkLogHistory.AutoSize = true;
            this.chkLogHistory.Checked = true;
            this.chkLogHistory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogHistory.Location = new System.Drawing.Point(297, 91);
            this.chkLogHistory.Name = "chkLogHistory";
            this.chkLogHistory.Size = new System.Drawing.Size(90, 16);
            this.chkLogHistory.TabIndex = 9;
            this.chkLogHistory.Text = "Log History";
            this.chkLogHistory.UseVisualStyleBackColor = true;
            this.chkLogHistory.CheckedChanged += new System.EventHandler(this.OnLogHistoryCheckedChanged);
            // 
            // chkAutoWordWrap
            // 
            this.chkAutoWordWrap.AutoSize = true;
            this.chkAutoWordWrap.Checked = true;
            this.chkAutoWordWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoWordWrap.Location = new System.Drawing.Point(449, 92);
            this.chkAutoWordWrap.Name = "chkAutoWordWrap";
            this.chkAutoWordWrap.Size = new System.Drawing.Size(108, 16);
            this.chkAutoWordWrap.TabIndex = 9;
            this.chkAutoWordWrap.Text = "Auto word wrap";
            this.chkAutoWordWrap.UseVisualStyleBackColor = true;
            // 
            // cmdChooseFont
            // 
            this.cmdChooseFont.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdChooseFont.Location = new System.Drawing.Point(120, 21);
            this.cmdChooseFont.Name = "cmdChooseFont";
            this.cmdChooseFont.Size = new System.Drawing.Size(158, 23);
            this.cmdChooseFont.TabIndex = 6;
            this.cmdChooseFont.Text = "&Choose Font";
            this.cmdChooseFont.UseVisualStyleBackColor = true;
            this.cmdChooseFont.Click += new System.EventHandler(this.OnChooseFontClick);
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(15, 26);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(35, 12);
            this.lblFont.TabIndex = 5;
            this.lblFont.Text = "&Font:";
            // 
            // lblConnectionTimeout
            // 
            this.lblConnectionTimeout.AutoSize = true;
            this.lblConnectionTimeout.Location = new System.Drawing.Point(295, 61);
            this.lblConnectionTimeout.Name = "lblConnectionTimeout";
            this.lblConnectionTimeout.Size = new System.Drawing.Size(119, 12);
            this.lblConnectionTimeout.TabIndex = 7;
            this.lblConnectionTimeout.Text = "Connection Timeout:";
            // 
            // lblMonitorRefreshInterval
            // 
            this.lblMonitorRefreshInterval.AutoSize = true;
            this.lblMonitorRefreshInterval.Location = new System.Drawing.Point(15, 61);
            this.lblMonitorRefreshInterval.Name = "lblMonitorRefreshInterval";
            this.lblMonitorRefreshInterval.Size = new System.Drawing.Size(107, 12);
            this.lblMonitorRefreshInterval.TabIndex = 7;
            this.lblMonitorRefreshInterval.Text = "&Monitor Interval:";
            // 
            // cboConnectionTimeouts
            // 
            this.cboConnectionTimeouts.FormattingEnabled = true;
            this.cboConnectionTimeouts.Location = new System.Drawing.Point(418, 58);
            this.cboConnectionTimeouts.MaxLength = 5;
            this.cboConnectionTimeouts.Name = "cboConnectionTimeouts";
            this.cboConnectionTimeouts.Size = new System.Drawing.Size(104, 20);
            this.cboConnectionTimeouts.TabIndex = 8;
            this.cboConnectionTimeouts.TextChanged += new System.EventHandler(this.OnConnectionTimeoutsTextChanged);
            // 
            // cboMonitorRefreshIntervals
            // 
            this.cboMonitorRefreshIntervals.FormattingEnabled = true;
            this.cboMonitorRefreshIntervals.Location = new System.Drawing.Point(122, 58);
            this.cboMonitorRefreshIntervals.MaxLength = 5;
            this.cboMonitorRefreshIntervals.Name = "cboMonitorRefreshIntervals";
            this.cboMonitorRefreshIntervals.Size = new System.Drawing.Size(104, 20);
            this.cboMonitorRefreshIntervals.TabIndex = 8;
            this.cboMonitorRefreshIntervals.TextChanged += new System.EventHandler(this.OnMonitorRefreshIntervalsTextChanged);
            // 
            // cmsObjects
            // 
            this.cmsObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiRegisterServer,
            this.tmiEditServer,
            this.tmiRemoveServer,
            this.tmiTruncateObject,
            this.tmiShrinkDatabase,
            this.tmiCheckDB,
            this.tmiTruncateTable,
            this.tmiCleanTable,
            this.tmiTableIndexDefrag,
            this.tmiSetDatabaseState,
            this.tmiDetachDatabase,
            this.tmiAttachDatabase,
            this.tmiBackupDatabase,
            this.tmiRestoreDatabase,
            this.tmiShowPerformance,
            this.tmiSearchDatabase,
            this.tmiObjectDependencies,
            this.tmiSetVersionControl,
            this.tmiOpenTable,
            this.tmiNewQuery});
            this.cmsObjects.Name = "cmsObjects";
            this.cmsObjects.Size = new System.Drawing.Size(239, 444);
            this.cmsObjects.Opening += new System.ComponentModel.CancelEventHandler(this.OnObjectsMenuOpening);
            // 
            // tmiRegisterServer
            // 
            this.tmiRegisterServer.Name = "tmiRegisterServer";
            this.tmiRegisterServer.Size = new System.Drawing.Size(238, 22);
            this.tmiRegisterServer.Text = "Register Connection";
            this.tmiRegisterServer.Click += new System.EventHandler(this.OnRegisterServerClick);
            // 
            // tmiEditServer
            // 
            this.tmiEditServer.Name = "tmiEditServer";
            this.tmiEditServer.Size = new System.Drawing.Size(238, 22);
            this.tmiEditServer.Text = "Edit Connection";
            this.tmiEditServer.Click += new System.EventHandler(this.OnEditServerClick);
            // 
            // tmiRemoveServer
            // 
            this.tmiRemoveServer.Name = "tmiRemoveServer";
            this.tmiRemoveServer.Size = new System.Drawing.Size(238, 22);
            this.tmiRemoveServer.Text = "Remove Connection";
            this.tmiRemoveServer.Click += new System.EventHandler(this.OnRemoveServerClick);
            // 
            // tmiTruncateObject
            // 
            this.tmiTruncateObject.Name = "tmiTruncateObject";
            this.tmiTruncateObject.Size = new System.Drawing.Size(238, 22);
            this.tmiTruncateObject.Text = "Truncate Log";
            this.tmiTruncateObject.Click += new System.EventHandler(this.OnTruncateObjectClick);
            // 
            // tmiShrinkDatabase
            // 
            this.tmiShrinkDatabase.Name = "tmiShrinkDatabase";
            this.tmiShrinkDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiShrinkDatabase.Text = "Shrink";
            this.tmiShrinkDatabase.Click += new System.EventHandler(this.OnShrinkDatabaseClick);
            // 
            // tmiCheckDB
            // 
            this.tmiCheckDB.Name = "tmiCheckDB";
            this.tmiCheckDB.Size = new System.Drawing.Size(238, 22);
            this.tmiCheckDB.Text = "Check DB";
            this.tmiCheckDB.Click += new System.EventHandler(this.OnCheckDbClick);
            // 
            // tmiTruncateTable
            // 
            this.tmiTruncateTable.Name = "tmiTruncateTable";
            this.tmiTruncateTable.Size = new System.Drawing.Size(238, 22);
            this.tmiTruncateTable.Text = "Truncate Table";
            this.tmiTruncateTable.Click += new System.EventHandler(this.OnTruncateTableClick);
            // 
            // tmiCleanTable
            // 
            this.tmiCleanTable.Name = "tmiCleanTable";
            this.tmiCleanTable.Size = new System.Drawing.Size(238, 22);
            this.tmiCleanTable.Text = "Clean Table (Recycle Space)";
            this.tmiCleanTable.Click += new System.EventHandler(this.OnCleanTableClick);
            // 
            // tmiTableIndexDefrag
            // 
            this.tmiTableIndexDefrag.Name = "tmiTableIndexDefrag";
            this.tmiTableIndexDefrag.Size = new System.Drawing.Size(238, 22);
            this.tmiTableIndexDefrag.Text = "Index Defrag";
            this.tmiTableIndexDefrag.Click += new System.EventHandler(this.OnTableIndexDefragClick);
            // 
            // tmiSetDatabaseState
            // 
            this.tmiSetDatabaseState.Name = "tmiSetDatabaseState";
            this.tmiSetDatabaseState.Size = new System.Drawing.Size(238, 22);
            this.tmiSetDatabaseState.Text = "Set Offline";
            this.tmiSetDatabaseState.Click += new System.EventHandler(this.OnSetDatabaseStateClick);
            // 
            // tmiDetachDatabase
            // 
            this.tmiDetachDatabase.Name = "tmiDetachDatabase";
            this.tmiDetachDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiDetachDatabase.Text = "Detach";
            this.tmiDetachDatabase.Click += new System.EventHandler(this.OnDetachDatabaseClick);
            // 
            // tmiAttachDatabase
            // 
            this.tmiAttachDatabase.Name = "tmiAttachDatabase";
            this.tmiAttachDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiAttachDatabase.Text = "Attach";
            this.tmiAttachDatabase.Click += new System.EventHandler(this.OnAttachClick);
            // 
            // tmiBackupDatabase
            // 
            this.tmiBackupDatabase.Name = "tmiBackupDatabase";
            this.tmiBackupDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiBackupDatabase.Text = "Backup";
            this.tmiBackupDatabase.Click += new System.EventHandler(this.OnBackupDatabaseClick);
            // 
            // tmiRestoreDatabase
            // 
            this.tmiRestoreDatabase.Name = "tmiRestoreDatabase";
            this.tmiRestoreDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiRestoreDatabase.Text = "Restore";
            this.tmiRestoreDatabase.Click += new System.EventHandler(this.OnRestoreDatabaseClick);
            // 
            // tmiShowPerformance
            // 
            this.tmiShowPerformance.Name = "tmiShowPerformance";
            this.tmiShowPerformance.Size = new System.Drawing.Size(238, 22);
            this.tmiShowPerformance.Text = "Show Performance";
            this.tmiShowPerformance.Click += new System.EventHandler(this.OnShowUserPerformanceClick);
            // 
            // tmiSearchDatabase
            // 
            this.tmiSearchDatabase.Name = "tmiSearchDatabase";
            this.tmiSearchDatabase.Size = new System.Drawing.Size(238, 22);
            this.tmiSearchDatabase.Text = "Search";
            this.tmiSearchDatabase.Click += new System.EventHandler(this.OnSearchDatabaseClick);
            // 
            // tmiObjectDependencies
            // 
            this.tmiObjectDependencies.Name = "tmiObjectDependencies";
            this.tmiObjectDependencies.Size = new System.Drawing.Size(238, 22);
            this.tmiObjectDependencies.Text = "Dependencies";
            this.tmiObjectDependencies.Click += new System.EventHandler(this.OnObjectDependenciesClick);
            // 
            // tmiSetVersionControl
            // 
            this.tmiSetVersionControl.Name = "tmiSetVersionControl";
            this.tmiSetVersionControl.Size = new System.Drawing.Size(238, 22);
            this.tmiSetVersionControl.Text = "Version Control";
            this.tmiSetVersionControl.Click += new System.EventHandler(this.OnSetVersionControlClick);
            // 
            // tmiOpenTable
            // 
            this.tmiOpenTable.Name = "tmiOpenTable";
            this.tmiOpenTable.Size = new System.Drawing.Size(238, 22);
            this.tmiOpenTable.Text = "Open Top 100 Rows";
            this.tmiOpenTable.Click += new System.EventHandler(this.OnOpenTableClick);
            // 
            // tmiNewQuery
            // 
            this.tmiNewQuery.Name = "tmiNewQuery";
            this.tmiNewQuery.Size = new System.Drawing.Size(238, 22);
            this.tmiNewQuery.Text = "New Query";
            this.tmiNewQuery.Click += new System.EventHandler(this.OnNewQueryClick);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tvObjects);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcMain);
            this.scMain.Size = new System.Drawing.Size(851, 578);
            this.scMain.SplitterDistance = 239;
            this.scMain.TabIndex = 16;
            this.scMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnMainSpliterMoved);
            // 
            // tvObjects
            // 
            this.tvObjects.AllowDrop = true;
            this.tvObjects.ContextMenuStrip = this.cmsObjects;
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.HideSelection = false;
            this.tvObjects.HotTracking = true;
            this.tvObjects.ImageIndex = 1;
            this.tvObjects.ImageList = this.il16;
            this.tvObjects.LabelEdit = true;
            this.tvObjects.Location = new System.Drawing.Point(0, 0);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.SelectedImageIndex = 0;
            this.tvObjects.ShowLines = false;
            this.tvObjects.Size = new System.Drawing.Size(239, 578);
            this.tvObjects.TabIndex = 0;
            this.tvObjects.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnObjectsBeforeLabelEdit);
            this.tvObjects.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnObjectsAfterLabelEdit);
            this.tvObjects.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnObjectsBeforeExpand);
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnObjectsAfterSelect);
            this.tvObjects.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnObjectsNodeMouseDoubleClick);
            // 
            // il16
            // 
            this.il16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il16.ImageStream")));
            this.il16.TransparentColor = System.Drawing.Color.Transparent;
            this.il16.Images.SetKeyName(0, "server2.png");
            this.il16.Images.SetKeyName(1, "Folder2.png");
            this.il16.Images.SetKeyName(2, "Page2.png");
            this.il16.Images.SetKeyName(3, "Gear2.png");
            this.il16.Images.SetKeyName(4, "List2.gif");
            this.il16.Images.SetKeyName(5, "proxy2.png");
            this.il16.Images.SetKeyName(6, "Accelerator2.png");
            this.il16.Images.SetKeyName(7, "Edit2.png");
            this.il16.Images.SetKeyName(8, "Table2.gif");
            // 
            // epHint
            // 
            this.epHint.ContainerControl = this;
            // 
            // cmsObjectList
            // 
            this.cmsObjectList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiFindObjectReferences});
            this.cmsObjectList.Name = "cmsObjectList";
            this.cmsObjectList.Size = new System.Drawing.Size(178, 26);
            // 
            // tmiFindObjectReferences
            // 
            this.tmiFindObjectReferences.Name = "tmiFindObjectReferences";
            this.tmiFindObjectReferences.Size = new System.Drawing.Size(177, 22);
            this.tmiFindObjectReferences.Text = "&Find References...";
            this.tmiFindObjectReferences.Click += new System.EventHandler(this.OnFindObjectReferencesClick);
            // 
            // dgvServerHealth
            // 
            this.dgvServerHealth.AllowUserToAddRows = false;
            this.dgvServerHealth.AllowUserToDeleteRows = false;
            this.dgvServerHealth.AllowUserToResizeRows = false;
            this.dgvServerHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvServerHealth.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvServerHealth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvServerHealth.CollapseIcon = ((System.Drawing.Image)(resources.GetObject("dgvServerHealth.CollapseIcon")));
            this.dgvServerHealth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerHealth.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dtcHealthCategory,
            this.dtcHealthName,
            this.dtcHealthCurrent,
            this.dtcHealthReference,
            this.dtcHealthDescription,
            this.dtcHealthObject});
            this.dgvServerHealth.ExpandIcon = ((System.Drawing.Image)(resources.GetObject("dgvServerHealth.ExpandIcon")));
            this.dgvServerHealth.Location = new System.Drawing.Point(7, 171);
            this.dgvServerHealth.Name = "dgvServerHealth";
            this.dgvServerHealth.ReadOnly = true;
            this.dgvServerHealth.RowHeadersVisible = false;
            this.dgvServerHealth.Size = new System.Drawing.Size(586, 373);
            this.dgvServerHealth.TabIndex = 14;
            this.dgvServerHealth.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnServerHealthCellClick);
            this.dgvServerHealth.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnServerHealthCellContentClick);
            // 
            // dtcHealthCategory
            // 
            this.dtcHealthCategory.DataPropertyName = "Category";
            this.dtcHealthCategory.FillWeight = 80F;
            this.dtcHealthCategory.HeaderText = "Category";
            this.dtcHealthCategory.Name = "dtcHealthCategory";
            this.dtcHealthCategory.ReadOnly = true;
            this.dtcHealthCategory.Width = 80;
            // 
            // dtcHealthName
            // 
            this.dtcHealthName.FillWeight = 130F;
            this.dtcHealthName.HeaderText = "Item";
            this.dtcHealthName.Name = "dtcHealthName";
            this.dtcHealthName.ReadOnly = true;
            this.dtcHealthName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dtcHealthName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dtcHealthName.Width = 130;
            // 
            // dtcHealthCurrent
            // 
            this.dtcHealthCurrent.FillWeight = 130F;
            this.dtcHealthCurrent.HeaderText = "Current";
            this.dtcHealthCurrent.Name = "dtcHealthCurrent";
            this.dtcHealthCurrent.ReadOnly = true;
            this.dtcHealthCurrent.Width = 130;
            // 
            // dtcHealthReference
            // 
            this.dtcHealthReference.FillWeight = 130F;
            this.dtcHealthReference.HeaderText = "Reference";
            this.dtcHealthReference.Name = "dtcHealthReference";
            this.dtcHealthReference.ReadOnly = true;
            this.dtcHealthReference.Width = 130;
            // 
            // dtcHealthDescription
            // 
            this.dtcHealthDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dtcHealthDescription.HeaderText = "Description";
            this.dtcHealthDescription.Name = "dtcHealthDescription";
            this.dtcHealthDescription.ReadOnly = true;
            // 
            // dtcHealthObject
            // 
            this.dtcHealthObject.HeaderText = "";
            this.dtcHealthObject.Name = "dtcHealthObject";
            this.dtcHealthObject.ReadOnly = true;
            this.dtcHealthObject.Visible = false;
            // 
            // pgPerformance
            // 
            this.pgPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgPerformance.Location = new System.Drawing.Point(3, 3);
            this.pgPerformance.Name = "pgPerformance";
            this.pgPerformance.Size = new System.Drawing.Size(594, 546);
            this.pgPerformance.TabIndex = 0;
            // 
            // dgvAnalysis
            // 
            this.dgvAnalysis.AllowUserToAddRows = false;
            this.dgvAnalysis.AllowUserToDeleteRows = false;
            this.dgvAnalysis.AllowUserToResizeRows = false;
            this.dgvAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAnalysis.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAnalysis.CollapseIcon = ((System.Drawing.Image)(resources.GetObject("dgvAnalysis.CollapseIcon")));
            this.dgvAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnalysis.ExpandIcon = ((System.Drawing.Image)(resources.GetObject("dgvAnalysis.ExpandIcon")));
            this.dgvAnalysis.Location = new System.Drawing.Point(3, 30);
            this.dgvAnalysis.Name = "dgvAnalysis";
            this.dgvAnalysis.ReadOnly = true;
            this.dgvAnalysis.RowHeadersVisible = false;
            this.dgvAnalysis.Size = new System.Drawing.Size(586, 339);
            this.dgvAnalysis.TabIndex = 2;
            this.dgvAnalysis.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnAnalysisCellClick);
            this.dgvAnalysis.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.OnAnalysisCellFormatting);
            this.dgvAnalysis.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnAnalysisRowEnter);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 603);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.tsCommands);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQLMon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyDown);
            this.tsCommands.ResumeLayout(false);
            this.tsCommands.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).EndInit();
            this.cmsActivityScript.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpSummary.ResumeLayout(false);
            this.tpSummary.PerformLayout();
            this.tpObjects.ResumeLayout(false);
            this.scObjects.Panel1.ResumeLayout(false);
            this.scObjects.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scObjects)).EndInit();
            this.scObjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).EndInit();
            this.pnlObjectScript.ResumeLayout(false);
            this.pnlObjectScript.PerformLayout();
            this.pnlSearchCommands.ResumeLayout(false);
            this.cmsObjectScript.ResumeLayout(false);
            this.tpActivities.ResumeLayout(false);
            this.tpPerformance.ResumeLayout(false);
            this.tpAnalysis.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.cmsAnalysisScript.ResumeLayout(false);
            this.tpAlerts.ResumeLayout(false);
            this.tpAlerts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorItems)).EndInit();
            this.tpHistories.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistories)).EndInit();
            this.tpOptions.ResumeLayout(false);
            this.gbAnalysisOptions.ResumeLayout(false);
            this.gbAnalysisOptions.PerformLayout();
            this.gbAlertTemplate.ResumeLayout(false);
            this.gbAlertMethod.ResumeLayout(false);
            this.gbAlertMethod.PerformLayout();
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.cmsObjects.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).EndInit();
            this.cmsObjectList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerHealth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalysis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip tsCommands;
        private ToolStripButton tbRefresh;
        private DataGridView dgvProcesses;
        private TextEditorControl rtbProcessSQL;
        private SplitContainer splitContainer1;
        private ToolStripSeparator tssRuntime;
        private ToolStripButton tbProjectHomepage;
        private ToolStripComboBox tcbRefreshActivitiesIntervals;
        private TabControl tcMain;
        private TabPage tpSummary;
        private TabPage tpActivities;
        private LinkLabel lklConnections;
        private TextBox txtVersion;
        private Label lblConnectionCount;
        private Label lblVersion;
        private TabPage tpObjects;
        private LinkLabel lklObjects;
        private Label lblObjectCount;
        private SplitContainer scMain;
        private TreeView tvObjects;
        private SplitContainer scObjects;
        private DataGridView dgvObjects;
        private Panel pnlObjectScript;
        private ImageList il16;
        private ContextMenuStrip cmsObjects;
        private ToolStripMenuItem tmiTruncateObject;
        private ToolStripMenuItem tmiShrinkDatabase;
        private ToolStripMenuItem tmiSetDatabaseState;
        private ToolStripMenuItem tmiDetachDatabase;
        private ToolStripMenuItem tmiAttachDatabase;
        private ToolStripMenuItem tmiBackupDatabase;
        private ToolStripMenuItem tmiRestoreDatabase;
        private ToolStripMenuItem tmiSearchDatabase;
        private DataGridViewImageColumn State;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn Count;
        private DataGridViewTextBoxColumn CreateDate;
        private DataGridViewTextBoxColumn ModifyDate;
        private DataGridViewTextBoxColumn Path;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn TypeName;
        private ToolStripMenuItem tmiOpenTable;
        private ToolStripMenuItem tmiObjectDependencies;
        private TabPage tpAlerts;
        private TabPage tpAnalysis;
        private OutlookGrid dgvAnalysis;
        private Label lblAnalysisType;
        private ComboBox cboAnalysisTypes;
        private Label lblAlert;
        private ComboBox cboAlertTypes;
        private TabPage tpOptions;
        private Label lblFont;
        private Button cmdChooseFont;
        private ToolStripMenuItem tmiNewQuery;
        private SplitContainer splitContainer4;
        private TextEditorControl rtbAnalysisSQL;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tmiRegisterServer;
        private ToolStripMenuItem tmiRemoveServer;
        private ToolStripMenuItem tmiEditServer;
        private ComboBox cboMonitorRefreshIntervals;
        private Label lblMonitorRefreshInterval;
        private Button cmdSaveMonitorItem;
        private TextBox txtAlertTarget;
        private ComboBox cboAlertConditionTypes;
        private ComboBox cboAlertCondictionValues;
        private DataGridView dgvMonitorItems;
        private Button cmdDeleteMonitorItem;
        private ComboBox cboAlertConnections;
        private ErrorProvider epHint;
        private Button cmdEditMonitorItem;
        private TabPage tpHistories;
        private DataGridView dgvHistories;
        private ComboBox cboAlertMethods;
        private Label lblAlertMethod;
        private TextBox txtAlertMailPassword;
        private Label lblAlertMailPassword;
        private TextBox txtAlertMailUser;
        private Label lblAlertUserName;
        private ComboBox cboAlertMailServers;
        private Label lblAlertMailServer;
        private GroupBox gbAlertMethod;
        private GroupBox gbBasic;
        private Label lblAlertMailReceivers;
        private TextBox txtAlertMailReceiver;
        private Label lblAlertMetrict;
        private ComboBox cboAlertTitle;
        private Label lblAlertTitle;
        private GroupBox gbAlertTemplate;
        private RichTextBox rtbAlertTemplate;
        private Button cmdNewMonitorItem;
        private ToolStripMenuItem tmiTruncateTable;
        private ToolStripMenuItem tmiCheckDB;
        private ToolStripMenuItem tmiCleanTable;
        private ToolStripMenuItem tmiTableIndexDefrag;
        private Button cmdSetDefaultAlertTemplate;
        private SplitContainer splitContainer5;
        private TextEditorControl rtbHistoryDetail;
        private CheckBox chkAutoWordWrap;
        private CheckBox chkEnableAlert;
        private TextEditorControl rtbObjectScript;
        private ComboBox cboObjectScriptVersions;
        private Label lblObjectScriptVersion;
        private ToolStripMenuItem tmiSetVersionControl;
        private ContextMenuStrip cmsObjectScript;
        private ToolStripMenuItem tmiSaveScript;
        private Button cmdCompareObjectVersion;
        private ToolStripMenuItem tiCompareScript;
        private ContextMenuStrip cmsActivityScript;
        private ToolStripMenuItem tmiSaveToFile;
        private ContextMenuStrip cmsAnalysisScript;
        private ToolStripMenuItem tiAnalysisScriptSaveToFile;
        private Label lblConnectionTimeout;
        private ComboBox cboConnectionTimeouts;
        private Label lblConnectionTimeoutSeconds;
        private Panel pnlSearchCommands;
        private Button cmdSearchObjectPrevious;
        private Button cmdSearchObjectNext;
        private TextBox txtServerStartTime;
        private Label lblServerStartTime;
        private TextBox txtServerInstallationTime;
        private Label lblServerInstallationTime;
        private TextBox txtServerProcessID;
        private Label lblServerProcessID;
        private TextBox txtServerInstanceName;
        private Label lblServerInstanceName;
        private ContextMenuStrip cmsObjectList;
        private ToolStripMenuItem tmiFindObjectReferences;
        private GroupBox gbAnalysisOptions;
        private Label lblDatabaseDiskFreeSpaceRatio;
        private ComboBox cboDatabaseDiskFreeSpaceRatios;
        private Label lblDatabaseDataLogSpaceRatio;
        private ComboBox cboDatabaseDataLogSpaceRatios;
        private Label lblTableDataIndexSpaceRatio;
        private ComboBox cboTableDataIndexSpaceRatios;
        private TabPage tpPerformance;
        private ToolStripSeparator toolStripSeparator2;
        private DataGridViewTextBoxColumn dtcActivitiesSPID;
        private DataGridViewTextBoxColumn dtcActivitiesHostName;
        private DataGridViewTextBoxColumn dtcActivitiesHostProcess;
        private DataGridViewTextBoxColumn dtcActivitiesProgramName;
        private DataGridViewTextBoxColumn dtcActivitiesDB;
        private DataGridViewTextBoxColumn dtcActivitiesCPU;
        private DataGridViewTextBoxColumn dtcActivitiesPhysicalIO;
        private DataGridViewTextBoxColumn dtcActivitiesLoginTime;
        private DataGridViewTextBoxColumn dtcActivitiesLastRequestStart;
        private DataGridViewTextBoxColumn dtcActivitiesLastRequestEnd;
        private DataGridViewTextBoxColumn dtcActivitiesStatus;
        private DataGridViewTextBoxColumn dtcActivitiesCommand;
        private DataGridViewTextBoxColumn dtcActivitiesPercent;
        private DataGridViewTextBoxColumn dtcActivitiesLoginName;
        private DataGridViewTextBoxColumn dtcActivitiesEnabled;
        private Label lblPerformanceInterval;
        private ComboBox cboPerformanceIntervals;
        private Label lblPerformanceIntervalSeconds;
        private Label lblMonitorRefreshIntervalSeconds;
        private CheckBox chkLogHistory;
        private Performance pgPerformance;
        private ToolStripMenuItem tmiShowPerformance;
        private ToolStripSplitButton tbNewConnection;
        private ToolStripMenuItem tmNewQuery;
        private ToolStripDropDownButton tbRecentObjects;
        private ToolStripSeparator tsRecentObjects;
        private ToolStripMenuItem tmClearRecentObjects;
        private ToolStripButton tbRunManagementStudio;
        private OutlookGrid dgvServerHealth;
        private Label lblFreeMemoryRatio;
        private ComboBox cboFreeMemoryRatios;
        private DataGridViewTextBoxColumn dtcHealthCategory;
        private DataGridViewLinkColumn dtcHealthName;
        private DataGridViewTextBoxColumn dtcHealthCurrent;
        private DataGridViewTextBoxColumn dtcHealthReference;
        private DataGridViewTextBoxColumn dtcHealthDescription;
        private DataGridViewTextBoxColumn dtcHealthObject;
        private Label lblFreeCPURatio;
        private ComboBox cboFreeCPURatios;

    }
}