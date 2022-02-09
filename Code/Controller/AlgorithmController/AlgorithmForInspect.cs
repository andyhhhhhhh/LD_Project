using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using AlgorithmModel;
using ServiceCollection.Services;

namespace AlgorithmController
{
    public class AlgorithmForInspect
    {
        List<AlgorithmParamModel> m_AlgorithmParamModelList = new List<AlgorithmParamModel>();
        AlgorithmParamService m_AlgorithmParamService2 = new AlgorithmParamService();

        HWindow m_WindowHandle = null;
        public bool Init(object parameter)
        {
            try
            {
                m_WindowHandle = (HWindow)parameter;
                m_AlgorithmParamModelList = m_AlgorithmParamService2.QueryAll();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AlgorithmResultModel Run(HObject ho_Image, AlgorithmParamModel tModel)
        {
            AlgorithmResultModel resultModel = new AlgorithmResultModel();

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            HObject ho_ContCircle = null, ho_MeasureCircleContours1 = null;
            HObject ho_MeasureCross1 = null, ho_CircleContours1 = null;

            // Local control variables 
            HTuple hv_WindowHandle = null, hv_SaveModelPath = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Radius = new HTuple(), hv_ModelID = new HTuple();
            HTuple hv_Index = null, hv_TupleRadius = new HTuple();
            HTuple hv_Row3 = new HTuple(), hv_Column3 = new HTuple();
            HTuple hv_Angle1 = new HTuple(), hv_Score1 = new HTuple();
            HTuple hv_InCircleRow1 = new HTuple(), hv_InCircleCol1 = new HTuple();
            HTuple hv_InCircleRadiu = new HTuple(), hv_InMeasureLength1 = new HTuple();
            HTuple hv_InMeasureLength2 = new HTuple(), hv_InMeasureSigma = new HTuple();
            HTuple hv_InMeasureThreshold = new HTuple(), hv_InMeasureSelect = new HTuple();
            HTuple hv_InMeasureTransition = new HTuple(), hv_InMeasureNumber = new HTuple();
            HTuple hv_InMeasureSore = new HTuple(), hv_bDisp = new HTuple();
            HTuple hv_CircleCenterRow = new HTuple(), hv_CircleCenterColumn = new HTuple();
            HTuple hv_CircleRadius = new HTuple(), hv_bFindCircle2D = new HTuple();
            HTuple hv_CircleCenterRow1 = new HTuple(), hv_CircleCenterColumn1 = new HTuple();
            HTuple hv_CircleRadius1 = new HTuple(), hv_bFindCircle2D1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross1);
            HOperatorSet.GenEmptyObj(out ho_CircleContours1);
            try
            {
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.CloseWindow(HDevWindowStack.Pop());
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }

                resultModel.ResultImage = ho_Image.Clone();
                hv_SaveModelPath = Environment.CurrentDirectory + "/DOC/Model/";

                //HObject ExpTmpOutVar_0;
                //HOperatorSet.Rgb1ToGray(ho_Image, out ExpTmpOutVar_0);
                //ho_Image.Dispose();
                //ho_Image = ExpTmpOutVar_0;

                HOperatorSet.Rgb1ToGray(ho_Image, out ho_Image);

                //输入参数
                HOperatorSet.ReadTuple(hv_SaveModelPath  + "CheckRadus.tup", out hv_TupleRadius);
                HOperatorSet.ReadShapeModel(hv_SaveModelPath + "ModelID", out hv_ModelID);
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), 0.5, 1, 0.5, "least_squares", 0, 0.9,
                    out hv_Row3, out hv_Column3, out hv_Angle1, out hv_Score1);

                if ((int)(new HTuple((new HTuple(1)).TupleNotEqual(new HTuple(hv_Row3.TupleLength()
                    )))) != 0)
                { 
                    resultModel.Error = 1;
                    resultModel.ResultImage = ho_Image;
                    resultModel.ErrorMessage = "模板匹配失败，物料NG，或请检查是否有物料到位，或光源是否打开";
                    return resultModel;
                }

                hv_Row = hv_Row3.Clone();
                hv_Column = hv_Column3.Clone();
                hv_Radius = hv_TupleRadius.Clone();
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_Row, hv_Column, hv_Radius,
                    0, 6.28318, "positive", 1);


                hv_InCircleRow1 = hv_Row.Clone();
                hv_InCircleCol1 = hv_Column.Clone();
                hv_InCircleRadiu = hv_Radius.Clone();
 
                hv_InMeasureLength1 = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureLength1").ParamValue;
                hv_InMeasureLength2 = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureLength2").ParamValue;
                hv_InMeasureSigma = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureSigma").ParamValue;
                hv_InMeasureThreshold = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureThreshold").ParamValue;
                hv_InMeasureNumber = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureNumber").ParamValue;
                hv_InMeasureSore = m_AlgorithmParamModelList.FirstOrDefault(x => x.ParamName == "InMeasureScore").ParamValue;

                hv_InMeasureLength1 = 100;
                hv_InMeasureLength2 = 20;
                hv_InMeasureSigma = 1.0;
                hv_InMeasureThreshold = 20;
                hv_InMeasureSelect = "first";
                hv_InMeasureTransition = "negative";
                hv_InMeasureNumber = 200;
                hv_InMeasureSore = 0.2;
                hv_bDisp = 0;
                //输出参数
                hv_CircleCenterRow = new HTuple();
                hv_CircleCenterColumn = new HTuple();
                hv_CircleRadius = new HTuple();
                hv_bFindCircle2D = 0;

                ho_MeasureCircleContours1.Dispose(); ho_MeasureCross1.Dispose(); ho_CircleContours1.Dispose();
                AlgorithmCommHelper.FindCircle2D(ho_Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                    out ho_CircleContours1, hv_InCircleRow1, hv_InCircleCol1, hv_InCircleRadiu,
                    hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                    hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureSore,
                    hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                    out hv_bFindCircle2D1);


                HObject dispObj;
                HOperatorSet.GenEmptyObj(out dispObj);
                HOperatorSet.ConcatObj(dispObj, ho_CircleContours1, out dispObj);

                resultModel.DispObj = dispObj;

                //resultModel.ResultImage = ho_Image;
                resultModel.dValueRow = Math.Round((double)hv_InCircleRow1, 3);
                resultModel.dValueCol = Math.Round((double)hv_InCircleCol1, 3);

                resultModel.Error = 0;
                

                ho_ContCircle.Dispose();
                ho_MeasureCircleContours1.Dispose();
                ho_MeasureCross1.Dispose();
                ho_CircleContours1.Dispose();

                return resultModel;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ContCircle.Dispose();
                ho_MeasureCircleContours1.Dispose();
                ho_MeasureCross1.Dispose();
                ho_CircleContours1.Dispose();

                resultModel.Error = 1;
                return resultModel;
            }
            
        }

        //找模板--九点标定用
        public AlgorithmResultModel SearchShapeModel_Cal(HObject ho_Image, AlgorithmCalibModel tModel, out HObject ho_ContoursAffinTrans)
        {
            string tuplePath = tModel.ModelPath.Substring(0, tModel.ModelPath.LastIndexOf("\\"));
            AlgorithmResultModel resultModel = new AlgorithmResultModel();
            ho_ContoursAffinTrans = null;

            // Local control variables 

            HTuple hv_ModelPath = null, hv_Row = null;
            HTuple hv_Column = null, hv_Angle = null, hv_Score = null;
            HTuple hv_ModelID1 = null;
            // Initialize local and output iconic variables   
            hv_ModelPath = tuplePath + "//ModelID.shm";

            HOperatorSet.ReadShapeModel(hv_ModelPath, out hv_ModelID1);
            HOperatorSet.FindShapeModel(ho_Image, hv_ModelID1, -0.39, 0.79, 0.5, 1, 0.5,
                "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID1, "red", hv_Row, hv_Column, hv_Angle, 1, 1, 0, out ho_ContoursAffinTrans);
            HOperatorSet.ClearShapeModel(hv_ModelID1);

            resultModel.ResultImage = ho_Image;
            resultModel.DispObj = ho_ContoursAffinTrans;
            resultModel.dValueRow = hv_Row.D;
            resultModel.dValueCol = hv_Column.D;

            return resultModel;
        }

    }
}
