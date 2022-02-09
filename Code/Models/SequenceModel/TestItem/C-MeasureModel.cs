using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 几何测量 集合
/// </summary>
namespace SequenceTestModel
{ 
    /// <summary>
    /// 找圆Model
    /// </summary>
    public class FindCircleModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }

        public int SearchRadius { get; set; }

        public int MeasureLength1 { get; set; }

        public int MeasureLength2 { get; set; }

        public string MeasureTransition { get; set; }

        public string MeasureSelect { get; set; }

        public int MeasureThreshold { get; set; }

        public int MeasureNumber { get; set; }

        public double MeasureScore { get; set; }

        public double Sigma { get; set; }

        public bool IsModelFollow { get; set; }
        public string ModelFollow { get; set; }
        public bool IsReal { get; set; }
        public string PixPrec { get; set; }

        public bool IsSetPoint { get; set; }
        public string RowFrom { get; set; }
        public string ColFrom { get; set; }
        public string RadiusFrom { get; set; }

        public bool IsJudgeRadius { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }

        public FindCircleModel Clone()
        {
            FindCircleModel tModel = new FindCircleModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.SearchRadius = SearchRadius;
            tModel.MeasureLength1 = MeasureLength1;
            tModel.MeasureLength2 = MeasureLength2;
            tModel.MeasureTransition = MeasureTransition;
            tModel.MeasureSelect = MeasureSelect;
            tModel.MeasureThreshold = MeasureThreshold;
            tModel.MeasureNumber = MeasureNumber;
            tModel.MeasureScore = MeasureScore;
            tModel.Sigma = Sigma;
            tModel.IsModelFollow = IsModelFollow;
            tModel.ModelFollow = ModelFollow;

            tModel.IsSetPoint = IsSetPoint;
            tModel.RowFrom = RowFrom;
            tModel.ColFrom = ColFrom;
            tModel.RadiusFrom = RadiusFrom;

            tModel.IsJudgeRadius = IsJudgeRadius;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.找圆.ToString();
            }

            set
            {
                value = FeatureType.找圆.ToString();
            }
        }

        public FindCircleModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("圆中心Row值")]
            public double CircleRow { get; set; }

            [Description("圆中心Column值")]
            public double CircleCol { get; set; }

            [Description("圆中心半径值")]
            public double CircleRadius { get; set; }

            [Description("圆中心Row数组")]
            public double[] AllRow { get; set; }

            [Description("圆中心Column数组")]
            public double[] AllCol { get; set; }

            [Description("圆中心半径数组")]
            public double[] AllRadius { get; set; }
        }

    }
    
    /// <summary>
    /// 找圆弧Model
    /// </summary>
    public class FindCircleArcModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }

        public int SearchRadius { get; set; }

        public int MeasureLength1 { get; set; }

        public int MeasureLength2 { get; set; }

        public string MeasureTransition { get; set; }

        public string MeasureSelect { get; set; }

        public int MeasureThreshold { get; set; }

        public int MeasureNumber { get; set; }

        public double MeasureScore { get; set; }

        public double Sigma { get; set; }

        public double StartPhi { get; set; }

        public double EndPhi { get; set; }

        public bool IsModelFollow { get; set; }
        public string ModelFollow { get; set; }

        public FindCircleArcModel Clone()
        {
            FindCircleArcModel tModel = new FindCircleArcModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.SearchRadius = SearchRadius;
            tModel.MeasureLength1 = MeasureLength1;
            tModel.MeasureLength2 = MeasureLength2;
            tModel.MeasureTransition = MeasureTransition;
            tModel.MeasureSelect = MeasureSelect;
            tModel.MeasureThreshold = MeasureThreshold;
            tModel.MeasureNumber = MeasureNumber;
            tModel.MeasureScore = MeasureScore;
            tModel.Sigma = Sigma;
            tModel.EndPhi = EndPhi;
            tModel.StartPhi = StartPhi;
            tModel.IsModelFollow = IsModelFollow;
            tModel.ModelFollow = ModelFollow;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.圆弧测量.ToString();
            }

            set
            {
                value = FeatureType.圆弧测量.ToString();
            }
        }

        public FindCircleArcModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("圆中心Row数组")]
            public double CircleRow { get; set; }

            [Description("圆中心Column值")]
            public double CircleCol { get; set; }

            [Description("圆中心半径数组")]
            public double CircleRadius { get; set; }
        }

    }

    /// <summary>
    /// 找线Model
    /// </summary>
    public class FindLineModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }

        public int MeasureLength1 { get; set; }

        public int MeasureLength2 { get; set; }

        public string MeasureTransition { get; set; }

        public string MeasureSelect { get; set; }

        public int MeasureThreshold { get; set; }

        public int MeasureNumber { get; set; }

        public double MeasureScore { get; set; }

        public double Sigma { get; set; }

        public bool IsModelFollow { get; set; }

        public string ModelFollow { get; set; }

        public bool IsShowPoint { get; set; }
        public bool IsShowLine { get; set; }
        public bool IsExtend { get; set; }

        public FindLineModel Clone()
        {
            FindLineModel tModel = new FindLineModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.MeasureLength1 = MeasureLength1;
            tModel.MeasureLength2 = MeasureLength2;
            tModel.MeasureTransition = MeasureTransition;
            tModel.MeasureSelect = MeasureSelect;
            tModel.MeasureThreshold = MeasureThreshold;
            tModel.MeasureNumber = MeasureNumber;
            tModel.MeasureScore = MeasureScore;
            tModel.Sigma = Sigma;
            tModel.IsModelFollow = IsModelFollow;
            tModel.ModelFollow = ModelFollow;
            tModel.IsShowLine = IsShowLine;
            tModel.IsShowPoint = IsShowPoint;
            tModel.IsExtend = IsExtend;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.找线.ToString();
            }

            set
            {
                value = FeatureType.找线.ToString();
            }
        }

        public FindLineModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("直线起始点Row")]
            public double LineBeginRow { get; set; }

            [Description("直线起始点Column")]
            public double LineBeginCol { get; set; }

            [Description("直线终点Row")]
            public double LineEndRow { get; set; }

            [Description("直线终点Column")]
            public double LineEndCol { get; set; }

            [Description("直线角度")]
            public double LineAngle { get; set; }


            [Description("直线拟合所有点Row")]
            public double[] AllRow { get; set; }

            [Description("直线拟合所有点Col")]
            public double[] AllCol { get; set; }             
        }

    }
    
    /// <summary>
    /// 一维测量Model
    /// </summary>
    public class OneMeasureModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }

        public int Threshold { get; set; }

        public int RoiWidth { get; set; }

        public double Sigma { get; set; }

        public string Transition { get; set; }

        public string Select { get; set; }

        public string LineRowStart { get; set; }

        public string LineColumnStart { get; set; }

        public string LineRowEnd { get; set; }

        public string LineColumnEnd { get; set; }

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }

        public OneMeasureModel Clone()
        {
            OneMeasureModel tModel = new OneMeasureModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.Threshold = Threshold;
            tModel.RoiWidth = RoiWidth;
            tModel.Sigma = Sigma;
            tModel.Transition = Transition;
            tModel.Select = Select;
            tModel.LineRowStart = LineRowStart;
            tModel.LineColumnStart = LineColumnStart;
            tModel.LineRowEnd = LineRowEnd;
            tModel.LineColumnEnd = LineColumnEnd;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.一维测量.ToString();
            }

            set
            {
                value = FeatureType.一维测量.ToString();
            }
        }

        public OneMeasureModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("一维测量Row数组")]
            public double[] MeasureRow { get; set; }

            [Description("一维测量Column数组")]
            public double[] MeasureCol { get; set; }

            [Description("一维测量Distance数组")]
            public double[] MeasureDis { get; set; }
        }

    }

    /// <summary>
    /// 检测同心度Model
    /// </summary>
    public class CircleDDModel : BaseSeqModel, IMeasure
    {
        public string FindCircle1Name { get; set; }

        public string FindCircle2Name { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public bool IsOneCircle { get; set; } 

        public CircleDDModel Clone()
        {
            CircleDDModel tModel = new CircleDDModel();
            tModel.Name = Name; 
            tModel.FindCircle1Name = FindCircle1Name;
            tModel.FindCircle2Name = FindCircle2Name;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.IsOneCircle = IsOneCircle; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.圆心距.ToString();
            }

            set
            {
                value = FeatureType.圆心距.ToString();
            }
        }

        public CircleDDModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("同心度结果")]
            public double CircleDDValue { get; set; }

            [Description("同心度结果数组")]
            public double[] AllCircleDD { get; set; }
        }
    }
        
    /// <summary>
    /// 两线距离Model
    /// </summary>
    public class LineDDModel : BaseSeqModel, IMeasure
    {
        public string Line1Row1 { get; set; }
        public string Line1Row2 { get; set; }
        public string Line1Col1 { get; set; }
        public string Line1Col2 { get; set; }

        public string Line2Row1 { get; set; }
        public string Line2Row2 { get; set; }
        public string Line2Col1 { get; set; }
        public string Line2Col2 { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; } 

        public LineDDModel Clone()
        {
            LineDDModel tModel = new LineDDModel();
            tModel.Name = Name;
            tModel.Line1Row1 = Line1Row1;
            tModel.Line1Row2 = Line1Row2;
            tModel.Line1Col1 = Line1Col1;
            tModel.Line1Col2 = Line1Col2;
            tModel.Line2Row1 = Line2Row1;
            tModel.Line2Row2 = Line2Row2;
            tModel.Line2Col1 = Line2Col1;
            tModel.Line2Col2 = Line2Col2;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.两线距离.ToString();
            }

            set
            {
                value = FeatureType.两线距离.ToString();
            }
        }

        public LineDDModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("两线距离结果")]
            public double LineDDValue { get; set; }
        }
    }

    /// <summary>
    /// 线圆交点Model
    /// </summary>
    public class LCPointCrossModel : BaseSeqModel, IMeasure
    {
        public string LineRow1 { get; set; }
        public string LineRow2 { get; set; }
        public string LineCol1 { get; set; }
        public string LineCol2 { get; set; }

        public string CircleRow1 { get; set; }
        public string CircleColumn1 { get; set; }
        public string CircleRadius1 { get; set; }

        public LCPointCrossModel Clone()
        {
            LCPointCrossModel tModel = new LCPointCrossModel();
            tModel.Name = Name;
            tModel.LineRow1 = LineRow1;
            tModel.LineRow2 = LineRow2;
            tModel.LineCol1 = LineCol1;
            tModel.LineCol2 = LineCol2;
            tModel.CircleRow1 = CircleRow1;
            tModel.CircleColumn1 = CircleColumn1;
            tModel.CircleRadius1 = CircleRadius1;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.线圆交点.ToString();
            }

            set
            {
                value = FeatureType.线圆交点.ToString();
            }
        }

        public LCPointCrossModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("线圆交点Row1")]
            public double Row1 { get; set; }

            [Description("线圆交点Column1")]
            public double Column1 { get; set; }

            [Description("线圆交点Row2")]
            public double Row2 { get; set; }

            [Description("线圆交点Column2")]
            public double Column2 { get; set; }

        }
    }

    /// <summary>
    /// 两线交点Model
    /// </summary>
    public class LLPointCrossModel : BaseSeqModel, IMeasure
    {
        public string Line1Row1 { get; set; }
        public string Line1Row2 { get; set; }
        public string Line1Col1 { get; set; }
        public string Line1Col2 { get; set; }

        public string Line2Row1 { get; set; }
        public string Line2Row2 { get; set; }
        public string Line2Col1 { get; set; }
        public string Line2Col2 { get; set; }
        public LLPointCrossModel Clone()
        {
            LLPointCrossModel tModel = new LLPointCrossModel();
            tModel.Name = Name;
            tModel.Line1Row1 = Line1Row1;
            tModel.Line1Row2 = Line1Row2;
            tModel.Line1Col1 = Line1Col1;
            tModel.Line1Col2 = Line1Col2;
            tModel.Line2Row1 = Line2Row1;
            tModel.Line2Row2 = Line2Row2;
            tModel.Line2Col1 = Line2Col1;
            tModel.Line2Col2 = Line2Col2;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.两线交点.ToString();
            }

            set
            {
                value = FeatureType.两线交点.ToString();
            }
        }

        public LLPointCrossModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("两线交点Row")]
            public double Row { get; set; }

            [Description("两线交点Column")]
            public double Column { get; set; }
        }
    }

    /// <summary>
    /// 两线角度Model
    /// </summary>
    public class LLAngleModel : BaseSeqModel, IMeasure
    {
        public string Line1Row1 { get; set; }
        public string Line1Row2 { get; set; }
        public string Line1Col1 { get; set; }
        public string Line1Col2 { get; set; }

        public string Line2Row1 { get; set; }
        public string Line2Row2 { get; set; }
        public string Line2Col1 { get; set; }
        public string Line2Col2 { get; set; }


        public LLAngleModel Clone()
        {
            LLAngleModel tModel = new LLAngleModel();
            tModel.Name = Name;
            tModel.Line1Row1 = Line1Row1;
            tModel.Line1Row2 = Line1Row2;
            tModel.Line1Col1 = Line1Col1;
            tModel.Line1Col2 = Line1Col2;
            tModel.Line2Row1 = Line2Row1;
            tModel.Line2Row2 = Line2Row2;
            tModel.Line2Col1 = Line2Col1;
            tModel.Line2Col2 = Line2Col2;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.两线角度.ToString();
            }

            set
            {
                value = FeatureType.两线角度.ToString();
            }
        }

        public LLAngleModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("两线角度")]
            public double Angle { get; set; }
        }
    }

    /// <summary>
    /// 圆与圆的距离Model
    /// </summary>
    public class CCDistanceModel : BaseSeqModel, IMeasure
    {
        public string CircleRow1 { get; set; }
        public string CircleColumn1 { get; set; }
        public string CircleRadius1 { get; set; }

        public string CircleRow2 { get; set; }
        public string CircleColumn2 { get; set; }
        public string CircleRadius2 { get; set; }


        public CCDistanceModel Clone()
        {
            CCDistanceModel tModel = new CCDistanceModel();
            tModel.Name = Name;
            tModel.CircleRow1 = CircleRow1;
            tModel.CircleColumn1 = CircleColumn1;
            tModel.CircleRadius1 = CircleRadius1;
            tModel.CircleRow2 = CircleRow2;
            tModel.CircleColumn2 = CircleColumn2;
            tModel.CircleRadius2 = CircleRadius2;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.两圆距离.ToString();
            }

            set
            {
                value = FeatureType.两圆距离.ToString();
            }
        }

        public CCDistanceModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("两圆的距离")]
            public double Distance { get; set; }
        }
    }
    
    /// <summary>
    /// 点与直线的距离Model
    /// </summary>
    public class PLDistanceModel : BaseSeqModel, IMeasure
    {
        public string PointRow { get; set; }
        public string PointColumn { get; set; }

        public string LineRow1 { get; set; }
        public string LineCol1 { get; set; }
        public string LineRow2 { get; set; }
        public string LineCol2 { get; set; }

        public bool IsManyPoint { get; set; }

        public string ValueSelect { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public double ScaleValue { get; set; }

        public double OffSetValue { get; set; }

        public PLDistanceModel Clone()
        {
            PLDistanceModel tModel = new PLDistanceModel();
            tModel.Name = Name;
            tModel.PointRow = PointRow;
            tModel.PointColumn = PointColumn;
            tModel.LineRow1 = LineRow1;
            tModel.LineCol1 = LineCol1;
            tModel.LineRow2 = LineRow2;
            tModel.LineCol2 = LineCol2;
            tModel.IsManyPoint = IsManyPoint;
            tModel.ValueSelect = ValueSelect;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.ScaleValue = ScaleValue;
            tModel.OffSetValue = OffSetValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点线距离.ToString();
            }

            set
            {
                value = FeatureType.点线距离.ToString();
            }
        }

        public PLDistanceModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("点线的距离")]
            public double Distance { get; set; }
        }
    }
    
    /// <summary>
    /// 点与圆的距离Model
    /// </summary>
    public class PCDistanceModel : BaseSeqModel, IMeasure
    {
        public string PointRow { get; set; }
        public string PointColumn { get; set; }

        public string CircleRow { get; set; }
        public string CircleColumn { get; set; }
        public string CircleRadius { get; set; }


        public PCDistanceModel Clone()
        {
            PCDistanceModel tModel = new PCDistanceModel();
            tModel.Name = Name;
            tModel.PointRow = PointRow;
            tModel.PointColumn = PointColumn;
            tModel.CircleRow = CircleRow;
            tModel.CircleColumn = CircleColumn;
            tModel.CircleRadius = CircleRadius;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点圆距离.ToString();
            }

            set
            {
                value = FeatureType.点圆距离.ToString();
            }
        }

        public PCDistanceModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("点圆的距离")]
            public double Distance { get; set; }
        }
    }

    /// <summary>
    /// 两圆交点Model
    /// </summary>
    public class CCPointCrossModel : BaseSeqModel, IMeasure
    { 
        public string CircleRow1 { get; set; }
        public string CircleColumn1 { get; set; }
        public string CircleRadius1 { get; set; }
        
        public string CircleRow2 { get; set; }
        public string CircleColumn2 { get; set; }
        public string CircleRadius2 { get; set; }

        public CCPointCrossModel Clone()
        {
            CCPointCrossModel tModel = new CCPointCrossModel();
            tModel.Name = Name; 
            tModel.CircleRow1 = CircleRow1;
            tModel.CircleColumn1 = CircleColumn1;
            tModel.CircleRadius1 = CircleRadius1;
            tModel.CircleRow2 = CircleRow2;
            tModel.CircleColumn2 = CircleColumn2;
            tModel.CircleRadius2 = CircleRadius2;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.两圆交点.ToString();
            }

            set
            {
                value = FeatureType.两圆交点.ToString();
            }
        }

        public CCPointCrossModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("两圆的交点Row1")]
            public double PointRow1 { get; set; }

            [Description("两圆的交点Row2")]
            public double PointRow2 { get; set; }

            [Description("两圆的交点Column1")]
            public double PointCol1 { get; set; }

            [Description("两圆的交点Column2")]
            public double PointCol2 { get; set; }

        }
    }

    /// <summary>
    /// 区域找圆Model
    /// </summary>
    public class FindRegionCircleModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }

        public int MinGray { get; set; }
        public int MaxGray { get; set; }
        public int MinArea { get; set; }

        public string SearchRadius { get; set; }

        public int MeasureLength1 { get; set; }

        public int MeasureLength2 { get; set; }

        public string MeasureTransition { get; set; }

        public string MeasureSelect { get; set; }

        public int MeasureThreshold { get; set; }

        public int MeasureNumber { get; set; }

        public double MeasureScore { get; set; }

        public double Sigma { get; set; }

        public bool IsReal { get; set; }
        public string PixPrec { get; set; }

        public bool IsJudgeRadius { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }

        public int CloseRadius { get; set; }

        public FindRegionCircleModel Clone()
        {
            FindRegionCircleModel tModel = new FindRegionCircleModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.SearchRadius = SearchRadius;
            tModel.MeasureLength1 = MeasureLength1;
            tModel.MeasureLength2 = MeasureLength2;
            tModel.MeasureTransition = MeasureTransition;
            tModel.MeasureSelect = MeasureSelect;
            tModel.MeasureThreshold = MeasureThreshold;
            tModel.MeasureNumber = MeasureNumber;
            tModel.MeasureScore = MeasureScore;
            tModel.Sigma = Sigma;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;
            tModel.MinArea = MinArea;
            tModel.IsReal = IsReal;
            tModel.PixPrec = PixPrec;

            tModel.IsJudgeRadius = IsJudgeRadius;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域找圆.ToString();
            }

            set
            {
                value = FeatureType.区域找圆.ToString();
            }
        }

        public FindRegionCircleModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("圆中心Row")]
            public double CircleRow { get; set; }

            [Description("圆中心Column")]
            public double CircleCol { get; set; }

            [Description("圆半径")]
            public double CircleRadius { get; set; }


            [Description("所有的圆中心Row")]
            public double[] AllRow { get; set; }

            [Description("所有的圆中心Column")]
            public double[] AllCol { get; set; }

            [Description("所有的圆半径")]
            public double[] AllRadius { get; set; }
            
            [Description("圆形结果区域")]
            public HObject CircleObj { get; set; }
        }

    }


    /// <summary>
    /// 矩形测量Model
    /// </summary>
    public class MeasureRectModel : BaseSeqModel, IMeasure
    {
        public string ImageForm { get; set; }

        public int MeasureLength1 { get; set; }

        public int MeasureLength2 { get; set; }

        public string MeasureTransition { get; set; }

        public string MeasureSelect { get; set; }

        public int MeasureThreshold { get; set; }

        public int MeasureNumber { get; set; }

        public double MeasureScore { get; set; }

        public double Sigma { get; set; }

        public bool IsModelFollow { get; set; }

        public string ModelFollow { get; set; }

        public bool IsShowPoint { get; set; }
        public bool IsShowLine { get; set; } 

        public MeasureRectModel Clone()
        {
            MeasureRectModel tModel = new MeasureRectModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.MeasureLength1 = MeasureLength1;
            tModel.MeasureLength2 = MeasureLength2;
            tModel.MeasureTransition = MeasureTransition;
            tModel.MeasureSelect = MeasureSelect;
            tModel.MeasureThreshold = MeasureThreshold;
            tModel.MeasureNumber = MeasureNumber;
            tModel.MeasureScore = MeasureScore;
            tModel.Sigma = Sigma;
            tModel.IsModelFollow = IsModelFollow;
            tModel.ModelFollow = ModelFollow;
            tModel.IsShowLine = IsShowLine;
            tModel.IsShowPoint = IsShowPoint;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.矩形测量.ToString();
            }

            set
            {
                value = FeatureType.矩形测量.ToString();
            }
        }

        public MeasureRectModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("矩形的四点Row值")]
            public double[] AllRow { get; set; }

            [Description("矩形的四点Column值")]
            public double[] AllCol { get; set; }

            [Description("矩形中心Row值")]
            public double CenterRow { get; set; }

            [Description("矩形中心Column值")]
            public double CenterCol { get; set; }

            [Description("矩形角度值")]
            public double Angle { get; set; }
        }

    }
}
