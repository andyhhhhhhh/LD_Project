using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    /// <summary>
    /// 配置的流程Model
    /// </summary>
    public class CheckFeatureModel
    {
        [NonSerialized]
        private object returnValue = null;
        
        //0
        public int Id { get; set; }
        //1
        public string Name { get; set; }
        //2
        /// <summary>
        /// Model类型
        /// </summary>
        public FeatureType featureType { get; set; }
        //3
        /// <summary>
        /// 单元描述
        /// </summary>
        public string Description { get; set; }
        //4
        /// <summary>
        /// 单元结果 "OK"/"NG"
        /// </summary>
        public string Result { get; set; }
        //5
        /// <summary>
        /// 是否启用 ture:启用 false:屏蔽
        /// </summary>
        public bool Enable { get; set; }//是否启用
        //6
        /// <summary>
        /// 返回值 ---暂时不需要 用ItemResult代替
        /// </summary>
        public object ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }
        //7
        /// <summary>
        /// 是否设置为断点 true:设置断点 false:取消断点
        /// </summary>
        public bool BreakPoint { get; set; }
        //8
        /// <summary>
        /// 单元运行时间
        /// </summary>
        public double ExecuteTime { get; set; }
        //9
        /// <summary>
        /// 单元绑定窗口
        /// </summary>
        public int LayOut { get; set; }
        //10
        /// <summary>
        /// 下一个运行单元的No
        /// </summary>
        [XmlIgnore]
        public int NextNo { get; set; } 
 
        /// <summary>
        /// 单元序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 报警跳转
        /// </summary>
        public string AlarmGoTo { get; set; }

        public CheckFeatureModel Clone()
        {
            CheckFeatureModel tModel = new CheckFeatureModel();
            tModel.Id = Id;
            tModel.Name = Name;
            tModel.featureType = featureType;
            tModel.Description = Description;
            tModel.Result = Result;
            tModel.Enable = Enable;
            tModel.BreakPoint = BreakPoint;
            tModel.ExecuteTime = ExecuteTime;
            tModel.LayOut = LayOut;
            tModel.Index = Index;
            tModel.AlarmGoTo = AlarmGoTo;

            return tModel;
        }
    }

    /// <summary>
    /// 3D图像设置Model
    /// </summary>
    public class Camera3DSetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public int Port { get; set; }
        public int Profile { get; set; }

        public int ExposureTime { get; set; }

        public int Brightness { get; set; }

        public int Gain { get; set; }

        public bool EnableGain { get; set; }

        public double XScale { get; set; }

        public double YScale { get; set; }

        public double XYResolution { get; set; }

        public double ZResolution { get; set; }

        public int LaserThreshold { get; set; }
    }


    /// <summary>
    /// 2D图像设置Model
    /// </summary>
    public class Camera2DSetModel
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public int ExposureTime { get; set; } 

        public int Gain { get; set; }

        public string UniqueName { get; set; }

        public string DeviceName { get; set; } 

        public string ExposureParamName { get; set; }

        public string GainParamName { get; set; }

        public string InterfaceName { get; set; }
        
        public int Index { get; set; }

        [XmlIgnore]
        public HTuple AcqHandle { get; set; }

        [XmlIgnore]
        public bool IsOpen { get; set; }

        [XmlIgnore]
        public bool IsContinue { get; set; }
         
        public bool bLocalImage { get; set; } 

        public string LocalPath { get; set; }

        public CameraType cameraType { get; set; }

        public bool IsExternTrig { get; set; }

        [XmlIgnore]
        public bool IsCameraSnaping { get; set; } 
    }

    public enum CameraType
    {
        MVHAL,
        MVS, 
        BASLER,
        ITEK,
        IDS,
    }


    /// <summary>
    /// 在结果上面添加的偏移类型
    /// </summary>
    public enum OffSetType
    {
        无,
        加,
        减,
        乘,
        除
    }

    /// <summary>
    /// TCPIP通讯设置
    /// </summary> 
    public class CommunicationModel : BaseSeqModel
    {
        public override int Id { get; set; }
        public override string Name { get; set; }

        public string Paramter { get; set; }
        public string StartChar { get; set; }
        public string SpiltChar { get; set; }
        public string EndChar { get; set; }

        //通讯方式
        public string MsgFunc { get; set; }
        public string MsgName { get; set; }

        public CommunicationModel Clone()
        {
            CommunicationModel tModel = new CommunicationModel();
            tModel.Name = Name;
            tModel.Id = Id;
            tModel.Paramter = Paramter;
            tModel.StartChar = StartChar;
            tModel.SpiltChar = SpiltChar;
            tModel.EndChar = EndChar;
            tModel.MsgFunc = MsgFunc;
            tModel.MsgName = MsgName; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public CommunicationModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
        }
    }
        
    /// <summary>
    /// TCPIP通讯Model
    /// </summary>
    public class TCPIPModel : BaseSeqModel
    {
        public string IPAddress { get; set; }

        public int PortNum { get; set; } 
        
        public bool IsService { get; set; }

        [XmlIgnore]
        public bool IsConnected { get; set; }

        [XmlIgnore]
        public List<IntPtr> Handler { get; set; } 

        [XmlIgnore]
        public string SendContent { get; set; }

        public int TimeOut { get; set; }

        public TCPIPModel Clone()
        {
            TCPIPModel tModel = new TCPIPModel();
            tModel.Name = Name;
            tModel.IPAddress = IPAddress;
            tModel.PortNum = PortNum;
            tModel.IsService = IsService; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public TCPIPModel()
        {
            itemResult = new ItemResult();
            Handler = new List<IntPtr>();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
        }

    }

    /// <summary>
    /// Com串口Model
    /// </summary>
    public class ComModel : BaseSeqModel
    {
        public string ComPortName { get; set; }

        public Parity Parity { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public StopBits StopBits { get; set; }

        public int TimeOut { get; set; }

        public string SendContent { get; set; }

        public bool IsByEnd { get; set; }

        public bool IsHex { get; set; }

        [XmlIgnore]
        public bool IsConnected { get; set; }


        public ComModel Clone()
        {
            ComModel tModel = new ComModel();
            tModel.Name = Name;
            tModel.ComPortName = ComPortName;
            tModel.Parity = Parity;
            tModel.BaudRate = BaudRate;
            tModel.DataBits = DataBits;
            tModel.StopBits = StopBits;
            tModel.TimeOut = TimeOut;
            tModel.IsHex = IsHex;
            tModel.IsByEnd = IsByEnd;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public ComModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
        }
    }

    /// <summary>
    /// Ftp客户端 Model
    /// </summary>
    public class FtpClientModel : BaseSeqModel
    {
        public string IPAddress { get; set; }

        public int PortNum { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public FtpClientModel Clone()
        {
            FtpClientModel tModel = new FtpClientModel();
            tModel.Name = Name;
            

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public FtpClientModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    /// <summary>
    /// SFC Model
    /// </summary>
    public class SFCModel : BaseSeqModel
    { 
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string StationID { get; set; }
        public string Lang { get; set; }
        public string Site { get; set; }
        public string Bu { get; set; }

        public string AccType { get; set; }

        public SFCModel Clone()
        {
            SFCModel tModel = new SFCModel();
            tModel.Name = Name;


            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public SFCModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    #region 暂时无用
    /// <summary>
    /// 产品信息 Model
    /// </summary>
    public class ProductInfoModel : BaseSeqModel
    {
        public override int Id { get; set; }

        public override string Name { get; set; }

        public string SevenTitle { get; set; }

        public string BoxCode { get; set; }

        /// <summary>
        /// 是否检查梳子
        /// </summary>
        public bool IsCheckComb { get; set; }

        /// <summary>
        /// 是否检查防潮袋
        /// </summary>
        public bool IsCheckBag { get; set; }

        /// <summary>
        /// 是否检查说明书
        /// </summary>
        public bool IsCheckManual { get; set; }

        /// <summary>
        /// 是否检查干燥剂
        /// </summary>
        public bool IsCheckDesiccant { get; set; }

        /// <summary>
        /// 是否检查隔热套
        /// </summary>
        public bool IsCheckSleeve { get; set; }
        public ProductInfoModel Clone()
        {
            ProductInfoModel tModel = new ProductInfoModel();
            tModel.Name = Name;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public ProductInfoModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }
    #endregion
    
    /// <summary>
    /// 产品选择信息
    /// </summary>
    public class ProductSelModel : BaseSeqModel
    {
        public override int Id { get; set; }

        public override string Name { get; set; }

        public double Radius { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public string Description { get; set; }

        public ProductSelModel Clone()
        {
            ProductSelModel tModel = new ProductSelModel();
            tModel.Name = Name;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.Radius = Radius;
            tModel.Description = Description;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return "";
            }

            set
            {
                value = "";
            }
        }

        public ProductSelModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }
    
    /// <summary>
    /// 添加具体类型的区域
    /// </summary>
    public enum AreaType
    {
        圆,
        固定圆,
        矩形,
        固定矩形,
        矩形2,
        线,
        固定线,
        折线
    }

    /// <summary>
    /// 界面添加区域类
    /// </summary>
    public class AddArea
    {
        public AreaType areaType { get; set; }
        public int radius { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public bool bClear { get; set; }
        public bool bMask { get; set; }//是否是掩模  
        public int count { get; set; }

        public AddArea()
        {
            count = 1;
        }
    }

    /// <summary>
    /// 表达式类
    /// </summary>
    public class ExpressClass
    {
        public int Id { get; set; }
        public string ExpressValue { get; set; }
        public ComputeType ComputeType { get; set; }
    }

    /// <summary>
    /// 表达式的计算类型
    /// </summary>
    public enum ComputeType
    {
        大于等于,
        小于等于,
        大于,
        小于,
        等于,
        或,
        与,
        非,
        不等于
    }

    #region 暂时无用
    /// <summary>
    /// 预处理图Model
    /// </summary>
    public class PreImageModel
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("PreCalType")]
        public PreCalType preCalType { get; set; }

        [XmlAttribute("Paramter")]
        public string Paramter { get; set; }

        [XmlAttribute("Enable")]
        public bool Enable { get; set; }
    }
    #endregion 

    public enum PreCalType
    {
        水平翻转,
        垂直翻转,
        左转90度,
        右转90度,
        中值滤波,
        均值滤波,
        高斯滤波,
        Sobel滤波,
        Gamma校正,
        灰度腐蚀,
        灰度膨胀,
        直方图,
        阈值分割,
        比例增大,
        旋转角度,
        图像锐化,
        亮度调节,
        对比度,
        二值化,
        自动二值化,
        动态二值化,
        图像反转,
        彩色转灰度,
        灰度转彩色,
        图像镜像,
        改变尺寸,
        提取单通道,
        字符旋转
    }

    public enum EnumCard
    {
        正运动,
        软PLC,
        虚拟卡,
    }

    /// <summary>
    /// 单个Model的类型 如果新增类型则要在此添加新类型
    /// </summary>
    public enum FeatureType
    {
        //图像处理
        相机采集,//相机2D采集
        镭射采集,
        预处理图,
        图像保存,
        显示图像,
        二值图像,
        阈值分割,
        图像剪切,
        图像差分,
        图像展开,
        图像相减,
        颜色转换,
        图像拆分,
        多图采集,

        //检测识别
        检测点,
        基准面,
        创建ROI,
        矩形阵列,
        圆形阵列,
        读二维码,
        读一维码,
        字符识别,
        检测亮度,
        统计像素,
        测清晰度,
        检测正反,
        斑点检测,
        字符训练,
        颜色抽取,

        //几何测量
        找圆,
        找线,
        圆弧测量,
        圆心距,
        两线距离,
        线圆交点,
        两线交点,
        两线角度,
        两圆距离,
        点线距离,
        点圆距离, 
        区域找圆,
        两圆交点,
        一维测量,
        矩形测量,

        //定位工具
        特征匹配,
        轮廓匹配,
        灰度匹配,
        手绘模板,
        交点匹配,
        直线匹配,

        //变量工具
        变量设置,
        等待变量,
        局部变量,
        数组定义,
        数组设置,
        文本分割,
        创建文本,
        数据入队,
        数据出队,
        清空队列,
        变量存储,
        变量读取,

        //系统流程
        流程触发,
        发送消息,
        接收消息,
        延时等待,
        调用流程,
        循环开始,
        循环结束,
        执行片段,
        停止片段,
        条件分支,
        判断跳转,
        弹框提示,
        条件选择,
        切换方案,
        流程结果,

        //数据操作
        数据保存,
        数据运算,
        算表达式,
        算法引擎,
        时间统计,
        脚本分析,
        产品选择,
        脚本编辑,
        结果显示,
        显示数据,
        系统时间,

        //检测3D
        图片处理,
        平面度,
        检测高度,
        基准图像,
        胶路缺陷,
        胶路四边, 
        外壳内边,
        检测胶路,
        取线轮廓,
        区域高度,

        //标定工具
        测量标定,
        N点标定,
        坐标映射,
        仿射变换,
        畸变标定,
        矩阵变换,
        区域映射,
        旋转中心,

        //区域处理
        面积中心,
        腐蚀膨胀,
        开闭运算,
        区域运算,
        区域填充,
        区域转边,
        形状筛选,
        区域形状,
        特征检测,
        区域排序,
        区域分割,
        顶帽处理,

        //轮廓处理
        边缘提取,
        轮廓筛选,
        轮廓交点,
        生成十字,
        生成直线,
        点构建,
        点拟合圆,
        边转区域,
        点排序,

        //暂时无用
        PLC通讯,
        PLC读取,
        PLC写入,
        批量写入,
        接收文本,
        发送文本,
        读取文本,
        写入文本,
        Ftp操作,
        光源控制,  
        批处理,
        连接螺批,
        读螺丝批,
        上传SFC,
        上传SAP,
        斑马打印,

        //运动控制
        IO操作,
        IO检测,
        轴回零,
        工站走点,
        走偏移量,
        设置速度,
        停止运动,
        等待就绪,
        点位修改,
        获取位置,
    }

}
