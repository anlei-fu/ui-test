using System.Collections.Generic;
using System.Windows.Forms;

namespace UiTest
{
    public class TreeViewRender
    {
        private readonly TreeView _treeView;

        public TreeViewRender(TreeView treeView)
        {
            _treeView = treeView;
        }

        public void Render(Tree tree)
        {
            if (tree.Children != null)
            {
                foreach (var item in tree.Children)
                {
                    var node = _treeView.Nodes.Add(item.Key,item.Text);
                    node.Tag = item.Data;
                    RenderCore(node, item);
                }
            }
        }


        public void Clear()
        {
            _treeView.Nodes.Clear();
        }

        public List<TreeNode> GetCheckedNodes()
        {
            var ls = new List<TreeNode>();

            foreach (var item in _treeView.Nodes)
            {
                var subNode = item as TreeNode;
                ls.AddRange(GetCheckedNodeCore(subNode));
            }

            return ls;
        }

        private List<TreeNode> GetCheckedNodeCore(TreeNode node)
        {
            var ls = new List<TreeNode>();
            foreach (var item in node.Nodes)
            {
                var subNode = item as TreeNode;
                if (subNode.Checked && subNode.Nodes.Count == 0)
                {
                    ls.Add(subNode);
                }

                ls.AddRange(GetCheckedNodeCore(subNode));
            }

            return ls;
        }

        private void RenderCore(TreeNode node, Tree tree)
        {
            if (tree.Children != null)
            {
                foreach (var item in tree.Children)
                {
                    var childNode = new TreeNode(item.Text);
                    childNode.Tag = item.Data;
                    node.Nodes.Add(childNode);
                    RenderCore(childNode, item);
                }
            }
        }
    }
}
