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
        Camera2DParamView m_CameraView = new Camera2DParamView();
        //AlgorithmView m_algorithmView = new AlgorithmView();
        LDAlgorithmView m_ldalgorithmView = new LDAlgorithmView();
        CalibrationView m_calibrationView = new CalibrationView();
        UnloadTeachView m_unloadTeachView = new UnloadTeachView();
        FixtureTeachView m_fixtureView = new FixtureTeachView();
        public VisionView()
        {
            InitializeComponent();
        }

        private void VisionView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTabIndex = 0;

                CommHelper.LayoutChildFillView(panelCamera, m_CameraView);
                CommHelper.LayoutChildFillView(panelAlgrithm, m_ldalgorithmView);
                CommHelper.LayoutChildFillView(panelCalibration, m_calibrationView);
                CommHelper.LayoutChildFillView(panelUnload, m_unloadTeachView);
                CommHelper.LayoutChildFillView(panelFixture, m_fixtureView);

                m_calibrationView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;
                //m_calibrationView.m_FuncAlgorithm = m_algorithmView.AlgorithmFunc;
                //m_algorithmView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;

                m_unloadTeachView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc;
                //m_unloadTeachView.m_FuncAlgorithm = m_algorithmView.AlgorithmFunc;

                m_fixtureView.m_FuncCameraSnap = m_CameraView.CameraSnapFunc; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
