using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.UI
{
    public partial class ContentDialog : BaseDialog
    {
        private readonly bool _isLoading = true;

        public ContentDialog()
        {
            InitializeComponent();
        }

        public ContentDialog(string content, bool isCaseSenstive, bool isObject, List<string> items)
            : this()
        {
            txtContent.Text = content;
            chkCaseSenstive.Checked = isCaseSenstive;
            if (isObject)
                rbSearchTypeObject.Checked = true;
            else
                rbSearchTypeContent.Checked = true;
            if (items.Count == 0 && content != null)
                items.Add(content);
            items.Where(f => f != null).Reverse().ForEach(i => cboHistories.Items.Add(i));
            if (cboHistories.Items.Count > 0)
                cboHistories.SelectedIndex = 0;
            _isLoading = false;
            txtContent.Focus();
        }

        public string Content
        {
            get { return txtContent.Text; }
        }

        public bool IsObject
        {
            get { return rbSearchTypeObject.Checked; }
        }

        public bool IsCaseSenstive
        {
            get { return chkCaseSenstive.Checked; }
        }

        private void OnGoClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtContent.Text))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
                epHint.SetError(txtContent, "Please input content to search.");
        }

        private void OnHistoriesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
                txtContent.Text = cboHistories.SelectedItem.ToString();
        }

        private void OnContentDragDrop(object sender, DragEventArgs e)
        {
            Utils.SetDragDropContent(txtContent, e);
        }

        private void OnContentDragEnter(object sender, DragEventArgs e)
        {
            Utils.HandleContentDragEnter(e);
        }
    }
}
