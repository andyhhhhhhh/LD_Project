using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ManagementView.EditView
{
    public partial class ESetText : UserControl
    {
        public ESetText()
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

        public int _TextLength;
        [Browsable(true)]
        [Description("Text信息的长度")]
        public int TextLength
        {
            get { return _TextLength; }
            set
            {
                _TextLength = value;
                tableLayoutPanel1.ColumnStyles[0].Width = _TextLength < 70 ? 70 : value;
            }
        }

        private void ESetText_Load(object sender, EventArgs e)
        {
            try
            {
                txtSetValue.TxtValueEvent += txtSetValue_TextChanged;
                if(!string.IsNullOrEmpty(LinkValue))
                {
                    object o = XMLController.XmlControl.GetLinkValue("Global."  + LinkValue);
                    if(o != null)
                    {
                        txtSetValue.sText = o.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void txtSetValue_TextChanged(object sender, string e)
        {
            try
            {
                var model = XMLController.XmlControl.sequenceModelNew.VariableSetModels.FirstOrDefault(x => x.Name == LinkValue);
                if(model != null)
                {
                    if(model.variableType == SequenceTestModel.VariableType.Double || model.variableType == SequenceTestModel.VariableType.Int)
                    {
                        double result = 0;
                        string str = txtSetValue.sText;
                        if (!string.IsNullOrEmpty(str) && !Double.TryParse(str, out result) && str != "-")
                        {
                            MessageBoxEx.Show("输入数字类型有误,请重新输入!");
                            txtSetValue.sText = str.Remove(str.Length - 1);
                            return;
                        }
                    }
                }

                string value = txtSetValue.sText;
                XMLController.XmlControl.SetLinkValue(null, "Global." + LinkValue, value);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
