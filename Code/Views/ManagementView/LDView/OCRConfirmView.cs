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
    public partial class OCRConfirmView : Form
    {
        string m_testOcr = "";

        private string inputOcr = "";
        /// <summary>
        /// 操作员输入正确OCR
        /// </summary>
        public string InputOcr
        {
            get
            {
                return inputOcr;
            }
            set
            {
                inputOcr = value;
            }
        }
        public OCRConfirmView(string testOcr)
        {
            InitializeComponent();
            m_testOcr = testOcr;
        }

        private void OCRConfirmView_Load(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(m_testOcr))
                {
                    txtTestOCR.Text = m_testOcr;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                InputOcr = txtInputOCR.Text;
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
}
