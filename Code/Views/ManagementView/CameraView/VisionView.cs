using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementView._3DViews; 

namespace ManagementView
{
    /// <summary>
    /// 相机界面
    /// </summary>
    public partial class VisionView : UserControl
    {
        Camera2DParamView m_CameraView;
        LDAlgorithmView m_ldalgorithmView;
        CalibrationView m_calibrationView;
        FixtureTeachView m_fixtureView;
        CheckAlgorithmView m_checkView;
        public VisionView()
        {
            InitializeComponent();
        }

        private void VisionView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTabIndex = 0;

                m_CameraView = new Camera2DParamView();
                m_ldalgorithmView = new LDAlgorithmView();
                m_calibrationView = new CalibrationView();
                m_fixtureView = new FixtureTeachView();
                m_checkView = new CheckAlgorithmView();

                CommHelper.LayoutChildFillView(panelCamera, m_CameraView);
                CommHelper.LayoutChildFillView(panelAlgrithm, m_ldalgorithmView);
                CommHelper.LayoutChildFillView(panelCalibration, m_calibrationView);
                CommHelper.LayoutChildFillView(panelFixture, m_fixtureView);
                CommHelper.LayoutChildFillView(panelcheck, m_checkView);

                m_calibrationView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;
                //m_calibrationView.m_FuncAlgorithm = m_ldalgorithmView.AlgorithmFunc; 

                m_ldalgorithmView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc; 

                m_fixtureView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;

                m_checkView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
