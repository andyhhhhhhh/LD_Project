using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using SequenceTestModel;
using HalconDotNet;
using System.IO;
using GlobalCore;

namespace VisionController
{
    public class LDAlgorithmControl : IAlgorithmControl
    {
        public bool Init(object parameter)
        {
            throw new NotImplementedException();
        }

        public BaseResultModel Run(BaseAlgorithmModel controlModel, AlgorithmControlType controlType)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                if (controlModel is BigFeedAlgorithmModel)
                {
                    BigFeedAlgorithmModel tModel = controlModel as BigFeedAlgorithmModel;
                    HObject ho_Rect = new HObject();
                    HOperatorSet.GenRectangle1(out ho_Rect, tModel.Row1, tModel.Column1, tModel.Row2, tModel.Column2);

                    HObject ho_DispObjects, ho_Rectangle1;
                    HTuple hv_BigCenterRow, hv_ProdAngleMean, hv_BigCenterCol, hv_Error;
                    GetProdRegionAngle(tModel.Image, ho_Rect, out ho_DispObjects, out ho_Rectangle1, tModel.GrayOpenHeight, tModel.GrayOpenWidth,
                        tModel.DynThr, tModel.HysThrMin, tModel.HysThrMax, tModel.SelectAreaMin, tModel.SingleSelectAreaMin, tModel.SingleSelectAreaMax,
                        tModel.ClosingW, tModel.ClosingH, out hv_ProdAngleMean, out hv_BigCenterRow, out hv_BigCenterCol, out hv_Error);

                    BigFeedResultModel bigFeedResult = new BigFeedResultModel();
                    bigFeedResult.RunResult = hv_Error == 0;
                    bigFeedResult.DispObjects = ho_DispObjects;
                    bigFeedResult.Rectangle1 = ho_Rectangle1;
                    bigFeedResult.ProdAngleMean = hv_ProdAngleMean;
                    bigFeedResult.BigCenterRow = hv_BigCenterRow;
                    bigFeedResult.BigCenterCol = hv_BigCenterCol;

                    return bigFeedResult;
                }
                else if (controlModel is BigFixedAlgorithmModel)
                {
                    BigFixedAlgorithmModel tModel = controlModel as BigFixedAlgorithmModel;
                    HObject ho_Rect = new HObject();
                    HOperatorSet.GenRectangle1(out ho_Rect, tModel.Row1, tModel.Column1, tModel.Row2, tModel.Column2);

                    HObject ho_OutRegion;
                    HTuple hv_AnyRow, hv_AnyCol, hv_AnyAng, hv_Count;
                    LDFindAnyCenter(tModel.Image, ho_Rect, out ho_OutRegion, out hv_AnyRow, out hv_AnyCol, out hv_AnyAng, out hv_Count);

                    BigFixedResultModel bigFixedResult = new BigFixedResultModel();
                    bigFixedResult.RunResult = true;
                    bigFixedResult.OutRegion = ho_OutRegion;
                    bigFixedResult.AnyRow = hv_AnyRow;
                    bigFixedResult.AnyCol = hv_AnyCol;
                    bigFixedResult.AnyAng = hv_AnyAng;
                    bigFixedResult.Count = hv_Count;

                    return bigFixedResult;
                }
                else if (controlModel is SmallJudgePosModel)
                {
                    SmallJudgePosModel tModel = controlModel as SmallJudgePosModel;
                    HObject ho_OutRegion;
                    HTuple hv_IsCenterPos, hv_IsExistProduct, hv_SubCol;
                    LDSmallJudgePos(tModel.Image, out ho_OutRegion, tModel.MinSubCol, tModel.MinGray, out hv_IsCenterPos, out hv_IsExistProduct, out hv_SubCol);

                    SmallJudgePosResultModel smallJudgeResult = new SmallJudgePosResultModel();
                    smallJudgeResult.IsCenterPos = hv_IsCenterPos == 1;
                    smallJudgeResult.IsExistProduct = hv_IsExistProduct == 1;
                    smallJudgeResult.OutRegion = ho_OutRegion;
                    smallJudgeResult.SubCol = hv_SubCol;
                    smallJudgeResult.RunResult = true;

                    return smallJudgeResult;
                }
                else if (controlModel is SmallFixedPosModel)
                {
                    SmallFixedPosModel tModel = controlModel as SmallFixedPosModel;
                    HObject ho_OcrRegion, ho_OcrOutRegion, ho_OutRegion, AllProdAndKongSortObj;
                    HTuple AllProdAndKongSortName;
                    string strLog;
                    bool bResult;
                    double OcrCenterRow = 0;
                    double OcrCenterCol = 0;
                    double OcrCenterPhi = 0;
                    int ICanGet = 0;
                    int IExistProduct = 0;
                    string bar = "";
                    double distance = 0;
                    string firstOcr = "";

                    string modelIDPath = Global.Model3DPath + "//" + "SmallFixed//";

                    HOperatorSet.GenRectangle1(out ho_OcrRegion, tModel.OcrRow1, tModel.OcrColumn1, tModel.OcrRow2, tModel.OcrColumn2);

                    LDSmallFixedAlgorithm(tModel.Image, ho_OcrRegion, modelIDPath, tModel.PyramidLevel, tModel.LastPyramidLevel, tModel.MinScore, tModel.Greediness,
                        tModel.ThrsholdMin, tModel.HysThresholdMin, tModel.HysThesholdMax, tModel.CloseRec, tModel.ModelRegionAreaDownValue, tModel.IsNotProdValue, tModel.IsNotProdValue * -1,
                        tModel.AutoThreshold, tModel.DistancePP, tModel.NeedOcr, tModel.IsIngoreCalu, tModel.OcrLength, tModel.JudgeBarLength, tModel.OcrBinPath, out AllProdAndKongSortObj, out ho_OutRegion, out ho_OcrOutRegion, out AllProdAndKongSortName,
                        out OcrCenterRow, out OcrCenterCol, out OcrCenterPhi, out distance, out ICanGet, out IExistProduct, out firstOcr, out bar, out strLog, out bResult);

                    SmallFixedPosResultModel smallFixedResult = new SmallFixedPosResultModel();

                    smallFixedResult.RunResult = bResult;
                    smallFixedResult.OutRegion = ho_OutRegion;
                    smallFixedResult.OcrOutRegion = ho_OcrOutRegion;
                    smallFixedResult.OcrCenterRow = OcrCenterRow;
                    smallFixedResult.OcrCenterCol = OcrCenterCol;
                    smallFixedResult.OcrCenterPhi = OcrCenterPhi;
                    smallFixedResult.ICanGet = ICanGet;
                    smallFixedResult.IExistProduct = IExistProduct;
                    smallFixedResult.strLog = strLog;
                    smallFixedResult.Bar = bar;
                    smallFixedResult.Distance = distance;
                    smallFixedResult.FirstOcr = firstOcr;
                    smallFixedResult.NeedOcr = tModel.NeedOcr;

                    return smallFixedResult;
                }
                else if (controlModel is FixtureAlgorithmModel)
                {
                    FixtureAlgorithmModel tModel = controlModel as FixtureAlgorithmModel;
                    var result = NTestAlgorithm(tModel.Image);

                    return result;
                }
                else if (controlModel is AlgorithmModelP) //P面检测算法
                {
                    AlgorithmModelP tModel = controlModel as AlgorithmModelP;

                    HObject ho_OutObj, ho_ModelProdObject = null, ho_CheckRegion = null, ho_BengQueRegions = null, ho_ZangWuRegions = null, ho_SeChaRegions = null, ho_ProdObject = null;
                    HTuple hv_IsProd = null, hv_Exception = null;
                    HTuple hv_ModelProdRegionPosRCA = null, hv_BengQueNumber = null, hv_ZangWuNumber = null, hv_SeChaNumber = null;
                    HOperatorSet.GenEmptyObj(out ho_OutObj);

                    string strpath = Global.Model3DPath + "//Model";
                    HOperatorSet.ReadRegion(out ho_ModelProdObject, strpath + "//P_ProdRegion.hobj");
                    HOperatorSet.ReadTuple(strpath + "//P_ProdRegionPosRCA.tup", out hv_ModelProdRegionPosRCA);

                    HOperatorSet.GenRectangle1(out ho_CheckRegion, tModel.SRow1, tModel.SColumn1, tModel.SRow2, tModel.SColumn2);

                    PCheck(tModel.Image, ho_CheckRegion, ho_ModelProdObject, out ho_BengQueRegions,
                        out ho_ZangWuRegions, out ho_SeChaRegions, out ho_ProdObject, tModel.DynThr,
                        tModel.HysThrMin, tModel.HysThrMax, tModel.DarkThrMin, tModel.LightThrMax, tModel.BenQueAreaMin,
                        tModel.BQwidth, tModel.BQheight, tModel.ZangWuAreaMin, hv_ModelProdRegionPosRCA,
                        out hv_IsProd, out hv_Exception);

                    HOperatorSet.ConcatObj(ho_BengQueRegions, ho_ZangWuRegions, out ho_OutObj);
                    HOperatorSet.ConcatObj(ho_OutObj, ho_SeChaRegions, out ho_OutObj);
                    HOperatorSet.ConcatObj(ho_OutObj, ho_ProdObject, out ho_OutObj);

                    HOperatorSet.CountObj(ho_BengQueRegions, out hv_BengQueNumber);
                    HOperatorSet.CountObj(ho_ZangWuRegions, out hv_ZangWuNumber);
                    HOperatorSet.CountObj(ho_SeChaRegions, out hv_SeChaNumber);

                    PResultModel pResult = new PResultModel();
                    pResult.DispObjects = ho_OutObj; 
                    pResult.RunResult = false;
                    if (hv_IsProd != null && hv_IsProd == 0)
                    {
                        //没有产品
                        pResult.ErrorMessage = "没有产品";
                    }
                    else if (hv_BengQueNumber > 0)
                    {
                        //崩缺产品
                        pResult.ErrorMessage = "崩缺产品";
                    }
                    else if (hv_ZangWuNumber > 0)
                    {
                        //脏污产品
                        pResult.ErrorMessage = "脏污产品";
                    }
                    else if (hv_SeChaNumber > 0)
                    {
                        //色差产品
                        pResult.ErrorMessage = "色差产品";
                    }
                    else
                    {
                        pResult.RunResult = true;
                    }

                    return pResult;
                }
                else if (controlModel is AlgorithmModelN) //N面检测算法
                {
                    AlgorithmModelN tModel = controlModel as AlgorithmModelN;
                    HObject ho_OutObj = null, ho_NModelRegion = null, ho_SubBenQueObject = null, ho_CeXiObject = null, ho_LieWenObject = null, ho_BigDarkObject = null;
                    HTuple hv_NModelPos = null, hv_Error = null, hv_IsProd = null;
                    HTuple hv_CeXiNumber = null, hv_BenQueNumber = null, hv_LieWenNumber = null, hv_DarkNumber = null;
                    HOperatorSet.GenEmptyObj(out ho_OutObj);

                    string strpath = Global.Model3DPath + "//Model";
                    HOperatorSet.ReadRegion(out ho_NModelRegion, strpath + "//N_ProModelRegion.hobj");
                    HOperatorSet.ReadTuple(strpath + "//N_ProModelPosRCA.tup", out hv_NModelPos);
                     
                    //崩缺断料检测参数 
                    hv_Error = 1;

                    NFaceCheck(tModel.Image, ho_NModelRegion, out ho_SubBenQueObject, out ho_CeXiObject,
                        out ho_LieWenObject, out ho_BigDarkObject, hv_NModelPos, tModel.MaskMean,
                        tModel.DynThreshold, tModel.CloseWidth, tModel.CloseHeight, tModel.BQAreaMin, tModel.BQWidthHeight,
                        tModel.DiffValue, tModel.LieWenNum, tModel.stdWH, tModel.HysthrMin, tModel.HysthrMax, tModel.DustAreaMin,
                        tModel.DustWidth, tModel.DustHeight, out hv_IsProd, out hv_Error);

                    HOperatorSet.ConcatObj(ho_SubBenQueObject, ho_CeXiObject, out ho_OutObj);
                    HOperatorSet.ConcatObj(ho_OutObj, ho_LieWenObject, out ho_OutObj);
                    HOperatorSet.ConcatObj(ho_OutObj, ho_BigDarkObject, out ho_OutObj); 

                    HOperatorSet.CountObj(ho_CeXiObject, out hv_CeXiNumber);
                    HOperatorSet.CountObj(ho_SubBenQueObject, out hv_BenQueNumber);
                    HOperatorSet.CountObj(ho_LieWenObject, out hv_LieWenNumber);
                    HOperatorSet.CountObj(ho_BigDarkObject, out hv_DarkNumber);

                    NResultModel nResult = new NResultModel();
                    nResult.RunResult = false;
                    if ((int)(new HTuple(hv_CeXiNumber.TupleGreater(0))) != 0)
                    {
                        nResult.ErrorMessage = "侧吸产品"; 
                    }
                    else if ((int)(new HTuple(hv_BenQueNumber.TupleGreater(0))) != 0)
                    {
                        nResult.ErrorMessage = "崩缺产品"; 
                    }
                    else if ((int)(new HTuple(hv_IsProd.TupleEqual(0))) != 0)
                    {
                        nResult.ErrorMessage = "无产品"; 
                    }
                    else if ((int)(new HTuple(hv_LieWenNumber.TupleGreater(0))) != 0)
                    {
                        nResult.ErrorMessage = "裂纹产品"; 
                    }
                    else if ((int)(new HTuple(hv_DarkNumber.TupleGreater(0))) != 0)
                    {
                        nResult.ErrorMessage = "黑块产品"; 
                    }
                    else
                    {
                        nResult.RunResult = true;
                    }

                    //获取N面中心还有角度
                    var result = NTestAlgorithm(tModel.Image);
                    if(!result.RunResult)
                    {
                        nResult.RunResult = false;
                    }
                    else
                    {
                        nResult.CenterRow = result.CenterRow;
                        nResult.CenterColumn = result.CenterColumn;
                        nResult.CenterPhi = result.CenterPhi;
                        HOperatorSet.ConcatObj(ho_OutObj, result.ObjectResult as HObject, out ho_OutObj);
                    }
                    nResult.DispObjects = ho_OutObj;

                    return nResult;
                }
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }

        /// <summary>
        /// 大视野相机入料补正算法
        /// </summary>
        /// <param name="ho_GrayImage"></param>
        /// <param name="ho_Circle"></param>
        /// <param name="ho_DispObjectsConcat"></param>
        /// <param name="ho_Rectangle1"></param>
        /// <param name="hv_GrayOpenHeight"></param>
        /// <param name="hv_GrayOpenWidth"></param>
        /// <param name="hv_DynThr"></param>
        /// <param name="hv_HysThrMin"></param>
        /// <param name="hv_HysThrMax"></param>
        /// <param name="hv_SelectAreaMin"></param>
        /// <param name="hv_SingleSelectAreaMin"></param>
        /// <param name="hv_SingleSelectAreaMax"></param>
        /// <param name="hv_ClosingW"></param>
        /// <param name="hv_ClosingH"></param>
        /// <param name="hv_ProdAngleMean"></param>
        /// <param name="hv_BigCenterRow"></param>
        /// <param name="hv_BigCenterCol"></param>
        /// <param name="hv_Error"></param>
        public void GetProdRegionAngle(HObject ho_GrayImage, HObject ho_Circle, out HObject ho_DispObjectsConcat,
                                        out HObject ho_Rectangle1, HTuple hv_GrayOpenHeight, HTuple hv_GrayOpenWidth,
                                        HTuple hv_DynThr, HTuple hv_HysThrMin, HTuple hv_HysThrMax, HTuple hv_SelectAreaMin,
                                        HTuple hv_SingleSelectAreaMin, HTuple hv_SingleSelectAreaMax, HTuple hv_ClosingW,
                                        HTuple hv_ClosingH, out HTuple hv_ProdAngleMean, out HTuple hv_BigCenterRow,
                                        out HTuple hv_BigCenterCol, out HTuple hv_Error)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageOpening = null, ho_RegionDynThresh = null;
            HObject ho_RegionHysteresis1 = null, ho_ObjectsConcat1 = null;
            HObject ho_RegionUnion1 = null, ho_RegionFillUp = null, ho_ConnectedRegions = null;
            HObject ho_RegionIntersection = null, ho_SelectedRegions = null;
            HObject ho_RegionUnion = null, ho_RegionClosing = null, ho_RegionOpening = null;
            HObject ho_SelectedRegions1 = null, ho_RegionTrans = null;

            // Local control variables 

            HTuple hv_Row4 = new HTuple(), hv_Column4 = new HTuple();
            HTuple hv_Phi1 = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_Sorted = new HTuple();
            HTuple hv_Length = new HTuple(), hv_StartP = new HTuple();
            HTuple hv_EndP = new HTuple(), hv_Selected = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DispObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            hv_ProdAngleMean = new HTuple();
            hv_BigCenterRow = new HTuple();
            hv_BigCenterCol = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    ho_ImageOpening.Dispose();
                    HOperatorSet.GrayOpeningRect(ho_GrayImage, out ho_ImageOpening, hv_GrayOpenHeight,
                        hv_GrayOpenWidth);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_GrayImage, ho_ImageOpening, out ho_RegionDynThresh,
                        hv_DynThr, "light");
                    ho_RegionHysteresis1.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_GrayImage, out ho_RegionHysteresis1,
                        hv_HysThrMin, hv_HysThrMax, 100);
                    ho_ObjectsConcat1.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionDynThresh, ho_RegionHysteresis1, out ho_ObjectsConcat1
                        );
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat1, out ho_RegionUnion1);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionUnion1, out ho_RegionFillUp);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_ConnectedRegions, ho_Circle, out ho_RegionIntersection
                        );
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_RegionIntersection, out ho_SelectedRegions, "area",
                        "and", hv_SelectAreaMin, 50000);
                    //
                    //
                    HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row4, out hv_Column4,
                        out hv_Phi1, out hv_Length11, out hv_Length21);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_Row4, hv_Column4, hv_Phi1,
                        hv_Length11, hv_Length21);
                    HOperatorSet.TupleSort(hv_Phi1, out hv_Sorted);
                    HOperatorSet.TupleLength(hv_Sorted, out hv_Length);
                    //取角度排序的中间数据
                    hv_StartP = ((hv_Length * 0.2)).TupleRound();
                    hv_EndP = ((hv_Length * 0.8)).TupleRound();
                    HOperatorSet.TupleSelectRange(hv_Sorted, hv_StartP, hv_EndP, out hv_Selected);
                    HOperatorSet.TupleMean(hv_Selected, out hv_ProdAngleMean);
                    //
                    //
                    //获取产品中心坐标
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_Rectangle1, out ho_SelectedRegions, "area", "and",
                        hv_SingleSelectAreaMin, hv_SingleSelectAreaMax);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing, hv_ClosingW,
                        hv_ClosingH);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, 200,
                        200);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions1,
                        "max_area", 70);
                    HOperatorSet.AreaCenter(ho_SelectedRegions1, out hv_Area, out hv_BigCenterRow,
                        out hv_BigCenterCol);
                    ho_RegionTrans.Dispose();
                    HOperatorSet.ShapeTrans(ho_SelectedRegions1, out ho_RegionTrans, "rectangle2");
                    HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area1, out hv_BigCenterRow,
                        out hv_BigCenterCol);
                    ho_DispObjectsConcat.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_DispObjectsConcat, hv_BigCenterRow,
                        hv_BigCenterCol, 500, 0);
                    //
                    //concat_obj (Rectangle1, RegionTrans, ObjectsConcat)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_RegionTrans, ho_DispObjectsConcat, out ExpTmpOutVar_0
                            );
                        ho_DispObjectsConcat.Dispose();
                        ho_DispObjectsConcat = ExpTmpOutVar_0;
                    }
                    //
                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ImageOpening.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionHysteresis1.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageOpening.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionHysteresis1.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 大视野定位每行产品中心
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_Rectangle"></param>
        /// <param name="ho_OutRegion"></param>
        /// <param name="hv_AnyRow"></param>
        /// <param name="hv_AnyCol"></param>
        /// <param name="hv_AnyAng"></param>
        /// <param name="hv_Count"></param>
        public void LDFindAnyCenter(HObject ho_Image, HObject ho_Rectangle, out HObject ho_OutRegion,
                                        out HTuple hv_AnyRow, out HTuple hv_AnyCol, out HTuple hv_AnyAng, out HTuple hv_Count)
        {



            // Local iconic variables 

            HObject ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_RegionClosing1, ho_ConnectedRegions, ho_SelectedRegions;
            HObject ho_SortedRegions, ho_Contours, ho_Cross;

            // Local control variables 

            HTuple hv_UsedThreshold = null, hv_Row = null;
            HTuple hv_Column = null, hv_Phi = null, hv_Length1 = null;
            HTuple hv_Length2 = null, hv_PointOrder = null, hv_Area = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Deg = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OutRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);

            ho_Region.Dispose();
            HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region, "max_separability",
                "light", out hv_UsedThreshold);

            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, 100, 1);
            ho_RegionClosing1.Dispose();
            HOperatorSet.ClosingRectangle1(ho_RegionClosing, out ho_RegionClosing1, 100,
                1800);

            HOperatorSet.OpeningRectangle1(ho_RegionClosing1, out ho_RegionClosing1, 100, 200);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionClosing1, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 200000, 9999999);
            //shape_trans (SelectedRegions, RegionTrans, 'rectangle2')
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character",
                "true", "row");
            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_SortedRegions, out ho_Contours, "border");

            HOperatorSet.FitRectangle2ContourXld(ho_Contours, "regression", -1, 0, 0, 3,
                2, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2,
                out hv_PointOrder);

            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row1, out hv_Column1);
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 200, hv_Phi);

            ho_OutRegion.Dispose();
            HOperatorSet.ConcatObj(ho_Contours, ho_Cross, out ho_OutRegion);
            hv_Count = new HTuple(hv_Row.TupleLength());

            hv_Row1 = ((hv_Row1.TupleString(".2f"))).TupleNumber();
            hv_Column1 = ((hv_Column1.TupleString(".2f"))).TupleNumber();

            HOperatorSet.TupleDeg(hv_Phi, out hv_Deg);
            hv_Deg = ((hv_Deg.TupleString(".2f"))).TupleNumber();

            hv_AnyRow = hv_Row1.Clone();
            hv_AnyCol = hv_Column1.Clone();
            hv_AnyAng = hv_Deg.Clone();


            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionClosing.Dispose();
            ho_RegionClosing1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SortedRegions.Dispose();
            ho_Contours.Dispose();
            ho_Cross.Dispose();

            return;
        }

        /// <summary>
        /// 获取产品位置区分产品类型
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_DanGeProdRegion"></param>
        /// <param name="ho_WeiJieProdRegion"></param>
        /// <param name="ho_WeiJieReduceProdRegion"></param>
        /// <param name="hv_ModelRegionArea"></param>
        /// <param name="hv_ModelRegionWidth"></param>
        /// <param name="hv_ModelRegionHeight"></param>
        /// <param name="hv_ScaleImageValue"></param>
        /// <param name="hv_ThrsholdMin"></param>
        /// <param name="hv_HysThresholdMin"></param>
        /// <param name="hv_HysThesholdMax"></param>
        /// <param name="hv_CloseRec"></param>
        /// <param name="hv_ModelRegionAreaDownValue"></param>
        /// <param name="hv_IsNotProdValue"></param>
        /// <param name="hv_IsNotProdValueMin"></param>
        /// <param name="hv_AutoThreshold"></param>
        /// <param name="hv_DanGeStateName"></param>
        /// <param name="hv_WeiJieProdName"></param>
        /// <param name="hv_WeiJieReduceProdName"></param>
        /// <param name="hv_Exception"></param>
        /// <param name="hv_Error"></param>
        public void GetAllProdZoomRegion(HObject ho_Image1, out HObject ho_DanGeProdRegion,
                                        out HObject ho_WeiJieProdRegion, out HObject ho_WeiJieReduceProdRegion, HTuple hv_ModelRegionArea,
                                        HTuple hv_ModelRegionWidth, HTuple hv_ModelRegionHeight, HTuple hv_ScaleImageValue,
                                        HTuple hv_ThrsholdMin, HTuple hv_HysThresholdMin, HTuple hv_HysThesholdMax,
                                        HTuple hv_CloseRec, HTuple hv_ModelRegionAreaDownValue, HTuple hv_IsNotProdValue,
                                        HTuple hv_IsNotProdValueMin, HTuple hv_AutoThreshold, out HTuple hv_DanGeStateName,
                                        out HTuple hv_WeiJieProdName, out HTuple hv_WeiJieReduceProdName, out HTuple hv_Exception,
                                        out HTuple hv_Error)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageZoomed2 = null, ho_ZImage1 = null;
            HObject ho_ZImage2 = null, ho_ZImage3 = null, ho_ZImageH = null;
            HObject ho_ZImageS = null, ho_ZImageI = null, ho_GrayImage2 = null;
            HObject ho_Region = null, ho_RegionHysteresis = null, ho_RegionUnion = null;
            HObject ho_RegionFillUp1 = null, ho_RegionClosing = null, ho_Region1 = null;
            HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_RegionOpening1 = null, ho_RegionDifference1 = null;
            HObject ho_RegionOpening2 = null, ho_ConnectedRegions3 = null;
            HObject ho_SelectedRegions4 = null, ho_SortedRegions2 = null;
            HObject ho_ProductObject = null, ho_ObjectSelected = null, ho_ImageReduced2 = null;
            HObject ho_ImageMedian = null, ho_Regions = null, ho_RegionFillUp = null;
            HObject ho_SelectedRegions1 = null, ho_RegionOpening3 = null;
            HObject ho_ConnectedRegions5 = null, ho_SelectedRegions5 = null;
            HObject ho_CopySortedRegionsIsProd = null, ho_ObjectSelected1 = null;
            HObject ho_SelectedRegions7 = null, ho_ConnectedRegions1 = null;
            HObject ho_SelectedRegions2 = null, ho_ObjectSelected3 = null;
            HObject ho_RegionDifference = null, ho_ConnectedRegions2 = null;
            HObject ho_SelectedRegions3 = null, ho_RegionTrans1 = null;
            HObject ho_InPartitionRegion = null, ho_PartitionedRegionTrans = null;
            HObject ho_ObjectSelected4 = null, ho_RegionDifference2 = null;
            HObject ho_RegionUnion1 = null, ho_ImageReduced = null, ho_ImageMedian2 = null;
            HObject ho_Region4 = null, ho_ImageReduced1 = null, ho_ImageMedian3 = null;
            HObject ho_Region2 = null, ho_ObjectsConcat = null, ho_RegionUnion2 = null;
            HObject ho_RegionClosing3 = null, ho_RegionOpening = null, ho_ConnectedRegions4 = null;
            HObject ho_RegionClosing4 = null, ho_SelectedRegions6 = null;
            HObject ho_Partitioned1 = null, ho_ObjectSelected5 = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Number6 = new HTuple(), hv_ZImageIValue = new HTuple();
            HTuple hv_ZImageHValue = new HTuple(), hv_Length3 = new HTuple();
            HTuple hv_ZImageIMax = new HTuple(), hv_ZImageHMin = new HTuple();
            HTuple hv_Length = new HTuple(), hv_Newtuple = new HTuple();
            HTuple hv_Diff = new HTuple(), hv_Greater = new HTuple();
            HTuple hv_Sum = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Reduced = new HTuple(), hv_Index4 = new HTuple();
            HTuple hv_SelectedIValue = new HTuple(), hv_SelectedHvalue = new HTuple();
            HTuple hv_DeltaIValue = new HTuple(), hv_DeltaHValue = new HTuple();
            HTuple hv_DynThr = new HTuple(), hv_SelectWidth = new HTuple();
            HTuple hv_Number7 = new HTuple(), hv_Error1 = new HTuple();
            HTuple hv_Number4 = new HTuple(), hv_Index7 = new HTuple();
            HTuple hv_Number2 = new HTuple(), hv_Index3 = new HTuple();
            HTuple hv_Number5 = new HTuple(), hv_Index8 = new HTuple();
            HTuple hv_IValue = new HTuple(), hv_IValueMean = new HTuple();
            HTuple hv_HValue = new HTuple(), hv_HValueMean = new HTuple();
            HTuple hv_Number1 = new HTuple(), hv_Number3 = new HTuple();
            HTuple hv_Index6 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DanGeProdRegion);
            HOperatorSet.GenEmptyObj(out ho_WeiJieProdRegion);
            HOperatorSet.GenEmptyObj(out ho_WeiJieReduceProdRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageZoomed2);
            HOperatorSet.GenEmptyObj(out ho_ZImage1);
            HOperatorSet.GenEmptyObj(out ho_ZImage2);
            HOperatorSet.GenEmptyObj(out ho_ZImage3);
            HOperatorSet.GenEmptyObj(out ho_ZImageH);
            HOperatorSet.GenEmptyObj(out ho_ZImageS);
            HOperatorSet.GenEmptyObj(out ho_ZImageI);
            HOperatorSet.GenEmptyObj(out ho_GrayImage2);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions3);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions2);
            HOperatorSet.GenEmptyObj(out ho_ProductObject);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening3);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions5);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions5);
            HOperatorSet.GenEmptyObj(out ho_CopySortedRegionsIsProd);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions7);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions3);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans1);
            HOperatorSet.GenEmptyObj(out ho_InPartitionRegion);
            HOperatorSet.GenEmptyObj(out ho_PartitionedRegionTrans);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected4);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian2);
            HOperatorSet.GenEmptyObj(out ho_Region4);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian3);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing3);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions4);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing4);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions6);
            HOperatorSet.GenEmptyObj(out ho_Partitioned1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected5);
            hv_DanGeStateName = new HTuple();
            hv_WeiJieProdName = new HTuple();
            hv_WeiJieReduceProdName = new HTuple();
            hv_Error = new HTuple();
            hv_Exception = new HTuple();
            try
            {
                try
                {
                    ho_ImageZoomed2.Dispose();
                    HOperatorSet.ZoomImageFactor(ho_Image1, out ho_ImageZoomed2, hv_ScaleImageValue,
                        hv_ScaleImageValue, "nearest_neighbor");
                    ho_ZImage1.Dispose(); ho_ZImage2.Dispose(); ho_ZImage3.Dispose();
                    HOperatorSet.Decompose3(ho_ImageZoomed2, out ho_ZImage1, out ho_ZImage2,
                        out ho_ZImage3);
                    ho_ZImageH.Dispose(); ho_ZImageS.Dispose(); ho_ZImageI.Dispose();
                    HOperatorSet.TransToRgb(ho_ZImage1, ho_ZImage2, ho_ZImage3, out ho_ZImageH,
                        out ho_ZImageS, out ho_ZImageI, "hsi");

                    //去除两端浅黄色的不是产品,Reduce出去这个区域，然后复判这个区域
                    ho_GrayImage2.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_ImageZoomed2, out ho_GrayImage2);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_GrayImage2, out ho_Region, hv_ThrsholdMin, 255);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_GrayImage2, out ho_RegionHysteresis,
                        hv_HysThresholdMin, hv_HysThesholdMax, 50);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho_Region, ho_RegionHysteresis, out ho_RegionUnion);
                    ho_RegionFillUp1.Dispose();
                    HOperatorSet.FillUp(ho_RegionUnion, out ho_RegionFillUp1);
                    //将轻微断裂的包含进来
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionFillUp1, out ho_RegionClosing, hv_CloseRec + 100,
                        hv_CloseRec);
                    //增加接近靠拢部分，应该判断为单个产品 2021-11-18
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_GrayImage2, out ho_Region1, 0, 20);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_Region1, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
                        "max_area", 70);
                    ho_RegionOpening1.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_SelectedRegions, out ho_RegionOpening1,
                        3, 3);
                    ho_RegionDifference1.Dispose();
                    HOperatorSet.Difference(ho_RegionClosing, ho_RegionOpening1, out ho_RegionDifference1
                        );
                    ho_RegionOpening2.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionDifference1, out ho_RegionOpening2,
                        30, 30);

                    ho_ConnectedRegions3.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening2, out ho_ConnectedRegions3);
                    ho_SelectedRegions4.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions3, out ho_SelectedRegions4, "area",
                        "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, 8 * hv_ModelRegionArea);
                    ho_SortedRegions2.Dispose();
                    HOperatorSet.SortRegion(ho_SelectedRegions4, out ho_SortedRegions2, "first_point",
                        "true", "row");
                    //去除两端不是产品 与 产品未解离的情况,将SortedRegions2 变更为 ProductObject  2021-11-20
                    HOperatorSet.CountObj(ho_SortedRegions2, out hv_Number);
                    ho_ProductObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ProductObject);
                    HTuple end_val27 = hv_Number;
                    HTuple step_val27 = 1;
                    for (hv_Index1 = 1; hv_Index1.Continue(end_val27, step_val27); hv_Index1 = hv_Index1.TupleAdd(step_val27))
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_SortedRegions2, out ho_ObjectSelected, hv_Index1);
                        ho_ImageReduced2.Dispose();
                        HOperatorSet.ReduceDomain(ho_ZImageH, ho_ObjectSelected, out ho_ImageReduced2
                            );
                        ho_ImageMedian.Dispose();
                        HOperatorSet.MedianRect(ho_ImageReduced2, out ho_ImageMedian, 8, 8);
                        ho_Regions.Dispose();
                        HOperatorSet.AutoThreshold(ho_ImageMedian, out ho_Regions, hv_AutoThreshold);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUpShape(ho_Regions, out ho_RegionFillUp, "area", 1, 5000);
                        ho_SelectedRegions1.Dispose();
                        HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions1, "area",
                            "and", 90000, 99999999);
                        ho_RegionOpening3.Dispose();
                        HOperatorSet.OpeningCircle(ho_SelectedRegions1, out ho_RegionOpening3,
                            10);
                        ho_ConnectedRegions5.Dispose();
                        HOperatorSet.Connection(ho_RegionOpening3, out ho_ConnectedRegions5);
                        ho_SelectedRegions5.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions5, out ho_SelectedRegions5,
                            "area", "and", 90000, 99999999);
                        HOperatorSet.CountObj(ho_SelectedRegions5, out hv_Number6);
                        if ((int)(new HTuple(hv_Number6.TupleGreater(1))) != 0)
                        {
                            //两端 不是产品和 产品 有未解离
                        }
                        else
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_ProductObject, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_ProductObject.Dispose();
                                ho_ProductObject = ExpTmpOutVar_0;
                            }
                        }
                    }

                    //CopySortedRegionsIsProd 是产品，包括产品与产品链接在一起的
                    ho_CopySortedRegionsIsProd.Dispose();
                    HOperatorSet.CopyObj(ho_ProductObject, out ho_CopySortedRegionsIsProd, 1,
                        1000);
                    HOperatorSet.GrayFeatures(ho_ProductObject, ho_ZImageI, "mean", out hv_ZImageIValue);
                    HOperatorSet.GrayFeatures(ho_ProductObject, ho_ZImageH, "mean", out hv_ZImageHValue);
                    HOperatorSet.TupleLength(hv_ZImageIValue, out hv_Length3);
                    HOperatorSet.TupleMax(hv_ZImageIValue, out hv_ZImageIMax);
                    HOperatorSet.TupleMin(hv_ZImageHValue, out hv_ZImageHMin);
                    //判断如果ZImageHMin 太黑，为不是非产品 2021 - 11 -20
                    //判断图像中偶发出现亮度不均匀的产品，剔除，整体留着  2021-11-23
                    HOperatorSet.TupleLength(hv_ZImageHValue, out hv_Length);
                    if ((int)(new HTuple(hv_Length.TupleGreater(0))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_Length, hv_ZImageHMin, out hv_Newtuple);
                        HOperatorSet.TupleSub(hv_ZImageHValue, hv_Newtuple, out hv_Diff);
                        //比50的的灰度差异大，不需要黑的产品 2021-11-23
                        HOperatorSet.TupleGreaterElem(hv_Diff, 50, out hv_Greater);
                        HOperatorSet.TupleSum(hv_Greater, out hv_Sum);
                        if ((int)(new HTuple(hv_Sum.TupleGreater(0))) != 0)
                        {
                            HOperatorSet.TupleFind(hv_ZImageHValue, hv_ZImageHMin, out hv_Indices);
                            HOperatorSet.TupleRemove(hv_ZImageHValue, hv_Indices, out hv_Reduced);
                            HOperatorSet.TupleMin(hv_Reduced, out hv_ZImageHMin);
                        }
                    }


                    //比较在H，I图像通道的区域灰度最大最小差异值，
                    HTuple end_val70 = hv_Length3 - 1;
                    HTuple step_val70 = 1;
                    for (hv_Index4 = 0; hv_Index4.Continue(end_val70, step_val70); hv_Index4 = hv_Index4.TupleAdd(step_val70))
                    {
                        HOperatorSet.TupleSelect(hv_ZImageIValue, hv_Index4, out hv_SelectedIValue);
                        HOperatorSet.TupleSelect(hv_ZImageHValue, hv_Index4, out hv_SelectedHvalue);
                        hv_DeltaIValue = hv_ZImageIMax - hv_SelectedIValue;
                        hv_DeltaHValue = hv_ZImageHMin - hv_SelectedHvalue;
                        if ((int)((new HTuple(hv_DeltaIValue.TupleGreater(hv_IsNotProdValue))).TupleOr(
                            new HTuple(hv_DeltaHValue.TupleLess(hv_IsNotProdValueMin)))) != 0)
                        {
                            //去除不是产品，留下真实的产品 CopySortedRegionsIsProd
                            ho_ObjectSelected1.Dispose();
                            HOperatorSet.SelectObj(ho_ProductObject, out ho_ObjectSelected1, hv_Index4 + 1);
                            //根据内部纹理判断到底是否产品变暗导致，变暗的仍然为产品   2021 - 11-29
                            hv_DynThr = 30;
                            hv_SelectWidth = 1000;
                            ho_SelectedRegions7.Dispose();
                            CheckTexture(ho_ImageZoomed2, ho_ObjectSelected1, out ho_SelectedRegions7,
                                hv_DynThr, hv_SelectWidth, out hv_Number7, out hv_Error1);
                            if ((int)(new HTuple(hv_Number7.TupleGreaterEqual(2))) != 0)
                            {
                                //是产品
                            }
                            else
                            {
                                //不是产品
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Difference(ho_CopySortedRegionsIsProd, ho_ObjectSelected1,
                                        out ExpTmpOutVar_0);
                                    ho_CopySortedRegionsIsProd.Dispose();
                                    ho_CopySortedRegionsIsProd = ExpTmpOutVar_0;
                                }
                            }
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_CopySortedRegionsIsProd, out ExpTmpOutVar_0,
                            "area", "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, 6 * hv_ModelRegionArea);
                        ho_CopySortedRegionsIsProd.Dispose();
                        ho_CopySortedRegionsIsProd = ExpTmpOutVar_0;
                    }


                    //***************筛选出完整单个产品,命名
                    ho_DanGeProdRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_DanGeProdRegion);
                    hv_DanGeStateName = new HTuple();
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_CopySortedRegionsIsProd, out ho_ConnectedRegions1
                        );
                    ho_SelectedRegions2.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions2, "area",
                        "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, hv_ModelRegionArea + hv_ModelRegionAreaDownValue);
                    HOperatorSet.CountObj(ho_SelectedRegions2, out hv_Number4);
                    HTuple end_val99 = hv_Number4;
                    HTuple step_val99 = 1;
                    for (hv_Index7 = 1; hv_Index7.Continue(end_val99, step_val99); hv_Index7 = hv_Index7.TupleAdd(step_val99))
                    {
                        ho_ObjectSelected3.Dispose();
                        HOperatorSet.SelectObj(ho_SelectedRegions2, out ho_ObjectSelected3, hv_Index7);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DanGeProdRegion, ho_ObjectSelected3, out ExpTmpOutVar_0
                                );
                            ho_DanGeProdRegion.Dispose();
                            ho_DanGeProdRegion = ExpTmpOutVar_0;
                        }
                        hv_DanGeStateName = hv_DanGeStateName.TupleConcat("是单个独立产品");
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_DanGeProdRegion, out ExpTmpOutVar_0, "area",
                            "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, hv_ModelRegionArea + hv_ModelRegionAreaDownValue);
                        ho_DanGeProdRegion.Dispose();
                        ho_DanGeProdRegion = ExpTmpOutVar_0;
                    }
                    //未解离开的产品进行分割，命名
                    ho_WeiJieProdRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_WeiJieProdRegion);
                    hv_WeiJieProdName = new HTuple();
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_CopySortedRegionsIsProd, ho_DanGeProdRegion, out ho_RegionDifference
                        );
                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions2);
                    ho_SelectedRegions3.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions3, "area",
                        "and", (2 * hv_ModelRegionArea) - hv_ModelRegionAreaDownValue, 6 * hv_ModelRegionArea);
                    ho_RegionTrans1.Dispose();
                    HOperatorSet.ShapeTrans(ho_SelectedRegions3, out ho_RegionTrans1, "rectangle2");
                    HOperatorSet.CountObj(ho_RegionTrans1, out hv_Number2);
                    if ((int)(new HTuple(hv_Number2.TupleGreater(0))) != 0)
                    {
                        //有未解离开的产品
                        HTuple end_val115 = hv_Number2;
                        HTuple step_val115 = 1;
                        for (hv_Index3 = 1; hv_Index3.Continue(end_val115, step_val115); hv_Index3 = hv_Index3.TupleAdd(step_val115))
                        {
                            ho_InPartitionRegion.Dispose();
                            HOperatorSet.SelectObj(ho_RegionTrans1, out ho_InPartitionRegion, hv_Index3);
                            ho_PartitionedRegionTrans.Dispose();
                            PartitionRegionTransAngle(ho_InPartitionRegion, ho_ZImageH, out ho_PartitionedRegionTrans,
                                hv_ModelRegionWidth, hv_ModelRegionHeight, hv_AutoThreshold, out hv_Error);
                            HOperatorSet.CountObj(ho_PartitionedRegionTrans, out hv_Number5);
                            HTuple end_val119 = hv_Number5;
                            HTuple step_val119 = 1;
                            for (hv_Index8 = 1; hv_Index8.Continue(end_val119, step_val119); hv_Index8 = hv_Index8.TupleAdd(step_val119))
                            {
                                ho_ObjectSelected4.Dispose();
                                HOperatorSet.SelectObj(ho_PartitionedRegionTrans, out ho_ObjectSelected4,
                                    hv_Index8);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_WeiJieProdRegion, ho_ObjectSelected4, out ExpTmpOutVar_0
                                        );
                                    ho_WeiJieProdRegion.Dispose();
                                    ho_WeiJieProdRegion = ExpTmpOutVar_0;
                                }
                                hv_WeiJieProdName = hv_WeiJieProdName.TupleConcat("未解离产品");
                            }
                        }
                    }
                    else
                    {
                        //没有未解离开的产品
                        ho_WeiJieProdRegion.Dispose();
                        HOperatorSet.GenEmptyRegion(out ho_WeiJieProdRegion);
                        hv_WeiJieProdName = new HTuple();
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_WeiJieProdRegion, out ExpTmpOutVar_0, "area",
                            "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, hv_ModelRegionArea + hv_ModelRegionAreaDownValue);
                        ho_WeiJieProdRegion.Dispose();
                        ho_WeiJieProdRegion = ExpTmpOutVar_0;
                    }


                    //复判 Reduce出去这个区域，看是否有产品链接一起未解离开
                    //从ZImageI中提取产品，未解离开的产品与非产品 也要提取，命名
                    ho_RegionDifference2.Dispose();
                    HOperatorSet.Difference(ho_SelectedRegions4, ho_CopySortedRegionsIsProd,
                        out ho_RegionDifference2);
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_RegionDifference2, out ho_RegionUnion1);
                    HOperatorSet.GrayFeatures(ho_CopySortedRegionsIsProd, ho_ZImageI, "mean",
                        out hv_IValue);
                    HOperatorSet.TupleMean(hv_IValue, out hv_IValueMean);
                    HOperatorSet.GrayFeatures(ho_CopySortedRegionsIsProd, ho_ZImageH, "mean",
                        out hv_HValue);
                    HOperatorSet.TupleMean(hv_HValue, out hv_HValueMean);
                    //从ZImageI 通道提取
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ZImageI, ho_RegionUnion1, out ho_ImageReduced
                        );
                    ho_ImageMedian2.Dispose();
                    HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian2, 5, 5);
                    ho_Region4.Dispose();
                    HOperatorSet.FastThreshold(ho_ImageMedian2, out ho_Region4, hv_IValueMean - 30,
                        255, 200);


                    //从ZImageH 通道提取
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_ZImageH, ho_RegionUnion1, out ho_ImageReduced1
                        );
                    ho_ImageMedian3.Dispose();
                    HOperatorSet.MedianRect(ho_ImageReduced1, out ho_ImageMedian3, 5, 5);
                    ho_Region2.Dispose();
                    HOperatorSet.Threshold(ho_ImageMedian3, out ho_Region2, hv_HValueMean - 30,
                        hv_HValueMean + 30);

                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_Region4, ho_Region2, out ho_ObjectsConcat);
                    ho_RegionUnion2.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion2);
                    ho_RegionClosing3.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion2, out ho_RegionClosing3, 50,
                        10);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionClosing3, out ho_RegionOpening, 30,
                        30);
                    ho_ConnectedRegions4.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions4);
                    ho_RegionClosing4.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_ConnectedRegions4, out ho_RegionClosing4,
                        20, 20);
                    ho_SelectedRegions6.Dispose();
                    HOperatorSet.SelectShape(ho_RegionClosing4, out ho_SelectedRegions6, "area",
                        "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, 6 * hv_ModelRegionArea);

                    HOperatorSet.CountObj(ho_SelectedRegions6, out hv_Number1);
                    ho_WeiJieReduceProdRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_WeiJieReduceProdRegion);
                    hv_WeiJieReduceProdName = new HTuple();
                    if ((int)(new HTuple(hv_Number1.TupleEqual(0))) != 0)
                    {
                        //产品与非产品没有连接
                    }
                    else if ((int)(new HTuple(hv_Number1.TupleEqual(1))) != 0)
                    {
                        //产品与非产品有连接  需要通过图像分析自动分割与非产品是否有未解离  2021-11-20
                        ho_Partitioned1.Dispose();
                        PartitionRegionTransAngle(ho_SelectedRegions6, ho_ZImageH, out ho_Partitioned1,
                            hv_ModelRegionWidth, hv_ModelRegionHeight, hv_AutoThreshold, out hv_Error);
                        HOperatorSet.CountObj(ho_Partitioned1, out hv_Number3);
                        HTuple end_val169 = hv_Number3;
                        HTuple step_val169 = 1;
                        for (hv_Index6 = 1; hv_Index6.Continue(end_val169, step_val169); hv_Index6 = hv_Index6.TupleAdd(step_val169))
                        {
                            ho_ObjectSelected5.Dispose();
                            HOperatorSet.SelectObj(ho_Partitioned1, out ho_ObjectSelected5, hv_Index6);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_WeiJieReduceProdRegion, ho_ObjectSelected5,
                                    out ExpTmpOutVar_0);
                                ho_WeiJieReduceProdRegion.Dispose();
                                ho_WeiJieReduceProdRegion = ExpTmpOutVar_0;
                            }
                            hv_WeiJieReduceProdName = hv_WeiJieReduceProdName.TupleConcat("复判是产品区域未解离");
                        }
                    }
                    else
                    {
                        //非产品时独立的
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SelectShape(ho_WeiJieReduceProdRegion, out ExpTmpOutVar_0, "area",
                            "and", hv_ModelRegionArea - hv_ModelRegionAreaDownValue, hv_ModelRegionArea + hv_ModelRegionAreaDownValue);
                        ho_WeiJieReduceProdRegion.Dispose();
                        ho_WeiJieReduceProdRegion = ExpTmpOutVar_0;
                    }

                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ImageZoomed2.Dispose();
                ho_ZImage1.Dispose();
                ho_ZImage2.Dispose();
                ho_ZImage3.Dispose();
                ho_ZImageH.Dispose();
                ho_ZImageS.Dispose();
                ho_ZImageI.Dispose();
                ho_GrayImage2.Dispose();
                ho_Region.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionClosing.Dispose();
                ho_Region1.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionOpening1.Dispose();
                ho_RegionDifference1.Dispose();
                ho_RegionOpening2.Dispose();
                ho_ConnectedRegions3.Dispose();
                ho_SelectedRegions4.Dispose();
                ho_SortedRegions2.Dispose();
                ho_ProductObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionOpening3.Dispose();
                ho_ConnectedRegions5.Dispose();
                ho_SelectedRegions5.Dispose();
                ho_CopySortedRegionsIsProd.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_SelectedRegions7.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionDifference.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions3.Dispose();
                ho_RegionTrans1.Dispose();
                ho_InPartitionRegion.Dispose();
                ho_PartitionedRegionTrans.Dispose();
                ho_ObjectSelected4.Dispose();
                ho_RegionDifference2.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMedian2.Dispose();
                ho_Region4.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageMedian3.Dispose();
                ho_Region2.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionClosing3.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions4.Dispose();
                ho_RegionClosing4.Dispose();
                ho_SelectedRegions6.Dispose();
                ho_Partitioned1.Dispose();
                ho_ObjectSelected5.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoomed2.Dispose();
                ho_ZImage1.Dispose();
                ho_ZImage2.Dispose();
                ho_ZImage3.Dispose();
                ho_ZImageH.Dispose();
                ho_ZImageS.Dispose();
                ho_ZImageI.Dispose();
                ho_GrayImage2.Dispose();
                ho_Region.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionClosing.Dispose();
                ho_Region1.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionOpening1.Dispose();
                ho_RegionDifference1.Dispose();
                ho_RegionOpening2.Dispose();
                ho_ConnectedRegions3.Dispose();
                ho_SelectedRegions4.Dispose();
                ho_SortedRegions2.Dispose();
                ho_ProductObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionOpening3.Dispose();
                ho_ConnectedRegions5.Dispose();
                ho_SelectedRegions5.Dispose();
                ho_CopySortedRegionsIsProd.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_SelectedRegions7.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionDifference.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions3.Dispose();
                ho_RegionTrans1.Dispose();
                ho_InPartitionRegion.Dispose();
                ho_PartitionedRegionTrans.Dispose();
                ho_ObjectSelected4.Dispose();
                ho_RegionDifference2.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMedian2.Dispose();
                ho_Region4.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageMedian3.Dispose();
                ho_Region2.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionClosing3.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions4.Dispose();
                ho_RegionClosing4.Dispose();
                ho_SelectedRegions6.Dispose();
                ho_Partitioned1.Dispose();
                ho_ObjectSelected5.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 判断产品是否居中
        /// </summary>
        /// <param name="ho_Image">图像</param>
        /// <param name="ho_OutRegion">输出Region</param>
        /// <param name="hv_MinCol">最小Column</param>
        /// <param name="hv_MinGray">最小灰度值</param>
        /// <param name="hv_IsCenterPos">产品是否居中</param>
        /// <param name="hv_IsExistProduct">视野是否存在产品</param>
        public void LDSmallJudgePos(HObject ho_Image, out HObject ho_OutRegion, HTuple hv_MinCol,
                                        HTuple hv_MinGray, out HTuple hv_IsCenterPos, out HTuple hv_IsExistProduct, out HTuple hv_SubCol)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_GrayImage, ho_Region, ho_RegionFillUp;
            HObject ho_ConnectedRegions, ho_SelectedRegions;

            // Local control variables 

            HTuple hv_Area = null, hv_Row = null, hv_Column = null;
            HTuple hv_Width = null, hv_Height = null, hv_MeanCol = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OutRegion);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            hv_IsCenterPos = new HTuple();
            hv_IsExistProduct = new HTuple();
            hv_SubCol = new HTuple();
            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);

            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_GrayImage, out ho_Region, hv_MinGray, 255);
            //binary_threshold (GrayImage, Region, 'max_separability', 'light', UsedThreshold)

            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 200000, 9999999);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Union1(ho_SelectedRegions, out ExpTmpOutVar_0);
                ho_SelectedRegions.Dispose();
                ho_SelectedRegions = ExpTmpOutVar_0;
            }

            HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);

            ho_OutRegion.Dispose();
            ho_OutRegion = ho_SelectedRegions.CopyObj(1, -1);

            HOperatorSet.GetImageSize(ho_GrayImage, out hv_Width, out hv_Height);


            if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleGreater(0))) != 0)
            {
                //tuple_mean (Row, MeanRow)
                HOperatorSet.TupleMean(hv_Column, out hv_MeanCol);
                //*     SubRow := Height/2 - MeanRow
                hv_SubCol = (hv_Width / 2) - hv_MeanCol;
                hv_IsExistProduct = 1;
                if ((int)(new HTuple(((hv_SubCol.TupleAbs())).TupleGreater(hv_MinCol))) != 0)
                {
                    hv_IsCenterPos = 0;
                }
                else
                {
                    hv_IsCenterPos = 1;
                }
            }
            else
            {
                //SubRow := 0
                hv_SubCol = 0;
                hv_IsCenterPos = 0;
                hv_IsExistProduct = 0;
            }

            ho_GrayImage.Dispose();
            ho_Region.Dispose();
            ho_RegionFillUp.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();

            return;
        }

        public void CreateShapeModelInfo(HObject ho_Image, HObject ho_CreateRectangle,
                                        out HObject ho_ModelRectangle, HTuple hv_ScaleImage, HTuple hv_NumLevels, HTuple hv_AngleStart,
                                        HTuple hv_AngleExtent, HTuple hv_AngleStep, HTuple hv_Optimization, HTuple hv_Metric,
                                        HTuple hv_Contrast, HTuple hv_MinContrast, HTuple hv_ShapeModel, out HTuple hv_ModelID,
                                        out HTuple hv_ModelRow, out HTuple hv_ModelColumn, out HTuple hv_ModelAngle,
                                        out HTuple hv_ModelScore, out HTuple hv_ModelRegionArea, out HTuple hv_ModelRegionRow,
                                        out HTuple hv_ModelRegionColumn, out HTuple hv_Rec2ModelRow, out HTuple hv_Rec2ModelColumn,
                                        out HTuple hv_Rec2ModelPhi, out HTuple hv_ModelRegionWidth, out HTuple hv_ModelRegionHeight,
                                        out HTuple hv_ModelRValue, out HTuple hv_ModelGValue, out HTuple hv_ModelBValue,
                                        out HTuple hv_ModelIValue, out HTuple hv_bCreateModel)
        {
            // Local iconic variables 

            HObject ho_ImageZoomed = null, ho_RegionZoom = null;
            HObject ho_ImageReduced = null, ho_ImageGauss = null, ho_ImageMedian = null;
            HObject ho_GrayImage = null, ho_RegionHysteresis = null, ho_RegionClosing = null;
            HObject ho_RegionFillUp = null, ho_Contours = null, ho_ModelContours = null;
            HObject ho_ContoursAffineTrans = null, ho_ModelRegion = null;
            HObject ho_RegionTransRec2 = null, ho_ImageR = null, ho_ImageG = null;
            HObject ho_ImageB = null, ho_ImageH = null, ho_ImageS = null;
            HObject ho_ImageI = null;

            // Local control variables 

            HTuple hv_HomMat2D = new HTuple(), hv_Rec2ModelLength1 = new HTuple();
            HTuple hv_Rec2ModelLength2 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelRectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageZoomed);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ModelRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionTransRec2);
            HOperatorSet.GenEmptyObj(out ho_ImageR);
            HOperatorSet.GenEmptyObj(out ho_ImageG);
            HOperatorSet.GenEmptyObj(out ho_ImageB);
            HOperatorSet.GenEmptyObj(out ho_ImageH);
            HOperatorSet.GenEmptyObj(out ho_ImageS);
            HOperatorSet.GenEmptyObj(out ho_ImageI);
            hv_ModelID = new HTuple();
            hv_ModelRow = new HTuple();
            hv_ModelColumn = new HTuple();
            hv_ModelAngle = new HTuple();
            hv_ModelScore = new HTuple();
            hv_ModelRegionArea = new HTuple();
            hv_ModelRegionRow = new HTuple();
            hv_ModelRegionColumn = new HTuple();
            hv_Rec2ModelRow = new HTuple();
            hv_Rec2ModelColumn = new HTuple();
            hv_Rec2ModelPhi = new HTuple();
            hv_ModelRegionWidth = new HTuple();
            hv_ModelRegionHeight = new HTuple();
            hv_ModelRValue = new HTuple();
            hv_ModelGValue = new HTuple();
            hv_ModelBValue = new HTuple();
            hv_ModelIValue = new HTuple();
            hv_bCreateModel = new HTuple();
            try
            {
                try
                {
                    ho_ImageZoomed.Dispose();
                    HOperatorSet.ZoomImageFactor(ho_Image, out ho_ImageZoomed, hv_ScaleImage,
                        hv_ScaleImage, "constant");
                    ho_RegionZoom.Dispose();
                    HOperatorSet.ZoomRegion(ho_CreateRectangle, out ho_RegionZoom, hv_ScaleImage,
                        hv_ScaleImage);

                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageZoomed, ho_RegionZoom, out ho_ImageReduced
                        );
                    ho_ImageGauss.Dispose();
                    HOperatorSet.GaussFilter(ho_ImageReduced, out ho_ImageGauss, 5);
                    ho_ImageMedian.Dispose();
                    HOperatorSet.MedianImage(ho_ImageGauss, out ho_ImageMedian, "circle", 5,
                        "mirrored");
                    if ((int)(new HTuple(hv_ShapeModel.TupleEqual("ImageCreate"))) != 0)
                    {
                        HOperatorSet.CreateShapeModel(ho_ImageReduced, hv_NumLevels, hv_AngleStart,
                            hv_AngleExtent, hv_AngleStep, hv_Optimization, hv_Metric, hv_Contrast,
                            hv_MinContrast, out hv_ModelID);
                    }
                    else if ((int)(new HTuple(hv_ShapeModel.TupleEqual("RegionXLD"))) != 0)
                    {
                        ho_GrayImage.Dispose();
                        HOperatorSet.Rgb1ToGray(ho_ImageReduced, out ho_GrayImage);
                        ho_RegionHysteresis.Dispose();
                        HOperatorSet.HysteresisThreshold(ho_GrayImage, out ho_RegionHysteresis,
                            30, 60, 10);
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionHysteresis, out ho_RegionClosing,
                            100, 100);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                        ho_Contours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_RegionFillUp, out ho_Contours, "border");
                        HOperatorSet.CreateShapeModelXld(ho_Contours, hv_NumLevels, hv_AngleStart,
                            hv_AngleExtent, hv_AngleStep, hv_Optimization, "ignore_local_polarity",
                            10, out hv_ModelID);
                    }
                    HOperatorSet.FindShapeModel(ho_GrayImage, hv_ModelID, -0.39, 0.79, 0.5, 0,
                        0, "least_squares", (new HTuple(6)).TupleConcat(2), 0.9, out hv_ModelRow,
                        out hv_ModelColumn, out hv_ModelAngle, out hv_ModelScore);
                    //dev_display_shape_matching_results(hv_ModelID, "red", hv_ModelRow, hv_ModelColumn,
                    //    hv_ModelAngle, 1, 1, 0);
                    //获得拟合矩形的坐标
                    ho_ModelContours.Dispose();
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_ModelRow, hv_ModelColumn, hv_ModelAngle,
                        out hv_HomMat2D);
                    ho_ContoursAffineTrans.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffineTrans,
                        hv_HomMat2D);
                    ho_ModelRegion.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans, out ho_ModelRegion,
                        "filled");
                    HOperatorSet.AreaCenter(ho_ModelRegion, out hv_ModelRegionArea, out hv_ModelRegionRow,
                        out hv_ModelRegionColumn);
                    ho_RegionTransRec2.Dispose();
                    HOperatorSet.ShapeTrans(ho_ModelRegion, out ho_RegionTransRec2, "rectangle2");
                    HOperatorSet.SmallestRectangle2(ho_RegionTransRec2, out hv_Rec2ModelRow,
                        out hv_Rec2ModelColumn, out hv_Rec2ModelPhi, out hv_Rec2ModelLength1,
                        out hv_Rec2ModelLength2);
                    ho_ModelRectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_ModelRectangle, hv_Rec2ModelRow, hv_Rec2ModelColumn,
                        hv_Rec2ModelPhi, hv_Rec2ModelLength1, hv_Rec2ModelLength2);
                    hv_ModelRegionWidth = hv_Rec2ModelLength1 * 2.0;
                    hv_ModelRegionHeight = hv_Rec2ModelLength2 * 2.0;
                    //获得模板灰度信息
                    ho_ImageR.Dispose(); ho_ImageG.Dispose(); ho_ImageB.Dispose();
                    HOperatorSet.Decompose3(ho_ImageZoomed, out ho_ImageR, out ho_ImageG, out ho_ImageB
                        );
                    ho_ImageH.Dispose(); ho_ImageS.Dispose(); ho_ImageI.Dispose();
                    HOperatorSet.TransToRgb(ho_ImageR, ho_ImageG, ho_ImageB, out ho_ImageH, out ho_ImageS,
                        out ho_ImageI, "hsi");
                    HOperatorSet.GrayFeatures(ho_ModelRegion, ho_ImageR, "mean", out hv_ModelRValue);
                    HOperatorSet.GrayFeatures(ho_ModelRegion, ho_ImageG, "mean", out hv_ModelGValue);
                    HOperatorSet.GrayFeatures(ho_ModelRegion, ho_ImageB, "mean", out hv_ModelBValue);
                    HOperatorSet.GrayFeatures(ho_ModelRegion, ho_ImageI, "mean", out hv_ModelIValue);


                    //输出：ModelRegionArea，Rec2ModelRow, Rec2ModelColumn, Rec2ModelPhi，ModelRegionWidth，ModelRegionHeight


                    hv_bCreateModel = 1;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_bCreateModel = 0;
                }
                ho_ImageZoomed.Dispose();
                ho_RegionZoom.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageGauss.Dispose();
                ho_ImageMedian.Dispose();
                ho_GrayImage.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Contours.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffineTrans.Dispose();
                ho_ModelRegion.Dispose();
                ho_RegionTransRec2.Dispose();
                ho_ImageR.Dispose();
                ho_ImageG.Dispose();
                ho_ImageB.Dispose();
                ho_ImageH.Dispose();
                ho_ImageS.Dispose();
                ho_ImageI.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoomed.Dispose();
                ho_RegionZoom.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageGauss.Dispose();
                ho_ImageMedian.Dispose();
                ho_GrayImage.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Contours.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffineTrans.Dispose();
                ho_ModelRegion.Dispose();
                ho_RegionTransRec2.Dispose();
                ho_ImageR.Dispose();
                ho_ImageG.Dispose();
                ho_ImageB.Dispose();
                ho_ImageH.Dispose();
                ho_ImageS.Dispose();
                ho_ImageI.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 小视野定位算法
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_OCRRectangle1"></param>
        /// <param name="ModelPath"></param>
        /// <param name="PyramidLevel"></param>
        /// <param name="LastPyramidLevel"></param>
        /// <param name="MinScore"></param>
        /// <param name="Greediness"></param>
        /// <param name="hv_ThrsholdMin"></param>
        /// <param name="hv_HysThresholdMin"></param>
        /// <param name="hv_HysThesholdMax"></param>
        /// <param name="hv_CloseRec"></param>
        /// <param name="hv_ModelRegionAreaDownValue"></param>
        /// <param name="hv_IsNotProdValue"></param>
        /// <param name="hv_IsNotProdValueMin"></param>
        /// <param name="hv_AutoThreshold"></param>
        /// <param name="hv_DistancePP"></param>
        /// <param name="AllProdAndKongSortObj"></param>
        /// <param name="ho_OutRegion"></param>
        /// <param name="AllProdAndKongSortName"></param>
        /// <param name="strLog"></param>
        /// <param name="bResult"></param>
        public void LDSmallFixedAlgorithm(HObject ho_Image, HObject ho_OCRRectangle1, string ModelPath, HTuple PyramidLevel, HTuple LastPyramidLevel, HTuple MinScore, HTuple Greediness, HTuple hv_ThrsholdMin, HTuple hv_HysThresholdMin, HTuple hv_HysThesholdMax,
                                        HTuple hv_CloseRec, HTuple hv_ModelRegionAreaDownValue, HTuple hv_IsNotProdValue, HTuple hv_IsNotProdValueMin,
                                        HTuple hv_AutoThreshold, HTuple hv_DistancePP, string NeedOcr, bool IsIngoreCalu, int OcrLength, int judgeBarLength, string binPath, out HObject AllProdAndKongSortObj, out HObject ho_OutRegion, out HObject ho_OcrOutRegion, out HTuple AllProdAndKongSortName,
                                        out double OcrCenterRow, out double OcrCenterCol, out double OcrCenterPhi, out double distance, out int ICanGet, out int IExistProduct, out string firstOcr, out string bar, out string strLog, out bool bResult)
        {
            HOperatorSet.GenEmptyObj(out AllProdAndKongSortObj);
            HOperatorSet.GenEmptyObj(out ho_OutRegion);
            HOperatorSet.GenEmptyObj(out ho_OcrOutRegion);
            AllProdAndKongSortName = new HTuple();
            strLog = "";
            bResult = true;
            OcrCenterRow = 0;
            OcrCenterCol = 0;
            OcrCenterPhi = 0;
            ICanGet = 0;
            IExistProduct = 0;
            bar = "-9999";
            distance = 0;
            firstOcr = "";
            try
            {
                HTuple hv_ModelRegionTup = new HTuple();
                HOperatorSet.ReadTuple(ModelPath + "Rec2Model.tup", out hv_ModelRegionTup);

                HTuple hv_ModelRegionArea = hv_ModelRegionTup[3];
                HTuple hv_ModelRegionWidth = hv_ModelRegionTup[4];
                HTuple hv_ModelRegionHeight = hv_ModelRegionTup[5];
                HTuple hv_ScaleImageValue = 0.4;
                //输出：
                HTuple hv_DanGeStateName = new HTuple();
                HTuple hv_WeiJieProdName = new HTuple();
                HTuple hv_WeiJieReduceProdName = new HTuple();
                HTuple hv_Exception = new HTuple();

                HObject ho_DanGeProdRegion, ho_WeiJieProdRegion, ho_WeiJieReduceProdRegion;
                HOperatorSet.GenEmptyObj(out ho_DanGeProdRegion);
                HOperatorSet.GenEmptyObj(out ho_WeiJieProdRegion);
                HOperatorSet.GenEmptyObj(out ho_WeiJieReduceProdRegion);
                ho_DanGeProdRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DanGeProdRegion);
                ho_WeiJieProdRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_WeiJieProdRegion);
                ho_WeiJieReduceProdRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_WeiJieReduceProdRegion);
                HTuple hv_Error = 1;

                ho_DanGeProdRegion.Dispose(); ho_WeiJieProdRegion.Dispose(); ho_WeiJieReduceProdRegion.Dispose();
                GetAllProdZoomRegion(ho_Image, out ho_DanGeProdRegion, out ho_WeiJieProdRegion,
                     out ho_WeiJieReduceProdRegion, hv_ModelRegionArea, hv_ModelRegionWidth,
                     hv_ModelRegionHeight, hv_ScaleImageValue, hv_ThrsholdMin, hv_HysThresholdMin,
                     hv_HysThesholdMax, hv_CloseRec, hv_ModelRegionAreaDownValue, hv_IsNotProdValue, hv_IsNotProdValueMin, hv_AutoThreshold,
                     out hv_DanGeStateName, out hv_WeiJieProdName, out hv_WeiJieReduceProdName,
                     out hv_Exception, out hv_Error);

                //*******************综合所有产品****从上到下排序****中间空格的产品包含进来*********************
                //输入：
                //HTuple hv_DistancePP = tModel.DistancePP;
                //输出：
                HObject ho_AllProdAndKongSortObj;
                HOperatorSet.GenEmptyObj(out ho_AllProdAndKongSortObj);
                HTuple hv_AllProdAndKongSortName = new HTuple();
                hv_Error = 1;
                ho_AllProdAndKongSortObj.Dispose();
                SortAllProdAndKongRegion(ho_DanGeProdRegion, ho_WeiJieProdRegion, ho_WeiJieReduceProdRegion,
                    out ho_AllProdAndKongSortObj, hv_DanGeStateName, hv_WeiJieProdName, hv_WeiJieReduceProdName,
                    hv_DistancePP, out hv_AllProdAndKongSortName, out hv_Error, out hv_Exception);

                //*******************ZOOM图像中匹配模板位置，大图中截取OCR区域图像*********************
                //根据产品区域，定位OCR区域。
                //输入参数：  
                HObject ho_ModelRectangle, ho_OCRImagePart;
                HOperatorSet.GenEmptyObj(out ho_ModelRectangle);

                HOperatorSet.ReadRegion(out ho_ModelRectangle, ModelPath + "ModelRectangle.hobj");

                HTuple hv_ModelID = new HTuple();
                HTuple hv_NameLength = new HTuple();
                HTuple hv_ObjectsNumber = new HTuple();
                HTuple hv_Index9 = new HTuple();
                HTuple hv_ModelTup = new HTuple();
                HOperatorSet.ReadTuple(ModelPath + "ModelTup.tup", out hv_ModelTup);
                HOperatorSet.ReadShapeModel(ModelPath + "ModelID.shm", out hv_ModelID);
                HTuple hv_Rec2ModelRow = hv_ModelRegionTup[0];
                HTuple hv_Rec2ModelColumn = hv_ModelRegionTup[1];
                HTuple hv_Rec2ModelPhi = hv_ModelRegionTup[2];
                HTuple hv_FirstModelRow = hv_ModelTup[0];
                HTuple hv_FirstModelCol = hv_ModelTup[1];
                HTuple hv_FirstModelAngle = hv_ModelTup[2];

                HTuple hv_WriteImagePath = "E://OCR";
                string[] files = Directory.GetFiles(hv_WriteImagePath);
                for (int i = 0; i < files.Count(); i++)
                {
                    File.Delete(files[i]);
                }
                //输出：
                HOperatorSet.GenEmptyObj(out ho_OCRImagePart);
                ho_OCRImagePart.Dispose();
                hv_Error = 1;
                HTuple hv_writeImagei = 0;

                try
                {
                    HOperatorSet.TupleLength(hv_AllProdAndKongSortName, out hv_NameLength);
                    HOperatorSet.CountObj(ho_AllProdAndKongSortObj, out hv_ObjectsNumber);
                    if ((int)(new HTuple(hv_NameLength.TupleEqual(hv_ObjectsNumber))) != 0)
                    {
                        HTuple end_val108 = hv_NameLength - 1;
                        HTuple step_val108 = 1;
                        for (hv_Index9 = 0; hv_Index9.Continue(end_val108, step_val108); hv_Index9 = hv_Index9.TupleAdd(step_val108))
                        {
                            ho_OCRImagePart.Dispose();
                            GetOCRImagePartFromAll(ho_Image, ho_AllProdAndKongSortObj, ho_OCRRectangle1,
                                ho_ModelRectangle, ho_OCRRectangle1, out ho_OCRImagePart, hv_AllProdAndKongSortName,
                                hv_Index9, hv_ScaleImageValue, hv_ModelID, hv_Rec2ModelRow, hv_Rec2ModelColumn,
                                hv_Rec2ModelPhi, hv_FirstModelRow, hv_FirstModelCol, hv_FirstModelAngle,
                                PyramidLevel, LastPyramidLevel, MinScore, Greediness,
                                out hv_Error);
                            if (HDevWindowStack.IsOpen())
                            {
                                HOperatorSet.DispObj(ho_OCRImagePart, HDevWindowStack.GetActive());
                            }
                            //保存图像
                            HOperatorSet.WriteImage(ho_OCRImagePart, "png", 0, ((hv_WriteImagePath + "/") + hv_writeImagei) + ".png");
                            hv_writeImagei = hv_writeImagei + 1;
                        }
                    }
                    HOperatorSet.ClearShapeModel(hv_ModelID);
                    hv_Error = 0;
                }
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }

                ho_DanGeProdRegion.Dispose();
                ho_WeiJieProdRegion.Dispose();
                ho_WeiJieReduceProdRegion.Dispose();
                ho_OCRRectangle1.Dispose();
                ho_ModelRectangle.Dispose();
                ho_OCRImagePart.Dispose();

                AllProdAndKongSortObj = ho_AllProdAndKongSortObj;
                AllProdAndKongSortName = hv_AllProdAndKongSortName;

                int length = hv_AllProdAndKongSortName.Length;
                strLog = "";
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        strLog += hv_AllProdAndKongSortName[i].S + ",";
                    }
                    strLog = strLog.TrimEnd(',');
                }

                HObject ho_regionZoom;
                HOperatorSet.ZoomRegion(ho_AllProdAndKongSortObj, out ho_regionZoom, 1 / 0.4, 1 / 0.4);

                bResult = hv_AllProdAndKongSortName.Length > 0;
                ho_OutRegion = ho_regionZoom;

                if (bResult)
                {
                    List<string> listOcrValue = new List<string>();
                    string strOcrLog = "";
                    bool bGetOcr = LDSmallOcr(IsIngoreCalu, OcrLength, judgeBarLength, binPath, out bar, out strOcrLog, out listOcrValue);
                    strLog += strOcrLog;

                    bResult = bGetOcr;

                    if (listOcrValue.Count > 0)
                    {
                        firstOcr = listOcrValue.FirstOrDefault(x => x != "0");
                    }
                    else
                    {
                        firstOcr = "0000";
                    }

                    if (bGetOcr)
                    {
                        //屏蔽推算则取第一个OCR
                        if (IsIngoreCalu)
                        {
                            NeedOcr = firstOcr;
                        }
                        if (NeedOcr == null)
                        {
                            NeedOcr = "";
                        }

                        string strCenterLog = "";
                        bool bGetCenter = LDGetCenter(ho_AllProdAndKongSortObj, hv_AllProdAndKongSortName, listOcrValue, NeedOcr, out ho_OcrOutRegion,
                            out OcrCenterRow, out OcrCenterCol, out OcrCenterPhi, out distance, out ICanGet, out IExistProduct, out strCenterLog);

                        strLog += strCenterLog;

                        bResult = bGetCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                strLog = ex.Message;
            }
        }

        LHFOcr m_lHFOcr = new LHFOcr();
        /// <summary>
        /// 深度识别OCR
        /// </summary>
        /// <param name="IsIngoreCalu">是否屏蔽计算</param>
        /// <param name="strLog">Log显示</param>
        /// <param name="listOcrValue">输出OCR</param>
        public bool LDSmallOcr(bool IsIngoreCalu, int ocrLength, int judgeBarLength, string binPath, out string bar, out string strLog, out List<string> listOcrValue)
        {
            strLog = "";
            bar = "";
            listOcrValue = new List<string>();
            try
            {
                HKOcr.InitOCR(binPath);

                List<string> listOcr = new List<string>();
                var files = Directory.GetFiles("E://OCR", "*.jpg").Union(Directory.GetFiles("E://OCR", "*.png"));

                string strResult = "";
                foreach (var file in files)
                {
                    string ocr = HKOcr.OcrReadAction(file);
                    if (ocr.Length < ocrLength && !IsIngoreCalu)
                    {
                        ocr = "0";
                    }
                    int iOcr = 0;
                    bool bresult = int.TryParse(ocr, out iOcr);
                    if (!bresult)
                    {
                        ocr = "0";
                    }

                    listOcr.Add(ocr);
                    strResult += ocr.Trim() + ",";
                }
                int count = 10 - listOcr.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        listOcr.Add("-1");
                    }
                }

                strResult = strResult.TrimEnd(',');
                strLog += Environment.NewLine;
                strLog += "OCR Read:" + strResult;

                if (IsIngoreCalu)
                {
                    //根据ocr推算Bar条号
                    int length = ocrLength;
                    List<string> listNew = new List<string>();
                    foreach (var item in listOcr)
                    {
                        if (item.Length > length - 2)
                        {
                            listNew.Add(item.Substring(0, length - 2));
                        }
                    }

                    string[] arr = listNew.ToArray();

                    var res = from n in arr
                              group n by n into g
                              orderby g.Count() descending
                              select g;
                    // 分组中第一个组就是重复最多的
                    var gr = res.First();

                    if (gr.Count() < judgeBarLength)
                    {
                        bar = "-9999";
                    }
                    else
                    {
                        foreach (string x in gr)
                        {
                            bar = x;
                        }
                    }

                    strLog += Environment.NewLine;
                    strLog += ("Bar 结果:" + bar);
                }

                //推算OCR
                {
                    strResult = "";
                    //获取的OCR进行排序
                    int length = listOcr.Count();
                    m_lHFOcr.strOCR1 = length > 0 ? listOcr[0] : "-1";
                    m_lHFOcr.strOCR2 = length > 1 ? listOcr[1] : "-1";
                    m_lHFOcr.strOCR3 = length > 2 ? listOcr[2] : "-1";
                    m_lHFOcr.strOCR4 = length > 3 ? listOcr[3] : "-1";
                    m_lHFOcr.strOCR5 = length > 4 ? listOcr[4] : "-1";
                    m_lHFOcr.strOCR6 = length > 5 ? listOcr[5] : "-1";
                    m_lHFOcr.strOCR7 = length > 6 ? listOcr[6] : "-1";
                    m_lHFOcr.strOCR8 = length > 7 ? listOcr[7] : "-1";
                    m_lHFOcr.strOCR9 = length > 8 ? listOcr[8] : "-1";
                    m_lHFOcr.strOCR10 = length > 9 ? listOcr[9] : "-1";

                    List<int> vecNewDeduct = new List<int>();
                    m_lHFOcr.GetDeductionVect(ref vecNewDeduct);

                    listOcr.Clear();
                    foreach (var vecInt in vecNewDeduct)
                    {
                        string str = vecInt.ToString();
                        if (vecInt != -1)
                        {
                            if (str.Length > ocrLength)
                            {
                                str = vecInt.ToString().Substring(0, ocrLength);
                            }
                            else
                            {
                                str = vecInt.ToString().PadLeft(ocrLength, '0');
                            }
                        }
                        listOcr.Add(str);

                        strResult += str;
                        strResult += ",";
                    }
                    strLog += Environment.NewLine;
                    strLog += ("OCR 推算:" + strResult);
                }

                listOcrValue = listOcr;

                return listOcrValue.Count > 0;
            }
            catch (Exception ex)
            {
                strLog += ex.Message;
                return false;
            }
        }

        /// <summary>
        /// N面检测获取中心算法
        /// </summary>
        private AlgorithmResultModel NTestAlgorithm(HObject ho_Image)
        {
            AlgorithmResultModel resultModel = new AlgorithmResultModel();
            try
            {
                // Local iconic variables 
                HObject ho_GrayImage, ho_Region, ho_OutObj;
                HObject ho_RegionOpening, ho_RegionFillUp, ho_ConnectedRegions;
                HObject ho_SelectedRegions, ho_Cross;

                // Local control variables 

                HTuple hv_UsedThreshold = null, hv_Row = null;
                HTuple hv_Column = null, hv_Phi = null, hv_Length1 = null;
                HTuple hv_Length2 = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_GrayImage);
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_OutObj);

                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);

                ho_Region.Dispose();
                HOperatorSet.BinaryThreshold(ho_GrayImage, out ho_Region, "max_separability",
                    "light", out hv_UsedThreshold);

                ho_RegionOpening.Dispose();
                HOperatorSet.ClosingCircle(ho_Region, out ho_RegionOpening, 5.5);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionOpening, out ho_RegionFillUp);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                    70);

                HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_SelectedRegions, "rectangle2");

                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column,
                    out hv_Phi, out hv_Length1, out hv_Length2);

                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 100, hv_Phi);

                HOperatorSet.ConcatObj(ho_SelectedRegions, ho_Cross, out ho_OutObj);

                ho_GrayImage.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Cross.Dispose();

                resultModel.RunResult = hv_Row.Length > 0;

                HOperatorSet.TupleDeg(hv_Phi, out hv_Phi);

                resultModel.CenterRow = hv_Row;
                resultModel.CenterColumn = hv_Column;
                resultModel.CenterPhi = hv_Phi;
                resultModel.ObjectResult = ho_OutObj;

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                return resultModel;
            }

        }

        public bool LDGetCenter(HObject ho_AllProdAndKongSortObj, HTuple hv_AllProdAndKongSortName, List<string> listOcrValue, string strNeedOcr, out HObject ho_OutRegion,
                                        out double OcrCenterRow, out double OcrCenterCol, out double OcrCenterPhi, out double Distance, out int ICanGet, out int IExistProduct, out string strLog)
        {
            strLog = "";
            OcrCenterRow = 0;
            OcrCenterCol = 0;
            OcrCenterPhi = 0;
            ICanGet = 0;
            IExistProduct = 0;
            Distance = 0;
            HOperatorSet.GenEmptyObj(out ho_OutRegion);
            try
            {
                //获取每个的距离
                double distance = GetObjDistance(ho_AllProdAndKongSortObj);
                if (distance == 0 || distance < 420 || distance > 700)
                {
                    strLog = string.Format("间距超过限度:{0}", distance);
                    distance = 560;
                }
                Distance = distance;

                HTuple hv_ReadOcr = new HTuple();
                hv_ReadOcr[0] = 1025;
                hv_ReadOcr[1] = 1026;
                hv_ReadOcr[2] = 1027;
                hv_ReadOcr[3] = 2018;
                string[] value = listOcrValue.ToArray();
                hv_ReadOcr = value;
                HTuple hv_NeedOcr = strNeedOcr;
                HTuple hv_ScaleImageValue = 0.4;
                //输出：
                HTuple hv_OcrCenterRow = new HTuple();
                HTuple hv_OcrCenterColumn = new HTuple();
                HTuple hv_OcrCenterPhi = new HTuple();
                HTuple hv_OcrCenterLength1 = new HTuple();
                HTuple hv_OcrCenterLength2 = new HTuple();
                HObject ho_OcrCenterRectangle2, ho_AllProdRegionZoom;
                HOperatorSet.GenEmptyObj(out ho_OcrCenterRectangle2);
                HOperatorSet.GenEmptyObj(out ho_AllProdRegionZoom);
                HTuple hv_Error = 1;
                ho_AllProdRegionZoom.Dispose(); ho_OcrCenterRectangle2.Dispose();
                GetOcrRegionPos(ho_AllProdAndKongSortObj, out ho_AllProdRegionZoom, out ho_OcrCenterRectangle2,
                    hv_ReadOcr, hv_NeedOcr, hv_ScaleImageValue, out hv_OcrCenterRow, out hv_OcrCenterColumn,
                    out hv_OcrCenterPhi, out hv_OcrCenterLength1, out hv_OcrCenterLength2,
                    out hv_Error);

                bool bresult = hv_OcrCenterRow.Length > 0;
                if (bresult)
                {
                    OcrCenterRow = hv_OcrCenterRow;
                    OcrCenterCol = hv_OcrCenterColumn;
                    OcrCenterPhi = hv_OcrCenterPhi;

                    HTuple hv_Indices = new HTuple();
                    HTuple hv_Length = new HTuple();
                    HOperatorSet.TupleFind(hv_ReadOcr, hv_NeedOcr, out hv_Indices);
                    HOperatorSet.TupleLength(hv_Indices, out hv_Length);

                    HTuple hv_Index = new HTuple();
                    if (hv_Length > 0)
                    {
                        hv_Index = hv_Indices[0];
                    }
                    HTuple hv_ocrInfo = hv_AllProdAndKongSortName;
                    HTuple hv_ocr = hv_ocrInfo[hv_Index];
                    ICanGet = hv_ocr.S.Contains("是单个独立产品") ? 1 : 2;
                    IExistProduct = 1;
                }
                else
                {
                    OcrCenterRow = 0;
                    OcrCenterCol = 0;
                    OcrCenterPhi = 0;
                    IExistProduct = 0;
                    ICanGet = 0;
                }

                ho_AllProdRegionZoom.Dispose();

                ho_OutRegion = ho_OcrCenterRectangle2;

                return bresult;
            }
            catch (Exception ex)
            {
                strLog += ex.Message;
                return false;
            }
        }

        public double GetObjDistance(HObject ho_Object)
        {
            try
            {
                HObject ho_Region;
                HOperatorSet.GenEmptyObj(out ho_Region);
                HObject ho_SortRegions;
                HOperatorSet.GenEmptyObj(out ho_SortRegions);

                HTuple hv_ScaleImageValue = 0.4;

                HOperatorSet.ZoomRegion(ho_Object, out ho_Region,
                       1.0 / hv_ScaleImageValue, 1.0 / hv_ScaleImageValue);

                HOperatorSet.Connection(ho_Region, out ho_Region);

                HOperatorSet.SortRegion(ho_Region, out ho_SortRegions, "character", "true", "row");

                HTuple hv_Area, hv_Row, hv_Column;
                HOperatorSet.AreaCenter(ho_SortRegions, out hv_Area, out hv_Row, out hv_Column);

                if (hv_Row.Length < 2)
                {
                    return 0;
                }

                HTuple hv_Distance = new HTuple(), hv_Mean;
                for (int i = 1; i < hv_Row.Length; i++)
                {
                    hv_Distance.Append(hv_Row[i] - hv_Row[i - 1]);
                }

                HOperatorSet.TupleMean(hv_Distance, out hv_Mean);

                ho_Region.Dispose();
                ho_SortRegions.Dispose();

                return hv_Mean;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void GetOcrRegionPos(HObject ho_AllProdAndKongSortObj, out HObject ho_AllProdRegionZoom,
            out HObject ho_OcrCenterRectangle2, HTuple hv_ReadOcr, HTuple hv_NeedOcr, HTuple hv_ScaleImageValue,
            out HTuple hv_OcrCenterRow, out HTuple hv_OcrCenterColumn, out HTuple hv_OcrCenterPhi,
            out HTuple hv_OcrCenterLength1, out HTuple hv_OcrCenterLength2, out HTuple hv_Error)
        {
            HObject ho_NeedObjectSelected = null;

            // Local control variables 

            HTuple hv_Indices = new HTuple(), hv_Length = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_AllProdRegionZoom);
            HOperatorSet.GenEmptyObj(out ho_OcrCenterRectangle2);
            HOperatorSet.GenEmptyObj(out ho_NeedObjectSelected);
            hv_OcrCenterRow = new HTuple();
            hv_OcrCenterColumn = new HTuple();
            hv_OcrCenterPhi = new HTuple();
            hv_OcrCenterLength1 = new HTuple();
            hv_OcrCenterLength2 = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    HOperatorSet.TupleFind(hv_ReadOcr, hv_NeedOcr, out hv_Indices);
                    HOperatorSet.TupleLength(hv_Indices, out hv_Length);
                    ho_AllProdRegionZoom.Dispose();
                    HOperatorSet.ZoomRegion(ho_AllProdAndKongSortObj, out ho_AllProdRegionZoom,
                        1.0 / hv_ScaleImageValue, 1.0 / hv_ScaleImageValue);
                    if ((int)(new HTuple(hv_Length.TupleGreater(-1))) != 0)
                    {
                        ho_NeedObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_AllProdRegionZoom, out ho_NeedObjectSelected,
                            hv_Indices[0] + 1);
                        HOperatorSet.SmallestRectangle2(ho_NeedObjectSelected, out hv_OcrCenterRow,
                            out hv_OcrCenterColumn, out hv_OcrCenterPhi, out hv_OcrCenterLength1,
                            out hv_OcrCenterLength2);
                        ho_OcrCenterRectangle2.Dispose();
                        HOperatorSet.GenRectangle2(out ho_OcrCenterRectangle2, hv_OcrCenterRow,
                            hv_OcrCenterColumn, hv_OcrCenterPhi, hv_OcrCenterLength1, hv_OcrCenterLength2);

                    }
                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 0;
                }
                ho_NeedObjectSelected.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_NeedObjectSelected.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void GetOCRImagePartFromAll(HObject ho_Image1, HObject ho_AllProdObjectsSort,
                                         HObject ho_OCRRectangle1, HObject ho_ModelRectangle, HObject ho_BigOCRRectangle1,
                                         out HObject ho_OCRImagePart, HTuple hv_AllProdStateNmaeSort, HTuple hv_Index9,
                                         HTuple hv_ScaleImage, HTuple hv_ModelID, HTuple hv_Rec2ModelRow, HTuple hv_Rec2ModelColumn,
                                         HTuple hv_Rec2ModelPhi, HTuple hv_FirstModelRow, HTuple hv_FirstModelCol, HTuple hv_FirstModelAngle,
                                         double PyramidLevel, double LastPyramid, double minScore, double Greediness,
                                         out HTuple hv_Error)
        {
            // Local iconic variables 
            HObject ho_ImageZoomed2 = null, ho_RegionZoom5 = null;
            HObject ho_ProdSelected = null, ho_Rectangle1 = null, ho_RegionAffineTrans2 = null;
            HObject ho_RegionZoom3 = null, ho_ImageAffineTrans = null, ho_RegionAffineTrans3 = null;
            HObject ho_Rectangle2 = null, ho_RegionZoom4 = null, ho_ImageAffineTransToOrg = null;
            HObject ho_RegionDilation1 = null, ho_FindImageReduced = null;
            HObject ho_RegionAffineTrans1 = null, ho_RegionZoom1 = null;
            HObject ho_ModelContours = null, ho_ContoursAffineTrans1 = null;
            HObject ho_ContoursAffineTrans = null, ho_Region1 = null, ho_RegionZoom = null;
            HObject ho_ImageAffineTrans3 = null, ho_RegionAffineTrans = null;
            HObject ho_Region3 = null, ho_RegionZoom2 = null, ho_Image = null;
            HObject ho_ImageResult = null;

            // Local control variables 
            HTuple hv_SelectedName = new HTuple(), hv_EqualWeiJieProd = new HTuple();
            HTuple hv_EqualDanGeProd = new HTuple(), hv_EqualChunWeiJieProd = new HTuple();
            HTuple hv_EqualKongGe = new HTuple(), hv_Row5 = new HTuple();
            HTuple hv_Column5 = new HTuple(), hv_Phi2 = new HTuple();
            HTuple hv_Length12 = new HTuple(), hv_Length22 = new HTuple();
            HTuple hv_HomMat2D3 = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row6 = new HTuple(), hv_Column6 = new HTuple();
            HTuple hv_dAngle1 = new HTuple(), hv_HomMat2DIdentity3 = new HTuple();
            HTuple hv_HomMat2DRotate1 = new HTuple(), hv_Area4 = new HTuple();
            HTuple hv_Row10 = new HTuple(), hv_Column10 = new HTuple();
            HTuple hv_Row4 = new HTuple(), hv_Column4 = new HTuple();
            HTuple hv_Phi1 = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Row9 = new HTuple(), hv_Column9 = new HTuple();
            HTuple hv_HomMat2DIdentity4 = new HTuple(), hv_HomMat2DTranslate = new HTuple();
            HTuple hv_Row13 = new HTuple(), hv_Column13 = new HTuple();
            HTuple hv_Row23 = new HTuple(), hv_Column23 = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_HomMat2D2 = new HTuple(), hv_HomMat2DIdentity1 = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_Area2 = new HTuple();
            HTuple hv_Row7 = new HTuple(), hv_Column7 = new HTuple();
            HTuple hv_dAngle = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DRotate = new HTuple(), hv_Area3 = new HTuple();
            HTuple hv_Row8 = new HTuple(), hv_Column8 = new HTuple();
            HTuple hv_HomMat2DIdentity2 = new HTuple(), hv_HomMat2DTranslate1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OCRImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageZoomed2);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom5);
            HOperatorSet.GenEmptyObj(out ho_ProdSelected);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans2);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom3);
            HOperatorSet.GenEmptyObj(out ho_ImageAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans3);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom4);
            HOperatorSet.GenEmptyObj(out ho_ImageAffineTransToOrg);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation1);
            HOperatorSet.GenEmptyObj(out ho_FindImageReduced);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans1);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom1);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffineTrans1);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom);
            HOperatorSet.GenEmptyObj(out ho_ImageAffineTrans3);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_Region3);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom2);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            hv_Error = new HTuple();
            try
            {
                try
                {
                    HOperatorSet.SetSystem("border_shape_models", "true");
                    //单个产品区域循环OCR
                    ho_ImageZoomed2.Dispose();
                    HOperatorSet.ZoomImageFactor(ho_Image1, out ho_ImageZoomed2, hv_ScaleImage,
                        hv_ScaleImage, "nearest_neighbor");
                    ho_RegionZoom5.Dispose();
                    HOperatorSet.ZoomRegion(ho_OCRRectangle1, out ho_RegionZoom5, hv_ScaleImage,
                        hv_ScaleImage);
                    ho_ProdSelected.Dispose();
                    HOperatorSet.SelectObj(ho_AllProdObjectsSort, out ho_ProdSelected, hv_Index9 + 1);
                    HOperatorSet.TupleSelect(hv_AllProdStateNmaeSort, hv_Index9, out hv_SelectedName);
                    HOperatorSet.TupleEqual(hv_SelectedName, "复判是产品区域未解离", out hv_EqualWeiJieProd);
                    HOperatorSet.TupleEqual(hv_SelectedName, "是单个独立产品", out hv_EqualDanGeProd);
                    HOperatorSet.TupleEqual(hv_SelectedName, "未解离产品", out hv_EqualChunWeiJieProd);
                    HOperatorSet.TupleEqual(hv_SelectedName, "空隔产品", out hv_EqualKongGe);
                    if ((int)(hv_EqualWeiJieProd.TupleOr(hv_EqualChunWeiJieProd)) != 0)
                    {
                        //未解离开的产品不能用模板匹配
                        //首先旋转图像同模板图像角度一致
                        HOperatorSet.SmallestRectangle2(ho_ProdSelected, out hv_Row5, out hv_Column5,
                            out hv_Phi2, out hv_Length12, out hv_Length22);
                        ho_Rectangle1.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_Row5, hv_Column5, hv_Phi2,
                            hv_Length12, hv_Length22);
                        HOperatorSet.VectorAngleToRigid(hv_Rec2ModelRow, hv_Rec2ModelColumn, hv_Rec2ModelPhi,
                            hv_Row5, hv_Column5, hv_Phi2, out hv_HomMat2D3);
                        ho_RegionAffineTrans2.Dispose();
                        HOperatorSet.AffineTransRegion(ho_RegionZoom5, out ho_RegionAffineTrans2,
                            hv_HomMat2D3, "nearest_neighbor");
                        ho_RegionZoom3.Dispose();
                        HOperatorSet.ZoomRegion(ho_Rectangle1, out ho_RegionZoom3, 1.0 / hv_ScaleImage,
                            1.0 / hv_ScaleImage);
                        HOperatorSet.AreaCenter(ho_RegionZoom3, out hv_Area, out hv_Row6, out hv_Column6);
                        hv_dAngle1 = hv_Rec2ModelPhi - hv_Phi2;
                        HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity3);
                        HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity3, hv_dAngle1, hv_Column6,
                            hv_Row6, out hv_HomMat2DRotate1);
                        ho_ImageAffineTrans.Dispose();
                        HOperatorSet.AffineTransImage(ho_Image1, out ho_ImageAffineTrans, hv_HomMat2DRotate1,
                            "nearest_neighbor", "false");
                        ho_RegionAffineTrans3.Dispose();
                        HOperatorSet.AffineTransRegion(ho_RegionZoom3, out ho_RegionAffineTrans3,
                            hv_HomMat2DRotate1, "nearest_neighbor");
                        HOperatorSet.AreaCenter(ho_RegionAffineTrans3, out hv_Area4, out hv_Row10,
                            out hv_Column10);
                        //其次变换创建模板矩形到大图像
                        HOperatorSet.SmallestRectangle2(ho_ModelRectangle, out hv_Row4, out hv_Column4,
                            out hv_Phi1, out hv_Length11, out hv_Length21);
                        ho_Rectangle2.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_Row4, hv_Column4, hv_Phi1,
                            hv_Length11, hv_Length21);
                        ho_RegionZoom4.Dispose();
                        HOperatorSet.ZoomRegion(ho_Rectangle2, out ho_RegionZoom4, 1.0 / hv_ScaleImage,
                            1.0 / hv_ScaleImage);
                        HOperatorSet.AreaCenter(ho_RegionZoom4, out hv_Area1, out hv_Row9, out hv_Column9);
                        //将图像平移到模板图像位置
                        HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity4);
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity4, hv_Row9 - hv_Row10,
                            hv_Column9 - hv_Column10, out hv_HomMat2DTranslate);
                        ho_ImageAffineTransToOrg.Dispose();
                        HOperatorSet.AffineTransImage(ho_ImageAffineTrans, out ho_ImageAffineTransToOrg,
                            hv_HomMat2DTranslate, "nearest_neighbor", "false");
                        //裁剪OCR区域图像
                        HOperatorSet.SmallestRectangle1(ho_BigOCRRectangle1, out hv_Row13, out hv_Column13,
                            out hv_Row23, out hv_Column23);
                        ho_OCRImagePart.Dispose();
                        HOperatorSet.CropPart(ho_ImageAffineTransToOrg, out ho_OCRImagePart, hv_Row13,
                            hv_Column13, hv_Column23 - hv_Column13, hv_Row23 - hv_Row13);
                    }
                    if ((int)(hv_EqualDanGeProd) != 0)
                    {
                        //可以用模板匹配
                        ho_RegionDilation1.Dispose();
                        HOperatorSet.DilationRectangle1(ho_ProdSelected, out ho_RegionDilation1,
                            30, 30);
                        ho_FindImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageZoomed2, ho_RegionDilation1, out ho_FindImageReduced
                            );
                        //HOperatorSet.FindShapeModel(ho_FindImageReduced, hv_ModelID, -0.39, 0.79,
                        //    0.7, 0, 0, "least_squares", (new HTuple(5)).TupleConcat(-3), 0, out hv_Row,
                        //    out hv_Column, out hv_Angle, out hv_Score);
                        HOperatorSet.FindShapeModel(ho_FindImageReduced, hv_ModelID, -0.39, 0.79,
                            minScore, 0, 0, "least_squares", (new HTuple(PyramidLevel)).TupleConcat(LastPyramid), Greediness, out hv_Row,
                            out hv_Column, out hv_Angle, out hv_Score);
                        //dev_display_shape_matching_results(hv_ModelID, "red", hv_Row, hv_Column,
                        //    hv_Angle, 1, 1, 0);
                        if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleGreater(0))) != 0)
                        {
                            //匹配成功
                            HOperatorSet.VectorAngleToRigid(hv_FirstModelRow, hv_FirstModelCol, hv_FirstModelAngle,
                                hv_Row, hv_Column, hv_Angle, out hv_HomMat2D2);
                            ho_RegionAffineTrans1.Dispose();
                            HOperatorSet.AffineTransRegion(ho_RegionZoom5, out ho_RegionAffineTrans1,
                                hv_HomMat2D2, "nearest_neighbor");
                            ho_RegionZoom1.Dispose();
                            HOperatorSet.ZoomRegion(ho_RegionAffineTrans1, out ho_RegionZoom1, 1.0 / hv_ScaleImage,
                                1.0 / hv_ScaleImage);
                            //将缩小图像找到的模板区域返回到大图
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity1);
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_FirstModelRow, hv_FirstModelCol,
                                hv_FirstModelAngle, out hv_HomMat2D);
                            ho_ModelContours.Dispose();
                            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID,
                                1);
                            ho_ContoursAffineTrans1.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffineTrans1,
                                hv_HomMat2D);
                            ho_ContoursAffineTrans.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_ContoursAffineTrans1, out ho_ContoursAffineTrans,
                                hv_HomMat2D2);
                            ho_Region1.Dispose();
                            HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans, out ho_Region1,
                                "filled");
                            ho_RegionZoom.Dispose();
                            HOperatorSet.ZoomRegion(ho_Region1, out ho_RegionZoom, 1.0 / hv_ScaleImage,
                                1.0 / hv_ScaleImage);
                            HOperatorSet.AreaCenter(ho_RegionZoom, out hv_Area2, out hv_Row7, out hv_Column7);

                            //首先旋转图像
                            hv_dAngle = hv_FirstModelAngle - hv_Angle;
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                            HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_dAngle, hv_Row7,
                                hv_Column7, out hv_HomMat2DRotate);
                            ho_ImageAffineTrans3.Dispose();
                            HOperatorSet.AffineTransImage(ho_Image1, out ho_ImageAffineTrans3, hv_HomMat2DRotate,
                                "nearest_neighbor", "true");
                            ho_RegionAffineTrans.Dispose();
                            HOperatorSet.AffineTransRegion(ho_RegionZoom, out ho_RegionAffineTrans,
                                hv_HomMat2DRotate, "nearest_neighbor");
                            //将创建模板还原到大图
                            ho_Region3.Dispose();
                            HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans1, out ho_Region3,
                                "filled");
                            ho_RegionZoom2.Dispose();
                            HOperatorSet.ZoomRegion(ho_Region3, out ho_RegionZoom2, 1.0 / hv_ScaleImage,
                                1.0 / hv_ScaleImage);
                            HOperatorSet.AreaCenter(ho_RegionZoom2, out hv_Area3, out hv_Row8, out hv_Column8);
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity2);
                            HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity2, hv_Row8 - hv_Row7,
                                hv_Column8 - hv_Column7, out hv_HomMat2DTranslate1);
                            ho_ImageAffineTransToOrg.Dispose();
                            HOperatorSet.AffineTransImage(ho_ImageAffineTrans3, out ho_ImageAffineTransToOrg,
                                hv_HomMat2DTranslate1, "nearest_neighbor", "false");
                            HOperatorSet.SmallestRectangle1(ho_BigOCRRectangle1, out hv_Row13, out hv_Column13,
                                out hv_Row23, out hv_Column23);
                            ho_OCRImagePart.Dispose();
                            HOperatorSet.CropPart(ho_ImageAffineTransToOrg, out ho_OCRImagePart,
                                hv_Row13, hv_Column13, hv_Column23 - hv_Column13, hv_Row23 - hv_Row13);
                        }
                        else
                        {
                            //匹配失败，但是产品存在
                            HOperatorSet.SmallestRectangle1(ho_BigOCRRectangle1, out hv_Row13, out hv_Column13,
                                out hv_Row23, out hv_Column23);
                            ho_Image.Dispose();
                            HOperatorSet.GenImageConst(out ho_Image, "byte", hv_Column23 - hv_Column13,
                                hv_Row23 - hv_Row13);
                            ho_ImageResult.Dispose();
                            HOperatorSet.PaintRegion(ho_Image, ho_Image, out ho_ImageResult, 128,
                                "fill");
                            ho_OCRImagePart.Dispose();
                            HOperatorSet.CropPart(ho_ImageResult, out ho_OCRImagePart, 0, 0, hv_Column23 - hv_Column13,
                                hv_Row23 - hv_Row13);
                        }

                    }

                    if ((int)(hv_EqualKongGe) != 0)
                    {
                        HOperatorSet.SmallestRectangle1(ho_BigOCRRectangle1, out hv_Row13, out hv_Column13,
                            out hv_Row23, out hv_Column23);
                        ho_Image.Dispose();
                        HOperatorSet.GenImageConst(out ho_Image, "byte", hv_Column23 - hv_Column13,
                            hv_Row23 - hv_Row13);
                        ho_ImageResult.Dispose();
                        HOperatorSet.PaintRegion(ho_Image, ho_Image, out ho_ImageResult, 128, "fill");
                        ho_OCRImagePart.Dispose();
                        HOperatorSet.CropPart(ho_ImageResult, out ho_OCRImagePart, 0, 0, hv_Column23 - hv_Column13,
                            hv_Row23 - hv_Row13);
                    }

                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }

                ho_ImageZoomed2.Dispose();
                ho_RegionZoom5.Dispose();
                ho_ProdSelected.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionAffineTrans2.Dispose();
                ho_RegionZoom3.Dispose();
                ho_ImageAffineTrans.Dispose();
                ho_RegionAffineTrans3.Dispose();
                ho_Rectangle2.Dispose();
                ho_RegionZoom4.Dispose();
                ho_ImageAffineTransToOrg.Dispose();
                ho_RegionDilation1.Dispose();
                ho_FindImageReduced.Dispose();
                ho_RegionAffineTrans1.Dispose();
                ho_RegionZoom1.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffineTrans1.Dispose();
                ho_ContoursAffineTrans.Dispose();
                ho_Region1.Dispose();
                ho_RegionZoom.Dispose();
                ho_ImageAffineTrans3.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_Region3.Dispose();
                ho_RegionZoom2.Dispose();
                ho_Image.Dispose();
                ho_ImageResult.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoomed2.Dispose();
                ho_RegionZoom5.Dispose();
                ho_ProdSelected.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionAffineTrans2.Dispose();
                ho_RegionZoom3.Dispose();
                ho_ImageAffineTrans.Dispose();
                ho_RegionAffineTrans3.Dispose();
                ho_Rectangle2.Dispose();
                ho_RegionZoom4.Dispose();
                ho_ImageAffineTransToOrg.Dispose();
                ho_RegionDilation1.Dispose();
                ho_FindImageReduced.Dispose();
                ho_RegionAffineTrans1.Dispose();
                ho_RegionZoom1.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffineTrans1.Dispose();
                ho_ContoursAffineTrans.Dispose();
                ho_Region1.Dispose();
                ho_RegionZoom.Dispose();
                ho_ImageAffineTrans3.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_Region3.Dispose();
                ho_RegionZoom2.Dispose();
                ho_Image.Dispose();
                ho_ImageResult.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void SortAllProdAndKongRegion(HObject ho_DanGeProdRegion, HObject ho_WeiJieProdRegion,
                                         HObject ho_WeiJieReduceProdRegion, out HObject ho_AllProdAndKongSortObj, HTuple hv_DanGeStateName,
                                         HTuple hv_WeiJieProdName, HTuple hv_WeiJieReduceProdName, HTuple hv_DistancePP,
                                            out HTuple hv_AllProdAndKongSortName, out HTuple hv_Error, out HTuple hv_Exception)
        {
            hv_Exception = new HTuple();
            // Local iconic variables 
            HObject ho_AllProdObjectsSort = null;

            // Local control variables 

            HTuple hv_AllProdStateNmaeSort = new HTuple();
            HTuple hv_Error1 = new HTuple(), hv_Error2 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_AllProdAndKongSortObj);
            HOperatorSet.GenEmptyObj(out ho_AllProdObjectsSort);
            hv_AllProdAndKongSortName = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    ho_AllProdObjectsSort.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_AllProdObjectsSort);
                    hv_AllProdStateNmaeSort = new HTuple();

                    ho_AllProdObjectsSort.Dispose();
                    SortRegionAndTuple(ho_DanGeProdRegion, ho_WeiJieProdRegion, ho_WeiJieReduceProdRegion,
                        out ho_AllProdObjectsSort, hv_DanGeStateName, hv_WeiJieProdName, hv_WeiJieReduceProdName,
                        out hv_AllProdStateNmaeSort, out hv_Error1);
                    //判断是否有空格物料出现,作为推算使用,

                    ho_AllProdAndKongSortObj.Dispose();
                    GetAllProdKongSort(ho_AllProdObjectsSort, out ho_AllProdAndKongSortObj, hv_AllProdStateNmaeSort,
                        hv_DistancePP, out hv_AllProdAndKongSortName, out hv_Error2);
                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_AllProdObjectsSort.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_AllProdObjectsSort.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void SortRegionAndTuple(HObject ho_DanGeProdRegion, HObject ho_WeiJieProdRegion,
                                        HObject ho_WeiJieReduceProdRegion, out HObject ho_AllProdObjectsSortOut, HTuple hv_DanGeStateName,
                                        HTuple hv_WeiJieProdName, HTuple hv_WeiJieReduceProdName, out HTuple hv_AllProdStateNmaeSortOut,
                                        out HTuple hv_Error)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ObjectsConcat = null, ho_AllProdObjectsConcat = null;
            HObject ho_ObjectSelected6 = null;

            // Local control variables 

            HTuple hv_AllProdStateNmae = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row5 = new HTuple();
            HTuple hv_Column5 = new HTuple(), hv_Sorted = new HTuple();
            HTuple hv_Length4 = new HTuple(), hv_Index9 = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_Indices1 = new HTuple();
            HTuple hv_Selected1 = new HTuple(), hv_Number6 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_AllProdObjectsSortOut);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_AllProdObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected6);
            hv_Error = new HTuple();
            try
            {
                ho_AllProdObjectsSortOut.Dispose();
                HOperatorSet.GenEmptyObj(out ho_AllProdObjectsSortOut);
                hv_AllProdStateNmaeSortOut = new HTuple();
                try
                {
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_DanGeProdRegion, ho_WeiJieProdRegion, out ho_ObjectsConcat
                        );
                    ho_AllProdObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_WeiJieReduceProdRegion, out ho_AllProdObjectsConcat
                        );
                    hv_AllProdStateNmae = new HTuple();
                    hv_AllProdStateNmae = hv_AllProdStateNmae.TupleConcat(hv_DanGeStateName);
                    hv_AllProdStateNmae = hv_AllProdStateNmae.TupleConcat(hv_WeiJieProdName);
                    hv_AllProdStateNmae = hv_AllProdStateNmae.TupleConcat(hv_WeiJieReduceProdName);
                    HOperatorSet.AreaCenter(ho_AllProdObjectsConcat, out hv_Area, out hv_Row5,
                        out hv_Column5);
                    HOperatorSet.TupleSort(hv_Row5, out hv_Sorted);
                    HOperatorSet.TupleLength(hv_Sorted, out hv_Length4);
                    HTuple end_val9 = hv_Length4 - 1;
                    HTuple step_val9 = 1;
                    for (hv_Index9 = 0; hv_Index9.Continue(end_val9, step_val9); hv_Index9 = hv_Index9.TupleAdd(step_val9))
                    {
                        HOperatorSet.TupleSelect(hv_Sorted, hv_Index9, out hv_Selected);
                        HOperatorSet.TupleFind(hv_Row5, hv_Selected, out hv_Indices1);
                        ho_ObjectSelected6.Dispose();
                        HOperatorSet.SelectObj(ho_AllProdObjectsConcat, out ho_ObjectSelected6,
                            hv_Indices1 + 1);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_AllProdObjectsSortOut, ho_ObjectSelected6, out ExpTmpOutVar_0
                                );
                            ho_AllProdObjectsSortOut.Dispose();
                            ho_AllProdObjectsSortOut = ExpTmpOutVar_0;
                        }
                        HOperatorSet.TupleSelect(hv_AllProdStateNmae, hv_Indices1, out hv_Selected1);
                        hv_AllProdStateNmaeSortOut = hv_AllProdStateNmaeSortOut.TupleConcat(hv_Selected1);
                    }
                    HOperatorSet.CountObj(ho_AllProdObjectsSortOut, out hv_Number6);
                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ObjectsConcat.Dispose();
                ho_AllProdObjectsConcat.Dispose();
                ho_ObjectSelected6.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectsConcat.Dispose();
                ho_AllProdObjectsConcat.Dispose();
                ho_ObjectSelected6.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void GetAllProdKongSort(HObject ho_AllProdObjectsSort, out HObject ho_AllProdKongSortObjOut,
                                            HTuple hv_AllProdStateNmaeSort, HTuple hv_DistancePP, out HTuple hv_AllProdKongSortNameOut,
                                            out HTuple hv_Error)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_DanGeObjectSelected = null, ho_Cross = null;
            HObject ho_Cross1 = null, ho_RegionMoved = null;

            // Local control variables 

            HTuple hv_Number6 = new HTuple(), hv_AllProdArea = new HTuple();
            HTuple hv_AllProdRow = new HTuple(), hv_AllProdColumn = new HTuple();
            HTuple hv_Index5 = new HTuple(), hv_DanGeNameSelected = new HTuple();
            HTuple hv_SelectedR1 = new HTuple(), hv_SelectedC1 = new HTuple();
            HTuple hv_SelectedR2 = new HTuple(), hv_SelectedC2 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_DistanceRow = new HTuple();
            HTuple hv_Dst = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_Indices1 = new HTuple();
            HTuple hv_Indices4 = new HTuple(), hv_DstT = new HTuple();
            HTuple hv_Indices2 = new HTuple(), hv_Selected1 = new HTuple();
            HTuple hv_Indices3 = new HTuple(), hv_Round = new HTuple();
            HTuple hv_Index10 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_AllProdKongSortObjOut);
            HOperatorSet.GenEmptyObj(out ho_DanGeObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            hv_Error = new HTuple();
            try
            {
                ho_AllProdKongSortObjOut.Dispose();
                HOperatorSet.GenEmptyObj(out ho_AllProdKongSortObjOut);
                hv_AllProdKongSortNameOut = new HTuple();
                try
                {
                    HOperatorSet.CountObj(ho_AllProdObjectsSort, out hv_Number6);
                    HOperatorSet.AreaCenter(ho_AllProdObjectsSort, out hv_AllProdArea, out hv_AllProdRow,
                        out hv_AllProdColumn);
                    if ((int)((new HTuple(hv_Number6.TupleLess(2))).TupleAnd(new HTuple(hv_Number6.TupleGreater(
                        0)))) != 0)
                    {
                        hv_AllProdKongSortNameOut = hv_AllProdKongSortNameOut.TupleConcat(hv_AllProdStateNmaeSort);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_AllProdKongSortObjOut, ho_AllProdObjectsSort,
                                out ExpTmpOutVar_0);
                            ho_AllProdKongSortObjOut.Dispose();
                            ho_AllProdKongSortObjOut = ExpTmpOutVar_0;
                        }
                    }
                    HTuple end_val9 = hv_Number6 - 2;
                    HTuple step_val9 = 1;
                    for (hv_Index5 = 0; hv_Index5.Continue(end_val9, step_val9); hv_Index5 = hv_Index5.TupleAdd(step_val9))
                    {
                        ho_DanGeObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_AllProdObjectsSort, out ho_DanGeObjectSelected,
                            hv_Index5 + 1);
                        HOperatorSet.TupleSelect(hv_AllProdStateNmaeSort, hv_Index5, out hv_DanGeNameSelected);
                        HOperatorSet.TupleSelect(hv_AllProdRow, hv_Index5, out hv_SelectedR1);
                        HOperatorSet.TupleSelect(hv_AllProdColumn, hv_Index5, out hv_SelectedC1);
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_SelectedR1, hv_SelectedC1,
                            60, 0);
                        HOperatorSet.TupleSelect(hv_AllProdRow, hv_Index5 + 1, out hv_SelectedR2);
                        HOperatorSet.TupleSelect(hv_AllProdColumn, hv_Index5 + 1, out hv_SelectedC2);
                        ho_Cross1.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_SelectedR2, hv_SelectedC2,
                            60, 0);
                        HOperatorSet.DistancePp(hv_SelectedR1, hv_SelectedC1, hv_SelectedR2, hv_SelectedC2,
                            out hv_Distance);

                        hv_DistanceRow = ((hv_SelectedR2 - hv_SelectedR1)).TupleAbs();
                        hv_Dst = (1.0 * hv_DistanceRow) / hv_DistancePP;
                        //判断是否有多个未解离，需要增大Dst 2021-12-1
                        //未解离的在上面
                        HOperatorSet.TupleFind(hv_DanGeNameSelected, "未解离产品", out hv_Indices);
                        if ((int)(new HTuple(hv_Indices.TupleEqual(0))) != 0)
                        {
                            HOperatorSet.TupleSelect(hv_AllProdStateNmaeSort, hv_Index5 + 1, out hv_Selected);
                            HOperatorSet.TupleFind(hv_Selected, "是单个独立产品", out hv_Indices1);
                            HOperatorSet.TupleFind(hv_Selected, "未解离产品", out hv_Indices4);
                            if ((int)((new HTuple(hv_Indices1.TupleEqual(0))).TupleOr(new HTuple(hv_Indices4.TupleEqual(
                                0)))) != 0)
                            {
                                hv_DstT = 1.7;
                            }
                            else
                            {
                                hv_DstT = 1.55;
                            }
                        }
                        else
                        {
                            //未解离的在下面
                            HOperatorSet.TupleFind(hv_DanGeNameSelected, "是单个独立产品", out hv_Indices2);
                            if ((int)(new HTuple(hv_Indices2.TupleEqual(0))) != 0)
                            {
                                HOperatorSet.TupleSelect(hv_AllProdStateNmaeSort, hv_Index5 + 1, out hv_Selected1);
                                HOperatorSet.TupleFind(hv_Selected1, "未解离产品", out hv_Indices3);
                                if ((int)(new HTuple(hv_Indices3.TupleEqual(0))) != 0)
                                {
                                    hv_DstT = 1.7;
                                }
                                else
                                {
                                    hv_DstT = 1.55;
                                }
                            }
                            else
                            {
                                hv_DstT = 1.55;
                            }
                        }

                        HOperatorSet.TupleRound(hv_Dst, out hv_Round);
                        if ((int)(new HTuple(hv_Dst.TupleGreater(hv_DstT))) != 0)
                        {
                            //有1个以上的空格产品
                            //空隔前一个产品先包含进去
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_AllProdKongSortObjOut, ho_DanGeObjectSelected,
                                    out ExpTmpOutVar_0);
                                ho_AllProdKongSortObjOut.Dispose();
                                ho_AllProdKongSortObjOut = ExpTmpOutVar_0;
                            }
                            hv_AllProdKongSortNameOut = hv_AllProdKongSortNameOut.TupleConcat(hv_DanGeNameSelected);
                            HTuple end_val59 = hv_Round - 1;
                            HTuple step_val59 = 1;
                            for (hv_Index10 = 1; hv_Index10.Continue(end_val59, step_val59); hv_Index10 = hv_Index10.TupleAdd(step_val59))
                            {
                                ho_RegionMoved.Dispose();
                                HOperatorSet.MoveRegion(ho_DanGeObjectSelected, out ho_RegionMoved,
                                    hv_DistancePP * hv_Index10, 0);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_AllProdKongSortObjOut, ho_RegionMoved, out ExpTmpOutVar_0
                                        );
                                    ho_AllProdKongSortObjOut.Dispose();
                                    ho_AllProdKongSortObjOut = ExpTmpOutVar_0;
                                }
                                hv_AllProdKongSortNameOut = hv_AllProdKongSortNameOut.TupleConcat("空隔产品");

                            }
                        }
                        else
                        {
                            //没有空隔产品
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_AllProdKongSortObjOut, ho_DanGeObjectSelected,
                                    out ExpTmpOutVar_0);
                                ho_AllProdKongSortObjOut.Dispose();
                                ho_AllProdKongSortObjOut = ExpTmpOutVar_0;
                            }
                            hv_AllProdKongSortNameOut = hv_AllProdKongSortNameOut.TupleConcat(hv_DanGeNameSelected);
                        }
                        if ((int)(new HTuple(hv_Index5.TupleEqual(hv_Number6 - 2))) != 0)
                        {
                            ho_DanGeObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho_AllProdObjectsSort, out ho_DanGeObjectSelected,
                                hv_Index5 + 2);
                            HOperatorSet.TupleSelect(hv_AllProdStateNmaeSort, hv_Index5 + 1, out hv_DanGeNameSelected);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_AllProdKongSortObjOut, ho_DanGeObjectSelected,
                                    out ExpTmpOutVar_0);
                                ho_AllProdKongSortObjOut.Dispose();
                                ho_AllProdKongSortObjOut = ExpTmpOutVar_0;
                            }
                            hv_AllProdKongSortNameOut = hv_AllProdKongSortNameOut.TupleConcat(hv_DanGeNameSelected);
                        }
                    }

                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }

                ho_DanGeObjectSelected.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_RegionMoved.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DanGeObjectSelected.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_RegionMoved.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public void CheckTexture(HObject ho_ImageZoomed2, HObject ho_ObjectSelected1,
                                        out HObject ho_SelectedRegions7, HTuple hv_DynThr, HTuple hv_SelectWidth, out HTuple hv_Number7,
                                        out HTuple hv_Error)
        {




            // Local iconic variables 

            HObject ho_ImageReduced3 = null, ho_GrayImage = null;
            HObject ho_ImageEquHisto = null, ho_ImageMean = null, ho_ImageMedian1 = null;
            HObject ho_RegionDynThresh = null, ho_ConnectedRegions7 = null;
            HObject ho_SelectedRegions8 = null, ho_RegionUnion3 = null;
            HObject ho_RegionClosing1 = null, ho_ConnectedRegions6 = null;

            // Local control variables 

            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions7);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced3);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_ImageEquHisto);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian1);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions7);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions8);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion3);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions6);
            hv_Number7 = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {

                    ho_ImageReduced3.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageZoomed2, ho_ObjectSelected1, out ho_ImageReduced3
                        );
                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_ImageReduced3, out ho_GrayImage);
                    ho_ImageEquHisto.Dispose();
                    HOperatorSet.EquHistoImage(ho_GrayImage, out ho_ImageEquHisto);
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_ImageEquHisto, out ho_ImageMean, 5, 5);
                    ho_ImageMedian1.Dispose();
                    HOperatorSet.MedianRect(ho_ImageMean, out ho_ImageMedian1, 15, 20);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageMean, ho_ImageMedian1, out ho_RegionDynThresh,
                        hv_DynThr, "dark");
                    ho_ConnectedRegions7.Dispose();
                    HOperatorSet.Connection(ho_RegionDynThresh, out ho_ConnectedRegions7);
                    ho_SelectedRegions8.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions7, out ho_SelectedRegions8, "area",
                        "and", 200, 999999);
                    ho_RegionUnion3.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions8, out ho_RegionUnion3);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion3, out ho_RegionClosing1, 20,
                        1);
                    ho_ConnectedRegions6.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing1, out ho_ConnectedRegions6);
                    ho_SelectedRegions7.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions6, out ho_SelectedRegions7, "width",
                        "and", hv_SelectWidth, 99999);
                    HOperatorSet.CountObj(ho_SelectedRegions7, out hv_Number7);
                    hv_Error = 0;

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }



                ho_ImageReduced3.Dispose();
                ho_GrayImage.Dispose();
                ho_ImageEquHisto.Dispose();
                ho_ImageMean.Dispose();
                ho_ImageMedian1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ConnectedRegions7.Dispose();
                ho_SelectedRegions8.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions6.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced3.Dispose();
                ho_GrayImage.Dispose();
                ho_ImageEquHisto.Dispose();
                ho_ImageMean.Dispose();
                ho_ImageMedian1.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ConnectedRegions7.Dispose();
                ho_SelectedRegions8.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions6.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void PartitionRegionTransAngle(HObject ho_InPartitionRegion, HObject ho_ZImageH,
                                        out HObject ho_PartitionedRegionTrans, HTuple hv_ModelRegionWidth, HTuple hv_ModelRegionHeight,
                                        HTuple hv_AutoThreshold, out HTuple hv_Error)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ProductObject = null, ho_ObjectSelected = null;
            HObject ho_ImageReduced2 = null, ho_ImageMedian = null, ho_Regions = null;
            HObject ho_RegionFillUp = null, ho_SelectedRegions1 = null;
            HObject ho_RegionOpening3 = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions5 = null, ho_RegionClosing = null;
            HObject ho_ObjectSelected1 = null, ho_RegionAffineTrans3 = null;
            HObject ho_Partitioned2 = null, ho_PartitionedRegionTrans1 = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Number6 = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
            HTuple hv_HomMat2DIdentity3 = new HTuple(), hv_HomMat2DRotate1 = new HTuple();
            HTuple hv_HomMat2DInvert = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_PartitionedRegionTrans);
            HOperatorSet.GenEmptyObj(out ho_ProductObject);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening3);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions5);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans3);
            HOperatorSet.GenEmptyObj(out ho_Partitioned2);
            HOperatorSet.GenEmptyObj(out ho_PartitionedRegionTrans1);
            hv_Error = new HTuple();
            try
            {
                try
                {
                    //两端不是产品与产品有未解离 2021-11-20
                    ho_PartitionedRegionTrans.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_PartitionedRegionTrans);
                    HOperatorSet.CountObj(ho_InPartitionRegion, out hv_Number);
                    ho_ProductObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ProductObject);
                    HTuple end_val5 = hv_Number;
                    HTuple step_val5 = 1;
                    for (hv_Index1 = 1; hv_Index1.Continue(end_val5, step_val5); hv_Index1 = hv_Index1.TupleAdd(step_val5))
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_InPartitionRegion, out ho_ObjectSelected, hv_Index1);
                        ho_ImageReduced2.Dispose();
                        HOperatorSet.ReduceDomain(ho_ZImageH, ho_ObjectSelected, out ho_ImageReduced2
                            );
                        ho_ImageMedian.Dispose();
                        HOperatorSet.MedianRect(ho_ImageReduced2, out ho_ImageMedian, 8, 8);
                        ho_Regions.Dispose();
                        HOperatorSet.AutoThreshold(ho_ImageMedian, out ho_Regions, hv_AutoThreshold);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUpShape(ho_Regions, out ho_RegionFillUp, "area", 1, 5000);
                        ho_SelectedRegions1.Dispose();
                        HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions1, "area",
                            "and", 90000, 99999999);
                        ho_RegionOpening3.Dispose();
                        HOperatorSet.OpeningCircle(ho_SelectedRegions1, out ho_RegionOpening3,
                            10);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionOpening3, out ho_ConnectedRegions);
                        ho_SelectedRegions5.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions5,
                            "area", "and", 90000, 99999999);
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_SelectedRegions5, out ho_RegionClosing,
                            200, 200);
                        HOperatorSet.CountObj(ho_RegionClosing, out hv_Number6);
                        if ((int)(new HTuple(hv_Number6.TupleGreater(1))) != 0)
                        {
                            //两端 不是产品和 产品 有未解离
                            HTuple end_val19 = hv_Number6;
                            HTuple step_val19 = 1;
                            for (hv_Index = 1; hv_Index.Continue(end_val19, step_val19); hv_Index = hv_Index.TupleAdd(step_val19))
                            {
                                ho_ObjectSelected1.Dispose();
                                HOperatorSet.SelectObj(ho_RegionClosing, out ho_ObjectSelected1, hv_Index);
                                HOperatorSet.SmallestRectangle2(ho_ObjectSelected1, out hv_Row, out hv_Column,
                                    out hv_Phi, out hv_Length1, out hv_Length2);
                                if ((int)(new HTuple(((2 * hv_Length2)).TupleLess(200))) != 0)
                                {
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(ho_PartitionedRegionTrans, ho_ObjectSelected1,
                                            out ExpTmpOutVar_0);
                                        ho_PartitionedRegionTrans.Dispose();
                                        ho_PartitionedRegionTrans = ExpTmpOutVar_0;
                                    }
                                }
                            }

                        }
                        else
                        {
                            HOperatorSet.SmallestRectangle2(ho_InPartitionRegion, out hv_Row4, out hv_Column4,
                                out hv_Phi1, out hv_Length11, out hv_Length21);
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity3);
                            HOperatorSet.HomMat2dRotateLocal(hv_HomMat2DIdentity3, -hv_Phi1, out hv_HomMat2DRotate1);
                            ho_RegionAffineTrans3.Dispose();
                            HOperatorSet.AffineTransRegion(ho_InPartitionRegion, out ho_RegionAffineTrans3,
                                hv_HomMat2DRotate1, "nearest_neighbor");
                            ho_Partitioned2.Dispose();
                            HOperatorSet.PartitionRectangle(ho_RegionAffineTrans3, out ho_Partitioned2,
                                hv_ModelRegionWidth, hv_ModelRegionHeight);
                            HOperatorSet.HomMat2dInvert(hv_HomMat2DRotate1, out hv_HomMat2DInvert);
                            ho_PartitionedRegionTrans1.Dispose();
                            HOperatorSet.AffineTransRegion(ho_Partitioned2, out ho_PartitionedRegionTrans1,
                                hv_HomMat2DInvert, "nearest_neighbor");
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_PartitionedRegionTrans, ho_PartitionedRegionTrans1,
                                    out ExpTmpOutVar_0);
                                ho_PartitionedRegionTrans.Dispose();
                                ho_PartitionedRegionTrans = ExpTmpOutVar_0;
                            }

                        }
                    }


                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ProductObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionOpening3.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions5.Dispose();
                ho_RegionClosing.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionAffineTrans3.Dispose();
                ho_Partitioned2.Dispose();
                ho_PartitionedRegionTrans1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ProductObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionOpening3.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions5.Dispose();
                ho_RegionClosing.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionAffineTrans3.Dispose();
                ho_Partitioned2.Dispose();
                ho_PartitionedRegionTrans1.Dispose();

                throw HDevExpDefaultException;
            }
        }



        public void PCheck(HObject ho_Image, HObject ho_Rectangle, HObject ho_ModelProdObject,
                                out HObject ho_BengQueRegions, out HObject ho_ZangWuRegions, out HObject ho_SeChaRegions,
                                out HObject ho_ProdObject, HTuple hv_DynThr, HTuple hv_HysThrMin, HTuple hv_HysThrMax,
                                HTuple hv_DarkThrMin, HTuple hv_LightThrMax, HTuple hv_BenQueAreaMin, HTuple hv_BQwidth,
                                HTuple hv_BQheight, HTuple hv_ZangWuAreaMin, HTuple hv_ModelProdRegionPosRCA,
                                out HTuple hv_IsProd, out HTuple hv_Exception)
        {
            // Local iconic variables 

            HObject ho_GrayImage = null, ho_TModelProdObject = null;
            HObject ho_RegionDifference = null, ho_RegionDifference1 = null;
            HObject ho_ObjectsConcat = null, ho_RegionUnion = null, ho_RegionOpening = null;
            HObject ho_ImageReduced1 = null, ho_Region2 = null, ho_RegionClosing2 = null;
            HObject ho_RegionFillUp = null, ho_RegionOpening3 = null, ho_BengQueRegions1 = null;
            HObject ho_RegionTrans1 = null, ho_RegionTrans = null, ho_RegionErosion = null;
            HObject ho_ImageReduced = null, ho_ImageSMedian = null, ho_Region = null;
            HObject ho_RegionHysteresis = null, ho_ObjectsConcat1 = null;
            HObject ho_RegionUnion1 = null, ho_RegionClosing = null, ho_RegionOpening1 = null;
            HObject ho_ConnectedRegions1 = null, ho_ImageMedian = null;
            HObject ho_Region1 = null, ho_RegionClosing1 = null, ho_RegionOpening2 = null;
            HObject ho_SelectedRegions = null;

            // Local control variables 

            HTuple hv_ProdRegionPosRCA = new HTuple();
            HTuple hv_Length = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_AbsoluteHisto = new HTuple(), hv_RelativeHisto = new HTuple();
            HTuple hv_MinThresh = new HTuple(), hv_MaxThresh = new HTuple();
            HTuple hv_Number = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_BengQueRegions);
            HOperatorSet.GenEmptyObj(out ho_ZangWuRegions);
            HOperatorSet.GenEmptyObj(out ho_SeChaRegions);
            HOperatorSet.GenEmptyObj(out ho_ProdObject);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_TModelProdObject);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening3);
            HOperatorSet.GenEmptyObj(out ho_BengQueRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans1);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageSMedian);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            hv_IsProd = new HTuple();
            hv_Exception = new HTuple();
            try
            {
                try
                {
                    hv_IsProd = 1;
                    ho_BengQueRegions.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_BengQueRegions);
                    ho_ZangWuRegions.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ZangWuRegions);
                    ho_SeChaRegions.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SeChaRegions);
                    ho_ProdObject.Dispose();
                    GetProdRegion(ho_Image, ho_Rectangle, out ho_ProdObject, hv_DynThr, hv_HysThrMin,
                        hv_HysThrMax, out hv_ProdRegionPosRCA, out hv_IsProd, out hv_Exception);

                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                    HOperatorSet.TupleLength(hv_ProdRegionPosRCA, out hv_Length);
                    if ((int)(new HTuple(hv_Length.TupleNotEqual(3))) != 0)
                    {
                        //没有找到产品
                        ho_GrayImage.Dispose();
                        ho_TModelProdObject.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_RegionDifference1.Dispose();
                        ho_ObjectsConcat.Dispose();
                        ho_RegionUnion.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_Region2.Dispose();
                        ho_RegionClosing2.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening3.Dispose();
                        ho_BengQueRegions1.Dispose();
                        ho_RegionTrans1.Dispose();
                        ho_RegionTrans.Dispose();
                        ho_RegionErosion.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_ImageSMedian.Dispose();
                        ho_Region.Dispose();
                        ho_RegionHysteresis.Dispose();
                        ho_ObjectsConcat1.Dispose();
                        ho_RegionUnion1.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionOpening1.Dispose();
                        ho_ConnectedRegions1.Dispose();
                        ho_ImageMedian.Dispose();
                        ho_Region1.Dispose();
                        ho_RegionClosing1.Dispose();
                        ho_RegionOpening2.Dispose();
                        ho_SelectedRegions.Dispose();

                        return;
                    }
                    HOperatorSet.VectorAngleToRigid(hv_ModelProdRegionPosRCA.TupleSelect(0),
                        hv_ModelProdRegionPosRCA.TupleSelect(1), hv_ModelProdRegionPosRCA.TupleSelect(
                        2), hv_ProdRegionPosRCA.TupleSelect(0), hv_ProdRegionPosRCA.TupleSelect(
                        1), hv_ProdRegionPosRCA.TupleSelect(2), out hv_HomMat2D);
                    ho_TModelProdObject.Dispose();
                    HOperatorSet.AffineTransRegion(ho_ModelProdObject, out ho_TModelProdObject,
                        hv_HomMat2D, "nearest_neighbor");
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_TModelProdObject, ho_ProdObject, out ho_RegionDifference
                        );
                    ho_RegionDifference1.Dispose();
                    HOperatorSet.Difference(ho_ProdObject, ho_TModelProdObject, out ho_RegionDifference1
                        );
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionDifference, ho_RegionDifference1, out ho_ObjectsConcat
                        );
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionUnion, out ho_RegionOpening, 10.5);
                    //需要里面去除白色部分
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionOpening, out ho_ImageReduced1
                        );
                    ho_Region2.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 0, hv_DarkThrMin);
                    ho_RegionClosing2.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region2, out ho_RegionClosing2, 5.5);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing2, out ho_RegionFillUp);
                    ho_RegionOpening3.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening3, 5.5);
                    ho_BengQueRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_RegionOpening3, out ho_BengQueRegions1, "area",
                        "and", hv_BenQueAreaMin, 99999999);
                    ho_RegionTrans1.Dispose();
                    HOperatorSet.ShapeTrans(ho_BengQueRegions1, out ho_RegionTrans1, "rectangle2");
                    ho_BengQueRegions.Dispose();
                    HOperatorSet.SelectShape(ho_BengQueRegions1, out ho_BengQueRegions, (new HTuple("rect2_len1")).TupleConcat(
                        "rect2_len2"), "and", hv_BQwidth.TupleConcat(hv_BQheight), (new HTuple(99999)).TupleConcat(
                        99999));

                    //拟合的矩形区域内是否有黑色或者高亮缺陷
                    ho_RegionTrans.Dispose();
                    HOperatorSet.ShapeTrans(ho_ProdObject, out ho_RegionTrans, "rectangle2");
                    ho_RegionErosion.Dispose();
                    HOperatorSet.ErosionRectangle1(ho_RegionTrans, out ho_RegionErosion, 20,
                        20);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionErosion, out ho_ImageReduced
                        );
                    ho_ImageSMedian.Dispose();
                    HOperatorSet.MedianSeparate(ho_ImageReduced, out ho_ImageSMedian, 10, 10,
                        "mirrored");
                    ho_Region.Dispose();
                    HOperatorSet.FastThreshold(ho_ImageSMedian, out ho_Region, 0, hv_DarkThrMin,
                        100);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_ImageSMedian, out ho_RegionHysteresis,
                        hv_LightThrMax - 40, hv_LightThrMax, 100);
                    ho_ObjectsConcat1.Dispose();
                    HOperatorSet.ConcatObj(ho_Region, ho_RegionHysteresis, out ho_ObjectsConcat1
                        );
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat1, out ho_RegionUnion1);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionUnion1, out ho_RegionClosing, 13.5);
                    ho_RegionOpening1.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening1, 5.5);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening1, out ho_ConnectedRegions1);
                    ho_ZangWuRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_ZangWuRegions, "area",
                        "and", hv_ZangWuAreaMin, 9999999999);
                    //两端颜色不均匀
                    ho_ImageMedian.Dispose();
                    HOperatorSet.MedianImage(ho_ImageSMedian, out ho_ImageMedian, "circle", 10,
                        "mirrored");
                    HOperatorSet.GrayHisto(ho_ImageMedian, ho_ImageMedian, out hv_AbsoluteHisto,
                        out hv_RelativeHisto);
                    HOperatorSet.HistoToThresh(hv_AbsoluteHisto, 5, out hv_MinThresh, out hv_MaxThresh);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageMedian, out ho_Region1, hv_MinThresh, hv_MaxThresh);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region1, out ho_RegionClosing1, 13.5);
                    ho_RegionOpening2.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionClosing1, out ho_RegionOpening2,
                        30, 30);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_RegionOpening2, out ho_SelectedRegions, "area",
                        "and", 50000, 9999999999);
                    HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleGreater(1))) != 0)
                    {
                        ho_SeChaRegions.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedRegions, out ho_SeChaRegions, 1, 2);
                    }
                    else
                    {
                        ho_SeChaRegions.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_SeChaRegions);
                    }

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);

                }
                ho_GrayImage.Dispose();
                ho_TModelProdObject.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_RegionClosing2.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening3.Dispose();
                ho_BengQueRegions1.Dispose();
                ho_RegionTrans1.Dispose();
                ho_RegionTrans.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageSMedian.Dispose();
                ho_Region.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_ImageMedian.Dispose();
                ho_Region1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_RegionOpening2.Dispose();
                ho_SelectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_GrayImage.Dispose();
                ho_TModelProdObject.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_RegionClosing2.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening3.Dispose();
                ho_BengQueRegions1.Dispose();
                ho_RegionTrans1.Dispose();
                ho_RegionTrans.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageSMedian.Dispose();
                ho_Region.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_ImageMedian.Dispose();
                ho_Region1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_RegionOpening2.Dispose();
                ho_SelectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void GetProdRegion(HObject ho_Image, HObject ho_Rectangle, out HObject ho_ProdObject,
                                 HTuple hv_DynThr, HTuple hv_HysThrMin, HTuple hv_HysThrMax, out HTuple hv_ProdRegionPosRCA,
                                 out HTuple hv_IsProd, out HTuple hv_Exception)
        {
            // Local iconic variables 

            HObject ho_GrayImage = null, ho_ImageReduced = null;
            HObject ho_ImageMean = null, ho_RegionDynThresh = null, ho_ImageMedian = null;
            HObject ho_ImageMean1 = null, ho_RegionHysteresis = null, ho_ObjectsConcat = null;
            HObject ho_RegionUnion = null, ho_RegionFillUp = null, ho_RegionClosing = null;
            HObject ho_RegionFillUp1 = null, ho_RegionOpening = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_Rectangle1 = null;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_ProdWidth = new HTuple();
            HTuple hv_ProdHeight = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ProdObject);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            hv_ProdRegionPosRCA = new HTuple();
            hv_IsProd = new HTuple();
            try
            {
                try
                {
                    ho_ProdObject = null;
                    hv_ProdRegionPosRCA = null;
                    hv_IsProd = null;
                    hv_Exception = null;
                    //ho_ProdObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ProdObject);
                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced
                        );
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, 200, 200);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_RegionDynThresh,
                        hv_DynThr, "light");

                    ho_ImageMedian.Dispose();
                    HOperatorSet.MedianRect(ho_ImageReduced, out ho_ImageMedian, 15, 15);
                    ho_ImageMean1.Dispose();
                    HOperatorSet.MeanImage(ho_ImageMedian, out ho_ImageMean1, 9, 9);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_ImageMean1, out ho_RegionHysteresis,
                        hv_HysThrMin, hv_HysThrMax, 200);

                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionHysteresis, ho_RegionDynThresh, out ho_ObjectsConcat
                        );
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionUnion, out ho_RegionFillUp);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionFillUp, out ho_RegionClosing, 10,
                        10);
                    ho_RegionFillUp1.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp1);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp1, out ho_RegionOpening, 50,
                        50);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
                        "max_area", 70);
                    HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_Row, hv_Column, hv_Phi,
                        hv_Length1, hv_Length2);
                    //判断产品长度
                    hv_ProdWidth = 2 * hv_Length1;
                    hv_ProdHeight = 2 * hv_Length2;
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_ProdWidth.TupleGreater(3000))).TupleAnd(
                        new HTuple(hv_ProdWidth.TupleLess(5000))))).TupleAnd(new HTuple(hv_ProdHeight.TupleGreater(
                        250))))).TupleAnd(new HTuple(hv_ProdHeight.TupleLess(450)))) != 0)
                    {
                        ho_ProdObject.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedRegions, out ho_ProdObject, 1, 1);
                        hv_ProdRegionPosRCA = new HTuple();
                        hv_ProdRegionPosRCA = hv_ProdRegionPosRCA.TupleConcat(hv_Row);
                        hv_ProdRegionPosRCA = hv_ProdRegionPosRCA.TupleConcat(hv_Column);
                        hv_ProdRegionPosRCA = hv_ProdRegionPosRCA.TupleConcat(hv_Phi);
                        hv_IsProd = 1;
                    }
                    else if ((int)((new HTuple((new HTuple(hv_ProdWidth.TupleGreater(
                        3000))).TupleAnd(new HTuple(hv_ProdWidth.TupleLess(5000))))).TupleAnd(
                        new HTuple(hv_ProdHeight.TupleGreater(400)))) != 0)
                    {
                        ho_ProdObject.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_ProdObject);
                        hv_ProdRegionPosRCA = new HTuple();
                        hv_IsProd = 2;
                    }
                    else
                    {
                        ho_ProdObject.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_ProdObject);
                        hv_ProdRegionPosRCA = new HTuple();
                        hv_IsProd = 0;
                    }

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);

                }
                ho_GrayImage.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ImageMedian.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Rectangle1.Dispose();


            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_GrayImage.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ImageMedian.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Rectangle1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void NFaceCheck(HObject ho_Image, HObject ho_NModelRegion, out HObject ho_SubBenQueObject,
                                out HObject ho_CeXiObject, out HObject ho_LieWenObject, out HObject ho_BigDarkObject,
                                HTuple hv_NModelPos, HTuple hv_MaskMean, HTuple hv_DynThreshold, HTuple hv_CloseWidth,
                                HTuple hv_CloseHeight, HTuple hv_BQAreaMin, HTuple hv_BQWidthHeight, HTuple hv_DiffValue,
                                HTuple hv_LieWenNum, HTuple hv_stdWH, HTuple hv_HysthrMin, HTuple hv_HysthrMax,
                                HTuple hv_DustAreaMin, HTuple hv_DustWidth, HTuple hv_DustHeight, out HTuple hv_IsProd,
                                out HTuple hv_Error)
        {
            // Local iconic variables 

            HObject ho_GrayImage = null, ho_Image1 = null;
            HObject ho_Image2 = null, ho_Image3 = null, ho_ImageResult1 = null;
            HObject ho_ImageResult2 = null, ho_ImageResult3 = null, ho_RegionAffineTrans = null;

            // Local control variables 

            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SubBenQueObject);
            HOperatorSet.GenEmptyObj(out ho_CeXiObject);
            HOperatorSet.GenEmptyObj(out ho_LieWenObject);
            HOperatorSet.GenEmptyObj(out ho_BigDarkObject);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_Image3);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult3);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            hv_IsProd = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                    ho_Image1.Dispose(); ho_Image2.Dispose(); ho_Image3.Dispose();
                    HOperatorSet.Decompose3(ho_Image, out ho_Image1, out ho_Image2, out ho_Image3
                        );
                    ho_ImageResult1.Dispose(); ho_ImageResult2.Dispose(); ho_ImageResult3.Dispose();
                    HOperatorSet.TransFromRgb(ho_Image1, ho_Image2, ho_Image3, out ho_ImageResult1,
                        out ho_ImageResult2, out ho_ImageResult3, "hsi");

                    //********************崩缺断料、侧吸产品 检测*********************
                    ho_SubBenQueObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SubBenQueObject);
                    ho_CeXiObject.Dispose(); ho_SubBenQueObject.Dispose(); ho_RegionAffineTrans.Dispose();
                    CheckNBengQue_COPY_1(ho_GrayImage, ho_NModelRegion, out ho_CeXiObject, out ho_SubBenQueObject,
                        out ho_RegionAffineTrans, hv_MaskMean, hv_DynThreshold, hv_CloseWidth,
                        hv_CloseHeight, hv_NModelPos, hv_BQAreaMin, hv_BQWidthHeight, out hv_IsProd,
                        out hv_Exception);

                    //********************裂纹检测************************* 观象智能

                    ho_LieWenObject.Dispose();
                    CheckLieWen(ho_GrayImage, out ho_LieWenObject, hv_DiffValue, hv_HysthrMin,
                        hv_HysthrMax, hv_stdWH, hv_LieWenNum, out hv_Exception);
                    //********************产品区域大缺陷*************************

                    ho_BigDarkObject.Dispose();
                    DarkRegionCheck(ho_GrayImage, out ho_BigDarkObject, hv_DustAreaMin, hv_DustWidth,
                        hv_DustHeight, out hv_Exception);
                    hv_Error = 0;

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;

                }
                ho_GrayImage.Dispose();
                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult3.Dispose();
                ho_RegionAffineTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_GrayImage.Dispose();
                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImageResult3.Dispose();
                ho_RegionAffineTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }
         
        public void CheckNBengQue_COPY_1(HObject ho_GrayImage, HObject ho_NModelRegion,
                                out HObject ho_CeXiObject, out HObject ho_SubBenQueObject, out HObject ho_RegionAffineTrans,
                                HTuple hv_MaskMean, HTuple hv_DynThreshold, HTuple hv_CloseWidth, HTuple hv_CloseHeight,
                                HTuple hv_NModelPos, HTuple hv_BQAreaMin, HTuple hv_BQWidthHeight, out HTuple hv_IsProd,
                                out HTuple hv_Exception)
        {




            // Local iconic variables 

            HObject ho_ImageClosing = null, ho_ImageMean = null;
            HObject ho_RegionDynThresh = null, ho_RegionClosing = null;
            HObject ho_RegionFillUp = null, ho_RegionOpening = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_RegionTrans = null, ho_RegionDifference2 = null;
            HObject ho_RegionOpening2 = null, ho_ConnectedRegions2 = null;
            HObject ho_SelectedRegions1 = null, ho_Rectangle = null, ho_RegionDifference = null;
            HObject ho_RegionDifference1 = null, ho_ObjectsConcat = null;
            HObject ho_ObjectsConcat1 = null, ho_RegionUnion = null, ho_RegionOpening1 = null;
            HObject ho_ConnectedRegions1 = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Phi1 = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_Length = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_HomMat2D = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_CeXiObject);
            HOperatorSet.GenEmptyObj(out ho_SubBenQueObject);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ImageClosing);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference2);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            hv_IsProd = new HTuple();
            hv_Exception = new HTuple();
            try
            {
                try
                {
                    hv_IsProd = 1;
                    ho_CeXiObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_CeXiObject);
                    ho_SubBenQueObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SubBenQueObject);
                    ho_ImageClosing.Dispose();
                    HOperatorSet.GrayClosingRect(ho_GrayImage, out ho_ImageClosing, 20, 20);
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_ImageClosing, out ho_ImageMean, hv_MaskMean, hv_MaskMean);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageClosing, ho_ImageMean, out ho_RegionDynThresh,
                        hv_DynThreshold, "light");
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionDynThresh, out ho_RegionClosing,
                        hv_CloseWidth, hv_CloseHeight);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, 50,
                        50);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
                        "max_area", 70);
                    //判断是否有吸上产品
                    ho_RegionTrans.Dispose();
                    HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_RegionTrans, "convex");
                    //拟合矩形与实际产品区域的差异
                    ho_RegionDifference2.Dispose();
                    HOperatorSet.Difference(ho_RegionTrans, ho_SelectedRegions, out ho_RegionDifference2
                        );
                    ho_RegionOpening2.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionDifference2, out ho_RegionOpening2, 10.5);
                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening2, out ho_ConnectedRegions2);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions1, "area",
                        "and", 20000, 9999999999999);

                    HOperatorSet.SmallestRectangle2(ho_RegionTrans, out hv_Row1, out hv_Column1,
                        out hv_Phi1, out hv_Length11, out hv_Length21);
                    HOperatorSet.TupleLength(hv_Length11, out hv_Length);
                    if ((int)(new HTuple(hv_Length.TupleGreater(0))) != 0)
                    {
                        if ((int)((new HTuple(hv_Length11.TupleLess(500))).TupleAnd(new HTuple(hv_Length21.TupleLess(
                            50)))) != 0)
                        {
                            //没有物料
                            hv_IsProd = 0;
                            ho_ImageClosing.Dispose();
                            ho_ImageMean.Dispose();
                            ho_RegionDynThresh.Dispose();
                            ho_RegionClosing.Dispose();
                            ho_RegionFillUp.Dispose();
                            ho_RegionOpening.Dispose();
                            ho_ConnectedRegions.Dispose();
                            ho_SelectedRegions.Dispose();
                            ho_RegionTrans.Dispose();
                            ho_RegionDifference2.Dispose();
                            ho_RegionOpening2.Dispose();
                            ho_ConnectedRegions2.Dispose();
                            ho_SelectedRegions1.Dispose();
                            ho_Rectangle.Dispose();
                            ho_RegionDifference.Dispose();
                            ho_RegionDifference1.Dispose();
                            ho_ObjectsConcat.Dispose();
                            ho_ObjectsConcat1.Dispose();
                            ho_RegionUnion.Dispose();
                            ho_RegionOpening1.Dispose();
                            ho_ConnectedRegions1.Dispose();

                            return;
                        }

                    }


                    HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                        hv_Length2);
                    //判断是否侧吸，是侧吸需要人工介入。
                    if ((int)(new HTuple(hv_Length2.TupleLess(90))) != 0)
                    {
                        ho_CeXiObject.Dispose();
                        HOperatorSet.GenRectangle2(out ho_CeXiObject, hv_Row, hv_Column, hv_Phi,
                            hv_Length1, hv_Length2);
                    }
                    else
                    {
                        ho_CeXiObject.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_CeXiObject);
                    }

                    HOperatorSet.VectorAngleToRigid(hv_NModelPos.TupleSelect(0), hv_NModelPos.TupleSelect(
                        1), hv_NModelPos.TupleSelect(2), hv_Row, hv_Column, hv_Phi, out hv_HomMat2D);
                    ho_RegionAffineTrans.Dispose();
                    HOperatorSet.AffineTransRegion(ho_NModelRegion, out ho_RegionAffineTrans,
                        hv_HomMat2D, "nearest_neighbor");
                    //SUB缺陷检测
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionAffineTrans, ho_Rectangle, out ho_RegionDifference
                        );
                    ho_RegionDifference1.Dispose();
                    HOperatorSet.Difference(ho_Rectangle, ho_RegionAffineTrans, out ho_RegionDifference1
                        );
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionDifference, ho_RegionDifference1, out ho_ObjectsConcat
                        );
                    ho_ObjectsConcat1.Dispose();
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_SelectedRegions1, out ho_ObjectsConcat1
                        );

                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat1, out ho_RegionUnion);
                    ho_RegionOpening1.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionUnion, out ho_RegionOpening1, 10.5);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening1, out ho_ConnectedRegions1);
                    ho_SubBenQueObject.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SubBenQueObject, ((new HTuple("area")).TupleConcat(
                        "width")).TupleConcat("height"), "and", ((hv_BQAreaMin.TupleConcat(hv_BQWidthHeight))).TupleConcat(
                        hv_BQWidthHeight), ((new HTuple(999999999)).TupleConcat(99999)).TupleConcat(
                        99999));

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);

                }
                ho_ImageClosing.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionTrans.Dispose();
                ho_RegionDifference2.Dispose();
                ho_RegionOpening2.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening1.Dispose();
                ho_ConnectedRegions1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageClosing.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionTrans.Dispose();
                ho_RegionDifference2.Dispose();
                ho_RegionOpening2.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening1.Dispose();
                ho_ConnectedRegions1.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void CheckLieWen(HObject ho_GrayImage, out HObject ho_LieWenObject, HTuple hv_DiffValue,
                                HTuple hv_HysthrMin, HTuple hv_HysthrMax, HTuple hv_stdWH, HTuple hv_LieWenNum,
                                out HTuple hv_Exception)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionHysteresis = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions1 = null, ho_RegionUnion = null, ho_RegionClosing = null;
            HObject ho_RegionFillUp = null, ho_RegionOpening = null, ho_SelectedRegions = null;
            HObject ho_Partitioned = null, ho_SelectedRegions2 = null, ho_RegionErosion = null;
            HObject ho_ImageTexture = null, ho_ImageTexture1 = null, ho_ImageResult = null;
            HObject ho_ImageReduced = null, ho_XiLieWenObj = null, ho_ImageClosing = null;
            HObject ho_Partitioned1 = null, ho_RegionErosion1 = null;

            // Local control variables 

            HTuple hv_Value = new HTuple(), hv_Max = new HTuple();
            HTuple hv_Min = new HTuple(), hv_DeltaValue = new HTuple();
            HTuple hv_Value1 = new HTuple(), hv_Min1 = new HTuple();
            HTuple hv_Max1 = new HTuple(), hv_DeltaLiangDuan = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_LieWenObject);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Partitioned);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageTexture);
            HOperatorSet.GenEmptyObj(out ho_ImageTexture1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_XiLieWenObj);
            HOperatorSet.GenEmptyObj(out ho_ImageClosing);
            HOperatorSet.GenEmptyObj(out ho_Partitioned1);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion1);
            try
            {
                try
                {
                    //两端灰度差异大不均匀，判断是裂纹
                    ho_LieWenObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_LieWenObject);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_GrayImage, out ho_RegionHysteresis, 60,
                        100, 100);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionHysteresis, out ho_ConnectedRegions);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "area",
                        "and", 5000, 999999999);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions1, out ho_RegionUnion);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing, 100,
                        100);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, 100,
                        100);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShapeStd(ho_RegionOpening, out ho_SelectedRegions, "max_area",
                        70);
                    //判断区域里面的颜色是否均匀，如果不均匀为断裂
                    ho_Partitioned.Dispose();
                    HOperatorSet.PartitionRectangle(ho_SelectedRegions, out ho_Partitioned, 300,
                        500);
                    ho_SelectedRegions2.Dispose();
                    HOperatorSet.SelectShape(ho_Partitioned, out ho_SelectedRegions2, "area",
                        "and", 20000, 9999999);
                    HOperatorSet.GrayFeatures(ho_SelectedRegions2, ho_GrayImage, "mean", out hv_Value);
                    HOperatorSet.TupleMax(hv_Value, out hv_Max);
                    HOperatorSet.TupleMin(hv_Value, out hv_Min);
                    hv_DeltaValue = ((hv_Max - hv_Min)).TupleAbs();
                    if ((int)(new HTuple(hv_DeltaValue.TupleGreater(hv_DiffValue))) != 0)
                    {
                        //是裂纹
                        ho_LieWenObject.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedRegions, out ho_LieWenObject, 1, 1);
                    }

                    ho_RegionErosion.Dispose();
                    HOperatorSet.ErosionRectangle1(ho_SelectedRegions, out ho_RegionErosion,
                        50, 20);
                    ho_ImageTexture.Dispose();
                    HOperatorSet.TextureLaws(ho_GrayImage, out ho_ImageTexture, "el", 4, 5);
                    ho_ImageTexture1.Dispose();
                    HOperatorSet.TextureLaws(ho_GrayImage, out ho_ImageTexture1, "le", 4, 5);
                    ho_ImageResult.Dispose();
                    HOperatorSet.AddImage(ho_ImageTexture, ho_ImageTexture1, out ho_ImageResult,
                        1, 0);

                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageResult, ho_RegionErosion, out ho_ImageReduced
                        );
                    //细长裂纹检测
                    ho_XiLieWenObj.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_XiLieWenObj);
                    ho_XiLieWenObj.Dispose();
                    CheckXiTiaoLieWen(ho_ImageReduced, out ho_XiLieWenObj, hv_HysthrMin, hv_HysthrMax,
                        hv_stdWH, hv_LieWenNum, out hv_Exception);
                    {
                        if(ho_XiLieWenObj != null)
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_XiLieWenObj, ho_LieWenObject, out ExpTmpOutVar_0);
                            ho_LieWenObject.Dispose();
                            ho_LieWenObject = ExpTmpOutVar_0;
                        }
                        else
                        {
                            HOperatorSet.GenEmptyObj(out ho_XiLieWenObj);
                        }
                    }

                    //产品左右不均匀特征为裂纹产生
                    ho_ImageClosing.Dispose();
                    HOperatorSet.GrayClosingRect(ho_ImageReduced, out ho_ImageClosing, 5, 5);
                    ho_Partitioned1.Dispose();
                    HOperatorSet.PartitionRectangle(ho_SelectedRegions, out ho_Partitioned1,
                        1500, 500);
                    ho_RegionErosion1.Dispose();
                    HOperatorSet.ErosionRectangle1(ho_Partitioned1, out ho_RegionErosion1, 21,
                        21);
                    HOperatorSet.GrayFeatures(ho_RegionErosion1, ho_ImageClosing, "mean", out hv_Value1);
                    HOperatorSet.TupleMin(hv_Value1, out hv_Min1);
                    HOperatorSet.TupleMax(hv_Value1, out hv_Max1);
                    hv_DeltaLiangDuan = hv_Max1 - hv_Min1;
                    if ((int)(new HTuple(hv_DeltaLiangDuan.TupleGreater(20))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_LieWenObject, ho_SelectedRegions, out ExpTmpOutVar_0
                                );
                            ho_LieWenObject.Dispose();
                            ho_LieWenObject = ExpTmpOutVar_0;
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_RegionHysteresis.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Partitioned.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageTexture.Dispose();
                ho_ImageTexture1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageReduced.Dispose();
                ho_XiLieWenObj.Dispose();
                ho_ImageClosing.Dispose();
                ho_Partitioned1.Dispose();
                ho_RegionErosion1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionHysteresis.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Partitioned.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageTexture.Dispose();
                ho_ImageTexture1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageReduced.Dispose();
                ho_XiLieWenObj.Dispose();
                ho_ImageClosing.Dispose();
                ho_Partitioned1.Dispose();
                ho_RegionErosion1.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void CheckXiTiaoLieWen(HObject ho_ImageReduced, out HObject ho_LieWenObject,
                                 HTuple hv_HysthrMin, HTuple hv_HysthrMax, HTuple hv_stdWH, HTuple hv_LieWenNum,
                                 out HTuple hv_Exception)
        {
            // Local iconic variables 

            HObject ho_RegionHysteresis1 = null, ho_RegionClosing = null;
            HObject ho_RegionClosing1 = null, ho_ConnectedRegions1 = null;
            HObject ho_SelectedRegions2 = null, ho_SampleLieWenObject = null;
            HObject ho_Rectangle = null;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_Length = new HTuple();
            HTuple hv_dWH = new HTuple(), hv_Greater = new HTuple();
            HTuple hv_Sum = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Number = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_LieWenObject);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SampleLieWenObject);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            try
            {
                try
                {
                    ho_LieWenObject = null;
                    hv_Exception = null;
                    ho_RegionHysteresis1.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_ImageReduced, out ho_RegionHysteresis1,
                        hv_HysthrMin, hv_HysthrMax, 500);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionHysteresis1, out ho_RegionClosing, 5.5);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionClosing, out ho_RegionClosing1, 5,
                        20);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing1, out ho_ConnectedRegions1);
                    ho_SelectedRegions2.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions2, (((
                        (new HTuple("area")).TupleConcat("width")).TupleConcat("height")).TupleConcat(
                        "rect2_len2")).TupleConcat("rect2_len1"), "and", ((((new HTuple(50)).TupleConcat(
                        5)).TupleConcat(5)).TupleConcat(5)).TupleConcat(100), ((((new HTuple(99999999)).TupleConcat(
                        99999999)).TupleConcat(99999999)).TupleConcat(99999999)).TupleConcat(
                        99999999));
                    //根据长宽比判断
                    ho_SampleLieWenObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SampleLieWenObject);
                    HOperatorSet.SmallestRectangle2(ho_SelectedRegions2, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);

                    HOperatorSet.TupleLength(hv_Length1, out hv_Length);

                    if ((int)(new HTuple(hv_Length.TupleGreater(0))) != 0)
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                            hv_Length2);
                        hv_dWH = (1.0 * hv_Length1) / hv_Length2;
                        HOperatorSet.TupleGreaterElem(hv_dWH, hv_stdWH, out hv_Greater);
                        HOperatorSet.TupleSum(hv_Greater, out hv_Sum);
                        if ((int)(new HTuple(hv_Sum.TupleGreater(0))) != 0)
                        {
                            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                            ho_SampleLieWenObject.Dispose();
                            HOperatorSet.SelectObj(ho_SelectedRegions2, out ho_SampleLieWenObject,
                                hv_Indices + 1);
                        }
                    }

                    //如果纹理太多，不是断裂
                    HOperatorSet.CountObj(ho_SampleLieWenObject, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleGreater(hv_LieWenNum))) != 0)
                    {

                    }
                    else if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                    {

                    }
                    else
                    {
                        ho_LieWenObject.Dispose();
                        HOperatorSet.CopyObj(ho_SampleLieWenObject, out ho_LieWenObject, 1, 5);
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);

                }
                ho_RegionHysteresis1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_SampleLieWenObject.Dispose();
                ho_Rectangle.Dispose();

            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionHysteresis1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_SampleLieWenObject.Dispose();
                ho_Rectangle.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void DarkRegionCheck(HObject ho_GrayImage, out HObject ho_BigDarkObject,
                                 HTuple hv_DustAreaMin, HTuple hv_DustWidth, HTuple hv_DustHeight, out HTuple hv_Exception)
        {
            // Local iconic variables 

            HObject ho_RegionHysteresis = null, ho_RegionFillUp = null;
            HObject ho_RegionClosing = null, ho_ConnectedRegions1 = null;
            HObject ho_SelectedRegions1 = null, ho_RegionDifference = null;
            HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_RegionUnion = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_BigDarkObject);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            try
            {
                try
                {
                    ho_BigDarkObject = null;
                    hv_Exception = null;
                    HOperatorSet.GenEmptyObj(out ho_BigDarkObject);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_GrayImage, out ho_RegionHysteresis, 100,
                        150, 100);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionHysteresis, out ho_RegionFillUp);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionFillUp, out ho_RegionClosing, 200,
                        100);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions1);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions1, out ho_SelectedRegions1,
                        "max_area", 70);

                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_SelectedRegions1, ho_RegionHysteresis, out ho_RegionDifference
                        );
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions);
                    //筛选缺陷
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat(
                        "width")).TupleConcat("height"), "and", (((new HTuple(1000)).TupleConcat(
                        hv_DustWidth))).TupleConcat(hv_DustHeight), ((new HTuple(999999999)).TupleConcat(
                        99999)).TupleConcat(99999));
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                    ho_BigDarkObject.Dispose();
                    HOperatorSet.SelectShape(ho_RegionUnion, out ho_BigDarkObject, "area", "and",
                        hv_DustAreaMin, 99999999);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);

                }
                ho_RegionHysteresis.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionDifference.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();

            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionHysteresis.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionDifference.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();

                throw HDevExpDefaultException;
            }
        }

    }

}
