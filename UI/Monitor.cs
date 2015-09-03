using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Controls.OutlookGrid;
using Xnlab.SQLMon.Diff;
using Xnlab.SQLMon.Logic;
using Xnlab.SQLMon.Properties;
using Settings = Xnlab.SQLMon.Logic.Settings;
using SortOrder = System.Windows.Forms.SortOrder;
using Timer = System.Windows.Forms.Timer;

namespace Xnlab.SQLMon.UI
{
    public partial class Monitor : Form
    {
        private const string KeyTables = "|Tables";
        private const string KeySPs = "|SPs";
        private const string KeyViews = "|Views";
        private const string KeyFunctions = "|Functions";
        private const string KeyAssemblies = "|Assemblies";
        private const string KeyTriggers = "|Triggers";
        private const string KeyIndexes = "|Indexes";
        private const string KeyJobs = "|Jobs";
        private const string KeyTable = "|Table";
        private const string KeySp = "|SP";
        private const string KeyView = "|View";
        private const string KeyFunction = "|Function";
        private const string KeyTrigger = "|Trigger";
        private const string KeyAssembly = "|Assembly";
        private const string KeyDatabase = "|Database";
        private const string KeyServer = "|Server";
        private const string KeyLoading = "|Loading";
        private const string KeyName = "Name";
        private const string KeySchemaName = "SchemaName";
        private const string KeyState = "State";
        private const string KeySpaceUsed = "SpaceUsed";
        private const string KeyCount = "Count";
        private const string KeyCreateDate = "CreateDate";
        private const string KeyModifyDate = "ModifyDate";
        private const string KeyPath = "Path";
        private const string KeyValue = "Value";
        private const string KeyType = "Type";
        private const string KeyText = "Text";
        private const string CommandRefresh = "tbRefresh";
        private const string CommandActivityStatuses = "tcbActivityStatuses";
        private const string CommandJobs = "Jobs";
        private const string CommandProcesses = "Processes";
        private const string CommandDelete = "Delete";
        private const string CommandVisualize = "Visualize";
        private const int ResultSamplePrefix = 15;
        private const int ResultSampleCount = 100;
        private const string AnalysisColumnRule = "Rule";
        private const string AnalysisColumnObject = "Object";
        private const string AnalysisColumnReference = "Reference";
        private const string AnalysisColumnCurrent = "Current";
        private const string AnalysisColumnFactor = "Factor";
        private const string AnalysisColumnSuggestion = "Suggestion";
        private const string SizePercentage = "%";
        private const int ImageIndexOnline = 5;

        private const int HealthIndexCagtegory = 0;
        private const int HealthIndexName = 1;
        private const int HealthIndexCurrent = 2;
        private const int HealthIndexReference = 3;
        private const int HealthIndexDescription = 4;
        private const int HealthIndexObject = 5;

        private WorkModes _currentWorkMode = WorkModes.Summary;

        private Timer _tmrActivitiesRefresh = null;
        private readonly Timer _tmrStartup = null;
        private bool _isUpdating = false;
        private ObjectModes _currentObjectMode = ObjectModes.None;
        private ObjectModes _previousObjectMode = ObjectModes.None;
        private string _currentDatabase = string.Empty;
        private string _currentObjectScript = string.Empty;
        private string _currentObjectName = string.Empty;
        private string _currentObjectType = string.Empty;
        private string _previousDatabase = string.Empty;
        private string _previousObjectType = string.Empty;
        private int _userQueryCount = 0;
        private bool _isInSearch = false;
        private bool _isSearching = false;
        private int _currentSearchIndex = 0;
        private ServerInfo _currentServerInfo = new ServerInfo();
        private ServerInfo _previousServerInfo = new ServerInfo();
        private MonitorItem _currentMonitorItem = null;
        private int _healthPrevColIndex = -1;
        private ListSortDirection _healthPrevSortDirection = ListSortDirection.Ascending;
        private int _analysisPrevColIndex = -1;
        private ListSortDirection _analysisPrevSortDirection = ListSortDirection.Ascending;

        private static Monitor _instance = null;

        internal static Monitor Instance
        {
            get { return _instance; }
        }

        public Monitor()
        {
            InitializeComponent();

            Utils.SetTextBoxStyle(rtbAnalysisSQL);
            Utils.SetTextBoxStyle(rtbHistoryDetail);
            Utils.SetTextBoxStyle(rtbProcessSQL);
            Utils.SetTextBoxStyle(rtbObjectScript);

            Enum.GetValues(typeof(AnalysisTypes)).Cast<AnalysisTypes>().ForEach((s) => cboAnalysisTypes.Items.Add(s));
            cboAnalysisTypes.SelectedIndex = 0;

            Enum.GetValues(typeof(AlertTypes)).Cast<AlertTypes>().ForEach((s) => cboAlertTypes.Items.Add(s));
            cboAlertTypes.SelectedIndex = 0;

            Enum.GetValues(typeof(AlertMethods)).Cast<AlertMethods>().ForEach((s) => cboAlertMethods.Items.Add(s));
            cboAlertMethods.SelectedIndex = 0;

            tcbRefreshActivitiesIntervals.Items.AddRange(new object[] { "(manual)", 1, 2, 3, 4, 5, 8, 10, 15, 20, 30, 60, 120, 180, 240 });
            tcbRefreshActivitiesIntervals.SelectedIndex = 0;

            cboMonitorRefreshIntervals.Items.AddRange(new object[] { "(Disable)", 3, 5, 8, 10, 15, 20, 30, 60 });
            cboMonitorRefreshIntervals.SelectedIndex = 0;

            cboPerformanceIntervals.Items.AddRange(new object[] { "(Disable)", 5, 10, 15, 20, 30, 45, 60 });
            cboPerformanceIntervals.SelectedIndex = 0;

            cboConnectionTimeouts.Items.AddRange(Enumerable.Range(1, 300).Where(i => i % 5 == 0).Select(i => i.ToString()).ToArray());
            cboConnectionTimeouts.Text = Settings.Instance.ConnectionTimeout.ToString();

            SetPercentage(cboDatabaseDiskFreeSpaceRatios, Settings.Instance.DatabaseDiskFreeSpaceRatio);

            SetPercentage(cboDatabaseDataLogSpaceRatios, Settings.Instance.DatabaseDataLogSpaceRatio);

            SetPercentage(cboTableDataIndexSpaceRatios, Settings.Instance.TableDataIndexSpaceRatio);

            SetPercentage(cboFreeMemoryRatios, Settings.Instance.FreeMemoryRatio);

            SetPercentage(cboFreeCPURatios, Settings.Instance.FreeCpuRatio);

            chkAutoWordWrap.Checked = Settings.Instance.AutoWordWrap;
            chkLogHistory.Checked = Settings.Instance.LogHistory;

            cboAlertMailServers.Text = Settings.Instance.AlertMailServer;
            txtAlertMailUser.Text = Settings.Instance.AlertMailUser;
            txtAlertMailPassword.Text = Settings.Instance.AlertMailPassword;
            txtAlertMailReceiver.Text = Settings.Instance.AlertMailReceiver;
            rtbAlertTemplate.Text = Settings.Instance.AlertTemplate;
            dgvObjects.AutoGenerateColumns = false;

            if (!DesignMode)
                this.rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged += OnObjectScriptSelectionChanged;

            cboAlertTitle.Items.Add("SQL Server Alert");

            SetDefaultAlertTemplate();

            LoadServers();

            LoadRecentObjects();

            LoadMonitorItems();

            UpdateFont();

            SetSearchMode(false);

            this.Text += " " + Application.ProductVersion;

            //crazy .net 4.0 will cause SEHException, damn it!
            //tcbPassword.TextBox.PasswordChar = '*';

            _instance = this;

            var monitorRefreshInterval = Settings.Instance.MonitorRefreshInterval.ToString();
            cboMonitorRefreshIntervals.Text = monitorRefreshInterval;
            cboPerformanceIntervals.Text = Settings.Instance.PerformanceInterval.ToString();
            MonitorEngine.Instance.Message += OnMonitorEngineMessage;
            MonitorEngine.Instance.Alert += OnMonitorEngineAlert;
            MonitorEngine.Instance.RequestHealthServer += OnMonitorEngineRequestHealthServer;
            MonitorEngine.Instance.Health += OnMonitorEngineHealth;

            _tmrStartup = new Timer();
            _tmrStartup.Interval = 500;
            _tmrStartup.Tick += OnStartupTick;
            _tmrStartup.Enabled = true;
        }

        private void OnMonitorEngineHealth(object sender, HealthEventArgs e)
        {
            this.Invoke(() =>
                {
                    var healthPreviousGroups = new Dictionary<string, int>();
                    dgvServerHealth.Rows.Cast<OutlookGridRow>().ToList().ForEach(r =>
                    {
                        var category = r.Cells[HealthIndexCagtegory].Value as string;
                        if (!healthPreviousGroups.ContainsKey(category))
                            healthPreviousGroups.Add(category, 0);
                        healthPreviousGroups[category]++;
                    });
                    healthPreviousGroups = healthPreviousGroups.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);

                    var existingRows = new HashSet<OutlookGridRow>();
                    e.Result.ForEach(h =>
                        {
                            var row = dgvServerHealth.Rows.Cast<OutlookGridRow>().FirstOrDefault(r =>
                                {
                                    if (r.Cells[HealthIndexObject].Value != null)
                                    {
                                        var healthType = (HealthTypes)Enum.Parse(typeof(HealthTypes), r.Cells[HealthIndexObject].Value.ToString());
                                        return healthType == h.HealthType
                                            && (string)r.Cells[HealthIndexName].Value == h.ItemName
                                            && (healthType == HealthTypes.BlockedProcess ? (string)r.Cells[HealthIndexCurrent].Value == h.CurrentValue : true);
                                    }
                                    else
                                        return false;
                                });
                            if (row == null)
                            {
                                row = new OutlookGridRow();
                                row.CreateCells(dgvServerHealth, "", "", "", "", null);
                                dgvServerHealth.Rows.Add(row);
                            }
                            row.DefaultCellStyle.BackColor = h.IsAlert ? Color.LightSalmon : SystemColors.Window;
                            row.Cells[HealthIndexCagtegory].Value = h.Category;
                            row.Cells[HealthIndexName].Value = h.ItemName;
                            var cell = row.Cells[HealthIndexName];
                            cell.Style.ForeColor = Color.Blue;

                            row.Cells[HealthIndexCurrent].Value = h.CurrentValue;
                            row.Cells[HealthIndexReference].Value = h.ReferenceValue;
                            row.Cells[HealthIndexDescription].Value = h.Description;
                            row.Cells[HealthIndexObject].Value = h.HealthType;
                            existingRows.Add(row);
                        });

                    dgvServerHealth.Rows.Cast<OutlookGridRow>().ToList().ForEach(r =>
                        {
                            if (!existingRows.Contains(r) && r.Cells[HealthIndexObject].Value != null
                                && !string.IsNullOrEmpty(r.Cells[HealthIndexName].Value as string))
                                dgvServerHealth.Rows.Remove(r);
                        });

                    var healthCurrentGroups = new Dictionary<string, int>();
                    dgvServerHealth.Rows.Cast<OutlookGridRow>().ToList().ForEach(r =>
                    {
                        var category = r.Cells[HealthIndexCagtegory].Value as string;
                        if (!healthCurrentGroups.ContainsKey(category))
                            healthCurrentGroups.Add(category, 0);
                        healthCurrentGroups[category]++;
                    });
                    healthCurrentGroups = healthCurrentGroups.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);

                    var hasChanged = false;
                    if (healthPreviousGroups != null && healthPreviousGroups.Count == healthCurrentGroups.Count)
                    {
                        for (var i = 0; i < healthCurrentGroups.Count; i++)
                        {
                            if (healthPreviousGroups.Keys.ElementAt(i) != healthCurrentGroups.Keys.ElementAt(i)
                                || healthPreviousGroups.Values.ElementAt(i) != healthCurrentGroups.Values.ElementAt(i))
                            {
                                hasChanged = true;
                                break;
                            }
                        }
                    }
                    else
                        hasChanged = true;

                    dgvServerHealth.Rows.Cast<OutlookGridRow>().ToList().ForEach(r =>
                    {
                        var category = r.Cells[HealthIndexCagtegory].Value as string;
                        if (string.IsNullOrEmpty(r.Cells[HealthIndexName].Value as string))
                        {
                            if (!healthCurrentGroups.ContainsKey(category))
                                dgvServerHealth.Rows.Remove(r);
                        }
                    });

                    if (dgvServerHealth.GroupTemplate.Column != null && hasChanged)
                        dgvServerHealth.ClearGroups();

                    if (dgvServerHealth.GroupTemplate.Column == null)
                    {
                        dgvServerHealth.GroupTemplate.Column = dtcHealthCategory;
                        //dgvServerHealth.GroupTemplate.Collapsed = healthprevGroup.Collapsed;
                        dgvServerHealth.Sort(dgvServerHealth.GroupTemplate.Column, ListSortDirection.Descending);
                    }
                });
        }

        private void OnMonitorEngineRequestHealthServer(object sender, ServerInfoEventArgs e)
        {
            e.Server = CurrentServerInfo;
        }

        private void SetPercentage(ComboBox cboItems, int ratio)
        {
            cboItems.Items.AddRange(Enumerable.Range(1, 300).Where(i => i % 5 == 0).Select(i => i.ToString()).ToArray());
            cboItems.Text = ratio.ToString();
        }

        private void SetSearchMode(bool isSearch)
        {
            _isInSearch = isSearch;
            pnlSearchCommands.Visible = _isInSearch;
            rtbObjectScript.Height = pnlObjectScript.Height - (pnlSearchCommands.Visible ? pnlSearchCommands.Height : 0) - rtbObjectScript.Top - 2;
        }

        private void OnStartupTick(object sender, EventArgs e)
        {
            _tmrStartup.Enabled = false;
            if (Settings.Instance.Servers.Count == 0)
                if (ShowQuestion("Would you like to add a new connection now?"))
                    EditConnection(null);
        }

        private void LoadMonitorItems()
        {
            dgvMonitorItems.DataSource = null;
            dgvMonitorItems.DataSource = Settings.Instance.MonitorItems;
        }

        private void LoadNotifiedAlerts()
        {
            rtbHistoryDetail.Text = string.Empty;
            rtbHistoryDetail.Refresh();
            dgvHistories.DataSource = null;
            dgvHistories.DataSource = Settings.Instance.NotifiedAlerts;
        }

        private void LoadServers()
        {
            tvObjects.Nodes.Clear();
            cboAlertConnections.Items.Clear();
            cboAlertConnections.ValueMember = "Server";
            cboAlertConnections.DisplayMember = "Server";
            Settings.Instance.Servers.ForEach(s =>
                {
                    LoadServer(s);
                    cboAlertConnections.Items.Add(s);
                });
        }

        private void LoadServer(ServerInfo info)
        {
            var state = new ServerState { AuthType = info.AuthType, Server = info.Server, Database = info.Database, User = info.User, Password = info.Password, IsReady = false, Key = KeyServer };
            var node = new TreeNode { Text = info.Server, Name = info.Server, ImageIndex = 0, SelectedImageIndex = 0, Tag = state };
            tvObjects.Nodes.Add(node);
            node.Nodes.Add(new TreeNode { Text = "Loading...", Tag = new ServerState { Key = KeyLoading } });
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            RefreshData(true);
        }

        private void RefreshData(bool reload)
        {
            SaveSettings();
            LoadData(reload);
        }

        private void ResetPerformance()
        {
            var server = GetCurrentServer();

            pgPerformance.Init(_currentObjectMode, server);
            pgPerformance.ResetPerformance();
        }

        private ServerInfo GetCurrentServer()
        {
            ServerInfo server;
            if (CheckCurrentServer())
            {
                var isServer = _currentObjectMode == ObjectModes.Server;
                if (isServer)
                    server = DefaultServerInfo;
                else
                    server = GetServerInfo(_currentDatabase);
            }
            else
                server = null;
            return server;
        }

        private void GetPerformanceData()
        {
            pgPerformance.GetPerformanceData();
        }

        private void LoadData(bool reload)
        {
            _isUpdating = true;
            var sql = string.Empty;

            using (NewWait())
            {
                switch (_currentWorkMode)
                {
                    case WorkModes.Alerts:
                        LoadMonitorItems();
                        break;
                    case WorkModes.Histories:
                        LoadNotifiedAlerts();
                        break;
                    case WorkModes.Performance:
                        GetPerformanceData();
                        break;
                    case WorkModes.Analysis:
                        Analyze();
                        break;
                    case WorkModes.TableData:
                        var tableData = tcMain.SelectedTab.Controls[0] as UserTableData;
                        tableData.Execute();
                        break;
                    case WorkModes.Query:
                        var queryData = tcMain.SelectedTab.Controls[0] as UserQuery;
                        queryData.Execute();
                        break;
                    case WorkModes.Activities:
                        if (CheckCurrentServerMessage())
                        {
                            switch (ActivitiesObjectType)
                            {
                                case ActivityTypes.Process:
                                    var filter = string.Empty;
                                    var tcbActivityStatuses = FindCommand<ToolStripComboBox>(CommandActivityStatuses);
                                    var status = (ActivityStatuses)tcbActivityStatuses.SelectedItem;
                                    if (status != ActivityStatuses.All)
                                        filter = " s.status = '" + status.ToString() + "'";
                                    if (!string.IsNullOrEmpty(filter))
                                        filter = " AND " + filter;
                                    sql = QueryEngine.SqlProcesses + filter;
                                    break;
                                case ActivityTypes.Job:
                                    sql = QueryEngine.SqlJobs;
                                    break;
                                default:
                                    break;
                            }
                            var data = Query(sql);
                            if (ActivitiesObjectType == ActivityTypes.Job)
                            {
                                data.AsEnumerable().ForEach(d =>
                                    {
                                        var stepId = d["dbid"];
                                        if (stepId != DBNull.Value)
                                        {
                                            var steps = SqlHelper.Query(string.Format("SELECT step_id, step_name FROM msdb.dbo.sysjobsteps WHERE job_id = '{0}'", d["spid"]), DefaultServerInfo);
                                            if (steps.Rows.Count > 0)
                                            {
                                                var rows = steps.AsEnumerable().Where(r => r["step_id"].ToString() == stepId.ToString()).ToList();
                                                if (rows.Count > 0)
                                                {
                                                    d["dbid"] = rows[0]["step_name"];
                                                }
                                                d["percent_complete"] = Convert.ToDouble(stepId) / steps.Rows.Count * 100;
                                            }
                                        }
                                    });
                            }
                            var sortedColumnName = dgvProcesses.SortedColumn != null ? dgvProcesses.SortedColumn.Name : string.Empty;
                            var sortedOrder = dgvProcesses.SortOrder;
                            var selection = new List<Point>();
                            dgvProcesses.SelectedCells.Cast<DataGridViewCell>().ForEach(c => selection.Add(new Point(c.ColumnIndex, c.RowIndex)));
                            dgvProcesses.AutoGenerateColumns = false;
                            dgvProcesses.DataSource = data;
                            if (!string.IsNullOrEmpty(sortedColumnName) && sortedOrder != SortOrder.None)
                                dgvProcesses.Sort(dgvProcesses.Columns[sortedColumnName], sortedOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
                            if (selection.Count > 0)
                            {
                                dgvProcesses.ClearSelection();
                                selection.ForEach(s =>
                                    {
                                        if (data.Rows.Count > s.Y && data.Columns.Count > s.X)
                                            dgvProcesses.Rows[s.Y].Cells[s.X].Selected = true;
                                    });
                                var first = selection.First();
                                GetProcessCommand(first.Y, first.X);
                            }
                            lblConnectionCount.Text = data != null ? data.Rows.Count.ToString() : "0";
                        }

                        //auto refresh
                        if (!reload)
                        {
                            Application.OpenForms.Cast<Form>().Where(f => f is ProcessVisualizer).ForEach(f =>
                                {
                                    ((ProcessVisualizer)f).LoadProcesses();
                                });

                        }
                        break;
                    case WorkModes.Summary:
                    case WorkModes.Objects:
                        if (reload)
                        {
                            LoadServers();
                            this.Invoke(() => { dgvServerHealth.Rows.Clear(); });
                            MonitorEngine.Instance.CheckServerHealth();
                        }
                        break;
                    default:
                        break;
                }

                _isUpdating = false;

                switch (_currentWorkMode)
                {
                    case WorkModes.Activities:
                        if (dgvProcesses.Rows.Count > 0)
                            GetProcessCommand(0, 0);
                        break;
                    default:
                        break;
                }
            }
        }

        private bool CheckCurrentServerMessage()
        {
            if (CheckCurrentServer())
                return true;
            else
            {
                ShowMessage("Please select a sql server.");
                return false;
            }
        }

        private bool CheckCurrentServer()
        {
            return CurrentServerInfo != null && !string.IsNullOrEmpty(CurrentServerInfo.Server);
        }

        private bool LoadServer(TreeNode Node)
        {
            if (CheckCurrentServer())
            {
                _currentDatabase = string.Empty;
                try
                {
                    var result = SqlHelper.ExecuteScalar("SELECT @@version", DefaultServerInfo);
                    var version = result != null ? result.ToString() : "(N/A)";
                    var lines = version.Split('\t').ToList();
                    if (lines.Count > 1)
                    {
                        var line = lines[1];
                        DateTime date;
                        if (DateTime.TryParse(line, out date))
                        {
                            lines.RemoveAt(1);
                            txtServerInstallationTime.Text = date.ToString();
                        }
                    }
                    txtVersion.Text = string.Join("\r\n", lines.ToArray());

                    try
                    {
                        DateTime serverStartTime;
                        QueryEngine.GetOsInfo(DefaultServerInfo, out serverStartTime);
                        txtServerStartTime.Text = serverStartTime.ToString();
                    }
                    catch (Exception)
                    {
                    }

                    result = SqlHelper.ExecuteScalar("SELECT @@SERVICENAME", DefaultServerInfo);
                    var serviceName = result != null ? result.ToString() : "(N/A)";
                    txtServerInstanceName.Text = serviceName;

                    result = SqlHelper.ExecuteScalar("SELECT ServerProperty('ProcessID')", DefaultServerInfo);
                    var processId = result != null ? result.ToString() : "(N/A)";
                    txtServerProcessID.Text = processId;

                    using (var connection = NewConnection)
                    {
                        connection.Open();
                        var data = connection.GetSchema("Databases");
                        Node.Nodes.Clear();
                        var databases = GetDatabasesInfo();
                        data.AsEnumerable().OrderBy(r => r.Field<string>("database_name")).ForEach((d) =>
                        {
                            var name = d["database_name"].ToString();
                            var info = GetDatabaseInfo(name);
                            if (info != null && info.Rows.Count > 0)
                            {
                                var row = info.Rows[0];
                                var state = databases.AsEnumerable().First(r => r["name"].ToString() == name);
                                var isReady = state != null && Convert.ToInt32(state["state"]) == 0;
                                var image = isReady ? ImageIndexOnline : 0;
                                var tag = new ServerState { Key = KeyDatabase, IsReady = false, State = isReady };
                                var node = new TreeNode { Name = name, Text = name, ImageIndex = image, SelectedImageIndex = image, Tag = tag };
                                Node.Nodes.Add(node);
                            }
                        });
                        Node.Nodes.Cast<TreeNode>().ForEach((n) =>
                        {
                            n.Nodes.AddRange(new TreeNode[] { new TreeNode { Name = KeyTables, Text = "Tables", Tag = new ServerState { Key = KeyTables, IsReady = false } }
                                , new TreeNode { Name = KeyViews, Text = "Views", Tag = new ServerState { Key = KeyViews, IsReady = false } }
                                , new TreeNode { Name = KeyFunctions, Text = "Functions", Tag = new ServerState { Key = KeyFunctions, IsReady = false } }
                                , new TreeNode { Name = KeySPs, Text = "Stored Procedures", Tag = new ServerState { Key = KeySPs, IsReady = false } }
                                , new TreeNode { Name = KeyAssemblies, Text = "Assemblies", Tag = new ServerState { Key = KeyAssemblies, IsReady = false } } 
                                , new TreeNode { Name = KeyTriggers, Text = "Triggers", Tag = new ServerState { Key = KeyTriggers, IsReady = false } }   });
                            n.Nodes.Cast<TreeNode>().ForEach((m) => m.SelectedImageIndex = 1);
                        });
                        connection.Close();
                    }
                    //LoadDatabase(Node);
                    var counts = Node.Nodes.Count;
                    lblObjectCount.Text = counts.ToString();
                    if (counts == 1 && (Node.Nodes[0].Tag as ServerState).Key == KeyLoading)
                    {
                        Node.Collapse();
                        return false;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Node.Collapse();
                    ShowError(ex);
                    return false;
                }
            }
            else
            {
                Node.Collapse();
                return false;
            }
        }

        private void ShowError(Exception ex)
        {
            ShowMessage(ex.Message, MessageBoxIcon.Error);
        }

        private DataTable GetDatabasesInfo()
        {
            return QueryEngine.GetDatabasesInfo(DefaultServerInfo);
        }

        private DataTable GetDatabaseInfo(string database)
        {
            return QueryEngine.GetDatabaseInfo(DefaultServerInfo, database);
        }

        private void LoadDatabase(TreeNode parent)
        {
            SetSearchMode(false);
            var objects = NewObjects;
            parent.Nodes.Cast<TreeNode>().ForEach((n) =>
            {
                var row = objects.NewRow();
                row[KeyName] = n.Text;
                var tableInfo = GetDatabaseInfo(n.Text);
                if (tableInfo != null && tableInfo.Rows.Count > 0)
                {
                    var item = tableInfo.Rows[0];
                    var size = (Convert.ToDecimal(item["size"]) / Utils.Size1K).ToString() + Utils.SizeMb;
                    if (tableInfo.Rows.Count > 1)
                        size += " / " + (Convert.ToDecimal(tableInfo.Rows[1]["size"]) / Utils.Size1K) + Utils.SizeMb;
                    row[KeySpaceUsed] = size;
                    if (n.Nodes.ContainsKey(KeyTables))
                        row[KeyCount] = n.Nodes[KeyTables].Nodes.Count;
                    row[KeyPath] = item["Physical_Name"];
                    row[KeyValue] = item["state"];
                }
                else
                    row[KeyValue] = 1;
                objects.Rows.Add(row);

                //if (tableInfo != null)
                //{
                //    var count = 0;
                //    tableInfo.AsEnumerable().ForEach((r) => 
                //    {
                //        var row = objects.NewRow();                        
                //        row[KeyName] = r["Logical_Name"];
                //        row[KeyPath] = r["Physical_Name"];
                //        if (n.Nodes.ContainsKey(KeyTables))
                //            row[KeyCount] = n.Nodes[KeyTables].Nodes.Count;
                //        row[KeySpaceUsed] = r["Size"];
                //        objects.Rows.Add(row);
                //        count++;
                //    });
                //}
            });
            LoadObjects(objects);
            _currentObjectMode = ObjectModes.Databases;

            if (!string.IsNullOrEmpty(CurrentServerInfo.Database))
                GetVersionControlState();
        }

        private void GetVersionControlState()
        {
            this.Invoke(() =>
                {
                    bool exists;
                    var node = tvObjects.SelectedNode;
                    var isOnline = true;
                    if (node != null)
                    {
                        var tag = node.Tag as ServerState;
                        isOnline = node.Tag == null || tag.State;
                    }
                    if (isOnline)
                    {
                        var state = CheckVersionControl(out exists) && exists;
                        tmiSetVersionControl.Text = state ? "Disable Version Control" : "Enable Version Control";
                        tmiSetVersionControl.Tag = state;
                    }
                });
        }

        private ServerInfo DefaultServerInfo
        {
            get { return GetServerInfo(string.Empty); }
        }

        internal ServerInfo CurrentServerInfo
        {
            get { return _currentServerInfo; }
        }

        private ServerInfo GetServerInfo(string catalog)
        {
            return QueryEngine.GetServerInfo(CurrentServerInfo, catalog);
        }

        private SqlConnection NewConnection
        {
            get { return SqlHelper.CreateNewConnection(DefaultServerInfo); }
        }

        internal DataTable Query(string sql)
        {
            return Query(sql, DefaultServerInfo);
        }

        internal DataTable Query(string sql, ServerInfo info)
        {
            var data = QuerySet(sql, info);
            if (data != null && data.Tables.Count > 0)
                return data.Tables[0];
            else
                return null;
        }

        internal ToolStripItem[] Commands
        {
            get
            {
                return tsCommands.Items.Cast<ToolStripItem>().Where(t => t != tbProjectHomepage).ToArray();
            }
        }

        private DataSet QuerySet(string sql, ServerInfo info)
        {
            //using (NewWait())
            {
                try
                {
                    return SqlHelper.QuerySet(sql, info);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    return null;
                }
            }
        }

        internal void ShowMessage(string message)
        {
            ShowMessage(message, MessageBoxIcon.Information);
        }

        internal void ShowMessage(string message, MessageBoxIcon icon)
        {
            ShowMessage(message, Settings.Title, icon);
        }

        internal void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            this.Invoke(() => MessageBox.Show(this, message, title, MessageBoxButtons.OK, icon));
        }

        internal bool ShowQuestion(string message)
        {
            return MessageBox.Show(this, message, Settings.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void OnProcessesRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            GetProcessCommand(e.RowIndex, e.ColumnIndex);
        }

        internal void SetExecute(bool cancel)
        {
            this.Invoke(() =>
                {
                    tbRefresh.Image = cancel ? Resources.Cross2 : Resources.Refresh2;
                });
        }

        private void GetProcessCommand(int rowIndex, int columnIndex)
        {
            var text = string.Empty;
            if (!_isUpdating && columnIndex >= 0 && rowIndex >= 0)
            {
                var spid = dgvProcesses.Rows[rowIndex].Cells[0].Value;
                if (spid != null)
                {
                    var id = spid.ToString();
                    switch (ActivitiesObjectType)
                    {
                        case ActivityTypes.Process:
                            text = QueryEngine.GetSessionSql(id, DefaultServerInfo);
                            break;
                        case ActivityTypes.Job:
                            var job = QuerySet("exec msdb.dbo.sp_help_job '" + id + "'", DefaultServerInfo);
                            if (job != null && job.Tables.Count >= 4)
                            {
                                var step = job.Tables[0].Rows[0]["current_execution_step"].ToString();
                                var index = step.IndexOf("(");
                                if (index != -1)
                                    step = step.Substring(0, index);
                                index = Convert.ToInt32(step);
                                if (index < job.Tables[1].Rows.Count)
                                    text = "--Current step: " + index + "\r\n--" + job.Tables[1].Rows[index]["subsystem"].ToString() + "\r\n\r\n" + job.Tables[1].Rows[index]["command"].ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            rtbProcessSQL.Text = text;
            rtbProcessSQL.Refresh();
        }

        private void OnProcessesCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CopyText(e.RowIndex, e.ColumnIndex);
        }

        private void CopyText(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var content = dgvProcesses.Rows[rowIndex].Cells[columnIndex].Value;
                if (content != null && !string.IsNullOrEmpty(content.ToString()))
                    Clipboard.SetText(content.ToString());
            }
        }

        private void OnVisualizeProcessClick(object sender, EventArgs e)
        {
            ShowProcessVisualizer();
        }

        private void ShowProcessVisualizer()
        {
            if (DefaultServerInfo != null && !string.IsNullOrEmpty(DefaultServerInfo.Server))
            {
                var form = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f is ProcessVisualizer
                    && ((ProcessVisualizer)f).Server.Server == DefaultServerInfo.Server);
                if (form != null)
                    form.BringToFront();
                else
                {
                    var visualizer = new ProcessVisualizer(DefaultServerInfo);
                    visualizer.Show();
                }
            }
        }

        private void OnKillProcessClick(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedCells.Count > 0)
            {
                var row = dgvProcesses.SelectedCells[0].RowIndex;
                if (row >= 0)
                {
                    var spid = dgvProcesses.Rows[row].Cells[ActivitiesObjectType == ActivityTypes.Process ? 0 : 9].Value;
                    if (spid != null)
                    {
                        var id = spid.ToString();
                        using (NewWait())
                        {
                            try
                            {
                                var sql = ActivitiesObjectType == ActivityTypes.Process ? "kill " + id : "EXEC msdb.dbo.sp_stop_job N'" + id + "'";
                                SqlHelper.ExecuteNonQuery(sql, DefaultServerInfo);
                                LoadData(false);
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }
                        }
                    }
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            UnloadForms();
        }

        private void SaveSettings()
        {
            var tcbActivityStatuses = FindCommand<ToolStripComboBox>(CommandActivityStatuses);
            if (tcbActivityStatuses != null)
                Settings.Instance.ActivityState = (ActivityStatuses)tcbActivityStatuses.SelectedItem;
            Settings.Instance.ActivityType = ActivitiesObjectType;
            if (_currentServerInfo != null)
                Settings.Instance.LastServer = _currentServerInfo.Server;
            Settings.Instance.AutoWordWrap = chkAutoWordWrap.Checked;
            Settings.Instance.LogHistory = chkLogHistory.Checked;
            Settings.Instance.AlertMailServer = cboAlertMailServers.Text;
            Settings.Instance.AlertMailUser = txtAlertMailUser.Text;
            Settings.Instance.AlertMailPassword = txtAlertMailPassword.Text;
            Settings.Instance.AlertMailReceiver = txtAlertMailReceiver.Text;
            Settings.Instance.AlertTemplate = rtbAlertTemplate.Text;
            Settings.Instance.Save();
        }

        private void UnloadForms()
        {
            for (var i = 0; i < tcMain.TabPages.Count; i++)
            {
                WorkModes workMode;
                if (i < (int)WorkModes.Query)
                    workMode = (WorkModes)i;
                else
                    workMode = (WorkModes)tcMain.TabPages[i].Tag;
                switch (workMode)
                {
                    case WorkModes.Query:
                        var queryData = tcMain.TabPages[i].Controls[0] as UserQuery;
                        queryData.Cancel();
                        break;
                    case WorkModes.TableData:
                        var tableData = tcMain.TabPages[i].Controls[0] as UserTableData;
                        tableData.Cancel();
                        break;
                    default:
                        break;
                }
            }
        }

        internal ActivityTypes ActivitiesObjectType
        {
            get
            {
                var command = FindCommand<ToolStripButton>(CommandProcesses);
                if (command != null)
                    return command.Checked ? ActivityTypes.Process : ActivityTypes.Job;
                else
                    return ActivityTypes.Process;
            }
        }

        private void SetActivityType(ActivityTypes activityType)
        {
            switch (activityType)
            {
                case ActivityTypes.Process:
                    FindCommand<ToolStripButton>(CommandJobs).Checked = false;
                    dtcActivitiesDB.HeaderText = "DB";
                    break;
                case ActivityTypes.Job:
                    FindCommand<ToolStripButton>(CommandProcesses).Checked = false;
                    dtcActivitiesDB.HeaderText = "Step";
                    break;
                default:
                    break;
            }
            var tcbActivityStatuses = FindCommand<ToolStripComboBox>(CommandActivityStatuses);
            tcbActivityStatuses.Visible = activityType == ActivityTypes.Process;
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RefreshData();
                    break;
                case Keys.Escape:
                    tcbRefreshActivitiesIntervals.SelectedIndex = 0;
                    break;
                case Keys.F4:
                    if (e.Control)
                        CloseCurrentTab();
                    break;
                default:
                    break;
            }
        }

        private void OnProjectHomepageClick(object sender, EventArgs e)
        {
            Process.Start("https://github.com/unruledboy/SQLMonitor");
        }

        private void OnServerClick(object sender, EventArgs e)
        {
            using (NewWait())
            {
                var data = SqlDataSourceEnumerator.Instance.GetDataSources();
                var servers = data.AsEnumerable().Select((r) => !r.IsNull("InstanceName") && !string.IsNullOrEmpty(r["InstanceName"].ToString()) ? r["InstanceName"].ToString() : r["ServerName"].ToString());
                servers.ForEach((s) => { if (Settings.Instance.Servers.Where((v) => v.Server.ToLower() == s.ToLower()).Count() == 0) Settings.Instance.Servers.Add(new ServerInfo { AuthType = AuthTypes.SqlServer, Server = s, User = "sa", Password = string.Empty }); });
                //Settings.Instance.LastServer = tcbServers.Text;
                LoadServers();
            }
        }

        private void OnMonitorRefreshIntervalsTextChanged(object sender, EventArgs e)
        {
            SetMonitorRefreshIntervals();
        }

        private void SetMonitorRefreshIntervals()
        {
            if (_instance != null)
                MonitorEngine.Instance.SetMonitorInterval(cboMonitorRefreshIntervals.Text);
        }

        private void OnPerformanceIntervalsTextChanged(object sender, EventArgs e)
        {
            if (_instance != null)
            {
                for (var i = 0; i < tcMain.TabPages.Count; i++)
                {
                    WorkModes workMode;
                    if (i < (int)WorkModes.Query)
                        workMode = (WorkModes)i;
                    else
                        workMode = (WorkModes)tcMain.TabPages[i].Tag;
                    switch (workMode)
                    {
                        case WorkModes.Performance:
                        case WorkModes.UserPerformance:
                            var performance = tcMain.TabPages[i].Controls[0] as Performance;
                            performance.SetInterval(cboPerformanceIntervals.Text);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void OnMonitorEngineMessage(object sender, MessageEventArgs e)
        {
            this.Invoke(() =>
                {
                    if (e.Cancel)
                    {
                        DisableMonitor();
                    }
                    ShowMessage(e.Message, MessageBoxIcon.Information);
                });
        }

        internal void DisableMonitor()
        {
            cboMonitorRefreshIntervals.SelectedIndex = 0;
            SetMonitorRefreshIntervals();
        }

        private void OnMonitorEngineAlert(object sender, AlertEventArgs e)
        {
            var title = e.Item.Title ?? "SQL Server Alert (" + e.Item.Server + ")";
            switch (e.Item.AlertMethod)
            {
                case AlertMethods.Log:
                    break;
                case AlertMethods.MsgBox:
                    this.Invoke(() => ShowMessage(e.Message, title, MessageBoxIcon.Exclamation));
                    break;
                case AlertMethods.Mail:
                    var smtpClient = new SmtpClient();
                    var basicCredential =
                        new NetworkCredential(Settings.Instance.AlertMailUser, Settings.Instance.AlertMailUser);
                    var message = new MailMessage();
                    var fromAddress = new MailAddress(Settings.Instance.AlertMailUser + "@" + Settings.Instance.AlertMailServer);

                    smtpClient.Host = Settings.Instance.AlertMailUser;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    message.From = fromAddress;
                    message.Subject = title;
                    //message.IsBodyHtml = true;
                    message.Body = e.Message;
                    message.To.Add(Settings.Instance.AlertMailReceiver);
                    smtpClient.Send(message);
                    break;
                default:
                    break;
            }
        }

        private void OnRefreshActivitiesIntervalsTextChanged(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(tcbRefreshActivitiesIntervals.Text, out interval))
            {
                if (_tmrActivitiesRefresh == null)
                {
                    _tmrActivitiesRefresh = new Timer();
                    _tmrActivitiesRefresh.Tick += OnRefreshActivitiesTick;
                }
                _tmrActivitiesRefresh.Interval = interval * 1000;
                _tmrActivitiesRefresh.Enabled = true;
            }
            else if (_tmrActivitiesRefresh != null)
                _tmrActivitiesRefresh.Enabled = false;
        }

        private void OnRefreshActivitiesTick(object sender, EventArgs e)
        {
            RefreshData(false);
        }

        private void OnConnectionsLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tcMain.SelectedTab = tpActivities;
        }

        private void OnObjectsLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tcMain.SelectedTab = tpObjects;
        }

        private void OnObjectsBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            new Thread(ShowObjects).Start(e);
        }

        private void ShowObjects(object e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                var arg = e as TreeViewCancelEventArgs;
                var state = arg.Node.Tag as ServerState;
                if (arg.Node.Tag == null || (state != null && !state.IsReady))
                    arg.Cancel = !ShowObjects(arg.Node);
            }));
        }

        private bool ShowObjects(TreeNode node)
        {
            var ready = false;
            using (NewWait())
            {
                DataTable data;
                var key = node.Name;
                var root = GetRootNode(node);
                var serverState = node.Tag as ServerState;
                _previousServerInfo = _currentServerInfo;
                _currentServerInfo = root.Tag as ServerState;
                switch (serverState.Key)
                {
                    case KeyServer:
                        ready = LoadServer(node);
                        break;
                    case KeyDatabase:
                        _currentDatabase = node.Text;
                        _currentServerInfo.Database = _currentDatabase;
                        var databases = GetDatabasesInfo();
                        var state = databases.AsEnumerable().First(r => r["name"].ToString() == _currentDatabase);
                        if (state != null && Convert.ToInt32(state["state"]) == 0)
                        {
                            var objects = new string[] { KeyTables, KeyViews, KeyFunctions, KeySPs, KeyTriggers };
                            objects.ForEach(o =>
                                {
                                    data = GetObjects(o);
                                    data.AsEnumerable().ForEach((d) =>
                                        {
                                            var image = 4;
                                            var type = string.Empty;
                                            switch (o)
                                            {
                                                case KeyTables:
                                                    type = KeyTable;
                                                    image = 8;
                                                    break;
                                                case KeyViews:
                                                    type = KeyView;
                                                    image = 4;
                                                    break;
                                                case KeySPs:
                                                    type = KeySp;
                                                    image = 3;
                                                    break;
                                                case KeyFunctions:
                                                    type = KeyFunction;
                                                    image = 3;
                                                    break;
                                                case KeyAssemblies:
                                                    type = KeyAssembly;
                                                    image = 3;
                                                    break;
                                                case KeyTriggers:
                                                    type = KeyTrigger;
                                                    image = 3;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            var tag = new ServerState { Key = type, IsReady = false };
                                            var child = new TreeNode { Text = QueryEngine.GetObjectName(d[KeySchemaName].ToString(), d[KeyName].ToString()), ImageIndex = image, SelectedImageIndex = image, Tag = tag };
                                            node.Nodes[o].Nodes.Add(child);
                                        });
                                });
                            ready = true;
                        }
                        else if (ShowQuestion(string.Format("The database [{0}] is currently offline. Do you bring it back to online?", _currentDatabase)))
                        {
                            SetOnlineOffline(_currentDatabase, true);
                            ShowObjects(node);
                        }
                        break;
                    default:
                        ready = true;
                        break;
                }
                serverState.IsReady = ready;
            }
            return ready;
        }

        private void SetOnlineOffline(string database, bool isOnline)
        {
            using (NewWait())
            {
                SqlHelper.ExecuteNonQuery(string.Format("ALTER DATABASE [{0}] SET {1} WITH ROLLBACK IMMEDIATE", database, isOnline ? "ONLINE" : "OFFLINE"), isOnline ? DefaultServerInfo : GetServerInfo(database));
                if (tvObjects.SelectedNode != null)
                    tvObjects.SelectedNode.ImageIndex = ImageIndexOnline;
                RefreshData();
            }
        }

        private TreeNode GetRootNode(TreeNode node)
        {
            var root = node;
            while (root != null && root.Parent != null)
            {
                root = root.Parent;
            }
            return root;
        }

        private DataTable NewObjects
        {
            get
            {
                var objects = new DataTable();
                objects.Columns.Add(new DataColumn { ColumnName = KeyState });
                objects.Columns.Add(new DataColumn { ColumnName = KeyName, Caption = "Name" });
                objects.Columns.Add(new DataColumn { ColumnName = KeySpaceUsed, Caption = "Space Used" });
                objects.Columns.Add(new DataColumn { ColumnName = KeyCount, Caption = "Count", DataType = typeof(long) });
                objects.Columns.Add(new DataColumn { ColumnName = KeyCreateDate, Caption = "Create Date", DataType = typeof(DateTime) });
                objects.Columns.Add(new DataColumn { ColumnName = KeyModifyDate, Caption = "Modify Date", DataType = typeof(DateTime) });
                objects.Columns.Add(new DataColumn { ColumnName = KeyPath, Caption = "Path" });
                objects.Columns.Add(new DataColumn { ColumnName = KeyValue });
                objects.Columns.Add(new DataColumn { ColumnName = KeyType, DataType = typeof(ObjectModes) });
                return objects;
            }
        }

        private void OnObjectsAfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowObject(e.Node);
        }

        private void ShowObject(TreeNode node)
        {
            Task.Factory.StartNew(() =>
                {
                    using (NewWait())
                    {
                        SetSearchMode(false);
                        _previousDatabase = _currentDatabase;
                        _currentDatabase = string.Empty;
                        _previousObjectType = _currentObjectType;
                        _previousObjectMode = _currentObjectMode;
                        _currentObjectType = string.Empty;
                        var enableContextMenu = false;
                        if (node != null)
                        {
                            var root = GetRootNode(node);
                            DataTable data;
                            var objects = NewObjects;
                            var serverState = node.Tag as ServerState;
                            _currentServerInfo = root.Tag as ServerState;

                            //server is changed
                            if (_currentServerInfo != null && _previousServerInfo != null
                                && _currentServerInfo.Server != _previousServerInfo.Server)
                            {
                                this.Invoke(() =>
                                    {
                                        dgvServerHealth.Rows.Clear();
                                    });
                            }

                            switch (serverState.Key)
                            {
                                case KeyDatabase:
                                    _currentDatabase = node.Text;
                                    _currentServerInfo.Database = _currentDatabase;
                                    _currentObjectType = KeyDatabase;
                                    LoadDatabase(node.Parent);
                                    break;
                                case KeyTables:
                                case KeyViews:
                                case KeyFunctions:
                                case KeyAssemblies:
                                case KeySPs:
                                case KeyTriggers:
                                    var key = node.Name;
                                    if (_previousObjectType == key)
                                        return;
                                    enableContextMenu = true;
                                    _currentDatabase = node.Parent.Text;
                                    _currentObjectType = key;
                                    switch (key)
                                    {
                                        case KeyAssemblies:
                                            //todo:
                                            data = Query("SELECT a.name, NULL AS SpaceUsed, NULL AS Count, create_date AS CreateDate, modify_date AS ModifyDate, f.name AS Path FROM sys.assemblies a WITH (NOLOCK) LEFT JOIN sys.assembly_files f WITH (NOLOCK) ON a.assembly_id = f.assembly_id", CurrentServerInfo);
                                            break;
                                        case KeyTables:
                                            data = GetObjects(KeyTables);
                                            /*sql = @"Declare @Low float
            select @Low = d.low / 1024.
            from master.dbo.spt_values d
            where d.number = 1
            and d.type = 'E'
            select o.name, str((sum(i1.dpages) + isnull(sum(i2.used), 0)) * @Low) as size
            from sysindexes i1
            inner join sysobjects o on o.id = i1.id
            inner join sysindexes i2 on o.id = i2.id
            where i1.indid < 2 
            and i2.indid < 255
            group by o.name";
                                            var space = Query(sql, CurrentServerInfo);*/

                                            var count = Query(string.Format(QueryEngine.SqlTableInfo, string.Empty), CurrentServerInfo);
                                            data.AsEnumerable().ForEach((d) =>
                                            {
                                                var row = objects.NewRow();
                                                var tableName = d[KeyName] as string;
                                                var schemaName = d[KeySchemaName] as string;
                                                row[KeyName] = QueryEngine.GetObjectName(schemaName, tableName);
                                                var space = Query(string.Format("EXEC sp_spaceused '{0}'", row[KeyName]), CurrentServerInfo);
                                                if (space.Rows.Count > 0)
                                                    row[KeySpaceUsed] = ToMb(space.Rows[0]["data"]) + Utils.SizeMb + " / " + ToMb(space.Rows[0]["index_size"]) + Utils.SizeMb;
                                                var rows = count.Select("TableName = '" + tableName + "'");
                                                if (rows.Length > 0)
                                                    row[KeyCount] = rows[0]["RowCount"];
                                                row[KeyCreateDate] = d[KeyCreateDate];
                                                row[KeyModifyDate] = d[KeyModifyDate];
                                                row[KeyType] = ObjectModes.Table;
                                                row[KeyState] = il16.Images[8];
                                                objects.Rows.Add(row);
                                            });
                                            LoadObjects(objects);
                                            break;
                                        case KeySPs:
                                        case KeyViews:
                                        case KeyFunctions:
                                        case KeyTriggers:
                                            data = GetObjects(key);
                                            data.AsEnumerable().ForEach((d) =>
                                            {
                                                var row = objects.NewRow();
                                                row[KeyName] = QueryEngine.GetObjectName(d[KeySchemaName], d[KeyName].ToString());
                                                row[KeyCreateDate] = d[KeyCreateDate];
                                                row[KeyModifyDate] = d[KeyModifyDate];
                                                int image;
                                                switch (key)
                                                {
                                                    case KeySPs:
                                                        image = 3;
                                                        row[KeyType] = ObjectModes.Sp;
                                                        break;
                                                    case KeyViews:
                                                        row[KeyType] = ObjectModes.View;
                                                        image = 4;
                                                        break;
                                                    case KeyFunctions:
                                                        row[KeyType] = ObjectModes.Function;
                                                        image = 3;
                                                        break;
                                                    case KeyTriggers:
                                                        row[KeyType] = ObjectModes.Trigger;
                                                        image = 3;
                                                        break;
                                                    default:
                                                        image = 0;
                                                        break;
                                                }
                                                row[KeyState] = il16.Images[image];
                                                objects.Rows.Add(row);
                                            });
                                            LoadObjects(objects);
                                            break;
                                        default:
                                            break;
                                    }
                                    _currentObjectMode = ObjectModes.Objects;
                                    this.Invoke(() =>
                                    {
                                        node.ExpandAll();
                                    });
                                    break;
                                case KeyTable:
                                case KeySp:
                                case KeyView:
                                case KeyFunction:
                                case KeyTrigger:
                                case KeyAssembly:
                                    enableContextMenu = true;
                                    _currentDatabase = node.Parent.Parent.Text;
                                    _currentObjectType = node.Parent.Name;
                                    if (_currentServerInfo != GetRootNode(node).Tag as ServerState
                                        || _previousDatabase != _currentDatabase
                                        || _previousObjectType != _currentObjectType)
                                    {
                                        _currentServerInfo = GetRootNode(node).Tag as ServerState;
                                        _currentServerInfo.Database = _currentDatabase;
                                        ShowObject(node.Parent);
                                    }
                                    GetObjectScript(node.Text, null, ObjectModes.Objects);
                                    break;
                                default:
                                    _currentObjectMode = ObjectModes.Server;
                                    break;
                            }
                            this.Invoke(() =>
                                {
                                    lblObjectCount.Text = node.Nodes.Count > 0 ? node.Nodes.Count.ToString() : "1";
                                });
                            //tcMain.SelectedTab = tpObjects;
                        }
                        this.Invoke(() =>
                                {
                                    SetupPerformance();
                                    dgvObjects.ContextMenuStrip = enableContextMenu ? cmsObjectList : null;
                                });
                        _previousDatabase = _currentDatabase;
                        _previousObjectType = _currentObjectType;
                        _previousObjectMode = _currentObjectMode;
                    }
                }).LogExceptions();
        }

        private string ToMb(object kb)
        {
            var value = ToKb(kb);
            return (value / Utils.Size1K).ToString("0.###");
        }

        private decimal ToKb(object kb)
        {
            return Convert.ToDecimal(Regex.Replace(kb.ToString(), Utils.SizeKb, string.Empty, RegexOptions.IgnoreCase));
        }

        private DataTable GetObjects(string objectType)
        {
            string types;
            switch (objectType)
            {
                case KeyTriggers:
                    return Query("SELECT '' AS SchemaName, name, create_date AS CreateDate, modify_date AS ModifyDate, type FROM sys.triggers WITH (NOLOCK) WHERE parent_class = 0", CurrentServerInfo);
                default:
                    switch (objectType)
                    {
                        case KeyTables:
                            types = "'U'";
                            break;
                        case KeyViews:
                            types = "'V'";
                            break;
                        case KeyFunctions:
                            types = "'FN', 'IF', 'TF'";
                            break;
                        case KeySPs:
                            types = "'P'";
                            break;
                        default:
                            types = string.Empty;
                            break;
                    }
                    var filter = !string.IsNullOrEmpty(types) ? " WHERE so.type IN (" + types + ")" : string.Empty;
                    return GetObjectsFilter(filter);
            }
        }

        private DataTable GetObjectsFilter(string filter)
        {
            return Query(string.Format("SELECT su.name AS SchemaName, so.name, so.create_date AS CreateDate, so.modify_date AS ModifyDate, so.type FROM sys.objects so WITH (NOLOCK) LEFT JOIN sys.schemas su WITH (NOLOCK) ON so.schema_id = su.schema_id {0} ORDER BY su.name, so.name", filter), CurrentServerInfo);
        }

        private bool CheckVersionControl(out bool exists)
        {
            var data = SqlHelper.ExecuteScalar(string.Format("SELECT is_disabled FROM sys.triggers WITH (NOLOCK) WHERE NAME = '{0}'", Settings.Instance.VersionControlTriggerName), CurrentServerInfo);
            if (data != null)
            {
                exists = true;
                return !Convert.ToBoolean(data);
            }
            else
            {
                exists = false;
                return false;
            }
        }

        private void SetVersionControl(bool enable)
        {
            bool exists;
            var state = CheckVersionControl(out exists);
            if (enable)
            {
                if (state)
                {
                }
                else if (exists)
                    SqlHelper.ExecuteNonQuery(string.Format("ENABLE TRIGGER [{0}] ON DATABASE", Settings.Instance.VersionControlTriggerName), CurrentServerInfo);
                else
                {
                    if (ShowQuestion(string.Format("By enabling version control, table {0} will be created. Are you sure to procceed?", Settings.Instance.VersionControlTableName)))
                    {
                        var sql = string.Format(@"
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WITH (NOLOCK) WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[{0}](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[databasename] [varchar](256) NULL,
	[eventtype] [varchar](50) NULL,
	[objectname] [varchar](256) NULL,
	[objecttype] [varchar](25) NULL,
	[sqlcommand] [nvarchar](max) NULL,
	[loginname] [varchar](256) NULL,
	[hostname] [varchar](256) NULL,
	[PostTime] [datetime] NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [{1}]
ON DATABASE
FOR CREATE_PROCEDURE, ALTER_PROCEDURE, DROP_PROCEDURE,
CREATE_TABLE, ALTER_TABLE, DROP_TABLE,
CREATE_FUNCTION, ALTER_FUNCTION, DROP_FUNCTION,
CREATE_TRIGGER, ALTER_TRIGGER, DROP_TRIGGER,
CREATE_VIEW, ALTER_VIEW, DROP_VIEW,
CREATE_INDEX, ALTER_INDEX, DROP_INDEX

AS

SET NOCOUNT ON

DECLARE @CurrentVersion int
DECLARE @CurrentID int
DECLARE @DatabaseName varchar(256)
DECLARE @ObjectName varchar(256)
DECLARE @data XML

SET @data = EVENTDATA()

INSERT INTO dbo.{0}(databasename, eventtype,objectname, objecttype, sqlcommand, loginname,Hostname,PostTime, Version)
VALUES(
@data.value('(/EVENT_INSTANCE/DatabaseName)[1]', 'varchar(256)'),
@data.value('(/EVENT_INSTANCE/EventType)[1]', 'varchar(50)'),  -- value is case-sensitive
@data.value('(/EVENT_INSTANCE/ObjectName)[1]', 'varchar(256)'), 
@data.value('(/EVENT_INSTANCE/ObjectType)[1]', 'varchar(25)'), 
@data.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'varchar(max)'), 
@data.value('(/EVENT_INSTANCE/LoginName)[1]', 'varchar(256)'),
HOST_NAME(),
GETDATE(),
0
)

SET @CurrentID = IDENT_CURRENT('{0}')
SELECT @DatabaseName = databasename, @ObjectName = objectname FROM {0} WITH (NOLOCK) WHERE ID = @CurrentID
IF (@DatabaseName IS NOT NULL AND @ObjectName IS NOT NULL)
BEGIN
	SELECT @CurrentVersion = MAX(Version) FROM {0} WITH (NOLOCK) WHERE databasename = @DatabaseName AND objectname = @ObjectName
	UPDATE {0} SET Version = ISNULL(@CurrentVersion, 0) + 1 WHERE ID = @CurrentID
END
GO
ENABLE TRIGGER [{1}] ON DATABASE
GO
", Settings.Instance.VersionControlTableName, Settings.Instance.VersionControlTriggerName);
                        var statements = sql.Split(new string[] { "GO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        statements.ForEach(s => SqlHelper.ExecuteNonQuery(s, CurrentServerInfo));
                    }
                }
            }
            else
            {
                if (state && exists)
                    SqlHelper.ExecuteNonQuery(string.Format("DISABLE TRIGGER [{0}] ON DATABASE", Settings.Instance.VersionControlTriggerName), CurrentServerInfo);
            }
            GetVersionControlState();
        }

        internal List<KeyValuePair<string, int>> GetObjectVersions()
        {
            var versions = new List<KeyValuePair<string, int>>();
            versions.Add(new KeyValuePair<string, int>("Current", 0));
            try
            {
                bool exists;
                if (CheckVersionControl(out exists) || exists)
                {
                    string schemaName;
                    _currentObjectName = QueryEngine.ParseObjectName(_currentObjectName, out schemaName);
                    var data = Query(string.Format("SELECT HostName, LoginName, PostTime, Version FROM {2} WITH (NOLOCK) WHERE databasename = '{0}' AND objectname = '{1}' ORDER BY Version DESC", CurrentServerInfo.Database, _currentObjectName, Settings.Instance.VersionControlTableName), CurrentServerInfo);
                    data.AsEnumerable().ForEach(r => versions.Add(new KeyValuePair<string, int>(string.Format("{0}  at ({1}) from ({2}) as ({3})", r["Version"].ToString(), r["PostTime"].ToString(), r["HostName"].ToString(), r["LoginName"].ToString()), Convert.ToInt32(r["Version"]))));
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("Version could not be fetched, maybe version control table {0} does not exits \r\n{1}", Settings.Instance.VersionControlTableName, ex.Message));
            }
            return versions;
        }

        private void GetObjectScript(string objectName, string script, ObjectModes objectMode)
        {
            this.Invoke(() =>
                {
                    _currentObjectName = objectName;
                    _currentObjectScript = script;
                    _currentObjectMode = objectMode;
                    cboObjectScriptVersions.DataSource = GetObjectVersions();
                    cboObjectScriptVersions.SelectedIndex = 0;
                    SetCompareVersion();
                });
        }

        private void SetObjectScriptVersion(string objectName, int version)
        {
            SetObjectScript(GetObjectScriptVersionText(objectName, version));
        }

        internal string GetObjectScriptVersionText(int version)
        {
            return GetObjectScriptVersionText(_currentObjectName, version);
        }

        private string GetObjectScriptVersionText(string objectName, int version)
        {
            bool exists;
            string script;
            if (CheckVersionControl(out exists) || exists)
            {
                var data = SqlHelper.ExecuteScalar(string.Format("SELECT sqlcommand FROM {3} WITH (NOLOCK) WHERE databasename = '{0}' AND objectname = '{1}' AND Version = {2} ORDER BY Version DESC", CurrentServerInfo.Database, objectName, version, Settings.Instance.VersionControlTableName), CurrentServerInfo);
                script = data != null ? data.ToString() : string.Empty;
            }
            else
                script = "(COULD NOT FIND VERSION)";
            return script;
        }

        private void SetObjectScriptEx(string objectName, string script, ObjectModes objectMode)
        {
            SetObjectScript(GetObjectScriptText(objectName, script, objectMode));
        }

        internal string GetObjectScriptTextEx()
        {
            return GetObjectScriptText(_currentObjectName, _currentObjectScript, _currentObjectMode);
        }

        private string GetObjectScriptText(string objectName, string script, ObjectModes objectMode)
        {
            string text;
            string sql;
            string objectType;
            switch (objectMode)
            {
                case ObjectModes.Databases:
                case ObjectModes.Objects:
                case ObjectModes.Assembly:
                    objectType = _currentObjectType;
                    break;
                case ObjectModes.Table:
                    objectType = KeyTables;
                    break;
                case ObjectModes.Job:
                    objectType = KeyJobs;
                    break;
                case ObjectModes.Trigger:
                    objectType = KeyTriggers;
                    break;
                case ObjectModes.Index:
                    objectType = KeyIndexes;
                    break;
                default:
                    objectType = string.Empty;
                    break;
            }
            string schemaName;
            var parsedObjectName = QueryEngine.ParseObjectName(objectName, out schemaName);
            switch (objectType)
            {
                case KeyAssemblies:
                    text = string.Empty;
                    break;
                case KeyTables:
                    sql = @"	declare @Id int, @i int, @i2 int,@Sql varchar(max),@Sql2 varchar(max), @f1 varchar(5), @f2 varchar(5), @f3 varchar(5), @f4 varchar(5), @T varchar(5)
	select @Id=object_id('" + objectName + @"'), @f1 = char(13) + char(10), @f2 = '	', @f3=@f1+@f2, @f4=',' + @f3
	
	if not(@Id is null)
    BEGIN
	declare @Data table(Id int identity primary key, D varchar(max) not null, ic int null, re int null, o int not null);
	
	-- Columns
	with c as(
		select c.column_id, Nr = row_number() over(order by c.column_id), Clr=count(*) over(),
			D = quotename(c.name) + ' ' +
				case when s.name = 'sys' or c.is_computed=1 then '' else quotename(s.name) + '.' end +
				case when c.is_computed=1 then '' when s.name = 'sys' then t.Name else quotename(t.name) end +
				case when c.user_type_id!=c.system_type_id or c.is_computed=1 then ''
					when t.Name in ('xml', 'uniqueidentifier', 'tinyint', 'timestamp', 'time', 'text', 'sysname', 'sql_variant', 'smallmoney', 'smallint', 'smalldatetime', 'ntext', 'money',
									'int', 'image', 'hierarchyid', 'geometry', 'geography', 'float', 'datetimeoffset', 'datetime2', 'datetime', 'date', 'bigint', 'bit') then ''
					when t.Name in('varchar','varbinary', 'real', 'numeric', 'decimal', 'char', 'binary')
						then '(' + isnull(convert(varchar,nullif(c.max_length,-1)), 'max') + isnull(','+convert(varchar,nullif(c.scale, 0)), '') + ')'
					when t.Name in('nvarchar','nchar')
						then '(' + isnull(convert(varchar,nullif(c.max_length,-1) / 2), 'max') + isnull(','+convert(varchar,nullif(c.scale, 0)), '') + ')'
					else '??'
					end + 
				case when ic.object_id is not null then ' identity(' + convert(varchar,ic.seed_value) + ',' + convert(varchar,ic.increment_value) + ')' else '' end +
				case when c.is_computed=1 then 'as' + cc.definition when c.is_nullable = 1 then ' null' else ' not null' end +
				case c.is_rowguidcol when 1 then ' rowguidcol' else '' end +
				case when d.object_id is not null then ' default ' + d.definition else  '' end
		from sys.columns c
		inner join sys.types t
		on t.user_type_id = c.user_type_id
		inner join sys.schemas s
		on s.schema_id=t.schema_id
		left outer join sys.computed_columns cc
		on cc.object_id=c.object_id and cc.column_id=c.column_id
		left outer join sys.default_constraints d
		on d.parent_object_id=@id and d.parent_column_id=c.column_id
		left outer join sys.identity_columns ic
		on ic.object_id=c.object_id and ic.column_id=c.column_id
		where c.object_id=@Id
		
	)
		insert into @Data(D, o)
		select '	' + D + case Nr when Clr then '' else ',' + @f1 end, 0
		from c where NOT D IS NULL 
		order by column_id
	
	-- SubObjects
	set @i=0
	while 1=1
		begin
		select top 1 @i=c.object_id, @T = c.type, @i2=i.index_id
		from sys.objects c 
		left outer join sys.indexes i
		on i.object_id=@Id and i.name=c.name
		where parent_object_id=@Id and c.object_id>@i and c.type not in('D')
		order by c.object_id
		if @@rowcount=0 break
		if @T = 'C' 
			insert into @Data 
			select @f4 + 'check ' + case is_not_for_replication when 1 then 'not for replication ' else '' end + definition, null, null, 10
			from sys.check_constraints where object_id=@i
		else if @T = 'Pk'
			insert into @Data 
			select @f4 + 'primary key' + isnull(' ' + nullif(lower(i.type_desc),'clustered'), ''), @i2, null, 20
			from sys.indexes i
			where i.object_id=@Id and i.index_id=@i2
		else if @T = 'uq'
			insert into @Data values(@f4 + 'unique', @i2, null, 30)
		else if @T = 'f'
			begin
			insert into @Data 
			select @f4 + 'foreign key', -1, @i, 40
			from sys.foreign_keys f
			where f.object_id=@i
			
			insert into @Data 
			select ' references ' + quotename(s.name) + '.' + quotename(o.name), -2, @i, 41
			from sys.foreign_keys f
			inner join sys.objects o
			on o.object_id=f.referenced_object_id
			inner join sys.schemas s
			on s.schema_id=o.schema_id
			where f.object_id=@i
			
			insert into @Data 
			select ' not for replication', -3, @i, 42
			from sys.foreign_keys f
			inner join sys.objects o
			on o.object_id=f.referenced_object_id
			inner join sys.schemas s
			on s.schema_id=o.schema_id
			where f.object_id=@i and f.is_not_for_replication=1
			end
		else
			insert into @Data values(@f4 + 'Unknow SubObject [' + @T + ']', null, null, 99)
		end

	insert into @Data values(@f1+')', null, null, 100)
	
	-- Indexes
	insert into @Data
	select @f1 + 'create ' + case is_unique when 1 then 'unique ' else '' end + lower(s.type_desc) + ' index ' + 'i' + convert(varchar, row_number() over(order by index_id)) + ' on ' + quotename(sc.Name) + '.' + quotename(o.name), index_id, null, 1000
	from sys.indexes s
	inner join sys.objects o
	on o.object_id=s.object_id
	inner join sys.schemas sc
	on sc.schema_id=o.schema_id
	where s.object_id=@Id and is_unique_constraint=0 and is_primary_key=0 and s.type_desc != 'heap'
	
	-- columns
	set @i=0
	while 1=1
		begin
		select top 1 @i=ic from @Data where ic>@i order by ic 
		if @@rowcount=0 break
		select @i2=0, @Sql=null, @Sql2=null
		while 1=1
			begin
			select @i2=index_column_id, 
				@Sql = case c.is_included_column when 1 then @Sql else isnull(@Sql + ', ', '(') + cc.Name + case c.is_descending_key when 1  then ' desc' else '' end end,
				@Sql2 = case c.is_included_column when 0 then @Sql2 else isnull(@Sql2 + ', ', '(') + cc.Name + case c.is_descending_key when 1  then ' desc' else '' end end
			from sys.index_columns c
			inner join sys.columns cc
			on c.column_id=cc.column_id and cc.object_id=c.object_id
			where c.object_id=@Id and index_id=@i and index_column_id>@i2
			order by index_column_id
			if @@rowcount=0 break
			end
		update @Data set D=D+@Sql +')' + isnull(' include' + @Sql2 + ')', '') where ic=@i
		end
		
	-- references
	set @i=0
	while 1=1
		begin
		select top 1 @i=re from @Data where re>@i order by re
		if @@rowcount=0 break
		
		select @i2=0, @Sql=null, @Sql2=null
		while 1=1
			begin
			select @i2=f.constraint_column_id, 
				@Sql = isnull(@Sql + ', ', '(') + c1.Name,
				@Sql2 = isnull(@Sql2 + ', ', '(') + c2.Name
			from sys.foreign_key_columns f
			inner join sys.columns c1
			on c1.column_id=f.parent_column_id and c1.object_id=f.parent_object_id
			inner join sys.columns c2
			on c2.column_id=f.referenced_column_id and c2.object_id=f.referenced_object_id
			where f.constraint_object_id=@i and f.constraint_column_id>@i2
			order by f.constraint_column_id
			if @@rowcount=0 break
			end
		update @Data set D = D + @Sql + ')'  where re=@i and ic=-1
		update @Data set D = D + @Sql2 + ')'  where re=@i and ic=-2
		end;
	
	-- Render
	with x as(
		select id=d.id-1, D=d.D + isnull(d2.D,'')
		from @Data d
		left outer join @Data d2
		on d.re=d2.re and d2.o=42
		where d.o=41
		
	)
	update @Data
		set D=d.D+x.D
	from @Data d
	inner join x
	on x.id=d.id
	
	delete @Data where o in(41, 42)
	
	select @Sql = 'create table ' + quotename(s.name) + '.' + quotename(o.name) + '(' + @f1
	from sys.objects o
	inner join sys.schemas s
	on o.schema_id = s.schema_id
	where o.object_id=@Id
	
	set @i=0
	while 1=1
		begin
		select top 1 @I=Id, @Sql = @Sql + D from @Data order by o, case when o=0 then right('0000' + convert(varchar,id),5)  else D end, id
		if @@rowcount=0 break
		delete @Data where id=@i
		end
    END
    SELECT @Sql";
                    text = SqlHelper.ExecuteScalar(sql, CurrentServerInfo) as string;
                    break;
                case KeyJobs:
                    text = script;
                    break;
                case KeyTriggers:
                    //http://sqlserver-qa.net/blogs/t-sql/archive/2008/12/11/5147.aspx
                    sql = string.Format("SELECT object_definition(m.object_id) FROM sys.sql_modules m LEFT JOIN sys.triggers t ON m.object_id = t.object_id WHERE t.name = '{0}'", parsedObjectName);
                    text = SqlHelper.ExecuteScalar(sql, CurrentServerInfo) as string;
                    break;
                case KeyIndexes:
                    //http://blog.sqlauthority.com/2007/12/18/sql-server-get-information-of-index-of-tables-and-indexed-columns/
                    sql = string.Format("EXEC sp_helpindex '{0}'", parsedObjectName);
                    text = SqlHelper.ExecuteScalar(sql, CurrentServerInfo) as string;
                    break;
                default:
                    if (objectMode != ObjectModes.None)
                    {
                        sql = @"
SELECT DISTINCT o.name, o.xtype, c.text, c.colid
FROM syscomments c WITH (NOLOCK)
LEFT JOIN sysobjects o WITH (NOLOCK) ON c.id=o.id
LEFT JOIN sys.schemas u WITH (NOLOCK) ON o.uid = u.schema_id
WHERE o.name = '" + parsedObjectName + "' AND u.name = '" + schemaName + "'" +
" ORDER BY c.colid";
                        var data = Query(sql, CurrentServerInfo);
                        if (data != null)
                        {
                            var result = new StringBuilder();
                            foreach (DataRow item in data.Rows)
                            {
                                result.Append(item["text"] as string);
                            }
                            text = result.ToString();
                        }
                        else
                            text = string.Empty;
                    }
                    else
                        text = string.Empty;
                    break;
            }
            return text;
        }

        private void SetObjectScript(string script)
        {
            rtbObjectScript.Text = script != null ? script.Trim() : string.Empty;
            rtbObjectScript.Refresh();
        }

        private void OnObjectsRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isUpdating && e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var objectMode = dgvObjects.Rows[e.RowIndex].Cells[8].Value != DBNull.Value ? (ObjectModes)dgvObjects.Rows[e.RowIndex].Cells[8].Value : ObjectModes.None;
                GetObjectScript(dgvObjects.Rows[e.RowIndex].Cells[1].Value as string, dgvObjects.Rows[e.RowIndex].Cells[6].Value as string, objectMode);
                if (_isInSearch && _isSearching)
                {
                    _isSearching = false;
                    _currentSearchIndex = 0;
                    cmdSearchObjectNext.Enabled = true;
                    cmdSearchObjectPrevious.Enabled = true;
                    FindObjectScriptKeyword(true);
                }
            }
        }

        private void FindObjectScriptKeyword(bool isNext)
        {
            var text = rtbObjectScript.Text;
            var keyword = Settings.Instance.LastSearchContent;
            var isCaseSenstive = Settings.Instance.LastSearchIsCaseSenstive;
            var compare = isCaseSenstive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            int index;
            if (isNext)
            {
                if (_currentSearchIndex + 1 <= text.Length)
                    index = text.IndexOf(keyword, _currentSearchIndex + 1, compare);
                else
                    index = -1;
            }
            else
            {
                if (_currentSearchIndex == -1)
                    _currentSearchIndex = text.Length;
                if (_currentSearchIndex > 0 && _currentSearchIndex - 1 <= text.Length)
                    index = text.LastIndexOf(keyword, _currentSearchIndex - 1, compare);
                else
                    index = -1;
            }
            if (index != -1)
            {
                var endOffset = index + keyword.Length;
                rtbObjectScript.ActiveTextAreaControl.TextArea.Caret.Position = rtbObjectScript.ActiveTextAreaControl.TextArea.Document.OffsetToPosition(endOffset);
                rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
                var document = rtbObjectScript.ActiveTextAreaControl.TextArea.Document;
                var selection = new DefaultSelection(document, document.OffsetToPosition(index), document.OffsetToPosition(endOffset));
                rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.SetSelection(selection);
                rtbObjectScript.ActiveTextAreaControl.TextArea.Caret.Position = document.OffsetToPosition(index);
            }
            else
            {
                rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
                rtbObjectScript.ActiveTextAreaControl.TextArea.Caret.Position = new TextLocation(0, 0);
            }
            rtbObjectScript.ActiveTextAreaControl.TextArea.ScrollToCaret();
            _currentSearchIndex = index;
            if (isNext)
            {
                cmdSearchObjectNext.Enabled = _currentSearchIndex != -1;
                cmdSearchObjectPrevious.Enabled = true;
            }
            else
            {
                cmdSearchObjectPrevious.Enabled = _currentSearchIndex != -1;
                cmdSearchObjectNext.Enabled = true;
            }
        }

        private void OnTruncateObjectClick(object sender, EventArgs e)
        {
            TruncateLog(_currentDatabase);
        }

        private void TruncateLog(string database)
        {
            if (!string.IsNullOrEmpty(database))
            {
                var info = GetDatabaseInfo(database);
                if (info != null && info.Rows.Count > 0)
                {
                    var logName = info.Rows[1]["Logical_Name"] as string;
                    using (NewWait())
                    {
                        SqlHelper.ExecuteNonQuery("DBCC SHRINKFILE ([" + logName + "] , 0, TRUNCATEONLY)", GetServerInfo(database));
                    }
                    ShowMessage(string.Format("{0} log truncated.", database));
                }
            }
        }

        //private string CurrentObjectName
        //{
        //    get
        //    {
        //        if (dgvObjects.SelectedCells.Count > 0)
        //            return dgvObjects.SelectedCells[0].OwningRow.Cells[1].Value as string;
        //        else
        //            return null;
        //    }
        //}

        private void OnShrinkDatabaseClick(object sender, EventArgs e)
        {
            ShrinkDatabase(_currentDatabase);
        }

        private void ShrinkDatabase(string database)
        {
            if (!string.IsNullOrEmpty(database))
            {
                using (NewWait())
                {
                    SqlHelper.ExecuteNonQuery("DBCC SHRINKDATABASE ([" + database + "])", GetServerInfo(database));
                }
                ShowMessage(string.Format("{0} shrinked.", database));
            }
        }

        private void OnObjectsMenuOpening(object sender, CancelEventArgs e)
        {
            var node = tvObjects.SelectedNode;
            var tag = node != null ? node.Tag as ServerState : null;
            var isServer = node != null && tag.Key == KeyServer;
            var isDatabase = node != null && tag.Key == KeyDatabase;
            tmiDetachDatabase.Visible = isDatabase;
            tmiSetDatabaseState.Visible = isDatabase;
            if (isDatabase)
                tmiSetDatabaseState.Text = tag.State ? "Set Offline" : "Set Online";
            tmiShrinkDatabase.Visible = isDatabase;
            tmiTruncateObject.Visible = isDatabase;
            tmiBackupDatabase.Visible = isDatabase;
            tmiCheckDB.Visible = isDatabase;
            var hasParent = node != null && node.Parent != null;
            var isTable = hasParent && tag.Key == KeyTable;
            tmiOpenTable.Visible = isTable;
            tmiTruncateTable.Visible = isTable;
            tmiCleanTable.Visible = isTable;
            tmiTableIndexDefrag.Visible = isTable;
            tmiObjectDependencies.Visible = hasParent && (isTable || tag.Key == KeySp || tag.Key == KeyView || tag.Key == KeyFunction || tag.Key == KeyAssembly);
            tmiRemoveServer.Visible = isServer;
            tmiEditServer.Visible = isServer;
            tmiSetVersionControl.Visible = isDatabase && !QueryEngine.SystemDatabases.Contains(_currentDatabase);
            tmiShowPerformance.Visible = isDatabase || isServer;
            tmiSearchDatabase.Visible = !string.IsNullOrEmpty(_currentDatabase) && _currentObjectMode != ObjectModes.Server;
        }

        private void OnObjectsCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            this.Invoke(() =>
                {
                    if (e.ColumnIndex == 0)
                    {
                        var node = tvObjects.SelectedNode;
                        Image image = null;
                        switch (_currentObjectMode)
                        {
                            case ObjectModes.None:
                                image = Resources.List2;
                                break;
                            case ObjectModes.Databases:
                                image = Convert.ToInt32(dgvObjects.Rows[e.RowIndex].Cells[KeyValue].Value) == 0 ? Resources.Proxy2 : Resources.Server2;
                                break;
                            case ObjectModes.Objects:
                            case ObjectModes.Table:
                                image = Resources.Table2;
                                break;
                            case ObjectModes.Sp:
                            case ObjectModes.Function:
                                image = Resources.Gear2;
                                break;
                            case ObjectModes.View:
                                image = Resources.List2;
                                break;
                            case ObjectModes.Trigger:
                                image = Resources.Accelerator2;
                                break;
                            case ObjectModes.Job:
                                image = Resources.History2;
                                break;
                            default:
                                image = Resources.List2;
                                break;
                        }
                        e.Value = image;
                    }
                });
        }

        private void AddCommand(string key, string text)
        {
            AddCommand(key, text, null);
        }

        private void AddCommand(string key, string text, Image icon)
        {
            var item = FindCommand<ToolStripButton>(key);
            if (item != null)
            {
                item.Text = text;
                if (icon != null)
                    item.Image = icon;
            }
        }

        private T FindCommand<T>(string key) where T : ToolStripItem
        {
            return (T)tsCommands.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == key);
        }

        private ToolStripComboBox AddCommand<T>(string key, T selectedValue)
        {
            var item = FindCommand<ToolStripComboBox>(key);
            if (item == null)
            {
                item = new ToolStripComboBox { Name = key, Width = 90 };
                item.Tag = true;
                Enum.GetValues(typeof(T)).Cast<T>().ForEach((s) => item.Items.Add(s));
                item.SelectedItem = selectedValue;
                tsCommands.Items.Insert(tsCommands.Items.IndexOf(tssRuntime), item);
            }
            item.Visible = true;
            return item;
        }

        private ToolStripButton AddCommand(string key, string text, Bitmap image, bool checkOnClick, EventHandler onItemClick)
        {
            var item = FindCommand<ToolStripButton>(key);
            if (item == null)
            {
                item = new ToolStripButton(text, image, onItemClick, key);
                item.CheckOnClick = checkOnClick;
                item.Tag = true;
                tsCommands.Items.Insert(tsCommands.Items.IndexOf(tssRuntime), item);
            }
            else
            {
                var f1 = typeof(ToolStripItem).GetField("EventClick",
                    BindingFlags.Static | BindingFlags.NonPublic);
                var obj = f1.GetValue(item);
                var pi = item.GetType().GetProperty("Events",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList)pi.GetValue(item, null);
                list.RemoveHandler(obj, list[obj]);
                item.Click += onItemClick;
                item.Text = text;
            }
            item.Visible = true;
            return item;
        }

        private void SetSeparator(bool visible)
        {
            tssRuntime.Visible = visible;
        }

        private void OnMainSelectedIndexChanged(object sender, EventArgs e)
        {
            tsCommands.Items.Cast<ToolStripItem>().ForEach(t => { if (t.Tag != null) t.Visible = false; });
            SetSeparator(false);
            if (tcMain.SelectedIndex < (int)WorkModes.Query)
                _currentWorkMode = (WorkModes)tcMain.SelectedIndex;
            else
                _currentWorkMode = (WorkModes)tcMain.SelectedTab.Tag;
            switch (_currentWorkMode)
            {
                case WorkModes.Summary:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    break;
                case WorkModes.Objects:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    break;
                case WorkModes.Activities:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    var tbProcesses = AddCommand(CommandProcesses, "Processes", Resources.Gear2, true, (sn, ev) =>
                        {
                            SetActivityType(ActivityTypes.Process);
                            RefreshData();
                        });
                    var tbJobs = AddCommand(CommandJobs, "Jobs", Resources.Schedule2, true, (sn, ev) =>
                        {
                            SetActivityType(ActivityTypes.Job);
                            RefreshData();
                        });
                    var command = AddCommand<ActivityStatuses>(CommandActivityStatuses, Settings.Instance.ActivityState);
                    AddCommand(CommandVisualize, "Visualize", Resources.Gear2, false, OnVisualizeProcessClick);
                    AddCommand(CommandDelete, "Kill", Resources.Cross2, false, OnKillProcessClick);
                    switch (Settings.Instance.ActivityType)
                    {
                        case ActivityTypes.Process:
                            tbProcesses.Checked = true;
                            break;
                        case ActivityTypes.Job:
                            tbJobs.Checked = true;
                            break;
                        default:
                            break;
                    }
                    SetActivityType(Settings.Instance.ActivityType);
                    SetSeparator(true);
                    break;
                case WorkModes.Analysis:
                case WorkModes.Alerts:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    break;
                case WorkModes.Histories:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    AddCommand(CommandDelete, "Clear", Resources.Cross2, false, OnClearHistoriesClick);
                    SetSeparator(true);
                    break;
                case WorkModes.Query:
                    AddCommand(CommandRefresh, "Execute", GetExecuteIcon());
                    break;
                case WorkModes.TableData:
                    AddCommand(CommandRefresh, "Execute", GetExecuteIcon());
                    break;
                default:
                    AddCommand(CommandRefresh, "Refresh", Resources.Refresh2);
                    break;
            }
            SaveSettings();
        }

        private Image GetExecuteIcon()
        {
            Image icon;
            switch (_currentWorkMode)
            {
                case WorkModes.Query:
                    var queryData = tcMain.SelectedTab.Controls[0] as UserQuery;
                    icon = queryData.IsRunning ? Resources.Cross2 : Resources.Refresh2;
                    break;
                case WorkModes.TableData:
                    var tableData = tcMain.SelectedTab.Controls[0] as UserTableData;
                    icon = tableData.IsRunning ? Resources.Cross2 : Resources.Refresh2;
                    break;
                default:
                    icon = null;
                    break;
            }
            return icon;
        }

        private void OnClearHistoriesClick(object sender, EventArgs e)
        {
            rtbHistoryDetail.Text = string.Empty;
            Settings.Instance.NotifiedAlerts.Clear();
            LoadNotifiedAlerts();
        }

        private void OnObjectsAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                var tag = e.Node.Tag as ServerState;
                switch (tag.Key)
                {
                    case KeySp:
                    case KeyTable:
                    case KeyFunction:
                    case KeyDatabase:
                        if (ShowQuestion(string.Format("Are you sure to change the name from {0} to {1}?", e.Node.Text, e.Label)))
                        {
                            string schemaName;
                            var newObjectName = QueryEngine.ParseObjectName(e.Label, out schemaName);
                            if (tag.Key == KeyDatabase)
                                SqlHelper.ExecuteNonQuery(string.Format("EXEC sp_renamedb N'{0}', N'{1}'", e.Node.Text, newObjectName), CurrentServerInfo);
                            else
                                SqlHelper.ExecuteNonQuery(string.Format("EXEC sp_rename '{0}', '{1}'", e.Node.Text, newObjectName), CurrentServerInfo);
                        }
                        break;
                    default:
                        e.CancelEdit = true;
                        break;
                }
            }
        }

        private void OnSetDatabaseStateClick(object sender, EventArgs e)
        {
            var node = tvObjects.SelectedNode;
            bool isOnline;
            if (node != null)
            {
                var tag = node.Tag as ServerState;
                isOnline = node.Tag == null || tag.State;
            }
            else
                isOnline = true;
            SetOnlineOffline(_currentDatabase, !isOnline);
        }

        private void OnDetachDatabaseClick(object sender, EventArgs e)
        {
            DetachDatabase(_currentDatabase);
        }

        private void DetachDatabase(string database)
        {
            using (NewWait())
            {
                SqlHelper.ExecuteNonQuery(string.Format("EXEC sp_detach_db @dbname = '{0}'", database), DefaultServerInfo);
            }
        }

        private void OnAttachClick(object sender, EventArgs e)
        {
            using (var dlg = new FileNameDialog("Attach Database", false, string.Empty))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    using (NewWait())
                    {
                        //SQLHelper.ExecuteNonQuery(string.Format("EXEC sp_attach_single_file_db @dbname = '{0}', @physname = '{1}'", dlg.ObjectName, dlg.FilePath), DefaultServerInfo);
                        var dataFile = dlg.FilePath;
                        var logFile = System.IO.Path.ChangeExtension(dataFile, "_log.ldf");
                        if (File.Exists(logFile))
                            SqlHelper.ExecuteNonQuery(string.Format("CREATE DATABASE [{0}] ON (FILENAME = '{1}'), (FILENAME = '{2}') FOR ATTACH", dlg.ObjectName, dataFile, logFile), DefaultServerInfo);
                        else
                            SqlHelper.ExecuteNonQuery(string.Format("CREATE DATABASE [{0}] ON ( FILENAME = N'{1}' ) FOR ATTACH_REBUILD_LOG", dlg.ObjectName, dataFile), DefaultServerInfo);
                        tcMain.SelectedTab = tpSummary;
                        RefreshData();
                    }
                }
            }
        }

        private void OnBackupDatabaseClick(object sender, EventArgs e)
        {
            using (var dlg = new FileNameDialog("Backup Database", true, _currentDatabase + " full backup"))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    using (NewWait())
                    {
                        SqlHelper.ExecuteNonQuery(string.Format("BACKUP DATABASE {2} TO DISK = '{1}' WITH NOFORMAT, NO_COMPRESSION, NOINIT,  NAME = N'{0}', SKIP, NOREWIND, NOUNLOAD, STATS = 10", dlg.ObjectName, dlg.FilePath, _currentDatabase), DefaultServerInfo);
                        tcMain.SelectedTab = tpSummary;
                        RefreshData();
                    }
                }
            }
        }

        private string SafeSql(string content)
        {
            var chars = new string[] { "[]", "%", "[", "_" };
            chars.ForEach(c => { content = content.Replace(c, string.Format("[{0}]", c)); });
            return content.Replace("'", "''");
        }

        private ObjectModes GetObjectMode(string typeName)
        {
            switch (typeName.Trim().ToUpper())
            {
                case "P":
                    return ObjectModes.Sp;
                case "U":
                    return ObjectModes.Table;
                case "V":
                    return ObjectModes.View;
                case "FN":
                case "IF":
                case "TF":
                    return ObjectModes.Function;
                case "TR":
                    return ObjectModes.Trigger;
                case "JOB":
                    return ObjectModes.Job;
                case "PK":
                    return ObjectModes.Index;
                case "IDX":
                    return ObjectModes.Index;
                default:
                    return ObjectModes.None;
            }
        }

        private void OnSearchDatabaseClick(object sender, EventArgs e)
        {
            SearchDatabase(Settings.Instance.LastSearchContent);
        }

        private void SearchDatabase(string Content)
        {
            using (var dlg = new ContentDialog(Content, Settings.Instance.LastSearchIsCaseSenstive, Settings.Instance.LastSearchIsObject, Settings.Instance.SearchHistories))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var content = dlg.Content;
                    var compare = dlg.IsCaseSenstive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
                    var objects = NewObjects;
                    Settings.Instance.LastSearchContent = content;
                    Settings.Instance.LastSearchIsCaseSenstive = dlg.IsCaseSenstive;
                    Settings.Instance.LastSearchIsObject = dlg.IsObject;
                    Settings.Instance.SearchHistories.Remove(content);
                    Settings.Instance.SearchHistories.Add(content);
                    rtbObjectScript.Text = string.Empty;
                    rtbObjectScript.Refresh();
                    SetSearchMode(true);
                    Task.Factory.StartNew(() =>
                        {
                            using (NewWait())
                            {
                                //triggers: http://dbscripter.codeplex.com/SourceControl/changeset/view/508aacd6909d?ProjectName=dbscripter#ObjectHelper%2fSQL%2fTriggers_9.sql
                                var triggers = Query(@"select 
	s.name as SchemaName,
	t.name,
	t.create_date AS CreateDate,
    t.modify_date AS ModifyDate,
    t.type
from sys.triggers t left join sys.objects o on o.object_id = t .parent_id
left join sys.objects o2 on o2.object_id=t.object_id
left join sys.schemas s on s.schema_id = o2.schema_id
left join sys.schemas s2 on s2.schema_id = o.schema_id
 where t.type='TR'", CurrentServerInfo);

                                var jobs = Query("SELECT job_id, name, date_created AS CreateDate, date_modified AS ModifyDate, 'Job' AS type FROM msdb.dbo.sysjobs WITH (NOLOCK)", CurrentServerInfo);
                                if (dlg.IsObject)
                                {
                                    var allObjects = GetObjects(string.Empty);
                                    allObjects.Merge(jobs);
                                    allObjects.Merge(triggers);
                                    var results = allObjects.AsEnumerable().Where(r => r["name"].ToString().IndexOf(content, compare) != -1);
                                    results.ForEach(r =>
                                    {
                                        var row = objects.NewRow();
                                        row[KeyName] = r[KeyName];
                                        row[KeyCreateDate] = r[KeyCreateDate];
                                        row[KeyModifyDate] = r[KeyModifyDate];
                                        row[KeyType] = GetObjectMode(r[KeyType] as string);
                                        row[KeyState] = il16.Images[3];
                                        if ((ObjectModes)row[KeyType] == ObjectModes.Job)
                                        {
                                            var id = r["job_id"].ToString();
                                            var job = QuerySet("exec msdb.dbo.sp_help_job '" + id + "'", DefaultServerInfo);
                                            var script = new StringBuilder();
                                            if (job != null && job.Tables.Count >= 4)
                                            {
                                                job.Tables[1].AsEnumerable().ForEach(step =>
                                                {
                                                    script.AppendFormat("--Step {0}: ({1}) {2}\r\n", step["step_id"].ToString(), step["subsystem"].ToString(), step["step_name"].ToString());
                                                    script.AppendLine(step["command"].ToString());
                                                    script.AppendLine();
                                                });
                                            }
                                            row[KeyPath] = script.ToString();
                                        }
                                        objects.Rows.Add(row);
                                    });
                                }
                                else
                                {
                                    var results = Query(string.Format(QueryEngine.SqlObjectScripts + " where [Text] like '%{0}%'", SafeSql(content)), CurrentServerInfo);
                                    jobs.AsEnumerable().ForEach(j =>
                                    {
                                        var id = j["job_id"].ToString();
                                        var job = QuerySet("exec msdb.dbo.sp_help_job '" + id + "'", DefaultServerInfo);
                                        if (job != null && job.Tables.Count >= 4)
                                        {
                                            var steps = job.Tables[1].AsEnumerable().Where(step => step["command"].ToString().IndexOf(content, compare) != -1);
                                            var jobSteps = steps.ToList();
                                            if (jobSteps.Count > 0)
                                            {
                                                var row = results.NewRow();
                                                //row[KeyState] = il16.Images[3];
                                                row[KeyName] = j[KeyName];
                                                row[KeyCreateDate] = j[KeyCreateDate];
                                                row[KeyModifyDate] = j[KeyModifyDate];
                                                var script = new StringBuilder();
                                                jobSteps.ForEach(s =>
                                                {
                                                    script.AppendFormat("--Step {0}: ({1}) {2}\r\n", s["step_id"].ToString(), s["subsystem"].ToString(), s["step_name"].ToString());
                                                    script.AppendLine(s["command"].ToString());
                                                    script.AppendLine();
                                                });
                                                row[KeyText] = script.ToString();
                                                row[KeyType] = j[KeyType];
                                                results.Rows.Add(row);
                                            }
                                        }
                                    });

                                    var triggerText = Query(string.Format(@"select 
	t.name,
	t.create_date AS CreateDate,
    t.modify_date AS ModifyDate,
    t.type,
    OBJECT_DEFINITION(t.object_id) AS text
from sys.triggers t left join sys.objects o on o.object_id = t .parent_id
left join sys.objects o2 on o2.object_id=t.object_id
left join sys.schemas s on s.schema_id = o2.schema_id
left join sys.schemas s2 on s2.schema_id = o.schema_id
 where OBJECT_DEFINITION(t.object_id) like '%{0}%'", SafeSql(content)), CurrentServerInfo);
                                    results.Merge(triggerText);
                                    results.AsEnumerable().ForEach(r =>
                                    {
                                        var exists = objects.Select(string.Format("{0} = '{1}'", KeyName, r[KeyName]));
                                        if (exists == null || exists.Length == 0)
                                        {
                                            var text = r[KeyText] as string;
                                            var index = text.IndexOf(content, compare);
                                            if (index != -1)
                                            {
                                                var row = objects.NewRow();
                                                //row[KeyState] = il16.Images[3];
                                                row[KeyName] = r[KeyName];
                                                row[KeyCreateDate] = r[KeyCreateDate];
                                                row[KeyModifyDate] = r[KeyModifyDate];
                                                var type = r[KeyType] as string;
                                                if (type != "Job")
                                                {
                                                    var prefix = index < ResultSamplePrefix ? 0 : index - ResultSamplePrefix;
                                                    text = text.Substring(prefix);
                                                    row[KeyPath] = text;
                                                }
                                                else
                                                    row[KeyPath] = r[KeyText];
                                                row[KeyType] = GetObjectMode(type);
                                                objects.Rows.Add(row);
                                            }
                                        }
                                    });
                                }
                                _currentObjectMode = ObjectModes.Objects;
                                this.Invoke(() =>
                                    {
                                        LoadObjects(objects);
                                        _isSearching = true;
                                        tcMain.SelectedTab = tpObjects;
                                        SetSearchMode(true);
                                    });
                            }
                        }).LogExceptions();
                }
            }
        }

        private void OnOpenTableClick(object sender, EventArgs e)
        {
            OpenTable();
        }

        private void OpenTable()
        {
            var node = tvObjects.SelectedNode;
            var database = node.Parent.Parent.Text;
            var table = node.Text;
            OpenTable(CurrentServerInfo, database, table);
        }

        private void OpenTable(ServerInfo server, string database, string table)
        {
            var key = server.Server + "." + database + "." + table;
            TabPage found = null;
            tcMain.TabPages.Cast<TabPage>().ForEach(t =>
                {
                    if (t.Tag != null)
                    {
                        if ((WorkModes)t.Tag == WorkModes.TableData)
                        {
                            var tableData = t.Controls[0] as UserTableData;
                            if (tableData.Key == key)
                                found = t;
                        }
                    }
                }
            );

            if (found == null)
            {
                var tabPage = new TabPage(table + "(" + database + ")");
                tabPage.Tag = WorkModes.TableData;
                var userData = new UserTableData(server, table);
                userData.Dock = DockStyle.Fill;
                tabPage.Controls.Add(userData);
                tcMain.TabPages.Add(tabPage);
                tcMain.SelectedTab = tabPage;
            }
            else
                tcMain.SelectedTab = found;
            AddRecentObject(server, database, table, RecentObjectTypes.EditData, KeyTable);
        }

        internal void AddRecentObject(ServerInfo server, string database, string objectName, RecentObjectTypes recentObjectType, string objectType)
        {
            var recentObject = new RecentObject { Server = server.Server, User = server.User, Database = database, ObjectName = objectName, RecentObjectType = recentObjectType, ObjectType = objectType };
            var match = Settings.Instance.RecentObjects.FirstOrDefault(o => o.Server.ToLower() == recentObject.Server.ToLower()
                && o.User.ToLower() == recentObject.User.ToLower()
                && o.Database.ToLower() == recentObject.Database.ToLower()
                && o.ObjectName.ToLower() == recentObject.ObjectName.ToLower()
                && o.RecentObjectType == recentObject.RecentObjectType);
            if (match != null)
                Settings.Instance.RecentObjects.Remove(match);
            Settings.Instance.RecentObjects.Insert(0, recentObject);
            ClearRecentObjects();
            LoadRecentObjects();
        }

        private void LoadRecentObjects()
        {
            var index = 0;
            Settings.Instance.RecentObjects.ForEach(o =>
            {
                var name = o.Server + (!string.IsNullOrEmpty(o.Server) ? " - " : string.Empty) + o.ObjectName;
                var item = new ToolStripMenuItem(name, Resources.Edit2, (s, e) =>
                {
                    var server = Settings.Instance.FindServer(o.Server, o.User);
                    switch (o.RecentObjectType)
                    {
                        case RecentObjectTypes.EditData:
                            if (server != null)
                                OpenTable(server, o.Database, o.ObjectName);
                            break;
                        case RecentObjectTypes.FilePath:
                            if (server != null)
                                NewQuery(server, o.Database, o.ObjectType, o.ObjectName);
                            break;
                        case RecentObjectTypes.Other:
                            if (server != null)
                                NewQuery(server, o.Database, o.ObjectType, o.ObjectName);
                            break;
                        default:
                            break;
                    }
                }, name);
                item.Tag = o;
                tbRecentObjects.DropDownItems.Insert(index, item);
                index++;
            });
        }

        private void OnNewQueryClick(object sender, EventArgs e)
        {
            var node = tvObjects.SelectedNode;
            var sql = string.Empty;
            var serverInfo = CurrentServerInfo;
            string objectName;
            string objectType;
            if (node != null)
            {
                var root = GetRootNode(node);
                serverInfo = root.Tag as ServerState;
                objectName = node.Text;
                objectType = (node.Tag as ServerState).Key;
            }
            else
            {
                objectName = string.Empty;
                objectType = string.Empty;
            }
            NewQuery(serverInfo, _currentDatabase, objectType, objectName);
        }

        private void NewQuery(ServerInfo server, string database, string objectType, string objectName)
        {
            _userQueryCount++;
            var tabPage = new TabPage("New Query " + _userQueryCount);
            tabPage.Tag = WorkModes.Query;
            var sql = string.Empty;
            if (!string.IsNullOrEmpty(objectName))
            {
                tabPage.Text = server.Server + "." + _currentDatabase + " " + tabPage.Text;
                switch (objectType)
                {
                    case KeyTable:
                    case KeyView:
                        sql = "SELECT * FROM " + objectName;
                        break;
                    case KeyFunction:
                        sql = "SELECT " + objectName + "()";
                        break;
                    case KeySp:
                        sql = "EXEC " + objectName;
                        break;
                    default:
                        if (File.Exists(objectName))
                            sql = File.ReadAllText(objectName);
                        break;
                }
                AddRecentObject(server, database, objectName, RecentObjectTypes.Other, objectType);
            }
            var serverInfo = server.Clone();
            serverInfo.Database = database;
            var userQuery = new UserQuery(_userQueryCount == 1 && string.IsNullOrEmpty(sql) ? Settings.Instance.LastQuery : sql, serverInfo);
            userQuery.Dock = DockStyle.Fill;
            tabPage.Controls.Add(userQuery);
            tcMain.TabPages.Add(tabPage);
            tcMain.SelectedTab = tabPage;
        }

        private void OnRestoreDatabaseClick(object sender, EventArgs e)
        {
            using (var dlg = new FileNameDialog("Restore Database", false, string.Empty))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    using (NewWait())
                    {
                        if (ShowQuestion(string.Format("Are you sure to restore database {0} from {1}?", dlg.ObjectName, dlg.FilePath)))
                        {
                            SqlHelper.ExecuteNonQuery(string.Format(@"
ALTER DATABASE {0} SET SINGLE_USER

RESTORE DATABASE {0}
	FROM  DISK = N'{1}'
WITH  
	FILE = 1,  
	REPLACE,
	STATS = 10

ALTER DATABASE {0} SET MULTI_USER", dlg.ObjectName, dlg.FilePath), DefaultServerInfo);
                            tcMain.SelectedTab = tpSummary;
                            RefreshData();
                        }
                    }
                }
            }
        }

        private void OnMainMouseDoubleClick(object sender, MouseEventArgs e)
        {
            CloseCurrentTabPage(e.Location);
        }

        private void CloseCurrentTabPage(Point location)
        {
            if (tcMain.SelectedIndex > (int)WorkModes.Options)
            {
                var rect = tcMain.GetTabRect(tcMain.SelectedIndex);
                if (rect.Contains(location))
                {
                    CloseCurrentTab();
                }
            }
        }

        private void CloseCurrentTab()
        {
            if (tcMain.SelectedTab != null && tcMain.SelectedTab.Tag != null)
            {
                var objectType = (WorkModes)tcMain.SelectedTab.Tag;
                switch (objectType)
                {
                    case WorkModes.Query:
                    case WorkModes.TableData:
                    case WorkModes.UserPerformance:
                        if (objectType == WorkModes.UserPerformance)
                        {
                            var performance = tcMain.SelectedTab.Controls[0] as Performance;
                            performance.RemovePerformanceItem();
                        }
                        tcMain.TabPages.Remove(tcMain.SelectedTab);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnObjectDependenciesClick(object sender, EventArgs e)
        {
            SetSearchMode(false);
            var objects = NewObjects;
            var objectName = tvObjects.SelectedNode.Text;
            var dependencies = Query("SELECT referencing_schema_name, referencing_entity_name FROM sys.dm_sql_referencing_entities('" + objectName + "', 'OBJECT')", CurrentServerInfo);
            var names = dependencies.AsEnumerable().Select(r => "'" + r["referencing_schema_name"] as string + "." + r["referencing_entity_name"] as string + "'");
            if (names.Count() > 0)
            {
                var filter = string.Join(",", names.ToArray());
                var results = Query("Select u.name + '.' + o.name AS Name, o.create_date AS CreateDate, o.modify_date AS ModifyDate, o.type from sys.objects o WITH (NOLOCK) LEFT JOIN sys.schemas u WITH (NOLOCK) ON o.schema_id = u.schema_id where u.name + '.' + o.name in (" + filter + ")", CurrentServerInfo);
                results.AsEnumerable().ForEach(r =>
                {
                    var row = objects.NewRow();
                    row[KeyName] = r[KeyName];
                    row[KeyCreateDate] = r[KeyCreateDate];
                    row[KeyModifyDate] = r[KeyModifyDate];
                    row[KeyType] = GetObjectMode(r[KeyType] as string);
                    objects.Rows.Add(row);
                });
            }
            _currentObjectMode = ObjectModes.Objects;
            LoadObjects(objects);
            tcMain.SelectedTab = tpObjects;
        }

        private void LoadObjects(DataTable objects)
        {
            this.Invoke(() =>
                {
                    cboObjectScriptVersions.DataSource = null;
                    SetCompareVersion();
                    rtbObjectScript.Text = string.Empty;
                    dgvObjects.DataSource = objects;
                });
        }

        private void SetCompareVersion()
        {
            this.Invoke(() =>
                {
                    cmdCompareObjectVersion.Enabled = cboObjectScriptVersions.Items.Count > 0;
                });
        }

        private DisposableState NewWait()
        {
            return new DisposableState(this, Commands);
        }

        private void OnAnalysisTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsHandleCreated)
                Analyze();
        }

        private void OnAnalysisCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                var direction = ListSortDirection.Ascending;
                if (e.ColumnIndex == _analysisPrevColIndex) // reverse sort order
                    direction = _analysisPrevSortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                // remember the column that was clicked and in which direction is ordered
                _analysisPrevColIndex = e.ColumnIndex;
                _analysisPrevSortDirection = direction;

                // set the column to be grouped
                dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns[e.ColumnIndex];

                dgvAnalysis.Sort(dgvAnalysis.Columns[e.ColumnIndex], direction);
            }
        }

        private void Analyze()
        {
            var type = (AnalysisTypes)cboAnalysisTypes.SelectedItem;
            switch (type)
            {
                case AnalysisTypes.TablesSpace:
                case AnalysisTypes.IndexUsage:
                case AnalysisTypes.LogicFault:
                    if (string.IsNullOrEmpty(_currentDatabase))
                    {
                        ShowMessage("Please select a database to analyze.");
                        return;
                    }
                    break;
            }

            Task.Factory.StartNew(() =>
                {
                    using (NewWait())
                    {
                        var sql = string.Empty;
                        DataTable data = null;
                        switch (type)
                        {
                            case AnalysisTypes.DatabasesSpace:
                            case AnalysisTypes.TablesSpace:
                            case AnalysisTypes.Performance:
                            case AnalysisTypes.IndexUsage:
                            case AnalysisTypes.LogicFault:
                                var analysisResult = new List<AnalysisResult>();
                                switch (type)
                                {
                                    case AnalysisTypes.IndexUsage:
                                        //unusded index
                                        var indexSql = @"SELECT OBJECT_SCHEMA_NAME(I.OBJECT_ID) AS SchemaName,
OBJECT_NAME(I.OBJECT_ID) AS ObjectName,
I.NAME AS IndexName 
FROM sys.indexes I 
WHERE -- only get indexes for user created tables
OBJECTPROPERTY(I.OBJECT_ID, 'IsUserTable') = 1 
-- find all indexes that exists but are NOT used
AND NOT EXISTS ( 
SELECT index_id 
FROM sys.dm_db_index_usage_stats
WHERE OBJECT_ID = I.OBJECT_ID 
AND I.index_id = index_id 
-- limit our query only for the current db
AND database_id = DB_ID()
AND (user_seeks >0 OR user_scans > 0)
) 
and I.is_primary_key = 0 and I.is_unique = 0
AND I.NAME IS NOT NULL
ORDER BY SchemaName, ObjectName, IndexName";
                                        var indexUsage = Query(indexSql, CurrentServerInfo);
                                        indexUsage.AsEnumerable().ForEach(i =>
                                        {
                                            var indexName = i["IndexName"].ToString();
                                            var objectName = i["ObjectName"].ToString();
                                            var schemaName = i["SchemaName"].ToString();

                                            analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.TableIndexUsage, ObjectName = string.Format("{0}.{1}.{2}", schemaName, objectName, indexName), ReferenceValue = 0, CurrentValue = 0, Factor = "0", Key = 0 });
                                        });

                                        //missing index
                                        indexSql = @"SELECT  D.statement AS ObjectName, column_name, column_usage
FROM    sys.dm_db_missing_index_groups G
        JOIN sys.dm_db_missing_index_group_stats GS ON G.index_group_handle = GS.group_handle
        JOIN sys.dm_db_missing_index_details D ON G.index_handle = D.index_handle
        CROSS APPLY sys.dm_db_missing_index_columns (D.index_handle) DC
ORDER BY D.index_handle, D.statement";
                                        indexUsage = Query(indexSql, CurrentServerInfo);
                                        indexUsage.AsEnumerable().GroupBy(i => i.Field<string>("ObjectName")).ForEach(i =>
                                        {
                                            var columns = new List<string>();
                                            i.ForEach(r =>
                                                {
                                                    var column = r["column_name"].ToString();
                                                    if (!columns.Contains(column))
                                                        columns.Add(column);
                                                });
                                            analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.TableIndexUsage, ObjectName = i.Key, ReferenceValue = 0, CurrentValue = 0, Factor = "0", Key = 1, RuntimeValue = string.Join(",", columns) });
                                        });
                                        break;
                                    case AnalysisTypes.DatabasesSpace:
                                        //database & disk free space
                                        var spaces = QueryEngine.GetDiskSpace(DefaultServerInfo);
                                        spaces.ForEach(s =>
                                        {
                                            if (s.Value.Key < s.Value.Value / 100 * Settings.Instance.DatabaseDiskFreeSpaceRatio)
                                            {
                                                analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.DiskFreeSpace, ObjectName = s.Key, ReferenceValue = s.Value.Key, CurrentValue = s.Value.Value, Factor = Settings.Instance.DatabaseDiskFreeSpaceRatio + SizePercentage });
                                            }
                                        });
                                        var dbLogSpaces = QueryEngine.GetDbLogSpace(DefaultServerInfo);
                                        dbLogSpaces.ForEach(s =>
                                            {
                                                analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.DatabaseLogSpace, ObjectName = s.Key, ReferenceValue = s.Value.Item1, CurrentValue = s.Value.Item2, Factor = Settings.Instance.DatabaseDataLogSpaceRatio + SizePercentage, Key = s.Value.Item3 ? 1 : 0 });
                                            });
                                        break;
                                    case AnalysisTypes.TablesSpace:
                                        //sub rule 1: data/index space ratio
                                        //sub rule 2: db / table space ratio
                                        //sub rule 3: index efficency
                                        //consider non-clustered index count & index unused space
                                        var tables = GetObjects(KeyTables);
                                        tables.AsEnumerable().ForEach(t =>
                                            {
                                                var name = t[KeyName].ToString();
                                                var space = Query(string.Format("EXEC sp_spaceused '{0}'", name), CurrentServerInfo);
                                                if (space.Rows.Count > 0)
                                                {
                                                    var row = space.Rows[0];
                                                    var dataSize = ToKb(row["data"]) / Utils.Size1K;
                                                    var indexSize = ToKb(row["index_size"]) / Utils.Size1K;
                                                    if (indexSize > dataSize / 100 * Settings.Instance.TableDataIndexSpaceRatio)
                                                        analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.TableIndexSpace, ObjectName = name, ReferenceValue = dataSize, CurrentValue = indexSize, Factor = Settings.Instance.DatabaseDataLogSpaceRatio + SizePercentage, Key = (int)TableIndexSpaceRules.DataIndexSpaceRatio });
                                                }
                                            });
                                        break;
                                    case AnalysisTypes.Performance:
                                        var databaseStalls = QueryEngine.GetDatabaseStall(DefaultServerInfo);
                                        databaseStalls.ForEach(db =>
                                            {
                                                var runtime = string.Format("Higher stall (in millisecond) means worse database performance.\r\n\r\nDB Read Stall: {0},\r\nDB Write Stall: {1}, \r\nLog Read Stall: {2}, \r\nLog Write Stall: {3}", db.DbReadStall, db.DbWriteStall, db.LogReadStall, db.LogwriteStall);
                                                analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.Performance, ObjectName = db.Database, ReferenceValue = QueryEngine.DbStallThreshold, CurrentValue = db.Max, Factor = "", Key = db.IsExceeded ? 1 : 0, RuntimeValue = runtime });
                                            });
                                        break;
                                    case AnalysisTypes.LogicFault:
                                        var spScripts = QueryEngine.GetSpScripts(CurrentServerInfo);
                                        spScripts.AsEnumerable().ForEach(s =>
                                            {
                                                var script = s.Field<string>("text");
                                                var lines = script.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                                var foundComment = false;
                                                var beginTransactionCount = 0;
                                                var transactionPairCount = 0;

                                                var openCursor = "open ";
                                                var closeCursor = "close ";
                                                var deallocateCursor = "deallocate ";
                                                var cursorInstances = new Dictionary<string, KeyValue<int, int>>();

                                                var createTempTable = "create table #";
                                                var dropTempTable = "drop table #";
                                                string objectName;
                                                var tempTableInstances = new Dictionary<string, bool>();

                                                var lineIndex = 0;
                                                lines.Select(l => l.Trim().RemoveSpace().ToLower()).ForEach(l =>
                                                    {
                                                        lineIndex++;
                                                        if (l.StartsWith(Utils.MultiCommentStart) && !l.EndsWith(Utils.MultiCommentEnd))
                                                            foundComment = true;
                                                        else if (l.StartsWith(Utils.MultiCommentEnd)
                                                            || (!l.StartsWith(Utils.MultiCommentStart) && l.EndsWith(Utils.MultiCommentEnd)))
                                                            foundComment = false;
                                                        if (!foundComment)
                                                        {
                                                            if (!l.StartsWith(Utils.SingleCommentStart))
                                                            {
                                                                if (l.StartsWith("begin tran"))
                                                                    beginTransactionCount++;
                                                                else if (l.StartsWith("commit")
                                                                    || l.StartsWith("rollback"))
                                                                    transactionPairCount++;

                                                                    //found open cursor
                                                                else if (l.StartsWith(openCursor))
                                                                {
                                                                    objectName = l.Substring(openCursor.Length - 1).ParseObjectName();
                                                                    //first occurrence
                                                                    if (!cursorInstances.ContainsKey(objectName))
                                                                    {
                                                                        //put close/deallocate pending counter
                                                                        cursorInstances.Add(objectName, new KeyValue<int, int>(1, 1));
                                                                    }
                                                                    //another occurrence
                                                                    else
                                                                    {
                                                                        //increase close/deallocate pending counter
                                                                        cursorInstances[objectName].Key++;
                                                                        cursorInstances[objectName].Value++;
                                                                    }
                                                                }
                                                                else if (l.StartsWith(closeCursor))
                                                                {
                                                                    objectName = l.Substring(closeCursor.Length - 1).ParseObjectName();
                                                                    if (cursorInstances.ContainsKey(objectName))
                                                                        cursorInstances[objectName].Key--;
                                                                }
                                                                else if (l.StartsWith(deallocateCursor))
                                                                {
                                                                    objectName = l.Substring(deallocateCursor.Length - 1).ParseObjectName();
                                                                    if (cursorInstances.ContainsKey(objectName))
                                                                        cursorInstances[objectName].Value--;
                                                                }

                                                                else if (l.StartsWith(createTempTable))
                                                                {
                                                                    objectName = l.Substring(createTempTable.Length - 1).ParseObjectName();
                                                                    if (!tempTableInstances.ContainsKey(objectName))
                                                                        tempTableInstances.Add(objectName, true);
                                                                }
                                                                else if (l.StartsWith(dropTempTable))
                                                                {
                                                                    objectName = l.Substring(dropTempTable.Length - 1).ParseObjectName();
                                                                    if (tempTableInstances.ContainsKey(objectName))
                                                                        tempTableInstances[objectName] = false;
                                                                }
                                                            }
                                                        }
                                                    });

                                                var name = s.Field<string>("name");

                                                //rule 1: cursor
                                                //"OPEN " pair with "CLOSE " and "DEALLOCATE "
                                                cursorInstances.Where(t => t.Value.Key > 0 || t.Value.Value > 0).ForEach(t =>
                                                {
                                                    var suggestion = string.Format("opened cursor {0} has pending close or deallocate", t.Key);
                                                    analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.Fault, ObjectName = name, ReferenceValue = 0, CurrentValue = 0, Factor = "", Key = 1, RuntimeValue = suggestion });
                                                });

                                                //rule 2: transaction
                                                //"commit" or "rollback count is less than "begin tran" count
                                                if (transactionPairCount < beginTransactionCount)
                                                {
                                                    var suggestion = string.Format("commit or rollback count is less than begin tran count\r\nbegin tran count: {0}\r\ncommit or roll back count: {1}", beginTransactionCount, transactionPairCount);
                                                    analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.Fault, ObjectName = name, ReferenceValue = 0, CurrentValue = 0, Factor = "", Key = 2, RuntimeValue = suggestion });
                                                }

                                                //rule 3: create / drop temp table
                                                //"create table #" pair with "drop table #"
                                                var tableSuggestion = new StringBuilder();
                                                var instances = tempTableInstances.Where(t => t.Value);
                                                if (instances.Any())
                                                {
                                                    tableSuggestion.AppendLine("create temporary table count is not matching with drop temporary table count");
                                                    instances.ForEach(t =>
                                                    {
                                                        tableSuggestion.AppendLine(string.Format("table name: {0}", t.Key));
                                                    });
                                                    analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.Fault, ObjectName = name, ReferenceValue = 0, CurrentValue = 0, Factor = "", Key = 3, RuntimeValue = tableSuggestion.ToString() });
                                                }
                                            });
                                        break;
                                    default:
                                        break;
                                }

                                data = new DataTable();
                                data.Columns.Add(AnalysisColumnRule, typeof(string));
                                data.Columns.Add(AnalysisColumnObject, typeof(string));
                                data.Columns.Add(AnalysisColumnReference, typeof(string));
                                data.Columns.Add(AnalysisColumnCurrent, typeof(string));
                                data.Columns.Add(AnalysisColumnFactor, typeof(string));
                                data.Columns.Add(AnalysisColumnSuggestion, typeof(string));

                                if (analysisResult.Count == 0)
                                    analysisResult.Add(new AnalysisResult { ResultType = AnalysisResultTypes.None });

                                analysisResult.ForEach(a =>
                                    {
                                        var row = data.NewRow();
                                        row[AnalysisColumnObject] = a.ObjectName;
                                        switch (a.ResultType)
                                        {
                                            case AnalysisResultTypes.DiskFreeSpace:
                                            case AnalysisResultTypes.DatabaseLogSpace:
                                            case AnalysisResultTypes.TableIndexSpace:
                                                row[AnalysisColumnFactor] = a.Factor;
                                                row[AnalysisColumnReference] = a.ReferenceValue + " " + Utils.SizeMb;
                                                row[AnalysisColumnCurrent] = a.CurrentValue + " " + Utils.SizeMb;
                                                break;
                                            case AnalysisResultTypes.TableIndexUsage:
                                                break;
                                            case AnalysisResultTypes.Fault:
                                                break;
                                            case AnalysisResultTypes.Performance:
                                                switch (a.Key)
                                                {
                                                    case 0:
                                                    case 1:
                                                        row[AnalysisColumnReference] = a.ReferenceValue + " " + Utils.TimeMs;
                                                        row[AnalysisColumnCurrent] = a.CurrentValue + " " + Utils.TimeMs;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                break;
                                            case AnalysisResultTypes.None:
                                                break;
                                            default:
                                                break;
                                        }

                                        switch (a.ResultType)
                                        {
                                            case AnalysisResultTypes.DiskFreeSpace:
                                                row[AnalysisColumnObject] += @":\";
                                                row[AnalysisColumnRule] = "Disk Free Space";
                                                row[AnalysisColumnSuggestion] = "Increase disk space";
                                                break;
                                            case AnalysisResultTypes.DatabaseLogSpace:
                                                row[AnalysisColumnRule] = "Database Data/Log Space";

                                                if (a.Key == 0)
                                                    row[AnalysisColumnSuggestion] = "Truncate log";
                                                else
                                                    row[AnalysisColumnSuggestion] = "Shrink log";
                                                break;
                                            case AnalysisResultTypes.TableIndexSpace:
                                                row[AnalysisColumnRule] = "Table Data/Index Space";
                                                row[AnalysisColumnSuggestion] = "Optimize index";
                                                break;
                                            case AnalysisResultTypes.TableIndexUsage:
                                                row[AnalysisColumnRule] = "Table Index Usage";
                                                if (a.Key == 0)
                                                    row[AnalysisColumnSuggestion] = "Drop unused index";
                                                else if (a.Key == 1)
                                                    row[AnalysisColumnSuggestion] = "Create index for " + a.RuntimeValue;
                                                break;
                                            case AnalysisResultTypes.Fault:
                                                row[AnalysisColumnRule] = "Logic Fault";
                                                row[AnalysisColumnSuggestion] = a.RuntimeValue;
                                                break;
                                            case AnalysisResultTypes.Performance:
                                                row[AnalysisColumnRule] = "Database Stall";
                                                if (a.Key == 1)
                                                    row[AnalysisColumnSuggestion] = "Consider improve hard disk performance by separating database files / log files into different hard disks.\r\n" + a.RuntimeValue;
                                                else
                                                    row[AnalysisColumnSuggestion] = a.RuntimeValue;
                                                break;
                                            case AnalysisResultTypes.None:
                                                row[AnalysisColumnRule] = "(Analysis finished.)";
                                                break;
                                            default:
                                                break;
                                        }
                                        data.Rows.Add(row);
                                    });
                                break;
                            case AnalysisTypes.WaitingTasks:
                            case AnalysisTypes.Cpu:
                            case AnalysisTypes.ExecutionCount:
                            case AnalysisTypes.Io:
                            case AnalysisTypes.LockedObjects:
                                switch (type)
                                {
                                    case AnalysisTypes.LockedObjects:
                                        sql = QueryEngine.SqlLockedObjects;
                                        break;
                                    case AnalysisTypes.WaitingTasks:
                                        sql = QueryEngine.SqlWaitingTasks;
                                        break;
                                    case AnalysisTypes.ExecutionCount:
                                        sql = @"SELECT TOP 20 getdate() as [Log Time], 
        qs.execution_count AS [Execution Count],qs.plan_generation_num AS [Plan Generation Num],
        SUBSTRING(qt.text,qs.statement_start_offset/2, 
                  (case when qs.statement_end_offset = -1 
                  then len(convert(nvarchar(max), qt.text)) * 2 
                  else qs.statement_end_offset end -qs.statement_start_offset)/2) 
            as [Query Text],
            qt.dbid AS [DB Id], d.name AS [DB Name],
            qt.objectid AS [Object Id] 
FROM sys.dm_exec_query_stats qs
cross apply sys.dm_exec_sql_text(qs.sql_handle) as qt
left join sys.databases d on qt.dbid = d.database_id
ORDER BY qs.execution_count DESC";
                                        break;
                                    case AnalysisTypes.Io:
                                        sql = @"select top 20 getdate() as [Log Time]
    ,       creation_time AS [Creation time]
    ,       last_execution_time AS [Last Execution Time]
    ,       case when sql_handle IS NULL
                    then ' '
                    else ( substring(st.text,(qs.statement_start_offset+2)/2,
                (case when qs.statement_end_offset = -1        
                    then len(convert(nvarchar(MAX),st.text))*2      
                    else qs.statement_end_offset    
                end - qs.statement_start_offset) /2  ) )
            end as [Query Text]
    ,       (total_worker_time+0.0)/1000 as [Total Worker Time]
    ,       (total_worker_time+0.0)/(execution_count*1000) as [Avg CPU Time]
    ,       total_logical_reads as [Logical Reads]
    ,       total_logical_writes as [Logical Writes]
    ,       execution_count AS [Execution Count]
    ,       total_logical_reads+total_logical_writes as [Total IO]
    ,       (total_logical_reads+total_logical_writes)/(execution_count+0.0) as [Avg IO]
    ,       qp.value as [DB ID]
    ,       d.name as [DB Name]
    ,       st.objectid as [Object Id]
    from sys.dm_exec_query_stats qs
    cross apply sys.dm_exec_sql_text(sql_handle) st
    cross apply sys.dm_exec_plan_attributes(plan_handle) as qp
    left join sys.databases d on qp.value = d.database_id
    where total_logical_reads+total_logical_writes > 0 
    and qp.attribute = 'dbid' 
    order by [Total IO] desc";
                                        break;
                                    case AnalysisTypes.Cpu:
                                        sql = @"select 
    highest_cpu_queries.last_execution_time AS [Last Execution Time],
    highest_cpu_queries.execution_count AS [Execution Count],
    highest_cpu_queries.total_worker_time AS [Total Worker Time],
    q.[text] AS [Query Text],
    qp.value As [DB ID],
    d.name as [DB Name],
    q.objectid AS [Object Id],
    q.number AS Number,
    q.encrypted AS [Encrypted]
    -- highest_cpu_queries.plan_handle
from 
    (select top 20 
        qs.last_execution_time,
        qs.execution_count,
        qs.plan_handle, 
        qs.total_worker_time
    from 
        sys.dm_exec_query_stats qs
    order by qs.total_worker_time desc) as highest_cpu_queries
    cross apply sys.dm_exec_sql_text(plan_handle) as q
    cross apply sys.dm_exec_plan_attributes(plan_handle) as qp
    left join sys.databases d on qp.value = d.database_id
WHERE DATEDIFF(hour, last_execution_time, getdate()) < 1 -- change hour time frame 
and qp.attribute = 'dbid' 
order by highest_cpu_queries.total_worker_time desc";
                                        break;
                                    default:
                                        break;
                                }
                                data = Query(sql);
                                break;
                        }

                        this.Invoke(() =>
                            {
                                rtbAnalysisSQL.Text = string.Empty;
                                rtbAnalysisSQL.Refresh();

                                //dgvAnalysis.BindData(data, null);

                                dgvAnalysis.ClearGroups();
                                dgvAnalysis.Columns.Clear();
                                data.Columns.Cast<DataColumn>().ForEach(c =>
                                    {
                                        dgvAnalysis.Columns.Add(c.ColumnName, c.Caption);
                                    });
                                data.Rows.Cast<DataRow>().ForEach(r =>
                                    {
                                        var row = new OutlookGridRow();
                                        row.CreateCells(dgvAnalysis, r.ItemArray);
                                        dgvAnalysis.Rows.Add(row);
                                    });
                                switch (type)
                                {
                                    case AnalysisTypes.DatabasesSpace:
                                    case AnalysisTypes.TablesSpace:
                                    case AnalysisTypes.IndexUsage:
                                    case AnalysisTypes.Performance:
                                    case AnalysisTypes.LogicFault:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["Rule"];
                                        break;
                                    case AnalysisTypes.LockedObjects:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["SPID"];
                                        break;
                                    case AnalysisTypes.WaitingTasks:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["[Wait Type]"];
                                        break;
                                    case AnalysisTypes.ExecutionCount:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["DB Name"];
                                        break;
                                    case AnalysisTypes.Io:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["[DB Name]"];
                                        break;
                                    case AnalysisTypes.Cpu:
                                        dgvAnalysis.GroupTemplate.Column = dgvAnalysis.Columns["[DB Name]"];
                                        break;
                                    default:
                                        break;
                                }
                                dgvAnalysis.Sort(dgvAnalysis.GroupTemplate.Column, ListSortDirection.Descending);
                            });
                    }
                }).LogExceptions();
        }

        private void OnChooseFontClick(object sender, EventArgs e)
        {
            using (var dlg = new FontDialog())
            {
                dlg.Font = SetFont();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Instance.EditorFont = new SerializableFont { Name = dlg.Font.Name, Size = dlg.Font.Size, Bold = dlg.Font.Bold };
                    UpdateFont();
                }
            }
        }

        internal Font SetFont()
        {
            return new Font(Settings.Instance.EditorFont.Name, Settings.Instance.EditorFont.Size, Settings.Instance.EditorFont.Bold ? FontStyle.Bold : FontStyle.Regular);
        }

        private void UpdateFont()
        {
            cmdChooseFont.Font = SetFont();
            foreach (var item in tcMain.TabPages)
            {
                var tabPage = item as TabPage;
                UpdateFont(tabPage.Controls);
            }
        }

        private void UpdateFont(Control.ControlCollection controls)
        {
            foreach (Control item in controls)
            {
                if (item.HasChildren)
                    UpdateFont(item.Controls);
                else if (item is TextEditorControl)
                    (item as TextEditorControl).Font = SetFont();
            }
        }

        private void OnAnalysisRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var text = string.Empty;
            if (e.RowIndex >= 0)
            {
                try
                {
                    switch ((AnalysisTypes)cboAnalysisTypes.SelectedItem)
                    {
                        case AnalysisTypes.DatabasesSpace:
                        case AnalysisTypes.TablesSpace:
                        case AnalysisTypes.IndexUsage:
                        case AnalysisTypes.Performance:
                        case AnalysisTypes.LogicFault:
                            text = dgvAnalysis.Rows[e.RowIndex].Cells["Suggestion"].Value.ToString();
                            break;
                        case AnalysisTypes.LockedObjects:
                            var spid = dgvAnalysis.Rows[e.RowIndex].Cells["SPID"].Value;
                            if (spid != null && !string.IsNullOrEmpty(spid.ToString()))
                                text = QueryEngine.GetSessionSql(spid.ToString(), DefaultServerInfo); break;
                        case AnalysisTypes.WaitingTasks:
                            var id = dgvAnalysis.Rows[e.RowIndex].Cells["Session Id"].Value;
                            if (id != null && !string.IsNullOrEmpty(id.ToString()))
                                text = QueryEngine.GetSessionSql(id.ToString(), DefaultServerInfo);
                            break;
                        case AnalysisTypes.ExecutionCount:
                        case AnalysisTypes.Io:
                        case AnalysisTypes.Cpu:
                            text = dgvAnalysis.Rows[e.RowIndex].Cells[3].Value as string;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
            rtbAnalysisSQL.Text = text;
            rtbAnalysisSQL.Refresh();
        }

        private void OnObjectsBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var tag = e.Node.Tag as ServerState;
            switch (tag.Key)
            {
                case KeySp:
                case KeyTable:
                case KeyFunction:
                case KeyDatabase:
                    break;
                default:
                    e.CancelEdit = true;
                    break;
            }
        }

        private void OnRegisterServerClick(object sender, EventArgs e)
        {
            EditConnection(null);
        }

        private void EditConnection(ServerInfo info)
        {
            using (var dlg = new ConnectionDialog(info))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var server = dlg.Server;
                    ServerInfo item;
                    if (info == null)
                        item = Settings.Instance.FindServer(server, dlg.UserName);
                    else
                        item = Settings.Instance.FindServer(info.Server, dlg.UserName);
                    var isNew = false;
                    if (item == null)
                    {
                        item = new ServerInfo();
                        Settings.Instance.Servers.Add(item);
                        isNew = true;
                    }
                    item.AuthType = dlg.AuthType;
                    item.Server = server;
                    item.Password = dlg.Password;
                    item.User = dlg.UserName;
                    Settings.Instance.Save();
                    if (info != null)
                    {
                        info.AuthType = item.AuthType;
                        info.Password = item.Password;
                        info.Server = item.Server;
                        info.User = item.User;
                    }
                    if (isNew)
                        LoadServer(item);
                    else if (info == null)
                        ShowMessage(string.Format("Server [{0}] already exists", item.Server));
                    else
                        tvObjects.SelectedNode.Text = item.Server;
                }
            }
        }

        private void OnRemoveServerClick(object sender, EventArgs e)
        {
            if (ShowQuestion("Are you sure to remove the sql server connection?"))
            {
                var item = Settings.Instance.FindServer(tvObjects.SelectedNode.Text);
                if (item != null)
                {
                    tcbRefreshActivitiesIntervals.SelectedIndex = 0;
                    Settings.Instance.Servers.Remove(item);
                    Settings.Instance.Save();
                    tvObjects.Nodes.Remove(tvObjects.SelectedNode);
                    _currentServerInfo = null;
                    _currentDatabase = string.Empty;
                    _currentObjectMode = ObjectModes.None;
                    ResetPerformance();
                }
            }
        }

        private void OnEditServerClick(object sender, EventArgs e)
        {
            var server = tvObjects.SelectedNode.Tag as ServerState;
            EditConnection(server);
        }

        private void OnAlertTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            var alertType = (AlertTypes)cboAlertTypes.SelectedItem;
            var targetEnable = false;
            cboAlertConditionTypes.Items.Clear();
            cboAlertCondictionValues.Items.Clear();
            switch (alertType)
            {
                case AlertTypes.Sql:
                    cboAlertConditionTypes.Items.Add("Executes");
                    cboAlertConditionTypes.Items.Add("Lasts");
                    cboAlertConditionTypes.Items.Add("Blocked");
                    cboAlertConditionTypes.Items.Add("Table Is Empty");
                    targetEnable = true;
                    break;
                case AlertTypes.Server:
                    cboAlertConditionTypes.Items.Add("Unavailable");
                    break;
                //case AlertTypes.CPU:
                //    cboConditionTypes.Items.Add("Higher than(%)");
                //    cboCondictionValues.Items.AddRange(Enumerable.Range(1, 99).Where(n => n % 5 == 0).Cast<object>().ToArray());
                //    break;
                //case AlertTypes.Memory:
                //    cboConditionTypes.Items.Add("Lower than(MB)");
                //    break;
                //case AlertTypes.Diskspace://todo:which disk?
                //    break;
                default:
                    break;
            }
            if (cboAlertConditionTypes.Items.Count > 0)
                cboAlertConditionTypes.SelectedIndex = 0;
            txtAlertTarget.Enabled = targetEnable;
        }

        private void OnSaveMonitorItemClick(object sender, EventArgs e)
        {
            epHint.Clear();
            var server = (ServerInfo)cboAlertConnections.SelectedItem;
            if (server != null)
            {
                var alertType = (AlertTypes)cboAlertTypes.SelectedItem;
                var isNew = false;
                MonitorItem item;
                if (_currentMonitorItem != null)
                    item = _currentMonitorItem;
                else
                {
                    item = new MonitorItem();
                    isNew = true;
                }
                item.Server = server.Server;
                item.AlertType = alertType;
                item.AlertMethod = (AlertMethods)cboAlertMethods.SelectedItem;
                item.CondictionType = cboAlertConditionTypes.SelectedIndex;
                item.CondictionValue = cboAlertCondictionValues.Text;
                item.Target = txtAlertTarget.Text;
                item.Title = cboAlertTitle.Text;
                item.IsEnabled = chkEnableAlert.Checked;
                switch (alertType)
                {
                    case AlertTypes.Sql:
                        break;
                    case AlertTypes.Server:
                        break;
                    //case AlertTypes.CPU:
                    //case AlertTypes.Memory:
                    //    break;
                    //case AlertTypes.Diskspace:
                    //    break;
                    default:
                        break;
                }
                //currentMonitorItem = null;
                bool refresh;
                if (isNew)
                {
                    var duplicate = Settings.Instance.MonitorItems.FirstOrDefault(m => m.Server.ToLower() == item.Server.ToLower()
                        && m.AlertType == item.AlertType
                        && m.Target == item.Target
                        && m.CondictionType == item.CondictionType
                        && m.CondictionValue == item.CondictionValue);
                    if (duplicate == null)
                    {
                        Settings.Instance.MonitorItems.Add(item);
                        refresh = true;
                    }
                    else
                        refresh = false;
                }
                else
                    refresh = true;
                if (refresh)
                    LoadMonitorItems();
            }
            else
                epHint.SetError(cboAlertConnections, "Please select connection.");
        }

        private void OnCondictionTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            var alertType = (AlertTypes)cboAlertTypes.SelectedItem;
            var enableValue = true;
            var metrict = string.Empty;
            switch (alertType)
            {
                case AlertTypes.Sql:
                    switch (cboAlertConditionTypes.SelectedIndex)
                    {
                        case 0:
                        case 2:
                            enableValue = false;
                            break;
                        case 1:
                            metrict = "Seconds";
                            break;
                        default:
                            break;
                    }
                    break;
                case AlertTypes.Server:
                    enableValue = false;
                    break;
                //case AlertTypes.CPU:
                //    break;
                //case AlertTypes.Memory:
                //    break;
                //case AlertTypes.Diskspace:
                //    break;
                default:
                    break;
            }
            cboAlertCondictionValues.Enabled = enableValue;
            lblAlertMetrict.Text = metrict;
        }

        private void OnMonitorItemsCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditMonitorItem();
        }

        private void EditMonitorItem()
        {
            var rows = dgvMonitorItems.SelectedRows;
            if (rows.Count > 0)
            {
                var item = rows[0].DataBoundItem as MonitorItem;
                cboAlertTypes.SelectedItem = item.AlertType;
                cboAlertMethods.SelectedItem = item.AlertMethod;
                cboAlertConditionTypes.SelectedIndex = item.CondictionType;
                cboAlertCondictionValues.Text = item.CondictionValue.ToString();
                txtAlertTarget.Text = item.Target;
                cboAlertTitle.Text = item.Title;
                chkEnableAlert.Checked = item.IsEnabled;
                _currentMonitorItem = item;
                var server = Settings.Instance.FindServer(item.Server);
                if (server != null)
                    cboAlertConnections.SelectedItem = server;
            }
        }

        private void OnAnalysisCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch ((AnalysisTypes)cboAnalysisTypes.SelectedItem)
            {
                case AnalysisTypes.LockedObjects:
                    break;
                case AnalysisTypes.WaitingTasks:
                    if (dgvAnalysis.Columns[e.ColumnIndex].Name == "[Blocking Session Id]")
                    {
                        var row = dgvAnalysis.Rows[e.RowIndex];
                        var blockingId = row.Cells[e.ColumnIndex].Value;
                        if (blockingId != null)
                        {
                            var id = blockingId.ToString();
                            if (!string.IsNullOrEmpty(id))
                                row.DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                    break;
                case AnalysisTypes.ExecutionCount:
                    break;
                case AnalysisTypes.Io:
                    break;
                case AnalysisTypes.Cpu:
                    break;
                default:
                    break;
            }
        }

        private void OnAlertConnectionsDropDown(object sender, EventArgs e)
        {
            FillServers();
        }

        private void FillServers()
        {
            var item = cboAlertConnections.SelectedItem;
            cboAlertConnections.Items.Clear();
            Settings.Instance.Servers.ForEach(s =>
            {
                cboAlertConnections.Items.Add(s);
            });
            if (item != null)
            {
                if (cboAlertConnections.Items.Contains(item))
                    cboAlertConnections.SelectedItem = item;
            }
        }

        private void OnEditMonitorItemClick(object sender, EventArgs e)
        {
            EditMonitorItem();
        }

        private void OnDeleteMonitorItemClick(object sender, EventArgs e)
        {
            _currentMonitorItem = null;
            dgvMonitorItems.SelectedRows.Cast<DataGridViewRow>().ForEach(m =>
                {
                    var monitorItem = (MonitorItem)m.DataBoundItem;
                    if (Settings.Instance.MonitorItems.Contains(monitorItem))
                        Settings.Instance.MonitorItems.Remove(monitorItem);
                });
            LoadMonitorItems();
        }

        private void OnObjectsNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var serverInfo = tvObjects.SelectedNode.Tag as ServerState;//e.Node.Tag as ServerState;
                switch (serverInfo.Key)
                {
                    case KeyTable:
                        OpenTable();
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnNewMonitorItemClick(object sender, EventArgs e)
        {
            _currentMonitorItem = null;
            cboAlertTypes.SelectedIndex = 0;
            txtAlertTarget.Text = string.Empty;
            cboAlertConditionTypes.SelectedIndex = 0;
            cboAlertCondictionValues.Text = string.Empty;
            cboAlertTitle.SelectedIndex = 0;
            cboAlertMethods.SelectedIndex = 0;
            chkEnableAlert.Checked = true;
        }

        private void OnTruncateTableClick(object sender, EventArgs e)
        {
            var table = tvObjects.SelectedNode.Text;
            if (ShowQuestion(string.Format("Truncating the table [{0}] will lose all the data. Are you sure to proceed?", table)))
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(string.Format("TRUNCATE TABLE [{0}]", table), CurrentServerInfo);
                    ShowMessage(string.Format("Table [{0}] truncated", table));
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void OnCheckDbClick(object sender, EventArgs e)
        {
            var db = _currentDatabase;
            try
            {
                var result = SqlHelper.ExecuteNonQuery(string.Format("DBCC CHECKDB ('{0}')", db), CurrentServerInfo);
                ShowResult("Check Database Result", result);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ShowResult(string title, string content)
        {
            using (var dlg = new ViewTextDialog(title, content))
            {
                dlg.ShowDialog(this);
            }
        }

        private void OnCleanTableClick(object sender, EventArgs e)
        {
            var table = tvObjects.SelectedNode.Text;
            try
            {
                var result = SqlHelper.ExecuteNonQuery(string.Format("DBCC CLEANTABLE ('{0}', '{1}')", _currentDatabase, table), CurrentServerInfo);
                ShowResult("Clean Table Result", result);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void OnTableIndexDefragClick(object sender, EventArgs e)
        {
            var table = tvObjects.SelectedNode.Text;
            try
            {
                var result = SqlHelper.ExecuteNonQuery(string.Format("DBCC INDEXDEFRAG ('{0}', '{1}')", _currentDatabase, table), CurrentServerInfo);
                ShowResult("Table Index Defrag Result", result);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void OnSetDefaultAlertTemplateClick(object sender, EventArgs e)
        {
            SetDefaultAlertTemplate();
        }

        private void SetDefaultAlertTemplate()
        {
            rtbAlertTemplate.Text = Settings.DefaultTemplate;
        }

        private void OnHistoriesRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isUpdating && e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var detail = dgvHistories.Rows[e.RowIndex].Cells[1].Value as string;
                rtbHistoryDetail.Text = detail;
                rtbHistoryDetail.Refresh();
            }
        }

        private void OnSetVersionControlClick(object sender, EventArgs e)
        {
            SetVersionControl(!(bool)tmiSetVersionControl.Tag);
        }

        private void OnObjectScriptVersionsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboObjectScriptVersions.SelectedIndex == 0)
                SetObjectScriptEx(_currentObjectName, _currentObjectScript, _currentObjectMode);
            else
                SetObjectScriptVersion(_currentObjectName, Convert.ToInt32(cboObjectScriptVersions.SelectedValue));
        }

        private void OnSaveScriptClick(object sender, EventArgs e)
        {
            SaveScript(_currentObjectName + Utils.FileExtenionSql, rtbObjectScript.Text);
        }

        internal string SaveScript(string fileName, string script)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.OverwritePrompt = true;
                dlg.Filter = "SQL Script File(*.sql)|*.sql|All Files(*.*)|*.*";
                dlg.FileName = fileName;
                if (dlg.ShowDialog(this.ParentForm) == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, script);
                    return dlg.FileName;
                }
                else
                    return string.Empty;
            }
        }

        private void OnCompareObjectVersionClick(object sender, EventArgs e)
        {
            var currentVersion = Convert.ToInt32(cboObjectScriptVersions.SelectedValue);
            var previousVersion = -1;
            if (cboObjectScriptVersions.Items.Count > 1)
            {
                if (cboObjectScriptVersions.SelectedIndex < cboObjectScriptVersions.Items.Count - 2)
                {
                    var item = (KeyValuePair<string, int>)cboObjectScriptVersions.Items[cboObjectScriptVersions.SelectedIndex + 1];
                    previousVersion = item.Value;
                }
                else if (currentVersion != 0)
                    previousVersion = 0;
                else if (cboObjectScriptVersions.SelectedIndex > 0)
                {
                    var item = (KeyValuePair<string, int>)cboObjectScriptVersions.Items[cboObjectScriptVersions.SelectedIndex - 1];
                    previousVersion = item.Value;
                }

            }
            using (var dlg = new DiffResults(previousVersion, currentVersion))
            {
                dlg.ShowDialog(this);
            }
        }

        private void OnCompareScriptClick(object sender, EventArgs e)
        {
            using (var dlg = new DiffResults(Utils.EmptyIndex, Convert.ToInt32(cboObjectScriptVersions.SelectedValue)))
            {
                dlg.ShowDialog(this);
            }
        }

        private void OnActivityScriptSaveToFileClick(object sender, EventArgs e)
        {
            SaveScript("Process" + Utils.FileExtenionSql, rtbProcessSQL.Text);
        }

        private void OnAnalysisScriptSaveToFileClick(object sender, EventArgs e)
        {
            SaveScript("Analysis" + Utils.FileExtenionSql, rtbAnalysisSQL.Text);
        }

        private void OnConnectionTimeoutsTextChanged(object sender, EventArgs e)
        {
            Settings.Instance.ConnectionTimeout = Convert.ToInt32(cboConnectionTimeouts.Text);
        }

        private void OnDatabaseDiskFreeSpaceRatiosTextChanged(object sender, EventArgs e)
        {
            Settings.Instance.DatabaseDiskFreeSpaceRatio = Convert.ToInt32(cboDatabaseDiskFreeSpaceRatios.Text);
        }

        private void OnFreeMemoryRatiosTextChanged(object sender, EventArgs e)
        {
            Settings.Instance.FreeMemoryRatio = Convert.ToInt32(cboFreeMemoryRatios.Text);
        }

        private void OnFreeCpuRatiosTextChanged(object sender, EventArgs e)
        {
            Settings.Instance.FreeCpuRatio = Convert.ToInt32(cboFreeCPURatios.Text);
        }

        private void OnSearchObjectPreviousClick(object sender, EventArgs e)
        {
            FindObjectScriptKeyword(false);
        }

        private void OnSearchObjectNextClick(object sender, EventArgs e)
        {
            FindObjectScriptKeyword(true);
        }

        private void OnObjectScriptSelectionChanged(object sender, EventArgs e)
        {
            _currentSearchIndex = rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection.Count > 0 ? rtbObjectScript.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection[0].Offset : 0;
        }

        private void OnMainSpliterMoved(object sender, SplitterEventArgs e)
        {
            Settings.Instance.MainSplitterDistance = scMain.SplitterDistance;
        }

        private void OnObjectsSpliterMoved(object sender, SplitterEventArgs e)
        {
            Settings.Instance.ObjectsSplitterDistance = scObjects.SplitterDistance;
        }

        private void OnFindObjectReferencesClick(object sender, EventArgs e)
        {
            var cells = dgvObjects.SelectedCells;
            if (cells.Count > 0)
            {
                var objectName = dgvObjects.Rows[cells[0].RowIndex].Cells[1].Value as string;
                if (!string.IsNullOrEmpty(objectName))
                    SearchDatabase(objectName);
            }
        }

        private void OnProcessesCellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            var status = Convert.ToInt32(grid.Rows[e.RowIndex].Cells[14].Value);
            Color color;
            switch (ActivitiesObjectType)
            {
                case ActivityTypes.Process:
                    switch (status)
                    {
                        case -1:
                            color = Color.Red;
                            break;
                        case 1:
                            color = e.CellStyle.ForeColor;
                            break;
                        default:
                            color = Color.Gray;
                            break;
                    }
                    break;
                case ActivityTypes.Job:
                    switch (status)
                    {
                        case 0:
                            color = Color.Red;
                            break;
                        case 1:
                            color = e.CellStyle.ForeColor;
                            break;
                        default:
                            color = Color.Gray;
                            break;
                    }
                    break;
                default:
                    color = e.CellStyle.ForeColor;
                    break;
            }
            e.CellStyle.ForeColor = color;

            if (dgvProcesses.Columns[e.ColumnIndex].DataPropertyName == "percent_complete")
            {
                if (Convert.ToDouble(e.Value) == 0)
                    e.Value = "-";
                else if (Convert.ToDouble(e.Value) < 100)
                    dgvProcesses.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            }
        }

        private void OnDatabaseDataLogSpaceRatiosTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboDatabaseDataLogSpaceRatios.Text))
                Settings.Instance.DatabaseDataLogSpaceRatio = Convert.ToInt32(cboDatabaseDataLogSpaceRatios.Text);
        }

        private void OnTableDataIndexSpaceRatiosTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboTableDataIndexSpaceRatios.Text))
                Settings.Instance.TableDataIndexSpaceRatio = Convert.ToInt32(cboTableDataIndexSpaceRatios.Text);
        }

        private void SetupPerformance()
        {
            if (_previousServerInfo != _currentServerInfo
                || _previousDatabase != _currentDatabase
                || (_currentObjectMode == ObjectModes.Server
                && _previousObjectMode == ObjectModes.Databases)
                || (_currentObjectMode == ObjectModes.Databases
                && _currentObjectMode == ObjectModes.Server))
            {
                ResetPerformance();
            }
        }

        private void ShowPerformance()
        {
            tcMain.SelectedTab = tpPerformance;
        }

        private void OnRunManagementStudioClick(object sender, EventArgs e)
        {
            var paths = new string[] { @"Microsoft SQL Server\120\Tools\Binn\ManagementStudio\", 
                                       @"Microsoft SQL Server\110\Tools\Binn\ManagementStudio\", 
                                       @"Microsoft SQL Server\100\Tools\Binn\VSShell\Common7\IDE\", 
                                       @"Microsoft SQL Server\90\Tools\Binn\VSShell\Common7\IDE\", 
                                       @"Microsoft SQL Server\80\Tools\Binn\VSShell\Common7\IDE\"};

            var result = paths.Select(p =>
                    {
                        var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\" + p + "Ssms.exe";
                        if (File.Exists(path))
                            return path;
                        path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + p + "Ssms.exe";
                        if (File.Exists(path))
                            return path;
                        else
                            return null;
                    }
                    ).FirstOrDefault(p => p != null);
            if (result != null)
                Process.Start(result);
            else
                ShowMessage("Could not find management studio executable.");
        }

        internal void ShowAnalysis(AnalysisTypes analysisType)
        {
            tcMain.SelectedTab = tpAnalysis;
            cboAnalysisTypes.SelectedItem = analysisType;
        }

        internal void ShowActivities()
        {
            tcMain.SelectedTab = tpActivities;
        }

        private void OnLogHistoryCheckedChanged(object sender, EventArgs e)
        {
            if (_instance != null)
                Settings.Instance.LogHistory = chkLogHistory.Checked;
        }

        internal void RemoveCurrentTab()
        {
            tcMain.TabPages.Remove(tcMain.SelectedTab);
        }

        internal void AddPerformance(Performance item)
        {
            var tabPage = new TabPage(item.Title);
            tabPage.Tag = WorkModes.UserPerformance;
            item.Dock = DockStyle.Fill;
            tabPage.Controls.Add(item);
            tcMain.TabPages.Add(tabPage);
            tcMain.SelectedTab = tabPage;
        }

        private void OnShowUserPerformanceClick(object sender, EventArgs e)
        {
            var server = GetCurrentServer();
            var index = -1;
            for (var i = 0; i < tcMain.TabPages.Count; i++)
            {
                WorkModes workMode;
                if (i < (int)WorkModes.Query)
                    workMode = (WorkModes)i;
                else
                    workMode = (WorkModes)tcMain.TabPages[i].Tag;
                switch (workMode)
                {
                    case WorkModes.UserPerformance:
                        var performance = tcMain.TabPages[i].Controls[0] as Performance;
                        if (performance.Server.Server == server.Server
                            && performance.Server.Database == server.Database)
                        {
                            index = i;
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            Form form = null;
            if (index == -1)
            {
                var forms = Application.OpenForms.Cast<Form>().Where(f => f is PerformanceDialog);
                if (forms.Any())
                {
                    form = forms.First(f =>
                        {
                            var p = (f as PerformanceDialog).Controls[0] as Performance;
                            return (p.Server.Server == server.Server
                                && p.Server.Database == server.Database);
                        });
                    index = form != null ? 1 : -1;
                }
            }
            if (index == -1)
            {
                var performance = new Performance();
                performance.Init(_currentObjectMode, server);
                performance.ResetPerformance();
                performance.SetInterval(cboPerformanceIntervals.Text);
                performance.ShowPopDock();
                AddPerformance(performance);
            }
            else
            {
                if (form != null)
                    form.BringToFront();
                else
                    tcMain.SelectedIndex = index;
            }
        }

        private void OnMainMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                CloseCurrentTabPage(e.Location);
        }

        private void OnClearRecentObjectsClick(object sender, EventArgs e)
        {
            Settings.Instance.RecentObjects.Clear();
            ClearRecentObjects();
        }

        private void ClearRecentObjects()
        {
            tbRecentObjects.DropDownItems.Cast<ToolStripItem>().Where(i => i != tsRecentObjects && i != tmClearRecentObjects).ToList().ForEach(i =>
            {
                tbRecentObjects.DropDownItems.Remove(i);
            });
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (Settings.Instance.ObjectsSplitterDistance > 0)
                scObjects.SplitterDistance = Settings.Instance.ObjectsSplitterDistance;

            if (Settings.Instance.MainSplitterDistance > 0)
                scMain.SplitterDistance = Settings.Instance.MainSplitterDistance;

            //Updater.Instance.FoundNewVersion += OnUpdaterFoundNewVersion;
            //Updater.Instance.Detect();
        }

        //private void OnUpdaterFoundNewVersion(object sender, UpdateEventArgs e)
        //{
        //    this.Invoke(() =>
        //        {
        //            if (ShowQuestion(e.Message))
        //                Process.Start(e.Url);
        //            else
        //                Settings.Instance.IgnoredVersionUpdate = e.Version;
        //        });
        //}

        private void OnServerHealthCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                var direction = ListSortDirection.Ascending;
                if (e.ColumnIndex == _healthPrevColIndex) // reverse sort order
                    direction = _healthPrevSortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                // remember the column that was clicked and in which direction is ordered
                _healthPrevColIndex = e.ColumnIndex;
                _healthPrevSortDirection = direction;

                // set the column to be grouped
                dgvServerHealth.GroupTemplate.Column = dgvServerHealth.Columns[e.ColumnIndex];

                dgvServerHealth.Sort(dgvServerHealth.Columns[e.ColumnIndex], direction);
            }
        }

        private void OnServerHealthCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == HealthIndexName && e.RowIndex != -1)
            {
                var healthType = dgvServerHealth.Rows[e.RowIndex].Cells[HealthIndexObject].Value;
                if (healthType != null)
                {
                    switch ((HealthTypes)Enum.Parse(typeof(HealthTypes), healthType.ToString()))
                    {
                        case HealthTypes.ServerMemory:
                            ShowPerformance();
                            break;
                        case HealthTypes.ServerCpu:
                            ShowPerformance();
                            break;
                        case HealthTypes.ServerSpace:
                            ShowAnalysis(AnalysisTypes.DatabasesSpace);
                            break;
                        case HealthTypes.DatabaseLogSpace:
                            ShowAnalysis(AnalysisTypes.DatabasesSpace);
                            break;
                        case HealthTypes.DatabaseStall:
                            ShowAnalysis(AnalysisTypes.Performance);
                            break;
                        case HealthTypes.BlockedProcess:
                            ShowActivities();
                            break;
                        case HealthTypes.LockedObjects:
                            ShowAnalysis(AnalysisTypes.LockedObjects);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
