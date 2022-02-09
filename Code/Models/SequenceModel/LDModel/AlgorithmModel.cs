using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Xml.Serialization;

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
    public class FixtureAlgorithmModel
    {
        public int ExposureTime { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int InMeasureLength1 { get; set; }
        public int InMeasureLength2 { get; set; }
        public double InMeasureSigma { get; set; }
        public int InMeasureThreshold { get; set; }
        public string InMeasureSelect { get; set; }
        public string InMeasureTransition { get; set; }
        public int InMeasureNumber { get; set; }
        public double InMeasureScore { get; set; }

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
}
