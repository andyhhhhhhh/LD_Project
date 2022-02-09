using AlgorithmController; 
using DevComponents.DotNetBar;
using GlobalCore;
using HalconDotNet;
using HalconView;
using Infrastructure.Log;
using ManagementView.Comment;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;
using MathNet.Numerics.LinearAlgebra;
using SocketController;
using ServiceController;
using System.Text.RegularExpressions;
using System.Net;

namespace ManagementView._3DViews
{

    #region 通用类
    public class CommFuncView
    {
        public delegate void Del_OutPutLog(string log, LogLevel loglevel = LogLevel.Info);
        /// <summary>
        /// 委托 -- 主界面输出Log信息
        /// </summary>
        public static Del_OutPutLog m_DelOutPutLog;

        public delegate void Del_OutExLog(Exception ex);
        /// <summary>
        /// 委托 -- 主界面输出Exception信息
        /// </summary>
        public static Del_OutExLog m_DelOutExLog;

        public static MTFCommand[] m_MTFCommands;
        

        public static SFCMethod m_SFCMethod = new SFCMethod();

        /// <summary>
        /// 在界面上显示工具对应的配置界面
        /// </summary>
        /// <param name="panelView">需要显示的主Panel</param>
        /// <param name="featureType">工具类型</param>
        /// <param name="checkModel">工具</param>
        public static void ShowPanelView(Panel panelView, FeatureType featureType, CheckFeatureModel checkModel)
        {
            try
            {
                UnitSetting _unitSetting = new UnitSetting();
                switch (featureType)
                {
                    default:
                        //根据继承的方式来找到子窗口
                        panelView.Controls.Clear();
                        UnitSetting unitSetting = new UnitSetting();

                        var aType = typeof(UnitSetting);
                        var types = typeof(UnitSetting).Assembly.GetTypes().ToList().FindAll(x=>x.BaseType!= null && x.BaseType.FullName == aType.FullName);
                        //var types = Assembly.GetCallingAssembly().GetTypes();  //获取所有类型
                        foreach (var t in types)
                        {
                            unitSetting = Activator.CreateInstance(t) as UnitSetting;
                            if (unitSetting.UnitType == featureType.ToString())
                            {
                                break;
                            }
                        }

                        unitSetting.FeatureModel = checkModel;
                        CommHelper.LayoutChildFillView(panelView, unitSetting);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("ShowPanelView:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //在Panel上显示配置界面
        public static void LayOutPanel(Panel panelView, UnitSetting unitSetting, CheckFeatureModel checkModel)
        {
            panelView.Controls.Clear();
            unitSetting.FeatureModel = checkModel;
            CommHelper.LayoutChildFillView(panelView, unitSetting);
        }
          
        // 弹出错误信息到显示屏的右下角
        public static void ShowErrMsg(string strTitle, string strInfo)
        {
            try
            {
                BallonView view = new BallonView();
                view.ShowInfo(strTitle, strInfo);
                view.Show(false);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        //找线
        public static List<double> FindLineFunc(HObject ho_Image, HTuple hv_Line, FindLineModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {
            HObject ho_Cross;
            HObject ho_MeasureCross1; 
            HObject ho_MeasuredLines1, ho_MeasureLineContours1;
            HObject ho_Contour;
            HObject ho_Polygons;


            HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
            HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
            HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
            HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
            HTuple hv_bDisp = null;
            HTuple hv_bFindLine2D = null;
            HTuple hv_RowBegin1 = new HTuple();
            HTuple hv_ColBegin1 = new HTuple(), hv_RowEnd1 = new HTuple();
            HTuple hv_ColEnd1 = new HTuple(), hv_AllRow = new HTuple();
            HTuple hv_AllColumn = new HTuple();
            HTuple hv_InLineStartRow = new HTuple(), hv_InLineStartCol = new HTuple();
            HTuple hv_InLineEndRow = new HTuple(), hv_InLineEndCol = new HTuple();

             
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Polygons);

            HOperatorSet.GenEmptyObj(out ho_MeasuredLines1);
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours1);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1); 

            //测量直线,上边线
            //输入参数
            hv_InLineStartRow = hv_Line[0].D;
            hv_InLineStartCol = hv_Line[1].D;
            hv_InLineEndRow = hv_Line[2].D;
            hv_InLineEndCol = hv_Line[3].D;
            hv_InMeasureLength1 = tModel.MeasureLength1;
            hv_InMeasureLength2 = tModel.MeasureLength2;
            hv_InMeasureSigma = tModel.Sigma;

            hv_InMeasureThreshold = tModel.MeasureThreshold;
            hv_InMeasureTransition = tModel.MeasureTransition;
            hv_InMeasureSelect = tModel.MeasureSelect;

            hv_InMeasureNumber = tModel.MeasureNumber;
            hv_InMeasureScore = tModel.MeasureScore;
            hv_bDisp = 0;

            //输出参数
            ho_MeasureLineContours1.Dispose(); ho_MeasureCross1.Dispose(); ho_MeasuredLines1.Dispose();
            AlgorithmFor3D.FindLine2D(ho_Image, out ho_MeasureLineContours1, out ho_MeasureCross1, out ho_MeasuredLines1,
                hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol,
                hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                hv_bDisp, out hv_RowBegin1, out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1,
                out hv_AllRow, out hv_AllColumn, out hv_bFindLine2D);


            #region 显示找线的方向箭头

            if(bDebug)
            {
                HTuple hv_Angle;
                HOperatorSet.AngleLx(hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, out hv_Angle);

                HObject ho_Arrow1;
                hSmartWindow.gen_arrow_contour_xld(out ho_Arrow1, tModel.MeasureLength1 * -1, 0, tModel.MeasureLength1, 0, 5, 5);

                HTuple hv_Mat2d;
                HOperatorSet.VectorAngleToRigid(0, 0, 0, (hv_InLineStartRow + hv_InLineEndRow) / 2, (hv_InLineStartCol + hv_InLineEndCol) / 2,
                    hv_Angle, out hv_Mat2d);
                HOperatorSet.AffineTransContourXld(ho_Arrow1, out ho_Arrow1, hv_Mat2d);

                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("magenta");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_Arrow1);
            }

            #endregion


            #region 是否延长线显示
            if (tModel.IsExtend)
            {
                HTuple hv_Width, hv_Height;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HTuple hv_row = new HTuple();
                HTuple hv_col = new HTuple();
                HTuple hv_row1, hv_col1, hv_row2, hv_col2, hv_row3, hv_col3, hv_row4, hv_col4, hv_IsPararllel;
                HOperatorSet.IntersectionLl(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1,
                   0, 0, hv_Height, 0, out hv_row1, out hv_col1, out hv_IsPararllel);
                if (hv_row1.Length != 0 && Math.Abs(hv_row1.D) <= hv_Height + 1 && Math.Abs(hv_col1.D) <= hv_Width + 1)
                {
                    if (Math.Abs(hv_row1.D) < 0.1)
                    {
                        hv_row1 = 0;
                    }
                    if (Math.Abs(hv_col1.D) < 0.1)
                    {
                        hv_col1 = 0;
                    }
                    hv_row.Append(hv_row1);
                    hv_col.Append(hv_col1);
                }
                HOperatorSet.IntersectionLl(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1,
                   0, 0, 0, hv_Width, out hv_row2, out hv_col2, out hv_IsPararllel);
                if (hv_row2.Length != 0 && Math.Abs(hv_row2.D) <= hv_Height + 1 && Math.Abs(hv_col2.D) <= hv_Width + 1)
                {
                    if (Math.Abs(hv_row2.D) < 0.1)
                    {
                        hv_row2 = 0;
                    }
                    if (Math.Abs(hv_col2.D) < 0.1)
                    {
                        hv_col2 = 0;
                    }
                    hv_row.Append(hv_row2);
                    hv_col.Append(hv_col2);
                }
                HOperatorSet.IntersectionLl(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1,
                   0, hv_Width, hv_Height, hv_Width, out hv_row3, out hv_col3, out hv_IsPararllel);
                if (hv_row3.Length != 0 && Math.Abs(hv_row3.D) <= hv_Height + 1 && Math.Abs(hv_col3.D) <= hv_Width + 1)
                {
                    if (Math.Abs(hv_row3.D) < 0.1)
                    {
                        hv_row1 = 0;
                    }
                    if (Math.Abs(hv_col3.D) < 0.1)
                    {
                        hv_col1 = 0;
                    }
                    hv_row.Append(hv_row3);
                    hv_col.Append(hv_col3);
                }
                HOperatorSet.IntersectionLl(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1,
                   hv_Height, hv_Width, hv_Height, 0, out hv_row4, out hv_col4, out hv_IsPararllel);
                if (hv_row4.Length != 0 && Math.Abs(hv_row4.D) <= hv_Height + 1 && Math.Abs(hv_col4.D) <= hv_Width + 1)
                {
                    if (Math.Abs(hv_row4.D) < 0.1)
                    {
                        hv_row1 = 0;
                    }
                    if (Math.Abs(hv_col4.D) < 0.1)
                    {
                        hv_col1 = 0;
                    }
                    hv_row.Append(hv_row4);
                    hv_col.Append(hv_col4);
                }

                if (hv_row.Length == 2)
                {
                    hv_RowBegin1 = hv_row[0];
                    hv_ColBegin1 = hv_col[0];
                    hv_RowEnd1 = hv_row[1];
                    hv_ColEnd1 = hv_col[1];

                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("yellow");
                    hSmartWindow.GetWindowHandle().SetDraw("margin");
                    HObject ho_RegionLine;
                    HOperatorSet.GenEmptyObj(out ho_RegionLine);
                    ho_RegionLine.Dispose();
                    HOperatorSet.GenRegionLine(out ho_RegionLine, hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1);
                    hSmartWindow.GetWindowHandle().DispLine(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1);

                    tModel.itemResult.ResultObj = ho_RegionLine;
                }
            }
            #endregion 
             
            if (hv_bFindLine2D.I != 0)
            {
                HTuple hv_CenterRow = (hv_RowBegin1 + hv_RowEnd1) / 2;
                HTuple hv_CenterCol = (hv_RowEnd1 + hv_ColEnd1) / 2;

                if (bDebug)
                {
                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("yellow");
                    hSmartWindow.GetWindowHandle().SetDraw("margin");
                    hSmartWindow.DispObj(ho_MeasuredLines1);

                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("red");
                    hSmartWindow.GetWindowHandle().SetDraw("margin");
                    hSmartWindow.DispObj(ho_MeasureLineContours1);
                    hSmartWindow.DispObj(ho_MeasureCross1);
                }
                else
                {
                    if (tModel.IsShowLine)
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("yellow");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasuredLines1);
                    }

                    if (tModel.IsShowPoint)
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("red");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasureCross1);
                    }
                    else
                    {
                        ho_MeasureCross1.Dispose();
                    }

                    ho_MeasureLineContours1.Dispose();
                }

                //ho_Image.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Polygons.Dispose();

                if(!tModel.IsExtend)
                {
                    tModel.itemResult.ResultObj = ho_MeasuredLines1;
                }

                tModel.itemResult.AllRow = hv_AllRow.DArr;
                tModel.itemResult.AllCol = hv_AllColumn.DArr;
                return new List<double> {  Math.Round(hv_RowBegin1.D, 3), Math.Round(hv_ColBegin1.D, 3),
                    Math.Round(hv_RowEnd1.D, 3), Math.Round(hv_ColEnd1.D, 3) };
            }
            else
            {
                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_MeasureLineContours1);
                hSmartWindow.DispObj(ho_MeasureCross1);

                //ho_Image.Dispose();
                ho_Cross.Dispose();
                ho_MeasureCross1.Dispose();
                ho_MeasuredLines1.Dispose();
                // ho_MeasureLineContours1.Dispose();
                //ho_Contour.Dispose();
                ho_Polygons.Dispose();

                return new List<double> { 0, 0, 0, 0 };
            }
        }

        //找线
        public static List<double> FindRect2LineFunc(HObject ho_Image, HTuple hv_Line, MeasureRectModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {
            HObject ho_Cross;
            HObject ho_MeasureCross1;
            HObject ho_MeasuredLines1, ho_MeasureLineContours1;
            HObject ho_Contour;
            HObject ho_Polygons;


            HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
            HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
            HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
            HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
            HTuple hv_bDisp = null;
            HTuple hv_bFindLine2D = null;
            HTuple hv_RowBegin1 = new HTuple();
            HTuple hv_ColBegin1 = new HTuple(), hv_RowEnd1 = new HTuple();
            HTuple hv_ColEnd1 = new HTuple(), hv_AllRow = new HTuple();
            HTuple hv_AllColumn = new HTuple();
            HTuple hv_InLineStartRow = new HTuple(), hv_InLineStartCol = new HTuple();
            HTuple hv_InLineEndRow = new HTuple(), hv_InLineEndCol = new HTuple();


            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Polygons);

            HOperatorSet.GenEmptyObj(out ho_MeasuredLines1);
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours1);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);

            //测量直线,上边线
            //输入参数
            hv_InLineStartRow = hv_Line[0].D;
            hv_InLineStartCol = hv_Line[1].D;
            hv_InLineEndRow = hv_Line[2].D;
            hv_InLineEndCol = hv_Line[3].D;
            hv_InMeasureLength1 = tModel.MeasureLength1;
            hv_InMeasureLength2 = tModel.MeasureLength2;
            hv_InMeasureSigma = tModel.Sigma;

            hv_InMeasureThreshold = tModel.MeasureThreshold;
            hv_InMeasureTransition = tModel.MeasureTransition;
            hv_InMeasureSelect = tModel.MeasureSelect;

            hv_InMeasureNumber = tModel.MeasureNumber;
            hv_InMeasureScore = tModel.MeasureScore;
            hv_bDisp = 0;

            //输出参数
            ho_MeasureLineContours1.Dispose(); ho_MeasureCross1.Dispose(); ho_MeasuredLines1.Dispose();
            AlgorithmFor3D.FindLine2D(ho_Image, out ho_MeasureLineContours1, out ho_MeasureCross1, out ho_MeasuredLines1,
                hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol,
                hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                hv_bDisp, out hv_RowBegin1, out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1,
                out hv_AllRow, out hv_AllColumn, out hv_bFindLine2D);
              
            if (hv_bFindLine2D.I != 0)
            {
                HTuple hv_CenterRow = (hv_RowBegin1 + hv_RowEnd1) / 2;
                HTuple hv_CenterCol = (hv_RowEnd1 + hv_ColEnd1) / 2;

                if (bDebug)
                {
                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("yellow");
                    hSmartWindow.GetWindowHandle().SetDraw("margin");
                    hSmartWindow.DispObj(ho_MeasuredLines1);

                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("red");
                    hSmartWindow.GetWindowHandle().SetDraw("margin");
                    hSmartWindow.DispObj(ho_MeasureLineContours1);
                    hSmartWindow.DispObj(ho_MeasureCross1);
                }
                else
                {
                    if (tModel.IsShowLine)
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("yellow");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasuredLines1);
                    }

                    if (tModel.IsShowPoint)
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("red");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasureCross1);
                    }
                    else
                    {
                        ho_MeasureCross1.Dispose();
                    }

                    ho_MeasureLineContours1.Dispose();
                }

                //ho_Image.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Polygons.Dispose(); 

                tModel.itemResult.AllRow = hv_AllRow.DArr;
                tModel.itemResult.AllCol = hv_AllColumn.DArr;
                return new List<double> {  Math.Round(hv_RowBegin1.D, 3), Math.Round(hv_ColBegin1.D, 3),
                    Math.Round(hv_RowEnd1.D, 3), Math.Round(hv_ColEnd1.D, 3) };
            }
            else
            {
                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_MeasureLineContours1);
                hSmartWindow.DispObj(ho_MeasureCross1);

                //ho_Image.Dispose();
                ho_Cross.Dispose();
                ho_MeasureCross1.Dispose();
                ho_MeasuredLines1.Dispose();
                // ho_MeasureLineContours1.Dispose();
                //ho_Contour.Dispose();
                ho_Polygons.Dispose();

                return new List<double> { 0, 0, 0, 0 };
            }
        }

        //找圆
        public static List<double> FindCircleFunc(HObject ho_Image, HTuple hv_Circle, FindCircleModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {

            HObject ho_MeasureCircleContours1, ho_Cross;
            HObject ho_MeasureCross1, ho_CircleContours1;
            HObject ho_outImage;

            HTuple hv_Radius1 = null, hv_InCircleRow = null;
            HTuple hv_InCircleCol = null, hv_InCircleRadiu = null;
            HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
            HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
            HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
            HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
            HTuple hv_bDisp = null, hv_CircleCenterRow1 = null, hv_CircleCenterColumn1 = null;
            HTuple hv_CircleRadius1 = null, hv_bFindCircle2D = null;


            HOperatorSet.GenEmptyObj(out ho_outImage);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
            HOperatorSet.GenEmptyObj(out ho_CircleContours1);

            hv_Radius1 = tModel.SearchRadius;
            hv_InMeasureThreshold = tModel.MeasureThreshold;
            hv_InMeasureTransition = tModel.MeasureTransition;
            hv_InMeasureSelect = tModel.MeasureSelect; 

            ////输入参数
            hv_InCircleRow = hv_Circle[0];
            hv_InCircleCol = hv_Circle[1];
            hv_InCircleRadiu = hv_Circle[2];
            hv_InMeasureLength1 = tModel.MeasureLength1;
            hv_InMeasureLength2 = tModel.MeasureLength2;
            hv_InMeasureSigma = tModel.Sigma;

            hv_InMeasureNumber = tModel.MeasureNumber;
            hv_InMeasureScore = tModel.MeasureScore;
            hv_bDisp = 0;
            //输出参数
            hv_CircleCenterRow1 = new HTuple();
            hv_CircleCenterColumn1 = new HTuple();
            hv_CircleRadius1 = new HTuple();
            ho_MeasureCircleContours1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
            ho_MeasureCross1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
            ho_CircleContours1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_CircleContours1);

            ho_outImage.Dispose();
            HOperatorSet.ScaleImageMax(ho_Image, out ho_outImage); 

            ho_MeasureCircleContours1.Dispose(); ho_MeasureCross1.Dispose(); ho_CircleContours1.Dispose();
            AlgorithmCommHelper.FindCircle2D(ho_Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                out ho_CircleContours1, hv_InCircleRow, hv_InCircleCol, hv_InCircleRadiu,
                hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                out hv_bFindCircle2D);


            hSmartWindow.GetWindowHandle().SetLineWidth(2);
            hSmartWindow.GetWindowHandle().SetColor("spring green");
            hSmartWindow.GetWindowHandle().SetDraw("margin");
            hSmartWindow.DispObj(ho_CircleContours1);

            if (bDebug)
            {
                HObject ho_Arrow1;
                hSmartWindow.gen_arrow_contour_xld(out ho_Arrow1, hv_InCircleRow + hv_InCircleRadiu - hv_InMeasureLength1, hv_InCircleCol, 
                    hv_InCircleRow + hv_InCircleRadiu + hv_InMeasureLength1, hv_InCircleCol, 5, 5);
                
                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("yellow");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_Arrow1);
            }


            if (hv_bFindCircle2D.I != 0)
            {
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_CircleCenterRow1, hv_CircleCenterColumn1, 30, 0);
                hSmartWindow.GetWindowHandle().SetColor("spring green");
                hSmartWindow.DispObj(ho_Cross);

                if (bDebug)
                {
                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("red");
                    hSmartWindow.DispObj(ho_MeasureCircleContours1);
                    hSmartWindow.DispObj(ho_MeasureCross1);
                }
                else
                {
                    ho_MeasureCircleContours1.Dispose();
                    ho_MeasureCross1.Dispose();
                } 

                tModel.itemResult.ResultObj = ho_CircleContours1;
                ho_outImage.Dispose();
                //ho_CircleContours1.Dispose();
                return new List<double> { Math.Round(hv_CircleCenterRow1.D, 3), Math.Round(hv_CircleCenterColumn1.D, 3), Math.Round(hv_CircleRadius1.D, 3) };
            }
            else
            {
                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_MeasureCircleContours1);
                hSmartWindow.DispObj(ho_MeasureCross1);
                 
                ho_MeasureCircleContours1.Dispose();
                ho_MeasureCross1.Dispose();
                ho_outImage.Dispose();
                //ho_CircleContours1.Dispose();

                return new List<double> { 0, 0, 0 };
            }
        }

        //根据设定点找圆
        public static bool FindSetCircleFunc(HObject ho_Image, object Row, object Col, object Radius, FindCircleModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {
            try
            {
                HObject ho_MeasureCircleContours1, ho_Cross;
                HObject ho_MeasureCross1, ho_CircleContours1, ho_CircleContours; 

                HTuple hv_Radius1 = null, hv_InCircleRow = null;
                HTuple hv_InCircleCol = null, hv_InCircleRadiu = null;
                HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
                HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
                HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
                HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
                HTuple hv_bDisp = null, hv_CircleCenterRow1 = null, hv_CircleCenterColumn1 = null;
                HTuple hv_CircleRadius1 = null, hv_bFindCircle2D = null;

                 
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
                HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
                HOperatorSet.GenEmptyObj(out ho_CircleContours1);
                HOperatorSet.GenEmptyObj(out ho_CircleContours);

                hv_Radius1 = tModel.SearchRadius;
                hv_InMeasureThreshold = tModel.MeasureThreshold;
                hv_InMeasureTransition = tModel.MeasureTransition;
                hv_InMeasureSelect = tModel.MeasureSelect;

                int count = 1;
                double[] dArrRow, dArrCol;
                if (Row is Array)
                {
                    dArrRow = (double[])Row;
                    dArrCol = (double[])Col;
                    count = dArrRow.Count();
                }
                else
                {
                    dArrRow = new double[1] { (double)Row };
                    dArrCol = new double[1] { (double)Col };
                }

                double dRadius = Double.Parse(Radius.ToString());

                List<double> listRow = new List<double>();
                List<double> listCol = new List<double>();
                List<double> listRadius = new List<double>();

                //HOperatorSet.ScaleImageMax(ho_Image, out ho_OutImage);
                string strRow = "";
                string strCol = "";
                string strRadius = "";

                bool bresult = true;
                for (int i = 0; i < count; i++)
                {
                    //输入参数
                    hv_InCircleRow = dArrRow[i];
                    hv_InCircleCol = dArrCol[i];
                    hv_InCircleRadiu = dRadius;
                    hv_InMeasureLength1 = tModel.MeasureLength1;
                    hv_InMeasureLength2 = tModel.MeasureLength2;
                    hv_InMeasureSigma = tModel.Sigma;

                    hv_InMeasureNumber = tModel.MeasureNumber;
                    hv_InMeasureScore = tModel.MeasureScore;
                    hv_bDisp = 0;
                    //输出参数
                    hv_CircleCenterRow1 = new HTuple();
                    hv_CircleCenterColumn1 = new HTuple();
                    hv_CircleRadius1 = new HTuple();
                    ho_MeasureCircleContours1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
                    ho_MeasureCross1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
                    ho_CircleContours1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_CircleContours1);
                     
                    AlgorithmCommHelper.FindCircle2D(ho_Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                        out ho_CircleContours1, hv_InCircleRow, hv_InCircleCol, hv_InCircleRadiu,
                        hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                        hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                        hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                        out hv_bFindCircle2D);
                    

                    if (hv_bFindCircle2D.I != 0)
                    { 
                        HOperatorSet.ConcatObj(ho_CircleContours, ho_CircleContours1, out ho_CircleContours);
                        ho_CircleContours1.Dispose();

                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_CircleCenterRow1, hv_CircleCenterColumn1, 20, 0); 
                        HOperatorSet.ConcatObj(ho_CircleContours, ho_Cross, out ho_CircleContours);
                        ho_Cross.Dispose();

                        if (bDebug)
                        {
                            hSmartWindow.GetWindowHandle().SetLineWidth(1);
                            hSmartWindow.GetWindowHandle().SetColor("red");
                            hSmartWindow.DispObj(ho_MeasureCircleContours1);
                            hSmartWindow.DispObj(ho_MeasureCross1);
                        }
                        else
                        {
                            ho_MeasureCircleContours1.Dispose();
                            ho_MeasureCross1.Dispose();
                        }

                        listRow.Add(Math.Round(hv_CircleCenterRow1.D, 3));
                        listCol.Add(Math.Round(hv_CircleCenterColumn1.D, 3));
                        listRadius.Add(Math.Round(hv_CircleRadius1.D, 3));


                        strRow += (Math.Round(hv_CircleCenterRow1.D, 1).ToString() + ", ");
                        strCol += (Math.Round(hv_CircleCenterColumn1.D, 1).ToString() + ", ");
                        strRadius += (Math.Round(hv_CircleRadius1.D, 1).ToString() + ", "); 
                    }
                    else
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("red");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasureCircleContours1);
                        hSmartWindow.DispObj(ho_MeasureCross1);
                        
                        ho_MeasureCircleContours1.Dispose();
                        ho_MeasureCross1.Dispose();

                        listRow.Add(0);
                        listCol.Add(0);
                        listRadius.Add(0); 

                        strRow += "0, ";
                        strCol += "0, ";
                        strRadius += "0, ";
                        bresult = false;
                    }
                }

                hSmartWindow.GetWindowHandle().SetLineWidth(2);
                hSmartWindow.GetWindowHandle().SetColor("orange");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_CircleContours);

                m_DelOutPutLog(string.Format("{0} Row: {1} Col: {2} Radius: {3}", tModel.Name, strRow.TrimEnd(' ').TrimEnd(',') + "\r\n", strCol.TrimEnd(' ').TrimEnd(',') + "\r\n",
                    strRadius.TrimEnd(' ').TrimEnd(',')));


                if(listRow.Count > 0)
                {
                    tModel.itemResult.AllRow = listRow.ToArray();
                    tModel.itemResult.AllCol = listCol.ToArray();
                    tModel.itemResult.AllRadius = listRadius.ToArray();

                    tModel.itemResult.CircleRow = listRow[0];
                    tModel.itemResult.CircleCol = listCol[0];
                    tModel.itemResult.CircleRadius = listRadius[0];
                      
                    //是否设置为真值
                    if (tModel.IsReal)
                    {
                        double dPixPrec = Double.Parse(XmlControl.GetLinkValue(tModel.PixPrec).ToString());
                        List<double> listNew = new List<double>();
                        foreach (var item in listRadius)
                        {
                            listNew.Add(item * dPixPrec);
                        }

                        tModel.itemResult.AllRadius = listNew.ToArray();
                        tModel.itemResult.CircleRadius = listNew[0];
                    }
                    
                    //判断半径是否超过范围
                    if (tModel.IsJudgeRadius)
                    {
                        double minValue = Double.Parse(XmlControl.GetLinkValue(tModel.MinValue).ToString());
                        double maxValue = Double.Parse(XmlControl.GetLinkValue(tModel.MaxValue).ToString());
                        
                        int i = 0;
                        string str = "";
                        foreach (var item in tModel.itemResult.AllRadius)
                        {
                            if (item > maxValue || item < minValue)
                            {
                                double row2 = tModel.itemResult.AllRow[i];
                                double column2 = tModel.itemResult.AllCol[i];
                                double radius = tModel.itemResult.AllRadius[i];

                                //把Ng的框选出来
                                HObject ho_obj;
                                HOperatorSet.GenEmptyObj(out ho_obj);
                                ho_obj.Dispose();
                                HOperatorSet.GenRectangle1(out ho_obj, row2 - radius - 100, column2 - radius - 100,
                                                          row2 + radius + 100, column2 + radius + 100);

                                HOperatorSet.ConcatObj(ho_CircleContours, ho_obj, out ho_CircleContours);

                                hSmartWindow.GetWindowHandle().SetColor("red");
                                hSmartWindow.DispObj(ho_obj);
                                ho_obj.Dispose();

                                bresult = false;
                                str += "0,";
                            }
                            else
                            {
                                str += "1,";
                            }
                            i++;
                        }

                        str = str.TrimEnd(',');
                        tModel.itemResult.ResultStr = str;                        
                    }
                    
                    tModel.itemResult.ResultObj = ho_CircleContours;
                }
                else
                {
                    return false;
                }

                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            } 
           
        }
         
        //根据设定区域找圆
        public static bool FindRegionCircleFunc(HObject ho_Image, FindRegionCircleModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {
            try
            {
                HObject ho_MeasureCircleContours1, ho_Cross;
                HObject ho_MeasureCross1, ho_CircleContours1, ho_CircleContours, ho_CircleObj; 

                HTuple hv_Radius1 = null, hv_InCircleRow = null;
                HTuple hv_InCircleCol = null, hv_InCircleRadiu = null;
                HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
                HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
                HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
                HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
                HTuple hv_bDisp = null, hv_CircleCenterRow1 = null, hv_CircleCenterColumn1 = null;
                HTuple hv_CircleRadius1 = null, hv_bFindCircle2D = null;
                
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
                HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
                HOperatorSet.GenEmptyObj(out ho_CircleContours1);
                HOperatorSet.GenEmptyObj(out ho_CircleContours);
                HOperatorSet.GenEmptyObj(out ho_CircleObj);

                hv_Radius1 = Int32.Parse(XmlControl.GetLinkValue(tModel.SearchRadius).ToString());
                hv_InMeasureThreshold = tModel.MeasureThreshold;
                hv_InMeasureTransition = tModel.MeasureTransition;
                hv_InMeasureSelect = tModel.MeasureSelect;

                HObject ho_Region = (HObject)XmlControl.GetLinkValue(tModel.RegionForm);
                HOperatorSet.Connection(ho_Region, out ho_Region);

                HTuple hv_num;
                HOperatorSet.CountObj(ho_Region, out hv_num);
                
                List<double> listRow = new List<double>();
                List<double> listCol = new List<double>();
                List<double> listRadius = new List<double>();

                //HOperatorSet.ScaleImageMax(ho_Image, out ho_OutImage);

                string strRow = "";
                string strCol = "";
                string strRadius = "";
                string strArea = "";
                bool bresult = true;
                for (int i = 0; i < hv_num.I; i++)
                {
                    HObject ho_ImageReduced;
                    HObject ho_OutRegion;

                    HOperatorSet.SelectObj(ho_Region, out ho_OutRegion, i + 1);
                    
                    HOperatorSet.ReduceDomain(ho_Image, ho_OutRegion, out ho_ImageReduced);

                    HOperatorSet.Threshold(ho_ImageReduced, out ho_OutRegion, tModel.MinGray, tModel.MaxGray);

                    if(tModel.CloseRadius != 0)
                    {
                        HOperatorSet.ClosingCircle(ho_OutRegion, out ho_OutRegion, tModel.CloseRadius);
                    }

                    HOperatorSet.FillUp(ho_OutRegion, out ho_OutRegion);

                    HOperatorSet.Connection(ho_OutRegion, out ho_OutRegion);
                    HOperatorSet.SelectShape(ho_OutRegion, out ho_OutRegion, "area", "and", tModel.MinArea, 99999999);

                    HOperatorSet.SelectShapeStd(ho_OutRegion, out ho_OutRegion, "max_area", 70);

                    HTuple hv_Area, hv_Row, hv_Column;
                    HOperatorSet.AreaCenter(ho_OutRegion, out hv_Area, out hv_Row, out hv_Column);
                    if(hv_Area.Length != 0 )
                    {
                        strArea += (Math.Round(hv_Area.D, 1).ToString() + ", ");
                    }

                    ho_ImageReduced.Dispose();
                    ho_OutRegion.Dispose();

                    //输入参数
                    hv_InCircleRow = hv_Row;
                    hv_InCircleCol = hv_Column;
                    hv_InCircleRadiu = hv_Radius1;
                    hv_InMeasureLength1 = tModel.MeasureLength1;
                    hv_InMeasureLength2 = tModel.MeasureLength2;
                    hv_InMeasureSigma = tModel.Sigma;

                    hv_InMeasureNumber = tModel.MeasureNumber;
                    hv_InMeasureScore = tModel.MeasureScore;
                    hv_bDisp = 0;
                    //输出参数
                    hv_CircleCenterRow1 = new HTuple();
                    hv_CircleCenterColumn1 = new HTuple();
                    hv_CircleRadius1 = new HTuple();
                    ho_MeasureCircleContours1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
                    ho_MeasureCross1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
                    ho_CircleContours1.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_CircleContours1);

                    AlgorithmCommHelper.FindCircle2D(ho_Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                        out ho_CircleContours1, hv_InCircleRow, hv_InCircleCol, hv_InCircleRadiu,
                        hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                        hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                        hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                        out hv_bFindCircle2D);

                    if (hv_bFindCircle2D.I != 0)
                    {
                        HOperatorSet.ConcatObj(ho_CircleContours, ho_CircleContours1, out ho_CircleContours);
                        HOperatorSet.ConcatObj(ho_CircleObj, ho_CircleContours1, out ho_CircleObj);
                        ho_CircleContours1.Dispose();

                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_CircleCenterRow1, hv_CircleCenterColumn1, 20, 0);
                        HOperatorSet.ConcatObj(ho_CircleContours, ho_Cross, out ho_CircleContours);
                        ho_Cross.Dispose();

                        if (bDebug)
                        {
                            hSmartWindow.GetWindowHandle().SetLineWidth(1);
                            hSmartWindow.GetWindowHandle().SetColor("red");
                            hSmartWindow.GetWindowHandle().SetDraw("margin");
                            //hSmartWindow.GetWindowHandle().DispObj(ho_MeasureCircleContours1);
                            //hSmartWindow.GetWindowHandle().DispObj(ho_MeasureCross1);

                            hSmartWindow.DispObj(ho_MeasureCircleContours1);
                            hSmartWindow.DispObj(ho_MeasureCross1);
                        }
                        else
                        {
                            ho_MeasureCircleContours1.Dispose();
                            ho_MeasureCross1.Dispose();
                        }

                        listRow.Add(Math.Round(hv_CircleCenterRow1.D, 3));
                        listCol.Add(Math.Round(hv_CircleCenterColumn1.D, 3));
                        listRadius.Add(Math.Round(hv_CircleRadius1.D, 3));

                        strRow += (Math.Round(hv_CircleCenterRow1.D, 1).ToString() + ", ");
                        strCol += (Math.Round(hv_CircleCenterColumn1.D, 1).ToString() + ", ");
                        strRadius += (Math.Round(hv_CircleRadius1.D, 1).ToString() + ", ");

                        HSmartWindow.disp_message(hSmartWindow.GetWindowHandle(), (i+1).ToString(), "window", hv_CircleCenterRow1, hv_CircleCenterColumn1, "lime green", "false");
                    }
                    else
                    {
                        hSmartWindow.GetWindowHandle().SetLineWidth(1);
                        hSmartWindow.GetWindowHandle().SetColor("red");
                        hSmartWindow.GetWindowHandle().SetDraw("margin");
                        hSmartWindow.DispObj(ho_MeasureCircleContours1);
                        hSmartWindow.DispObj(ho_MeasureCross1);

                        //把没找到的框选出来
                        if(hv_Row.Length != 0)
                        {
                            HObject ho_obj;
                            HOperatorSet.GenEmptyObj(out ho_obj);
                            ho_obj.Dispose();
                            HOperatorSet.GenRectangle1(out ho_obj, hv_Row - hv_Radius1 - 100, hv_Column - hv_Radius1 - 100,
                                                      hv_Row + hv_Radius1 + 100, hv_Column + hv_Radius1 + 100);

                            HOperatorSet.ConcatObj(ho_CircleContours, ho_obj, out ho_CircleContours);

                            hSmartWindow.GetWindowHandle().SetColor("red");
                            hSmartWindow.DispObj(ho_obj);
                            ho_obj.Dispose();
                        }                       

                        ho_MeasureCircleContours1.Dispose();
                        ho_MeasureCross1.Dispose();

                        listRow.Add(0);
                        listCol.Add(0);
                        listRadius.Add(0);
                        strRow += "0, ";
                        strCol += "0, ";
                        strRadius += "0, ";
                        bresult = false;
                    }
                }

                hSmartWindow.GetWindowHandle().SetLineWidth(2);
                hSmartWindow.GetWindowHandle().SetColor("orange");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_CircleContours);

                m_DelOutPutLog(string.Format("{0} Row: {1} Col: {2} Radius: {3} Area: {4}", tModel.Name, strRow.TrimEnd(' ').TrimEnd(',') + "\r\n",
                    strCol.TrimEnd(' ').TrimEnd(',') + "\r\n", strRadius.TrimEnd(' ').TrimEnd(',') + "\r\n", strArea.TrimEnd(' ').TrimEnd(',')));
                
                if (listRow.Count == hv_num.I)
                {
                    tModel.itemResult.AllRow = listRow.ToArray();
                    tModel.itemResult.AllCol = listCol.ToArray();
                    tModel.itemResult.AllRadius = listRadius.ToArray();

                    tModel.itemResult.CircleRow = listRow[0];
                    tModel.itemResult.CircleCol = listCol[0];
                    tModel.itemResult.CircleRadius = listRadius[0]; 

                    //是否设置为真值
                    if (tModel.IsReal)
                    {
                        double dPixPrec = Double.Parse(XmlControl.GetLinkValue(tModel.PixPrec).ToString());
                        List<double> listNew = new List<double>();
                        foreach (var item in listRadius)
                        {
                            listNew.Add(item * dPixPrec);
                        }

                        tModel.itemResult.AllRadius = listNew.ToArray();
                        tModel.itemResult.CircleRadius = listNew[0];
                    }

                    //判断半径是否超过范围
                    if (tModel.IsJudgeRadius)
                    {
                        double minValue = Double.Parse(XmlControl.GetLinkValue(tModel.MinValue).ToString());
                        double maxValue = Double.Parse(XmlControl.GetLinkValue(tModel.MaxValue).ToString());

                        int i = 0;
                        string str = "";
                        foreach (var item in tModel.itemResult.AllRadius)
                        {
                            if(item == 0)//如果是没有找到圆
                            {
                                str += "2,";
                            }
                            else
                            {
                                if (item > maxValue || item < minValue)
                                {
                                    double row2 = tModel.itemResult.AllRow[i];
                                    double column2 = tModel.itemResult.AllCol[i];
                                    double radius = tModel.itemResult.AllRadius[i];

                                    //把Ng的框选出来
                                    HObject ho_obj;
                                    HOperatorSet.GenEmptyObj(out ho_obj);
                                    ho_obj.Dispose();
                                    HOperatorSet.GenRectangle1(out ho_obj, row2 - radius - 100, column2 - radius - 100,
                                                              row2 + radius + 100, column2 + radius + 100);

                                    HOperatorSet.ConcatObj(ho_CircleContours, ho_obj, out ho_CircleContours);

                                    hSmartWindow.GetWindowHandle().SetColor("red");
                                    hSmartWindow.DispObj(ho_obj);
                                    ho_obj.Dispose();

                                    bresult = false;
                                    str += "0,";
                                }
                                else
                                {
                                    str += "1,";
                                }
                            }
                            
                            i++;
                        }

                        str = str.TrimEnd(',');
                        tModel.itemResult.ResultStr = str;
                        hSmartWindow.DisplayResult(str);
                    }


                    tModel.itemResult.ResultObj = ho_CircleContours;
                    tModel.itemResult.CircleObj = ho_CircleObj;
                }
                else
                {
                    return false;
                }

                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }

        }

        //找圆弧
        public static List<double> FindCircleArcFunc(HObject ho_Image, HTuple hv_Circle, FindCircleArcModel tModel, HSmartWindow hSmartWindow, bool bDebug = false)
        {

            HObject ho_MeasureCircleContours1, ho_Cross;
            HObject ho_MeasureCross1, ho_CircleContours1; 

            HTuple hv_Radius1 = null, hv_InCircleRow = null;
            HTuple hv_InCircleCol = null, hv_InCircleRadiu = null;
            HTuple hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
            HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
            HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
            HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
            HTuple hv_bDisp = null, hv_CircleCenterRow1 = null, hv_CircleCenterColumn1 = null;
            HTuple hv_CircleRadius1 = null, hv_bFindCircle2D = null;

             
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
            HOperatorSet.GenEmptyObj(out ho_CircleContours1);

            hv_Radius1 = tModel.SearchRadius;
            hv_InMeasureThreshold = tModel.MeasureThreshold;
            hv_InMeasureTransition = tModel.MeasureTransition;
            hv_InMeasureSelect = tModel.MeasureSelect;

            ////输入参数
            hv_InCircleRow = hv_Circle[0];
            hv_InCircleCol = hv_Circle[1];
            hv_InCircleRadiu = hv_Circle[2];
            hv_StartPhi = hv_Circle[3];
            hv_EndPhi = hv_Circle[4];
            hv_InMeasureLength1 = tModel.MeasureLength1;
            hv_InMeasureLength2 = tModel.MeasureLength2;
            hv_InMeasureSigma = tModel.Sigma;

            hv_InMeasureNumber = tModel.MeasureNumber;
            hv_InMeasureScore = tModel.MeasureScore;
            hv_bDisp = 0;
            //输出参数
            hv_CircleCenterRow1 = new HTuple();
            hv_CircleCenterColumn1 = new HTuple();
            hv_CircleRadius1 = new HTuple();
            ho_MeasureCircleContours1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
            ho_MeasureCross1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
            ho_CircleContours1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_CircleContours1);

            //ho_Image.Dispose();
            //HOperatorSet.ScaleImageMax(hSmartWindow.Image, out ho_Image); 

            ho_MeasureCircleContours1.Dispose(); ho_MeasureCross1.Dispose(); ho_CircleContours1.Dispose();
            AlgorithmCommHelper.FindCircleArc2D(ho_Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                out ho_CircleContours1, hv_InCircleRow, hv_InCircleCol, hv_InCircleRadiu,
                hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,hv_StartPhi, hv_EndPhi,
                hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                out hv_bFindCircle2D);


            hSmartWindow.GetWindowHandle().SetLineWidth(2);
            hSmartWindow.GetWindowHandle().SetColor("yellow");
            hSmartWindow.GetWindowHandle().SetDraw("margin");
            hSmartWindow.DispObj(ho_CircleContours1);


            if (hv_bFindCircle2D.I != 0)
            {
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_CircleCenterRow1, hv_CircleCenterColumn1, 10, 0);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.DispObj(ho_Cross);

                if (bDebug)
                {
                    hSmartWindow.GetWindowHandle().SetLineWidth(1);
                    hSmartWindow.GetWindowHandle().SetColor("red");
                    hSmartWindow.DispObj(ho_MeasureCircleContours1);
                    hSmartWindow.DispObj(ho_MeasureCross1);
                }
                else
                {
                    ho_MeasureCircleContours1.Dispose();
                    ho_MeasureCross1.Dispose();
                }

                tModel.itemResult.ResultObj = ho_CircleContours1;
                 
                //ho_CircleContours1.Dispose();
                return new List<double> { Math.Round(hv_CircleCenterRow1.D, 3), Math.Round(hv_CircleCenterColumn1.D, 3), Math.Round(hv_CircleRadius1.D, 3) };
            }
            else
            {
                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_MeasureCircleContours1);
                hSmartWindow.DispObj(ho_MeasureCross1);
                
                ho_MeasureCircleContours1.Dispose();
                ho_MeasureCross1.Dispose();
                //ho_CircleContours1.Dispose();

                return new List<double> { 0, 0, 0 };
            }
        }

        //数据保存到csv文件
        public static void DataSave(string filePath, string dataStr, string dataTitle)
        {
            StreamWriter fileWriter;
            if (!File.Exists(filePath))
            {
                fileWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312"));//TRUE 存在则添加，不存在则新建  
                fileWriter.Write(dataTitle + "\r\n");//时间字段名
            }
            else
            {
                fileWriter = new StreamWriter(filePath, true, Encoding.GetEncoding("gb2312"));//TRUE 存在则添加，不存在则新建 
            }

            fileWriter.Write(DateTime.Now.ToLongTimeString() + ",");
            fileWriter.Write(dataStr);
            fileWriter.Write("\r\n");

            fileWriter.Flush();
            fileWriter.Close();
        }

        //执行表达式
        public static bool RunExpressView(ExpressModel tModel, ref string strMsg)
        {
            try
            {
                strMsg = "";
                List<bool> ListResult = new List<bool>();
                foreach (var express in tModel.ListExpress)
                {
                    bool bResult = true;
                    List<object> listValue = new List<object>();
                    double dvalue1 = 0;
                    double dvalue2 = 0;

                    switch (express.ComputeType)
                    {
                        case ComputeType.大于等于:
                            listValue = GetExpressParam(express.ExpressValue, ">=");
                            if (listValue == null)
                            {
                                continue;
                            }
                            dvalue1 = Double.Parse(listValue[0].ToString());
                            dvalue2 = Double.Parse(listValue[1].ToString());
                            if (dvalue1 >= dvalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }

                            break;
                        case ComputeType.小于等于:
                            listValue = GetExpressParam(express.ExpressValue, "<=");
                            if (listValue == null)
                            {
                                continue;
                            }
                            dvalue1 = Double.Parse(listValue[0].ToString());
                            dvalue2 = Double.Parse(listValue[1].ToString());
                            if (dvalue1 <= dvalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;
                        case ComputeType.大于:
                            listValue = GetExpressParam(express.ExpressValue, ">");
                            if (listValue == null)
                            {
                                continue;
                            }
                            dvalue1 = Double.Parse(listValue[0].ToString());
                            dvalue2 = Double.Parse(listValue[1].ToString());
                            if (dvalue1 > dvalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;
                        case ComputeType.小于:
                            listValue = GetExpressParam(express.ExpressValue, "<");
                            if (listValue == null)
                            {
                                continue;
                            }
                            dvalue1 = Double.Parse(listValue[0].ToString());
                            dvalue2 = Double.Parse(listValue[1].ToString());
                            if (dvalue1 < dvalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;
                        case ComputeType.等于:
                            listValue = GetExpressParam(express.ExpressValue, "==");
                            if (listValue == null)
                            {
                                continue;
                            }
                            string svalue3 =listValue[0].ToString();
                            string svalue4 = listValue[1].ToString();

                            
                            if (svalue3 == svalue4)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;
                        case ComputeType.与:
                            listValue = GetExpressParam(express.ExpressValue, "&&");
                            if (listValue == null)
                            {
                                continue;
                            }
                            bool bvalue1 = Boolean.Parse(listValue[0].ToString());
                            bool bvalue2 = Boolean.Parse(listValue[1].ToString());
                            if (bvalue1 && bvalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;
                        case ComputeType.或:
                            listValue = GetExpressParam(express.ExpressValue, "||");
                            if (listValue == null)
                            {
                                continue;
                            }
                            bool bvalue3 = Boolean.Parse(listValue[0].ToString());
                            bool bvalue4 = Boolean.Parse(listValue[1].ToString());
                            if (bvalue3 && bvalue4)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;

                        case ComputeType.非:
                            listValue = GetExpressParam(express.ExpressValue, "!");
                            if (listValue == null)
                            {
                                continue;
                            }
                            bool bvalue5 = Boolean.Parse(listValue[0].ToString());
                            if (!bvalue5)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;

                        case ComputeType.不等于:
                            listValue = GetExpressParam(express.ExpressValue, "==");
                            if (listValue == null)
                            {
                                continue;
                            }
                            string svalue1 = listValue[0].ToString();
                            string svalue2 = listValue[1].ToString();
                            if (svalue1 != svalue2)
                            {
                                bResult = true;
                            }
                            else
                            {
                                bResult = false;
                            }
                            break;

                        default:
                            break;
                    }

                    ListResult.Add(bResult);
                }

                return !ListResult.Contains(false);
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                m_DelOutExLog(ex);
                return false;
            }
        }

        //获取表达式的参数
        public static List<object> GetExpressParam(string strValue, string spilt)
        {
            try
            {
                int index = strValue.IndexOf(spilt);
                if (index == -1)
                {
                    return null;
                }

                string value1 = strValue.Substring(0, index);
                string value2 = strValue.Substring(index + spilt.Length);

                object ovalue1 = XmlControl.GetLinkValue(value1);
                object ovalue2 = XmlControl.GetLinkValue(value2);

                return new List<object> { ovalue1, ovalue2 };
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        //图像预处理
        public static HObject PretreatImage(PretreatImageModel tModel, HSmartWindow hSmartWindow)
        {
            try
            {
                HObject image = (HObject)XmlControl.GetLinkValue(tModel.ImageForm);
                if (image == null || !image.IsInitialized())
                {
                    return null;
                }

                if(tModel.IsRegion)
                { 
                    HObject ho_Region = (HObject)XmlControl.GetLinkValue(tModel.RegionForm);
                    if (image != null && image.IsInitialized())
                    {
                        HOperatorSet.ReduceDomain(image, ho_Region, out image);
                    }
                }

                if (tModel.listPretreatImage == null)
                {
                    return null;
                }

                HImage img = new HImage(image);
                foreach (var preModel in tModel.listPretreatImage)
                {
                    if (preModel != null && preModel.Enable)
                    {
                        switch (preModel.preCalType)
                        {
                            case PreCalType.二值化:
                                string[] str = preModel.Paramter.Split('-');
                                if (str != null && str.Length == 2)
                                {
                                    double minGray = Double.Parse(str[0]);
                                    double maxGray = Double.Parse(str[1]);
                                    img = AlgorithmFunc.ThresholdImage(img, minGray, maxGray);
                                }
                                break;

                            case PreCalType.自动二值化:
                                img = AlgorithmFunc.BinaryThresholdImage(img, preModel.Paramter);
                                break;

                            case PreCalType.动态二值化:
                                string[] dynParam = preModel.Paramter.Split(',');
                                if (dynParam != null && dynParam.Length == 2)
                                {
                                    int offset = Int32.Parse(dynParam[0]);
                                    string lightOrDark =  dynParam[1];
                                    img = AlgorithmFunc.DynThresholdImage(new HImage(image), img, offset, lightOrDark);
                                }
                                break;

                            case PreCalType.旋转角度:
                                img = AlgorithmFunc.RotateImage(img, Convert.ToInt32(preModel.Paramter));
                                break;

                            case PreCalType.图像镜像:
                                img = AlgorithmFunc.MirrorImage(img, preModel.Paramter);
                                break;

                            case PreCalType.图像反转:
                                img = AlgorithmFunc.InverseImage(img);
                                break;

                            case PreCalType.中值滤波:
                                img = AlgorithmFunc.MedianRun(img, Convert.ToInt32(preModel.Paramter));
                                break;
                            case PreCalType.均值滤波:
                                img = AlgorithmFunc.MeanRun(img, Convert.ToInt32(preModel.Paramter));
                                break;
                            case PreCalType.高斯滤波:
                                //Paramter 3, 5,7,9, 11
                                img = AlgorithmFunc.GaussRun(img, Convert.ToInt32(preModel.Paramter));
                                break;
                            case PreCalType.Sobel滤波:
                                //Paramter 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39
                                img = AlgorithmFunc.SobelAmp(img, Convert.ToInt32(preModel.Paramter));
                                break;
                            case PreCalType.Gamma校正:
                                img = AlgorithmFunc.GammaFilter(img);
                                break;
                            case PreCalType.灰度腐蚀: 
                                //Paramter 3, 5, 7, 9, 11, 13, 15
                                string[] erosionParam = preModel.Paramter.Split(',');
                                if (erosionParam != null && erosionParam.Length == 2)
                                {
                                    int maskHeight = Int32.Parse(erosionParam[0]);
                                    int maskWidth = Int32.Parse(erosionParam[1]); 
                                    img = AlgorithmFunc.GrayErosion(img, maskWidth, maskWidth);
                                }
                                break; 
                            case PreCalType.灰度膨胀:
                                //Paramter 3, 5, 7, 9, 11, 13, 15
                                string[] dilationParam = preModel.Paramter.Split(',');
                                if (dilationParam != null && dilationParam.Length == 2)
                                {
                                    int maskHeight = Int32.Parse(dilationParam[0]);
                                    int maskWidth = Int32.Parse(dilationParam[1]);
                                    img = AlgorithmFunc.GrayDilation(img, maskWidth, maskWidth);
                                }
                                break;

                            case PreCalType.直方图:
                                img = AlgorithmFunc.EquHistoImage(img);
                                break;

                            case PreCalType.比例增大:
                                img = AlgorithmFunc.ScaleImageMax(img);
                                break;

                            case PreCalType.提取单通道:
                                img = AlgorithmFunc.DeCompose3Image(img, preModel.Paramter);
                                break;

                            case PreCalType.彩色转灰度:
                                img = AlgorithmFunc.RGBToGrayImage(img);
                                break;

                            case PreCalType.灰度转彩色:
                                img = AlgorithmFunc.GrayToRGBImage(img);
                                break;

                            case PreCalType.图像锐化:
                                string[] strParam = preModel.Paramter.Split(',');
                                if (strParam != null && strParam.Length == 3)
                                {
                                    int maskWidth = Int32.Parse(strParam[0]);
                                    int maskHeight = Int32.Parse(strParam[1]);
                                    double factor = Double.Parse(strParam[2]);
                                    img = AlgorithmFunc.EmphasizeImage(img, maskWidth, maskWidth, factor);
                                }
                                break;

                            case PreCalType.亮度调节:
                                string[] scaleParam = preModel.Paramter.Split(',');
                                if (scaleParam != null && scaleParam.Length == 2)
                                {
                                    double mult = double.Parse(scaleParam[0]);
                                    double add = double.Parse(scaleParam[1]);

                                    img = AlgorithmFunc.ScaleImage(img, mult, add);
                                }
                                break;

                            case PreCalType.对比度:
                                string[] rangeParam = preModel.Paramter.Split(',');
                                if (rangeParam != null && rangeParam.Length == 2)
                                {
                                    int min = Int32.Parse(rangeParam[0]);
                                    int max = Int32.Parse(rangeParam[1]);
                                    HObject ho_Image;
                                    HOperatorSet.GenEmptyObj(out ho_Image);
                                    ho_Image.Dispose();
                                    AlgorithmCommHelper.scale_image_range(img, out ho_Image, min, max);
                                    img = new HImage(ho_Image);
                                }
                                break;

                            case PreCalType.改变尺寸:
                                string[] listPar = preModel.Paramter.Split(',');
                                if (listPar != null && listPar.Length == 2)
                                {
                                    int Width = Int32.Parse(listPar[0]);
                                    int Height = Int32.Parse(listPar[1]);
                                    img = AlgorithmFunc.ZoomImage(img, Width, Height);
                                }
                                break;
                                 
                            case PreCalType.字符旋转:
                                int charHeight = Int32.Parse(preModel.Paramter);
                                img = AlgorithmFunc.TextLineImage(img, charHeight);
                                break;

                            default:
                                break;
                        }
                    }
                }

                //hSmartWindow.DispImage(img, true, false);
                //hSmartWindow.GetWindowHandle().DispObj(img);
                //hSmartWindow.DispImage(img);
                hSmartWindow.FitImageToWindow(img, null);

                return img;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        //根据基准面生成基准图像
        public static HObject GenFittingImage(GenFittingImageModel tModel, HObject ho_Image, HObject ho_BaseRegion, HSmartWindow hsmartWindow = null)
        {
            if (ho_Image == null)
            {
                ho_Image = (HObject)hsmartWindow.Image;
            }

            HObject ho_UseNULLImage, ho_RegionUnion, ho_Z1;
            HObject ho_ImageScaled, ho_ImageScaled1, ho_ImageScaled2;
            HObject ho_UseNULLImageConverted;


            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UseNULLImage);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled1);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled2);
            HOperatorSet.GenEmptyObj(out ho_UseNULLImageConverted);
            HOperatorSet.GenEmptyObj(out ho_Z1);

            ho_Z1.Dispose();
            AlgorithmFor3D.GenFitting(ho_Image, ho_BaseRegion, out ho_Z1, null);

            //ho_ImageScaled.Dispose();
            //HOperatorSet.ScaleImage(ho_Z1, out ho_ImageScaled, 833.333, 32768);
            ////拉升图像
            //ho_ImageScaled1.Dispose();
            //HOperatorSet.ScaleImage(ho_ImageScaled, out ho_ImageScaled1, 1, -30000);
            //ho_ImageScaled2.Dispose();
            //HOperatorSet.ScaleImage(ho_ImageScaled1, out ho_ImageScaled2, 8, 0);
            //ho_UseNULLImageConverted.Dispose();
            //HOperatorSet.ConvertImageType(ho_ImageScaled2, out ho_UseNULLImageConverted,
            //    "uint2");

            ho_UseNULLImage.Dispose();
            ho_RegionUnion.Dispose();
            //ho_Z1.Dispose();
            ho_ImageScaled.Dispose();
            ho_ImageScaled1.Dispose();
            ho_ImageScaled2.Dispose();
            ho_UseNULLImageConverted.Dispose();

            hsmartWindow.FitImageToWindow(ho_Z1, null);

            return ho_Z1;
        }

        //预处理图
        public static HObject ProcessImage(SingleSequenceModel sequence, ProcessImageModel tModel, HObject ho_Image)
        {
            try
            {
                HObject ho_ImageOut = null;
                HOperatorSet.GenEmptyObj(out ho_ImageOut);

 
                AlgorithmFor3D.ProcessImage(ho_Image, out ho_ImageOut, tModel.ThrLow, tModel.ThrHigh,
                    tModel.FillArea, tModel.CloseSize, tModel.MedianWidth);

                if (tModel.IsModelFollow)
                {
                    HTuple hv_HomMat2DInvert = new HTuple();
                    HTuple hv_HomMat2D = GetHomMat2d(sequence, tModel.FixedItemName);

                    HOperatorSet.HomMat2dInvert(hv_HomMat2D, out hv_HomMat2DInvert);
                     
                    HOperatorSet.AffineTransImage(ho_ImageOut, out ho_ImageOut,
                        hv_HomMat2DInvert, "constant", "false"); 
                }

                return ho_ImageOut;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }

        }
        
        //轮廓匹配模板查询
        public static object FindScaleShapeModel(HObject ho_Image, HTuple hv_ModelID, MatchingModel tModel, HSmartWindow hSmartWindow)
        {
            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HTuple hv_OutRow, hv_OutCol, hv_OutAngle, hv_OutScaleRow, hv_Score;

            HTuple levels = new HTuple(new int[] { tModel.PyramidLevel, tModel.LastPyramidLevel });

            string ModelIDPath = Global.Model3DPath + "//" + tModel.Name + "_";
            string strSearchPath = ModelIDPath + "SearchRegion.hobj";
            if (tModel.IsSearchArea && File.Exists(strSearchPath))
            { 
                HObject ho_SearchRegion;
                HOperatorSet.GenEmptyObj(out ho_SearchRegion);
                ho_SearchRegion.Dispose();
                HOperatorSet.ReadRegion(out ho_SearchRegion, strSearchPath);

                HObject ho_ImageReduced;
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_SearchRegion, out ho_ImageReduced);
                
                HTuple angleStart, angleExtent;
                HOperatorSet.TupleRad(tModel.StartingAngle, out angleStart);
                HOperatorSet.TupleRad(tModel.AngleExtent, out angleExtent);
                HOperatorSet.FindScaledShapeModel(ho_ImageReduced,
                                                       hv_ModelID,
                                                       angleStart,
                                                       angleExtent,
                                                       tModel.MinScale,
                                                       tModel.MaxScale,
                                                       tModel.MinScore,
                                                       tModel.NumMatches,
                                                       tModel.MaxOverlap,
                                                       new HTuple(tModel.Subpixel),
                                                       levels,
                                                       tModel.Greediness,
                                                       out hv_OutRow,
                                                       out hv_OutCol,
                                                       out hv_OutAngle,
                                                       out hv_OutScaleRow,
                                                       out hv_Score);


                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.GetWindowHandle().SetColor("blue");
                hSmartWindow.DispObj(ho_SearchRegion);

                HTuple hv_Row1, hv_Column1, hv_Row2, hv_Column2;
                HOperatorSet.SmallestRectangle1(ho_SearchRegion, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2); 
                HSmartWindow.disp_message(hSmartWindow.GetWindowHandle(), "搜索区域", "window", hv_Row1 + 2, hv_Column1 + 2, "blue", "false"); 

                ho_SearchRegion.Dispose();
                ho_ImageReduced.Dispose();
            }
            else
            {
                HOperatorSet.FindScaledShapeModel(ho_Image,
                                                        hv_ModelID,
                                                        tModel.StartingAngle,
                                                        tModel.AngleExtent,
                                                        tModel.MinScale,
                                                        tModel.MaxScale,
                                                        tModel.MinScore,
                                                        tModel.NumMatches,
                                                        tModel.MaxOverlap,
                                                        new HTuple(tModel.Subpixel),
                                                        levels,
                                                        tModel.Greediness,
                                                        out hv_OutRow,
                                                        out hv_OutCol,
                                                        out hv_OutAngle,
                                                        out hv_OutScaleRow,
                                                        out hv_Score);
            }

            ho_Contour.Dispose();
            AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_OutRow, hv_OutCol, hv_OutAngle,
               1, 1, 0, out ho_Contour);

            HOperatorSet.ClearShapeModel(hv_ModelID);

            if (hv_OutRow.Length != 0)
            {
                //是否设置参考点
                if (tModel.IsSetPoint)
                {
                    HTuple hv_Point;
                    string pointPath = ModelIDPath + "SetPoint.tup";
                    if (File.Exists(pointPath))
                    {
                        HOperatorSet.ReadTuple(pointPath, out hv_Point);

                        HTuple hv_SetTup;
                        HOperatorSet.ReadTuple(ModelIDPath + "ModelTup.tup", out hv_SetTup);

                        for (int i = 0; i < hv_OutRow.Length; i++)
                        {
                            HTuple hv_HomMat2d, hv_Row, hv_Col;
                            HOperatorSet.VectorAngleToRigid(hv_SetTup[0], hv_SetTup[1], hv_SetTup[2], hv_OutRow[i], hv_OutCol[i], hv_OutAngle[i], out hv_HomMat2d);
                            HOperatorSet.AffineTransPoint2d(hv_HomMat2d, hv_Point[0], hv_Point[1], out hv_Row, out hv_Col);

                            hv_OutRow[i] = Math.Round(hv_Row.D, 3);
                            hv_OutCol[i] = Math.Round(hv_Col.D, 3);
                            hv_OutAngle[i] = Math.Round(hv_OutAngle.D, 3);
                        }
                    }
                }

                //是否设置限定找模板角度 20201218
                if (tModel.IsLimitAngle)
                {
                    for (int i = 0; i < hv_OutAngle.Length; i++)
                    {
                        //2.61是弧度 = 150度    3.66是弧度 = 210度
                        if (hv_OutAngle[i].D > 2.61 && hv_OutAngle[i].D < 3.66)
                        {
                            hv_OutAngle[i] = 3.14 - hv_OutAngle[i];
                        }
                        else if (hv_OutAngle.D < -2.61 && hv_OutAngle[i].D > -3.66)
                        {
                            hv_OutAngle[i] = 3.14 + hv_OutAngle[i];
                        } 
                    }
                }

                return new List<object>() { Math.Round(hv_OutRow.D, 3), Math.Round(hv_OutCol.D, 3), Math.Round(hv_OutAngle.D, 3),
                    ho_Contour, Math.Round(hv_Score.D, 3), (double)hv_OutRow.Length, hv_OutRow.DArr, hv_OutCol.DArr, hv_OutAngle.DArr };
            }
            else
            {
                return null;
            }

        }

        //灰度匹配模板查询
        public static object FindNccShapeModel(HObject ho_Image, HTuple hv_ModelID, NccMatchingModel ParameterSet, HSmartWindow hSmartWindow)
        {
            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);

            HTuple hv_OutRow, hv_OutCol, hv_OutAngle, hv_Score;

            HTuple levels = new HTuple(new int[] { ParameterSet.NumLevels, ParameterSet.LastPyramidLevel });
            if (ParameterSet.IsAutoNumLevels)
            {
                levels = 0;
            }

            string ModelIDPath = Global.Model3DPath + "//" + ParameterSet.Name + "_";
            string strSearchPath = ModelIDPath + "SearchRegion.hobj";
            if (ParameterSet.IsSearchArea && File.Exists(strSearchPath))
            {
                HObject ho_SearchRegion;
                HOperatorSet.GenEmptyObj(out ho_SearchRegion);
                ho_SearchRegion.Dispose();
                HOperatorSet.ReadRegion(out ho_SearchRegion, strSearchPath);

                HObject ho_ImageReduced;
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_SearchRegion, out ho_ImageReduced);

                HTuple angleStart, angleExtent;
                HOperatorSet.TupleRad(ParameterSet.AngleStart, out angleStart);
                HOperatorSet.TupleRad(ParameterSet.AngleExtent, out angleExtent);
                HOperatorSet.FindNccModel(ho_ImageReduced,
                                             hv_ModelID,
                                             angleStart,
                                             angleExtent,
                                             ParameterSet.MinScore,
                                             ParameterSet.NumMatches,
                                             ParameterSet.MaxOverlap,
                                             new HTuple(ParameterSet.Subpixel),
                                             levels,
                                             out hv_OutRow,
                                             out hv_OutCol,
                                             out hv_OutAngle,
                                             out hv_Score);

                hSmartWindow.GetWindowHandle().SetLineWidth(1);
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.GetWindowHandle().SetColor("blue");
                hSmartWindow.DispObj(ho_SearchRegion);

                HTuple hv_Row1, hv_Column1, hv_Row2, hv_Column2;
                HOperatorSet.SmallestRectangle1(ho_SearchRegion, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                HSmartWindow.disp_message(hSmartWindow.GetWindowHandle(), "搜索区域", "window", hv_Row1 + 2, hv_Column1 + 2, "blue", "false");

                ho_ImageReduced.Dispose();
                ho_SearchRegion.Dispose();
            }
            else
            {
                HOperatorSet.FindNccModel(ho_Image,
                                              hv_ModelID,
                                              ParameterSet.AngleStart,
                                              ParameterSet.AngleExtent,
                                              ParameterSet.MinScore,
                                              ParameterSet.NumMatches,
                                              ParameterSet.MaxOverlap,
                                              new HTuple(ParameterSet.Subpixel),
                                              levels,
                                              out hv_OutRow,
                                              out hv_OutCol,
                                              out hv_OutAngle,
                                              out hv_Score);
            }
                

            ho_Contour.Dispose();
            AlgorithmCommHelper.dev_display_ncc_matching_results(hv_ModelID, "red", hv_OutRow, hv_OutCol, hv_OutAngle,
                0, out ho_Contour);

            HOperatorSet.ClearNccModel(hv_ModelID);

            if (hv_OutRow.Length != 0)
            {
                //是否设置限定找模板角度 20201218
                if (ParameterSet.IsLimitAngle)
                {
                    for (int i = 0; i < hv_OutAngle.Length; i++)
                    {
                        //2.61是弧度 = 150度    3.66是弧度 = 210度
                        if (hv_OutAngle[i].D > 2.61 && hv_OutAngle[i].D < 3.66)
                        {
                            hv_OutAngle[i] = 3.14 - hv_OutAngle[i];
                        }
                        else if (hv_OutAngle.D < -2.61 && hv_OutAngle[i].D > -3.66)
                        {
                            hv_OutAngle[i] = 3.14 + hv_OutAngle[i];
                        }
                    }
                }

                return new List<object>() { Math.Round(hv_OutRow.D,3), Math.Round(hv_OutCol.D,3), Math.Round(hv_OutAngle.D, 3),
                    ho_Contour, Math.Round(hv_Score.D, 3) , (double)hv_OutRow.Length};
            }
            else
            {
                return null;
            }
        }

        //检测胶路
        public static bool CheckGule(CheckGlueModel tModel, HObject ho_GlueImageSub, HObject ho_UseNULLImage, HObject ho_KEdgeContour, HSmartWindow hsmartWindow)
        {
            bool bResult = true;
            HObject ho_OutGlueImage = null;
            HObject ho_GlueSelectedRegions = null;
            HOperatorSet.GenEmptyObj(out ho_OutGlueImage);
            HOperatorSet.GenEmptyObj(out ho_GlueSelectedRegions);

            string rowPath = Global.Model3DPath + "//" + tModel.FindLineForm + "_" + "DrawRows";
            string colPath = Global.Model3DPath + "//" + tModel.FindLineForm + "_" + "DrawCols";
            HTuple hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple();
            HOperatorSet.ReadTuple(rowPath, out hv_Rows);
            HOperatorSet.ReadTuple(colPath, out hv_Columns);

            HObject ho_CheckPartionRegion = null;
            HOperatorSet.GenEmptyObj(out ho_CheckPartionRegion);
            HOperatorSet.ReadRegion(out ho_CheckPartionRegion, Global.Model3DPath + "//" + tModel.FindLineForm + "_" + "CheckPartionRegion.hobj");

            //输出 
            HTuple hv_ERROR0 = 1;
            ho_OutGlueImage.Dispose();
            ho_GlueSelectedRegions.Dispose();
            AlgorithmFor3D.GetGlueImage(ho_GlueImageSub, ho_UseNULLImage, ho_CheckPartionRegion, out ho_OutGlueImage,
                out ho_GlueSelectedRegions, 1, 1, tModel.GlueThrLow,
                tModel.GlueThrHigh, tModel.GlueAreaMin, tModel.GlueAreaMax, out hv_ERROR0);


            HTuple hv_GlueWidthValue, hv_GlueInerRow, hv_GlueIneerCol;
            HTuple hv_ERROR1 = 1;
            HObject ho_GlueWidthObject = null;
            HOperatorSet.GenEmptyObj(out ho_GlueWidthObject);
            ho_GlueWidthObject.Dispose();
            AlgorithmFor3D.CheckArcGlueWidth(ho_OutGlueImage, out ho_GlueWidthObject, hv_Rows, hv_Columns,
                tModel.SegMentDist, tModel.RoiWidthLen2, tModel.RoiLen1, tModel.AmplitudeThreshold, tModel.WidthTransition,
                tModel.WidthSelect, tModel.Sigma, tModel.GlueSide, out hv_GlueWidthValue, out hv_GlueInerRow,
                out hv_GlueIneerCol, out hv_ERROR1);


            HObject ho_Contour1 = null;
            HOperatorSet.GenEmptyObj(out ho_Contour1);

            ho_Contour1.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_GlueInerRow, hv_GlueIneerCol);


            //需要判断是否有断胶和溢胶  的 GlueWidthValue
            HTuple hv_Less = new HTuple();
            HTuple hv_Indices1 = new HTuple();
            HObject ho_ObjectSelected = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);


            HOperatorSet.TupleLessElem(hv_GlueWidthValue, tModel.GlueWidthMin, out hv_Less);
            HOperatorSet.TupleFind(hv_Less, 1, out hv_Indices1);
            if ((int)(new HTuple(hv_Indices1.TupleNotEqual(-1))) != 0)
            {
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_GlueWidthObject, out ho_ObjectSelected, hv_Indices1 + 1);
            }

            //判断是否溢胶
            HTuple hv_Greater = new HTuple();
            HTuple hv_Indices = new HTuple();

            HObject ho_ObjectSelected1 = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);


            HOperatorSet.TupleGreaterElem(hv_GlueWidthValue, tModel.GlueWidthMax, out hv_Greater);
            HOperatorSet.TupleFind(hv_Greater, 1, out hv_Indices);
            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
            {
                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_GlueWidthObject, out ho_ObjectSelected1, hv_Indices + 1);
            }

            if (tModel.bEnableGlueWidth)
            {
                HTuple hv_Number = new HTuple();
                HOperatorSet.CountObj(ho_ObjectSelected, out hv_Number);
                if (hv_Number > 10)
                {
                    bResult = false;
                }

                HTuple hv_Number1 = new HTuple();
                HOperatorSet.CountObj(ho_ObjectSelected, out hv_Number1);
                if (hv_Number1 > 10)
                {
                    bResult = false;
                }
            }

            hsmartWindow.GetWindowHandle().DispImage(new HImage(ho_OutGlueImage));
            hsmartWindow.GetWindowHandle().SetLineWidth(2);
            hsmartWindow.GetWindowHandle().SetColor("blue");
            hsmartWindow.GetWindowHandle().SetDraw("margin");

            if (ho_GlueSelectedRegions != null)
            {
                hsmartWindow.DispObj(ho_GlueSelectedRegions);
            }

            if (ho_Contour1 != null)
            {
                hsmartWindow.GetWindowHandle().SetColor("gold");
                hsmartWindow.DispObj(ho_Contour1);
            }

            if (ho_ObjectSelected != null)
            {
                hsmartWindow.GetWindowHandle().SetColor("orange");
                hsmartWindow.DispObj(ho_ObjectSelected);
            }

            if (ho_ObjectSelected1 != null)
            {
                hsmartWindow.GetWindowHandle().SetColor("khaki");
                hsmartWindow.DispObj(ho_ObjectSelected1);
            }


            if (tModel.bEnableGlueHeight)
            {
                bool breturn = CommFuncView.CheckGlueHeight(tModel, ho_GlueWidthObject, ho_GlueImageSub, hsmartWindow);
                if (!breturn)
                {
                    bResult = false;
                }
            }

            if (tModel.bEnableDeterMin)
            {
                bool breturn = CommFuncView.DeterminGlue(tModel, ho_KEdgeContour, hv_GlueInerRow, hv_GlueIneerCol);
                if (!breturn)
                {
                    bResult = false;
                }
            }

            ho_GlueWidthObject.Dispose();

            return bResult;
        }

        //胶宽
        public static bool CheckGlueHeight(CheckGlueModel tModel, HObject ho_GlueWidthObject, HObject ho_GlueImageSub, HSmartWindow hsmartWindow)
        {
            bool bResult = true;
            //设置选择最低胶路高度以上
            HTuple hv_GlueValueMin = tModel.GlueValueMin;
            //胶路高度
            HTuple hv_GlueHeightMedian = new HTuple();
            AlgorithmFor3D.GetGlueHeight(ho_GlueWidthObject, ho_GlueImageSub, hv_GlueValueMin, out hv_GlueHeightMedian);
            //判断胶路高度  
            HTuple hv_Less1 = new HTuple();
            HTuple hv_Indices2 = new HTuple();
            HObject ho_ObjectSelected2 = null;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);

            //找到低于下限的
            HOperatorSet.TupleLessElem(hv_GlueHeightMedian, tModel.GlueHeightMin, out hv_Less1);
            HOperatorSet.TupleFind(hv_Less1, 1, out hv_Indices2);
            if ((int)(new HTuple(hv_Indices2.TupleNotEqual(-1))) != 0)
            {
                ho_ObjectSelected2.Dispose();
                HOperatorSet.SelectObj(ho_GlueWidthObject, out ho_ObjectSelected2, hv_Indices2 + 1);
            }
            else
            {
                ho_ObjectSelected2.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);
            }

            HTuple hv_Number, hv_Newtuple, hv_Area, hv_Row, hv_Column;
            HObject ho_ContCircle1;
            HOperatorSet.GenEmptyObj(out ho_ContCircle1);

            HOperatorSet.CountObj(ho_ObjectSelected2, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
            {
                HOperatorSet.TupleGenConst(hv_Number, 2, out hv_Newtuple);
                HOperatorSet.AreaCenter(ho_ObjectSelected2, out hv_Area, out hv_Row, out hv_Column);
                ho_ContCircle1.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle1, hv_Row, hv_Column, hv_Newtuple,
                    0, 6.28318, "positive", 1);
            }

            if (hv_Number > 10)
            {
                bResult = false;
            }

            if (ho_ContCircle1 != null)
            {
                hsmartWindow.GetWindowHandle().SetColor("violet red");
                hsmartWindow.DispObj(ho_ContCircle1);
            }

            //找到超过上限的
            HTuple hv_Greater1, hv_Indices3;
            HObject ho_ObjectSelected3;
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);

            HOperatorSet.TupleGreaterElem(hv_GlueHeightMedian, tModel.GlueHeightMax, out hv_Greater1);
            HOperatorSet.TupleFind(hv_Greater1, 1, out hv_Indices3);
            if ((int)(new HTuple(hv_Indices3.TupleNotEqual(-1))) != 0)
            {
                ho_ObjectSelected3.Dispose();
                HOperatorSet.SelectObj(ho_GlueWidthObject, out ho_ObjectSelected3, hv_Indices3 + 1);
            }
            else
            {
                ho_ObjectSelected3.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ObjectSelected3);
            }

            HTuple hv_Number1, hv_Newtuple1, hv_Area1, hv_Row4, hv_Column4;
            HObject ho_ContCircle2;
            HOperatorSet.GenEmptyObj(out ho_ContCircle2);

            HOperatorSet.CountObj(ho_ObjectSelected3, out hv_Number1);
            if ((int)(new HTuple(hv_Number1.TupleGreater(0))) != 0)
            {
                HOperatorSet.TupleGenConst(hv_Number1, 2, out hv_Newtuple1);
                HOperatorSet.AreaCenter(ho_ObjectSelected3, out hv_Area1, out hv_Row4, out hv_Column4);
                ho_ContCircle2.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle2, hv_Row4, hv_Column4,
                    hv_Newtuple1, 0, 6.28318, "positive", 1);
            }


            if (hv_Number1 > 10)
            {
                bResult = false;
            }

            if (ho_ContCircle2 != null)
            {
                hsmartWindow.GetWindowHandle().SetColor("indian red");
                hsmartWindow.DispObj(ho_ContCircle2);
            }


            return bResult;
        }

        //胶偏
        public static bool DeterminGlue(CheckGlueModel tModel, HObject ho_KEdgeContours, HTuple hv_GlueInerRow, HTuple hv_GlueIneerCol)
        {
            bool bResult = false;
            //************************判定胶水距离外壳边缘距离
            //输入判定
            HTuple hv_GlueMaxDistanceSTD = 10;
            HTuple hv_GlueMinDistanceSTD = 2;
            //输出
            HTuple hv_GlueMaxValue = new HTuple();
            HTuple hv_GlueMaxNumber = new HTuple();
            HTuple hv_GlueMinValue = new HTuple();
            HTuple hv_GlueMinNumber = new HTuple();

            HTuple hv_MaxGlueInerRow = new HTuple();
            HTuple hv_MaxGlueIneerCol = new HTuple();
            HTuple hv_MinGlueInerRow = new HTuple();
            HTuple hv_MinGlueIneerCol = new HTuple();
            HTuple hv_ERROR = 1;
            AlgorithmFor3D.DeterminGlueValue(ho_KEdgeContours, hv_GlueInerRow, hv_GlueIneerCol, hv_GlueMaxDistanceSTD,
                   hv_GlueMinDistanceSTD, out hv_GlueMaxValue, out hv_GlueMaxNumber, out hv_GlueMinValue,
                   out hv_GlueMinNumber, out hv_MaxGlueInerRow, out hv_MaxGlueIneerCol, out hv_MinGlueInerRow,
                   out hv_MinGlueIneerCol, out hv_ERROR);



            if (hv_GlueMinNumber > 10 || hv_GlueMaxNumber > 10)
            {
                bResult = false;
            }

            return bResult;
        }

        //检测胶路缺陷
        public static HObject CheckGlueDefect(CheckGlueDefectModel tModel, HObject  ho_Image, HObject ho_RegionLine, HSmartWindow hSmartWindow = null)
        {
            if (ho_Image == null)
            {
                ho_Image = (HObject)hSmartWindow.Image;
            }

            string pLinePath = Global.Model3DPath + "//" + tModel.FindLineName + "_LineRegion.hobj";

            HObject ho_LineRegion = null;
            HOperatorSet.GenEmptyObj(out ho_LineRegion);
            if (ho_RegionLine == null)
            {
                ho_LineRegion.Dispose();
                HOperatorSet.ReadRegion(out ho_LineRegion, pLinePath);
                ho_RegionLine = ho_LineRegion;
            }


            HObject ho_RegionOut = new HObject();
            HOperatorSet.GenEmptyObj(out ho_RegionOut);
            HObject ho_RegionOut1 = new HObject();
            HOperatorSet.GenEmptyObj(out ho_RegionOut1);
            HObject ho_RegionOut2 = new HObject();
            HOperatorSet.GenEmptyObj(out ho_RegionOut2);
            HObject ho_RegionOut3 = new HObject();
            HOperatorSet.GenEmptyObj(out ho_RegionOut3);

            ho_RegionOut.Dispose();
            ho_RegionOut1.Dispose();
            ho_RegionOut2.Dispose();
            ho_RegionOut3.Dispose();
            AlgorithmFor3D.CheckGlueDefect2Modul(ho_Image, ho_RegionLine, out ho_RegionOut, out ho_RegionOut1, out ho_RegionOut2, out ho_RegionOut3,
                tModel.CheckEdgeSide, tModel.MoveDistance,
                tModel.WH, tModel.BigValue, tModel.Step, tModel.DefectAreaMin, tModel.EdgeHeightMax,
                tModel.InnerEdgeHeightMax, tModel.ShortValue, tModel.MoveGlueHeight, tModel.MoveGlueInner, tModel.MoveLineEdge);

            hSmartWindow.GetWindowHandle().SetLineWidth(1);
            hSmartWindow.GetWindowHandle().SetDraw("margin");
            //hSmartWindow.GetWindowHandle().SetColor("red");
            //hSmartWindow.GetWindowHandle().DispObj(ho_RegionOut);
            hSmartWindow.GetWindowHandle().SetColor("orange");
            hSmartWindow.DispObj(ho_RegionOut1);
            hSmartWindow.GetWindowHandle().SetColor("blue");
            hSmartWindow.DispObj(ho_RegionOut2);
            hSmartWindow.GetWindowHandle().SetColor("red");
            hSmartWindow.DispObj(ho_RegionOut3);

            return ho_RegionOut;
        }

        //找外壳的第四边
        public static HObject CheckGlueDefectFourth(CheckGlueFourthModel tModel, HObject ho_Image, HObject ho_RegionLine, HSmartWindow hSmartWindow = null)
        {
            if (ho_Image == null)
            {
                ho_Image = (HObject)hSmartWindow.Image;
            }

            string pLinePath = Global.Model3DPath + "//" + tModel.FindLineName + "_LineRegion.hobj";
            string pCheckAreaPath = Global.Model3DPath + "//" + tModel.CheckAreaName + "_CheckRegion.hobj";

            HObject ho_LineRegion = null;
            HOperatorSet.GenEmptyObj(out ho_LineRegion);
            if (ho_RegionLine == null)
            {
                ho_LineRegion.Dispose();
                HOperatorSet.ReadRegion(out ho_LineRegion, pLinePath);
                ho_RegionLine = ho_LineRegion;
            }

            HObject ho_AreaRegion = null;
            HOperatorSet.GenEmptyObj(out ho_AreaRegion);
            ho_AreaRegion.Dispose();
            HOperatorSet.ReadRegion(out ho_AreaRegion, pCheckAreaPath);


            HObject ho_Rectangle1 = new HObject();
            HObject ho_Rectangle2 = new HObject();
            HObject ho_Rectangle3 = new HObject();

            HOperatorSet.Connection(ho_AreaRegion, out ho_AreaRegion);
            HOperatorSet.SelectObj(ho_AreaRegion, out ho_Rectangle1, 1);
            HOperatorSet.SelectObj(ho_AreaRegion, out ho_Rectangle2, 2);
            HOperatorSet.SelectObj(ho_AreaRegion, out ho_Rectangle3, 3);


            HObject ho_RegionOut = new HObject();
            HOperatorSet.GenEmptyObj(out ho_RegionOut);
            ho_RegionOut.Dispose();
            AlgorithmFor3D.CheckGlueDefect2Modul_Fourth(ho_Image, ho_RegionLine, ho_Rectangle1, ho_Rectangle2, ho_Rectangle3, out ho_RegionOut,
               tModel.CheckEdgeSide, tModel.BigValue, tModel.ShortValue, tModel.DefectAreaMin, tModel.MoveGlueHeight,
               tModel.MoveGlueInner, tModel.MoveLineEdge, tModel.DistanceUpMin, tModel.DistanceUpMax, tModel.DistanceMidMin
               , tModel.DistanceMidMax, tModel.DistanceDownMin, tModel.DistanceDownMax, tModel.SubGrayValue);


            hSmartWindow.GetWindowHandle().SetLineWidth(1);
            hSmartWindow.GetWindowHandle().SetDraw("margin");
            hSmartWindow.GetWindowHandle().SetColor("red");
            hSmartWindow.DispObj(ho_RegionOut);

            hSmartWindow.GetWindowHandle().SetColor("blue");
            hSmartWindow.DispObj(ho_AreaRegion);

            return ho_RegionOut;
        }

        //提取外壳边缘
        public static HObject CheckKEdgeContour(CheckKEdgeContourModel tModel, HObject ho_Image, HSmartWindow hSmartWindow = null)
        {


            if (ho_Image == null)
            {
                ho_Image = (HObject)hSmartWindow.Image;
            }


            HTuple hv_KEdgeAllRows = new HTuple();
            HTuple hv_KEdgeAllCols = new HTuple();
            HTuple hv_Error = new HTuple();
            string rowPath = Global.Model3DPath + "//" + tModel.FindLineForm + "_" + "DrawRows";
            string colPath = Global.Model3DPath + "//" + tModel.FindLineForm + "_" + "DrawCols";


            HTuple hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple();
            HOperatorSet.ReadTuple(rowPath, out hv_Rows);
            HOperatorSet.ReadTuple(colPath, out hv_Columns);

            HObject ho_KEdgeContours = null;
            HOperatorSet.GenEmptyObj(out ho_KEdgeContours);

            ho_KEdgeContours.Dispose();
            AlgorithmFor3D.CheckKEdgePointContour(ho_Image, out ho_KEdgeContours, hv_Rows, hv_Columns, tModel.ScaleDivider, tModel.ScaleValue,
                  (new HTuple(tModel.KEdgeBottomRowStart)).TupleConcat(tModel.KEdgeBottomRowEnd), (new HTuple(tModel.KEdgeBottomColStart)).TupleConcat(tModel.KEdgeBottomColEnd),
                  tModel.KEdgeIncreaseTolerance, tModel.KEdgeLength1, tModel.KEdgeLength2, tModel.KEdgeSigma, tModel.KEdgeThreshold,
                  tModel.KEdgeSelect, tModel.KEdgeTransition, tModel.KEdgeNumber1, tModel.KEdgeScore, tModel.KEdgeSmooth,
                  out hv_KEdgeAllRows, out hv_KEdgeAllCols, out hv_Error);

            hSmartWindow.GetWindowHandle().SetLineWidth(2);
            hSmartWindow.GetWindowHandle().SetColor("red");
            hSmartWindow.GetWindowHandle().SetDraw("margin");
            hSmartWindow.DispObj(ho_KEdgeContours);

            return ho_KEdgeContours;
        }
        
        //获取Mat2d
        public static HTuple GetHomMat2d(SingleSequenceModel sequence, string FollowName)
        {
            try
            {
                HTuple hv_Mat2d = new HTuple();
                var CheckModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == FollowName);

                double row = 0;
                double column = 0;
                double angle = 0;
                switch (CheckModel.featureType)
                {
                    case FeatureType.特征匹配:
                        var fixedModel = sequence.FixedItemModels.FirstOrDefault(x => x.Name == FollowName);
                        row = fixedModel.itemResult.Row;
                        column = fixedModel.itemResult.Column;
                        angle = fixedModel.itemResult.Angle;
                        break;
                    case FeatureType.轮廓匹配:
                        var shapeModel = sequence.MatchingModels.FirstOrDefault(x => x.Name == FollowName);
                        row = shapeModel.itemResult.Row;
                        column = shapeModel.itemResult.Column;
                        angle = shapeModel.itemResult.Angle;
                        break;
                    case FeatureType.灰度匹配:
                        var nccModel = sequence.NccMatchingModels.FirstOrDefault(x => x.Name == FollowName);
                        row = nccModel.itemResult.Row;
                        column = nccModel.itemResult.Column;
                        angle = nccModel.itemResult.Angle;
                        break;
                    case FeatureType.交点匹配:
                        var lPModel = sequence.LinePMatchModels.FirstOrDefault(x => x.Name == FollowName);
                        row = lPModel.itemResult.Row;
                        column = lPModel.itemResult.Column;
                        angle = lPModel.itemResult.Angle;
                        break;
                    case FeatureType.直线匹配:
                        var lModel = sequence.LineMatchModels.FirstOrDefault(x => x.Name == FollowName);
                        row = lModel.itemResult.Row;
                        column = lModel.itemResult.Column;
                        angle = lModel.itemResult.Angle;
                        break;
                    case FeatureType.手绘模板:
                        var dModel = sequence.DrawMatchModels.FirstOrDefault(x => x.Name == FollowName);
                        row = dModel.itemResult.Row;
                        column = dModel.itemResult.Column;
                        angle = dModel.itemResult.Angle;
                        break;

                    default:
                        break;
                }

                string strPath = Global.Model3DPath + "//" + FollowName + "_ModelTup.tup";
                HTuple htup = new HTuple();
                HOperatorSet.ReadTuple(strPath, out htup);
                HOperatorSet.VectorAngleToRigid(htup[0], htup[1], htup[2], row, column, angle, out hv_Mat2d);

                return hv_Mat2d;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        //获取模板结果
        public static HTuple GetMatchData(SingleSequenceModel sequence, string FollowName)
        {
            try
            {
                HTuple hv_Data = new HTuple();
                var CheckModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == FollowName);

                double row = 0;
                double column = 0;
                double angle = 0;
                switch (CheckModel.featureType)
                {
                    case FeatureType.特征匹配:
                        var fixedModel = sequence.FixedItemModels.FirstOrDefault(x => x.Name == FollowName);
                        row = fixedModel.itemResult.Row;
                        column = fixedModel.itemResult.Column;
                        angle = fixedModel.itemResult.Angle;
                        break;
                    case FeatureType.轮廓匹配:
                        var shapeModel = sequence.MatchingModels.FirstOrDefault(x => x.Name == FollowName);
                        row = shapeModel.itemResult.Row;
                        column = shapeModel.itemResult.Column;
                        angle = shapeModel.itemResult.Angle;
                        break;
                    case FeatureType.灰度匹配:
                        var nccModel = sequence.NccMatchingModels.FirstOrDefault(x => x.Name == FollowName);
                        row = nccModel.itemResult.Row;
                        column = nccModel.itemResult.Column;
                        angle = nccModel.itemResult.Angle;
                        break;
                    case FeatureType.交点匹配:
                        var lPModel = sequence.LinePMatchModels.FirstOrDefault(x => x.Name == FollowName);
                        row = lPModel.itemResult.Row;
                        column = lPModel.itemResult.Column;
                        angle = lPModel.itemResult.Angle;
                        break;
                    case FeatureType.直线匹配:
                        var lModel = sequence.LineMatchModels.FirstOrDefault(x => x.Name == FollowName);
                        row = lModel.itemResult.Row;
                        column = lModel.itemResult.Column;
                        angle = lModel.itemResult.Angle;
                        break;

                    default:
                        break;
                }

                hv_Data.Append(row);
                hv_Data.Append(column);
                hv_Data.Append(angle);

                return hv_Data;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        //取线轮廓-立讯
        public static List<object> GetLineContour(SingleSequenceModel sequence, LuxGetLineModel model, HSmartWindow hSmartWindow)
        {
            try
            {
                // Local iconic variables 

                HObject ho_Image, ho_BaseRegion, ho_Region1;
                HObject ho_RegionAffineTrans1, ho_RegionAffineTrans, ho_ImageScaled;
                HObject ho_ImageReduced, ho_Region3, ho_RegionFillUp, ho_ConnectedRegions;
                HObject ho_SelectedRegions, ho_RegionOpening, ho_ImageReduced1;
                HObject ho_OutRegion, ho_Rectangle, ho_Cross3, ho_Cross, ho_Cross2;

                // Local control variables 

                HTuple hv_MinGray = null, hv_MaxGray = null;
                HTuple hv_LineMinGray = null, hv_PointTup = null, hv_Row = null;
                HTuple hv_Column = null, hv_HomMat2D = null, hv_Value = null;
                HTuple hv_CenterRow = null, hv_CenterColumn = null, hv_Phi = null;
                HTuple hv_Length1 = null, hv_Length2 = null, hv_drow = null;
                HTuple hv_dcol = null, hv_uprow = null, hv_upcol = null;
                HTuple hv_downrow = null, hv_downcol = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_BaseRegion);
                HOperatorSet.GenEmptyObj(out ho_Region1);
                HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans1);
                HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
                HOperatorSet.GenEmptyObj(out ho_ImageScaled);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_Region3);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
                HOperatorSet.GenEmptyObj(out ho_OutRegion);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_Cross3);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_Cross2);


                ho_Image.Dispose();
                ho_Image = (HObject)XmlControl.GetLinkValue(model.ImageForm);


                hv_MinGray = model.MinGray;
                hv_MaxGray = model.MaxGray;

                hv_LineMinGray = model.LineMinGray;

                ho_BaseRegion.Dispose();
                ho_BaseRegion = (HObject)XmlControl.GetLinkValue(model.BaseRegionForm);

                ho_Region1.Dispose();
                ho_Region1 = (HObject)XmlControl.GetLinkValue(model.CheckRegionForm);

                //string strPath = GlobalCore.Global.Model3DPath + "//" + model.PointTupForm + "_PointTup.tup";
                //HOperatorSet.ReadTuple(strPath, out hv_PointTup);

                ////线的交点坐标
                //hv_Row = (double)XmlControl.GetLinkValue(model.LinePointRowForm);
                //hv_Column = (double)XmlControl.GetLinkValue(model.LinePointColForm);

                //HOperatorSet.VectorAngleToRigid(hv_PointTup.TupleSelect(0), hv_PointTup.TupleSelect(
                //    1), 0, hv_Row, hv_Column, 0, out hv_HomMat2D);

                //使用模板跟随 
                hv_HomMat2D = GetHomMat2d(sequence, model.ModelForm);

                ho_RegionAffineTrans1.Dispose();
                HOperatorSet.AffineTransRegion(ho_Region1, out ho_RegionAffineTrans1, hv_HomMat2D,
                    "nearest_neighbor");
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_BaseRegion, out ho_RegionAffineTrans, hv_HomMat2D,
                    "nearest_neighbor");

                HOperatorSet.GrayFeatures(ho_RegionAffineTrans, ho_Image, "mean", out hv_Value);

                //获取焊锡1的位置轮廓
                ho_ImageScaled.Dispose();
                HOperatorSet.ScaleImage(ho_Image, out ho_ImageScaled, 1, -hv_Value);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageScaled, ho_RegionAffineTrans1, out ho_ImageReduced
                    );
                ho_Region3.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region3, hv_MinGray, hv_MaxGray);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_Region3, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                    70);

                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_SelectedRegions, out ho_RegionOpening, 23.5);

                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageScaled, ho_RegionOpening, out ho_ImageReduced1
                    );
                ho_OutRegion.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced1, out ho_OutRegion, hv_LineMinGray, hv_MaxGray);

                //获取最大面积的
                HOperatorSet.Connection(ho_OutRegion, out ho_OutRegion);
                HOperatorSet.SelectShapeStd(ho_OutRegion, out ho_OutRegion, "max_area",
                   70);

                //测试偏位
                HOperatorSet.SmallestRectangle2(ho_OutRegion, out hv_CenterRow, out hv_CenterColumn,
                    out hv_Phi, out hv_Length1, out hv_Length2);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_CenterRow, hv_CenterColumn, hv_Phi,
                    hv_Length1, hv_Length2);
                ho_Cross3.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_CenterRow, hv_CenterColumn,
                    15, hv_Phi);
                hv_drow = hv_Length1 * (hv_Phi.TupleSin());
                hv_dcol = hv_Length1 * (hv_Phi.TupleCos());
                hv_uprow = hv_CenterRow - hv_drow;
                hv_upcol = hv_CenterColumn + hv_dcol;
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_uprow, hv_upcol, 15, 0);
                hv_downrow = hv_CenterRow + hv_drow;
                hv_downcol = hv_CenterColumn - hv_dcol;
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_downrow, hv_downcol, 15, 0);

                HTuple row1, col1, row2, col2;
                HOperatorSet.SmallestRectangle1(ho_RegionOpening, out row1, out col1, out row2, out col2);

                HObject ho_RegionLine1 = null;
                HObject ho_RegionLine2 = null;
                HOperatorSet.GenEmptyObj(out ho_RegionLine1);
                HOperatorSet.GenEmptyObj(out ho_RegionLine2);

                ho_RegionLine1.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLine1, row1, col1, row1, col2);
                ho_RegionLine2.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLine2, row1, col1, row2, col1);


                //ho_BaseRegion.Dispose();
                //ho_Region1.Dispose();
                //ho_RegionAffineTrans1.Dispose();
                //ho_RegionAffineTrans.Dispose();
                ho_ImageScaled.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region3.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced1.Dispose();
                //ho_OutRegion.Dispose();
                ho_Rectangle.Dispose();
                //ho_Cross3.Dispose();
                //ho_Cross.Dispose();

                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.GetWindowHandle().SetDraw("margin");
                hSmartWindow.DispObj(ho_Cross3);
                hSmartWindow.DispObj(ho_Cross);
                hSmartWindow.DispObj(ho_Cross2);

                hSmartWindow.GetWindowHandle().SetColor("yellow");
                hSmartWindow.DispObj(ho_OutRegion);

                hSmartWindow.GetWindowHandle().SetColor("blue");
                hSmartWindow.DispObj(ho_RegionLine1);
                hSmartWindow.DispObj(ho_RegionLine2);

                hSmartWindow.GetWindowHandle().SetColor("green");
                hSmartWindow.DispObj(ho_RegionAffineTrans1);
                hSmartWindow.GetWindowHandle().SetColor("light blue");
                hSmartWindow.DispObj(ho_RegionAffineTrans);

                double dUpRow = Math.Round(hv_uprow.D, 1);
                double dUpCol = Math.Round(hv_upcol.D, 1);
                double dDownRow = Math.Round(hv_downrow.D, 1);
                double dDownCol = Math.Round(hv_downcol.D, 1);
                if (hv_uprow.D > hv_downrow.D)
                {
                    dUpRow = Math.Round(hv_downrow.D, 1);
                    dUpCol = Math.Round(hv_downcol.D, 1);
                    dDownRow = Math.Round(hv_uprow.D, 1);
                    dDownCol = Math.Round(hv_upcol.D, 1);
                }

                double dCenterRow = Math.Round(hv_CenterRow.D, 1);
                double dCenterCol = Math.Round(hv_CenterColumn.D, 1);

                return new List<object> { dUpRow, dUpCol, dCenterRow, dCenterCol, dDownRow, dDownCol, row1.D, col1.D, row2.D, col2.D, ho_OutRegion };
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }

        //OCR字符识别
        public static HTuple OcrFindText_暂时无用(OcrModel ocrModel, HObject ho_Image, HSmartWindow hSmartWindow)
        {
            // Local iconic variables 
            HTuple hv_Result = new HTuple();

            HObject ho_Region = null, ho_RegionOpening = null;
            HObject ho_ConnectedRegions = null, ho_SelectedRegion = null;
            HObject ho_ImageReduced = null, ho_ImageScaleMax = null, ho_ImageRotate = null;
            HObject ho_Line = null, ho_Character = null;

            // Local control variables 

            HTuple hv_WindowHandle = null, hv_OCRHandle = null;
            HTuple hv_TextModel = null, hv_TextPattern1 = null, hv_TextPattern2 = null;
            HTuple hv_Expression = null;
            HTuple hv_UsedThreshold = new HTuple(), hv_OrientationAngle = new HTuple();
            HTuple hv_TextResult = new HTuple(), hv_NumLines = new HTuple();
            HTuple hv_J = new HTuple(), hv_Class = new HTuple(), hv_Confidence = new HTuple();
            HTuple hv_Word = new HTuple(), hv_Score = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_NumberOfCharacters = new HTuple(), hv_K = new HTuple();
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageScaleMax);
            HOperatorSet.GenEmptyObj(out ho_ImageRotate);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Character);
            try
            {
                hv_WindowHandle = hSmartWindow.GetWindowHandle();

                //Read the OCR classifier from file
                HOperatorSet.ReadOcrClassMlp("DotPrint_NoRej", out hv_OCRHandle);
                //
                //Create the text model and specify the text properties
                HOperatorSet.CreateTextModelReader("manual", new HTuple(), out hv_TextModel);
                //
                //HOperatorSet.SetTextModelParam(hv_TextModel, "manual_char_width", 24);
                //HOperatorSet.SetTextModelParam(hv_TextModel, "manual_char_height", 33);
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_char_width", ocrModel.MaxCharWidth);
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_char_height", ocrModel.MaxCharHeight);
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_is_dotprint", "true");
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_max_line_num", 2);
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_return_punctuation", "false");
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_return_separators", "false");
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_stroke_width", 4);
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_eliminate_horizontal_lines",
                    "true");
               
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_text_line_structure_0",
                    "6 1 8");
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_text_line_structure_1",
                    "8 10");
                HOperatorSet.SetTextModelParam(hv_TextModel, "manual_text_line_structure_2",
                    "19");
                
                //in do_ocr_word_mlp to increase the robustness of the OCR.
                hv_TextPattern1 = "(FLEXID[0-9][A-Z][0-9]{3}[A-F0-9]{4})";
                hv_TextPattern2 = "([A-Z]{3}[0-9]{5}.?[A-Z][0-9]{4}[A-Z][0-9]{4})";
                hv_Expression = (hv_TextPattern1 + "|") + hv_TextPattern2;

                //
                //Preprocessing:
                ho_Region.Dispose();
                HOperatorSet.BinaryThreshold(ho_Image, out ho_Region, "max_separability",
                    "dark", out hv_UsedThreshold);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningRectangle1(ho_Region, out ho_RegionOpening, 400, 50);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ErosionRectangle1(ho_RegionOpening, out ExpTmpOutVar_0, 11,
                        11);
                    ho_RegionOpening.Dispose();
                    ho_RegionOpening = ExpTmpOutVar_0;
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegion.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegion, "max_area",
                    70);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_SelectedRegion, out ho_ImageReduced
                    );
                ho_ImageScaleMax.Dispose();
                HOperatorSet.ScaleImageMax(ho_ImageReduced, out ho_ImageScaleMax);
                HOperatorSet.TextLineOrientation(ho_SelectedRegion, ho_ImageScaleMax, 30,
                    (new HTuple(-30)).TupleRad(), (new HTuple(30)).TupleRad(), out hv_OrientationAngle);
                ho_ImageRotate.Dispose();
                HOperatorSet.RotateImage(ho_ImageScaleMax, out ho_ImageRotate, ((-hv_OrientationAngle)).TupleDeg()
                    , "constant");
                //
                //Find text and display results for every segmented region
                HOperatorSet.FindText(ho_ImageRotate, hv_TextModel, out hv_TextResult);
                HOperatorSet.GetTextResult(hv_TextResult, "manual_num_lines", out hv_NumLines);
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_ImageRotate, HDevWindowStack.GetActive());
                }
                HTuple end_val61 = hv_NumLines - 1;
                HTuple step_val61 = 1;
                for (hv_J = 0; hv_J.Continue(end_val61, step_val61); hv_J = hv_J.TupleAdd(step_val61))
                {
                    ho_Line.Dispose();
                    HOperatorSet.GetTextObject(out ho_Line, hv_TextResult, (new HTuple("manual_line")).TupleConcat(
                        hv_J));
                    //The OCR uses regular expressions to read the text
                    //more robustly.
                    HOperatorSet.DoOcrWordMlp(ho_Line, ho_ImageRotate, hv_OCRHandle, hv_Expression,
                        3, 5, out hv_Class, out hv_Confidence, out hv_Word, out hv_Score);
                    hv_Result.Append(hv_Word);
                    //
                    //Display results
                    HOperatorSet.SmallestRectangle1(ho_Line, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);
                    HOperatorSet.CountObj(ho_Line, out hv_NumberOfCharacters);

                    HTuple end_val73 = hv_NumberOfCharacters;
                    HTuple step_val73 = 1;
                    for (hv_K = 1; hv_K.Continue(end_val73, step_val73); hv_K = hv_K.TupleAdd(step_val73))
                    {
                        ho_Character.Dispose();
                        HOperatorSet.SelectObj(ho_Line, out ho_Character, hv_K);
                        HOperatorSet.SetTposition(hv_WindowHandle, (hv_Row2.TupleSelect(0)) + 4,
                            hv_Column1.TupleSelect(hv_K - 1));
                        HOperatorSet.WriteString(hv_WindowHandle, hv_Word.TupleStrBitSelect(hv_K - 1));
                    }
                }
                //Clean up memory
                HOperatorSet.ClearTextResult(hv_TextResult);

                //Clean up memory
                HOperatorSet.ClearTextModel(hv_TextModel);
                HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);

            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegion.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageScaleMax.Dispose();
                ho_ImageRotate.Dispose();
                ho_Line.Dispose();
                ho_Character.Dispose();

                throw HDevExpDefaultException;
            }
            ho_Region.Dispose();
            ho_RegionOpening.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegion.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageScaleMax.Dispose();
            ho_ImageRotate.Dispose();
            ho_Line.Dispose();
            ho_Character.Dispose();

            return hv_Result;
        }

        public static HTuple OcrFindText(OcrModel tModel, HObject ho_Image, HSmartWindow hSmartWindow)
        {
            // Local iconic variables 
            HObject ho_TextLines;

            // Local control variables 

            HTuple hv_WindowHandle = null, hv_OCRHandle = null;
            HTuple hv_TextModel = null, hv_TextResult = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null, hv_SingleCharacters = null, hv_TextLineCharacters = null;
            HTuple hv_CharacterIndex = null;
            // Initialize local and output iconic variables 

            hv_WindowHandle = hSmartWindow.GetWindowHandle();
            HOperatorSet.GenEmptyObj(out ho_TextLines);
            try
            {
                //Read OCR classifier and create the text model

                if (tModel.IsTrain)
                {
                    HOperatorSet.ReadOcrClassMlp(Global.Model3DPath + "//" + tModel.TrainName + ".omc", out hv_OCRHandle);
                }
                else
                {
                    HOperatorSet.ReadOcrClassMlp(tModel.OcrType + ".omc", out hv_OCRHandle);
                }

                //An OCR Classifier based on a multilayer perceptron (MLP) is required
                HOperatorSet.CreateTextModelReader("auto", hv_OCRHandle, out hv_TextModel);
                HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);

                if (tModel.CharColor == "黑底白字")
                { 
                    HOperatorSet.SetTextModelParam(hv_TextModel, "polarity", "light_on_dark");
                }
                else
                {
                    HOperatorSet.SetTextModelParam(hv_TextModel, "polarity", "dark_on_light");
                }

                HOperatorSet.SetTextModelParam(hv_TextModel, "min_char_height", tModel.MinCharHeight);
                HOperatorSet.SetTextModelParam(hv_TextModel, "max_char_height", tModel.MaxCharHeight);
                HOperatorSet.SetTextModelParam(hv_TextModel, "min_char_width", tModel.MinCharWidth);
                HOperatorSet.SetTextModelParam(hv_TextModel, "max_char_width", tModel.MaxCharWidth);
                HOperatorSet.SetTextModelParam(hv_TextModel, "min_contrast", tModel.MinContrast);
                //set_text_model_param (TextModel, 'text_line_separators', '/')
                //set_text_model_param (TextModel, 'text_line_structure', '2 4')
                 
                //text within the input image
                HOperatorSet.FindText(ho_Image, hv_TextModel, out hv_TextResult);
                //
                //The segmented regions can be obtained by calling get_text_object
                ho_TextLines.Dispose();
                HOperatorSet.GetTextObject(out ho_TextLines, hv_TextResult, "all_lines");
                 

                //Display the single characters
                HOperatorSet.SmallestRectangle1(ho_TextLines, out hv_Row1, out hv_Column1,
                    out hv_Row2, out hv_Column2);
                //gen_rectangle1 (Rectangle1, Row1, Column1, Row2, Column2)
                HOperatorSet.GetTextResult(hv_TextResult, "class", out hv_SingleCharacters);
                HOperatorSet.TupleSum(hv_SingleCharacters, out hv_TextLineCharacters);

                HOperatorSet.SetColor(hv_WindowHandle, "lime green"); 

                for (hv_CharacterIndex = 0; (int)hv_CharacterIndex <= (int)((new HTuple(hv_SingleCharacters.TupleLength()
                    )) - 1); hv_CharacterIndex = (int)hv_CharacterIndex + 1)
                {
                    HOperatorSet.SetTposition(hv_WindowHandle, (hv_Row2.TupleSelect(hv_CharacterIndex)) + 10,
                        hv_Column1.TupleSelect(hv_CharacterIndex));
                    HOperatorSet.WriteString(hv_WindowHandle, hv_SingleCharacters.TupleSelect(
                        hv_CharacterIndex));
                }

                HOperatorSet.ClearTextResult(hv_TextResult);
                HOperatorSet.ClearTextModel(hv_TextModel);
                 
            }
            catch (HalconException HDevExpDefaultException)
            { 
                ho_TextLines.Dispose();

                throw HDevExpDefaultException;
            } 
            ho_TextLines.Dispose();

            return hv_TextLineCharacters;

        }

        //利用已找到的Region来直接Ocr
        public static HTuple Ocr_FindText_Region(OcrModel tModel, HObject ho_Image, HObject ho_Region, HSmartWindow hSmartWindow)
        { 
            HObject ho_Characters, ho_SortedRegions;

            // Local control variables 

            HTuple hv_WindowHandle = null, hv_Area = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_OCRHandle = null;
            HTuple hv_Class = null, hv_Confidence = null, hv_Result = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_Characters);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            try
            {
                //先过滤掉一些无用的
                HTuple hv_Number; 
                HOperatorSet.CountObj(ho_Region, out hv_Number);
                if(hv_Number.I == 1)
                { 
                    HOperatorSet.Connection(ho_Region, out ho_Region);
                }
                HOperatorSet.SelectShape(ho_Region, out ho_Region, ((new HTuple("area")).TupleConcat("height")).TupleConcat("width"), 
                    "and", ((new HTuple(tModel.MinCharArea)).TupleConcat(tModel.MinCharHeight)).TupleConcat(tModel.MinCharWidth),
                    ((new HTuple(tModel.MaxCharArea)).TupleConcat(tModel.MaxCharHeight)).TupleConcat(tModel.MaxCharWidth));

                hv_WindowHandle = hSmartWindow.GetWindowHandle(); 
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PartitionDynamic(ho_Region, out ExpTmpOutVar_0, tModel.MaxCharWidth, 20);
                    ho_Region.Dispose();
                    ho_Region = ExpTmpOutVar_0;
                }

                //Filter out resulting empty regions
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row1, out hv_Column1);
                ho_Characters.Dispose();
                HSmartWindow.select_mask_obj(ho_Region, out ho_Characters, hv_Area.TupleGreaterElem(0));                
                
                if(tModel.IsTrain)
                {
                    HOperatorSet.ReadOcrClassMlp(Global.Model3DPath + "//" + tModel.TrainName + ".omc", out hv_OCRHandle);
                }
                else
                {
                    HOperatorSet.ReadOcrClassMlp(tModel.OcrType + ".omc", out hv_OCRHandle);
                }

                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_Characters, out ho_SortedRegions, "character", "true",
                    "row");
                HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row1, out hv_Column1);
                HOperatorSet.DoOcrMultiClassMlp(ho_SortedRegions, ho_Image, hv_OCRHandle, out hv_Class, out hv_Confidence);

                //替换
                HOperatorSet.TupleRegexpReplace(hv_Class.TupleSum(), "0", "0", out hv_Result);   

                HSmartWindow.disp_message(hv_WindowHandle, hv_Class, "window", hv_Row1 + 30, hv_Column1, "lime green", "false");  
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Characters.Dispose();
                ho_SortedRegions.Dispose();

                throw HDevExpDefaultException;
            } 
            ho_Characters.Dispose();
            ho_SortedRegions.Dispose();

            return hv_Class;
        }

        //测清晰度
        public static void Evaluate_definition(HObject ho_Image, HTuple hv_Method, out HTuple hv_Value)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageMean = null, ho_ImageSub = null;
            HObject ho_ImageResult = null, ho_ImageLaplace4 = null, ho_ImageLaplace8 = null;
            HObject ho_ImageResult1 = null, ho_ImagePart00 = null, ho_ImagePart01 = null;
            HObject ho_ImagePart10 = null, ho_ImageSub1 = null, ho_ImageSub2 = null;
            HObject ho_ImageResult2 = null, ho_ImagePart20 = null, ho_EdgeAmplitude = null;
            HObject ho_Region1 = null, ho_BinImage = null, ho_ImageResult4 = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Deviation = new HTuple();
            HTuple hv_Min = new HTuple(), hv_Max = new HTuple(), hv_Range = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageLaplace4);
            HOperatorSet.GenEmptyObj(out ho_ImageLaplace8);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho_ImagePart00);
            HOperatorSet.GenEmptyObj(out ho_ImagePart01);
            HOperatorSet.GenEmptyObj(out ho_ImagePart10);
            HOperatorSet.GenEmptyObj(out ho_ImageSub1);
            HOperatorSet.GenEmptyObj(out ho_ImageSub2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult2);
            HOperatorSet.GenEmptyObj(out ho_ImagePart20);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_BinImage);
            HOperatorSet.GenEmptyObj(out ho_ImageResult4);
            hv_Value = new HTuple();
            try
            {
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImageMax(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0);
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                HOperatorSet.GetImageSize(ho_Image_COPY_INP_TMP, out hv_Width, out hv_Height);

                if ((int)(new HTuple(hv_Method.TupleEqual("Deviation"))) != 0)
                {
                    //方差法
                    ho_ImageMean.Dispose();
                    HOperatorSet.RegionToMean(ho_Image_COPY_INP_TMP, ho_Image_COPY_INP_TMP, out ho_ImageMean
                        );
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImageMean, out ExpTmpOutVar_0, "real");
                        ho_ImageMean.Dispose();
                        ho_ImageMean = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0,
                            "real");
                        ho_Image_COPY_INP_TMP.Dispose();
                        ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                    ho_ImageSub.Dispose();
                    HOperatorSet.SubImage(ho_Image_COPY_INP_TMP, ho_ImageMean, out ho_ImageSub,
                        1, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub, ho_ImageSub, out ho_ImageResult, 1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);

                }
                else if ((int)(new HTuple(hv_Method.TupleEqual("laplace"))) != 0)
                {
                    //拉普拉斯能量函数
                    ho_ImageLaplace4.Dispose();
                    HOperatorSet.Laplace(ho_Image_COPY_INP_TMP, out ho_ImageLaplace4, "signed",
                        3, "n_4");
                    ho_ImageLaplace8.Dispose();
                    HOperatorSet.Laplace(ho_Image_COPY_INP_TMP, out ho_ImageLaplace8, "signed",
                        3, "n_8");
                    ho_ImageResult1.Dispose();
                    HOperatorSet.AddImage(ho_ImageLaplace4, ho_ImageLaplace4, out ho_ImageResult1,
                        1, 0);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AddImage(ho_ImageLaplace4, ho_ImageResult1, out ExpTmpOutVar_0,
                            1, 0);
                        ho_ImageResult1.Dispose();
                        ho_ImageResult1 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AddImage(ho_ImageLaplace8, ho_ImageResult1, out ExpTmpOutVar_0,
                            1, 0);
                        ho_ImageResult1.Dispose();
                        ho_ImageResult1 = ExpTmpOutVar_0;
                    }
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageResult1, ho_ImageResult1, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);

                }
                else if ((int)(new HTuple(hv_Method.TupleEqual("energy"))) != 0)
                {
                    //能量梯度函数
                    ho_ImagePart00.Dispose();
                    HOperatorSet.CropPart(ho_Image_COPY_INP_TMP, out ho_ImagePart00, 0, 0, hv_Width - 1,
                        hv_Height - 1);
                    ho_ImagePart01.Dispose();
                    HOperatorSet.CropPart(ho_Image_COPY_INP_TMP, out ho_ImagePart01, 0, 1, hv_Width - 1,
                        hv_Height - 1);
                    ho_ImagePart10.Dispose();
                    HOperatorSet.CropPart(ho_Image_COPY_INP_TMP, out ho_ImagePart10, 1, 0, hv_Width - 1,
                        hv_Height - 1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart00, out ExpTmpOutVar_0, "real");
                        ho_ImagePart00.Dispose();
                        ho_ImagePart00 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart10, out ExpTmpOutVar_0, "real");
                        ho_ImagePart10.Dispose();
                        ho_ImagePart10 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart01, out ExpTmpOutVar_0, "real");
                        ho_ImagePart01.Dispose();
                        ho_ImagePart01 = ExpTmpOutVar_0;
                    }
                    ho_ImageSub1.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart10, ho_ImagePart00, out ho_ImageSub1, 1,
                        0);
                    ho_ImageResult1.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub1, ho_ImageSub1, out ho_ImageResult1, 1,
                        0);
                    ho_ImageSub2.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart01, ho_ImagePart00, out ho_ImageSub2, 1,
                        0);
                    ho_ImageResult2.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub2, ho_ImageSub2, out ho_ImageResult2, 1,
                        0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.AddImage(ho_ImageResult1, ho_ImageResult2, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                }
                else if ((int)(new HTuple(hv_Method.TupleEqual("Brenner"))) != 0)
                {
                    //Brenner函数法
                    ho_ImagePart00.Dispose();
                    HOperatorSet.CropPart(ho_Image_COPY_INP_TMP, out ho_ImagePart00, 0, 0, hv_Width,
                        hv_Height - 2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart00, out ExpTmpOutVar_0, "real");
                        ho_ImagePart00.Dispose();
                        ho_ImagePart00 = ExpTmpOutVar_0;
                    }
                    ho_ImagePart20.Dispose();
                    HOperatorSet.CropPart(ho_Image_COPY_INP_TMP, out ho_ImagePart20, 2, 0, hv_Width,
                        hv_Height - 2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart20, out ExpTmpOutVar_0, "real");
                        ho_ImagePart20.Dispose();
                        ho_ImagePart20 = ExpTmpOutVar_0;
                    }
                    ho_ImageSub.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart20, ho_ImagePart00, out ho_ImageSub, 1,
                        0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub, ho_ImageSub, out ho_ImageResult, 1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                }
                else if ((int)(new HTuple(hv_Method.TupleEqual("Tenegrad"))) != 0)
                {
                    //Tenegrad函数法
                    ho_EdgeAmplitude.Dispose();
                    HOperatorSet.SobelAmp(ho_Image_COPY_INP_TMP, out ho_EdgeAmplitude, "sum_sqrt",
                        3);
                    HOperatorSet.MinMaxGray(ho_EdgeAmplitude, ho_EdgeAmplitude, 0, out hv_Min,
                        out hv_Max, out hv_Range);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_EdgeAmplitude, out ho_Region1, 11.8, 255);
                    ho_BinImage.Dispose();
                    HOperatorSet.RegionToBin(ho_Region1, out ho_BinImage, 1, 0, hv_Width, hv_Height);
                    ho_ImageResult4.Dispose();
                    HOperatorSet.MultImage(ho_EdgeAmplitude, ho_BinImage, out ho_ImageResult4,
                        1, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageResult4, ho_ImageResult4, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);

                }

                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageMean.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageLaplace4.Dispose();
                ho_ImageLaplace8.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImagePart00.Dispose();
                ho_ImagePart01.Dispose();
                ho_ImagePart10.Dispose();
                ho_ImageSub1.Dispose();
                ho_ImageSub2.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImagePart20.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_Region1.Dispose();
                ho_BinImage.Dispose();
                ho_ImageResult4.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageMean.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageLaplace4.Dispose();
                ho_ImageLaplace8.Dispose();
                ho_ImageResult1.Dispose();
                ho_ImagePart00.Dispose();
                ho_ImagePart01.Dispose();
                ho_ImagePart10.Dispose();
                ho_ImageSub1.Dispose();
                ho_ImageSub2.Dispose();
                ho_ImageResult2.Dispose();
                ho_ImagePart20.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_Region1.Dispose();
                ho_BinImage.Dispose();
                ho_ImageResult4.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //畸变标定来校正图像
        public static HObject MapImage(HObject ho_Image, string strName)
        {
            try
            { 
                // Local iconic variables 
                HObject ho_Map, ho_ImageMapped;

                // Local control variables 
                HTuple hv_CameraParam = null;
                HTuple hv_CameraPose = null, hv_CarParamVirtualFixed = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Map);
                HOperatorSet.GenEmptyObj(out ho_ImageMapped);

                string path1 = Global.Model3DPath + "//" + strName + "_campar.dat";
                string path2 = Global.Model3DPath + "//" + strName + "_campos.dat";
                HOperatorSet.ReadCamPar(path1, out hv_CameraParam);
                HOperatorSet.ReadPose(path2, out hv_CameraPose);

                //图像矫正

                //②仅使用内参
                ho_Image.Dispose();
                hv_CarParamVirtualFixed = hv_CameraParam.Clone();

                //对于area(diversion)相机模型(7个参数)
                HOperatorSet.ChangeRadialDistortionCamPar("fixed", hv_CameraParam, 0, out hv_CarParamVirtualFixed);
                //上述相机模型选一种后进行下面的map_image
                //创建一个投射图，其描述图像与其相应正在改变的径向畸变，而对于12个参数的畸变包括径向和切向畸变
                ho_Map.Dispose();
                HOperatorSet.GenRadialDistortionMap(out ho_Map, hv_CameraParam, hv_CarParamVirtualFixed,
                    "bilinear");

                ho_ImageMapped.Dispose();
                HOperatorSet.MapImage(ho_Image, ho_Map, out ho_ImageMapped);
                ho_Image.Dispose();
                ho_Map.Dispose();

                return ho_ImageMapped;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return null;
            }
        }
        
        //保存数据到txt文件
        public static void WriteTxt(String strFileName, String strLogDesc)
        {
            try
            {  
                string dt = "_" + DateTime.Now.ToString("yyyy_MM_dd");//加上日期

                Task task = new Task(new Action(() =>
                {
                    try
                    {
                        if (!strFileName.Contains(".txt"))
                        {
                            strFileName += dt + ".txt";
                        }
                        StreamWriter writer = File.AppendText(strFileName);

                        writer.WriteLine(strLogDesc);
                        writer.Flush();
                        writer.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }));
                task.Start();
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        //根据条件来实现筛选点
        public static List<double> GetPointValue(HObject ho_Image, HObject ho_CheckAreaRegion, PointToAreaModel tModel)
        {
            try
            {
                List<double> listValue = new List<double>();
                //如果是有多个检测区域
                HTuple hv_num;
                HOperatorSet.Connection(ho_CheckAreaRegion, out ho_CheckAreaRegion);
                HOperatorSet.CountObj(ho_CheckAreaRegion, out hv_num);
                HObject ho_Obj;
                HOperatorSet.GenEmptyObj(out ho_Obj);

                for (int i = 0; i < hv_num.I; i++)
                {
                    HTuple hv_Rows, hv_Columns, hv_GrayVal, hv_Sorted, hv_Length;

                    ho_Obj.Dispose();
                    HOperatorSet.SelectObj(ho_CheckAreaRegion, out ho_Obj, i + 1);

                    HOperatorSet.GetRegionPoints(ho_Obj, out hv_Rows, out hv_Columns);
                    HOperatorSet.GetGrayval(ho_Image, hv_Rows, hv_Columns, out hv_GrayVal);
                    //HOperatorSet.TupleAbs(hv_GrayVal, out hv_GrayVal);
                    HOperatorSet.TupleSort(hv_GrayVal, out hv_Sorted);
                    HOperatorSet.TupleLength(hv_Sorted, out hv_Length);

                    HTuple hv_Value = new HTuple();
                    HTuple hv_SelectValue = new HTuple();

                    //筛选值
                    if (tModel.FilterType == "全部值")
                    {
                        hv_SelectValue = hv_Sorted;
                    }
                    else if (tModel.FilterType == "移除最大i个点，选取剩余最大j个点")
                    {
                        HTuple hv_Reduced;
                        HTuple iValue, jValue;

                        iValue = !tModel.IsIPercent ? (HTuple)tModel.IValue : hv_Length * ((double)tModel.IValue / 100);
                        jValue = !tModel.IsJPercent ? (HTuple)tModel.JValue : hv_Length * ((double)tModel.JValue / 100);

                        int iNum = (int)iValue.D;
                        int jNum = (int)jValue.D;
                        HOperatorSet.TupleSelectRange(hv_Sorted, 0, hv_Length - iNum - 1, out hv_Reduced);
                        HOperatorSet.TupleSelectRange(hv_Reduced, hv_Length - iNum - jNum, hv_Length - iNum - 1, out hv_SelectValue);
                    }
                    else if (tModel.FilterType == "移除最小i个点，选取剩余最小j个点")
                    {
                        HTuple hv_Reduced;
                        HTuple iValue, jValue;

                        iValue = !tModel.IsIPercent ? (HTuple)tModel.IValue : hv_Length * ((double)tModel.IValue / 100);
                        jValue = !tModel.IsJPercent ? (HTuple)tModel.JValue : hv_Length * ((double)tModel.JValue / 100);

                        int iNum = (int)iValue.D;
                        int jNum = (int)jValue.D;
                        HOperatorSet.TupleSelectRange(hv_Sorted, iNum, hv_Length - 1, out hv_Reduced);
                        HOperatorSet.TupleSelectRange(hv_Reduced, 0, jNum, out hv_SelectValue);
                    }
                    else if (tModel.FilterType == "移除最大i个点，移除最小j个点")
                    {
                        HTuple iValue, jValue;

                        iValue = !tModel.IsIPercent ? (HTuple)tModel.IValue : hv_Length * ((double)tModel.IValue / 100);
                        jValue = !tModel.IsJPercent ? (HTuple)tModel.JValue : hv_Length * ((double)tModel.JValue / 100);

                        int iNum = (int)iValue.D;
                        int jNum = (int)jValue.D;
                        HOperatorSet.TupleSelectRange(hv_Sorted, iNum, hv_Length - jNum - 1, out hv_SelectValue);
                    }

                    if (tModel.ModeType == "最大值")
                    {
                        HOperatorSet.TupleMax(hv_SelectValue, out hv_Value);
                    }
                    else if (tModel.ModeType == "最小值")
                    {
                        HOperatorSet.TupleMin(hv_SelectValue, out hv_Value);
                    }
                    else if (tModel.ModeType == "平均值")
                    {
                        HOperatorSet.TupleMean(hv_SelectValue, out hv_Value);
                    }

                    //添加补偿量
                    switch (tModel.offSetType)
                    {
                        case OffSetType.乘:
                            hv_Value *= tModel.OffSetValue;
                            break;
                        case OffSetType.减:
                            hv_Value -= tModel.OffSetValue;
                            break;
                        case OffSetType.加:
                            hv_Value += tModel.OffSetValue;
                            break;
                        case OffSetType.无:
                            hv_Value += 0;
                            break;
                        case OffSetType.除:
                            hv_Value /= tModel.OffSetValue;
                            break;
                    }

                    listValue.Add( Math.Round(hv_Value.D, 5)); 
                }

                ho_Obj.Dispose();

                return listValue;               
                
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return new List<double>(-9999);
            }
        }

        //一维测量
        public static void OneMeasureFunc(HObject ho_Image, OneMeasureModel tModel, HTuple hv_HomeMat2d, out HTuple hv_Row_Measure, out HTuple hv_Column_Measure, out HTuple hv_Distance)
        {
            try
            {  
                // Local control variables 
                HTuple hv_AmplitudeThreshold = null, hv_RoiWidthLen2 = null;
                HTuple hv_LineRowStart_Measure_01_0 = null, hv_LineColumnStart_Measure_01_0 = null;
                HTuple hv_LineRowEnd_Measure_01_0 = null, hv_LineColumnEnd_Measure_01_0 = null;
                HTuple hv_TmpCtrl_Row = null, hv_TmpCtrl_Column = null;
                HTuple hv_TmpCtrl_Dr = null, hv_TmpCtrl_Dc = null, hv_TmpCtrl_Phi = null;
                HTuple hv_TmpCtrl_Len1 = null, hv_TmpCtrl_Len2 = null;
                HTuple hv_MsrHandle_Measure_01_0 = null, hv_Row_Measure_01_0 = null;
                HTuple hv_Column_Measure_01_0 = null, hv_Amplitude_Measure_01_0 = null;
                HTuple hv_Distance_Measure_01_0 = null;
                // Initialize local and output iconic variables  
                //Measure 01: Code generated by Measure 01
                //Measure 01: Prepare measurement
                hv_AmplitudeThreshold = tModel.Threshold;
                hv_RoiWidthLen2 = tModel.RoiWidth;
                HOperatorSet.SetSystem("int_zooming", "true");
                //Measure 01: Coordinates for line Measure 01 [0]
                hv_LineRowStart_Measure_01_0 = Double.Parse(XmlControl.GetLinkValue(tModel.LineRowStart).ToString());
                hv_LineColumnStart_Measure_01_0 = Double.Parse(XmlControl.GetLinkValue(tModel.LineColumnStart).ToString());
                hv_LineRowEnd_Measure_01_0 = Double.Parse(XmlControl.GetLinkValue(tModel.LineRowEnd).ToString());
                hv_LineColumnEnd_Measure_01_0 = Double.Parse(XmlControl.GetLinkValue(tModel.LineColumnEnd).ToString());

                if (tModel.IsModelForm)
                { 
                    HOperatorSet.AffineTransPoint2d(hv_HomeMat2d, hv_LineRowStart_Measure_01_0, hv_LineColumnStart_Measure_01_0, out hv_LineRowStart_Measure_01_0, out hv_LineColumnStart_Measure_01_0);
                    HOperatorSet.AffineTransPoint2d(hv_HomeMat2d, hv_LineRowEnd_Measure_01_0, hv_LineColumnEnd_Measure_01_0, out hv_LineRowEnd_Measure_01_0, out hv_LineColumnEnd_Measure_01_0);
                }

                //Measure 01: Convert coordinates to rectangle2 type
                hv_TmpCtrl_Row = 0.5 * (hv_LineRowStart_Measure_01_0 + hv_LineRowEnd_Measure_01_0);
                hv_TmpCtrl_Column = 0.5 * (hv_LineColumnStart_Measure_01_0 + hv_LineColumnEnd_Measure_01_0);
                hv_TmpCtrl_Dr = hv_LineRowStart_Measure_01_0 - hv_LineRowEnd_Measure_01_0;
                hv_TmpCtrl_Dc = hv_LineColumnEnd_Measure_01_0 - hv_LineColumnStart_Measure_01_0;
                hv_TmpCtrl_Phi = hv_TmpCtrl_Dr.TupleAtan2(hv_TmpCtrl_Dc);
                hv_TmpCtrl_Len1 = 0.5 * ((((hv_TmpCtrl_Dr * hv_TmpCtrl_Dr) + (hv_TmpCtrl_Dc * hv_TmpCtrl_Dc))).TupleSqrt()
                    );
                hv_TmpCtrl_Len2 = hv_RoiWidthLen2.Clone();
                //Measure 01: Create measure for line Measure 01 [0]
                HTuple hv_Width, hv_Height;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //Measure 01: Attention: This assumes all images have the same size!
                HOperatorSet.GenMeasureRectangle2(hv_TmpCtrl_Row, hv_TmpCtrl_Column, hv_TmpCtrl_Phi,
                    hv_TmpCtrl_Len1, hv_TmpCtrl_Len2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure_01_0);
                //Measure 01: ***************************************************************
                //Measure 01: * The code which follows is to be executed once / measurement *
                //Measure 01: ***************************************************************
                //Measure 01: The image is assumed to be made available in the
                //Measure 01: variable last displayed in the graphics window 

                //Measure 01: Execute measureyments
                HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure_01_0, 1, hv_AmplitudeThreshold,
                    tModel.Transition, tModel.Select, out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0,
                    out hv_Distance_Measure_01_0);
                //Measure 01: Do something with the results
                //Measure 01: Clear measure when done
                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure_01_0);


                hv_Row_Measure = hv_Row_Measure_01_0;
                hv_Column_Measure = hv_Column_Measure_01_0;
                hv_Distance = hv_Distance_Measure_01_0; 
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                hv_Row_Measure = null;
                hv_Column_Measure = null;
                hv_Distance = null;
            }
            
        }

        //平面度检测
        public static List<double> FlatnessFunc_New(FlatnessModel tModel, HSmartWindow hSmartWindow, HObject ho_Image, HObject ho_CheckRegion)
        {
            try
            {
                // Local iconic variables  
                HObject ho_Region;
                HObject ho_ConnectedRegions, ho_ObjectSelected = null;
                HObject ho_ContoursAffinTrans;
                HObject ho_Z1;

                // Local control variables 
                 
                HTuple hv_Number = null, hv_Error = null;
                HTuple hv_Value = new HTuple();
                HTuple hv_Max = null, hv_Min = null, hv_OffSet = null;
                // Initialize local and output iconic variables  
                HOperatorSet.GenEmptyObj(out ho_Z1);
                HOperatorSet.GenEmptyObj(out ho_Region); 
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
                HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
 

                ho_Z1.Dispose();
                GenFitting(ho_Image, ho_CheckRegion, out ho_Z1, hSmartWindow.GetWindowHandle());

                HTuple hv_Row, hv_Column, hv_Radius;
                HOperatorSet.SmallestCircle(ho_CheckRegion, out hv_Row, out hv_Column, out hv_Radius);

                //检测平面度
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_CheckRegion, out ho_ConnectedRegions);
                HOperatorSet.CountObj(ho_ConnectedRegions, out hv_Number);
                hv_Error = 0; 

                HTuple hv_CalValue = "max";
                HTuple hv_CheckValue = new HTuple();

                //如果是画一个面来测量
                if (tModel.IsOneRegion)
                {
                    HObject ho_Rectangle;
                    HObject ho_outObj, ho_Rectangle1 = null;
                    // Local control variables

                    HTuple hv_WindowHandle = new HTuple(), hv_Row1 = null;
                    HTuple hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                    HTuple hv_X = null, hv_Y = null, hv_RowLength = null;
                    HTuple hv_ColLength = null, hv_Index = null, hv_Index1 = new HTuple();
                    // Initialize local and output iconic variables  
                    HOperatorSet.GenEmptyObj(out ho_Rectangle);
                    HOperatorSet.GenEmptyObj(out ho_outObj);
                    HOperatorSet.GenEmptyObj(out ho_Rectangle1);

                    HOperatorSet.SmallestRectangle1(ho_ConnectedRegions, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);
                    hv_X = tModel.XScale;
                    hv_Y = tModel.YScale;
                    hv_RowLength = (hv_Row2 - hv_Row1) / hv_X;
                    hv_ColLength = (hv_Column2 - hv_Column1) / hv_Y;

                    ho_outObj.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_outObj);
                    HTuple end_val13 = hv_X;
                    HTuple step_val13 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val13, step_val13); hv_Index = hv_Index.TupleAdd(step_val13))
                    {
                        HTuple end_val14 = hv_Y;
                        HTuple step_val14 = 1;
                        for (hv_Index1 = 1; hv_Index1.Continue(end_val14, step_val14); hv_Index1 = hv_Index1.TupleAdd(step_val14))
                        {
                            ho_Rectangle1.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle1, hv_Row1 + ((hv_Index - 1) * hv_RowLength),
                                hv_Column1 + ((hv_Index1 - 1) * hv_ColLength), hv_Row1 + (hv_Index * hv_RowLength),
                                hv_Column1 + (hv_Index1 * hv_ColLength));
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_outObj, ho_Rectangle1, out ExpTmpOutVar_0);
                                ho_outObj.Dispose();
                                ho_outObj = ExpTmpOutVar_0;
                            }
                        }
                    }

                    HOperatorSet.GrayFeatures(ho_outObj, ho_Z1, hv_CalValue, out hv_CheckValue);

                    ho_Rectangle.Dispose();
                    ho_outObj.Dispose();
                    ho_Rectangle1.Dispose();
                }
                else
                {
                    HOperatorSet.GrayFeatures(ho_ConnectedRegions, ho_Z1, hv_CalValue, out hv_CheckValue);
                }

                HOperatorSet.TupleMax(hv_CheckValue, out hv_Max);
                HOperatorSet.TupleMin(hv_CheckValue, out hv_Min);
                hv_OffSet = hv_Max - hv_Min;

                List<double> listResult = new List<double>()
                {
                    Math.Round(hv_Min.D, 4),
                    Math.Round(hv_Max.D, 4),
                    Math.Round(hv_OffSet.D, 4)
                };


               hSmartWindow.GetWindowHandle().SetLineWidth(2);
                hSmartWindow.GetWindowHandle().SetColor("red");
                hSmartWindow.DispObj(ho_ContoursAffinTrans);

                hSmartWindow.GetWindowHandle().SetLineWidth(2);
                hSmartWindow.GetWindowHandle().SetColor("orange");
                hSmartWindow.GetWindowHandle().SetDraw("fill");
                hSmartWindow.DispObj(ho_CheckRegion);

                HSmartWindow.disp_message(hSmartWindow.GetWindowHandle(), hv_OffSet.D.ToString("0.000"), "window", hv_Row, hv_Column + hv_Radius + 10, "lime green", "false");


                ho_Region.Dispose();
                // ho_RegionAffineTrans.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Z1.Dispose();

                return listResult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return new List<double>() { -99999, -99999, -99999 };
            }
        }

        public static void GenFitting(HObject ho_ImageZoom, HObject ho_ROI_0, out HObject ho_Z1,
               HTuple hv_WindowHandle)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced2, ho_ImageMedian, ho_X1;
            HObject ho_Y1;

            // Local copy input parameter variables 
            HObject ho_ROI_0_COPY_INP_TMP;
            ho_ROI_0_COPY_INP_TMP = ho_ROI_0.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_XResolution = null, hv_YResolution = null;
            HTuple hv_ZResolution = null, hv_StandPlane3Dobject = null;
            HTuple hv_Realpart3Dobject = null, hv_ParFitting = null;
            HTuple hv_ValFitting = null, hv_FitObject = null, hv_PoseA = null;
            HTuple hv_Xangle = null, hv_Pose1 = new HTuple(), hv_PoseCompose = null;
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
                HOperatorSet.MedianImage(ho_ImageReduced2, out ho_ImageMedian, "circle", 1,
                    "mirrored");
                //设置图像的XY比例并生成3D模型
                //hv_XResolution = 0.014;
                //hv_YResolution = 0.014;
                //hv_ZResolution = 0.0012;

                hv_XResolution = Global.XYResolution;
                hv_YResolution = Global.XYResolution;
                hv_ZResolution = Global.ZResolution;

                //3D基准面
                convertZmapImageTo3DObject(ho_ImageMedian, hv_XResolution, hv_YResolution,
                    hv_ZResolution, out hv_StandPlane3Dobject);
                //3D产品
                convertZmapImageTo3DObject(ho_ImageZoom, hv_XResolution, hv_YResolution, hv_ZResolution,
                    out hv_Realpart3Dobject);
                //
                //dev_open_window (0, 0, 512, 512, 'black', WindowHandle1)
                //visualize_object_model_3d (WindowHandle1, Realpart3Dobject, [], [], [], [], [], [], [], PoseOut)
                //
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
                if ((int)(new HTuple(hv_Xangle.TupleGreater(300))) != 0)
                {
                    HOperatorSet.CreatePose(0, 0, 0, 0, 0, 0, "Rp+T", "gba", "point", out hv_Pose1);
                }
                else
                {
                    HOperatorSet.CreatePose(0, 0, 0, 180, 0, 0, "Rp+T", "gba", "point", out hv_Pose1);
                }
                //将180改变为0 lhf 2019-4-27
                //create_pose (0, 0, 0, 180, 0, 0, 'Rp+T', 'gba', 'point', Pose1)
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




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

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
                HOperatorSet.Threshold(ho_ZMapImage, out ho_Region1, 2000, 65535);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ZMapImage, ho_Region1, out ho_ImageReduced);
                ho_ImageMeasureReal.Dispose();
                HOperatorSet.ConvertImageType(ho_ImageReduced, out ho_ImageMeasureReal, "real");
                HOperatorSet.GetImageSize(ho_ImageMeasureReal, out hv_zMap_Width, out hv_zMap_Height);
                ho_PointCloudZ.Dispose();
                HOperatorSet.GenImageConst(out ho_PointCloudZ, "real", hv_zMap_Width, hv_zMap_Height);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageMeasureReal, out ho_Region, 2000, 9999999);
                HOperatorSet.GetRegionPoints(ho_Region, out hv_zMap_Height, out hv_zMap_Width);
                HOperatorSet.GetGrayval(ho_ImageMeasureReal, hv_zMap_Height, hv_zMap_Width,
                    out hv_Grayval);
                HOperatorSet.TuplePow(2, 15, out hv_hv_offset);
                hv_hvZ = (hv_Grayval - hv_hv_offset) * hv_ZResolution;
                HOperatorSet.SetGrayval(ho_PointCloudZ, hv_zMap_Height, hv_zMap_Width, hv_hvZ);
                ho_Union.Dispose();
                HOperatorSet.Threshold(ho_PointCloudZ, out ho_Union, (new HTuple(-35)).TupleConcat(
                    0.00001), (new HTuple(-0.00001)).TupleConcat(35));
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



        // 最小二乘法拟合圆获取圆心坐标、圆半径
        public static int FitCircle(List<PointF> pointFs, out float CenterX, out float CenterY, out float CenterR)
        {
            CenterX = 0;
            CenterY = 0;
            CenterR = 0;

            try
            {
                Matrix<float> YMat;
                Matrix<float> RMat;
                Matrix<float> AMat;
                List<float> YLit = new List<float>();
                List<float[]> RLit = new List<float[]>();

                foreach (var pointF in pointFs)
                    YLit.Add(pointF.X * pointF.X + pointF.Y * pointF.Y);

                float[,] Yarray = new float[YLit.Count, 1];
                for (int i = 0; i < YLit.Count; i++)
                    Yarray[i, 0] = YLit[i];

                YMat = CreateMatrix.DenseOfArray<float>(Yarray);

                foreach (var pointF in pointFs)
                    RLit.Add(new float[] { -pointF.X, -pointF.Y, -1 });

                float[,] Rarray = new float[RLit.Count, 3];
                for (int i = 0; i < RLit.Count; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Rarray[i, j] = RLit[i][j];
                    }
                }

                RMat = CreateMatrix.DenseOfArray<float>(Rarray);
                Matrix<float> RTMat = RMat.Transpose();
                Matrix<float> RRTInvMat = (RTMat.Multiply(RMat)).Inverse();
                AMat = RRTInvMat.Multiply(RTMat.Multiply(YMat));

                float[,] Array = AMat.ToArray();
                float A = Array[0, 0];
                float B = Array[1, 0];
                float C = Array[2, 0];
                CenterX = A / -2.0f;
                CenterY = B / -2.0f;
                CenterR = (float)(Math.Sqrt(A * A + B * B - 4 * C) / 2.0f);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
            return 0x0000;
        }

        //复制文件夹文件
        public static void CopyFiles(string varFromDirectory, string varToDirectory)
        {
            try
            {
                Directory.CreateDirectory(varToDirectory);

                if (!Directory.Exists(varFromDirectory)) return;

                string[] directories = Directory.GetDirectories(varFromDirectory);

                if (directories.Length > 0)
                {
                    foreach (string d in directories)
                    {
                        CopyFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                    }
                }

                string[] files = Directory.GetFiles(varFromDirectory);

                if (files.Length > 0)
                {
                    foreach (string s in files)
                    {
                        File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                    }
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
        
        //创建二维码模型
        public static void CreateDataMatrixECC200(HTuple hv_CodeSymbolType, out HTuple hv_DataCodeHandleHigh)
        {
            // Initialize local and output iconic variables 
            //创建读码类型
            //standard_recognition  标准模式
            //enhanced_recognition  加强模式
            //maximum_recognition   最强模式

            HOperatorSet.CreateDataCode2dModel(hv_CodeSymbolType, "default_parameters", "maximum_recognition",
                out hv_DataCodeHandleHigh);
            //**set_data_code_2d_param算子的参数解析
            //set_data_code_2d_param (DataCodeHandleHigh, 'contrast_tolerance', 'high')
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "timeout", 1000);
            //**对比容差
            //set_data_code_2d_param (DataCodeHandleHigh, 'contrast_tolerance', 'any')
            //**偏转
            //set_data_code_2d_param (DataCodeHandleHigh, 'slant_max', 0.25)
            //set_data_code_2d_param (DataCodeHandleHigh, 'finder_pattern_tolerance', 'any')
            //set_data_code_2d_param (DataCodeHandleHigh, 'module_grid', 'any')
            //码粒个数设置
            //set_data_code_2d_param (DataCodeHandleHigh, 'symbol_size_min', 12)
            //set_data_code_2d_param (DataCodeHandleHigh, 'symbol_size_max', 20)
            //码粒像素设置
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_size_min", 6);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_size_max", 40);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "small_modules_robustness",
                "high");
            //码粒间距
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_gap_min", "no");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_gap_max", "big");
            //鲁棒性
            //set_data_code_2d_param (DataCodeHandleHigh, 'finder_pattern_tolerance', 'any')
            //set_data_code_2d_param (DataCodeHandleHigh, 'contrast_tolerance', 'any')
            //set_data_code_2d_param (DataCodeHandleHigh, 'module_grid', 'any')

            return;
        }
        
        //查找二维码
        public static void FindDataMatrixECC200(HObject ho_Image, HObject ho_Rectangle, out HObject ho_SymbolXLDs,
            HTuple hv_DataCodeHandleHigh, out HTuple hv_DataMatrixECC200String, out HTuple hv_Error)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_SymbolXLDsFound = null, ho_ImageReduced = null;
            HObject ho_Domain = null, ho_ImagePart = null, ho_ImageZoomed = null;
            HObject ho_Region = null, ho_Region1 = null, ho_RegionAffineTrans = null;

            // Local control variables 

            HTuple hv_NumberStand = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Index = new HTuple();
            HTuple hv_ResultHandles1 = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDsFound);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageZoomed);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            hv_DataMatrixECC200String = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    hv_DataMatrixECC200String = new HTuple();
                    ho_SymbolXLDs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
                    ho_SymbolXLDsFound.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SymbolXLDsFound);
                    hv_NumberStand = 0;
                    //输出条码
                    //set_data_code_2d_param (DataCodeHandleStand, 'default_parameters', 'enhanced_recognition')
                    //set_data_code_2d_param (DataCodeHandleStand, 'default_parameters', 'maximum_recognition')
                    //set_data_code_2d_param (DataCodeHandleStand, 'persistence', 1)
                    //set_data_code_2d_param (DataCodeHandleStand, 'timeout', 200)
                    
                    //************2020.5.10
                    //gen_rectangle1 (Rectangle, 226, 510, 690, 1153)
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                    ho_Domain.Dispose();
                    HOperatorSet.GetDomain(ho_ImageReduced, out ho_Domain);
                    HOperatorSet.SmallestRectangle1(ho_Domain, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);

                    ho_ImagePart.Dispose();
                    HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                    //20200722 将 5  ——————改变为3 提速
                    //for (hv_Index = 0.5; (double)hv_Index < 3; hv_Index = (double)hv_Index + 0.5)
                    for (hv_Index = 0.5; (double)hv_Index < 3; hv_Index = (double)hv_Index + 0.5)
                    {
                        hv_DataMatrixECC200String = new HTuple();
                        ho_ImageZoomed.Dispose();
                        HOperatorSet.ZoomImageFactor(ho_ImagePart, out ho_ImageZoomed, hv_Index,
                            hv_Index, "constant");

                        //set_data_code_2d_param (DataCodeHandleStand, 'polarity', 'any')
                        //set_data_code_2d_param (DataCodeHandleStand, 'slant_max', 0.5)
                        ho_SymbolXLDsFound.Dispose();
                        //HOperatorSet.FindDataCode2d(ho_ImageZoomed, out ho_SymbolXLDsFound, hv_DataCodeHandleHigh,
                        //    "train", "all", out hv_ResultHandles1, out hv_DataMatrixECC200String);
                        HOperatorSet.FindDataCode2d(ho_ImageZoomed, out ho_SymbolXLDsFound, hv_DataCodeHandleHigh,
                            new HTuple(), new HTuple(), out hv_ResultHandles1, out hv_DataMatrixECC200String);

                        //find_data_code_2d (ImageZoomed, SymbolXLDsFound, DataCodeHandleStand, [], [], ResultHandles, DataMatrixECC200String)
                        HOperatorSet.CountObj(ho_SymbolXLDsFound, out hv_NumberStand);
                        if ((int)(new HTuple(hv_NumberStand.TupleEqual(0))) != 0)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_SymbolXLDsFound, out ho_Region, "margin");
                    ho_Region1.Dispose();
                    HOperatorSet.ZoomRegion(ho_Region, out ho_Region1, 1.0 / hv_Index, 1.0 / hv_Index);

                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row1, hv_Column1,
                        out hv_HomMat2DTranslate);
                    ho_RegionAffineTrans.Dispose();
                    HOperatorSet.AffineTransRegion(ho_Region1, out ho_RegionAffineTrans, hv_HomMat2DTranslate,
                        "nearest_neighbor");
                    ho_SymbolXLDsFound.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_RegionAffineTrans, out ho_SymbolXLDsFound,
                        "border");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_SymbolXLDsFound, ho_SymbolXLDs, out ExpTmpOutVar_0
                            );
                        ho_SymbolXLDs.Dispose();
                        ho_SymbolXLDs = ExpTmpOutVar_0;
                    }

                    if ((int)(new HTuple(hv_NumberStand.TupleEqual(0))) != 0)
                    {
                        hv_Error = 1;
                    }
                    else
                    {
                        hv_Error = 0;
                    }


                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_SymbolXLDsFound.Dispose();
                ho_ImageReduced.Dispose();
                ho_Domain.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageZoomed.Dispose();
                ho_Region.Dispose();
                ho_Region1.Dispose();
                ho_RegionAffineTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_SymbolXLDsFound.Dispose();
                ho_ImageReduced.Dispose();
                ho_Domain.Dispose();
                ho_ImagePart.Dispose();
                ho_ImageZoomed.Dispose();
                ho_Region.Dispose();
                ho_Region1.Dispose();
                ho_RegionAffineTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //字符训练算法
        public static void Ocr_Train(OcrTrainModel tModel, HSmartWindow hSmartWindow)
        {
            // Local iconic variables 

            HObject ho_SortedRegions, ho_CharaterRegions = null;
            // Local control variables 

            HTuple hv_FontName = null, hv_TrainingFileName = null;
            HTuple hv_TrainingNames = null, hv_i = null, hv_CharNames = null;
            HTuple hv_OCRHandle = null, hv_Error = null, hv_ErrorLog = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_CharaterRegions);

            HObject ho_Image = (HObject)XmlControl.GetLinkValue(tModel.ImageForm);
            ho_SortedRegions = (HObject)XmlControl.GetLinkValue(tModel.RegionForm);
            string str = XmlControl.GetLinkValue(tModel.CharValue).ToString();
            string[] strTransNames = str.Split(',');

            hv_FontName = Global.Model3DPath + "//" + tModel.Name;
            hv_TrainingFileName = hv_FontName + ".trf";

            hv_TrainingNames = strTransNames;

            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_TrainingNames.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {
                ho_CharaterRegions.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_CharaterRegions, hv_i + 1);
                HOperatorSet.AppendOcrTrainf(ho_CharaterRegions, ho_Image, hv_TrainingNames.TupleSelect(
                    hv_i), hv_TrainingFileName);
                HOperatorSet.WriteString(hSmartWindow.GetWindowHandle(), hv_TrainingNames.TupleSelect(hv_i));
            }
            hv_CharNames = ((hv_TrainingNames.TupleSort())).TupleUniq();
            HOperatorSet.CreateOcrClassMlp(8, 10, "constant", "default", hv_CharNames, 5,
                "none", 10, 42, out hv_OCRHandle);
            HOperatorSet.TrainfOcrClassMlp(hv_OCRHandle, hv_TrainingFileName, 200, 1, 0.01,
                out hv_Error, out hv_ErrorLog);
            HOperatorSet.WriteOcrClassMlp(hv_OCRHandle, hv_FontName);
            HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);

            ho_CharaterRegions.Dispose();
        }

        //连接螺批
        public static bool InitScrew(ScrewInitModel tModel)
        {
            try
            {
                if(m_MTFCommands == null)
                {
                    m_MTFCommands = new MTFCommand[10];
                }

                TCPIPModel tcpModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.Name == tModel.MsgName);
                if (m_MTFCommands[tcpModel.Id] == null)
                {
                    m_MTFCommands[tcpModel.Id] = new MTFCommand(tModel.MsgName, tModel.OffSetNum, 0);
                }

                m_MTFCommands[tcpModel.Id].Close();
                bool bresult = m_MTFCommands[tcpModel.Id].Init();
                if (!bresult)
                {
                    m_DelOutPutLog(tModel.Name + "初始化失败");
                    return false;
                }

                bresult = m_MTFCommands[tcpModel.Id].CreateConnect();
                if (!bresult)
                {
                    m_DelOutPutLog(tModel.Name + "创建连接失败");
                    return false;
                }

                m_MTFCommands[tcpModel.Id].StopSubscribe();
                bresult = m_MTFCommands[tcpModel.Id].StartSubscribe();
                if (!bresult)
                {
                    m_DelOutPutLog(tModel.Name + "订阅失败");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        //读螺丝批数据
        public static bool ReadScrew(ScrewReadModel tModel, int screwIndex)
        {
            try
            {
                TCPIPModel tcpModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.Name == tModel.MsgName);
                TighteningResult tighteningResult = new TighteningResult();
                bool bresult = m_MTFCommands[tcpModel.Id].RecvData(ref tighteningResult);

                //获取拍照结果
                bool bsnapResult = bool.Parse(XmlControl.GetLinkValue(tModel.SnapResult).ToString());
                tighteningResult.strSnapResult = bsnapResult ? "OK" : "NG";

                m_MTFCommands[tcpModel.Id].m_DicResult[screwIndex] = tighteningResult; 

                if(bresult)
                {
                    tModel.itemResult.MtfResult = tighteningResult.strMtfResult;
                    tModel.itemResult.ErrorInfo = tighteningResult.strMtfErrorInfo;
                    tModel.itemResult.PeakTorque = Double.Parse(tighteningResult.strPeakTorque);
                    tModel.itemResult.TotalAngle = Double.Parse(tighteningResult.strTotalAngle);
                    if(!string.IsNullOrEmpty(tighteningResult.strStepActualTorque1))
                    {
                        tModel.itemResult.StepActualTorque1 = Double.Parse(tighteningResult.strStepActualTorque1);
                    }
                    if (!string.IsNullOrEmpty(tighteningResult.strStepActualAngle1))
                    {
                        tModel.itemResult.StepActualAngle1 = Double.Parse(tighteningResult.strStepActualAngle1);
                    }
                    if (!string.IsNullOrEmpty(tighteningResult.strStepActualTorque2))
                    {
                        tModel.itemResult.StepActualTorque2 = Double.Parse(tighteningResult.strStepActualTorque2);
                    }
                    if (!string.IsNullOrEmpty(tighteningResult.strStepActualAngle2))
                    {
                        tModel.itemResult.StepActualAngle2 = Double.Parse(tighteningResult.strStepActualAngle2);
                    }
                    if (!string.IsNullOrEmpty(tighteningResult.strStepActualTorque3))
                    {
                        tModel.itemResult.StepActualTorque3 = Double.Parse(tighteningResult.strStepActualTorque3);
                    }
                    if (!string.IsNullOrEmpty(tighteningResult.strStepActualAngle3))
                    {
                        tModel.itemResult.StepActualAngle3 = Double.Parse(tighteningResult.strStepActualAngle3);
                    }
                }
                else
                {
                    tModel.itemResult.PeakTorque = 0;
                    tModel.itemResult.TotalAngle = 0;
                    tModel.itemResult.StepActualTorque1 = 0;
                    tModel.itemResult.StepActualAngle1 = 0;
                    tModel.itemResult.StepActualTorque2 = 0;
                    tModel.itemResult.StepActualAngle2 = 0;
                    tModel.itemResult.StepActualTorque3 = 0;
                    tModel.itemResult.StepActualAngle3 = 0; 
                }

                tModel.itemResult.SetSpeed1 = tModel.SetSpeed1;
                tModel.itemResult.SetAngle1 = tModel.SetAngle1;
                tModel.itemResult.SetTorque1 = tModel.SetTorque1;
                tModel.itemResult.SetSpeed2 = tModel.SetSpeed2;
                tModel.itemResult.SetAngle2 = tModel.SetAngle2;
                tModel.itemResult.SetTorque2 = tModel.SetTorque2;
                tModel.itemResult.SetSpeed3 = tModel.SetSpeed3;
                tModel.itemResult.SetAngle3 = tModel.SetAngle3;
                tModel.itemResult.SetTorque3 = tModel.SetTorque3; 

                string strLog = string.Format("结果: {0} 扭矩: {1} 总角度: {2} 一阶段扭矩: {3} 角度: {4} 二阶段扭矩: {5} 角度: {6} 三阶段扭矩: {7} 角度: {8} 错误信息:{9}",
                    tModel.itemResult.MtfResult, tModel.itemResult.PeakTorque, tModel.itemResult.TotalAngle,
                    tModel.itemResult.StepActualTorque1, tModel.itemResult.StepActualAngle1, tModel.itemResult.StepActualTorque2,
                    tModel.itemResult.StepActualAngle2, tModel.itemResult.StepActualTorque3, tModel.itemResult.StepActualAngle3,
                    tModel.itemResult.ErrorInfo);
                m_DelOutPutLog(strLog, LogLevel.Debug);

                return bresult;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        //上传富士康SFC
        public static bool UploadToSFC(UploadSFCModel tModel)
        {
            try
            {
                bool bResult = true;

                SFCModel sfcModel = XmlControl.sequenceModelNew.SfcModel;
                //SFC通信
                string errorMsg = "";
                LoginParam loginParam = new LoginParam()
                {
                    userName = sfcModel.UserName,
                    passWord = sfcModel.PassWord,
                    site = sfcModel.Site,
                    bu = sfcModel.Bu,
                    lang = sfcModel.Lang,
                    stationID = sfcModel.StationID,
                };

                tModel.itemResult.ResultStr = "";
                if (!m_SFCMethod.GetUserToken(loginParam, out errorMsg))
                {
                    m_DelOutPutLog("SFC通信初始化失败: " + errorMsg);
                    tModel.itemResult.ResultStr = "用户验证失败:" + errorMsg;
                    return false;
                }

                string pid = XmlControl.GetLinkValue(tModel.Pid).ToString();
                if (tModel.IsOnlyConfig)
                {
                    string dsn = "";
                    bool result = m_SFCMethod.GetAccDsnData(pid, sfcModel.AccType, out dsn, out errorMsg);

                    tModel.itemResult.DSN = result ? dsn : "";
                    m_DelOutPutLog("Acc:" + pid, LogLevel.Debug);
                    m_DelOutPutLog("SN:" + dsn, LogLevel.Debug);

                    if(!result)
                    { 
                        m_DelOutPutLog("获取DSN失败:" + errorMsg, LogLevel.Error);
                        tModel.itemResult.ResultStr = "获取DSN失败:" + errorMsg;
                        return false;
                    }

                    bool bNext = m_SFCMethod.CheckRoute(dsn, out errorMsg);
                    if (!bNext)
                    {
                        string strErr = string.Format("CheckRoute Error:Pid-{0} {1}", dsn, errorMsg);
                        m_DelOutPutLog(strErr, LogLevel.Error);
                        tModel.itemResult.ResultStr = strErr;
                        return false;
                    }
                }
                else
                {
                    string fixture = XmlControl.GetLinkValue(tModel.Fixture).ToString();
                    bool bNext = m_SFCMethod.CheckRoute(pid, out errorMsg);
                    if (!bNext)
                    {
                        string strErr = string.Format("CheckRoute Error:Pid-{0} {1}", pid, errorMsg);
                        m_DelOutPutLog(strErr, LogLevel.Error);
                        tModel.itemResult.ResultStr = strErr;
                        return false;
                    }

                    bNext = m_SFCMethod.GetNextJobID(out errorMsg);
                    if (!bNext)
                    {
                        m_DelOutPutLog("GetNextJobID Error:" + errorMsg, LogLevel.Error);
                        tModel.itemResult.ResultStr = "GetNextJobID Error:" + errorMsg;
                        return false;
                    }
                     
                    if (tModel.IsUploadOk)
                    {
                        bResult = true;
                    }
                    m_DelOutPutLog(string.Format("{0}上传{1}", pid, bResult ? "OK" : "NG"), LogLevel.Debug);
                    bNext = m_SFCMethod.ForwardRoute(pid, bResult ? "P" : "F", "", fixture, out errorMsg);
                    if(!bNext)
                    {
                        m_DelOutPutLog("ForwardRoute Error:" + errorMsg, LogLevel.Error);
                        tModel.itemResult.ResultStr = "ForwardRoute Error:" + errorMsg;
                        return false;
                    }
                    else
                    {
                        m_DelOutPutLog(string.Format("Pid:{0} 上传成功", pid), LogLevel.Debug);
                    }

                    //上传SFC测试Log
                    string logInfo = GetTestLog(bResult, pid, fixture, tModel.Index);
                    bNext = m_SFCMethod.UploadTestLogFile(logInfo, out errorMsg);
                    if (!bNext)
                    {
                        m_DelOutPutLog("UploadTestLogFile Error:" + errorMsg);
                        tModel.itemResult.ResultStr = "UploadTestLogFile Error:" + errorMsg;
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        public static bool InitSFC()
        {
            try
            {
                SFCModel sfcModel = XmlControl.sequenceModelNew.SfcModel;

                if(string.IsNullOrEmpty(sfcModel.UserName))
                {
                    return true;
                }
                //SFC通信
                string errorMsg = "";
                LoginParam loginParam = new LoginParam()
                {
                    userName = sfcModel.UserName,
                    passWord = sfcModel.PassWord,
                    site = sfcModel.Site,
                    bu = sfcModel.Bu,
                    lang = sfcModel.Lang,
                    stationID = sfcModel.StationID,
                };
                 
                if (!m_SFCMethod.GetUserToken(loginParam, out errorMsg))
                {
                    m_DelOutPutLog("SFC通信初始化失败: " + errorMsg); 
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
         
        }
        
        private static string GetTestLog(bool bResult, string pid, string groupName, int index)
        {
            string jobId = m_SFCMethod.m_JobID; 

            string result = bResult ? "P" : "F";
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string token = m_SFCMethod.m_Token;
            string fileData = WriteToSFCFile(bResult, index);

            //转成 Base64 形式的 System.String
            byte[] b = Encoding.Default.GetBytes(fileData);
            string strBase64 = Convert.ToBase64String(b);

            string str = "\"FileName\"" + ":" + pid + "^" + jobId + "^" + groupName + "^" + result + "^" + timeStamp + ".rec"
                        + "," + "Token" + ":" + token + "," + "FileData" + ":" + strBase64;

            str = "{" + string.Format("\"{0}\":\"{1}^{2}^{3}^{4}^{5}.rec\",\"{6}\":\"{7}\",\"{8}\":\"{9}\"", "FileName",
                pid, jobId, groupName, result, timeStamp, "Token", token, "FileData", strBase64) + "}";
            // {“FileName”:”Pid ^ jobID ^ GroupName ^ Result(P || F) ^ TimeStamp(y
            // yyyMMddHHmiss).rec”,”Token”:”Token”,”FileData”:”将数据轉
            // 換成 Base64 編碼字符串”} 

            return str;
        }
        
        public static string WriteToSFCFile(bool bResult, int index)
        {
            try
            {
                Dictionary<int, TighteningResult> stDicTigResult = new Dictionary<int, TighteningResult>();
                stDicTigResult = m_MTFCommands[index].m_DicResult;

                List<ResultInfo> listStr = new List<ResultInfo>();
                for (int i = 1; i <= 9; i++)
                {
                    if (!stDicTigResult.ContainsKey(i))
                    {
                        continue;
                    }

                    ResultInfo info = new ResultInfo()
                    {
                        Name = "StepTorque1_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualTorque1,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetTorque1,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "StepAngle1_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualAngle1,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetAngle1,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "StepTorque2_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualTorque2,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetTorque2,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "StepAngle2_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualAngle2,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetAngle2,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "StepTorque3_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualTorque3,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetTorque3,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "StepAngle3_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strStepActualAngle3,
                        MinValue = "0",
                        MaxValue = stDicTigResult[i].strStepSetAngle3,
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "PeakTorque_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strPeakTorque,
                        MinValue = "0",
                        MaxValue = "1.2",
                        PassFail = "PASS"
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "Snap_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strSnapResult,
                        MinValue = "-",
                        MaxValue = "-",
                        PassFail = stDicTigResult[i].strSnapResult == "OK" ? "PASS" : "FAIL",
                    };
                    listStr.Add(info);
                    info = new ResultInfo()
                    {
                        Name = "MtfResult_Test_" + i.ToString(),
                        Result = stDicTigResult[i].strMtfResult,
                        MinValue = "-",
                        MaxValue = "-",
                        PassFail = stDicTigResult[i].strMtfResult == "OK" ? "PASS" : "FAIL",
                    };
                    listStr.Add(info);
                }

                string saveStr = "";
                string strItem = "TEST_ITEM_";
                int num = 1;
                foreach (var item in listStr)
                {
                    string str = strItem + num.ToString("00") + "=" + item.Name + "^" + "-" + "^" + item.Result + "^" + item.MaxValue + "^" + item.MinValue +
                        "^" + item.PassFail + "^-" + "\r\n";
                    saveStr += str;
                    num++;
                }

                if (!Directory.Exists("D://SFC"))
                {
                    Directory.CreateDirectory("D://SFC");
                }

                string strPath = "D://SFC//" + "test.log";
                m_SFCMethod.WriteToFile(strPath, saveStr);

                return saveStr;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return "";
            }
        }
        
        //上传美律SAP
        public static bool UploadToSAP(SAPControlModel tModel)
        {
            try
            {
                bool bresult = true;
                SAPControl sapControl = new SAPControl();
                string strQRCode = XmlControl.GetLinkValue(tModel.QRCode).ToString();

                string strStatus, strBatch, strReturn;
                string strJobNum = XmlControl.GetLinkValue(tModel.JobNum).ToString();
                string strTaxAB = XmlControl.GetLinkValue(tModel.TaxAB).ToString();

                sapControl.SAPTest(tModel, strQRCode, strJobNum, strTaxAB, out strStatus, out strBatch, out strReturn);
                 
                tModel.itemResult.Status = strStatus;
                tModel.itemResult.Batch = strBatch;
                tModel.itemResult.Return = strReturn;

                //strBatch = "FIELD MATNR=00230300000B FIELD WERKS=2010 FIELD CHARG=B220B1100P FIELD MAKTX=MEMORY W25Q128JWYIQ WLCSP 21 128Mbit FIELD HSDAT=2020-11-11";
                ////strBatch = "STRUCTURE ZMMT063 { FIELD MATNR = 03C2000000JY FIELD WERKS = 2010 FIELD CHARG = B220C05007 FIELD MAKTX = C C CHIP 4.7μF 10V ±10％ X5R 0603 FIELD HSDAT = 2020-12-05 }";
                //strReturn = "STRUCTURE ZMMT062 { FIELD CODE2D2=BGC5P2Y/CL10A475KP8NNNC/0443/4000 FIELD TYPE1=E FIELD NUMBER1=025 FIELD MESSAGE1=從二維碼資料中的PO+物料號碼無法取得PO項次！ }";
                //strStatus = "Y";

                m_DelOutPutLog("Status:" + strStatus);
                m_DelOutPutLog("Batch:" + strBatch);
                m_DelOutPutLog("Return:" + strReturn);
                 
                int index1 = strBatch.IndexOf("MATNR");
                int index22 = strBatch.IndexOf("WERKS");
                string strPartNum = strBatch.Substring(index1 + 6, index22 - index1 - 13);

                int index11 = strBatch.IndexOf("WERKS");
                string FctName = strBatch.Substring(index11 + 6, 4);

                int startindex = strBatch.IndexOf("MAKTX");
                int endindex = strBatch.LastIndexOf("FIELD");
                string strPartName = strBatch.Substring(startindex + 6, endindex - startindex - 6);

                int index2 = strBatch.IndexOf("CHARG");
                string Batch = strBatch.Substring(index2 + 6, startindex - index2 - 13);

                int index3 = strBatch.IndexOf("HSDAT");
                string ManuDate = strBatch.Substring(index3 + 6, 10);

                int index_1 = strReturn.IndexOf("MESSAGE1");
                string strSapErr = strReturn.Substring(index_1 + 9);

                SAPQueryView.struct_PData pData; 
                pData.BatchNum = Batch;
                pData.DataOfM = ManuDate;
                pData.DataOfTog = DateTime.Now.ToString("yyyy-MM-dd");
                pData.ErrorOfSAP = strSapErr;
                pData.FctName = FctName;
                pData.PartNO = strPartNum;//料号
                pData.PepNO = strJobNum;
                pData.ProductName = strPartName;
                pData.ReMark = strStatus == "Y" ? "OK" : "NG";
                pData.ScanRCode = strQRCode;
                pData.ScanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff");
                pData.TaxNO = strTaxAB;

                SAPQueryView.WriteCSVData(pData);

                return bresult;
            }
            catch (Exception ex)
            {
                tModel.itemResult.Status = null;
                tModel.itemResult.Batch = null;
                tModel.itemResult.Return = null; 
                m_DelOutPutLog("SAP连接失败，请检查网络。\r\n", LogLevel.Error);
                //m_DelOutExLog(ex);
                return false;
            }
        }

        //拆分一张图像成N张
        public static  List<HObject> SplitImage(HObject ho_Image1, int spiltNum = 3)
        { 
            // Local iconic variables  
            HObject[] ho_Image = new HObject[spiltNum];
            HObject ho_Partitioned1;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_j = null;
            HTuple hv_ImageRow = new HTuple(), hv_SequenceCol = new HTuple();
            HTuple hv_SelectedRow = new HTuple(), hv_Grayval = new HTuple();
            // Initialize local and output iconic variables   
            HOperatorSet.GenEmptyObj(out ho_Partitioned1); 
            HOperatorSet.GetImageSize(ho_Image1, out hv_Width, out hv_Height);

            for (int i = 0; i < spiltNum; i++)
            { 
                HOperatorSet.GenImageConst(out ho_Image[i], "byte", hv_Width, hv_Height / spiltNum);
            }

            ho_Partitioned1.Dispose();
            HOperatorSet.PartitionRectangle(ho_Image1, out ho_Partitioned1, hv_Width, 1); 

            HTuple end_val25 = (hv_Height / spiltNum) - 1;
            HTuple step_val25 = 1;
            for (hv_j = 0; hv_j.Continue(end_val25, step_val25); hv_j = hv_j.TupleAdd(step_val25))
            {
                HOperatorSet.TupleGenConst(hv_Width, hv_j, out hv_ImageRow);
                HOperatorSet.TupleGenSequence(0, hv_Width - 1, 1, out hv_SequenceCol);

                for (int i = 0; i < spiltNum; i++)
                {
                    HOperatorSet.TupleGenConst(hv_Width, hv_j * 3 + i, out hv_SelectedRow);
                    HOperatorSet.GetGrayval(ho_Image1, hv_SelectedRow, hv_SequenceCol, out hv_Grayval);
                    HOperatorSet.SetGrayval(ho_Image[i], hv_ImageRow, hv_SequenceCol, hv_Grayval);
                } 
            }

            ho_Partitioned1.Dispose();

            return ho_Image.ToList();
        }
       
        //初始化PLC
        public static void InitPLC()
        {
            try
            {
                var plcModels = XmlControl.sequenceModelNew.PLCSetModels;
                if (plcModels == null || plcModels.Count == 0)
                {
                    return;
                }

                foreach (var item in plcModels)
                {
                    SetPLCType(item);
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        //设置PLC类型
        public static void SetPLCType(PLCSetModel tModel)
        {
            try
            {
                switch (tModel.plcType)
                {
                    case PLCTYPE.ABB:
                        break;
                    //case PLCTYPE.三菱:
                    //    m_PlcEquip[tModel.Id] = new MitsubishiControl.Equip();
                    //    break;
                    //case PLCTYPE.松下:
                    //    if (tModel.ConnType == "串口方式")
                    //    {
                    //        m_PlcEquip[tModel.Id] = new PanasonicControl.COM.Equip();
                    //    }
                    //    else
                    //    {
                    //        m_PlcEquip[tModel.Id] = new PanasonicControl.TCP.Equip(tModel.Id);
                    //    }
                    //    break;
                    //case PLCTYPE.欧姆龙:
                    //    if (tModel.ConnType == "串口方式")
                    //    {
                    //        m_PlcEquip[tModel.Id] = new PLC.Equip.OMRON.HostLink.COM.Equip();
                    //    }
                    //    else
                    //    {
                    //        m_PlcEquip[tModel.Id] = new PLC.Equip.OMRON.FINS.GroupNet.Equip();
                    //    }
                    //    break;
                    //case PLCTYPE.汇川:
                    //    m_PlcEquip[tModel.Id] = new PLC.Equip.Inovance.Equip();
                    //    break;
                    //case PLCTYPE.西门子:
                    //    break;
                    //case PLCTYPE.台达:
                    //    m_PlcEquip[tModel.Id] = new DMTControl.Equip();
                    //    break;

                    default:
                        break;
                }

                //if (m_PlcEquip[tModel.Id] != null)
                //{
                //    m_PlcEquip[tModel.Id].Name = tModel.ConnObj;
                //    m_PlcEquip[tModel.Id].StationNum = tModel.StationNum;
                //}
            }
            catch (Exception ex)
            {

            }
        }

        //斑马打印机打印
        public static bool ZebraPrint(ZebraPrintModel tModel)
        {
            try
            {
                string strNowTime = DateTime.Now.ToString("yyyy-MM-dd");

                //string strPartNum = XmlControl.GetLinkValue(tModel.PartNumber).ToString();
                //string strBatch = XmlControl.GetLinkValue(tModel.Batch).ToString();
                //string strPartName = XmlControl.GetLinkValue(tModel.PartName).ToString();
                //string strCode2D = string.Format("######{0}{1}, strPartNum, strBatch"); 
                //string part1 = strPartName.Substring(0, 17);
                //string part2 = strPartName.Substring(17); 
                //string strManuDate = XmlControl.GetLinkValue(tModel.ManuDate).ToString();

                string strPartNum = "02A10000026M";
                string strBatch = "A220A12001"; 
                string strCode2D = "######012345678912AABBqrcode";
                string part1 = "R PREISION 0.51& +1^FS";
                string part2 = "% 1 / 16W 0402^FS ";
                string strManuDate = "2021-03-11";

                string s_PartNum = "料号";
              

                StringBuilder str = new StringBuilder();
                str.Append("^XA");
                str.AppendLine();
                str.Append("^FO800,200");
                str.Append("^BQ,2,10");
                str.Append(string.Format("^FDD03040C,LA,{0}^FS", strCode2D));
                str.AppendLine();
                str.Append("^FO300,550");
                str.Append("^A0N,150,150");
                str.Append(string.Format("^FD{0}:{1}^FS", s_PartNum, strPartNum));
                str.AppendLine();
                str.Append("^FO300,630");
                str.Append("^A@N,150,150");
                str.Append("^FD      __________^FS");
                str.AppendLine();
                str.Append("^FO300,830");
                str.Append("^A0N,50,50");
                str.Append(string.Format("^FD批次：{0}^FS", strBatch));
                str.AppendLine();
                str.Append("^FO300,830");
                str.Append("^A@N,150,150");
                str.Append("^FD      __________^FS");
                str.AppendLine();
                str.Append("^FO300,950");
                str.Append("^A0N,120,120");
                str.Append(string.Format("^FD品名：{0}^FS", part1));
                str.AppendLine();
                str.Append("^FO300,1000");
                str.Append("^A@N,120,120");
                str.Append("^FD      __________^FS");
                str.AppendLine();
                str.Append("^FO300,1090");
                str.Append("^A0N,120,120");
                str.Append(string.Format("^FD      {0}^FS", part2));
                str.Append("^FO300,1140");
                str.Append("^A@N,120,120");
                str.Append("^FD      __________^FS");
                str.AppendLine();
                str.Append("^FO300,1230");
                str.Append("^A0,120,120");
                str.Append(string.Format("^FD制造日期：{0}^FS", strManuDate));
                str.AppendLine();
                str.Append("^FO300,1370");
                str.Append("^A0,120,120");
                str.Append(string.Format("^FD收料日期：{0}^FS", strNowTime));
                str.AppendLine();
                str.Append("^XZ");

                RawPrinterHelper.SendStringToPrinter(tModel.PrintName, str.ToString());

                //读写文件
                byte[] myByte = Encoding.UTF8.GetBytes(str.ToString());
                using (FileStream sw = new FileStream("D://Zebra_1.txt", FileMode.OpenOrCreate))
                {
                    sw.Write(myByte, 0, str.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
                return false;
            }
        }

        public static bool ZebraPrint2(ZebraPrintModel model)
        {
            string strPartNum = "N";
            object partnum = XmlControl.GetLinkValue(model.PartNumber);
            if (partnum != null)
            {
                strPartNum = XmlControl.GetLinkValue(model.PartNumber).ToString();
            }

            string strBatch = "N";
            object batch = XmlControl.GetLinkValue(model.Batch);
            if (batch != null && batch.ToString() != "")
            {
                strBatch = batch.ToString();
            }

            string strPartName = "N";
            string part1 = "N";
            string part2 = "N";
            object partName = XmlControl.GetLinkValue(model.PartName);
            if (partName != null)
            {
                strPartName = partName.ToString();
                if (strPartName.Length > 20)
                {
                    part1 = strPartName.Substring(0, 19);
                    part2 = strPartName.Substring(19);
                }
            }

            string strManuDate = "N";
            object manuDate = XmlControl.GetLinkValue(model.ManuDate);
            if (manuDate != null)
            {
                strManuDate = manuDate.ToString();
            }              

            ZebraPicControl.drawFinalLable(model.PrintName, strPartNum, strBatch, part1, part2, strManuDate);
            
            return true;
        }

        //保存旋转中心
        public static void GetRotateCenter(RotateCenterModel tModel)
        {
            string strPath = Global.Model3DPath + "//" + tModel.Name + "_Mat2d.tup";

            HTuple hv_Mat2d = new HTuple();
            double[] hv_Rows = XmlControl.GetLinkValue(tModel.FormX) as double[];
            double[] hv_Cols = XmlControl.GetLinkValue(tModel.FormY) as double[];
            HObject ho_Contour1 = null;
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            ho_Contour1.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_Rows, hv_Cols);

            HTuple hv_RowX, hv_ColumnY, hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
            HOperatorSet.FitCircleContourXld(ho_Contour1, "algebraic", -1, 0, 0, 3, 2, out hv_RowX,
                out hv_ColumnY, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);

            //保存图像的 旋转中心  
            HTuple hv_ModelParam = new HTuple();
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_RowX);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_ColumnY);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_Radius);

            m_DelOutPutLog(string.Format("X:{0} Y:{1} Radius:{2}", hv_RowX, hv_ColumnY, hv_Radius), LogLevel.Debug);

            HOperatorSet.WriteTuple(hv_ModelParam, strPath);
        }

        //通过旋转中心求坐标值
        public static void GetRoationXYValue(string path, HTuple phi, HTuple findRow, HTuple findCol, out HTuple rotateRow, out HTuple rotateCol)
        {
            double[] dValue = new double[2];
            HTuple RoatateTuple1 = null, hv_Qx1 = null, hv_Qy1 = null;
              
            HTuple hv_homat2dIdentity, hv_RotateMat;
            HOperatorSet.ReadTuple(path, out RoatateTuple1);
            HOperatorSet.HomMat2dIdentity(out hv_homat2dIdentity);
            HOperatorSet.HomMat2dRotate(hv_homat2dIdentity, phi, RoatateTuple1[0], RoatateTuple1[1], out hv_RotateMat);

            HOperatorSet.AffineTransPoint2d(hv_RotateMat, findRow, findCol, out rotateRow, out rotateCol);

        }

        public static int UploadFtp(string filename, string savePath, string strIp, string strUser, string strPassword, out string errorMsg)
        {
            FtpWebRequest reqFTP = null;
            string serverIP;
            string userName;
            string password;
            string url;

            try
            {
                FileInfo fileInf = new FileInfo(filename);
                serverIP = strIp;
                userName = strUser;
                password = strPassword;
                //url = "ftp://" + serverIP + "/" + Path.GetFileName(filename);
                FtpCheckDirectoryExist("ftp://" + strIp, strUser, strPassword, savePath);
                url = "ftp://" + serverIP + "/" + savePath;

                Uri uri = dealSpecialChar(new Uri(url));
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();

                errorMsg = "";
                return 0;
            }
            catch (Exception ex)
            {
                if (reqFTP != null)
                {
                    reqFTP.Abort();
                }

                errorMsg = ex.Message;
                return -2;
            }
        }

        public static Uri dealSpecialChar(Uri uri)
        {
            Uri newUri;
            if (uri.ToString().Contains("#"))
            {
                newUri = new Uri(uri.ToString().Replace("#", Uri.HexEscape('#')));
            }
            else
            {
                newUri = uri;
            }

            return newUri;
        }
        //判断文件的目录是否存,不存则创建
        public static void FtpCheckDirectoryExist(string strIp, string strUser, string strPassword, string destFilePath)
        {
            string fullDir = destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
            string[] dirs = fullDir.Split('/');
            string curDir = "//";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "//";
                        if (Directory.Exists(strIp + curDir))
                        {
                            continue;
                        }

                        string strPath = strIp + curDir;
                        string str = strPath.Substring(strPath.Length - 2);
                        if (str == "//")
                        {
                            strPath = strPath.Substring(0, strPath.Length - 2);
                        }
                        //创建目录

                        Uri uri = dealSpecialChar(new Uri(strPath));
                        //FtpWebRequest req = (FtpWebRequest)WebRequest.Create(strPath);
                        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uri);
                        req.Credentials = new NetworkCredential(strUser, strPassword);
                        req.Method = WebRequestMethods.Ftp.MakeDirectory;
                        try
                        {
                            FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                            response.Close();
                        }
                        catch (Exception ex)
                        {
                            req.Abort();
                            continue;
                        }
                        req.Abort();
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public static int DownloadFtp(string filename, string strIp, string strUser, string strPassword)
        {
            FtpWebRequest reqFTP;
            string serverIP;
            string userName;
            string password;
            string url;

            try
            {
                serverIP = strIp;
                userName = strUser;
                password = strPassword;
                url = "ftp://" + serverIP + "/" + Path.GetFileName(filename);

                FileStream outputStream = new FileStream(filename, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return 0;
            }
            catch (Exception ex)
            {
                //SystemLog.logger(ex.InnerException.Message);
                return -2;
            }
        }

    }
    #endregion

}
//5578