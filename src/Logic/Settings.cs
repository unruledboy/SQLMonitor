using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.Logic
{
    public enum AnalysisTypes
    {
        DatabasesSpace,
        TablesSpace,
        Performance,
        LockedObjects,
        IndexUsage,
        LogicFault,
        WaitingTasks,
        ExecutionCount,
        Io,
        Cpu,
    }

    public enum ActivityTypes
    {
        Process = 0,
        Job = 1
    }

    public enum AlertTypes
    {
        Sql = 0,
        Server = 1,
        //CPU = 2,
        //Memory = 3,
        //Diskspace = 4
    }

    public enum AuthTypes
    {
        Windows = 1,
        SqlServer = 0
    }

    public enum AnalysisResultTypes
    {
        DiskFreeSpace = 1,
        DatabaseLogSpace = 2,
        TableIndexSpace = 3,
        TableIndexUsage = 4,
        Performance = 5,
        Fault = 6,
        None = 0
    }

    public enum DatabaseFileTypes : int
    {
        Data = 0,
        Log = 1
    }

    public enum TableIndexSpaceRules : int
    {
        DataIndexSpaceRatio = 0,
        DatabaseTableSpaceRatio = 1,
        IndexEfficency = 2
    }

    public class AnalysisResult
    {
        public AnalysisResultTypes ResultType { get; set; }
        public string ObjectName { get; set; }
        public decimal ReferenceValue { get; set; }
        public decimal CurrentValue { get; set; }
        public string Factor { get; set; }
        public long Key { get; set; }
        public string RuntimeValue { get; set; }
    }

    [Serializable]
    public class ServerInfoEx : ServerInfo
    {
        public bool IsServer { get; set; }
    }

    [Serializable]
    public class ServerInfo
    {
        public AuthTypes AuthType { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsEncrypted { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        internal bool IsAzure { get; set; }

        public ServerInfo()
        {
            IsEncrypted = false;
        }

        public override string ToString()
        {
            return Server + "," + Database + "," + User;
        }

        public ServerInfo Clone()
        {
            return new ServerInfo { IsAzure = this.IsAzure, Server = this.Server, AuthType = this.AuthType, Database = this.Database, User = this.User, Password = this.Password, IsEncrypted = this.IsEncrypted };
        }
    }

    [Serializable]
    public class MonitorItem
    {
        public string Server { get; set; }
        public AlertTypes AlertType { get; set; }
        public string Target { get; set; }
        public int CondictionType { get; set; }
        public string CondictionValue { get; set; }
        public AlertMethods AlertMethod { get; set; }
        public string Title { get; set; }
        public bool IsEnabled { get; set; }

        public MonitorItem()
        {
            IsEnabled = true;
        }

        public override string ToString()
        {
            return Server + ", " + AlertType + ", " + CondictionType + ", " + CondictionValue + ", " + Target;
        }
    }

    public enum HealthTypes
    {
        ServerCpu = 0,
        ServerMemory = 1,
        ServerSpace = 2,
        DatabaseLogSpace = 3,
        DatabaseStall = 4,
        BlockedProcess = 5,
        LockedObjects = 6
    }

    [Serializable]
    public class DatabaseStall
    {
        public string Database { get; set; }
        public long DbReadStall { get; set; }
        public long DbWriteStall { get; set; }
        public long LogReadStall { get; set; }
        public long LogwriteStall { get; set; }
        public long Max { get; set; }
        public bool IsExceeded { get; set; }
    }

    [Serializable]
    public class HealthItem
    {
        public string Category { get; set; }
        public HealthTypes HealthType { get; set; }
        public string ItemName { get; set; }
        public string CurrentValue { get; set; }
        public string ReferenceValue { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
    }

    public enum RecentObjectTypes
    {
        EditData = 0,
        FilePath = 1,
        Other = 2
    }

    [Serializable]
    public class RecentObject
    {
        public RecentObjectTypes RecentObjectType { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
    }

    [Serializable]
    public class NotifiedMonitorItem
    {
        public string Server { get; set; }
        public string CurrentValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    internal class ServerState : ServerInfo
    {
        internal string Key { get; set; }
        internal bool IsReady { get; set; }
        internal bool State { get; set; }
    }

    public enum ActivityStatuses
    {
        Running = 0,
        Sleeping = 1,
        Suspended = 2,
        Background = 3,
        Runnable = 4,
        All = 5
    }

    public enum AlertMethods
    {
        MsgBox = 0,
        Mail = 1,
        Log = 2,
    }

    [Serializable]
    public class SerializableFont
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public bool Bold { get; set; }
    }

    [Serializable]
    public class Settings
    {
        public const string DefaultTemplate = "#Type# #Action# \r\n raised on #Server# at #Now#";
        public List<ServerInfo> Servers = new List<ServerInfo>();
        public List<NotifiedMonitorItem> NotifiedAlerts = new List<NotifiedMonitorItem>();
        public List<MonitorItem> MonitorItems = new List<MonitorItem>();
        public List<ServerInfoEx> PerformanceItems = new List<ServerInfoEx>();
        public ActivityStatuses ActivityState { get; set; }
        public string LastQuery { get; set; }
        public string LastServer { get; set; }
        public List<string> SearchHistories = new List<string>();
        public string LastSearchContent { get; set; }
        public bool LastSearchIsCaseSenstive { get; set; }
        public bool LastSearchIsObject { get; set; }
        public ActivityTypes ActivityType { get; set; }
        public int MonitorRefreshInterval { get; set; }
        public int PerformanceInterval { get; set; }
        public SerializableFont EditorFont { get; set; }
        public bool LogHistory { get; set; }
        public bool AutoWordWrap { get; set; }
        public string AlertMailServer { get; set; }
        public string AlertMailUser { get; set; }
        public string AlertMailPassword { get; set; }
        public string AlertMailReceiver { get; set; }
        public string AlertTemplate { get; set; }
        public string VersionControlTableName { get; set; }
        public string VersionControlTriggerName { get; set; }
        public int ConnectionTimeout { get; set; }
        public int ObjectsSplitterDistance { get; set; }
        public int MainSplitterDistance { get; set; }
        public int DatabaseDiskFreeSpaceRatio { get; set; }
        public int DatabaseDataLogSpaceRatio { get; set; }
        public int TableDataIndexSpaceRatio { get; set; }
        public List<RecentObject> RecentObjects = new List<RecentObject>();
        public int FreeMemoryRatio { get; set; }
        public int FreeCpuRatio { get; set; }
        public string IgnoredVersionUpdate { get; set; }

        public Settings()
        {
            ActivityState = ActivityStatuses.All;
            EditorFont = new SerializableFont { Name = "Tahoma", Size = 10, Bold = false };
            ActivityType = ActivityTypes.Process;
            VersionControlTableName = "SQLMonSystemObjectVersionControls";
            VersionControlTriggerName = "trg_SQLMonSystemObjectVersionControls";
            LastSearchIsObject = true;
            MonitorRefreshInterval = 10;
            PerformanceInterval = 5;
            LogHistory = true;
            AutoWordWrap = true;
            ConnectionTimeout = 30;
            DatabaseDiskFreeSpaceRatio = 30;
            DatabaseDataLogSpaceRatio = 40;
            TableDataIndexSpaceRatio = 100;
            FreeMemoryRatio = 20;
            FreeCpuRatio = 20;
            IgnoredVersionUpdate = string.Empty;
        }

        private static Settings _settings = null;

        public static string SettingsFile
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\SQLMon.cfg"; }
        }

        internal static string Title
        {
            get { return ((Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]) as AssemblyTitleAttribute).Title; }
        }

        internal void AddPerformanceItem(ServerInfo server, bool isServer)
        {
            if (FindPerformanceServer(server, isServer) == null)
                PerformanceItems.Add(new ServerInfoEx { Database = server.Database, Server = server.Server, IsServer = isServer });
        }

        internal void RemovePerformanceItem(ServerInfo server, bool isServer)
        {
            var item = FindPerformanceServer(server, isServer);
            if (item != null)
                PerformanceItems.Remove(item);
        }

        private ServerInfoEx FindPerformanceServer(ServerInfo server, bool isServer)
        {
            return PerformanceItems.FirstOrDefault(s => s.Server == server.Server
                && s.Database == server.Database
                && s.IsServer == isServer);
        }

        public static Settings Instance
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new Settings();
                    if (File.Exists(SettingsFile))
                    {
                        var serializer = new XmlSerializer(typeof(Settings));
                        using (var reader = File.OpenText(SettingsFile))
                        {
                            _settings = (Settings)serializer.Deserialize(reader);

                            _settings.Servers.ForEach(s =>
                                {
                                    if (s.IsEncrypted)
                                    {
                                        try
                                        {
                                            s.Password = AES.Decrypt(s.Password);
                                            s.IsEncrypted = false;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                });
                        }
                    }
                }
                return _settings;
            }
        }

        public void Save()
        {
            if (File.Exists(SettingsFile))
                File.Delete(SettingsFile);
            var serializer = new XmlSerializer(typeof(Settings));
            var settings = Utils.CloneObject(this);
            using (var writer = File.OpenWrite(SettingsFile))
            {
                settings.Servers.ForEach(s =>
                {
                    if (!s.IsEncrypted)
                    {
                        s.Password = AES.Encrypt(s.Password);
                        s.IsEncrypted = true;
                    }
                });
                serializer.Serialize(writer, settings);
            }
        }

        public ServerInfo FindServer(string server)
        {
            return Servers.FirstOrDefault((s) => s.Server.ToLower() == server.ToLower());
        }

        public ServerInfo FindServer(string server, string user)
        {
            return Servers.FirstOrDefault((s) => s.Server.ToLower() == server.ToLower()
                    && s.User == user);
        }
    }
}
