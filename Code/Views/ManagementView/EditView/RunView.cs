using DevComponents.DotNetBar;
using GlobalCore;
using HalconView;
using ManagementView._3DViews;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace ManagementView.EditView
{
    public partial class RunView : Form
    {
        public delegate void Del_RunViewClose();
        public static Del_RunViewClose m_DelRunViewClose;
        public bool m_bShowBox = false;
        public RunView()
        {
            InitializeComponent();
            SetResolution();
            SetViewStyle();
        }

        private void RunView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if(m_bShowBox)
                {
                    DialogResult dr = MessageBoxEx.Show(this, "即将退出程序，请确认是否保存配置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        //e.Cancel = false;  //关闭窗体 
                        XmlControl.SetObject();
                        XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        e.Cancel = true;   //不执行操作
                        return;
                    }
                } 

                m_DelRunViewClose();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnStyle_ExpandChange(object sender, EventArgs e)
        {
            try
            {
                ButtonItem[] btnItems = new ButtonItem[10] {btnViewStyle1, btnViewStyle2, btnViewStyle3,
                    btnViewStyle4, btnViewStyle5, btnViewStyle6, btnViewStyle7, btnViewStyle8, btnViewStyle9, btnViewStyle10};
                string str = Global.GetNodeValue("ViewStyle");
                for (int i = 0; i < btnItems.Length; i++)
                {
                    btnItems[i].Checked = btnItems[i].Text == str;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnStyle_ExpandChange", ex.Message);
            }
        }
        
        private void SetViewStyle()
        {
            try
            {
                string viewStyle = Global.GetNodeValue("ViewStyle");
                switch (viewStyle)
                {
                    case "Office2007Blue":
                        styleManager1.ManagerStyle = eStyle.Office2007Blue;
                        break;
                    case "Office2007Silver":
                        styleManager1.ManagerStyle = eStyle.Office2007Silver;
                        break;
                    case "Office2007Black":
                        styleManager1.ManagerStyle = eStyle.Office2007Black;
                        break;
                    case "Office2007VistaGlass":
                        styleManager1.ManagerStyle = eStyle.Office2007VistaGlass;
                        break;
                    case "Office2010Silver":
                        styleManager1.ManagerStyle = eStyle.Office2010Silver;
                        break;
                    case "Office2010Black":
                        styleManager1.ManagerStyle = eStyle.Office2010Black;
                        break;
                    case "Office2010Blue":
                        styleManager1.ManagerStyle = eStyle.Office2010Blue;
                        break;
                    case "Windows7Blue":
                        styleManager1.ManagerStyle = eStyle.Windows7Blue;
                        break;
                    case "VisualStudio2010Blue":
                        styleManager1.ManagerStyle = eStyle.VisualStudio2010Blue;
                        break;
                    case "Metro":
                        styleManager1.ManagerStyle = eStyle.Metro;
                        break;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("SetViewStyle", ex.Message);
            }
        }
        
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonItem btn = sender as ButtonItem; 
                Global.SaveNodeValue("ViewStyle", btn.Text);
                SetViewStyle();
            }
            catch (Exception ex)
            {

            }
        }

        //设置View的分辨率
        private void SetResolution()
        {
            try
            {
                string str = Global.GetNodeValue("Resolution");
                string[] strArr = str.Split('*');
                if (strArr == null || strArr.Length != 2)
                {
                    return;
                }
                int width, height;
                Int32.TryParse(strArr[0], out width);
                Int32.TryParse(strArr[1], out height);
                this.Width = width;
                this.Height = height;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("SetResolution", ex.Message);
            }
        }

        public void SetText(int num, string text)
        {
            try
            {
                switch(num)
                {
                    case 0:
                        lblStatus.Text = text;
                        break;
                    case 1:
                        lblRunStatus.Text = text;
                        break;
                    case 2:
                        lblLoginUser.Text = text;
                        break;
                    case 3:
                        lblRunTime.Text = text;
                        break;
                    case 4:
                        lblMemory.Text = text;
                        break;
                    case 5:
                        lblCPU.Text = text;
                        break;
                    case 6:
                        lblCurrentTime.Text = text;
                        break;

                    default:
                        break; 
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        public void SetColor(int num, Color backColor)
        {
            try
            {
                switch(num)
                {
                    case 0:
                        lblRunStatus.BackColor = backColor;
                        break;
                    case 1:
                        lblCommunictionStatus.BackColor = backColor;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        } 
       
    }
}
