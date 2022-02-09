using GlobalCore;
using ManagementView;
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
    /// <summary>
    /// 调试界面
    /// </summary>
    public partial class RunParamView : Form
    {
        public string m_Path = Global.CurrentPath + "//Sequence//Design//";
        public string m_DesginPath = "";
        public DesginModel m_DesginModel = new DesginModel();
        public RunParamView()
        {
            try
            {
                InitializeComponent();

                string strPath = Global.SequencePath;
                string name = Path.GetFileNameWithoutExtension(strPath);

                m_DesginPath = m_Path + name + "_Debug_Design.dsr";

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

                if (m_DesginModel.Width != 0)
                {
                    this.Width = m_DesginModel.Width + 12;
                }
                if (m_DesginModel.Height != 0)
                {
                    this.Height = m_DesginModel.Height + 35;
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
                    if (ebtn.EText == Global.EnumEButtonRun.编辑 && Global.UserName == Global.OperatorName)
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

                    //m_ListErrorItem.Add(ebtn);
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
    }
}
