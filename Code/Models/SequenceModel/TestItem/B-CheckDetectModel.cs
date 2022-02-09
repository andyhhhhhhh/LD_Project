using HalconDotNet;
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
    /// 读二维码Model
    /// </summary>
    public class ReadCode2DModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string CodeType { get; set; }
        public bool IsRegion { get; set; }
        public string RegionForm { get; set; }

        public bool IsJudge { get; set; }
        public string JudgeValue { get; set; }
        public ReadCode2DModel Clone()
        {
            ReadCode2DModel tModel = new ReadCode2DModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.CodeType = CodeType;
            tModel.IsRegion = IsRegion;
            tModel.RegionForm = RegionForm;
            tModel.IsJudge = IsJudge;
            tModel.JudgeValue = JudgeValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.读二维码.ToString();
            }

            set
            {
                value = FeatureType.读二维码.ToString();
            }
        }

        public ReadCode2DModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的二维码结果")]
            public string Code2DValue { get; set; }

            [Description("输出的二维码List")]
            public string[] ListCode2DValue { get; set; }
        }
    }

    /// <summary>
    /// 读一维码Model
    /// </summary>
    public class ReadBarCodeModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string CodeType { get; set; }
        public bool IsRegion { get; set; }
        public string RegionForm { get; set; }
        public ReadBarCodeModel Clone()
        {
            ReadBarCodeModel tModel = new ReadBarCodeModel();
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
                return FeatureType.读一维码.ToString();
            }

            set
            {
                value = FeatureType.读一维码.ToString();
            }
        }

        public ReadBarCodeModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的一维码结果")]
            public string BarCodeValue { get; set; }
        }
    }

    /// <summary>
    /// 字符识别Model
    /// </summary>
    public class OcrModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string OcrType { get; set; }
        public bool IsRegion { get; set; }
        public string RegionForm { get; set; }

        public bool IsTrain { get; set; }
        public string TrainName { get; set; }

        public int MinContrast { get; set; }
        public int MinCharHeight { get; set; }
        public int MaxCharHeight { get; set; }
        public int MinCharWidth { get; set; }
        public int MaxCharWidth { get; set; }
        public int MinCharArea { get; set; }
        public int MaxCharArea { get; set; }

        public string CharColor { get; set; }
        public OcrModel Clone()
        {
            OcrModel tModel = new OcrModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.OcrType = OcrType;
            tModel.IsRegion = IsRegion;
            tModel.RegionForm = RegionForm;
            tModel.IsTrain = IsTrain;
            tModel.TrainName = TrainName;
            tModel.MinContrast = MinContrast;
            tModel.MinCharHeight = MinCharHeight;
            tModel.MaxCharHeight = MaxCharHeight;
            tModel.MinCharWidth = MinCharWidth;
            tModel.MaxCharWidth = MaxCharWidth;
            tModel.MinCharArea = MinCharArea;
            tModel.MaxCharArea = MaxCharArea;
            tModel.CharColor = CharColor;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.字符识别.ToString();
            }

            set
            {
                value = FeatureType.字符识别.ToString();
            }
        }

        public OcrModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的字符结果")]
            public string OcrValue { get; set; }
        }
    }



    /// <summary>
    /// 检测点 Model
    /// </summary>
    public class CheckAreaModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string CalValue { get; set; }//检测点的计算方式

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsFillDisplay { get; set; }

        public CheckAreaModel Clone()
        {
            CheckAreaModel tModel = new CheckAreaModel();
            tModel.Name = Name;
            tModel.Id = Id;
            tModel.CalValue = CalValue;
            tModel.ImageForm = ImageForm;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
            tModel.IsDisplay = IsDisplay;
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
                return FeatureType.检测点.ToString();
            }

            set
            {
                value = FeatureType.检测点.ToString();
            }
        }

        public CheckAreaModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的Region")]
            public HObject CheckRegion { get; set; }
        }
    }


    /// <summary>
    /// 基准面Model
    /// </summary>
    public class BaseLevelModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }
        public bool IsDisplay { get; set; }
        public BaseLevelModel Clone()
        {
            BaseLevelModel tModel = new BaseLevelModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
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
                return FeatureType.基准面.ToString();
            }

            set
            {
                value = FeatureType.基准面.ToString();
            }
        }

        public BaseLevelModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的基准Region")]
            public HObject BaseRegion { get; set; }
        }
    }


    /// <summary>
    /// 矩形阵列Model
    /// </summary>
    public class RectArrayModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }
        public bool IsDisplay { get; set; }

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int EndRow { get; set; }
        public int EndCol { get; set; }
        public int Length1 { get; set; }
        public int Length2 { get; set; }
        public int Angle { get; set; }
        public RectArrayModel Clone()
        {
            RectArrayModel tModel = new RectArrayModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
            tModel.IsDisplay = IsDisplay; 

            tModel.RowCount = RowCount;
            tModel.ColumnCount = ColumnCount;
            tModel.StartRow = StartRow;
            tModel.StartCol = StartCol;
            tModel.EndRow = EndRow;
            tModel.EndCol = EndCol;
            tModel.Length1 = Length1;
            tModel.Length2 = Length2;
            tModel.Angle = Angle;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.矩形阵列.ToString();
            }

            set
            {
                value = FeatureType.矩形阵列.ToString();
            }
        }

        public RectArrayModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的阵列Region")]
            public HObject RectRegion { get; set; }
        }
    }

    /// <summary>
    /// 圆形阵列Model
    /// </summary>
    public class CircleArrayModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }
        public bool IsDisplay { get; set; }

        public int CircleRow { get; set; }
        public int CircleCol { get; set; }
        public int CircleRadius { get; set; }
        public int RectCount { get; set; } 
        public int Length1 { get; set; }
        public int Length2 { get; set; }
        public int Angle { get; set; }
        public int StartAngle { get; set; }
        public CircleArrayModel Clone()
        {
            CircleArrayModel tModel = new CircleArrayModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsModelForm = IsModelForm;
            tModel.ModelForm = ModelForm;
            tModel.IsDisplay = IsDisplay;

            tModel.CircleRow = CircleRow;
            tModel.CircleCol = CircleCol;
            tModel.CircleRadius = CircleRadius;
            tModel.RectCount = RectCount;
            tModel.Length1 = Length1;
            tModel.Length2 = Length2;
            tModel.Angle = Angle;
            tModel.StartAngle = StartAngle;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.圆形阵列.ToString();
            }

            set
            {
                value = FeatureType.圆形阵列.ToString();
            }
        }

        public CircleArrayModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的阵列Region")]
            public HObject CircleRegion { get; set; }
        }
    }


    /// <summary>
    /// 检测亮度Model
    /// </summary>
    public class CheckLightModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public double Row { get; set; }
        public double Column { get; set; }
        public double Phi { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }

        public string RegionForm { get; set; }
        public bool IsManualRegion { get; set; }

        public CheckLightModel Clone()
        {
            CheckLightModel tModel = new CheckLightModel();
            tModel.Name = Name;
            tModel.Row = Row;
            tModel.Column = Column;
            tModel.Phi = Phi;
            tModel.Length1 = Length1;
            tModel.Length2 = Length2;
            tModel.RegionForm = RegionForm;
            tModel.IsManualRegion = IsManualRegion;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.检测亮度.ToString();
            }

            set
            {
                value = FeatureType.检测亮度.ToString();
            }
        }

        public CheckLightModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的亮度值")]
            public double OutResult { get; set; }
        }
    }


    /// <summary>
    /// 创建ROI区域Model
    /// </summary>
    public class CreateRegionModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public bool IsRect2 { get; set; }
        public bool IsCircle { get; set; }
        public bool IsCircleArc { get; set; }
        public bool IsDoubleCircle { get; set; }
        public bool IsManualRegion { get; set; }

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }


        public CreateRegionModel Clone()
        {
            CreateRegionModel tModel = new CreateRegionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.IsRect2 = IsRect2;
            tModel.IsCircle = IsCircle;
            tModel.IsCircleArc = IsCircleArc;
            tModel.IsDoubleCircle = IsDoubleCircle;
            tModel.IsManualRegion = IsManualRegion;
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
                return FeatureType.创建ROI.ToString();
            }

            set
            {
                value = FeatureType.创建ROI.ToString();
            }
        }

        public CreateRegionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的ROI")]
            public HObject Region { get; set; }
        }
    }


    /// <summary>
    /// 统计像素Model
    /// </summary>
    public class CountPixModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public double MinGray { get; set; }
        public double MaxGray { get; set; }

        public CountPixModel Clone()
        {
            CountPixModel tModel = new CountPixModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.统计像素.ToString();
            }

            set
            {
                value = FeatureType.统计像素.ToString();
            }
        }

        public CountPixModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的像素个数")]
            public int PixCount { get; set; }
        }
    }

    /// <summary>
    /// 测清晰度Model
    /// </summary>
    public class DefinitionModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string DefinitionType { get; set; } 

        public DefinitionModel Clone()
        {
            DefinitionModel tModel = new DefinitionModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.DefinitionType = DefinitionType;  

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.测清晰度.ToString();
            }

            set
            {
                value = FeatureType.测清晰度.ToString();
            }
        }

        public DefinitionModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的清晰值")]
            public double Value { get; set; }
        }
    }


    /// <summary>
    /// 检测正反Model
    /// </summary>
    public class CheckReverseModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
       
        public string RegionForm { get; set; }

        public int RegionRadius { get; set; }
        public int DilRadius { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }


        public CheckReverseModel Clone()
        {
            CheckReverseModel tModel = new CheckReverseModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.RegionRadius = RegionRadius;
            tModel.DilRadius = DilRadius;
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
                return FeatureType.检测正反.ToString();
            }

            set
            {
                value = FeatureType.检测正反.ToString();
            }
        }

        public CheckReverseModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 

        }
    }

    /// <summary>
    /// 字符训练Model
    /// </summary>
    public class OcrTrainModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public string CharValue { get; set; }

        public OcrTrainModel Clone()
        {
            OcrTrainModel tModel = new OcrTrainModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.CharValue = CharValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.字符训练.ToString();
            }

            set
            {
                value = FeatureType.字符训练.ToString();
            }
        }

        public OcrTrainModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的字符值")]
            public string OcrValue { get; set; }
        }
    }

    /// <summary>
    /// 颜色抽取Model
    /// </summary>
    public class ColorExtractModel : BaseSeqModel, ICheckDetect
    {
        public string ImageForm { get; set; }
        public string RegionForm { get; set; }
        public int MinGray { get; set; }
        public int MaxGray { get; set; }
        public string ColorType { get; set; } 
        public  int MinArea { get; set; }

        public ColorExtractModel Clone()
        {
            ColorExtractModel tModel = new ColorExtractModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.RegionForm = RegionForm;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;
            tModel.ColorType = ColorType;
            tModel.MinArea = MinArea;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.颜色抽取.ToString();
            }

            set
            {
                value = FeatureType.颜色抽取.ToString();
            }
        }

        public ColorExtractModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
}
