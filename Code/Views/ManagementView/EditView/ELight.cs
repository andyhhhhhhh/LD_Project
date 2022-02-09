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
    public partial class ELight : UserControl
    {
        public ELight()
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
                btnLight.Text = value;
            }
        }

        public string _ComName;
        [Browsable(true)]
        [Description("关联的串口")]
        [TypeConverter(typeof(DropDownListConverter))]
        public string ComName
        {
            get { return _ComName; }
            set
            {
                _ComName = value;
            }
        }

        public string _OpenText;
        [Browsable(true)]
        [Description("打开光源的命令")]
        public string OpenText
        {
            get { return _OpenText; }
            set
            {
                _OpenText = value; 
            }
        }

        public string _CloseText;
        [Browsable(true)]
        [Description("关闭光源的命令")]
        public string CloseText
        {
            get { return _CloseText; }
            set
            {
                _CloseText = value;
            }
        }


        public delegate void ELightControl(string command, string comName);
        public static ELightControl m_ELightControl;

        private void btnLight_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_ELightControl != null)
                {
                    if(btnLight.BackColor == Color.Transparent)
                    {
                        m_ELightControl(OpenText, ComName);
                        btnLight.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        btnLight.BackColor = Color.Transparent;
                        m_ELightControl(CloseText, ComName);
                    }
                }
            }
            catch (Exception ex)
            {
                 
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
                var listVar = XMLController.XmlControl.sequenceModelNew.ComModels;
                if (listVar != null && listVar.Count > 0)
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
}
