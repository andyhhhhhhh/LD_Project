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
    public partial class EHWindow : UserControl
    {
        public EHWindow()
        {
            InitializeComponent();
        }

        private void EHSmartWindow_Load(object sender, EventArgs e)
        {
            try
            {
                object hSmartWindow = m_DelSetLayoutWindow(LayoutWindow);
                CommHelper.LayoutChildFillView(panelEx1, (UserControl)hSmartWindow);
            }
            catch (Exception ex)
            {

            }
        }

        private int layoutWinodw;
        [Browsable(true)]
        [Description("关联的图形窗口的Id")]
        public int LayoutWindow
        {
            get { return layoutWinodw; }
            set
            {
                layoutWinodw = value;
            }
        }

        private string _LText;
        [Browsable(true)]
        [Description("窗口名称")]
        [TypeConverter(typeof(DropDownListConverter))]
        public string LText
        {
            get { return _LText; }
            set
            {
                _LText = value;
                if(!string.IsNullOrEmpty(_LText))
                {
                    label1.Text = value;
                }
            }
        }

        public delegate object Del_SetLayOutWindow(int layOut);
        public static Del_SetLayOutWindow m_DelSetLayoutWindow;


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
                var listVar = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels;
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
