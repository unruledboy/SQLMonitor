using System.ComponentModel;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace Xnlab.SQLMon.UI
{
    partial class UserQuery
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
            this.scQuery = new System.Windows.Forms.SplitContainer();
            this.rtbSQL = new ICSharpCode.TextEditor.TextEditorControl();
            this.cmsQuery = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmiChangeConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiOpenScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiSaveScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tiCompareScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tcQueryResult = new System.Windows.Forms.TabControl();
            this.tpData = new System.Windows.Forms.TabPage();
            this.tpInfo = new System.Windows.Forms.TabPage();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.scQuery)).BeginInit();
            this.scQuery.Panel1.SuspendLayout();
            this.scQuery.Panel2.SuspendLayout();
            this.scQuery.SuspendLayout();
            this.cmsQuery.SuspendLayout();
            this.tcQueryResult.SuspendLayout();
            this.tpInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // scQuery
            // 
            this.scQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scQuery.Location = new System.Drawing.Point(0, 0);
            this.scQuery.Name = "scQuery";
            this.scQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scQuery.Panel1
            // 
            this.scQuery.Panel1.Controls.Add(this.rtbSQL);
            // 
            // scQuery.Panel2
            // 
            this.scQuery.Panel2.AutoScroll = true;
            this.scQuery.Panel2.Controls.Add(this.tcQueryResult);
            this.scQuery.Panel2.Margin = new System.Windows.Forms.Padding(2);
            this.scQuery.Size = new System.Drawing.Size(724, 418);
            this.scQuery.SplitterDistance = 150;
            this.scQuery.TabIndex = 1;
            // 
            // rtbSQL
            // 
            this.rtbSQL.AllowDrop = true;
            this.rtbSQL.AutoScroll = true;
            this.rtbSQL.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbSQL.ContextMenuStrip = this.cmsQuery;
            this.rtbSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSQL.IsReadOnly = false;
            this.rtbSQL.Location = new System.Drawing.Point(0, 0);
            this.rtbSQL.Name = "rtbSQL";
            this.rtbSQL.Size = new System.Drawing.Size(724, 150);
            this.rtbSQL.TabIndex = 0;
            this.rtbSQL.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnContentDragDrop);
            this.rtbSQL.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnContentDragEnter);
            // 
            // cmsQuery
            // 
            this.cmsQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiChangeConnection,
            this.tmiOpenScript,
            this.tmiSaveScript,
            this.tiCompareScript});
            this.cmsQuery.Name = "cmsQuery";
            this.cmsQuery.Size = new System.Drawing.Size(190, 92);
            // 
            // tmiChangeConnection
            // 
            this.tmiChangeConnection.Name = "tmiChangeConnection";
            this.tmiChangeConnection.Size = new System.Drawing.Size(189, 22);
            this.tmiChangeConnection.Text = "&Change Connection";
            this.tmiChangeConnection.Click += new System.EventHandler(this.OnChangeConnectionClick);
            // 
            // tmiOpenScript
            // 
            this.tmiOpenScript.Name = "tmiOpenScript";
            this.tmiOpenScript.Size = new System.Drawing.Size(189, 22);
            this.tmiOpenScript.Text = "&Open Script";
            this.tmiOpenScript.Click += new System.EventHandler(this.OnOpenScriptClick);
            // 
            // tmiSaveScript
            // 
            this.tmiSaveScript.Name = "tmiSaveScript";
            this.tmiSaveScript.Size = new System.Drawing.Size(189, 22);
            this.tmiSaveScript.Text = "&Save to File";
            this.tmiSaveScript.Click += new System.EventHandler(this.OnSaveScriptClick);
            // 
            // tiCompareScript
            // 
            this.tiCompareScript.Name = "tiCompareScript";
            this.tiCompareScript.Size = new System.Drawing.Size(189, 22);
            this.tiCompareScript.Text = "Compa&re";
            this.tiCompareScript.Click += new System.EventHandler(this.OnCompareScriptClick);
            // 
            // tcQueryResult
            // 
            this.tcQueryResult.Controls.Add(this.tpData);
            this.tcQueryResult.Controls.Add(this.tpInfo);
            this.tcQueryResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcQueryResult.Location = new System.Drawing.Point(0, 0);
            this.tcQueryResult.Name = "tcQueryResult";
            this.tcQueryResult.SelectedIndex = 0;
            this.tcQueryResult.Size = new System.Drawing.Size(724, 264);
            this.tcQueryResult.TabIndex = 0;
            // 
            // tpData
            // 
            this.tpData.Location = new System.Drawing.Point(4, 22);
            this.tpData.Name = "tpData";
            this.tpData.Padding = new System.Windows.Forms.Padding(3);
            this.tpData.Size = new System.Drawing.Size(716, 238);
            this.tpData.TabIndex = 0;
            this.tpData.Text = "Data";
            this.tpData.UseVisualStyleBackColor = true;
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.rtbInfo);
            this.tpInfo.Location = new System.Drawing.Point(4, 22);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpInfo.Size = new System.Drawing.Size(716, 238);
            this.tpInfo.TabIndex = 1;
            this.tpInfo.Text = "Info";
            this.tpInfo.UseVisualStyleBackColor = true;
            // 
            // rtbInfo
            // 
            this.rtbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.Location = new System.Drawing.Point(3, 3);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(710, 232);
            this.rtbInfo.TabIndex = 0;
            this.rtbInfo.Text = "";
            // 
            // UserQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scQuery);
            this.Name = "UserQuery";
            this.Size = new System.Drawing.Size(724, 418);
            this.scQuery.Panel1.ResumeLayout(false);
            this.scQuery.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scQuery)).EndInit();
            this.scQuery.ResumeLayout(false);
            this.cmsQuery.ResumeLayout(false);
            this.tcQueryResult.ResumeLayout(false);
            this.tpInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer scQuery;
        private ContextMenuStrip cmsQuery;
        private ToolStripMenuItem tmiChangeConnection;
        private ToolStripMenuItem tmiOpenScript;
        private ToolStripMenuItem tmiSaveScript;
        private TabControl tcQueryResult;
        private TabPage tpData;
        private TabPage tpInfo;
        private RichTextBox rtbInfo;
        private TextEditorControl rtbSQL;
        private ToolStripMenuItem tiCompareScript;
    }
}
