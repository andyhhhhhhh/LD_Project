using BaseController;
using DevComponents.DotNetBar;
using GlobalCore;
using ManagementView.Comment;
using SequenceTestModel;
using SerialPortController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace ManagementView._3DViews
{
    public partial class Communication_View : UserControl
    {
        bool m_close = false;
        public Communication_View()
        {
            InitializeComponent();
        }

        private void CommunicationView_Load(object sender, EventArgs e)
        {
            SingleSequenceModel sequence = XmlControl.sequenceSingle;

            //初始化网口
            UpdateSocket();

            //初始化串口
            InitComDataSource();
            UpdateCom();

            //初始化PLC
            UpdatePLC();

            //初始化Ftp
            UpdateFtp();

            //初始化SFC
            UpdateSFC();

            tabControl1.SelectedTabIndex = 0;

            if(this.ParentForm != null)
            {
                this.ParentForm.FormClosing += ParentForm_FormClosing;
            }
        }

        private void ParentForm_FormClosing(object sender, EventArgs e)
        {
            m_close = true;
            m_tcpListen = false;
            m_comListen = false;
        }
      
        /// <summary>
        /// Socket更新
        /// </summary>
        private void UpdateSocket()
        {
            //初始化网口
            var listSocket = XmlControl.sequenceModelNew.TCPIPModels;
            if (listSocket != null && listSocket.Count > 0)
            {
                cmbSocket.DataSource = null;
                cmbSocket.DataSource = listSocket;
                cmbSocket.DisplayMember = "Name";
            }

            cmbSocket.Refresh();
        }

        /// <summary>
        /// Com更新
        /// </summary>
        private void UpdateCom()
        {
            //初始化串口  
            var listCom = XmlControl.sequenceModelNew.ComModels;
            if (listCom != null && listCom.Count > 0)
            {
                cmbCom.DataSource = null;
                cmbCom.DataSource = listCom;
                cmbCom.DisplayMember = "Name";
            }

            cmbCom.Refresh();
        }

        /// <summary>
        /// PLC更新
        /// </summary>
        private void UpdatePLC()
        {
            try
            {
                var listPLC = XmlControl.sequenceModelNew.PLCSetModels;
                if (listPLC != null && listPLC.Count > 0)
                {
                    cmbPLC.DataSource = null;
                    cmbPLC.DataSource = listPLC;
                    cmbPLC.DisplayMember = "Name";
                }

                cmbPLC.Refresh();
            }
            catch (Exception ex)
            {

            } 
        }

        /// <summary>
        /// Ftp更新
        /// </summary>
        private void UpdateFtp()
        {
            try
            {
                var listFtp = XmlControl.sequenceModelNew.FtpClientModels;
                if (listFtp != null && listFtp.Count > 0)
                {
                    cmbFtp.DataSource = null;
                    cmbFtp.DataSource = listFtp;
                    cmbFtp.DisplayMember = "Name";
                }

                cmbFtp.Refresh();
            }
            catch (Exception ex)
            {
                 
            }
        }

        #region 网口配置 
        private void btnAddSocket_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                if(string.IsNullOrEmpty(txtName.sText))
                {
                    MessageBoxEx.Show("名称为空...");
                    return;
                }

                TCPIPModel tcpipModel = new TCPIPModel()
                {
                    Name = txtName.sText,
                    IPAddress = txtIPAddress.sText,
                    PortNum = Int32.Parse(txtPort.sText),
                    IsService = chkService.Checked,
                    TimeOut = Int32.Parse(txtTCPTimeOut.sText),
                };

                if (sequence.TCPIPModels.FindIndex(x => x.Name == txtName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在名称:" + txtName.sText);
                    return;
                }

                sequence.TCPIPModels.Add(tcpipModel);

                int id = 0;
                foreach (var item in sequence.TCPIPModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("新增【{0}】成功", tcpipModel.Name));
                UpdateSocket();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnAdd_Click:" + ex.Message);
            }
        }

        private void btnUpdateSocket_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                TCPIPModel tcpipModel = sequence.TCPIPModels.FirstOrDefault(x => x.Name == cmbSocket.Text);

                tcpipModel.Name = txtName.sText;
                tcpipModel.IPAddress = txtIPAddress.sText;
                tcpipModel.PortNum = Int32.Parse(txtPort.sText);
                tcpipModel.IsService = chkService.Checked;
                tcpipModel.TimeOut = Int32.Parse(txtTCPTimeOut.sText);

                //XmlControl.SaveToXml(GlobalCore.Global.SequencePath, sequence, sequence.GetType());

                MessageBoxEx.Show(string.Format("更新【{0}】成功", tcpipModel.Name));
                // UpdateSocket();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnUpdateSocket_Click:" + ex.Message);
            }
        }

        private void btnDeleteSocket_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                TCPIPModel tcpipModel = sequence.TCPIPModels.FirstOrDefault(x => x.Name == txtName.sText);

                sequence.TCPIPModels.Remove(tcpipModel);

                int id = 0;
                foreach (var item in sequence.TCPIPModels)
                {
                    item.Id = id;
                    id++;
                }

                // XmlControl.SaveToXml(GlobalCore.Global.SequencePath, sequence, sequence.GetType());

                MessageBoxEx.Show(string.Format("删除【{0}】成功", tcpipModel.Name));
                UpdateSocket();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnDeleteSocket_Click:" + ex.Message);
            }
        }

        private void cmbSocket_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TCPIPModel tcpipModel = cmbSocket.SelectedItem as TCPIPModel;
                if (tcpipModel == null)
                {
                    return;
                }
                txtName.sText = tcpipModel.Name;
                txtIPAddress.sText = tcpipModel.IPAddress;
                txtPort.sText = tcpipModel.PortNum.ToString();
                chkService.Checked = tcpipModel.IsService;
                txtId.sText = tcpipModel.Id.ToString();
                txtTCPTimeOut.sText = tcpipModel.TimeOut.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("cmbSocket_SelectedIndexChanged:" + ex.Message);
            }
        }


        #endregion

        #region 串口配置
        private void btnAddCom_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                if (string.IsNullOrEmpty(txtComName.sText))
                {
                    MessageBoxEx.Show("名称为空...");
                    return;
                }

                Parity parity;
                Enum.TryParse(cmbParity.SelectedItem.ToString(), out parity);
                StopBits stopBits;
                Enum.TryParse(cmbStopBits.SelectedItem.ToString(), out stopBits);
                
                ComModel comModel = new ComModel()
                {
                    Name = txtComName.sText,
                    ComPortName = txtComPort.sText,
                    BaudRate = Convert.ToInt32(cmbBaudRate.SelectedItem),
                    DataBits = Convert.ToInt32(cmbDataBits.SelectedItem),
                    Parity = parity,
                    StopBits = stopBits,
                    IsHex = chkIsHex.Checked,
                    TimeOut = Convert.ToInt32(txtTimeOut.sText),
                    IsByEnd = chkIsByEnd.Checked
                };

                if (sequence.ComModels.FindIndex(x => x.Name == txtComName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在名称:" + txtComName.sText);
                    return;
                }

                sequence.ComModels.Add(comModel);

                int id = 0;
                foreach (var item in sequence.ComModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("新增【{0}】成功", comModel.Name));
                UpdateCom();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnAddCom_Click:" + ex.Message);
            }
        }

        private void btnSaveCom_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                ComModel comModel = sequence.ComModels.FirstOrDefault(x => x.Name == cmbCom.Text);
                Parity parity;
                Enum.TryParse(cmbParity.SelectedItem.ToString(), out parity);
                StopBits stopBits;
                Enum.TryParse(cmbStopBits.SelectedItem.ToString(), out stopBits);

                comModel.Name = txtComName.sText;
                comModel.Name = txtComName.sText;
                comModel.ComPortName = txtComPort.sText;
                comModel.BaudRate = Convert.ToInt32(cmbBaudRate.SelectedItem);
                comModel.DataBits = Convert.ToInt32(cmbDataBits.SelectedItem);
                comModel.Parity = parity;
                comModel.StopBits = stopBits;
                comModel.IsHex = chkIsHex.Checked;
                comModel.TimeOut = Convert.ToInt32(txtTimeOut.sText);
                comModel.IsByEnd = chkIsByEnd.Checked;
                comModel.SendContent = txtSendContent.sText;

                MessageBoxEx.Show(string.Format("更新【{0}】成功", comModel.Name)); 
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnSaveCom_Click:" + ex.Message);
            }
        }

        private void btnDelCom_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                ComModel comModel = sequence.ComModels.FirstOrDefault(x => x.Name == txtComName.sText);

                sequence.ComModels.Remove(comModel);

                int id = 0;
                foreach (var item in sequence.ComModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("删除【{0}】成功", comModel.Name));
                UpdateCom();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnDelCom_Click:" + ex.Message);
            }
        }

        private void cmbCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComModel comModel = cmbCom.SelectedItem as ComModel;
                if (comModel == null)
                {
                    return;
                }
                txtComName.sText = comModel.Name;
                txtComPort.sText = comModel.ComPortName;
                cmbBaudRate.SelectedIndex = CommHelper.GetComboxSelectIndex(cmbBaudRate, comModel.BaudRate.ToString());
                cmbDataBits.SelectedIndex = CommHelper.GetComboxSelectIndex(cmbDataBits, comModel.DataBits.ToString());
                cmbParity.SelectedIndex = CommHelper.GetComboxSelectIndex(cmbParity, comModel.Parity.ToString());
                cmbStopBits.SelectedIndex = CommHelper.GetComboxSelectIndex(cmbStopBits, comModel.StopBits.ToString());
                chkIsHex.Checked = comModel.IsHex;
                txtTimeOut.sText = comModel.TimeOut.ToString();
                chkIsByEnd.Checked = comModel.IsByEnd;
                txtSendContent.sText = comModel.SendContent;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("cmbCom_SelectedIndexChanged:" + ex.Message);
            }
        }

        private void InitComDataSource()
        {
            cmbBaudRate.DataSource = new List<string>()
            {
                "9600","14400","19200","38400","43000","57600","76800","115200","128000"
            };
            cmbDataBits.DataSource = new List<string> { "8", "7", "6", "5" };
            cmbParity.DataSource = new List<Parity> { Parity.None, Parity.Odd, Parity.Space, Parity.Mark, Parity.Even };
            cmbStopBits.DataSource = new List<StopBits> { StopBits.One, StopBits.None, StopBits.OnePointFive, StopBits.Two };
        }

        #endregion

        #region PLC配置
        private void btnPLCSave_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                PLCSetModel tModel = cmbPLC.SelectedItem as PLCSetModel;
                tModel.ConnType = cmbConnType.Text;
                tModel.ConnObj = cmbConnObj.Text;
                tModel.plcType = (PLCTYPE)Enum.Parse(typeof(PLCTYPE), cmbPLCType.Text);
                tModel.StationNum = Int32.Parse(txtStationNum.sText);
                //PLCSetModel tModel = sequence.PLCSetModels.FirstOrDefault(x => x.Name == txtPLCName.sText);

                tModel.Name = txtPLCName.sText;

                MessageBoxEx.Show(string.Format("更新【{0}】成功", tModel.Name));
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnPLCSave_Click:" + ex.Message);
            }
        }

        private void btnPLCAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                if (string.IsNullOrEmpty(txtPLCName.sText))
                {
                    MessageBoxEx.Show("名称为空...");
                    return;
                }
                PLCSetModel tModel = new PLCSetModel()
                {
                    Name = txtPLCName.sText,
                    ConnType = cmbConnType.Text,
                    ConnObj = cmbConnObj.Text,
                    plcType = (PLCTYPE)Enum.Parse(typeof(PLCTYPE), cmbPLCType.Text),
                    StationNum = Int32.Parse(txtStationNum.sText),
                };

                if (sequence.PLCSetModels.FindIndex(x => x.Name == txtName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在名称:" + txtName.sText);
                    return;
                }

                sequence.PLCSetModels.Add(tModel);

                int id = 0;
                foreach (var item in sequence.PLCSetModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("新增【{0}】成功", tModel.Name));
                UpdatePLC();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnPLCAdd_Click:" + ex.Message);
            }
        }

        private void btnPLCDel_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                PLCSetModel tModel = sequence.PLCSetModels.FirstOrDefault(x => x.Name == txtPLCName.sText);

                sequence.PLCSetModels.Remove(tModel);

                int id = 0;
                foreach (var item in sequence.PLCSetModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("删除【{0}】成功", tModel.Name));
                UpdatePLC();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnPLCDel_Click:" + ex.Message);
            }
        }

        private void cmbPLC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PLCSetModel tModel = cmbPLC.SelectedItem as PLCSetModel;
                if (tModel == null)
                {
                    return;
                }
                txtPLCName.sText = tModel.Name;
                cmbPLCType.Text = tModel.plcType.ToString();
                cmbConnObj.Text = tModel.ConnObj;
                cmbConnType.Text = tModel.ConnType;
                txtStationNum.sText = tModel.StationNum.ToString();

                switch (tModel.plcType)
                {
                    case PLCTYPE.ABB:
                        //IControlProvider<BaseEquip>.Register<MitsubishiControl.Equip>();
                        break;
                    //case PLCTYPE.三菱:
                    //    IControlProvider<BaseEquip>.Register<MitsubishiControl.Equip>();
                    //    break;
                    //case PLCTYPE.松下:
                    //    if (tModel.ConnType == "串口方式")
                    //    {
                    //        IControlProvider<BaseEquip>.Register<PanasonicControl.COM.Equip>();
                    //    }
                    //    else
                    //    {
                    //        //IControlProvider<BaseEquip>.Register<PanasonicControl.TCP.Equip>();
                    //    }
                    //    break;
                    //case PLCTYPE.欧姆龙:
                    //    if (tModel.ConnType == "串口方式")
                    //    {
                    //        IControlProvider<BaseEquip>.Register<PLC.Equip.OMRON.HostLink.COM.Equip>();
                    //    }
                    //    else
                    //    {
                    //        IControlProvider<BaseEquip>.Register<PLC.Equip.OMRON.FINS.GroupNet.Equip>();
                    //    } 
                    //    break;
                    //case PLCTYPE.汇川:
                    //    IControlProvider<BaseEquip>.Register<PLC.Equip.Inovance.Equip>();
                    //    break;
                    //case PLCTYPE.西门子:
                    //    //IControlProvider<BaseEquip>.Register<PLC.Equip.Siemens.S7.Default.Equip>();
                    //    break;
                    //case PLCTYPE.台达: 
                    //    IControlProvider<BaseEquip>.Register<DMTControl.Equip>();
                    //    break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("cmbPLC_SelectedIndexChanged:" + ex.Message);
            }
        }

        private void cmbConnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbConnType.SelectedIndex == 0)
                {
                    cmbConnObj.DataSource = null;
                    cmbConnObj.DataSource = XmlControl.sequenceModelNew.ComModels;
                    cmbConnObj.DisplayMember = "Name";
                }
                else if(cmbConnType.SelectedIndex == 1)
                {
                    cmbConnObj.DataSource = null;
                    cmbConnObj.DataSource = XmlControl.sequenceModelNew.TCPIPModels;
                    cmbConnObj.DisplayMember = "Name";
                }
                else
                {
                    cmbConnObj.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("cmbConnType_SelectedIndexChanged:" + ex.Message);
            }
        }
         
        private void btnOpenPLC_Click(object sender, EventArgs e)
        {
            try
            {
                //BaseEquip m_baseEquip;
                //m_baseEquip = IControlProvider<BaseEquip>.Get();

                //m_baseEquip.Main.IpAddr = "192.168.0.1";
                //m_baseEquip.Main.PortNum = "9500";

                //m_baseEquip.Name = cmbConnObj.Text;
                
                //bool bOpen = m_baseEquip.Open();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnDebug_Click(object sender, EventArgs e)
        {
            try
            {                
                //if(cmbConnType.SelectedIndex == 2)
                //{
                //    BaseEquip m_baseEquip;
                //    m_baseEquip = IControlProvider<BaseEquip>.Get();
                //    m_baseEquip.OpenDebugView();
                //}
                //else
                //{
                //    ShowControl view = new ShowControl(new PlcDebugView(), "Plc调试");
                //    view.Show();
                //}

                //m_baseEquip.Name = cmbConnObj.Text; 
                //m_baseEquip.Open();
                //m_baseEquip.Close();

                //string str = "";

                //Process p = new System.Diagnostics.Process();
                //p.StartInfo.FileName = "cmd.exe";
                //p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                //p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                //p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                //p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                //p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                //p.Start();//启动程序

                ////向cmd窗口发送输入信息
                //p.StandardInput.WriteLine(str + "&exit");

                //p.StandardInput.AutoFlush = true;

                return;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnDebug_Click", ex.Message);
            }
        }
        #endregion

        #region 串口模拟操作
        SerialControl m_serialControl = new SerialControl();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                ComModel comModel = cmbCom.SelectedItem as ComModel;
                if (comModel != null)
                {
                    try
                    {
                        if (btnOpen.Text == "打开")
                        {
                            m_serialControl.Run(comModel, BaseController.ControlType.SerialPortOpen);
                            logView1.LogMessage("串口" + comModel.ComPortName + "打开成功");
                            btnOpen.Text = "关闭";
                        }
                        else
                        {
                            m_serialControl.Run(comModel, BaseController.ControlType.SerialPortClose);
                            logView1.LogMessage("串口" + comModel.ComPortName + "关闭成功");
                            btnOpen.Text = "打开";
                        }
                    }
                    catch (Exception ex)
                    {
                        logView1.LogMessage("串口" + comModel.ComPortName + btnOpen.Text + "失败:" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                ComModel comModel = cmbCom.SelectedItem as ComModel;
                comModel.SendContent = txtSendContent.sText;

                var resultModel = m_serialControl.Run(comModel, BaseController.ControlType.SerialPortOpen);
                if (!resultModel.RunResult)
                {
                    logView1.LogMessage("打开串口失败" + resultModel.ErrorMessage);
                    return;
                }
                resultModel = m_serialControl.Run(comModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    logView1.LogMessage("发送串口消息失败" + resultModel.ErrorMessage);
                    return;
                }
                logView1.LogMessage("发送串口消息" + comModel.SendContent);
            }
            catch (Exception ex)
            {
                logView1.LogShowMessage(ex.Message);
            }
        }

        private void btnSendRecv_Click(object sender, EventArgs e)
        {
            try
            {
                ComModel comModel = cmbCom.SelectedItem as ComModel;
                comModel.SendContent = txtSendContent.sText;

                string strResult = "";
                comModel.SendContent = txtSendContent.sText;
                var resultModel = m_serialControl.Run(comModel, BaseController.ControlType.SerialPortSend);
                resultModel = m_serialControl.Run(comModel, BaseController.ControlType.SerialPortReceive);


                if (resultModel.ObjectResult != null)
                {
                    strResult = resultModel.ObjectResult.ToString().Trim('\r', '\n');
                }

                logView1.LogMessage("Send:" + comModel.SendContent);
                logView1.LogMessage("Receive:" + strResult);
            }
            catch (Exception ex)
            {
                logView1.LogShowMessage(ex.Message);
            }
        }

        bool m_comListen = false;
        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                ComModel comModel = cmbCom.SelectedItem as ComModel;
                if (!m_serialControl.Init(comModel))
                {
                    logView1.LogShowMessage("连接失败！");
                }
                if (btnListen.Text == "监听")
                {
                    logView1.LogMessage("开始监听！");
                    btnListen.Text = "停止监听";
                    m_comListen = true;
                    Thread t = new Thread(new ThreadStart(() =>
                    {
                        while (true)
                        {
                            if (Global.Break)
                            {
                                return;
                            }
                            if (!m_comListen)
                            {
                                if (!m_close)
                                {
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        btnListen.Text = "监听";
                                        logView1.LogMessage("结束监听！");
                                    }));
                                }
                                break;
                            }
                            var resultModel = m_serialControl.Run(comModel, ControlType.SerialPortReceive);
                            if (resultModel.ObjectResult != null && !string.IsNullOrEmpty(resultModel.ObjectResult.ToString()))
                            {
                                logView1.LogMessage(resultModel.ObjectResult.ToString().Trim('\0'));
                            }
                            Thread.Sleep(100);
                        }
                    }));
                    t.Start();
                }
                else
                {
                    logView1.LogMessage("正在结束监听！");
                    btnListen.Text = "正在结束...";
                    m_comListen = false;
                }
            }
            catch (Exception ex)
            {
                logView1.LogMessage(ex.Message);
            }
        }
        #endregion

        #region 网口模拟操作
        public delegate bool Del_TestPing(TCPIPModel tcpipModel);
        public static Del_TestPing m_DelTestPing;
        private void btnTestPing_Click(object sender, EventArgs e)
        {
            try
            {
                TCPIPModel tcpipModel = cmbSocket.SelectedItem as TCPIPModel;
                if (m_DelTestPing(tcpipModel))
                {
                    ShowLog(tcpipModel.Name + "打开成功");
                }
                else
                {
                    ShowLog(tcpipModel.Name + "打开失败");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public delegate void Del_TcpSend(TCPIPModel tcpipModel, string strSend);
        public static Del_TcpSend m_DelTcpSend;
        private void btnTcpSend_Click(object sender, EventArgs e)
        {
            try
            {
                TCPIPModel tcpipModel = cmbSocket.SelectedItem as TCPIPModel;
                m_DelTcpSend(tcpipModel, txtTcpContent.sText);

                ShowLog(tcpipModel.Name + "发送:" + txtTcpContent.sText);
            }
            catch (Exception ex)
            {
                ShowLog("发送失败");
            }
        }

        private void btnTcpSendRecv_Click(object sender, EventArgs e)
        {
            try
            {
                TCPIPModel tcpipModel = cmbSocket.SelectedItem as TCPIPModel;
                m_DelTcpSend(tcpipModel, txtTcpContent.sText);

                Stopwatch sp = new Stopwatch();
                sp.Start();
                Thread t = new Thread(new ThreadStart(() =>
                {
                    while (sp.ElapsedMilliseconds < 5000)
                    {
                        string str = m_DelTcpListen(tcpipModel);
                        if (!string.IsNullOrEmpty(str))
                        {
                            ShowLog(str);
                        }
                        Thread.Sleep(100);
                    }
                }));
                t.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public delegate string Del_TcpListen(TCPIPModel tcpipModel);
        public static Del_TcpListen m_DelTcpListen;
        bool m_tcpListen = false;
        private void btnTcpListen_Click(object sender, EventArgs e)
        {
            try
            {
                TCPIPModel tcpipModel = cmbSocket.SelectedItem as TCPIPModel;
                if (btnTcpListen.Text == "监听")
                {
                    logView2.LogMessage("开始监听！");
                    btnTcpListen.Text = "停止监听";
                    m_tcpListen = true;
                    Thread t = new Thread(new ThreadStart(() =>
                    {
                        while (true)
                        {
                            if (Global.Break)
                            {
                                return;
                            }
                            if (!m_tcpListen)
                            {
                                if (!m_close)
                                {
                                    this.BeginInvoke(new Action(() =>
                                    { 
                                        btnTcpListen.Text = "监听";
                                        logView2.LogMessage("结束监听！"); 
                                    }));
                                }
                                break;
                            }

                            string str = m_DelTcpListen(tcpipModel);
                            if (!string.IsNullOrEmpty(str))
                            {
                                ShowLog(str);
                            }
                            Thread.Sleep(100);
                        }
                    }));
                    t.Start();
                }
                else
                {
                    logView2.LogMessage("正在结束监听！");
                    btnTcpListen.Text = "正在结束...";
                    m_tcpListen = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowLog(string strLog)
        {
            try
            {
                logView2.LogMessage(strLog);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
         
        #region Ftp客户端 配置
        private void btnFtp_Connect_Click(object sender, EventArgs e)
        {

        }

        private void btnFtp_Del_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                FtpClientModel ftpModel = sequence.FtpClientModels.FirstOrDefault(x => x.Name == txtFtpName.sText);

                sequence.FtpClientModels.Remove(ftpModel);

                int id = 0;
                foreach (var item in sequence.FtpClientModels)
                {
                    item.Id = id;
                    id++;
                }

                // XmlControl.SaveToXml(GlobalCore.Global.SequencePath, sequence, sequence.GetType());

                MessageBoxEx.Show(string.Format("删除【{0}】成功", ftpModel.Name));
                UpdateFtp();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnFtp_Del_Click:" + ex.Message);
            }
        }

        private void btnFtp_Add_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                if (string.IsNullOrEmpty(txtFtpName.sText))
                {
                    MessageBoxEx.Show("名称为空...");
                    return;
                }
                FtpClientModel ftpModel = new FtpClientModel()
                {
                    Name = txtFtpName.sText,
                    IPAddress = txtFtpIP.sText,
                    PortNum = Int32.Parse(txtFtpPort.sText),
                    UserName = txtUserName.sText,
                    PassWord = txtPassWord.sText
                };

                if (sequence.FtpClientModels.FindIndex(x => x.Name == txtFtpName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在名称:" + txtFtpName.sText);
                    return;
                }

                sequence.FtpClientModels.Add(ftpModel);

                int id = 0;
                foreach (var item in sequence.FtpClientModels)
                {
                    item.Id = id;
                    id++;
                }

                MessageBoxEx.Show(string.Format("新增【{0}】成功", ftpModel.Name));
                UpdateFtp();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnFtp_Add_Click:" + ex.Message);
            }
        }

        private void btnFtp_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                FtpClientModel ftpModel = sequence.FtpClientModels.FirstOrDefault(x => x.Name == txtFtpName.sText);

                ftpModel.Name = txtFtpName.sText;
                ftpModel.IPAddress = txtFtpIP.sText;
                ftpModel.PortNum = Int32.Parse(txtFtpPort.sText);
                ftpModel.UserName = txtUserName.sText;
                ftpModel.PassWord = txtPassWord.sText;

                //XmlControl.SaveToXml(GlobalCore.Global.SequencePath, sequence, sequence.GetType());

                MessageBoxEx.Show(string.Format("更新【{0}】成功", ftpModel.Name)); 
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("btnFtp_Save_Click:" + ex.Message);
            }
        }

        private void cmbFtp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FtpClientModel ftpModel = cmbFtp.SelectedItem as FtpClientModel;
                if (ftpModel == null)
                {
                    return;
                }

                txtFtpName.sText = ftpModel.Name;
                txtFtpIP.sText = ftpModel.IPAddress;
                txtFtpPort.sText = ftpModel.PortNum.ToString();
                txtUserName.sText = ftpModel.UserName;
                txtPassWord.sText = ftpModel.PassWord;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("cmbFtp_SelectedIndexChanged:" + ex.Message);
            }
        }

        #endregion

        #region SFC配置
        private void btnSFC_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                sequence.SfcModel.UserName = txtSFC_UserName.sText;
                sequence.SfcModel.PassWord = txtPassWord_SFC.sText;
                sequence.SfcModel.StationID = txtStationID.sText;
                sequence.SfcModel.Lang = txtLang.sText;
                sequence.SfcModel.Site = txtSite.sText;
                sequence.SfcModel.Bu = txtBu.sText;
                sequence.SfcModel.AccType = txtAccType.sText;

                MessageBoxEx.Show("更新SFC成功");
            }
            catch (Exception ex)
            { 
                MessageBoxEx.Show("btnSFC_Save_Click:" + ex.Message);
            }
        }

        private void btnSFC_Del_Click(object sender, EventArgs e)
        {

        }

        private void UpdateSFC()
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                txtSFC_UserName.sText = sequence.SfcModel.UserName;
                txtPassWord_SFC.sText = sequence.SfcModel.PassWord;
                txtStationID.sText = sequence.SfcModel.StationID;
                txtLang.sText = sequence.SfcModel.Lang;
                txtSite.sText = sequence.SfcModel.Site;
                txtBu.sText = sequence.SfcModel.Bu; 
                txtAccType.sText = sequence.SfcModel.AccType; 
            }
            catch (Exception ex)
            {
                 
            }
        }

        #endregion
    }
}
