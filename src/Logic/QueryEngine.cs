using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.Logic
{
    internal class QueryEngine
    {
        internal const int DbStallThreshold = 20;
        internal const string Dot = ".";

        internal const string DefaultSchema = "dbo";
        internal const string SqlWaitingTasks = "SELECT session_id AS [Session Id], exec_context_id AS [Exec Context Id], wait_duration_ms AS [Wait Duration ms], wait_type AS [Wait Type], blocking_session_id AS [Blocking Session Id], blocking_exec_context_id AS [Blocking Exec Context Id], resource_description AS [Resource Description] FROM sys.dm_os_waiting_tasks WITH (NOLOCK)";
        internal const string SqlProcesses = "SELECT s.session_id AS spid, s.login_time, s.host_name AS hostname, s.host_process_id AS hostprocess, s.login_name AS loginname, CASE WHEN (s.reads + s.writes) = 0 AND r.reads IS NOT NULL THEN (r.reads + r.writes) ELSE (s.reads + s.writes) END AS physical_io, CASE WHEN s.cpu_time = 0 AND r.cpu_time IS NOT NULL THEN r.cpu_time ELSE s.cpu_time END AS cpu, s.program_name, DB_NAME(r.database_id) AS db, s.last_request_start_time AS last_batch_begin, CASE WHEN s.status = 'running' THEN GETDATE() ELSE dateadd(ms, s.cpu_time, s.last_request_end_time) END AS last_batch_end, s.status, CASE WHEN r.blocking_session_id <> 0 THEN -1 ELSE (CASE WHEN s.status = 'running' THEN 1 ELSE 0 END) END AS enabled, CASE WHEN r.percent_complete IS NULL THEN 0 ELSE r.percent_complete END AS percent_complete, r.blocking_session_id FROM sys.dm_exec_sessions s WITH (NOLOCK) LEFT JOIN sys.dm_exec_requests r WITH (NOLOCK) ON s.session_id = r.session_id WHERE s.is_user_process = 1";
        internal const string SqlJobs = "SELECT j.job_id AS spid, j.name AS program_name, CAST(a.last_executed_step_id AS nvarchar(10)) AS dbid, 0 AS cpu, 0 AS physical_io, NULL AS login_time, a.start_execution_date AS last_batch_begin, a.stop_execution_date AS last_batch_end, CASE Enabled WHEN 1 THEN 'Enabled' ELSE 'Disabled' END AS status, @@SERVERNAME AS hostname, @@SPID AS hostprocess, NULL AS cmd, SYSTEM_USER AS loginname, enabled, 0 AS percent_complete FROM msdb.dbo.sysjobs j WITH (NOLOCK) LEFT JOIN msdb.dbo.sysjobactivity a WITH (NOLOCK) on j.job_id = a.job_id WHERE (a.session_id = (SELECT max(session_id) FROM msdb.dbo.sysjobactivity WITH (NOLOCK) WHERE job_id = j.job_id AND start_execution_date IS NOT NULL)) ORDER BY program_name";
        internal const string SqlLockedObjects = @"SELECT l.request_session_id AS SPID, s.program_name AS ProgramName, DB_NAME(l.resource_database_id) AS DatabaseName, schema_name(o.schema_id) AS SchemaName, o.name AS ObjectName FROM master.sys.dm_tran_locks l
LEFT JOIN sys.all_objects o ON o.object_id = l.resource_associated_entity_id
LEFT JOIN sys.dm_exec_sessions s ON l.request_session_id = s.session_id
WHERE l.resource_associated_entity_id <> 0 AND o.name IS NOT NULL";
        //t.parent_task_address
        private const string SqlSessions = @"SELECT e.session_id, DB_NAME(r.database_id) AS database_name, ISNULL(r.status, e.status) AS session_status, e.transaction_isolation_level, ISNULL(r.open_transaction_count, 0) AS open_transaction_count, r.command, ISNULL(r.blocking_session_id, 0) AS blocking_session_id, ISNULL(r.sql_handle, c.most_recent_sql_handle) as sql_handle, 
q.text, SUBSTRING(q.text, (r.statement_start_offset/2)+1, ((CASE r.statement_end_offset WHEN -1 THEN DATALENGTH(q.text) ELSE r.statement_end_offset END - r.statement_start_offset)/2)+1) as current_statement, ISNULL(ISNULL(r.task_address, t.task_address), 0) AS task_address, 0 AS parent_task_address, ISNULL(t.worker_address, 0) AS worker_address, ISNULL(w.scheduler_address, 0) AS scheduler_address, w.status as worker_status, ISNULL(w.is_sick, 0) AS is_sick, w.is_in_cc_exception, ISNULL(w.is_fatal_exception, 0) AS is_fatal_exception, ISNULL(w.state, '') as worker_state, ISNULL(w.tasks_processed_count, 0) AS tasks_processed_count, ISNULL(w.exception_num, 0) AS exception_num, ISNULL(w.return_code, 0) AS return_code, ISNULL(t.task_state, '') AS task_state, ISNULL(s.scheduler_id, 0) AS scheduler_id, ISNULL(s.current_workers_count, 0) AS current_workers_count, ISNULL(s.active_workers_count, 0) AS active_workers_count, ISNULL(s.is_idle, 0) AS is_idle
FROM sys.dm_exec_sessions e
LEFT JOIN sys.dm_exec_requests r ON e.session_id = r.session_id
LEFT JOIN sys.dm_os_tasks t ON e.session_id = t.session_id
LEFT JOIN sys.dm_os_workers w ON t.worker_address = w.worker_address
LEFT JOIN sys.dm_os_schedulers s ON w.scheduler_address = s.scheduler_address
LEFT JOIN sys.dm_exec_connections c ON c.session_id = e.session_id
CROSS APPLY sys.dm_exec_sql_text(ISNULL(r.sql_handle, c.most_recent_sql_handle)) as q
ORDER BY e.session_id";
        //http://thesqlguy.wordpress.com/2010/11/15/sql-2005-blocking-chains-a-friendly-display-using-cte-and-recursion/
        private const string SqlLockedProcesses = @"DECLARE @processes TABLE(
	SPID SMALLINT,
	BlockingSPID SMALLINT,
	Definition VARCHAR(MAX)
)

INSERT INTO @processes
SELECT
	s.spid, 
	BlockingSPID = s.blocked, 
	Definition = CAST(text AS VARCHAR(MAX))
FROM sys.sysprocesses s WITH (nolock)
	CROSS APPLY sys.dm_exec_sql_text (sql_handle)
--WHERE s.spid > 50;

;WITH Blocking(SPID, BlockingSPID, BlockingStatement, RowNo, LevelRow)
AS
(
	SELECT
		s.SPID, 
		s.BlockingSPID, 
		s.Definition, 
		ROW_NUMBER() OVER(ORDER BY s.SPID) AS RowNo,
		0 AS LevelRow
	FROM @processes s
		INNER JOIN @processes s1 ON s.SPID = s1.BlockingSPID
	WHERE s.BlockingSPID = 0
		UNION ALL
	SELECT
		r.SPID,
		r.BlockingSPID, 
		r.Definition,
		d.RowNo,
		d.LevelRow + 1
	FROM @processes r
		INNER JOIN Blocking d ON r.BlockingSPID = d.SPID
	WHERE r.BlockingSPID > 0
)
SELECT 
	BlockingSPID, 
	SPID, 
	BlockingStatement, 
	MIN(RowNo) AS LockGroup, 
	LevelRow 
FROM Blocking
GROUP BY BlockingSPID, SPID, BlockingStatement, LevelRow
ORDER BY MIN(RowNo), LevelRow";

        internal const string SqlObjectScripts = @"Select s.name, s.create_date AS CreateDate, s.modify_date AS ModifyDate, s.type, c.text from syscomments c WITH (NOLOCK) left join sys.objects s WITH (NOLOCK) on c.id = s.object_id LEFT JOIN sysobjects o WITH (NOLOCK) ON c.id=o.id LEFT JOIN sys.schemas u WITH (NOLOCK) ON o.uid = u.schema_id";
        internal const string SqlTableInfo = @"SELECT 
[SchemaName] = su.name,
[TableName] = so.name, 
[RowCount] = MAX(si.rows) 
FROM 
sys.sysobjects so WITH (NOLOCK) LEFT JOIN
sys.schemas su WITH (NOLOCK) ON so.uid = su.schema_id LEFT JOIN
sys.sysindexes si WITH (NOLOCK) ON si.id = OBJECT_ID(su.name + '.' + so.name)
WHERE 
so.xtype = 'U' 
 {0} 
GROUP BY 
su.name, so.name
ORDER BY su.name, so.name";
        private static readonly List<string> systemDatabases = new List<string> { "master", "msdb", "model", "tempdb" };

        internal static List<string> SystemDatabases
        {
            get { return systemDatabases; }
        }

        internal static int GetServerVersion(ServerInfo server)
        {
            var version = SqlHelper.ExecuteScalar("SELECT SERVERPROPERTY('ProductVersion')", server);
            var value = version.ToString();
            var major = value.Split('.')[0];
            return Convert.ToInt32(major);
        }

        internal static DataTable GetSpScripts(ServerInfo server)
        {
            var data = SqlHelper.Query(SqlObjectScripts + " WHERE s.type = 'P' ORDER BY s.name", server);
            var result = data.Clone();
            data.Rows.Cast<DataRow>().GroupBy(r => r.Field<string>("name")).ForEach(g =>
                {
                    var text = new StringBuilder();
                    var row = result.NewRow();
                    var first = g.First();
                    row.ItemArray = first.ItemArray;
                    row["text"] = g.Aggregate(new StringBuilder(), (a, b) => a.Append("\r\n" + b.Field<string>("text")), (a) => a.Remove(0, 2).ToString());

                    result.Rows.Add(row);
                });
            return result;
        }

        internal static void GetOsInfo(ServerInfo server, out DateTime serverStartTime)
        {
            //what's wrong with the SQL Server team??? they just keep changing the column name in different versions
            //sqlserver_start_time
            var info = SqlHelper.Query("SELECT * FROM sys.dm_os_sys_info WITH (NOLOCK)", server);
            var row = info.Rows[0];
            if (info.Columns.Contains("sqlserver_start_time"))
                serverStartTime = Convert.ToDateTime(row["sqlserver_start_time"]);
            else if (info.Columns.Contains("ms_ticks"))
            {
                var startTime = row["ms_ticks"].ToString();
                var ticks = Convert.ToInt64(startTime);
                serverStartTime = DateTime.Now.AddMilliseconds(-ticks);
            }
            else
                serverStartTime = DateTime.Now;
        }

        private static long GetSizeLong(DataTable table, DataRow row, Dictionary<string, int> columns)
        {
            var result = 0L;
            foreach (var item in columns)
            {
                if (table.Columns.Contains(item.Key))
                {
                    result = Convert.ToInt64(row[item.Key]) * item.Value;
                    break;
                }
            }
            return result;
        }

        internal static void GetMemoryInfo(ServerInfo server, out long physicalMemory, out long availableMemory)
        {
            var version = GetServerVersion(server);
            if (version > 9)
            {
                var info = SqlHelper.Query("SELECT * FROM sys.dm_os_sys_memory WITH (NOLOCK)", server);
                var row = info.Rows[0];
                physicalMemory = GetSizeLong(info, row, new Dictionary<string, int> { { "total_physical_memory_kb", 1 }, { "total_physical_memory_in_bytes", Utils.Size1K } });
                physicalMemory = physicalMemory /= Utils.Size1K;

                availableMemory = GetSizeLong(info, row, new Dictionary<string, int> { { "available_physical_memory_kb", 1 }, { "available_physical_memory_in_bytes", Utils.Size1K } });
                availableMemory = availableMemory /= Utils.Size1K;
            }
            else
            {
                var info = SqlHelper.Query("SELECT * FROM sys.dm_os_sys_info WITH (NOLOCK)", server);
                var row = info.Rows[0];
                physicalMemory = GetSizeLong(info, row, new Dictionary<string, int> { { "physical_memory_kb", 1 }, { "physical_memory_in_bytes", Utils.Size1K } });
                physicalMemory = physicalMemory /= Utils.Size1K;

                availableMemory = GetSizeLong(info, row, new Dictionary<string, int> { { "committed_target_kb", 1 }, { "committed_target_in_bytes", Utils.Size1K } });
                availableMemory = availableMemory /= Utils.Size1K;
            }
            //var virtualMemory = 0L;
            //if (info.Columns.Contains("virtual_memory_kb"))
            //    virtualMemory = Convert.ToInt64(row["virtual_memory_kb"]);
            //else if (info.Columns.Contains("virtual_memory_in_bytes"))
            //    virtualMemory = Convert.ToInt64(row["virtual_memory_in_bytes"]) / Utils.Size1K;
            //virtualMemory /= Utils.Size1K;

        }

        internal static DataTable GetDatabaseInfo(ServerInfo server, string database)
        {
            if (server.IsAzure)
            {
                server.Database = database;
                return SqlHelper.Query(string.Format("SELECT '{0}' AS DatabaseName, Name AS Logical_Name, Physical_Name, CAST(size AS decimal(30,0))*8 AS Size, state, type FROM sys.database_files", database), server);
            }
            else
                return SqlHelper.Query("SELECT DB_NAME(database_id) AS DatabaseName, Name AS Logical_Name, Physical_Name, CAST(size AS decimal(30,0))*8 AS Size, state, type FROM sys.master_files WITH (NOLOCK) WHERE DB_NAME(database_id) = '" + database + "'", server);
        }

        internal static DataTable GetDatabasesInfo(ServerInfo server)
        {
            return SqlHelper.Query("SELECT * FROM sys.databases WITH (NOLOCK)", server);
        }

        internal static Dictionary<string, KeyValue<long, long>> GetDiskSpace(ServerInfo server)
        {
            var databases = GetDatabasesInfo(server);
            var files = new List<Tuple<bool, string, long>>();
            databases.AsEnumerable().ForEach(d =>
            {
                if (Convert.ToInt32(d["state"]) == 0)
                {
                    var database = GetDatabaseInfo(server, d["name"].ToString());
                    database.AsEnumerable().ForEach(f =>
                    {
                        files.Add(new Tuple<bool, string, long>(Convert.ToInt32(f["type"]) == 1, f["physical_name"].ToString(), Convert.ToInt64(Convert.ToDecimal(f["Size"]) / Utils.Size1K)));
                    }
                    );
                }
            });
            var spaces = new Dictionary<string, KeyValue<long, long>>();
            if (!server.IsAzure)
            {
                //MB free
                var driveSpaces = SqlHelper.Query("EXEC master.sys.xp_fixeddrives", server);
                driveSpaces.AsEnumerable().ForEach(s =>
                {
                    //could not use name but rather index, because the column name will change according to locale
                    spaces.Add(s[0].ToString(), new KeyValue<long, long>(Convert.ToInt64(s[1]), 0));
                });
            }
            files.ForEach(f =>
            {
                var drive = f.Item2.Substring(0, 1);
                if (spaces.ContainsKey(drive))
                {
                    spaces[drive].Value += f.Item3;
                }

            });
            return spaces;
        }

        internal static Dictionary<string, Tuple<long, long, bool>> GetDbLogSpace(ServerInfo server)
        {
            var result = new Dictionary<string, Tuple<long, long, bool>>();
            var databases = GetDatabasesInfo(server);
            //database data file & log file space
            databases.AsEnumerable().ForEach(d =>
            {
                if (Convert.ToInt32(d["state"]) == 0)
                {
                    var name = d["name"].ToString();
                    if (!SystemDatabases.Contains(name))
                    {
                        var database = GetDatabaseInfo(server, name);
                        var databaseSpace = new Dictionary<DatabaseFileTypes, long> { { DatabaseFileTypes.Data, 0 }, { DatabaseFileTypes.Log, 0 } };
                        database.AsEnumerable().ForEach(f =>
                        {
                            var key = (DatabaseFileTypes)Convert.ToInt32(f["type"]);
                            if (databaseSpace.ContainsKey(key))
                                databaseSpace[key] += Convert.ToInt64(Convert.ToDecimal(f["Size"]) / Utils.Size1K);
                        }
                        );
                        bool? shrink = null;
                        if (databaseSpace[DatabaseFileTypes.Log] > databaseSpace[DatabaseFileTypes.Data] / 100.0 * Settings.Instance.DatabaseDataLogSpaceRatio)
                            shrink = false;
                        else
                        {
                            var logSpaces = SqlHelper.Query("DBCC SQLPERF(LOGSPACE)", GetServerInfo(server, name));
                            var columnName = "DB Name";
                            if (!logSpaces.Columns.Contains(columnName))
                                columnName = "Database Name";
                            var logSpace = logSpaces.Select(string.Format("[{0}] = '{1}'", columnName, name));
                            if (logSpace.Length > 0)
                            {
                                var logSpacedUsed = Convert.ToDouble(logSpace[0]["Log Space Used (%)"]);
                                if (logSpacedUsed > Settings.Instance.DatabaseDataLogSpaceRatio)
                                    shrink = true;
                            }
                        }
                        if (shrink != null)
                            result.Add(name, new Tuple<long, long, bool>(databaseSpace[DatabaseFileTypes.Log], databaseSpace[DatabaseFileTypes.Data], (bool)shrink));
                    }
                }
            });
            return result;
        }

        internal static ServerInfo GetServerInfo(ServerInfo server, string catalog)
        {
            return new ServerInfo { IsAzure = server.IsAzure, AuthType = server.AuthType, Server = server.Server, Database = catalog, User = server.User, Password = server.Password };
        }

        internal static List<DatabaseStall> GetDatabaseStall(ServerInfo server)
        {
            var result = new List<DatabaseStall>();
            var databaseList = GetDatabasesInfo(server);
            databaseList.AsEnumerable().ForEach(db =>
            {
                if (Convert.ToInt32(db["state"]) == 0)
                {
                    var name = db["name"].ToString();
                    if (!SystemDatabases.Contains(name))
                    {
                        var databaseInfo = GetDatabaseIoInfo(GetServerInfo(server, name));
                        var row = databaseInfo.Rows[0];
                        var dbIsStall = Convert.ToInt64(row["IsStall"]);
                        var dbIsReadStall = Convert.ToInt64(row["IsReadStall"]);
                        var dbIsWriteStall = Convert.ToInt64(row["IsWriteStall"]);
                        var hasLog = databaseInfo.Rows.Count > 1;
                        long logIsStall = 0;
                        long logIsReadStall = 0;
                        long logIsWriteStall = 0;
                        if (hasLog)
                        {
                            var log = databaseInfo.Rows[1];
                            logIsStall = Convert.ToInt64(log["IsStall"]);
                            logIsReadStall = Convert.ToInt64(log["IsReadStall"]);
                            logIsWriteStall = Convert.ToInt64(log["IsWriteStall"]);
                        }
                        var stalls = new long[] { dbIsReadStall, dbIsWriteStall, logIsReadStall, logIsWriteStall };
                        var max = stalls.Max();
                        var isExceeded = max >= DbStallThreshold;
                        result.Add(new DatabaseStall { Database = name, DbReadStall = dbIsReadStall, DbWriteStall = dbIsWriteStall, LogReadStall = logIsReadStall, LogwriteStall = logIsWriteStall, Max = max, IsExceeded = isExceeded });
                    }
                }
            });
            return result;
        }

        internal static DataRow GetTableInfo(ServerInfo server, string tableName)
        {
            var data = SqlHelper.Query(string.Format(SqlTableInfo, tableName), server);
            if (data != null && data.Rows.Count > 0)
                return data.Rows[0];
            else
                return null;
        }

        internal static void GetCpuInfo(ServerInfo server, out int sqlProcess, out int systemIdle, out int otherProcesses)
        {
            var sql = @"select TOP (1) SQLProcessUtilization,
      SystemIdle,
      100 - SystemIdle - SQLProcessUtilization as OtherProcessUtilization
from (
      select
            record.value('(./Record/@id)[1]', 'int') as record_id,
            record.value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]', 'int') as SystemIdle,
            record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 'int') as SQLProcessUtilization,
            timestamp
      from (
            select timestamp, convert(xml, record) as record
            from sys.dm_os_ring_buffers
            where ring_buffer_type = N'RING_BUFFER_SCHEDULER_MONITOR'
            and record like '%<SystemHealth>%') as x
      ) as y
order by record_id desc";
            var data = SqlHelper.Query(sql, server);
            if (data.Rows.Count > 0)
            {
                var row = data.Rows[0];
                sqlProcess = row.Field<int>("SQLProcessUtilization");
                systemIdle = row.Field<int>("SystemIdle");
                otherProcesses = row.Field<int>("OtherProcessUtilization");
            }
            else
            {
                sqlProcess = 0;
                systemIdle = 0;
                otherProcesses = 0;
            }
        }

        internal static DataTable GetLockedProcesses(ServerInfo server)
        {
            return SqlHelper.Query(SqlLockedProcesses, server);
        }

        internal static DataTable GetSessions(ServerInfo server)
        {
            return SqlHelper.Query(SqlSessions, server);
        }

        internal static DataTable GetLockedObjects(short sessionId, ServerInfo server)
        {
            var sql = SqlLockedObjects + " AND l.request_session_id = " + sessionId;
            return SqlHelper.Query(sql, server);
        }

        internal static string GetSessionSql(string sessionId, ServerInfo server)
        {
            var data = SqlHelper.Query("dbcc INPUTBUFFER(" + sessionId + ")", server);
            var sql = data != null && data.Rows.Count > 0 ? (data.Rows[0][2] as string) : string.Empty;
            sql = !string.IsNullOrEmpty(sql) ? sql.Replace("\0", string.Empty) : string.Empty;
            if (!server.IsAzure)
            {
                data = SqlHelper.Query(@"declare @s nvarchar(max)
declare @handle binary(20)
declare @start int
declare @end int
select @handle = sql_handle,@start = stmt_start, @end = stmt_end from sys.sysprocesses where spid=" + sessionId + @"
select @s = text FROM sys.dm_exec_sql_text( @handle )
select @s as FullStatement, SUBSTRING(@s, (@start/2)+1, ((CASE @end WHEN -1 THEN DATALENGTH(@s) ELSE @end END - @start)/2)+1) as CurrentStatement", server);
                if (data.Rows.Count > 0)
                {
                    sql = sql.Trim();
                    var full = data.Rows[0]["FullStatement"] as string;
                    if (!string.IsNullOrEmpty(full) && !string.IsNullOrEmpty(full.Trim()))
                    {
                        full = full.Trim();
                        var statement = data.Rows[0]["CurrentStatement"] as string;
                        if (!string.IsNullOrEmpty(statement) && !string.IsNullOrEmpty(statement.Trim()))
                        {
                            statement = statement.Trim();
                            if (statement != sql)
                            {
                                var finalSql = "--actual command\r\n" + sql + "\r\n\r\n--current statement\r\n" + statement;
                                if (full != sql)
                                    finalSql += "\r\n\r\n--full sql\r\n" + full;
                                sql = finalSql;
                            }
                        }
                    }
                }
            }
            return sql;
        }

        internal static DataTable GetDatabaseIoInfo(ServerInfo server)
        {
            string sql;
            if (server.IsAzure)
                sql = "SELECT DB_ID() AS dbid, file_id AS fileid, physical_name AS filename FROM sys.database_files";
            else
                sql = string.Format(@"SELECT sys.databases.database_id AS dbid, sys.master_files.file_id AS fileid, sys.master_files.physical_name AS filename
FROM sys.master_files INNER JOIN sys.databases 
ON sys.master_files.database_id = sys.databases.database_id 
WHERE sys.databases.name = '{0}'", server.Database);
            var dataFiles = SqlHelper.Query(sql, server);
            var data = new DataTable();
            data.Columns.Add("StartDate", typeof(DateTime));
            data.Columns.Add("IsStall", typeof(double));
            data.Columns.Add("IsReadStall", typeof(double));
            data.Columns.Add("IsWriteStall", typeof(double));
            data.Columns.Add("NumberReads", typeof(long));
            data.Columns.Add("BytesRead", typeof(long));
            data.Columns.Add("NumberWrites", typeof(long));
            data.Columns.Add("BytesWritten", typeof(long));
            data.Columns.Add("CurrentNumberReads", typeof(long));
            data.Columns.Add("CurrentNumberWrites", typeof(long));
            data.Columns.Add("IsLog", typeof(bool));
            data.Columns.Add("FileCount", typeof(long));
            data.Columns.Add("IoStallReadMS", typeof(long));
            data.Columns.Add("IoStallWriteMS", typeof(long));
            for (var i = 0; i < 2; i++)
            {
                var row = data.NewRow();
                row.ItemArray = new object[] { DateTime.Now, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                row["IsLog"] = Convert.ToBoolean(i);
                data.Rows.Add(row);
            }
            dataFiles.AsEnumerable().ForEach(d =>
            {
                var dbId = d["dbid"];
                var fileId = d["fileid"];
                var fileName = d["filename"].ToString();
                var index = Path.GetExtension(fileName).ToLower() == ".ldf" ? 1 : 0;
                //sys.dm_io_virtual_file_stats()
                var fileStats = SqlHelper.Query(string.Format("SELECT DATEADD(ss, -1 * Timestamp/1000 , getdate()) AS StartDate, IoStallReadMS, IoStallWriteMS, NumberReads, BytesRead, NumberWrites, BytesWritten FROM ::fn_virtualfilestats({0}, {1})", dbId, fileId), server);
                fileStats.AsEnumerable().ForEach(f =>
                {
                    data.Rows[index]["StartDate"] = f["StartDate"];
                    data.Rows[index]["NumberReads"] = Convert.ToInt64(data.Rows[index]["NumberReads"]) + Convert.ToInt64(f["NumberReads"]);
                    if (Convert.ToInt64(data.Rows[index]["NumberReads"]) == 0)
                        data.Rows[index]["NumberReads"] = 1;
                    data.Rows[index]["BytesRead"] = Convert.ToInt64(data.Rows[index]["BytesRead"]) + Convert.ToInt64(f["BytesRead"]);
                    data.Rows[index]["NumberWrites"] = Convert.ToInt64(data.Rows[index]["NumberWrites"]) + Convert.ToInt64(f["NumberWrites"]);
                    if (Convert.ToDouble(data.Rows[index]["NumberWrites"]) == 0)
                        data.Rows[index]["NumberWrites"] = 1;
                    data.Rows[index]["BytesWritten"] = Convert.ToInt64(data.Rows[index]["BytesWritten"]) + Convert.ToInt64(f["BytesWritten"]);
                    data.Rows[index]["FileCount"] = Convert.ToInt64(data.Rows[index]["FileCount"]) + 1;
                    data.Rows[index]["IoStallReadMS"] = Convert.ToInt64(data.Rows[index]["IoStallReadMS"]) + Convert.ToInt64(f["IoStallReadMS"]);
                    data.Rows[index]["IoStallWriteMS"] = Convert.ToInt64(data.Rows[index]["IoStallWriteMS"]) + Convert.ToInt64(f["IoStallWriteMS"]);

                    data.Rows[index]["IsReadStall"] = Convert.ToDouble(data.Rows[index]["IoStallReadMS"]) / Convert.ToInt64(data.Rows[index]["NumberReads"]);
                    data.Rows[index]["IsWriteStall"] = Convert.ToDouble(data.Rows[index]["IoStallWriteMS"]) / Convert.ToInt64(data.Rows[index]["NumberWrites"]);
                    data.Rows[index]["IsStall"] = (Convert.ToDouble(data.Rows[index]["IoStallReadMS"]) + Convert.ToDouble(data.Rows[index]["IoStallWriteMS"])) / (Convert.ToInt64(data.Rows[index]["NumberReads"]) + Convert.ToInt64(data.Rows[index]["NumberWrites"]));
                });
            });
            return data;
        }

        public static string GetObjectName(object schemaName, string objectName)
        {
            if (schemaName != null && !string.IsNullOrEmpty(schemaName.ToString()))
                return string.Format("{0}{1}{2}", schemaName, Dot, objectName);
            else
                return objectName;
        }

        public static string ParseObjectName(string objectName, out string schemaName)
        {
            var pos = objectName.IndexOf(Dot);
            if (pos != -1)
            {
                schemaName = objectName.Substring(0, pos);
                return objectName.Substring(pos + 1);
            }
            else
            {
                schemaName = "dbo";
                return objectName;
            }
        }

    }
}
