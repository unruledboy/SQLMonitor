using System.ComponentModel;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using Xnlab.SQLMon.Controls;

namespace Xnlab.SQLMon.UI
{
    partial class ProcessVisualizer
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
            this.picProcesses = new XtendPicBox();
            this.ttHint = new System.Windows.Forms.ToolTip(this.components);
            this.rtbInfo = new ICSharpCode.TextEditor.TextEditorControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmdView = new System.Windows.Forms.Button();
            this.cmdKill = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picProcesses
            // 
            this.picProcesses.AutoScroll = true;
            this.picProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picProcesses.Image = null;
            this.picProcesses.Location = new System.Drawing.Point(0, 0);
            this.picProcesses.Name = "picProcesses";
            this.picProcesses.Size = new System.Drawing.Size(778, 555);
            this.picProcesses.TabIndex = 9;
            this.picProcesses.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnProccessMouseClick);
            this.picProcesses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnProcessMouseDoubleClick);
            this.picProcesses.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnProcessesMouseMove);
            // 
            // rtbInfo
            // 
            this.rtbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbInfo.AutoScroll = true;
            this.rtbInfo.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.rtbInfo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbInfo.IsReadOnly = false;
            this.rtbInfo.Location = new System.Drawing.Point(4, 32);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(248, 520);
            this.rtbInfo.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 59);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.picProcesses);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cmdRefresh);
            this.splitContainer1.Panel2.Controls.Add(this.cmdView);
            this.splitContainer1.Panel2.Controls.Add(this.cmdKill);
            this.splitContainer1.Panel2.Controls.Add(this.rtbInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1037, 555);
            this.splitContainer1.SplitterDistance = 778;
            this.splitContainer1.TabIndex = 11;
            // 
            // cmdView
            // 
            this.cmdView.Location = new System.Drawing.Point(85, 3);
            this.cmdView.Name = "cmdView";
            this.cmdView.Size = new System.Drawing.Size(75, 23);
            this.cmdView.TabIndex = 11;
            this.cmdView.Text = "&Detail";
            this.cmdView.UseVisualStyleBackColor = true;
            this.cmdView.Click += new System.EventHandler(this.OnViewClick);
            // 
            // cmdKill
            // 
            this.cmdKill.Enabled = false;
            this.cmdKill.Location = new System.Drawing.Point(4, 3);
            this.cmdKill.Name = "cmdKill";
            this.cmdKill.Size = new System.Drawing.Size(75, 23);
            this.cmdKill.TabIndex = 11;
            this.cmdKill.Text = "&Kill";
            this.cmdKill.UseVisualStyleBackColor = true;
            this.cmdKill.Click += new System.EventHandler(this.OnKillClick);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Location = new System.Drawing.Point(166, 3);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(75, 23);
            this.cmdRefresh.TabIndex = 11;
            this.cmdRefresh.Text = "&Refresh";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.OnRefreshClick);
            // 
            // ProcessVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 614);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "ProcessVisualizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Process Visualizer";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XtendPicBox picProcesses;
        private ToolTip ttHint;
        private TextEditorControl rtbInfo;
        private SplitContainer splitContainer1;
        private Button cmdKill;
        private Button cmdView;
        private Button cmdRefresh;
    }
}