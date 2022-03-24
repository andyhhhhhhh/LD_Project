using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SequenceTestModel;
using CameraContorller;
using VisionController;
using HalconView;
using XMLController;
using HalconDotNet;
using System.IO;
using DevComponents.DotNetBar;

namespace ManagementView
{
    public partial class CheckAlgorithmView : UserControl
    {
        LDAlgorithmControl m_algorithm = new LDAlgorithmControl();
        HSmartWindow m_hsmartWindow = new HSmartWindow();

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;
        public CheckAlgorithmView()
        {
            InitializeComponent();
        }

        private void CheckAlgorithmView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl.SelectedTabIndex = 0;
                CommHelper.LayoutChildFillView(panelView1, m_hsmartWindow);
                UpdateData();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        #region 按钮操作

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

        private void btnNewRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnNewRegion.Text == "添加区域")
                {
                    btnNewRegion.Text = "添加完成";
                    btnNewRegion.Style = eDotNetBarStyle.VS2005;

                    if (tabControl.SelectedTabIndex == 0)
                    {
                        if (XmlControl.sequenceModelNew.algorithmModelP.Row1 == 0)
                        {
                            m_hsmartWindow.AddRect();
                        }
                        else
                        {
                            m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.algorithmModelP.Row1, XmlControl.sequenceModelNew.algorithmModelP.Column1,
                                XmlControl.sequenceModelNew.algorithmModelP.Row2, XmlControl.sequenceModelNew.algorithmModelP.Column2);
                        }
                    }
                    else if(tabControl.SelectedTabIndex == 1)
                    {
                        if (XmlControl.sequenceModelNew.algorithmModelN.Row1 == 0)
                        {
                            m_hsmartWindow.AddRect();
                        }
                        else
                        {
                            m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.algorithmModelN.Row1, XmlControl.sequenceModelNew.algorithmModelN.Column1,
                                XmlControl.sequenceModelNew.algorithmModelN.Row2, XmlControl.sequenceModelNew.algorithmModelN.Column2);
                        }
                    }
                }
                else
                {
                    btnNewRegion.Text = "添加区域";
                    btnNewRegion.Style = eDotNetBarStyle.StyleManagerControlled;

                    m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                    m_hsmartWindow.GetWindowHandle().SetColor("red");
                    m_hsmartWindow.GetWindowHandle().SetLineWidth(2);

                    var listValue = m_hsmartWindow.GetRectArea();
                    if (tabControl.SelectedTabIndex == 0)
                    {
                        XmlControl.sequenceModelNew.algorithmModelP.Row1 = listValue[0];
                        XmlControl.sequenceModelNew.algorithmModelP.Column1 = listValue[1];
                        XmlControl.sequenceModelNew.algorithmModelP.Row2 = listValue[2];
                        XmlControl.sequenceModelNew.algorithmModelP.Column2 = listValue[3];

                    }
                    else if (tabControl.SelectedTabIndex == 1)
                    {
                        XmlControl.sequenceModelNew.algorithmModelN.Row1 = listValue[0];
                        XmlControl.sequenceModelNew.algorithmModelN.Column1 = listValue[1];
                        XmlControl.sequenceModelNew.algorithmModelN.Row2 = listValue[2];
                        XmlControl.sequenceModelNew.algorithmModelN.Column2 = listValue[3];
                    }

                    m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[0], listValue[1], listValue[2], listValue[3]); 
                    m_hsmartWindow.ClearAllObjects();
                    UpdateData();
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }

        }

        private void btnSearchRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if(btnSearchRegion.Text == "添加搜索区域")
                {
                    btnSearchRegion.Text = "添加完成";
                    btnSearchRegion.Style = eDotNetBarStyle.VS2005;

                    if (tabControl.SelectedTabIndex == 0)
                    {
                        if (XmlControl.sequenceModelNew.algorithmModelP.SRow1 == 0)
                        {
                            m_hsmartWindow.AddRect();
                        }
                        else
                        {
                            m_hsmartWindow.AddRect(XmlControl.sequenceModelNew.algorithmModelP.SRow1, XmlControl.sequenceModelNew.algorithmModelP.SColumn1,
                                XmlControl.sequenceModelNew.algorithmModelP.SRow2, XmlControl.sequenceModelNew.algorithmModelP.SColumn2);
                        }
                    }
                }
                else
                {
                    btnSearchRegion.Text = "添加搜索区域";
                    btnSearchRegion.Style = eDotNetBarStyle.StyleManagerControlled;

                    if (tabControl.SelectedTabIndex == 0)
                    {
                        var listValue = m_hsmartWindow.GetRectArea();
                        XmlControl.sequenceModelNew.algorithmModelP.SRow1 = listValue[0];
                        XmlControl.sequenceModelNew.algorithmModelP.SColumn1 = listValue[1];
                        XmlControl.sequenceModelNew.algorithmModelP.SRow2 = listValue[2];
                        XmlControl.sequenceModelNew.algorithmModelP.SColumn2 = listValue[3];

                        m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                        m_hsmartWindow.GetWindowHandle().SetColor("blue");
                        m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                        m_hsmartWindow.GetWindowHandle().DispRectangle1(listValue[0], listValue[1], listValue[2], listValue[3]);

                        m_hsmartWindow.ClearAllObjects();
                    }
                    UpdateData();
                }
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
                switch (tabControl.SelectedTabIndex)
                {
                    case 0:
                        CreateShapeModel_P();
                        break;
                    case 1:
                        CreateShapeModel_N();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;

                    default:
                        break;
                }

                AddLog("创建模板成功");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl.SelectedTabIndex)
                {
                    case 0:
                        if (propertyP.SelectedObject != null)
                        {
                            XmlControl.sequenceModelNew.algorithmModelP = propertyP.SelectedObject as AlgorithmModelP;
                        }
                        break;
                    case 1:
                        if (propertyN.SelectedObject != null)
                        {
                            XmlControl.sequenceModelNew.algorithmModelN = propertyN.SelectedObject as AlgorithmModelN;
                        }
                        break;
                    case 2:
                        break;
                    case 3:
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl.SelectedTabIndex)
                {
                    case 0:
                        TestAlgorithm_P();
                        break;
                    case 1:
                        TestAlgorithm_N();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;

                    default:
                        break;
                }

                AddLog("测试算法完成");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        #endregion

        #region 算法操作

        /// <summary>
        /// P面创建模板
        /// </summary>
        private void CreateShapeModel_P()
        {
            try
            {
                HObject ho_Rectangle = null, ho_ProdObject = null;
                HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                HTuple hv_DynThr = null, hv_HysThrMin = null, hv_HysThrMax = null, hv_ProdRegionPosRCA = null,
                       hv_IsProd1 = null, hv_Exception = null;
                hv_Row1 = XmlControl.sequenceModelNew.algorithmModelP.Row1;
                hv_Column1 = XmlControl.sequenceModelNew.algorithmModelP.Column1;
                hv_Row2 = XmlControl.sequenceModelNew.algorithmModelP.Row2;
                hv_Column2 = XmlControl.sequenceModelNew.algorithmModelP.Column2;

                hv_DynThr = XmlControl.sequenceModelNew.algorithmModelP.DynRThr;
                hv_HysThrMin = XmlControl.sequenceModelNew.algorithmModelP.HysThrMin;
                hv_HysThrMax = XmlControl.sequenceModelNew.algorithmModelP.HysThrMax;
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
                GetProdRegionP(m_hsmartWindow.Image, ho_Rectangle, out ho_ProdObject, hv_DynThr, hv_HysThrMin,
                                      hv_HysThrMax, out hv_ProdRegionPosRCA, out hv_IsProd1, out hv_Exception);

                string strpath = GlobalCore.Global.Model3DPath + "//Model";
                if (!Directory.Exists(strpath))
                {
                    Directory.CreateDirectory(strpath);
                }

                HOperatorSet.WriteRegion(ho_ProdObject, strpath + "//P_ProdRegion.hobj");
                HOperatorSet.WriteTuple(hv_ProdRegionPosRCA, strpath + "//P_ProdRegionPosRCA.tup");

                m_hsmartWindow.FitImageToWindow(m_hsmartWindow.Image, ho_ProdObject);
            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());
            }
        }

        /// <summary>
        /// P面执行算法
        /// </summary>
        private void TestAlgorithm_P()
        {
            try
            {
                AlgorithmModelP tModel = XmlControl.sequenceModelNew.algorithmModelP;

                tModel.Image = m_hsmartWindow.Image;
                var resultModel = m_algorithm.Run(tModel, AlgorithmControlType.AlgorithmRun) as PResultModel;

                if (!resultModel.RunResult)
                {
                    AddLog(resultModel.ErrorMessage);
                }

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.DispObj(resultModel.DispObjects);

                AddLog("P面算法执行" + (resultModel.RunResult ? "成功" : "失败"));
            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());
            }
        }

        /// <summary>
        /// N面创建模板
        /// </summary>
        private void CreateShapeModel_N()
        {
            try
            {
                HObject ho_Rectangle = null, ho_ProModelRegion = null;
                HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null, hv_Column2 = null;
                HTuple hv_ProDynThr = null, hv_ProHysMin = null, hv_ProHysMax = null, hv_CloseWH = null, hv_ProModelPosRCA = null, hv_Error = null;

                var tModel = XmlControl.sequenceModelNew.algorithmModelN;
                hv_Row1 = tModel.Row1;
                hv_Column1 = tModel.Column1;
                hv_Row2 = tModel.Row2;
                hv_Column2 = tModel.Column2;
                 
                hv_ProDynThr = tModel.ProDynThr;
                hv_ProHysMin = tModel.ProHysMin;
                hv_ProHysMax = tModel.ProHysMax;
                hv_CloseWH = tModel.CloseWH;
                hv_Error = 1; 

                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
                GetPordRegionN(m_hsmartWindow.Image, ho_Rectangle, out ho_ProModelRegion, hv_ProDynThr,
                              hv_ProHysMin, hv_ProHysMax, hv_CloseWH, out hv_ProModelPosRCA, out hv_Error);

                string strpath = GlobalCore.Global.Model3DPath + "//Model";
                if (!Directory.Exists(strpath))
                {
                    Directory.CreateDirectory(strpath);
                }

                HOperatorSet.WriteRegion(ho_ProModelRegion, strpath + "//N_ProModelRegion.hobj");
                HOperatorSet.WriteTuple(hv_ProModelPosRCA, strpath + "//N_ProModelPosRCA.tup");

                m_hsmartWindow.FitImageToWindow(m_hsmartWindow.Image, ho_ProModelRegion);
            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());
            }
        }

        /// <summary>
        /// N面执行算法
        /// </summary>
        private void TestAlgorithm_N()
        {
            try
            {
                AlgorithmModelN tModel = XmlControl.sequenceModelNew.algorithmModelN;

                tModel.Image = m_hsmartWindow.Image;
                var resultModel = m_algorithm.Run(tModel, AlgorithmControlType.AlgorithmRun) as NResultModel;

                if (!resultModel.RunResult)
                {
                    AddLog(resultModel.ErrorMessage);
                }

                m_hsmartWindow.GetWindowHandle().SetDraw("margin");
                m_hsmartWindow.GetWindowHandle().SetColor("red");
                m_hsmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hsmartWindow.DispObj(resultModel.DispObjects);

                AddLog("N面算法执行" + (resultModel.RunResult ? "成功" : "失败"));
            }
            catch (Exception ex)
            {
                AddLog(ex.ToString());
            }
        }
        #endregion

        #region 界面操作
        
        /// <summary>
        /// 算法切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            try
            {
                UpdateData();
                btnSearchRegion.Enabled = tabControl.SelectedTabIndex == 0;
            }
            catch (Exception ex)
            {

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
                        var model = XmlControl.sequenceModelNew.algorithmModelP;
                        if (model != null)
                        {
                            propertyP.SelectedObject = model;
                        }
                        break;

                    case 1:
                        var model2 = XmlControl.sequenceModelNew.algorithmModelN;
                        if (model2 != null)
                        {
                            propertyN.SelectedObject = model2;
                        }
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 算法实现

        //P面
        public void GetProdRegionP(HObject ho_Image, HObject ho_Rectangle, out HObject ho_ProdObject,
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

        //N面
        public void GetPordRegionN(HObject ho_Image, HObject ho_Rectangle, out HObject ho_ProModelRegion,
                                 HTuple hv_ProDynThr, HTuple hv_ProHysMin, HTuple hv_ProHysMax, HTuple hv_CloseWH,
                                 out HTuple hv_ProModelPosRCA, out HTuple hv_Error)
        {
            // Local iconic variables  
            HObject ho_ImageReduced = null, ho_GrayImage = null;
            HObject ho_ImageMean = null, ho_RegionDynThresh = null, ho_ImageMean1 = null;
            HObject ho_RegionHysteresis = null, ho_ObjectsConcat = null;
            HObject ho_RegionUnion = null, ho_RegionClosing = null, ho_RegionFillUp = null;
            HObject ho_RegionOpening = null, ho_ConnectedRegions = null;
            HObject ho_RegionTrans = null;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ProModelRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_RegionHysteresis);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            hv_ProModelPosRCA = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    ho_ProModelRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ProModelRegion);
                    hv_ProModelPosRCA = new HTuple();
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_ImageReduced, out ho_GrayImage);
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_GrayImage, out ho_ImageMean, 500, 500);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_GrayImage, ho_ImageMean, out ho_RegionDynThresh,
                        hv_ProDynThr, "light");
                    ho_ImageMean1.Dispose();
                    HOperatorSet.MeanImage(ho_GrayImage, out ho_ImageMean1, 15, 15);
                    ho_RegionHysteresis.Dispose();
                    HOperatorSet.HysteresisThreshold(ho_ImageMean1, out ho_RegionHysteresis,
                        hv_ProHysMin, hv_ProHysMax, 100);
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionHysteresis, ho_RegionDynThresh, out ho_ObjectsConcat
                        );
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing, hv_CloseWH,
                        hv_CloseWH);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, 100,
                        50);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                    ho_ProModelRegion.Dispose();
                    HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_ProModelRegion, "max_area",
                        70);

                    ho_RegionTrans.Dispose();
                    HOperatorSet.ShapeTrans(ho_ProModelRegion, out ho_RegionTrans, "rectangle2");
                    HOperatorSet.SmallestRectangle2(ho_RegionTrans, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    hv_ProModelPosRCA = new HTuple();
                    hv_ProModelPosRCA = hv_ProModelPosRCA.TupleConcat(hv_Row);
                    hv_ProModelPosRCA = hv_ProModelPosRCA.TupleConcat(hv_Column);
                    hv_ProModelPosRCA = hv_ProModelPosRCA.TupleConcat(hv_Phi);

                    hv_Error = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ImageReduced.Dispose();
                ho_GrayImage.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_GrayImage.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_ImageMean1.Dispose();
                ho_RegionHysteresis.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        #endregion

        private void AddLog(string strLog)
        {
            logView1.LogMessage(strLog);
        }
        
    }
}
