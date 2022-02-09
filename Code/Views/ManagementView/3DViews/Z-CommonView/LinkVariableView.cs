using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementView.Comment;

namespace ManagementView._3DViews.CommonView
{
    public partial class LinkVariableView : UserControl
    {             
        private string linkText = "";
        /// <summary>
        /// 输出的内容
        /// </summary>
        public string LinkText
        {
            get
            {
                return linkText;
            }
            set
            {
                linkText = value;
                txtLineText.Text = linkText;
            }
        }
                        
        private string outValueType = "";
        /// <summary>
        /// 输出内容的类型 比如:Bool String Int Double
        /// </summary>
        public string OutValueType
        {
            get
            {
                return outValueType;
            }
            set
            {
                outValueType = value;
            }
        }
                        
        private string valueType = "";
        /// <summary>
        /// 设置只显示特定类型的内容 比如:Bool String Int Double
        /// </summary>
        public string ValueType
        {
            get
            {
                return valueType;
            }
            set
            {
                valueType = value;
            }
        }

        private string modelType = "";
        /// <summary>
        /// 设置只显示特定类型的Model 比如:找线 找圆
        /// </summary>
        public string ModelType
        {
            get
            {
                return modelType;
            }
            set
            {
                modelType = value;
            }
        }

        /// <summary>
        /// 设置是否显示Model下面的所有参数
        /// </summary>
        private bool showParam = true;
        public bool ShowParam
        {
            get
            {
                return showParam;
            }
            set
            {
                showParam = value;
            }
        }
        public LinkVariableView()
        {
            InitializeComponent();
            txtLineText.TextChanged += SkinTxt_TextChanged;
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            try
            {
                CommVariableView view = new CommVariableView();
                view.m_valueType = ValueType;
                view.m_modelType = ModelType;
                view.m_bShowParam = ShowParam;

                ShowControl showControl = new ShowControl(view, "链接变量");
                showControl.ShowDialog();

                if(view.OutPutValue != "")
                {
                    txtLineText.Text = view.OutPutValue;
                    OutValueType = view.OutPutType;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLineText.Clear();
        }

        private void SkinTxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LinkText = txtLineText.Text;
                LinkTextChangeEvent(txtLineText.Text, null);
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void ShowTxt(string value)
        {
            try
            {
                txtLineText.Text = value;
            }
            catch (Exception ex)
            {
                 
            }
        }


        //定义委托
        public delegate void LinkTextChange(object sender, EventArgs e);
        //定义事件
        public event LinkTextChange LinkTextChangeEvent;

        private AOSKView m_oskView;
        private void txtLineText_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalCore.Global.EnableOsk)
                {
                    return;
                }
                Rectangle rect = new Rectangle();
                rect = Screen.GetWorkingArea(this);
                if (m_oskView == null || m_oskView.IsDisposed)
                {
                    m_oskView = new AOSKView(txtLineText);
                    m_oskView.InitValue = txtLineText.Text;
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }


                    m_oskView.Location = p;
                    m_oskView.ShowDialog();
                }
                else
                {
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }
                    m_oskView.Location = p;
                    m_oskView.InitValue = txtLineText.Text;
                    m_oskView.ShowDialog();
                }
            }
            catch (Exception ex)
            {

            }
        }
         
    }
}
