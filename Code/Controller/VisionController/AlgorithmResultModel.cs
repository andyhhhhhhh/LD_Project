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
}