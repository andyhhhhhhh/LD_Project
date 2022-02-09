using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using AlgorithmModel;

namespace AlgorithmController
{
    public class AlgorithmTeachPos
    {
        int posX = 50;
        int posY = 50;
        HWindow m_WindowHandle = null;
        public bool Init(object parameter)
        {
            try
            {
                m_WindowHandle = (HWindow)parameter;
                HOperatorSet.SetColor(m_WindowHandle, "red");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AlgorithmResultModel Run(HObject ho_Image, AlgorithmParamModel tModel)
        {
            HObject ho_ContoursAffinTrans;
            AlgorithmResultModel resultModel = new AlgorithmResultModel();
            CreateShapeModel(ho_Image,tModel.ModelPath, out ho_ContoursAffinTrans); 

            resultModel.ResultImage = ho_Image.Clone();
            resultModel.DispObj = ho_ContoursAffinTrans;
            return resultModel;
        }

        public void CreateShapeModel(HObject m_hImage, string ModelPath, out HObject ho_ContoursAffinTrans)
        {
            // Local iconic variables 
            //string ModelPath = string.Format(@"{0}\Doc\Model\", Environment.CurrentDirectory);

            HObject ho_Rectangle, ho_ImageReduced; 
            // Local control variables 

            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null, hv_ModelID = null, hv_Row = null;
            HTuple hv_Column = null, hv_Angle = null, hv_Score = null;
            HTuple hv_ModelParam = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);


            //创建模板,选择特征明显区域绘制矩形，可以在BUTTON里面直接做，需要先把产品干净
            m_WindowHandle.SetColor("red");
            AlgorithmCommHelper.disp_message(m_WindowHandle, "画矩形模板区域", "window", 50, 50, "red", "true");
            HOperatorSet.DrawRectangle1(m_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                out hv_Column2);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(m_hImage, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", (new HTuple(0)).TupleRad()
                , (new HTuple(360)).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto",
                out hv_ModelID);
            HOperatorSet.FindShapeModel(m_hImage, hv_ModelID, (new HTuple(0)).TupleRad(),
                (new HTuple(360)).TupleRad(), 0.5, 1, 0.5, "least_squares", 0, 0.9, out hv_Row,
                out hv_Column, out hv_Angle, out hv_Score);
            AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_Row, hv_Column, hv_Angle,
                1, 1, 0, out ho_ContoursAffinTrans);

            //保存模板和模板坐标位置
            //HOperatorSet.WriteShapeModel(hv_ModelID, ModelPath + "Model.shm");
            HOperatorSet.WriteShapeModel(hv_ModelID, ModelPath);

            hv_ModelParam = new HTuple();
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_Row);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_Column);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_Angle);

            string dir = ModelPath.Substring(0, ModelPath.LastIndexOf("\\"));
            HOperatorSet.WriteTuple(hv_ModelParam, dir + "\\ModelTup.tup");
            HOperatorSet.ClearShapeModel(hv_ModelID);
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
             
        }

        public void CreateShapeModel_Cal(HObject ho_Image, AlgorithmCalibModel tModel, out HObject ho_ContoursAffinTrans)
        {
            string tuplePath = tModel.ModelPath.Substring(0, tModel.ModelPath.LastIndexOf("\\"));
            ho_ContoursAffinTrans = null;
            // Local iconic variables 

            HObject ho_Rectangle, ho_ImageReduced;

            // Local control variables 

            HTuple hv_ModelPath = null, hv_Row1 = null;
            HTuple hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
            HTuple hv_ModelID = null, hv_Row = null, hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);

            hv_ModelPath = tuplePath + "//ModelID.shm";
            HOperatorSet.DrawRectangle1(m_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);

            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", 0, (new HTuple(360)).TupleRad()
                , "auto", "auto", "use_polarity", "auto", "auto", out hv_ModelID);
            HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, -0.39, 0.79, 0.5, 1, 0.5, "least_squares",
                0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_Row, hv_Column, hv_Angle,
                1, 1, 0, out ho_ContoursAffinTrans);

            HOperatorSet.WriteShapeModel(hv_ModelID, hv_ModelPath);
            HOperatorSet.ClearShapeModel(hv_ModelID);

            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();

        }
    }
}
