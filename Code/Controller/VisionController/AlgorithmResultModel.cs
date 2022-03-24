using BaseModels;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionController
{
    /// <summary>
    /// 算法返回结果类
    /// </summary>
    public class AlgorithmResultModel : BaseResultModel
    {
        public AlgorithmResultModel()
        {
            RunResult = false;
            ErrorMessage = "";
            strOCR = "";
        }
        
        /// <summary>
        /// OCR 结果
        /// </summary>
        public string strOCR { get; set; }

        /// <summary>
        /// OCR识别结果
        /// </summary>
        public bool OcrResult { get; set; }

        /// <summary>
        /// 字符旋转图像
        /// </summary>
        public HObject ImageRotateText= null;
        /// <summary>
        /// 结果XLD
        /// </summary>
        public HObject ProXLDTrans= null;
        /// <summary>
        /// 中心点
        /// </summary>
        public HObject CenterCross= null;

        /// <summary>
        /// 模板中心Row
        /// </summary>
        public HTuple ModelFindRow= null;
        /// <summary>
        /// 模板中心Column
        /// </summary>
        public HTuple ModelFindColumn= null;
        /// <summary>
        /// 模板中心Angle
        /// </summary>
        public HTuple ModelFindAngle= null;
        /// <summary>
        /// 模板分数
        /// </summary>
        public HTuple ModelFindScore= null;

        /// <summary>
        /// 中心点Row
        /// </summary>
        public HTuple CenterRow= null;
        /// <summary>
        /// 中心点Column
        /// </summary>
        public HTuple CenterColumn= null;
        /// <summary>
        /// 中心点角度
        /// </summary>
        public HTuple CenterPhi= null;
        /// <summary>
        /// 是否找到中心点
        /// </summary>
        public HTuple bFindCenter= null;

        /// <summary>
        /// 是否有旋转
        /// </summary>
        public bool bImageRotate = false;

        public HTuple Exception= null;
    }

    /// <summary>
    /// 大视野相机入料补正算法结果
    /// </summary>
    public class BigFeedResultModel : BaseResultModel
    {
        public HObject DispObjects { get; set; }
        public HObject Rectangle1 { get; set; }
        public HTuple ProdAngleMean { get; set; }
        public HTuple BigCenterRow { get; set; }
        public HTuple BigCenterCol { get; set; }

    }

    /// <summary>
    /// 大视野相机定位算法结果
    /// </summary>
    public class BigFixedResultModel : BaseResultModel
    {
        public HObject OutRegion { get; set; } 
        public HTuple AnyRow { get; set; }
        public HTuple AnyCol { get; set; }
        public HTuple AnyAng { get; set; }
        public HTuple Count { get; set; }
    }

    /// <summary>
    /// 小视野判断入料位置结果
    /// </summary>
    public class SmallJudgePosResultModel : BaseResultModel
    {
        public bool IsCenterPos { get; set; }
        public bool IsExistProduct { get; set; }
        public HObject OutRegion { get; set; }
        public double SubRow { get; set; }
        public double SubCol { get; set; }
    }

    /// <summary>
    /// 小视野定位置结果
    /// </summary>
    public class SmallFixedPosResultModel : BaseResultModel
    {
        public HObject OutRegion { get; set; }
        public HObject OcrOutRegion { get; set; }

        public string strLog { get; set; }
        public string Bar { get; set; }
        
        public double OcrCenterRow { get; set; }
        public double OcrCenterCol { get; set; }
        public double OcrCenterPhi { get; set; }
        public int ICanGet { get; set; }
        public int IExistProduct { get; set; } 
        public double Distance { get; set; }
        public string FirstOcr { get; set; }
        public string NeedOcr { get; set; }
    }

    /// <summary>
    /// P面算法结果
    /// </summary>
    public class PResultModel : BaseResultModel
    {
        public HObject DispObjects { get; set; }
    }

    /// <summary>
    /// N面算法结果
    /// </summary>
    public class NResultModel : BaseResultModel
    {
        public HObject DispObjects { get; set; }
        public double CenterRow { get; set; }
        public double CenterColumn { get; set; }
        public double CenterPhi { get; set; }
    }

    /// <summary>
    ///  产品结果判断类
    /// </summary>
    public class QResultModel
    {
        /// <summary>
        /// 当前OCR
        /// </summary>
        public string Ocr { get; set; }

        /// <summary>
        /// P面结果 0--OK 1--NG 2--疑似NG
        /// </summary>
        public EResult PResult { get; set; }

        /// <summary>
        /// P面结果 0--OK 1--NG 2--疑似NG
        /// </summary>
        public EResult NResult { get; set; }

        /// <summary>
        /// P面结果 0--OK 1--NG 2--疑似NG
        /// </summary>
        public EResult ARResult { get; set; }

        /// <summary>
        /// P面结果 0--OK 1--NG 2--疑似NG
        /// </summary>
        public EResult HRResult { get; set; }

        /// <summary>
        /// 结果枚举
        /// </summary>
        public enum EResult
        {
            EOK,
            ENG,
            ESeemNG,
        }
    }
}