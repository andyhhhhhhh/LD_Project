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
    public partial class EErrorItem : UserControl
    { 
        public EErrorItem()
        { 
            InitializeComponent();
        }

        private Global.EnumEProcess eText;
        /// <summary>
        /// Button显示的text信息
        /// </summary>
        [Description("设置关联的流程名")]
        [Browsable(true)]
        public Global.EnumEProcess EText
        {
            get { return eText; }
            set
            {
                eText = value;
                txtTitle.Text = value.ToString();

                var seq = XMLController.XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == eText.ToString());
                if (seq != null)
                {
                    SText = seq.AnotherName;
                }
            }
        }

        private string sText;
        /// <summary>
        /// 列头显示的text信息
        /// </summary>
        [Browsable(true)]
        [Description("控件上面显示的流程信息描述")]
        public string SText
        {
            get { return sText; }
            set
            {
                sText = value;
                if (string.IsNullOrEmpty(sText))
                {
                    txtTitle.Text = eText.ToString();
                }
                else
                {
                    txtTitle.Text = value.ToString();
                }
            }
        }

        /// <summary>
        /// 显示错误项目
        /// </summary>
        /// <param name="listNGItem">NG项目集合</param> 
        public void ShowErrorItem(string listNGItem)
        {
            try
            {
                if (listNGItem != null)
                {
                    richTextItem.BeginInvoke(new Action(() =>
                    {
                        richTextItem.Text = "";
                        richTextItem.AppendText(listNGItem);
                    }));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
