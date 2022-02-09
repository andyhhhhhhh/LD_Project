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
                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
                cmbFixture.Items.Add("1");
                cmbFixture.Items.Add("2");
                cmbFixture.Items.Add("3");
                cmbFixture.Items.Add("4");
                cmbFixture.SelectedIndex = 0;

                cmbCamera.Items.Add("治具相机");
                cmbCamera.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                  
            }
        }

        #region 治具拍照示教位置
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
                if (cmbFixture.Text == "3" || cmbFixture.Text == "4")
                {
                    strPos = "积分球2转盘拍照位置";
                }

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

        private void btnGetPos_Click_1(object sender, EventArgs e)
        {
            try
            {
                string strPos = "积分球1转盘拍照位置";
                bool bSphere1 = true;
                if (cmbFixture.Text == "3" || cmbFixture.Text == "4")
                {
                    strPos = "积分球2转盘拍照位置";
                    bSphere1 = false;
                }
                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                var pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);

                string stroffset = bSphere1 ? "上相机相对位置3" : "上相机相对位置1";
                var pointOffSet = station.PointModels.FirstOrDefault(x => x.Name == stroffset);//"上相机相对位置"

                string strTeach = string.Format("取测试料位置{0}", cmbFixture.Text);
                var pointModel_Teach = station.PointModels.FirstOrDefault(x => x.Name == strTeach);

                Snap();

                var result = TestAlgorithm();

                if (result == null)
                {
                    return;
                }


                double saveRow = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageRow_3;
                double saveCol = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageCol_3;
                if (!bSphere1)
                {
                    saveRow = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageRow;
                    saveCol = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageCol;
                }

                double offsetRow = result.CenterRow - saveRow;
                double offsetCol = result.CenterColumn - saveCol;

                //读取标定文件 Row -- Y
                HTuple hv_mat2d, qy, qx;
                HOperatorSet.ReadTuple(Global.Model3DPath + "//Mat2d.tup", out hv_mat2d);
                HOperatorSet.AffineTransPoint2d(hv_mat2d, offsetRow, offsetCol, out qy, out qx);

                double pos_X = pointModel.Pos_X + pointOffSet.Pos_X - qx;
                double pos_Y = pointModel.Pos_Y + pointOffSet.Pos_Y - qy;

                txtTeachX.sText = pointModel_Teach.Pos_X.ToString();
                txtTeachY.sText = pointModel_Teach.Pos_Y.ToString();
                txtTeachAngle.sText = result.CenterPhi.D.ToString("0.000");

                var resultD = MessageBox.Show("确定保存点位: " + pointModel_Teach.Name + "?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (resultD == DialogResult.Yes)
                {
                    pointModel_Teach.Pos_X = Math.Round(pos_X, 3);
                    pointModel_Teach.Pos_Y = Math.Round(pos_Y, 3);
                }
                else
                {
                    MessageBox.Show("取消操作");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSetPos_Click_1(object sender, EventArgs e)
        {
            try
            {
                string strPos = "积分球1转盘拍照位置";
                bool bSphere1 = true;
                if (cmbFixture.Text == "3" || cmbFixture.Text == "4")
                {
                    strPos = "积分球2转盘拍照位置";
                    bSphere1 = false;
                }
                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                var pointModel = station.PointModels.FirstOrDefault(x => x.Name == strPos);

                string stroffset = bSphere1 ? "上相机相对位置1" : "上相机相对位置2";
                var pointOffSet = station.PointModels.FirstOrDefault(x => x.Name == stroffset);//"上相机相对位置"

                string strTeach = string.Format("放料位置{0}", cmbFixture.Text);
                var pointModel_Teach = station.PointModels.FirstOrDefault(x => x.Name == strTeach);

                Snap();

                var result = TestAlgorithm();

                if (result == null)
                {
                    return;
                }

                double saveRow = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageRow;
                double saveCol = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageCol;
                if (!bSphere1)
                {
                    saveRow = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageRow_2;
                    saveCol = XMLController.XmlControl.sequenceModelNew.calibrationModel.TeachImageCol_2;
                }

                double offsetRow = result.CenterRow - saveRow;
                double offsetCol = result.CenterColumn - saveCol;

                //读取标定文件 Row -- Y
                HTuple hv_mat2d, qy, qx;
                HOperatorSet.ReadTuple(Global.Model3DPath + "//Mat2d.tup", out hv_mat2d);
                HOperatorSet.AffineTransPoint2d(hv_mat2d, offsetRow, offsetCol, out qy, out qx);

                double pos_X = pointModel.Pos_X + pointOffSet.Pos_X - qx;
                double pos_Y = pointModel.Pos_Y + pointOffSet.Pos_Y - qy;

                txtTeachX.sText = pointModel_Teach.Pos_X.ToString();
                txtTeachY.sText = pointModel_Teach.Pos_Y.ToString();
                txtTeachAngle.sText = result.CenterPhi.D.ToString("0.000");

                var resultD = MessageBox.Show("确定保存点位: " + pointModel_Teach.Name + "?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (resultD == DialogResult.Yes)
                {
                    pointModel_Teach.Pos_X = Math.Round(pos_X, 3);
                    pointModel_Teach.Pos_Y = Math.Round(pos_Y, 3);
                }
                else
                {
                    MessageBox.Show("取消操作");
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnGetPos_Click(object sender, EventArgs e)
        {
            try
            {
                var station = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                var stationR1 = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);
                var stationR2 = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);
                var stationR3 = XMLController.XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);

                string strTeach = string.Format("取测试料位置{0}", cmbFixture.Text);
                var pointModel_Teach = station.PointModels.FirstOrDefault(x => x.Name == strTeach);

                string strSet = string.Format("放料位置{0}", cmbFixture.Text);
                var pointModel_Set = station.PointModels.FirstOrDefault(x => x.Name == strSet);

                Snap();

                var result = TestAlgorithm();

                if (!result.RunResult)
                {
                    AddLog("算法执行失败");
                    return;
                }

                var fixtureModel = XmlControl.sequenceModelNew.fixtureTeachModels.FirstOrDefault(x => x.Id == Int32.Parse(cmbFixture.Text));

                double saveRow = fixtureModel.TeachImageRow;
                double saveCol = fixtureModel.TeachImageCol;
                double saveAng = fixtureModel.TeachImageAng;

                double offsetRow = result.CenterRow - saveRow;
                double offsetCol = result.CenterColumn - saveCol;
                double offsetAng = result.CenterPhi - saveAng;

                //读取标定文件 Row -- Y
                HTuple hv_mat2d, qy, qx;
                HOperatorSet.ReadTuple(Global.Model3DPath + "//Mat2d.tup", out hv_mat2d);
                HOperatorSet.AffineTransPoint2d(hv_mat2d, offsetRow, offsetCol, out qy, out qx);

                double pos_X = pointModel_Teach.Pos_X - qx;
                double pos_Y = pointModel_Teach.Pos_Y - qy;

                double set_X = pointModel_Set.Pos_X - qx;
                double set_Y = pointModel_Set.Pos_Y - qy;

                txtTeachX.sText = pos_X.ToString();
                txtTeachY.sText = pos_Y.ToString();

                txtSetX.sText = set_X.ToString();
                txtSetY.sText = set_Y.ToString();

                txtCenterRow.sText = result.CenterRow.D.ToString("0.000");
                txtCenterAngle.sText = result.CenterColumn.D.ToString("0.000");
                txtCenterAngle.sText = result.CenterPhi.D.ToString("0.000");

                if(Math.Abs(qx.D) > 5 || Math.Abs(qy.D) > 5 || Math.Abs(offsetAng) > 5)
                {
                    MessageBoxEx.Show(string.Format("示教偏差X:{0} Y:{1} R:{2} 过大，请检查!!", qx, qy, offsetAng));
                    return;
                }

                var resultD = MessageBoxEx.Show("确定保存点位: " + pointModel_Teach.Name + "?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (resultD == DialogResult.Yes)
                {
                    pointModel_Teach.Pos_X = Math.Round(pos_X, 3);
                    pointModel_Teach.Pos_Y = Math.Round(pos_Y, 3);

                    pointModel_Set.Pos_X = Math.Round(set_X, 3);
                    pointModel_Set.Pos_Y = Math.Round(set_Y, 3);

                    //设置R轴位置
                    int fixtureId = Int32.Parse(cmbFixture.Text);
                    switch(fixtureId)
                    {
                        case 1:
                        case 2:
                            var pointModel_TeachR = stationR3.PointModels.FirstOrDefault(x => x.Name == strTeach);
                            var pointModel_SetR = stationR1.PointModels.FirstOrDefault(x => x.Name == strSet);
                            pointModel_TeachR.Pos_X += offsetAng;
                            pointModel_SetR.Pos_X += offsetAng;
                            break;
                        case 3:
                        case 4:
                            var pointModel_TeachR2 = stationR1.PointModels.FirstOrDefault(x => x.Name == strTeach);
                            var pointModel_SetR2 = stationR2.PointModels.FirstOrDefault(x => x.Name == strSet);
                            pointModel_TeachR2.Pos_X += offsetAng;
                            pointModel_SetR2.Pos_X += offsetAng;
                            break;

                        default:
                            break;
                    }

                    fixtureModel.TeachImageRow = result.CenterRow.D;
                    fixtureModel.TeachImageCol = result.CenterColumn.D;
                    fixtureModel.TeachImageAng = result.CenterPhi.D;
                }
                else
                {
                    MessageBox.Show("取消操作");
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
         
        
        private void btnSavePix_Click(object sender, EventArgs e)
        {
            try
            {
                var fixtureModel = XmlControl.sequenceModelNew.fixtureTeachModels.FirstOrDefault(x => x.Id == Int32.Parse(cmbFixture.Text));

                if(fixtureModel == null)
                {
                    fixtureModel = new FixtureTeachModel();
                    XmlControl.sequenceModelNew.fixtureTeachModels.Add(fixtureModel);
                }

                var result = MessageBoxEx.Show(this, "确认保存像素信息？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.No)
                {
                    return;
                }

                fixtureModel.Id = Int32.Parse(cmbFixture.Text);
                fixtureModel.TeachImageRow = Double.Parse(txtCenterRow.sText);
                fixtureModel.TeachImageCol = Double.Parse(txtCenterCol.sText);
                fixtureModel.TeachImageAng = Double.Parse(txtCenterAngle.sText);
                AddLog("保存成功");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        #endregion

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
        
        private void Snap()
        {
            Camera2DSetModel tModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == 0);
            //int exposureTime = tModel.ExposureTime;
            //tModel.ExposureTime = numExposureTime.Value;
            CameraResultModel resultModel = m_FuncCameraSnap(tModel);
            //tModel.ExposureTime = 4000;

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

        List<HDrawingObject> m_DrawLine4List = new List<HDrawingObject>();
        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            try
            {
                string path_startRow = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineStartRow" + cmbFixture.Text;
                string path_startCol = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineStartCol" + cmbFixture.Text;
                string path_endRow = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineEndRow" + cmbFixture.Text;
                string path_endCol = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineEndCol" + cmbFixture.Text;
                if (File.Exists(path_startRow) && File.Exists(path_startCol) && File.Exists(path_endRow) && File.Exists(path_endCol))
                {
                    HTuple hv_startRow, hv_startCol, hv_endRow, hv_endCol;
                    HOperatorSet.ReadTuple(path_startRow, out hv_startRow);
                    HOperatorSet.ReadTuple(path_startCol, out hv_startCol);
                    HOperatorSet.ReadTuple(path_endRow, out hv_endRow);
                    HOperatorSet.ReadTuple(path_endCol, out hv_endCol);

                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(hv_endRow[0], hv_endCol[0], hv_startRow[0], hv_startCol[0]));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(hv_endRow[1], hv_endCol[1], hv_startRow[1], hv_startCol[1]));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(hv_endRow[2], hv_endCol[2], hv_startRow[2], hv_startCol[2]));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(hv_endRow[3], hv_endCol[3], hv_startRow[3], hv_startCol[3]));
                }
                else
                {
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(1337, 1820, 2679, 1873));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(1252, 3554, 1347, 1831));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(2563, 3628, 1263, 3554));
                    m_DrawLine4List.Add(m_hSmartWindow.AddLine(2701, 1884, 2613, 3626));
                }

                AddLog("增加绘制边线成功！");
            }
            catch (Exception ex)
            {
            }
        }
        
        private void btnSaveParam_Click(object sender, EventArgs e)
        {
            try
            {
                FixtureAlgorithmModel tModel = XMLController.XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == cmbFixture.SelectedIndex + 1);
                if(tModel == null)
                {
                    tModel = new FixtureAlgorithmModel();
                    XMLController.XmlControl.sequenceModelNew.fixtureAlgorithmModels.Add(tModel);
                }

                tModel.Id = cmbFixture.SelectedIndex + 1;
                tModel.Name = cmbCamera.Text;
                tModel.InMeasureLength1 = numLength1.Value;
                tModel.InMeasureLength2 = numLength2.Value;
                tModel.InMeasureNumber = numCount.Value;
                tModel.InMeasureScore = numSocre.Value;
                tModel.InMeasureSelect = cmbSelect.Text;
                tModel.InMeasureSigma = numSigma.Value;
                tModel.InMeasureThreshold = numThreshold.Value;
                tModel.InMeasureTransition = cmbTransition.Text;
                tModel.ExposureTime = numExposureTime.Value;

                if (m_DrawLine4List.Count == 4)
                {
                    int index = 0;

                    List<HTuple> Line4Param = new List<HTuple>();
                    HTuple m_DrawLineStartRow = new HTuple();
                    HTuple m_DrawLineStartCol = new HTuple();
                    HTuple m_DrawLineEndRow = new HTuple();
                    HTuple m_DrawLineEndCol = new HTuple();

                    HObject ho_Obj;
                    foreach (HDrawingObject hdraw in m_DrawLine4List)
                    {
                        HTuple paramName;
                        string[] paramlist = { "row2", "column2", "row1", "column1" };
                        paramName = paramlist;
                        HTuple param = hdraw.GetDrawingObjectParams(paramName);
                        Line4Param.Add(param);
                        m_DrawLineStartRow.Append(Line4Param[index][0]);
                        m_DrawLineStartCol.Append(Line4Param[index][1]);
                        m_DrawLineEndRow.Append(Line4Param[index][2]);
                        m_DrawLineEndCol.Append(Line4Param[index][3]);

                        HOperatorSet.GenRegionLine(out ho_Obj, Line4Param[index][0], Line4Param[index][1], Line4Param[index][2], Line4Param[index][3]);

                        m_hSmartWindow.GetWindowHandle().SetColor("green");
                        m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                        m_hSmartWindow.GetWindowHandle().SetLineWidth(2);

                        m_hSmartWindow.GetWindowHandle().DispObj(ho_Obj);
                        index++;
                    }

                    string strPath = Global.Model3DPath + "//" + cmbCamera.Text;
                    if(!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    HOperatorSet.WriteTuple(m_DrawLineStartRow, strPath + "\\DrawLineStartRow" + cmbFixture.Text);
                    HOperatorSet.WriteTuple(m_DrawLineStartCol, strPath + "\\DrawLineStartCol" + cmbFixture.Text);
                    HOperatorSet.WriteTuple(m_DrawLineEndRow, strPath + "\\DrawLineEndRow" + cmbFixture.Text);
                    HOperatorSet.WriteTuple(m_DrawLineEndCol, strPath + "\\DrawLineEndCol" + cmbFixture.Text);
                }
                m_hSmartWindow.ClearAllObjects();
                m_DrawLine4List.Clear();

                XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));
                AddLog("保存参数成功!");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }
        
        AlgorithmControl m_algorithmControl = new AlgorithmControl();
        private AlgorithmResultModel TestAlgorithm()
        {
            AlgorithmResultModel result = new AlgorithmResultModel();
            try
            {
                string paramPath = Global.Model3DPath + "//" + cmbCamera.Text;
                var algorithmModel = XMLController.XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == cmbFixture.SelectedIndex + 1);

                HObject ho_Xld, ho_Cross, ho_OutObj;
                HTuple hv_bFindCenter, hv_Exception;

                HTuple hv_Row, hv_Column, hv_Phi;
                m_algorithmControl.FindFixturePos(m_hSmartWindow.Image, paramPath, cmbFixture.Text, out ho_Xld, out ho_Cross, algorithmModel.InMeasureLength1,
                    algorithmModel.InMeasureLength2, algorithmModel.InMeasureSigma, algorithmModel.InMeasureThreshold, algorithmModel.InMeasureSelect, algorithmModel.InMeasureTransition,
                    algorithmModel.InMeasureNumber, algorithmModel.InMeasureScore, out hv_Row, out hv_Column, out hv_Phi, out hv_bFindCenter, out hv_Exception, out ho_OutObj);
                
                m_hSmartWindow.GetWindowHandle().SetColor("red");
                m_hSmartWindow.GetWindowHandle().SetDraw("margin");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(2);
                m_hSmartWindow.GetWindowHandle().DispObj(ho_Xld);
                m_hSmartWindow.GetWindowHandle().DispObj(ho_Cross);
                m_hSmartWindow.GetWindowHandle().DispObj(ho_OutObj);

                result.RunResult = hv_bFindCenter == 1;

                result.CenterRow = hv_Row;
                result.CenterColumn = hv_Column;
                result.CenterPhi = hv_Phi;

                if(result.RunResult)
                {
                    txtCenterRow.sText = result.CenterRow.D.ToString();
                    txtCenterCol.sText = result.CenterColumn.D.ToString();
                    txtCenterAngle.sText = result.CenterPhi.D.ToString();
                }
                 
                return result;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                return result;
            }
        }

        private void cmbFixture_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FixtureAlgorithmModel tModel = XMLController.XmlControl.sequenceModelNew.fixtureAlgorithmModels.FirstOrDefault(x => x.Id == cmbFixture.SelectedIndex + 1);
                if (tModel == null)
                {
                    tModel = new FixtureAlgorithmModel();
                    numLength1.Value = tModel.InMeasureLength1;
                    numLength2.Value = tModel.InMeasureLength2;
                    numCount.Value = tModel.InMeasureNumber;
                    numSocre.Value = tModel.InMeasureScore;
                    cmbSelect.Text = tModel.InMeasureSelect;
                    numSigma.Value = tModel.InMeasureSigma;
                    numThreshold.Value = tModel.InMeasureThreshold;
                    cmbTransition.Text = tModel.InMeasureTransition;
                    numExposureTime.Value = tModel.ExposureTime;
                }
                else
                {
                    numLength1.Value = tModel.InMeasureLength1;
                    numLength2.Value = tModel.InMeasureLength2;
                    numCount.Value = tModel.InMeasureNumber;
                    numSocre.Value = tModel.InMeasureScore;
                    cmbSelect.Text = tModel.InMeasureSelect;
                    numSigma.Value = tModel.InMeasureSigma;
                    numThreshold.Value = tModel.InMeasureThreshold;
                    cmbTransition.Text = tModel.InMeasureTransition;
                    numExposureTime.Value = tModel.ExposureTime;
                }
                

                var fixtureModel = XmlControl.sequenceModelNew.fixtureTeachModels.FirstOrDefault(x => x.Id == Int32.Parse(cmbFixture.Text));
                if(fixtureModel == null)
                {
                    txtCenterRow.sText = "0";
                    txtCenterCol.sText = "0";
                    txtCenterAngle.sText = "0";
                }

                txtCenterRow.sText = fixtureModel.TeachImageRow.ToString();
                txtCenterCol.sText = fixtureModel.TeachImageCol.ToString();
                txtCenterAngle.sText = fixtureModel.TeachImageAng.ToString();
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
