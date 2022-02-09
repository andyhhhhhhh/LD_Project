using DevComponents.DotNetBar;
using GlobalCore;
using ManagementView;
using ManagementView._3DViews;
using ManagementView.EditView;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace FormDesignView
{
    public partial class RunEditView : Form
    {
        public delegate void Del_RunViewClose();
        public static Del_RunViewClose m_DelRunViewClose;

        public delegate void Del_ToEditView();
        public static Del_ToEditView m_DelToEditView;

        public bool m_bShowBox = false;


        public string m_Path = Global.CurrentPath + "//Sequence//Design//";
        public string m_DesginPath = "";
        public DesginModel m_DesginModel = new DesginModel();

        public List<EErrorItem> m_ListErrorItem = new List<EErrorItem>();
        public RunEditView()
        {
            try
            {
                InitializeComponent(); 
                SetViewStyle();

                string strPath = GlobalCore.Global.SequencePath;
                string name = Path.GetFileNameWithoutExtension(strPath);

                m_DesginPath = m_Path + name + "_Design.dsr";

                m_DesginModel = (DesginModel)XmlControl.LoadFromXml(m_DesginPath, typeof(DesginModel));
                if (m_DesginModel == null)
                {
                    m_DesginModel = new DesginModel();
                }

                LoadConfig();
            }
            catch (Exception ex)
            {
               
            } 
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadConfig()
        {
            try
            {
                m_DesginModel = (DesginModel)XmlControl.LoadFromXml(m_DesginPath, typeof(DesginModel));
                if (m_DesginModel == null)
                {
                    m_DesginModel = new DesginModel();
                    return;
                }

                if(m_DesginModel.Width != 0)
                { 
                    this.Width = m_DesginModel.Width - 3;
                }
                if(m_DesginModel.Height != 0)
                {
                    this.Height = m_DesginModel.Height + 53;
                }

                //设置窗口最大化
                this.MaximumSize = new Size(this.Width + 2, this.Height + 2);

                if (!string.IsNullOrEmpty(m_DesginModel.Name))
                {
                    this.Text = m_DesginModel.Name;
                }

                foreach (var ibtn in m_DesginModel.EGroupBoxModels)
                {
                    EGroupBox ebtn = new EGroupBox()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText, 
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.SendToBack();
                }

                foreach (var ibtn in m_DesginModel.EButtonModels)
                {
                    EButton ebtn = new EButton()
                    {
                        Name = ibtn.Name,
                        EText = ibtn.EText,
                        FontSize = ibtn.FontSize,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                        EBackColor = !string.IsNullOrEmpty(ibtn.EBackColor) ?
                                     ColorTranslator.FromHtml(ibtn.EBackColor) : Color.LightSkyBlue,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y); 

                    //操作员则设定不能编辑
                    if(ebtn.EText == Global.EnumEButtonRun.编辑  && Global.UserName == Global.OperatorName)
                    {
                        ebtn.Enabled = false;
                    }
                    else
                    { 
                        ebtn.Enabled = true;
                    }

                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EButtonProModels)
                {
                    EButtonPro ebtn = new EButtonPro()
                    {
                        Name = ibtn.Name,
                        EText = ibtn.EText,
                        SText = ibtn.SText,
                        FontSize = ibtn.FontSize,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EDataOutputModels)
                {
                    EDataOutput ebtn = new EDataOutput()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EHSmartWindowModels)
                {
                    EHWindow ebtn = new EHWindow()
                    {
                        Name = ibtn.Name,
                        LayoutWindow = ibtn.LayoutWindow,
                        LText = ibtn.LText,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.ELblResultModels)
                {
                    ELblResult ebtn = new ELblResult()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }


                foreach (var ibtn in m_DesginModel.ELblStatusModels)
                {
                    ELblStatus ebtn = new ELblStatus()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.ELogModels)
                {
                    ELog ebtn = new ELog()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.ETextBoxModels)
                {
                    ETextBox ebtn = new ETextBox()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText,
                        LinkValue = ibtn.LinkValue,
                        TextLength = ibtn.TextLength,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.ESetTextModels)
                {
                    ESetText ebtn = new ESetText()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText,
                        LinkValue = ibtn.LinkValue,
                        TextLength = ibtn.TextLength,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EComboProductModels)
                {
                    EProduct ebtn = new EProduct()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EItemResultModels)
                {
                    EItemResult ebtn = new EItemResult()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                        LText = ibtn.LText,
                        SText = "",
                        LinkValue = ibtn.LinkValue,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                
                foreach (var ibtn in m_DesginModel.EErrorItemModels)
                {
                    EErrorItem ebtn = new EErrorItem()
                    {
                        Name = ibtn.Name,
                        EText = ibtn.EText,
                        SText = ibtn.SText,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();

                    m_ListErrorItem.Add(ebtn);
                }

                foreach (var ibtn in m_DesginModel.ECheckModels)
                {
                    ECheck ebtn = new ECheck()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText,
                        LinkValue = ibtn.LinkValue,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }


                foreach (var ibtn in m_DesginModel.EProductSelModels)
                {
                    EProductSel ebtn = new EProductSel()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }


                foreach (var ibtn in m_DesginModel.ELightModels)
                {
                    ELight ebtn = new ELight()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText,
                        ComName = ibtn.ComName,
                        OpenText = ibtn.OpenText,
                        CloseText = ibtn.CloseText,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

                foreach (var ibtn in m_DesginModel.EComboModels)
                {
                    ECombox ebtn = new ECombox()
                    {
                        Name = ibtn.Name,
                        LText = ibtn.LText,
                        LinkValue = ibtn.LinkValue,
                        ListValue = ibtn.ListValue, 
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    panel1.Controls.Add(ebtn);
                    ebtn.BringToFront();
                }

            }
            catch (Exception ex)
            {

            }
        }
        
        private void RunEditView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_bShowBox)
                {
                    DialogResult dr = MessageBoxEx.Show(this, "即将退出程序，请确认是否保存配置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        XmlControl.SetObject();
                        XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));

                        //e.Cancel = false;  //关闭窗体 
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
         
        public void SetText(int num, string text)
        {
            try
            {
                switch (num)
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
                switch (num)
                {
                    case 0:
                        lblRunStatus.BackColor = backColor;
                        break;
                    case 1:
                        //lblCommunictionStatus.BackColor = backColor;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        //用户管理界面
        private void btnUserManage_Click(object sender, EventArgs e)
        {
            try
            {
                ManagementView.UserView.UserLogin view = new ManagementView.UserView.UserLogin();
                view.ShowDialog();


                //设置操作员不可编辑
                foreach (var item in panel1.Controls)
                {
                    if(item is EButton)
                    {
                        EButton ebtn = item as EButton;
                        if(ebtn.EText == Global.EnumEButtonRun.编辑)
                        {
                            ebtn.Enabled = Global.UserName != Global.OperatorName;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //切换到编辑界面
        private void btnEditView_Click(object sender, EventArgs e)
        {
            try
            {
                m_DelToEditView();
            }
            catch (Exception ex)
            {                 

            }
        }

        //显示ErrorItem在界面上
        public void ShowErrItem(string listNGItem, string strSeqence)
        {
            try
            {
                foreach (var item in m_ListErrorItem)
                {
                    if(item.EText.ToString() == strSeqence)
                    {
                        item.ShowErrorItem(listNGItem);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //弹出参数配置界面
        static RunParamView m_RunParamView;
        private void btnDebugView_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_RunParamView == null || m_RunParamView.IsDisposed)
                {
                    m_RunParamView = new RunParamView();
                    m_RunParamView.Show();
                }
                else
                {
                    m_RunParamView.Focus();
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
