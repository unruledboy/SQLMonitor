using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.Logic
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public bool Cancel { get; set; }

        public MessageEventArgs(string message, bool cancel)
        {
            this.Message = message;
            this.Cancel = cancel;
        }
    }

    public class AlertEventArgs : MessageEventArgs
    {
        public MonitorItem Item { get; set; }
        public NotifiedMonitorItem Notification { get; set; }

        public AlertEventArgs(MonitorItem item, NotifiedMonitorItem notification, string message)
            : base(message, false)
        {
            this.Item = item;
            this.Notification = notification;
        }
    }

    public class ServerInfoEventArgs : EventArgs
    {
        public ServerInfo Server { get; set; }
        public bool IsServer { get; set; }
        public bool Cancel { get; set; }

        public ServerInfoEventArgs()
        {
        }
    }

    public class HealthEventArgs : EventArgs
    {
        public List<HealthItem> Result { get; set; }

        public HealthEventArgs()
        {
        }
    }

    public class PerformanceRecordEventArgs : EventArgs
    {
        public ServerInfo Server { get; set; }
        public PerformanceRecord Data { get; set; }

        public PerformanceRecordEventArgs()
        {
        }
    }

    public class MonitorEngine
    {
        public const string HealthCategoryServer = "Server";
        public const string HealthCategoryProcess = "Process";
        public const string HealthCategoryDatabase = "Database";

        public event EventHandler<MessageEventArgs> Message;
        public event EventHandler<AlertEventArgs> Alert;
        public event EventHandler<ServerInfoEventArgs> RequestPerformanceServer;
        public event EventHandler<PerformanceRecordEventArgs> UpdateServerInfo;
        public event EventHandler<ServerInfoEventArgs> RequestHealthServer;
        public event EventHandler<HealthEventArgs> Health;
        private readonly Dictionary<string, NotifiedMonitorItem> _notifiedAlerts = new Dictionary<string, NotifiedMonitorItem>();
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private static MonitorEngine _instance = null;
        private Timer _tmrMonitorEngineRefresh = null;
        private Timer _tmrPerformanceRefresh = null;
        private DataTable _lastPerformanceData = null;
        private readonly List<ServerInfoEx> _userPerformanceItems = new List<ServerInfoEx>();
        private volatile bool _lastChecked = true;

        public MonitorEngine()
        {
            try
            {
                _cpuCounter = new PerformanceCounter();
                _cpuCounter.CategoryName = "Processor";
                _cpuCounter.CounterName = "% Processor Time";
                _cpuCounter.InstanceName = "_Total";

                _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception)
            {
                _cpuCounter = null;
                _ramCounter = null;
            }
        }

        public static MonitorEngine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MonitorEngine();
                return _instance;
            }
        }

        public int CurrentCpuUsage
        {
            get
            {
                return _cpuCounter != null ? Convert.ToInt32(_cpuCounter.NextValue()) : 0;
            }
        }

        public int AvailableRam
        {
            get { return _ramCounter != null ? Convert.ToInt32(_ramCounter.NextValue()) : 0; }
        }

        public void SetMonitorInterval(string Interval)
        {
            int interval;
            if (int.TryParse(Interval, out interval) && interval > 0)
            {
                var span = TimeSpan.FromSeconds(interval);
                _tmrMonitorEngineRefresh = new Timer(OnMonitorRefreshTick, null, span, span);
            }
            else if (_tmrMonitorEngineRefresh != null)
            {
                _tmrMonitorEngineRefresh.Dispose();
                _tmrMonitorEngineRefresh = null;
                interval = 0;
            }
            Settings.Instance.MonitorRefreshInterval = interval;
        }

        public void DisablePerformance()
        {
            SetPerformanceInterval(string.Empty);
        }

        public void SetPerformanceInterval(string Interval)
        {
            int interval;
            if (int.TryParse(Interval, out interval) && interval > 0)
            {
                var span = TimeSpan.FromSeconds(interval);
                _tmrPerformanceRefresh = new Timer(OnPerformanceRefreshTick, null, span, span);
            }
            else if (_tmrPerformanceRefresh != null)
            {
                _tmrPerformanceRefresh.Dispose();
                _tmrPerformanceRefresh = null;
                interval = 0;
            }
            Settings.Instance.PerformanceInterval = interval;
        }

        public void CheckServerHealth()
        {
            if (RequestHealthServer != null && Health != null)
            {
                var e = new ServerInfoEventArgs();
                RequestHealthServer(this, e);
                var healthItems = new List<HealthItem>();
                if (!e.Cancel && e.Server != null && !string.IsNullOrEmpty(e.Server.Server))
                {
                    var isValid = true;
                    try
                    {
                        var version = QueryEngine.GetServerVersion(e.Server);
                    }
                    catch (Exception)
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {
                        var isAlert = false;

                        //memory
                        long physicalMemory;
                        long availableMemory;
                        var serverState = e.Server as ServerState;
                        if (!serverState.IsAzure)
                            QueryEngine.GetMemoryInfo(e.Server, out physicalMemory, out availableMemory);
                        else
                        {
                            physicalMemory = 0;
                            availableMemory = 0;
                        }
                        var memoryMb = SqlHelper.ExecuteScalar("SELECT (cntr_value/1024.0) FROM sys.dm_os_performance_counters WHERE counter_name = 'Total Server Memory (KB)'", e.Server);
                        if (memoryMb != null)
                        {
                            var memory = Convert.ToInt32(memoryMb);
                            isAlert = availableMemory * 1.0 / physicalMemory < Settings.Instance.FreeMemoryRatio / 100.0;
                            healthItems.Add(new HealthItem { Category = HealthCategoryServer, HealthType = HealthTypes.ServerMemory, CurrentValue = availableMemory.ToString() + " " + Utils.SizeMb, ReferenceValue = memory + " " + Utils.SizeMb, ItemName = "Server Memory", Description = "Free/DB Used", IsAlert = isAlert });
                        }

                        //cpu
                        int cpuSqlProcess;
                        int cpuSystemIdle;
                        int cpuOtherProcesses;
                        QueryEngine.GetCpuInfo(e.Server, out cpuSqlProcess, out cpuSystemIdle, out cpuOtherProcesses);
                        isAlert = cpuSystemIdle < Settings.Instance.FreeCpuRatio;
                        healthItems.Add(new HealthItem { Category = HealthCategoryServer, HealthType = HealthTypes.ServerCpu, CurrentValue = cpuSystemIdle.ToString() + " %", ReferenceValue = cpuSqlProcess + " %", ItemName = "Server CPU", Description = "Free/DB Used", IsAlert = isAlert });

                        //disk space
                        var diskSpaces = QueryEngine.GetDiskSpace(e.Server);
                        diskSpaces.Where(s => s.Value.Value > 0).ForEach(s =>
                        {
                            isAlert = s.Value.Key < s.Value.Value / 100 * Settings.Instance.DatabaseDiskFreeSpaceRatio;
                            healthItems.Add(new HealthItem { Category = HealthCategoryServer, HealthType = HealthTypes.ServerSpace, CurrentValue = s.Value.Key.ToString() + " " + Utils.SizeMb, ReferenceValue = s.Value.Value + " " + Utils.SizeMb, ItemName = "Disk Space (" + s.Key + ")", Description = "Free/DB Used", IsAlert = isAlert });
                        });

                        if (!e.Server.IsAzure)
                        {
                            //locked objects
                            var lockedObjects = SqlHelper.Query(QueryEngine.SqlLockedObjects, e.Server);
                            lockedObjects.Rows.Cast<DataRow>().ForEach(r =>
                            {
                                isAlert = false;
                                healthItems.Add(new HealthItem { Category = HealthCategoryProcess, HealthType = HealthTypes.LockedObjects, CurrentValue = r.Field<string>("SchemaName") + "." + r.Field<string>("ObjectName"), ReferenceValue = r.Field<string>("DatabaseName"), ItemName = "Locked Object", Description = r.Field<string>("ProgramName"), IsAlert = isAlert });
                            });
                        }

                        //blocked processes
                        var blockedProcesses = SqlHelper.Query(QueryEngine.SqlWaitingTasks + " WHERE blocking_session_id IS NOT NULL", e.Server);
                        blockedProcesses.Rows.Cast<DataRow>().ForEach(r =>
                        {
                            isAlert = true;
                            healthItems.Add(new HealthItem { Category = HealthCategoryProcess, HealthType = HealthTypes.BlockedProcess, CurrentValue = r["Session Id"].ToString(), ReferenceValue = r["Blocking Session Id"].ToString(), ItemName = "Blocked Process", Description = "Blocked/Blocking", IsAlert = isAlert });
                        });

                        //db performance
                        var databaseStalls = QueryEngine.GetDatabaseStall(e.Server);
                        databaseStalls.ForEach(db =>
                        {
                            isAlert = db.IsExceeded;
                            healthItems.Add(new HealthItem { Category = HealthCategoryDatabase, HealthType = HealthTypes.DatabaseStall, CurrentValue = db.Max + " " + Utils.TimeMs, ReferenceValue = QueryEngine.DbStallThreshold + " " + Utils.TimeMs, ItemName = "DB Perf (" + db.Database + ")", Description = "Higher = worse", IsAlert = isAlert });
                        });

                        //db/log space
                        var dbLogSpaces = QueryEngine.GetDbLogSpace(e.Server);
                        dbLogSpaces.Where(s => !s.Value.Item3).ForEach(s =>
                        {
                            healthItems.Add(new HealthItem { Category = HealthCategoryDatabase, HealthType = HealthTypes.DatabaseLogSpace, CurrentValue = s.Value.Item1.ToString() + " " + Utils.SizeMb, ReferenceValue = s.Value.Item2 + " " + Utils.SizeMb, ItemName = "DB/Log Space (" + s.Key + ")", Description = "Log/DB", IsAlert = true });
                        });
                    }
                }
                Health(this, new HealthEventArgs { Result = healthItems });
            }
        }

        private void OnMonitorRefreshTick(object sender)
        {
            if (_lastChecked)
            {
                _lastChecked = false;
                CheckServerHealth();
                CheckMonitorItems();
                _lastChecked = true;
            }
        }

        private void OnPerformanceRefreshTick(object sender)
        {
            CheckPerformance();
        }

        private bool IsNotified(MonitorItem item, string Key, string runtimeValue, string message, out NotifiedMonitorItem notified)
        {
            notified = null;
            var key = item.ToString() + ", " + Key + ", " + runtimeValue;
            if (!_notifiedAlerts.ContainsKey(key))
            {
                notified = new NotifiedMonitorItem { Server = item.Server, CurrentValue = message, CreatedDate = DateTime.Now };
                _notifiedAlerts.Add(key, notified);
                if (Settings.Instance.LogHistory)
                    Settings.Instance.NotifiedAlerts.Add(notified);
                return false;
            }
            else
                return true;
        }

        private bool IsTextMatch(string content, string pattern)
        {
            try
            {
                if (content.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) != -1)
                    return true;
                else if (Regex.IsMatch(content, pattern))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void OnUpdateServerInfo(PerformanceRecord Record, ServerInfo server, bool isServer)
        {
            UpdateServerInfo(this, new PerformanceRecordEventArgs() { Data = Record, Server = server });
            if (Settings.Instance.LogHistory)
            {
                var record = new HistoryRecord(Record)
                {
                    Date = DateTime.Now.ToString(),
                    Key = History.GetKey(server, isServer)
                };
                History.AddRecords(new List<HistoryRecord> { record });
            }
        }

        public void RemoveUserPerformanceItem(ServerInfo server, bool isServer)
        {
            var item = _userPerformanceItems.FirstOrDefault(p => p.Server == server.Server
                && p.Database == server.Database && p.IsServer == isServer);
            if (item != null)
                _userPerformanceItems.Remove(item);
        }

        public void AddUserPerformanceItem(ServerInfo server, bool isServer)
        {
            if (!_userPerformanceItems.Exists(p => p.Server == server.Server
                && p.Database == server.Database && p.IsServer == isServer))
                _userPerformanceItems.Add(new ServerInfoEx
                {
                    AuthType = server.AuthType,
                    Database = server.Database,
                    IsEncrypted = server.IsEncrypted,
                    IsServer = isServer,
                    Password = server.Password,
                    Server = server.Server,
                    User = server.User
                });
        }

        public void CheckPerformance()
        {
            try
            {
                if (RequestPerformanceServer != null)
                {
                    var e = new ServerInfoEventArgs();
                    RequestPerformanceServer(this, e);
                    if (!e.Cancel)
                    {
                        Instance.AddUserPerformanceItem(e.Server, e.IsServer);
                        CheckPerformanceItem(e.Server, e.IsServer);
                    }
                }

                if (this.Equals(Instance))
                {
                    Settings.Instance.PerformanceItems.ForEach(i =>
                        {
                            var exists = Instance._userPerformanceItems.Exists(p => p.Server == i.Server
                                && p.Database == i.Database && p.IsServer == i.IsServer);
                            if (!exists)
                                CheckPerformanceItem(i, i.IsServer);
                        });
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void CheckPerformanceItem(ServerInfo server, bool isServer)
        {
            if (server.IsAzure)
                return;

            PerformanceRecord record;
            if (isServer)
            {
                var sql = @"declare @now 		datetime
declare @cpu_busy 	bigint
declare @io_busy	bigint
declare @idle		bigint
declare @pack_received	bigint
declare @pack_sent	bigint
declare @pack_errors	bigint
declare @connections	bigint
declare @total_read	bigint
declare @total_write	bigint
declare @total_errors	bigint

declare @oldcpu_busy 	bigint	/* used to see if DataServer has been rebooted */
declare @interval	bigint
declare @mspertick	bigint	/* milliseconds per tick */


/*
**  Set @mspertick.  This is just used to make the numbers easier to handle
**  and avoid overflow.
*/
select @mspertick = convert(int, @@timeticks / 1000.0)

/*
**  Get current monitor values.
*/
select
	@now = getdate(),
	@cpu_busy = @@cpu_busy,
	@io_busy = @@io_busy,
	@idle = @@idle,
	@pack_received = @@pack_received,
	@pack_sent = @@pack_sent,
	@connections = @@connections,
	@pack_errors = @@packet_errors,
	@total_read = @@total_read,
	@total_write = @@total_write,
	@total_errors = @@total_errors

/*
**  Check to see if DataServer has been rebooted.  If it has then the
**  value of @@cpu_busy will be less than the value of spt_monitor.cpu_busy.
**  If it has update spt_monitor.
*/
select @oldcpu_busy = cpu_busy
	from master.dbo.spt_monitor
if @oldcpu_busy > @cpu_busy
begin
	update master.dbo.spt_monitor
		set
			lastrun = @now,
			cpu_busy = @cpu_busy,
			io_busy = @io_busy,
			idle = @idle,
			pack_received = @pack_received,
			pack_sent = @pack_sent,
			connections = @connections,
			pack_errors = @pack_errors,
			total_read = @total_read,
			total_write = @total_write,
			total_errors = @total_errors
end

/*
**  Now print out old and new monitor values.
*/
set nocount on
select @interval = datediff(ss, lastrun, @now)
	from master.dbo.spt_monitor
/* To prevent a divide by zero error when run for the first
** time after boot up
*/
if @interval = 0
	select @interval = 1
select last_run = lastrun, current_run = @now, seconds = @interval,
	cpu_busy_total = convert(bigint, (@cpu_busy / 1000.0 * @mspertick)),
	cpu_busy_current = convert(bigint, ((@cpu_busy - cpu_busy)
		 / 1000.0 * @mspertick)),
	cpu_busy_percentage = convert(bigint, (((@cpu_busy - cpu_busy)
		 / 1000.0 * @mspertick) / @interval * 100.0)),
	io_busy_total = convert(bigint, (@io_busy / 1000 * @mspertick)),
	io_busy_current = convert(bigint, ((@io_busy - io_busy)
		 / 1000.0 * @mspertick)),
	io_busy_percentage = convert(bigint, (((@io_busy - io_busy)
		 / 1000.0 * @mspertick) / @interval * 100.0)),
	idle_total = convert(bigint, (convert(bigint,@idle) / 1000.0 * @mspertick)),
	idle_current = convert(bigint, ((@idle - idle)
		 / 1000.0 * @mspertick)),
	idle_percentage = convert(bigint, (((@idle - idle)
		 / 1000.0 * @mspertick) / @interval * 100.0)),
	packets_received_total = @pack_received,
	packets_received_current = @pack_received - pack_received,
	packets_sent_total = @pack_sent,
	packets_sent_current = @pack_sent - pack_sent,
	packet_errors_total = @pack_errors,
	packet_errors_current = @pack_errors - pack_errors,
	total_read = @total_read,
	current_read = @total_read - total_read,
	total_write = @total_write,
	current_write =	@total_write - total_write,
	total_errors = @total_errors,
	current_errors = @total_errors - total_errors,
	connections_total = @connections,
	connections_current = @connections - connections
from master.dbo.spt_monitor

/*
**  Now update spt_monitor
*/
update master.dbo.spt_monitor
	set
		lastrun = @now,
		cpu_busy = @cpu_busy,
		io_busy = @io_busy,
		idle = @idle,
		pack_received = @pack_received,
		pack_sent = @pack_sent,
		connections = @connections,
		pack_errors = @pack_errors,
		total_read = @total_read,
		total_write = @total_write,
		total_errors = @total_errors";
                var serverInfo = SqlHelper.Query(sql, server);
                var row = serverInfo.Rows[0];
                record = new PerformanceRecord
                {
                    Value1 = Convert.ToInt64(row["cpu_busy_current"]),
                    Value2 = Convert.ToInt64(row["io_busy_current"]),
                    Value3 = Convert.ToInt64(row["current_read"]),
                    Value4 = Convert.ToInt64(row["current_write"]),
                    Value5 = Convert.ToInt64(row["packets_received_current"]),
                    Value6 = Convert.ToInt64(row["packets_sent_current"]),
                    Value7 = Convert.ToInt64(row["connections_current"]),
                    Value8 = Convert.ToInt64(row["io_busy_total"]),
                    Value9 = Convert.ToInt64(row["cpu_busy_total"]),
                    Value10 = Convert.ToInt64(row["total_read"]),
                    Value11 = Convert.ToInt64(row["total_write"]),
                    Value12 = Convert.ToInt64(row["packets_received_total"]),
                    Value13 = Convert.ToInt64(row["packets_sent_total"]),
                    Value14 = Convert.ToInt64(row["connections_total"]),
                };
                OnUpdateServerInfo(record, server, isServer);
            }
            else
            {
                var data = QueryEngine.GetDatabaseIoInfo(server);
                if (_lastPerformanceData != null)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var row = data.Rows[i];
                        var last = _lastPerformanceData.Rows[i];
                        row["CurrentNumberReads"] = Convert.ToInt64(row["NumberReads"]) - Convert.ToInt64(last["NumberReads"]);
                        row["CurrentNumberWrites"] = Convert.ToInt64(row["NumberWrites"]) - Convert.ToInt64(last["NumberWrites"]);
                    }
                }
                var db = data.Rows[0];
                record = new PerformanceRecord
                {
                    Value1 = Convert.ToInt64(db["NumberReads"]),
                    Value2 = Convert.ToInt64(db["BytesRead"]),
                    Value3 = Convert.ToInt64(db["NumberWrites"]),
                    Value4 = Convert.ToInt64(db["BytesWritten"]),
                    Value5 = Convert.ToInt64(db["CurrentNumberReads"]),
                    Value6 = Convert.ToInt64(db["CurrentNumberWrites"]),
                    Value13 = Convert.ToInt64(db["IsStall"]),
                    Value16 = Convert.ToDateTime(db["StartDate"]),
                    Value15 = Convert.ToInt64(db["FileCount"])
                };
                var hasLog = data.Rows.Count > 1;
                if (hasLog)
                {
                    var log = data.Rows[1];
                    record.Value7 = Convert.ToInt64(log["NumberReads"]);
                    record.Value8 = Convert.ToInt64(log["BytesRead"]);
                    record.Value9 = Convert.ToInt64(log["NumberWrites"]);
                    record.Value10 = Convert.ToInt64(log["BytesWritten"]);
                    record.Value11 = Convert.ToInt64(log["CurrentNumberReads"]);
                    record.Value12 = Convert.ToInt64(log["CurrentNumberWrites"]);
                    record.Value14 = Convert.ToInt64(log["IsStall"]);
                    record.Value15 += Convert.ToInt64(log["FileCount"]);
                }
                OnUpdateServerInfo(record, server, isServer);
                _lastPerformanceData = data;
            }
        }

        private void OnError(Exception ex)
        {
            if (Message != null)
                Message(this, new MessageEventArgs(ex.Message, ex is SqlException));
        }

        private void CheckMonitorItems()
        {
            try
            {
                Settings.Instance.MonitorItems.Where(i => i.IsEnabled).ForEach(item =>
                    {
                        var server = Settings.Instance.FindServer(item.Server);
                        if (server != null)
                        {
                            server.Database = "master";
                            switch (item.AlertType)
                            {
                                case AlertTypes.Sql:
                                    switch (item.CondictionType)
                                    {
                                        case 0:
                                        case 1:
                                            var sessions = SqlHelper.Query(QueryEngine.SqlProcesses, server);
                                            sessions.Rows.Cast<DataRow>().ForEach(r =>
                                                {
                                                    var id = r["spid"].ToString();
                                                    var sql = QueryEngine.GetSessionSql(id, server);
                                                    if (IsTextMatch(sql, item.Target))
                                                    {
                                                        bool result;
                                                        if (item.CondictionType == 1)
                                                        {
                                                            var span = ((DateTime)r["last_batch_end"]).Subtract((DateTime)r["last_batch_begin"]);
                                                            result = span > TimeSpan.FromSeconds(Convert.ToDouble(item.CondictionValue));
                                                            sql = span.ToString() + ", " + sql;
                                                        }
                                                        else
                                                            result = true;
                                                        if (result)
                                                        {
                                                            var key = id + "|" + r["hostname"].ToString() + "|" + r["hostprocess"].ToString();
                                                            ShowAlert(item, key, sql);
                                                        }
                                                    }
                                                });
                                            break;
                                        case 2:
                                            var tasks = SqlHelper.Query(QueryEngine.SqlWaitingTasks, server);
                                            tasks.Rows.Cast<DataRow>().ForEach(r =>
                                                {
                                                    var id = r["[Blocking Session Id]"].ToString();
                                                    if (!string.IsNullOrEmpty(id))
                                                    {
                                                        var sql = QueryEngine.GetSessionSql(id, server);
                                                        if (string.IsNullOrEmpty(sql))
                                                            sql = "session id = " + id;
                                                        ShowAlert(item, sql, sql);
                                                    }
                                                });
                                            break;
                                        case 3:
                                            var tableServer = server.Clone();
                                            tableServer.Database = item.Target;
                                            var count = QueryEngine.GetTableInfo(tableServer, "AND so.name='" + item.CondictionValue + "'");
                                            if (count != null && Convert.ToInt64(count["RowCount"]) == 0)
                                                ShowAlert(item, item.Target + QueryEngine.Dot + item.CondictionValue, item.Target + QueryEngine.Dot + item.CondictionValue);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case AlertTypes.Server:
                                    switch (item.CondictionType)
                                    {
                                        case 0:
                                            try
                                            {
                                                SqlHelper.ExecuteScalar("SELECT @@version", server);
                                            }
                                            catch (Exception ex)
                                            {
                                                ShowAlert(item, server.Server, ex.Message);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                //case AlertTypes.CPU:
                                //    var cpu = CurrentCpuUsage;
                                //    if (cpu > item.CondictionValue)
                                //        ShowAlert(item, cpu.ToString());
                                //    break;
                                //case AlertTypes.Memory:
                                //    var mem = AvailableRAM;
                                //    if (mem > item.CondictionValue)
                                //        ShowAlert(item, mem.ToString());
                                //    break;
                                //case AlertTypes.Diskspace:
                                //    break;
                                default:
                                    break;
                            }
                        }
                    });

            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void ShowAlert(MonitorItem item, string key, string currentValue)
        {
            var message = Settings.Instance.AlertTemplate;
            if (string.IsNullOrEmpty(message))
                message = Settings.DefaultTemplate;
            var type = string.Empty;
            var action = string.Empty;
            switch (item.AlertType)
            {
                case AlertTypes.Sql:
                    action = currentValue;
                    switch (item.CondictionType)
                    {
                        case 0:
                            type = "SQL executes: ";
                            break;
                        case 1:
                            type = "SQL lasts: ";
                            break;
                        case 2:
                            type = "SQL blocked: ";
                            break;
                        case 3:
                            type = "Empty table: ";
                            break;
                        default:
                            break;
                    }
                    break;
                case AlertTypes.Server:
                    break;
                //case AlertTypes.CPU:
                //    message = string.Format("CPU usage is now {0}, higher than {1}", CurrentValue, Item.CondictionValue);
                //    break;
                //case AlertTypes.Memory:
                //    message = string.Format("Available memory is now {0}, lower than {1}", CurrentValue, Item.CondictionValue);
                //    break;
                //case AlertTypes.Diskspace:
                //    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                var fullMessage = message.Replace("#Type#", type);
                fullMessage = fullMessage.Replace("#Action#", action);
                fullMessage = fullMessage.Replace("#Server#", item.Server);
                fullMessage = fullMessage.Replace("#Now#", DateTime.Now.ToString());
                NotifiedMonitorItem notified;
                if (!IsNotified(item, key, type + action, fullMessage, out notified))
                    if (Alert != null)
                        Alert(this, new AlertEventArgs(item, notified, fullMessage));
            }
        }
    }
}
