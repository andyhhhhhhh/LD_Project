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
    public partial class ECheck : UserControl
    {
        public ECheck()
        {
            InitializeComponent();
        }
        public string _LText;
        [Browsable(true)]
        [Description("CheckBox上面显示的Text信息")]
        public string LText
        {
            get { return _LText; }
            set
            {
                _LText = value;
                chkItem.Text = value;
            }
        }

        public string _LinkValue;
        [Browsable(true)]
        [Description("设置的全局变量的名称")]
        [TypeConverter(typeof(DropDownBoolListConverter))]
        public string LinkValue
        {
            get { return _LinkValue; }
            set
            {
                _LinkValue = value;
            }
        }

        private void ECheck_Load(object sender, EventArgs e)
        {
            try
            {
                object o = XMLController.XmlControl.GetLinkValue("Global." + LinkValue);
                bool result = false;
                bool bresult = Boolean.TryParse(o.ToString(), out result);
                if(bresult)
                {
                    chkItem.Checked = result;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void chkItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool value = chkItem.Checked;
                XMLController.XmlControl.SetLinkValue(null, "Global." + LinkValue, value);
            }
            catch (Exception ex)
            {

            }
        }
    }

    //下拉框BOOL类型转换器
    public class DropDownBoolListConverter : StringConverter
    {
        object[] m_Objects;
        
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
            if (listVar != null && listVar.Count > 0)
            {
                var listBool = listVar.FindAll(x => x.variableType == SequenceTestModel.VariableType.Bool);
                if(listBool != null && listBool.Count > 0)
                {
                    foreach (var item in listBool)
                    {
                        listString.Add(item.Name);
                    }
                }                
            }

            m_Objects = listString.ToArray();

            return new StandardValuesCollection(m_Objects);//我们可以直接在内部定义一个数组，但并不建议这样做，这样对于下拉框的灵活             //性有很大影响
        }
    }
}
