using System;
using System.Linq;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.UI
{
    public partial class ConnectionDialog : BaseDialog
    {
        public ConnectionDialog()
        {
            InitializeComponent();

            Enum.GetValues(typeof(AuthTypes)).Cast<AuthTypes>().ForEach((s) => cboAuthTypes.Items.Add(s));
            cboAuthTypes.SelectedIndex = 0;
        }

        public ConnectionDialog(ServerInfo info)
            : this()
        {
            if (info != null)
            {
                AuthType = info.AuthType;
                Server = info.Server;
                UserName = info.User;
                Password = info.Password;
                AuthType = info.AuthType;
            }
        }

        public AuthTypes AuthType
        {
            get { return (AuthTypes)cboAuthTypes.SelectedItem; }
            set { cboAuthTypes.SelectedItem = value; }
        }

        public string Server
        {
            get { return cboServers.Text; }
            set { cboServers.Text = value; }
        }

        public string UserName
        {
            get { return txtUserName.Text; }
            set { txtUserName.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        private ServerInfo GetServerInfo
        {
            get
            {
                return new ServerInfo { AuthType = this.AuthType, Server = this.Server, User = this.UserName, Password = this.Password, Database = "master" };
            }
        }

        private void OnTestConnectionClick(object sender, EventArgs e)
        {
            if (IsSqlServer2005OrAbove())
                Monitor.Instance.ShowMessage("Connection is successful.");
        }

        private bool IsSqlServer2005OrAbove()
        {
            try
            {
                var version = QueryEngine.GetServerVersion(GetServerInfo);
                var is2005OrAbove = version >= 9;
                if (!is2005OrAbove)
                    Monitor.Instance.ShowMessage(string.Format("Current version {0}, only SQL Server 2005 or above is supported.", version));
                return is2005OrAbove;
            }
            catch (Exception ex)
            {
                Monitor.Instance.ShowMessage(ex.Message);
                return false;
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Server))
            {
                if (!string.IsNullOrEmpty(UserName) || AuthType == AuthTypes.Windows)
                {
                    if (IsSqlServer2005OrAbove())
                        this.DialogResult = DialogResult.OK;
                }
                else
                    epHint.SetError(txtUserName, "Please input user name.");
            }
            else
                epHint.SetError(cboServers, "Please input server.");
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnServersDropDown(object sender, EventArgs e)
        {
            if (cboServers.Items.Count == 0)
            {
                cboServers.Items.Clear();
                cboServers.Items.AddRange(Settings.Instance.Servers.Select((p) => p.Server).ToArray());
            }
        }

        private void OnAuthTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            var enable = (AuthTypes)cboAuthTypes.SelectedItem == AuthTypes.SqlServer;
            txtUserName.Enabled = enable;
            txtPassword.Enabled = enable;
        }
    }
}
