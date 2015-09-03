using System.ComponentModel;
using System.Windows.Forms;

namespace Xnlab.SQLMon.UI
{
    partial class FileNameDialog
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmdChooseFile = new System.Windows.Forms.Button();
            this.cmdGo = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.epHint = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(9, 76);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(25, 12);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "&File:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 109);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(32, 12);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "&Name";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(70, 73);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(451, 22);
            this.txtFile.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(70, 106);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(451, 22);
            this.txtName.TabIndex = 3;
            // 
            // cmdChooseFile
            // 
            this.cmdChooseFile.Location = new System.Drawing.Point(534, 73);
            this.cmdChooseFile.Name = "cmdChooseFile";
            this.cmdChooseFile.Size = new System.Drawing.Size(40, 23);
            this.cmdChooseFile.TabIndex = 4;
            this.cmdChooseFile.Text = "...";
            this.cmdChooseFile.UseVisualStyleBackColor = true;
            this.cmdChooseFile.Click += new System.EventHandler(this.OnChooseFileClick);
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(416, 145);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(75, 23);
            this.cmdGo.TabIndex = 5;
            this.cmdGo.Text = "&Go";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.OnGoClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(497, 145);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // epHint
            // 
            this.epHint.ContainerControl = this;
            // 
            // FileNameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(584, 180);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.cmdChooseFile);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileNameDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database";
            ((System.ComponentModel.ISupportInitialize)(this.epHint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblFileName;
        private Label lblName;
        private TextBox txtFile;
        private TextBox txtName;
        private Button cmdChooseFile;
        private Button cmdGo;
        private Button cmdCancel;
        private ErrorProvider epHint;
    }
}