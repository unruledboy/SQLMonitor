using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Controls.Tree;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.UI
{
    public partial class ProcessVisualizer : BaseDialog
    {
        private const string KeyScheduler = "S";
        private const string KeyWorker = "W";
        private const string KeyTask = "T";
        private const string KeyRequest = "R";
        private TreeBuilder _tree = null;
        private DataTable _processes = null;
        private int _currentProcess = 0;
        private readonly ServerInfo _serverInfo = null;

        public ProcessVisualizer()
        {
            InitializeComponent();
            this.Description = "Single click a process to view info, double click a process to kill";
            Utils.SetTextBoxStyle(rtbInfo);
        }

        public ProcessVisualizer(ServerInfo server)
            : this()
        {
            if (server != null)
                _serverInfo = server.Clone();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadProcesses();
        }

        internal ServerInfo Server
        {
            get { return _serverInfo; }
        }

        internal void LoadProcesses()
        {
            rtbInfo.Text = string.Empty;
            rtbInfo.Refresh();
            cmdKill.Enabled = false;
            var model = GetProcesses();
            _tree = new TreeBuilder(model);
            _tree.BgColor = this.BackColor;
            if (model.Count > 0)
                picProcesses.Image = Image.FromStream(_tree.GenerateTree(-1, -1, model.First.Id, ImageFormat.Png));
            else
                picProcesses.Image = null;
        }

        private void OnProccessMouseClick(object sender, MouseEventArgs e)
        {
            var node = GetCurrentNode(e.X, e.Y);
            var description = string.Empty;
            if (node != null)
            {
                description = node.Attributes["nodeDescription"].Value;
                var id = node.Attributes["nodeID"].Value;
                if (id.StartsWith(KeyRequest))
                    _currentProcess = Convert.ToInt32(id.Substring(1));
                else
                    _currentProcess = 0;
            }
            else
                _currentProcess = 0;
            rtbInfo.Text = string.Format(description);
            rtbInfo.Refresh();
            cmdKill.Enabled = _currentProcess != 0;
        }

        private XmlNode GetCurrentNode(int x, int y)
        {
            XmlNode node = null;
            Rectangle currentRect;
            //find the node
            foreach (XmlNode oNode in _tree.XmlTree.SelectNodes("//Node"))
            {
                //iterate through all nodes until found.
                currentRect = _tree.GetRectangleFromNode(oNode);
                if (x >= currentRect.Left &&
                    x <= currentRect.Right &&
                    y >= currentRect.Top &&
                    y <= currentRect.Bottom)
                {
                    node = oNode;
                    break;
                }
            }
            return node;
        }

        private void OnProcessesMouseMove(object sender, MouseEventArgs e)
        {
            var cursor = Cursors.Default;
            var node = GetCurrentNode(e.X, e.Y);
            if (node != null)
                cursor = Cursors.Hand;
            if (picProcesses.Cursor != cursor)
            {
                picProcesses.Cursor = cursor;
                ttHint.SetToolTip(picProcesses.InnerPicture, node != null ? node.Attributes["nodeDescription"].Value : string.Empty);
            }
        }

        private Tree GetProcesses()
        {
            var model = new Tree();
            if (_serverInfo != null)
            {
                try
                {
                    var server = _serverInfo.Server;
                    var rootId = server;
                    model.AddNode(rootId, string.Empty, server, server, server);

                    var taskList = new Dictionary<byte[], int>();
                    _processes = QueryEngine.GetSessions(_serverInfo);
                    var rows = _processes.AsEnumerable().DistinctBy(r => r.Field<short>("session_id"));
                    var schedulers = rows.GroupBy(r => r.Field<byte[]>("scheduler_address"));
                    var schedulerList = new List<string>();
                    var workerId = 0;
                    var taskId = 0;
                    var blockColor = Color.Salmon;
                    schedulers.ForEach(s =>
                        {
                            //http://msdn.microsoft.com/en-us/library/ms177526.aspx
                            var scheduler = s.First();
                            var schedulerId = scheduler.Field<int>("scheduler_id").ToString();
                            string description;
                            var schedulerRoot = KeyScheduler + schedulerId;
                            if (!schedulerList.Contains(schedulerId))
                            {
                                schedulerList.Add(schedulerId);
                                var isIdle = scheduler.Field<bool>("is_idle");
                                description = string.Format("is idle: {0}\r\ncurrent workers: {1}\r\nactive workers: {2}", isIdle.ToString(), scheduler.Field<int>("current_workers_count").ToString(), scheduler.Field<int>("active_workers_count"));
                                model.AddNode(schedulerRoot, rootId, schedulerRoot, "Scheduler " + schedulerId, description, isIdle ? Color.LightGray : Color.White);
                            }
                            var workers = s.GroupBy(w => w.Field<byte[]>("worker_address"));
                            workers.ForEach(w =>
                                {
                                    //http://msdn.microsoft.com/en-us/library/ms178626.aspx
                                    var workerRoot = KeyWorker + workerId;
                                    var worker = w.First();
                                    var workerState = worker.Field<string>("worker_state");
                                    var workerStateName = string.Empty;
                                    var workerColor = Color.White;
                                    switch (workerState.ToUpper())
                                    {
                                        case "INIT":
                                            workerStateName = "initializing";
                                            workerColor = Color.LightGreen;
                                            break;
                                        case "RUNNING":
                                            workerStateName = "running";
                                            workerColor = Color.LightBlue;
                                            break;
                                        case "RUNNABLE":
                                            workerStateName = "runnable";
                                            workerColor = Color.Green;
                                            break;
                                        case "SUSPENDED":
                                            workerStateName = "suspended";
                                            workerColor = Color.LightGray;
                                            break;
                                        default:
                                            workerStateName = "unkown";
                                            break;
                                    }
                                    var workerIsSick = worker.Field<bool>("is_sick");
                                    var workerIsFatalException = worker.Field<bool>("is_fatal_exception");
                                    var workerExceptionCount = worker.Field<int>("exception_num");
                                    var workerTasksProcessedCount = worker.Field<int>("tasks_processed_count");
                                    var workerLastResult = worker.Field<int>("return_code");
                                    var workerLastResultName = string.Empty;
                                    switch (workerLastResult)
                                    {
                                        case 0:
                                            workerLastResultName = "success";
                                            break;
                                        case 3:
                                            workerLastResultName = "dead lock";
                                            break;
                                        case 4:
                                            workerLastResultName = "premature wakeup";
                                            break;
                                        case 258:
                                            workerLastResultName = "time out";
                                            break;
                                        default:
                                            workerLastResultName = string.Format("(unkown:{0})", workerLastResult);
                                            break;
                                    }
                                    description = string.Format("processed tasks: {0}\r\nstate: {1}\r\nis sick: {2}\r\nis fatal exception:{3}\r\nexception number:{4}\r\nlast result:{5}", workerTasksProcessedCount, workerStateName, workerIsSick.ToString(), workerIsFatalException.ToString(), workerExceptionCount, workerLastResultName);

                                    model.AddNode(workerRoot, schedulerRoot, workerRoot, "Worker " + workerId, description, workerColor);

                                    var tasks = w.GroupBy(t => t.Field<byte[]>("task_address"));
                                    tasks.ForEach(t =>
                                        {
                                            //http://msdn.microsoft.com/en-us/library/ms174963.aspx
                                            var taskRoot = KeyTask + taskId;
                                            var task = t.First();
                                            var taskAddress = task.Field<byte[]>("task_address");
                                            if (!taskList.ContainsKey(taskAddress))
                                                taskList.Add(taskAddress, taskId);
                                            var taskState = task.Field<string>("task_state");
                                            var taskStateName = string.Empty;
                                            var taskColor = Color.White;
                                            switch (taskState.ToUpper())
                                            {
                                                case "PENDING":
                                                    taskStateName = "pending";
                                                    taskColor = Color.LightGreen;
                                                    break;
                                                case "RUNNING":
                                                    taskStateName = "running";
                                                    taskColor = Color.LightBlue;
                                                    break;
                                                case "RUNNABLE":
                                                    taskStateName = "runnable";
                                                    taskColor = Color.Green;
                                                    break;
                                                case "SUSPENDED":
                                                    taskStateName = "suspended";
                                                    taskColor = Color.LightGray;
                                                    break;
                                                case "DONE":
                                                    taskStateName = "done";
                                                    taskColor = Color.White;
                                                    break;
                                                case "SPINLOOP":
                                                    taskStateName = "spin loop";
                                                    taskColor = Color.LightSalmon;
                                                    break;
                                                default:
                                                    taskStateName = "unknown";
                                                    break;
                                            }
                                            description = string.Format("state:" + taskStateName);
                                            model.AddNode(taskRoot, workerRoot, taskRoot, "Task " + taskId, description, taskColor);

                                            taskId++;
                                        });

                                    workerId++;
                                });
                        });


                    var addedProcesses = new List<short>();
                    var lockedProcesses = QueryEngine.GetLockedProcesses(_serverInfo);
                    lockedProcesses.AsEnumerable().ForEach(p =>
                        {
                            var spid = p.Field<short>("SPID");
                            var task = rows.FirstOrDefault(r => r.Field<short>("session_id") == spid);
                            if (task != null)
                            {
                                var taskAddress = task.Field<byte[]>("task_address");
                                int parent;
                                if (taskList.TryGetValue(taskAddress, out parent))
                                {
                                    var taskRoot = KeyTask + parent;
                                    var requstId = KeyRequest + spid;
                                    var description = GetProcessDescription(task);
                                    var blockingId = p.Field<short?>("BlockingSPID");
                                    Color backColor;
                                    if (blockingId != null && blockingId != 0)
                                    {
                                        taskRoot = KeyRequest + blockingId;
                                        backColor = blockColor;
                                    }
                                    else
                                    {
                                        backColor = Color.Red;
                                        description = "!blocking root, kill it!\r\n\r\n" + description;

                                    }
                                    var currentServer = _serverInfo.Clone();
                                    currentServer.Database = task.Field<string>("database_name");
                                    var lockedObjects = QueryEngine.GetLockedObjects(spid, currentServer);
                                    var lockedObjectList = new StringBuilder();
                                    if (lockedObjects.Rows.Count > 0)
                                    {
                                        var sessionLockedObjects = lockedObjects.AsEnumerable();
                                        if (sessionLockedObjects.Any())
                                        {
                                            lockedObjectList.AppendLine("locked objects:");
                                            sessionLockedObjects.ForEach(r =>
                                                {
                                                    lockedObjectList.AppendFormat("  {0}.{1}.{2}\r\n", r.Field<string>("DatabaseName"), r.Field<string>("SchemaName"), r.Field<string>("ObjectName"));
                                                });
                                        }
                                    }
                                    description = lockedObjectList.ToString() + description;
                                    addedProcesses.Add(spid);
                                    model.AddNode(requstId, taskRoot, requstId, "Process " + spid, description, backColor);
                                }
                            }
                        });


                    rows.ForEach(p =>
                    {
                        var sessionId = p.Field<short>("session_id");
                        if (!addedProcesses.Contains(sessionId))
                        {
                            var session = KeyRequest + sessionId;
                            var taskAddress = p.Field<byte[]>("task_address");
                            var task = taskList[taskAddress];
                            var taskRoot = KeyTask + task;
                            var description = GetProcessDescription(p);
                            Color backColor;
                            var sessionStatus = p.Field<string>("session_status");
                            switch (sessionStatus.ToUpper())
                            {
                                case "RUNNING":
                                    backColor = Color.LightBlue;
                                    break;
                                case "SLEEPING":
                                    backColor = Color.LightGray;
                                    break;
                                case "DORMANT":
                                    backColor = Color.Gray;
                                    break;
                                case "PRECONNECT":
                                    backColor = Color.Gray;
                                    break;
                                default:
                                    backColor = Color.White;
                                    break;
                            }
                            model.AddNode(session, taskRoot, session, "Process " + sessionId, description, backColor);
                        }
                    });

                    var title = "Process Visualizer ({0})";
                    if (lockedProcesses.Rows.Count > 0)
                        title += ", found {1} dead lock processes!";
                    this.Text = string.Format(title, _serverInfo.Server, lockedProcesses.Rows.Count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return model;
        }

        private string GetProcessDescription(DataRow row)
        {
            var sessionStatus = row.Field<string>("session_status");
            var openTransactionCount = row.Field<int>("open_transaction_count");
            var command = row.Field<string>("command");
            var databaseName = row.Field<string>("database_name");
            var currentStatement = row.Field<string>("current_statement");
            var text = currentStatement ?? row.Field<string>("text");
            return string.Format("status:{0}\r\nopen transaction count:{1}\r\ndatabase:{2}\r\ncommand:{3}\r\nexecuting statement:\r\n\r\n{4}", sessionStatus, openTransactionCount, databaseName, command, text);
        }

        private void OnProcessMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Kill();
        }

        private void OnKillClick(object sender, EventArgs e)
        {
            Kill();
        }

        private void Kill()
        {
            if (_currentProcess != 0)
            {
                if (MessageBox.Show(this, "Are you sure to kill the selected process?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlHelper.ExecuteNonQuery(string.Format("kill {0}", _currentProcess), _serverInfo);
                    LoadProcesses();
                }
            }
        }

        private void OnViewClick(object sender, EventArgs e)
        {
            Monitor.Instance.ShowActivities();
            Monitor.Instance.BringToFront();
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            LoadProcesses();
        }
    }
}
