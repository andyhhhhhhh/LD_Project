using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 图像处理 集合
/// </summary>
namespace SequenceTestModel
{

    /// <summary>
    /// 3D相机采集图像Model
    /// </summary>
    public class SnapImageModel : BaseSeqModel, IImageControl
    {
        public int LaserId { get; set; }
        public string LaserName { get; set; }
        public int TimeOut { get; set; }

        public int Profile { get; set; }

        public int ExposureTime { get; set; }

        public int Brightness { get; set; }

        public int Gain { get; set; }

        public bool EnableGain { get; set; }

        public bool bLocalImage { get; set; }

        public string LocalPath { get; set; }

        public bool bLoacalPath { get; set; }

        public string ConfigPath { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.镭射采集.ToString();
            }

            set
            {
                value = FeatureType.镭射采集.ToString();
            }
        }

        public SnapImageModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("采集的图片")]
            public HObject Image { get; set; }

            [Description("图片的高度")]
            public int Height { get; set; }

            [Description("图片的宽度")]
            public int Width { get; set; }
        }

        public SnapImageModel Clone()
        {
            SnapImageModel tModel = new SnapImageModel();
            tModel.Name = Name;
            tModel.LaserName = LaserName;
            tModel.Profile = Profile;
            tModel.ExposureTime = ExposureTime;
            tModel.Brightness = Brightness;
            tModel.Gain = Gain;
            tModel.EnableGain = EnableGain;
            tModel.bLocalImage = bLocalImage;
            tModel.LocalPath = LocalPath;
            tModel.ConfigPath = ConfigPath;

            return tModel;
        }


    }
    

    /// <summary>
    /// 保存图像Model
    /// </summary>
    public class ImageSaveModel : BaseSeqModel, IImageControl
    {

        public string ImageForm { get; set; }

        public bool IsSaveObj { get; set; }

        public string ImagePath { get; set; }

        public string ImageType { get; set; }

        public string ImageName { get; set; }

        public bool IsAddTime { get; set; }

        public string NGPicForm { get; set; }

        public bool IsSaveOnlyNG { get; set; }

        public bool IsPathAddTime { get; set; }

        public bool IsClearDir { get; set; }
        public int SaveDays { get; set; }

        public bool IsOKNGPath { get; set; }

        public bool IsChildDir { get; set; }
        public string ChildDirForm { get; set; }

        public bool IsJudgeNG { get; set; }
        public string StartItem { get; set; }
        public string EndItem { get; set; }


        public ImageSaveModel Clone()
        {
            ImageSaveModel tModel = new ImageSaveModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsSaveObj = IsSaveObj;
            tModel.ImagePath = ImagePath;
            tModel.ImageType = ImageType;
            tModel.ImageName = ImageName;
            tModel.IsAddTime = IsAddTime;
            tModel.NGPicForm = NGPicForm;
            tModel.IsSaveOnlyNG = IsSaveOnlyNG;
            tModel.IsPathAddTime = IsPathAddTime;
            tModel.IsChildDir = IsChildDir;
            tModel.ChildDirForm = ChildDirForm;
            tModel.IsClearDir = IsClearDir;
            tModel.SaveDays = SaveDays;
            tModel.IsJudgeNG = IsJudgeNG;
            tModel.StartItem = StartItem;
            tModel.EndItem = EndItem;
            tModel.IsOKNGPath = IsOKNGPath;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图像保存.ToString();
            }

            set
            {
                value = FeatureType.图像保存.ToString();
            }
        }

        public ImageSaveModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    /// <summary>
    /// 相机2D拍照 Model
    /// </summary>
    public class Camera2DSnapModel : BaseSeqModel, IImageControl
    {
        public string CameraName { get; set; }
        public string UniqueName { get; set; }

        public string DeviceName { get; set; }

        public int ExposureTime { get; set; }

        public string ExposureParamName { get; set; }

        public int Gain { get; set; }

        public string GainParamName { get; set; }

        public string InterfaceName { get; set; }        

        [XmlIgnore]
        public HTuple AcqHandle { get; set; }

        [XmlIgnore]
        public bool IsOpen { get; set; }

        [XmlIgnore]
        public bool IsContinue { get; set; }

        public bool bLocalImage { get; set; }

        public bool bLocalPath { get; set; }

        public string LocalPath { get; set; }

        public bool IsMeasureCali { get; set; }
        public string MeasureCali { get; set; }
        public bool IsDistrotionCali { get; set; }
        public string DistrotionCali { get; set; }

        public bool IsSetParam { get; set; }
        public bool IsConfigExposure { get; set; }
        public string ConfigExposure { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.相机采集.ToString();
            }

            set
            {
                value = FeatureType.相机采集.ToString();
            }
        }

        public Camera2DSnapModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("2D相机采集的图片")]
            public HObject Image { get; set; }

            [Description("图片的高度")]
            public int Height { get; set; }

            [Description("图片的宽度")]
            public int Width { get; set; }
        }

        public Camera2DSnapModel Clone()
        {
            Camera2DSnapModel tModel = new Camera2DSnapModel();
            tModel.Name = Name;
            tModel.UniqueName = UniqueName;
            tModel.DeviceName = DeviceName;
            tModel.ExposureTime = ExposureTime;
            tModel.Gain = Gain;
            tModel.ExposureParamName = ExposureParamName;
            tModel.GainParamName = GainParamName;
            tModel.InterfaceName = InterfaceName;
            tModel.bLocalImage = bLocalImage;
            tModel.LocalPath = LocalPath;
            tModel.bLocalPath = bLocalPath;
            tModel.CameraName = CameraName;
            tModel.IsSetParam = IsSetParam;
            tModel.IsMeasureCali = IsMeasureCali;
            tModel.MeasureCali = MeasureCali;
            tModel.IsDistrotionCali = IsDistrotionCali;
            tModel.DistrotionCali = DistrotionCali;
            tModel.IsConfigExposure = IsConfigExposure;
            tModel.ConfigExposure = ConfigExposure;

            return tModel;
        }

        /// <summary>
        /// 采集接口
        /// </summary>
        public enum EnumInterface
        {
            GigEVision2 = 0,
            USB3Vision,
            _1394IIDC,
            Other
        }

        public EnumInterface GetInterfaceFilter(string eInterface)
        {
            EnumInterface interfaceResult;
            switch (eInterface)
            {
                case "GigEVision2":
                    interfaceResult = EnumInterface.GigEVision2;
                    break;
                case "1394IIDC":
                    interfaceResult = EnumInterface._1394IIDC;
                    break;
                case "USB3Vision":
                    interfaceResult = EnumInterface.USB3Vision;
                    break;
                case "Other":
                default:
                    interfaceResult = EnumInterface.Other;
                    break;
            }
            return interfaceResult;
        }

        public bool IsEuqalInterface(string eInterface1, EnumInterface eInterface2)
        {
            return GetInterfaceFilter(eInterface1) == eInterface2;
        }
    }
    

    /// <summary>
    /// 预处理图 Model
    /// </summary>
    public class PretreatImageModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }
        public List<ImagePretreat> listPretreatImage { get; set; }

        public bool IsRegion { get; set; }
        public string RegionForm { get; set; }

        public PretreatImageModel Clone()
        {
            PretreatImageModel tModel = new PretreatImageModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.listPretreatImage = listPretreatImage;
            tModel.IsRegion = IsRegion;
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
                return FeatureType.预处理图.ToString();
            }

            set
            {
                value = FeatureType.预处理图.ToString();
            }
        }

        public PretreatImageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("预处理结果图像")]
            public HObject Image { get; set; }
        }
    }


    /// <summary>
    /// 预处理图 class
    /// </summary>
    public class ImagePretreat
    {
        public int Index { get; set; }

        public PreCalType preCalType { get; set; }

        public string Paramter { get; set; }

        public bool Enable { get; set; }
    }
     

    /// <summary>
    /// 显示图像Model
    /// </summary>
    public class DisplayImgModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }

        public string SetColor { get; set; }

        public string FillType { get; set; }

        public int LineWidth { get; set; }


        public DisplayImgModel Clone()
        {
            DisplayImgModel tModel = new DisplayImgModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.SetColor = SetColor;
            tModel.FillType = FillType;
            tModel.LineWidth = LineWidth;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.显示图像.ToString();
            }

            set
            {
                value = FeatureType.显示图像.ToString();
            }
        }

        public DisplayImgModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }


    /// <summary>
    /// 二值图像Model
    /// </summary>
    public class ThresholdImgModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }
        public bool IsRegion { get; set; }
        public string RegionForm { get; set; }

        public double MinGray { get; set; }

        public double MaxGray { get; set; }
        public bool IsFillDisplay { get; set; }
        public bool IsDisplay { get; set; }

        public ThresholdImgModel Clone()
        {
            ThresholdImgModel tModel = new ThresholdImgModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsRegion = IsRegion;
            tModel.RegionForm = RegionForm;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;
            tModel.IsFillDisplay = IsFillDisplay;
            tModel.IsDisplay = IsDisplay;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.二值图像.ToString();
            }

            set
            {
                value = FeatureType.二值图像.ToString();
            }
        }

        public ThresholdImgModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("二值化输出区域")]
            public HObject OutRegion { get; set; }
        }
    }


    /// <summary>
    /// 阈值分割Model
    /// </summary>
    public class ThreshodSegModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public bool IsRegion { get; set; }

        public string SegType { get; set; }
         
        public double MinGray { get; set; }

        public double MaxGray { get; set; }

        public double MinSize { get; set; }

        public string LightDark { get; set; }

        public bool IsFillDisplay { get; set; }
        public bool IsClearDisplay { get; set; }


        public ThreshodSegModel Clone()
        {
            ThreshodSegModel tModel = new ThreshodSegModel();
            tModel.Name = Name;
            tModel.RegionForm = RegionForm;
            tModel.ImageForm = ImageForm;
            tModel.IsRegion = IsRegion;
            tModel.SegType = SegType;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;
            tModel.MinSize = MinSize;
            tModel.LightDark = LightDark;
            tModel.IsFillDisplay = IsFillDisplay;
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
                return FeatureType.阈值分割.ToString();
            }

            set
            {
                value = FeatureType.阈值分割.ToString();
            }
        }

        public ThreshodSegModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("阈值分割输出区域")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 图像剪切Model
    /// </summary>
    public class ImageCutModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; } 
        public bool IsCustomRegion { get; set; }
        public string RegionForm { get; set; }
        public string StartRowForm { get; set; }
        public string StartColForm { get; set; }
        public string EndRowForm { get; set; }
        public string EndColForm { get; set; }
        public ImageCutModel Clone()
        {
            ImageCutModel tModel = new ImageCutModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsCustomRegion = IsCustomRegion;
            tModel.RegionForm = RegionForm;
            tModel.StartRowForm = StartRowForm;
            tModel.StartColForm = StartColForm;
            tModel.EndRowForm = EndRowForm;
            tModel.EndColForm = EndColForm;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图像剪切.ToString();
            }

            set
            {
                value = FeatureType.图像剪切.ToString();
            }
        }

        public ImageCutModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的剪切图像")]
            public HObject OutImage { get; set; }
        }
    }

    /// <summary>
    /// 图像展开Model
    /// </summary>
    public class PolarImageModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }

        public string RowForm { get; set; }

        public string ColumnForm { get; set; }

        public int StartAngle { get; set; }
        public int EndAngle { get; set; }
        public int StartRadius { get; set; }
        public int EndRadius { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }  
        public string Interpolation{get;set;}

        public PolarImageModel Clone()
        {
            PolarImageModel tModel = new PolarImageModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RowForm = RowForm;
            tModel.ColumnForm = ColumnForm;
            tModel.StartAngle = StartAngle;
            tModel.EndAngle = EndAngle;
            tModel.StartRadius = StartRadius;
            tModel.EndRadius = EndRadius;
            tModel.Width = Width;
            tModel.Height = Height;
            tModel.Interpolation = Interpolation;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图像展开.ToString();
            }

            set
            {
                value = FeatureType.图像展开.ToString();
            }
        }

        public PolarImageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("展开的结果图像")]
            public HObject OutImage { get; set; }
        }
    }

    /// <summary>
    /// 图像相减Model
    /// </summary>
    public class ImageSubModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }
        public string ImageSub { get; set; } 
        public int Mult { get; set; }

        public int Add { get; set; } 

        public ImageSubModel Clone()
        {
            ImageSubModel tModel = new ImageSubModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.ImageSub = ImageSub;
            tModel.Mult = Mult;
            tModel.Add = Add; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图像相减.ToString();
            }

            set
            {
                value = FeatureType.图像相减.ToString();
            }
        }

        public ImageSubModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("相减的结果图像")]
            public HObject OutImage { get; set; }
        }
    }

    /// <summary>
    /// 颜色转换Model
    /// </summary>
    public class TransRGBModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; }
        public bool IsTrans { get; set; }
        public string TransType { get; set; } 

        public TransRGBModel Clone()
        {
            TransRGBModel tModel = new TransRGBModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsTrans = IsTrans;
            tModel.TransType = TransType;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.颜色转换.ToString();
            }

            set
            {
                value = FeatureType.颜色转换.ToString();
            }
        }

        public TransRGBModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("R通道图像")]
            public HObject ImageRed { get; set; }

            [Description("G通道图像")]
            public HObject ImageGreen { get; set; }

            [Description("B通道图像")]
            public HObject ImageBlue { get; set; }
        }
    }
     
    /// <summary>
    /// 图像拆分Model
    /// </summary>
    public class ImageSplitModel : BaseSeqModel, IImageControl
    {
        public string ImageForm { get; set; } 
        public ImageSplitModel Clone()
        {
            ImageSplitModel tModel = new ImageSplitModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.图像拆分.ToString();
            }

            set
            {
                value = FeatureType.图像拆分.ToString();
            }
        }

        public ImageSplitModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("拆分图像1")]
            public HObject Image1 { get; set; }

            [Description("拆分图像2")]
            public HObject Image2 { get; set; }

            [Description("拆分图像3")]
            public HObject Image3 { get; set; }
        }
    }


    /// <summary>
    /// 多图采集Model
    /// </summary>
    public class MultiCameraSnapModel : BaseSeqModel, IImageControl
    {
        public List<MultiCameraClass> ListMultiCamera { get; set; } 

        public MultiCameraSnapModel Clone()
        {
            MultiCameraSnapModel tModel = new MultiCameraSnapModel();
            tModel.Name = Name;
            tModel.ListMultiCamera = ListMultiCamera;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.多图采集.ToString();
            }

            set
            {
                value = FeatureType.多图采集.ToString();
            }
        }

        public MultiCameraSnapModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("结果图像1")]
            public HObject OutImage1 { get; set; }
            [Description("结果图像2")]
            public HObject OutImage2 { get; set; }
            [Description("结果图像3")]
            public HObject OutImage3 { get; set; }
            [Description("结果图像4")]
            public HObject OutImage4 { get; set; }
            [Description("结果图像5")]
            public HObject OutImage5 { get; set; }
            [Description("结果图像6")]
            public HObject OutImage6 { get; set; }
        }

    }

    public class MultiCameraClass
    {
        public int Id { get; set; }
        public string CameraName { get; set; }
        public int Exposure { get; set; }
        public int Gain { get; set; }
        public bool IsSetParam { get; set; }
        public int iLayOut { get; set; }

        public MultiCameraClass()
        {
            Exposure = 100;
            Gain = 1;
            IsSetParam = false;
            iLayOut = 0;
        }
    }

}
