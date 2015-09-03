using System.ComponentModel;
using System.Windows.Forms;

namespace Xnlab.SQLMon.UI
{
    partial class ContentDialog
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
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.cmdGo = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.epHint = new System.Windows.Forms.ErrorProvider(this.components);
            this.rbSearchTypeObject = new System.Windows.Forms.RadioButton();
            this.rbSearchTypeContent = new System.Windows.Forms.RadioButton();
            this.chkCaseSenstive = new System.Windows.Forms.CheckBox();
            this.cboHistories = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).BeginInit();
            this.SuspendLayout();
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(7, 105);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(51, 12);
            this.lblContent.TabIndex = 2;
            this.lblContent.Text = "&Keyword:";
            // 
            // txtContent
            // 
            this.txtContent.AllowDrop = true;
            this.txtContent.Location = new System.Drawing.Point(65, 102);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(507, 156);
            this.txtContent.TabIndex = 3;
            this.txtContent.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnContentDragDrop);
            this.txtContent.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnContentDragEnter);
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(411, 274);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(75, 23);
            this.cmdGo.TabIndex = 7;
            this.cmdGo.Text = "&Find";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.OnGoClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(492, 274);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // epHint
            // 
            this.epHint.ContainerControl = this;
            // 
            // rbSearchTypeObject
            // 
            this.rbSearchTypeObject.AutoSize = true;
            this.rbSearchTypeObject.Checked = true;
            this.rbSearchTypeObject.Location = new System.Drawing.Point(65, 274);
            this.rbSearchTypeObject.Name = "rbSearchTypeObject";
            this.rbSearchTypeObject.Size = new System.Drawing.Size(83, 16);
            this.rbSearchTypeObject.TabIndex = 4;
            this.rbSearchTypeObject.TabStop = true;
            this.rbSearchTypeObject.Text = "&Object Name";
            this.rbSearchTypeObject.UseVisualStyleBackColor = true;
            // 
            // rbSearchTypeContent
            // 
            this.rbSearchTypeContent.AutoSize = true;
            this.rbSearchTypeContent.Location = new System.Drawing.Point(180, 274);
            this.rbSearchTypeContent.Name = "rbSearchTypeContent";
            this.rbSearchTypeContent.Size = new System.Drawing.Size(50, 16);
            this.rbSearchTypeContent.TabIndex = 5;
            this.rbSearchTypeContent.Text = "&Script";
            this.rbSearchTypeContent.UseVisualStyleBackColor = true;
            // 
            // chkCaseSenstive
            // 
            this.chkCaseSenstive.AutoSize = true;
            this.chkCaseSenstive.Location = new System.Drawing.Point(290, 274);
            this.chkCaseSenstive.Name = "chkCaseSenstive";
            this.chkCaseSenstive.Size = new System.Drawing.Size(87, 16);
            this.chkCaseSenstive.TabIndex = 6;
            this.chkCaseSenstive.Text = "C&ase Senstive";
            this.chkCaseSenstive.UseVisualStyleBackColor = true;
            // 
            // cboHistories
            // 
            this.cboHistories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHistories.FormattingEnabled = true;
            this.cboHistories.Location = new System.Drawing.Point(65, 70);
            this.cboHistories.Name = "cboHistories";
            this.cboHistories.Size = new System.Drawing.Size(507, 20);
            this.cboHistories.TabIndex = 1;
            this.cboHistories.SelectedIndexChanged += new System.EventHandler(this.OnHistoriesSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Histories:";
            // 
            // ContentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(584, 307);
            this.Controls.Add(this.cboHistories);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.chkCaseSenstive);
            this.Controls.Add(this.rbSearchTypeContent);
            this.Controls.Add(this.rbSearchTypeObject);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.label1);
            this.Name = "ContentDialog";
            this.Text = "Database";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblContent, 0);
            this.Controls.SetChildIndex(this.cmdGo, 0);
            this.Controls.SetChildIndex(this.cmdCancel, 0);
            this.Controls.SetChildIndex(this.rbSearchTypeObject, 0);
            this.Controls.SetChildIndex(this.rbSearchTypeContent, 0);
            this.Controls.SetChildIndex(this.chkCaseSenstive, 0);
            this.Controls.SetChildIndex(this.txtContent, 0);
            this.Controls.SetChildIndex(this.cboHistories, 0);
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblContent;
        private TextBox txtContent;
        private Button cmdGo;
        private Button cmdCancel;
        private ErrorProvider epHint;
        private RadioButton rbSearchTypeContent;
        private RadioButton rbSearchTypeObject;
        private CheckBox chkCaseSenstive;
        private ComboBox cboHistories;
        private Label label1;
    }
}