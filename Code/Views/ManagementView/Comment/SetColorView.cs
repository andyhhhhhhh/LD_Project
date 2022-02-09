using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class SetColorView : UserControl
    {
        string m_DrawColor = "#FF0000";
        string m_FillType = "margin";
        int m_ContourWidth = 1;

        public string DrawColor
        {
            get
            {
                return m_DrawColor;
            }
            set
            {
                m_DrawColor = value;
                try
                { 
                    btnColor.BackColor = ColorTranslator.FromHtml(m_DrawColor);
                }
                catch (Exception ex)
                {
                     
                }
            }
        }

        public string FillType
        {
            get
            {
                return m_FillType;
            }
            set
            {
                m_FillType = value;
                chkIsFill.Checked = m_FillType == "fill";
            }
        }

        public int ContourWidth
        {
            get
            {
                return m_ContourWidth;
            }
            set
            {
                m_ContourWidth = value;
                numContourWidth.Value = m_ContourWidth;
            }
        }
         
        public SetColorView()
        {
            InitializeComponent();
        }

        private void SetColorView_Load(object sender, EventArgs e)
        {
             
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true; //是否显示ColorDialog有半部分
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应
            {
                Color color_from = colorDialog.Color;
                btnColor.BackColor = color_from;
                string drawColor = color_from.ToArgb().ToString("X02");
                drawColor = drawColor.Substring(2);
                DrawColor = drawColor.Insert(0, "#");
            }
        } 

        private void chkIsFill_CheckedChanged(object sender, EventArgs e)
        {
            FillType = chkIsFill.Checked ? "fill" : "margin";
        }

        private void numContourWidth_ValueChanged(object sender, EventArgs e)
        {
            ContourWidth = (int)numContourWidth.Value;
        }
         
    }
}
