using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.Logic
{
    public class UpdateEventArgs : MessageEventArgs
    {
        public string Version { get; set; }
        public string Url { get; set; }

        public UpdateEventArgs(string message, bool cancel, string url, string version)
            : base(message, cancel)
        {
            this.Url = url;
            this.Version = version;
        }
    }

    public class Updater
    {
        internal event EventHandler<UpdateEventArgs> FoundNewVersion;

        private static Updater _instance = null;

        public static Updater Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Updater();
                return _instance;
            }
        }

        public void Detect()
        {
            if (FoundNewVersion != null)
            {
                Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var isNew = false;
                            var version = string.Empty;
                            var descrption = string.Empty;
                            var url = string.Empty;
                            using (var client = new WebClient())
                            {
                                var result = client.DownloadString("http://xnlab.com/base/SQLMonitorVersionInfo.txt");
                                Debug.WriteLine(result);
                                var lines = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                lines.ForEach(l =>
                                    {
                                        string key;
                                        string value;
                                        Utils.Split(l, "=", out key, out value);
                                        switch (key.ToLower())
                                        {
                                            case "version":
                                                version = value;
                                                break;
                                            case "description":
                                                descrption = value;
                                                break;
                                            case "url":
                                                url = value;
                                                break;
                                            default:
                                                break;
                                        }
                                    });
                                var currentVersion = Application.ProductVersion.Split('.');
                                var newVersion = version.Split('.');
                                for (var i = 0; i < currentVersion.Length; i++)
                                {
                                    if (Convert.ToInt32(newVersion[i]) > Convert.ToInt32(currentVersion[i]))
                                    {
                                        isNew = true;
                                        break;
                                    }
                                }
                            }
                            if (isNew && Settings.Instance.IgnoredVersionUpdate != version)
                            {
                                var e = new UpdateEventArgs(string.Format("Do you want to update to new version? \r\n\r\n{0}:\r\n{1}", version, descrption), false, url, version);
                                FoundNewVersion(this, e);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }).LogExceptions();
            }
        }
    }
}
