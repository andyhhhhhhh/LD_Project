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
using HalconDotNet;

namespace ManagementView.Comment
{
    public partial class RefrencePointView : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();

        double m_SetRow = 0;
        double m_SetCol = 0;
        string m_strPath = "";
        double m_StartRow = 0;
        double m_StartCol = 0; 
        public RefrencePointView(HObject ho_Image, double Row, double Col, string path, double StartRow, double StartCol)
        {
            try
            {
                InitializeComponent();

                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);

                m_SetRow = Row;
                m_SetCol = Col;
                m_hSmartWindow.FitImageToWindow(ho_Image, null);

                m_StartRow = StartRow;
                m_StartCol = StartCol;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3); 
                m_hSmartWindow.GetWindowHandle().DispCross(m_SetRow, m_SetCol, 60, 0);

                m_strPath = path + "SetPoint.tup";
            }
            catch (Exception ex)
            {
                  
            }
        }

        private void RefrencePointView_Load(object sender, EventArgs e)
        {
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                double dis = Double.Parse(numMoveDis.sText);
                double row = m_SetRow + dis;
                double col = m_SetCol;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(row, col, 60, 0);

                m_SetRow = row;
                m_SetCol = col;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            try
            {
                double dis = Double.Parse(numMoveDis.sText);
                double row = m_SetRow;
                double col = m_SetCol + dis;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(row, col, 60, 0);

                m_SetRow = row;
                m_SetCol = col;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            try
            {
                double dis = Double.Parse(numMoveDis.sText);
                double row = m_SetRow;
                double col = m_SetCol - dis;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(row, col, 60, 0);

                m_SetRow = row;
                m_SetCol = col;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                double dis = Double.Parse(numMoveDis.sText);
                double row = m_SetRow - dis;
                double col = m_SetCol;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(row, col, 60, 0);

                m_SetRow = row;
                m_SetCol = col;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_tup = new HTuple();

                HTuple hv_Row, hv_Col;
                hv_Row = m_StartRow + m_SetRow;
                hv_Col = m_StartCol + m_SetCol;

                hv_tup.Append(hv_Row);
                hv_tup.Append(hv_Col);

                HOperatorSet.WriteTuple(hv_tup, m_strPath);
            }
            catch (Exception ex)
            {                

            }
        }

        private void RadioCenter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(!RadioCenter.Checked)
                {
                    return;
                }

                HTuple Width, Height;
                HOperatorSet.GetImageSize(m_hSmartWindow.Image, out Width, out Height);
                m_SetRow = Height / 2;
                m_SetCol = Width / 2;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(m_SetRow, m_SetCol, 60, 0);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void RadioWhite_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!RadioWhite.Checked)
                {
                    return;
                }

                HObject ho_Region;
                HTuple hv_Threshold;
                HOperatorSet.BinaryThreshold(m_hSmartWindow.Image, out ho_Region, "max_separability", "light", out hv_Threshold);

                HTuple hv_Area, hv_Row, hv_Col;
                HOperatorSet.Connection(ho_Region, out ho_Region);
                HOperatorSet.SelectShapeStd(ho_Region, out ho_Region, "max_area", 70);

                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Col);

                m_SetRow = hv_Row;
                m_SetCol = hv_Col;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(m_SetRow, m_SetCol, 60, 0);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void RadioBlack_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!RadioBlack.Checked)
                {
                    return;
                }

                HObject ho_Region;
                HTuple hv_Threshold;
                HOperatorSet.BinaryThreshold(m_hSmartWindow.Image, out ho_Region, "max_separability", "dark", out hv_Threshold);

                HTuple hv_Area, hv_Row, hv_Col;
                HOperatorSet.Connection(ho_Region, out ho_Region);
                HOperatorSet.SelectShapeStd(ho_Region, out ho_Region, "max_area", 70);

                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Col);

                m_SetRow = hv_Row;
                m_SetCol = hv_Col;

                m_hSmartWindow.GetWindowHandle().SetColor("green");
                m_hSmartWindow.GetWindowHandle().SetLineWidth(3);
                m_hSmartWindow.GetWindowHandle().ClearWindow();
                m_hSmartWindow.GetWindowHandle().DispObj(m_hSmartWindow.Image);
                m_hSmartWindow.GetWindowHandle().DispCross(m_SetRow, m_SetCol, 60, 0);
            }
            catch (Exception ex)
            {

            }
        }

        private void radioManual_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
