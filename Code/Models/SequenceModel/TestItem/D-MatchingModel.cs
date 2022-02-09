using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 定位工具 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 定位(模板匹配)Model
    /// </summary>
    public class FixedItemModel : BaseSeqModel, ICreateModel
    {
        /// <summary>
        /// 图像来源
        /// </summary>
        public string ImageForm { get; set; }
        /// <summary>
        /// 开始角度
        /// </summary>
        public double StartingAngle { get; set; }
        /// <summary>
        /// 角度范围
        /// </summary>
        public double AngleExtent { get; set; }
        /// <summary>
        /// 金字塔等级
        /// </summary>
        public int PyramidLevel { get; set; }
        /// <summary>
        ///最大金字塔级别,确定速度
        /// </summary>
        public int LastPyramidLevel { get; set; }
        /// <summary>
        /// 最小分数
        /// </summary>
        public double MinScore { get; set; }
        /// <summary>
        /// 匹配个数
        /// </summary>
		public int NumMatches { get; set; }
        /// <summary>
        /// 贪心算法
        /// </summary>
        ///
        public double Greediness { get; set; }

        /// <summary>
        /// 自动NumLevel
        /// </summary>
        public bool AutoNumLevel { get; set; }

        /// <summary>
        /// 是否局部搜索区域
        /// </summary>
        public bool IsSearchArea { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.特征匹配.ToString();
            }

            set
            {
                value = FeatureType.特征匹配.ToString();
            }
        }

        public FixedItemModel()
        {
            itemResult = new ItemResult();
        }

        public FixedItemModel Clone()
        {
            FixedItemModel tModel = new FixedItemModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.StartingAngle = StartingAngle;
            tModel.AngleExtent = AngleExtent;
            tModel.PyramidLevel = PyramidLevel;
            tModel.LastPyramidLevel = LastPyramidLevel;
            tModel.MinScore = MinScore;
            tModel.NumMatches = NumMatches;
            tModel.Greediness = Greediness;
            tModel.AutoNumLevel = AutoNumLevel;
            tModel.IsSearchArea = IsSearchArea;

            return tModel;
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度(弧度)")]
            public double Angle { get; set; }

            [Description("模板个数")]
            public int Count { get; set; }
        }
    }

    /// <summary>
    /// 轮廓匹配Model
    /// </summary>
    public class MatchingModel : BaseSeqModel, ICreateModel
    {
        public string ImageForm { get; set; }

        /// <summary>
        /// 是否设置参考点
        /// </summary>
        public bool IsSetPoint { get; set; }

        #region 创建模板参数
         
        /// <summary>
        /// 金字塔等级
        /// </summary>
        public int PyramidLevel { get; set; }
        public bool IsPyramidLevelAuto { get; set; }
        /// <summary>
        /// 对比度
        /// </summary>
        public int Contrast { get; set; }
        public bool IsContrastAuto { get; set; }
        /// <summary>
        /// 最小缩放倍率
        /// </summary>
        public double MinScale { get; set; }
        /// <summary>
        ///最大缩放倍率
        /// </summary>
        public double MaxScale { get; set; }
        /// <summary>
        /// 缩放步长
        /// </summary>
        public double ScaleStep { get; set; }

        public bool IsScaleStepAuto { get; set; }
        /// <summary>
        /// 开始角度
        /// </summary>
        public double StartingAngle { get; set; }
        /// <summary>
        /// 角度范围
        /// </summary>
        public double AngleExtent { get; set; }
        /// <summary>
        /// 角度步长
        /// </summary>
        public double AngleStep { get; set; }

        public bool IsAngleStepAuto { get; set; }
        /// <summary>
        /// 最小对比度
        /// </summary>
        public int MinContrast { get; set; }
        public bool IsMinContrastAuto { get; set; }
        /// <summary>
        /// 度量方式
        /// </summary>
        public string Metric { get; set; }
        /// <summary>
        ///最优化方式
        /// </summary>
        public string Optimization { get; set; }
        public bool IsOptimizationAuto { get; set; }
        #endregion

        #region 查找模板参数
        /// <summary>
        /// 最小分数
        /// </summary>
        public double MinScore { get; set; }
        /// <summary>
        /// 匹配个数
        /// </summary>
		public int NumMatches { get; set; }
        /// <summary>
        /// 贪心算法
        /// </summary>
		public double Greediness { get; set; }
        /// <summary>
        /// 最大重叠
        /// </summary>
		public double MaxOverlap { get; set; }
        /// <summary>
        /// 亚像素模式
        /// </summary>
		public string Subpixel { get; set; }
        /// <summary>
        ///最大金字塔级别,确定速度
        /// </summary>
		public int LastPyramidLevel { get; set; }

        public bool IsLimitAngle { get; set; }

        public bool IsSearchArea { get; set; }

        #endregion

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.轮廓匹配.ToString();
            }

            set
            {
                value = FeatureType.轮廓匹配.ToString();
            }
        }

        public MatchingModel()
        {
            itemResult = new ItemResult();
        }

        public MatchingModel Clone()
        {
            MatchingModel tModel = new MatchingModel();
            tModel.ImageForm = ImageForm;
            tModel.IsSetPoint = IsSetPoint;
            tModel.PyramidLevel = PyramidLevel;
            tModel.IsPyramidLevelAuto = IsPyramidLevelAuto;
            tModel.Contrast = Contrast;
            tModel.IsContrastAuto = IsContrastAuto;
            tModel.MinScale = MinScale;
            tModel.MaxScale = MaxScale;
            tModel.ScaleStep = ScaleStep;
            tModel.IsScaleStepAuto = IsScaleStepAuto;
            tModel.StartingAngle = StartingAngle;
            tModel.AngleExtent = AngleExtent;
            tModel.AngleStep = AngleStep;
            tModel.IsAngleStepAuto = IsAngleStepAuto;
            tModel.MinContrast = MinContrast;
            tModel.IsMinContrastAuto = IsMinContrastAuto;
            tModel.Metric = Metric;
            tModel.Optimization = Optimization;
            tModel.IsOptimizationAuto = IsOptimizationAuto;
            tModel.MinScore = MinScore;
            tModel.NumMatches = NumMatches;
            tModel.Greediness = Greediness;
            tModel.MaxOverlap = MaxOverlap;
            tModel.Subpixel = Subpixel;
            tModel.LastPyramidLevel = LastPyramidLevel;
            tModel.IsSearchArea = IsSearchArea;
            tModel.IsLimitAngle = IsLimitAngle;

            return tModel;
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度(弧度)")]
            public double Angle { get; set; }

            [Description("模板个数")]
            public double Count { get; set; }

            [Description("模板中心Row数组")]
            public double[] AllRow { get; set; }

            [Description("模板中心Column数组")]
            public double[] AllCol { get; set; }

            [Description("模板角度数组")]
            public double[] AllAngle { get; set; }
        }
    }

    /// <summary>
    /// 灰度匹配Model
    /// </summary>
    public class NccMatchingModel : BaseSeqModel, ICreateModel
    {
        public string ImageForm { get; set; }

        public double AngleStart { get; set; }

        public double AngleExtent { get; set; }

        public int NumLevels { get; set; }

        public bool IsAutoNumLevels { get; set; }

        public double MinScore { get; set; }

        public int NumMatches { get; set; }

        public Level Accuracy { get; set; }

        /// <summary>
        /// 最大重叠
        /// </summary>
        public double MaxOverlap { get; set; }
        /// <summary>
        /// 亚像素模式
        /// </summary>
		public string Subpixel { get; set; }
        /// <summary>
        ///最大金字塔级别,确定速度
        /// </summary>
		public int LastPyramidLevel { get; set; }
         
        public bool IsSearchArea { get; set; }

        public bool IsLimitAngle { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.灰度匹配.ToString();
            }

            set
            {
                value = FeatureType.灰度匹配.ToString();
            }
        }

        public NccMatchingModel()
        {
            itemResult = new ItemResult();
        }

        public NccMatchingModel Clone()
        {
            NccMatchingModel tModel = new NccMatchingModel();
            tModel.ImageForm = ImageForm;
            tModel.AngleStart = AngleStart;
            tModel.AngleExtent = AngleExtent;
            tModel.IsAutoNumLevels = IsAutoNumLevels;
            tModel.MinScore = MinScore;
            tModel.NumMatches = NumMatches;
            tModel.Accuracy = Accuracy;
            tModel.MaxOverlap = MaxOverlap;
            tModel.Subpixel = Subpixel;
            tModel.LastPyramidLevel = LastPyramidLevel;
            tModel.IsSearchArea = IsSearchArea;
            tModel.IsLimitAngle = IsLimitAngle;

            return tModel;
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度")]
            public double Angle { get; set; }

            [Description("模板个数")]
            public double Count { get; set; }
        }

    }

    /// <summary>
    /// 交点匹配Model
    /// </summary>
    public class LinePMatchModel : BaseSeqModel, ICreateModel
    {
        public string ImageForm { get; set; }

        public string LinePForm { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.交点匹配.ToString();
            }

            set
            {
                value = FeatureType.交点匹配.ToString();
            }
        }

        public LinePMatchModel()
        {
            itemResult = new ItemResult();
        }

        public LinePMatchModel Clone()
        {
            LinePMatchModel tModel = new LinePMatchModel();

            return tModel;
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度")]
            public double Angle { get; set; } 
        }

    }


    /// <summary>
    /// 直线匹配Model
    /// </summary>
    public class LineMatchModel : BaseSeqModel, ICreateModel
    {
        public string ImageForm { get; set; }

        public string CenterRowForm { get; set; }
        public string CenterColForm { get; set; }
        public string AngleForm { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.直线匹配.ToString();
            }

            set
            {
                value = FeatureType.直线匹配.ToString();
            }
        }

        public LineMatchModel()
        {
            itemResult = new ItemResult();
        }

        public LineMatchModel Clone()
        {
            LineMatchModel tModel = new LineMatchModel();
            tModel.ImageForm = ImageForm;
            tModel.CenterRowForm = CenterRowForm;
            tModel.CenterColForm = CenterColForm;
            tModel.AngleForm = AngleForm;

            return tModel;
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度(弧度)")]
            public double Angle { get; set; }
        }

    }



    /// <summary>
    /// 手绘模板Model
    /// </summary>
    public class DrawMatchModel : BaseSeqModel, ICreateModel
    {
        /// <summary>
        /// 图像来源
        /// </summary>
        public string ImageForm { get; set; }
        /// <summary>
        /// 开始角度
        /// </summary>
        public double StartingAngle { get; set; }
        /// <summary>
        /// 角度范围
        /// </summary>
        public double AngleExtent { get; set; }
        /// <summary>
        /// 金字塔等级
        /// </summary>
        public int PyramidLevel { get; set; }
        /// <summary>
        ///最大金字塔级别,确定速度
        /// </summary>
        public int LastPyramidLevel { get; set; }
        /// <summary>
        /// 最小分数
        /// </summary>
        public double MinScore { get; set; }
        /// <summary>
        /// 匹配个数
        /// </summary>
		public int NumMatches { get; set; }
        /// <summary>
        /// 贪心算法
        /// </summary>
        ///
        public double Greediness { get; set; }

        /// <summary>
        /// 自动NumLevel
        /// </summary>
        public bool AutoNumLevel { get; set; }

        /// <summary>
        /// 是否局部搜索区域
        /// </summary>
        public bool IsSearchArea { get; set; }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.手绘模板.ToString();
            }

            set
            {
                value = FeatureType.手绘模板.ToString();
            }
        }

        public DrawMatchModel()
        {
            itemResult = new ItemResult();
        }

        public DrawMatchModel Clone()
        {
            DrawMatchModel tModel = new DrawMatchModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.StartingAngle = StartingAngle;
            tModel.AngleExtent = AngleExtent;
            tModel.PyramidLevel = PyramidLevel;
            tModel.LastPyramidLevel = LastPyramidLevel;
            tModel.MinScore = MinScore;
            tModel.NumMatches = NumMatches;
            tModel.Greediness = Greediness;
            tModel.AutoNumLevel = AutoNumLevel;
            tModel.IsSearchArea = IsSearchArea;

            return tModel;
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("模板中心Row")]
            public double Row { get; set; }

            [Description("模板中心Column")]
            public double Column { get; set; }

            [Description("模板角度(弧度)")]
            public double Angle { get; set; }

            [Description("模板个数")]
            public int Count { get; set; }
        }
    }

    public enum Level
    {
        use_polarity = 0,
        ignore_global_polarity
    }


    /// <summary>
    /// 斑点检测Model
    /// </summary>
    public class SpotCheckModel : BaseSeqModel, ICreateModel
    {
        public string ImageForm { get; set; }

        public string RegionForm { get; set; }
        
        /// <summary>
        /// 极性
        /// </summary>
        public string Polar { get; set; }
        public bool IsAutoThr { get; set; }
        public int MinGray { get; set; }
        public int MaxGray { get; set; }

        public int OutPutNum { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// 检测模式
        /// </summary>
        public string CheckMode { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 排序模式
        /// </summary>
        public string SortMode { get; set; }

        /// <summary>
        /// 输出模式
        /// </summary>
        public string OutPutMode { get; set; }

        public bool IsSpiltArea { get; set; }
        public double MinArea { get; set; }
        public double MaxArea { get; set; }
        public bool IsSpiltCircle { get; set; }
        public double MinCircle { get; set; }
        public double MaxCircle { get; set; }
        public bool IsSpiltRect { get; set; }
        public double MinRect { get; set; }
        public double MaxRect { get; set; }

        public List<ShapeClass> ListShape { get; set; }

        public SpotCheckModel Clone()
        {
            SpotCheckModel tModel = new SpotCheckModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.Polar = Polar;
            tModel.IsAutoThr = IsAutoThr;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;

            tModel.OutPutNum = OutPutNum;
            tModel.SortType = SortType;
            tModel.CheckMode = CheckMode;
            tModel.SortOrder = SortOrder;
            tModel.SortMode = SortMode;
            tModel.OutPutMode = OutPutMode;
            tModel.IsSpiltArea = IsSpiltArea;
            tModel.MinArea = MinArea;
            tModel.MaxArea = MaxArea;
            tModel.IsSpiltCircle = IsSpiltCircle;
            tModel.MinCircle = MinCircle;
            tModel.MaxCircle = MaxCircle;
            tModel.IsSpiltRect = IsSpiltRect;
            tModel.MinRect = MinRect;
            tModel.MaxRect = MaxRect;

            tModel.ListShape = ListShape;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.斑点检测.ToString();
            }

            set
            {
                value = FeatureType.斑点检测.ToString();
            }
        }

        public SpotCheckModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("斑点输出Region")]
            public HObject OutRegion { get; set; }

            [Description("斑点个数")]
            public int OutNum { get; set; }
        }

        /// <summary>
        /// 形态学
        /// </summary>
        public class ShapeClass
        {
            public int Id { get; set; }
            public string ShapeType { get; set; }
            public int ShapeNum { get; set; } 
        }
    }
}
