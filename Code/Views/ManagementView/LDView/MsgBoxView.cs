using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView
{
    /// <summary>
    /// 弹框显示信息窗体
    /// </summary>
    public partial class MsgBoxView : Form
    {
        /// <summary>
        /// 框头显示
        /// </summary>
        string m_title = "提示";
        /// <summary>
        /// 信息内容
        /// </summary>
        string m_info = "提示内容";
        /// <summary>
        /// 是否是询问框
        /// </summary>
        EnumBox m_enumBox; 
        public MsgBoxView(string info, EnumBox enumBox = EnumBox.提示)
        {
            InitializeComponent(); 
            m_info = info;
            m_enumBox = enumBox;
        }

        private void MsgBoxView_Load(object sender, EventArgs e)
        {
            try
            {
                switch (m_enumBox)
                {
                    case EnumBox.提示:
                        m_title = "提示";
                        pictureBox1.BackgroundImage = ManagementView.Properties.Resources.信息;
                        btnConcel.Visible = false;
                        lblTitle.ForeColor = Color.DimGray;
                        lblInfo.ForeColor = Color.DimGray;
                        break;
                    case EnumBox.询问:
                        m_title = "询问";
                        pictureBox1.BackgroundImage = ManagementView.Properties.Resources.询问;
                        btnConcel.Visible = true; 
                        break;
                    case EnumBox.报警:
                        m_title = "警告";
                        pictureBox1.BackgroundImage = ManagementView.Properties.Resources.错误;
                        btnConcel.Visible = false;
                        lblTitle.ForeColor = Color.Red;
                        lblInfo.ForeColor = Color.Red;
                        break;

                    default:
                        break;
                }

                lblTitle.Text = m_title;
                lblInfo.Text = m_info;
            }
            catch (Exception ex)
            {
                 
            }
        }

        #region 拖动窗体移动
        private bool formMove = false;//窗体是否移动
        private Point formPoint;//记录窗体的位置
        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                formMove = false;//停止移动
            }
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y);
                Location = mousePos;
            }
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.FrameBorderSize.Height;//SystemInformation.CaptionHeight -
                formPoint = new Point(xOffset, yOffset);
                formMove = true;//开始移动
            }
        }
        #endregion
    }

    public enum EnumBox
    {
        提示,
        询问,
        报警
    }
}
