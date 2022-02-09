using AlgorithmController;
using DevComponents.DotNetBar; 
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView._3DViews
{
    public partial class ShowView : Form
    {
        CheckFeatureModel m_checkModel = null;
        public ShowView(CheckFeatureModel checkModel)
        {
            InitializeComponent();
            m_checkModel = checkModel;
        }

        private void ShowView_Load(object sender, EventArgs e)
        { 
            UnitSetting set = new UnitSetting();
            int height = set.Height;
            int width = set.Width;

            this.Width = width + 10;
            this.Height = height + 28;

            if (m_checkModel != null)
            {
                LayOutView();
                this.Text = m_checkModel.Name;
            }

            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width + 2,
                Screen.PrimaryScreen.WorkingArea.Height + 2);

            //CommFunc.OpenAnimateWindow2(this.Handle);
        }

        //显示具体特征检测的事件
        private void LayOutView()
        {
            try
            {
                CommFuncView.ShowPanelView(panelView, m_checkModel.featureType, m_checkModel);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("LayOutView:" + ex.Message);
            }
        }

        private void ShowView_FormClosing(object sender, FormClosingEventArgs e)
        { 
            CommFunc.CloseAnimateWindow2(this.Handle);
        }

    }
}
