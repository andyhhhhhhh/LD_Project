using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 检测3D 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 图片处理Model
    /// </summary> 
    public class ProcessImageModel : BaseSeqModel, ICheck3D
    { 
        public int ThrLow { get; set; }
        public int ThrHigh { get; set; }
        public int FillArea { get; set; }
        public int CloseSize { get; set; }
        public int MedianWidth { get; set; }

        public string ImageForm { get; set; }

        public bool IsModelFollow { get; set; }

        public string FixedItemName { get; set; }

        #region 固定模块需要
        public ProcessImageModel Clone()
        {
            ProcessImageModel tModel = new ProcessImageModel();
            tModel.Name = Name;
            tModel.ThrLow = ThrLow;
            tModel.ThrHigh = ThrHigh;
            tModel.FillArea = FillArea;
            tModel.CloseSize = CloseSize;
            tModel.MedianWidth = MedianWidth;
            tModel.ImageForm = ImageForm;
            tModel.IsModelFollow = IsModelFollow;
            tModel.FixedItemName = FixedItemName;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图片处理.ToString();
            }

            set
            {
                value = FeatureType.图片处理.ToString();
            }
        }

        public ProcessImageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("预处理的输出图像")]
            public HObject Image { get; set; }

        }
        #endregion

    }


    /// <summary>
    /// 检测平面度Model
    /// </summary>
    public class FlatnessModel : BaseSeqModel, ICheck3D
    {
        public string ImageForm { get; set; }
        public string CheckAreaName { get; set; }
        public string FixedItemName { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int LoopTimes { get; set; }

        public int XScale { get; set; }

        public int YScale { get; set; }

        public bool IsOneRegion { get; set; }

        public string Result { get; set; }

        public bool IsMatch { get; set; }

        public FlatnessModel Clone()
        {
            FlatnessModel tModel = new FlatnessModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.CheckAreaName = CheckAreaName;
            tModel.FixedItemName = FixedItemName;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.LoopTimes = LoopTimes;
            tModel.XScale = XScale;
            tModel.YScale = YScale;
            tModel.Result = Result;
            tModel.IsMatch = IsMatch;
            tModel.IsOneRegion = IsOneRegion;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.平面度.ToString();
            }

            set
            {
                value = FeatureType.平面度.ToString();
            }
        }

        public FlatnessModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的平面度值")]
            public double FlatnessValue { get; set; }
        }
    }


    /// <summary>
    /// 检测高度 Model
    /// </summary>
    public class PointToAreaModel : BaseSeqModel, ICheck3D
    {
        public string BaseLevelName { get; set; }
        public string CheckAreaName { get; set; }
        public string FixedItemName { get; set; }

        public bool IsPtoP { get; set; }
        public bool IsRegionPic { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int LoopTimes { get; set; }

        public double OffSetValue { get; set; }

        public OffSetType offSetType { get; set; }

        public string Result { get; set; }
        public bool IsMatch { get; set; }

        public string ImageForm { get; set; }

        public string ModeType { get; set; }
        public string FilterType { get; set; }
        public int IValue { get; set; }
        public int JValue { get; set; }
        public bool IsIPercent { get; set; }
        public bool IsJPercent { get; set; }

        public PointToAreaModel Clone()
        {
            PointToAreaModel tModel = new PointToAreaModel();
            tModel.Name = Name;
            tModel.BaseLevelName = BaseLevelName;
            tModel.CheckAreaName = CheckAreaName;
            tModel.FixedItemName = FixedItemName;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.LoopTimes = LoopTimes;
            tModel.OffSetValue = OffSetValue;
            tModel.offSetType = offSetType;
            tModel.Result = Result;
            tModel.IsMatch = IsMatch;
            tModel.IsPtoP = IsPtoP;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }


        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.检测高度.ToString();
            }

            set
            {
                value = FeatureType.检测高度.ToString();
            }
        }

        public PointToAreaModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的高度值")]
            public double PointToAreaValue { get; set; }

            [Description("输出的高度数组")]
            public double[] pValueArr { get; set; }

            [Description("高度字符串以,分隔")]
            public string strResult { get; set; }
        }
    }


    /// <summary>
    /// 基准面图像Model
    /// </summary>
    public class GenFittingImageModel : BaseSeqModel, ICheck3D
    {

        public string ImageForm { get; set; }
        public string BaseName { get; set; }

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }

        public GenFittingImageModel Clone()
        {
            GenFittingImageModel tModel = new GenFittingImageModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.BaseName = BaseName;
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
                return FeatureType.基准图像.ToString();
            }

            set
            {
                value = FeatureType.基准图像.ToString();
            }
        }

        public GenFittingImageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的基准面图像")]
            public HObject Image { get; set; }
        }
    }


    /// <summary>
    /// 图像差分 Model
    /// </summary> 
    public class GetSubImageModel : BaseSeqModel, ICheck3D
    {
        public int GlueImageThrLow { get; set; }
        public int GlueImageThrHigh { get; set; }
        public int DeletGlueLightMin { get; set; }
        public int DeletGlueLightMax { get; set; }
        public int ErosionWH { get; set; }

        public string ImageBefore { get; set; }
        public string ImageAfter { get; set; }

        public override string BaseType
        {
            get
            {
                return FeatureType.图像差分.ToString();
            }

            set
            {
                value = FeatureType.图像差分.ToString();
            }
        }

        public GetSubImageModel Clone()
        {
            GetSubImageModel tModel = new GetSubImageModel();
            tModel.Name = Name;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        public GetSubImageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的差分图像")]
            public HObject Image
            {
                get; set;
            }
        }


    }

    /// <summary>
    /// 胶路缺陷Model
    /// </summary>
    public class CheckGlueDefectModel : BaseSeqModel, ICheck3D
    { 
        public int MoveDistance { get; set; }
        public double WH { get; set; }
        public int Step { get; set; }
        public int BigValue { get; set; }

        public int DefectAreaMin { get; set; }

        public int CheckEdgeSide { get; set; }

        public int EdgeHeightMax { get; set; }

        public int InnerEdgeHeightMax { get; set; }

        public int ShortValue { get; set; }

        public int MoveGlueHeight { get; set; }

        public int MoveGlueInner { get; set; }

        public int MoveLineEdge { get; set; }

        public int SubGrayValue { get; set; }

        public int NGMaxArea { get; set; }

        public string ImageForm { get; set; }

        public string FindLineName { get; set; }

        public CheckGlueDefectModel Clone()
        {
            CheckGlueDefectModel tModel = new CheckGlueDefectModel();
            tModel.Name = Name;
            tModel.MoveDistance = MoveDistance;
            tModel.WH = WH;
            tModel.Step = Step;
            tModel.BigValue = BigValue;
            tModel.DefectAreaMin = DefectAreaMin;
            tModel.CheckEdgeSide = CheckEdgeSide;
            tModel.EdgeHeightMax = EdgeHeightMax;
            tModel.InnerEdgeHeightMax = InnerEdgeHeightMax;
            tModel.ShortValue = ShortValue;
            tModel.MoveGlueHeight = MoveGlueHeight;
            tModel.MoveGlueInner = MoveGlueInner;
            tModel.MoveLineEdge = MoveLineEdge;
            tModel.SubGrayValue = SubGrayValue;
            tModel.NGMaxArea = NGMaxArea;
            tModel.ImageForm = ImageForm;
            tModel.FindLineName = FindLineName;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.胶路缺陷.ToString();
            }

            set
            {
                value = FeatureType.胶路缺陷.ToString();
            }
        }

        public CheckGlueDefectModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
        }
    }

    /// <summary>
    /// 检测第四条边缺陷Model
    /// </summary> 
    public class CheckGlueFourthModel : BaseSeqModel, ICheck3D
    {
        public int BigValue { get; set; }

        public int DefectAreaMin { get; set; }

        public int CheckEdgeSide { get; set; }

        public int ShortValue { get; set; }

        public int MoveGlueHeight { get; set; }

        public int MoveGlueInner { get; set; }

        public int MoveLineEdge { get; set; }

        public int DistanceUpMin { get; set; }
        public int DistanceUpMax { get; set; }
        public int DistanceMidMin { get; set; }
        public int DistanceMidMax { get; set; }
        public int DistanceDownMin { get; set; }
        public int DistanceDownMax { get; set; }

        public int SubGrayValue { get; set; }

        public int NGMaxArea { get; set; }

        public string ImageForm { get; set; }

        public string FindLineName { get; set; }

        public string CheckAreaName { get; set; }

        public CheckGlueFourthModel Clone()
        {
            CheckGlueFourthModel tModel = new CheckGlueFourthModel();
            tModel.Name = Name;
            tModel.BigValue = BigValue;
            tModel.DefectAreaMin = DefectAreaMin;
            tModel.CheckEdgeSide = CheckEdgeSide;
            tModel.ShortValue = ShortValue;
            tModel.NGMaxArea = NGMaxArea;
            tModel.ImageForm = ImageForm;
            tModel.FindLineName = FindLineName;
            tModel.MoveGlueHeight = MoveGlueHeight;
            tModel.MoveGlueInner = MoveGlueInner;
            tModel.MoveLineEdge = MoveLineEdge;

            tModel.CheckAreaName = CheckAreaName;
            tModel.SubGrayValue = SubGrayValue;
            tModel.DistanceUpMin = DistanceUpMin;
            tModel.DistanceUpMax = DistanceUpMax;
            tModel.DistanceMidMin = DistanceMidMin;
            tModel.DistanceMidMax = DistanceMidMax;
            tModel.DistanceDownMin = DistanceDownMin;
            tModel.DistanceDownMax = DistanceDownMax;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.胶路四边.ToString();
            }

            set
            {
                value = FeatureType.胶路四边.ToString();
            }
        }

        public CheckGlueFourthModel()
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
    /// 提取胶水外壳边缘Model
    /// </summary>
    public class CheckKEdgeContourModel : BaseSeqModel
    { 
        public string ImageForm { get; set; }
        public string FindLineForm { get; set; }

        public int KEdgeThreshold { get; set; }
        public string KEdgeTransition { get; set; }
        public string KEdgeSelect { get; set; }
        public int KEdgeLength1 { get; set; }
        public int KEdgeLength2 { get; set; }
        public int KEdgeSigma { get; set; }
        public int KEdgeNumber1 { get; set; }
        public double KEdgeScore { get; set; }
        public double KEdgeSmooth { get; set; }
        public double ScaleValue { get; set; }
        public double ScaleDivider { get; set; }
        public int KEdgeBottomRowStart { get; set; }
        public int KEdgeBottomRowEnd { get; set; }
        public int KEdgeBottomColStart { get; set; }
        public int KEdgeBottomColEnd { get; set; }
        public int KEdgeIncreaseTolerance { get; set; }

        public override string BaseType
        {
            get
            {
                return FeatureType.外壳内边.ToString();
            }

            set
            {
                value = FeatureType.外壳内边.ToString();
            }
        }
    }

    /// <summary>
    /// 检测胶水宽度 高度 胶偏 
    /// </summary>
    public class CheckGlueModel : BaseSeqModel
    { 
        public string ImageForm { get; set; }
        public string SubImageForm { get; set; }
        public string KEdgeForm { get; set; }
        public string FindLineForm { get; set; }


        [Category("胶路提取参数")]
        public int GlueThrLow { get; set; }
        [Category("胶路提取参数")]
        public int GlueThrHigh { get; set; }
        [Category("胶路提取参数")]
        public int GlueAreaMin { get; set; }
        [Category("胶路提取参数")]
        public int GlueAreaMax { get; set; }
        [Category("胶路提取参数")]
        public int SegMentDist { get; set; }
        [Category("胶路提取参数")]
        public int RoiWidthLen2 { get; set; }
        [Category("胶路提取参数")]
        public int RoiLen1 { get; set; }
        [Category("胶路提取参数")]
        public int AmplitudeThreshold { get; set; }
        [Category("胶路提取参数")]
        public string WidthTransition { get; set; }
        [Category("胶路提取参数")]
        public string WidthSelect { get; set; }
        [Category("胶路提取参数")]
        public int Sigma { get; set; }
        [Category("胶路提取参数")]
        public int GlueSide { get; set; }


        [Category("断胶和溢胶参数")]
        public bool bEnableGlueWidth { get; set; }
        [Category("断胶和溢胶参数")]
        public int GlueWidthMin { get; set; }
        [Category("断胶和溢胶参数")]
        public int GlueWidthMax { get; set; }


        [Category("胶路高度参数")]
        public bool bEnableGlueHeight { get; set; }
        [Category("胶路高度参数")]
        public int GlueValueMin { get; set; }
        [Category("胶路高度参数")]
        public int GlueHeightMin { get; set; }
        [Category("胶路高度参数")]
        public int GlueHeightMax { get; set; }

        [Category("胶偏参数")]
        public bool bEnableDeterMin { get; set; }
        [Category("胶偏参数")]
        public int GlueMaxDistanceSTD { get; set; }
        [Category("胶偏参数")]
        public int GlueMinDistanceSTD { get; set; }

        public override string BaseType
        {
            get
            {
                return FeatureType.检测胶路.ToString();
            }

            set
            {
                value = FeatureType.检测胶路.ToString();
            }
        }
    }
    #endregion
    

}

