using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalconView
{
    public partial class Diplay3DView : Form
    {
        HObject m_hoImage = null;
        bool m_bVisible = true;
        Thread m_thread;
        public Diplay3DView(HObject ho_Image)
        {
            InitializeComponent();
            m_hoImage = ho_Image.Clone();
        }

        private void Diplay3DView_Load(object sender, EventArgs e)
        {
            try
            {
                Display3DView();
            }
            catch (Exception ex)
            {
                 
            }
        }
         
        #region 3D显示
        //3D Display 
        private void Display3DView()
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            HObject ho_Image;
             
            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_factor = new HTuple();
            HTuple hv_WindowHandle3 = null, hv_ShowCoordinates = null;
            HTuple hv_Mode = null;

            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            ho_Image = m_hoImage.Clone(); 

            // Initialize local and output iconic variables  
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //防止数据太大导致卡
            if ((int)(new HTuple(((hv_Width * hv_Height)).TupleGreater(800 * 800))) != 0)
            {
                hv_factor = hv_Height / 900;
                HTuple hv_factor2 = hv_Width / 900;
                HOperatorSet.TupleMax2(hv_factor, hv_factor2, out hv_factor);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ZoomImageSize(ho_Image, out ExpTmpOutVar_0, hv_Width / hv_factor,
                        hv_Height / hv_factor, "constant"); 
                    ho_Image.Dispose();
                    ho_Image = ExpTmpOutVar_0;
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //HOperatorSet.WriteImage(ho_Image, "png", 0, "D://hoImage.png");
            } 
            //Set ShowCoordinates to true to display image coordinates
            hv_ShowCoordinates = 0;
            //ShowCoordinates := true
            //Choose one of the following modes available for 3d plot
            hv_Mode = "shaded";
            //Mode := 'contour_lines'
            //Mode := 'hidden_lines'
            //Mode := 'texture'
            hv_WindowHandle3 = hWindowControl1.HalconWindow;
            HOperatorSet.SetPart(hv_WindowHandle3, 0, 0, hv_Height, hv_Width);
            
            //HOperatorSet.DispObj(ho_Image, hv_WindowHandle3);

            //return;
            interactive_3d_plot(ho_Image, hv_WindowHandle3, hv_Mode, (((new HTuple("plot_quality")).TupleConcat(
                "show_coordinates")).TupleConcat("step")).TupleConcat("display_grid"), (((new HTuple("best")).TupleConcat(
                hv_ShowCoordinates))).TupleConcat((new HTuple(1)).TupleConcat("false"))); 
        }

        public void interactive_3d_plot(HObject ho_HeightField, HTuple hv_WindowHandle,
                         HTuple hv_Mode, HTuple hv_GenParamName, HTuple hv_GenParamValue)
        { 

            HSystem sys = new HSystem(); 

            HTuple hv_PreviousPlotMode = null, hv_Indices = null;
            HTuple hv_ShowCoordinates = null, hv_Step = null, hv_Button = null;
            HTuple hv_ButtonDown = null, hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_ImageRow = new HTuple(), hv_ImageColumn = new HTuple();
            HTuple hv_Height = new HTuple(), hv_WindowRow = new HTuple();
            HTuple hv_WindowColumn = new HTuple(), hv_WindowWidth = new HTuple();
            HTuple hv_WindowHeight = new HTuple(), hv_BackgroundColor = new HTuple();
            HTuple hv_mode = new HTuple(), hv_lastRow = new HTuple();
            HTuple hv_lastCol = new HTuple();
            // Initialize local and output iconic variables 
            //This procedure is used for the interactive display of a height field
            //and demonstrates the use of the operators update_window_pose and
            //unproject_coordinates.
            //The user manipulates the pose of the height field using the mouse.
            //If the mouse is moved while the left mouse button is pressed, the
            //height field is rotated using a virtual trackball model. If the mouse
            //is moved up and down while the right mouse button is pressed, the
            //camera zooms in and out. If the mouse is moved while the left and
            //the right mouse button are pressed, the height field is moved.
            //Interactive display ends as soon as the middle mouse button is
            //pressed.
            //Using GenParamName and GenParamValue the following parameters can be
            //passed:
            //  plot_quality       the quality of the 3d_plot (see set_window_param)
            //  display_grid       display a grid at height = 0
            //  angle_of_view      parameter of the camera (see set_window_param)
            //  show_coordinates   if true, image coordinates are shown using unproject_coordinates
            //  step               step size of the 3d plot
            // dev_set_preferences(...); only in hdevelop
            HOperatorSet.GetPaint(hv_WindowHandle, out hv_PreviousPlotMode);
            HOperatorSet.TupleFind(hv_GenParamName, "plot_quality", out hv_Indices);
            if ((int)((new HTuple(hv_Indices.TupleEqual(-1))).TupleNot()) != 0)
            {
                HOperatorSet.SetWindowParam(hv_WindowHandle, "plot_quality", hv_GenParamValue.TupleSelect(
                    hv_Indices.TupleSelect(0)));
            }
            HOperatorSet.TupleFind(hv_GenParamName, "display_grid", out hv_Indices);
            if ((int)((new HTuple(hv_Indices.TupleEqual(-1))).TupleNot()) != 0)
            {
                HOperatorSet.SetWindowParam(hv_WindowHandle, "display_grid", hv_GenParamValue.TupleSelect(
                    hv_Indices.TupleSelect(0)));
            }
            HOperatorSet.TupleFind(hv_GenParamName, "angle_of_view", out hv_Indices);
            if ((int)((new HTuple(hv_Indices.TupleEqual(-1))).TupleNot()) != 0)
            {
                HOperatorSet.SetWindowParam(hv_WindowHandle, "angle_of_view", hv_GenParamValue.TupleSelect(
                    hv_Indices.TupleSelect(0)));
            }
            hv_ShowCoordinates = 0;
            HOperatorSet.TupleFind(hv_GenParamName, "show_coordinates", out hv_Indices);
            if ((int)((new HTuple(hv_Indices.TupleEqual(-1))).TupleNot()) != 0)
            {
                hv_ShowCoordinates = hv_GenParamValue.TupleSelect(hv_Indices.TupleSelect(0));
            }
            hv_Step = "*";
            HOperatorSet.TupleFind(hv_GenParamName, "step", out hv_Indices);
            if ((int)((new HTuple(hv_Indices.TupleEqual(-1))).TupleNot()) != 0)
            {
                hv_Step = hv_GenParamValue.TupleSelect(hv_Indices.TupleSelect(0));
            }
            HOperatorSet.SetPaint(hv_WindowHandle, (((((new HTuple("3d_plot")).TupleConcat(
                hv_Mode))).TupleConcat(hv_Step))).TupleConcat("auto"));
            HOperatorSet.SetWindowParam(hv_WindowHandle, "interactive_plot", "true");
            HOperatorSet.SetLut(hv_WindowHandle, "change3"); 
            //set_colored (WindowHandle, 12)
            hv_Button = new HTuple();
            hv_ButtonDown = 0;

            m_thread = new Thread(new ThreadStart(() =>
            {
                while (m_bVisible)
                {
                    // (dev_)set_check ("~give_error")
                    try
                    {
                        HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row, out hv_Column,
                    out hv_Button);
                    }
                    catch (HalconException e)
                    {
                        int error = e.GetErrorCode();
                        if (error < 0)
                            throw e;
                    }
                    if ((int)(hv_ShowCoordinates) != 0)
                    {
                        try
                        {
                            HOperatorSet.UnprojectCoordinates(ho_HeightField, hv_WindowHandle, hv_Row,
                      hv_Column, out hv_ImageRow, out hv_ImageColumn, out hv_Height);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_WindowRow, out hv_WindowColumn,
                      out hv_WindowWidth, out hv_WindowHeight);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.GetWindowParam(hv_WindowHandle, "background_color", out hv_BackgroundColor);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_BackgroundColor);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.DispRectangle1(hv_WindowHandle, 0, 0, 19, hv_WindowWidth - 1);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        if ((int)(new HTuple(hv_BackgroundColor.TupleEqual("black"))) != 0)
                        {
                            try
                            {
                                HOperatorSet.SetColor(hv_WindowHandle, "white");
                            }
                            catch (HalconException e)
                            {
                                int error = e.GetErrorCode();
                                if (error < 0)
                                    throw e;
                            }
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetColor(hv_WindowHandle, "black");
                            }
                            catch (HalconException e)
                            {
                                int error = e.GetErrorCode();
                                if (error < 0)
                                    throw e;
                            }
                        }
                        try
                        {
                            HOperatorSet.SetTposition(hv_WindowHandle, 1, 10);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.WriteString(hv_WindowHandle, "ImageRow: " + hv_ImageRow);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.WriteString(hv_WindowHandle, "   ImageColumn: " + hv_ImageColumn);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        try
                        {
                            HOperatorSet.WriteString(hv_WindowHandle, "   Height: " + hv_Height);
                        }
                        catch (HalconException e)
                        {
                            int error = e.GetErrorCode();
                            if (error < 0)
                                throw e;
                        }
                        //reset colors, because the axis are drawn in the first three colors
                        //set_colored (WindowHandle, 12)
                    }
                    // (dev_)set_check ("give_error")
                    if ((int)(new HTuple(hv_Button.TupleEqual(new HTuple()))) != 0)
                    {
                        hv_Button = 0;
                    }
                    if ((int)(hv_ButtonDown.TupleAnd(new HTuple(hv_Button.TupleEqual(0)))) != 0)
                    {
                        hv_ButtonDown = 0;
                    }
                    if ((int)((new HTuple(hv_Button.TupleEqual(0))).TupleNot()) != 0)
                    {
                        if ((int)(hv_ButtonDown) != 0)
                        {
                            if ((int)(new HTuple(hv_Button.TupleEqual(1))) != 0)
                            {
                                hv_mode = "rotate";
                            }
                            if ((int)(new HTuple(hv_Button.TupleEqual(4))) != 0)
                            {
                                hv_mode = "scale";
                            }
                            if ((int)(new HTuple(hv_Button.TupleEqual(5))) != 0)
                            {
                                hv_mode = "move";
                            }
                            HOperatorSet.UpdateWindowPose(hv_WindowHandle, hv_lastRow, hv_lastCol,
                                hv_Row, hv_Column, hv_mode);
                        }
                        else
                        {
                            if ((int)(new HTuple(hv_Button.TupleEqual(2))) != 0)
                            {
                               // break;
                            }
                            hv_ButtonDown = 1;
                        }
                        hv_lastCol = hv_Column.Clone();
                        hv_lastRow = hv_Row.Clone();
                    }
                    //disp_image can not be used because it discards all channels but
                    //the first, hence the texture mode would not work. 
                    if(!hWindowControl1.IsDisposed)
                    { 
                        HOperatorSet.DispObj(ho_HeightField, hv_WindowHandle);
                    }
                    Thread.Sleep(10);
                }
                HOperatorSet.SetWindowParam(hv_WindowHandle, "interactive_plot", "false");
                HOperatorSet.SetPaint(hv_WindowHandle, hv_PreviousPlotMode);
            }));
            m_thread.Start(); 
        }
        #endregion

        private void Diplay3DView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_bVisible = false;
                m_thread.Abort();
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
