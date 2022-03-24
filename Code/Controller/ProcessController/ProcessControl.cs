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
using ExcelController;

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

        public delegate void Del_RefreshMap();
        /// <summary>
        /// 刷新Map数据
        /// </summary>
        public Del_RefreshMap m_DelRefreshMap;

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
        List<EnumLDResult> m_listCosResult = new List<EnumLDResult>();

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
        /// 下料料盘是否已放满
        /// </summary>
        bool m_UnLoadProductDone = false;
             
        /// <summary>
        /// 执行算法实例
        /// </summary>
        IAlgorithmControl m_algorithmControll = new AlgorithmControl();
        IAlgorithmControl m_algorithmControl = new LDAlgorithmControl();

        ParamRangeModel m_paramRangeModel = new ParamRangeModel();

        /// <summary>
        /// 小视野推算Bar失败次数
        /// </summary>
        int m_iFixedBarNG = 0;

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
        /// 当前剩余产品行数
        /// </summary>
        int m_SurplusRowCount = 0;

        /// <summary>
        /// 产品总行数
        /// </summary>
        int m_LDRowCount = 0;

        /// <summary>
        /// 当前行需取料总数
        /// </summary>
        int m_NeedGetCount = 0;
        
        /// <summary>
        /// 存储Bar条数据容器
        /// </summary>
        private Dictionary<int, List<Bar>> m_DicBar = new Dictionary<int, List<Bar>>();

        /// <summary>
        /// 当前Bar条的序号从1开始
        /// </summary>
        private int m_barIndex = 1;

        //操作Excel实例
        OleExcel m_oleExcel = new OleExcel();

        /// <summary>
        /// 小视野入料中心点位
        /// </summary>
        private List<PointClass> m_ListPoint = new List<PointClass>();

        /// <summary>
        /// 当前行需要取的料
        /// </summary>
        private List<Product> m_listProduct = new List<Product>();

        /// <summary>
        /// 产品相隔的间距
        /// </summary>
        double m_proDistance = 0;

        /// <summary>
        /// LD测试结果
        /// </summary>
        EnumLDResult m_LdResult = EnumLDResult.Enum_Fail;

        SmallFixedPosResultModel m_fixedResult = new SmallFixedPosResultModel();

        /// <summary>
        /// 是否手动换排
        /// </summary>
        bool m_isChangeRow = false;

        /// <summary>
        /// 是否继续上次产品检测
        /// </summary>
        public bool m_isContinueLoad = false;

        /// <summary>
        /// 是否是第一次运行
        /// </summary>
        bool m_isFirstRun = true;
        bool m_isFirstRun_Check = true;

        /// <summary>
        /// 视野里面判断不存在产品的次数
        /// </summary>
        int m_iNoProduct = 0;

        /// <summary>
        /// Wafer号
        /// </summary>
        string m_sWaferNo = "";

        /// <summary>
        /// 产品结果队列
        /// </summary>
        Queue<QResultModel> m_QResult = new Queue<QResultModel>();
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

                //检查Map表
                m_barIndex = 1;
                if (!CheckMap())
                {
                    m_DelOutPutLog("请录入Map表", LogLevel.Error);
                    return false;
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
                    bInit = InitRunStatus();
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
                m_isChangeRow = false;
                m_iFixedBarNG = 0;
                m_SurplusRowCount = 0;
                m_NeedGetCount = 0;
                m_proDistance = 800;
                m_iNoProduct = 0;
                m_isFirstRun = true;
                m_isFirstRun_Check = true;
                m_QResult.Clear();

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

            if (this.m_SmallThread == null || !this.m_SmallThread.IsAlive)
            {
                m_SmallThread = new Thread(SmallGetThread);
                m_SmallThread.Start();
            }

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
                Autostep = AutoRunStep.StepLoadZMoveSafePos;

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
                            case AutoRunStep.StepLoadZMoveSafePos://上料Z移动到安全位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepLoadXMoveSafePos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepLoadXMoveSafePos://上料X移动到安全位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        Autostep = AutoRunStep.StepSuckFilmOff;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepSuckFilmOff://吸膜破真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetSuctionVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    Thread.Sleep(m_CosModel.UpVacuumBreakDelay);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30001);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepUpMoveSafePos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepUpMoveSafePos://顶升模组到安全位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    if(!JudgeUpZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40001);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_GetTray);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10002);
                                        break;
                                    }
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderUnClamp, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10002);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepCheckMap;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepCheckMap://检查Map表是否录入
                                {
                                    if (!CheckMap())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_10006);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepWaitLoadDone;
                                    }

                                    break;
                                }

                            case AutoRunStep.StepWaitLoadDone://等待员工上料完成
                                {
                                    AutoRunNG(null, "");
                                    m_DelMessageBox("请上料!!", 1);
                                    Autostep = AutoRunStep.StepGetCylinderClamp;
                                    break;
                                }

                            case AutoRunStep.StepGetCylinderClamp://取产品位气缸夹紧
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderUnClamp, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10002);
                                        break;
                                    }
                                    ioModel = GetIOModel(MotionParam.DO_GetCylinderClamp, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10002);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepCheckClamp;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepCheckClamp://检测气缸夹紧到位
                                {
                                    ioModel = GetIOModel(MotionParam.DI_Get1CylinderClamp, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.QueryDITime);
                                    int ivalue = Int32.Parse(resultModel.ObjectResult.ToString());
                                    ioModel = GetIOModel(MotionParam.DI_Get2CylinderClamp, 1);
                                    var resultModel2 = m_MotroContorl.Run(ioModel, MotorControlType.QueryDITime);
                                    int ivalue2 = Int32.Parse(resultModel2.ObjectResult.ToString());
                                    if (!m_CosModel.IsShieldClamp)
                                    {
                                        if (ivalue != 1 || ivalue2 != 1)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10005);
                                        }
                                        else
                                        {
                                            Autostep = AutoRunStep.StepXYMoveBigCameraPos;
                                        }
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepXYMoveBigCameraPos;
                                    }
                                    break;
                                }

                            case AutoRunStep.StepXYMoveBigCameraPos://XY移动到大视野拍照位置
                                {
                                    if (!JudgeUpZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40001);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    if (!JudgeLoadXSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40002);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_Camera, MotionParam.Pos_BigCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    resultModel = AlgorithmRun(0, 0);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20001);
                                    }
                                    else
                                    {
                                        Autostep = AutoRunStep.StepXYMoveBigOffSetPos;
                                    }
                                    break;
                                }
                              
                            case AutoRunStep.StepXYMoveBigOffSetPos://XYR移动大视野补偿位
                                {
                                    if (!JudgeUpZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40001);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCameraOffSet);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    resultModel = AlgorithmRun(0, 1);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20002);
                                    }
                                    else
                                    {
                                        m_LDRowCount = m_ListPoint.Count;
                                        //如果是根据上次运行则去掉不取行
                                        SetContinueLoad();
                                        m_SurplusRowCount = m_ListPoint.Count;

                                        m_ReadySmall = true;
                                        m_SmallGetDone = false;

                                        //赋值Wafer号
                                        if (m_CosModel.mapModel.ListMap.Count > m_barIndex - 1)
                                        {
                                            m_sWaferNo = m_CosModel.mapModel.ListMap[m_barIndex - 1].WaferNo;
                                        }

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
                                        RemoveBar();
                                        Autostep = AutoRunStep.StepLoadZMoveSafePos;
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
                            Thread.Sleep(m_CosModel.BuzzerTime);
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
                                        if (m_SurplusRowCount > 0)
                                        {
                                            m_SurplusRowCount--;
                                            m_DelOutPutLog("剩余行数：" + m_SurplusRowCount.ToString());
                                            SmallStep = SmallRunStep.StepSuckFilmOff;
                                        }
                                        else
                                        {
                                            if(m_IsSmallCanSnap)
                                            {
                                                m_ReadySmall = false;
                                                m_SmallGetDone = true;
                                            }
                                            continue;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSuckFilmOff://吸膜破真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_GetSuctionVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    Thread.Sleep(m_CosModel.UpVacuumBreakDelay);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30001);
                                    }
                                    else
                                    {
                                        SmallStep = SmallRunStep.StepMoveSuckSafePos;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepMoveSuckSafePos://顶升模组到安全位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepXYMoveSmallCenterPos;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYMoveSmallCenterPos://XYR移动小视野中心位置
                                {
                                    if (!JudgeUpZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40001);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);
                                    pointModel.Pos_X = m_ListPoint[0].X;
                                    pointModel.Pos_Y = m_ListPoint[0].Y;
                                    pointModel.Pos_Z = m_ListPoint[0].U;
                                    m_ListPoint.RemoveAt(0);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    if (!JudgeLoadXSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40002);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_Camera, MotionParam.Pos_SmallCamera);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30001);
                                    }
                                    else
                                    {
                                        SmallStep = SmallRunStep.StepSmallJudge;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallJudge://小视野入料判断--推算Bar条
                                {
                                    resultModel = AlgorithmRun(1, 2);
                                    SmallJudgePosResultModel fixedResult = resultModel as SmallJudgePosResultModel;
                                    if (!fixedResult.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20003);
                                    }
                                    else
                                    {
                                        if (fixedResult.IsCenterPos)
                                        {
                                            SmallStep = SmallRunStep.StepSmallCameraSnapBar;
                                        }
                                        else
                                        {
                                            SmallStep = SmallRunStep.StepXYMoveBarJudgePos;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYMoveBarJudgePos://XYR移动到Bar条判定位置
                                {
                                    //视野里面连续无产品
                                    if(m_iNoProduct > 5)
                                    {
                                        m_iNoProduct = 0;
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_10007);
                                        break;
                                    }
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);

                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepSmallJudge;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallCameraSnapBar://小视野相机拍照推算Bar条号
                                {
                                    resultModel = AlgorithmRun(1, 4);
                                    SmallFixedPosResultModel fixedResult = resultModel as SmallFixedPosResultModel;
                                    if(fixedResult.Bar == "-9999")//未确认Bar成功
                                    {
                                        m_iFixedBarNG++;
                                        if(m_iFixedBarNG > 3)//大于3次直接弹框确认
                                        {
                                            string outBar = "";
                                            InputOCR("-9999", ref outBar);

                                            int iBar = Int32.Parse(outBar.Substring(0, 2));
                                            fixedResult.FirstOcr = outBar;
                                            bool bresult = SetBar(fixedResult, 5472, 3648, iBar);
                                            if(bresult)
                                            {
                                                m_iFixedBarNG = 0;
                                                SmallStep = SmallRunStep.StepWaitLoadDone;
                                            }
                                            else//Bar是否存在
                                            {
                                                AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20003);
                                            }
                                        }
                                        else//往后移动3个产品位
                                        {
                                            SmallStep = SmallRunStep.StepBarNGMovePos;
                                        }
                                    }
                                    else
                                    {
                                        if (!fixedResult.RunResult)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20003);
                                        }
                                        else
                                        {
                                            m_iFixedBarNG = 0;
                                            SmallStep = SmallRunStep.StepWaitLoadDone;
                                        }
                                    }

                                    break;
                                }

                            case SmallRunStep.StepBarNGMovePos://推算Bar条号重新移动位置
                                {
                                    m_DelOutPutLog("Bar条推算失败，继续执行下一个!");
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);
                                    //bar条推算失败则往后移动3位 
                                    double distance = m_proDistance > 800 || m_proDistance < 500 ? 800 : m_proDistance;
                                    pointModel.Pos_X = Math.Round(pointModel.Pos_X - distance * 3, 2);

                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepSmallCameraSnapBar; 
                                    }
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
                                        //是否需要手动换行
                                        if(m_isChangeRow)
                                        {
                                            m_isChangeRow = false;
                                            m_ReadyLoad = false;
                                            SmallStep = SmallRunStep.StepWaitBigDone;
                                        }
                                        else
                                        {
                                            SetMapValue();
                                            SmallStep = SmallRunStep.StepXYMoveSmallGetPos;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYMoveSmallGetPos://XYR移动到拍照抓取位置
                                {
                                    //视野里面连续无产品
                                    if (m_iNoProduct > 5)
                                    {
                                        m_iNoProduct = 0;
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_10007);
                                        break;
                                    }
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);

                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        SmallStep = SmallRunStep.StepSmallJudge_2;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallJudge_2://小视野入料判断2
                                {
                                    resultModel = AlgorithmRun(1, 3);
                                    SmallJudgePosResultModel fixedResult = resultModel as SmallJudgePosResultModel;
                                    if (!fixedResult.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20003);
                                    }
                                    else
                                    {
                                        if(fixedResult.IsCenterPos)
                                        {
                                            SmallStep = SmallRunStep.StepSmallCameraSnapOCR;
                                        }
                                        else
                                        {
                                            Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                            SmallStep = SmallRunStep.StepXYMoveSmallGetPos;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallCameraSnapOCR://小视野相机定位产品OCR
                                {
                                    resultModel = AlgorithmRun(1, 5);

                                    if (!m_fixedResult.RunResult)//如果执行算法失败则OCR弹框
                                    {
                                        string outOcr = "";
                                        AutoRunNG(resultModel, strStepDesc);
                                        bool bresult = InputOCR(m_fixedResult.NeedOcr, ref outOcr);
                                        if(!bresult)//点击取消之后直接跳过改产品
                                        {
                                            SetExcelColor(m_listProduct[m_listProduct.Count - m_NeedGetCount]);
                                            m_NeedGetCount--;
                                            if (m_NeedGetCount > 0)
                                            {
                                                int index = m_listProduct.Count - m_NeedGetCount;
                                                GetNextSmallPos(m_listProduct[index].productOcr, m_listProduct[index - 1].productOcr);
                                                SmallStep = SmallRunStep.StepWaitLoadDone;
                                                m_DelOutPutLog(string.Format("剩余行：{0} 剩余产品: {1}", m_SurplusRowCount, m_NeedGetCount));
                                            }
                                            else
                                            {
                                                SmallStep = SmallRunStep.StepWaitBigDone;
                                            }
                                        }
                                        else//输入正确OCR则重新设置拍照位置
                                        {
                                            GetNextSmallPos(m_fixedResult.NeedOcr, outOcr);
                                            SmallStep = SmallRunStep.StepWaitLoadDone;
                                        }
                                    }
                                    else
                                    {
                                        //如果物料可以取
                                        if (m_fixedResult.ICanGet == 1)
                                        {
                                            SmallStep = SmallRunStep.StepXYRMoveSuckPos;
                                        }
                                        else
                                        {
                                            m_NeedGetCount--;
                                            SmallStep = m_NeedGetCount > 0 ? SmallRunStep.StepWaitLoadDone : SmallRunStep.StepWaitBigDone;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepXYRMoveSuckPos://XYR移动到顶针上方位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_ProFixed); 
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        SmallStep = SmallRunStep.StepAlgorithmSmall;
                                    }
                                    break;
                                }

                            case SmallRunStep.StepAlgorithmSmall://小视野拍照检测缺陷
                                {
                                    resultModel = AlgorithmRun(1, 6);
                                    if (!resultModel.RunResult && string.IsNullOrEmpty(resultModel.ErrorMessage))
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20010);
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
                                            m_DelOutPutLog(string.Format("剩余行：{0} 剩余产品: {1}", m_SurplusRowCount, m_NeedGetCount));
                                        }
                                        else
                                        {
                                            SmallStep = SmallRunStep.StepSmallGetDone;
                                        }
                                    }
                                    break;
                                }

                            case SmallRunStep.StepSmallGetDone://小视野拍照取完判断
                                {
                                    //等待最后一个物料取完
                                    if(m_IsSmallCanSnap)
                                    {
                                        SmallStep = SmallRunStep.StepWaitBigDone;
                                    }
                                    else
                                    {
                                        Thread.Sleep(50);
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
                LoadStep = LoadRunStep.StepLoadZMoveSafePos;
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
                            case LoadRunStep.StepLoadZMoveSafePos://上料模组Z移动到安全位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove); 
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadXMoveWaitPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadXMoveWaitPos://上料模组X移动到等待位置
                                {
                                    if (!JudgeLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepWaitReadLoad;
                                        if(m_isFirstRun)
                                        {
                                            m_IsSmallCanSnap = true;
                                            m_isFirstRun = false;
                                        }
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
                                        m_IsSmallCanSnap = false;
                                        m_ReadyLoad = false;
                                        LoadStep = LoadRunStep.StepLoadXMoveTakePos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadXMoveTakePos://上料模组X移动到上料位置
                                {
                                    if (!JudgeCameraSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40003);
                                        break;
                                    }

                                    if (!JudgeLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Load);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadZMoveTakePos;
                                    }

                                    break;
                                }

                            case LoadRunStep.StepLoadZMoveTakePos://上料模组Z移动到上料位置
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Load);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    //pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_ThimbleRise);
                                    pointModel = GetPointModel(MotionParam.Station_Up, MotionParam.Pos_ThimbleDown);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    ioModel = GetIOModel(MotionParam.DO_LoadBreakVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    ioModel = GetIOModel(MotionParam.DO_LoadSuctionVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    Thread.Sleep(m_CosModel.VacuumDelayTime);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30002);
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadZMoveSafePos_1;
                                        m_LoadCount++;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadZMoveSafePos_1://上料模组Z移动到安全位置1
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadXMoveNSnapPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadXMoveNSnapPos://上料模组X移动N面拍照位
                                {
                                    if (!JudgeLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                        break;
                                    }
                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_NSnap);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove); 
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadZMoveNSnapPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadZMoveNSnapPos://上料模组Z移动N面拍照位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_NSnap);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        m_IsSmallCanSnap = true;
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepNSnap;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepNSnap://N面拍照执行算法
                                {
                                    resultModel = AlgorithmRun(2, 7);
                                    if (!resultModel.RunResult && string.IsNullOrEmpty(resultModel.ErrorMessage))
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20005);
                                    }
                                    else
                                    {
                                        LoadStep = LoadRunStep.StepMoveUnLoadWaitPos;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepMoveUnLoadWaitPos://移动到下料等待位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    //pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Safe);
                                    //resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                        LoadStep = LoadRunStep.StepLoadXMoveDDR;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadXMoveDDR://上料模组X移动到DDR马达
                                {
                                    if (!JudgeUnLoadXSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40004);
                                        break;
                                    }

                                    if (!JudgeNCameraLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadZMoveDDR;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadZMoveDDR://上料模组Z移动到DDR马达
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    ioModel = GetIOModel(MotionParam.DO_LoadSuctionVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    ioModel = GetIOModel(MotionParam.DO_LoadBreakVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10003);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumBreakDelay);
                                        LoadStep = LoadRunStep.StepLoadZMoveSafePos_2;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadZMoveSafePos_2://上料模组Z移动到安全位2
                                {
                                    pointModel = GetPointModel(MotionParam.Station_LoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        LoadStep = LoadRunStep.StepLoadXMoveWaitPos_2;
                                    }
                                    break;
                                }

                            case LoadRunStep.StepLoadXMoveWaitPos_2://移动到上料等待位2
                                {
                                    if (!JudgeLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                        break;
                                    }
                                    //判断是否可以上料
                                    if(!m_ReadyLoad)
                                    {
                                        pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Safe);
                                        resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                        if (!resultModel.RunResult)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                        }
                                        else
                                        {
                                            Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                            LoadStep = LoadRunStep.StepLoadZMoveSafePos;
                                            m_ReadyDDRCheck = true;//可以来取DDR物料
                                        }
                                    }
                                    else//如果可以上料则直接运动到上料位置
                                    {
                                        if (!JudgeCameraSafe())
                                        {
                                            AutoRunNG(null, strStepDesc, (int)EAlarm.A_40003);
                                            break;
                                        }

                                        if (!JudgeLoadZSafe())
                                        {
                                            AutoRunNG(null, strStepDesc, (int)EAlarm.A_40005);
                                            break;
                                        }

                                        pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Load);
                                        resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                        if (!resultModel.RunResult)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                        }
                                        else
                                        {
                                            Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                            LoadStep = LoadRunStep.StepWaitReadLoad;
                                            m_ReadyDDRCheck = true;//可以来取DDR物料
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
                CheckStep = CheckRunStep.StepUnLoadZMoveSafePos;
                PointModel pointModel;
                RelatIoModel relatIoModel;
                IOModel ioModel;

                while (!Global.Break)
                {
                    if (!Global.Pause)
                    {
                        string strStepDesc = CommFunc.GetEnumDescription<CheckRunStep>(CheckStep);//获取当前步骤的描述 
                        m_DelShowStep(3, "检测下料模组", (int)CheckStep, strStepDesc);

                        switch (CheckStep)
                        {
                            case CheckRunStep.StepUnLoadZMoveSafePos://下料Z移动到下料安全位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadXYMoveSafePos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadXYMoveSafePos://下料X移动到下料安全位
                                {
                                    if(!JudgeUnLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40006);
                                        break;
                                    }
                                    

                                    StationModel station = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                                    resultModel = m_MotroContorl.Run(station.Axis_X, MotorControlType.AxisGetPosition);
                                    if(!resultModel.RunResult)
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_10001);
                                        break;
                                    }
                                    double dCurrentX = double.Parse(resultModel.ObjectResult.ToString()); 
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_Safe);
                                    if(dCurrentX > pointModel.Pos_X)
                                    {
                                        station.Axis_X.pos = pointModel.Pos_X;
                                        resultModel = m_MotroContorl.Run(station.Axis_X, MotorControlType.AxisMove);
                                    }
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepCheckMoveLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepCheckMoveLoadPos://检测模组移动到上料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Load);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepWaitReadyDDRCheck;

                                        if (m_isFirstRun_Check)
                                        {
                                            m_IsCanPutCheck = true;
                                            m_isFirstRun_Check = false;
                                        }
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
                                        m_IsCanPutCheck = false;
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    LightControl("B");
                                    resultModel = AlgorithmRun(3, 8);
                                    LightControl("B", false);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20006);
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    LightControl("E");
                                    resultModel = AlgrithmRun(4, ESuck.吸嘴1);
                                    LightControl("E", false);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20007);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepMoveCorrect2Pos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepMoveCorrect2Pos://检测模组移动到上料校正位置2
                                {
                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Correct2);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    LightControl("B");
                                    resultModel = AlgorithmRun(3, 9);
                                    LightControl("B", false);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20008);
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
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    LightControl("D");
                                    resultModel = AlgrithmRun(5, ESuck.吸嘴1);
                                    LightControl("D", false);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_20009);
                                    }
                                    else
                                    {
                                        CheckStep = CheckRunStep.StepCheckMoveUnLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepCheckMoveUnLoadPos://检测模组移动到下料位
                                {
                                    if (!GetUnLoadTrayPos())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_10004);
                                        break;
                                    }

                                    pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadMoveLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadMoveLoadPos://下料模组移动到取料位
                                {
                                    if (!JudgeLoadXSafe(false))
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40002);
                                        break;
                                    }

                                    if (!JudgeUnLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40006);
                                        break;
                                    }
                                     
                                    StationModel station = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_Take);
                                    station.Axis_X.pos = pointModel.Pos_X;
                                    resultModel = m_MotroContorl.Run(station.Axis_X, MotorControlType.AxisMove); 
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadZMoveLoadPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadZMoveLoadPos://下料模组Z移动到取料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_Take);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
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
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadBreakVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadSuctionVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    Thread.Sleep(m_CosModel.VacuumDelayTime);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30003);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumDelayTime);
                                        CheckStep = CheckRunStep.StepUnLoadZMoveSafePos1;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadZMoveSafePos1://下料Z移动到下料安全位1
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_Safe);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadMovePutPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadMovePutPos://下料模组移动到下料位
                                {
                                    if (m_UnLoadProductDone)//检查料盘是否已放满
                                    {
                                        AutoRunNG(null, "下料料盘换料");

                                        m_DelMessageBox(m_unLoadDoneModel.Name + " 下料盘已满，请更换料盘，点击确认运动到换料位置！", 3);

                                        //执行更换上料盘动作
                                        pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_Safe);
                                        resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                        if (!resultModel.RunResult)
                                        {
                                            AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                            continue;
                                        }
                                        else
                                        {
                                            CheckStep = CheckRunStep.StepChangeUnLoadTray;
                                        }

                                        continue;
                                    }

                                    if (!JudgeUnLoadZSafe())
                                    {
                                        AutoRunNG(null, strStepDesc, (int)EAlarm.A_40006);
                                        break;
                                    }
                                    pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        m_IsCanPutCheck = true;
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadZMovePutPos;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepUnLoadZMovePutPos://下料模组Z移动到下料位
                                {
                                    pointModel = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_UnLoad);
                                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_10001);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.AxisInPlaceDelay);
                                        CheckStep = CheckRunStep.StepUnLoadBreakVaccum;
                                    }
                                    break;
                                }

                            case CheckRunStep.StepChangeUnLoadTray://下料模组移动到换料位
                                {
                                    //更换完成请点击确认
                                    m_DelMessageBox(m_unLoadDoneModel.Name + " 下料盘请更换，更换完成后请点击确认！", 3);

                                    m_UnLoadProductDone = false;
                                    CheckStep = CheckRunStep.StepUnLoadMovePutPos;
                                    break;
                                }

                            case CheckRunStep.StepUnLoadBreakVaccum://下料吸嘴破真空
                                {
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadSuctionVacuum, 0);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    ioModel = GetIOModel(MotionParam.DO_UnLoadBreakVacuum, 1);
                                    resultModel = m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                                    Thread.Sleep(m_CosModel.VacuumBreakDelay);
                                    if (!resultModel.RunResult)
                                    {
                                        AutoRunNG(resultModel, strStepDesc, (int)EAlarm.A_30004);
                                    }
                                    else
                                    {
                                        Thread.Sleep(m_CosModel.VacuumDelayTime);
                                        CheckStep = CheckRunStep.StepUnLoadZMoveSafePos;

                                        SetUnLoadStatus(m_LdResult);
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

                    resultModel.RunResult = InitRunStatus();
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

                    //轴需要到安全位置
                    Thread.Sleep(300);
                    var pointModel = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_Safe);
                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMoveNotWait);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }

                    var pointModel_z = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_Safe);
                    resultModel = m_MotroContorl.Run(pointModel_z, MotorControlType.AxisMoveNotWait);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }

                    var pointModel_1 = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_Safe);
                    resultModel = m_MotroContorl.Run(pointModel_1, MotorControlType.AxisMoveNotWait);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }

                    var pointModel_2 = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Safe);
                    resultModel = m_MotroContorl.Run(pointModel_2, MotorControlType.AxisMoveNotWait);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }

                   var pointModel_3 = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_Safe);
                    resultModel = m_MotroContorl.Run(pointModel_3, MotorControlType.AxisMove);
                    if (!resultModel.RunResult)
                    {
                        m_DelOutPutLog(resultModel.ErrorMessage);
                        return false;
                    }

                    resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisWaitMoveDone);
                    resultModel = m_MotroContorl.Run(pointModel_z, MotorControlType.AxisWaitMoveDone);
                    resultModel = m_MotroContorl.Run(pointModel_1, MotorControlType.AxisWaitMoveDone);
                    resultModel = m_MotroContorl.Run(pointModel_2, MotorControlType.AxisWaitMoveDone);

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
                        //m_DelOutPutLog(item.Name + "复位失败", LogLevel.Error);
                        //bresult = false;
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
        public bool InitRunStatus()
        {
            try
            {
                BaseResultModel resultModel = new BaseResultModel();
                resultModel.RunResult = true;

                //取产品气缸张开
                var ioModel = GetIOModel(MotionParam.DO_GetCylinderUnClamp, 1);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger); 
                ioModel = GetIOModel(MotionParam.DO_GetCylinderClamp, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

                //关掉真空
                ioModel = GetIOModel(MotionParam.DO_CheckCCDVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

                ioModel = GetIOModel(MotionParam.DO_GetSuctionVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);
                Thread.Sleep(m_CosModel.UpVacuumBreakDelay);

                ioModel = GetIOModel(MotionParam.DO_LoadBreakVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

                ioModel = GetIOModel(MotionParam.DO_LoadSuctionVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

                ioModel = GetIOModel(MotionParam.DO_UnLoadBreakVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

                ioModel = GetIOModel(MotionParam.DO_UnLoadSuctionVacuum, 0);
                m_MotroContorl.Run(ioModel, MotorControlType.IOTrigger);

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
        /// 判断顶升模组是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeUpZSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue > pointModel.Pos_X + 0.5)
                {
                    return false;
                }

                resultModel = m_MotroContorl.Run(stationModel.Axis_Y, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue > pointModel.Pos_Y + 0.5)
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

        /// <summary>
        /// 判断上料模组X是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeLoadXSafe(bool bLeft = true)
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_LoadX);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if(bLeft)
                {
                    if (dvalue < pointModel.Pos_X - 0.5)
                    {
                        return false;
                    }
                }
                else
                {
                    if (dvalue > pointModel.Pos_X + 0.5)
                    {
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
        /// 判断上料模组Z是否在安全位置
        /// </summary> 
        /// <returns></returns>
        public bool JudgeLoadZSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_LoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue > pointModel.Pos_X + 0.5)
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

        /// <summary>
        /// 判断上料模组Z是否在N面拍照位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeNCameraLoadZSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_LoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_NSnap);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue > pointModel.Pos_X + 0.1)
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

        /// <summary>
        /// 判断双目相机是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeCameraSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_SmallCamera);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue > pointModel.Pos_X + 0.5)
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

        /// <summary>
        /// 判断下料模组X是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeUnLoadXSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue > pointModel.Pos_X + 0.5)
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

        /// <summary>
        /// 判断下料模组Z是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeUnLoadZSafe()
        {
            try
            {
                if (m_parameter.MotionCard == EnumCard.虚拟卡.ToString())
                {
                    return true;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue > pointModel.Pos_X + 0.2 || dvalue < pointModel.Pos_X - 0.2)
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

        #region 相机计算流程

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

        private bool InputOCR(string firstOcr, ref string outOcr)
        {
            try
            {
                outOcr = m_DelOcrView(firstOcr);
                if (string.IsNullOrEmpty(outOcr))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="cameraIndex">相机Id</param>
        /// <param name="algorithmIndex">0--大视野入料 1--大视野定位 2--小视野入料补正 3--小视野定位</param>
        /// <returns></returns>
        private BaseResultModel AlgorithmRun(int cameraIndex, int algorithmIndex)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                Thread.Sleep(m_CosModel.SnapTimeOut);
                Camera2DSetModel model = XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == cameraIndex);
                CameraResultModel cameraResult = Camera2DSnap(model); 
                OnDispPicEvent(cameraResult);
                resultModel.RunResult = true;

                HTuple hv_Width, hv_Height;
                HOperatorSet.GetImageSize(cameraResult.Image as HObject, out hv_Width, out hv_Height);
                string fileName = "";
                string pathName = "";

                switch (algorithmIndex)
                {
                    case 0://大视野相机入料补正算法
                        {
                            var bigFeedModel = m_sequenceModel.bigFeedAlgorithmModel;
                            bigFeedModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(bigFeedModel, AlgorithmControlType.AlgorithmRun);

                            BigFeedResultModel feedResult = resultModel as BigFeedResultModel;
                            if (feedResult != null && feedResult.RunResult)
                            {
                                cameraResult.DispObj = feedResult.DispObjects;
                                OnDispPicEvent(cameraResult);

                                string strLog = string.Format("中心Row:{0} Column:{1} Angle:{2}", feedResult.BigCenterRow, feedResult.BigCenterCol, feedResult.ProdAngleMean);
                                m_DelOutPutLog(strLog);

                                resultModel.RunResult = SetBigCenterPos(feedResult, hv_Width, hv_Height);
                            }

                            pathName = model.Name;
                            fileName = algorithmIndex.ToString();

                            break;
                        }

                    case 1://大视野相机定位算法
                        {
                            var bigFixedModel = m_sequenceModel.bigFixedAlgorithmModel;
                            bigFixedModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(bigFixedModel, AlgorithmControlType.AlgorithmRun);
                             
                            BigFixedResultModel fixedResult = resultModel as BigFixedResultModel;
                            if(fixedResult != null && fixedResult.RunResult)
                            {
                                cameraResult.DispObj = fixedResult.OutRegion;
                                OnDispPicEvent(cameraResult);

                                resultModel.RunResult = GetSnapCenterPos(fixedResult, hv_Width, hv_Height);
                            }

                            pathName = model.Name;
                            fileName = algorithmIndex.ToString();

                            break;
                        }

                    case 2://小视野入料判断位置算法--推算Bar条
                        {
                            var smallJudgeModel = m_sequenceModel.smallJudgePosModel;
                            smallJudgeModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(smallJudgeModel, AlgorithmControlType.AlgorithmRun);

                            SmallJudgePosResultModel smallResult = resultModel as SmallJudgePosResultModel;
                            if(smallResult != null && smallResult.RunResult)
                            {
                                cameraResult.DispObj = smallResult.OutRegion;
                                OnDispPicEvent(cameraResult);

                                resultModel.RunResult = GetSmallJudgeBarPos(smallResult);
                            }

                            pathName = model.Name + "//入料判断";
                            fileName = algorithmIndex.ToString() + "_" + m_sWaferNo;
                            break;
                        }

                    case 3://小视野入料判断位置算法
                        {
                            var smallJudgeModel = m_sequenceModel.smallJudgePosModel;
                            smallJudgeModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(smallJudgeModel, AlgorithmControlType.AlgorithmRun);

                            SmallJudgePosResultModel smallResult = resultModel as SmallJudgePosResultModel;
                            if (smallResult != null && smallResult.RunResult)
                            {
                                cameraResult.DispObj = smallResult.OutRegion;
                                OnDispPicEvent(cameraResult);

                                resultModel.RunResult = GetSmallJudgePos(smallResult);
                            }

                            pathName = model.Name + "//" + "入料判断";
                            fileName = algorithmIndex.ToString() + "_" + m_sWaferNo;

                            break;
                        }

                    case 4://小视野定位Bar推算算法
                        {
                            //推算Bar条需要同一个视野里面有4个产品Bar条一致
                            var smallPosModel = m_sequenceModel.smallFixedPosModel;
                            smallPosModel.Image = cameraResult.Image as HObject;
                            smallPosModel.NeedOcr = "-1";
                            smallPosModel.IsIngoreCalu = true;
                            smallPosModel.OcrLength = m_paramRangeModel.OcrNum;
                            smallPosModel.OcrBinPath = m_paramRangeModel.OcrBinPath;
                            resultModel = m_algorithmControl.Run(smallPosModel, AlgorithmControlType.AlgorithmRun);

                            SmallFixedPosResultModel fixedResult = resultModel as SmallFixedPosResultModel;
                            if (fixedResult != null && fixedResult.RunResult)
                            {
                                cameraResult.DispObj = fixedResult.OutRegion;
                                OnDispPicEvent(cameraResult);
                                cameraResult.DispObj = fixedResult.OcrOutRegion;
                                OnDispPicEvent(cameraResult);
                                
                                if(fixedResult.Bar != "-9999")
                                {
                                    //找到对应Bar条行的数据
                                    List<Bar> listBar = m_DicBar[m_barIndex];
                                    var bar = listBar.FirstOrDefault(x => x.barId == Int32.Parse(fixedResult.Bar));
                                    if(bar == null)
                                    {
                                        fixedResult.Bar = "-9999";
                                    }
                                    m_listProduct = bar.product.ToList().FindAll(x => x.isReclaimer == 1);

                                    //是否需要继续取上次物料
                                    SetContinueLoad(true, Int32.Parse(fixedResult.Bar));
                                    m_NeedGetCount = m_listProduct.Count;

                                    resultModel.RunResult = GetSmallFirstSnapPos(fixedResult, hv_Width, hv_Height);
                                }
                            }

                            pathName = model.Name + "//" + "Bar推算";
                            fileName = algorithmIndex.ToString() + "_" + m_sWaferNo;

                            m_DelOutPutLog(fixedResult.strLog);
                            break;
                        }

                    case 5://小视野定位算法
                        {
                            var smallPosModel = m_sequenceModel.smallFixedPosModel;
                            smallPosModel.Image = cameraResult.Image as HObject;
                            int count = m_listProduct.Count;
                            smallPosModel.NeedOcr = m_listProduct[count - m_NeedGetCount].productOcr;
                            smallPosModel.IsIngoreCalu = false;
                            smallPosModel.OcrLength = m_paramRangeModel.OcrNum;
                            smallPosModel.OcrBinPath = m_paramRangeModel.OcrBinPath;
                            resultModel = m_algorithmControl.Run(smallPosModel, AlgorithmControlType.AlgorithmRun);

                            m_fixedResult = resultModel as SmallFixedPosResultModel;
                            if (m_fixedResult != null && m_fixedResult.RunResult)
                            {
                                cameraResult.DispObj = m_fixedResult.OutRegion;
                                OnDispPicEvent(cameraResult);
                                cameraResult.DispObj = m_fixedResult.OcrOutRegion;
                                OnDispPicEvent(cameraResult);
                                
                                if (m_fixedResult.ICanGet != 1)
                                {
                                    //设置未取到的产品Excel标记颜色
                                    SetExcelColor(m_listProduct[count - m_NeedGetCount]);
                                    if (m_NeedGetCount > 1)
                                    {
                                        GetNextSmallPos(m_fixedResult, hv_Width, hv_Height, m_fixedResult.NeedOcr, m_listProduct[count - m_NeedGetCount + 1].productOcr);
                                    }
                                }
                                else
                                {
                                    resultModel.RunResult = GetSuckFixedPos(m_fixedResult);
                                }
                            }

                            m_DelOutPutLog(m_fixedResult.strLog);
                            m_DelOutPutLog(string.Format("OCR:{0} FirstOcr:{1} ICanGet:{2} IExisProduct:{3}", m_fixedResult.NeedOcr, m_fixedResult.FirstOcr, m_fixedResult.ICanGet, m_fixedResult.IExistProduct));

                            pathName = model.Name + "//" + "定位图片";
                            fileName = algorithmIndex.ToString() + "_" + m_sWaferNo + "_" + smallPosModel.NeedOcr;

                            break;
                        }

                    case 6://小视野拍照P面检测缺陷
                        { 
                            var pAlgorithmModel = m_sequenceModel.algorithmModelP;
                            pAlgorithmModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(pAlgorithmModel, AlgorithmControlType.AlgorithmRun);

                            PResultModel pResult = resultModel as PResultModel;

                            bool result = GetNextSmallSnapPos(m_fixedResult.NeedOcr);
                            if (!result)
                            {
                                pResult.ErrorMessage += "GetNextSmallSnapPos失败";
                                pResult.RunResult = false;
                            }
                            m_DelOutPutLog(pResult.ErrorMessage);

                            cameraResult.DispObj = pResult.DispObjects;
                            cameraResult.ResultLabel = "OCR: " + m_fixedResult.NeedOcr;
                            OnDispPicEvent(cameraResult);

                            pathName = model.Name + "//" + "定位图片";
                            fileName = algorithmIndex.ToString() + "_" + m_sWaferNo + "_" + m_fixedResult.NeedOcr;

                            //把结果压进队列
                            m_QResult.Enqueue(new QResultModel()
                            {
                                Ocr = m_fixedResult.NeedOcr,
                                PResult = pResult.RunResult ? QResultModel.EResult.EOK : QResultModel.EResult.ENG,
                            });
                            break;
                        }

                    case 7://N面下相机纠偏
                        {
                            var nAlgorithmModel = m_sequenceModel.algorithmModelN;
                            nAlgorithmModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(nAlgorithmModel, AlgorithmControlType.AlgorithmRun);

                            NResultModel nResult = resultModel as NResultModel;

                            bool result = SetFixedPos(7, nResult);
                            if (!result)
                            {
                                nResult.ErrorMessage += "SetFixedPos失败";
                                nResult.RunResult = false;
                            }

                            m_DelOutPutLog(nResult.ErrorMessage);

                            cameraResult.DispObj = nResult.DispObjects;
                            OnDispPicEvent(cameraResult);

                            pathName = model.Name;
                            fileName = algorithmIndex.ToString();

                            m_QResult.First().NResult = nResult.RunResult ? QResultModel.EResult.EOK : QResultModel.EResult.ENG;
                            break;
                        }

                    case 8://AR检测纠偏
                        {
                            var fixtureModel = m_sequenceModel.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 1);
                            fixtureModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(fixtureModel, AlgorithmControlType.AlgorithmRun);

                            cameraResult.DispObj = resultModel.ObjectResult;
                            OnDispPicEvent(cameraResult);

                            if (resultModel.RunResult)
                            {
                                resultModel.RunResult = SetFixedPos(8, resultModel as AlgorithmResultModel);
                            }

                            pathName = model.Name;
                            fileName = algorithmIndex.ToString();
                            break;
                        }

                    case 9://HR检测纠偏
                        {
                            var fixtureModel = m_sequenceModel.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 2);
                            fixtureModel.Image = cameraResult.Image as HObject;
                            resultModel = m_algorithmControl.Run(fixtureModel, AlgorithmControlType.AlgorithmRun);

                            cameraResult.DispObj = resultModel.ObjectResult;
                            OnDispPicEvent(cameraResult);

                            if (resultModel.RunResult)
                            {
                                resultModel.RunResult = SetFixedPos(9, resultModel as AlgorithmResultModel);
                            }

                            pathName = model.Name;
                            fileName = algorithmIndex.ToString();
                            break;
                        }
                    default:
                        break;
                }

                //保存图片
                WriteImage(pathName, fileName, resultModel.RunResult, cameraResult.Image as HObject);

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                m_DelOutExLog(ex);
                return resultModel;
            }
        }

        /// <summary>
        /// 图片保存
        /// </summary>
        /// <param name="fileName">图片名称</param>
        /// <param name="result">结果</param>
        /// <param name="ocrresult">OCR识别结果</param>
        /// <param name="ho_Image">图片</param>
        private void WriteImage(string pathName, string fileName, bool result, HObject ho_Image)
        {
            try
            {
                string strPath = m_parameter.ImagePath + "//" + pathName + "//" + DateTime.Now.ToString("yyyyMMdd") + "//";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strName = fileName + "_" + DateTime.Now.ToString("HHmmssfff") + ".jpg";
                
                if (m_parameter.IsSaveImg)
                {
                    //只保存NG图片
                    if (m_parameter.IsSaveNGImg && !result)
                    { 
                        //NG图片
                        string ngPath = strPath + "NG//";
                        AlgorithmCommHelper.SaveImage(ho_Image as HObject, ngPath, strName);
                       
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
        
        /// <summary>
        /// 纠正大视野相机
        /// </summary>
        private bool SetBigCenterPos(BigFeedResultModel feedResult, int Width, int Height)
        {
            try
            {
                //读取旋转中心文件
                string rotatePath = Global.Model3DPath +  "\\流程8-旋转中心 1_Mat2d.tup";
                HTuple hv_RotateTup;
                HOperatorSet.ReadTuple(rotatePath, out hv_RotateTup);

                //读取标定文件
                string mat2dPath = Global.Model3DPath + "\\流程7-N点标定 2_Mat2d.tup";
                HTuple hv_Mat2dTup;
                HOperatorSet.ReadTuple(mat2dPath, out hv_Mat2dTup);
                //旋转中心映射到轴
                HTuple hv_qx, hv_qy;
                HOperatorSet.AffineTransPoint2d(hv_Mat2dTup, hv_RotateTup[0], hv_RotateTup[1], out hv_qx, out hv_qy);
                //当前运算中心映射到轴
                HTuple hv_CenterX, hv_CenterY;
                HOperatorSet.AffineTransPoint2d(hv_Mat2dTup, feedResult.BigCenterRow, feedResult.BigCenterCol, out hv_CenterX, out hv_CenterY);
                //图像中心映射到轴
                HTuple hv_PicX, hv_PicY;
                HOperatorSet.AffineTransPoint2d(hv_Mat2dTup, Height / 2, Width / 2, out hv_PicX, out hv_PicY);
                 
                double dCircleX = hv_qx;
                double dCircleY = hv_qy;
                double dPhi = 0 - feedResult.ProdAngleMean;

                HTuple hv_homat2dIdentity, hv_RotateMat;
                HOperatorSet.HomMat2dIdentity(out hv_homat2dIdentity);
                HOperatorSet.HomMat2dRotate(hv_homat2dIdentity, dPhi, dCircleX, dCircleY, out hv_RotateMat);
                 
                double dCurrentX = hv_CenterX;
                double dCurrentY = hv_CenterY;

                HTuple dValueX = new HTuple();
                HTuple dValueY = new HTuple();
                HOperatorSet.AffineTransPoint2d(hv_RotateMat, dCurrentX, dCurrentY, out dValueX, out dValueY);
                
                HTuple angValue = 0;
                HOperatorSet.TupleDeg(dPhi, out angValue);
                  
                double xValue = Math.Round(hv_PicX.D - dValueX.D, 2);
                double yValue = Math.Round(hv_PicY.D - dValueY.D, 2);
                double uValue = Math.Round(angValue.D*1000, 2);

                m_DelOutPutLog(string.Format("大视野补偿位 X:{0} Y:{1} Angle:{2}", xValue, yValue, uValue));

                //设置偏移到补偿位
                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCamera);
                var offSetModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCameraOffSet);
                offSetModel.Pos_X = Math.Round(pointModel.Pos_X + xValue, 2);
                offSetModel.Pos_Y = Math.Round(pointModel.Pos_Y + yValue, 2);
                offSetModel.Pos_Z = Math.Round(pointModel.Pos_Z + uValue, 2);

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 根据大视野获取小视野拍照位置
        /// </summary>
        /// <param name="fixedResult"></param>
        private bool GetSnapCenterPos(BigFixedResultModel fixedResult, int Width, int Height)
        {
            try
            {
                //大视野转换到小视野相机像素
                string mat2dPath = Global.Model3DPath + "\\流程9-仿射变换 1_Mat2d.tup";
                HTuple hv_Mat2dTup;
                HOperatorSet.ReadTuple(mat2dPath, out hv_Mat2dTup);

                HTuple dXArr, dYArr;
                HOperatorSet.AffineTransPoint2d(hv_Mat2dTup, fixedResult.AnyRow, fixedResult.AnyCol, out dXArr, out dYArr);
                
                //小视野到中心位置
                int length = dXArr.DArr.Count();
                double[] dXArr2 = new double[length];
                double[] dYArr2 = new double[length];
                for (int i = 0; i < length; i++)
                {
                    double row = Height / 2 - dXArr[i];
                    double col = Width / 2 - dYArr[i];

                    dXArr2[i] = Math.Round(row, 2);
                    dYArr2[i] = Math.Round(col, 2);
                }

                //像素转换到轴位置
                string ninePath = Global.Model3DPath + "\\流程11-N点标定 1_Mat2d.tup";
                HTuple hv_nineTup;
                HOperatorSet.ReadTuple(ninePath, out hv_nineTup);

                HTuple smallXArr, smallYArr;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, dXArr2, dYArr2, out smallXArr, out smallYArr);

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCameraOffSet);

                //组合坐标输出Log
                int count = smallXArr.DArr.Count();
                string strOut = "";
                m_ListPoint.Clear();
                for (int i = 0; i < count; i++)
                {
                    strOut += smallXArr[i].D.ToString("0") + "," + smallYArr[i].D.ToString("0") + ",0,";

                    m_ListPoint.Add(new PointClass()
                    {
                        X = Math.Round(smallXArr[i].D + pointModel.Pos_X, 2),
                        Y = Math.Round(smallYArr[i].D + pointModel.Pos_Y, 2),
                        U = 0 + pointModel.Pos_Z,
                    });
                }
                strOut = strOut.TrimEnd(',');

                m_DelOutPutLog(strOut);

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取小视野产品是否居中
        /// </summary>
        /// <param name="judgeResult"></param>
        /// <returns></returns>
        private bool GetSmallJudgePos(SmallJudgePosResultModel judgeResult)
        {
            try
            {
                //如果产品在中间位置则返回
                if(judgeResult.IsCenterPos)
                {
                    return true;
                }

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);

                var stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare); 

                //判断视野里是否有产品
                if (judgeResult.IsExistProduct)
                {
                    double dCol = judgeResult.SubCol;

                    string ninePath = Global.Model3DPath + "\\流程11-N点标定 1_Mat2d.tup";
                    HTuple hv_nineTup;
                    HOperatorSet.ReadTuple(ninePath, out hv_nineTup);
                    HTuple hv_qx, hv_qy;
                    HOperatorSet.AffineTransPoint2d(hv_nineTup, 0, dCol, out hv_qx, out hv_qy);
                     
                    pointModel.Pos_Y = Math.Round(pointModel.Pos_Y + hv_qy.D, 2);
                    m_iNoProduct = 0;
                }
                else
                { 
                    pointModel.Pos_X = Math.Round(pointModel.Pos_X - 800, 2); 
                    m_iNoProduct++;
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
        /// 推算Bar条时获取小视野产品是否居中
        /// </summary>
        /// <param name="judgeResult"></param>
        /// <returns></returns>
        private bool GetSmallJudgeBarPos(SmallJudgePosResultModel judgeResult)
        {
            try
            {
                //如果产品在中间位置则返回
                if (judgeResult.IsCenterPos)
                {
                    return true;
                }

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);

                //判断视野里是否有产品
                if (judgeResult.IsExistProduct)
                {
                    double dCol = judgeResult.SubCol;

                    string ninePath = Global.Model3DPath + "\\流程11-N点标定 1_Mat2d.tup";
                    HTuple hv_nineTup;
                    HOperatorSet.ReadTuple(ninePath, out hv_nineTup);
                    HTuple hv_qx, hv_qy;
                    HOperatorSet.AffineTransPoint2d(hv_nineTup, 0, dCol, out hv_qx, out hv_qy);

                    pointModel.Pos_Y = Math.Round(pointModel.Pos_Y + hv_qy.D, 2);
                    m_iNoProduct = 0;
                }
                else
                {
                    pointModel.Pos_X = Math.Round(pointModel.Pos_X - 800, 2);
                    m_iNoProduct++;
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
        /// 推算Bar条后获取第一个拍照位置
        /// </summary>
        /// <param name="fixedResult"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        private bool GetSmallFirstSnapPos(SmallFixedPosResultModel fixedResult, int Width, int Height)
        {
            try
            {
                double dRow = fixedResult.OcrCenterRow;
                double dCol = fixedResult.OcrCenterCol;

                double row = Height / 2 - dRow;
                double col = Width / 2 - dCol; 
                //ocr离中心的位置
                double distance = fixedResult.Distance;

                string ninePath = Global.Model3DPath + "\\流程11-N点标定 1_Mat2d.tup";
                HTuple hv_nineTup;
                HOperatorSet.ReadTuple(ninePath, out hv_nineTup);
                HTuple hv_qx, hv_qy;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, row, col, out hv_qx, out hv_qy);

                //产品间距
                HTuple hv_Distance, hv_Y;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, distance, 0, out hv_Distance, out hv_Y); 

                //算法推算第一个OCR
                string strFirstOcr = fixedResult.FirstOcr;
                int colFirst = Int32.Parse(strFirstOcr.Substring(strFirstOcr.Length - 2, 2));
                //需要取料第一个OCR
                var product = m_listProduct.First();

                int colIndex = product.colOcr;
                int subCol = colFirst - colIndex + 3;

                //判断是否大于
                if(subCol > 53 || subCol < -53)
                {
                    subCol = 25;
                }

                m_proDistance = hv_Distance.D;
                if(m_proDistance < 500 || m_proDistance > 800)
                {
                    m_proDistance = 750;
                }
                double dValueX = Math.Round(m_proDistance * subCol + hv_qx.D, 2);

                //设置第一个拍照位置
                var pointModel_1 = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallCameraCenter);
                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);
                
                if(dValueX > 30000 && dValueX < -30000)
                {
                    m_DelOutPutLog(string.Format("偏移值{0}过大", dValueX), LogLevel.Error);
                    return false;
                }
                else
                {
                    pointModel.Pos_X = Math.Round(pointModel_1.Pos_X + dValueX, 2);
                    pointModel.Pos_Y = pointModel_1.Pos_Y;
                    pointModel.Pos_Z = pointModel_1.Pos_Z;
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
        /// 获取位置到顶针位置上方
        /// </summary>
        /// <param name="fixedResult"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        private bool GetSuckFixedPos(SmallFixedPosResultModel fixedResult)
        {
            try
            {
                //读取旋转中心文件
                string rotatePath = Global.Model3DPath + "\\流程11-旋转中心 1_Mat2d.tup";
                HTuple hv_RotateTup;
                HOperatorSet.ReadTuple(rotatePath, out hv_RotateTup);

                //旋转中心映射到轴
                string ninePath = Global.Model3DPath + "\\流程11-N点标定 2_Mat2d.tup";
                HTuple hv_nineTup;
                HOperatorSet.ReadTuple(ninePath, out hv_nineTup);
                HTuple dCircleX, dCircleY;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, hv_RotateTup[0], hv_RotateTup[1], out dCircleX, out dCircleY);

                //定位产品中心到轴
                HTuple dCurrentX, dCurrentY;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, fixedResult.OcrCenterRow, fixedResult.OcrCenterCol, out dCurrentX, out dCurrentY);
                
                //基准中心到轴
                HTuple dBaseX, dBaseY;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, m_paramRangeModel.BaseRow, m_paramRangeModel.BaseColumn, out dBaseX, out dBaseY);

                //角度计算
                double dPhi = m_paramRangeModel.BaseAngle - fixedResult.OcrCenterPhi;
                HTuple hv_Deg;
                HOperatorSet.TupleDeg(dPhi, out hv_Deg);
                double deg = hv_Deg.D * 1000;

                // 获取拍照位置
                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_BigCamera);
                double snapPosX = pointModel.Pos_X;
                double snapPosY = pointModel.Pos_Y;

                //获取当前位置
                var stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                var result1 = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                var result2 = m_MotroContorl.Run(stationModel.Axis_Y, MotorControlType.AxisGetPosition);
                var result3 = m_MotroContorl.Run(stationModel.Axis_Z, MotorControlType.AxisGetPosition);
                double currentX = Double.Parse(result1.ObjectResult.ToString());
                double currentY = Double.Parse(result2.ObjectResult.ToString());
                double currentU = Double.Parse(result3.ObjectResult.ToString());
                m_DelOutPutLog(string.Format("当前位置X:{0} Y:{1} Z:{2}", currentX, currentY, currentU));

                double dNewCircleX = dCircleX + currentX - snapPosX;
                double dNewCircleY = dCircleY + currentY - snapPosY;

                HTuple hv_homat2dIdentity, hv_RotateMat;
                HOperatorSet.HomMat2dIdentity(out hv_homat2dIdentity);
                HOperatorSet.HomMat2dRotate(hv_homat2dIdentity, dPhi, dNewCircleX, dNewCircleY, out hv_RotateMat);
                 
                HTuple dValueX = new HTuple();
                HTuple dValueY = new HTuple();
                HOperatorSet.AffineTransPoint2d(hv_RotateMat, dCurrentX, dCurrentY, out dValueX, out dValueY);

                double dX = dBaseX - dValueX;
                double dY = dBaseY - dValueY;

                //设置位置
                pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_ProFixed);

                m_DelOutPutLog(string.Format("偏移值X:{0} Y:{1} U:{2}", dX, dY, deg));

                if (dX > 18000 || dX < -18000 || dY > 18000 || dY < -18000 || deg > 8000 || deg < -8000)
                {
                    m_DelOutPutLog("偏移值过大", LogLevel.Error);
                    return false;
                }
                else
                {
                    pointModel.Pos_X = Math.Round(currentX + dX, 2);
                    pointModel.Pos_Y = Math.Round(currentY + dY, 2);
                    pointModel.Pos_Z = Math.Round(currentU + deg, 2);
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
        /// 获取下一个产品的拍照位置
        /// </summary>
        private bool GetNextSmallSnapPos(string needOcr)
        {
            try
            {
                var index = m_listProduct.FindIndex(x => x.productOcr == needOcr);
                if(index == -1)
                {
                    return true;
                }

                if(m_listProduct.Count <= index + 1)
                {
                    return true;
                }
                var nextproduct = m_listProduct[index + 1];
                int nextCol = nextproduct.colOcr-3;

                string strNeedOcr = needOcr;
                int currentCol = Int32.Parse(strNeedOcr.Substring(strNeedOcr.Length - 2, 2));

                int subCol = nextCol - currentCol;
                if (subCol > 53 || subCol < -53)
                {
                    subCol = 25;
                }
                if (m_proDistance < 500 || m_proDistance > 800)
                {
                    m_proDistance = 750;
                }
                double dValueX = Math.Round(m_proDistance * subCol, 2);

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);
                var pointModel_Fixed = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_ProFixed);

                pointModel.Pos_X = Math.Round(pointModel_Fixed.Pos_X - dValueX, 2);
                pointModel.Pos_Y = pointModel_Fixed.Pos_Y;
                pointModel.Pos_Z = pointModel_Fixed.Pos_Z;

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }
        
        /// <summary>
        /// 获取下一个产品的拍照位置
        /// </summary>
        private bool GetNextSmallPos(string needOcr, string firstOcr)
        {
            try
            {
                var index = m_listProduct.FindIndex(x => x.productOcr == needOcr);
                if (index == -1)
                {
                    return true;
                }

                if (m_listProduct.Count <= index + 1)
                {
                    return true;
                }
                var nextproduct = m_listProduct[index];
                int nextCol = nextproduct.colOcr - 3;
                
                int currentCol = Int32.Parse(firstOcr.Substring(firstOcr.Length - 2, 2));

                int subCol = nextCol - currentCol;
                if (subCol > 53 || subCol < -53)
                {
                    subCol = 25;
                }
                if (m_proDistance < 500 || m_proDistance > 800)
                {
                    m_proDistance = 750;
                }
                double dValueX = Math.Round(m_proDistance * subCol, 2);

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);

                pointModel.Pos_X = Math.Round(pointModel.Pos_X - dValueX, 2);
                pointModel.Pos_Y = pointModel.Pos_Y;
                pointModel.Pos_Z = pointModel.Pos_Z;

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        private bool GetNextSmallPos(SmallFixedPosResultModel fixedResult, int Width, int Height, string nowOcr, string nextOcr)
        {
            try
            {
                double dRow = fixedResult.OcrCenterRow;
                double dCol = fixedResult.OcrCenterCol;

                double row = Height / 2 - dRow;
                double col = Width / 2 - dCol; 

                string ninePath = Global.Model3DPath + "\\流程11-N点标定 1_Mat2d.tup"; 
                HTuple hv_nineTup;
                HOperatorSet.ReadTuple(ninePath, out hv_nineTup);
                HTuple hv_qx, hv_qy;
                HOperatorSet.AffineTransPoint2d(hv_nineTup, row, col, out hv_qx, out hv_qy);
                
                //获取下一个的Index
                var index = m_listProduct.FindIndex(x => x.productOcr == nextOcr);
                if (index == -1)
                {
                    return true;
                }

                if (m_listProduct.Count <= index + 1)
                {
                    return true;
                }
                var nextproduct = m_listProduct[index];
                int nextCol = nextproduct.colOcr - 3;

                int currentCol = Int32.Parse(nowOcr.Substring(nowOcr.Length - 2, 2));

                int subCol = nextCol - currentCol;
                if (subCol > 53 || subCol < -53)
                {
                    subCol = 25;
                }
                if (m_proDistance < 500 || m_proDistance > 800)
                {
                    m_proDistance = 750;
                }
                double dValueX = Math.Round(m_proDistance * subCol + hv_qx.D, 2);

                var pointModel = GetPointModel(MotionParam.Station_PrePare, MotionParam.Pos_SmallGet);

                pointModel.Pos_X = Math.Round(pointModel.Pos_X - dValueX, 2);
                pointModel.Pos_Y = pointModel.Pos_Y;
                pointModel.Pos_Z = pointModel.Pos_Z;

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 设置纠偏位置
        /// </summary>
        /// <param name="index">7-N 8-AR检测 9-HR检测</param>
        /// <param name="resultModel"></param>
        /// <returns></returns>
        private bool SetFixedPos(int index, AlgorithmResultModel resultModel)
        {
            try
            {
                FixtureAlgorithmModel fixtureModel;
                PointModel pointModel;
                PointModel pointModel_base;
                double subRow, subCol, subAng, offSetX, offSetY;
                switch (index)
                {
                    case 7:
                        pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_LoadFixed);
                        pointModel_base = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Load);
                        var pointModel_loadb = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_UnLoad);
                        var pointModel_x = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_UnLoadFixed);

                        fixtureModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 0);
                        subRow = fixtureModel.Row - resultModel.CenterRow;
                        subCol = fixtureModel.Column - resultModel.CenterColumn;
                        subAng = fixtureModel.Angle - resultModel.CenterPhi;

                        if (Math.Abs(subRow) > 1000 || Math.Abs(subCol) > 1000 || Math.Abs(subAng) > 60)
                        {
                            m_DelOutPutLog("纠偏值过大");
                            return false;
                        }
                        offSetX = Math.Round(subCol * 1.2, 2);
                        offSetY = Math.Round(subRow * 1.2, 2);
                        pointModel.Pos_X = pointModel_base.Pos_X + offSetY;
                        pointModel.Pos_Y = pointModel_base.Pos_Y + Math.Round(subAng, 2);
                        pointModel_x.Pos_X = pointModel_loadb.Pos_X + offSetX;

                        break;

                    case 8:
                        pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_ARFixed); 
                        pointModel_base = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_ARCheck);

                        fixtureModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 1);
                        subRow = fixtureModel.Row - resultModel.CenterRow;
                        subCol = fixtureModel.Column - resultModel.CenterColumn;
                        subAng = fixtureModel.Angle - resultModel.CenterPhi;

                        if (Math.Abs(subRow) > 1000 || Math.Abs(subCol) > 1000 || Math.Abs(subAng) > 60)
                        {
                            m_DelOutPutLog("纠偏值过大");
                            return false;
                        }
                        offSetX = Math.Round(subCol * 0.8625, 2);
                        pointModel.Pos_X = pointModel_base.Pos_X + offSetX;
                        pointModel.Pos_Y = pointModel_base.Pos_Y + Math.Round(subAng, 2);

                        break;
                    case 9:
                        pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_HRFixed);
                        pointModel_base = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_HRCheck);

                        fixtureModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 1);
                        subRow = fixtureModel.Row - resultModel.CenterRow;
                        subCol = fixtureModel.Column - resultModel.CenterColumn;
                        subAng = fixtureModel.Angle - resultModel.CenterPhi;

                        if (Math.Abs(subRow) > 1000 || Math.Abs(subCol) > 1000 || Math.Abs(subAng) > 60)
                        {
                            m_DelOutPutLog("纠偏值过大");
                            return false;
                        }
                        offSetX = Math.Round(subCol * 0.8625, 2);
                        pointModel.Pos_X = pointModel_base.Pos_X + offSetX;
                        pointModel.Pos_Y = pointModel_base.Pos_Y + Math.Round(subAng, 2);

                        break;
                    default:
                        break;
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
        /// N面设置纠偏位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="resultModel"></param>
        /// <returns></returns>
        private bool SetFixedPos(int index, NResultModel resultModel)
        {
            try
            {
                FixtureAlgorithmModel fixtureModel;
                PointModel pointModel;
                PointModel pointModel_base;
                double subRow, subCol, subAng, offSetX, offSetY;
                switch (index)
                {
                    case 7:
                        pointModel = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_LoadFixed);
                        pointModel_base = GetPointModel(MotionParam.Station_Check, MotionParam.Pos_Load);
                        var pointModel_loadb = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_UnLoad);
                        var pointModel_x = GetPointModel(MotionParam.Station_LoadX, MotionParam.Pos_UnLoadFixed);

                        fixtureModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 0);
                        subRow = fixtureModel.Row - resultModel.CenterRow;
                        subCol = fixtureModel.Column - resultModel.CenterColumn;
                        subAng = fixtureModel.Angle - resultModel.CenterPhi;

                        if (Math.Abs(subRow) > 1000 || Math.Abs(subCol) > 1000 || Math.Abs(subAng) > 60)
                        {
                            m_DelOutPutLog("纠偏值过大");
                            return false;
                        }
                        offSetX = Math.Round(subCol * 1.2, 2);
                        offSetY = Math.Round(subRow * 1.2, 2);
                        pointModel.Pos_X = pointModel_base.Pos_X + offSetY;
                        pointModel.Pos_Y = pointModel_base.Pos_Y + Math.Round(subAng, 2);
                        pointModel_x.Pos_X = pointModel_loadb.Pos_X + offSetX;

                        break;
  
                    default:
                        break;
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
                    try
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
                            else if (value == 0)
                            {
                                if (bvalue)
                                {
                                    Global.IsEmergency = false;
                                    m_DelEmergency(false);
                                    bvalue = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                         
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
        /// 光源控制
        /// </summary>
        /// <param name="channel">通道 A B C D E</param>
        /// <param name="bOpen"></param>
        public void LightControl(string channel, bool bOpen = true)
        {
            try
            {
                lock(this)
                {
                    var comModel = XmlControl.sequenceModelNew.ComModels[0];
                    string strContent = string.Format("S{0}{1}#", channel, bOpen ? "0255" : "0000");
                    comModel.SendContent = strContent;
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
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
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
                    if (m_socketControl[tcpipModel.Id] == null)
                    {
                        continue;
                    }
                    m_socketControl[tcpipModel.Id].Close();
                }

                //关闭串口通讯
                foreach (var comModel in XmlControl.sequenceModelNew.ComModels)
                {
                    if (m_serialControl[comModel.Id] == null)
                    {
                        continue;
                    }
                    m_serialControl[comModel.Id].Run(comModel, ControlType.SerialPortClose);
                }

                //关闭相机
                foreach (var cameraModel in XmlControl.sequenceModelNew.Camera2DSetModels)
                {
                    if (m_CameraControl[cameraModel.Id] == null)
                    {
                        continue;
                    }
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
        /// <param name="ldResult">物料测试结果</param>
        private void SetUnLoadStatus(EnumLDResult ldResult)
        {
            try
            {
                List<int> listIndex = new List<int>();
                for (int i = 0; i < m_listCosResult.Count; i++)
                {
                    if (m_listCosResult[i] == ldResult)
                    {
                        listIndex.Add(i + 1);
                    }
                }
                UnLoadModel unLoadModel = GetUnLoadModel(ldResult);

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
        /// <param name="ldResult">测试结果类型</param>
        /// <returns></returns>
        private UnLoadModel GetUnLoadModel(EnumLDResult ldResult)
        {
            UnLoadModel unLoadModel = new UnLoadModel();
            try
            {
                switch (ldResult)
                {
                    case EnumLDResult.Enum_Pass:
                        unLoadModel = m_unLoadTrayModel.PassModel;
                        break;
                    case EnumLDResult.Enum_Fail:
                        unLoadModel = m_unLoadTrayModel.FailModel;
                        break;            
                    case EnumLDResult.Enum_SeemNG:
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
        /// <param name="ldResult">结果</param>
        /// <param name="index">3：吸嘴3 1：吸嘴1</param>
        /// <param name="xValue">输出x坐标</param>
        /// <param name="yValue">输出ys坐标</param>
        private bool GetUnLoadTrayPos()
        {
            try
            {
                EnumLDResult ldResult = JudgeQResult();
                m_DelOutPutLog(string.Format("OCR:{0} Result:{1}", m_QResult.First().Ocr, ldResult.ToString()), LogLevel.Debug);
                m_QResult.Dequeue();

                int trayCurrentRow, trayCurrentCol;
                UnLoadModel unLoadModel = GetUnLoadModel(ldResult);
                GetUnLoadRowCol(unLoadModel.TrayCurrentNum, out trayCurrentRow, out trayCurrentCol);

                int productCurrentRow = unLoadModel.ProductCurrentRow;
                int productCurrentCol = unLoadModel.ProductCurrentCol;

                //获取吸嘴下料的起始示教位置
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                
                string strTeach = string.Format("放料位{0}", unLoadModel.TrayCurrentNum);  
                var pointModel = GetPointModel(MotionParam.Station_UnLoad, strTeach);
                
                //物料位置
                double xproductdis = (productCurrentCol - 1) * m_unLoadTrayModel.ProductColDis;
                double yproductdis = (productCurrentRow - 1) * m_unLoadTrayModel.ProductRowDis;
                
                //某个料盘里面物料
                double xValue = Math.Round(pointModel.Pos_X - xproductdis, 2);
                double yValue = Math.Round(pointModel.Pos_Y + yproductdis, 2);
                
                pointModel = GetPointModel(MotionParam.Station_UnLoad, MotionParam.Pos_UnLoad);

                pointModel.Pos_X = xValue;
                pointModel.Pos_Y = yValue;

                //获取下料Z轴 
                var pointModelZT = GetPointModel(MotionParam.Station_UnLoadZ, strTeach);
                var pointModelZ = GetPointModel(MotionParam.Station_UnLoadZ, MotionParam.Pos_UnLoad);
                pointModelZ.Pos_X = pointModelZT.Pos_X;

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        private EnumLDResult JudgeQResult()
        {
            try
            {
                var qResult = m_QResult.First();
                if (qResult.PResult == QResultModel.EResult.EOK && 
                    qResult.NResult == QResultModel.EResult.EOK &&
                    qResult.ARResult == QResultModel.EResult.EOK &&
                    qResult.HRResult == QResultModel.EResult.EOK)
                {
                    return EnumLDResult.Enum_Pass;
                }
                else if(qResult.PResult == QResultModel.EResult.ENG ||
                    qResult.NResult == QResultModel.EResult.ENG ||
                    qResult.ARResult == QResultModel.EResult.ENG ||
                    qResult.HRResult == QResultModel.EResult.ENG)
                {
                    return EnumLDResult.Enum_Fail;
                }
                else
                {
                    return EnumLDResult.Enum_SeemNG;
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return EnumLDResult.Enum_Fail;
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
                GetBlowingList();

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
        private void WriteTestData(CharResultModel charResult, LIVResultModel livResult, string strOcr, EnumLDResult eresult, int stationindex = 1, string qr1 = "", string qr2 = "", string coldWave = "")
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
                if (pointModel == null)
                {
                    m_DelOutPutLog(string.Format("【{0}】不存在点位【{1}】", stationName, pointName), LogLevel.Error);
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
                //relatIo.IsWaitTimeOut = !m_CosModel.IsShieldCylinderUp;

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

        #region Map操作数据

        /// <summary>
        /// 录入Map
        /// </summary>
        /// <param name="index"></param>
        /// <param name="path"></param>
        /// <param name="waferName"></param>
        /// <param name="productType"></param>
        /// <param name="bcheck">true--检查Map表 false--录入Map表</param>
        /// <returns></returns>
        public bool AddMap(int index, string path, string waferName, string productType, bool bcheck)
        {
            try
            {
                var data = m_oleExcel.ExcelToDS(path, "验证质检发货bar明细");
                List<Bar> listBar = m_oleExcel.GetData(data, waferName, productType);

                if(listBar.Count > 0)
                {
                    if(!bcheck)
                    {
                        if(m_DicBar.ContainsKey(index))
                        {
                            m_DicBar.Remove(index);
                        }
                        m_DicBar.Add(index, listBar);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除已录入的Map
        /// </summary>
        /// <param name="index"></param>
        public void DeleteMap(int index)
        {
            try
            {
                if(m_DicBar.ContainsKey(index))
                {
                    m_DicBar.Remove(index);
                }
            }
            catch (Exception ex)
            {

            }

        }
        
        /// <summary>
        /// 设置未取产品Excel颜色
        /// </summary>
        /// <param name="product"></param>
        private void SetExcelColor(Product product)
        {
            try
            {
                var setMap = m_sequenceModel.LDModel.mapModel.ListMap[0];
                m_oleExcel.SetBackClolor(setMap.WaferPath, product.rowOcr, product.colOcr, "验证质检发货bar明细");
            }
            catch (Exception ex)
            {

            }
        }
        
        /// <summary>
        /// 未确认Bar条，手动输入设置
        /// </summary>
        /// <param name="fixedResult"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="ibar"></param>
        /// <returns></returns>
        private bool SetBar(SmallFixedPosResultModel fixedResult, int width, int height, int ibar)
        {
            try
            {
                List<Bar> listBar = m_DicBar[m_barIndex];
                var bar = listBar.FirstOrDefault(x => x.barId == ibar);
                m_listProduct = bar.product.ToList().FindAll(x => x.isReclaimer == 1);
                m_NeedGetCount = m_listProduct.Count;

                bool bresult = GetSmallFirstSnapPos(fixedResult, width, height);
                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 设置Map的数据
        /// </summary>
        private void SetMapValue()
        {
            try
            {
                int index = m_listProduct.Count - m_NeedGetCount;
                m_CosModel.mapModel.CurrentOcr = m_listProduct[index].productOcr;
                m_CosModel.mapModel.BarCount = m_listProduct.Count;
                m_CosModel.mapModel.GetCount = index;
                m_CosModel.mapModel.CurrentRow = m_LDRowCount - m_SurplusRowCount;

                m_DelRefreshMap();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 手动换排
        /// </summary>
        /// <returns></returns>
        public bool ChangeRow()
        {
            try
            {
                m_isChangeRow = true;

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 依据上次状态继续运行
        /// </summary>
        private void SetContinueLoad(bool bcheckbar = false, int getbar = 0)
        {
            try
            {
                if (!m_isContinueLoad)
                {
                    return;
                }

                if(!bcheckbar)
                {
                    int currentRow = m_CosModel.mapModel.CurrentRow;

                    //设置当前去取料的所在行
                    for (int i = 0; i < currentRow - 1; i++)
                    {
                        m_ListPoint.RemoveAt(0);
                    }
                    return;
                }
                else
                {
                    m_isContinueLoad = false;
                }

                string currentOcr = m_CosModel.mapModel.CurrentOcr;
                int getCount = m_CosModel.mapModel.GetCount;

                string bar = currentOcr.Remove(currentOcr.Length - 2);
                if(Int32.Parse(bar) != getbar)
                {
                    return;
                }

                m_listProduct.RemoveRange(0, getCount);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        /// <summary>
        /// 检查Map表
        /// </summary>
        /// <returns></returns>
        private bool CheckMap()
        {
            try
            {
                bool bresult = m_DicBar.ContainsKey(m_barIndex);
                
                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 移除已取完的Map数据
        /// </summary>
        private void RemoveBar()
        {
            try
            {
                m_DicBar.Remove(m_barIndex);
                var map = m_CosModel.mapModel.ListMap.FirstOrDefault(x => x.Index == m_barIndex);
                if(map != null)
                {
                    map.IsEffective = false;
                }
                m_barIndex++;

                m_DelRefreshMap();
            }
            catch (Exception ex)
            {
                 
            }
        }
        #endregion
    }
}