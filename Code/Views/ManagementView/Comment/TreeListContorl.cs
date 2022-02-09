using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseModels;
using Infrastructure.DBCore;

namespace ManagementView.Comment
{
    public partial class TreeListContorl : UserControl
    {

        #region 扩展属性

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("设置是否可以使用右键")]
        public bool EnableRightButton
        {
            set
            {
                m_EnableRightButton = value;
            }
            get { return m_EnableRightButton; }
        }
        private bool m_EnableRightButton;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("设置是否可以使用右键")]
        public ContextMenuStrip RightContextMenuStrip
        {
            get { return m_RightContextMenuStrip; }
            set { m_RightContextMenuStrip = value; }
        }
        private ContextMenuStrip m_RightContextMenuStrip;

        public TreeNodeCollection TreeNodes
        {
            get { return this.treeView1.Nodes; }
        }

        #endregion

        #region 扩展事件

        public event EventHandler<TreeNodeMouseClickEventArgs> TreeNodeSelected;
        protected void OnTreeNodeSelected(BaseEntity model, TreeNodeMouseClickEventArgs e)
        {
            EventHandler<TreeNodeMouseClickEventArgs> handler = TreeNodeSelected;
            if (handler != null)
            {
                handler(model, e);
            }
        }

        public event EventHandler<NodeLabelEditEventArgs> AfterLabelEdit;
        protected void OnAfterLabelEdit(TreeNode node, NodeLabelEditEventArgs e)
        {
            EventHandler<NodeLabelEditEventArgs> handler = AfterLabelEdit;
            if (handler != null)
            {
                handler(node, e);
            }
        }

        #endregion

        #region 全局变量

        private TreeNode m_CurrentNode;

        public TreeView GetTreeView
        {
            get { return treeView1; }
        }

        #endregion

        public TreeListContorl()
        {
            InitializeComponent();
            treeView1.LabelEdit = true;
        }

        #region 初始化左边工具树节点
        public void InitTreeView<T>(List<T> models) where T : BaseEntity
        {
            try
            {
                treeView1.Nodes.Clear();
                //算法, 参数，相机，控制类
                TreeNode node = null;
                TreeNode childNode = new TreeNode();
                childNode.Text = "控制单元";
                treeView1.Nodes.Add(childNode);
                foreach (BaseEntity item in models)
                {
                    node = new TreeNode();
                    node.Text = item.Name;
                    node.Tag = item;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    childNode.Nodes.Add(node);
                }

                foreach (TreeNode item in treeView1.Nodes)
                {
                    item.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InitTreeView(List<TreeCombineNode> treeCombineNodes, string rootName = "控制单元")
        {
            try
            {
                treeView1.Nodes.Clear();
                //算法, 参数，相机，控制类
                TreeNode node = null;
                TreeNode rootNode = new TreeNode();
                rootNode.Text = rootName;
                treeView1.Nodes.Add(rootNode);
                foreach (var nodeEntity in treeCombineNodes)
                {
                    node = new TreeNode();
                    if (!string.IsNullOrEmpty(nodeEntity.Name))
                    {
                        node.Text = nodeEntity.Name;
                    }
                    else
                    {
                        node.Text = nodeEntity.RootNode.Name;
                    }
                    node.Tag = nodeEntity;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    foreach (var item in nodeEntity.ChildrenNodes)
                    {
                        SetInnerNode(node, item);
                    }
                    rootNode.Nodes.Add(node);
                }

                foreach (TreeNode item in treeView1.Nodes)
                {
                    item.ExpandAll();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetInnerNode(TreeNode parent, TreeCombineNode treeCombineNode)
        {
            try
            {
                TreeNode parentNode = new TreeNode();
                if (treeCombineNode == null)
                {
                    return;
                }
                if (treeCombineNode.RootNode != null)
                {
                    if (!string.IsNullOrEmpty(treeCombineNode.NodeDescrption))
                    {
                        parentNode.Text = treeCombineNode.NodeDescrption;
                    }
                    else
                    {
                        parentNode.Text = treeCombineNode.RootNode.Name;
                    }
                    parentNode.Tag = treeCombineNode;
                    parentNode.ImageIndex = 1;
                    parentNode.SelectedImageIndex = 1;
                    if (treeCombineNode.ChildrenNodes != null && treeCombineNode.ChildrenNodes.Count > 0)
                    {
                        foreach (var item in treeCombineNode.ChildrenNodes)
                        {
                            SetInnerNode(parentNode, item);
                        }
                    }
                    //else//最后没有子节点了，增加描述
                    //{
                    //    if (!string.IsNullOrEmpty(treeCombineNode.NodeDescrption))
                    //    {
                    //        parentNode.Text = treeCombineNode.NodeDescrption;
                    //    }
                    //}
                    parent.Nodes.Add(parentNode);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 公共方法
         
        #endregion

        #region 事件

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                m_CurrentNode = e.Node;
                if (e.Button == MouseButtons.Right)
                {
                    if (m_EnableRightButton)
                    {
                        if (((System.Windows.Forms.TreeView)sender).SelectedNode != null)
                        {
                            ((System.Windows.Forms.TreeView)sender).SelectedNode.ContextMenuStrip = m_RightContextMenuStrip;
                            object tag = e.Node.Tag;
                            m_RightContextMenuStrip.Tag = tag;
                        }
                    }
                    treeView1.SelectedNode = e.Node;
                }
                OnTreeNodeSelected(e.Node.Tag as BaseEntity, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                var node = e.Node;
                if (!string.IsNullOrEmpty(e.Label))
                {
                    node.Text = e.Label;
                }
                OnAfterLabelEdit(node, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    public class TreeCombineNode : BaseEntity
    {
        public TreeCombineNode()
        {
            ChildrenNodes = new List<TreeCombineNode>();
        }
        public BaseEntity RootNode { get; set; }

        /// <summary>
        /// 节点描述，不添加描述时，节点显示对象的名称
        /// </summary>
        public string NodeDescrption { get; set; }

        public List<TreeCombineNode> ChildrenNodes { get; set; }
    }

}
