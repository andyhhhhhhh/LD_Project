using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDesigner;
using VisionDesigner.MVDCNNOCR;

namespace VisionController
{
    public class HKOcr
    {
        public static CNNOCRTool m_stCNNOCRToolObj = null;
        public static CMvdImage m_stInputImage = null;
        public static MVD_ALGORITHM_PLATFORM_TYPE m_enPlatformType = MVD_ALGORITHM_PLATFORM_TYPE.MVD_ALGORITHM_PLATFORM_CPU;
        public static CNNOCRBasicParam m_cocrbasicp = null;

        private static string m_ProductInfo = "";
        /// <summary>
        /// 初始化OCR
        /// </summary>
        public static void InitOCR(string path)
        {
            try
            {
                if (m_stCNNOCRToolObj == null)
                {
                    m_stCNNOCRToolObj = new CNNOCRTool(m_enPlatformType);

                    m_stCNNOCRToolObj.BasicParam.LoadModel(path);
                    m_cocrbasicp = m_stCNNOCRToolObj.BasicParam;

                    m_ProductInfo = "28W";
                }
                else
                {
                    if (m_ProductInfo != "28W")
                    {
                        m_stCNNOCRToolObj = new CNNOCRTool(m_enPlatformType);
                        m_stCNNOCRToolObj.BasicParam.LoadModel(path);
                        m_cocrbasicp = m_stCNNOCRToolObj.BasicParam;
                        m_ProductInfo = "28W";
                    }
                }
                if (m_stInputImage == null)
                {
                    m_stInputImage = new CMvdImage();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static string OcrReadAction(string imagePath)
        {
            try
            {
                m_stInputImage.InitImage(imagePath);
                MVD_IMAGE_DATA_INFO stInputImageData = m_stInputImage.GetImageData();
                CMvdImage shallowCopyImage = new CMvdImage();
                shallowCopyImage.InitImage(m_stInputImage.Width, m_stInputImage.Height, m_stInputImage.PixelFormat, stInputImageData);
                string ocr = OCRRead(m_stInputImage);

                if (string.IsNullOrEmpty(ocr.Trim()))
                {
                    ocr = "0";
                }
                return ocr;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        //OCR 深度学习
        public static string OCRRead(CMvdImage HKimage)
        {
            if ((null == m_stCNNOCRToolObj) || null == HKimage)
            {
                string str = "OCR参数未初始化！";
                return null;
            }

            double fOCRStartTime = GetTimeStamp();
            m_stCNNOCRToolObj.InputImage = HKimage;
            //m_stCNNOCRToolObj.BasicParam.LoadModel(tModel.strOCRPath);
            //cocrbasicp = m_stCNNOCRToolObj.BasicParam;
            m_stCNNOCRToolObj.Run();
            double fOCRCostTime = GetTimeStamp() - fOCRStartTime;
            if (m_stCNNOCRToolObj.Result.RecogInfoList.Count > 0)
            {
                return m_stCNNOCRToolObj.Result.RecogInfoList[0].RecogString;
            }
            else
            {
                return null;
            }
        }

        private static double GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
    }
}
