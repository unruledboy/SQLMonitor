using System.ComponentModel;
using System.Windows.Forms;

namespace Xnlab.SQLMon.UI
{
    partial class ConnectionDialog
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cboServers = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.cmdTestConnection = new System.Windows.Forms.Button();
            this.epHint = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblAuthType = new System.Windows.Forms.Label();
            this.cboAuthTypes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(462, 232);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(381, 232);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "&Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // cboServers
            // 
            this.cboServers.FormattingEnabled = true;
            this.cboServers.Location = new System.Drawing.Point(77, 76);
            this.cboServers.Name = "cboServers";
            this.cboServers.Size = new System.Drawing.Size(460, 20);
            this.cboServers.TabIndex = 1;
            this.cboServers.DropDown += new System.EventHandler(this.OnServersDropDown);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 79);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(47, 12);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "S&erver:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(12, 160);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(35, 12);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "&User:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(77, 150);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(460, 21);
            this.txtUserName.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(77, 188);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(460, 21);
            this.txtPassword.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 198);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 12);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "&Password:";
            // 
            // cmdTestConnection
            // 
            this.cmdTestConnection.Location = new System.Drawing.Point(229, 232);
            this.cmdTestConnection.Name = "cmdTestConnection";
            this.cmdTestConnection.Size = new System.Drawing.Size(117, 23);
            this.cmdTestConnection.TabIndex = 8;
            this.cmdTestConnection.Text = "&Test Connection";
            this.cmdTestConnection.UseVisualStyleBackColor = true;
            this.cmdTestConnection.Click += new System.EventHandler(this.OnTestConnectionClick);
            // 
            // epHint
            // 
            this.epHint.ContainerControl = this;
            // 
            // lblAuthType
            // 
            this.lblAuthType.AutoSize = true;
            this.lblAuthType.Location = new System.Drawing.Point(12, 121);
            this.lblAuthType.Name = "lblAuthType";
            this.lblAuthType.Size = new System.Drawing.Size(35, 12);
            this.lblAuthType.TabIndex = 2;
            this.lblAuthType.Text = "&Auth:";
            // 
            // cboAuthTypes
            // 
            this.cboAuthTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthTypes.FormattingEnabled = true;
            this.cboAuthTypes.Location = new System.Drawing.Point(77, 113);
            this.cboAuthTypes.Name = "cboAuthTypes";
            this.cboAuthTypes.Size = new System.Drawing.Size(460, 20);
            this.cboAuthTypes.TabIndex = 3;
            this.cboAuthTypes.SelectedIndexChanged += new System.EventHandler(this.OnAuthTypesSelectedIndexChanged);
            // 
            // ConnectionDialog
            // 
            this.AcceptButton = this.cmdSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 268);
            this.Controls.Add(this.lblAuthType);
            this.Controls.Add(this.cboAuthTypes);
            this.Controls.Add(this.cmdTestConnection);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.cboServers);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSave);
            this.Name = "ConnectionDialog";
            this.Text = "Connection";
            this.Controls.SetChildIndex(this.cmdSave, 0);
            this.Controls.SetChildIndex(this.cmdCancel, 0);
            this.Controls.SetChildIndex(this.cboServers, 0);
            this.Controls.SetChildIndex(this.lblServer, 0);
            this.Controls.SetChildIndex(this.lblUserName, 0);
            this.Controls.SetChildIndex(this.txtUserName, 0);
            this.Controls.SetChildIndex(this.lblPassword, 0);
            this.Controls.SetChildIndex(this.txtPassword, 0);
            this.Controls.SetChildIndex(this.cmdTestConnection, 0);
            this.Controls.SetChildIndex(this.cboAuthTypes, 0);
            this.Controls.SetChildIndex(this.lblAuthType, 0);
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cmdCancel;
        private Button cmdSave;
        private ComboBox cboServers;
        private Label lblServer;
        private Label lblUserName;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private Label lblPassword;
        private Button cmdTestConnection;
        private ErrorProvider epHint;
        private Label lblAuthType;
        private ComboBox cboAuthTypes;
    }
}