using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SequenceTestModel
{
    /// <summary>
    /// 算法参数类
    /// </summary
    public class AlgorithmModel : BaseAlgorithmModel
    {
        public AlgorithmModel()
        {
            AngleStartFind = -0.5;
            AngleExtentFind = 0.5;
            MinScoreFind = 0.3;
            NumMatchesFind = 1;
            NumLevelsFind = 0;
            Greediness = 0.9;
            ModelRow = 0.0;
            ModelColumn = 0.0;
            ModelAngle = 0.0;
            Contrast = 20;//模板对比度

            InMeasureLength1 = 200;
            InMeasureLength2 = 10;
            InMeasureSigma = 0.8;
            InMeasureThreshold = 10;
            InMeasureSelect = "first"; // 'first'
            InMeasureTransition = "positive";// 'positive'
            InMeasureNumber = 50;
            InMeasureScore = 0.5;
            ModelID = "";
            HFileParamPath = "";
            strOCRPath = "";
        }

        public int ThresholdMin { get; set; }
        public int ThresholdMax { get; set; }

        public string ModelID { get; set; }
        public double AngleStartFind { get; set; }
        public double AngleExtentFind { get; set; }
        public double MinScoreFind { get; set; }
        public int NumMatchesFind { get; set; }
        public int NumLevelsFind { get; set; }
        public double Greediness { get; set; }
        public int Contrast { get; set; }
        public double ModelRow { get; set; }
        public double ModelColumn { get; set; }
        public double ModelAngle { get; set; }
        public double DrawLineStartRow { get; set; }
        public double DrawLineStartCol { get; set; }
        public double DrawLineEndRow { get; set; }
        public double DrawLineEndCol { get; set; }
        public double InMeasureLength1 { get; set; }
        public double InMeasureLength2 { get; set; }
        public double InMeasureSigma { get; set; }
        public int InMeasureThreshold { get; set; }
        public string InMeasureSelect { get; set; }
        public string InMeasureTransition { get; set; }
        public int InMeasureNumber { get; set; }
        public double InMeasureScore { get; set; }
        public string HFileParamPath { get; set; }
        public string strOCRPath { get; set; }

        [XmlIgnore]
        public bool IsShieldOcr { get; set; }

        [XmlIgnore]
        public HObject Image { get; set; }
    }

    /// <summary>
    /// 治具示教算法参数
    /// </summary>
    public class FixtureAlgorithmModel : BaseAlgorithmModel
    {
        public int ExposureTime { get; set; }

        [Category("算法参数")]
        public int InMeasureLength1 { get; set; }
        [Category("算法参数")]
        public int InMeasureLength2 { get; set; }
        [Category("算法参数")]
        public double InMeasureSigma { get; set; }
        [Category("算法参数")]
        public int InMeasureThreshold { get; set; }
        [Category("算法参数")]
        public string InMeasureSelect { get; set; }
        [Category("算法参数")]
        public string InMeasureTransition { get; set; }
        [Category("算法参数")]
        public int InMeasureNumber { get; set; }
        [Category("算法参数")]
        public double InMeasureScore { get; set; }

        [Category("图像中心")]
        public double Row { get; set; }
        [Category("图像中心")]
        public double Column { get; set; }
        [Category("图像中心")]
        public double Angle { get; set; }

        [XmlIgnore]
        public HObject Image { get; set; }

        public FixtureAlgorithmModel()
        {
            ExposureTime = 4000;
            InMeasureLength1 = 200;
            InMeasureLength2 = 10;
            InMeasureSigma = 0.8;
            InMeasureThreshold = 10;
            InMeasureSelect = "first"; // 'first'
            InMeasureTransition = "positive";// 'positive'
            InMeasureNumber = 50;
            InMeasureScore = 0.5;
        }
    }

    /// <summary>
    /// 大视野相机入料补正算法参数
    /// </summary>
    public class BigFeedAlgorithmModel : BaseAlgorithmModel
    {
        [CategoryAttribute("算法参数"), Description("GrayOpenHeight1")]
        public int GrayOpenHeight { get; set; }

        [CategoryAttribute("算法参数"), Description("GrayOpenWidth1")]
        public int GrayOpenWidth { get; set; }

        [CategoryAttribute("算法参数"), Description("DynThr1")]
        public int DynThr { get; set; }

        [CategoryAttribute("算法参数"), Description("HysThrMin1")]
        public int HysThrMin { get; set; }

        [CategoryAttribute("算法参数"), Description("HysThrMax1")]
        public int HysThrMax { get; set; }

        [CategoryAttribute("算法参数"), Description("SelectAreaMin1")]
        public int SelectAreaMin { get; set; }

        [CategoryAttribute("算法参数"), Description("SingleSelectAreaMin1")]
        public int SingleSelectAreaMin { get; set; }

        [CategoryAttribute("算法参数"), Description("SingleSelectAreaMax1")]
        public int SingleSelectAreaMax { get; set; }

        [CategoryAttribute("算法参数"), Description("ClosingW1")]
        public int ClosingW { get; set; }

        [CategoryAttribute("算法参数"), Description("ClosingH1")]
        public int ClosingH { get; set; }
        

        [CategoryAttribute("区域参数"), Description("起点Row")]
        public double Row1 { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Column")]
        public double Column1 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Row")]
        public double Row2 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Column")]
        public double Column2 { get; set; }

        [XmlIgnore]
        public HObject Image { get; set; }
    }

    /// <summary>
    /// 大视野相机定位算法参数
    /// </summary>
    public class BigFixedAlgorithmModel : BaseAlgorithmModel
    {


        [CategoryAttribute("区域参数"), Description("起点Row")]
        public double Row1 { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Column")]
        public double Column1 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Row")]
        public double Row2 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Column")]
        public double Column2 { get; set; }

        [XmlIgnore]
        public HObject Image { get; set; }
    }

    /// <summary>
    /// 小视野入料补正算法参数
    /// </summary>
    public class SmallJudgePosModel:BaseAlgorithmModel
    {
        [XmlIgnore]
        public HObject Image { get; set; }

        [CategoryAttribute("算法参数"), Description("最小灰度值")]
        public int MinGray { get; set; }

        [CategoryAttribute("算法参数"), Description("最小与中间值差")]
        public int MinSubCol { get; set; }
    }


    /// <summary>
    /// 小视野定位算法参数
    /// </summary>
    public class SmallFixedPosModel : BaseAlgorithmModel
    {
        [XmlIgnore]
        public HObject Image { get; set; }
        [XmlIgnore]
        public bool IsIngoreCalu { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Row")]
        public double Row1 { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Column")]
        public double Column1 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Row")]
        public double Row2 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Column")]
        public double Column2 { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Row")]
        public double OcrRow1 { get; set; }

        [CategoryAttribute("区域参数"), Description("起点Column")]
        public double OcrColumn1 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Row")]
        public double OcrRow2 { get; set; }

        [CategoryAttribute("区域参数"), Description("终点Column")]
        public double OcrColumn2 { get; set; }


        [CategoryAttribute("模板参数"), Description("最小灰度值")]
        public double StartingAngle { get; set; }

        [CategoryAttribute("模板参数"), Description("最小与中间值差")]
        public double AngleExtent { get; set; }

        [CategoryAttribute("模板参数"), Description("金字塔等级")]
        public int PyramidLevel { get; set; }

        [CategoryAttribute("模板参数"), Description("最大金字塔级别,确定速度")]
        public int LastPyramidLevel { get; set; }

        [CategoryAttribute("模板参数"), Description("最小分数")]
        public double MinScore { get; set; }

        [CategoryAttribute("模板参数"), Description("贪心算法")]
        public double Greediness { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int ThrsholdMin { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int HysThresholdMin { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int HysThesholdMax { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int CloseRec { get; set; }        

        [CategoryAttribute("算法参数"), Description("")]
        public int IsNotProdValue { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int DistancePP { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int AutoThreshold { get; set; }

        [CategoryAttribute("算法参数"), Description("")]
        public int ModelRegionAreaDownValue { get; set; }
        

        [CategoryAttribute("OCR参数"), Description("OCR的长度设置")]
        public int OcrLength { get; set; }
        [CategoryAttribute("OCR参数"), Description("确定Bar条取得个数")]
        public int JudgeBarLength { get; set; } 

        [CategoryAttribute("OCR参数"), Description("OCR训练文件路径")]
        public string OcrBinPath { get; set; }

        [CategoryAttribute("OCR参数"), Description("需要的OCR")]
        public string NeedOcr { get; set; }
    }

    /// <summary>
    /// P面检测算法参数
    /// </summary>
    public class AlgorithmModelP : BaseAlgorithmModel
    {
        [Category("建模参数")]
        public int DynThr { get; set; }

        [Category("建模参数")]
        public int HysThrMin { get; set; }

        [Category("建模参数")]
        public int HysThrMax { get; set; }
        [Category("建模参数")]
        public int DynRThr { get; set; }

        [Category("崩缺断料检测参数")]
        public int HysRThrMin { get; set; }

        [Category("崩缺断料检测参数")]
        public int HysRThrMax { get; set; }

        [Category("崩缺断料检测参数")]
        public int BenQueAreaMin { get; set; }

        [Category("崩缺断料检测参数")]
        public int BQwidth { get; set; }

        [Category("崩缺断料检测参数")]
        public int BQheight { get; set; }

        [Category("崩缺断料检测参数")]
        public int DarkThrMin { get; set; }

        [Category("裂纹检测")]
        public int LightThrMax { get; set; }

        [Category("裂纹检测")]
        public int ZangWuAreaMin { get; set; }

        [Category("区域参数")]
        public double Row1 { get; set; }
        [Category("区域参数")]
        public double Row2 { get; set; }
        [Category("区域参数")]
        public double Column1 { get; set; }
        [Category("区域参数")]
        public double Column2 { get; set; }

        [Category("区域参数")]
        public double SRow1 { get; set; }
        [Category("区域参数")]
        public double SRow2 { get; set; }
        [Category("区域参数")]
        public double SColumn1 { get; set; }
        [Category("区域参数")]
        public double SColumn2 { get; set; }

        [XmlIgnore]
        public HObject Image { get; set; }


        public AlgorithmModelP()
        {
            DynThr = 15;
            HysThrMin = 50;
            HysThrMax = 150;
            DynRThr = 15;
            HysRThrMin = 50;
            HysRThrMax = 150;
            BenQueAreaMin = 2000;
            BQwidth = 40;
            BQheight = 40;
            DarkThrMin = 50;

            LightThrMax = 220;
            ZangWuAreaMin = 3000;
        }
    }
    
    /// <summary>
    /// N面检测算法参数
    /// </summary>
    public class AlgorithmModelN : BaseAlgorithmModel
    {
        [Category("建模参数")]
        public int ProDynThr { get; set; }

        [Category("建模参数")]
        public int ProHysMin { get; set; }

        [Category("建模参数")]
        public int CloseWH { get; set; }
        [Category("建模参数")]
        public int ProHysMax { get; set; }

        [Category("崩缺断料检测参数")]
        public int MaskMean { get; set; }

        [Category("崩缺断料检测参数")]
        public int DynThreshold { get; set; }

        [Category("崩缺断料检测参数")]
        public int CloseWidth { get; set; }

        [Category("崩缺断料检测参数")]
        public int CloseHeight { get; set; }

        [Category("崩缺断料检测参数")]
        public int BQAreaMin { get; set; }

        [Category("崩缺断料检测参数")]
        public int BQWidthHeight { get; set; }

        [Category("裂纹检测")]
        public int DiffValue { get; set; }

        [Category("裂纹检测")]
        public int LieWenNum { get; set; }

        [Category("裂纹检测")]
        public int stdWH { get; set; }

        [Category("裂纹检测")]
        public int HysthrMin { get; set; }

        [Category("裂纹检测")]
        public int HysthrMax { get; set; }

        [Category("大区域缺陷")]
        public int DustAreaMin { get; set; }

        [Category("大区域缺陷")]
        public int DustWidth { get; set; }

        [Category("大区域缺陷")]
        public int DustHeight { get; set; }



        [Category("区域参数")]
        public double Row1 { get; set; }
        [Category("区域参数")]
        public double Row2 { get; set; }
        [Category("区域参数")]
        public double Column1 { get; set; }
        [Category("区域参数")]
        public double Column2 { get; set; }


        [XmlIgnore]
        public HObject Image { get; set; }
        public AlgorithmModelN()
        {
            ProDynThr = 20;
            ProHysMin = 100;
            ProHysMax = 180;
            CloseWH = 200;
            MaskMean = 500;
            DynThreshold = 35;
            CloseWidth = 100;
            CloseHeight = 100;
            BQAreaMin = 10000;
            BQWidthHeight = 30;

            DiffValue = 80;
            LieWenNum = 5;
            stdWH = 3;
            HysthrMin = 150;
            HysthrMax = 180;
            DustAreaMin = 20000;
            DustWidth = 20;
            DustHeight = 20;
        }
    }

}
