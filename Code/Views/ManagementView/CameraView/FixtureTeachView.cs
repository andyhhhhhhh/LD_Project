using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionController;
using HalconView;
using MotionController;
using SequenceTestModel;
using HalconDotNet;
using CameraContorller;
using System.IO;
using GlobalCore;
using DevComponents.DotNetBar;
using XMLController;

namespace ManagementView
{
    public partial class FixtureTeachView : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();
        IMotorControl m_MotroContorl;
        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
        List<HDrawingObject> m_DrawLine4List = new List<HDrawingObject>(); 
        AlgorithmResultModel m_resultModel = new AlgorithmResultModel();
        IAlgorithmControl m_algorithmControl = new LDAlgorithmControl();

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;
         
        public FixtureTeachView()
        {
            InitializeComponent();
        }

        private void FixtureTeachView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTabIndex = 0;
                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
            }
            catch (Exception ex)
            {
                  
            }
        }
         
        #region 按钮操作

        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                int index = tabControl1.SelectedTabIndex;

                int id = index == 0 ? 1 : 2;
                Camera2DSetModel tModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == id);
                CameraResultModel resultModel = m_FuncCameraSnap(tModel);

                if (resultModel.RunResult)
                {
                    m_hSmartWindow.FitImageToWindow(resultModel.Image as HObject, null);
                }
                else
                {
                    AddLog(resultModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                Stream inputStream = null;
                this.OpenFileDialogImage.Filter = "All files (*.*)|*.*";
                if (this.OpenFileDialogImage.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((inputStream = this.OpenFileDialogImage.OpenFile()) != null)
                        {
                            String strImageFile = this.OpenFileDialogImage.FileName;
                            this.OpenFileDialogImage.InitialDirectory = strImageFile;
                            HObject ho_Image;
                            HOperatorSet.ReadImage(out ho_Image, strImageFile);
                            m_hSmartWindow.FitImageToWindow(new HImage(ho_Image), null);
                            AddLog("读入图像成功。");
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

            }
        }

        private void btnSaveParam_Click(object sender, EventArgs e)
        {
            try
            {
                int id = tabControl1.SelectedTabIndex;
               var model = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    model = new FixtureAlgorithmModel();
                    model.Id = id;
                    XmlControl.sequenceModelNew.fixtureAlgorithmModels.Add(model);
                }

                switch (id)
                {
                    case 0:
                        model = advPropertyGrid1.SelectedObject as FixtureAlgorithmModel;
                        break;

                    case 1:
                        model = advPropertyGrid2.SelectedObject as FixtureAlgorithmModel;
                        break;

                    case 2:
                        model = advPropertyGrid3.SelectedObject as FixtureAlgorithmModel;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        
        private void btnAlgorithm_Click(object sender, EventArgs e)
        {
            try
            {
               m_resultModel = TestAlgorithm(); 
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnGoFixtureSnap_Click(object sender, EventArgs e)
        {
            try
            {
                m_MotroContorl = MotorInstance.GetInstance();

                StationModel station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = station.PointModels.FirstOrDefault(x => x.Name == "安全位");
                var resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }
                string strPos = "积分球1转盘拍照位置";

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);

                pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);

                pointModel = station.PointModels.FirstOrDefault(x => x.Name == "积分球1转盘拍照位置");
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                AddLog("移动到:" + strPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnSetBase_Click(object sender, EventArgs e)
        {
            try
            { 
                var model = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == tabControl1.SelectedTabIndex);
                if(model == null)
                {
                    return;
                }

                model.Row = Math.Round(m_resultModel.CenterRow.D, 3);
                model.Column = Math.Round(m_resultModel.CenterColumn.D, 3);
                model.Angle = Math.Round(m_resultModel.CenterPhi.D, 3);

                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnSavePix_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        #endregion
        
        /// <summary>
        /// 执行算法
        /// </summary>
        /// <returns></returns>
        private AlgorithmResultModel TestAlgorithm()
        {
            AlgorithmResultModel result = new AlgorithmResultModel();
            try
            {
                string paramPath = Global.Model3DPath;
                var algorithmModel = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == tabControl1.SelectedTabIndex);
                algorithmModel.Image = m_hSmartWindow.Image;
                result = m_algorithmControl.Run(algorithmModel, AlgorithmControlType.AlgorithmRun) as AlgorithmResultModel;
                switch (tabControl1.SelectedTabIndex)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }

                string strLog = string.Format("Result:{0} Row:{1} Column:{2} Angle:{3}", result.RunResult, result.CenterRow, result.CenterColumn, result.CenterPhi);
                AddLog(strLog);

                m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                m_hSmartWindow.GetWindowHandle().SetColor("red");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hSmartWindow.GetWindowHandle().DispObj(result.ObjectResult as HObject);

                return result;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// 相机选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void UpdateData()
        {
            try
            {
                FixtureAlgorithmModel model = new FixtureAlgorithmModel();
                switch (tabControl1.SelectedTabIndex)
                {
                    case 0: 
                        model = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 0);
                        if(model == null)
                        {
                            model = new FixtureAlgorithmModel();
                        }
                        advPropertyGrid1.SelectedObject = model;
                        break;

                    case 1:
                        model = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 1);
                        if (model == null)
                        {
                            model = new FixtureAlgorithmModel();
                        }
                        advPropertyGrid2.SelectedObject = model;
                        break;

                    case 2:
                        model = XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == 2);
                        if (model == null)
                        {
                            model = new FixtureAlgorithmModel();
                        }
                        advPropertyGrid3.SelectedObject = model;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// 显示Log
        /// </summary>
        /// <param name="strLog">log内容</param>
        private void AddLog(string strLog)
        {
            logView1.LogMessage(strLog);
        }
    }
}
