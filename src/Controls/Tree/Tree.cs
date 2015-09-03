using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Xnlab.SQLMon.Controls.Tree
{
    public class Tree
    {
        private readonly List<TreeNode> _nodes = new List<TreeNode>();

        public void AddNode(string id, string parentId, string name, string note, string description)
        {
            AddNode(id, parentId, name, note, description, Color.White);
        }

        public void AddNode(string id, string parentId, string name, string note, string description, Color backColor)
        {
            AddNode(id, parentId, name, note, description, backColor, Color.Black);
        }

        public void AddNode(string id, string parentId, string name, string note, string description, Color backColor, Color foreColor)
        {
            _nodes.Add(new TreeNode { Id = id, ParentId = parentId, Name = name, Note = note, Description = description, BackColor = backColor, ForeColor = foreColor });
        }

        public TreeNode Find(string id)
        {
            return _nodes.FirstOrDefault(n => n.Id == id);
        }

        public IEnumerable<TreeNode> Parents(string id)
        {
            return _nodes.Where(n => n.ParentId == id);
        }

        public TreeNode First
        {
            get { return _nodes.FirstOrDefault(); }
        }

        public int Count
        {
            get { return _nodes.Count; }
        }
    }
}
