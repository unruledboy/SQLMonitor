namespace Xnlab.SQLMon.UI
{
    public partial class ViewTextDialog : BaseDialog
    {
        public ViewTextDialog()
        {
            InitializeComponent();
        }
        public ViewTextDialog(string title, string content)
            : this()
        {
            this.Text = title;
            rtbContent.Font = Monitor.Instance.SetFont();
            rtbContent.Text = content;
            rtbContent.SelectionStart = rtbContent.Text.Length;
            rtbContent.ScrollToCaret();
        }
    }
}
