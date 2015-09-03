using System.Drawing;

namespace Xnlab.SQLMon.Controls.Tree
{
    public class TreeNode
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
    }
}
