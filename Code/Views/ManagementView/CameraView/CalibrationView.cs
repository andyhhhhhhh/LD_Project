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
using SequenceTestModel;
using HalconDotNet;
using System.IO;
using System.Threading;
using MotionController;
using VisionController;
using AlgorithmController;
using CameraContorller;

namespace ManagementView
{
    /// <summary>
    /// 标定界面
    /// </summary>
    public partial class CalibrationView : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();

        /// <summary>
        /// 九点标定矩阵保存路径
        /// </summary>
        string m_SaveMat2dPath = "";

        /// <summary>
        /// 旋转标定矩阵保存路径
        /// </summary>
        string m_RotatePath = "";

        /// <summary>
        /// 开始标定标志位
        /// </summary>
        bool m_StartCal = false;

        IMotorControl m_MotroContorl;

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;

        /// <summary>
        /// 执行算法Func
        /// </summary>
        public Func<HObject, AlgorithmModel, AlgorithmResultModel> m_FuncAlgorithm;

        CalibrationModel m_calibrationModel;

        public CalibrationView()
        {
            InitializeComponent();
        }

        private void CalibrationView_Load(object sender, EventArgs e)
        {
            try
            {
                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
                m_calibrationModel = XMLController.XmlControl.sequenceModelNew.calibrationModel;
                if (m_calibrationModel == null)
                {
                    m_calibrationModel = new CalibrationModel();
                }

                txtImageRow.sText = m_calibrationModel.TeachImageRow.ToString();
                txtImageCol.sText = m_calibrationModel.TeachImageCol.ToString();
                txtImageAng.sText = m_calibrationModel.TeachImageAng.ToString();

                cmbSuck.Items.Clear();
                cmbSuck.Items.Add("吸嘴1");
                cmbSuck.Items.Add("吸嘴2");
                cmbSuck.Items.Add("吸嘴3");
                cmbSuck.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlgorithm_Click(object sender, EventArgs e)
        {
            try
            {
                //执行算法 
                var algorithmModel = XMLController.XmlControl.sequenceModelNew.algorithmModels.FirstOrDefault(x => x.Name == "上相机");
                if (m_FuncAlgorithm == null)
                {
                    return;
                }
                var algorithmResult = m_FuncAlgorithm(m_hSmartWindow.Image, algorithmModel);
                if (!algorithmResult.RunResult)
                {
                    AddLog("执行算法失败");
                    return;
                }

                m_hSmartWindow.GetWindowHandle().SetColor("red");
                m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.ProXLDTrans);
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.CenterCross);
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        /// <summary>
        /// 开始九点标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartCal_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_StartCal)
                {
                    AddLog("程序已经在标定中...");
                    return;
                }

                var result = MessageBox.Show("请确认是否开始九点标定?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                m_StartCal = true;

                m_SaveMat2dPath = GlobalCore.Global.Model3DPath + "//Mat2d.tup";

                Camera2DSetModel cameraModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == 0);

                var algorithmModel = XMLController.XmlControl.sequenceModelNew.algorithmModels.FirstOrDefault(x => x.Name == cameraModel.Name);

                StationModel station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                PointModel pointModel = station.PointModels.FirstOrDefault(x => x.Name == "九点标定起始点位");
                double xStart = pointModel.Pos_X;
                double yStart = pointModel.Pos_Y;
                double xSpan = 4;
                double ySpan = 3;

                m_MotroContorl = MotorInstance.GetInstance();
                NineCalibration(cameraModel, algorithmModel, m_SaveMat2dPath, xStart, yStart, xSpan, ySpan, false, true);
            }
            catch (Exception ex)
            {
                m_StartCal = false;
                AddLog(ex.Message);
            }
        }

        /// <summary>
        /// 停止九点标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopCal_Click(object sender, EventArgs e)
        {
            m_StartCal = false;
        }

        /// <summary>
        /// 九点标定方法
        /// </summary>
        /// <param name="cameraModel"></param>
        /// <param name="algorithmModel"></param>
        /// <param name="mat2dPath"></param>
        /// <param name="xStart"></param>
        /// <param name="yStart"></param>
        /// <param name="xSpan"></param>
        /// <param name="ySpan"></param>
        /// <param name="isRowToX"></param>
        /// <param name="isReleativeCal"></param>
        private void NineCalibration(Camera2DSetModel cameraModel, AlgorithmModel algorithmModel, string mat2dPath, double xStart, double yStart, double xSpan, double ySpan,
            bool isRowToX, bool isReleativeCal)
        {
            string dir = mat2dPath.Substring(0, mat2dPath.LastIndexOf("\\"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //默认就是9点 
            //组合N个点位（4点或9点）  
            List<double> xPositionList = new List<double>()
                {
                    xStart, xStart + xSpan, xStart + xSpan +xSpan,
                    xStart + xSpan + xSpan, xStart + xSpan, xStart,
                    xStart, xStart + xSpan, xStart + xSpan + xSpan
                };
            List<double> yPositionList = new List<double>()
                {
                    yStart, yStart, yStart,
                    yStart + ySpan, yStart + ySpan, yStart + ySpan,
                    yStart + ySpan + ySpan,  yStart + ySpan + ySpan,  yStart + ySpan + ySpan
                };

            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    AddLog(cameraModel.Name + "  开始进行九点标定...");

                    List<CalibPositionModel> listCaliPosModel = new List<CalibPositionModel>();
                    for (int i = 0; i < 9; i++)
                    {
                        if (!m_StartCal)
                        {
                            AddLog("停止标定");
                            return;
                        }

                        //开始走标定位置、 
                        StationModel station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                        PointModel pointModel = station.PointModels.FirstOrDefault(x => x.Name == "九点标定点位");
                        pointModel.Pos_X = xPositionList[i];
                        pointModel.Pos_Y = yPositionList[i];
                        var resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                        if (!resultModel.RunResult)
                        {
                            AddLog("移动失败");
                            m_StartCal = false;
                            return;
                        }

                        //拍照
                        var cameraResult = m_FuncCameraSnap(cameraModel);
                        if (!cameraResult.RunResult)
                        {
                            AddLog("拍照失败:" + cameraResult.ErrorMessage);
                            m_StartCal = false;
                            return;
                        }
                        HObject ho_Image = cameraResult.Image as HObject;
                        m_hSmartWindow.FitImageToWindow(ho_Image, null);

                        //执行算法
                        var algorithmResult = m_FuncAlgorithm(ho_Image, algorithmModel);
                        if (!algorithmResult.RunResult)
                        {
                            AddLog("执行算法失败");
                            m_StartCal = false;
                            return;
                        }
                        m_hSmartWindow.GetWindowHandle().SetColor("red");
                        m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                        m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.ProXLDTrans);
                        m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.CenterCross);

                        //保存图片
                        string strPath = "D://CaliImage//" + cameraModel.Name;
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }
                        AlgorithmCommHelper.SaveImage(ho_Image, strPath, i.ToString());

                        //存储像素跟物理坐标
                        CalibPositionModel calibPos = new CalibPositionModel()
                        {
                            MachineX = xPositionList[i],
                            MachineY = yPositionList[i],
                            ImageRow = algorithmResult.CenterRow,
                            ImageCol = algorithmResult.CenterColumn,
                        };

                        listCaliPosModel.Add(calibPos);
                    }

                    //保存标定文件
                    SaveCoordinateTupleFiles(listCaliPosModel, cameraModel.Name, mat2dPath, isRowToX, isReleativeCal);
                    m_StartCal = false;
                }
                catch (Exception ex)
                {
                    AddLog(ex.Message);
                    m_StartCal = false;
                }
            }));
            t.Start();
        }

        /// <summary>
        /// 保存标定文件
        /// </summary>
        /// <param name="carlPositions">标定点位</param>
        /// <param name="cameraName">相机名称</param>
        /// <param name="mat2dPath">矩阵路径</param>
        /// <param name="isRowToX">Row 对应 X</param>
        /// <param name="isReleativeCal">是否为相对位置标定</param>
        private void SaveCoordinateTupleFiles(List<CalibPositionModel> carlPositions, string cameraName, string mat2dPath, bool isRowToX, bool isReleativeCal)
        {
            try
            {
                HTuple imgRow = new HTuple();
                HTuple imgCol = new HTuple();
                HTuple machineX = new HTuple();
                HTuple machineY = new HTuple();

                string machineXPositionStr = "";
                string machineYPositionStr = "";
                string imgRowPositionStr = "";
                string imgColPositionStr = "";
                foreach (CalibPositionModel item in carlPositions)
                {
                    machineX.Append(item.MachineX);
                    machineY.Append(item.MachineY);
                    imgRow.Append(item.ImageRow);
                    imgCol.Append(item.ImageCol);
                    machineXPositionStr += item.MachineX + ",";
                    machineYPositionStr += item.MachineY + ",";
                    imgRowPositionStr += item.ImageRow + ",";
                    imgColPositionStr += item.ImageCol + ",";
                }

                //如果是相对位置标定
                if (isReleativeCal)
                {
                    machineXPositionStr = "";
                    machineYPositionStr = "";
                    imgRowPositionStr = "";
                    imgColPositionStr = "";

                    double machineX0 = machineX[0];
                    double machineY0 = machineY[0];
                    double imgRow0 = imgRow[0];
                    double imgCol0 = imgCol[0];
                    //计算相对坐标
                    for (int i = 0; i < machineX.Length; i++)
                    {
                        machineX[i] = machineX[i] - machineX0;
                        machineY[i] = machineY[i] - machineY0;
                        imgRow[i] = imgRow[i] - imgRow0;
                        imgCol[i] = imgCol[i] - imgCol0;

                        machineXPositionStr += machineX[i] + ",";
                        machineYPositionStr += machineY[i] + ",";
                        imgRowPositionStr += imgRow[i] + ",";
                        imgColPositionStr += imgCol[i] + ",";
                    }
                }

                AddLog(string.Format(cameraName + ": machineX:=[{0}] \r\n machineY:=[{1}] \r\n imageX:=[{2}] \r\n imageY:=[{3}]",
                    machineXPositionStr, machineYPositionStr, imgRowPositionStr, imgColPositionStr));


                //生成矩阵
                HTuple Mat2D;
                if (isRowToX)
                {
                    HOperatorSet.VectorToHomMat2d(imgRow, imgCol, machineX, machineY, out Mat2D);
                }
                else
                {
                    //这里交换图像的row，col，表示row对应物理Y，col对应X
                    HOperatorSet.VectorToHomMat2d(imgRow, imgCol, machineY, machineX, out Mat2D);
                }

                string path = mat2dPath;
                HOperatorSet.WriteTuple(Mat2D, path);
                HTuple sx, sy, phi, theta, tx, ty;
                HOperatorSet.HomMat2dToAffinePar(Mat2D, out sx, out sy, out phi, out theta, out tx, out ty);
                if (sx != null && sx.Length > 0)
                {
                    AddLog(string.Format("sx:{0},sy:{1}", sx.D.ToString("0.0000"), sy.D.ToString("0.0000")));
                }
                else
                {
                    AddLog("标定坐标失败，请确保获取的坐标组均为有效值！");
                }
            }
            catch (Exception ex)
            {
                AddLog("标定坐标失败，请确保获取的坐标组均为有效值！" + ex.Message.ToString());
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

        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
        private void btnLoadImage_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSuck.SelectedIndex == 0)
                {
                    m_calibrationModel.TeachImageRow = Double.Parse(txtImageRow.sText);
                    m_calibrationModel.TeachImageCol = Double.Parse(txtImageCol.sText);
                    m_calibrationModel.TeachImageAng = Double.Parse(txtImageAng.sText);
                }
                else if (cmbSuck.SelectedIndex == 1)
                {
                    m_calibrationModel.TeachImageRow_2 = Double.Parse(txtImageRow.sText);
                    m_calibrationModel.TeachImageCol_2 = Double.Parse(txtImageCol.sText);
                    m_calibrationModel.TeachImageAng_2 = Double.Parse(txtImageAng.sText);
                }
                else if (cmbSuck.SelectedIndex == 2)
                {
                    m_calibrationModel.TeachImageRow_3 = Double.Parse(txtImageRow.sText);
                    m_calibrationModel.TeachImageCol_3 = Double.Parse(txtImageCol.sText);
                    m_calibrationModel.TeachImageAng_3 = Double.Parse(txtImageAng.sText);
                }

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            try
            {
                //执行算法 
                var algorithmModel = XMLController.XmlControl.sequenceModelNew.algorithmModels.FirstOrDefault(x => x.Name == "上相机");
                var algorithmResult = m_FuncAlgorithm(m_hSmartWindow.Image, algorithmModel);
                if (!algorithmResult.RunResult)
                {
                    AddLog("执行算法失败");
                    return;
                }

                txtImageRow.sText = Math.Round(algorithmResult.CenterRow.D, 3).ToString();
                txtImageCol.sText = Math.Round(algorithmResult.CenterColumn.D, 3).ToString();
                txtImageAng.sText = Math.Round(algorithmResult.CenterPhi.D, 3).ToString();
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        /// <summary>
        /// 吸嘴选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSuck_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                if (cmbSuck.SelectedIndex == 0)
                {
                    txtImageRow.sText = m_calibrationModel.TeachImageRow.ToString();
                    txtImageCol.sText = m_calibrationModel.TeachImageCol.ToString();
                    txtImageAng.sText = m_calibrationModel.TeachImageAng.ToString();
                }
                else if (cmbSuck.SelectedIndex == 1)
                {
                    txtImageRow.sText = m_calibrationModel.TeachImageRow_2.ToString();
                    txtImageCol.sText = m_calibrationModel.TeachImageCol_2.ToString();
                    txtImageAng.sText = m_calibrationModel.TeachImageAng_2.ToString();
                }
                else if (cmbSuck.SelectedIndex == 2)
                {
                    txtImageRow.sText = m_calibrationModel.TeachImageRow_3.ToString();
                    txtImageCol.sText = m_calibrationModel.TeachImageCol_3.ToString();
                    txtImageAng.sText = m_calibrationModel.TeachImageAng_3.ToString(); 
                }

                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                string strRelPos = "上相机相对位置" + (cmbSuck.SelectedIndex + 1).ToString();
                var relPosModel = station.PointModels.FirstOrDefault(x => x.Name == strRelPos);
                txtXOffSet.sText = relPosModel.Pos_X.ToString();
                txtYOffSet.sText = relPosModel.Pos_Y.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        #region 暂时无用 -- 求旋转中心
        /// <summary>
        /// 求旋转中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRotateCenter_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_StartCal)
                {
                    AddLog("程序已经在标定中...");
                    return;
                }

                var result = MessageBox.Show("请确认是否开始旋转中心标定?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                m_StartCal = true;

                m_RotatePath = GlobalCore.Global.Model3DPath + "//" + cmbSuck.Text + "_Rotate.tup";


                m_MotroContorl = MotorInstance.GetInstance();

                StationModel station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = station.PointModels.FirstOrDefault(x => x.Name == "安全位");
                var resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                pointModel = station.PointModels.FirstOrDefault(x => x.Name == "吸嘴1下相机拍照位置");
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }

                station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                pointModel = station.PointModels.FirstOrDefault(x => x.Name == "吸嘴1下相机拍照位置");
                resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }
                RotaryThread();
            }
            catch (Exception ex)
            {
                m_StartCal = false;
                AddLog(ex.Message);
            }
        }

        private void RotaryThread()
        {
            Camera2DSetModel cameraModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == 1);
            var algorithmModel = XMLController.XmlControl.sequenceModelNew.algorithmModels.FirstOrDefault(x => x.Name == cameraModel.Name);
            m_MotroContorl = MotorInstance.GetInstance();

            var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);
            var pointModel = station.PointModels.FirstOrDefault(x => x.Name == "吸嘴1下相机拍照位置");

            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            for (int i = 0; i < 6; i++)
            {
                var resultModel = m_MotroContorl.Run(pointModel, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {
                    return;
                }
                Thread.Sleep(100);
                //拍照
                var cameraResult = m_FuncCameraSnap(cameraModel);
                if (!cameraResult.RunResult)
                {
                    AddLog("拍照失败:" + cameraResult.ErrorMessage);
                    m_StartCal = false;
                    return;
                }
                HObject ho_Image = cameraResult.Image as HObject;
                m_hSmartWindow.FitImageToWindow(ho_Image, null);

                //执行算法
                var algorithmResult = m_FuncAlgorithm(ho_Image, algorithmModel);
                if (!algorithmResult.RunResult)
                {
                    AddLog("执行算法失败");
                    m_StartCal = false;
                    return;
                }
                listX.Add(algorithmResult.CenterRow);
                listY.Add(algorithmResult.CenterColumn);

                m_hSmartWindow.GetWindowHandle().SetColor("red");
                m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.ProXLDTrans);
                m_hSmartWindow.GetWindowHandle().DispObj(algorithmResult.CenterCross);
                pointModel.Pos_X += 30;
            }

            HObject ho_Contour1 = null;
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            ho_Contour1.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour1, listX.ToArray(), listY.ToArray());

            HTuple hv_RowX, hv_ColumnY, hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
            HOperatorSet.FitCircleContourXld(ho_Contour1, "algebraic", -1, 0, 0, 3, 2, out hv_RowX,
                out hv_ColumnY, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);

            //保存图像的 旋转中心  
            HTuple hv_ModelParam = new HTuple();
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_RowX);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_ColumnY);
            hv_ModelParam = hv_ModelParam.TupleConcat(hv_Radius);

            AddLog(string.Format("X:{0} Y:{1} Radius:{2}", hv_RowX, hv_ColumnY, hv_Radius));

            HOperatorSet.WriteTuple(hv_ModelParam, m_RotatePath);
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

        #endregion

        private void btnGetSuckCamera_Click(object sender, EventArgs e)
        {
            try
            {
                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
             
                string strTeachPos = cmbSuck.Text + "上料示教";
                var teachPosModel = station.PointModels.FirstOrDefault(x => x.Name == strTeachPos);

                var result = MessageBox.Show("请确认是否运动到拍照位置?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                double xValue = 0;
                double yValue = 0;

                m_MotroContorl = MotorInstance.GetInstance();
                var resultModel =  m_MotroContorl.Run(station.Axis_X, MotorControlType.AxisGetPosition);
                if(!resultModel.RunResult)
                {
                    xValue = Double.Parse(resultModel.ObjectResult.ToString());
                    MessageBox.Show("获取X位置错误");
                    return;
                }
                resultModel = m_MotroContorl.Run(station.Axis_Y, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    yValue = Double.Parse(resultModel.ObjectResult.ToString());
                    MessageBox.Show("获取Y位置错误");
                    return;
                }

                txtXOffSet.sText = (teachPosModel.Pos_X - xValue).ToString();
                txtYOffSet.sText = (teachPosModel.Pos_Y - yValue).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveSuckCamera_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("是否保存相对位置?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);

                string strRelPos = "上相机相对位置" + (cmbSuck.SelectedIndex + 1).ToString();
                var relPosModel = station.PointModels.FirstOrDefault(x => x.Name == strRelPos);

                double xValue = Double.Parse(txtXOffSet.sText);
                double yValue = Double.Parse(txtYOffSet.sText);

                if(xValue < 30 || xValue > 80 || yValue < -60 || yValue > 60)
                {
                    MessageBox.Show("数值有误，请确认!!");
                    return;
                }

                relPosModel.Pos_X = xValue;
                relPosModel.Pos_Y = yValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    /// <summary>
    /// 标定数据类
    /// </summary>
    public class CalibPositionModel
    {
        /// <summary>
        /// 物理坐标X
        /// </summary>
        public double MachineX { get; set; }
        /// <summary>
        /// 物理坐标Y
        /// </summary>
        public double MachineY { get; set; }
        /// <summary>
        /// 图像Row
        /// </summary>
        public double ImageRow { get; set; }
        /// <summary>
        /// 图像Column
        /// </summary>
        public double ImageCol { get; set; }
    }

}
