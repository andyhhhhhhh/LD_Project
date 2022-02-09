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
    public partial class ETextBox : UserControl
    { 
        public ETextBox()
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
                labelX1.Text = value;
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

        private void ETextBox_Load(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                object value = XMLController.XmlControl.GetLinkValue("Global." + LinkValue);
                if (value != null)
                {
                    textBoxX1.Text = value.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }

    //下拉框类型转换器
    public class DropDownListConverter : StringConverter
    {
        object[] m_Objects;
        //public DropDownListConverter(object[] objects)
        //{
        //    m_Objects = objects;
        //}
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;//true下拉框不可编辑
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> listString = new List<string>();
            var listVar = XMLController.XmlControl.sequenceModelNew.VariableSetModels;
            if(listVar != null && listVar.Count > 0)
            {
                foreach (var item in listVar)
                {
                    listString.Add(item.Name);
                }
            }
          
            m_Objects = listString.ToArray();

            return new StandardValuesCollection(m_Objects);//我们可以直接在内部定义一个数组，但并不建议这样做，这样对于下拉框的灵活             //性有很大影响
        }
    }
}
