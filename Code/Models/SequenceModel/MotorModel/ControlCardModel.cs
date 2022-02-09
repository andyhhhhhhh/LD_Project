using BaseModels;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    /// <summary>
    /// Sequence根节点
    /// </summary>
    [XmlRoot("ControlCardModel")]
    public class ControlCardModel : BaseMotorModel
    {
        /// <summary>
        /// 控制卡List
        /// </summary>
        [XmlArrayItem("CardModels")]
        public List<CardModel> CardModels { get; set; }

        /// <summary>
        /// 工位List
        /// </summary>
        [XmlArrayItem("StationModels")]
        public List<StationModel> StationModels { get; set; }

        /// <summary>
        /// IO List
        /// </summary>
        [XmlArrayItem("IOModels")]
        public List<IOModel> IOModels { get; set; }

        /// <summary>
        /// 关联IO List
        /// </summary> 
        [XmlArrayItem("RelatIoModels")]
        public List<RelatIoModel> RelatIoModels { get; set; }

        public ControlCardModel()
        {
            Id = 0;
            Name = "控制卡配置";
            CardModels = new List<CardModel>();
            StationModels = new List<StationModel>();
            IOModels = new List<IOModel>();
            RelatIoModels = new List<RelatIoModel>();
        }
    }

    /// <summary>
    /// 控制卡Model
    /// </summary>
    public class CardModel : BaseMotorModel
    {
        /// <summary>
        /// 轴数量
        /// </summary>
        [XmlAttribute("AxisNum")]
        public int AxisNum { get; set; }

        /// <summary>
        /// Io数量
        /// </summary>
        [XmlAttribute("IoNum")]
        public int IoNum { get; set; }

        /// <summary>
        /// 轴List
        /// </summary>
        [XmlArrayItem("AxisParamModels")]
        public List<AxisParamModel> AxisParamModels { get; set; }

        /// <summary>
        /// 扩展IO List
        /// </summary>
        [XmlArrayItem("ExtIoModels")]
        public List<ExtIoModel> ExtIoModels { get; set; } 

        public CardModel()
        {
            AxisNum = 8;
            IoNum = 16;
            AxisParamModels = new List<AxisParamModel>();
            ExtIoModels = new List<ExtIoModel>();
        }
    }

    /// <summary>
    /// 轴Model
    /// </summary>
    public class AxisParamModel : BaseMotorModel
    {
        /// <summary>
        /// 轴别名
        /// </summary>
        //[XmlElement("aliasName")]
        [Category("通用配置")]
        [Description("轴别名")]
        public string aliasName { get; set; }

        /// <summary>
        /// 卡索引
        /// </summary>
        //[XmlElement("cardIndex")]  
        [Category("通用配置")]
        [Description("卡的Index")]
        public ushort cardIndex { get; set; }

        /// <summary>
        /// 轴索引
        /// </summary>
       // [XmlElement("axisIndex")]
        [Category("通用配置")]
        [Description("轴的Index")]
        public ushort axisIndex { get; set; }

        /// <summary>
        /// 关联轴索引
        /// </summary>
       // [XmlElement("releAxisIndex")]
        [Category("通用配置")]
        [Description("关联轴的Index")]
        public short releAxisIndex { get; set; }

        /// <summary>
        /// 参考 EM_AXIS_TYPE 0-伺服, 1步进, 2 dd马达参考 EM_AXIS_TYPE 0-伺服, 1步进, 2 dd马达
        /// </summary> 
        //[XmlElement("motorType")]
        [Category("通用配置")]
        [Description("轴的类型")]
        public EnumMotorType motorType { get; set; }

        /// <summary>
        /// 回限位方式，EM_LIMIT_TYPE 0 负限位 1 回正限位 2 无限位
        /// </summary>
        //[XmlElement("limitType")]
        [Category("通用配置")]
        [Description("回限位方式")]
        public EnumLimitType limitType { get; set; }

        /// <summary>
        /// 回原方式 参考 EM_HOME_TYPE 0 正常 1 硬件捕获 2 index 3 自定义 dd马达类型时表示 回原操作IO 8:null,8:card,8:ioindex,8:extmodle
        /// </summary>
       // [XmlElement("homeType")]
        [Category("通用配置")]
        [Description("回原方式")]
        public EnumHomeType homeType { get; set; }

        /// <summary>
        /// 默认工作速度  这里所有速度都是 脉冲/毫秒
        /// </summary>
        //[XmlElement("vel")]
        [Category("通用配置")]
        [Description("工作速度")]
        public double vel { get; set; }

        /// <summary>
        /// 回原速度 速度加速度 都需要 / 1000
        /// </summary>
        // [XmlElement("homeVel")]
        [Category("通用配置")]
        [Description("回原速度")]
        public double homeVel { get; set; }

        /// <summary>
        /// 最大速度
        /// </summary>
        // [XmlElement("maxVel")]
        [Category("通用配置")]
        [Description("最大速度")]
        public double maxVel { get; set; }

        /// <summary>
        /// 最大加速度
        /// </summary>
       // [XmlElement("maxAcc")]
        [Category("通用配置")]
        [Description("最大加速度")]
        public double maxAcc { get; set; }

        /// <summary>
        /// 最大减速度
        /// </summary>
        //  [XmlElement("maxDec")]
        [Category("通用配置")]
        [Description("最大减速度")]
        public double maxDec { get; set; }

        /// <summary>
        /// 1mm对应多少脉冲
        /// </summary>
       // [XmlElement("stepvalue")]
        [Category("通用配置")]
        [Description("脉冲当量")]
        public double stepvalue { get; set; }

        /// <summary>
        /// 负限位脉冲
        /// </summary>
      //  [XmlElement("limitN")]
        [Category("通用配置")]
        [Description("负限位脉冲")]
        public long limitN { get; set; }

        /// <summary>
        /// 正限位脉冲
        /// </summary>
       // [XmlElement("limitP")]
        [Category("通用配置")]
        [Description("正限位脉冲")]
        public long limitP { get; set; }

        /// <summary>
        /// 回原搜索的距离 脉冲
        /// </summary>
       // [XmlElement("homePos")]
        [Category("通用配置")]
        [Description("回原二段速度")]
        public double homeSecondVel { get; set; }

        /// <summary>
        /// 原点偏移量 回原前，若处于原点位置，则偏移
        /// </summary>
        //[XmlElement("homePos")]
        [Category("通用配置")]
        [Description("原点偏移量")]
        public long iInHomeOffset { get; set; }

        /// <summary>
        /// 回原后偏移量
        /// </summary>
        //[XmlElement("iAfterhomeOffset")]
        [Category("通用配置")]
        [Description("回原后偏移量")]
        public long iAfterhomeOffset { get; set; }

        /// <summary>
        /// 运动绝对位置
        /// </summary>
        public double pos { get; set; }

        /// <summary>
        /// 运动相对位置
        /// </summary>
        public double relPos { get; set; }

        /// <summary>
        /// 运动方向 dir -0 正向 1 负向
        /// </summary>
        public int dir { get; set; }

        /// <summary>
        /// 回零IO
        /// </summary>
        public uint homeIo { get; set; }

        public AxisParamModel()
        {
            homeSecondVel = 10;
        }

    }

    /// <summary>
    /// 轴类型枚举
    /// </summary>
    public enum EnumMotorType
    {
        伺服,
        步进,
        DD马达,
        流水线电机,
    }

    /// <summary>
    /// 回原方式枚举
    /// </summary>
    public enum EnumHomeType
    {
        普通回原,
        捕获回原,
        精确回原,
        自定义回原
    }

    /// <summary>
    /// 回限位方式枚举
    /// </summary>
    public enum EnumLimitType
    {
        回正限位,
        回负限位,
        无限位
    }

    /// <summary>
    /// 扩展卡Model
    /// </summary>
    public class ExtIoModel : BaseMotorModel
    {
        /// <summary>
        /// Io数量
        /// </summary>
        [XmlElement("IoNum")]
        public int IoNum { get; set; }
    }
    
    /// <summary>
    /// 工位Model
    /// </summary>
    public class StationModel : BaseMotorModel
    {
        [XmlElement("Id")]
        public override int Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                base.Id = value;
            }
        }

        [XmlElement("Name")]
        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                base.Name = value;
            }
        } 

        /// <summary>
        /// 轴数量
        /// </summary>
        [XmlElement("AxisNum")]
        public int AxisNum { get; set; }

        /// <summary>
        /// 检测IO，满足IO信号工站可移动
        /// </summary>
        public IOModel CheckIOModel { get; set; }

        /// <summary>
        /// 轴X
        /// </summary>
        [XmlElement("Axis_X")]
        public AxisParamModel Axis_X { get; set; }

        /// <summary>
        /// 轴Y
        /// </summary>
        [XmlElement("Axis_Y")]
        public AxisParamModel Axis_Y { get; set; }

        /// <summary>
        /// 轴Z
        /// </summary>
        [XmlElement("Axis_Z")]
        public AxisParamModel Axis_Z { get; set; }

        /// <summary>
        /// 轴U
        /// </summary>
        [XmlElement("Axis_U")]
        public AxisParamModel Axis_U { get; set; }

        /// <summary>
        /// 点位
        /// </summary>
        [XmlArrayItem("PointModels")]
        public List<PointModel> PointModels { get; set; }

        /// <summary>
        /// 运行速度百分比
        /// </summary>
        public double PercentSpeed { get; set; }

        public StationModel()
        {
            AxisNum = 0;
            Axis_X = null;
            Axis_Y = null;
            Axis_Z = null;
            Axis_U = null;
            PercentSpeed = 0.5;
            CheckIOModel = null;
            PointModels = new List<PointModel>();
        }
    }
    
    /// <summary>
    /// 点位Model
    /// </summary>
    public class PointModel : BaseMotorModel
    {
        /// <summary>
        /// Axis_X 位置
        /// </summary>
        public double Pos_X { get; set; }

        /// <summary>
        /// Axis_Y 位置
        /// </summary>
        public double Pos_Y { get; set; }

        /// <summary>
        /// Axis_Z 位置
        /// </summary>
        public double Pos_Z { get; set; }

        /// <summary>
        /// Axis_U 位置
        /// </summary>
        public double Pos_U { get; set; }


        /// <summary>
        /// Axis_X 位置最小值
        /// </summary>
        public double pos_X_Min { get; set; }

        /// <summary>
        /// Axis_X 位置最大值
        /// </summary>
        public double pos_X_Max { get; set; }

        /// <summary>
        /// Axis_Y 位置最小值
        /// </summary>
        public double pos_Y_Min { get; set; }

        /// <summary>
        /// Axis_Y 位置最大值
        /// </summary>
        public double pos_Y_Max { get; set; }

        /// <summary>
        /// Axis_Z 位置最小值
        /// </summary>
        public double pos_Z_Min { get; set; }

        /// <summary>
        /// Axis_Z 位置最大值
        /// </summary>
        public double pos_Z_Max { get; set; }

        /// <summary>
        /// Axis_U 位置最小值
        /// </summary>
        public double pos_U_Min { get; set; }

        /// <summary>
        /// Axis_U 位置最大值
        /// </summary>
        public double pos_U_Max { get; set; } 

        /// <summary>
        /// 点位所在的工站
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 判断是否合法
        /// </summary>
        public bool JudgeIllegal()
        {
            try
            {
                if (Pos_X < pos_X_Min || Pos_X > pos_X_Max)
                {
                    return false;
                }

                if (Pos_Y < pos_Y_Min || Pos_Y > pos_Y_Max)
                {
                    return false;
                }

                if (Pos_Z < pos_Z_Min || Pos_Z > pos_Z_Max)
                {
                    return false;
                }

                if (Pos_U < pos_U_Min || Pos_U > pos_U_Max)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PointModel Clone()
        {
            PointModel tModel = new PointModel();
            tModel.Pos_X = Pos_X;
            tModel.Pos_Y = Pos_Y;
            tModel.Pos_Z = Pos_Z;
            tModel.Pos_U = Pos_U;
            tModel.pos_X_Min = pos_X_Min;
            tModel.pos_Y_Min = pos_Y_Min;
            tModel.pos_Z_Min = pos_Z_Min;
            tModel.pos_U_Min = pos_U_Min;
            tModel.pos_X_Max = pos_X_Max;
            tModel.pos_Y_Max = pos_Y_Max;
            tModel.pos_Z_Max = pos_Z_Max;
            tModel.pos_U_Max = pos_U_Max;
            tModel.StationName = StationName;

            return tModel;
        }

    }

    /// <summary>
    /// IO Model
    /// </summary>
    public class IOModel : BaseMotorModel
    {
        /// <summary>
        /// 卡Index
        /// </summary>
        public int cardIndex { get; set; }

        /// <summary>
        /// 扩展模块Index
        /// </summary>
        public int extIndex { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 类型 通用输入/通用输出
        /// </summary>
        public EnumIO enumIo { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        public EnumIOType enumIoType { get; set; }

        /// <summary>
        /// 是否取反 true:取反 false:正常
        /// </summary>
        public bool bReverse { get; set; }

        /// <summary>
        /// 输入值
        /// </summary>
        [XmlIgnore] 
        public ushort val;

    }

    /// <summary>
    /// 关联IO Model
    /// </summary>
    public class RelatIoModel : BaseMotorModel
    {
        /// <summary>
        /// 输出IO
        /// </summary>
        public IOModel OutIoModel { get; set; }
        /// <summary>
        /// 响应输入IO_1
        /// </summary>
        public IOModel InIoModel1 { get; set; }
        /// <summary>
        /// 响应输入IO2
        /// </summary>
        public IOModel InIoModel2 { get; set; }

        /// <summary>
        /// 是否等待In完成
        /// </summary>
        public bool IsWaitTimeOut { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; }
    }



}
