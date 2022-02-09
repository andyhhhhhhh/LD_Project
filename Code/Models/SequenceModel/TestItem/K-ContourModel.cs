using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 轮廓处理集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 边缘提取 Model
    /// </summary>
    public class EdgeExtractModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public bool IsRegion { get; set; }
        public string Filter { get; set; }
        public double Alpha { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public EdgeExtractModel Clone()
        {
            EdgeExtractModel tModel = new EdgeExtractModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.IsRegion = IsRegion;
            tModel.Filter = Filter;
            tModel.Alpha = Alpha;
            tModel.Low = Low;
            tModel.High = High;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.边缘提取.ToString();
            }

            set
            {
                value = FeatureType.边缘提取.ToString();
            }
        }

        public EdgeExtractModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("边缘提取后的轮廓")]
            public HObject OutContour { get; set; }
        }
    }

    /// <summary>
    /// 轮廓筛选 Model
    /// </summary>
    public class ContourFilterModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public string ContourForm { get; set; }
        public string Feature { get; set; }
        public double Min1 { get; set; }
        public double Max1 { get; set; }
        public double Min2 { get; set; }
        public double Max2 { get; set; }
        public ContourFilterModel Clone()
        {
            ContourFilterModel tModel = new ContourFilterModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.ContourForm = ContourForm;
            tModel.Feature = Feature;
            tModel.Min1 = Min1;
            tModel.Max1 = Max1;
            tModel.Min2 = Min2;
            tModel.Max2 = Max2;

            return tModel;
        } 

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.轮廓筛选.ToString();
            }

            set
            {
                value = FeatureType.轮廓筛选.ToString();
            }
        }

        public ContourFilterModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("筛选后的轮廓")]
            public HObject OutContour { get; set; }
        }
    }

    /// <summary>
    /// 轮廓交点 Model
    /// </summary>
    public class ContourIntersectModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public string InterType { get; set; }
        public string Contour1Form { get; set; }
        public string Contour2Form { get; set; }
        public string LineStartRow { get; set; }
        public string LineStartCol { get; set; }
        public string LineEndRow { get; set; }
        public string LineEndCol { get; set; }
        public ContourIntersectModel Clone()
        {
            ContourIntersectModel tModel = new ContourIntersectModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.InterType = InterType;
            tModel.Contour1Form = Contour1Form;
            tModel.Contour2Form = Contour2Form;
            tModel.LineStartRow = LineStartRow;
            tModel.LineStartCol = LineStartCol;
            tModel.LineEndRow = LineEndRow;
            tModel.LineEndCol = LineEndCol; 

            return tModel;
        }
        
        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.轮廓交点.ToString();
            }

            set
            {
                value = FeatureType.轮廓交点.ToString();
            }
        }

        public ContourIntersectModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("轮廓的所有交点Row")]
            public double[] OutRow { get; set; }

            [Description("轮廓的所有交点Column")]
            public double[] OutCol { get; set; }
        }
    }

    /// <summary>
    /// 生成十字 Model
    /// </summary>
    public class GenCrossTenModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public double Angle { get; set; }
        public double Size { get; set; }
        public GenCrossTenModel Clone()
        {
            GenCrossTenModel tModel = new GenCrossTenModel();
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
                return FeatureType.生成十字.ToString();
            }

            set
            {
                value = FeatureType.生成十字.ToString();
            }
        }

        public GenCrossTenModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("生成的十字轮廓")]
            public HObject OutContour { get; set; }
        }
    }


    /// <summary>
    /// 生成直线Model
    /// </summary>
    public class TwoPLineModel : BaseSeqModel, IContourModel
    {
        public string Point1Row { get; set; }
        public string Point1Column { get; set; }

        public string Point2Row { get; set; }
        public string Point2Column { get; set; }

        public bool IsAdd360 { get; set; }


        public TwoPLineModel Clone()
        {
            TwoPLineModel tModel = new TwoPLineModel();
            tModel.Name = Name;
            tModel.Point1Row = Point1Row;
            tModel.Point1Column = Point1Column;
            tModel.Point2Row = Point2Row;
            tModel.Point2Column = Point2Column;
            tModel.IsAdd360 = IsAdd360;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.生成直线.ToString();
            }

            set
            {
                value = FeatureType.生成直线.ToString();
            }
        }

        public TwoPLineModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("直线中心Row")]
            public double CenterRow { get; set; }

            [Description("直线中心Column")]
            public double CenterCol { get; set; }

            [Description("直线角度")]
            public double LineAngle { get; set; }

            [Description("直线长度")]
            public double Distance { get; set; }

        }
    }
    /// <summary>
    /// 点构建Model
    /// </summary>
    public class PointCreateModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public double PointRow { get; set; }
        public double PointColumn { get; set; }  

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }

        public int PointSize { get; set; }

        public PointCreateModel Clone()
        {
            PointCreateModel tModel = new PointCreateModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.PointRow = PointRow;
            tModel.PointColumn = PointColumn;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
            tModel.PointSize = PointSize;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点构建.ToString();
            }

            set
            {
                value = FeatureType.点构建.ToString();
            }
        }

        public PointCreateModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("点Row值")]
            public double ResultRow { get; set; }

            [Description("点Column值")]
            public double ResultCol { get; set; }

        }

    }

    /// <summary>
    /// 点拟合圆Model
    /// </summary>
    public class PointGenCircleModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public double PointRow { get; set; }
        public double PointColumn { get; set; }

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }

        public int PointSize { get; set; }

        public List<PointF> ListPoint { get; set; }

        public PointGenCircleModel Clone()
        {
            PointGenCircleModel tModel = new PointGenCircleModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.PointRow = PointRow;
            tModel.PointColumn = PointColumn;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
            tModel.PointSize = PointSize;
            tModel.ListPoint = ListPoint;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点拟合圆.ToString();
            }

            set
            {
                value = FeatureType.点拟合圆.ToString();
            }
        }

        public PointGenCircleModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("圆中心Row")]
            public double ResultRow { get; set; }

            [Description("圆中心Column")]
            public double ResultCol { get; set; }

            [Description("圆半径")]
            public double ResultRadius { get; set; }

        }

    }

    /// <summary>
    /// 边转区域 Model
    /// </summary>
    public class ContourToRegionModel : BaseSeqModel, IContourModel
    {
        public string ImageForm { get; set; }
        public string ContourForm { get; set; }
        public string Mode { get; set; }
        public ContourToRegionModel Clone()
        {
            ContourToRegionModel tModel = new ContourToRegionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.ContourForm = ContourForm;
            tModel.Mode = Mode;


            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.边转区域.ToString();
            }

            set
            {
                value = FeatureType.边转区域.ToString();
            }
        }

        public ContourToRegionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 

        }
    }
    /// <summary>
    /// 点排序 Model
    /// </summary>
    public class SortPointModel : BaseSeqModel, IContourModel
    {
        public string RowForm { get; set; }
        public string ColumnForm { get; set; }
        public string Mode { get; set; }

        public string Order { get; set; } 

        public bool IsDisplayOrder { get; set; }

        public SortPointModel Clone()
        {
            SortPointModel tModel = new SortPointModel();
            tModel.Name = Name;
            tModel.RowForm = RowForm;
            tModel.ColumnForm = ColumnForm;
            tModel.Mode = Mode;
            tModel.Order = Order; 
            tModel.IsDisplayOrder = IsDisplayOrder;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点排序.ToString();
            }

            set
            {
                value = FeatureType.点排序.ToString();
            }
        }

        public SortPointModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("排序后的点Row数组")]
            public double[] OutRows { get; set; }

            [Description("排序后的点Column数组")]
            public double[] OutColumns { get; set; }
        }
    }
}
