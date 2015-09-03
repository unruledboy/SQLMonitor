using System;
using System.Windows.Forms;

namespace Xnlab.SQLMon.UI
{
    public partial class FileNameDialog : BaseDialog
    {
        private readonly bool _isSave;

        public FileNameDialog()
        {
            InitializeComponent();
        }

        public FileNameDialog(string title, bool isSave, string name)
            : this()
        {
            _isSave = isSave;
            this.Text = title;
            txtName.Text = name;
        }

        public string FilePath
        {
            get { return txtFile.Text; }
        }

        public string ObjectName
        {
            get { return txtName.Text; }
        }

        private void OnGoClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFile.Text))
            {
                if (!string.IsNullOrEmpty(txtName.Text))
                    this.DialogResult = DialogResult.OK;
                else
                    epHint.SetError(txtFile, "Please input name.");
            }
            else
                epHint.SetError(txtFile, "Please input file.");
        }

        private void OnChooseFileClick(object sender, EventArgs e)
        {
            FileDialog dlg;
            if (_isSave)
                dlg = new SaveFileDialog();
            else
                dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                txtFile.Text = dlg.FileName;
        }
    }
}
