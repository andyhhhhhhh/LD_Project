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
using DevComponents.DotNetBar;
using XMLController;
using HalconDotNet;
using CameraContorller;
using JsonController;
using System.Threading;

namespace ManagementView._3DViews
{
    /// <summary>
    /// 2D相机参数配置界面
    /// </summary>
    public partial class Camera2DParamView : UserControl
    {
        ICameraControl[] m_CameraControl;
        HSmartWindow m_hSmartWindow = new HSmartWindow();

        //保存画面个数需要
        public JsonControl m_jsonControl = new JsonControl();
        public Camera2DParamView()
        {
            InitializeComponent();
        }

        private void Camera2DParamView_Load(object sender, EventArgs e)
        {
            try
            {
                m_CameraControl = new ICameraControl[9];
                
                this.ParentForm.FormClosing += ParentForm_FormClosing;

                cmbCameraType.DataSource = Enum.GetNames(typeof(CameraType));

                cmbCameraType.SelectedIndex = -1;

                UpdateData();
                 
                m_ViewNum = m_jsonControl.SystemPara.ViewNum;
                InitLayOutView();
                SetBtnView();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("Camera2DParamView_Load", ex.Message);
            }
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            { 
                foreach (var item in XmlControl.sequenceModelNew.Camera2DSetModels)
                {
                    item.IsContinue = false;
                    //如果是外触发，则关闭相机
                    if(item.IsExternTrig)
                    {
                        m_CameraControl[item.Id].Run(item, CameraControlType.CameraClose);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void M_Camera2DParamView_ContinueEvent(object sender, object e)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        //m_hSmartWindow.FitImageToWindow((HObject)e, null, true);
                        List<object> list = e as List<object>;
                        int id = (int)list[0];
                        HObject ho_Image = list[1] as HObject; 
                        HSmartWindow hsmartWindow = GetSmartWindow(id);
                        hsmartWindow.DisplayImage(new HImage(ho_Image));
                    }));
                }
                else
                {
                    //m_hSmartWindow.FitImageToWindow((HObject)e, null, true);
                    List<object> list = e as List<object>;
                    int id = (int)list[0];
                    HObject ho_Image = list[1] as HObject;
                    HSmartWindow hsmartWindow = GetSmartWindow(id);
                    hsmartWindow.DisplayImage(new HImage(ho_Image));
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        #region 按钮操作

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(m_Camera2Dmodel, CameraControlType.CameraDiscover);
                if (resultModel.RunResult)
                {
                    cmbCameras.DataSource = resultModel.ObjectResult as List<string>;
                }
                else
                {
                    MessageBox.Show("未找到相机设备!");
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnSearch_Click", ex.Message);
            }
        }

        private void chkRealDisplay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkRealDisplay.Checked)
                {
                    if(m_Camera2Dmodel.IsContinue)
                    {
                        return;
                    }
                    var resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(m_Camera2Dmodel, CameraControlType.CameraOpenContinue);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(resultModel.ErrorMessage);
                        return;
                    }

                    //设置参数
                    resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(m_Camera2Dmodel, CameraControlType.CameraSetParam) as CameraResultModel;
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(resultModel.ErrorMessage);
                        return;
                    }

                    resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(m_Camera2Dmodel, CameraControlType.CameraStartContinue);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(resultModel.ErrorMessage);
                    }
                }
                else
                {
                    m_Camera2Dmodel.IsContinue = false;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("chkRealDisplay_CheckedChanged", ex.Message);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew; 

                 Camera2DSetModel tModel = cmbName.SelectedItem as Camera2DSetModel;

                if (tModel.Name != txtName.sText)
                {
                    int index = sequence.Camera3DSet.FindIndex(x => x.Name == txtName.sText);
                    if (index != -1)
                    {
                        MessageBoxEx.Show("已存在此名称!!");
                        return;
                    }
                }

                tModel.Name = txtName.sText;
                tModel.DeviceName = cmbCameras.Text;
                tModel.UniqueName = cmbCameras.Text;
                tModel.GainParamName = cmbGainName.Text;
                tModel.ExposureParamName = cmbExposureName.Text;
                tModel.ExposureTime = Convert.ToInt32(numExposureTime.sText);
                tModel.Gain = Convert.ToInt32(numGain.sText);
                tModel.InterfaceName = cmbInterfaceName.Text;
                tModel.Index = cmbIndex.SelectedIndex;
                tModel.cameraType = (CameraType)Enum.Parse(typeof(CameraType), cmbCameraType.Text);

                tModel.bLocalImage = chkbLocalImage.Checked;
                tModel.LocalPath = localImagePath.FilePath;
                tModel.IsExternTrig = chkIsExternTrig.Checked;

                UpdateData();

                m_CameraControl[tModel.Id].Run(tModel, CameraControlType.CameraSetParam);

                MessageBoxEx.Show("保存成功");
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew; 
                Camera2DSetModel tModel = new Camera2DSetModel();

                if (sequence.Camera2DSetModels.FindIndex(x => x.Name == txtName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在此名称!!");
                    return;
                }

                tModel.Name = txtName.sText;
                tModel.DeviceName = cmbCameras.Text;
                tModel.UniqueName = cmbCameras.Text;
                tModel.GainParamName = cmbGainName.Text;
                tModel.ExposureParamName = cmbExposureName.Text;
                tModel.ExposureTime = Convert.ToInt32(numExposureTime.sText);
                tModel.Gain = Convert.ToInt32(numGain.sText);
                tModel.InterfaceName = cmbInterfaceName.Text;
                tModel.Index = cmbIndex.SelectedIndex;
                tModel.cameraType = (CameraType)Enum.Parse(typeof(CameraType), cmbCameraType.Text);
                tModel.IsExternTrig = chkIsExternTrig.Checked;
                tModel.bLocalImage = chkbLocalImage.Checked;
                tModel.LocalPath = localImagePath.FilePath;

                sequence.Camera2DSetModels.Add(tModel);
                 
                UpdateData();
                MessageBoxEx.Show("新增成功");
            }
            catch (Exception ex)
            {

            }
        }

        Camera2DSetModel m_Camera2Dmodel = null;
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                HObject ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Image);
                ho_Image.Dispose(); 

                if (m_Camera2Dmodel.bLocalImage)
                {
                    MessageBoxEx.Show("配置为本地图片读取!!");
                    return;
                }
                string msg = "";
                CameraResultModel resultModel = CameraSnapFunc(m_Camera2Dmodel);
                ho_Image = resultModel.Image as HObject;

                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                }
                else
                {
                    HSmartWindow hSmartWindow = GetSmartWindow(m_Camera2Dmodel.Id);
                    hSmartWindow.FitImageToWindow(ho_Image, null, true);
                } 
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnSnap_Click", ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(m_Camera2Dmodel, CameraControlType.CameraClose) as CameraResultModel;
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("[{0}]{1}", m_Camera2Dmodel.Name, resultModel.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnDisconnect_Click", ex.Message);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Camera2DSetModel camera2DModel = cmbName.SelectedItem as Camera2DSetModel;
                if (camera2DModel == null)
                {
                    return;
                }

                XmlControl.sequenceModelNew.Camera2DSetModels.Remove(camera2DModel);

                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //切换时先把实时采集关闭
                //if(m_Camera2Dmodel != null)
                //{
                //    m_Camera2Dmodel.IsContinue = false;
                //}

                Camera2DSetModel tModel = cmbName.SelectedItem as Camera2DSetModel;
                if (tModel != null)
                {
                    m_Camera2Dmodel = tModel;

                    txtName.sText = tModel.Name;
                    cmbCameras.Text = tModel.DeviceName;
                    cmbGainName.Text = tModel.GainParamName;
                    cmbExposureName.Text = tModel.ExposureParamName;
                    numExposureTime.sText = tModel.ExposureTime.ToString();
                    numGain.sText = tModel.Gain.ToString();
                    cmbInterfaceName.Text = tModel.InterfaceName;
                    cmbIndex.SelectedIndex = tModel.Index;
                    cmbCameraType.Text = tModel.cameraType.ToString();
                    chkIsExternTrig.Checked = tModel.IsExternTrig;
                    lblId.Text = tModel.Id.ToString();

                    chkRealDisplay.Checked = tModel.IsContinue;
                    chkbLocalImage.Checked = tModel.bLocalImage;
                    localImagePath.FilePath = tModel.LocalPath;

                    if(m_ViewNum > 1)
                    {
                        for (int i = 0; i < m_ViewNum; i++)
                        {
                            m_hSmartWindowArr[i].ShowResult(true, tModel.Id != i);
                        }
                    }

                    cmbCameraType_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void cmbCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbIndex.SelectedIndex = cmbCameras.SelectedIndex;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void UpdateData()
        {
            try
            {
                var listCamera2D = XmlControl.sequenceModelNew.Camera2DSetModels;
                if (listCamera2D != null && listCamera2D.Count > 0)
                {
                    cmbName.DataSource = null;
                    cmbName.DataSource = listCamera2D;
                    cmbName.DisplayMember = "Name";

                    int index = 0;
                    foreach (var item in listCamera2D)
                    {
                        item.Id = index;
                        index++;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        
        public CameraResultModel CameraSnapFunc(Camera2DSetModel model)
        {
            CameraResultModel resultModel = new CameraResultModel();
            try
            {
                //打开相机
                resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(model, CameraControlType.CameraOpenBySoft) as CameraResultModel;
                if (!resultModel.RunResult)
                {
                    resultModel.ErrorMessage = string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage);
                    return resultModel;
                }

                //设置参数
                resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(model, CameraControlType.CameraSetParam) as CameraResultModel;
                if (!resultModel.RunResult)
                {
                    resultModel.ErrorMessage = string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage);
                    return resultModel;
                }

                //执行拍照
                resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(model, CameraControlType.CameraStartSnapBySoft) as CameraResultModel;
                if (!resultModel.RunResult)
                {
                    resultModel.ErrorMessage = string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage);
                    return resultModel;
                }

                //如果是外部触发
                if (model.IsExternTrig)
                {
                    if(m_Camera2Dmodel.cameraType == CameraType.ITEK)
                    {
                        Thread.Sleep(1000);
                    }
                    resultModel = m_CameraControl[m_Camera2Dmodel.Id].Run(model, CameraControlType.CameraGetImageByTirgger) as CameraResultModel;
                    if (!resultModel.RunResult)
                    {
                        resultModel.ErrorMessage = string.Format("[{0}]{1}", model.Name, resultModel.ErrorMessage);
                        return resultModel;
                    }
                }

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }

        private void cmbCameraType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(m_Camera2Dmodel == null)
                {
                    return;
                }

                m_Camera2Dmodel.cameraType = (CameraType)Enum.Parse(typeof(CameraType), cmbCameraType.Text);

                SetCameraType(m_Camera2Dmodel);

                cmbInterfaceName.Enabled = cmbCameraType.SelectedIndex == 0;
                cmbExposureName.Enabled = cmbCameraType.SelectedIndex == 0;
                cmbGainName.Enabled = cmbCameraType.SelectedIndex == 0; 
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void SetCameraType(Camera2DSetModel tModel)
        {
            try
            {
                switch(tModel.cameraType)
                {
                    case CameraType.BASLER:
                        m_CameraControl[tModel.Id] = new CameraByBaslerControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;

                    case CameraType.MVHAL: 
                        m_CameraControl[tModel.Id] = new CameraByHalconControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;

                    case CameraType.MVS:
                        m_CameraControl[tModel.Id] = new CameraByMVSControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;

                    case CameraType.ITEK:
                        m_CameraControl[tModel.Id] = new CameraByITekControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;

                    case CameraType.IDS:
                        m_CameraControl[tModel.Id] = new CameraByIDSControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;

                    default:
                        m_CameraControl[tModel.Id] = new CameraByHalconControl();
                        m_CameraControl[tModel.Id].GetImageByContinue += M_Camera2DParamView_ContinueEvent;
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        #region 切换多画面

        private void btnView_1_Click(object sender, EventArgs e)
        {
            try
            {
                CCWin.SkinControl.SkinButton btn = sender as CCWin.SkinControl.SkinButton;
                if (btn.Name == "btnView_1")
                {
                    m_ViewNum = 1;
                }
                else if(btn.Name == "btnView_2")
                {
                    m_ViewNum = 2;
                }
                else if (btn.Name == "btnView_4")
                {
                    m_ViewNum = 4;
                }
                else if (btn.Name == "btnView_6")
                {
                    m_ViewNum = 6;
                }
                else if (btn.Name == "btnView_9")
                {
                    m_ViewNum = 9;
                } 
               
                InitLayOutView();

                m_jsonControl.SystemPara.ViewNum = m_ViewNum;
                m_jsonControl.SaveConfigurationClassToJsonFile();

                SetBtnView();
            }
            catch (Exception ex)
            {
                 
            }
        }

        int m_ViewNum = 1;
        HSmartWindow[] m_hSmartWindowArr;
        private void InitLayOutView()
        {
            //切换时先把所有画面的实时都关闭
            chkRealDisplay.Checked = false;
            foreach (var item in XmlControl.sequenceModelNew.Camera2DSetModels)
            {
                if(item.IsContinue)
                {
                    item.IsContinue = false;
                    Thread.Sleep(200);
                }
            }

            if (m_ViewNum == 1)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(sPanelCamera);
                sPanelCamera.Dock = DockStyle.Fill;

                if (m_hSmartWindow == null || m_hSmartWindow.IsDisposed)
                {
                    m_hSmartWindow = new HSmartWindow();
                }
                LayOutHalWindow(sPanelCamera, m_hSmartWindow);
            }
            else if (m_ViewNum == 2)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_2);
                panel_2.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_ViewNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_ViewNum];
                }
                if (m_hSmartWindowArr[0] == null || m_hSmartWindowArr[0].IsDisposed)
                {
                    m_hSmartWindowArr[0] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[1] == null || m_hSmartWindowArr[1].IsDisposed)
                {
                    m_hSmartWindowArr[1] = new HSmartWindow();
                }
                LayOutHalWindow(panel_21V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_22V, m_hSmartWindowArr[1]);
            } 
            else if (m_ViewNum == 4)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_4);
                panel_4.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_ViewNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_ViewNum];
                }
                if (m_hSmartWindowArr[0] == null || m_hSmartWindowArr[0].IsDisposed)
                {
                    m_hSmartWindowArr[0] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[1] == null || m_hSmartWindowArr[1].IsDisposed)
                {
                    m_hSmartWindowArr[1] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[2] == null || m_hSmartWindowArr[2].IsDisposed)
                {
                    m_hSmartWindowArr[2] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[3] == null || m_hSmartWindowArr[3].IsDisposed)
                {
                    m_hSmartWindowArr[3] = new HSmartWindow();
                }
                LayOutHalWindow(panel_41V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_42V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_43V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_44V, m_hSmartWindowArr[3]);
            }
            else if (m_ViewNum == 6)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_6);
                panel_6.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_ViewNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_ViewNum];
                }
                if (m_hSmartWindowArr[0] == null || m_hSmartWindowArr[0].IsDisposed)
                {
                    m_hSmartWindowArr[0] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[1] == null || m_hSmartWindowArr[1].IsDisposed)
                {
                    m_hSmartWindowArr[1] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[2] == null || m_hSmartWindowArr[2].IsDisposed)
                {
                    m_hSmartWindowArr[2] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[3] == null || m_hSmartWindowArr[3].IsDisposed)
                {
                    m_hSmartWindowArr[3] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[4] == null || m_hSmartWindowArr[4].IsDisposed)
                {
                    m_hSmartWindowArr[4] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[5] == null || m_hSmartWindowArr[5].IsDisposed)
                {
                    m_hSmartWindowArr[5] = new HSmartWindow();
                }
                LayOutHalWindow(panel_61V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_62V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_63V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_64V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_65V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_66V, m_hSmartWindowArr[5]);
            }
            else if (m_ViewNum == 9)
            {
                panelView.Controls.Clear();
                panelView.Controls.Add(panel_9);
                panel_9.Dock = DockStyle.Fill;

                if (m_hSmartWindowArr == null || m_hSmartWindowArr.Length < m_ViewNum)
                {
                    m_hSmartWindowArr = new HSmartWindow[m_ViewNum];
                }
                if (m_hSmartWindowArr[0] == null || m_hSmartWindowArr[0].IsDisposed)
                {
                    m_hSmartWindowArr[0] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[1] == null || m_hSmartWindowArr[1].IsDisposed)
                {
                    m_hSmartWindowArr[1] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[2] == null || m_hSmartWindowArr[2].IsDisposed)
                {
                    m_hSmartWindowArr[2] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[3] == null || m_hSmartWindowArr[3].IsDisposed)
                {
                    m_hSmartWindowArr[3] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[4] == null || m_hSmartWindowArr[4].IsDisposed)
                {
                    m_hSmartWindowArr[4] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[5] == null || m_hSmartWindowArr[5].IsDisposed)
                {
                    m_hSmartWindowArr[5] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[6] == null || m_hSmartWindowArr[6].IsDisposed)
                {
                    m_hSmartWindowArr[6] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[7] == null || m_hSmartWindowArr[7].IsDisposed)
                {
                    m_hSmartWindowArr[7] = new HSmartWindow();
                }
                if (m_hSmartWindowArr[8] == null || m_hSmartWindowArr[8].IsDisposed)
                {
                    m_hSmartWindowArr[8] = new HSmartWindow();
                }
                LayOutHalWindow(panel_91V, m_hSmartWindowArr[0]);
                LayOutHalWindow(panel_92V, m_hSmartWindowArr[1]);
                LayOutHalWindow(panel_93V, m_hSmartWindowArr[2]);
                LayOutHalWindow(panel_94V, m_hSmartWindowArr[3]);
                LayOutHalWindow(panel_95V, m_hSmartWindowArr[4]);
                LayOutHalWindow(panel_96V, m_hSmartWindowArr[5]);
                LayOutHalWindow(panel_97V, m_hSmartWindowArr[6]);
                LayOutHalWindow(panel_98V, m_hSmartWindowArr[7]);
                LayOutHalWindow(panel_99V, m_hSmartWindowArr[8]);
            }
        }
         
        public void LayOutHalWindow(Control control, UserControl userControl)
        {
            if (!control.Controls.Contains(userControl))
            {
                CommHelper.LayoutChildFillView(control, userControl);
            }
        }
        
        private HSmartWindow GetSmartWindow(int iNum)
        {
            if (m_ViewNum == 1)
            {
                return m_hSmartWindow;
            }
            else if (iNum > m_ViewNum - 1)
            {
                return m_hSmartWindowArr[0];
            }
            else
            {
                return m_hSmartWindowArr[iNum];
            }
        }

        private void SetBtnView()
        {
            try
            {
                switch(m_ViewNum)
                {
                    case 1:
                        btnView_1.BackColor = Color.RoyalBlue;
                        btnView_2.BackColor = Color.Transparent;
                        btnView_4.BackColor = Color.Transparent;
                        btnView_6.BackColor = Color.Transparent;
                        btnView_9.BackColor = Color.Transparent;
                        break;
                    case 2:
                        btnView_1.BackColor = Color.Transparent;
                        btnView_2.BackColor = Color.RoyalBlue;
                        btnView_4.BackColor = Color.Transparent;
                        btnView_6.BackColor = Color.Transparent;
                        btnView_9.BackColor = Color.Transparent;
                        break;
                    case 4:
                        btnView_1.BackColor = Color.Transparent;
                        btnView_2.BackColor = Color.Transparent;
                        btnView_4.BackColor = Color.RoyalBlue;
                        btnView_6.BackColor = Color.Transparent;
                        btnView_9.BackColor = Color.Transparent;
                        break;
                    case 6:
                        btnView_1.BackColor = Color.Transparent;
                        btnView_2.BackColor = Color.Transparent;
                        btnView_4.BackColor = Color.Transparent;
                        btnView_6.BackColor = Color.RoyalBlue;
                        btnView_9.BackColor = Color.Transparent;
                        break;
                    case 9:
                        btnView_1.BackColor = Color.Transparent;
                        btnView_2.BackColor = Color.Transparent;
                        btnView_4.BackColor = Color.Transparent;
                        btnView_6.BackColor = Color.Transparent;
                        btnView_9.BackColor = Color.RoyalBlue;
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

        private void chkbLocalImage_CheckedChanged(object sender, EventArgs e)
        {
            localImagePath.Enabled = chkbLocalImage.Checked;
        }
    }
}
