using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    /// <summary>
    /// LD工站配置Model
    /// </summary>
    public class LDModel
    {
        /// <summary>
        /// 操作员ID
        /// </summary>
        public string OperatorID { get; set; }
        /// <summary>
        /// 机台ID
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// 屏蔽蜂鸣器
        /// </summary>
        public bool IsShieldBuzzer { get; set; }
        /// <summary>
        /// 屏蔽真空感应
        /// </summary>
        public bool IsShieldVacuum { get; set; }
        /// <summary>
        /// 屏蔽夹紧气缸
        /// </summary>
        public bool IsShieldClamp { get; set; }

        /// <summary>
        /// 屏蔽下相机
        /// </summary>
        public bool IsShieldUnClasp { get; set; }

        /// <summary>
        /// 空跑模式
        /// </summary>
        public bool IsEmptyRun { get; set; }
        /// <summary>
        /// 屏蔽安全门
        /// </summary>
        public bool IsShieldDoor { get; set; }
        /// <summary>
        /// 屏蔽下相机
        /// </summary>
        public bool IsShieldDownCamera { get; set; }
        /// <summary>
        /// 屏蔽上升气缸感应
        /// </summary>

        public bool IsShieldCylinderUp { get; set; }

        /// <summary>
        /// 屏蔽OCR
        /// </summary>
        public bool IsShieldOCR { get; set; }

        /// <summary>
        /// 屏蔽MES
        /// </summary>
        public bool IsShieldMes { get; set; }

        /// <summary>
        /// 料盘排列
        /// </summary>
        public List<EnumLDResult> listBlowType1 { get; set; }

        /// <summary>
        /// 2寸料盘参数配置
        /// </summary>
        public TrayModel TrayModelInch2 { get; set; }

        /// <summary>
        /// 拍照延迟时间
        /// </summary>
        public int SnapTimeOut { get; set; }
        /// <summary>
        /// 真空超时
        /// </summary>
        public int VacuumTimeOut { get; set; }
        /// <summary>
        /// 吸真空延迟时间
        /// </summary>
        public int VacuumDelayTime { get; set; }
        /// <summary>
        /// 轴到位超时
        /// </summary>
        public int AxisInPlaceTimeOut { get; set; }
        /// <summary>
        /// 轴到位延迟时间
        /// </summary>
        public int AxisInPlaceDelay { get; set; }
        /// <summary>
        /// 破真空延迟时间
        /// </summary>
        public int VacuumBreakDelay { get; set; }
        /// <summary>
        /// 气缸动作延迟时间
        /// </summary>
        public int CylinderDelay { get; set; }
        /// <summary>
        /// 连续NG计数
        /// </summary>
        public int ContinueNGCount { get; set; }

        /// <summary>
        /// 下料料盘Model
        /// </summary>
        public UnLoadTrayModel unLoadTrayModel { get; set; }

        /// <summary>
        /// 产品参数设置集合
        /// </summary>
        public List<ParamRangeModel> paramRangeModels { get; set; }

        /// <summary>
        /// 二维码输入集合
        /// </summary>
        public List<EnterQRModel> enterQRModels { get; set; }

        /// <summary>
        /// Map数据
        /// </summary>
        public MapModel mapModel { get; set; }


        public LDModel()
        {
            mapModel = new MapModel();
            enterQRModels = new List<EnterQRModel>();
            paramRangeModels = new List<ParamRangeModel>();
            unLoadTrayModel = new UnLoadTrayModel();
        }
    }

    /// <summary>
    /// 生产数据Model
    /// </summary>
    public class ProductDataModel
    {
        /// <summary>
        /// 上料总数
        /// </summary>
        public int LoadCount { get; set; }
        /// <summary>
        /// 下料总数
        /// </summary>
        public int UnLoadCount { get; set; }
        /// <summary>
        /// OK数量
        /// </summary>
        public int OKCount { get; set; }

        /// <summary>
        /// 扫码不良数量
        /// </summary>
        public int ScanNGCount { get; set; }

        /// <summary>
        /// 疑似不良数量
        /// </summary>
        public int SeemNGCount { get; set; }

        /// <summary>
        /// 成品不良数量
        /// </summary>
        public int NGCount { get; set; }

        /// <summary>
        /// 良率
        /// </summary>
        public double OKPercent { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public double CostTime { get; set; }
        /// <summary>
        /// 暂停时间
        /// </summary>
        public double PauseTime { get; set; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public double WarnTime { get; set; }

        /// <summary>
        /// 测试CT
        /// </summary>
        public double CT { get; set; }
    }

    /// <summary>
    /// 上料料盘Model
    /// </summary>
    public class TrayModel
    {
        /// <summary>
        /// 料盘行数
        /// </summary>
        public int TrayRowCount { get; set; }
        /// <summary>
        /// 料盘列数
        /// </summary>
        public int TrayColCount { get; set; }
        /// <summary>
        /// 料盘里物料行数
        /// </summary>
        public int ProductRowCount { get; set; }
        /// <summary>
        /// 料盘里物料列数
        /// </summary>
        public int ProductColCount { get; set; }
        /// <summary>
        /// 产品行间距
        /// </summary>
        public double ProductRowDis { get; set; }
        /// <summary>
        /// 产品列间距
        /// </summary>
        public double ProductColDis { get; set; }
        /// <summary>
        /// 料盘行间距
        /// </summary>
        public double TrayRowDis { get; set; }
        /// <summary>
        /// 料盘列间距
        /// </summary>
        public double TrayColDis { get; set; }
        /// <summary>
        /// 物料放置行数
        /// </summary>
        public int RowSetCount { get; set; }
        /// <summary>
        /// 物料放置列数
        /// </summary>
        public int ColSetCount { get; set; }
        /// <summary>
        /// 当前物料行
        /// </summary>
        public int ProductCurrentRow { get; set; }
        /// <summary>
        /// 当前物料列
        /// </summary>
        public int ProductCurrentCol { get; set; }
        /// <summary>
        /// 料盘当前行数
        /// </summary>
        public int TrayCurrentRow { get; set; }
        /// <summary>
        /// 料盘当前列数
        /// </summary>
        public int TrayCurrentCol { get; set; }
    }

    /// <summary>
    /// 下料料盘Model
    /// </summary>
    public class UnLoadTrayModel
    {
        /// <summary>
        /// 料盘里物料行数
        /// </summary>
        public int ProductRowCount { get; set; }
        /// <summary>
        /// 料盘里物料列数
        /// </summary>
        public int ProductColCount { get; set; }
        /// <summary>
        /// 料盘行间距
        /// </summary>
        public double TrayRowDis { get; set; }
        /// <summary>
        /// 料盘列间距
        /// </summary>
        public double TrayColDis { get; set; }
        /// <summary>
        /// 物料行间距
        /// </summary>
        public double ProductRowDis { get; set; }
        /// <summary>
        /// 物料列间距
        /// </summary>
        public double ProductColDis { get; set; }
        /// <summary>
        /// 料盘X向偏差
        /// </summary>
        public double TrayXOffSet { get; set; }
        /// <summary>
        /// 料盘Y向偏差
        /// </summary>
        public double TrayYOffSet { get; set; }

        /// <summary>
        /// OK Model
        /// </summary>
        public UnLoadModel PassModel { get; set; }
        /// <summary>
        /// NG Model
        /// </summary>
        public UnLoadModel FailModel { get; set; }

        /// <summary>
        /// 阈值电流不良的下料Model
        /// </summary>
        public UnLoadModel SeemNGModel { get; set; }

    }

    /// <summary>
    /// 下料单个料盘Model
    /// </summary>
    public class UnLoadModel
    {
        /// <summary>
        /// 料盘名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前物料行
        /// </summary>
        public int ProductCurrentRow { get; set; }
        /// <summary>
        /// 当前物料列
        /// </summary>
        public int ProductCurrentCol { get; set; }

        /// <summary>
        /// 料盘当前个数
        /// </summary>
        public int TrayCurrentNum { get; set; }
    }

    /// <summary>
    /// 参数范围Model
    /// </summary>
    public class ParamRangeModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// OCR设定个数
        /// </summary>
        public int OcrNum { get; set; }

    }

    /// <summary>
    /// 输入二维码Model
    /// </summary>
    public class EnterQRModel
    {
        public int Index { get; set; }
        public string QR1 { get; set; }
        public int QR1Num { get; set; }
        public string QR2 { get; set; }
        public int QR2Num { get; set; }
    }

    /// <summary>
    /// Map表Model
    /// </summary>
    public class MapModel
    {
        /// <summary>
        /// bar条取料总数
        /// </summary>
        public int BarCount { get; set; }
        /// <summary>
        /// 已取总数
        /// </summary>
        public int GetCount { get; set; }
        /// <summary>
        /// 当前OCR
        /// </summary>
        public string CurrentOcr { get; set; }
        /// <summary>
        /// bar条设置
        /// </summary>
        public string BarSet { get; set; }

        public List<SetMap> ListMap { get; set; }

        public MapModel()
        {
            ListMap = new List<SetMap>();
        }

        public class SetMap
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// Wafer是否有效
            /// </summary>
            public bool IsEffective { get; set; }

            /// <summary>
            /// Wafer号
            /// </summary>
            public string WaferNo { get; set; }
            /// <summary>
            /// Wafer路径
            /// </summary>
            public string WaferPath { get; set; }
        }

    }

    /// 芯片测试结果类型
    /// </summary>
    public enum EnumLDResult
    {
        /// <summary>
        /// 合格
        /// </summary>
        Enum_Pass,
        /// <summary>
        /// 不合格
        /// </summary>
        Enum_Fail,
        /// <summary>
        /// 疑似NG
        /// </summary>
        Enum_SeemNG,
    }

    public class PointClass
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double U { get; set; }
    }

    /// <summary>
    /// 报警代码
    /// </summary>
    public enum EAlarm
    {
        /// <summary>
        /// 轴未走到位，通知检查轴参数或者驱动器是否报警
        /// </summary>
        A_10001 = 10001,

        /// <summary>
        /// 气缸操作失败，请检查气路！！
        /// </summary>
        A_10002,

        /// <summary>
        /// 真空操作失败，请检查气路！！
        /// </summary>
        A_10003,

        /// <summary>
        /// 下料位置获取失败，请检查下料配置！！
        /// </summary>
        A_10004,

        /// <summary>
        /// 大视野相机入料拍照执行算法失败，请检查图像或者参数！！
        /// </summary>
        A_20001 = 20001,

        /// <summary>
        /// 大视野相机定位拍照执行算法失败！！
        /// </summary>
        A_20002,

        /// <summary>
        /// 小视野相机入料补正拍照执行算法失败！！
        /// </summary>
        A_20003,

        /// <summary>
        /// 小视野相机定位拍照执行算法失败！！
        /// </summary>
        A_20004,

        /// <summary>
        /// N面拍照执行算法失败！！
        /// </summary>
        A_20005,

        /// <summary>
        /// 校正位置1拍照执行算法失败！！
        /// </summary>
        A_20006,

        /// <summary>
        /// AR检测拍照执行算法失败！！
        /// </summary>
        A_20007,

        /// <summary>
        /// 校正位置2拍照执行算法失败！！
        /// </summary>
        A_20008,

        /// <summary>
        /// HR检测拍照执行算法失败！！
        /// </summary>
        A_20009,

        /// <summary>
        /// 吸蓝膜失败，请检查真空！！
        /// </summary>
        A_30001 = 30001,

        /// <summary>
        /// 上料吸嘴吸真空失败，请检查真空！！
        /// </summary>
        A_30002,

        /// <summary>
        /// 下料吸嘴吸真空失败，请检查真空！！
        /// </summary>
        A_30003,

        /// <summary>
        /// 下料吸嘴破真空失败，请检查真空！！
        /// </summary>
        A_30004,

        /// <summary>
        /// 顶升模组未在安全位置！！
        /// </summary>
        A_40001 = 40001,

        /// <summary>
        /// 上料模组X不在安全位置！！
        /// </summary>
        A_40002,

        /// <summary>
        /// 双目相机模组不在安全位置！！
        /// </summary>
        A_40003,

        /// <summary>
        /// 下料模组X不在安全位置！！
        /// </summary>
        A_40004,

        /// <summary>
        /// 上料模组Z不在安全位置！！
        /// </summary>
        A_40005,

        /// <summary>
        /// 下料模组Z不在安全位置！！
        /// </summary>
        A_40006,

    }
}
