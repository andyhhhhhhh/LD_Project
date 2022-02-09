using BaseController;
using GlobalCore;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaseModels;
using Infrastructure.DBCore;
using Infrastructure.Log;
using MotionController;
using CameraContorller;
using XMLController;
using System.IO;
using HalconDotNet;
using SerialPortController;
using SocketController;
using AlgorithmController;
using System.ComponentModel;
using System.Diagnostics;
using JsonController;
using VisionController;
using ServiceController;

namespace ProcessController
{
    /// <summary>
    /// 主流程类
    /// </summary>
    public class ProcessControl : IBaseControl
    {
        #region 主界面委托、事件
        public delegate void Del_OutPutLog(string log, LogLevel loglevel = LogLevel.Info);
        /// <summary>
        /// 委托 -- 主界面输出Log信息
        /// </summary>
        public static Del_OutPutLog m_DelOutPutLog;

        public delegate void Del_OutExLog(Exception ex);
        /// <summary>
        /// 委托 -- 主界面输出Exception信息
        /// </summary>
        public static Del_OutExLog m_DelOutExLog;

        public delegate void Del_PcAlaram(BaseResultModel resultModel, int alarmID, string strAlarm = "");
        /// <summary>
        /// 委托 -- 主界面输出报警信息
        /// </summary>
        public Del_PcAlaram m_DelPcAlarm;

        public delegate void Del_ShowStep(int id, string modelName, int stepIndex, string strStep);
        /// <summary>
        /// 委托 -- 主界面显示当前执行步骤
        /// </summary>
        public Del_ShowStep m_DelShowStep;

        public delegate void Del_GetTestData(int loadcount, int unloadcount, int okcount, int ngcount, int scanngcount, int seemngcount, double costtime, double pausetime, double warntime);
        /// <summary>
        /// 委托 -- 获取测试生产数据
        /// </summary>
        public Del_GetTestData m_DelGetTestData;

        public event EventHandler<CameraResultModel> DispPicEvent;
        /// <summary>
        /// 委托 -- 主界面显示图片
        /// </summary>
        /// <param name="e">相机结果</param>
        protected void OnDispPicEvent(CameraResultModel e)
        {
            EventHandler<CameraResultModel> handler = DispPicEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 显示弹框委托
        /// </summary>
        /// <param name="msg">显示信息</param>
        /// <param name="index">信息类型， 1：信息 2：询问 3：警告</param>
        public delegate bool Del_MessageBox(string msg, int index);
        /// <summary>
        /// 显示弹框委托  1：信息 2：询问 3：警告
        /// </summary>
        public Del_MessageBox m_DelMessageBox;
           
        public delegate string Del_OcrConfirmView(string msg);
        /// <summary>
        /// 显示OCR弹框委托
        /// </summary>
        public Del_OcrConfirmView m_DelOcrView;

        public delegate void Del_RefreshLoadParam();
        /// <summary>
        /// 刷新上料盘位置
        /// </summary>
        public Del_RefreshLoadParam m_DelRefreshLoadParam;

        public delegate void Del_RefreshUnLoadParam();
        /// <summary>
        /// 刷新下料盘位置
        /// </summary>
        public Del_RefreshUnLoadParam m_DelRefreshUnLoadParam;
         
        public delegate void Del_Start();
        /// <summary>
        /// 开始按钮执行
        /// </summary>
        public Del_Start m_DelStart;

        public delegate void Del_Stop();
        /// <summary>
        /// 暂停按钮执行
        /// </summary>
        public Del_Stop m_DelStop;

        public delegate void Del_Emergency(bool bEmergency);
        /// <summary>
        /// 急停按钮执行
        /// </summary>
        public Del_Emergency m_DelEmergency;
          
        public delegate void Del_SaveScreen(int index, string path);
        /// <summary>
        /// 保存结果截屏
        /// </summary>
        public Del_SaveScreen m_DelSaveScreen;

        #endregion

        #region 调试参数 
        /// <summary>
        /// 程序流程配置
        /// </summary>
        SequenceModel m_sequenceModel;
        /// <summary>
        /// 运动控制实例
        /// </summary>
        private IMotorControl m_MotroContorl;
        /// <summary>
        /// 相机实时采集互斥锁
        /// </summary>
        Mutex[] m_Mutexs = new Mutex[16];
        /// <summary>
        /// 相机采集实例
        /// </summary>
        ICameraControl[] m_CameraControl = new ICameraControl[16];
        /// <summary>
        /// 串口控制实例
        /// </summary>
        SerialControl[] m_serialControl;
        /// <summary>
        /// 网口控制实例
        /// </summary>
        SocketControl[] m_socketControl;
        /// <summary>
        /// 实时采集标志位
        /// </summary>
        bool m_RealDisplay = false;
        /// <summary>
        /// 通用参数配置类
        /// </summary>
        public Systemparameters m_parameter = new Systemparameters();

        public int m_stepCount = 4;

        /// <summary>
        /// 等待积分球1测试Event
        /// </summary>
        AutoResetEvent m_autoEvent1 = new AutoResetEvent(false);
          
        /// <summary>
        /// 上料测试线程实例
        /// </summary>
        private Thread m_LoadThread = null;

        /// <summary>
        /// 检测测试线程实例
        /// </summary>
        private Thread m_CheckThread = null;

        /// <summary>
        /// 小视野抓取线程实例
        /// </summary>
        private Thread m_SmallThread = null;
         
        /// <summary>
        /// 监控按钮按下线程实例
        /// </summary>
        private Thread m_DetectBtnThread = null;
        /// <summary>
        /// 监控急停按下线程实例
        /// </summary>
        public Thread m_EmergencyThread = null;

        /// <summary>
        /// 获取Tec温度线程
        /// </summary>
        public Thread m_GetTecThread = null;

        /// <summary>
        /// 当前流程步骤
        /// </summary>
        AutoRunStep Autostep;

        /// <summary>
        /// 上料流程步骤
        /// </summary>
        LoadRunStep LoadStep;
         
        /// <summary>
        /// 工位3检测流程步骤
        /// </summary>
        CheckRunStep CheckStep;

        /// <summary>
        /// 小视野流程步骤
        /// </summary>
        SmallRunStep SmallStep;

        /// <summary>
        /// 上料数量
        /// </summary>
        int m_LoadCount = 0;
        /// <summary>
        /// 下料数量
        /// </summary>
        int m_UnLoadCount = 0;
        /// <summary>
        /// 良品数量
        /// </summary>
        int m_OKCount = 0;
        /// <summary>
        /// 成品不良数量
        /// </summary>
        int m_NGCount = 0;
        /// <summary>
        /// 扫码不良数量
        /// </summary>
        int m_ScanNGCount = 0;
        /// <summary>
        /// 疑似不良数量
        /// </summary>
        int m_SeemNGCount = 0;
        /// <summary>
        /// 设备测试时间
        /// </summary>
        double m_CostTime = 0.0;
        /// <summary>
        /// 设备暂停时间
        /// </summary>
        double m_PauseTime = 0.0;
        /// <summary>
        /// 设备报警时间
        /// </summary>
        double m_WarnTime = 0.0;
        /// <summary>
        /// 测试计时
        /// </summary>
        Stopwatch m_stopWatch = new Stopwatch();
        /// <summary>
        /// 正常暂停计时
        /// </summary>
        Stopwatch m_pauseWatch = new Stopwatch();
        /// <summary>
        /// 报警计时
        /// </summary>
        Stopwatch m_warnWatch = new Stopwatch();

        /// <summary>
        /// 参数配置的Model
        /// </summary>
        LDModel m_CosModel = new LDModel();

        /// <summary>
        /// 当前放料料盘结果排列
        /// </summary>
        List<EnumCosResult> m_listCosResult = new List<EnumCosResult>();

        /// <summary>
        /// Tray盘参数的Model
        /// </summary>
        TrayModel m_TrayModel = new TrayModel();

        /// <summary>
        /// 下料料盘参数的Model
        /// </summary>
        UnLoadTrayModel m_unLoadTrayModel = new UnLoadTrayModel();

        /// <summary>
        /// 放料完成单个料盘种类Model
        /// </summary>
        UnLoadModel m_unLoadDoneModel = new UnLoadModel();
         
        /// <summary>
        /// 上料料盘是否取完
        /// </summary>
        bool m_GetProductDone = false;

        /// <summary>
        /// 下料料盘是否已放满
        /// </summary>
        bool m_UnLoadProductDone = false;
             
        /// <summary>
        /// 执行算法实例
        /// </summary>
        IAlgorithmControl m_algorithmControll = new AlgorithmControl();

        ParamRangeModel m_paramRangeModel = new ParamRangeModel();

        /// <summary>
        /// 取测试料失败次数
        /// </summary>
        int m_GetNGCount = 0;

        /// <summary>
        /// 上料准备OK--吸嘴可以吸料
        /// </summary>
        bool m_ReadyLoad = false;

        /// <summary>
        /// 下料模组移开，可以放料到检测DDR
        /// </summary>
        bool m_IsCanPutCheck = false;

        /// <summary>
        /// 上料模组移开，可以检测DDR物料
        /// </summary>
        bool m_ReadyDDRCheck = false;

        /// <summary>
        /// 小视野取完，可以放空料盘
        /// </summary>
        bool m_SmallGetDone = false;

        /// <summary>
        /// 小视野相机是否可以拍照
        /// </summary>
        bool m_IsSmallCanSnap = false;

        /// <summary>
        /// 大视野拍完，可以执行小视野流程
        /// </summary>
        bool m_ReadySmall = false;

        /// <summary>
        /// 产品行数
        /// </summary>
        int m_ProductRowCount = 0;

        /// <summary>
        /// 当前行需取料总数
        /// </summary>
        int m_NeedGetCount = 0;
        #endregion

        #region 自动流程控制
        /// <summary>
        /// 初始化动作
        /// </summary>
        /// <param name="parameter">传入的当前产品配置</param>
        /// <returns></returns>
        public bool Init(object parameter)
        {
            try
            {
                m_stopWatch.Reset();
                if (Global.IsEmergency)
                {
                    m_DelOutPutLog("设备急停，请松开急停按钮！", LogLevel.Error);
                    return false;
                }

                bool bInit = true;
                if (parameter == null)
                {
                    throw new ArgumentNullException("传入的参数不能为空");
                }

                if (parameter is SequenceModel)
                {
                    m_sequenceModel = parameter as SequenceModel;
                    LoadData(m_sequenceModel);
                }
                
                //所有轴回零
                if (!Global.IsAllgohome)
                {
                    bool result = AxisAllGoHome();
                    if (result)
                    {
                        Global.IsAllgohome = true;
                    }
                    else
                    {
                        m_DelOutPutLog("轴回零失败", LogLevel.Error);
                        bInit = false;
                    }
                }
                else
                {
                    bInit = InitRunStatus(true);
                }

                // 初始化相机
                if (!InitCamera())
                {
                    m_DelOutPutLog("初始化相机失败", LogLevel.Error);
                    bInit = false;
                }

                //初始化TCP
                if (!InitTCP())
                {
                    m_DelOutPutLog("初始化TCP失败", LogLevel.Error);
                    bInit = false;
                }

                //初始化COM
                if (!InitCOM())
                {
                    m_DelOutPutLog("初始化COM失败", LogLevel.Error);
                    bInit = false;
                }

                //参数初始化  
                m_ReadyLoad = false;
                m_IsCanPutCheck = false;
                m_ReadyDDRCheck = false;
                m_SmallGetDone = false;
                m_IsSmallCanSnap = false;
                m_ReadySmall = false;
                m_GetNGCount = 0;
                m_ProductRowCount = 0;
                m_NeedGetCount = 0;

                GetData();

                //初始化成功开始计时
                if (m_pauseWatch.IsRunning)
                {
                    m_pauseWatch.Stop();
                    m_pauseWatch.Reset();
                }

                return bInit;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="controlModel">传入参数</param>
        /// <param name="controlType">控制类型</param>
        /// <returns></returns>
        public BaseResultModel Run(BaseEntity controlModel, ControlType controlType)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                SetThreeLight(EThreeLight.运行);

                //开始计时 
                m_stopWatch.Start();
                m_warnWatch.Reset();

                //开始测试线程
                ThreadStart();

                resultModel.RunResult = true;

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage += ex.Message;
                return resultModel;
            }
        }

        /// <summary>
        /// 开始线程
        /// </summary>
        Thread m_threadStart;
        private void ThreadStart()
        {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ProcessThread);
            m_threadStart = new Thread(threadStart);
            m_threadStart.Start(m_sequenceModel);

            if (this.m_LoadThread == null || !this.m_LoadThread.IsAlive)
            {
                m_LoadThread = new Thread(LoadThread);
                m_LoadThread.Start();
            }

            if (this.m_CheckThread == null || !this.m_CheckThread.IsAlive)
            {
                m_CheckThread = new Thread(CheckThread);
                m_CheckThread.Start();
            }

            if (this.m_SmallThread == null || !this.m_SmallThread.IsAlive)
            {
                m_SmallThread = new Thread(SmallGetThread);
                m_SmallThread.Start();
            }

            CreateThread();
        }

        /// <summary>
        /// 线程停止
        /// </summary>
        public void ThreadAbort()
        {
            try
            {
                if(m_threadStart != null && m_threadStart.IsAlive)
                {
                    m_threadStart.Abort();
                }

                if(m_LoadThread != null && m_LoadThread.IsAlive)
                {
                    m_LoadThread.Abort();
                }

                if (m_SmallThread != null && m_SmallThread.IsAlive)
                {
                    m_SmallThread.Abort();
                }

                if (m_CheckThread != null && m_CheckThread.IsAlive)
                {
                    m_CheckThread.Abort();
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 执行主流程
        /// </summary>
        /// <param name="obj">输入参数</param>
        private void ProcessThread(object obj)
        {
            try
            {
                BaseResultModel resultModel;
                Autostep = AutoRunStep.StepZMoveSafePos;

                PointModel pointModel;
                RelatIoModel relatIoModel;
                IOModel ioModel;

                AutoRunStep preStep;
                //主流程循环执行
                while (!Global.Break)
                {
                    if (!Global.Pause)
                    {
                        string strStepDesc = CommFunc.GetEnumDescription<AutoRunStep>(Autostep);//获取当前步骤的描述
                        m_DelShowStep(0, "备料模组", (int)Autostep, strStepDesc);
                        preStep = Autostep;
                        switch (Autostep)
                        {
                            case AutoRunStep.StepZMoveSafePos://Z移动到安全位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepXYRMoveGetTrayPos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepXYRMoveGetTrayPos://移动XYR到接料盘位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_GetTray);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepTakeTrayMoveGetTrayPos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepTakeTrayMoveGetTrayPos://移动到接料盘位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_TakeTray, MotionParam.Pos_GetTray);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepGetCylinderUnClamp;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepGetCylinderUnClamp://取产品位气缸松开
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderClamp, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                        break;
                                    }
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderUnClamp, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepWaitLoadDone;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepWaitLoadDone://等待员工上料完成
                                {
                                    m_DelMessageBox("请上料!!", 1);
                                    AutoRunNG(null, "");
                                    Autostep = AutoRunStep.StepGetCylinderClamp;
                                    break;
                                }

                            case AutoRunStep.StepGetCylinderClamp://取产品位气缸夹紧
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderUnClamp, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                        break;
                                    }
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderClamp, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepXYMoveBigCameraPos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepXYMoveBigCameraPos://XY移动到大视野拍照位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepMoveBigCameraPos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepMoveBigCameraPos://双目相机移动到大视野拍照位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Camera, MotionParam.Pos_BigCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepBigCameraSnap;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepBigCameraSnap://大视野相机拍照执行算法
                                {
                                    resultModel = AlgrithmRun(0, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepXYMoveBigOffSetPos;
                                    }
                                    break;
                                }
                              
                            case AutoRunStep.StepXYMoveBigOffSetPos://XYR移动大视野补偿位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Camera, MotionParam.Pos_BigCameraOffSet);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepBigCameraSnapSecond;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepBigCameraSnapSecond://大视野相机第二次拍照
                                {
                                    resultModel = AlgrithmRun(0, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        m_ReadySmall = true;
                                        m_SmallGetDone = false;
                                        m_ProductRowCount = 6;
                                        Autostep = AutoRunStep.StepWaitSmallDone;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepWaitSmallDone://等待小视野完成
                                {
                                    if(!m_SmallGetDone)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepZMoveSafePos;
                                    }
                                    break;
                                }

                            default:
                                break;
                        }

                        if (preStep != Autostep)
                        {
                            //SaveCSV(Global.DataPath + "Process.csv", "时间,序号,步骤", string.Format("{0},{1},{2}\r\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"), (int)Autostep, strStepDesc));
                        }
                    }

                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    m_DelOutPutLog("ProcessThread Error", LogLevel.Error);
                    m_DelOutExLog(ex);
                }
                else
                {
                    m_DelOutPutLog("ProcessThread" + ex.Message, LogLevel.Debug);
                }
            }
        }

        /// <summary>
        /// 自动NG处理
        /// </summary>
        /// <param name="resultModel">结果Model</param>
        /// <param name="strStep">报警步骤名称</param>
        /// <param name="alarmID">报警ID</param>
        private void AutoRunNG(BaseResultModel resultModel, string strStep, int alarmID = -1)
        {
            if (Global.Run)
            {
                //显示错误信息Log
                if (resultModel != null)
                {
                    if (!string.IsNullOrEmpty(resultModel.ErrorMessage))
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage, LogLevel.Error);
                    }
                }

                //界面显示报警
                m_DelPcAlarm(resultModel, alarmID, strStep);
                Global.SetSystemStauts(Global.EnumSystemRunStatus.Pause);

                //设置报警三色灯蜂鸣器 
                SetThreeLight(EThreeLight.报警);

                PauseTiming(resultModel != null);

                //弹框显示报警信息
                if (alarmID != -1)
                {
                    //查询报警配置
                    AlarmValue alarmValue = m_sequenceModel.alarmConfigModel.ListAlarm.FirstOrDefault(x => x.AlarmID == alarmID);
                    if (alarmValue != null)
                    {
                        m_DelMessageBox(alarmValue.AlarmInfo, 3);
                    }
                    else
                    {
                        m_DelOutPutLog(string.Format("【{0}】报警配置为空！", alarmID), LogLevel.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 设置三色灯信号
        /// </summary>
        /// <param name="elight">类型</param>
        public void SetThreeLight(EThreeLight elight)
        {
            try
            {
                IOModel ioGreen = GetIOModel(MotionParam.DO_GreenLight, 1);
                IOModel ioYellow = GetIOModel(MotionParam.DO_YellowLight, 1);
                IOModel ioRedOn = GetIOModel(MotionParam.DO_RedLight, 1);
                IOModel ioBeep = GetIOModel(MotionParam.DO_Beep, 0);

                switch (elight)
                {
                    case EThreeLight.运行:
                        m_MotroContorl.Run(ioGreen, MotorControlType.IOTrigger);
                        ioYellow.val = 0;
                        m_MotroContorl.Run(ioYellow, MotorControlType.IOTrigger);
                        ioRedOn.val = 0;
                        m_MotroContorl.Run(ioRedOn, MotorControlType.IOTrigger);
                        m_MotroContorl.Run(ioBeep, MotorControlType.IOTrigger);
                        break;
                    case EThreeLight.暂停:
                        ioGreen.val = 0;
                        m_MotroContorl.Run(ioGreen, MotorControlType.IOTrigger);
                        ioYellow.val = 1;
                        m_MotroContorl.Run(ioYellow, MotorControlType.IOTrigger);
                        ioRedOn.val = 0;
                        m_MotroContorl.Run(ioRedOn, MotorControlType.IOTrigger);
                        m_MotroContorl.Run(ioBeep, MotorControlType.IOTrigger);
                        break;
                    case EThreeLight.报警:
                        ioGreen.val = 0;
                        m_MotroContorl.Run(ioGreen, MotorControlType.IOTrigger);
                        ioYellow.val = 0;
                        m_MotroContorl.Run(ioYellow, MotorControlType.IOTrigger);
                        ioRedOn.val = 1;
                        m_MotroContorl.Run(ioRedOn, MotorControlType.IOTrigger); 
                        ioBeep.val = 1;
                        m_MotroContorl.Run(ioBeep, MotorControlType.IOTrigger);
                        if (m_CosModel.IsShieldBuzzer)
                        {
                            Thread.Sleep(200);
                            ioBeep.val = 0;
                            m_MotroContorl.Run(ioBeep, MotorControlType.IOTrigger);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 蜂鸣器设置关闭
        /// </summary>
        public void SetBeepOff()
        {
            try
            {
                IOModel ioBeep = GetIOModel(MotionParam.DO_Beep, 0);
                m_MotroContorl.Run(ioBeep, MotorControlType.IOTrigger);
            }
            catch (Exception ex)
            {
                 
            }
        }

        #endregion

        #region 子流程

        /// <summary>
        /// 小视野抓取线程
        /// </summary>
        private void SmallGetThread()
        {
            try
            {
                BaseResultModel resultModel;
                SmallStep = SmallRunStep.StepWaitBigDone;
                PointModel pointModel;
                RelatIoModel relatIoModel;
                IOModel ioModel;

                while (!Global.Break)
                {
                    if (!Global.Pause)
                    {
                        string strStepDesc = CommFunc.GetEnumDescription<SmallRunStep>(SmallStep);//获取当前步骤的描述 
                        m_DelShowStep(1, "小视野模组", (int)SmallStep, strStepDesc);

                        switch (SmallStep)
                        {
                            case SmallRunStep.StepWaitBigDone://等待大视野相机拍完
                                {
                                    if (!m_ReadySmall)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        //确认余下待取产品行
                                        if (m_ProductRowCount > 0)
                                        {
                                            m_ProductRowCount--;
                                            m_DelOutPutLog("剩余行数：" + m_ProductRowCount.ToString());
                                            SmallStep = SmallRunStep.StepXYMoveSmallCenterPos;
                                        }
                                        else
                                        {
                                            m_ReadySmall = false;
                                            m_SmallGetDone = true;
                                            continue;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYMoveSmallCenterPos://XYR移动小视野中心位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepMoveSmallCameraPos;
                                    }

                                    break;
                                }

                            case SmallRunStep.StepMoveSmallCameraPos://双目相机移动到小视野拍照位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Camera, MotionParam.Pos_SmallCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepMoveSuckFilmPos;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepMoveSuckFilmPos://顶升模组移到吸蓝膜位置
                                {
                                    if (m_IsSmallCanSnap)
                                    {
                                        pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_SuckFilm);
                                        resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                        if (!resultModel.RunResult)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, 10001);
                                        }
                                        else
                                        {
                                            Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                            SmallStep = SmallRunStep.StepSuckFilm;
                                        }
                                    }
                                    else
                                    {
                                        Thread.Sleep(50);
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSuckFilm://吸蓝膜
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetSuctionVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        SmallStep = SmallRunStep.StepSmallCameraSnapBar;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallCameraSnapBar://小视野相机拍照推算Bar条号
                                {
                                    resultModel = AlgrithmRun(1, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        SmallStep = SmallRunStep.StepWaitLoadDone;
                                    }
                                    m_NeedGetCount = 6;

                                    break;
                                }

                            case SmallRunStep.StepWaitLoadDone://等待上料取产品完成
                                {
                                    if(!m_IsSmallCanSnap)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    { 
                                        SmallStep = SmallRunStep.StepXYMoveSmallGetPos;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYMoveSmallGetPos://XYR移动到拍照抓取位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepSmallCameraSnapOCR;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallCameraSnapOCR://小视野相机定位产品OCR
                                {
                                    resultModel = AlgrithmRun(1, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        m_IsSmallCanSnap = false;
                                        m_ReadyLoad = true;

                                        //判断当前行是否存在需要取的物料？
                                        m_NeedGetCount--;
                                        if (m_NeedGetCount > 0)
                                        {
                                            SmallStep = SmallRunStep.StepWaitLoadDone;
                                            m_DelOutPutLog(string.Format("剩余行：{0} 剩余产品: {1}", m_ProductRowCount, m_NeedGetCount));
                                        }
                                        else
                                        {
                                            SmallStep = SmallRunStep.StepWaitBigDone;
                                        }
                                    }
                                    break;
                                }

                            default:
                                break;
                        }
                        Thread.Sleep(5);
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    m_DelOutPutLog("EmptyThread Error", LogLevel.Error);
                    m_DelOutExLog(ex);
                }
                else
                {
                    m_DelOutPutLog("EmptyThread" + ex.Message, LogLevel.Debug);
                }
            }
        }

        /// <summary>
        /// 上料测试线程
        /// </summary>
        private void LoadThread()
        {
            try
            {
                BaseResultModel resultModel;
                LoadStep = LoadRunStep.StepLoadMoveWaitPos;
                PointModel pointModel;
                RelatIoModel relatIoModel;
                IOModel ioModel;

                while (!Global.Break)
                {
                    if (!Global.Pause)
                    {
                        string strStepDesc = CommFunc.GetEnumDescription<LoadRunStep>(LoadStep);//获取当前步骤的描述  
                        m_DelShowStep(2, "上料模组", (int)LoadStep, strStepDesc);
                        switch (LoadStep)
                        {
                            case LoadRunStep.StepLoadMoveWaitPos://上料模组移动到等待位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_LoadWait);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepWaitReadLoad;
                                        m_IsSmallCanSnap = true;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepWaitReadLoad://等待是否可以取料
                                {
                                    if(!m_ReadyLoad)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        m_ReadyLoad = false;
                                        LoadStep = LoadRunStep.StepLoadMoveTakePos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadMoveTakePos://上料模组移动到上料位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_Load);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepThimbleRisePos;
                                    }

                                    break;
                                }

                            case LoadRunStep.StepThimbleRisePos://顶针上升
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_ThimbleRise);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadSuctionVaccum;
                                    }
                                    break;
                                }
                                 
                            case LoadRunStep.StepLoadSuctionVaccum://上料吸嘴吸真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_LoadSuctionVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumDelayTime);
                                        LoadStep = LoadRunStep.StepThimbleDownPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepThimbleDownPos://顶针下降
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_ThimbleDown);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadMoveNSnapPos;
                                        m_LoadCount++;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadMoveNSnapPos://上料模组移动N面拍照位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_NSnap);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepNSnap;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepNSnap://N面拍照执行算法
                                {
                                    resultModel = AlgrithmRun(2, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        LoadStep = LoadRunStep.StepMoveUnLoadWaitPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepMoveUnLoadWaitPos://移动到下料等待位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_UnLoadWait);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepWaitReadyCheck;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepWaitReadyCheck://等待是否放产品到检测位
                                {
                                    if (!m_IsCanPutCheck)//是否可以移动放料到DDR
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        m_IsCanPutCheck = false;
                                        LoadStep = LoadRunStep.StepLoadMoveDDR;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadMoveDDR://移动到DDR马达
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadBreakVaccum;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadBreakVaccum://上料吸嘴破真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_LoadBreakVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumBreakDelay);
                                        LoadStep = LoadRunStep.StepLoadMoveWaitPos_2;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadMoveWaitPos_2://移动到上料等待位2
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Load, MotionParam.Pos_LoadWait);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadMoveWaitPos;
                                        m_ReadyDDRCheck = true;//可以来取DDR物料
                                    }
                                    break;
                                }

                            default:
                                break;
                        }

                        Thread.Sleep(5);
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    m_DelOutPutLog("LoadThread Error", LogLevel.Error);
                    m_DelOutExLog(ex);
                }
                else
                {
                    m_DelOutPutLog("LoadThread" + ex.Message, LogLevel.Debug);
                }
            }
        }
        
        /// <summary>
        /// 检测下料测试线程
        /// </summary>
        private void CheckThread()
        {
            try
            {
                BaseResultModel resultModel;
                CheckStep = CheckRunStep.StepMoveUnLoadSafePos;
                PointModel pointModel;
                RelatIoModel relatIoModel;
                IOModel ioModel;

                while (!Global.Break)
                {
                    if (!Global.Pause)
                    {
                        string strStepDesc = CommFunc.GetEnumDescription<CheckRunStep>(CheckStep);//获取当前步骤的描述 
                        m_DelShowStep(3, "检测模组", (int)CheckStep, strStepDesc);

                        switch (CheckStep)
                        {
                            case CheckRunStep.StepMoveUnLoadSafePos://移动到下料安全位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_UnLoadWait);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepWaitReadyDDRCheck;
                                        m_IsCanPutCheck = true;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepWaitReadyDDRCheck://等待DDR开始检测产品
                                {
                                    if (!m_ReadyDDRCheck)//是否可以开始检测产品
                                    {
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        m_ReadyDDRCheck = false;
                                        CheckStep = CheckRunStep.StepMoveCorrect1Pos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepMoveCorrect1Pos://检测模组移动到上料校正位置1
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Correct1);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadMoveARPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadMoveARPos://下料模组移动到AR定位位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_ARLocation);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepCorrect1Snap;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepCorrect1Snap://校正位置1拍照执行算法
                                {
                                    resultModel = AlgrithmRun(3, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepMoveARCheckPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepMoveARCheckPos://检测模组移动到AR检测位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_ARCheck);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepARCheckSnap;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepARCheckSnap://AR检测拍照执行算法
                                {
                                    resultModel = AlgrithmRun(4, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepMoveCorrect2Pos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepMoveCorrect2Pos://检测模组移动到上料校正位置2
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_ARCheck);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadMoveHRPos;
                                    }
                                    break;
                                }
                                
                            case CheckRunStep.StepUnLoadMoveHRPos://下料模组移动到HR定位位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_HRLocation);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepCorrect2Snap;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepCorrect2Snap://校正位置2拍照执行算法
                                {
                                    resultModel = AlgrithmRun(3, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepMoveHRCheckPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepMoveHRCheckPos://检测模组移动到HR检测位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_HRCheck);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepHRCheckSnap;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepHRCheckSnap://HR检测拍照执行算法
                                {
                                    resultModel = AlgrithmRun(5, ESuck.吸嘴1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepCheckMoveUnLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepCheckMoveUnLoadPos://检测模组移动到下料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadMoveLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadMoveLoadPos://下料模组移动到上料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadSuctionVaccum;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadSuctionVaccum://下料吸嘴吸真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadSuctionVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumDelayTime);
                                        CheckStep = CheckRunStep.StepUnLoadMovePutPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadMovePutPos://下料模组移动到放料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_PutTray);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadBreakVaccum;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadBreakVaccum://下料吸嘴破真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadBreakVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, 20001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumDelayTime);
                                        CheckStep = CheckRunStep.StepMoveUnLoadSafePos;
                                        m_OKCount++;
                                        m_UnLoadCount++;
                                    }
                                    break;
                                }

                            default:
                                break;
                        }

                        Thread.Sleep(5);
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    m_DelOutPutLog("CheckThread Error", LogLevel.Error);
                    m_DelOutExLog(ex);
                }
                else
                {
                    m_DelOutPutLog("CheckThread" + ex.Message, LogLevel.Debug);
                }
            }
        }
        
        /// <summary>
        /// 创建执行线程
        /// </summary>
        private void CreateThread()
        {
            if (this.m_DetectBtnThread == null || !this.m_DetectBtnThread.IsAlive)
            {
                m_DetectBtnThread = new Thread(DetectBtnStatus);
                m_DetectBtnThread.Start();
            }
        }

        /// <summary>
        /// 创建急停线程
        /// </summary>
        public void EmergencyThread()
        {
            if (this.m_EmergencyThread == null || !this.m_EmergencyThread.IsAlive)
            {
                m_EmergencyThread = new Thread(DetectEmergency);
                m_EmergencyThread.IsBackground = true;
                m_EmergencyThread.Start();
            }
        }

        #endregion

        #region 初始化设备 机台初始化

        /// <summary>
        /// 初始化相机
        /// </summary>
        public bool InitCamera()
        {
            try
            {
                var listCamera = XmlControl.sequenceModelNew.Camera2DSetModels;
                foreach (var item in listCamera)
                {
                    SetCameraType(item);

                    if (item.bLocalImage)
                    {
                        m_DelOutPutLog(string.Format("【{0}】配置为本地图片!", item.Name), LogLevel.Debug);
                        continue;
                    }
                    var resultModel = m_CameraControl[item.Id].Run(item, CameraControlType.CameraOpenBySoft);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(string.Format("【{0}】打开失败!", item.Name), LogLevel.Error);
                        return false;
                    }
                    resultModel = m_CameraControl[item.Id].Run(item, CameraControlType.CameraSetParam);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(string.Format("【{0}】设置参数失败!", item.Name), LogLevel.Error);
                        return false;
                    }

                    m_Mutexs[item.Id] = new Mutex();
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 设置相机类型
        /// </summary>
        /// <param name="tModel">相机</param>
        private void SetCameraType(Camera2DSetModel tModel)
        {
            try
            {
                switch (tModel.cameraType)
                {
                    case CameraType.BASLER:
                        m_CameraControl[tModel.Id] = new CameraByBaslerControl();
                        break;
                    case CameraType.MVHAL:
                        m_CameraControl[tModel.Id] = new CameraByHalconControl();
                        break;
                    case CameraType.MVS:
                        m_CameraControl[tModel.Id] = new CameraByMVSControl();
                        break;
                    case CameraType.ITEK:
                        m_CameraControl[tModel.Id] = new CameraByITekControl();
                        break;
                    case CameraType.IDS:
                        m_CameraControl[tModel.Id] = new CameraByIDSControl();
                        break;

                    default:
                        m_CameraControl[tModel.Id] = new CameraByHalconControl();
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 初始化运动Model
        /// </summary>
        public bool InitMotorModel()
        {
            try
            {
                bool bresult = true;
                string path = Global.CurrentPath + "//Sequence//Card//Card.dsr";
                var model = (ControlCardModel)XmlControl.LoadFromXml(path, typeof(ControlCardModel));
                if (model == null)
                {
                    XmlControl.controlCardModel = new ControlCardModel();
                }
                else
                {
                    XmlControl.controlCardModel = model;
                }

                //初始化控制卡
                m_MotroContorl = MotorInstance.GetInstance();
                if (!m_MotroContorl.Init(m_parameter.MotionCard))
                {
                    m_DelOutPutLog("初始化运动控制卡失败", LogLevel.Error);
                    return false;
                }
                else
                {
                    Thread.Sleep(800);
                    bresult = ResetAxisStatus();

                    SetThreeLight(EThreeLight.暂停);
                }

                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 初始化TCP通讯
        /// </summary>
        /// <returns></returns>
        public bool InitTCP(TCPIPModel tcp = null)
        {
            try
            {
                if (tcp == null)
                {
                    //初始化TCP通讯
                    foreach (var tcpModel in XmlControl.sequenceModelNew.TCPIPModels)
                    {
                        //如果PLC有链接则不需要初始化
                        if (XmlControl.sequenceModelNew.PLCSetModels.FindIndex(x => x.ConnObj == tcpModel.Name) != -1)
                        {
                            continue;
                        }

                        NewSocket();
                        bool bresult = m_socketControl[tcpModel.Id].Init(tcpModel);
                        if (!bresult)
                        {
                            m_DelOutPutLog(string.Format("初始化网口{0}失败", tcpModel.Name), LogLevel.Error);
                            return false;
                        }
                    }
                }
                else
                {
                    NewSocket();
                    bool bresult = m_socketControl[tcp.Id].Init(tcp);
                    if (!bresult)
                    {
                        m_DelOutPutLog(string.Format("初始化网口{0}失败", tcp.Name), LogLevel.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 创建TCP实例
        /// </summary>
        private void NewSocket()
        {
            if (m_socketControl == null)
            {
                m_socketControl = new SocketControl[6];
                for (int i = 0; i < 6; i++)
                {
                    m_socketControl[i] = new SocketControl();
                }
            }
        }

        /// <summary>
        /// 初始化串口通讯
        /// </summary>
        /// <returns></returns>
        public bool InitCOM()
        {
            try
            {
                //初始化串口通讯
                foreach (var comModel in XmlControl.sequenceModelNew.ComModels)
                {
                    //如果PLC有链接则不需要初始化
                    if (XmlControl.sequenceModelNew.PLCSetModels.FindIndex(x => x.ConnObj == comModel.Name) != -1)
                    {
                        continue;
                    }

                    NewSerial();
                    var resultModel = m_serialControl[comModel.Id].Run(comModel, ControlType.SerialPortOpen);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(string.Format("初始化串口{0}失败", comModel.Name), LogLevel.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 创建串口实例
        /// </summary>
        private void NewSerial()
        {
            if (m_serialControl == null)
            {
                m_serialControl = new SerialControl[6];
                for (int i = 0; i < 6; i++)
                {
                    m_serialControl[i] = new SerialControl();
                }
            }
        }

        /// <summary>
        /// 所有轴回零
        /// </summary>
        /// <returns>返回结果</returns>
        public bool AxisAllGoHome()
        {
            try
            {
                lock (this)
                {
                    BaseResultModel resultModel = new BaseResultModel();

                    resultModel.RunResult = InitRunStatus(false);
                    if (!resultModel.RunResult)
                    {
                        return false;
                    }

                    //伺服轴回零 
                    resultModel = m_MotroContorl.Run(null, MotorControlType.AxisAllGoHome);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }
                    
                    //resultModel = CKDHome();

                    return resultModel.RunResult;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 复位轴状态
        /// </summary>
        /// <returns>返回结果</returns>
        public bool ResetAxisStatus()
        {
            try
            {
                bool bresult = true;
                foreach (var item in XmlControl.controlCardModel.StationModels)
                {
                    var resultModel = m_MotroContorl.Run(item, MotorControlType.AxisStop);
                    Thread.Sleep(50);
                    resultModel = m_MotroContorl.Run(item, MotorControlType.AxisReset);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(item.Name + "复位失败", LogLevel.Error);
                        bresult = false;
                    }
                    Thread.Sleep(100);
                    resultModel = m_MotroContorl.Run(item, MotorControlType.AxisEnable);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(item.Name + "使能失败", LogLevel.Error);
                        bresult = false;
                    }
                    Thread.Sleep(100);
                }

                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            } 
        }

        /// <summary>
        /// 初始化运行状态
        /// </summary>
        /// <returns></returns>
        public bool InitRunStatus(bool bInitCKD)
        {
            try
            {
                BaseResultModel resultModel = new BaseResultModel();
                resultModel.RunResult = true;
              

                if(bInitCKD)
                {
                    //CKD回零                     
                    //resultModel = CKDHome();
                }

                return resultModel.RunResult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }
        #endregion

        #region 相机实时显示

        /// <summary>
        /// 开始运行后执行实时采集
        /// </summary>
        public void CameraRealDisplay()
        {
            try
            {
                if (!Global.IsRealDisplay)
                {
                    return;
                }

                Thread t = new Thread(new ThreadStart(() =>
                {
                    var listCamera = XmlControl.sequenceModelNew.Camera2DSetModels;

                    Parallel.ForEach(listCamera, new Action<Camera2DSetModel>(model =>
                    {
                        while (true)
                        {
                            if (Global.Break)
                            {
                                break;
                            }
                            if (model.IsCameraSnaping)
                            {
                                Thread.Sleep(4000);
                                continue;
                            }
                            if (model.IsOpen)
                            {
                                if (m_Mutexs[model.Id] == null)
                                {
                                    m_Mutexs[model.Id] = new Mutex();
                                }
                                m_Mutexs[model.Id].WaitOne();
                                //执行拍照  
                                var resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraStartSnapBySoft) as CameraResultModel;
                                if (!resultModel.RunResult)
                                {
                                    m_DelOutPutLog(string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage), LogLevel.Error);
                                }
                                HObject hImage = resultModel.Image as HObject;

                                if (hImage != null && hImage.IsInitialized())
                                {
                                    resultModel.IndexResult = model.Id;
                                    OnDispPicEvent(resultModel);
                                }
                                m_Mutexs[model.Id].ReleaseMutex();
                            }
                            Thread.Sleep(10);
                        }
                    }));
                }));
                t.Start();
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 点击按钮开始实时
        /// </summary>
        public void BtnRealDisplay()
        {
            try
            {
                if (m_RealDisplay)
                {
                    return;
                }
                Thread t = new Thread(new ThreadStart(() =>
                {
                    var listCamera = XmlControl.sequenceModelNew.Camera2DSetModels;
                    InitCamera();

                    Parallel.ForEach(listCamera, new Action<Camera2DSetModel>(model =>
                    {
                        while (true)
                        {
                            if (Global.Run)
                            {
                                m_RealDisplay = false;
                                m_DelOutPutLog("程序在运行中，请停止再操作");
                                break;
                            }

                            m_RealDisplay = true;
                            if (model.IsOpen)
                            {
                                //执行拍照  
                                var resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraStartSnapBySoft) as CameraResultModel;
                                if (!resultModel.RunResult)
                                {
                                    m_DelOutPutLog(string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage));
                                    m_RealDisplay = false;
                                    break;
                                }
                                HObject hImage = resultModel.Image as HObject;

                                if (hImage != null && hImage.IsInitialized())
                                {
                                    resultModel.IndexResult = model.Id;
                                    OnDispPicEvent(resultModel);
                                }
                            }
                            Thread.Sleep(10);
                        }
                    }));
                }));
                t.Start();
            }
            catch (Exception ex)
            {
                m_RealDisplay = false;
                m_DelOutExLog(ex);
            }
        }

        #endregion

        #region 位置判断

        /// <summary>
        /// 判断上升Z轴是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeZSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetEncodePosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }
        
        #endregion

        #region 相机流程

        /// <summary>
        /// 相机 算法 计算
        /// </summary>
        /// <param name="tModel"></param>
        /// <returns>返回图像</returns>
        private CameraResultModel Camera2DSnap(Camera2DSetModel model)
        {
            CameraResultModel cameraResultModel = new CameraResultModel();
            try
            {
                HObject hImage = new HObject();
                HOperatorSet.GenEmptyObj(out hImage);

                if (model.bLocalImage)//读取本地图片
                {
                    string lpath = model.LocalPath;
                    HOperatorSet.ReadImage(out hImage, lpath);

                    cameraResultModel.RunResult = true;
                }
                else//相机拍照
                {
                    if (m_CameraControl[model.Id] == null)
                    {
                        SetCameraType(model);
                    }
                    //为了实时画面显示
                    model.IsCameraSnaping = true;
                    if (m_Mutexs[model.Id] == null)
                    {
                        m_Mutexs[model.Id] = new Mutex();
                    }
                    m_Mutexs[model.Id].WaitOne();

                    //打开相机
                    var resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraOpenBySoft) as CameraResultModel;
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage));
                    }

                    //设置参数
                    resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraSetParam) as CameraResultModel;
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage));
                    }

                    //执行拍照
                    resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraStartSnapBySoft) as CameraResultModel;
                    if (!resultModel.RunResult)
                    {
                        resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraStartSnapBySoft) as CameraResultModel;
                        m_DelOutPutLog(string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage));
                    }

                    //如果是外部触发
                    if (model.IsExternTrig)
                    {
                        while (!Global.Break)
                        {
                            resultModel = m_CameraControl[model.Id].Run(model, CameraControlType.CameraGetImageByTirgger) as CameraResultModel;
                            if (resultModel.RunResult)
                            {
                                hImage = resultModel.Image as HObject;
                                break;
                            }
                            Thread.Sleep(20);
                        }
                    }
                    else
                    {
                        hImage = resultModel.Image as HObject;
                    }

                    cameraResultModel.RunResult = resultModel.RunResult;

                    model.IsCameraSnaping = false;

                    m_Mutexs[model.Id].ReleaseMutex();
                }

                //主界面显示拍照图片
                cameraResultModel.Image = hImage;
                cameraResultModel.IndexResult = model.Id;
                OnDispPicEvent(cameraResultModel);

                return cameraResultModel;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return cameraResultModel;
            }
        }

        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="index">相机Index 0:上相机 1：下相机</param>
        /// <returns>返回结果</returns>
        private BaseResultModel AlgrithmRun(int index, ESuck esuck)
        {
            AlgorithmResultModel algorithmResult = new AlgorithmResultModel();
            try
            {
                Thread.Sleep(m_CosModel.SnapTimeOut);
                Camera2DSetModel model = XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == index);
                CameraResultModel cameraResult = Camera2DSnap(model);

                algorithmResult.RunResult = true;
                return algorithmResult;

                var algorithmModel = m_sequenceModel.algorithmModels.FirstOrDefault(x => x.Name == model.Name);
                algorithmModel.Image = cameraResult.Image as HObject;
                algorithmModel.IsShieldOcr = m_CosModel.IsShieldOCR;
                algorithmResult = m_algorithmControll.Run(algorithmModel, AlgorithmControlType.AlgorithmRun) as AlgorithmResultModel;


                //显示结果信息
                HObject ho_Contour;
                HOperatorSet.ConcatObj(algorithmResult.ProXLDTrans, algorithmResult.CenterCross, out ho_Contour);
                if (algorithmResult.bFindCenter && !algorithmResult.bImageRotate)
                {
                    algorithmResult.RunResult = true;

                    //Log显示
                    string strlog = string.Format("Row:{0},Col:{1},Angle:{2},OCR:{3}", Math.Round(algorithmResult.CenterRow.D, 1),
                        Math.Round(algorithmResult.CenterColumn.D, 1), Math.Round(algorithmResult.CenterPhi.D, 1), algorithmResult.OcrResult ? algorithmResult.strOCR : "");
                    m_DelOutPutLog(strlog);
                    cameraResult.ResultLabel = strlog;

                    //判断读取OCR是否错误--位数与设定值不符
                    if(algorithmResult.OcrResult)
                    {
                        if(algorithmResult.strOCR.Length != m_paramRangeModel.OcrNum && !m_CosModel.IsShieldOCR)
                        {
                            algorithmResult.OcrResult = false;
                        }
                    } 
                }
                else
                {
                    algorithmResult.RunResult = false;
                    HOperatorSet.ConcatObj(ho_Contour, algorithmResult.ObjectResult as HObject, out ho_Contour); 
                }

                cameraResult.DispObj = ho_Contour;
                OnDispPicEvent(cameraResult); 

                //保存图片
                WriteImage(model.Name, algorithmResult.RunResult, algorithmResult.OcrResult, algorithmResult.strOCR, cameraResult.Image as HObject, algorithmResult.ImageRotateText as HObject);
                
                //OCR识别错误弹框
                if (algorithmResult.RunResult && !algorithmResult.OcrResult && index == 0)
                {
                    //添加NG暂停
                    AutoRunNG(new BaseResultModel(), "上相机拍照", -1);
                    string strOcr = m_DelOcrView(algorithmResult.strOCR);
                    algorithmResult.strOCR = strOcr;
                }

                return algorithmResult;
            }
            catch (Exception ex)
            {
                algorithmResult.RunResult = false;
                m_DelOutExLog(ex);
                return algorithmResult;
            }
        }

        private void InputOCR(AlgorithmResultModel algorithmResult, ESuck esuck)
        {
            try
            {
                //OCR识别错误弹框
                if (algorithmResult.RunResult && !algorithmResult.OcrResult)
                {
                    string strOcr = m_DelOcrView(algorithmResult.strOCR);
                    algorithmResult.strOCR = strOcr;
                }  
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 图片保存
        /// </summary>
        /// <param name="name">图片名称</param>
        /// <param name="result">结果</param>
        /// <param name="ocrresult">OCR识别结果</param>
        /// <param name="ho_Image">图片</param>
        private void WriteImage(string name, bool result, bool ocrresult, string ocr, HObject ho_Image, HObject ho_RotateImage)
        {
            try
            {
                string strPath = m_parameter.ImagePath + "//" + name + "//" + DateTime.Now.ToString("yyyyMMdd") + "//";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strName = name + "_" + DateTime.Now.ToString("HHmmssfff") + ".jpg";

                string strOcrPath = strPath + "OCR//";
                if (!Directory.Exists(strOcrPath))
                {
                    Directory.CreateDirectory(strOcrPath);
                }

                bool bsaveOcr = false;
                string strOcrName = "";
                if(!string.IsNullOrEmpty(ocr))
                {
                    bsaveOcr = true;
                    strOcrName = ocr + "_" + DateTime.Now.ToString("HHmmssfff") + ".jpg";
                }

                if (m_parameter.IsSaveImg)
                {
                    //只保存NG图片
                    if (m_parameter.IsSaveNGImg && !result)
                    { 
                        //NG图片
                        string ngPath = strPath + "NG//";
                        AlgorithmCommHelper.SaveImage(ho_Image as HObject, ngPath, strName);
                        if (bsaveOcr)
                        {
                            AlgorithmCommHelper.SaveImage(ho_RotateImage as HObject, strOcrPath, strOcrName);
                        }
                    }
                    else
                    {
                        //OK图片
                        if (result)
                        {
                            AlgorithmCommHelper.SaveImage(ho_Image as HObject, strPath, strName);
                        }
                        else
                        {
                            //NG图片
                            string ngPath = strPath + "NG//";
                            AlgorithmCommHelper.SaveImage(ho_Image as HObject, ngPath, strName);
                        }

                        //是否保存OCR图片
                        if (bsaveOcr)
                        {
                            //OK图片
                            if(ocrresult)
                            {
                                AlgorithmCommHelper.SaveImage(ho_RotateImage as HObject, strOcrPath, strOcrName);
                            }
                            else
                            {
                                //NG图片
                                string ngPath = strOcrPath + "NG//";
                                AlgorithmCommHelper.SaveImage(ho_RotateImage as HObject, ngPath, strOcrName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 保存截图到路径
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="ocr"></param>
        private void SaveScreenToPath(int index, string name, string ocr)
        {
            try
            {
                string strPath = m_parameter.ImagePath + "//结果图片//" + DateTime.Now.ToString("yyyyMMdd") + "//";
                if(!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                 
                string strName = strPath + ocr + "_" + name + "_" + DateTime.Now.ToString("HHmmssfff") + ".jpg";
                m_DelSaveScreen(index, strName);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
        
        AlgorithmControl m_algorithmControl = new AlgorithmControl();
        private AlgorithmResultModel TestAlgorithm(int fixtureId)
        {
            AlgorithmResultModel result = new AlgorithmResultModel();
            try
            {
                Camera2DSetModel model = XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == 0);
                CameraResultModel cameraResult = Camera2DSnap(model);

                string strPath = "D:\\CameraImage\\治具相机"; 
                AlgorithmCommHelper.SaveImage(cameraResult.Image as HObject, strPath, fixtureId.ToString() + ".jpg");

                string paramPath = Global.Model3DPath + "//" + "治具相机";
                var algorithmModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == fixtureId);

                HObject ho_Xld, ho_Cross, ho_OutObj;
                HTuple hv_bFindCenter, hv_Exception;

                HTuple hv_Row, hv_Column, hv_Phi;
                m_algorithmControl.FindFixturePos(cameraResult.Image as HObject, paramPath, fixtureId.ToString(), out ho_Xld, out ho_Cross, algorithmModel.InMeasureLength1,
                    algorithmModel.InMeasureLength2, algorithmModel.InMeasureSigma, algorithmModel.InMeasureThreshold, algorithmModel.InMeasureSelect, algorithmModel.InMeasureTransition,
                    algorithmModel.InMeasureNumber, algorithmModel.InMeasureScore, out hv_Row, out hv_Column, out hv_Phi, out hv_bFindCenter, out hv_Exception, out ho_OutObj);
                 
                HObject ho_Contour;
                HOperatorSet.GenEmptyObj(out ho_Contour);
                HOperatorSet.ConcatObj(ho_Cross, ho_Xld, out ho_Contour);
                HOperatorSet.ConcatObj(ho_Contour, ho_OutObj, out ho_Contour);
                cameraResult.DispObj = ho_Contour;
                OnDispPicEvent(cameraResult);

                result.RunResult = hv_bFindCenter == 1;

                result.CenterRow = hv_Row;
                result.CenterColumn = hv_Column;
                result.CenterPhi = hv_Phi;
                 
                return result;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return result;
            }
        }
        #endregion

        #region 侦测设备按钮
        /// <summary>
        /// 检测设备按钮状态
        /// </summary>
        public void DetectBtnStatus()
        {
            try
            {
                BaseResultModel resultModel = new BaseResultModel();
                IOModel ioModel = new IOModel();
                int value;
                while (!Global.Break)
                {
                    //开始按钮
                    ioModel = GetIOModel(MotionParam.DI_Start, 1);
                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.QueryDI);
                    if (resultModel.RunResult)
                    {
                        value = Int32.Parse(resultModel.ObjectResult.ToString());
                        if (value == 1)
                        {
                            m_DelStart();
                        }
                    }

                    //暂停按钮
                    ioModel = GetIOModel(MotionParam.DI_Stop, 1);
                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.QueryDI);
                    if (resultModel.RunResult)
                    {
                        value = Int32.Parse(resultModel.ObjectResult.ToString());
                        if (value == 1)
                        {
                            m_DelStop();
                        }
                    }

                    //安全门
                    if(!m_CosModel.IsShieldDoor)
                    {
                        ioModel = GetIOModel(MotionParam.DI_Door, 1);
                        resultModel = m_MotroContorl.Run(ioModel, MotorControlType.QueryDI);
                        if (resultModel.RunResult)
                        {
                            value = Int32.Parse(resultModel.ObjectResult.ToString());
                            if (value == 1)
                            {
                                m_DelStop();
                            }
                        }
                    }

                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 侦测急停按钮
        /// </summary>
        public void DetectEmergency()
        {
            try
            {
                bool bvalue = false;
                while (Global.Frame_Start)
                {
                    BaseResultModel resultModel = new BaseResultModel();
                    IOModel ioModel = new IOModel();
                    int value;
                    //急停按钮
                    ioModel = GetIOModel(MotionParam.DI_Emergey, 1);
                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.QueryDI);
                    if (resultModel.RunResult)
                    {
                        value = Int32.Parse(resultModel.ObjectResult.ToString());
                        if (value == 1 && !bvalue)
                        {
                            Global.IsEmergency = true;
                            m_DelEmergency(true);
                            bvalue = true;
                        }
                        else if(value == 0)
                        {
                            if (bvalue)
                            {
                                Global.IsEmergency = false; 
                                m_DelEmergency(false);
                                bvalue = false;
                            }
                        }
                    }

                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 设备操作

        /// <summary>
        /// 光源操作委托
        /// </summary>
        /// <param name="command">操作命令</param>
        /// <param name="comName">串口名称</param>
        public void Del_ELightControl(string command, string comName)
        {
            try
            {
                NewSerial();
                var comModel = XmlControl.sequenceModelNew.ComModels.FirstOrDefault(x => x.Name == comName);
                comModel.SendContent = command;
                var resultModel = m_serialControl[comModel.Id].Run(comModel, ControlType.SerialPortOpen);
                if (!resultModel.RunResult)
                {
                    m_DelOutPutLog("打开串口失败" + resultModel.ErrorMessage, LogLevel.Error);
                }
                resultModel = m_serialControl[comModel.Id].Run(comModel, ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_DelOutPutLog("发送串口消息失败" + resultModel.ErrorMessage, LogLevel.Error);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设备关闭
        /// </summary>
        public void CloseDevice()
        {
            try
            {
                //关闭网口通讯
                foreach (var tcpipModel in XmlControl.sequenceModelNew.TCPIPModels)
                {
                    m_socketControl[tcpipModel.Id].Close();
                }

                //关闭串口通讯
                foreach (var comModel in XmlControl.sequenceModelNew.ComModels)
                {
                    m_serialControl[comModel.Id].Run(comModel, ControlType.SerialPortClose);
                }

                //关闭相机
                foreach (var cameraModel in XmlControl.sequenceModelNew.Camera2DSetModels)
                {
                    m_CameraControl[cameraModel.Id].Run(cameraModel, CameraControlType.CameraClose);
                }

                m_MotroContorl.Run(null, MotorControlType.Close);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// TCP发送数据
        /// </summary>
        /// <param name="tcpModel">TCP的Model</param>
        /// <param name="content">发送内容</param>
        public void TCPSend(TCPIPModel tcpModel, string content)
        {
            try
            {
                tcpModel.SendContent = content;
                m_socketControl[tcpModel.Id].Run(tcpModel, ControlType.SocketSend);

                m_DelOutPutLog(string.Format("[{0}]Send:{1}", tcpModel.Name, content));
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// TCP接收数据
        /// </summary>
        /// <param name="tcpModel">TCP的Model</param>
        public string TCPRecive(TCPIPModel tcpModel)
        {
            try
            {
                BaseResultModel resultModel = m_socketControl[tcpModel.Id].Run(tcpModel, ControlType.SocketReceive);
                if (resultModel.RunResult)
                {
                    if (resultModel.ObjectResult != null && resultModel.ObjectResult.ToString() != "")
                    {
                        m_DelOutPutLog(string.Format("[{0}]Recv:{1}", tcpModel.Name, resultModel.ObjectResult.ToString()));

                        return resultModel.ObjectResult.ToString();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return "";
            }
        }

    
        #endregion

        #region 料盘操作
        /// <summary>
        /// 获取料盘排列
        /// </summary>
        public void GetBlowingList()
        {
            try
            {
                m_listCosResult = m_CosModel.listBlowType1;
            }
            catch (Exception ex)
            {

            }
        }
        
        /// <summary>
        /// 设置下料的料盘状态
        /// </summary>
        /// <param name="cosResult">物料测试结果</param>
        private void SetUnLoadStatus(EnumCosResult cosResult)
        {
            try
            {
                List<int> listIndex = new List<int>();
                for (int i = 0; i < m_listCosResult.Count; i++)
                {
                    if (m_listCosResult[i] == cosResult)
                    {
                        listIndex.Add(i + 1);
                    }
                }
                UnLoadModel unLoadModel = GetUnLoadModel(cosResult);

                int trayCurrentNum = unLoadModel.TrayCurrentNum;
                int productCurrentRow = unLoadModel.ProductCurrentRow;
                int productCurrentCol = unLoadModel.ProductCurrentCol;

                //设置物料当前列
                if (productCurrentCol < m_unLoadTrayModel.ProductColCount)
                {
                    productCurrentCol++;
                    unLoadModel.ProductCurrentCol = productCurrentCol;
                }
                else
                {
                    productCurrentRow++;
                    unLoadModel.ProductCurrentRow = productCurrentRow;
                    unLoadModel.ProductCurrentCol = 1;
                }

                //判断物料放到最后一行
                if (productCurrentRow > m_unLoadTrayModel.ProductRowCount)
                {
                    //设置从第一个开始放
                    unLoadModel.ProductCurrentRow = 1;
                    unLoadModel.ProductCurrentCol = 1;

                    int current = listIndex.FindLastIndex(x => x == trayCurrentNum);
                    //设置料盘当前列
                    if (current < listIndex.Count - 1)
                    {
                        current++;
                        unLoadModel.TrayCurrentNum = listIndex[current];
                    }
                    else
                    {
                        m_UnLoadProductDone = true;
                        unLoadModel.TrayCurrentNum = listIndex[0];
                        m_unLoadDoneModel = unLoadModel;
                    }
                }

                m_DelRefreshUnLoadParam();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取下料盘Model
        /// </summary>
        /// <param name="cosResult">测试结果类型</param>
        /// <returns></returns>
        private UnLoadModel GetUnLoadModel(EnumCosResult cosResult)
        {
            UnLoadModel unLoadModel = new UnLoadModel();
            try
            {
                switch (cosResult)
                {
                    case EnumCosResult.Enum_Pass:
                        unLoadModel = m_unLoadTrayModel.PassModel;
                        break;
                    case EnumCosResult.Enum_Fail:
                        unLoadModel = m_unLoadTrayModel.FailModel;
                        break;            
                    case EnumCosResult.Enum_SeemNG:
                        unLoadModel = m_unLoadTrayModel.SeemNGModel;
                        break;
                   
                    default:
                        break;
                }
                return unLoadModel;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取下料盘的当前行列
        /// </summary>
        /// <param name="currentNum">当前编号</param>
        /// <param name="row">输出行</param>
        /// <param name="col">输出列</param>
        private void GetUnLoadRowCol(int currentNum, out int row, out int col)
        {
            try
            {
                row = (currentNum - 1) / 4 + 1;
                col = (currentNum - 1) % 4 + 1;
            }
            catch (Exception ex)
            {
                row = -1;
                col = -1;
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 获取下料轴位置
        /// </summary>
        /// <param name="cosResult">结果</param>
        /// <param name="index">3：吸嘴3 1：吸嘴1</param>
        /// <param name="xValue">输出x坐标</param>
        /// <param name="yValue">输出ys坐标</param>
        private void GetUnLoadTrayPos(EnumCosResult cosResult, ESuck eSuck, ref double xValue, ref double yValue)
        {
            try
            {
                int trayCurrentRow, trayCurrentCol;
                UnLoadModel unLoadModel = GetUnLoadModel(cosResult);
                GetUnLoadRowCol(unLoadModel.TrayCurrentNum, out trayCurrentRow, out trayCurrentCol);

                int productCurrentRow = unLoadModel.ProductCurrentRow;
                int productCurrentCol = unLoadModel.ProductCurrentCol;

                //获取吸嘴下料的起始示教位置
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                
                string strTeach = string.Format("{0}下料示教位置{1}", eSuck.ToString(), unLoadModel.TrayCurrentNum);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == strTeach);
                
                //物料位置
                double xproductdis = (productCurrentCol - 1) * m_unLoadTrayModel.ProductColDis;
                double yproductdis = (productCurrentRow - 1) * m_unLoadTrayModel.ProductRowDis;
                
                //某个料盘里面物料
                xValue = Math.Round(pointModel.Pos_X + xproductdis, 3);
                yValue = Math.Round(pointModel.Pos_Y - yproductdis, 3);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
          
        #endregion
        
        #region DDR转盘操作

        /// <summary>
        /// CKD回零
        /// </summary>
        /// <returns>返回结果</returns>
        private BaseResultModel CKDHome()
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
               
                Thread.Sleep(1000);

                //resultModel = CKDWaitMoveFinish(MotionParam.DI_CKDInPlace);

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.Message;
                resultModel.RunResult = false;
                return resultModel;
            }
        }

        /// <summary>
        /// CKD运动
        /// </summary>
        /// <param name="ioName">运行程序IO</param>
        /// <param name="runName">启动IO</param>
        /// <param name="inName">到位IO</param>
        /// <returns></returns>
        private BaseResultModel CKDMove(string ioName, string runName, string inName)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                //CKD程序选择
                Thread.Sleep(100);
                if (ioName != "")
                {
                    IOModel ioModel = GetIOModel(ioName, 1);
                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                    if (!resultModel.RunResult)
                    {
                        return resultModel;
                    }
                }
                Thread.Sleep(100);
                //CKD启动
                IOModel runModel = GetIOModel(runName, 1);
                resultModel = m_MotroContorl.Run(runModel, MotorControlType.IOTrigger);
                if (!resultModel.RunResult)
                {
                    return resultModel;
                }

                Thread.Sleep(1200);
                //resultModel = CKDWaitMoveFinish(inName);

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }

        /// <summary>
        /// 等待CKD到位完成
        /// </summary>
        /// <param name="inName">到位IO</param>
        /// <returns>返回结果</returns>
        private BaseResultModel CKDWaitMoveFinish(string inName)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                //检测CKD是否运行到位
                Stopwatch sp = new Stopwatch();
                sp.Start();
                while (!Global.Break)
                {
                    if (sp.ElapsedMilliseconds > 15000)
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "CKD到位超时";
                        return resultModel;
                    }

                    //检测到位信号
                    IOModel inModel = GetIOModel(inName, 1);
                    resultModel = m_MotroContorl.Run(inModel, MotorControlType.QueryDI);
                    if (!resultModel.RunResult)
                    {
                        return resultModel;
                    }
                    else
                    {
                        int ivalue = Int32.Parse(resultModel.ObjectResult.ToString());
                        if (ivalue == 1)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(10);
                }

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }
        #endregion
        
        #region 通用方法

        /// <summary>
        /// 获取配置数据
        /// </summary>
        public void LoadData(SequenceModel sequence)
        {
            try
            {
                //设置当前参数
                m_CosModel = sequence.LDModel;

                m_TrayModel = m_CosModel.TrayModelInch2;

                m_unLoadTrayModel = m_CosModel.unLoadTrayModel;

                m_paramRangeModel = m_CosModel.paramRangeModels.FirstOrDefault(x => x.Name == XmlControl.sequenceModelNew.ProductInfo);
                if(m_paramRangeModel == null)
                {
                    m_DelOutPutLog("当前产品配置为空,请检查!", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 获取配置的生产数据
        /// </summary>
        public void GetData()
        {
            try
            {
                ProductDataModel model = XmlControl.sequenceModelNew.productDataModel;
                m_LoadCount = model.LoadCount;
                m_UnLoadCount = model.UnLoadCount;
                m_OKCount = model.OKCount;
                m_NGCount = model.NGCount;
                m_SeemNGCount = model.SeemNGCount;
                m_ScanNGCount = model.ScanNGCount;
                m_CostTime = model.CostTime;
                m_PauseTime = model.PauseTime;
                m_WarnTime = model.WarnTime;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public string GetSelectInfo(SequenceModel sequence)
        {
            try
            {
                string str = string.Format("请确认 产品型号：{0} 正确请点击确认按钮，否则点击取消？", sequence.ProductInfo);

                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
         
        /// <summary>
        /// 设置测试数据
        /// </summary>
        public void SetTestData()
        {
            try
            {
                if (!m_stopWatch.IsRunning && !m_pauseWatch.IsRunning && !m_warnWatch.IsRunning)
                {
                    return;
                }
                long dsecond = m_stopWatch.ElapsedMilliseconds / 1000;
                double costTime = m_CostTime + dsecond; 

                dsecond = m_pauseWatch.ElapsedMilliseconds / 1000;
                double pauseTime = m_PauseTime + dsecond; 

                dsecond = m_warnWatch.ElapsedMilliseconds / 1000;
                double warnTime = m_WarnTime + dsecond;

                m_DelGetTestData(m_LoadCount, m_UnLoadCount, m_OKCount, m_NGCount, m_ScanNGCount, m_SeemNGCount, costTime, pauseTime, warnTime);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        /// <param name="isWarn">是否为报警暂停</param>
        public void PauseTiming(bool isWarn = false)
        {
            try
            {
                //测试计时
                if (m_stopWatch.IsRunning)
                {
                    m_stopWatch.Stop(); 
                }

                //暂停计时
                if (!m_pauseWatch.IsRunning && !isWarn)
                {
                    m_pauseWatch.Start();
                }

                //报警计时
                if (!m_warnWatch.IsRunning && isWarn)
                {
                    m_warnWatch.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 重新启动计时
        /// </summary>
        public void RestoreTiming()
        {
            try
            {
                //测试计时
                if (!m_stopWatch.IsRunning)
                {
                    m_stopWatch.Start();
                }

                //暂停计时
                if (m_pauseWatch.IsRunning)
                {
                    m_pauseWatch.Stop();
                }

                //报警计时
                if (m_warnWatch.IsRunning)
                {
                    m_warnWatch.Stop();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void StopTiming()
        {
            try
            {
                m_stopWatch.Stop(); 

                if(!m_pauseWatch.IsRunning)
                {
                    m_pauseWatch.Start();
                }

                m_warnWatch.Stop(); 
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 重启计时
        /// </summary>
        public void RestartTiming()
        {
            try
            {
                if (Global.Run && !Global.Pause)
                {
                    if(m_stopWatch.IsRunning)
                    {
                        m_stopWatch.Restart();
                    }
                    if(m_warnWatch.IsRunning)
                    {
                        m_warnWatch.Restart();
                    }
                    if(m_pauseWatch.IsRunning)
                    {
                        m_pauseWatch.Restart();
                    }
                }
                else
                {
                    m_stopWatch.Reset();
                    m_warnWatch.Reset();
                    m_pauseWatch.Reset();
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 保存数据到csv
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="strTitle">文件头内容</param>
        /// <param name="dataStr">数据</param>
        public void SaveCSV(string filePath, string strTitle, string dataStr)
        {
            try
            {
                //目录不存在则创建
                string path = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                StreamWriter fileWriter;
                if (!File.Exists(filePath))
                {
                    fileWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312"));//TRUE 存在则添加，不存在则新建 
                    fileWriter.Write(strTitle + "\r\n");//时间字段名

                    if ("" != dataStr)
                    {
                        fileWriter.Write(dataStr);
                    }
                }
                else
                {
                    fileWriter = new StreamWriter(filePath, true, Encoding.GetEncoding("gb2312"));//TRUE 存在则添加，不存在则新建 
                    fileWriter.Write(dataStr);
                }

                fileWriter.Flush();
                fileWriter.Close();
                 
                //保存生产数据文件
                WriteProductData();
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 保存数据到csv
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="strTitle">文件头内容</param>
        /// <param name="dataStr">数据</param>
        public void SaveDataCSV(string filePath, string strTitle, string dataStr)
        {
            try
            {
                //目录不存在则创建
                string path = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                StreamWriter fileWriter;
                fileWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312"));//TRUE 存在则添加，不存在则新建 
                fileWriter.Write(strTitle + "\r\n");//时间字段名

                if ("" != dataStr)
                {
                    fileWriter.Write(dataStr);
                }
                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
        
        /// <summary>
        /// 保存测试数据
        /// </summary>
        /// <param name="charResult">测试结果</param>
        /// <param name="strOcr">OCR数据</param>
        private void WriteTestData(CharResultModel charResult, LIVResultModel livResult, string strOcr, EnumCosResult eresult, int stationindex = 1, string qr1 = "", string qr2 = "", string coldWave = "")
        {
            try
            {
                lock (this)
                {
                    string strTitle = "时间,机台ID,操作员,工位,OCR,结果,SN1,SN2,压力值,TE_P,TM_P,Pop,Iop,Ith,Polarization_Average,Polarization_Iop,Rs,SE,SE_Max,VIop,Von,WPE_Iop,Center,FW95,FWHM,Iop,Peak,ColdWave,Threshold_Watt,FW95_X,FW95_Y,FWHM_X,FWHM_Y,Iop";
                    string dataStr = DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff") + "," + m_CosModel.DeviceID + "," + m_CosModel.OperatorID + "," + stationindex.ToString() + ","
                         + strOcr + "," + eresult.ToString() + "," + qr1 + "," + qr2 + "," + "0" + "," + livResult.LDPower_TE + "," + livResult.LDPower_TM
                          + "," + charResult.piv_Pop + "," + charResult.piv_Iop + "," + charResult.piv_Ith + "," + charResult.piv_Plarization_Average + "," + charResult.piv_Polarization_Iop
                         + "," + charResult.piv_Rs + "," + charResult.piv_SE + "," + charResult.piv_SE_Max + "," + charResult.piv_VIop + "," + charResult.piv_Von + "," + charResult.piv_WPE_Iop
                          + "," + charResult.spectrum_Center + "," + charResult.spectrum_FW95 + "," + charResult.spectrum_FWHM + "," + charResult.spect_rum_Iop_Spect + "," + charResult.spectrum_Peak + "," + coldWave + "," + charResult.threshold_watt
                           + "," + charResult.ffp_FW95_X + "," + charResult.ffp_FW95_Y + "," + charResult.ffp_FWHM_X + "," + charResult.ffp_FWHM_Y + "," + charResult.ffp_Iop + "\r\n";

                    string str = m_paramRangeModel.Name;
                    string path = Global.DataPath + str + "//";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    SaveCSV(path + DateTime.Now.ToString("yyyyMMdd") + ".csv", strTitle, dataStr); 
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 保存生产数据
        /// </summary>
        public void WriteProductData()
        {
            try
            {
                string path = Global.DataPath + "生产数据//";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                double dOkPercent = 0;
                if (m_sequenceModel.productDataModel.LoadCount != 0)
                {
                    dOkPercent = (double)m_sequenceModel.productDataModel.OKCount / m_sequenceModel.productDataModel.LoadCount;
                    dOkPercent = Math.Round(dOkPercent, 2);
                }

                string strTitle = "生产总数,OK数,NG数,CT,生产时间,良率,报警时间,暂停时间";
                string dataStr = m_sequenceModel.productDataModel.LoadCount + "," + m_sequenceModel.productDataModel.OKCount + ","
                    + m_sequenceModel.productDataModel.NGCount + "," + m_sequenceModel.productDataModel.CT + ","
                    + m_sequenceModel.productDataModel.CostTime + "," + dOkPercent.ToString() + ","
                    + m_sequenceModel.productDataModel.WarnTime + "," + m_sequenceModel.productDataModel.PauseTime;

                SaveDataCSV(path + DateTime.Now.ToString("yyyyMMdd") + "_数据.csv", strTitle, dataStr);
            }
            catch (Exception ex)
            {

            }
        }
         
        /// <summary>
        /// 获取点位数据
        /// </summary>
        /// <param name="stationName">工站名</param>
        /// <param name="pointName">点位名</param>
        /// <returns></returns>
        public PointModel GetPointModel(string stationName, string pointName)
        {
            try
            {
                var stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == stationName);
                if (stationModel == null)
                {
                    m_DelOutPutLog(string.Format("不存在工站【{0}】", stationName), LogLevel.Error);
                    return null;
                }

                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == pointName);
                if (stationModel == null)
                {
                    m_DelOutPutLog(string.Format("不存在点位【{0}】", pointName), LogLevel.Error);
                    return null;
                }

                return pointModel;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取IO Model
        /// </summary>
        /// <param name="ioName">IO名称</param>
        /// <param name="value">写入值 1/0</param>
        /// <returns></returns>
        public IOModel GetIOModel(string ioName, ushort value)
        {
            try
            {
                IOModel ioModel = XmlControl.controlCardModel.IOModels.FirstOrDefault(x => x.Name == ioName);
                if (ioModel == null)
                {
                    m_DelOutPutLog(string.Format("不存在IO【{0}】", ioName), LogLevel.Error);
                    return null;
                }

                ioModel.val = value;

                return ioModel;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取组合IO Model
        /// </summary>
        /// <param name="ioName">输出IO名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public RelatIoModel GetRelatIoModel(string ioName, ushort value)
        {
            try
            {
                RelatIoModel relatIo = XmlControl.controlCardModel.RelatIoModels.FirstOrDefault(x => x.OutIoModel.Name == ioName);
                if (relatIo == null)
                {
                    m_DelOutPutLog(string.Format("不存在组合IO【{0}】", ioName), LogLevel.Error);
                    return null;
                }
                relatIo.TimeOut = m_CosModel.VacuumTimeOut;
                relatIo.IsWaitTimeOut = !m_CosModel.IsShieldCylinderUp;

                relatIo.OutIoModel.val = value;

                return relatIo;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 停止所有轴
        /// </summary>
        public void StopAllAxis()
        {
            try
            {
                m_MotroContorl.Run(null, MotorControlType.AllStop);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
        
        #endregion
    }
}