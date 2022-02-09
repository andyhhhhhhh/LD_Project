using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 标定工具 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 测量标定Model
    /// </summary> 
    public class MeasureCaliModel : BaseSeqModel, ICalibration
    {
        public MeasureCaliType MeasureCaliType { get; set; }

        public string ImageForm { get; set; }

        public double PhyDistance { get; set; }

        public int MaxThreshold { get; set; }

        public double ScaleValue { get; set; }

        public double StartRow { get; set; }
        public double StartCol { get; set; } 
        public double EndRow { get; set; } 
        public double EndCol { get; set; }


        public MeasureCaliModel Clone()
        {
            MeasureCaliModel tModel = new MeasureCaliModel();
            tModel.Name = Name;
            tModel.MeasureCaliType = MeasureCaliType;
            tModel.ImageForm = ImageForm;
            tModel.PhyDistance = PhyDistance;
            tModel.MaxThreshold = MaxThreshold;
            tModel.ScaleValue = ScaleValue;
            tModel.StartRow = StartRow;
            tModel.StartCol = StartCol;
            tModel.EndRow = EndRow;
            tModel.EndCol = EndCol;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.测量标定.ToString();
            }

            set
            {
                value = FeatureType.测量标定.ToString();
            }
        }

        public MeasureCaliModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的像素精度")]
            public double ScaleValue { get; set; }
        } 
    }

    /// <summary>
    /// 测量标定模式类型
    /// </summary>
    public enum MeasureCaliType
    {
        孔板模式,
        比例模式
    }

    /// <summary>
    /// 九点标定
    /// </summary> 
    public class NineCaliModel : BaseSeqModel, ICalibration
    {
        public string ImageForm { get; set; }
        public double XDistance { get; set; }
        public double YDistance { get; set; }
        public double XStartPos { get; set; }
        public double YStartPos { get; set; }

        public string InputRow { get; set; }
        public string InputCol { get; set; }

        public string InputX { get; set; }
        public string InputY { get; set; }

        public bool IsSetPos { get; set; }

        public bool IsAutoCali { get; set; }

        public bool IsCheckResult { get; set; }

        public double OffSet { get; set; }

        public double sx { get; set; }
        public double sy { get; set; }
        public double tx { get; set; }
        public double ty { get; set; }
        public double theta { get; set; }
        public double phi { get; set; }
         
        public int PointNum { get; set; }

        public bool IsRelative { get; set; }

        public bool IsRowToX { get; set; }

        public string CaliIndexForm { get; set; }

        [XmlIgnore]
        public int CaliIndex { get; set; }

        public List<NCalibPos> listCalibPos { get; set; }
        public NineCaliModel Clone()
        {
            NineCaliModel tModel = new NineCaliModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.XDistance = XDistance;
            tModel.YDistance = YDistance;
            tModel.XStartPos = XStartPos;
            tModel.YStartPos = YStartPos;
            tModel.InputRow = InputRow;
            tModel.InputCol = InputCol;
            tModel.InputX = InputX;
            tModel.InputY = InputY;
            tModel.IsSetPos = IsSetPos;
            tModel.IsAutoCali = IsAutoCali;
            tModel.IsCheckResult = IsCheckResult;
            tModel.OffSet = OffSet;
            tModel.PointNum = PointNum;
            tModel.IsRelative = IsRelative;
            tModel.IsRowToX = IsRowToX;
            tModel.CaliIndexForm = CaliIndexForm;
            
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.N点标定.ToString();
            }

            set
            {
                value = FeatureType.N点标定.ToString();
            }
        }

        public NineCaliModel()
        {
            itemResult = new ItemResult();  
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("输出的像素精度")]
            public double ScaleValue { get; set; }
        }
    }

    /// <summary>
    /// 九点标定点位的类
    /// </summary>
    public class NCalibPos
    {
        private int index;
        private double px;
        private double py;
        private double rx;
        private double ry;

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        /// <summary>
        /// 像素X
        /// </summary>
        public double Px
        {
            get
            {
                return px;
            }

            set
            {
                px = value;
            }
        }

        /// <summary>
        /// 像素Y
        /// </summary>
        public double Py
        {
            get
            {
                return py;
            }

            set
            {
                py = value;
            }
        }

        /// <summary>
        /// 物理X
        /// </summary>
        public double Rx
        {
            get
            {
                return rx;
            }

            set
            {
                rx = value;
            }
        }

        /// <summary>
        /// 物理Y
        /// </summary>
        public double Ry
        {
            get
            {
                return ry;
            }

            set
            {
                ry = value;
            }
        }
    }


    /// <summary>
    /// 坐标映射Model
    /// </summary 
    public class CoordMappingModel : BaseSeqModel, ICalibration
    {
        public string CoordForm { get; set; }

        public CoordMappingType CoordMappingType { get; set; }
         
        public bool bPointMapping { get; set; }

        public string InputX { get; set; }
        public string InputY { get; set; }

        public string InputAngle { get; set; }


        public CoordMappingModel Clone()
        {
            CoordMappingModel tModel = new CoordMappingModel();
            tModel.Name = Name;
            tModel.CoordForm = CoordForm;
            tModel.CoordMappingType = CoordMappingType;
            tModel.bPointMapping = bPointMapping;
            tModel.InputX = InputX;
            tModel.InputY = InputY;
            tModel.InputAngle = InputAngle;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.坐标映射.ToString();
            }

            set
            {
                value = FeatureType.坐标映射.ToString();
            }
        }

        public CoordMappingModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("映射的结果X")]
            public double OutPutX { get; set; }

            [Description("映射的结果Y")]
            public double OutPutY { get; set; }

            [Description("映射的结果X数组")]
            public double[] ArrOutPutX { get; set; }

            [Description("映射的结果Y数组")]
            public double[] ArrOutPutY { get; set; }
        }
    }

    /// <summary>
    /// 坐标映射的类型Enum
    /// </summary>
    public enum CoordMappingType
    {
        图像坐标到物理坐标,
        物理坐标到图像坐标,
        图像坐标到图像坐标,
        物理坐标到物理坐标,
    }

    /// <summary>
    /// 畸变标定Model
    /// </summary> 
    public class DistrotionCaliModel : BaseSeqModel, ICalibration
    {
        public string CailPicPath { get; set; }
        public string CailFilePath { get; set; }
        public double MaskDist { get; set; }
        public double DiameterRatio { get; set; }
        public int PointNum { get; set; }
        public double Focus { get; set; }
        public double PixX { get; set; }
        public double PixY { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }


        public DistrotionCaliModel Clone()
        {
            DistrotionCaliModel tModel = new DistrotionCaliModel();
            tModel.Name = Name;
            tModel.CailPicPath = CailPicPath;
            tModel.CailFilePath = CailFilePath;
            tModel.MaskDist = MaskDist;
            tModel.DiameterRatio = DiameterRatio;
            tModel.Focus = Focus;
            tModel.PixX = PixX;
            tModel.PixY = PixY;
            tModel.Height = Height;
            tModel.Width = Width;
            tModel.PointNum = PointNum;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.畸变标定.ToString();
            }

            set
            {
                value = FeatureType.畸变标定.ToString();
            }
        }

        public DistrotionCaliModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            
        }
    }

    /// <summary>
    /// 仿射变换Model
    /// </summary>
    public class AffineTransModel : BaseSeqModel, ICalibration
    { 
        public AffineTransType AffineTransType { get; set; }        

        public string FormX { get; set; }
        public string FormY { get; set; }
        public string FormAngle { get; set; }
        public string ToX { get; set; }
        public string ToY { get; set; }
        public string ToAngle { get; set; }

        public bool IsPoint { get; set; }

        public AffineTransModel Clone()
        {
            AffineTransModel tModel = new AffineTransModel();
            tModel.Name = Name;
            tModel.FormX = FormX;
            tModel.FormY = FormY;
            tModel.FormAngle = FormAngle;
            tModel.ToX = ToX;
            tModel.ToY = ToY;
            tModel.ToAngle = ToAngle;
            tModel.AffineTransType = AffineTransType;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.仿射变换.ToString();
            }

            set
            {
                value = FeatureType.仿射变换.ToString();
            }
        }

        public AffineTransModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 
            [Description("变换的X值")]
            public double OutPutX { get; set; }

            [Description("变换的Y值")]
            public double OutPutY { get; set; }
        }
    }
    /// <summary>
    /// 仿射变换类型Enum
    /// </summary>
    public enum AffineTransType
    {
        点到点,
        点角度到点角度,
        旋转中心
    }

    /// <summary>
    /// 矩阵变换Model
    /// </summary 
    public class MatTransModel : BaseSeqModel, ICalibration
    {
        public string CoordForm { get; set; } 

        public string MapType { get; set; }

        public bool IsIdentity { get; set; }

        public string CenterX { get; set; }

        public string CenterY { get; set; }
        
        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public MatTransModel Clone()
        {
            MatTransModel tModel = new MatTransModel();
            tModel.Name = Name;
            tModel.CoordForm = CoordForm;
            tModel.IsIdentity = IsIdentity;
            tModel.MapType = MapType;
            tModel.CenterX = CenterX;
            tModel.CenterY = CenterY;
            tModel.Value1 = Value1;
            tModel.Value2 = Value2;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.矩阵变换.ToString();
            }

            set
            {
                value = FeatureType.矩阵变换.ToString();
            }
        }

        public MatTransModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            
        }
    }


    /// <summary>
    /// 区域映射Model
    /// </summary 
    public class RegionMapModel : BaseSeqModel, ICalibration
    {
        public string CoordForm { get; set; }
        
        public string RegionForm { get; set; }

        public bool IsImage { get; set; }
        public bool IsRegion { get; set; }
        public bool IsContour { get; set; }

        public bool IsModelForm { get; set; }
        public string ModelForm { get; set; }

        public RegionMapModel Clone()
        {
            RegionMapModel tModel = new RegionMapModel();
            tModel.Name = Name;
            tModel.CoordForm = CoordForm;
            tModel.RegionForm = RegionForm;
            tModel.IsImage = IsImage;
            tModel.IsRegion = IsRegion;
            tModel.IsContour = IsContour;
            tModel.ModelForm = ModelForm;
            tModel.IsModelForm = IsModelForm;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域映射.ToString();
            } 
            set
            {
                value = FeatureType.区域映射.ToString();
            }
        }

        public RegionMapModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("映射的区域结果")]
            public HObject OutRegion { get; set; }
        }
    }

    /// <summary>
    /// 旋转中心Model
    /// </summary>
    public class RotateCenterModel : BaseSeqModel, ICalibration
    {
        public string FormX { get; set; }
        public string FormY { get; set; } 

        public RotateCenterModel Clone()
        {
            RotateCenterModel tModel = new RotateCenterModel();
            tModel.Name = Name;
            tModel.FormX = FormX;
            tModel.FormY = FormY; 

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.旋转中心.ToString();
            }

            set
            {
                value = FeatureType.旋转中心.ToString();
            }
        }

        public RotateCenterModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

}
