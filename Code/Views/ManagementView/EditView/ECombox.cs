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
    public partial class ECombox : UserControl
    {
        public ECombox()
        {
            InitializeComponent();
        }

        public string _LText;
        [Browsable(true)]
        [Description("控件显示的Text信息")]
        public string LText
        {
            get { return _LText; }
            set
            {
                _LText = value;
                lblSetName.Text = value;
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

        public string _ListValue;
        [Browsable(true)]
        [Description("组合框里面显示的值集合")]
        public string ListValue
        {
            get { return _ListValue; }
            set
            {
                _ListValue = value; 
            }
        }


        private void ECombox_Load(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(ListValue))
                {
                    return;
                }

                var spiltStrs = ListValue.Split(',');

                if(spiltStrs == null || spiltStrs.Count() == 0)
                {
                    return;
                }

                cmbValue.Items.AddRange(spiltStrs);

                cmbValue.Text = XMLController.XmlControl.GetLinkValue("Global." + LinkValue).ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                XMLController.XmlControl.SetLinkValue(null, "Global." + LinkValue, cmbValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
