using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalconView
{
    public partial class FullScreenView : Form
    {
        HSmartWindow m_hwindow = new HSmartWindow();
       // HWindow_Final m_hwindow = new HWindow_Final();
        public FullScreenView(HObject himage, HObject dispObj, string strResult)
        {
            InitializeComponent();
            LayoutChildFillView(panel1, m_hwindow);
            if (dispObj != null && dispObj.IsInitialized())
            {
                m_hwindow.GetWindowHandle().SetDraw("margin");
                m_hwindow.FitImageToWindow(himage, dispObj, false, strResult);
            }
            else
            {
                m_hwindow.FitImageToWindow(himage, null, false, strResult);
            }
        }

        public static void LayoutChildFillView(Control parentView, UserControl childView)
        {
            try
            {
                if (parentView.Controls != null)
                {
                    parentView.Controls.Add(childView);
                    Control[] resultView = parentView.Controls.Find(childView.Name, true);
                    if (resultView.Length > 0)
                    {
                        resultView[0].Dock = DockStyle.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FullScreenView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hwindow.Image != null && m_hwindow.Image.IsInitialized())
            {
                m_hwindow.Image.Dispose();
            }
            //if (m_hwindow.DisplayObj != null && m_hwindow.DisplayObj.IsInitialized())
            //{
            //    m_hwindow.DisplayObj.Dispose();
            //}
            HSmartWindow.m_bShow = true;
            HSmartWindow.view = null;
        }

        private void FullScreenView_Load(object sender, EventArgs e)
        {

        }
 
    }
}
