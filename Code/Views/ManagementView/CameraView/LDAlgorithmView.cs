using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using XMLController;
using SequenceTestModel;
using HalconView;
using System.IO;
using HalconDotNet;
using VisionController;
using AlgorithmController;
using GlobalCore;
using CameraContorller;

namespace ManagementView
{
    public partial class LDAlgorithmView : UserControl
    {
        LDAlgorithmControl m_algorithm = new LDAlgorithmControl();
        HSmartWindow m_hsmartWindow = new HSmartWindow();

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;
        public LDAlgorithmView()
        {
            InitializeComponent();
        }

        private void LDAlgorithmView_Load(object sender, EventArgs e)
        {
            try
            {
                CommHelper.LayoutChildFillView(panelView1, m_hsmartWindow);
                UpdateData();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void UpdateData()
        {
            try
            {
                switch (tabControl.SelectedTabIndex)
                {
                    case 0:
                        var model = XmlControl.sequenceModelNew.bigFeedAlgorithmModel;
                        if (model != null)
                        {
                            propertyBigFeed.SelectedObject = model;
                        }
                        break;

                    case 1:
                        var model2 = XmlControl.sequenceModelNew.bigFixedAlgorithmModel;
                        if (model2 != null)
                        {
                            propertyBigFixed.SelectedObject = model2;
                        }
                        break;

                    case 2:
                        var model3 = XmlControl.sequenceModelNew.smallJudgePosModel;
                        if(model3 != null)
                        {
                            propertySmallCenter.SelectedObject = model3;
                        }
                        break;

                    case 3:
                        var model4 = XmlControl.sequenceModelNew.smallFixedPosModel;
                        if (model4 != null)
                        {
                            propertySmallFixed.SelectedObject = model4;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region 按钮操作
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                switch(tabControl.SelectedTabIndex)
                {
                    case 0:
                        BigFeedSave();
                        break;
                    case 1:
                        BigFixedSave();
                        break;
                    case 2:
                        SmallJudgePosSave();
                        break;
                    case 3:
                        SmallFixedPosSave();
                        break; 

                    default:
                        break;
                }

                AddLog("保存参数成功");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnNewRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if(tabControl.SelectedTabIndex != 3)
                {
                    if (XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Row1 == 0)
                    {
                        m_hsmartWindow.AddRect();
                    }
                    else
                    {
                        m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Row1, XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Column1,
                            XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Row2, XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Column2);
                    }
                }
                else
                {
                    if (XmlControl.sequenceModelNew.smallFixedPosModel.Row1 == 0)
                    {
                        m_hsmartWindow.AddRect();
                    }
                    else
                    {
                        m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.smallFixedPosModel.Row1, XmlControl.sequenceModelNew.smallFixedPosModel.Column1,
                            XmlControl.sequenceModelNew.smallFixedPosModel.Row2, XmlControl.sequenceModelNew.smallFixedPosModel.Column2, "green");
                    }

                    if (XmlControl.sequenceModelNew.smallFixedPosModel.OcrRow1 == 0)
                    {
                        m_hsmartWindow.AddRect(100, 100, 500, 500, "red");
                    }
                    else
                    {
                        m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.smallFixedPosModel.OcrRow1, XmlControl.sequenceModelNew.smallFixedPosModel.OcrColumn1,
                            XmlControl.sequenceModelNew.smallFixedPosModel.OcrRow2, XmlControl.sequenceModelNew.smallFixedPosModel.OcrColumn2, "red");
                    }
                }

            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                bool bresult = false;
                switch (tabControl.SelectedTabIndex)
                {
                    case 0:
                        bresult = BigFeedAlgorithm();
                        break;
                    case 1:
                        bresult = BigFixedAlgorithm();
                        break;
                    case 2:
                        bresult = SmallJudgePosAlgorithm();
                        break;
                    case 3:
                        bresult = SmallFixedPosAlgorithm();
                        break;

                    default:
                        break;
                }

                AddLog(string.Format("执行算法{0}", bresult ? "成功" : "失败"));
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if(tabControl.SelectedTabIndex != 3)
                {
                    return;
                }

                string modelIDPath = Global.Model3DPath + "//" + "SmallFixed//";
                if(!Directory.Exists(modelIDPath))
                {
                    Directory.CreateDirectory(modelIDPath);
                }

                CreateSmallFixedModel(modelIDPath, XmlControl.sequenceModelNew.smallFixedPosModel);

                AddLog("创建模板成功");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
                Stream inputStream = null;
                OpenFileDialogImage.Filter = "All files (*.*)|*.*";
                if (OpenFileDialogImage.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((inputStream = OpenFileDialogImage.OpenFile()) != null)
                        {
                            String strImageFile = OpenFileDialogImage.FileName;
                            OpenFileDialogImage.InitialDirectory = strImageFile;
                            HObject hImage;
                            HOperatorSet.ReadImage(out hImage, strImageFile);
                            m_hsmartWindow.FitImageToWindow(new HImage(hImage), null);
                            AddLog("读入图像成功");
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog("打开图像失败!" + ex.Message + "");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                int index = tabControl.SelectedTabIndex == 0 || tabControl.SelectedTabIndex == 1 ? 0 : 1;
                Camera2DSetModel tModel = XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == index);

                CameraResultModel resultModel = m_FuncCameraSnap(tModel);

                if (resultModel.RunResult)
                {
                    m_hsmartWindow.FitImageToWindow(resultModel.Image as HObject, null);
                }
                else
                {
                    AddLog(resultModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        /// <summary>
        /// 大视野相机入料补正保存
        /// </summary>
        private void BigFeedSave()
        {
            if (propertyBigFeed.SelectedObject != null)
            {
                XmlControl.sequenceModelNew.bigFeedAlgorithmModel = propertyBigFeed.SelectedObject as BigFeedAlgorithmModel;
            }

            List<double> listValue = m_hsmartWindow.GetRectArea();
            if (listValue != null && listValue.Count > 0)
            {
                XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Row1 = listValue[0];
                XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Column1 = listValue[1];
                XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Row2 = listValue[2];
                XmlControl.sequenceModelNew.bigFeedAlgorithmModel.Column2 = listValue[3];

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[0], listValue[1], listValue[2], listValue[3]);
                m_hsmartWindow.ClearAllObjects();

                UpdateData();
            }
        }

        /// <summary>
        /// 大视野相机入料补正算法
        /// </summary>
        private bool BigFeedAlgorithm()
        {
            BigFeedAlgorithmModel tModel = XmlControl.sequenceModelNew.bigFeedAlgorithmModel;
            HObject ho_Rect = new HObject();
            HOperatorSet.GenRectangle1(out ho_Rect, tModel.Row1, tModel.Column1, tModel.Row2, tModel.Column2);

            HObject ho_DispObjects, ho_Rectangle1;
            HTuple hv_BigCenterRow, hv_ProdAngleMean, hv_BigCenterCol, hv_Error;
            m_algorithm.GetProdRegionAngle(m_hsmartWindow.Image, ho_Rect, out ho_DispObjects, out ho_Rectangle1, tModel.GrayOpenHeight, tModel.GrayOpenWidth,
                tModel.DynThr, tModel.HysThrMin, tModel.HysThrMax, tModel.SelectAreaMin, tModel.SingleSelectAreaMin, tModel.SingleSelectAreaMax,
                tModel.ClosingW, tModel.ClosingH, out hv_ProdAngleMean, out hv_BigCenterRow, out hv_BigCenterCol, out hv_Error);

            bool bresult = hv_Error == 0;
            if(bresult)
            {
                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(1);
                m_hsmartWindow.GetWindowHandle().DispObj(ho_DispObjects);

                string strLog = string.Format("中心Row:{0} Column:{1} Angle:{2}", hv_BigCenterRow, hv_BigCenterCol, hv_ProdAngleMean);
                AddLog(strLog);
            }

            return bresult; 
        }

        /// <summary>
        /// 大视野相机定位保存
        /// </summary>
        private void BigFixedSave()
        {
            List<double> listValue = m_hsmartWindow.GetRectArea();
            if (listValue != null && listValue.Count > 0)
            {
                XmlControl.sequenceModelNew.bigFixedAlgorithmModel.Row1 = listValue[0];
                XmlControl.sequenceModelNew.bigFixedAlgorithmModel.Column1 = listValue[1];
                XmlControl.sequenceModelNew.bigFixedAlgorithmModel.Row2 = listValue[2];
                XmlControl.sequenceModelNew.bigFixedAlgorithmModel.Column2 = listValue[3];

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[0], listValue[1], listValue[2], listValue[3]);
                m_hsmartWindow.ClearAllObjects();

                UpdateData();
            }
        }

        /// <summary>
        /// 大视野相机定位算法
        /// </summary>
        private bool BigFixedAlgorithm()
        {
            BigFixedAlgorithmModel tModel = XmlControl.sequenceModelNew.bigFixedAlgorithmModel;
            HObject ho_Rect = new HObject();
            HOperatorSet.GenRectangle1(out ho_Rect, tModel.Row1, tModel.Column1, tModel.Row2, tModel.Column2);

            HObject ho_OutRegion;
            HTuple hv_AnyRow, hv_AnyCol,hv_AnyAng, hv_Count;
            m_algorithm.LDFindAnyCenter(m_hsmartWindow.Image, ho_Rect, out ho_OutRegion, out hv_AnyRow, out hv_AnyCol, out hv_AnyAng, out hv_Count);
             
            m_hsmartWindow.GetWindowHandle().SetDraw("margin");
            m_hsmartWindow.GetWindowHandle().SetColor("red");
            m_hsmartWindow.GetWindowHandle().SetLineWidth(1);
            m_hsmartWindow.GetWindowHandle().DispObj(ho_OutRegion);

            return true;
        }
        
        /// <summary>
        /// 小视野相机入料补正保存
        /// </summary>
        private void SmallJudgePosSave()
        {
            if (propertySmallCenter.SelectedObject != null)
            {
                XmlControl.sequenceModelNew.smallJudgePosModel = propertySmallCenter.SelectedObject as SmallJudgePosModel;
            }

            UpdateData();
        }

        /// <summary>
        /// 小视野相机入料补正算法
        /// </summary>
        private bool SmallJudgePosAlgorithm()
        {
            SmallJudgePosModel tModel = XmlControl.sequenceModelNew.smallJudgePosModel;
            HObject ho_OutRegion;
            HTuple hv_IsExistProduct, hv_IsCenterPos, hv_SubCol;
            m_algorithm.LDSmallJudgePos(m_hsmartWindow.Image, out ho_OutRegion, tModel.MinSubCol, tModel.MinGray, out hv_IsCenterPos, out hv_IsExistProduct, out hv_SubCol);

            m_hsmartWindow.GetWindowHandle().SetDraw("margin");
            m_hsmartWindow.GetWindowHandle().SetColor("red");
            m_hsmartWindow.GetWindowHandle().SetLineWidth(1);
            m_hsmartWindow.GetWindowHandle().DispObj(ho_OutRegion);

            string strLog = string.Format("是否在中心:{0} 存在产品:{1} 离中心距离:{2}", hv_IsCenterPos, hv_IsExistProduct, hv_SubCol);
            AddLog(strLog);

            return true;
        }

        /// <summary>
        /// 小视野定位保存
        /// </summary>
        private void SmallFixedPosSave()
        {
            List<double> listValue = m_hsmartWindow.GetRectArea();
            if (listValue != null && listValue.Count > 0)
            {
                XmlControl.sequenceModelNew.smallFixedPosModel.Row1 = listValue[0];
                XmlControl.sequenceModelNew.smallFixedPosModel.Column1 = listValue[1];
                XmlControl.sequenceModelNew.smallFixedPosModel.Row2 = listValue[2];
                XmlControl.sequenceModelNew.smallFixedPosModel.Column2 = listValue[3];

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("green");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[0], listValue[1], listValue[2], listValue[3]);


                XmlControl.sequenceModelNew.smallFixedPosModel.OcrRow1 = listValue[4];
                XmlControl.sequenceModelNew.smallFixedPosModel.OcrColumn1 = listValue[5];
                XmlControl.sequenceModelNew.smallFixedPosModel.OcrRow2 = listValue[6];
                XmlControl.sequenceModelNew.smallFixedPosModel.OcrColumn2 = listValue[7];

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("green");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[4], listValue[5], listValue[6], listValue[7]);

                m_hsmartWindow.ClearAllObjects();                
            }

            if (propertySmallCenter.SelectedObject != null)
            {
                XmlControl.sequenceModelNew.smallFixedPosModel = propertySmallFixed.SelectedObject as SmallFixedPosModel;
            }

            UpdateData();
        }

        /// <summary>
        /// 小视野定位算法
        /// </summary>
        private bool SmallFixedPosAlgorithm()
        {
            SmallFixedPosModel tModel = XmlControl.sequenceModelNew.smallFixedPosModel;
            HObject ho_OcrRegion, ho_OutRegion, ho_OcrOutRegion, AllProdAndKongSortObj;
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
            
            m_algorithm.LDSmallFixedAlgorithm(m_hsmartWindow.Image, ho_OcrRegion, modelIDPath, tModel.PyramidLevel, tModel.LastPyramidLevel, tModel.MinScore, tModel.Greediness,
                tModel.ThrsholdMin, tModel.HysThresholdMin, tModel.HysThesholdMax, tModel.CloseRec, tModel.ModelRegionAreaDownValue, tModel.IsNotProdValue, tModel.IsNotProdValue * -1,
                tModel.AutoThreshold, tModel.DistancePP, tModel.NeedOcr, tModel.IsIngoreCalu, tModel.OcrLength, tModel.JudgeBarLength, tModel.OcrBinPath, out AllProdAndKongSortObj, out ho_OutRegion, out ho_OcrOutRegion, out AllProdAndKongSortName, 
                out  OcrCenterRow, out OcrCenterCol, out OcrCenterPhi, out distance, out ICanGet, out IExistProduct, out firstOcr, out bar, out strLog, out bResult);

            m_hsmartWindow.GetWindowHandle().SetDraw("margin");
            m_hsmartWindow.GetWindowHandle().SetColor("red");
            m_hsmartWindow.GetWindowHandle().SetLineWidth(1);
            m_hsmartWindow.GetWindowHandle().DispObj(ho_OutRegion);

            m_hsmartWindow.GetWindowHandle().SetColor("green");
            m_hsmartWindow.GetWindowHandle().SetLineWidth(3);
            m_hsmartWindow.GetWindowHandle().DispObj(ho_OcrOutRegion);

            AddLog("执行结果：" + (bResult ? "成功" : "失败"));
            AddLog(string.Format("Row:{0} Col:{1} Angle:{2}", OcrCenterRow, OcrCenterCol, OcrCenterPhi));
            AddLog(strLog);

            return true;
        }

        private void AddLog(string strLog)
        {
            logView1.LogMessage(strLog);
        }

        private void tabControl_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            try
            {
                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 小视野定位创建模板
        /// </summary>
        /// <param name="Modelpath">保存路径</param>
        /// <param name="tModel">Model</param>
        private void CreateSmallFixedModel(string Modelpath, SmallFixedPosModel tModel)
        {
            try
            {
                HObject ho_CreateRectangle, ho_ModelRectangle, ho_ImageZoomed;
                HOperatorSet.GenEmptyObj(out ho_CreateRectangle);
                HOperatorSet.GenEmptyObj(out ho_ImageZoomed);
                HOperatorSet.GenEmptyObj(out ho_ModelRectangle);        

                HOperatorSet.GenRectangle1(out ho_CreateRectangle, tModel.Row1, tModel.Column1, tModel.Row2, tModel.Column2);

                HTuple hv_AngleStart = -0.39;
                HTuple hv_AngleExtent = 0.79;
                HOperatorSet.TupleRad(tModel.StartingAngle, out hv_AngleStart);
                HOperatorSet.TupleRad(tModel.AngleExtent, out hv_AngleExtent);

                HTuple hv_ScaleImageValue = 4.0 / 10;
                HTuple hv_NumLevels = "auto";
                HTuple hv_AngleStep = "auto";
                HTuple hv_Optimization = "auto";
                HTuple hv_Metric = "use_polarity";
                HTuple hv_Contrast = "auto";
                HTuple hv_MinContrast = "auto";

                HTuple hv_ShapeModel = new HTuple();
                hv_ShapeModel[0] = "RegionXLD";
                hv_ShapeModel[1] = "ImageCreate";
                hv_ShapeModel = "RegionXLD";
                //输出
                HTuple hv_ModelID = new HTuple();
                HTuple hv_bCreateModel = 0;
                ho_ModelRectangle.Dispose();

                HTuple hv_ModelRow = null;
                HTuple hv_ModelColumn = null, hv_ModelAngle = null, hv_ModelScore = null;
                HTuple hv_ModelRegionArea = null, hv_ModelRegionRow = null;
                HTuple hv_ModelRegionColumn = null, hv_Rec2ModelRow = null;
                HTuple hv_Rec2ModelColumn = null, hv_Rec2ModelPhi = null;
                HTuple hv_ModelRegionWidth = null, hv_ModelRegionHeight = null;
                HTuple hv_ModelRValue = null, hv_ModelGValue = null, hv_ModelBValue = null;
                HTuple hv_ModelIValue = null;

                m_algorithm.CreateShapeModelInfo(m_hsmartWindow.Image, ho_CreateRectangle, out ho_ModelRectangle, hv_ScaleImageValue,
                       hv_NumLevels, hv_AngleStart, hv_AngleExtent, hv_AngleStep, hv_Optimization,
                       hv_Metric, hv_Contrast, hv_MinContrast, hv_ShapeModel, out hv_ModelID,
                       out hv_ModelRow, out hv_ModelColumn, out hv_ModelAngle, out hv_ModelScore,
                       out hv_ModelRegionArea, out hv_ModelRegionRow, out hv_ModelRegionColumn,
                       out hv_Rec2ModelRow, out hv_Rec2ModelColumn, out hv_Rec2ModelPhi, out hv_ModelRegionWidth,
                       out hv_ModelRegionHeight, out hv_ModelRValue, out hv_ModelGValue, out hv_ModelBValue,
                       out hv_ModelIValue, out hv_bCreateModel);

                HTuple hv_Row, hv_Column, hv_Angle, hv_Score;
                HObject ho_ImageReduced;
                HOperatorSet.ReduceDomain(m_hsmartWindow.Image, ho_CreateRectangle, out ho_ImageReduced);
                ho_ImageZoomed.Dispose();
                HOperatorSet.ZoomImageFactor(m_hsmartWindow.Image, out ho_ImageZoomed, hv_ScaleImageValue,
                    hv_ScaleImageValue, "constant");
                HOperatorSet.FindShapeModel(ho_ImageZoomed, hv_ModelID, (new HTuple(tModel.StartingAngle)).TupleRad()
                 , (new HTuple(tModel.AngleExtent)).TupleRad(), tModel.MinScore, 1, 0, "least_squares", (new HTuple(tModel.PyramidLevel)).TupleConcat(tModel.LastPyramidLevel), tModel.Greediness, out hv_Row,
                 out hv_Column, out hv_Angle, out hv_Score);

                HObject ho_Contour;
                AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_Row, hv_Column, hv_Angle, 1, 1, 0, out ho_Contour);
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().DispObj(ho_Contour);

                ho_ImageReduced.Dispose();

                HOperatorSet.WriteShapeModel(hv_ModelID, Modelpath + "ModelID.shm");
                HOperatorSet.WriteRegion(ho_ModelRectangle, Modelpath + "ModelRectangle.hobj");
                HTuple hv_ModelTup = new HTuple();
                hv_ModelTup.Append(hv_ModelRow);
                hv_ModelTup.Append(hv_ModelColumn);
                hv_ModelTup.Append(hv_ModelAngle);
                HOperatorSet.WriteTuple(hv_ModelTup, Modelpath + "ModelTup.tup");

                HTuple hv_RecParam = new HTuple();
                hv_RecParam.Append(hv_Rec2ModelRow);
                hv_RecParam.Append(hv_Rec2ModelColumn);
                hv_RecParam.Append(hv_Rec2ModelPhi);
                hv_RecParam.Append(hv_ModelRegionArea);
                hv_RecParam.Append(hv_ModelRegionWidth);
                hv_RecParam.Append(hv_ModelRegionHeight);
                HOperatorSet.WriteTuple(hv_RecParam, Modelpath + "Rec2Model.tup");
                                
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 循环测试算法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCycleTest_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = loadPath.FolderPath;
                var listFile = Directory.GetFiles(strPath, "*.jpg").Union(Directory.GetFiles(strPath, "*.png")).Union(Directory.GetFiles(strPath, "*.bmp")).ToArray().ToList();

                HObject ho_Image;
                foreach (var file in listFile)
                {
                    HOperatorSet.ReadImage(out ho_Image, file);
                    m_hsmartWindow.FitImageToWindow(ho_Image, null);
                    btnTest_Click(null, null);
                    var result = MessageBox.Show("是否继续测试?", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
