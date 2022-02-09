using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 区域处理 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 面积中心Model
    /// </summary>
    public class AreaCenterModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public AreaCenterModel Clone()
        {
            AreaCenterModel tModel = new AreaCenterModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;  

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.面积中心.ToString();
            }

            set
            {
                value = FeatureType.面积中心.ToString();
            }
        }

        public AreaCenterModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("区域面积")]
            public double AreaValue { get; set; }

            [Description("区域中心Row")]
            public double CenterRowValue { get; set; }

            [Description("区域中心Column")]
            public double CenterColValue { get; set; }

            [Description("所有的区域面积")]
            public long[] AllAreaValue { get; set; }

            [Description("所有的区域中心Row")]
            public double[] AllRowValue { get; set; }

            [Description("所有的区域中心Column")]
            public double[] AllColValue { get; set; }

        }
    }

    /// <summary>
    /// 腐蚀膨胀Model
    /// </summary>
    public class DErosionModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }

        public string ComputeType { get; set; }

        public string Shape { get; set; }

        public bool IsFillDisplay { get; set; }

        public double Radius { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public DErosionModel Clone()
        {
            DErosionModel tModel = new DErosionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.ComputeType = ComputeType;
            tModel.Shape = Shape;
            tModel.IsFillDisplay = IsFillDisplay;
            tModel.Radius = Radius;
            tModel.Width = Width;
            tModel.Height = Height;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.腐蚀膨胀.ToString();
            }

            set
            {
                value = FeatureType.腐蚀膨胀.ToString();
            }
        }

        public DErosionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("腐蚀膨胀后的Region")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 开闭运算 Model
    /// </summary>
    public class OpenCloseModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public string ComputeType { get; set; }
        public string Shape { get; set; }
        public double Radius { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsFillDisplay { get; set; }
        public OpenCloseModel Clone()
        {
            OpenCloseModel tModel = new OpenCloseModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.ComputeType = ComputeType;
            tModel.Shape = Shape;
            tModel.Radius = Radius;
            tModel.Width = Width;
            tModel.Height = Height;
            tModel.IsFillDisplay = IsFillDisplay;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.开闭运算.ToString();
            }

            set
            {
                value = FeatureType.开闭运算.ToString();
            }
        }

        public OpenCloseModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("开闭运算后的Region")]
            public HObject OutRegion { get; set; }
        }
    }
    /// <summary>
    /// 区域运算 Model
    /// </summary>
    public class RegionComputeModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public List<string> ListRegion { get; set; }

        public string ComputeType { get; set; }
        public RegionComputeModel Clone()
        {
            RegionComputeModel tModel = new RegionComputeModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.ListRegion = ListRegion;
            tModel.ComputeType = ComputeType;

            return tModel;
        }
         

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域运算.ToString();
            }

            set
            {
                value = FeatureType.区域运算.ToString();
            }
        }

        public RegionComputeModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 

        }
    }

    /// <summary>
    /// 区域填充 Model
    /// </summary>
    public class RegionFillModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }

        public string Feature { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public bool IsFillUpAll { get; set; }

        public RegionFillModel Clone()
        {
            RegionFillModel tModel = new RegionFillModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.Feature = Feature;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.IsFillUpAll = IsFillUpAll;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域填充.ToString();
            }

            set
            {
                value = FeatureType.区域填充.ToString();
            }
        }

        public RegionFillModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("填充后的Region")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 区域转边 Model
    /// </summary>
    public class RegionToContourModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public string Mode { get; set; }
        public bool IsContourClosed { get; set; }
        public RegionToContourModel Clone()
        {
            RegionToContourModel tModel = new RegionToContourModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.Mode = Mode;
            tModel.IsContourClosed = IsContourClosed;


            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域转边.ToString();
            }

            set
            {
                value = FeatureType.区域转边.ToString();
            }
        }

        public RegionToContourModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("转换的轮廓结果")]
            public HObject OutContour { get; set; }
        }
    }


    /// <summary>
    /// 区域排序 Model
    /// </summary>
    public class SortRegionModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public string Mode { get; set; } 

        public string Order { get; set; }
        public string RowOrCol { get; set; }

        public bool IsDisplayOrder { get; set; }

        public SortRegionModel Clone()
        {
            SortRegionModel tModel = new SortRegionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.Mode = Mode;
            tModel.Order = Order;
            tModel.RowOrCol = RowOrCol;
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
                return FeatureType.区域排序.ToString();
            }

            set
            {
                value = FeatureType.区域排序.ToString();
            }
        }

        public SortRegionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("排序后的Region")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 形状筛选 Model
    /// </summary>
    public class RegionFilterModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; } 

        public string Operation { get; set; }

        public string StdFeature { get; set; }
        public int Percent { get; set; }

        public bool IsStd { get; set; }
        public bool IsFillDisplay { get; set; }

        public List<FilterClass> ListFilter { get; set; }
        public bool IsClearDisplay { get; set; }
        
        public RegionFilterModel Clone()
        {
            RegionFilterModel tModel = new RegionFilterModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm; 
            tModel.Operation = Operation;
            tModel.StdFeature = StdFeature;
            tModel.Percent = Percent;
            tModel.IsStd = IsStd;
            tModel.IsFillDisplay = IsFillDisplay;
            tModel.ListFilter = ListFilter;
            tModel.IsClearDisplay = IsClearDisplay; 

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.形状筛选.ToString();
            }

            set
            {
                value = FeatureType.形状筛选.ToString();
            }
        }

        public RegionFilterModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("筛选后的Region")]
            public HObject OutRegion { get; set; }
        }


        public class FilterClass
        {
            public string Feature { get; set; }
            public int MinValue { get; set; }
            public int MaxValue { get; set; }
        }
    }

    /// <summary>
    /// 区域形状 Model
    /// </summary>
    public class RegionShapeModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }

        public string RegionForm { get; set; }

        public string ShapeType { get; set; }  

        public bool IsFillDisplay { get; set; }
        public RegionShapeModel Clone()
        {
            RegionShapeModel tModel = new RegionShapeModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.ShapeType = ShapeType;
            tModel.IsFillDisplay = IsFillDisplay;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域形状.ToString();
            }

            set
            {
                value = FeatureType.区域形状.ToString();
            }
        }

        public RegionShapeModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("切换形状后的Region")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 特征检测Model
    /// </summary>
    public class FeatureDetectModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public List<string> ListDetect { get; set; }
        public FeatureDetectModel Clone()
        {
            FeatureDetectModel tModel = new FeatureDetectModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.ListDetect = ListDetect;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.特征检测.ToString();
            }

            set
            {
                value = FeatureType.特征检测.ToString();
            }
        }

        public FeatureDetectModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("区域面积")]
            public double Area { get; set; }

            [Description("区域中心Row")]
            public double Row { get; set; }

            [Description("区域中心Column")]
            public double Column { get; set; }

            [Description("矩形Row1")]
            public double Row1 { get; set; }

            [Description("矩形Column1")]
            public double Column1 { get; set; }

            [Description("矩形Row2")]
            public double Row2 { get; set; }

            [Description("矩形Column2")]
            public double Column2 { get; set; }

            [Description("Rectangle2的中心Row")]
            public double Rect2_Row { get; set; }

            [Description("Rectangle2的中心Column")]
            public double Rect2_Column { get; set; }

            [Description("Rectangle2的角度")]
            public double Rect2_Phi { get; set; }

            [Description("Rectangle2的length1")]
            public double Rect2_len1 { get; set; }

            [Description("Rectangle2的length2")]
            public double Rect2_len2 { get; set; }

            [Description("轮廓的长度")]
            public double Contlength { get; set; }

            [Description("轮廓的圆度")]
            public double Circularity { get; set; }

            [Description("轮廓的矩形度")]
            public double Rectlarity { get; set; }

            [Description("圆中心Row")]
            public double Circle_Row { get; set; }

            [Description("圆中心Column")]
            public double Circle_Column { get; set; }

            [Description("圆半径")]
            public double Circle_Radius { get; set; }
             
            [Description("区域的最小灰度值")]
            public double Min_Gray { get; set; }

            [Description("区域的最大灰度值")]
            public double Max_Gray { get; set; } 

            [Description("区域的平均灰度值")]
            public double Mean { get; set; }

            [Description("区域的偏差值")]
            public double Deviation { get; set; }
        }
    }

    /// <summary>
    /// 区域分割 Model
    /// </summary>
    public class PartitionRegionModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        
        public int Distance { get; set; }
        public int Percent { get; set; }

        public PartitionRegionModel Clone()
        {
            PartitionRegionModel tModel = new PartitionRegionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.Distance = Distance;
            tModel.Percent = Percent;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域分割.ToString();
            }

            set
            {
                value = FeatureType.区域分割.ToString();
            }
        }

        public PartitionRegionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("分割后的结果Region")]
            public HObject OutRegion { get; set; }
        }
    }
    /// <summary>
    /// 顶帽处理Model
    /// </summary>
    public class GrayTophatModel : BaseSeqModel, IRegionModel
    {
        public string ImageForm { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Smax { get; set; }
        public bool bTophat { get; set; }


        public GrayTophatModel Clone()
        {
            GrayTophatModel tModel = new GrayTophatModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.Height = Height;
            tModel.Width = Width;
            tModel.Smax = Smax;
            tModel.bTophat = bTophat;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.顶帽处理.ToString();
            }

            set
            {
                value = FeatureType.顶帽处理.ToString();
            }
        }

        public GrayTophatModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("顶帽处理结果图像")]
            public HObject OutImage { get; set; }
        }
    }

}
