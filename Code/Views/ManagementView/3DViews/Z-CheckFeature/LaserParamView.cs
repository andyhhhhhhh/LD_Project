using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using XMLController;
using GlobalCore;
using SequenceTestModel;
using DevComponents.DotNetBar;
using HalconView;
using Smartray.Sample;
using HalconDotNet;
using System.Threading;
using Smartray;

namespace ManagementView._3DViews
{
    public partial class LaserParamView : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();
        public LaserParamView()
        {
            InitializeComponent(); 
        }

        private void ImageSet_Load(object sender, EventArgs e)
        {
            try
            {
                numExposureTime.TxtValueEvent += NumExposureTime_TxtValueEvent;
                numGain.TxtValueEvent += NumGain_TxtValueEvent;
                numBrightness.TxtValueEvent += NumBrightness_TxtValueEvent; 

                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
                this.ParentForm.FormClosing += ParentForm_FormClosing;

                if (File.Exists(Global.SequencePath))
                {
                    UpdateData();
                }
            }
            catch (Exception ex)
            { 

            }            
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisConnect();
            }
            catch (Exception ex)
            {
                 
            }
        }

        //保存参数
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = new SequenceModel();
                if (!File.Exists(Global.SequencePath)) 
                { 
                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                }
                else
                {
                    sequence = XmlControl.sequenceModelNew;
                } 

                Camera3DSetModel camera3DModel = cmbName.SelectedItem as Camera3DSetModel;

                if(camera3DModel.Name != txtName.sText)
                {
                    int index = sequence.Camera3DSet.FindIndex(x => x.Name == txtName.sText);
                    if (index != -1)
                    {
                        MessageBoxEx.Show("已存在此名称!!");
                        return;
                    }
                }

                camera3DModel.Name = txtName.sText;
                camera3DModel.IPAddress = txtIP.sText;
                camera3DModel.Port = Int32.Parse(numPort.sText);
                camera3DModel.Profile = Int32.Parse(numProfile.sText);
                camera3DModel.ExposureTime = Int32.Parse(numExposureTime.sText);
                camera3DModel.Brightness = Int32.Parse(numBrightness.sText);
                camera3DModel.Gain = Int32.Parse(numGain.sText);
                camera3DModel.EnableGain = chkEnableGain.Checked;
                camera3DModel.XScale = double.Parse(numXScale.sText);
                camera3DModel.YScale = double.Parse(numYScale.sText);
                camera3DModel.XYResolution = double.Parse(numXYResolution.sText);
                camera3DModel.ZResolution = double.Parse(numZResolution.sText);
                camera3DModel.LaserThreshold = Int32.Parse(numLaserThreshold.sText);

                Global.XYResolution = camera3DModel.XYResolution;
                Global.ZResolution = camera3DModel.ZResolution;

                //XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                UpdateData();
                MessageBoxEx.Show("保存成功");
            }
            catch (Exception ex)
            { 

            }
          
        }

        private void trackBarExposure_Scroll(object sender, EventArgs e)
        {
            try
            {
                numExposureTime.sText = trackBarExposure.Value.ToString();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            try
            { 
                numBrightness.sText = trackBarBrightness.Value.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void trackbarGain_Scroll(object sender, EventArgs e)
        {
            try
            {
                numGain.sText = trackbarGain.Value.ToString(); 
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLiveImage_Click(object sender, EventArgs e)
        {
            try
            {
                if(btnLiveImage.Text == "Live On")
                {
                    btnLiveImage.Text = "Live Off";
                    btnLiveImage.BackColor = Color.White;
                    m_bLiveImage = true;
                    Thread t = new Thread(new ThreadStart(() =>
                    {
                        LiveImageSample(); 
                    }));
                    t.Start();
                }
                else
                {
                    btnLiveImage.Text = "Live On";
                    btnLiveImage.BackColor = Color.LightGray;
                    m_bLiveImage = false; 
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        Sensor sensor = null;
        SensorManager sensorManager = null;
        bool m_bLiveImage = false;
        public void LiveImageSample()
        {
            //Register Acquisition One image Complete Event
            sensor.AcqusitionCompletedEvent += new Sensor.delegateAcqusitionCompleted(OnCompletedAcqEvent);
            m_bCompleted = true;
            while (m_bLiveImage)
            {
                if(!m_bCompleted)
                {
                    Thread.Sleep(100);
                    continue;
                }
                m_bCompleted = false;
                SetParam(); 
                sensor.StartAcquisition();
                sensor.WaitForImage(1);
                Thread.Sleep(50);
            }
            //sensor.Disconnect(); 
        }

        public void Connect()
        {
            if(sensor == null || sensor.GetSensorState().SensorConnection != SensorState.ConnectionState.Connected)
            {
                sensorManager = new SensorManager();
                sensor = sensorManager.CreateSensor("sensor 0");
                sensor.Connect();
                sensor.LoadParameterSet(Sensor.ParameterSet.LiveImage);
                sensor.SendParameterSet();
            } 
        }

        public void DisConnect()
        {
            m_bLiveImage = false;
            if (sensor != null && sensor.GetSensorState().SensorConnection == SensorState.ConnectionState.Connected)
            {
                sensor.StopAcquisition();
                sensor.Disconnect();
            }

            if(sensorManager != null)
            {
                sensorManager.Dispose();
                sensorManager = null;
            }
        }

        bool m_bCompleted = false;
        private void OnCompletedAcqEvent(object Sender, EventArgs e)
        {
            try
            {
                SensorImageData imageDatas = sensor.GetLastImageData();

                if (imageDatas != null)
                {
                    byte[] dataArray = imageDatas.LiveImage.GetImage();

                    if (dataArray != null)
                    {
                        HImage b = ConvertuShortToHimage(dataArray, imageDatas.Width, dataArray.Length / imageDatas.Width);
                        if (b != null)
                        {
                            m_hSmartWindow.FitImageToWindow(b, null);
                        }
                    }
                }

                //imageDatas.SaveLiveImage("LiveImageSample_LiveImage");

                sensor.StopAcquisition();
                m_bCompleted = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            
        }

        private HImage ConvertuShortToHimage(byte[] datas, int ImageWidth, int ImageHeight)
        {
            HImage hImage = new HImage();
            try
            {
                unsafe
                {
                    fixed (byte* m = datas)
                    {
                        hImage.GenImage1("byte", ImageWidth, ImageHeight, new IntPtr(m)); 
                    }
                }
                GC.Collect();
            }
            catch (Exception ce)
            {
                hImage = null; 
            }
            return hImage;
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Text == "Connect")
                {
                    Connect();
                    btnConnect.Text = "DisConnect";
                    btnConnect.BackColor = Color.PaleGreen;
                    btnLiveImage.Enabled = true;
                }
                else
                {
                    DisConnect();
                    btnConnect.Text = "Connect";
                    btnConnect.BackColor = Color.LightCoral;
                    btnLiveImage.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void NumExposureTime_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackBarExposure.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void NumBrightness_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackBarBrightness.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {

            }
        }

        private void NumGain_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackbarGain.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {

            }
        }
         
        private void chkEnableGain_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sensor == null)
                {
                    return;
                }
                //Api.SetGain(sensor._sensorObject, chkEnableGain.Checked, (int)numGain.Value);

            }
            catch (Exception ex)
            {

            }
        }
        
        private void SetParam()
        {
            if (sensor == null)
            {
                return;
            }

            Api.SetExposureTime(sensor._sensorObject, 0, Int32.Parse(numExposureTime.sText));
            Api.SetLaserBrightness(sensor._sensorObject, Int32.Parse(numBrightness.sText));
            Api.SetGain(sensor._sensorObject, chkEnableGain.Checked, Int32.Parse(numGain.sText));


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            
        }

        private void UpdateData()
        {
            try
            {
                var listCamera3D = XmlControl.sequenceModelNew.Camera3DSet;
                if (listCamera3D != null && listCamera3D.Count > 0)
                {
                    cmbName.DataSource = null;
                    cmbName.DataSource = listCamera3D;
                    cmbName.DisplayMember = "Name";

                    int index = 0;
                    foreach (var item in listCamera3D)
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

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Camera3DSetModel cameraSet = cmbName.SelectedItem as Camera3DSetModel;
                if (cameraSet != null)
                {
                    lblId.Text = cameraSet.Id.ToString();
                    txtName.sText = cameraSet.Name;
                    txtIP.sText = cameraSet.IPAddress;
                    numPort.sText = cameraSet.Port.ToString();
                    numProfile.sText = cameraSet.Profile.ToString();
                    numExposureTime.sText = cameraSet.ExposureTime.ToString();
                    trackBarExposure.Value = Int32.Parse(numExposureTime.sText);
                    numBrightness.sText = cameraSet.Brightness.ToString();
                    trackBarBrightness.Value = Int32.Parse(numBrightness.sText);
                    numGain.sText = cameraSet.Gain.ToString();
                    trackbarGain.Value = Int32.Parse(numGain.sText);
                    chkEnableGain.Checked = cameraSet.EnableGain;
                    numXScale.sText = cameraSet.XScale.ToString();
                    numYScale.sText = cameraSet.YScale.ToString();
                    numXYResolution.sText = cameraSet.XYResolution.ToString();
                    numZResolution.sText = cameraSet.ZResolution.ToString();
                    numLaserThreshold.sText = cameraSet.LaserThreshold.ToString();
                }
            }
            catch (Exception ex)
            { 

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Camera3DSetModel camera3DModel = cmbName.SelectedItem as Camera3DSetModel;
                if (camera3DModel == null)
                {
                    return;
                }
                XmlControl.sequenceModelNew.Camera3DSet.Remove(camera3DModel);

                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = new SequenceModel();
                if (!File.Exists(Global.SequencePath))
                {
                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                }
                else
                {
                    sequence = XmlControl.sequenceModelNew;
                }
                Camera3DSetModel camera3DModel = new Camera3DSetModel();

                if (sequence.Camera3DSet.FindIndex(x => x.Name == txtName.sText) != -1)
                {
                    MessageBoxEx.Show("已存在此名称!!");
                    return;
                }

                camera3DModel.Name = txtName.sText;
                camera3DModel.IPAddress = txtIP.sText;
                camera3DModel.Port = Int32.Parse(numPort.sText);
                camera3DModel.Profile = Int32.Parse(numProfile.sText);
                camera3DModel.ExposureTime = Int32.Parse(numExposureTime.sText);
                camera3DModel.Brightness = Int32.Parse(numBrightness.sText);
                camera3DModel.Gain = Int32.Parse(numGain.sText);
                camera3DModel.EnableGain = chkEnableGain.Checked;
                camera3DModel.XScale = double.Parse(numXScale.sText);
                camera3DModel.YScale = double.Parse(numYScale.sText);
                camera3DModel.XYResolution = double.Parse(numXYResolution.sText);
                camera3DModel.ZResolution = double.Parse(numZResolution.sText);
                camera3DModel.LaserThreshold = Int32.Parse(numLaserThreshold.sText);

                sequence.Camera3DSet.Add(camera3DModel);

                Global.XYResolution = camera3DModel.XYResolution;
                Global.ZResolution = camera3DModel.ZResolution;

                //XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                UpdateData();
                MessageBoxEx.Show("新增成功");
            }
            catch (Exception ex)
            {

            }
        }
         
    }
}
