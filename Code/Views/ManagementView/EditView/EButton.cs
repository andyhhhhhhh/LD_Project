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
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;
using System.Windows.Forms.Design;

namespace ManagementView
{
    //[Designer(typeof(MyDesigner))]
    public partial class EButton : UserControl
    {
        public EButton()
        {
            InitializeComponent();
        }
         
        private Global.EnumEButtonRun eText;
        /// <summary>
        /// Button显示的text信息
        /// </summary>
        //[Category("外观")]
        [Description("按钮上面显示的信息")]
        [Browsable(true)]
        public Global.EnumEButtonRun EText
        {
            get { return eText; }
            set
            {
                eText = value;
                buttonX1.Text = value.ToString();
                buttonX1.Tooltip = value.ToString();
            }
        }

        private Color eBackColor = Color.LightSkyBlue;
        [Description("按钮的背景设置颜色")]
        [Browsable(true)]
        public Color EBackColor
        {
            get
            {
                return eBackColor;
            }
            set
            {
                if(value != null)
                {
                    eBackColor = value;
                    this.BackColor = value;
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

                if(value == 0)
                {
                    fontSize = 9;
                }
                buttonX1.Font = new Font("宋体", fontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            }
        }

        public delegate void Del_EButtonRun(Global.EnumEButtonRun eText);
        public static Del_EButtonRun m_DelEButton;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                m_DelEButton(EText);
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    //设计时屏蔽某些属性
    public class MyDesigner : ControlDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            //取得设计时当前控件类型
            Type type = base.Control.GetType();
            PropertyInfo[] piArray = type.GetProperties();

            foreach (PropertyInfo p in piArray)
            {
                //if (p.Name != "EText")
                //{
                //    properties.Remove(p.Name);
                //}
            }

            base.PreFilterProperties(properties);
        }  
    }
      
}
