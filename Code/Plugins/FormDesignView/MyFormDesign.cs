using DevComponents.DotNetBar;
using ManagementView;
using ManagementView.EditView;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using XMLController;

namespace MyFormDesinger
{
    public partial class MyFormDesign : Form
    {
        public string m_DesginPath = "";
        public DesginModel m_DesginModel = new DesginModel();
        public string m_Path = Application.StartupPath + "//Sequence//Design//";
        public string m_Name = "";
        HostFrame hostFrame;

        private List<EGroupBox> m_ListGroup = new List<EGroupBox>();
        public MyFormDesign()
        {
            InitializeComponent();
        }

        private void MyFormDesigner_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalCore.Global.IsDesginMode = true;

                string strPath =  GlobalCore.Global.SequencePath;
                m_Name = Path.GetFileNameWithoutExtension(strPath);

                m_DesginPath = m_Path + m_Name +  "_Design.dsr";
                m_DesginModel = (DesginModel)XmlControl.LoadFromXml(m_DesginPath, typeof(DesginModel));
                if (m_DesginModel == null)
                {
                    m_DesginModel = new DesginModel();
                }
                else
                {
                    toolLoadConfig_Click(null, null);
                }
            }
            catch (Exception ex)
            {

            }
        } 

        public static event EventHandler<object> AlignEvent;
        protected void OnAlignEvent(object e)
        {
            EventHandler<object> handler = AlignEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        /// <summary>
        /// 拖拽控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip_MouseDown(object sender, MouseEventArgs e)
        {
            ToolStripItem ctrl = sender as ToolStripItem;
            string[] strs = { ctrl.Tag == null ? "" : ctrl.Tag.ToString(), ctrl.Text };
            DoDragDrop(strs, DragDropEffects.Copy);
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog of = new OpenFileDialog())
            {
                of.Filter = "net程序集(*.dll)|*.dll";
                if (of.ShowDialog() == DialogResult.OK)
                {
                    Assembly assem = null;
                    try
                    {
                        assem = Assembly.LoadFile(of.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("不可识别程序集！");
                    }
                    if (assem != null)
                    {
                        using (AddControlDialog add = new AddControlDialog())
                        {
                            add.Assembly = assem;
                            if (add.ShowDialog() == DialogResult.OK)
                            {
                                ToolStripMenuItem i = new ToolStripMenuItem(add.CtrlName);
                                i.Tag = add.FullName + "/" + of.FileName;
                                toolStrip1.Items.Insert(toolStrip1.Items.Count - 1, i);
                                i.MouseDown += new MouseEventHandler(toolStrip_MouseDown);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in designerControl1.Controls)
                {
                    if(item is HostFrame)
                    {
                        //HostFrame host = item as HostFrame; 
                        HostFrame host = designerControl1._hostFrame;
                        m_DesginModel = new DesginModel();
                        m_DesginModel.Height = host.Height;
                        m_DesginModel.Width = host.Width;
                        m_DesginModel.Name = host.Name;

                        foreach (var control in host.Controls)
                        {
                            if (control is EGroupBox)
                            {
                                EGroupBox econtrol = control as EGroupBox;
                                EGroupBoxModel ebtnModel = new EGroupBoxModel()
                                {
                                    Name = econtrol.Name,
                                    LText = econtrol.LText,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height, 
                                };
                                m_DesginModel.EGroupBoxModels.Add(ebtnModel);
                            }
                            if (control is EButton)
                            {
                                EButton econtrol = control as EButton;
                                EButtonModel ebtnModel = new EButtonModel()
                                {
                                    Name = econtrol.Name,
                                    EText = econtrol.EText,
                                    FontSize = econtrol.FontSize,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                    EBackColor = GetColorHtml(econtrol.EBackColor),
                                };
                                m_DesginModel.EButtonModels.Add(ebtnModel);
                            }
                            else if (control is EButtonPro)
                            {
                                EButtonPro econtrol = control as EButtonPro;
                                EButtonProModel ebtnModel = new EButtonProModel()
                                {
                                    Name = econtrol.Name,
                                    EText = econtrol.EText,
                                    SText = econtrol.SText,
                                    FontSize = econtrol.FontSize,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EButtonProModels.Add(ebtnModel);
                            }
                            else if (control is EDataOutput)
                            {
                                EDataOutput econtrol = control as EDataOutput;
                                EDataOutputModel ebtnModel = new EDataOutputModel()
                                {
                                    Name = econtrol.Name, 
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EDataOutputModels.Add(ebtnModel);
                            }
                            else if (control is EHWindow)
                            {
                                EHWindow econtrol = control as EHWindow;
                                EHSmartWindowModel ebtnModel = new EHSmartWindowModel()
                                {
                                    Name = econtrol.Name,
                                    LayoutWindow = econtrol.LayoutWindow,
                                    LText = econtrol.LText,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EHSmartWindowModels.Add(ebtnModel);
                            }
                            else if (control is ELblResult)
                            {
                                ELblResult econtrol = control as ELblResult;
                                ELblResultModel ebtnModel = new ELblResultModel()
                                {
                                    Name = econtrol.Name, 
                                    sResult = ELblResult.sResult,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ELblResultModels.Add(ebtnModel);
                            }
                            else if (control is ELog)
                            {
                                ELog econtrol = control as ELog;
                                ELogModel ebtnModel = new ELogModel()
                                {
                                    Name = econtrol.Name, 
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ELogModels.Add(ebtnModel);
                            }
                            else if (control is ETextBox)
                            {
                                ETextBox econtrol = control as ETextBox;
                                ETextBoxModel ebtnModel = new ETextBoxModel()
                                {
                                    Name = econtrol.Name,
                                    LText = econtrol.LText,
                                    LinkValue = econtrol.LinkValue,
                                    TextLength = econtrol.TextLength,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ETextBoxModels.Add(ebtnModel);
                            }
                            else if(control is ESetText)
                            { 
                                ESetText econtrol = control as ESetText;
                                ESetTextModel ebtnModel = new ESetTextModel()
                                {
                                    Name = econtrol.Name,
                                    LText = econtrol.LText,
                                    LinkValue = econtrol.LinkValue,
                                    TextLength = econtrol.TextLength,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ESetTextModels.Add(ebtnModel);
                            }
                            else if (control is EProduct)
                            {
                                EProduct econtrol = control as EProduct;
                                EComboProductModel ebtnModel = new EComboProductModel()
                                {
                                    Name = econtrol.Name, 
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EComboProductModels.Add(ebtnModel);
                            }
                            else if (control is EItemResult)
                            {
                                EItemResult econtrol = control as EItemResult;
                                EItemResultModel ebtnModel = new EItemResultModel()
                                {
                                    Name = econtrol.Name,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    LinkValue = econtrol.LinkValue,
                                    LText = econtrol.LText,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EItemResultModels.Add(ebtnModel);
                            }
                            else if (control is EErrorItem)
                            {
                                EErrorItem econtrol = control as EErrorItem;
                                EErrorItemModel ebtnModel = new EErrorItemModel()
                                {
                                    Name = econtrol.Name,
                                    EText = econtrol.EText,
                                    SText = econtrol.SText,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EErrorItemModels.Add(ebtnModel);
                            }
                            else if (control is ECheck)
                            {
                                ECheck econtrol = control as ECheck;
                                ECheckModel ebtnModel = new ECheckModel()
                                {
                                    Name = econtrol.Name,
                                    LText = econtrol.LText,
                                    LinkValue = econtrol.LinkValue,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ECheckModels.Add(ebtnModel);
                            }
                            else if (control is EProductSel)
                            {
                                EProductSel econtrol = control as EProductSel;
                                EProductSelModel ebtnModel = new EProductSelModel()
                                {
                                    Name = econtrol.Name,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EProductSelModels.Add(ebtnModel);
                            }
                            else if(control is ELight)
                            {
                                ELight econtrol = control as ELight;
                                ELightModel ebtnModel = new ELightModel()
                                {
                                    Name = econtrol.Name,
                                    ComName = econtrol.ComName,
                                    LText = econtrol.LText,
                                    OpenText = econtrol.OpenText,
                                    CloseText = econtrol.CloseText,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.ELightModels.Add(ebtnModel);
                            }
                            else if (control is ECombox)
                            {
                                ECombox econtrol = control as ECombox;
                                EComboModel ebtnModel = new EComboModel()
                                {
                                    Name = econtrol.Name,
                                    LText = econtrol.LText,
                                    LinkValue = econtrol.LinkValue,
                                    ListValue = econtrol.ListValue,
                                    X = econtrol.Location.X,
                                    Y = econtrol.Location.Y,
                                    Width = econtrol.Width,
                                    Height = econtrol.Height,
                                };
                                m_DesginModel.EComboModels.Add(ebtnModel);
                            }

                        }

                        break;
                    }
                }

                XmlControl.SaveToXml(m_DesginPath, m_DesginModel, typeof(DesginModel));
                MessageBoxEx.Show("保存界面配置完成.");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                m_DesginModel = (DesginModel)XmlControl.LoadFromXml(m_DesginPath, typeof(DesginModel));
                if(m_DesginModel == null)
                {
                    m_DesginModel = new DesginModel();

                    //如果是空的
                    foreach (var item in designerControl1.Controls)
                    {
                        if (item is HostFrame)
                        {
                            HostFrame host = item as HostFrame;
                            host.Controls.Clear();

                            SetCtrlBrowsable(host);
                            break;
                        }
                    }

                    return;
                }
                hostFrame = new HostFrame(); 
                foreach (var item in designerControl1.Controls)
                {
                    if (item is HostFrame)
                    {
                        hostFrame = item as HostFrame;
                        if(m_DesginModel.Width != 0)
                        { 
                            hostFrame.Width = m_DesginModel.Width;
                        }
                        if(m_DesginModel.Height != 0)
                        { 
                            hostFrame.Height = m_DesginModel.Height;
                        }
                        if(!string.IsNullOrEmpty(m_DesginModel.Name))
                        {
                            hostFrame.Name = m_DesginModel.Name;
                        }
                        hostFrame.Controls.Clear();

                        SetCtrlBrowsable(hostFrame);
                        break;
                    }
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

                    ebtn.SendToBack();
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    SetCtrlBrowsable(ebtn);
                    hostFrame.Controls.Add(ebtn);
                    ebtn.SendToBack();

                    m_ListGroup.Add(ebtn);
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
                    ebtn.BringToFront();
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
                }

                foreach (var ibtn in m_DesginModel.EItemResultModels)
                {
                    EItemResult ebtn = new EItemResult()
                    {
                        Name = ibtn.Name,
                        Width = ibtn.Width,
                        Height = ibtn.Height,
                        LinkValue = ibtn.LinkValue,
                        LText = ibtn.LText,
                    };
                    ebtn.Location = new Point(ibtn.X, ibtn.Y);
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
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
                    SetCtrlBrowsable(ebtn);
                    AddControl(ebtn);
                }

            }
            catch (Exception ex)
            {
                 
            }
        }

        //判断GroupBox是否包含控件
        private void AddControl(Control control)
        {
            try
            {
                bool bresult = false;
                //foreach (var groupBox in m_ListGroup)
                //{
                //    Rectangle r = groupBox.Bounds;
                //    if (r.Contains(control.Location))
                //    {
                //        groupBox.AddChildControl(control); 
                //        groupBox.SendToBack();
                //        control.BringToFront();

                //        bresult = true;
                //        break;
                //    }
                //}

                if(!bresult)
                {
                    hostFrame.Controls.Add(control);
                    control.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("AddControl: " + ex.Message); 
            }
        }
         
        /// <summary>
        /// 设置控件的公共属性设置成不可见
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        void SetPropertyVisibility(object obj, string propertyName)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);

            bool bVisible = false;
            if (propertyName == "Name" || propertyName == "Width" || propertyName == "Height"
                || propertyName == "Left" || propertyName == "Top")
            {
                bVisible = true;
            }
            fld.SetValue(attrs[type], bVisible);
        }

        private void SetCtrlBrowsable(Control ctrl)
        {
            try
            {
                PropertyInfo[] infos = ctrl.GetType().GetProperties();
                if (ctrl is EButton || ctrl is EButtonPro || ctrl is EErrorItem)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "EText" || item.Name == "EBackColor" || item.Name == "SText" || item.Name == "FontSize")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EDataOutput)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EHWindow || ctrl is EGroupBox)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LayoutWindow" || item.Name == "LText")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELblResult)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "sResult")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELblStatus)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELog)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ETextBox || ctrl is ESetText || ctrl is ECheck)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue" || item.Name == "TextLength")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if(ctrl is HostFrame)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EProduct || ctrl is EProductSel)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EItemResult)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if(ctrl is ELight)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "ComName" || item.Name == "OpenText" || item.Name == "CloseText")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ECombox)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue" || item.Name == "ListValue")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取对应颜色的字符串格式
        /// </summary>
        /// <param name="color_from"></param>
        /// <returns></returns>
        private string GetColorHtml(Color color_from)
        {
            try
            {
                string drawColor = color_from.ToArgb().ToString("X02");
                drawColor = drawColor.Substring(2);
                if(!string.IsNullOrEmpty(drawColor))
                { 
                    return drawColor.Insert(0, "#");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
        /// <summary>
        /// 删除掉控件委托
        /// </summary>
        public delegate void Del_DeleteItem();
        public static Del_DeleteItem m_DeleteItem;
        private void toolDelete_Click(object sender, EventArgs e)
        {
            try
            {
                m_DeleteItem();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 界面关闭时执行委托
        /// </summary>
        public delegate void Del_FormClose();
        public static Del_FormClose m_DelFormClose;
        private void MyFormDesign_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            { 
                DialogResult result = MessageBoxEx.Show(this, "是否保存界面设计?", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if(result == DialogResult.OK)
                {
                    toolSaveConfig.PerformClick();
                }

                m_DelFormClose();

                CloseSetProper();

                GlobalCore.Global.IsDesginMode = false;
            }
            catch (Exception ex)
            {
                GlobalCore.Global.IsDesginMode = false;
            }
        }

        #region 程序关闭设置Browsable属性为true，防止Model的DataSource无法正常显示
        private void CloseSetProper()
        {
            try
            {
                foreach (var item in designerControl1.Controls)
                {
                    if (item is HostFrame)
                    {
                        //HostFrame host = item as HostFrame; 
                        HostFrame host = designerControl1._hostFrame;
                        SetCtrlBrowsableTrue(host);

                        foreach (var control in host.Controls)
                        {
                            SetCtrlBrowsableTrue((Control)control);
                        }
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetCtrlBrowsableTrue(Control ctrl)
        {
            try
            {
                PropertyInfo[] infos = ctrl.GetType().GetProperties();
                foreach (var item in infos)
                {
                    SetProVisible(ctrl, item.Name);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void SetProVisible(object obj, string propertyName)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);

            bool bVisible = true;

            fld.SetValue(attrs[type], bVisible);
        }

        #endregion
        
        #region 同时操作多个控件
        private void toolLeft_Click(object sender, EventArgs e)
        {
            OnAlignEvent(1);
        }

        private void toolTop_Click(object sender, EventArgs e)
        {
            OnAlignEvent(2);
        }

        private void toolVertically_Click(object sender, EventArgs e)
        {
            OnAlignEvent(3);
        }

        private void toolHorizontally_Click(object sender, EventArgs e)
        {
            OnAlignEvent(4);
        }

        private void toolSameSize_Click(object sender, EventArgs e)
        {
            OnAlignEvent(5);
        }

        private void toolRight_Click(object sender, EventArgs e)
        {
            OnAlignEvent(6);
        }

        private void toolBottom_Click(object sender, EventArgs e)
        {
            OnAlignEvent(7);
        }
        
        private void toolWidthEqual_Click(object sender, EventArgs e)
        {
            OnAlignEvent(8);
        }

        private void toolHeightEqual_Click(object sender, EventArgs e)
        {
            OnAlignEvent(9);
        }
        #endregion

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            try
            {
                var o = propertyGrid1.SelectedObject;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void cmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbSelect.SelectedIndex == 0)
                { 
                    m_DesginPath = m_Path + m_Name + "_Design.dsr";
                    toolEHWindow.Enabled = true;
                    toolELog.Enabled = true;
                    toolEDataOutput.Enabled = true;
                }
                else
                {
                    m_DesginPath = m_Path + m_Name + "_Debug_Design.dsr";
                    toolEHWindow.Enabled = false;
                    toolELog.Enabled = false;
                    toolEDataOutput.Enabled = false;
                }

                toolLoadConfig_Click(null, null);
            }
            catch (Exception ex)
            {
                 
            }
        }
         
    }
}
