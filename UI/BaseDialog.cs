using System.Drawing;
using System.Windows.Forms;
using Xnlab.SQLMon.Properties;

namespace Xnlab.SQLMon.UI
{
    public partial class BaseDialog : Form
    {
        public BaseDialog()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Resources.Server2.GetHicon());
        }

        public string Description
        {
            set { lblDescription.Text = value; }
        }
    }
}
