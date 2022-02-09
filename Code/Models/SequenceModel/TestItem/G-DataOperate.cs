using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 数据操作 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 数据CSV保存Model
    /// </summary>
    public class DataSaveModel : BaseSeqModel, IDataOperate
    {
        public string FileName { get; set; }
        public bool IsAddTime { get; set; }
        public string FilePath { get; set; }
        public List<string> listVar { get; set; }
        public List<string> listDesc { get; set; }
        public DataSaveModel Clone()
        {
            DataSaveModel tModel = new DataSaveModel();
            tModel.Name = Name;
            tModel.FileName = FileName;
            tModel.IsAddTime = IsAddTime;
            tModel.FilePath = FilePath;
            tModel.listVar = listVar;
            tModel.listDesc = listDesc;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数据保存.ToString();
            }

            set
            {
                value = FeatureType.数据保存.ToString();
            }
        }

        public DataSaveModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }


    /// <summary>
    /// 数据运算 Model
    /// </summary>
    public class DataOperateModel : BaseSeqModel, IDataOperate
    {
        public string ExpressValue { get; set; }
        public DataOperateModel Clone()
        {
            DataOperateModel tModel = new DataOperateModel();
            tModel.Name = Name;
            tModel.ExpressValue = ExpressValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数据运算.ToString();
            }

            set
            {
                value = FeatureType.数据运算.ToString();
            }
        }

        public DataOperateModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("运算结果字符串")]
            public string OutResult { get; set; }

            [Description("运算的Double结果")]
            public double DOutResult { get; set; }
        }
    }

    /// <summary>
    /// 算表达式 Model
    /// </summary>
    public class ExpressModel : BaseSeqModel, IDataOperate
    {
        public bool IsSetResult { get; set; }
        public List<ExpressClass> ListExpress { get; set; }
        public ExpressModel Clone()
        {
            ExpressModel tModel = new ExpressModel();
            tModel.Name = Name;
            tModel.ListExpress = ListExpress;
            tModel.IsSetResult = IsSetResult;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.算表达式.ToString();
            }

            set
            {
                value = FeatureType.算表达式.ToString();
            }
        }

        public ExpressModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("表达式的结果true/false")]
            public bool Result { get; set; }
        }
    }


    /// <summary>
    /// H算法引擎Model
    /// </summary>
    public class HADevEngineModel : BaseSeqModel, IDataOperate
    {
        public string FileForm { get; set; }

        //程序是否直接运行
        public bool IsDirectRun { get; set; }

        public List<ParamSet> listParamSet { get; set; }

        public HADevEngineModel Clone()
        {
            HADevEngineModel tModel = new HADevEngineModel();
            tModel.Name = Name;
            tModel.FileForm = FileForm;
            tModel.IsDirectRun = IsDirectRun;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.算法引擎.ToString();
            }

            set
            {
                value = FeatureType.算法引擎.ToString();
            }
        }

        public HADevEngineModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("执行算法结果true/false")]
            public bool Result { get; set; }
        }

        public class ParamSet
        {
            public string FuncName { get; set; }

            public string Name { get; set; }

            public ParamType ParType { get; set; }

            public string Value { get; set; }

            public bool IsInput { get; set; }
        }

        /// <summary>
        /// 参数类型Enum
        /// </summary>
        public enum ParamType
        {
            数组,
            图像,
            轮廓,
            区域,
        }
    }


    /// <summary>
    /// 时间统计Model
    /// </summary>
    public class TimeComputeModel : BaseSeqModel, IDataOperate
    {
        public bool IsStart { get; set; }
        public string EndName { get; set; }

        [XmlIgnore]
        public Stopwatch sp { get; set; }

        public TimeComputeModel Clone()
        {
            TimeComputeModel tModel = new TimeComputeModel();
            tModel.Name = Name;
            tModel.IsStart = IsStart;
            tModel.EndName = EndName;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.时间统计.ToString();
            }

            set
            {
                value = FeatureType.时间统计.ToString();
            }
        }

        public TimeComputeModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("统计的时间 单位:s")]
            public double ComputeTime { get; set; }
        }
    }

    /// <summary>
    /// 脚本分析 Model
    /// </summary>
    public class ScriptModel : BaseSeqModel, IDataOperate
    {  
        public string ScriptText { get; set; }
        public ScriptModel Clone()
        {
            ScriptModel tModel = new ScriptModel();
            tModel.Name = Name;
            tModel.ScriptText = ScriptText;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.脚本分析.ToString();
            }

            set
            {
                value = FeatureType.脚本分析.ToString();
            }
        }

        public ScriptModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 

        }
    }

    /// <summary>
    /// 产品选择Model
    /// </summary>
    public class SelectProductModel : BaseSeqModel, IDataOperate
    { 
        
        public SelectProductModel Clone()
        {
            SelectProductModel tModel = new SelectProductModel();
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
                return FeatureType.产品选择.ToString();
            }

            set
            {
                value = FeatureType.产品选择.ToString();
            }
        }

        public SelectProductModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("最小值")]
            public double MinValue { get; set; }

            [Description("最大值")]
            public double MaxValue { get; set; }

            [Description("半径值")]
            public double Radius { get; set; }
        }
         
    }

    /// <summary>
    /// 脚本编辑 Model
    /// </summary>
    public class SharpScriptModel : BaseSeqModel, IDataOperate
    {
        public string ScriptText { get; set; }
        public SharpScriptModel Clone()
        {
            SharpScriptModel tModel = new SharpScriptModel();
            tModel.Name = Name;
            tModel.ScriptText = ScriptText;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.脚本编辑.ToString();
            }

            set
            {
                value = FeatureType.脚本编辑.ToString();
            }
        }

        public SharpScriptModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 显示数据Model
    /// </summary>
    public class DisplayDataModel : BaseSeqModel, IDataOperate
    {
        public string ImageForm { get; set; }
        public string DisplayValue { get; set; }
        public string FontSize { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }

        public string DrawColor { get; set; }
        public List<string> ListVar { get; set; }

        public DisplayDataModel Clone()
        {
            DisplayDataModel tModel = new DisplayDataModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.DisplayValue = DisplayValue;
            tModel.FontSize = FontSize;
            tModel.IsBold = IsBold;
            tModel.IsItalic = IsItalic;
            tModel.DrawColor = DrawColor;
            tModel.ListVar = ListVar;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.显示数据.ToString();
            }

            set
            {
                value = FeatureType.显示数据.ToString();
            }
        }

        public DisplayDataModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 系统时间Model
    /// </summary>
    public class SystemTimeModel : BaseSeqModel, IDataOperate
    { 
        public string TimeFormat { get; set; }
         
        public SystemTimeModel Clone()
        {
            SystemTimeModel tModel = new SystemTimeModel();
            tModel.Name = Name;
            tModel.TimeFormat = TimeFormat; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.系统时间.ToString();
            }

            set
            {
                value = FeatureType.系统时间.ToString();
            }
        }

        public SystemTimeModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("获取的系统时间")]
            public string OutTime { get; set; }
        }
    }

}
