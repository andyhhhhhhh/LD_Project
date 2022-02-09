using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDesigner;
using VisionDesigner.MVDCNNOCR;
using SequenceTestModel;
using BaseModels;
using AlgorithmController;

namespace VisionController
{
    /// <summary>
    /// COS定位算法与OCR识别算法
    /// </summary>
    public class AlgorithmControl : IAlgorithmControl
    {
        public CNNOCRTool m_stCNNOCRToolObj = null;
        private CMvdImage m_stInputImage = null;
        private MVD_ALGORITHM_PLATFORM_TYPE m_enPlatformType = MVD_ALGORITHM_PLATFORM_TYPE.MVD_ALGORITHM_PLATFORM_CPU;
        private CNNOCRBasicParam m_cocrbasicp = null;

        public AlgorithmControl()
        {
            //InitOCR();
        }
        
        public bool Init(object parameter)
        {
            return true;
        }

        public BaseResultModel Run(BaseAlgorithmModel controlModel, AlgorithmControlType controlType)
        { 
            AlgorithmResultModel resultModel = new AlgorithmResultModel(); 
            AlgorithmModel tModel = controlModel as AlgorithmModel;
            switch (controlType)
            {
                case AlgorithmControlType.AlgorithmInit:
                    resultModel.RunResult = Init(tModel);
                    break;
                case AlgorithmControlType.AlgorithmRun:
                    HObject ho_Image = tModel.Image;
                    ///算法主体... 
                    HObject emptyRegion = null;
                    HObject ho_OutObj = null;
                    HOperatorSet.GenEmptyRegion(out emptyRegion);
                    HOperatorSet.GenEmptyRegion(out ho_OutObj);
                    string errMsg = InitOCR(tModel);

                    HTuple levels = new HTuple(new int[] { tModel.NumLevelsFind, -3 }); 
                    FindCenterPos(ho_Image, emptyRegion, emptyRegion, tModel.HFileParamPath, emRunModel.AutoRun,
                                    out resultModel.ImageRotateText, out resultModel.ProXLDTrans, out resultModel.CenterCross,
                                    tModel.ModelID, tModel.AngleStartFind, tModel.AngleExtentFind, tModel.MinScoreFind,
                                    tModel.NumMatchesFind, levels, tModel.Greediness, tModel.ModelRow,
                                    tModel.ModelColumn, tModel.ModelAngle, tModel.DrawLineStartRow, tModel.DrawLineStartCol,
                                    tModel.DrawLineEndRow, tModel.DrawLineEndCol, tModel.InMeasureLength1,
                                    tModel.InMeasureLength2, tModel.InMeasureSigma, tModel.InMeasureThreshold,
                                    tModel.InMeasureSelect, tModel.InMeasureTransition, tModel.InMeasureNumber,
                                    tModel.InMeasureScore, out resultModel.ModelFindRow, out resultModel.ModelFindColumn,
                                    out resultModel.ModelFindAngle, out resultModel.ModelFindScore, out resultModel.CenterRow,
                                    out resultModel.CenterColumn, out resultModel.CenterPhi, out resultModel.bFindCenter,
                                    out resultModel.Exception, out resultModel.bImageRotate, out ho_OutObj);
                    if (resultModel.bFindCenter && tModel.Name.Contains("上相机"))
                    {
                        if(!tModel.IsShieldOcr)
                        {
                            string strErr = "";
                            resultModel.strOCR = OcrReadAction(resultModel.ImageRotateText, tModel, ref strErr);
                            resultModel.OcrResult = !string.IsNullOrEmpty(resultModel.strOCR);
                            errMsg += strErr;
                        }
                        else
                        {
                            resultModel.strOCR = "";
                            resultModel.OcrResult = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        resultModel.ErrorMessage = errMsg;
                    }

                    resultModel.ObjectResult = ho_OutObj;
                    resultModel.RunResult = resultModel.bFindCenter;
                    break;
                default:
                    break;
            }

            return resultModel;
        }

        private string OcrReadAction(HObject rotateImage, AlgorithmModel tModel, ref string errMsg)
        {
            try
            {
                HOperatorSet.WriteImage(rotateImage, "jpg", 0, "OCR");
                m_stInputImage.InitImage("OCR.jpg");
                MVD_IMAGE_DATA_INFO stInputImageData = m_stInputImage.GetImageData();
                CMvdImage shallowCopyImage = new CMvdImage();
                shallowCopyImage.InitImage(m_stInputImage.Width, m_stInputImage.Height, m_stInputImage.PixelFormat, stInputImageData);
                string ocr = OCRRead(m_stInputImage, tModel);

                return ocr;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 初始化OCR
        /// </summary>
        private string InitOCR(AlgorithmModel tModel)
        {
            try
            {
                if (m_stCNNOCRToolObj == null)
                {
                    m_stCNNOCRToolObj = new CNNOCRTool(m_enPlatformType);
                    m_stCNNOCRToolObj.BasicParam.LoadModel(tModel.strOCRPath);
                    m_cocrbasicp = m_stCNNOCRToolObj.BasicParam; 
                }
                if (m_stInputImage == null)
                {
                    m_stInputImage = new CMvdImage();
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
          
        /// <summary>
        /// 创建匹配模板
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_CreateRectangle"></param>
        /// <param name="ho_ImageMedian"></param>
        /// <param name="hv_NumLevels"></param>
        /// <param name="hv_AngleStart"></param>
        /// <param name="hv_AngleExtent"></param>
        /// <param name="hv_AngleStep"></param>
        /// <param name="hv_Optimization"></param>
        /// <param name="hv_Metric"></param>
        /// <param name="hv_Contrast"></param>
        /// <param name="hv_MinContrast"></param>
        /// <param name="hv_ModelID"></param>
        /// <param name="hv_ModelRow"></param>
        /// <param name="hv_ModelColumn"></param>
        /// <param name="hv_ModelAngle"></param>
        /// <param name="hv_ModelScore"></param>
        /// <param name="hv_bCreateModel"></param>
        public void CreateShapeModel(HObject ho_Image, HObject ho_CreateRectangle, out HObject ho_ImageMedian,
            HTuple hv_NumLevels, HTuple hv_AngleStart, HTuple hv_AngleExtent, HTuple hv_AngleStep,
            HTuple hv_Optimization, HTuple hv_Metric, HTuple hv_Contrast, HTuple hv_MinContrast,
            out HTuple hv_ModelID, out HTuple hv_ModelRow, out HTuple hv_ModelColumn, out HTuple hv_ModelAngle,
            out HTuple hv_ModelScore, out HTuple hv_bCreateModel, out HObject ho_Contour)
        {
            HObject ho_ImageGauss = null, ho_ImageReduced = null; 

            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_ModelID = new HTuple();
            hv_ModelRow = new HTuple();
            hv_ModelColumn = new HTuple();
            hv_ModelAngle = new HTuple();
            hv_ModelScore = new HTuple();
            hv_bCreateModel = new HTuple();
            try
            {
                try
                {
                    ho_ImageGauss.Dispose();
                    HOperatorSet.GaussFilter(ho_Image, out ho_ImageGauss, 5);
                    ho_ImageMedian.Dispose();
                    HOperatorSet.MedianImage(ho_ImageGauss, out ho_ImageMedian, "circle", 5, "mirrored");
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageMedian, ho_CreateRectangle, out ho_ImageReduced); 
                    HOperatorSet.CreateShapeModel(ho_ImageReduced, hv_NumLevels, hv_AngleStart,
                        hv_AngleExtent, hv_AngleStep, hv_Optimization, hv_Metric, hv_Contrast,
                        hv_MinContrast, out hv_ModelID); 
                    HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent, 0.5, 1, 0.5,
                        "least_squares", 0, 0.9, out hv_ModelRow, out hv_ModelColumn, out hv_ModelAngle,
                        out hv_ModelScore);
                    
                    
                    AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_ModelRow, hv_ModelColumn,
                        hv_ModelAngle, 1, 1, 0, out ho_Contour); 
                     
                    hv_bCreateModel = 1;
                } 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception); 
                    hv_bCreateModel = 0;
                }
                ho_ImageGauss.Dispose();
                ho_ImageReduced.Dispose();
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageGauss.Dispose();
                ho_ImageReduced.Dispose();
                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 寻找产品中心点像素坐标，并且输出字符区域图像
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_FindModelRectangle"></param>
        /// <param name="ho_OCRRectangle"></param>
        /// <param name="ho_ImageRotateText"></param>
        /// <param name="ho_ProXLDTrans"></param>
        /// <param name="ho_CenterCross"></param>
        /// <param name="hv_ModelID"></param>
        /// <param name="hv_AngleStartFind"></param>
        /// <param name="hv_AngleExtentFind"></param>
        /// <param name="hv_MinScoreFind"></param>
        /// <param name="hv_NumMatchesFind"></param>
        /// <param name="hv_NumLevelsFind"></param>
        /// <param name="hv_Greediness"></param>
        /// <param name="hv_ModelRow"></param>
        /// <param name="hv_ModelColumn"></param>
        /// <param name="hv_ModelAngle"></param>
        /// <param name="hv_DrawLineStartRow"></param>
        /// <param name="hv_DrawLineStartCol"></param>
        /// <param name="hv_DrawLineEndRow"></param>
        /// <param name="hv_DrawLineEndCol"></param>
        /// <param name="hv_InMeasureLength1"></param>
        /// <param name="hv_InMeasureLength2"></param>
        /// <param name="hv_InMeasureSigma"></param>
        /// <param name="hv_InMeasureThreshold"></param>
        /// <param name="hv_InMeasureSelect"></param>
        /// <param name="hv_InMeasureTransition"></param>
        /// <param name="hv_InMeasureNumber"></param>
        /// <param name="hv_InMeasureScore"></param>
        /// <param name="hv_ModelFindRow"></param>
        /// <param name="hv_ModelFindColumn"></param>
        /// <param name="hv_ModelFindAngle"></param>
        /// <param name="hv_ModelFindScore"></param>
        /// <param name="hv_CenterRow"></param>
        /// <param name="hv_CenterColumn"></param>
        /// <param name="hv_CenterPhi"></param>
        /// <param name="hv_bFindCenter"></param>
        /// <param name="hv_Exception"></param>
        public void FindCenterPos(HObject ho_Image, HObject FindModelRectangle, HObject OCRRectangle,
                                    string str_HalconParamPath, emRunModel runModel,
                                    out HObject ho_ImageRotateText, out HObject ho_ProXLDTrans, out HObject ho_CenterCross,
                                    HTuple hv_ModelID, HTuple hv_AngleStartFind, HTuple hv_AngleExtentFind, HTuple hv_MinScoreFind,
                                    HTuple hv_NumMatchesFind, HTuple hv_NumLevelsFind, HTuple hv_Greediness, HTuple hv_ModelRow,
                                    HTuple hv_ModelColumn, HTuple hv_ModelAngle, HTuple hv_DrawLineStartRow, HTuple hv_DrawLineStartCol,
                                    HTuple hv_DrawLineEndRow, HTuple hv_DrawLineEndCol, HTuple hv_InMeasureLength1,
                                    HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                                    HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                                    HTuple hv_InMeasureScore, out HTuple hv_ModelFindRow, out HTuple hv_ModelFindColumn,
                                    out HTuple hv_ModelFindAngle, out HTuple hv_ModelFindScore, out HTuple hv_CenterRow,
                                    out HTuple hv_CenterColumn, out HTuple hv_CenterPhi, out HTuple hv_bFindCenter,
                                    out HTuple hv_Exception, out bool bImageRotate, out HObject ho_OutObj)
        {
            HObject ho_FindModelRectangle = null;
            HObject ho_OCRRectangle = null;
            //读取保存本地Region
            if (runModel == emRunModel.AutoRun)
            {
                //自动
                hv_ModelID = new HTuple();
                hv_DrawLineStartRow = new HTuple();
                hv_DrawLineStartCol = new HTuple();
                hv_DrawLineEndRow = new HTuple();
                hv_DrawLineEndCol = new HTuple();
                HOperatorSet.ReadShapeModel(str_HalconParamPath + "\\Model.shm", out hv_ModelID);
                HOperatorSet.ReadRegion(out ho_FindModelRectangle, str_HalconParamPath + "\\SearchRegion.hobj");
                HOperatorSet.ReadRegion(out ho_OCRRectangle, str_HalconParamPath + "\\OCRRegion.hobj");
                HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineStartRow", out hv_DrawLineStartRow);
                HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineStartCol", out hv_DrawLineStartCol);
                HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineEndRow", out hv_DrawLineEndRow);
                HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineEndCol", out hv_DrawLineEndCol);
            }
            else
            {
                //手动测试
                HOperatorSet.CopyObj(FindModelRectangle, out ho_FindModelRectangle, 1, 1);
                HOperatorSet.CopyObj(OCRRectangle, out ho_OCRRectangle, 1, 1);
            }
            bImageRotate = false;

           // Local iconic variables 

           HObject ho_ImageReduced1 = null, ho_RegionAffineTrans = null;
            HObject ho_ImagePart = null, ho_ImageRotate = null, ho_ImageFull = null;
            HObject ho_ImageRotateText1 = null;
            HObject ho_MeasureLineContours = null, ho_MeasureCross = null;
            HObject ho_MeasuredLines = null;
            HOperatorSet.GenEmptyObj(out ho_OutObj);

            // Local control variables 

            HTuple hv_HomMat2D = new HTuple(), hv_TDrawLineStartRow = new HTuple();
            HTuple hv_TDrawLineStartCol = new HTuple(), hv_TDrawLineEndRow = new HTuple();
            HTuple hv_TDrawLineEndCol = new HTuple(), hv_Row15 = new HTuple();
            HTuple hv_Column15 = new HTuple(), hv_Row25 = new HTuple();
            HTuple hv_Column25 = new HTuple(), hv_OrientationAngle = new HTuple();
            HTuple hv_GetLineStartRow = new HTuple(), hv_GetLineStartCol = new HTuple();
            HTuple hv_GetLineEndRow = new HTuple(), hv_GetLineEndCol = new HTuple();
            HTuple hv_Index2 = new HTuple(), hv_InLineStartRow = new HTuple();
            HTuple hv_InLineStartCol = new HTuple(), hv_InLineEndRow = new HTuple();
            HTuple hv_InLineEndCol = new HTuple(), hv_bDisp = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_AllRow = new HTuple(), hv_AllColumn = new HTuple();
            HTuple hv_bFindLine2D = new HTuple(), hv_InterRow = new HTuple();
            HTuple hv_InterColumn = new HTuple(), hv_bRun = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageRotateText);
            HOperatorSet.GenEmptyObj(out ho_ProXLDTrans);
            HOperatorSet.GenEmptyObj(out ho_CenterCross);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageRotate);
            HOperatorSet.GenEmptyObj(out ho_ImageFull);
            HOperatorSet.GenEmptyObj(out ho_ImageRotateText1);
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
            hv_ModelFindRow = new HTuple();
            hv_ModelFindColumn = new HTuple();
            hv_ModelFindAngle = new HTuple();
            hv_ModelFindScore = new HTuple();
            hv_CenterRow = new HTuple();
            hv_CenterColumn = new HTuple();
            hv_CenterPhi = new HTuple();
            hv_bFindCenter = new HTuple();
            hv_Exception = new HTuple();
            try
            {
                try
                {
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_FindModelRectangle, out ho_ImageReduced1); 

                    HOperatorSet.TupleRad(hv_AngleStartFind, out hv_AngleStartFind);
                    HOperatorSet.TupleRad(hv_AngleExtentFind, out hv_AngleExtentFind);

                    HOperatorSet.FindShapeModel(ho_ImageReduced1, hv_ModelID, hv_AngleStartFind,
                        hv_AngleExtentFind, hv_MinScoreFind, hv_NumMatchesFind, 0.5, "least_squares",
                        hv_NumLevelsFind, hv_Greediness, out hv_ModelFindRow, out hv_ModelFindColumn,
                        out hv_ModelFindAngle, out hv_ModelFindScore);
                    HObject ho_Contour;
                    AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_ModelFindRow, hv_ModelFindColumn,
                        hv_ModelFindAngle, 1, 1, 0, out ho_Contour);
                    HOperatorSet.ConcatObj(ho_OutObj, ho_Contour, out ho_OutObj);

                    HOperatorSet.ClearShapeModel(hv_ModelID);
                    HOperatorSet.VectorAngleToRigid(hv_ModelRow, hv_ModelColumn, hv_ModelAngle,
                        hv_ModelFindRow, hv_ModelFindColumn, hv_ModelFindAngle, out hv_HomMat2D);
                    //跟随示教测量直线点
                    HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_DrawLineStartRow, hv_DrawLineStartCol,
                        out hv_TDrawLineStartRow, out hv_TDrawLineStartCol);
                    HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_DrawLineEndRow, hv_DrawLineEndCol,
                        out hv_TDrawLineEndRow, out hv_TDrawLineEndCol);
                    //跟随OCR区域
                    ho_RegionAffineTrans.Dispose();
                    HOperatorSet.AffineTransRegion(ho_OCRRectangle, out ho_RegionAffineTrans, hv_HomMat2D, "nearest_neighbor");
                    HOperatorSet.SmallestRectangle1(ho_RegionAffineTrans, out hv_Row15, out hv_Column15, out hv_Row25, out hv_Column25);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ImagePart.Dispose();
                        HOperatorSet.CropPart(ho_Image, out ho_ImagePart, hv_Row15, hv_Column15, hv_Column25 - hv_Column15, hv_Row25 - hv_Row15);
                    }

                    HTuple hv_angle;//转换成弧度
                    HOperatorSet.TupleDeg(hv_ModelFindAngle, out hv_angle);

                    ho_ImageRotate.Dispose();
                    if (hv_angle < 210 && hv_angle > 150)
                    {
                        HOperatorSet.RotateImage(ho_ImagePart, out ho_ImageRotate, 270, "constant");
                        bImageRotate = true;
                    }
                    else
                    { 
                        HOperatorSet.RotateImage(ho_ImagePart, out ho_ImageRotate, 90, "constant");
                        bImageRotate = false;
                    }

                    ho_ImageFull.Dispose();
                    HOperatorSet.FullDomain(ho_ImageRotate, out ho_ImageFull);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.TextLineOrientation(ho_ImageFull, ho_ImageRotate, 90, (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), out hv_OrientationAngle);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ImageRotateText1.Dispose();
                        HOperatorSet.RotateImage(ho_ImageRotate, out ho_ImageRotateText1, ((-hv_OrientationAngle) / ((new HTuple(180)).TupleRad())) * 180, "constant");
                    }
                    ho_ImageRotateText.Dispose();
                    AlgorithmCommHelper.scale_image_range(ho_ImageRotateText1, out ho_ImageRotateText, 0, 80);

                    //跟随粗模板，测量寻找边线点
                    hv_GetLineStartRow = new HTuple();
                    hv_GetLineStartCol = new HTuple();
                    hv_GetLineEndRow = new HTuple();
                    hv_GetLineEndCol = new HTuple();
                    for (hv_Index2 = 0; (int)hv_Index2 <= 3; hv_Index2 = (int)hv_Index2 + 1)
                    {
                        hv_InLineStartRow = hv_TDrawLineStartRow.TupleSelect(hv_Index2);
                        hv_InLineStartCol = hv_TDrawLineStartCol.TupleSelect(hv_Index2);
                        hv_InLineEndRow = hv_TDrawLineEndRow.TupleSelect(hv_Index2);
                        hv_InLineEndCol = hv_TDrawLineEndCol.TupleSelect(hv_Index2);

                        hv_bDisp = 0;
                        //ho_MeasureLineContours.Dispose(); ho_MeasureCross.Dispose(); ho_MeasuredLines.Dispose();
                        AlgorithmCommHelper.FindLine2D(ho_ImageReduced1, out ho_MeasureLineContours, out ho_MeasureCross,
                            out ho_MeasuredLines, hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow,
                            hv_InLineEndCol, hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma,
                            hv_InMeasureThreshold, hv_InMeasureSelect, hv_InMeasureTransition,
                            hv_InMeasureNumber, hv_InMeasureScore, hv_bDisp, out hv_RowBegin, out hv_ColBegin,
                            out hv_RowEnd, out hv_ColEnd, out hv_AllRow, out hv_AllColumn, out hv_bFindLine2D);

                        if(hv_bFindLine2D.I == 0)
                        { 
                            HOperatorSet.ConcatObj(ho_OutObj, ho_MeasureLineContours, out ho_OutObj);
                            HOperatorSet.ConcatObj(ho_OutObj, ho_MeasureCross, out ho_OutObj);
                        }
                        HOperatorSet.ConcatObj(ho_OutObj, ho_MeasuredLines, out ho_OutObj);

                        HTuple ExpTmpLocalVar_GetLineStartRow = hv_GetLineStartRow.TupleConcat(hv_RowBegin);
                        hv_GetLineStartRow = ExpTmpLocalVar_GetLineStartRow;
                        HTuple ExpTmpLocalVar_GetLineStartCol = hv_GetLineStartCol.TupleConcat(hv_ColBegin);
                        hv_GetLineStartCol = ExpTmpLocalVar_GetLineStartCol;
                        HTuple ExpTmpLocalVar_GetLineEndRow = hv_GetLineEndRow.TupleConcat(hv_RowEnd);
                        hv_GetLineEndRow = ExpTmpLocalVar_GetLineEndRow;
                        HTuple ExpTmpLocalVar_GetLineEndCol = hv_GetLineEndCol.TupleConcat(hv_ColEnd);
                        hv_GetLineEndCol = ExpTmpLocalVar_GetLineEndCol;
                    }

                    //通过直线相交拟合矩形
                    hv_InterRow = new HTuple();
                    hv_InterColumn = new HTuple();
                    hv_bRun = 0;
                    ho_ProXLDTrans.Dispose(); ho_CenterCross.Dispose();
                    AlgorithmCommHelper.FindRecPointCenter(out ho_ProXLDTrans, out ho_CenterCross, hv_GetLineStartRow,
                    hv_GetLineStartCol, hv_GetLineEndRow, hv_GetLineEndCol, out hv_InterRow,
                    out hv_InterColumn, out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_bRun); 
                    if ((int)(hv_bRun) != 0)
                    {
                        hv_bFindCenter = 1;
                    }
                    else
                    {
                        hv_bFindCenter = 0;
                    }

                }
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_bFindCenter = 0;
                }
                ho_ImageReduced1.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageRotate.Dispose();
                ho_ImageFull.Dispose();
                ho_ImageRotateText1.Dispose();
                //ho_MeasureLineContours.Dispose();
                //ho_MeasureCross.Dispose();
                //ho_MeasuredLines.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced1.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageRotate.Dispose();
                ho_ImageFull.Dispose();
                ho_ImageRotateText1.Dispose();
                ho_MeasureLineContours.Dispose();
                ho_MeasureCross.Dispose();
                ho_MeasuredLines.Dispose();

                throw HDevExpDefaultException;
            }
        }
 
        /// <summary>
        /// 示教治具位置时所用算法
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="FindModelRectangle"></param>
        /// <param name="OCRRectangle"></param>
        /// <param name="str_HalconParamPath"></param>
        /// <param name="ho_ProXLDTrans"></param>
        /// <param name="ho_CenterCross"></param>
        /// <param name="hv_DrawLineStartRow"></param>
        /// <param name="hv_DrawLineStartCol"></param>
        /// <param name="hv_DrawLineEndRow"></param>
        /// <param name="hv_DrawLineEndCol"></param>
        /// <param name="hv_InMeasureLength1"></param>
        /// <param name="hv_InMeasureLength2"></param>
        /// <param name="hv_InMeasureSigma"></param>
        /// <param name="hv_InMeasureThreshold"></param>
        /// <param name="hv_InMeasureSelect"></param>
        /// <param name="hv_InMeasureTransition"></param>
        /// <param name="hv_InMeasureNumber"></param>
        /// <param name="hv_InMeasureScore"></param>
        /// <param name="hv_CenterRow"></param>
        /// <param name="hv_CenterColumn"></param>
        /// <param name="hv_CenterPhi"></param>
        /// <param name="hv_bFindCenter"></param>
        /// <param name="hv_Exception"></param>
        /// <param name="ho_OutObj"></param>
        public void FindFixturePos(HObject ho_Image, string str_HalconParamPath, string fixtureId, out HObject ho_ProXLDTrans, out HObject ho_CenterCross, 
                                    HTuple hv_InMeasureLength1, HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                                    HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                                    HTuple hv_InMeasureScore,out HTuple hv_CenterRow,
                                    out HTuple hv_CenterColumn, out HTuple hv_CenterPhi, out HTuple hv_bFindCenter,
                                    out HTuple hv_Exception, out HObject ho_OutObj)
        {
            //自动 
            HTuple hv_DrawLineStartRow = new HTuple();
            HTuple hv_DrawLineStartCol = new HTuple();
            HTuple hv_DrawLineEndRow = new HTuple();
            HTuple hv_DrawLineEndCol = new HTuple();
            HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineStartRow" + fixtureId, out hv_DrawLineStartRow);
            HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineStartCol" + fixtureId, out hv_DrawLineStartCol);
            HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineEndRow" + fixtureId, out hv_DrawLineEndRow);
            HOperatorSet.ReadTuple(str_HalconParamPath + "\\DrawLineEndCol" + fixtureId, out hv_DrawLineEndCol); 

            // Local iconic variables 

            HObject ho_ImageReduced1 = null, ho_RegionAffineTrans = null;
            HObject ho_ImagePart = null, ho_ImageRotate = null, ho_ImageFull = null;
            HObject ho_ImageRotateText1 = null;
            HObject ho_MeasureLineContours = null, ho_MeasureCross = null;
            HObject ho_MeasuredLines = null;
            HOperatorSet.GenEmptyObj(out ho_OutObj);

            // Local control variables 

            HTuple hv_HomMat2D = new HTuple(), hv_TDrawLineStartRow = new HTuple();
            HTuple hv_TDrawLineStartCol = new HTuple(), hv_TDrawLineEndRow = new HTuple();
            HTuple hv_TDrawLineEndCol = new HTuple(), hv_Row15 = new HTuple();
            HTuple hv_Column15 = new HTuple(), hv_Row25 = new HTuple();
            HTuple hv_Column25 = new HTuple(), hv_OrientationAngle = new HTuple();
            HTuple hv_GetLineStartRow = new HTuple(), hv_GetLineStartCol = new HTuple();
            HTuple hv_GetLineEndRow = new HTuple(), hv_GetLineEndCol = new HTuple();
            HTuple hv_Index2 = new HTuple(), hv_InLineStartRow = new HTuple();
            HTuple hv_InLineStartCol = new HTuple(), hv_InLineEndRow = new HTuple();
            HTuple hv_InLineEndCol = new HTuple(), hv_bDisp = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_AllRow = new HTuple(), hv_AllColumn = new HTuple();
            HTuple hv_bFindLine2D = new HTuple(), hv_InterRow = new HTuple();
            HTuple hv_InterColumn = new HTuple(), hv_bRun = new HTuple();
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_ProXLDTrans);
            HOperatorSet.GenEmptyObj(out ho_CenterCross);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageRotate);
            HOperatorSet.GenEmptyObj(out ho_ImageFull);
            HOperatorSet.GenEmptyObj(out ho_ImageRotateText1);
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines); 
            hv_CenterRow = new HTuple();
            hv_CenterColumn = new HTuple();
            hv_CenterPhi = new HTuple();
            hv_bFindCenter = new HTuple();
            hv_Exception = new HTuple();
            try
            {
                try
                {
                    //跟随粗模板，测量寻找边线点
                    hv_GetLineStartRow = new HTuple();
                    hv_GetLineStartCol = new HTuple();
                    hv_GetLineEndRow = new HTuple();
                    hv_GetLineEndCol = new HTuple();
                    for (hv_Index2 = 0; (int)hv_Index2 <= 3; hv_Index2 = (int)hv_Index2 + 1)
                    {
                        hv_InLineStartRow = hv_DrawLineStartRow.TupleSelect(hv_Index2);
                        hv_InLineStartCol = hv_DrawLineStartCol.TupleSelect(hv_Index2);
                        hv_InLineEndRow = hv_DrawLineEndRow.TupleSelect(hv_Index2);
                        hv_InLineEndCol = hv_DrawLineEndCol.TupleSelect(hv_Index2);

                        hv_bDisp = 0;
                        AlgorithmCommHelper.FindLine2D(ho_Image, out ho_MeasureLineContours, out ho_MeasureCross,
                            out ho_MeasuredLines, hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow,
                            hv_InLineEndCol, hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma,
                            hv_InMeasureThreshold, hv_InMeasureSelect, hv_InMeasureTransition,
                            hv_InMeasureNumber, hv_InMeasureScore, hv_bDisp, out hv_RowBegin, out hv_ColBegin,
                            out hv_RowEnd, out hv_ColEnd, out hv_AllRow, out hv_AllColumn, out hv_bFindLine2D);

                        if (hv_bFindLine2D.I == 0)
                        {
                            HOperatorSet.ConcatObj(ho_OutObj, ho_MeasureLineContours, out ho_OutObj);
                            HOperatorSet.ConcatObj(ho_OutObj, ho_MeasureCross, out ho_OutObj);
                        }
                        HOperatorSet.ConcatObj(ho_OutObj, ho_MeasuredLines, out ho_OutObj);

                        HTuple ExpTmpLocalVar_GetLineStartRow = hv_GetLineStartRow.TupleConcat(hv_RowBegin);
                        hv_GetLineStartRow = ExpTmpLocalVar_GetLineStartRow;
                        HTuple ExpTmpLocalVar_GetLineStartCol = hv_GetLineStartCol.TupleConcat(hv_ColBegin);
                        hv_GetLineStartCol = ExpTmpLocalVar_GetLineStartCol;
                        HTuple ExpTmpLocalVar_GetLineEndRow = hv_GetLineEndRow.TupleConcat(hv_RowEnd);
                        hv_GetLineEndRow = ExpTmpLocalVar_GetLineEndRow;
                        HTuple ExpTmpLocalVar_GetLineEndCol = hv_GetLineEndCol.TupleConcat(hv_ColEnd);
                        hv_GetLineEndCol = ExpTmpLocalVar_GetLineEndCol;
                    }

                    //通过直线相交拟合矩形
                    hv_InterRow = new HTuple();
                    hv_InterColumn = new HTuple();
                    hv_bRun = 0;
                    ho_ProXLDTrans.Dispose(); ho_CenterCross.Dispose();
                    AlgorithmCommHelper.FindRecPointCenter_2(out ho_ProXLDTrans, out ho_CenterCross, hv_GetLineStartRow,
                    hv_GetLineStartCol, hv_GetLineEndRow, hv_GetLineEndCol, out hv_InterRow,
                    out hv_InterColumn, out hv_CenterRow, out hv_CenterColumn, out hv_CenterPhi, out hv_bRun);
                    if ((int)(hv_bRun) != 0)
                    {
                        hv_bFindCenter = 1;
                    }
                    else
                    {
                        hv_bFindCenter = 0;
                    }

                }
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_bFindCenter = 0;
                }
                ho_ImageReduced1.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageRotate.Dispose();
                ho_ImageFull.Dispose();
                ho_ImageRotateText1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced1.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageRotate.Dispose();
                ho_ImageFull.Dispose();
                ho_ImageRotateText1.Dispose();
                ho_MeasureLineContours.Dispose();
                ho_MeasureCross.Dispose();
                ho_MeasuredLines.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //OCR 深度学习
        public string OCRRead(CMvdImage HKimage, AlgorithmModel tModel)
        {
            if ((null == m_stCNNOCRToolObj) || null == HKimage)
            {
                string str = "OCR参数未初始化！";
                return null;
            }

            //double fOCRStartTime = GetTimeStamp();
            m_stCNNOCRToolObj.InputImage = HKimage;
            //m_stCNNOCRToolObj.BasicParam.LoadModel(tModel.strOCRPath);
            //CNNOCRBasicParam cocrbasicp = m_stCNNOCRToolObj.BasicParam;
            m_stCNNOCRToolObj.Run();
            //double fOCRCostTime = GetTimeStamp() - fOCRStartTime;
            if (m_stCNNOCRToolObj.Result.RecogInfoList.Count > 0)
            {
                return m_stCNNOCRToolObj.Result.RecogInfoList[0].RecogString;
            }
            else
            {
                return null;
            }

        }

        private double GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        } 
    }

    public enum emRunModel
    {
        ManualRun = 0,
        AutoRun = 1
    }
}
