using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmController
{
    public class AlgorithmFor3D
    {

        //预处理图片
        public static void ProcessImage(HObject ho_Image, out HObject ho_UseImage, HTuple hv_ThrLow,
                                     HTuple hv_ThrHigh, HTuple hv_FillArea, HTuple hv_CloseSize, HTuple hv_MedianWidth)
        {
            // Local iconic variables 

            HObject ho_Region, ho_RegionFillUp, ho_ImageReduced;
            HObject ho_ImageCleared, ho_MixedImage, ho_CloseImage;

            // Local control variables 

            HTuple hv_Value = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UseImage);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageCleared);
            HOperatorSet.GenEmptyObj(out ho_MixedImage);
            HOperatorSet.GenEmptyObj(out ho_CloseImage);
            try
            {
                //去除噪点
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, hv_ThrLow, hv_ThrHigh);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUpShape(ho_Region, out ho_RegionFillUp, "area", 1, hv_FillArea);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionFillUp, out ho_ImageReduced);
                HOperatorSet.GrayFeatures(ho_RegionFillUp, ho_ImageReduced, "mean", out hv_Value);
                //去除黑洞
                ho_ImageCleared.Dispose();
                HOperatorSet.GenImageProto(ho_Image, out ho_ImageCleared, 0);
                ho_MixedImage.Dispose();
                HOperatorSet.PaintGray(ho_ImageReduced, ho_ImageCleared, out ho_MixedImage);
                ho_CloseImage.Dispose();
                HOperatorSet.GrayClosingRect(ho_MixedImage, out ho_CloseImage, hv_CloseSize,
                    hv_CloseSize);
                //去除尖峰干扰
                ho_UseImage.Dispose();
                HOperatorSet.MedianSeparate(ho_CloseImage, out ho_UseImage, hv_MedianWidth,
                    hv_MedianWidth, "mirrored");
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageCleared.Dispose();
                ho_MixedImage.Dispose();
                ho_CloseImage.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageCleared.Dispose();
                ho_MixedImage.Dispose();
                ho_CloseImage.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //检测胶路缺陷
        public static void CheckGlueDefect2Modul(HObject ho_GlueImageAffineTrans, HObject ho_RegionLines,
                                out HObject ho_GlueDefectSelectedRegions, HTuple hv_MoveDistance, HTuple hv_WH,
                                HTuple hv_BigValue, HTuple hv_Step, HTuple hv_GlueDefectAreaMin)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_DefectObjectOut, ho_RegionMoved = null;
            HObject ho_Rectangle1 = null, ho_ObjectSelected1 = null, ho_ContCircle = null;
            HObject ho_Contour1 = null, ho_TestRegionLines = null, ho_ObjectSelected3 = null;
            HObject ho_RegionUnion, ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_ConnectedRegions;

            // Local control variables 

            HTuple hv_Index2 = null, hv_GlueGrayValue = new HTuple();
            HTuple hv_Rows2 = new HTuple(), hv_Columns1 = new HTuple();
            HTuple hv_RCSequence = new HTuple(), hv_RowSelected = new HTuple();
            HTuple hv_ColSelected = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple(), hv_Value = new HTuple();
            HTuple hv_GlueDefectValueMin = new HTuple(), hv_GlueDefectValueMax = new HTuple();
            HTuple hv_Length = new HTuple(), hv_MxCol = new HTuple();
            HTuple hv_len = new HTuple(), hv_r = new HTuple(), hv_Mb = new HTuple();
            HTuple hv_MyRow = new HTuple(), hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_Greater1 = new HTuple(), hv_Indices1 = new HTuple();
            HTuple hv_Selected = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_TestRegionLines);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            try
            {
                ho_DefectObjectOut.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
                ho_GlueDefectSelectedRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
                HTuple end_val2 = hv_Step;
                HTuple step_val2 = 1;
                for (hv_Index2 = 1; hv_Index2.Continue(end_val2, step_val2); hv_Index2 = hv_Index2.TupleAdd(step_val2))
                {
                    hv_GlueGrayValue = new HTuple();
                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, hv_MoveDistance + ((2 * hv_WH) * hv_Index2));
                    HOperatorSet.GetRegionPoints(ho_RegionMoved, out hv_Rows2, out hv_Columns1);
                    HOperatorSet.TupleGenSequence(0, (new HTuple(hv_Rows2.TupleLength())) - 1,
                        hv_MoveDistance, out hv_RCSequence);
                    HOperatorSet.TupleSelect(hv_Rows2, hv_RCSequence, out hv_RowSelected);
                    HOperatorSet.TupleSelect(hv_Columns1, hv_RCSequence, out hv_ColSelected);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle1, hv_RowSelected - hv_WH, hv_ColSelected - hv_WH,
                        hv_RowSelected + hv_WH, hv_ColSelected + hv_WH);
                    HOperatorSet.CountObj(ho_Rectangle1, out hv_Number1);
                    HTuple end_val11 = hv_Number1;
                    HTuple step_val11 = 1;
                    for (hv_Index1 = 1; hv_Index1.Continue(end_val11, step_val11); hv_Index1 = hv_Index1.TupleAdd(step_val11))
                    {
                        ho_ObjectSelected1.Dispose();
                        HOperatorSet.SelectObj(ho_Rectangle1, out ho_ObjectSelected1, hv_Index1);
                        HOperatorSet.GrayFeatures(ho_ObjectSelected1, ho_GlueImageAffineTrans,
                            "median", out hv_Value);
                        //在阈值范围内才选取是胶路
                        hv_GlueDefectValueMin = 35000;
                        hv_GlueDefectValueMax = 50000;
                        if ((int)((new HTuple(hv_Value.TupleGreater(hv_GlueDefectValueMin))).TupleAnd(
                            new HTuple(hv_Value.TupleLess(hv_GlueDefectValueMax)))) != 0)
                        {
                            hv_GlueGrayValue = hv_GlueGrayValue.TupleConcat(hv_Value);
                        }
                    }
                    HOperatorSet.TupleLength(hv_GlueGrayValue, out hv_Length);
                    HOperatorSet.TupleGenSequence(200, 200 + ((hv_Length - 1) * 10), 10, out hv_MxCol);
                    HOperatorSet.TupleLength(hv_MxCol, out hv_len);
                    HOperatorSet.TupleGenConst(hv_len, 3, out hv_r);
                    hv_Mb = hv_GlueGrayValue / 100;
                    hv_MyRow = hv_Mb.Clone();
                    ho_ContCircle.Dispose();
                    if (hv_MyRow.Length == 0)
                    {
                        continue;
                    }
                    HOperatorSet.GenCircle(out ho_ContCircle, hv_MyRow, hv_MxCol, hv_r);
                    ho_Contour1.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_MyRow, hv_MxCol);
                    HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    ho_TestRegionLines.Dispose();
                    HOperatorSet.GenRegionLine(out ho_TestRegionLines, hv_RowBegin, hv_ColBegin,
                        hv_RowEnd, hv_ColEnd);
                    HOperatorSet.DistancePl(hv_MyRow, hv_MxCol, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                        hv_ColEnd, out hv_Distance);
                    //判断同一区域内，胶路相对高度不良区域
                    HOperatorSet.TupleGreaterElem(hv_Distance, hv_BigValue, out hv_Greater1);
                    HOperatorSet.TupleFind(hv_Greater1, 1, out hv_Indices1);
                    if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
                    {
                        HOperatorSet.TupleSelect(hv_Distance, hv_Indices1, out hv_Selected);
                        ho_ObjectSelected3.Dispose();
                        HOperatorSet.SelectObj(ho_Rectangle1, out ho_ObjectSelected3, hv_Indices1 + 1);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectObjectOut, ho_ObjectSelected3, out ExpTmpOutVar_0
                                );
                            ho_DefectObjectOut.Dispose();
                            ho_DefectObjectOut = ExpTmpOutVar_0;
                        }
                    }
                }
                //筛选胶路缺陷
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_DefectObjectOut, out ho_RegionUnion);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GlueImageAffineTrans, ho_RegionUnion, out ho_ImageReduced
                    );
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 1000, 60000);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 3.5);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                ho_GlueDefectSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_GlueDefectSelectedRegions,
                    "area", "and", hv_GlueDefectAreaMin, 999999999);


                ho_DefectObjectOut.Dispose();
                ho_RegionMoved.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DefectObjectOut.Dispose();
                ho_RegionMoved.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();

                // throw HDevExpDefaultException;
                return;
            }
        }

        //图像差分
        public static void GetGlueImageSub(HObject ho_GlueImageAffineTrans, HObject ho_UseNULLImage,
                      out HObject ho_GlueImageSub, HTuple hv_GlueImageThrLow, HTuple hv_GlueImageThrHigh,
                      HTuple hv_DeletGlueLightMin, HTuple hv_DeletGlueLightMax, HTuple hv_ErosionWH)
        {
            // Local iconic variables 

            HObject ho_Region, ho_GlueImageReduced, ho_ImageSub;
            HObject ho_RegionHysteresis, ho_RegionDifference, ho_ImageReduced4;
            HObject ho_ImageCleared, ho_MixedImage;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueImageSub);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_GlueImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced4);
            HOperatorSet.GenEmptyObj(out ho_ImageCleared);
            HOperatorSet.GenEmptyObj(out ho_MixedImage);
            try
            {
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_GlueImageAffineTrans, out ho_Region, hv_GlueImageThrLow,
                    hv_GlueImageThrHigh);
                ho_GlueImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GlueImageAffineTrans, ho_Region, out ho_GlueImageReduced
                    );
                ho_ImageSub.Dispose();
                HOperatorSet.SubImage(ho_GlueImageReduced, ho_UseNULLImage, out ho_ImageSub,
                    1, 0);
                ho_RegionHysteresis.Dispose();
                HOperatorSet.HysteresisThreshold(ho_ImageSub, out ho_RegionHysteresis, hv_DeletGlueLightMin,
                    hv_DeletGlueLightMax, 10);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_ImageSub, ho_RegionHysteresis, out ho_RegionDifference
                    );
                ho_ImageReduced4.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageSub, ho_RegionDifference, out ho_ImageReduced4
                    );
                ho_ImageCleared.Dispose();
                HOperatorSet.GenImageProto(ho_UseNULLImage, out ho_ImageCleared, 0);
                ho_MixedImage.Dispose();
                HOperatorSet.PaintGray(ho_ImageReduced4, ho_ImageCleared, out ho_MixedImage
                    );
                ho_GlueImageSub.Dispose();
                HOperatorSet.GrayErosionRect(ho_MixedImage, out ho_GlueImageSub, hv_ErosionWH,
                    hv_ErosionWH);
                ho_Region.Dispose();
                ho_GlueImageReduced.Dispose();
                ho_ImageSub.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced4.Dispose();
                ho_ImageCleared.Dispose();
                ho_MixedImage.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Region.Dispose();
                ho_GlueImageReduced.Dispose();
                ho_ImageSub.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced4.Dispose();
                ho_ImageCleared.Dispose();
                ho_MixedImage.Dispose();

                throw HDevExpDefaultException;
            }
        }


        //根据折弯线，寻找有胶水外壳内边
        public static void CheckKEdgePointContour(HObject ho_Image0, out HObject ho_KEdgeContours,
                      HTuple hv_Rows, HTuple hv_Cols, HTuple hv_ScaleDivider, HTuple hv_ScaleValue,
                      HTuple hv_KEdgeBottomRow, HTuple hv_KEdgeBottomCol, HTuple hv_KEdgeIncreaseTolerance,
                      HTuple hv_KEdgeLength1, HTuple hv_KEdgeLength2, HTuple hv_KEdgeSigma, HTuple hv_KEdgeThreshold,
                      HTuple hv_KEdgeSelect, HTuple hv_KEdgeTransition, HTuple hv_KEdgeNumber1, HTuple hv_KEdgeScore,
                      HTuple hv_KEdgeSmooth, out HTuple hv_KEdgeAllRow, out HTuple hv_KEdgeAllCol,
                      out HTuple hv_ERROR)
        {
            // Local iconic variables 

            HObject ho_ImageZoom0 = null, ho_Regions = null;
            HObject ho_RegionFillUp = null, ho_Region = null, ho_RegionUnion = null;
            HObject ho_RegionUnion1 = null, ho_ImageResult = null, ho_ImageOpening = null;
            HObject ho_Arrow = null, ho_MeasureLineContours = null, ho_MeasureCross = null;
            HObject ho_KLine = null, ho_Cross = null, ho_Contour1 = null;
            HObject ho_Contour = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Grayval = new HTuple(), hv_KEdgeRow = new HTuple();
            HTuple hv_KEdgeCol = new HTuple(), hv_IndexLines = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Angle = new HTuple();
            HTuple hv_CheckKStartRow = new HTuple(), hv_CheckKStartColumn = new HTuple();
            HTuple hv_CheckKEndRow = new HTuple(), hv_CheckKEndColumn = new HTuple();
            HTuple hv_AllRow = new HTuple(), hv_AllColumn = new HTuple();
            HTuple hv_bFindLine2D = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_KEdgeContours);
            HOperatorSet.GenEmptyObj(out ho_ImageZoom0);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_KLine);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_KEdgeAllRow = new HTuple();
            hv_KEdgeAllCol = new HTuple();
            hv_ERROR = new HTuple();
            try
            {
                try
                {
                    HOperatorSet.GetImageSize(ho_Image0, out hv_Width, out hv_Height);
                    ho_ImageZoom0.Dispose();
                    HOperatorSet.ZoomImageSize(ho_Image0, out ho_ImageZoom0, hv_Width / hv_ScaleDivider,
                        (hv_Height * hv_ScaleValue) / hv_ScaleDivider, "constant");
                    //根据3D图像，筛选边界区域,去除不需要的干扰区域，台阶底面
                    HOperatorSet.GetGrayval(ho_ImageZoom0, hv_KEdgeBottomRow, hv_KEdgeBottomCol,
                        out hv_Grayval);
                    ho_Regions.Dispose();
                    HOperatorSet.RegiongrowingMean(ho_ImageZoom0, out ho_Regions, hv_KEdgeBottomRow,
                        hv_KEdgeBottomCol, hv_KEdgeIncreaseTolerance, 10);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_Regions, out ho_RegionFillUp);
                    //筛选数据丢失区域
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageZoom0, out ho_Region, 0, 20000);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho_Region, ho_RegionFillUp, out ho_RegionUnion);
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_RegionUnion, out ho_RegionUnion1);
                    ho_ImageResult.Dispose();
                    HOperatorSet.PaintRegion(ho_RegionUnion1, ho_ImageZoom0, out ho_ImageResult,
                        80, "fill");
                    ho_ImageOpening.Dispose();
                    HOperatorSet.GrayOpeningRect(ho_ImageResult, out ho_ImageOpening, 5, 5);


                    hv_KEdgeRow = new HTuple();
                    hv_KEdgeCol = new HTuple();

                    for (hv_IndexLines = 0; (int)hv_IndexLines <= (int)((new HTuple(hv_Rows.TupleLength()
                        )) - 2); hv_IndexLines = (int)hv_IndexLines + 1)
                    {
                        ho_Arrow.Dispose();
                        gen_arrow_contour_xld(out ho_Arrow, hv_Rows.TupleSelect(hv_IndexLines),
                            hv_Cols.TupleSelect(hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1),
                            hv_Cols.TupleSelect(hv_IndexLines + 1), 5, 5);
                        HOperatorSet.GetImageSize(ho_ImageOpening, out hv_Width, out hv_Height);
                        HOperatorSet.DistancePp(hv_Rows.TupleSelect(hv_IndexLines), hv_Cols.TupleSelect(
                            hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1), hv_Cols.TupleSelect(
                            hv_IndexLines + 1), out hv_Distance);
                        HOperatorSet.AngleLx(hv_Rows.TupleSelect(hv_IndexLines), hv_Cols.TupleSelect(
                            hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1), hv_Cols.TupleSelect(
                            hv_IndexLines + 1), out hv_Angle);


                        ho_MeasureLineContours.Dispose(); ho_MeasureCross.Dispose(); ho_KLine.Dispose();
                        FindLine2D(ho_ImageOpening, out ho_MeasureLineContours, out ho_MeasureCross,
                            out ho_KLine, hv_Rows.TupleSelect(hv_IndexLines), hv_Cols.TupleSelect(
                            hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1), hv_Cols.TupleSelect(
                            hv_IndexLines + 1), hv_KEdgeLength1, hv_KEdgeLength2, hv_KEdgeSigma,
                            hv_KEdgeThreshold, hv_KEdgeSelect, hv_KEdgeTransition, hv_KEdgeNumber1,
                            hv_KEdgeScore, 0, out hv_CheckKStartRow, out hv_CheckKStartColumn,
                            out hv_CheckKEndRow, out hv_CheckKEndColumn, out hv_AllRow, out hv_AllColumn,
                            out hv_bFindLine2D);


                        hv_KEdgeRow = hv_KEdgeRow.TupleConcat(hv_AllRow);
                        hv_KEdgeCol = hv_KEdgeCol.TupleConcat(hv_AllColumn);
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_KEdgeRow, hv_KEdgeCol,
                            20, 0);
                        hv_ERROR = 0;
                    }

                    ho_Contour1.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_KEdgeRow, hv_KEdgeCol);

                    ho_Contour.Dispose();
                    HOperatorSet.GenContourNurbsXld(out ho_Contour, hv_KEdgeRow, hv_KEdgeCol,
                        "auto", "auto", 3, 1, 5);
                    ho_KEdgeContours.Dispose();
                    HOperatorSet.SmoothContoursXld(ho_Contour, out ho_KEdgeContours, hv_KEdgeSmooth);
                    //gen_polygons_xld (KEdgeContours, Polygons, 'ramer', 5.5)
                    //split_contours_xld (Polygons, Contours, 'polygon', 5, 3)
                    //regress_contours_xld (Contours, RegressContours, 'median', 5000)

                    hv_KEdgeAllRow = hv_KEdgeRow.Clone();
                    hv_KEdgeAllCol = hv_KEdgeCol.Clone();
                    hv_ERROR = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ERROR = 1;
                }
                ho_ImageZoom0.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Region.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Arrow.Dispose();
                ho_MeasureLineContours.Dispose();
                ho_MeasureCross.Dispose();
                ho_KLine.Dispose();
                ho_Cross.Dispose();
                ho_Contour1.Dispose();
                ho_Contour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoom0.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Region.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Arrow.Dispose();
                ho_MeasureLineContours.Dispose();
                ho_MeasureCross.Dispose();
                ho_KLine.Dispose();
                ho_Cross.Dispose();
                ho_Contour1.Dispose();
                ho_Contour.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //找线
        public static void FindLine2D2(HObject ho_Image, out HObject ho_MeasureLineContours, out HObject ho_MeasureCross,
                         out HObject ho_MeasuredLines, HTuple hv_InLineStartRow, HTuple hv_InLineStartCol,
                         HTuple hv_InLineEndRow, HTuple hv_InLineEndCol, HTuple hv_InMeasureLength1,
                         HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                         HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                         HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_RowBegin, out HTuple hv_ColBegin,
                         out HTuple hv_RowEnd, out HTuple hv_ColEnd, out HTuple hv_AllRow, out HTuple hv_AllColumn,
                         out HTuple hv_bFindLine2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyLineIndices = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            hv_AllRow = new HTuple();
            hv_AllColumn = new HTuple();
            hv_bFindLine2D = new HTuple();
            try
            {
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
                //
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_InLineStartRow,
                    hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, hv_InMeasureLength1,
                    hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(),
                    new HTuple(), out hv_MetrologyLineIndices);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_measures", hv_InMeasureNumber);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_interpolation", "bicubic");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_begin", out hv_RowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_begin", out hv_ColBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_end", out hv_RowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_end", out hv_ColEnd);
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureLineContours, hv_MetrologyHandle,
                    "all", "all", out hv_AllRow, out hv_AllColumn);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_AllRow, hv_AllColumn,
                    3, 0.785398);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLines, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                //if (bDisp)
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureLineContours)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureCross)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasuredLines)
                }
                //endif
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_RowBegin.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_ColEnd.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindLine2D = 1;
                }
                else
                {
                    hv_bFindLine2D = 0;
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindLine2D = 0;
            }


            return;
        }

        //找圆
        public static void FindCircle2D(HObject ho_Image, out HObject ho_MeasureCircleContours,
            out HObject ho_MeasureCross, out HObject ho_CircleContours, HTuple hv_InCircleRow,
            HTuple hv_InCircleCol, HTuple hv_InCircleRadiu, HTuple hv_InMeasureLength1,
            HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
            HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
            HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_CircleCenterRow, out HTuple hv_CircleCenterColumn,
            out HTuple hv_CircleRadius, out HTuple hv_bFindCircle2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyCircleIndex = new HTuple();
            HTuple hv_CircleParameter = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_CircleContours);
            hv_CircleCenterRow = new HTuple();
            hv_CircleCenterColumn = new HTuple();
            hv_CircleRadius = new HTuple();
            hv_bFindCircle2D = new HTuple();
            try
            {
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_CircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_CircleContours);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_InCircleRow,
                    hv_InCircleCol, hv_InCircleRadiu, hv_InMeasureLength1, hv_InMeasureLength2,
                    hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(), new HTuple(), out hv_MetrologyCircleIndex);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_measures", (int)hv_InMeasureNumber.D);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_interpolation", "nearest_neighbor");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "all", "result_type", "all_param", out hv_CircleParameter);
                //Extract the parameters for better readability
                hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                    )) - 1, 3);
                hv_CircleCenterRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                hv_CircleCenterColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);

                ho_CircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_CircleContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureCircleContours, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_Row1, hv_Column1, 26,
                    0.785398);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(hv_bDisp) != 0)
                {
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCircleContours, HDevWindowStack.GetActive()
                            );
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCross, HDevWindowStack.GetActive());
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_CircleContours, HDevWindowStack.GetActive());
                    }
                }
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterRow.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterColumn.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindCircle2D = 1;
                }
                else
                {
                    hv_bFindCircle2D = 0;
                }

            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindCircle2D = 0;
            }


            return;
        }

        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
            HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            try
            {
                //This procedure generates arrow shaped XLD contours,
                //pointing from (Row1, Column1) to (Row2, Column2).
                //If starting and end point are identical, a contour consisting
                //of a single point is returned.
                //
                //input parameteres:
                //Row1, Column1: Coordinates of the arrows' starting points
                //Row2, Column2: Coordinates of the arrows' end points
                //HeadLength, HeadWidth: Size of the arrow heads in pixels
                //
                //output parameter:
                //Arrow: The resulting XLD contour
                //
                //The input tuples Row1, Column1, Row2, and Column2 have to be of
                //the same length.
                //HeadLength and HeadWidth either have to be of the same length as
                //Row1, Column1, Row2, and Column2 or have to be a single element.
                //If one of the above restrictions is violated, an error will occur.
                //
                //
                //Init
                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                //
                //Calculate the arrow length
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
                    if (hv_Length == null)
                        hv_Length = new HTuple();
                    hv_Length[hv_ZeroLengthIndices] = -1;
                }
                //
                //Calculate auxiliary variables.
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
                //
                //Calculate end points of the arrow head.
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
                //
                //Finally create output XLD contour for each input point pair
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                    {
                        //Create_ single points for arrows with identical start and end point
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
                    }
                    else
                    {
                        //Create arrow contour
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                        ho_Arrow.Dispose();
                        ho_Arrow = ExpTmpOutVar_0;
                    }
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_TempArrow.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //胶路提取
        public static void GetGlueImage(HObject ho_GlueImage, HObject ho_NullImage, HObject ho_CheckPartionRegion,
                                 out HObject ho_OutGlueImage, out HObject ho_GlueSelectedRegions, HTuple hv_ScaleDivider,
                                 HTuple hv_ScaleValue, HTuple hv_HyThrLow, HTuple hv_HyThrHigh, HTuple hv_SelectAreaMin,
                                 HTuple hv_SelectAreaMax, out HTuple hv_ERROR)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_NullImageZoom0 = null, ho_GlueImageZoom1 = null;
            HObject ho_Region = null, ho_ImageReduced = null, ho_ImageScaleMax1 = null;
            HObject ho_GlueRegionObject = null, ho_ObjectSelected = null;
            HObject ho_ImageReduced1 = null, ho_ImageMedian = null, ho_Regions = null;
            HObject ho_RegionUnion = null, ho_Region2 = null, ho_RegionDifference1 = null;
            HObject ho_ConnectedRegions = null, ho_ImageReduced2 = null;
            HObject ho_Region1 = null, ho_ObjectsConcat = null, ho_RegionUnion2 = null;
            HObject ho_RegionUnion3 = null, ho_RegionClosing = null, ho_ConnectedRegions2 = null;
            HObject ho_SelectedRegions = null, ho_DifRegions = null, ho_ImageResult1 = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_UsedThreshold = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_OutGlueImage);
            HOperatorSet.GenEmptyObj(out ho_GlueSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_NullImageZoom0);
            HOperatorSet.GenEmptyObj(out ho_GlueImageZoom1);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageScaleMax1);
            HOperatorSet.GenEmptyObj(out ho_GlueRegionObject);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion3);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_DifRegions);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            hv_ERROR = new HTuple();
            try
            {
                try
                {
                    HOperatorSet.GetImageSize(ho_GlueImage, out hv_Width, out hv_Height);
                    ho_NullImageZoom0.Dispose();
                    HOperatorSet.ZoomImageSize(ho_NullImage, out ho_NullImageZoom0, hv_Width / hv_ScaleDivider,
                        (hv_Height * hv_ScaleValue) / hv_ScaleDivider, "constant");
                    ho_GlueImageZoom1.Dispose();
                    HOperatorSet.ZoomImageSize(ho_GlueImage, out ho_GlueImageZoom1, hv_Width / hv_ScaleDivider,
                        (hv_Height * hv_ScaleValue) / hv_ScaleDivider, "constant");

                    //sub_image (GlueImageZoom1, NullImageZoom0, GlueImageSub, 1, 0)
                    //equ_histo_image (GlueImageSub, ImageEquHisto2)
                    //直接对图像减去差异分割
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_GlueImageZoom1, out ho_Region, hv_HyThrLow, hv_HyThrHigh);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_GlueImageZoom1, ho_Region, out ho_ImageReduced
                        );
                    //根据分割区域特点赛选
                    HOperatorSet.CountObj(ho_CheckPartionRegion, out hv_Number);
                    ho_ImageScaleMax1.Dispose();
                    HOperatorSet.ScaleImageMax(ho_ImageReduced, out ho_ImageScaleMax1);
                    ho_GlueRegionObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_GlueRegionObject);
                    HTuple end_val14 = hv_Number;
                    HTuple step_val14 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val14, step_val14); hv_Index = hv_Index.TupleAdd(step_val14))
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_CheckPartionRegion, out ho_ObjectSelected, hv_Index);
                        ho_ImageReduced1.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageScaleMax1, ho_ObjectSelected, out ho_ImageReduced1
                            );
                        ho_ImageMedian.Dispose();
                        HOperatorSet.MedianImage(ho_ImageReduced1, out ho_ImageMedian, "circle",
                            4, "mirrored");
                        ho_Regions.Dispose();
                        HOperatorSet.RegiongrowingN(ho_ImageMedian, out ho_Regions, "dot-product",
                            0, 5, 5);
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_Regions, out ho_RegionUnion);
                        ho_Region2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 1, 99999);
                        ho_RegionDifference1.Dispose();
                        HOperatorSet.Difference(ho_Region2, ho_RegionUnion, out ho_RegionDifference1
                            );
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionDifference1, out ho_ConnectedRegions);
                        //根据区域增长筛选胶水
                        ho_ImageReduced2.Dispose();
                        HOperatorSet.ReduceDomain(ho_GlueImageZoom1, ho_ObjectSelected, out ho_ImageReduced2
                            );
                        ho_Region1.Dispose();
                        HOperatorSet.BinaryThreshold(ho_ImageReduced2, out ho_Region1, "smooth_histo",
                            "light", out hv_UsedThreshold);
                        if ((int)(new HTuple(hv_UsedThreshold.TupleGreater(hv_HyThrHigh))) != 0)
                        {
                            ho_Region1.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_Region1);
                        }
                        ho_ObjectsConcat.Dispose();
                        HOperatorSet.ConcatObj(ho_ConnectedRegions, ho_Region1, out ho_ObjectsConcat
                            );
                        ho_RegionUnion2.Dispose();
                        HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion2);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_GlueRegionObject, ho_RegionUnion2, out ExpTmpOutVar_0
                                );
                            ho_GlueRegionObject.Dispose();
                            ho_GlueRegionObject = ExpTmpOutVar_0;
                        }
                    }
                    ho_RegionUnion3.Dispose();
                    HOperatorSet.Union1(ho_GlueRegionObject, out ho_RegionUnion3);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionUnion3, out ho_RegionClosing, 3.5);
                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions2);

                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions, "area",
                        "and", hv_SelectAreaMin, hv_SelectAreaMax);
                    ho_GlueSelectedRegions.Dispose();
                    HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_GlueSelectedRegions,
                        "area", 1, 10000);
                    ho_DifRegions.Dispose();
                    HOperatorSet.Difference(ho_GlueImageZoom1, ho_GlueSelectedRegions, out ho_DifRegions
                        );
                    ho_ImageResult1.Dispose();
                    HOperatorSet.PaintRegion(ho_DifRegions, ho_GlueImageZoom1, out ho_ImageResult1,
                        0, "fill");
                    ho_OutGlueImage.Dispose();
                    HOperatorSet.ScaleImageMax(ho_ImageResult1, out ho_OutGlueImage);


                    //**********************区域分别筛选胶路 结束

                    //*********计算无效数据区域  开始
                    //threshold (ImageZoom1, Region3, 10000, 50000)
                    //fill_up_shape (Region3, RegionFillUp, 'area', 1, 99999)
                    //difference (RegionFillUp, Region3, RegionDifference)
                    //gen_image_proto (ImageZoom1, ImageCleared, 0)
                    //union2 (RegionDifference, GlueSelectedRegions, RegionUnion1)
                    //union1 (RegionUnion1, RegionUnion4)
                    //fill_up_shape (RegionUnion4, RegionFillUp1, 'area', 1, 1000)
                    //connection (RegionFillUp1, ConnectedRegions1)
                    //select_shape (ConnectedRegions1, GlueSelectedRegions1, 'area', 'and', SelectAreaMin, SelectAreaMax)
                    //select_shape_std (GlueSelectedRegions1, GlueSelectedRegions, 'max_area', 70)
                    //paint_region (GlueSelectedRegions, ImageCleared, ImageResult, 255, 'fill')
                    //gray_closing_rect (ImageResult, OutGlueImage, 5, 5)
                    //*********计算无效数据区域  结束



                    hv_ERROR = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ERROR = 1;
                }
                ho_NullImageZoom0.Dispose();
                ho_GlueImageZoom1.Dispose();
                ho_Region.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageScaleMax1.Dispose();
                ho_GlueRegionObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionUnion.Dispose();
                ho_Region2.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Region1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions.Dispose();
                ho_DifRegions.Dispose();
                ho_ImageResult1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_NullImageZoom0.Dispose();
                ho_GlueImageZoom1.Dispose();
                ho_Region.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageScaleMax1.Dispose();
                ho_GlueRegionObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageMedian.Dispose();
                ho_Regions.Dispose();
                ho_RegionUnion.Dispose();
                ho_Region2.Dispose();
                ho_RegionDifference1.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Region1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions.Dispose();
                ho_DifRegions.Dispose();
                ho_ImageResult1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //测量胶宽
        public static void CheckArcGlueWidth(HObject ho_OutGlueImage, out HObject ho_GlueWidthObject,
                                       HTuple hv_Rows, HTuple hv_Cols, HTuple hv_SegMentDist, HTuple hv_RoiWidthLen2,
                                       HTuple hv_RoiLen1, HTuple hv_AmplitudeThreshold, HTuple hv_WidthTransition,
                                       HTuple hv_WidthSelect, HTuple hv_Sigma, HTuple hv_GlueSide, out HTuple hv_GlueWidthValue,
                                       out HTuple hv_GlueInerRow, out HTuple hv_GlueIneerCol, out HTuple hv_ERROR)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageScaled = null, ho_Arrow = null;
            HObject ho_Rectangle = null, ho_Cross1 = null, ho_Cross2 = null;
            HObject ho_RegionLines = null;

            // Local control variables 

            HTuple hv_GlueWidthValue1 = new HTuple(), hv_IndexLines = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Angle = new HTuple();
            HTuple hv_EachDist = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Rec2CenterRow = new HTuple(), hv_Rec2CenterCol = new HTuple();
            HTuple hv_MsrHandle_Measure_01_0 = new HTuple(), hv_MsrHandle_Measure_01_1 = new HTuple();
            HTuple hv_Row1_Measure_01_0 = new HTuple(), hv_Column1_Measure_01_0 = new HTuple();
            HTuple hv_Amplitude1_Measure_01_0 = new HTuple(), hv_Distance_Measure_01_0 = new HTuple();
            HTuple hv_Row2_Measure_01_0 = new HTuple(), hv_Column2_Measure_01_0 = new HTuple();
            HTuple hv_Amplitude2_Measure_01_0 = new HTuple(), hv_Width_Measure_01_0 = new HTuple();
            HTuple hv_Number = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueWidthObject);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            hv_GlueWidthValue = new HTuple();
            hv_GlueInerRow = new HTuple();
            hv_GlueIneerCol = new HTuple();
            hv_ERROR = new HTuple();
            try
            {
                try
                {
                    hv_GlueInerRow = new HTuple();
                    hv_GlueIneerCol = new HTuple();
                    //dev_update_on();
                    hv_GlueWidthValue1 = new HTuple();
                    ho_GlueWidthObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_GlueWidthObject);
                    ho_ImageScaled.Dispose();
                    scale_image_range(ho_OutGlueImage, out ho_ImageScaled, 0, 180);
                    for (hv_IndexLines = 0; (int)hv_IndexLines <= (int)((new HTuple(hv_Rows.TupleLength()
                        )) - 2); hv_IndexLines = (int)hv_IndexLines + 1)
                    {
                        ho_Arrow.Dispose();
                        gen_arrow_contour_xld(out ho_Arrow, hv_Rows.TupleSelect(hv_IndexLines),
                            hv_Cols.TupleSelect(hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1),
                            hv_Cols.TupleSelect(hv_IndexLines + 1), 5, 5);
                        HOperatorSet.GetImageSize(ho_ImageScaled, out hv_Width, out hv_Height);
                        HOperatorSet.DistancePp(hv_Rows.TupleSelect(hv_IndexLines), hv_Cols.TupleSelect(
                            hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1), hv_Cols.TupleSelect(
                            hv_IndexLines + 1), out hv_Distance);
                        HOperatorSet.AngleLx(hv_Rows.TupleSelect(hv_IndexLines), hv_Cols.TupleSelect(
                            hv_IndexLines), hv_Rows.TupleSelect(hv_IndexLines + 1), hv_Cols.TupleSelect(
                            hv_IndexLines + 1), out hv_Angle);
                        hv_EachDist = hv_Distance / hv_SegMentDist;
                        HTuple end_val13 = hv_EachDist;
                        HTuple step_val13 = 1;
                        for (hv_Index1 = 0; hv_Index1.Continue(end_val13, step_val13); hv_Index1 = hv_Index1.TupleAdd(step_val13))
                        {
                            hv_Rec2CenterRow = (hv_Rows.TupleSelect(hv_IndexLines)) - ((hv_SegMentDist * hv_Index1) * (hv_Angle.TupleSin()
                                ));
                            hv_Rec2CenterCol = (hv_Cols.TupleSelect(hv_IndexLines)) + ((hv_SegMentDist * hv_Index1) * (hv_Angle.TupleCos()
                                ));
                            //gen_cross_contour_xld (Cross, Rec2CenterRow, Rec2CenterCol, 16, 0)

                            HOperatorSet.SetSystem("int_zooming", "true");
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_Rec2CenterRow,
                                hv_Rec2CenterCol, hv_Angle - ((new HTuple(90)).TupleRad()), hv_RoiWidthLen2,
                                hv_RoiLen1);

                            HOperatorSet.GenMeasureRectangle2(hv_Rec2CenterRow, hv_Rec2CenterCol,
                                hv_Angle - ((new HTuple(90)).TupleRad()), hv_RoiWidthLen2, hv_RoiLen1,
                                hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure_01_0);
                            HOperatorSet.GenMeasureRectangle2(hv_Rec2CenterRow, hv_Rec2CenterCol,
                                hv_Angle + ((new HTuple(90)).TupleRad()), hv_RoiWidthLen2, hv_RoiLen1,
                                hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure_01_1);

                            HOperatorSet.MeasurePos(ho_ImageScaled, hv_MsrHandle_Measure_01_0, hv_Sigma,
                                hv_AmplitudeThreshold, "positive", "first", out hv_Row1_Measure_01_0,
                                out hv_Column1_Measure_01_0, out hv_Amplitude1_Measure_01_0, out hv_Distance_Measure_01_0);
                            ho_Cross1.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_Row1_Measure_01_0,
                                hv_Column1_Measure_01_0, 5, 0);

                            HOperatorSet.MeasurePos(ho_ImageScaled, hv_MsrHandle_Measure_01_1, hv_Sigma,
                                hv_AmplitudeThreshold, "positive", "first", out hv_Row2_Measure_01_0,
                                out hv_Column2_Measure_01_0, out hv_Amplitude2_Measure_01_0, out hv_Distance_Measure_01_0);
                            ho_Cross2.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_Row2_Measure_01_0,
                                hv_Column2_Measure_01_0, 5, 0);

                            //measure_pairs (ImageScaled, MsrHandle_Measure_01_0, Sigma, AmplitudeThreshold, WidthTransition, WidthSelect, Row1_Measure_01_0, Column1_Measure_01_0, Amplitude1_Measure_01_0, Row2_Measure_01_0, Column2_Measure_01_0, Amplitude2_Measure_01_0, Width_Measure_01_0, Distance_Measure_01_0)
                            if ((int)((new HTuple((new HTuple(hv_Row1_Measure_01_0.TupleLength())).TupleEqual(
                                0))).TupleOr(new HTuple((new HTuple(hv_Row2_Measure_01_0.TupleLength()
                                )).TupleEqual(0)))) != 0)
                            {
                                hv_Width_Measure_01_0 = -9999;
                            }
                            else
                            {
                                ho_RegionLines.Dispose();
                                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row1_Measure_01_0,
                                    hv_Column1_Measure_01_0, hv_Row2_Measure_01_0, hv_Column2_Measure_01_0);
                                HOperatorSet.DistancePp(hv_Row1_Measure_01_0, hv_Column1_Measure_01_0,
                                    hv_Row2_Measure_01_0, hv_Column2_Measure_01_0, out hv_Width_Measure_01_0);

                                //concat_obj (Cross1, Cross2, ObjectsConcat)
                                //concat_obj (ObjectsConcat, GlueWidthObject, GlueWidthObject)
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_GlueWidthObject, ho_RegionLines, out ExpTmpOutVar_0
                                        );
                                    ho_GlueWidthObject.Dispose();
                                    ho_GlueWidthObject = ExpTmpOutVar_0;
                                }
                                hv_GlueWidthValue1 = hv_GlueWidthValue1.TupleConcat(hv_Width_Measure_01_0);
                            }

                            if ((int)(hv_GlueSide) != 0)
                            {
                                hv_GlueInerRow = hv_GlueInerRow.TupleConcat(hv_Row2_Measure_01_0);
                                hv_GlueIneerCol = hv_GlueIneerCol.TupleConcat(hv_Column2_Measure_01_0);
                            }
                            else
                            {
                                hv_GlueInerRow = hv_GlueInerRow.TupleConcat(hv_Row1_Measure_01_0);
                                hv_GlueIneerCol = hv_GlueIneerCol.TupleConcat(hv_Column1_Measure_01_0);
                            }
                        }
                        HOperatorSet.CloseMeasure(hv_MsrHandle_Measure_01_0);
                        HOperatorSet.CloseMeasure(hv_MsrHandle_Measure_01_1);
                        HOperatorSet.CloseAllMeasures();
                        hv_ERROR = 0;
                    }
                    HOperatorSet.CountObj(ho_GlueWidthObject, out hv_Number);

                    hv_GlueWidthValue = hv_GlueWidthValue1.Clone();

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ERROR = 1;
                }
                ho_ImageScaled.Dispose();
                ho_Arrow.Dispose();
                ho_Rectangle.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_RegionLines.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageScaled.Dispose();
                ho_Arrow.Dispose();
                ho_Rectangle.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_RegionLines.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //获取胶高
        public static void GetGlueHeight(HObject ho_GlueWidthObject, HObject ho_GlueSubImage,
                                  HTuple hv_GlueValueMin, out HTuple hv_GlueHeightMedian)
        {




            // Local iconic variables 

            HObject ho_ObjectSelected = null, ho_Partitioned1 = null;
            HObject ho_Circle = null, ho_Contour3 = null;

            // Local control variables 

            HTuple hv_Number = null, hv_Index = null, hv_Value1 = new HTuple();
            HTuple hv_Greatereq = new HTuple(), hv_Indices1 = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Value1Selected = new HTuple(), hv_Diff = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_Newtuple = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Indices2 = new HTuple();
            HTuple hv_Length3 = new HTuple(), hv_MidSelect = new HTuple();
            HTuple hv_Selected1 = new HTuple(), hv_HeightMedian = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Partitioned1);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Contour3);
            try
            {
                HOperatorSet.CountObj(ho_GlueWidthObject, out hv_Number);
                hv_GlueHeightMedian = new HTuple();
                HTuple end_val2 = hv_Number;
                HTuple step_val2 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val2, step_val2); hv_Index = hv_Index.TupleAdd(step_val2))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_GlueWidthObject, out ho_ObjectSelected, hv_Index);
                    ho_Partitioned1.Dispose();
                    HOperatorSet.PartitionDynamic(ho_ObjectSelected, out ho_Partitioned1, 1,
                        1);
                    HOperatorSet.GrayFeatures(ho_Partitioned1, ho_GlueSubImage, "mean", out hv_Value1);
                    HOperatorSet.TupleGreaterEqualElem(hv_Value1, hv_GlueValueMin, out hv_Greatereq);
                    HOperatorSet.TupleFind(hv_Greatereq, 1, out hv_Indices1);
                    if ((int)(new HTuple((new HTuple(hv_Indices1.TupleLength())).TupleGreater(
                        10))) != 0)
                    {
                        HOperatorSet.TupleSelect(hv_Value1, hv_Indices1, out hv_Selected);
                        //选择波峰前1/5，后5/6 选择中间段
                        HOperatorSet.TupleLength(hv_Selected, out hv_Length2);
                        HOperatorSet.TupleSelectRange(hv_Selected, hv_Length2 / 5, (hv_Length2 * 5) / 6,
                            out hv_Value1Selected);
                        //缩放高度
                        HOperatorSet.TupleMult(hv_Value1Selected, 0.2, out hv_Diff);
                        HOperatorSet.TupleLength(hv_Diff, out hv_Length1);
                        hv_Sequence = HTuple.TupleGenSequence(500, 500 + ((hv_Length1 - 1) * 5), 5);
                        HOperatorSet.TupleGenConst(hv_Length1, 3, out hv_Newtuple);
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle, hv_Diff, hv_Sequence, hv_Newtuple);
                        HOperatorSet.AreaCenter(ho_Circle, out hv_Area, out hv_Row, out hv_Column);
                        ho_Contour3.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour3, hv_Row, hv_Column);
                    }
                    //获取胶路高度，左右各取
                    HOperatorSet.TupleMax(hv_Value1Selected, out hv_Max);
                    HOperatorSet.TupleFind(hv_Value1Selected, hv_Max, out hv_Indices2);
                    HOperatorSet.TupleLength(hv_Value1Selected, out hv_Length3);
                    hv_MidSelect = 3;
                    if ((int)((new HTuple((((hv_Indices2.TupleSelect(0)) - hv_MidSelect)).TupleGreater(
                        0))).TupleAnd(new HTuple((((hv_Indices2.TupleSelect(0)) + hv_MidSelect)).TupleLess(
                        hv_Length3)))) != 0)
                    {
                        HOperatorSet.TupleSelectRange(hv_Value1Selected, (hv_Indices2.TupleSelect(
                            0)) - hv_MidSelect, (hv_Indices2.TupleSelect(0)) + hv_MidSelect, out hv_Selected1);
                    }
                    else
                    {
                        HOperatorSet.TupleMax(hv_Value1Selected, out hv_Selected1);
                    }
                    HOperatorSet.TupleMedian(hv_Selected1, out hv_HeightMedian);
                    hv_GlueHeightMedian = hv_GlueHeightMedian.TupleConcat(hv_HeightMedian);

                    if (HDevWindowStack.IsOpen())
                    {
                        //dev_display (GlueSubImage)
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        //dev_display (ObjectSelected)
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        //dev_display (Circle)
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        //dev_display (Contour3)
                    }
                }
                ho_ObjectSelected.Dispose();
                ho_Partitioned1.Dispose();
                ho_Circle.Dispose();
                ho_Contour3.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectSelected.Dispose();
                ho_Partitioned1.Dispose();
                ho_Circle.Dispose();
                ho_Contour3.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //测量胶偏
        public static void DeterminGlueValue(HObject ho_KEdgeContours, HTuple hv_GlueInerRow,
                                      HTuple hv_GlueIneerCol, HTuple hv_GlueMaxDistanceSTD, HTuple hv_GlueMinDistanceSTD,
                                      out HTuple hv_GlueMaxValue, out HTuple hv_GlueMaxNumber, out HTuple hv_GlueMinValue,
                                      out HTuple hv_GlueMinNumber, out HTuple hv_MaxGlueInerRow, out HTuple hv_MaxGlueIneerCol,
                                      out HTuple hv_MinGlueInerRow, out HTuple hv_MinGlueIneerCol, out HTuple hv_ERROR)
        {




            // Local iconic variables 

            HObject ho_Cross = null, ho_Cross2 = null, ho_Cross1 = null;

            // Local control variables 

            HTuple hv_KEdgeGlueDistanceMin = new HTuple();
            HTuple hv_DistanceMax = new HTuple(), hv_Greater = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_Less = new HTuple();
            HTuple hv_Indices1 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            hv_GlueMaxValue = new HTuple();
            hv_GlueMaxNumber = new HTuple();
            hv_GlueMinValue = new HTuple();
            hv_GlueMinNumber = new HTuple();
            hv_MaxGlueInerRow = new HTuple();
            hv_MaxGlueIneerCol = new HTuple();
            hv_MinGlueInerRow = new HTuple();
            hv_MinGlueIneerCol = new HTuple();
            hv_ERROR = new HTuple();
            try
            {
                try
                {
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_GlueInerRow, hv_GlueIneerCol,
                        6, 0.785398);
                    HOperatorSet.DistancePc(ho_KEdgeContours, hv_GlueInerRow, hv_GlueIneerCol,
                        out hv_KEdgeGlueDistanceMin, out hv_DistanceMax);
                    //根据胶水内边到外壳边缘距离判定
                    //比大的大
                    ho_Cross2.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_Cross2);
                    HOperatorSet.TupleGreaterElem(hv_KEdgeGlueDistanceMin, hv_GlueMaxDistanceSTD,
                        out hv_Greater);
                    HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
                    if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                    {
                        HOperatorSet.TupleSelect(hv_GlueInerRow, hv_Indices, out hv_MaxGlueInerRow);
                        HOperatorSet.TupleSelect(hv_GlueIneerCol, hv_Indices, out hv_MaxGlueIneerCol);
                        ho_Cross2.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_MaxGlueInerRow, hv_MaxGlueIneerCol,
                            6, 0.785398);
                        HOperatorSet.TupleSelect(hv_KEdgeGlueDistanceMin, hv_Indices, out hv_GlueMaxValue);
                        HOperatorSet.TupleLength(hv_GlueMaxValue, out hv_GlueMaxNumber);
                    }


                    //比小的小
                    ho_Cross1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_Cross1);
                    HOperatorSet.TupleLessElem(hv_KEdgeGlueDistanceMin, hv_GlueMinDistanceSTD,
                        out hv_Less);
                    HOperatorSet.TupleFind(hv_Less, 1, out hv_Indices1);
                    if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
                    {
                        HOperatorSet.TupleSelect(hv_GlueInerRow, hv_Indices1, out hv_MinGlueInerRow);
                        HOperatorSet.TupleSelect(hv_GlueIneerCol, hv_Indices1, out hv_MinGlueIneerCol);
                        ho_Cross1.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_MinGlueInerRow, hv_MinGlueIneerCol,
                            6, 0.785398);
                        HOperatorSet.TupleSelect(hv_KEdgeGlueDistanceMin, hv_Indices1, out hv_GlueMinValue);
                        HOperatorSet.TupleLength(hv_GlueMinValue, out hv_GlueMinNumber);
                    }

                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_KEdgeContours, HDevWindowStack.GetActive());
                    }


                    hv_ERROR = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ERROR = 1;
                }
                ho_Cross.Dispose();
                ho_Cross2.Dispose();
                ho_Cross1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Cross.Dispose();
                ho_Cross2.Dispose();
                ho_Cross1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
    HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageSelected = null, ho_SelectedChannel = null;
            HObject ho_LowerRegion = null, ho_UpperRegion = null, ho_ImageSelectedScaled = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_NumImages = null;
            HTuple hv_ImageIndex = null, hv_Channels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_MinGray = new HTuple();
            HTuple hv_MaxGray = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageSelectedScaled);
            try
            {
                //Convenience procedure to scale the gray values of the
                //input image Image from the interval [Min,Max]
                //to the interval [0,255] (default).
                //Gray values < 0 or > 255 (after scaling) are clipped.
                //
                //If the image shall be scaled to an interval different from [0,255],
                //this can be achieved by passing tuples with 2 values [From, To]
                //as Min and Max.
                //Example:
                //scale_image_range(Image:ImageScaled:[100,50],[200,250])
                //maps the gray values of Image from the interval [100,200] to [50,250].
                //All other gray values will be clipped.
                //
                //input parameters:
                //Image: the input image
                //Min: the minimum gray value which will be mapped to 0
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //Max: The maximum gray value which will be mapped to 255
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //
                //Output parameter:
                //ImageScale: the resulting scaled image.
                //
                if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_LowerLimit = hv_Min_COPY_INP_TMP.TupleSelect(1);
                    hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP.TupleSelect(0);
                }
                else
                {
                    hv_LowerLimit = 0.0;
                }
                if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_UpperLimit = hv_Max_COPY_INP_TMP.TupleSelect(1);
                    hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP.TupleSelect(0);
                }
                else
                {
                    hv_UpperLimit = 255.0;
                }
                //
                //Calculate scaling parameters.
                hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
                hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
                //
                //Scale image.
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult,
                        hv_Add);
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                //
                //Clip gray values if necessary.
                //This must be done for each image and channel separately.
                ho_ImageScaled.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ImageScaled);
                HOperatorSet.CountObj(ho_Image_COPY_INP_TMP, out hv_NumImages);
                HTuple end_val49 = hv_NumImages;
                HTuple step_val49 = 1;
                for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val49, step_val49); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val49))
                {
                    ho_ImageSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Image_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                    HOperatorSet.CountChannels(ho_ImageSelected, out hv_Channels);
                    HTuple end_val52 = hv_Channels;
                    HTuple step_val52 = 1;
                    for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val52, step_val52); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val52))
                    {
                        ho_SelectedChannel.Dispose();
                        HOperatorSet.AccessChannel(ho_ImageSelected, out ho_SelectedChannel, hv_ChannelIndex);
                        HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                            out hv_MaxGray, out hv_Range);
                        ho_LowerRegion.Dispose();
                        HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                            hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                        ho_UpperRegion.Dispose();
                        HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                            ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                                hv_LowerLimit, "fill");
                            ho_SelectedChannel.Dispose();
                            ho_SelectedChannel = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                                hv_UpperLimit, "fill");
                            ho_SelectedChannel.Dispose();
                            ho_SelectedChannel = ExpTmpOutVar_0;
                        }
                        if ((int)(new HTuple(hv_ChannelIndex.TupleEqual(1))) != 0)
                        {
                            ho_ImageSelectedScaled.Dispose();
                            HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageSelectedScaled,
                                1, 1);
                        }
                        else
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AppendChannel(ho_ImageSelectedScaled, ho_SelectedChannel,
                                    out ExpTmpOutVar_0);
                                ho_ImageSelectedScaled.Dispose();
                                ho_ImageSelectedScaled = ExpTmpOutVar_0;
                            }
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ImageScaled, ho_ImageSelectedScaled, out ExpTmpOutVar_0
                            );
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled = ExpTmpOutVar_0;
                    }
                }
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageSelected.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();
                ho_ImageSelectedScaled.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageSelected.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();
                ho_ImageSelectedScaled.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public static void GenFitting(HObject ho_ImageZoom, HObject ho_ROI_0, out HObject ho_Z1,
    HTuple hv_WindowHandle)
        {
            HObject ho_ImageReduced2, ho_ImageMedian, ho_X1;
            HObject ho_Y1;

            // Local copy input parameter variables 
            HObject ho_ROI_0_COPY_INP_TMP;
            ho_ROI_0_COPY_INP_TMP = ho_ROI_0.CopyObj(1, -1);

            // Local control variables 

            HTuple hv_XResolution = null, hv_YResolution = null;
            HTuple hv_ZResolution = null, hv_ScaleDivider = null, hv_StandPlane3Dobject = null;
            HTuple hv_Realpart3Dobject = null, hv_ParFitting = null;
            HTuple hv_ValFitting = null, hv_FitObject = null, hv_PoseA = null;
            HTuple hv_Xangle = null, hv_Pose1 = null, hv_PoseCompose = null;
            HTuple hv_PoseInvert = null, hv_HomMat3D = null, hv_ObjectModel3DRigidTrans = null;
            HTuple hv_ObjectModel3DRigidTransBasePlane = null, hv_CamParam = null;
            HTuple hv_Pose = null, hv_GenParamName = null, hv_GenParamValue = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Z1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_X1);
            HOperatorSet.GenEmptyObj(out ho_Y1);
            try
            {
                //剪切出基准面图像
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_ROI_0_COPY_INP_TMP, out ExpTmpOutVar_0);
                    ho_ROI_0_COPY_INP_TMP.Dispose();
                    ho_ROI_0_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                ho_ImageReduced2.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageZoom, ho_ROI_0_COPY_INP_TMP, out ho_ImageReduced2
                    );
                //
                //对基准面平滑处理
                ho_ImageMedian.Dispose();
                HOperatorSet.MedianImage(ho_ImageReduced2, out ho_ImageMedian, "circle", 1, "mirrored");
                //设置图像的XY比例并生成3D模型
                hv_XResolution = GlobalCore.Global.XYResolution;
                hv_YResolution = GlobalCore.Global.XYResolution;
                hv_ZResolution = GlobalCore.Global.ZResolution;
                hv_ScaleDivider = GlobalCore.Global.ScaleDivider;
                //3D基准面
                convertZmapImageTo3DObject(ho_ImageMedian, hv_XResolution * hv_ScaleDivider, hv_YResolution * hv_ScaleDivider,
                    hv_ZResolution, out hv_StandPlane3Dobject);
                //3D产品
                convertZmapImageTo3DObject(ho_ImageZoom, hv_XResolution * hv_ScaleDivider, hv_YResolution * hv_ScaleDivider,
                    hv_ZResolution, out hv_Realpart3Dobject);
                //
                //dev_open_window (0, 0, 512, 512, 'black', WindowHandle1)
                //visualize_object_model_3d (WindowHandle1, Realpart3Dobject, [], [], [], [], [], [], [], PoseOut)
                //

                //基准面拟合平面并把3D模型转到基准面
                hv_ParFitting = new HTuple();
                hv_ParFitting[0] = "primitive_type";
                hv_ParFitting[1] = "fitting_algorithm";
                hv_ParFitting[2] = "output_xyz_mapping";
                hv_ValFitting = new HTuple();
                hv_ValFitting[0] = "plane";
                hv_ValFitting[1] = "least_squares_huber";
                hv_ValFitting[2] = "true";
                HOperatorSet.FitPrimitivesObjectModel3d(hv_StandPlane3Dobject, hv_ParFitting,
                    hv_ValFitting, out hv_FitObject);
                HOperatorSet.GetObjectModel3dParams(hv_FitObject, "primitive_pose", out hv_PoseA);
                //
                hv_Xangle = hv_PoseA.TupleSelect(3);
                if (hv_Xangle > 180)
                    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", out hv_Pose1);
                else
                    HOperatorSet.CreatePose(0, 0, 0, 180, 0, 0, "Rp+T", "gba", "point", out hv_Pose1);

                //将180改变为0 lhf 2019-4-27
                //HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", out hv_Pose1);
                //
                HOperatorSet.PoseCompose(hv_PoseA, hv_Pose1, out hv_PoseCompose);
                HOperatorSet.PoseInvert(hv_PoseCompose, out hv_PoseInvert);
                HOperatorSet.PoseToHomMat3d(hv_PoseInvert, out hv_HomMat3D);
                //
                //
                //旋转3D模型到基准面
                HOperatorSet.RigidTransObjectModel3d(hv_Realpart3Dobject, hv_PoseInvert, out hv_ObjectModel3DRigidTrans);
                HOperatorSet.RigidTransObjectModel3d(hv_FitObject, hv_PoseInvert, out hv_ObjectModel3DRigidTransBasePlane);
                //par_start<ThreadID> : segment_object_model_3d (ObjectModel3DRigidTrans, 'min_area', 50, ObjectModel3DOut)
                //surface_normals_object_model_3d (ObjectModel3DRigidTrans, 'mls', 'mls_force_inwards', 'true', ObjectModel3DNormals)
                //triangulate_object_model_3d (ObjectModel3DNormals, 'greedy', 'greedy_remove_small_surfaces', 200, ObjectModel3DReference, Information)
                hv_CamParam = new HTuple();
                hv_CamParam[0] = "area_scan_division";
                hv_CamParam[1] = 0.02;
                hv_CamParam[2] = 0;
                hv_CamParam[3] = 8.5e-006;
                hv_CamParam[4] = 8.5e-006;
                hv_CamParam[5] = 640;
                hv_CamParam[6] = 384;
                hv_CamParam[7] = 1024;
                hv_CamParam[8] = 768;
                hv_Pose = new HTuple();
                hv_Pose[0] = -0.055434;
                hv_Pose[1] = 6.58523;
                hv_Pose[2] = 190.222;
                hv_Pose[3] = 179.172;
                hv_Pose[4] = 359.503;
                hv_Pose[5] = 0.142553;
                hv_Pose[6] = 0;
                //
                hv_GenParamName = new HTuple();
                hv_GenParamName[0] = "lut";
                hv_GenParamName[1] = "color_attrib";
                hv_GenParamName[2] = "light_position";
                hv_GenParamName[3] = "disp_pose";
                hv_GenParamName[4] = "alpha";
                hv_GenParamValue = new HTuple();
                hv_GenParamValue[0] = "color1";
                hv_GenParamValue[1] = "coord_z";
                hv_GenParamValue[2] = "-3.0 -0.0001 0.0001 3.0";
                hv_GenParamValue[3] = "true";
                hv_GenParamValue[4] = 1;
                //
                //dev_open_window (0, 0, 512, 512, 'black', WindowHandle1)
                //visualize_object_model_3d (WindowHandle1, ObjectModel3DRigidTrans, [], [], [], [], [], [], [], PoseOut)
                //显示3D模型
                //visualize_object_model_3d (WindowHandle, [ObjectModel3DRigidTrans,ObjectModel3DRigidTransBasePlane], CamParam, Pose, GenParamName, GenParamValue, [], [], [], PoseOut)
                //3D模型重新生成Zmap图
                ho_X1.Dispose(); ho_Y1.Dispose(); ho_Z1.Dispose();
                HOperatorSet.ObjectModel3dToXyz(out ho_X1, out ho_Y1, out ho_Z1, hv_ObjectModel3DRigidTrans,
                    "from_xyz_map", new HTuple(), new HTuple());
                //
                //write_object_model_3d (ObjectModel3DRigidTrans, 'ply', 'C:/Users/Administrator/Desktop/3d.ply', [], [])
                //
                //释放3D模型，否则内存暴涨
                HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DRigidTransBasePlane);
                HOperatorSet.ClearObjectModel3d(hv_StandPlane3Dobject);
                HOperatorSet.ClearObjectModel3d(hv_Realpart3Dobject);
                HOperatorSet.ClearObjectModel3d(hv_FitObject);
                HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DRigidTrans);
                ho_ROI_0_COPY_INP_TMP.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_X1.Dispose();
                ho_Y1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ROI_0_COPY_INP_TMP.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageMedian.Dispose();
                ho_X1.Dispose();
                ho_Y1.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public static void convertZmapImageTo3DObject(HObject ho_ZMapImage, HTuple hv_XResolution,
    HTuple hv_YResolution, HTuple hv_ZResolution, out HTuple hv_Object3DModule)
        {
            HObject ho_PointCloudX, ho_ImageYVal, ho_PointCloudY;
            HObject ho_Region1, ho_ImageReduced, ho_ImageMeasureReal;
            HObject ho_PointCloudZ, ho_Region, ho_Union, ho_ImageReduced_PointCloudZ;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_zMap_Width = null;
            HTuple hv_zMap_Height = null, hv_Grayval = null, hv_hv_offset = null;
            HTuple hv_hvZ = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_PointCloudX);
            HOperatorSet.GenEmptyObj(out ho_ImageYVal);
            HOperatorSet.GenEmptyObj(out ho_PointCloudY);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageMeasureReal);
            HOperatorSet.GenEmptyObj(out ho_PointCloudZ);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_Union);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_PointCloudZ);
            try
            {
                HOperatorSet.GetImageSize(ho_ZMapImage, out hv_Width, out hv_Height);
                //XResolution := 0.019
                //YResolution := 0.019
                //ZResolution := 0.0016
                ho_PointCloudX.Dispose();
                HOperatorSet.GenImageSurfaceFirstOrder(out ho_PointCloudX, "real", hv_XResolution,
                    0, 0, hv_Height / 2, hv_Width / 2, hv_Width, hv_Height);
                ho_ImageYVal.Dispose();
                HOperatorSet.GenImageSurfaceFirstOrder(out ho_ImageYVal, "real", 0, hv_YResolution,
                    0, hv_Height / 2, hv_Width / 2, hv_Width, hv_Height);
                //gen_image_surface_first_order (PointCloudX, 'real', XResolution, 0, 0, 0, 0, Width, Height)
                //gen_image_surface_first_order (ImageYVal, 'real', 0, YResolution, 0, 0, 0, Width, Height)
                ho_PointCloudY.Dispose();
                HOperatorSet.ScaleImage(ho_ImageYVal, out ho_PointCloudY, 1, 0);
                //
                //gen_rectangle1 (Rectangle, 0, 0, Height-1, Width-1)
                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ZMapImage, out ho_Region1, 28000, 50000);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ZMapImage, ho_Region1, out ho_ImageReduced);
                ho_ImageMeasureReal.Dispose();
                HOperatorSet.ConvertImageType(ho_ImageReduced, out ho_ImageMeasureReal, "real");
                HOperatorSet.GetImageSize(ho_ImageMeasureReal, out hv_zMap_Width, out hv_zMap_Height);
                ho_PointCloudZ.Dispose();
                HOperatorSet.GenImageConst(out ho_PointCloudZ, "real", hv_zMap_Width, hv_zMap_Height);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageMeasureReal, out ho_Region, 20000, 50000);
                HOperatorSet.GetRegionPoints(ho_Region, out hv_zMap_Height, out hv_zMap_Width);
                HOperatorSet.GetGrayval(ho_ImageMeasureReal, hv_zMap_Height, hv_zMap_Width,
                    out hv_Grayval);
                HOperatorSet.TuplePow(2, 15, out hv_hv_offset);
                //hvZ := (Grayval-hv_offset)
                hv_hvZ = (hv_Grayval - hv_hv_offset) * hv_ZResolution;
                HOperatorSet.SetGrayval(ho_PointCloudZ, hv_zMap_Height, hv_zMap_Width, hv_hvZ);
                ho_Union.Dispose();
                HOperatorSet.Threshold(ho_PointCloudZ, out ho_Union, (new HTuple(-35)).TupleConcat(
                    0.0001), (new HTuple(-0.0001)).TupleConcat(35));
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_Union, out ExpTmpOutVar_0);
                    ho_Union.Dispose();
                    ho_Union = ExpTmpOutVar_0;
                }
                ho_ImageReduced_PointCloudZ.Dispose();
                HOperatorSet.ReduceDomain(ho_PointCloudZ, ho_Union, out ho_ImageReduced_PointCloudZ
                    );
                HOperatorSet.XyzToObjectModel3d(ho_PointCloudX, ho_PointCloudY, ho_ImageReduced_PointCloudZ,
                    out hv_Object3DModule);
                ho_PointCloudX.Dispose();
                ho_ImageYVal.Dispose();
                ho_PointCloudY.Dispose();
                ho_Region1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMeasureReal.Dispose();
                ho_PointCloudZ.Dispose();
                ho_Region.Dispose();
                ho_Union.Dispose();
                ho_ImageReduced_PointCloudZ.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_PointCloudX.Dispose();
                ho_ImageYVal.Dispose();
                ho_PointCloudY.Dispose();
                ho_Region1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMeasureReal.Dispose();
                ho_PointCloudZ.Dispose();
                ho_Region.Dispose();
                ho_Union.Dispose();
                ho_ImageReduced_PointCloudZ.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public static void CheckGlueDefect2Modul_pre(HObject ho_GlueImageAffineTrans, HObject ho_RegionLines,
    out HObject ho_GlueDefectSelectedRegions, HTuple hv_CheckEdgeSide, HTuple hv_MoveDistance,
    HTuple hv_WH, HTuple hv_BigValue, HTuple hv_Step, HTuple hv_GlueDefectAreaMin, HTuple hv_GlueEdgeHeightMax,
    HTuple hv_GlueInnerEdgeHeightMax)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_DefectObjectOut, ho_LineUsedRegion = null;
            HObject ho_RegionMoved = null, ho_Rectangle1 = null, ho_ObjectSelected1 = null;
            HObject ho_ContCircle = null, ho_Contour1 = null, ho_TestRegionLines = null;
            HObject ho_ObjectSelected3 = null, ho_RegionUnion1 = null, ho_RegionClosing1 = null;
            HObject ho_RegionUnion, ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_ConnectedRegions, ho_RegionDilationGlueCenter;
            HObject ho_RegionMoved4, ho_RegionDilationGlueInner, ho_RegionMoved1;
            HObject ho_RegionDilationLineEdge, ho_RegionAffineTrans;
            HObject ho_RegionDilation2, ho_RegionMoved3, ho_GlueHeightDefect;
            HObject ho_RegionMoved2 = null, ho_RegionIntersectionGlue = null;
            HObject ho_RegionIntersectionInnerGlue = null, ho_RegionIntersectionEdge = null;
            HObject ho_RegionUnion2, ho_RegionClosing2, ho_ConnectedRegions1;
            HObject ho_GlueHeightSelectedRegions;

            // Local control variables 

            HTuple hv_Index2 = null, hv_GlueGrayValue = new HTuple();
            HTuple hv_Rows2 = new HTuple(), hv_Columns1 = new HTuple();
            HTuple hv_RCSequence = new HTuple(), hv_RowSelected = new HTuple();
            HTuple hv_ColSelected = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple(), hv_Value = new HTuple();
            HTuple hv_GlueDefectValueMin = new HTuple(), hv_GlueDefectValueMax = new HTuple();
            HTuple hv_Length = new HTuple(), hv_MxCol = new HTuple();
            HTuple hv_len = new HTuple(), hv_r = new HTuple(), hv_Mb = new HTuple();
            HTuple hv_MyRow = new HTuple(), hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_Greater1 = new HTuple(), hv_Indices1 = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_MoveDistanceGlueHeight = new HTuple();
            HTuple hv_MoveDistanceGlueInner = new HTuple(), hv_MoveDistanceLineEdge = new HTuple();
            HTuple hv_Area = null, hv_Row = null, hv_Column = null;
            HTuple hv_HomMat2DIdentity = null, hv_HomMat2DRotate = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Phi = null;
            HTuple hv_Length1 = null, hv_Length2 = null, hv_LengthHeigth = null;
            HTuple hv_DistanceHeight = null, hv_GlueHeightDefectAreaMin = null;
            HTuple hv_NumberHeight = null, hv_Index = null, hv_GlueCenterValue = new HTuple();
            HTuple hv_GlueInnerValue = new HTuple(), hv_LineEdgeValue = new HTuple();
            HTuple hv_GlueEdgeHeight = new HTuple(), hv_GlueInnerEdgeHeight = new HTuple();
            HTuple hv_GlueEdgeHeightMin = new HTuple();
            HTuple hv_GlueInnerEdgeHeightMin = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
            HOperatorSet.GenEmptyObj(out ho_LineUsedRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_TestRegionLines);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationGlueCenter);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved4);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationGlueInner);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved1);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationLineEdge);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation2);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved3);
            HOperatorSet.GenEmptyObj(out ho_GlueHeightDefect);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved2);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionGlue);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionInnerGlue);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionEdge);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_GlueHeightSelectedRegions);
            try
            {
                ho_DefectObjectOut.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
                ho_GlueDefectSelectedRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
                //
                //
                //
                //
                //最小二乘法整条竖线检测
                HTuple end_val7 = hv_Step;
                HTuple step_val7 = 1;
                for (hv_Index2 = 1; hv_Index2.Continue(end_val7, step_val7); hv_Index2 = hv_Index2.TupleAdd(step_val7))
                {
                    hv_GlueGrayValue = new HTuple();
                    ho_LineUsedRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_LineUsedRegion);
                    //
                    switch (hv_CheckEdgeSide.I)
                    {
                        case 1:
                            ho_RegionMoved.Dispose();
                            HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, (-hv_MoveDistance) - ((2 * hv_WH) * hv_Index2));
                            break;
                        case 2:
                            ho_RegionMoved.Dispose();
                            HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, hv_MoveDistance + ((2 * hv_WH) * hv_Index2));
                            break;
                        case 3:
                            ho_RegionMoved.Dispose();
                            HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, hv_MoveDistance + ((2 * hv_WH) * hv_Index2));
                            break;
                        case 4:
                            ho_RegionMoved.Dispose();
                            HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, (-hv_MoveDistance) - ((2 * hv_WH) * hv_Index2));
                            break;
                        //
                        default:
                            break;
                    }
                    //
                    //
                    HOperatorSet.GetRegionPoints(ho_RegionMoved, out hv_Rows2, out hv_Columns1);
                    HOperatorSet.TupleGenSequence(0, (new HTuple(hv_Rows2.TupleLength())) - 1,
                        hv_MoveDistance, out hv_RCSequence);
                    HOperatorSet.TupleSelect(hv_Rows2, hv_RCSequence, out hv_RowSelected);
                    HOperatorSet.TupleSelect(hv_Columns1, hv_RCSequence, out hv_ColSelected);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle1, hv_RowSelected - hv_WH, hv_ColSelected - hv_WH,
                        hv_RowSelected + hv_WH, hv_ColSelected + hv_WH);
                    HOperatorSet.CountObj(ho_Rectangle1, out hv_Number1);
                    HTuple end_val36 = hv_Number1;
                    HTuple step_val36 = 2;
                    for (hv_Index1 = 1; hv_Index1.Continue(end_val36, step_val36); hv_Index1 = hv_Index1.TupleAdd(step_val36))
                    {
                        ho_ObjectSelected1.Dispose();
                        HOperatorSet.SelectObj(ho_Rectangle1, out ho_ObjectSelected1, hv_Index1);
                        HOperatorSet.GrayFeatures(ho_ObjectSelected1, ho_GlueImageAffineTrans,
                            "median", out hv_Value);
                        //在阈值范围内才选取是胶路
                        hv_GlueDefectValueMin = 25000;
                        hv_GlueDefectValueMax = 38000;
                        if ((int)((new HTuple(hv_Value.TupleGreater(hv_GlueDefectValueMin))).TupleAnd(
                            new HTuple(hv_Value.TupleLess(hv_GlueDefectValueMax)))) != 0)
                        {
                            hv_GlueGrayValue = hv_GlueGrayValue.TupleConcat(hv_Value);
                            //满足条件筛选的初步区域保存,与GlueGrayValue对应保存
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_LineUsedRegion, ho_ObjectSelected1, out ExpTmpOutVar_0
                                    );
                                ho_LineUsedRegion.Dispose();
                                ho_LineUsedRegion = ExpTmpOutVar_0;
                            }
                            //
                        }
                        else
                        {
                            //超出初步设置的范围，NG区域保存
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_DefectObjectOut, ho_ObjectSelected1, out ExpTmpOutVar_0
                                    );
                                ho_DefectObjectOut.Dispose();
                                ho_DefectObjectOut = ExpTmpOutVar_0;
                            }
                        }
                    }
                    HOperatorSet.TupleLength(hv_GlueGrayValue, out hv_Length);
                    HOperatorSet.TupleGenSequence(200, 200 + ((hv_Length - 1) * 10), 10, out hv_MxCol);
                    HOperatorSet.TupleLength(hv_MxCol, out hv_len);
                    HOperatorSet.TupleGenConst(hv_len, 3, out hv_r);
                    hv_Mb = hv_GlueGrayValue / 100;
                    hv_MyRow = hv_Mb.Clone();
                    //stop ()
                    //异常停留检查  2019-9-21
                    ho_ContCircle.Dispose();
                    HOperatorSet.GenCircle(out ho_ContCircle, hv_MyRow, hv_MxCol, hv_r);
                    ho_Contour1.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_MyRow, hv_MxCol);
                    HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    ho_TestRegionLines.Dispose();
                    HOperatorSet.GenRegionLine(out ho_TestRegionLines, hv_RowBegin, hv_ColBegin,
                        hv_RowEnd, hv_ColEnd);
                    HOperatorSet.DistancePl(hv_MyRow, hv_MxCol, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                        hv_ColEnd, out hv_Distance);
                    //判断同一区域内，胶路相对高度不良区域
                    HOperatorSet.TupleGreaterElem(hv_Distance, hv_BigValue, out hv_Greater1);
                    HOperatorSet.TupleFind(hv_Greater1, 1, out hv_Indices1);
                    if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
                    {
                        HOperatorSet.TupleSelect(hv_Distance, hv_Indices1, out hv_Selected);
                        ho_ObjectSelected3.Dispose();
                        HOperatorSet.SelectObj(ho_LineUsedRegion, out ho_ObjectSelected3, hv_Indices1 + 1);
                        ho_RegionUnion1.Dispose();
                        HOperatorSet.Union1(ho_ObjectSelected3, out ho_RegionUnion1);
                        ho_RegionClosing1.Dispose();
                        HOperatorSet.ClosingCircle(ho_RegionUnion1, out ho_RegionClosing1, 3.5);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectObjectOut, ho_RegionClosing1, out ExpTmpOutVar_0
                                );
                            ho_DefectObjectOut.Dispose();
                            ho_DefectObjectOut = ExpTmpOutVar_0;
                        }
                    }
                }
                //筛选胶路缺陷
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_DefectObjectOut, out ho_RegionUnion);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GlueImageAffineTrans, ho_RegionUnion, out ho_ImageReduced
                    );
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 30000, 40000);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 3.5);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                ho_GlueDefectSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_GlueDefectSelectedRegions,
                    "area", "and", hv_GlueDefectAreaMin, 999999999);
                //
                //
                //
                //*******求取中心胶路高度段差缺陷*******
                //
                switch (hv_CheckEdgeSide.I)
                {
                    case 1:
                        hv_MoveDistanceGlueHeight = -20;
                        hv_MoveDistanceGlueInner = -10;
                        hv_MoveDistanceLineEdge = 6;
                        break;
                    case 2:
                        hv_MoveDistanceGlueHeight = 25;
                        hv_MoveDistanceGlueInner = 15;
                        hv_MoveDistanceLineEdge = -6;
                        break;
                    case 3:
                        hv_MoveDistanceGlueHeight = 20;
                        hv_MoveDistanceGlueInner = 10;
                        hv_MoveDistanceLineEdge = -6;
                        break;
                    case 4:
                        hv_MoveDistanceGlueHeight = -20;
                        hv_MoveDistanceGlueInner = -10;
                        hv_MoveDistanceLineEdge = 6;
                        break;
                    default:
                        break;
                }
                //
                //
                //
                //胶路高
                ho_RegionMoved.Dispose();
                HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, hv_MoveDistanceGlueHeight);
                ho_RegionDilationGlueCenter.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionMoved, out ho_RegionDilationGlueCenter,
                    5, 5);
                //胶路靠近壳内边
                ho_RegionMoved4.Dispose();
                HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved4, 0, hv_MoveDistanceGlueInner);
                ho_RegionDilationGlueInner.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionMoved4, out ho_RegionDilationGlueInner,
                    5, 5);
                //
                //外壳边
                ho_RegionMoved1.Dispose();
                HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved1, 0, hv_MoveDistanceLineEdge);
                ho_RegionDilationLineEdge.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionMoved1, out ho_RegionDilationLineEdge,
                    5, 5);
                //
                //
                //
                HOperatorSet.AreaCenter(ho_RegionLines, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, (new HTuple(90)).TupleRad()
                    , hv_Row, hv_Column, out hv_HomMat2DRotate);
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_RegionLines, out ho_RegionAffineTrans, hv_HomMat2DRotate,
                    "nearest_neighbor");
                ho_RegionDilation2.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionAffineTrans, out ho_RegionDilation2,
                    5, 5);
                HOperatorSet.SmallestRectangle2(ho_RegionLines, out hv_Row1, out hv_Column1,
                    out hv_Phi, out hv_Length1, out hv_Length2);
                hv_LengthHeigth = hv_Length1 * 2;
                ho_RegionMoved3.Dispose();
                HOperatorSet.MoveRegion(ho_RegionDilation2, out ho_RegionMoved3, -hv_Length1,
                    0);
                //
                //
                //***间隔设定距离偏移
                hv_DistanceHeight = 5;
                hv_GlueHeightDefectAreaMin = 200;
                hv_NumberHeight = hv_LengthHeigth / hv_DistanceHeight;
                ho_GlueHeightDefect.Dispose();
                HOperatorSet.GenEmptyObj(out ho_GlueHeightDefect);
                HTuple end_val143 = hv_NumberHeight;
                HTuple step_val143 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val143, step_val143); hv_Index = hv_Index.TupleAdd(step_val143))
                {
                    ho_RegionMoved2.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionMoved3, out ho_RegionMoved2, hv_DistanceHeight * hv_Index,
                        0);
                    ho_RegionIntersectionGlue.Dispose();
                    HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationGlueCenter, out ho_RegionIntersectionGlue
                        );
                    ho_RegionIntersectionInnerGlue.Dispose();
                    HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationGlueInner, out ho_RegionIntersectionInnerGlue
                        );
                    ho_RegionIntersectionEdge.Dispose();
                    HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationLineEdge, out ho_RegionIntersectionEdge
                        );
                    //
                    HOperatorSet.GrayFeatures(ho_RegionIntersectionGlue, ho_GlueImageAffineTrans,
                        "median", out hv_GlueCenterValue);
                    HOperatorSet.GrayFeatures(ho_RegionIntersectionInnerGlue, ho_GlueImageAffineTrans,
                        "median", out hv_GlueInnerValue);
                    HOperatorSet.GrayFeatures(ho_RegionIntersectionEdge, ho_GlueImageAffineTrans,
                        "median", out hv_LineEdgeValue);
                    hv_GlueEdgeHeight = hv_LineEdgeValue - hv_GlueCenterValue;
                    hv_GlueInnerEdgeHeight = hv_LineEdgeValue - hv_GlueInnerValue;
                    //判断胶路与外壳高度不良
                    // hv_GlueEdgeHeightMax = 3200;
                    hv_GlueEdgeHeightMin = 1000;
                    if ((int)((new HTuple(hv_GlueEdgeHeight.TupleGreater(hv_GlueEdgeHeightMax))).TupleOr(
                        new HTuple(hv_GlueEdgeHeight.TupleLess(hv_GlueEdgeHeightMin)))) != 0)
                    {
                        //胶高不良
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_GlueHeightDefect, ho_RegionIntersectionGlue,
                                out ExpTmpOutVar_0);
                            ho_GlueHeightDefect.Dispose();
                            ho_GlueHeightDefect = ExpTmpOutVar_0;
                        }
                    }
                    //判断胶路溢胶与外壳高度不良
                    // hv_GlueInnerEdgeHeightMax = 3200;
                    hv_GlueInnerEdgeHeightMin = 800;
                    if ((int)((new HTuple(hv_GlueInnerEdgeHeight.TupleGreater(hv_GlueInnerEdgeHeightMax))).TupleOr(
                        new HTuple(hv_GlueInnerEdgeHeight.TupleLess(hv_GlueInnerEdgeHeightMin)))) != 0)
                    {
                        //胶和壳太近，溢胶
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_GlueHeightDefect, ho_RegionIntersectionInnerGlue,
                                out ExpTmpOutVar_0);
                            ho_GlueHeightDefect.Dispose();
                            ho_GlueHeightDefect = ExpTmpOutVar_0;
                        }
                        //
                    }
                }
                ho_RegionUnion2.Dispose();
                HOperatorSet.Union1(ho_GlueHeightDefect, out ho_RegionUnion2);
                ho_RegionClosing2.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionUnion2, out ho_RegionClosing2, 3.5);
                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionClosing2, out ho_ConnectedRegions1);
                ho_GlueHeightSelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_GlueHeightSelectedRegions,
                    "area", "and", hv_GlueHeightDefectAreaMin, 999999999);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_GlueDefectSelectedRegions, ho_GlueHeightSelectedRegions,
                        out ExpTmpOutVar_0);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                //
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                //
                ho_DefectObjectOut.Dispose();
                ho_LineUsedRegion.Dispose();
                ho_RegionMoved.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionDilationGlueCenter.Dispose();
                ho_RegionMoved4.Dispose();
                ho_RegionDilationGlueInner.Dispose();
                ho_RegionMoved1.Dispose();
                ho_RegionDilationLineEdge.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_RegionDilation2.Dispose();
                ho_RegionMoved3.Dispose();
                ho_GlueHeightDefect.Dispose();
                ho_RegionMoved2.Dispose();
                ho_RegionIntersectionGlue.Dispose();
                ho_RegionIntersectionInnerGlue.Dispose();
                ho_RegionIntersectionEdge.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionClosing2.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_GlueHeightSelectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DefectObjectOut.Dispose();
                ho_LineUsedRegion.Dispose();
                ho_RegionMoved.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected3.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionDilationGlueCenter.Dispose();
                ho_RegionMoved4.Dispose();
                ho_RegionDilationGlueInner.Dispose();
                ho_RegionMoved1.Dispose();
                ho_RegionDilationLineEdge.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_RegionDilation2.Dispose();
                ho_RegionMoved3.Dispose();
                ho_GlueHeightDefect.Dispose();
                ho_RegionMoved2.Dispose();
                ho_RegionIntersectionGlue.Dispose();
                ho_RegionIntersectionInnerGlue.Dispose();
                ho_RegionIntersectionEdge.Dispose();
                ho_RegionUnion2.Dispose();
                ho_RegionClosing2.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_GlueHeightSelectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void CheckGlueDefect2Modul(HObject ho_GlueImageAffineTrans, HObject ho_RegionLines,
            out HObject ho_GlueDefectSelectedRegions, out HObject ho_DefectRegion1, out HObject ho_DefectRegion2,
            out HObject ho_DefectRegion3, HTuple hv_CheckEdgeSide, HTuple hv_MoveDistance,
            HTuple hv_WH, HTuple hv_BigValue, HTuple hv_Step, HTuple hv_GlueDefectAreaMin,
            HTuple hv_GlueEdgeHeightMax, HTuple hv_GlueInnerEdgeHeightMax, HTuple hv_ShortValue,
            HTuple hv_MoveGlueHeight, HTuple hv_MoveGlueInner, HTuple hv_MoveLineEdge)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_DefectObjectOut, ho_Contours, ho_Polygons;
            HObject ho_ContCircle3, ho_RegionLinesIn, ho_LineUsedRegion = null;
            HObject ho_RegionMoved = null, ho_Rectangle1 = null, ho_ObjectSelected1 = null;
            HObject ho_ContCircle = null, ho_Contour1 = null, ho_TestRegionLines = null;
            HObject ho_ObjectSelected3 = null, ho_RegionUnion1 = null, ho_RegionClosing1 = null;
            HObject ho_RegionUnion, ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_ConnectedRegions, ho_RegionDilationGlueCenter;
            HObject ho_RegionMoved4, ho_RegionDilationGlueInner, ho_RegionMoved1;
            HObject ho_RegionDilationLineEdge, ho_RegionAffineTrans;
            HObject ho_RegionDilation2, ho_RegionMoved3, ho_GlueHeightDefect;
            HObject ho_RegionMoved2 = null, ho_RegionIntersectionGlue = null;
            HObject ho_RegionIntersectionInnerGlue = null, ho_RegionIntersectionEdge = null;
            HObject ho_RegionUnion2, ho_RegionClosing2, ho_ConnectedRegions1;
            HObject ho_GlueHeightSelectedRegions, ho_CheckGlueRegionMoved = null;
            HObject ho_ImageScaled, ho_ObjectsConcat, ho_RegionUnion3;
            HObject ho_RegionClosing3, ho_ImageReduced1, ho_ImageScaleMax;
            HObject ho_ImageOpening, ho_Regions, ho_SelectedRegions;
            HObject ho_RegionDifference, ho_RegionOpening, ho_ConnectedRegions2;
            HObject ho_SelectedRegions1, ho_BreakGlue, ho_ObjectSelected = null;
            HObject ho_RegionDilation = null, ho_RegionDifference1 = null;
            HObject ho_RegionIntersection = null;

            // Local control variables 

            HTuple hv_BeginRow = null, hv_BeginCol = null;
            HTuple hv_EndRow = null, hv_EndCol = null, hv_Length = null;
            HTuple hv_Phi = null, hv_centerRow = null, hv_centerCol = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_IsOverlapping1 = null;
            HTuple hv_Index2 = null, hv_GlueGrayValue = new HTuple();
            HTuple hv_Rows2 = new HTuple(), hv_Columns1 = new HTuple();
            HTuple hv_RCSequence = new HTuple(), hv_RowSelected = new HTuple();
            HTuple hv_ColSelected = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple(), hv_Value = new HTuple();
            HTuple hv_GlueDefectValueMin = new HTuple(), hv_GlueDefectValueMax = new HTuple();
            HTuple hv_MxCol = new HTuple(), hv_len = new HTuple();
            HTuple hv_r = new HTuple(), hv_Mb = new HTuple(), hv_MyRow = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Greater1 = new HTuple();
            HTuple hv_Indices1 = new HTuple(), hv_Selected = new HTuple();
            HTuple hv_MoveDistanceGlueHeight = new HTuple(), hv_MoveDistanceGlueInner = new HTuple();
            HTuple hv_MoveDistanceLineEdge = new HTuple(), hv_Area = null;
            HTuple hv_Row = null, hv_Column = null, hv_HomMat2DIdentity = null;
            HTuple hv_HomMat2DRotate = null, hv_Length1 = null, hv_Length2 = null;
            HTuple hv_LengthHeigth = null, hv_DistanceHeight = null;
            HTuple hv_GlueHeightDefectAreaMin = null, hv_NumberHeight = null;
            HTuple hv_Index = null, hv_GlueCenterValue = new HTuple();
            HTuple hv_GlueInnerValue = new HTuple(), hv_LineEdgeValue = new HTuple();
            HTuple hv_GlueEdgeHeight = new HTuple(), hv_GlueInnerEdgeHeight = new HTuple();
            HTuple hv_GlueEdgeHeightMin = new HTuple(), hv_GlueInnerEdgeHeightMin = new HTuple();
            HTuple hv_Value1 = null, hv_SUBvalue = null, hv_Width = null;
            HTuple hv_Height = null, hv_AbsoluteHisto = null, hv_RelativeHisto = null;
            HTuple hv_MinThresh = null, hv_MaxThresh = null, hv_Number = null;
            HTuple hv_Index3 = null, hv_OutValue2 = new HTuple(), hv_InnerValue2 = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_DeltaValue = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_DefectRegion1);
            HOperatorSet.GenEmptyObj(out ho_DefectRegion2);
            HOperatorSet.GenEmptyObj(out ho_DefectRegion3);
            HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Polygons);
            HOperatorSet.GenEmptyObj(out ho_ContCircle3);
            HOperatorSet.GenEmptyObj(out ho_RegionLinesIn);
            HOperatorSet.GenEmptyObj(out ho_LineUsedRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_TestRegionLines);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationGlueCenter);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved4);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationGlueInner);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved1);
            HOperatorSet.GenEmptyObj(out ho_RegionDilationLineEdge);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation2);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved3);
            HOperatorSet.GenEmptyObj(out ho_GlueHeightDefect);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved2);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionGlue);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionInnerGlue);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersectionEdge);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_GlueHeightSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_CheckGlueRegionMoved);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion3);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing3);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageScaleMax);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_BreakGlue);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            ho_DefectObjectOut.Dispose();
            HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
            ho_GlueDefectSelectedRegions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
            //
            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_RegionLines, out ho_Contours, "border");
            ho_Polygons.Dispose();
            HOperatorSet.GenPolygonsXld(ho_Contours, out ho_Polygons, "ramer", 2);
            HOperatorSet.GetLinesXld(ho_Polygons, out hv_BeginRow, out hv_BeginCol, out hv_EndRow,
                out hv_EndCol, out hv_Length, out hv_Phi);
            //
            //截取线段，壳两头有翘起来
            hv_centerRow = ((hv_BeginRow.TupleSelect(0)) + (hv_EndRow.TupleSelect(0))) / 2;
            hv_centerCol = ((hv_BeginCol.TupleSelect(0)) + (hv_EndCol.TupleSelect(0))) / 2;
            //
            ho_ContCircle3.Dispose();
            HOperatorSet.GenCircleContourXld(out ho_ContCircle3, hv_centerRow, hv_centerCol,
                hv_ShortValue, 0, 6.28318, "positive", 1);
            HOperatorSet.IntersectionLineContourXld(ho_ContCircle3, hv_BeginRow.TupleSelect(
                0), hv_BeginCol.TupleSelect(0), hv_EndRow.TupleSelect(0), hv_EndCol.TupleSelect(
                0), out hv_Row1, out hv_Column1, out hv_IsOverlapping1);
            ho_RegionLinesIn.Dispose();
            HOperatorSet.GenRegionLine(out ho_RegionLinesIn, hv_Row1.TupleSelect(0), hv_Column1.TupleSelect(
                0), hv_Row1.TupleSelect(1), hv_Column1.TupleSelect(1));
            //
            //最小二乘法整条竖线检测
            HTuple end_val16 = hv_Step;
            HTuple step_val16 = 1;
            for (hv_Index2 = 1; hv_Index2.Continue(end_val16, step_val16); hv_Index2 = hv_Index2.TupleAdd(step_val16))
            {
                hv_GlueGrayValue = new HTuple();
                ho_LineUsedRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_LineUsedRegion);
                //
                switch (hv_CheckEdgeSide.I)
                {
                    case 1:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved, 0, (-hv_MoveDistance) - ((2 * hv_WH) * hv_Index2));
                        break;
                    case 2:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved, 0, hv_MoveDistance + ((2 * hv_WH) * hv_Index2));
                        break;
                    case 3:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved, 0, hv_MoveDistance + ((2 * hv_WH) * hv_Index2));
                        break;
                    case 4:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved, 0, (-hv_MoveDistance) - ((2 * hv_WH) * hv_Index2));
                        break;
                    //
                    default:
                        break;
                }
                //
                //
                HOperatorSet.GetRegionPoints(ho_RegionMoved, out hv_Rows2, out hv_Columns1);
                HOperatorSet.TupleGenSequence(0, (new HTuple(hv_Rows2.TupleLength())) - 1, hv_MoveDistance,
                    out hv_RCSequence);
                HOperatorSet.TupleSelect(hv_Rows2, hv_RCSequence, out hv_RowSelected);
                HOperatorSet.TupleSelect(hv_Columns1, hv_RCSequence, out hv_ColSelected);
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle1, hv_RowSelected - hv_WH, hv_ColSelected - hv_WH,
                    hv_RowSelected + hv_WH, hv_ColSelected + hv_WH);
                HOperatorSet.CountObj(ho_Rectangle1, out hv_Number1);
                HTuple end_val45 = hv_Number1;
                HTuple step_val45 = 2;
                for (hv_Index1 = 1; hv_Index1.Continue(end_val45, step_val45); hv_Index1 = hv_Index1.TupleAdd(step_val45))
                {
                    ho_ObjectSelected1.Dispose();
                    HOperatorSet.SelectObj(ho_Rectangle1, out ho_ObjectSelected1, hv_Index1);
                    HOperatorSet.GrayFeatures(ho_ObjectSelected1, ho_GlueImageAffineTrans, "median",
                        out hv_Value);
                    //在阈值范围内才选取是胶路
                    hv_GlueDefectValueMin = 25000;
                    hv_GlueDefectValueMax = 38000;
                    if ((int)((new HTuple(hv_Value.TupleGreater(hv_GlueDefectValueMin))).TupleAnd(
                        new HTuple(hv_Value.TupleLess(hv_GlueDefectValueMax)))) != 0)
                    {
                        hv_GlueGrayValue = hv_GlueGrayValue.TupleConcat(hv_Value);
                        //满足条件筛选的初步区域保存,与GlueGrayValue对应保存
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_LineUsedRegion, ho_ObjectSelected1, out ExpTmpOutVar_0
                                );
                            ho_LineUsedRegion.Dispose();
                            ho_LineUsedRegion = ExpTmpOutVar_0;
                        }
                    }
                    else
                    {
                        //超出初步设置的范围，NG区域保存
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectObjectOut, ho_ObjectSelected1, out ExpTmpOutVar_0
                                );
                            ho_DefectObjectOut.Dispose();
                            ho_DefectObjectOut = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.TupleLength(hv_GlueGrayValue, out hv_Length);
                HOperatorSet.TupleGenSequence(200, 200 + ((hv_Length - 1) * 10), 10, out hv_MxCol);
                HOperatorSet.TupleLength(hv_MxCol, out hv_len);
                HOperatorSet.TupleGenConst(hv_len, 3, out hv_r);
                hv_Mb = hv_GlueGrayValue / 100;
                hv_MyRow = hv_Mb.Clone();
                //stop ()
                //异常停留检查  2019-9-21
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircle(out ho_ContCircle, hv_MyRow, hv_MxCol, hv_r);
                ho_Contour1.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_MyRow, hv_MxCol);
                HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                ho_TestRegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_TestRegionLines, hv_RowBegin, hv_ColBegin,
                    hv_RowEnd, hv_ColEnd);
                HOperatorSet.DistancePl(hv_MyRow, hv_MxCol, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                    hv_ColEnd, out hv_Distance);
                //判断同一区域内，胶路相对高度不良区域
                HOperatorSet.TupleGreaterElem(hv_Distance, hv_BigValue, out hv_Greater1);
                HOperatorSet.TupleFind(hv_Greater1, 1, out hv_Indices1);
                if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.TupleSelect(hv_Distance, hv_Indices1, out hv_Selected);
                    ho_ObjectSelected3.Dispose();
                    HOperatorSet.SelectObj(ho_LineUsedRegion, out ho_ObjectSelected3, hv_Indices1 + 1);
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_ObjectSelected3, out ho_RegionUnion1);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionUnion1, out ho_RegionClosing1, 3.5);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_DefectObjectOut, ho_RegionClosing1, out ExpTmpOutVar_0
                            );
                        ho_DefectObjectOut.Dispose();
                        ho_DefectObjectOut = ExpTmpOutVar_0;
                    }
                }
            }
            //筛选胶路缺陷
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_DefectObjectOut, out ho_RegionUnion);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_GlueImageAffineTrans, ho_RegionUnion, out ho_ImageReduced
                );
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 30000, 40000);
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 3.5);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
            ho_GlueDefectSelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_GlueDefectSelectedRegions,
                "area", "and", hv_GlueDefectAreaMin, 999999999);
            ho_DefectRegion1.Dispose();
            ho_DefectRegion1 = ho_GlueDefectSelectedRegions.CopyObj(1, -1);
            //
            //
            //*******求取中心胶路高度段差缺陷*******
            //
            switch (hv_CheckEdgeSide.I)
            {
                case 1:
                    //hv_MoveDistanceGlueHeight = -20;
                    //hv_MoveDistanceGlueInner = -10;
                    //hv_MoveDistanceLineEdge = 6;
                    hv_MoveDistanceGlueHeight = -1 * hv_MoveGlueHeight;
                    hv_MoveDistanceGlueInner = -1 * hv_MoveGlueInner;
                    hv_MoveDistanceLineEdge = 1 * hv_MoveLineEdge;
                    break;
                case 2:
                    //hv_MoveDistanceGlueHeight = 25;
                    //hv_MoveDistanceGlueInner = 15;
                    //hv_MoveDistanceLineEdge = -6;
                    hv_MoveDistanceGlueHeight = 1 * hv_MoveGlueHeight;
                    hv_MoveDistanceGlueInner = 1 * hv_MoveGlueInner;
                    hv_MoveDistanceLineEdge = -1 * hv_MoveLineEdge;
                    break;
                case 3:
                    //hv_MoveDistanceGlueHeight = 20;
                    //hv_MoveDistanceGlueInner = 10;
                    //hv_MoveDistanceLineEdge = -6;
                    hv_MoveDistanceGlueHeight = 1 * hv_MoveGlueHeight;
                    hv_MoveDistanceGlueInner = 1 * hv_MoveGlueInner;
                    hv_MoveDistanceLineEdge = -1 * hv_MoveLineEdge;
                    break;
                case 4:
                    //hv_MoveDistanceGlueHeight = -20;
                    //hv_MoveDistanceGlueInner = -10;
                    //hv_MoveDistanceLineEdge = 6;
                    hv_MoveDistanceGlueHeight = -1 * hv_MoveGlueHeight;
                    hv_MoveDistanceGlueInner = -1 * hv_MoveGlueInner;
                    hv_MoveDistanceLineEdge = 1 * hv_MoveLineEdge;
                    break;
                default:
                    break;
            }
            //
            //
            //
            //胶路高
            ho_RegionMoved.Dispose();
            HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved, 0, hv_MoveDistanceGlueHeight);
            ho_RegionDilationGlueCenter.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionMoved, out ho_RegionDilationGlueCenter,
                5, 5);
            //胶路靠近壳内边
            ho_RegionMoved4.Dispose();
            HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved4, 0, hv_MoveDistanceGlueInner);
            ho_RegionDilationGlueInner.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionMoved4, out ho_RegionDilationGlueInner,
                5, 5);
            //
            //外壳边
            ho_RegionMoved1.Dispose();
            HOperatorSet.MoveRegion(ho_RegionLinesIn, out ho_RegionMoved1, 0, hv_MoveDistanceLineEdge);
            ho_RegionDilationLineEdge.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionMoved1, out ho_RegionDilationLineEdge,
                5, 5);
            //
            //
            HOperatorSet.AreaCenter(ho_RegionLinesIn, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
            HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, (new HTuple(90)).TupleRad(),
                hv_Row, hv_Column, out hv_HomMat2DRotate);
            ho_RegionAffineTrans.Dispose();
            HOperatorSet.AffineTransRegion(ho_RegionLinesIn, out ho_RegionAffineTrans, hv_HomMat2DRotate,
                "nearest_neighbor");
            ho_RegionDilation2.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionAffineTrans, out ho_RegionDilation2,
                5, 5);
            HOperatorSet.SmallestRectangle2(ho_RegionLinesIn, out hv_Row1, out hv_Column1,
                out hv_Phi, out hv_Length1, out hv_Length2);
            hv_LengthHeigth = hv_Length1 * 2;
            ho_RegionMoved3.Dispose();
            HOperatorSet.MoveRegion(ho_RegionDilation2, out ho_RegionMoved3, -hv_Length1,
                0);
            //
            //
            //***间隔设定距离偏移
            hv_DistanceHeight = 5;
            hv_GlueHeightDefectAreaMin = hv_GlueDefectAreaMin.Clone();
            hv_NumberHeight = hv_LengthHeigth / hv_DistanceHeight;
            ho_GlueHeightDefect.Dispose();
            HOperatorSet.GenEmptyObj(out ho_GlueHeightDefect);
            HTuple end_val150 = hv_NumberHeight;
            HTuple step_val150 = 1;
            for (hv_Index = 1; hv_Index.Continue(end_val150, step_val150); hv_Index = hv_Index.TupleAdd(step_val150))
            {
                ho_RegionMoved2.Dispose();
                HOperatorSet.MoveRegion(ho_RegionMoved3, out ho_RegionMoved2, hv_DistanceHeight * hv_Index,
                    0);
                ho_RegionIntersectionGlue.Dispose();
                HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationGlueCenter, out ho_RegionIntersectionGlue
                    );
                ho_RegionIntersectionInnerGlue.Dispose();
                HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationGlueInner, out ho_RegionIntersectionInnerGlue
                    );
                ho_RegionIntersectionEdge.Dispose();
                HOperatorSet.Intersection(ho_RegionMoved2, ho_RegionDilationLineEdge, out ho_RegionIntersectionEdge
                    );
                //
                HOperatorSet.GrayFeatures(ho_RegionIntersectionGlue, ho_GlueImageAffineTrans,
                    "median", out hv_GlueCenterValue);
                HOperatorSet.GrayFeatures(ho_RegionIntersectionInnerGlue, ho_GlueImageAffineTrans,
                    "median", out hv_GlueInnerValue);
                HOperatorSet.GrayFeatures(ho_RegionIntersectionEdge, ho_GlueImageAffineTrans,
                    "median", out hv_LineEdgeValue);
                hv_GlueEdgeHeight = hv_LineEdgeValue - hv_GlueCenterValue;
                hv_GlueInnerEdgeHeight = hv_LineEdgeValue - hv_GlueInnerValue;
                //判断胶路与外壳高度不良
                //GlueEdgeHeightMax := 2600
                hv_GlueEdgeHeightMin = 1000;
                if ((int)((new HTuple(hv_GlueEdgeHeight.TupleGreater(hv_GlueEdgeHeightMax))).TupleOr(
                    new HTuple(hv_GlueEdgeHeight.TupleLess(hv_GlueEdgeHeightMin)))) != 0)
                {
                    //胶高不良
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_GlueHeightDefect, ho_RegionIntersectionGlue, out ExpTmpOutVar_0
                            );
                        ho_GlueHeightDefect.Dispose();
                        ho_GlueHeightDefect = ExpTmpOutVar_0;
                    }
                }
                //判断胶路溢胶与外壳高度不良
                //GlueInnerEdgeHeightMax := 3200
                hv_GlueInnerEdgeHeightMin = 800;
                if ((int)((new HTuple(hv_GlueInnerEdgeHeight.TupleGreater(hv_GlueInnerEdgeHeightMax))).TupleOr(
                    new HTuple(hv_GlueInnerEdgeHeight.TupleLess(hv_GlueInnerEdgeHeightMin)))) != 0)
                {
                    //胶和壳太近，溢胶
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_GlueHeightDefect, ho_RegionIntersectionInnerGlue,
                            out ExpTmpOutVar_0);
                        ho_GlueHeightDefect.Dispose();
                        ho_GlueHeightDefect = ExpTmpOutVar_0;
                    }
                    //
                }
            }
            ho_RegionUnion2.Dispose();
            HOperatorSet.Union1(ho_GlueHeightDefect, out ho_RegionUnion2);
            ho_RegionClosing2.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionUnion2, out ho_RegionClosing2, 3.5);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_RegionClosing2, out ho_ConnectedRegions1);
            ho_GlueHeightSelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_GlueHeightSelectedRegions,
                "area", "and", hv_GlueHeightDefectAreaMin, 999999999);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_GlueDefectSelectedRegions, ho_GlueHeightSelectedRegions,
                    out ExpTmpOutVar_0);
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            ho_DefectRegion2.Dispose();
            ho_DefectRegion2 = ho_GlueHeightSelectedRegions.CopyObj(1, -1);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Union1(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Connection(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            //
            //
            //
            //获取外壳的灰度值,单独提取胶路轮廓
            switch (hv_CheckEdgeSide.I)
            {
                case 1:
                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, -5);
                    //移动到截取区域
                    ho_CheckGlueRegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, -35);
                    break;
                case 2:
                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, 5);
                    ho_CheckGlueRegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, 35);
                    break;
                case 3:
                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, 5);
                    ho_CheckGlueRegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, 35);
                    break;
                case 4:
                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, -5);
                    ho_CheckGlueRegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, -35);
                    break;
                //
                default:
                    break;
            }
            //
            HOperatorSet.GrayFeatures(ho_RegionDilationLineEdge, ho_GlueImageAffineTrans,
                "mean", out hv_Value1);
            hv_SUBvalue = hv_Value1 - 3500;
            ho_ImageScaled.Dispose();
            HOperatorSet.ScaleImage(ho_GlueImageAffineTrans, out ho_ImageScaled, 1, -hv_SUBvalue);
            //
            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_CheckGlueRegionMoved, ho_RegionMoved, out ho_ObjectsConcat
                );
            ho_RegionUnion3.Dispose();
            HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion3);
            ho_RegionClosing3.Dispose();
            HOperatorSet.ClosingRectangle1(ho_RegionUnion3, out ho_RegionClosing3, 100, 100);
            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageScaled, ho_RegionClosing3, out ho_ImageReduced1
                );
            ho_ImageScaleMax.Dispose();
            HOperatorSet.ScaleImageMax(ho_ImageReduced1, out ho_ImageScaleMax);
            ho_ImageOpening.Dispose();
            HOperatorSet.GrayOpeningRect(ho_ImageScaleMax, out ho_ImageOpening, 11, 11);
            ho_Regions.Dispose();
            HOperatorSet.AutoThreshold(ho_ImageOpening, out ho_Regions, 5);
            //
            HOperatorSet.GetImageSize(ho_ImageOpening, out hv_Width, out hv_Height);
            HOperatorSet.GrayHisto(ho_ImageOpening, ho_ImageOpening, out hv_AbsoluteHisto,
                out hv_RelativeHisto);
            HOperatorSet.HistoToThresh(hv_RelativeHisto, 6, out hv_MinThresh, out hv_MaxThresh);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageOpening, out ho_Region, hv_MinThresh, hv_MaxThresh);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShapeStd(ho_Region, out ho_SelectedRegions, "max_area", 70);
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_Region, ho_SelectedRegions, out ho_RegionDifference
                );
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 3.5);
            ho_ConnectedRegions2.Dispose();
            HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions2);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions1, "area",
                "and", 300, 99999);
            //选择判断断胶
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);
            ho_BreakGlue.Dispose();
            HOperatorSet.GenEmptyObj(out ho_BreakGlue);
            HTuple end_val236 = hv_Number;
            HTuple step_val236 = 1;
            for (hv_Index3 = 1; hv_Index3.Continue(end_val236, step_val236); hv_Index3 = hv_Index3.TupleAdd(step_val236))
            {
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_SelectedRegions1, out ho_ObjectSelected, hv_Index3);
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_ObjectSelected, out ho_RegionDilation, 23.5);
                ho_RegionDifference1.Dispose();
                HOperatorSet.Difference(ho_RegionDilation, ho_ObjectSelected, out ho_RegionDifference1
                    );
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionDifference1, ho_RegionClosing3, out ho_RegionIntersection
                    );
                //
                HOperatorSet.GrayFeatures(ho_RegionIntersection, ho_ImageReduced1, "mean",
                    out hv_OutValue2);
                HOperatorSet.GrayFeatures(ho_ObjectSelected, ho_ImageReduced1, "mean", out hv_InnerValue2);
                //
                HOperatorSet.SmallestRectangle1(ho_ObjectSelected, out hv_Row11, out hv_Column11,
                    out hv_Row2, out hv_Column2);
                hv_Width = hv_Column2 - hv_Column11;
                hv_DeltaValue = hv_OutValue2 - hv_InnerValue2;
                if ((int)((new HTuple(hv_DeltaValue.TupleGreater(500))).TupleAnd(new HTuple(hv_Width.TupleGreater(
                    16)))) != 0)
                {
                    //高度差值太大是断胶
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_BreakGlue, ho_ObjectSelected, out ExpTmpOutVar_0
                            );
                        ho_BreakGlue.Dispose();
                        ho_BreakGlue = ExpTmpOutVar_0;
                    }
                }
            }
            //
            ho_DefectRegion3.Dispose();
            ho_DefectRegion3 = ho_BreakGlue.CopyObj(1, -1);
            //
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_GlueDefectSelectedRegions, ho_BreakGlue, out ExpTmpOutVar_0
                    );
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Union1(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Connection(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                ho_GlueDefectSelectedRegions.Dispose();
                ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
            }
            //
            //
            //stop ()
            ho_DefectObjectOut.Dispose();
            ho_Contours.Dispose();
            ho_Polygons.Dispose();
            ho_ContCircle3.Dispose();
            ho_RegionLinesIn.Dispose();
            ho_LineUsedRegion.Dispose();
            ho_RegionMoved.Dispose();
            ho_Rectangle1.Dispose();
            ho_ObjectSelected1.Dispose();
            ho_ContCircle.Dispose();
            ho_Contour1.Dispose();
            ho_TestRegionLines.Dispose();
            ho_ObjectSelected3.Dispose();
            ho_RegionUnion1.Dispose();
            ho_RegionClosing1.Dispose();
            ho_RegionUnion.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionClosing.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_RegionDilationGlueCenter.Dispose();
            ho_RegionMoved4.Dispose();
            ho_RegionDilationGlueInner.Dispose();
            ho_RegionMoved1.Dispose();
            ho_RegionDilationLineEdge.Dispose();
            ho_RegionAffineTrans.Dispose();
            ho_RegionDilation2.Dispose();
            ho_RegionMoved3.Dispose();
            ho_GlueHeightDefect.Dispose();
            ho_RegionMoved2.Dispose();
            ho_RegionIntersectionGlue.Dispose();
            ho_RegionIntersectionInnerGlue.Dispose();
            ho_RegionIntersectionEdge.Dispose();
            ho_RegionUnion2.Dispose();
            ho_RegionClosing2.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_GlueHeightSelectedRegions.Dispose();
            ho_CheckGlueRegionMoved.Dispose();
            ho_ImageScaled.Dispose();
            ho_ObjectsConcat.Dispose();
            ho_RegionUnion3.Dispose();
            ho_RegionClosing3.Dispose();
            ho_ImageReduced1.Dispose();
            ho_ImageScaleMax.Dispose();
            ho_ImageOpening.Dispose();
            ho_Regions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionOpening.Dispose();
            ho_ConnectedRegions2.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_BreakGlue.Dispose();
            ho_ObjectSelected.Dispose();
            ho_RegionDilation.Dispose();
            ho_RegionDifference1.Dispose();
            ho_RegionIntersection.Dispose();

            return;
        }

        public static void CheckGlueDefect2Modul_Fourth(HObject ho_GlueImageAffineTrans, HObject ho_RegionLines,
        HObject ho_RegionRectangle1, HObject ho_RegionRectangle2, HObject ho_RegionRectangle3,
        out HObject ho_GlueDefectSelectedRegions, HTuple hv_CheckEdgeSide, HTuple hv_BigValue,
        HTuple hv_ShortValue, HTuple hv_GlueDefectAreaMin, HTuple hv_MoveGlueHeight,
        HTuple hv_MoveGlueInner, HTuple hv_MoveLineEdge, HTuple hv_DistanceUpMin, HTuple hv_DistanceUpMax,
        HTuple hv_DistanceMidMin, HTuple hv_DistanceMidMax, HTuple hv_DistanceDownMin,
        HTuple hv_DistanceDownMax, HTuple hv_SubGrayValue)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_DefectObjectOut, ho_Contours, ho_Polygons;
            HObject ho_ContCircle3, ho_RegionLinesIn, ho_RegionUnion;
            HObject ho_RegionMoved = null, ho_CheckGlueRegionMoved = null;
            HObject ho_EdgeRegionMoved5 = null, ho_RegionDilation1 = null;
            HObject ho_ImageScaled, ho_ObjectsConcat, ho_RegionUnion3;
            HObject ho_RegionClosing3, ho_ImageReduced1, ho_ImageScaleMax;
            HObject ho_ImageOpening, ho_Partitioned, ho_ObjectSelected2 = null;
            HObject ho_Partitioned1 = null, ho_ObjectSelected4 = null, ho_RegionUnion4 = null;
            HObject ho_Cross = null, ho_Cross1, ho_Contour, ho_RegionLines1;
            HObject ho_DistanceDefectRegion, ho_Circle1 = null, ho_Circle = null;
            HObject ho_Circle2, ho_GlueHeightObject, ho_ContCircle;
            HObject ho_Contour1, ho_TestRegionLines, ho_ObjectSelected5 = null;
            HObject ho_RegionUnion1 = null, ho_RegionClosing1 = null, ho_ObjectsConcat1;

            // Local control variables 

            HTuple hv_BeginRow = null, hv_BeginCol = null;
            HTuple hv_EndRow = null, hv_EndCol = null, hv_Length = null;
            HTuple hv_Phi = null, hv_centerRow = null, hv_centerCol = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_IsOverlapping1 = null;
            HTuple hv_Value1 = null, hv_SUBvalue = null, hv_CenterRow = null;
            HTuple hv_CenterCol = null, hv_GlueHeightValue = null;
            HTuple hv_Number2 = null, hv_Index4 = null, hv_Value2 = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_Rows = null, hv_Columns = null;
            HTuple hv_RowBegin1 = null, hv_ColBegin1 = null, hv_RowEnd1 = null;
            HTuple hv_ColEnd1 = null, hv_Nr1 = null, hv_Nc1 = null;
            HTuple hv_Dist1 = null, hv_GlueHeight = null, hv_Index5 = null;
            HTuple hv_CenterRowSelected = new HTuple(), hv_CenterColSelected = new HTuple();
            HTuple hv_Value3 = new HTuple(), hv_Distance1 = new HTuple();
            HTuple hv_IsInsideUp = new HTuple(), hv_IsInsideMiddle = new HTuple();
            HTuple hv_IsInsideDown = new HTuple(), hv_DistanceMin = new HTuple();
            HTuple hv_DistanceMax = new HTuple(), hv_Newtuple = null;
            HTuple hv_MxCol = null, hv_len = null, hv_r = null, hv_Mb = null;
            HTuple hv_MyRow = null, hv_RowBegin = null, hv_ColBegin = null;
            HTuple hv_RowEnd = null, hv_ColEnd = null, hv_Nr = null;
            HTuple hv_Nc = null, hv_Dist = null, hv_Distance = null;
            HTuple hv_Greater1 = null, hv_Indices1 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Polygons);
            HOperatorSet.GenEmptyObj(out ho_ContCircle3);
            HOperatorSet.GenEmptyObj(out ho_RegionLinesIn);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_CheckGlueRegionMoved);
            HOperatorSet.GenEmptyObj(out ho_EdgeRegionMoved5);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation1);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion3);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing3);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageScaleMax);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_Partitioned);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);
            HOperatorSet.GenEmptyObj(out ho_Partitioned1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected4);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion4);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_RegionLines1);
            HOperatorSet.GenEmptyObj(out ho_DistanceDefectRegion);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out ho_GlueHeightObject);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_TestRegionLines);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected5);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat1);
            try
            {
                ho_DefectObjectOut.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectObjectOut);
                ho_GlueDefectSelectedRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_GlueDefectSelectedRegions);

                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionLines, out ho_Contours, "border");
                ho_Polygons.Dispose();
                HOperatorSet.GenPolygonsXld(ho_Contours, out ho_Polygons, "ramer", 2);
                HOperatorSet.GetLinesXld(ho_Polygons, out hv_BeginRow, out hv_BeginCol, out hv_EndRow,
                    out hv_EndCol, out hv_Length, out hv_Phi);

                //截取线段，壳两头有翘起来
                hv_centerRow = ((hv_BeginRow.TupleSelect(0)) + (hv_EndRow.TupleSelect(0))) / 2;
                hv_centerCol = ((hv_BeginCol.TupleSelect(0)) + (hv_EndCol.TupleSelect(0))) / 2;

                //gen_circle_contour_xld (ContCircle3, centerRow, centerCol, ShortValue, 0, 6.28318, 'positive', 1)
                ho_ContCircle3.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle3, hv_centerRow, hv_centerCol,
                    hv_ShortValue, 0, 6.28318, "positive", 1);
                HOperatorSet.IntersectionLineContourXld(ho_ContCircle3, hv_BeginRow.TupleSelect(
                    0), hv_BeginCol.TupleSelect(0), hv_EndRow.TupleSelect(0), hv_EndCol.TupleSelect(
                    0), out hv_Row1, out hv_Column1, out hv_IsOverlapping1);
                ho_RegionLinesIn.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLinesIn, hv_Row1.TupleSelect(0), hv_Column1.TupleSelect(
                    0), hv_Row1.TupleSelect(1), hv_Column1.TupleSelect(1));



                //获取外壳的灰度值,单独提取胶路轮廓
                switch (hv_CheckEdgeSide.I)
                {
                    case 1:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, -5);
                        //移动到截取区域
                        ho_CheckGlueRegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, -35);
                        break;
                    case 2:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, 5);
                        ho_CheckGlueRegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, 35);
                        break;
                    case 3:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, 5);
                        ho_CheckGlueRegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, 35);
                        break;
                    case 4:
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_RegionMoved, 0, -1 * hv_MoveGlueInner);
                        ho_CheckGlueRegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_CheckGlueRegionMoved, 0, -1 * hv_MoveGlueHeight);
                        ho_EdgeRegionMoved5.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionLines, out ho_EdgeRegionMoved5, 0, hv_MoveLineEdge);
                        ho_RegionDilation1.Dispose();
                        HOperatorSet.DilationRectangle1(ho_EdgeRegionMoved5, out ho_RegionDilation1,
                            5, 1);

                        break;

                    default:
                        break;
                }


                //使用胶路外壳边缘高度,选择固定的胶路区域******************************
                HOperatorSet.GrayFeatures(ho_RegionDilation1, ho_GlueImageAffineTrans, "mean",
                    out hv_Value1);
                hv_SUBvalue = hv_Value1 - hv_SubGrayValue;
                ho_ImageScaled.Dispose();
                HOperatorSet.ScaleImage(ho_GlueImageAffineTrans, out ho_ImageScaled, 1, -hv_SUBvalue);

                ho_ObjectsConcat.Dispose();
                HOperatorSet.ConcatObj(ho_CheckGlueRegionMoved, ho_RegionMoved, out ho_ObjectsConcat
                    );
                ho_RegionUnion3.Dispose();
                HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion3);
                ho_RegionClosing3.Dispose();
                HOperatorSet.ClosingRectangle1(ho_RegionUnion3, out ho_RegionClosing3, 100,
                    100);
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageScaled, ho_RegionClosing3, out ho_ImageReduced1
                    );
                ho_ImageScaleMax.Dispose();
                HOperatorSet.ScaleImageMax(ho_ImageReduced1, out ho_ImageScaleMax);
                ho_ImageOpening.Dispose();
                HOperatorSet.GrayOpeningRect(ho_ImageScaleMax, out ho_ImageOpening, 11, 11);
                //*****需要图像处理,选择最高点
                hv_CenterRow = new HTuple();
                hv_CenterCol = new HTuple();
                hv_GlueHeightValue = new HTuple();
                ho_Partitioned.Dispose();
                HOperatorSet.PartitionRectangle(ho_RegionClosing3, out ho_Partitioned, 200,
                    4);
                HOperatorSet.CountObj(ho_Partitioned, out hv_Number2);
                HTuple end_val152 = hv_Number2;
                HTuple step_val152 = 1;
                for (hv_Index4 = 1; hv_Index4.Continue(end_val152, step_val152); hv_Index4 = hv_Index4.TupleAdd(step_val152))
                {
                    ho_ObjectSelected2.Dispose();
                    HOperatorSet.SelectObj(ho_Partitioned, out ho_ObjectSelected2, hv_Index4);
                    ho_Partitioned1.Dispose();
                    HOperatorSet.PartitionRectangle(ho_ObjectSelected2, out ho_Partitioned1,
                        2, 5);
                    HOperatorSet.GrayFeatures(ho_Partitioned1, ho_ImageOpening, "mean", out hv_Value2);
                    HOperatorSet.TupleMax(hv_Value2, out hv_Max);
                    HOperatorSet.TupleFind(hv_Value2, hv_Max, out hv_Indices);
                    ho_ObjectSelected4.Dispose();
                    HOperatorSet.SelectObj(ho_Partitioned1, out ho_ObjectSelected4, hv_Indices + 1);
                    ho_RegionUnion4.Dispose();
                    HOperatorSet.Union1(ho_ObjectSelected4, out ho_RegionUnion4);
                    HOperatorSet.AreaCenter(ho_RegionUnion4, out hv_Area1, out hv_Row3, out hv_Column3);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row3, hv_Column3, 6, 0);
                    hv_CenterRow = hv_CenterRow.TupleConcat(hv_Row3);
                    hv_CenterCol = hv_CenterCol.TupleConcat(hv_Column3);
                    hv_GlueHeightValue = hv_GlueHeightValue.TupleConcat(hv_Max);

                }
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_CenterRow, hv_CenterCol,
                    6, 0);
                //分三个区域 上中下，到边缘的距离不相等,胶路中心高度到 外壳边缘距离异常
                HOperatorSet.GetRegionPoints(ho_RegionLines, out hv_Rows, out hv_Columns);
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Columns);
                HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_RowBegin1,
                    out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr1, out hv_Nc1,
                    out hv_Dist1);
                ho_RegionLines1.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_RowBegin1, hv_ColBegin1,
                    hv_RowEnd1, hv_ColEnd1);
                //上区域
                //gen_rectangle1 (Rectangle, 150, 576, 278, 784)
                //中区域
                //gen_rectangle1 (Rectangle2, 280, 543, 516, 776)
                //下区域
                //gen_rectangle1 (Rectangle4, 517, 547, 646, 775)

                ho_DistanceDefectRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DistanceDefectRegion);
                hv_GlueHeight = new HTuple();
                for (hv_Index5 = 0; (int)hv_Index5 <= (int)((new HTuple(hv_CenterRow.TupleLength()
                    )) - 1); hv_Index5 = (int)hv_Index5 + 1)
                {
                    HOperatorSet.TupleSelect(hv_CenterRow, hv_Index5, out hv_CenterRowSelected);
                    HOperatorSet.TupleSelect(hv_CenterCol, hv_Index5, out hv_CenterColSelected);
                    //获取胶最高值
                    ho_Circle1.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle1, hv_CenterRowSelected, hv_CenterColSelected,
                        2);
                    HOperatorSet.GrayFeatures(ho_Circle1, ho_ImageOpening, "mean", out hv_Value3);
                    hv_GlueHeight = hv_GlueHeight.TupleConcat(hv_Value3);
                    HOperatorSet.DistancePl(hv_CenterRowSelected, hv_CenterColSelected, hv_RowBegin1,
                        hv_ColBegin1, hv_RowEnd1, hv_ColEnd1, out hv_Distance1);

                    HOperatorSet.TestRegionPoint(ho_RegionRectangle1, hv_CenterRowSelected, hv_CenterColSelected,
                        out hv_IsInsideUp);
                    HOperatorSet.TestRegionPoint(ho_RegionRectangle2, hv_CenterRowSelected, hv_CenterColSelected,
                        out hv_IsInsideMiddle);
                    HOperatorSet.TestRegionPoint(ho_RegionRectangle3, hv_CenterRowSelected, hv_CenterColSelected,
                        out hv_IsInsideDown);
                    if ((int)(hv_IsInsideUp) != 0)
                    {
                        //点属于上 距离判断标准
                        hv_DistanceMin = hv_DistanceUpMin.Clone();
                        hv_DistanceMax = hv_DistanceUpMax.Clone();
                    }
                    else if ((int)(hv_IsInsideMiddle) != 0)
                    {
                        //点属于中 距离判断标准
                        hv_DistanceMin = hv_DistanceMidMin.Clone();
                        hv_DistanceMax = hv_DistanceMidMax.Clone();
                    }
                    else if ((int)(hv_IsInsideDown) != 0)
                    {
                        //点属于下 距离判断标准
                        hv_DistanceMin = hv_DistanceDownMin.Clone();
                        hv_DistanceMax = hv_DistanceDownMax.Clone();
                    }
                    if ((int)((new HTuple(hv_Distance1.TupleGreater(hv_DistanceMax))).TupleOr(
                        new HTuple(hv_Distance1.TupleLess(hv_DistanceMin)))) != 0)
                    {
                        //异常点
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle, hv_CenterRowSelected, hv_CenterColSelected,
                            2);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DistanceDefectRegion, ho_Circle, out ExpTmpOutVar_0
                                );
                            ho_DistanceDefectRegion.Dispose();
                            ho_DistanceDefectRegion = ExpTmpOutVar_0;
                        }
                    }
                    else
                    {
                        //正常点
                    }
                }

                //胶高不良检测，最小二乘法
                HOperatorSet.TupleGenConst(new HTuple(hv_CenterRow.TupleLength()), 2, out hv_Newtuple);
                ho_Circle2.Dispose();
                HOperatorSet.GenCircle(out ho_Circle2, hv_CenterRow, hv_CenterCol, hv_Newtuple);
                ho_GlueHeightObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_GlueHeightObject);
                HOperatorSet.TupleLength(hv_GlueHeight, out hv_Length);
                HOperatorSet.TupleGenSequence(0, 0 + ((hv_Length - 1) * 6), 6, out hv_MxCol);
                HOperatorSet.TupleLength(hv_MxCol, out hv_len);
                HOperatorSet.TupleGenConst(hv_len, 3, out hv_r);
                hv_Mb = hv_GlueHeight / 1;
                hv_MyRow = hv_Mb.Clone();
                //stop ()
                //异常停留检查  2019-9-21
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircle(out ho_ContCircle, hv_MyRow, hv_MxCol, hv_r);
                ho_Contour1.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_MyRow, hv_MxCol);
                HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                    out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);
                ho_TestRegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_TestRegionLines, hv_RowBegin, hv_ColBegin,
                    hv_RowEnd, hv_ColEnd);
                HOperatorSet.DistancePl(hv_MyRow, hv_MxCol, hv_RowBegin, hv_ColBegin, hv_RowEnd,
                    hv_ColEnd, out hv_Distance);
                //判断同一区域内，胶路相对高度不良区域
                //BigValue := 45
                HOperatorSet.TupleGreaterElem(hv_Distance, hv_BigValue, out hv_Greater1);
                HOperatorSet.TupleFind(hv_Greater1, 1, out hv_Indices1);
                if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
                {
                    ho_ObjectSelected5.Dispose();
                    HOperatorSet.SelectObj(ho_Circle2, out ho_ObjectSelected5, hv_Indices1 + 1);
                    ho_RegionUnion1.Dispose();
                    HOperatorSet.Union1(ho_ObjectSelected5, out ho_RegionUnion1);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionUnion1, out ho_RegionClosing1, 3.5);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_GlueHeightObject, ho_RegionClosing1, out ExpTmpOutVar_0
                            );
                        ho_GlueHeightObject.Dispose();
                        ho_GlueHeightObject = ExpTmpOutVar_0;
                    }
                }
                //筛选胶路缺陷
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_GlueHeightObject, out ho_RegionUnion);


                //******************************

                ho_ObjectsConcat1.Dispose();
                HOperatorSet.ConcatObj(ho_DistanceDefectRegion, ho_GlueHeightObject, out ho_ObjectsConcat1
                    );
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_GlueDefectSelectedRegions, ho_ObjectsConcat1, out ExpTmpOutVar_0
                        );
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_GlueDefectSelectedRegions, out ExpTmpOutVar_0,
                        "area", "and", hv_GlueDefectAreaMin, 99999);
                    ho_GlueDefectSelectedRegions.Dispose();
                    ho_GlueDefectSelectedRegions = ExpTmpOutVar_0;
                }
                //stop ()
                ho_DefectObjectOut.Dispose();
                ho_Contours.Dispose();
                ho_Polygons.Dispose();
                ho_ContCircle3.Dispose();
                ho_RegionLinesIn.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionMoved.Dispose();
                ho_CheckGlueRegionMoved.Dispose();
                ho_EdgeRegionMoved5.Dispose();
                ho_RegionDilation1.Dispose();
                ho_ImageScaled.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing3.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageScaleMax.Dispose();
                ho_ImageOpening.Dispose();
                ho_Partitioned.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_Partitioned1.Dispose();
                ho_ObjectSelected4.Dispose();
                ho_RegionUnion4.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Contour.Dispose();
                ho_RegionLines1.Dispose();
                ho_DistanceDefectRegion.Dispose();
                ho_Circle1.Dispose();
                ho_Circle.Dispose();
                ho_Circle2.Dispose();
                ho_GlueHeightObject.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected5.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ObjectsConcat1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DefectObjectOut.Dispose();
                ho_Contours.Dispose();
                ho_Polygons.Dispose();
                ho_ContCircle3.Dispose();
                ho_RegionLinesIn.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionMoved.Dispose();
                ho_CheckGlueRegionMoved.Dispose();
                ho_EdgeRegionMoved5.Dispose();
                ho_RegionDilation1.Dispose();
                ho_ImageScaled.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion3.Dispose();
                ho_RegionClosing3.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageScaleMax.Dispose();
                ho_ImageOpening.Dispose();
                ho_Partitioned.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_Partitioned1.Dispose();
                ho_ObjectSelected4.Dispose();
                ho_RegionUnion4.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Contour.Dispose();
                ho_RegionLines1.Dispose();
                ho_DistanceDefectRegion.Dispose();
                ho_Circle1.Dispose();
                ho_Circle.Dispose();
                ho_Circle2.Dispose();
                ho_GlueHeightObject.Dispose();
                ho_ContCircle.Dispose();
                ho_Contour1.Dispose();
                ho_TestRegionLines.Dispose();
                ho_ObjectSelected5.Dispose();
                ho_RegionUnion1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ObjectsConcat1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void FindLine2D(HObject ho_Image, out HObject ho_MeasureLineContours, out HObject ho_MeasureCross,
    out HObject ho_MeasuredLines, HTuple hv_InLineStartRow, HTuple hv_InLineStartCol,
    HTuple hv_InLineEndRow, HTuple hv_InLineEndCol, HTuple hv_InMeasureLength1,
    HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
    HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
    HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_RowBegin, out HTuple hv_ColBegin,
    out HTuple hv_RowEnd, out HTuple hv_ColEnd, out HTuple hv_AllRow, out HTuple hv_AllColumn,
    out HTuple hv_bFindLine2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyLineIndices = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            hv_AllRow = new HTuple();
            hv_AllColumn = new HTuple();
            hv_bFindLine2D = new HTuple();
            try
            {
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
                //
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_InLineStartRow,
                    hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, hv_InMeasureLength1,
                    hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(),
                    new HTuple(), out hv_MetrologyLineIndices);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_measures", hv_InMeasureNumber);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_interpolation", "bicubic");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_begin", out hv_RowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_begin", out hv_ColBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_end", out hv_RowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_end", out hv_ColEnd);
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureLineContours, hv_MetrologyHandle,
                    "all", "all", out hv_AllRow, out hv_AllColumn);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_AllRow, hv_AllColumn,
                    8, 0.785398);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLines, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                //if (bDisp)
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureLineContours)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureCross)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasuredLines)
                }
                //endif
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_RowBegin.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_ColEnd.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindLine2D = 1;
                }
                else
                {
                    hv_bFindLine2D = 0;
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindLine2D = 0;
            }


            return;
        }

        public static double MeasureCalibFunc(HObject ho_Image, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
                                             HTuple hv_Column2, HTuple hv_RealDis, HTuple hv_MaxThreshold, out HObject ho_SortedRegions)
        {
            // Local iconic variables 
            HObject ho_Rectangle, ho_ImageReduced;
            HObject ho_Region, ho_ConnectedRegions;

            // Local control variables 

            HTuple hv_Area = null, hv_Row = null;
            HTuple hv_Column = null, hv_length = null, hv_baseDistance = null;
            HTuple hv_DistanceArr = null, hv_Sum = null, hv_Index = null;
            HTuple hv_Distance = new HTuple(), hv_count = null, hv_MeanDis = null;
            HTuple hv_Scale = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
            }

            //hv_RealDis = 5;
            //hv_MaxThreshold = 120;

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, hv_MaxThreshold);

            HOperatorSet.FillUp(ho_Region, out ho_Region);

            HOperatorSet.ClosingCircle(ho_Region, out ho_Region, 20);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_ConnectedRegions, "area", "and", 300, 9999999);

            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "character",
                "true", "row");

            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);

            HObject ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_Cross);
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 50, 0);
            HOperatorSet.ConcatObj(ho_SortedRegions, ho_Cross, out ho_SortedRegions);

            ho_Cross.Dispose();

            hv_length = new HTuple(hv_Row.TupleLength());
            hv_baseDistance = 0;
            hv_DistanceArr = new HTuple();
            hv_Sum = 0;
            HTuple end_val23 = hv_length - 2;
            HTuple step_val23 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val23, step_val23); hv_Index = hv_Index.TupleAdd(step_val23))
            {
                if ((int)(new HTuple(hv_Index.TupleEqual(0))) != 0)
                {
                    HOperatorSet.DistancePp(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0),
                        hv_Row.TupleSelect(1), hv_Column.TupleSelect(1), out hv_baseDistance);
                    hv_Sum = hv_Sum + hv_baseDistance;
                    hv_DistanceArr = hv_DistanceArr.TupleConcat(hv_baseDistance);
                }
                else
                {
                    HOperatorSet.DistancePp(hv_Row.TupleSelect(hv_Index), hv_Column.TupleSelect(
                        hv_Index), hv_Row.TupleSelect(hv_Index + 1), hv_Column.TupleSelect(hv_Index + 1),
                        out hv_Distance);
                    if ((int)(new HTuple(((hv_Distance - hv_baseDistance)).TupleLess(15))) != 0)
                    {
                        hv_DistanceArr = hv_DistanceArr.TupleConcat(hv_Distance);
                        hv_Sum = hv_Sum + hv_Distance;
                    }
                }
            }

            hv_count = new HTuple(hv_DistanceArr.TupleLength());
            hv_MeanDis = hv_Sum / hv_count;

            hv_Scale = hv_RealDis / hv_MeanDis;
            //ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            //ho_SortedRegions.Dispose();

            return Math.Round(hv_Scale.D, 9);
        }
    }
}
