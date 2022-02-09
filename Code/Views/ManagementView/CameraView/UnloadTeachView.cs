using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconView;
using MotionController;
using SequenceTestModel;
using HalconDotNet;
using VisionController;
using CameraContorller;
using System.IO;
using GlobalCore;

namespace ManagementView
{
    public partial class UnloadTeachView : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();
        IMotorControl m_MotroContorl;

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;

        /// <summary>
        /// 执行算法Func
        /// </summary>
        public Func<HObject, AlgorithmModel, AlgorithmResultModel> m_FuncAlgorithm;
        public UnloadTeachView()
        {
            InitializeComponent();
        }

        private void UnloadTeachView_Load(object sender, EventArgs e)
        {
            try
            {
                cmbDownSelect.Items.AddRange(new List<object>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.ToArray());
                cmbDownSelect.SelectedIndex = 0;

                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
                cmbSuck.Items.Clear();
                cmbSuck.Items.Add("吸嘴3");
                cmbSuck.Items.Add("吸嘴1");
                cmbSuck.SelectedIndex = 0;

                cmbCamera.Items.Add("下料相机");
                //cmbCamera.Items.Add("治具相机");
                cmbCamera.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnGetTeachPos_Click(object sender, EventArgs e)
        {
            try
            {
                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                string strPos = string.Format("下料示教拍照位置{0}", cmbDownSelect.Text);
                var pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);

                string stroffset = cmbSuck.SelectedIndex == 0 ? "上相机相对位置3" : "上相机相对位置1";
                var pointOffSet = station.PointModels.FirstOrDefault(x => x.Name == stroffset);//"上相机相对位置"

                string strTeach = string.Format("{0}下料示教位置{1}", cmbSuck.Text, cmbDownSelect.Text);
                var pointModel_Teach = station.PointModels.FirstOrDefault(x => x.Name == strTeach);

                Snap();

                var result = TestAlgorithm();

                if(result == null)
                {
                    return;
                }

                double saveRow = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageRow_3;
                double saveCol = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageCol_3; 

                double offsetRow = result.CenterRow - saveRow;
                double offsetCol = result.CenterColumn - saveCol; 

                //读取标定文件 Row -- Y
                HTuple hv_mat2d, qy, qx;
                HOperatorSet.ReadTuple(Global.Model3DPath + "//Mat2d.tup", out hv_mat2d);
                HOperatorSet.AffineTransPoint2d(hv_mat2d, offsetRow, offsetCol, out qy, out qx);

                double pos_X = pointModel.Pos_X + pointOffSet.Pos_X - qx;
                double pos_Y = pointModel.Pos_Y + pointOffSet.Pos_Y - qy;

                pointModel_Teach.Pos_X = Math.Round(pos_X, 3);
                pointModel_Teach.Pos_Y = Math.Round(pos_Y, 3);

                txtUnLoadX.sText = pointModel_Teach.Pos_X.ToString();
                txtUnLoadY.sText = pointModel_Teach.Pos_Y.ToString();
                txtUnLoadAng.sText = result.CenterPhi.D.ToString("0.000");
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnMovePos_Click(object sender, EventArgs e)
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

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                string strPos = string.Format("下料示教拍照位置{0}", cmbDownSelect.Text);
                pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                strPos = "下料示教拍照位置1";
                pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                AddLog("移动到:" + strPos);
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
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

        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                Snap();
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
                TestAlgorithm();
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        
        private AlgorithmResultModel TestAlgorithm()
        {
            try
            {
                //执行算法 
                var algorithmModel = XMLController.XmlControl.sequenceModelNew.algorithmModels.FirstOrDefault(x => x.Name == cmbCamera.Text);
                if (m_FuncAlgorithm == null)
                {
                    return null;
                }
                var algorithmResult = m_FuncAlgorithm(m_hSmartWindow.Image, algorithmModel);
                if (!algorithmResult.RunResult)
                {
                    AddLog("执行算法失败");
                    return null;
                }

                m_hSmartWindow.GetWindowHandle().SetColor("red");
                m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.ProXLDTrans);
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.CenterCross);

                return algorithmResult;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);

                return null;
            }

        }
        
        private void Snap()
        {
            Camera2DSetModel tModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == 0);

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
