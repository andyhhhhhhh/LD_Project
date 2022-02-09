using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.EditView
{
    public partial class EItemResult : UserControl
    {
        public EItemResult()
        {
            InitializeComponent();
        }

        private void EItemResult_Load(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
            }
        }

        public string _LText;
        [Browsable(true)]
        [Description("显示的控件Text信息")]
        public string LText
        {
            get { return _LText; }
            set
            {
                _LText = value;
                lblName.Text = value;
            }
        }

        public string _LinkValue;
        [Browsable(true)]
        [Description("关联的全局变量名称")]
        [TypeConverter(typeof(DropDownListConverter))]
        public string LinkValue
        {
            get { return _LinkValue; }
            set
            {
                _LinkValue = value;
            }
        }

        public string _SText;
        public string SText
        {
            get { return _SText; }
            set
            {
                _SText = value;
                if(value == "" || value == null)
                {
                    lblResult.Text = "IDLE";
                    lblResult.BackColor = Color.Yellow;
                }
                else if(value == "False")
                {
                    lblResult.Text = "FAIL";
                    lblResult.BackColor = Color.Red;
                }
                else if(value == "True")
                {
                    lblResult.Text = "PASS";
                    lblResult.BackColor = Color.Green;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                object value = XMLController.XmlControl.GetLinkValue("Global." + LinkValue);
                if (value != null)
                {
                    SText = value.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
