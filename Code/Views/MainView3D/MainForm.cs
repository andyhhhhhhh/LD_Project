using AlgorithmController;
using DevComponents.DotNetBar;
using GlobalCore;
using HalconDotNet;
using HalconView;
using Infrastructure.Log;
using JsonController;
using ManagementView._3DViews;
using ManagementView.Comment;
using ManagementView.EditView;
using ManagementView.EncyptView;
using ManagementView.MotorView;
using ManagementView.Popup;
using ManagementView.UserView;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;
using ManagementView;
using System.Runtime.InteropServices;
using MyFormDesinger;
using FormDesignView;
using CameraContorller;
using System.Data.OleDb;
using ProcessController;
using BaseModels;

namespace MainView3D
{
    public partial class MainForm : Form
    {
        #region 属性
        HSmartWindow m_hSmartWindow;
        /// <summary>
        /// 软件运行时长
        /// </summary>
        Stopwatch m_timer = new Stopwatch();
        bool m_bGetCpu = false;
        public static JsonControl m_jsonControl = new JsonControl();
        RunEditView m_RunView;
        bool m_IsPrintLog = false;
        bool m_IsInitDevice = false;
        bool m_IsStopShowMsg = false;
        bool bregist = true;
        //欢迎界面显示
        public delegate void ShowStartStatus(string strStatus);
        public static ShowStartStatus m_ShowStarStatus;
        
        /// <summary>
        /// 参数界面
        /// </summary>
        ParamSetView m_paramView = new ParamSetView();
        /// <summary>
        /// 下料参数界面
        /// </summary>
        UnLoadParamView m_unLoadView = new UnLoadParamView();
        /// <summary>
        /// 范围参数界面
        /// </summary>
        ParamConfigView m_rangeView = new ParamConfigView();
        /// <summary>
        /// Map录入界面
        /// </summary>
        MapView m_mapView = new MapView();

        /// <summary>
        /// 执行步骤界面
        /// </summary>
        StepView m_stepView;
        /// <summary>
        /// 报警统计界面
        /// </summary>
        AlarmView m_alarmView = new AlarmView();
        /// <summary>
        /// 统计数据界面
        /// </summary>
        CountView m_countView = new CountView();
         

        /// <summary>
        /// 主流程执行实例
        /// </summary>
        ProcessControl m_ProcessControl = new ProcessControl();

        /// <summary>
        /// 按钮按下时长
        /// </summary>
        System.Windows.Forms.Timer m_downTimer = new System.Windows.Forms.Timer();
        #endregion

        #region MainForm
        public MainForm()
        {
            InitializeComponent();

            UserLogin.UserSetEvent += M_UserLogin_UserSelectEvent;

            SetView.ConfirmEvent += M_SetView_ConfirmEvent;
            
            CommFuncView.m_DelOutExLog += ShowExLog;
            CommFuncView.m_DelOutPutLog += ShowLog;
            EngineController.CsEngine.ToolScriptFun.m_DelShowLog += DelShowLog;
            EngineController.CsEngine.ToolScriptFun.m_DelShowObject += DelShowObject;

            EButton.m_DelEButton += Del_EButtonRun;
            EHWindow.m_DelSetLayoutWindow += Del_EHSmartWindow;
            ELog.m_DelELog += Del_ELog;
            EButtonPro.m_DelEButtonProRun += Del_EButtonProRun;
            EDataOutput.m_DelEDataOutPut += Del_EOutPut;
            RunEditView.m_DelRunViewClose += Del_RunViewClose;
            RunEditView.m_DelToEditView += Del_ToEditView;
            ELight.m_ELightControl += m_ProcessControl.Del_ELightControl;

            MyFormDesign.m_DelFormClose += Del_DesignClose;

            Communication_View.m_DelTcpSend += m_ProcessControl.TCPSend;
            Communication_View.m_DelTestPing += m_ProcessControl.InitTCP;
            Communication_View.m_DelTcpListen += m_ProcessControl.TCPRecive;

            SelectSequenceView.SelectEvent += M_SelectSequenceView_SelectEvent;

            ProcessControl.m_DelOutExLog += ShowExLog;
            ProcessControl.m_DelOutPutLog += ShowLog;
            m_ProcessControl.m_DelPcAlarm += Del_PCAlarm;
            m_ProcessControl.m_DelGetTestData += m_countView.GetTestData;
            m_ProcessControl.m_DelMessageBox += MessageBoxInfo;
            m_ProcessControl.DispPicEvent += M_ProcessControl_DispPicEvent;
            m_ProcessControl.m_DelOcrView += Del_OcrConfirmView;
            m_ProcessControl.m_DelRefreshLoadParam += Del_RefreshLoadParam;
            m_ProcessControl.m_DelRefreshUnLoadParam += Del_RefreshUnLoadParam; 
            m_ProcessControl.m_DelStart += Del_StartAction;
            m_ProcessControl.m_DelStop += Del_StopAction; 
            m_ProcessControl.m_DelEmergency += Del_EmergencyAction;
            m_ProcessControl.m_DelSaveScreen += Del_SaveScreen;

            ParamSetView.ConfirmEvent += M_ParamSetView_ConfirmEvent;
            ParamConfigView.ConfirmEvent += M_ParamSetView_ConfirmEvent;

            m_countView.ClearEvent += M_CountView_ClearEvent;

            m_downTimer.Interval = 1000;
            m_downTimer.Tick += new EventHandler(OnMouseDownTimer_Tick);

            //设置分辨率
            SetResolution();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (m_ShowStarStatus != null)
            {
                m_ShowStarStatus("检查软件注册...");
            }

            ribbonControl1.SelectedRibbonTabItem = ribbonTabItem2;
            //是否已经注册
            if (!RegisterFunc())
            {
                //如果未注册再次搜索C盘下面的注册文件
                RegistFileHelper.RegistInfofile = "C://Regist.key";
                if (!RegisterFunc())
                {
                    MessageBoxEx.Show(this, "软件未注册", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, true);
                    btnLogin.Enabled = false;
                    btnUserManage.Enabled = false;
                    ribbonPanel2.Enabled = false;
                    btnAddSeq.Enabled = false;
                    bar2.Enabled = false;
                    bar1.Enabled = false;
                    StartButton1.Enabled = false;
                    btnOpenConfig.Enabled = false;
                    btnSaveConfig.Enabled = false;
                    btnLayOut.Enabled = false;
                    ribbonControl1.SelectedRibbonTabItem = ribbonTabItem1;
                    bregist = false;
                }
            }

            if (m_ShowStarStatus != null)
            {
                m_ShowStarStatus("初始化工作区间...");
            }

            var systemPara = m_jsonControl.SystemPara;
            m_ProcessControl.m_parameter = systemPara;
            Global.EnableOsk = systemPara.EnableOsk;
            Global.EnableRunView = systemPara.EnableRunView;
            m_IsPrintLog = systemPara.IsPrintLog;
            m_IsInitDevice = systemPara.IsInitDevice;
            m_IsStopShowMsg = systemPara.IsStopShowMsg;
            HSmartWindow.m_bShowCross = systemPara.IsShowCross;

            m_hSmartWindow = new HSmartWindow();
            CommHelper.LayoutChildFillView(sPanelCamera, m_hSmartWindow); 
            m_timer.Start();

            Global.UserName = Global.OperatorName;
            lblLoginUser.Text = "登录身份: " + "操作员" + "";

            m_ProcessControl.InitMotorModel();
            InitData();

            m_stepView = new StepView(m_ProcessControl.m_stepCount); 
            m_ProcessControl.m_DelShowStep += m_stepView.ShowStep;
            CommHelper.LayoutChildFillView(panelShowStep, m_stepView);
            CommHelper.LayoutChildFillView(panelAlarm, m_alarmView);
            CommHelper.LayoutChildFillView(panelSetView, m_paramView);
            CommHelper.LayoutChildFillView(panelCoutView, m_countView);
            CommHelper.LayoutChildFillView(panelUnLoad, m_unLoadView);
            CommHelper.LayoutChildFillView(panelRange, m_rangeView);
            CommHelper.LayoutChildFillView(panelMap, m_mapView);

            dockSite2.Dock = DockStyle.Fill;
            bar2.SelectedDockTab = 0;
            bar1.SelectedDockTab = 0;
            bar5.SelectedDockTab = 0;
            tabControl1.SelectedTabIndex = 0;

            if (m_ShowStarStatus != null)
            {
                m_ShowStarStatus("初始化主界面...");
            }
             
            //dataGridView1.DefaultCellStyle.Font = new Font("微软雅黑", 9.75F, FontStyle.Regular);

            InitProcess();
            SetSequenceName();

            //获取CPU使用率
            m_bGetCpu = true;
            GetUsedCPU();
            AddRecentItem();
            
            if (!string.IsNullOrEmpty(systemPara.SavePath))
            {
                //检查是否有D盘
                var driveD = DriveInfo.GetDrives().ToList().Any(x => x.Name.Contains("D:"));
                if (!driveD)
                {
                    string drive = systemPara.SavePath.Substring(0, 1);
                    systemPara.SavePath = systemPara.SavePath.Replace(drive + ":", "C:");
                }
                Log.LogDirectory = systemPara.SavePath + "//";
            }

            if (systemPara.IsRunOtherSoft)
            {
                if (m_ShowStarStatus != null)
                {
                    m_ShowStarStatus("启动第三方软件...");
                }
                RunOtherSoft(systemPara.OtherSoftAddr);
            }

            Global.IsProVisible = systemPara.IsProVisible;
            Global.IsRealDisplay = systemPara.IsRealDisplay;
            Global.IsFullScreen = systemPara.IsFullScreen;

            SetViewStyle();
            SetViewName();

            ShowLog("***********************软件启动*************************"); 
            ShowLog(systemPara.MotionCard);

            //设置窗口是否全屏显示
            if (!Global.IsFullScreen)
            {
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width + 2, Screen.PrimaryScreen.WorkingArea.Height + 2);
            }
            

            if (m_ShowStarStatus != null)
            {
                m_ShowStarStatus("加载数据完毕...");
            }
            
            //开机自动启动时
            if(systemPara.OpenAutonRun && bregist)
            {
                btnStart_Click(null, null);
                m_ProcessControl.CameraRealDisplay();
            }

            ScrollAlarmSet();
            Global.SetSystemStauts(Global.EnumSystemRunStatus.Stop);

            if(XmlControl.sequenceModelNew!= null && XmlControl.sequenceModelNew.LDModel != null)
            {
                chkIsShieldBuzzer.Checked = XmlControl.sequenceModelNew.LDModel.IsShieldBuzzer;
                chkIsIngoreDoor.Checked = XmlControl.sequenceModelNew.LDModel.IsShieldDoor;
            }

            m_ProcessControl.EmergencyThread();

            m_ProcessControl.GetData();
            m_ProcessControl.PauseTiming();

            ////取消跨线程的访问
            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void RunOtherSoft(string path)
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                string fileName = Path.GetFileNameWithoutExtension(path);

                Process[] processes = Process.GetProcessesByName(fileName);
                //遍历与当前进程名称相同的进程列表 
                if (processes.Length == 0)
                {
                    Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("RunOtherSoft", ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bShowBox)
            {
                DialogResult dr = MessageBoxEx.Show(this, "即将退出程序，请确认是否保存配置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    //如果在运行则需要先停止
                    if (Global.Run)
                    {
                        Stop_Action();
                        Thread.Sleep(500);
                    } 
                    //e.Cancel = false;  //关闭窗体 
                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));

                    string path = Global.CurrentPath + "//Sequence//Card//Card.dsr";
                    XmlControl.SaveToXml(path, XmlControl.controlCardModel, typeof(ControlCardModel));

                    DeleteFiles();
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;   //不执行操作
                    return;
                }
            }
            //如果在运行则需要先停止
            if (Global.Run)
            {
                Stop_Action();
                Thread.Sleep(200);
            }

            Global.Frame_Start = false;

            Global.IsRealDisplay = false;

            panelDockDevice.Visible = false;

            Global.Break = true;

            m_bGetCpu = false;
            timer1.Stop();
            m_timer.Stop();
            
            //关闭设备
            m_ProcessControl.CloseDevice();
            //Environment.Exit(0);

            Application.ExitThread();
            Process.GetCurrentProcess().Kill();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            try
            {
                this.dockSite1.Width = this.Width - 425;
                
                if (!m_jsonControl.SystemPara.MinToPallet)
                {
                    return;
                }
                //判断是否选择的是最小化按钮
                if (WindowState == FormWindowState.Minimized)
                {
                    //隐藏任务栏区图标
                    this.ShowInTaskbar = false;
                    //图标显示在托盘区
                    notifyIcon1.Visible = true;
                } 
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }
        
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化Sequence
        /// </summary>
        private void InitData()
        {
            try
            {
                Global.SequencePath = Global.GetStrNodeValue("SequencePath");

                if (!File.Exists(Global.SequencePath))
                {
                    string strName = Path.GetFileName(Global.SequencePath);
                    string str = Global.CurrentPath + "//Sequence//" + strName;
                    if (File.Exists(str))
                    {
                        Global.SequencePath = str;
                        Global.SaveNodeValue("SequencePath", str);
                    }
                }

                SequenceModel sequence = new SequenceModel();
                sequence = XmlControl.LoadFromXml(Global.SequencePath, sequence.GetType()) as SequenceModel;
                if (sequence.ChildSequenceModels == null || sequence.ChildSequenceModels.Count == 0)
                {
                    sequence.ChildSequenceModels.Add(new ChildSequenceModel());
                }
                XmlControl.sequenceModelNew = sequence;
                //XmlControl.sequenceSingle = sequence.ChildSequenceModels[0].SingleSequenceModels[0];
                XmlControl.sequenceSingle = null;
                if (sequence != null && !string.IsNullOrEmpty(sequence.BasePath))
                {
                    Global.Model3DPath = sequence.BasePath;
                    if (!Directory.Exists(Global.Model3DPath))
                    {
                        Directory.CreateDirectory(Global.Model3DPath);
                    }

                    if (sequence.Camera3DSet != null && sequence.Camera3DSet.Count > 0)
                    {
                        Global.XYResolution = sequence.Camera3DSet[0].XYResolution;
                        Global.ZResolution = sequence.Camera3DSet[0].ZResolution;
                    }
                    Global.ProductInfo = sequence.ProductInfo;
                }

                m_LayOutNum = sequence.LayOutNum == 0 ? 1 : sequence.LayOutNum;
                InitLayOutView();

                BackupProject(Global.SequencePath);
                ReNameConfig();

                InitProcess();

                InitProduct();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("InitData", ex.Message);
            }
        }

        /// <summary>
        /// 初始化产品
        /// </summary>
        private void InitProduct()
        {
            try
            {
                var listModel = XmlControl.sequenceModelNew.LDModel.paramRangeModels;
                if (listModel == null)
                {
                    return;
                }

                cmbProduct.Items.Clear();
                foreach (var item in listModel)
                {
                    cmbProduct.Items.Add(item.Name);
                }

                if (!string.IsNullOrEmpty(XmlControl.sequenceModelNew.ProductInfo))
                {
                    cmbProduct.Text = XmlControl.sequenceModelNew.ProductInfo;
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                XmlControl.sequenceModelNew.ProductInfo = cmbProduct.Text;
                m_ProcessControl.LoadData(XmlControl.sequenceModelNew);
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 备份工程文件
        /// </summary>
        /// <param name="strPath">当前工程文件路径</param>
        private void BackupProject(string strPath)
        {
            try
            {
                string nowPath = Path.GetDirectoryName(strPath);
                string sequenceName = Path.GetFileName(strPath);

                string newPath = nowPath + "//backup//";
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                File.Copy(strPath, newPath + sequenceName, true);
                File.Copy(nowPath + "//Card//Card.dsr", newPath + "Card.dsr", true);//备份控制卡文件

                //备份基本配置文件
                int baseIndex = Global.Model3DPath.LastIndexOf("\\");
                string baseName = Global.Model3DPath.Substring(baseIndex + 1);
                string baseNewPath = Global.Model3DPath.Substring(0, baseIndex) + "//backup//" + baseName;

                if (!Directory.Exists(baseNewPath))
                {
                    Directory.CreateDirectory(baseNewPath);
                }
                CommFuncView.CopyFiles(Global.Model3DPath, baseNewPath);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("BackupProject", ex.Message);
            }
        }

        /// <summary>
        /// 重命名配置文件
        /// </summary>
        private void ReNameConfig()
        {
            try
            {
                string strName = Path.GetFileNameWithoutExtension(Global.SequencePath);
                //string str = Path.GetDirectoryName();
                if (!Global.Model3DPath.Contains(strName))
                {
                    string strBasePath = Path.GetDirectoryName(Global.Model3DPath);
                    string str = Path.GetFileName(Global.Model3DPath);
                    int index = str.LastIndexOf('#');
                    if (index != -1)
                    {
                        string str2 = str.Substring(0, index);
                        str = str2 + "#" + strName;
                    }
                    else
                    {
                        str += ("#" + strName);
                    }

                    string newPath = strBasePath + "\\" + str;
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    CommFuncView.CopyFiles(Global.Model3DPath, newPath);

                    XmlControl.sequenceModelNew.BasePath = newPath;
                    Global.Model3DPath = newPath;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 初始化流程界面
        /// </summary>
        private void InitProcess()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("InitProcess", ex.Message);
            }
        }

        /// <summary>
        /// 刷新选择的Sequence路径名称
        /// </summary>
        private void SetSequenceName()
        {
            try
            {
                lblStatus.Text = "当前项目:" + Global.SequencePath;
                //int index = Global.SequencePath.LastIndexOf("\\");
                //if (index != -1)
                //{
                //    string sequenceName = Global.SequencePath.Substring(index + 1);
                //    lblStatus.Text = "当前工作空间:" + sequenceName;
                //}
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("SetSequenceName", ex.Message);
            }
        }
        #endregion

        #region 检查是否注册

        private string encryptComputer = string.Empty;
        private const int timeCount = 120;
        private bool RegisterFunc()
        {
            string computer = ComputerInfo.GetComputerInfo();
            encryptComputer = new EncryptionHelper().EncryptString(computer);
            //MessageBox.Show(computer);
            if (CheckRegist() == true)
            {
                return true;
            }
            else
            {
                RegistFileHelper.WriteComputerInfoFile(encryptComputer);
                //EncyptForm encypt = new EncyptForm();
                //DialogResult result = encypt.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                //    return true;
                //} 
            }

            return false;
        }
        private bool CheckRegist()
        {
            EncryptionHelper helper = new EncryptionHelper();
            string md5key = helper.GetMD5String(encryptComputer);
            return CheckRegistData(md5key);
        }
        private bool CheckRegistData(string key)
        {
            if (RegistFileHelper.ExistRegistInfofile() == false)
            {
                return false;
            }
            else
            {
                string info = RegistFileHelper.ReadRegistFile();

                //增加使用时间限制 20210115
                bool bJudge = false; 
                bJudge = info.Length > 80 ? true : false;
                int days = 0;
                if (bJudge)
                {
                    string strDay = info.Substring(80);
                    bool breturn = Int32.TryParse(strDay, out days);
                    if(breturn)
                    {
                        info = info.Substring(0, 80);
                    }  
                }

                var helper = new EncryptionHelper(EncryptionHelper.EncryptionKeyEnum.KeyB);
                string registData = helper.DecryptString(info);
                if (key == registData)
                {
                    //增加使用时间限制 20210115
                    if (bJudge)
                    {
                        DateTime currenttime = DateTime.Now;
                        DateTime datetime = File.GetCreationTime(Global.CurrentPath + "//Regist.key");
                        TimeSpan midtime = currenttime - datetime;

                        if (midtime.Days >= days)
                        {
                            MessageBoxEx.Show(this, "使用期限到咯,请联系软件管理员！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error, 
                                MessageBoxDefaultButton.Button1, true);
                            return false;
                        } 
                        else if(days - midtime.Days < 3)
                        {
                            MessageBoxEx.Show(this, string.Format("软件使用期限只剩{0}天啦！", days - midtime.Days), 
                                "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, true);
                        }
                    }
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
        
        #region 事件处理
        private void M_UserLogin_UserSelectEvent(object sender, object e)
        {
            try
            {
                string user = e.ToString();

                lblLoginUser.Text = "登录身份:" + user; 

                bool benable = Global.UserName != Global.OperatorName;
                m_rangeView.SetUserEnable();
                m_paramView.SetUserEnable();
                m_unLoadView.SetUserEnable();
                m_countView.SetUserEnable();
                
                if(benable)
                {
                    //10分钟后切换为操作员权限
                    timerUser.Interval = 30 * 60 * 1000;
                    timerUser.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("M_UserLogin_UserSelectEvent", ex.Message);
            }
        }

        private void M_SetView_ConfirmEvent(object sender, object e)
        {
            try
            {
                var systemPara = m_jsonControl.SystemPara;
                m_ProcessControl.m_parameter = systemPara;
                Global.EnableOsk = systemPara.EnableOsk;
                Global.EnableRunView = systemPara.EnableRunView;
                m_IsPrintLog = systemPara.IsPrintLog;
                m_IsInitDevice = systemPara.IsInitDevice;
                m_IsStopShowMsg = systemPara.IsStopShowMsg;
                Global.IsProVisible = systemPara.IsProVisible;
                Global.IsRealDisplay = systemPara.IsRealDisplay;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("M_SetView_ConfirmEvent", ex.Message);
            }
        }

        private void M_ParamSetView_ConfirmEvent(object sender, object e)
        {
            try
            {
                m_ProcessControl.LoadData(XmlControl.sequenceModelNew);
                m_unLoadView.UpdateData();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("M_ParamSetView_ConfirmEvent", ex.Message);
            }
        }

        private void M_SelectSequenceView_SelectEvent(object sender, object e)
        {
            try
            {
                string path = e.ToString();
                Global.SaveNodeValue("SequencePath", path);
                Global.SequencePath = path;

                InitData();
                SetSequenceName();
                //m_checkItemTool.UpdateData();
                InitProcess();

                SaveRecentItem(path);
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }

        //启动是否启动运行界面
        private void timerShow_Tick(object sender, EventArgs e)
        {
            if (Global.EnableRunView && bregist)
            {
                btnViewEdit_Click(null, null);
            }
            timerShow.Stop();
            timerShow.Dispose();
        } 

        /// <summary>
        /// 切换用户后超过10分钟切换为操作员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUser_Tick(object sender, EventArgs e)
        {
            try
            {
                timerUser.Enabled = false;
                Global.UserName = Global.OperatorName;
                lblLoginUser.Text = "登录身份:" + "操作员";

                bool benable = Global.UserName != Global.OperatorName;
                m_rangeView.SetUserEnable();
                m_paramView.SetUserEnable();
                m_unLoadView.SetUserEnable();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void Del_DesignClose()
        {
            try
            {
                InitLayOutView();

                if (logView1 == null || logView1.IsDisposed)
                {
                    logView1 = new LogView();
                }
                CommHelper.LayoutChildFillView(panelLog, logView1);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 界面显示图片事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_ProcessControl_DispPicEvent(object sender, CameraResultModel e)
        {
            try
            {
                CameraResultModel cameraResultModel = e as CameraResultModel;
                if (cameraResultModel != null)
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (cameraResultModel.Image == null)
                            {
                                return;
                            }

                            HSmartWindow hsmartWindow = GetSmartWindow(cameraResultModel.IndexResult);
                            hsmartWindow.FitImageToWindow((HObject)cameraResultModel.Image, (HObject)cameraResultModel.DispObj, false, cameraResultModel.ResultLabel);

                        }));
                    }
                    else
                    {
                        if (cameraResultModel.Image == null)
                        {
                            return;
                        }

                        HSmartWindow hsmartWindow = GetSmartWindow(cameraResultModel.IndexResult);
                        hsmartWindow.FitImageToWindow((HObject)cameraResultModel.Image, (HObject)cameraResultModel.DispObj, false, cameraResultModel.ResultLabel);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设备报警执行动作
        /// </summary>
        /// <param name="bAlarm"></param>
        /// <param name="strStep"></param>
        private void Del_PCAlarm(BaseResultModel resultModel, int alarmID, string strStep)
        {
            try
            {
                ShowLog(string.Format("设备报警: {0}", strStep), LogLevel.Warning);
                Pause_Action(resultModel != null);
                m_alarmView.AddAlarm(alarmID, strStep);

                string path = Global.DataPath + "Alarm//";
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                m_ProcessControl.SaveCSV(path + DateTime.Now.ToString("yyyyMMdd") + ".csv", "时间,报警ID,报警", string.Format("{0},{1},{2}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"), alarmID, strStep));
                BeginInvoke(new Action(() =>
                {
                    lblAlarm.Text = string.Format("{0} 报警", strStep);
                    lblAlarm.ForeColor = Color.Red;
                    chkHandProtect.Checked = true;
                }));
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 复位报警信息
        /// </summary>
        private void ResetAlarm()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    lblAlarm.Text = "无报警";
                    lblAlarm.ForeColor = Color.DimGray;
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 急停显示信息
        /// </summary>
        private void SetEmergency(bool bEmergency)
        {
            try
            {
                if (bEmergency)
                {
                    lblAlarm.Text = "急停";
                    lblAlarm.ForeColor = Color.Red;
                }
                else
                {
                    lblAlarm.Text = "无报警";
                    lblAlarm.ForeColor = Color.DimGray;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置运行状态
        /// </summary>
        private void SetRun()
        {
            lblRunStatus.BackColor = Color.Lime;
            lblRunStatus.Text = "正在运行";
            lblRuningStatus.BackColor = Color.Lime;
            lblRuningStatus.Text = "正在运行";
        }

        /// <summary>
        /// 计数页面清空执行时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_CountView_ClearEvent(object sender, object e)
        {
            try
            {
                m_ProcessControl.GetData(); 
                m_ProcessControl.RestartTiming();
                m_ProcessControl.PauseTiming();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// OCR确认弹出框
        /// </summary>
        /// <param name="testOcr"></param>
        /// <returns></returns>
        private string Del_OcrConfirmView(string testOcr)
        {
            try
            {
                OCRConfirmView view = new OCRConfirmView(testOcr);
                var result = view.ShowDialog();
                if(result == DialogResult.Yes)
                {
                    return view.InputOcr;
                }
                else
                {
                    return "";
                } 
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 刷新上料参数
        /// </summary>
        private void Del_RefreshLoadParam()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    m_paramView.UpdateData();
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }
         
        /// <summary>
        /// 刷新下料参数
        /// </summary>
        private void Del_RefreshUnLoadParam()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    m_unLoadView.UpdateData();
                }));
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 启动按钮委托
        /// </summary>
        private void Del_StartAction()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    ReStore_Action();
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }
         
        /// <summary>
        /// 停止按钮委托
        /// </summary>
        private void Del_StopAction()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    Pause_Action();
                }));
            }
            catch (Exception ex)
            {

            }
        }
          
        /// <summary>
        /// 急停按钮委托
        /// </summary>
        private void Del_EmergencyAction(bool bTrue)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    if(bTrue)
                    {
                        Stop_Action();
                    }
                    SetEmergency(bTrue);
                }));
            }
            catch (Exception ex)
            {

            }
        }
          
        /// <summary>
        /// 保存结果截屏委托
        /// </summary>
        /// <param name="index">0 工位1 1 工位2</param>
        /// <param name="path">保存路径</param>
        private void Del_SaveScreen(int index, string path)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(20);
                    if (index == 0)
                    {
                        //PanelToBmp.OutTheControllerToPic(panel1, path);
                    }
                    else
                    {
                        //PanelToBmp.OutTheControllerToPic(panel2, path);
                    }
                }));
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }
         
        #endregion

        #region 运动控制配置 报警配置

        private void btnCardConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new CardView(), "控制卡配置");
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                 
            }
        }

        ShowControl m_pointView;
        private void btnPointConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_pointView == null || m_pointView.IsDisposed)
                {
                    m_pointView = new ShowControl(new AxisIOView(), "运动调试");
                }
                m_pointView.Show();
                if (m_pointView.WindowState == FormWindowState.Minimized)
                {
                    m_pointView.WindowState = FormWindowState.Normal;
                }
                m_pointView.BringToFront();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnIODebug_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnAlarmConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new AlarmConfigView(), "报警配置");
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                 
            }
        }

        #endregion

        #region 编辑 运行 外观样式 界面编辑 分辨率 最小化
        private void btnEdit_Click(object sender, EventArgs e)
        {
            bar1.Visible = true;
            bar2.Visible = true;
            ribbonControl1.Expanded = true;
        }

        //运行界面
        private void btnRun_Click(object sender, EventArgs e)
        {
            bar1.Visible = true;
            bar2.Visible = false;
            ribbonControl1.Expanded = false;
        }

        //*********************************样式编辑******************************//
        private void buttonManage_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonItem btn = sender as ButtonItem;
                Global.SaveNodeValue("ViewStyle", btn.Text); 
                Global.SaveColorValue("Color", "");
                SetViewStyle();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("buttonManage_Click", ex.Message);
            }
        }

        private void btnManageStyle_ExpandChange(object sender, EventArgs e)
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
                CommFuncView.ShowErrMsg("btnManageStyle_ExpandChange", ex.Message);
            }
        }

        bool m_ColorSelected = false;
        private eStyle m_BaseStyle = eStyle.Office2010Silver;
        int m_ColorArgb = 0;
        private void buttonStyleCustom_ColorPreview(object sender, ColorPreviewEventArgs e)
        {
            m_ColorArgb = e.Color.ToArgb();
            StyleManager.ColorTint = Color.FromArgb(m_ColorArgb); 
        }

        private void buttonStyleCustom_ExpandChange(object sender, EventArgs e)
        {
            if (buttonStyleCustom.Expanded)
            {
                // Remember the starting color scheme to apply if no color is selected during live-preview
                m_ColorSelected = false;
                m_BaseStyle = StyleManager.Style;
            }
            else
            {
                if (!m_ColorSelected)
                {
                    StyleManager.ChangeStyle(m_BaseStyle, Color.Empty);
                }
            }
        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, EventArgs e)
        {
            try
            {
                m_ColorSelected = true; // Indicate that color was selected for buttonStyleCustom_ExpandChange method
                buttonStyleCustom.CommandParameter = buttonStyleCustom.SelectedColor;
                Global.SaveColorValue("Color", m_ColorArgb.ToString()); 
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
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
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2007;  
                        break;
                    case "Office2007Silver":
                        styleManager1.ManagerStyle = eStyle.Office2007Silver;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2007;  
                        break;
                    case "Office2007Black":
                        styleManager1.ManagerStyle = eStyle.Office2007Black;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2007;  
                        break;
                    case "Office2007VistaGlass":
                        styleManager1.ManagerStyle = eStyle.Office2007VistaGlass;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2007;  
                        break;
                    case "Office2010Silver":
                        styleManager1.ManagerStyle = eStyle.Office2010Silver;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2010;  
                        break;
                    case "Office2010Black":
                        styleManager1.ManagerStyle = eStyle.Office2010Black;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2010;   
                        break;
                    case "Office2010Blue":
                        styleManager1.ManagerStyle = eStyle.Office2010Blue;
                        dotNetBarManager1.Style = eDotNetBarStyle.Office2010;  
                        break;
                    case "Windows7Blue":
                        styleManager1.ManagerStyle = eStyle.Windows7Blue;
                        dotNetBarManager1.Style = eDotNetBarStyle.Windows7;  
                        break;
                    case "VisualStudio2010Blue":
                        styleManager1.ManagerStyle = eStyle.VisualStudio2010Blue;
                        dotNetBarManager1.Style = eDotNetBarStyle.VS2005;  
                        break;
                    case "Metro":
                        styleManager1.ManagerStyle = eStyle.Metro;
                        dotNetBarManager1.Style = eDotNetBarStyle.Metro;  
                        break;
                }

                string sColor = Global.GetColorValue("Color");
                bool bresult = int.TryParse(sColor, out m_ColorArgb);
                if (bresult && m_ColorArgb != 0)
                {
                    StyleManager.ColorTint = Color.FromArgb(m_ColorArgb);
                }
                else
                {
                    StyleManager.ColorTint = Color.Empty;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("SetViewStyle", ex.Message);
            }
        }
        
        //******************************样式编辑******************************//


        private void btnViewEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    m_RunView = new RunEditView();
                    lblStatus_TextChanged(null, null);
                    lblRunStatus_TextChanged(null, null);
                    lblLoginUser_TextChanged(null, null);
                    lblRunTime_TextChanged(null, null);
                    lblMemory_TextChanged(null, null);
                    lblCPU_TextChanged(null, null);
                    lblCurrentTime_TextChanged(null, null);
                    lblRunStatus_BackColorChanged(null, null);
                }
                m_bEdit = false;
                m_bShowBox = true;
                m_RunView.m_bShowBox = true;
                m_RunView.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnViewEdit_Click", ex.Message);
            }
        }

        private void btnFormDesign_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Run)
                {
                    MessageBoxEx.Show(this, "设备正在运行中，请停止后再操作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (Global.UserName == Global.OperatorName)
                {
                    MessageBoxEx.Show(this, "操作员无权限!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MyFormDesign formDesign = new MyFormDesign();
                formDesign.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnFormDesign_Click", ex.Message);
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new ProductSelView(), "产品配置");
                view.ShowDialog();
                //SAPQueryView view = new SAPQueryView();
                //view.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnProduct_Click", ex.Message);
            }
        }

        private void bar2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                bar2.DockTabAlignment = eTabStripAlignment.Right;
                bar2.GrabHandleStyle = eGrabHandleStyle.Caption;
            }
            catch (Exception ex)
            {

            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    //还原窗体显示    
                    WindowState = FormWindowState.Normal;
                    //激活窗体并给予它焦点
                    this.Activate();
                    //任务栏区显示图标
                    this.ShowInTaskbar = true;
                    //托盘区图标隐藏
                    //notifyIcon1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }

        //*********************************分辨率编辑******************************//
        private void btnResolution_ExpandChange(object sender, EventArgs e)
        {
            try
            {
                ButtonItem[] btnItems = new ButtonItem[7] {btnResolution1, btnResolution2, btnResolution3,
                    btnResolution4, btnResolution5, btnResolution6, btnResolution7};
                string str = Global.GetNodeValue("Resolution");
                for (int i = 0; i < btnItems.Length; i++)
                {
                    btnItems[i].Checked = btnItems[i].Text == str;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnResolution_ExpandChange", ex.Message);
            }
        }

        private void btnResolution1_Click(object sender, EventArgs e)
        {
            try
            {
                var btnItem = sender as ButtonItem;
                Global.SaveNodeValue("Resolution", btnItem.Text);
                SetResolution();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnResolution1_Click", ex.Message);
            }
        }

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
        //*********************************分辨率编辑******************************//

        #endregion
        
        #region 开始 暂停 停止 复位
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Global.Run)
                {
                    if (Global.IsHandProtect)
                    {
                        ShowLog("请解锁手动保护!");
                        return;
                    }

                    //if (!MessageBoxInfo("设备启动，请小心操作，点击确认，开始运行！", 2))
                    if (!MessageBoxInfo(m_ProcessControl.GetSelectInfo(XmlControl.sequenceModelNew), 2))
                    {
                        MessageBoxEx.Show(this, "用户取消!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, true);
                        return;
                    }

                    if (!MessageBoxInfo("料盘是否根据上次状态，继续运行？\r\n是点击确定，否点击取消！", 2))
                    {
                        m_unLoadView.ResetConfig();
                        m_paramView.ResetConfig();
                    }

                    if (!m_ProcessControl.Init(XmlControl.sequenceModelNew))
                    {
                        MessageBoxEx.Show(this, "初始化失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ShowLog("初始化成功");
                    
                    Global.Break = false;
                    var resultModel = m_ProcessControl.Run(null, BaseController.ControlType.Run);
                    if (!resultModel.RunResult)
                    {
                        ShowLog(resultModel.ErrorMessage, LogLevel.Error);
                        return;
                    }
                    Global.SetSystemStauts(Global.EnumSystemRunStatus.Run);

                    SetRun();
                    ResetAlarm();

                    BeginInvoke(new Action(() =>
                    {
                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                        btnPause.Enabled = true;
                        btnAllGoHome.Enabled = false;
                    }));

                    ShowLog("点击开始按钮,开始运行..");
                    m_ProcessControl.CameraRealDisplay();
                }
                else
                {
                    MessageBoxEx.Show(this, "设备正在运行中，不可再次运行！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnStart_Click", ex.Message);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                Pause_Action(); 
                ShowLog("点击暂停按钮,暂停运行..");
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnPause_Click", ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_IsStopShowMsg)
                {
                    Pause_Action();
                    DialogResult dr = MessageBoxEx.Show("请确认程序是否需要停止运行?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
                    if (dr == DialogResult.No)
                    {
                        ReStore_Action();
                        return;
                    }
                }
                Stop_Action();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnStop_Click", ex.Message);
            }
        }

        private void btnReStore_Click(object sender, EventArgs e)
        {
            try
            {
                ReStore_Action();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnReStore_Click", ex.Message);
            }
        }
          
        private void btnAllGoHome_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_downTimer.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAllGoHome_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_downTimer.Enabled)
                    m_downTimer.Stop();
            }
        }

        void OnMouseDownTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                m_downTimer.Stop();
                if (!Global.Stop)
                {
                    MessageBoxEx.Show(this, "机台正在运行中，请先停止再操作...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, true);
                    return;
                }
                var result = MessageBoxEx.Show(this, "请确认是否初始化设备?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, true);
                if (result == DialogResult.No)
                {
                    return;
                }

                Application.DoEvents();
                ShowLog("机台正在初始化...");
                Global.IsAllgohome = m_ProcessControl.AxisAllGoHome();

                if (Global.IsAllgohome)
                {
                    btnAllGoHome.Enabled = false;
                }
                
                ShowLog("机台初始化" + (Global.IsAllgohome ? "成功" : "失败"));
            }
            catch (Exception ex)
            {

            }
        }

        private void Pause_Action(bool isWarn = false)
        {
            if (Global.Run)
            {
                BeginInvoke(new Action(() =>
                {
                    lblRunStatus.BackColor = Color.Yellow;
                    lblRunStatus.Text = "设备暂停"; 
                    lblRuningStatus.BackColor = Color.Yellow;
                    lblRuningStatus.Text = "设备暂停";
                    btnPause.Enabled = false;
                    btnReStore.Enabled = true;
                    chkHandProtect.Checked = true;
                    m_ProcessControl.SetThreeLight(EThreeLight.暂停);
                    m_ProcessControl.PauseTiming(isWarn);
                }));
            }

            Global.SetSystemStauts(Global.EnumSystemRunStatus.Pause);
        }

        private void Stop_Action()
        {
            Global.Break = true;
            m_ProcessControl.StopAllAxis();

            if(Global.Run)
            {
                Global.SetSystemStauts(Global.EnumSystemRunStatus.Stop);

                BeginInvoke(new Action(() =>
                {
                    lblRunStatus.BackColor = Color.Pink;
                    lblRunStatus.Text = "运行停止";
                    lblRuningStatus.BackColor = Color.Pink;
                    lblRuningStatus.Text = "运行停止";

                    btnStart.Enabled = true;
                    btnPause.Enabled = false;
                    btnStop.Enabled = false;
                    btnReStore.Enabled = false;
                    btnAllGoHome.Enabled = true;

                    chkHandProtect.Checked = true;
                }));

                m_ProcessControl.SetThreeLight(EThreeLight.暂停);
                ResetAlarm();
                m_ProcessControl.StopTiming();
                Global.IsAllgohome = false;
                m_ProcessControl.ThreadAbort();
                ShowLog("点击停止按钮,停止运行..");
            }
        }

        private void ReStore_Action()
        {
            if(Global.IsHandProtect)
            {
                ShowLog("请解锁手动保护!");
                return;
            }

            if (Global.Run)
            {
                SetRun();
            }

            if (Global.Pause)
            {
                m_ProcessControl.SetThreeLight(EThreeLight.运行);
                ResetAlarm();

                Global.Pause = false;
                btnReStore.Enabled = false;
                btnPause.Enabled = true;
                m_ProcessControl.RestoreTiming();

                ShowLog("点击恢复按钮,恢复运行...");

                //恢复运行写入报警日志
                string path = Global.DataPath + "Alarm//";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                m_ProcessControl.SaveCSV(path + DateTime.Now.ToString("yyyyMMdd") + ".csv", "时间,报警ID,报警", string.Format("{0},{1},{2}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"), 0, "恢复运行"));
            }
        }

        #endregion

        #region 用户管理 注册
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                UserLogin view = new UserLogin();
                view.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnUserManage_Click(object sender, EventArgs e)
        {
            try
            {
                SuperUserManage view = new SuperUserManage();
                view.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                EncyptForm view = new EncyptForm();
                view.ShowDialog();

                //ResigterForm regist = new ResigterForm();
                //regist.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnRegister_Click", ex.Message);
            }
        } 
        
        #endregion

        #region Log 计算器 键盘 通讯 全局变量 3D相机配置 切换配置界面 操作员ID 相机 算法配置
        private void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                TestLogView view = new TestLogView(this.Right, this.Bottom);
                view.Show();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                CalcView view = new CalcView();
                view.Show();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Environment.SystemDirectory + "\\osk.exe"))
                {
                    MessageBoxEx.Show(this, "软键盘可执行文件不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Process.Start("C:\\Windows\\System32\\osk.exe");
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCommuniction_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new Communication_View(), "通讯配置");
                view.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnGlobal_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new GlobalVariableView(), "全局变量");
                view.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new LaserParamView(), "镭射配置");
                view.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        ShowControl m_VisionView;
        private void btnCamera2D_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_VisionView == null || m_VisionView.IsDisposed)
                {
                    m_VisionView = new ShowControl(new VisionView(), "相机配置");
                }
                m_VisionView.Show();
                m_VisionView.BringToFront();
                if (m_VisionView.WindowState == FormWindowState.Minimized)
                {
                    m_VisionView.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            {

            }
        }
         
        private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTabIndex == 2)
                {
                    m_paramView.UpdateData();
                }
                else if (tabControl1.SelectedTabIndex == 3)
                {
                    m_unLoadView.UpdateData();
                }

                bar1.Text = tabControl1.SelectedTab.Text;
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }
         
        #endregion

        #region Status显示 显示具体信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                lblCurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                double value = GetProcessUsedMemory();

                lblMemory.Text = "内存:" + value.ToString("0.0") + "M";

                int day = m_timer.Elapsed.Days;
                double hour = m_timer.Elapsed.Hours;
                double minute = m_timer.Elapsed.Minutes;
                double second = m_timer.Elapsed.Seconds;
                lblRunTime.Text = string.Format("运行时间:{0}天{1}时{2}分{3}秒", day, hour, minute, second);

                m_ProcessControl.SetTestData();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("timer1_Tick", ex.Message);
            }
        }

        public double GetProcessUsedMemory()
        {
            double usedMemory = 0;
            usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;

            if (usedMemory > 60)
            {
                usedMemory -= 60;
            }

            return usedMemory;
        }

        public void GetUsedCPU()
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    var pro3 = Process.GetCurrentProcess();
                    string strName = pro3.ProcessName;
                    using (var pro = Process.GetProcessesByName(strName)[0])
                    {
                        //间隔时间（毫秒） 
                        int interval = 1000;
                        //上次记录的CPU时间 
                        var prevCpuTime = TimeSpan.Zero;

                        while (m_bGetCpu)
                        {
                            //当前时间
                            var curTime = pro.TotalProcessorTime;
                            //间隔时间内的CPU运行时间除以逻辑CPU数量
                            var value = (curTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;

                            prevCpuTime = curTime;
                            //输出
                            //Console.WriteLine(value); 

                            BeginInvoke(new Action(() =>
                            {
                                lblCPU.Text = "CPU:" + value.ToString("0.0") + "%";
                            }));

                            Thread.Sleep(interval);
                        }
                    }
                }));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("GetUsedCPU", ex.Message);
            }

        }
        
        #endregion

        #region Job操作 New Open Save Close SaveAs Exit SaveConfig
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory + "Sequence";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string foldPath = dialog.SelectedPath;
                    //Global.SaveNodeValue("SequencePath", foldPath + "NewJob.xml");
                    Global.SequencePath = foldPath + "\\NewJob.dsr";
                    //lblStatus.Text = "当前工作空间:" + Global.SequencePath;

                    SequenceModel sequence = new SequenceModel();
                    
                    sequence.ChildSequenceModels.Add(new ChildSequenceModel()
                    {
                        IsOnceProcess = true, 
                    });
                    

                    if (File.Exists(Global.SequencePath))
                    {
                        int count = Directory.GetFiles(foldPath).Count();
                        Global.SequencePath = string.Format("{0}\\NewJob{1}.dsr", foldPath, count + 1);
                    }

                    //检查电脑上是否有D盘
                    var driveD = DriveInfo.GetDrives().ToList().Any(x => x.Name.Contains("D:"));
                    if (driveD)
                    {
                        sequence.BasePath = @"D:\Documents\Dr3DVision\Model\" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss");
                    }
                    else
                    {
                        sequence.BasePath = @"C:\Documents\Dr3DVision\Model\" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss");
                    }

                    Global.Model3DPath = sequence.BasePath;
                    if (!Directory.Exists(Global.Model3DPath))
                    {
                        Directory.CreateDirectory(Global.Model3DPath);
                    }

                    XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                    XmlControl.sequenceModelNew = sequence;

                    Global.SaveNodeValue("SequencePath", Global.SequencePath);

                    SetSequenceName();

                    InitProcess();
                    //m_checkItemTool.UpdateNew();

                    InitData();

                    SaveRecentItem(Global.SequencePath);
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnNew_Click", ex.Message);
            }
        }

        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog(); 
        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBoxEx.Show(this, "即将切换项目，请确认是否保存配置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                XmlControl.SetObject();
                XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));
                DeleteFiles();
            }
            else if (dr == DialogResult.Cancel)
            {
                return;
            }
            m_InitDevice = false;

            //切换产品需要先停止当前流程
            if (Global.Run)
            {
                Stop_Action();
            }

            Stream inputStream = null;
            this.OpenFileDialogImage.Filter = "Config files (*.*)|*.dsr";
            OpenFileDialogImage.InitialDirectory = Global.CurrentPath;
            if (this.OpenFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((inputStream = this.OpenFileDialogImage.OpenFile()) != null)
                    {
                        String strImageFile = this.OpenFileDialogImage.FileName;
                        Global.SaveNodeValue("SequencePath", strImageFile);
                        Global.SequencePath = strImageFile;

                        InitData();
                        SetSequenceName();
                        //m_checkItemTool.UpdateData();
                        InitProcess();

                        SaveRecentItem(strImageFile);
                    }
                }
                catch (Exception ex)
                {
                    CommFuncView.ShowErrMsg("btnOpen_Click", ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;

                XmlControl.SetObject();
                XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel)); 

                string path = Global.CurrentPath + "//Sequence//Card//Card.dsr";
                XmlControl.SaveToXml(path, XmlControl.controlCardModel, typeof(ControlCardModel));
                DeleteFiles();

                ShowLog("项目保存成功", LogLevel.Debug);
                MessageBoxEx.Show(this, "项目保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, true);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnSave_Click", ex.Message);
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            btnOpen_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SaveFileDialogImage = new SaveFileDialog();
                SaveFileDialogImage.Filter = "DSR File(*.dsr)|*.dsr";
                SaveFileDialogImage.InitialDirectory = Global.SequencePath.Substring(0, Global.SequencePath.LastIndexOf("\\"));
                if (SaveFileDialogImage.ShowDialog() == DialogResult.OK)
                {
                    File.Move(Global.SequencePath, SaveFileDialogImage.FileName);

                    //把设计界面也重新命名
                    string path = Global.CurrentPath + "//Sequence//Design//";
                    string strDesign = path + Path.GetFileNameWithoutExtension(Global.SequencePath) + "_Design.dsr";
                    string newDesign = path + Path.GetFileNameWithoutExtension(SaveFileDialogImage.FileName) + "_Design.dsr";
                    if (File.Exists(strDesign))
                    {
                        File.Move(strDesign, newDesign);
                    }

                    string strdbgDesign = path + Path.GetFileNameWithoutExtension(Global.SequencePath) + "_Debug_Design.dsr";
                    string newdbgDesign = path + Path.GetFileNameWithoutExtension(SaveFileDialogImage.FileName) + "_Debug_Design.dsr";
                    if (File.Exists(strdbgDesign))
                    {
                        File.Move(strdbgDesign, newdbgDesign);
                    }

                    Global.SequencePath = SaveFileDialogImage.FileName;

                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));

                    Global.SaveNodeValue("SequencePath", Global.SequencePath);

                    SetSequenceName();

                    SaveRecentItem(Global.SequencePath);

                    ReNameConfig();
                }
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
                CommFuncView.ShowErrMsg("btnSaveAs_Click", ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        
        #region 通用方法
        
        /// <summary>
        /// 界面显示Log
        /// </summary>
        /// <param name="log">Log信息</param>
        /// <param name="loglevel">Log等级</param>
        private void ShowLog(string log, LogLevel loglevel = LogLevel.Info)
        {
            try
            {
                logView1.LogMessage(log, loglevel);
            }
            catch (Exception ex)
            {

            }
        }

        private void DelShowLog(string log, bool bError = false)
        {
            try
            {
                logView1.LogMessage(log, bError ? LogLevel.Exception : LogLevel.Debug);
            }
            catch (Exception ex)
            {

            }
        }

        private void DelShowObject(int inum, object dispObj)
        {
            try
            {
                HSmartWindow hsmartWindow = GetSmartWindow(inum);
                hsmartWindow.GetWindowHandle().SetColor("red");
                hsmartWindow.DispObj((HObject)dispObj);
            }
            catch (Exception)
            {
                 
            }
        }
         
        /// <summary>
        /// 界面显示异常Log
        /// </summary>
        /// <param name="log">Log信息</param>
        /// <param name="loglevel">Log等级</param>
        private void ShowExLog(Exception exception)
        {
            try
            {
                string log = exception.Message;

                string strStack = exception.StackTrace;
                int index = strStack.LastIndexOf('\n');
                if (index != -1)
                {
                    log += strStack.Substring(index + 3);
                }
                else
                {
                    log += strStack;
                }

                logView1.LogMessage(log, LogLevel.Exception);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        private void DeleteFiles()
        {
            try
            {
                foreach (var item in Global.DeleteFiles)
                {
                    File.Delete(item);
                }
                 
                if(Global.DeleteFiles != null && Global.DeleteFiles.Count > 0)
                {
                    Global.DeleteFiles.Clear();
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        #endregion

        #region 页面布局
        HSmartWindow[] m_hSmartWindowArr;
        int m_LayOutNum = 1;
        private HSmartWindow GetSmartWindow(int iNum)
        {
            if (m_LayOutNum == 1)
            {
                return m_hSmartWindow;
            }
            else if (iNum > m_LayOutNum - 1)
            {
                return m_hSmartWindowArr[0];
            }
            else
            {
                return m_hSmartWindowArr[iNum];
            }
        }

        private void toolStrip_1_Click(object sender, EventArgs e)
        {
            if (Global.UserName == Global.OperatorName)
            {
                MessageBoxEx.Show(this, "操作员无权限!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var toolStrip = sender as ButtonItem;
            if (toolStrip.Name == "toolStrip_1")
            {
                m_LayOutNum = 1;
            }
            else if (toolStrip.Name == "toolStrip_2")
            {
                m_LayOutNum = 2;
            }
            else if (toolStrip.Name == "toolStrip_3")
            {
                m_LayOutNum = 3;
            }
            else if (toolStrip.Name == "toolStrip_4")
            {
                m_LayOutNum = 4;
            }
            else if (toolStrip.Name == "toolStrip_5")
            {
                m_LayOutNum = 5;
            }
            else if (toolStrip.Name == "toolStrip_6")
            {
                m_LayOutNum = 6;
            }
            else if (toolStrip.Name == "toolStrip_9")
            {
                m_LayOutNum = 9;
            }
            else if (toolStrip.Name == "toolStrip_12")
            {
                m_LayOutNum = 12;
            }
            else if (toolStrip.Name == "toolStrip_16")
            {
                m_LayOutNum = 16;
            }

            SequenceModel sequenceModel = XmlControl.sequenceModelNew;
            sequenceModel.LayOutNum = m_LayOutNum;

            InitLayOutView();
        }

        public void LayOutHalWindow(Control control, UserControl userControl)
        {
            if (!control.Controls.Contains(userControl))
            {
                CommHelper.LayoutChildFillView(control, userControl);
            }
        }

        private void InitLayOutView()
        {
            if (m_LayOutNum == 1)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(sPanelCamera);
                sPanelCamera.Dock = DockStyle.Fill;

                if (m_hSmartWindow == null || m_hSmartWindow.IsDisposed)
                {
                    m_hSmartWindow = new HSmartWindow();
                }
                LayOutHalWindow(sPanelCamera, m_hSmartWindow); 
            }
            else if (m_LayOutNum == 2)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(tableLayoutPanel2);
                tableLayoutPanel2.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_21V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_22V, m_hSmartWindowArr[1]);
            }
            else if (m_LayOutNum == 3)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(tableLayoutPanel5);
                tableLayoutPanel5.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_31V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_32V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_33V, m_hSmartWindowArr[2]);
            }
            else if (m_LayOutNum == 4)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_4);
                panel_4.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_41V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_42V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_43V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_44V, m_hSmartWindowArr[3]);
            }
            else if (m_LayOutNum == 5)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_5);
                panel_5.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_51V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_52V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_53V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_54V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_55V, m_hSmartWindowArr[4]);
            }
            else if (m_LayOutNum == 6)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_6);
                panel_6.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_61V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_62V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_63V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_64V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_65V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_66V, m_hSmartWindowArr[5]);
            }
            else if (m_LayOutNum == 9)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_9);
                panel_9.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }

                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }

                LayOutHalWindow(panel_91V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_92V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_93V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_94V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_95V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_96V, m_hSmartWindowArr[5]);
                LayOutHalWindow(panel_97V, m_hSmartWindowArr[6]);
                LayOutHalWindow(panel_98V, m_hSmartWindowArr[7]);
                LayOutHalWindow(panel_99V, m_hSmartWindowArr[8]);
            }
            else if (m_LayOutNum == 12)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_12);
                panel_12.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }
                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                } 
                LayOutHalWindow(panel_121V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_122V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_123V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_124V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_125V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_126V, m_hSmartWindowArr[5]);
                LayOutHalWindow(panel_127V, m_hSmartWindowArr[6]);
                LayOutHalWindow(panel_128V, m_hSmartWindowArr[7]);
                LayOutHalWindow(panel_129V, m_hSmartWindowArr[8]);
                LayOutHalWindow(panel_1210V, m_hSmartWindowArr[9]);
                LayOutHalWindow(panel_1211V, m_hSmartWindowArr[10]);
                LayOutHalWindow(panel_1212V, m_hSmartWindowArr[11]);
            }
            else if (m_LayOutNum == 16)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_16);
                panel_16.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_LayOutNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_LayOutNum];
                }
                for (int i = 0; i < m_LayOutNum; i++)
                {
                    if (m_hSmartWindowArr[i] == null || m_hSmartWindowArr[i].IsDisposed)
                    {
                        m_hSmartWindowArr[i] = new HSmartWindow();
                    }
                }
                LayOutHalWindow(panel_161V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_162V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_163V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_164V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_165V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_166V, m_hSmartWindowArr[5]);
                LayOutHalWindow(panel_167V, m_hSmartWindowArr[6]);
                LayOutHalWindow(panel_168V, m_hSmartWindowArr[7]);
                LayOutHalWindow(panel_169V, m_hSmartWindowArr[8]);
                LayOutHalWindow(panel_1610V, m_hSmartWindowArr[9]);
                LayOutHalWindow(panel_1611V, m_hSmartWindowArr[10]);
                LayOutHalWindow(panel_1612V, m_hSmartWindowArr[11]);
                LayOutHalWindow(panel_1613V, m_hSmartWindowArr[12]);
                LayOutHalWindow(panel_1614V, m_hSmartWindowArr[13]);
                LayOutHalWindow(panel_1615V, m_hSmartWindowArr[14]);
                LayOutHalWindow(panel_1616V, m_hSmartWindowArr[15]);
            }
        }

        private void btnLayOut_Click(object sender, EventArgs e)
        {
            try
            {
                List<ButtonItem> listToolStrip = new List<ButtonItem>()
                    { toolStrip_1 , toolStrip_2, toolStrip_3, toolStrip_4, toolStrip_5, toolStrip_6, toolStrip_9, toolStrip_12, toolStrip_16};

                foreach (var item in listToolStrip)
                {
                    string str = item.Name.Substring(10);
                    int index = Int32.Parse(str);
                    if (index == m_LayOutNum)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                }

            }
            catch (Exception ex)
            {

            }            
        }

        private void btnLayOut_ExpandChange(object sender, EventArgs e)
        {
            try
            {
                List<ButtonItem> listToolStrip = new List<ButtonItem>()
                    { toolStrip_1 , toolStrip_2, toolStrip_3, toolStrip_4, toolStrip_5, toolStrip_6, toolStrip_9, toolStrip_12, toolStrip_16};

                foreach (var item in listToolStrip)
                {
                    string str = item.Name.Substring(10);
                    int index = Int32.Parse(str);
                    if (index == m_LayOutNum)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 最近打开项目操作 系统设置
        /// <summary>
        /// 添加最近打开项目到界面
        /// </summary>
        public void AddRecentItem()
        {
            try
            {
                List<ButtonItem> listButton = new List<ButtonItem>()
                {
                    btnRecentDoc1, btnRecentDoc2, btnRecentDoc3, btnRecentDoc4, btnRecentDoc5, btnRecentDoc6, btnRecentDoc7, btnRecentDoc8, btnRecentDoc9
                };
                string strValue = Global.GetStrNodeValue("RecentItems");

                string[] strvalues = strValue.Split('|');

                int i = 0;
                foreach (var item in strvalues)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    string str = item.Substring(item.LastIndexOf("\\") + 1);
                    if(i < 9)
                    {
                        listButton[i].Name = item;
                        if (File.Exists(item))
                        {
                            listButton[i++].Text = str;
                        }
                    }
                }

                for (int j = 9; j > i; j--)
                {
                    listButton[j - 1].Text = "";
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("AddRecentItem", ex.Message);
            }
        }
         
        /// <summary>
        /// 保存最近打开项目到文件中
        /// </summary>
        /// <param name="strItem">保存Item的名称</param>
        public void SaveRecentItem(string strItem)
        {
            try
            {
                string strValue = Global.GetStrNodeValue("RecentItems");
                string[] strvalues = strValue.Split('|');
                strvalues = strvalues.Where(x => x != "").ToArray();
                if (strvalues.Count() == 0)
                {
                    strValue += strItem;
                    strValue += "|";
                }
                else
                {
                    if (strvalues.Contains(strItem))
                    {
                        strValue = strValue.Replace(strItem + "|", "");
                    }

                    strValue = strValue.Insert(0, strItem + "|");
                }

                //设定最多显示9个
                int count = strvalues.Count();
                if (count > 9)
                {
                    strValue = strValue.Replace(strvalues[count - 1] + "|", "");
                }

                Global.SaveNodeValue("RecentItems", strValue);

                AddRecentItem();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("SaveRecentItem", ex.Message);
            }
        }

        private void btnRecentDoc1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBoxEx.Show(this, "即将切换项目，请确认是否保存配置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));
                    DeleteFiles();
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }

                //切换产品需要先停止当前流程
                if(Global.Run)
                {
                    Stop_Action();
                }

                ButtonItem btn = sender as ButtonItem;
                string str = btn.Name;

                m_InitDevice = false;
                if (string.IsNullOrEmpty(str)|| str.Contains("btnRecentDoc") || !File.Exists(str))
                {
                    return;
                }

                Global.SaveNodeValue("SequencePath", str);
                Global.SequencePath = str;

                InitData();
                SetSequenceName();
                //m_checkItemTool.UpdateData();
                InitProcess();

                SaveRecentItem(str);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnRecentDoc1_Click", ex.Message);
            }
        }
         
        private void btnOption_Click(object sender, EventArgs e)
        {
            try
            {
                SetView view = new SetView();
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnOption_Click", ex.Message);
            } 
        }

        private void SetViewName()
        {
            try
            { 
                var version = Assembly.GetExecutingAssembly().GetName().Version; 
                string sDateTime = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();

                string str = Global.GetNodeValue("ViewName");
                this.Text = str + " Ver:" + sDateTime + "_" + version.ToString();
                notifyIcon1.Text = str;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("", ex.Message);
            }
        }
        
        private void btnEmbeddedApp_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new EmbedAppView(), "外部程序");
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnEmbeddedApp_Click", ex.Message);
            }
        }
        #endregion

        #region 方法委托 
        
        public void LuxShowReslut(List<ShowResultValue> listValue)
        {
            try
            {
                if (listValue != null && listValue.Count > 0)
                {
                    if(m_outPutView != null && !m_outPutView.IsDisposed)
                    {
                        m_outPutView.SetOutPut(listValue);
                    }
                    else
                    {
                        outPutView1.SetOutPut(listValue);
                    }
                } 

            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }

        private bool MessageBoxQuestion(string msg)
        {
            try
            {
                MsgBoxView view = new MsgBoxView(msg, EnumBox.询问);
                var result = view.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 弹框显示信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="index">1：信息 2：询问 3：警告</param>
        /// <returns></returns>
        private bool MessageBoxInfo(string msg, int index)
        {
            try
            {
                EnumBox enumBox = EnumBox.提示;
                switch (index)
                {
                    case 1:
                        enumBox = EnumBox.提示;
                        break;
                    case 2:
                        enumBox = EnumBox.询问;
                        break;
                    case 3:
                        enumBox = EnumBox.报警;
                        break;
                }
                MsgBoxView view = new MsgBoxView(msg, enumBox);

                if (view.ShowDialog() == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if(!ex.Message.Contains("正在中止线程"))
                { 
                    ShowExLog(ex);
                }
                return false;
            }
        }

        #endregion

        #region 运行编辑界面委托 
        bool m_bEdit = false;//是否是点击界面编辑按钮
        bool m_bShowBox = true;//主界面关闭是否弹出确认窗口
        /// <summary>
        /// 运行状态按钮执行委托
        /// </summary>
        /// <param name="enumButton">执行按钮枚举类型</param>
        public void Del_EButtonRun(Global.EnumEButtonRun enumButton)
        {
            try
            {
                switch (enumButton)
                {
                    case Global.EnumEButtonRun.运行:
                        btnStart_Click(null, null);
                        break;
                    case Global.EnumEButtonRun.暂停:
                        Pause_Action();
                        break;
                    case Global.EnumEButtonRun.停止:
                        if (m_IsStopShowMsg)
                        {
                            Pause_Action();
                            DialogResult dr = MessageBoxEx.Show(this, "请确认程序是否需要停止运行?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.No)
                            {
                                ReStore_Action();
                                return;
                            }
                        }
                        Stop_Action();
                        break;
                    case Global.EnumEButtonRun.恢复:
                        ReStore_Action();
                        break;
                    case Global.EnumEButtonRun.编辑:
                        Del_ToEditView();
                        break;
                    case Global.EnumEButtonRun.产品配置:
                        ShowControl view = new ShowControl(new ProductSelView(), "产品配置");
                        view.ShowDialog();
                        break; 
                    case Global.EnumEButtonRun.SAP信息:
                        SAPQueryView Sap = new SAPQueryView();
                        Sap.ShowDialog();
                        break;
                    case Global.EnumEButtonRun.相机实时:
                        m_ProcessControl.BtnRealDisplay(); 
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("Del_EButtonRun", ex.Message);
            }
        }

        /// <summary>
        /// 执行流程按钮执行委托
        /// </summary>
        /// <param name="enumProcess">执行流程枚举</param>
        public void Del_EButtonProRun(Global.EnumEProcess enumProcess)
        {
            try
            {
                Task.Run(new Action(() =>
                {
                    if (Global.Run)
                    {
                        MessageBoxEx.Show("设备正在运行中，不可再次运行！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ChildSequenceModel sequence = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == enumProcess.ToString());
                    if (sequence == null)
                    {
                        return;
                    }

                    //M_CheckItemTool_RunStatusEvent(null, "单次运行中");
                    ShowLog(sequence.Name + " 调试运行");

                    Global.Break = false;
                    Global.SetSystemStauts(Global.EnumSystemRunStatus.Run);
                    
                    ShowLog(sequence.Name + " 运行完成");
                    //m_checkItemTool.RunCompleted();
                }));
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("Del_EButtonProRun", ex.Message);
            }
        }

        /// <summary>
        /// 获取图形窗口委托
        /// </summary>
        /// <param name="iLayOut">图形窗口Index</param>
        /// <returns></returns>
        public HSmartWindow Del_EHSmartWindow(int iLayOut)
        {
            return GetSmartWindow(iLayOut);
        }

        /// <summary>
        /// 执行日志委托
        /// </summary>
        /// <param name="logView">日志控件</param>
        public LogView Del_ELog()
        {
            return logView1;
        }

        /// <summary>
        /// 执行窗口关闭委托
        /// </summary>
        public void Del_RunViewClose()
        {
            try
            {
                if (!m_bEdit)
                {
                    m_bShowBox = false;
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 转到编辑界面委托
        /// </summary>
        public void Del_ToEditView()
        {
            try
            {
                if (Global.UserName == Global.OperatorName)
                {
                    MessageBoxEx.Show(this, "操作员无权限，请切换到管理员!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                InitLayOutView();

                //Log界面设置成跟主界面同一个
                if (logView1 == null || logView1.IsDisposed)
                {
                    logView1 = new LogView();
                }
                CommHelper.LayoutChildFillView(panelLog, logView1);

                m_bEdit = true;
                m_RunView.m_bShowBox = false;
                m_RunView.Close();
                this.Show();
            }
            catch (Exception ex)
            {

            }
        }

        //结果显示OK或者NG
        private void lblResult2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //ELblResult.sResult = lblResult2.Text;
            }
            catch (Exception ex)
            {

            }
        }

        OutPutView m_outPutView;
        /// <summary>
        /// 显示输出数据委托
        /// </summary>
        /// <param name="outPutView">输出数据窗口</param>
        public void Del_EOutPut(OutPutView outPutView)
        {
            m_outPutView = outPutView;
        }
         
        #endregion

        #region Stauts状态更新到编辑界面
        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(0, lblStatus.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblRunStatus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(1, lblRunStatus.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblLoginUser_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(2, lblLoginUser.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblRunTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(3, lblRunTime.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblMemory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(4, lblMemory.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblCPU_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(5, lblCPU.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblCurrentTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetText(6, lblCurrentTime.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private void lblRunStatus_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_RunView == null || m_RunView.IsDisposed)
                {
                    return;
                }

                m_RunView.SetColor(0, lblRunStatus.BackColor);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 添加设备状态显示

        bool m_InitDevice = false;
        private void panelDockDevice_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (panelDockDevice.Visible)
                {
                    if (!m_InitDevice)
                    {
                        int deviceCount = 0;
                        panelDevice.Controls.Clear();
                        var listTcpIp = XmlControl.sequenceModelNew.TCPIPModels;

                        int _posX = panelDevice.Location.X + 10;
                        int _posY = panelDevice.Location.Y + 10;

                        if (listTcpIp != null && listTcpIp.Count() > 0)
                        {
                            int posX = _posX;
                            int posY = _posY;
                            int length = listTcpIp.Count;
                            deviceCount += length;
                            DeviceStatus[] tcpDevices = new DeviceStatus[length];
                            int num = -1;
                            for (int i = 0; i < length; i++)
                            { 
                                //如果PLC有链接则不需要添加
                                if (XmlControl.sequenceModelNew.PLCSetModels.FindIndex(x => x.ConnObj == listTcpIp[i].Name) != -1)
                                {
                                    continue;
                                }
                                num++;

                                tcpDevices[i] = new DeviceStatus();

                                int pHeight = panelDevice.Height;

                                int height = tcpDevices[i].Height;
                                int width = tcpDevices[i].Width;

                                int count = pHeight / (height + 10);

                                int ix = num / count;

                                int iy = num;
                                if (num >= count)
                                {
                                    iy = num - count * ix;
                                }

                                tcpDevices[i].Location = new Point(posX + ix * (width + 10), posY + iy * (height + 10));
                                tcpDevices[i].DeviceName = listTcpIp[i].Name;
                                tcpDevices[i].EnumDevice = EnumDeviceType.TCP;
                                tcpDevices[i].ConnectStatus = false;

                                _posX = posX + ix * (width + 10);
                                _posY = posY + iy * (height + 10) + height + 10;

                                panelDevice.Controls.Add(tcpDevices[i]);
                            }
                        }

                        var listCom = XmlControl.sequenceModelNew.ComModels;
                        if (listCom != null && listCom.Count() > 0)
                        {
                            int posX = _posX;
                            int posY = _posY;

                            int length = listCom.Count;
                            deviceCount += length;
                            DeviceStatus[] comDevices = new DeviceStatus[length];
                            int num = -1;
                            for (int i = 0; i < length; i++)
                            { 
                                //如果PLC有链接则不需要添加
                                if (XmlControl.sequenceModelNew.PLCSetModels.FindIndex(x => x.ConnObj == listCom[i].Name) != -1)
                                {
                                    continue;
                                }
                                num++;

                                comDevices[i] = new DeviceStatus();

                                int pHeight = panelDevice.Height;

                                int height = comDevices[i].Height;
                                int width = comDevices[i].Width;

                                int count = pHeight / (height + 10);

                                int ix = num / count;

                                int iy = num;
                                if (num >= count)
                                {
                                    iy = num - count * ix;
                                }

                                comDevices[i].Location = new Point(posX + ix * (width + 10), posY + iy * (height + 10));
                                comDevices[i].DeviceName = listCom[i].Name;
                                comDevices[i].EnumDevice = EnumDeviceType.COM;
                                comDevices[i].ConnectStatus = true; 

                                _posX = posX + ix * (width + 10);
                                _posY = posY + iy * (height + 10) + height + 10;

                                panelDevice.Controls.Add(comDevices[i]);
                            }
                        }

                        var listPLC = XmlControl.sequenceModelNew.PLCSetModels;
                        if (listPLC != null && listPLC.Count() > 0)
                        {
                            int posX = _posX;
                            int posY = _posY;

                            int length = listPLC.Count;
                            deviceCount += length;
                            DeviceStatus[] plcDevices = new DeviceStatus[length]; 
                            for (int i = 0; i < length; i++)
                            {
                                plcDevices[i] = new DeviceStatus();

                                int pHeight = panelDevice.Height;

                                int height = plcDevices[i].Height;
                                int width = plcDevices[i].Width;

                                int count = pHeight / (height + 10);

                                int ix = i / count;

                                int iy = i;
                                if (i >= count)
                                {
                                    iy = i - count * ix;
                                }

                                plcDevices[i].Location = new Point(posX + ix * (width + 10), posY + iy * (height + 10));
                                plcDevices[i].DeviceName = listPLC[i].Name;
                                plcDevices[i].EnumDevice = EnumDeviceType.PLC;
                                plcDevices[i].ConnectStatus = true;

                                panelDevice.Controls.Add(plcDevices[i]);
                            }
                        }

                        m_InitDevice = true;
                    }
                }


                if (panelDockDevice.Visible)
                {
                    ShowDeviceStatus();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void ShowDeviceStatus()
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    while (panelDockDevice.Visible)
                    {
                        foreach (var item in panelDevice.Controls)
                        {
                            if (item is DeviceStatus)
                            {
                                DeviceStatus device = item as DeviceStatus;
                                switch (device.EnumDevice)
                                {
                                    case EnumDeviceType.TCP:
                                        TCPIPModel tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.Name == device.DeviceName);
                                        if(tcpipModel != null)
                                        {
                                            device.ConnectStatus = tcpipModel.IsConnected;
                                        }
                                        break;
                                    case EnumDeviceType.COM:
                                        ComModel comModel = XmlControl.sequenceModelNew.ComModels.FirstOrDefault(x => x.Name == device.DeviceName);
                                        if(comModel != null)
                                        {
                                            device.ConnectStatus = comModel.IsConnected;
                                        }
                                        break;
                                    case EnumDeviceType.FTP:
                                        break;
                                    case EnumDeviceType.PLC:
                                        PLCSetModel plcModel = XmlControl.sequenceModelNew.PLCSetModels.FirstOrDefault(x => x.Name == device.DeviceName);
                                        if(plcModel != null)
                                        {
                                            device.ConnectStatus = plcModel.IsConnected;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        Thread.Sleep(200);
                    }
                }));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                ShowExLog(ex);
            }
        }
        #endregion

        #region 帮助界面
        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                //m_ProcessControl.WriteProductData();
                double d = 92.4 + 0.5;
                int i = (int)d;

                // PanelToBmp.OutTheControllerToPic(panel1);
                return;
                //bool bresult = m_ProcessControl.HttpAction();
                //return;
                //About3DView view = new About3DView();
                //view.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 滚动显示报警
        private int positon_falg = 5;
        System.Windows.Forms.Timer timeScroll;
        /// <summary>
        /// 滚动报警设置
        /// </summary>
        private void ScrollAlarmSet()
        {
            timeScroll = new System.Windows.Forms.Timer();
            timeScroll.Interval = 200;
            timeScroll.Tick += new EventHandler(timeScroll_Tick);
            timeScroll.Start();
        }

        /// <summary>
        /// 计时器滚动显示报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timeScroll_Tick(object sender, EventArgs e)
        {
            try
            {
                //    if (this.lblAlarm.Location.X > 0 - this.lblAlarm.Width && this.lblAlarm.Location.X <= this.panelAlarmView.Width)
                //    {
                //        this.lblAlarm.Location = new Point(this.lblAlarm.Location.X - positon_falg, this.lblAlarm.Location.Y);
                //    }
                //    else
                //    {
                //        this.lblAlarm.Location = new Point(this.panelAlarmView.Size.Width, this.lblAlarm.Location.Y);
                //    }
                int width = ribbonBar11.Width + ribbonBar4.Width + ribbonBar15.Width + ribbonBar2.Width + ribbonBar1.Width + 20;

                if (this.lblAlarm.Location.X > width - this.lblAlarm.Width && this.lblAlarm.Location.X <= this.Width)
                {
                    this.lblAlarm.Location = new Point(this.lblAlarm.Location.X - positon_falg, this.lblAlarm.Location.Y);
                }
                else
                {
                    this.lblAlarm.Location = new Point(this.Width, this.lblAlarm.Location.Y);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 设置 屏蔽按钮操作

        /// <summary>
        /// 手动保护
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void chkHandProtect_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            Global.IsHandProtect = chkHandProtect.Checked;
        }
        
        private void chkIsShieldBuzzer_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            try
            {
                XmlControl.sequenceModelNew.LDModel.IsShieldBuzzer = chkIsShieldBuzzer.Checked; 
            }
            catch (Exception ex)
            { 
                 
            }
        } 

        private void chkIsShieldBuzzer_Click(object sender, EventArgs e)
        {
            try
            {
                m_ProcessControl.SetBeepOff();
            }
            catch (Exception ex)
            {

            }
        }

        private void chkIsIngoreDoor_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            try
            {
                XmlControl.sequenceModelNew.LDModel.IsShieldDoor = chkIsIngoreDoor.Checked;
            }
            catch (Exception ex)
            {

            }
        }

        private void chkIsShieldMes_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {

            try
            {
                XmlControl.sequenceModelNew.LDModel.IsShieldMes = chkIsShieldMes.Checked;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Global.Run)
                {
                    if (Global.IsHandProtect)
                    {
                        ShowLog("请解锁手动保护!");
                        return;
                    }

                    if (!Global.IsAllgohome)
                    {
                        btnAllGoHome.Enabled = true;
                        MessageBoxEx.Show(this, "请先初始化机台!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, true);
                        return;
                    }

                    if (!MessageBoxInfo("执行清料，请小心操作，点击确认，开始！", 2))
                    {
                        MessageBoxEx.Show(this, "用户取消!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, true);
                        return;
                    }

                    if (!m_ProcessControl.Init(XmlControl.sequenceModelNew))
                    {
                        MessageBoxEx.Show(this, "初始化失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ShowLog("初始化成功");

                    Global.Break = false;
                    Global.SetSystemStauts(Global.EnumSystemRunStatus.Run);

                    SetRun();
                    ResetAlarm();

                    BeginInvoke(new Action(() =>
                    {
                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                        btnPause.Enabled = true;
                        btnAllGoHome.Enabled = false;
                    }));


                    ShowLog("点击清料按钮,开始清料..");
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

    }
}
//3768