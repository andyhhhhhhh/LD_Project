using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalCore;

namespace ManagementView.EditView
{
    public partial class EButtonPro : UserControl
    {
        public EButtonPro()
        {
            InitializeComponent();
        }


        private Global.EnumEProcess eText;
        /// <summary>
        /// Button设置的流程信息
        /// </summary>
        [Description("设置的执行流程选择")]
        [Browsable(true)]
        public Global.EnumEProcess EText
        {
            get { return eText; }
            set
            {
                eText = value;
                buttonX1.Tooltip = value.ToString();
            }
        }

        private string sText;
        /// <summary>
        /// Button显示的text信息
        /// </summary>
        [Description("按钮上面显示的信息")]
        [Browsable(true)]
        public string SText
        {
            get { return sText; }
            set
            {
                sText = value;
                if(string.IsNullOrEmpty(sText))
                {
                    buttonX1.Text = eText.ToString();
                }
                else
                {
                    buttonX1.Text = value.ToString();
                }
            }
        }

        private float fontSize = 9;
        [Description("按钮字体大小")]
        [Browsable(true)]
        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;

                if (value == 0)
                {
                    fontSize = 9;
                }
                buttonX1.Font = new Font("宋体", fontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            }
        }

        public delegate void Del_EButtonProRun(Global.EnumEProcess eText);
        public static Del_EButtonProRun m_DelEButtonProRun;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                m_DelEButtonProRun(eText);
            }
            catch (Exception ex)
            {

            }
        }
        
    }
}
