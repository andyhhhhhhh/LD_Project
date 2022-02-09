using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;
using System.Runtime.InteropServices; 
using System.Diagnostics;
using System.Threading;

namespace HalconView
{
    /// <summary>
    /// 图形界面Window
    /// </summary>
    public partial class HSmartWindow : UserControl
    {
        #region 参数定义
        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
        private SaveFileDialog SaveFileDialogImage = new SaveFileDialog();
        private int hv_imageWidth, hv_imageHeight;
        private string str_imgSize;
        private HImage background_image = null;
        double positionX = 0, positionY = 0;
        string grayValue = "";
        private Size prevsize = new Size();

        public const string RECT1 = "RECTANGLE1";
        public const string RECT2 = "RECTANGLE2";
        public const string Circle = "Circle";
        public const string Ellipse = "Ellipse";
        public const string Circle_Sector = "Circle_Sector";
        public const string Circle_Sector2 = "Circle_Sector2";
        public const string Double_Circle = "Double_Circle";
        public const string Line = "Line";
        public const string LineM = "LineM";//添加MeasureLine
        public const string XLD = "Xld";

        public string m_Status = "";
        private bool m_bShowHat = false;
        
        /// <summary>
        /// 是否显示十字线
        /// </summary>
        public static bool m_bShowCross = false;

        private Dictionary<long, string> m_Dic = new Dictionary<long, string>();
        private List<HDrawingObject> drawing_objects = new List<HDrawingObject>();
        public HDrawingObject selected_drawing_object = new HDrawingObject(250, 250, 100);
        public HDrawingObject result_drawing_object = null;
        #endregion

        #region 窗体构造函数
        public HSmartWindow()
        {
            InitializeComponent();
            hsmartControl.HDoubleClickToFitContent = true;
        }

        private void HSmartWindow_Load(object sender, EventArgs e)
        {
            try
            {
                clearAllObjectsToolStripMenuItem.Text += string.Format("({0})", hsmartControl.HalconID);
                CrossHair.Checked = m_bShowCross;
            }
            catch (Exception ex)
            {

            }
        }
        
        #endregion

        #region 显示图片 Object
        public void DispImage(HImage image, bool isNoFit = false, bool bclearwindow = true)
        {
            if (image == null || image.Key == (IntPtr)0)
            {
                return;
            }

            ///**************用BegionInvoke会导致后面的Object不刷新****************///
            //BeginInvoke(new Action(() =>
            //{
            if (bclearwindow)
            {
                hsmartControl.HalconWindow.ClearWindow();
            }
            DisposeObj();

            background_image = image;
            background_image.GetImageSize(out hv_imageWidth, out hv_imageHeight);
            str_imgSize = String.Format("{0}X{1}", hv_imageWidth, hv_imageHeight);

            hsmartControl.HalconWindow.AttachBackgroundToWindow(background_image);

            if (!isNoFit)
            {
                hsmartControl.HalconWindow.SetPart(0, 0, hv_imageHeight - 1, hv_imageWidth - 1);
                hsmartControl.SetFullImagePart();
            }

            ShowCrossHair();
            //}));
        }

        public HImage Image
        {
            set { background_image = value; }
            get { return background_image; }
        }

        HObject ho_Contour;
        public HObject Contour
        {
            set { ho_Contour = value; }
            get { return ho_Contour; }
        }

        public void DisposeObj()
        {
            try
            {
                if (Image != null && Image.IsInitialized())
                {
                    Image.Dispose();
                }

                if (hv_DispObj != null && hv_DispObj.IsInitialized())
                {
                    hv_DispObj = null;
                    //hv_DispObj.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 显示Object
        /// </summary>
        /// <param name="hObj">传入的region.xld,image</param>
        public void DispObj(HObject hObj)
        {
            lock (this)
            {
                GetWindowHandle().DispObj(hObj);
                if (hObj != null)
                {
                    if (hv_DispObj != null && hv_DispObj.IsInitialized())
                    {
                        HOperatorSet.ConcatObj(hv_DispObj, hObj, out hv_DispObj);
                    }
                    else
                    {
                        hv_DispObj = hObj;
                    }
                }
            }
        }

        private HObject hv_DispObj;
        public HObject DisplayObj
        {
            get
            {
                return hv_DispObj;
            }
            set
            {
                if (value != null)
                {
                    if (this.hv_DispObj != null)
                    {
                        this.hv_DispObj.Dispose();
                    }
                    hv_DispObj = value;
                }
            }
        }
        #endregion 

        #region 右键ContextMenuStrip事件

        public void ShowContextMenu(bool bShow)
        {
            try
            {
                hsmartControl.ContextMenuStrip = bShow ? contextMenuStrip1 : null;
            }
            catch (Exception ex)
            {

            }
        }

        //Open Image
        private void openImageToolStripMenuItem_Click_1(object sender, EventArgs e)
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
                        HObject hImage;
                        HOperatorSet.ReadImage(out hImage, strImageFile);

                        FitImageToWindow(new HImage(hImage), null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "打开图像失败!" + ex.Message, "提示");
                }
            }
        }

        //Save Image
        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((null == Image) || (!Image.IsInitialized()))
                {
                    return;
                }

                this.SaveFileDialogImage.Filter = "JPEG File(*.jpg)|*.jpg|BMP File(*.bmp)|*.bmp|PNG File(*.png)|*.png";
                if (this.SaveFileDialogImage.ShowDialog() == DialogResult.OK)
                {
                    if (SaveFileDialogImage.FilterIndex == 1)
                    {
                        HOperatorSet.WriteImage(Image, "jpg", 0, this.SaveFileDialogImage.FileName);
                    }
                    else if (SaveFileDialogImage.FilterIndex == 2)
                    {
                        HOperatorSet.WriteImage(Image, "bmp", 0, this.SaveFileDialogImage.FileName);
                    }
                    else if (SaveFileDialogImage.FilterIndex == 3)
                    {
                        HOperatorSet.WriteImage(Image, "png", 0, this.SaveFileDialogImage.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "保存图像失败！" + ex.Message, "提示");
            }
        }

        //Save Screen
        private void SaveScreentoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                HObject ho_DumpImage;
                HOperatorSet.GenEmptyObj(out ho_DumpImage);

                ho_DumpImage.Dispose();
                HOperatorSet.DumpWindowImage(out ho_DumpImage, GetWindowHandle());

                if ((null == ho_DumpImage) || (!ho_DumpImage.IsInitialized()))
                {
                    return;
                }

                this.SaveFileDialogImage.Filter = "PNG File(*.png)|*.png|BMP File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg";
                if (this.SaveFileDialogImage.ShowDialog() == DialogResult.OK)
                {
                    if (SaveFileDialogImage.FilterIndex == 1)
                    {
                        HOperatorSet.WriteImage(ho_DumpImage, "png", 0, this.SaveFileDialogImage.FileName);
                    }
                    else if (SaveFileDialogImage.FilterIndex == 2)
                    {
                        HOperatorSet.WriteImage(ho_DumpImage, "bmp", 0, this.SaveFileDialogImage.FileName);
                    }
                    else if (SaveFileDialogImage.FilterIndex == 3)
                    {
                        HOperatorSet.WriteImage(ho_DumpImage, "jpg", 0, this.SaveFileDialogImage.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "保存窗口截图失败！" + ex.Message, "提示");
            }
        }

        //Draw rectangle1
        private void rectangle1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject rect1 = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.RECTANGLE1, 100, 100, 210, 210);
            rect1.SetDrawingObjectParams("color", "green");
            AttachDrawObj(rect1);
            m_Dic.Add(rect1.ID, RECT1);

        }

        public void AddRect(int height = 100, int width = 100, int count = 0)
        {
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    HDrawingObject rect1 = HDrawingObject.CreateDrawingObject(
                      HDrawingObject.HDrawingObjectType.RECTANGLE1, 100, 100, 100 + height, 100 + width);
                    rect1.SetDrawingObjectParams("color", "green");
                    AttachDrawObj(rect1);
                    m_Dic.Add(rect1.ID, RECT1);
                }
            }
            else
            {
                HDrawingObject rect1 = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.RECTANGLE1, 100, 100, 100 + height, 100 + width);
                rect1.SetDrawingObjectParams("color", "green");
                AttachDrawObj(rect1);
                m_Dic.Add(rect1.ID, RECT1);
            }
        }

        public void AddRect(ref HDrawingObject rect, double dStartRow, double dStartCol, double dEndRow, double dEndCol, string color = "green")
        {
            rect = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.RECTANGLE1, dStartRow, dStartCol, dEndRow, dEndCol);
            rect.SetDrawingObjectParams("color", color);
            AttachDrawObj(rect);
            m_Dic.Add(rect.ID, RECT1); 
        }

        //Draw rectangle2
        private void rectangle2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject rect2 = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.RECTANGLE2, 100, 100, 0, 100, 50);
            rect2.SetDrawingObjectParams("color", "yellow");
            AttachDrawObj(rect2);
            m_Dic.Add(rect2.ID, RECT2);
        }

        public void AddRect2(int count = 0)
        {
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    HDrawingObject rect2 = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.RECTANGLE2, 100, 100, 0, 100, 50);
                    rect2.SetDrawingObjectParams("color", "yellow");
                    AttachDrawObj(rect2);
                    m_Dic.Add(rect2.ID, RECT2);
                }
            }
            else
            {
                HDrawingObject rect2 = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.RECTANGLE2, 100, 100, 0, 100, 50);
                rect2.SetDrawingObjectParams("color", "yellow");
                AttachDrawObj(rect2);
                m_Dic.Add(rect2.ID, RECT2);
            }
        }

        public void AddRect2(double Row, double Col, double Phi, double Length1, double Length2)
        {
            HDrawingObject rect2 = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.RECTANGLE2, Row, Col, Phi, Length1, Length2);
            rect2.SetDrawingObjectParams("color", "yellow");
            AttachDrawObj(rect2);
            m_Dic.Add(rect2.ID, RECT2);
        }

        //Draw circle
        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
             HDrawingObject.HDrawingObjectType.CIRCLE, 200, 200, 70);
            circle.SetDrawingObjectParams("color", "magenta");
            AttachDrawObj(circle);
            m_Dic.Add(circle.ID, Circle);
        }

        public void AddCircle(int radius = 50, int count = 0)
        {
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                                            HDrawingObject.HDrawingObjectType.CIRCLE, 200, 200, radius);
                    circle.SetDrawingObjectParams("color", "magenta");
                    AttachDrawObj(circle);
                    m_Dic.Add(circle.ID, Circle);
                }
            }
            else
            {
                HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                                                           HDrawingObject.HDrawingObjectType.CIRCLE, 200, 200, radius);
                circle.SetDrawingObjectParams("color", "magenta");
                AttachDrawObj(circle);
                m_Dic.Add(circle.ID, Circle);
            }

        }

        public void AddCircle(double row, double col, double radius)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                                                       HDrawingObject.HDrawingObjectType.CIRCLE, row, col, radius);
            circle.SetDrawingObjectParams("color", "magenta");
            AttachDrawObj(circle);
            m_Dic.Add(circle.ID, Circle);
        }

        //Draw ellipse
        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject ellipse = HDrawingObject.CreateDrawingObject(
             HDrawingObject.HDrawingObjectType.ELLIPSE, 50, 50, 0, 100, 50);
            ellipse.SetDrawingObjectParams("color", "blue");
            AttachDrawObj(ellipse);
            m_Dic.Add(ellipse.ID, Ellipse);
        }

        //Draw line
        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, 50, 50, 100, 50);

            line.SetDrawingObjectParams("color", "red");
            AttachDrawObj(line);
            m_Dic.Add(line.ID, Line);
        }

        //Draw Point
        public void AddPoint(int row = 100, int col = 100)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, row, col, row, col);

            line.SetDrawingObjectParams("color", "red");
            AttachDrawObj(line);
            m_Dic.Add(line.ID, Line);
        }

        //Draw Line
        public HDrawingObject AddLine(double row1, double col1, double row2, double col2)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, row1, col1, row2, col2);

            line.SetDrawingObjectParams("color", "blue");
            AttachDrawObj(line);
            m_Dic.Add(line.ID, Line);

            return line;
        }

        public void AddLineM(double row1, double col1, double row2, double col2)
        {
            HDrawingObject lineM = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, row1, col1, row2, col2);

            lineM.SetDrawingObjectParams("color", "yellow");
            AttachDrawObj(lineM);
            lineM.OnDrag(OnDragDrawingObject);
            m_Dic.Add(lineM.ID, LineM);
        }

        //Draw Nurbs
        public void AddXLD(HTuple hv_Rows, HTuple hv_Cols)
        {
            try
            {
                HTuple hv_DrawID = new HTuple();
                HOperatorSet.CreateDrawingObjectXld(hv_Rows, hv_Cols, out hv_DrawID);
                HOperatorSet.SetDrawingObjectParams(hv_DrawID, "color", "green");
                HOperatorSet.AttachDrawingObjectToWindow(GetWindowHandle(), hv_DrawID);

                m_DrawXldId.Add(hv_DrawID);
            }
            catch (Exception ex)
            {

            }
        }

        List<long> m_DrawXldId = new List<long>();
        public void AddDrawXld()
        {
            HTuple hv_DrawID = new HTuple();
            HOperatorSet.CreateDrawingObjectXld((new HTuple(50)).TupleConcat(100).TupleConcat(50), (new HTuple(50)).TupleConcat(200).TupleConcat(400), out hv_DrawID);
            HOperatorSet.SetDrawingObjectParams(hv_DrawID, "color", "green");
            HOperatorSet.AttachDrawingObjectToWindow(GetWindowHandle(), hv_DrawID);

            m_DrawXldId.Add(hv_DrawID);
        }

        private void circleSectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCircleSector();
        }

        public void AddCircleSector()
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                HDrawingObject.HDrawingObjectType.CIRCLE_SECTOR, 100, 100, 80, 0, 3.14159);
            circle.SetDrawingObjectParams("color", "medium slate blue");
            AttachDrawObj(circle);
            m_Dic.Add(circle.ID, Circle_Sector);
        }

        private void ellipseSectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEllipseSector();
        }
        public void AddEllipseSector()
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                HDrawingObject.HDrawingObjectType.ELLIPSE_SECTOR, 200, 200, 0, 100, 60, 0, 3.14159);
            circle.SetDrawingObjectParams("color", "light steel blue");
            AttachDrawObj(circle);
            m_Dic.Add(circle.ID, Circle);
        }

        //Clear object
        private void clearAllObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAllObjects();
        }

        public void ClearAllObjects()
        {
            foreach (HDrawingObject dobj in drawing_objects)
            {
                HOperatorSet.DetachDrawingObjectFromWindow(GetWindowHandle(), dobj);
                dobj.Dispose();
            }
            drawing_objects.Clear();
            selected_drawing_object = null;
            m_Dic.Clear();
        }

        public void ClearDrawXld()
        {
            if(m_DrawXldId != null && m_DrawXldId.Count != 0)
            {
                foreach (var item in m_DrawXldId)
                {
                    HOperatorSet.DetachDrawingObjectFromWindow(GetWindowHandle(), item);

                }

                m_DrawXldId.Clear();
            }          
        }

        public void ClearSelectObject()
        {
            try
            {
                if (m_Dic.Count() == 1)
                {
                    ClearAllObjects();
                    return;
                }

                m_Dic.Remove(selected_drawing_object.ID);
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (dobj.ID == selected_drawing_object.ID)
                    {
                        drawing_objects.Remove(dobj);
                        dobj.Dispose();
                    }
                }
                selected_drawing_object = null;
            }
            catch (Exception ex)
            {

            }
        }

        //Set Color Red
        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "red");
        }

        //Set Color Yellow
        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "yellow");
        }

        //Set Color Green
        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "green");
        }

        //Set Color Blue
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
                selected_drawing_object.SetDrawingObjectParams("color", "blue");
        }

        //Set LineStyle dashed
        private void dashedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
            {
                HTuple hv_tup = "line_style";
                selected_drawing_object.SetDrawingObjectParams(hv_tup, (((new HTuple(20)).TupleConcat(
                 7)).TupleConcat(3)).TupleConcat(7));
            }
        }

        //Set LineStyle continuous
        private void continuousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_drawing_object != null)
            {
                HTuple hv_tup = "line_style";
                selected_drawing_object.SetDrawingObjectParams(hv_tup, new HTuple());
            }
        }

        public void DisplayImage(HImage image, bool bclearwindow = true)
        {
            if (image == null || image.Key == (IntPtr)0)
            {
                return;
            } 

            BeginInvoke(new Action(() =>
            {
                HOperatorSet.SetSystem("flush_graphic", "false");
                if (bclearwindow)
                {
                    hsmartControl.HalconWindow.ClearWindow();
                }
                image.GetImageSize(out hv_imageWidth, out hv_imageHeight);
                str_imgSize = String.Format("{0}X{1}", hv_imageWidth, hv_imageHeight);
               
                HTuple row1, column1, row2, column2;
                GetWindowHandle().GetPart(out row1, out column1, out row2, out column2);
                //if(row1.D != 0)
                //{
                //    hsmartControl.HalconWindow.SetPart(0, 0, hv_imageHeight - 0, hv_imageWidth - 0);
                //}
                background_image = image;
                hsmartControl.HalconWindow.AttachBackgroundToWindow(background_image);
                HOperatorSet.SetSystem("flush_graphic", "true");

                ShowCrossHair();
            }));
        }


        public void DisplayImageNo(HImage image)
        {
            if (image == null || image.Key == (IntPtr)0)
            {
                return;
            } 

            HOperatorSet.SetSystem("flush_graphic", "false"); 
            hsmartControl.HalconWindow.ClearWindow();
            image.GetImageSize(out hv_imageWidth, out hv_imageHeight);
            str_imgSize = String.Format("{0}X{1}", hv_imageWidth, hv_imageHeight);

            HTuple row1, column1, row2, column2;
            GetWindowHandle().GetPart(out row1, out column1, out row2, out column2); 
            hsmartControl.HalconWindow.AttachBackgroundToWindow(image);
            HOperatorSet.SetSystem("flush_graphic", "true");

            ShowCrossHair();
        }

        private void CrossHair_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_bShowHat = CrossHair.Checked;
                ShowCrossHair();
            }
            catch (Exception ex)
            {

            }
        }
        //显示十字
        private void ShowCrossHair()
        {
            var window = hsmartControl.HalconWindow;
            if (m_bShowHat)
            {
                //获取当前显示信息
                HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
                int hv_lineWidth;

                window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

                hv_lineWidth = (int)window.GetLineWidth();
                string hv_Draw = window.GetDraw();
                window.SetLineWidth(1);//设置线宽
                window.SetLineStyle(new HTuple());
                window.SetColor("#00ff00c0");//十字架显示颜色
                double CrossCol = (double)hv_imageWidth / 2.0, CrossRow = (double)hv_imageHeight / 2.0;
                double borderWidth = (double)hv_imageWidth / 50.0;
                CrossCol = (double)hv_imageWidth / 2.0;
                CrossRow = (double)hv_imageHeight / 2.0;
                //竖线1
                //window.DispLine(0, CrossCol, CrossRow - 50, CrossCol);

                //window.DispLine(CrossRow + 50, CrossCol, imageHeight, CrossCol);

                window.DispPolygon(new HTuple(0, CrossRow - 50), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow + 50, (int)hv_imageHeight), new HTuple(CrossCol, CrossCol));


                //中心点
                window.DispPolygon(new HTuple(CrossRow - 5, CrossRow + 5), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol - 5, CrossCol + 5));

                //横线

                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(0, CrossCol - 50));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol + 50, (int)hv_imageWidth));


                //window.DispLine(CrossRow, 0, CrossRow, CrossCol - 50);
                //window.DispLine(CrossRow, CrossCol + 50, CrossRow, imageWidth);

                //恢复窗口显示信息
                window.SetRgb(hv_Red, hv_Green, hv_Blue);
                window.SetLineWidth(hv_lineWidth);
                window.SetDraw(hv_Draw);
            }
            else
            {
                window.ClearWindow();
                window.SetColor("black");
                window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
        }

        private void TestDistance_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (dobj.Handle != null)
                    {
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Line)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            HTuple hv_Distance;
                            HOperatorSet.DistancePp(row1, column1, row2, column2, out hv_Distance);

                            HTuple hv_Angle;
                            HOperatorSet.AngleLx(row2, column2, row1, column1, out hv_Angle);
                            HOperatorSet.TupleDeg(hv_Angle, out hv_Angle);

                            double rowDistance = Math.Abs(row2 - row1);
                            double colDistance = Math.Abs(column2 - column1);

                            MessageBox.Show(string.Format("直线距离: {0:f2} px\r\nRow距离: {1:f2} px\r\nCol距离: {2:f2} px\r\n角度: {3:f2}",
                                hv_Distance.D, rowDistance, colDistance, hv_Angle.D),
                                "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region HSmartControl事件响应

        private void hsmartControl_Load(object sender, EventArgs e)
        {
            this.MouseWheel += hsmartControl.HSmartWindowControl_MouseWheel;
            prevsize.Width = Width;
            prevsize.Height = Height;
        }

        private void hsmartControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (Image != null)
            {
                try
                {
                    DisplayValue(false);
                    //Cursor = cursor; 
                    string str_value;
                    string str_position = "";
                    bool _isXOut = true, _isYOut = true;
                    HTuple channel_count;

                    HOperatorSet.CountChannels(Image, out channel_count);
                    Cursor = Cursors.Default;
                    positionX = e.X;
                    positionY = e.Y;
                    //hsmartControl.HalconWindow.GetTposition(out positionY, out positionX);
                    //hsmartControl.HalconWindow.GetMposition(out positionY, out positionX, out button_state);
                    str_position = String.Format("Row: {0:0.#} Col: {1:0.#}", positionY, positionX);
                    //var aa = hsmartControl.HalconWindow.QueryGray();

                    _isXOut = (positionX < 0 || positionX >= hv_imageWidth);
                    _isYOut = (positionY < 0 || positionY >= hv_imageHeight);

                    if (!_isXOut && !_isYOut)
                    {
                        if ((int)channel_count == 1)
                        {
                            double grayVal;
                            grayVal = Image.GetGrayval((int)positionY, (int)positionX);
                            grayValue = String.Format("{0:0.###}", grayVal);
                            str_value = String.Format("Val: {0:0.###}", grayVal);
                        }
                        else if ((int)channel_count == 3)
                        {
                            double grayValRed, grayValGreen, grayValBlue;

                            HImage _RedChannel, _GreenChannel, _BlueChannel;

                            _RedChannel = Image.AccessChannel(1);
                            _GreenChannel = Image.AccessChannel(2);
                            _BlueChannel = Image.AccessChannel(3);

                            grayValRed = _RedChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValGreen = _GreenChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValBlue = _BlueChannel.GetGrayval((int)positionY, (int)positionX);

                            _RedChannel.Dispose();
                            _GreenChannel.Dispose();
                            _BlueChannel.Dispose();

                            grayValue = String.Format("({0:0.#}, {1:0.#}, {2:0.#})", grayValRed, grayValGreen, grayValBlue);
                            str_value = String.Format("Val: ({0:0.#}, {1:0.#}, {2:0.#})", grayValRed, grayValGreen, grayValBlue);
                        }
                        else
                        {
                            str_value = "";
                        }
                        info.Text = str_imgSize + " " + str_position + " " + str_value;

                        GetGray();
                        DisplayValue(true);
                        //DaubRegion(DuabPath, DuabSize);
                    }
                }
                catch (Exception ex)
                {
                    //不处理
                }
            }
        }

        private double hv_lastX, hv_lastY;
        private double hv_firstX, hv_firstY;
        bool m_bMouseDown = false;
        public bool m_bRightDown = false;
        private void hsmartControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (hsmartControl.Focused == false)
                    hsmartControl.Focus();

                hv_firstX = e.Y;
                hv_firstY = e.X;

                if (e.Button == MouseButtons.Left)
                {
                    m_bMouseDown = true;
                }

                OnMouseAddPointEvent(e);
            }
            catch (Exception ex)
            {

            }
        }

        public event EventHandler<HMouseEventArgs> MouseAddPointEvent;
        protected void OnMouseAddPointEvent(HMouseEventArgs e)
        {
            MouseAddPointEvent?.Invoke(this, e);
        }

        bool m_b3DView = false;
        private void hsmartControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            hv_lastX = e.Y;
            hv_lastY = e.X;
            m_bMouseDown = false;
            if (!m_b3DView)
            {
                return;
            }

            //interactive_3d_plot(Image, GetWindowHandle(), "texture", (((new HTuple("plot_quality")).TupleConcat(
            //  "show_coordinates")).TupleConcat("step")).TupleConcat("display_grid"), (((new HTuple("best")).TupleConcat(
            //    0))).TupleConcat((new HTuple(1)).TupleConcat("false")));
        }

        private void HSmartWindow_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Size delta = new Size(Width - prevsize.Width, Height - prevsize.Height);
                hsmartControl.Size += delta;

                prevsize.Width = Width;
                prevsize.Height = Height;
            }
            catch (Exception ex)
            {
                 
            } 
        }         

        #endregion

        #region Attach与Select图形
        public void AttachDrawObj(HDrawingObject obj)
        {
            hsmartControl.Focus();
            drawing_objects.Add(obj);

            obj.OnAttach(OnSelectDrawingObject);
            obj.OnSelect(OnSelectDrawingObject);

            if (selected_drawing_object == null)
                selected_drawing_object = obj;
            hsmartControl.HalconWindow.AttachDrawingObjectToWindow(obj);
        }

        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                m_Dic.TryGetValue(dobj.ID, out m_Status);

                selected_drawing_object = dobj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public HDrawingObject GetSelectedDarwObjType()
        {
            return selected_drawing_object;
        }

        #endregion

        #region FitImageToWindow GetWindowHandle 显示数据 结果
        public HWindow GetWindowHandle()
        {
            return hsmartControl.HalconWindow;
        }

        public void FitImageToWindow(HObject image, HObject dispObj, bool isNeedDispose = false, string strResult = "", bool bclearwindow = true)
        {
            if ((null == image) || (!image.IsInitialized()) || image.Key == (IntPtr)0)
            {
                return;
            }

            // hsmartControl.HImagePart = new Rectangle(0, 0, hv_imageHeight, hv_imageWidth);
            //GetWindowHandle().SetPart(0, 0, -2, -2);

            DispImage(new HImage(image), false, bclearwindow);

            if (dispObj != null)
            {
                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("red");
                GetWindowHandle().DispObj(dispObj);
            }

            DisplayResult(strResult);

            //释放内存
            if (isNeedDispose)
            {
                if (dispObj != null)
                {
                    dispObj.Dispose();
                }

                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// 显示结果数据到图形
        /// </summary>
        /// <param name="strValue">显示信息</param>
        string m_strValue = "";
        public void DisplayResult(string strValue, string color = "green", string font = "default-Bold-13")
        {
            try
            {
                if(InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (strValue == null)
                        {
                            return;
                        }

                        if (strValue == "")
                        {
                            strValue = " ";
                        }
                        m_strValue = strValue;

                        if (result_drawing_object != null)
                        {
                            //HOperatorSet.ClearDrawingObject(result_drawing_object);
                            result_drawing_object.Dispose();
                            result_drawing_object = null;
                        }

                        result_drawing_object = HDrawingObject.CreateDrawingObject(
                            HDrawingObject.HDrawingObjectType.TEXT, 0, 0, strValue);
                        result_drawing_object.SetDrawingObjectParams("color", color);
                        result_drawing_object.SetDrawingObjectParams("font", font);
                        GetWindowHandle().AttachDrawingObjectToWindow(result_drawing_object);
                    }));
                }
                else
                {
                    if (strValue == null)
                    {
                        return;
                    }

                    if (strValue == "")
                    {
                        strValue = " ";
                    }
                    m_strValue = strValue;

                    if (result_drawing_object != null)
                    {
                        //HOperatorSet.ClearDrawingObject(result_drawing_object);
                        result_drawing_object.Dispose();
                        result_drawing_object = null;
                    }

                    result_drawing_object = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.TEXT, 0, 0, strValue);
                    result_drawing_object.SetDrawingObjectParams("color", color);
                    result_drawing_object.SetDrawingObjectParams("font", font);
                    GetWindowHandle().AttachDrawingObjectToWindow(result_drawing_object);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 显示项目结果到图像边缘 true:绿色 false:红色
        /// </summary>
        /// <param name="bOK"></param>
        public void ShowResult(bool bOK, bool bReset = false)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    Color color = bOK ? Color.Lime : Color.HotPink;
                    if(bReset)
                    {
                        color = Color.Transparent;
                    }
                    panel_1.BackColor = color;
                    panel_2.BackColor = color;
                    panel_3.BackColor = color;
                    panel_4.BackColor = color;
                }));
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 定位区域界面
        List<long> m_MaskAreaId = new List<long>();
        //保存掩模区域
        public void SaveFixedMaskArea(string Modelpath)
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion, ho_ImageReduced;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (!m_MaskAreaId.Contains(dobj.ID))
                    {
                        continue;
                    }
                    // if (dobj.Handle != (IntPtr)0)
                    if (dobj.Handle != null)
                    {
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if (status == RECT2)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple phi = dobj.GetDrawingObjectParams("phi");
                            HTuple length1 = dobj.GetDrawingObjectParams("length1");
                            HTuple length2 = dobj.GetDrawingObjectParams("length2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                    }
                }

                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + ".hobj");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().SetColor("yellow");
                    GetWindowHandle().DispObj(ho_RegionUnion);
                    ClearAllObjects();
                }

                //if (!Modelpath.Contains("Mask"))//如果是掩模则不创建模板
                //{
                //    CreateShapeModel(ho_RegionUnion, Modelpath);
                //}
            }
            catch (Exception ex)
            {

            }
        }

        //保存定位区域
        public void SaveFixedArea(string Modelpath)
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion, ho_ImageReduced;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);

                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (dobj.Handle != null /*&& m_Dic.Last().Key == dobj.ID*/)
                    {
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if (status == RECT2)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple phi = dobj.GetDrawingObjectParams("phi");
                            HTuple length1 = dobj.GetDrawingObjectParams("length1");
                            HTuple length2 = dobj.GetDrawingObjectParams("length2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                    }
                }


                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "Region.hobj");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().SetColor("green");
                    GetWindowHandle().DispObj(ho_RegionUnion);
                    ClearAllObjects();
                }

                m_MaskAreaId.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        //保存定位区域
        public void SaveSearchRegion(string Modelpath)
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion, ho_ImageReduced;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);

                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (dobj.Handle != null && m_Dic.Last().Key == dobj.ID)
                    {
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                        else if(status == RECT2)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple phi = dobj.GetDrawingObjectParams("phi");
                            HTuple length1 = dobj.GetDrawingObjectParams("length1");
                            HTuple length2 = dobj.GetDrawingObjectParams("length2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            bSave = true;
                        }
                    }
                }

                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "SearchRegion.hobj");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().SetColor("blue");
                    GetWindowHandle().DispObj(ho_RegionUnion);
                    ClearAllObjects();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateShapeModel(List<object> listValue)
        {
            //先保存掩模区域
            string Modelpath = listValue[0].ToString();
            SaveFixedMaskArea(Modelpath + "MaskRegion");

            HObject ho_ImageReduced;
            HObject ho_ContoursAffinTrans;
            HObject ho_RegionUnion;
            HObject ho_MaskRegion;
            // Local control variables  

            HTuple hv_ModelID = null, hv_Row3 = null;
            HTuple hv_Column3 = null, hv_Angle = null, hv_Score = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_MaskRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);


            double StartingAngle = Convert.ToDouble(listValue[1]);
            double AngleExtent = Convert.ToDouble(listValue[2]);
            int PyramidLevel = Convert.ToInt32(listValue[3]);
            int LastPyramidLevel = Convert.ToInt32(listValue[4]);
            double MinScore = Convert.ToDouble(listValue[5]);
            double Greediness = Convert.ToDouble(listValue[6]);

            string regionPath = Modelpath + "Region.hobj";
            ho_RegionUnion.Dispose();
            HOperatorSet.ReadRegion(out ho_RegionUnion, regionPath);

            string maskregionPath = Modelpath + "MaskRegion.hobj";//扣掉掩模区域
            if (File.Exists(maskregionPath))
            {
                ho_MaskRegion.Dispose();
                HOperatorSet.ReadRegion(out ho_MaskRegion, maskregionPath);
                HOperatorSet.Difference(ho_RegionUnion, ho_MaskRegion, out ho_RegionUnion);
            }

            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(Image, ho_RegionUnion, out ho_ImageReduced);
            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", (new HTuple(StartingAngle)).TupleRad()
                , (new HTuple(AngleExtent)).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto",
                out hv_ModelID);


            HOperatorSet.FindShapeModel(Image, hv_ModelID, (new HTuple(StartingAngle)).TupleRad()
           , (new HTuple(AngleExtent)).TupleRad(), MinScore, 1, 0.5, "least_squares", (new HTuple(PyramidLevel)).TupleConcat(LastPyramidLevel), Greediness, out hv_Row3,
           out hv_Column3, out hv_Angle, out hv_Score);

            ho_ContoursAffinTrans.Dispose();
            dev_display_shape_matching_results(hv_ModelID, "red", hv_Row3, hv_Column3, hv_Angle,
               1, 1, 0, out ho_ContoursAffinTrans);

            HTuple tup = new HTuple();
            tup.Append(hv_Row3);
            tup.Append(hv_Column3);
            tup.Append(hv_Angle);

            HOperatorSet.WriteTuple(tup, Modelpath + "ModelTup.tup");

            //保存模板和图片
            HOperatorSet.WriteShapeModel(hv_ModelID, Modelpath + "ModelID.shm");
            if (Image != null && Image.IsInitialized() && Image.Key != (IntPtr)0)
            {
                HOperatorSet.WriteImage(Image, "png", 0, Modelpath + "ModelImage.png");
                //Global.InitialImage = Image;
            }

            if (ho_ContoursAffinTrans != null)
            {
                GetWindowHandle().DispObj(Image);
                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("red");
                GetWindowHandle().DispObj(ho_ContoursAffinTrans);

                Contour = ho_ContoursAffinTrans;
            }
            ho_ImageReduced.Dispose();
            HOperatorSet.ClearShapeModel(hv_ModelID);
            ClearAllObjects();
        }

        public void CreateXldModel(List<object> listValue)
        {
            HObject ho_Contour;
            HObject ho_ContoursAffinTrans;
            HObject ho_RegionUnion;
            HObject ho_MaskRegion;
            // Local control variables  

            HTuple hv_ModelID = null, hv_Row3 = null;
            HTuple hv_Column3 = null, hv_Angle = null, hv_Score = null;
            // Initialize local and output iconic variables   
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_MaskRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);


            string Modelpath = listValue[0].ToString();

            ho_Contour = (HObject)listValue[8];
            double StartingAngle = Convert.ToDouble(listValue[1]);
            double AngleExtent = Convert.ToDouble(listValue[2]);
            int PyramidLevel = Convert.ToInt32(listValue[3]);
            int LastPyramidLevel = Convert.ToInt32(listValue[4]);
            double MinScore = Convert.ToDouble(listValue[5]);
            double Greediness = Convert.ToDouble(listValue[6]);


            HOperatorSet.CreateShapeModelXld(ho_Contour, "auto", (new HTuple(StartingAngle)).TupleRad()
                , (new HTuple(AngleExtent)).TupleRad(), "auto", "auto", "ignore_local_polarity", 5,
                out hv_ModelID);

            HOperatorSet.FindShapeModel(Image, hv_ModelID, (new HTuple(StartingAngle)).TupleRad()
           , (new HTuple(AngleExtent)).TupleRad(), MinScore, 1, 0.5, "least_squares", (new HTuple(PyramidLevel)).TupleConcat(LastPyramidLevel), Greediness, out hv_Row3,
           out hv_Column3, out hv_Angle, out hv_Score);

            ho_ContoursAffinTrans.Dispose();
            dev_display_shape_matching_results(hv_ModelID, "red", hv_Row3, hv_Column3, hv_Angle,
               1, 1, 0, out ho_ContoursAffinTrans);

            HTuple tup = new HTuple();
            tup.Append(hv_Row3);
            tup.Append(hv_Column3);
            tup.Append(hv_Angle);

            HOperatorSet.WriteTuple(tup, Modelpath + "ModelTup.tup");

            //保存模板和图片
            HOperatorSet.WriteShapeModel(hv_ModelID, Modelpath + "ModelID.shm");
            if (Image != null && Image.IsInitialized() && Image.Key != (IntPtr)0)
            {
                HOperatorSet.WriteImage(Image, "png", 0, Modelpath + "ModelImage.png");
            }

            if (ho_ContoursAffinTrans != null)
            {
                GetWindowHandle().DispObj(Image);
                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("red");
                GetWindowHandle().DispObj(ho_ContoursAffinTrans);

                Contour = ho_ContoursAffinTrans;
            }
            HOperatorSet.ClearShapeModel(hv_ModelID);
        }

        public object[] FindShapeModel(List<object> listValue)
        {
            try
            {
                HTuple hv_ModelID = null, hv_Row3 = null;
                HTuple hv_Column3 = null, hv_Angle = null, hv_Score = null;
                HObject ho_ContoursAffinTrans;
                HObject ho_SearchRegion;
                HObject ho_ReducedImage;
                HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
                HOperatorSet.GenEmptyObj(out ho_SearchRegion);
                HOperatorSet.GenEmptyObj(out ho_ReducedImage);

                string Modelpath = listValue[0].ToString();

                double StartingAngle = Convert.ToDouble(listValue[1]);
                double AngleExtent = Convert.ToDouble(listValue[2]);
                int PyramidLevel = Convert.ToInt32(listValue[3]);
                int LastPyramidLevel = Convert.ToInt32(listValue[4]);
                double MinScore = Convert.ToDouble(listValue[5]);
                double Greediness = Convert.ToDouble(listValue[6]);
                int NumMatchs = Convert.ToInt32(listValue[7]);
                bool IsSearchArea = (bool)listValue[8];

                string modelPath = Modelpath + "ModelID.shm";

                HOperatorSet.ReadShapeModel(modelPath, out hv_ModelID);

                string searchPath = Modelpath + "SearchRegion.hobj";
                if (File.Exists(searchPath) && IsSearchArea)
                {
                    ho_SearchRegion.Dispose();
                    HOperatorSet.ReadRegion(out ho_SearchRegion, searchPath);

                    ho_ReducedImage.Dispose();
                    HOperatorSet.ReduceDomain(Image, ho_SearchRegion, out ho_ReducedImage);
                    HOperatorSet.FindShapeModel(ho_ReducedImage, hv_ModelID, (new HTuple(StartingAngle)).TupleRad()
                    , (new HTuple(AngleExtent)).TupleRad(), MinScore, NumMatchs, 0.5, "least_squares", (new HTuple(PyramidLevel)).TupleConcat(LastPyramidLevel), Greediness, out hv_Row3,
                      out hv_Column3, out hv_Angle, out hv_Score);
                }
                else
                {
                    HOperatorSet.FindShapeModel(Image, hv_ModelID, (new HTuple(StartingAngle)).TupleRad()
                        , (new HTuple(AngleExtent)).TupleRad(), MinScore, NumMatchs, 0.5, "least_squares",
                        (new HTuple(PyramidLevel)).TupleConcat(LastPyramidLevel), Greediness, out hv_Row3,
                        out hv_Column3, out hv_Angle, out hv_Score);
                }

                ho_ContoursAffinTrans.Dispose();
                dev_display_shape_matching_results(hv_ModelID, "red", hv_Row3, hv_Column3, hv_Angle,
                   1, 1, 0, out ho_ContoursAffinTrans);

                HOperatorSet.ClearShapeModel(hv_ModelID);

                string strResult = "";
                if (ho_ContoursAffinTrans != null)
                {
                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().DispObj(ho_ContoursAffinTrans);

                    //显示结果到界面
                    HTuple hv_Deg = new HTuple();
                    HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
                    strResult = "Row:" + hv_Row3.D.ToString("0.0") + " " +
                                        "Col:" + hv_Column3.D.ToString("0.0") + " " +
                                        "Angle:" + hv_Deg.D.ToString("0.0") + " " +
                                        "Score:" + hv_Score.D.ToString("0.00") + " " +
                                        "Count:" + hv_Row3.Length;
                    DisplayResult(strResult);

                    ShowMatch(hv_Row3.D, hv_Column3.D, hv_Angle.D, hv_Score.D);
                }

                GetWindowHandle().SetLineWidth(2);
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().SetColor("blue");
                GetWindowHandle().DispObj(ho_SearchRegion);

                //ho_ContoursAffinTrans.Dispose();
                ho_ReducedImage.Dispose();

                if (hv_Row3.Length != 0)
                {
                    return new object[6] { Math.Round(hv_Row3.D, 3), Math.Round(hv_Column3.D, 3), Math.Round(hv_Angle.D, 3),
                        ho_ContoursAffinTrans, strResult, hv_Row3.Length};
                }
                else
                {
                    return new object[6] { 0, 0, 0, null, "", 0 };
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ShowModelRegion(List<object> listValue)
        {
            try
            {
                HTuple hv_ModelID = null, hv_Row3 = null;
                HTuple hv_Column3 = null, hv_Angle = null, hv_Score = null;
                HObject ho_RegionUnion;
                HObject ho_ImageReduced;
                HObject ho_ContoursAffinTrans;
                HObject ho_MaskRegion;
                HObject ho_SearchRegion;
                HOperatorSet.GenEmptyObj(out ho_MaskRegion);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_SearchRegion);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);

                string Modelpath = listValue[0].ToString();

                double StartingAngle = Convert.ToDouble(listValue[1]);
                double AngleExtent = Convert.ToDouble(listValue[2]);
                int PyramidLevel = Convert.ToInt32(listValue[3]);
                int LastPyramidLevel = Convert.ToInt32(listValue[4]);
                double MinScore = Convert.ToDouble(listValue[5]);
                double Greediness = Convert.ToDouble(listValue[6]);

                string modelPath = Modelpath + "ModelID.shm";

                ho_RegionUnion.Dispose();
                HOperatorSet.ReadRegion(out ho_RegionUnion, Modelpath + "Region.hobj");

                if (File.Exists(Modelpath + "MaskRegion.hobj"))
                {
                    // ho_MaskRegion.Dispose();
                    //HOperatorSet.ReadRegion(out ho_MaskRegion, Modelpath + "MaskRegion.hobj");//扣掉掩模区域
                    File.Delete(Modelpath + "MaskRegion.hobj");
                }

                if (File.Exists(Modelpath + "SearchRegion.hobj"))
                {
                    ho_SearchRegion.Dispose();
                    HOperatorSet.ReadRegion(out ho_SearchRegion, Modelpath + "SearchRegion.hobj");//扣掉掩模区域
                }

                ho_ImageReduced.Dispose();
                ho_ContoursAffinTrans.Dispose();

                HOperatorSet.ReadShapeModel(modelPath, out hv_ModelID);

                HOperatorSet.FindShapeModel(Image, hv_ModelID, (new HTuple(StartingAngle)).TupleRad()
                      , (new HTuple(AngleExtent)).TupleRad(), MinScore, 1, 0.5, "least_squares", (new HTuple(PyramidLevel)).TupleConcat(LastPyramidLevel), Greediness, out hv_Row3,
                      out hv_Column3, out hv_Angle, out hv_Score);

                ho_ContoursAffinTrans.Dispose();
                dev_display_shape_matching_results(hv_ModelID, "red", hv_Row3, hv_Column3, hv_Angle,
                   1, 1, 0, out ho_ContoursAffinTrans);

                //根据模板位置跟随
                HTuple hv_Tuple = new HTuple();
                HOperatorSet.ReadTuple(Modelpath + "ModelTup.tup", out hv_Tuple);
                HTuple hv_HomMat2D = new HTuple();
                HOperatorSet.VectorAngleToRigid(hv_Tuple.TupleSelect(0), hv_Tuple.TupleSelect(
                    1), hv_Tuple.TupleSelect(2), hv_Row3, hv_Column3, hv_Angle, out hv_HomMat2D);

                HOperatorSet.AffineTransRegion(ho_RegionUnion, out ho_RegionUnion, hv_HomMat2D,
                    "nearest_neighbor");
                HOperatorSet.AffineTransRegion(ho_MaskRegion, out ho_MaskRegion, hv_HomMat2D,
                    "nearest_neighbor");


                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("red");
                GetWindowHandle().DispObj(ho_ContoursAffinTrans);
                Contour = ho_ContoursAffinTrans;


                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("green");
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().DispObj(ho_RegionUnion);

                //GetWindowHandle().SetLineWidth(1);
                //GetWindowHandle().SetColor("#f0e68c80");
                //GetWindowHandle().SetDraw("fill");
                //GetWindowHandle().DispObj(ho_MaskRegion);


                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("blue");
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().DispObj(ho_SearchRegion);

                ho_ImageReduced.Dispose();
                //ho_ContoursAffinTrans.Dispose();
                //ho_RegionUnion.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowMatchingModelRegion(List<object> listValue)
        {
            try
            {
                HObject ho_RegionUnion;
                HObject ho_MaskRegion;
                HObject ho_SearchRegion;
                HOperatorSet.GenEmptyObj(out ho_MaskRegion);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_SearchRegion);

                string Modelpath = listValue[0].ToString();

                ho_RegionUnion.Dispose();
                string strRegion = Modelpath + "Region.hobj";
                if (File.Exists(strRegion))
                { 
                    HOperatorSet.ReadRegion(out ho_RegionUnion, Modelpath + "Region.hobj"); 
                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("green");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_RegionUnion);
                }

                ho_MaskRegion.Dispose();
                string strMaskRegion = Modelpath + "MaskRegion.hobj";
                if (File.Exists(strMaskRegion))
                {
                    File.Delete(strMaskRegion);
                }

                ho_SearchRegion.Dispose();
                string strSearchRegion = Modelpath + "SearchRegion.hobj";
                if (File.Exists(strSearchRegion))
                { 
                    HOperatorSet.ReadRegion(out ho_SearchRegion, strSearchRegion);//搜索区域
                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("blue");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_SearchRegion);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AddMaskAreaDone()
        {
            try
            {
                m_MaskAreaId.Add(m_Dic.Last().Key);
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region 基准界面保存显示区域

        List<long> m_BaseLevelAreaId = new List<long>();
        public void SaveBaseRegion(string Modelpath)//可以保存多个区域
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (!m_BaseLevelAreaId.Contains(dobj.ID))
                    {
                        continue;
                    }
                    //HDrawingObject dobj = selected_drawing_object;
                    // if (dobj.Handle != (IntPtr)0)
                    if (dobj.Handle != null)
                    {
                        bSave = true;
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.Union2(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                        }

                    }

                    if (bSave)
                    {
                        HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "BaseRegion.hobj");

                        GetWindowHandle().SetLineWidth(2);
                        GetWindowHandle().SetColor("blue");
                        GetWindowHandle().SetDraw("margin");
                        GetWindowHandle().DispObj(ho_RegionUnion);
                    }
                }

                m_BaseLevelAreaId.Clear();
                ClearAllObjects();
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowBaseRegion(string Modelpath)
        {
            try
            {
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                string regionPath = Modelpath + "BaseRegion.hobj";
                ho_RegionUnion.Dispose();
                HOperatorSet.ReadRegion(out ho_RegionUnion, regionPath);

                GetWindowHandle().SetLineWidth(2);
                GetWindowHandle().SetColor("blue");
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().DispObj(ho_RegionUnion);
            }
            catch (Exception ex)
            {

            }
        }

        public void AddBaseLevelAreaDone(int count)
        {
            try
            {
                Dictionary<long, string> Dic = new Dictionary<long, string>();
                foreach (var item in m_Dic)
                {
                    Dic.Add(item.Key, item.Value);
                }

                for (int i = 0; i < count; i++)
                {
                    m_BaseLevelAreaId.Add(Dic.Last().Key);
                    Dic.Remove(Dic.Last().Key);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 检测区界面保存显示区域
        List<long> m_CheckAreaId = new List<long>();
        public void SaveCheckRegion(string Modelpath)
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                // HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (!m_CheckAreaId.Contains(dobj.ID))
                    {
                        continue;
                    }
                    // if (dobj.Handle != (IntPtr)0)
                    if (dobj.Handle != null)
                    {
                        bSave = true;
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                        }
                        else if (status == RECT2)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple length1 = dobj.GetDrawingObjectParams("length1");
                            HTuple length2 = dobj.GetDrawingObjectParams("length2");
                            HTuple phi = dobj.GetDrawingObjectParams("phi");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                        }
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "CheckRegion.hobj");

                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("yellow");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_RegionUnion);
                }
                m_CheckAreaId.Clear();
                ClearAllObjects();
            }
            catch (Exception ex)
            {

            }
        }
        public void ShowCheckRegion(string Modelpath)
        {
            try
            {
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                string regionPath = Modelpath + "CheckRegion.hobj";
                ho_RegionUnion.Dispose();
                HOperatorSet.ReadRegion(out ho_RegionUnion, regionPath);

                GetWindowHandle().SetLineWidth(2);
                GetWindowHandle().SetColor("yellow");
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().DispObj(ho_RegionUnion);
            }
            catch (Exception ex)
            {

            }
        }
        public void AddCheckAreaDone()
        {
            try
            {
                m_CheckAreaId.Add(m_Dic.Last().Key);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 矩形阵列保存区域
        public void SaveArrayRegion(string Modelpath)
        {
            try
            {
                HObject ho_Circle, ho_Rectangle;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                bool bSave = false;
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    // if (dobj.Handle != (IntPtr)0)
                    if (dobj.Handle != null)
                    {
                        bSave = true;
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == Circle)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple radius = dobj.GetDrawingObjectParams("radius");

                            ho_Circle.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                        }
                        else if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                        }
                        else if (status == RECT2)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row");
                            HTuple column1 = dobj.GetDrawingObjectParams("column");
                            HTuple phi = dobj.GetDrawingObjectParams("phi");
                            HTuple length1 = dobj.GetDrawingObjectParams("length1");
                            HTuple length2 = dobj.GetDrawingObjectParams("length2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                            HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                        }
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "ArrayRegion.hobj");

                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("blue");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_RegionUnion);

                    ClearAllObjects();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 标定显示区域
        public List<double> GetRectArea()
        {
            List<double> listDouble = new List<double>();
            try
            {
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == RECT1)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row1");
                        HTuple column1 = dobj.GetDrawingObjectParams("column1");
                        HTuple row2 = dobj.GetDrawingObjectParams("row2");
                        HTuple column2 = dobj.GetDrawingObjectParams("column2");

                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(row2.D, 3));
                        listDouble.Add(Math.Round(column2.D, 3));
                    }
                    else if (status == RECT2)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple phi = dobj.GetDrawingObjectParams("phi");
                        HTuple length1 = dobj.GetDrawingObjectParams("length1");
                        HTuple length2 = dobj.GetDrawingObjectParams("length2");

                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(phi.D, 3));
                        listDouble.Add(Math.Round(length1.D, 3));
                        listDouble.Add(Math.Round(length2.D, 3));
                    }
                    else if (status == Circle)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple radius = dobj.GetDrawingObjectParams("radius");

                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(radius.D, 3));
                    }
                    else if (status == Circle_Sector2 && dobj.ID != m_Circle2.ID)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple radius = dobj.GetDrawingObjectParams("radius");
                        HTuple startPhi = dobj.GetDrawingObjectParams("start_angle");
                        HTuple endphi = dobj.GetDrawingObjectParams("end_angle");

                        HTuple radius2 = m_Circle2.GetDrawingObjectParams("radius");

                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(radius.D, 3));
                        listDouble.Add(Math.Round(startPhi.D, 3));
                        listDouble.Add(Math.Round(endphi.D, 3));
                        listDouble.Add(Math.Round((radius - radius2).D, 3));
                    }
                    else if (status == Double_Circle && dobj.ID != m_Circle2.ID)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple radius = dobj.GetDrawingObjectParams("radius");

                        HTuple radius2 = m_Circle2.GetDrawingObjectParams("radius");

                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(radius.D, 3));
                        listDouble.Add(Math.Round((radius - radius2).D, 3));
                    }
                    else if (status == LineM)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row1");
                        HTuple column1 = dobj.GetDrawingObjectParams("column1");
                        HTuple row2 = dobj.GetDrawingObjectParams("row2");
                        HTuple column2 = dobj.GetDrawingObjectParams("column2");
                        listDouble.Add(Math.Round(row1.D, 3));
                        listDouble.Add(Math.Round(column1.D, 3));
                        listDouble.Add(Math.Round(row2.D, 3));
                        listDouble.Add(Math.Round(column2.D, 3));
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return listDouble;
        }
        #endregion

        #region 找圆保存显示区域
        public void SaveCircleRegion(string Modelpath)
        {
            try
            {
                HObject ho_Circle;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;
                HTuple hv_CircleTup = new HTuple();

                // if (dobj.Handle != (IntPtr)0)
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Circle)
                    {
                        bSave = true;
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple radius = dobj.GetDrawingObjectParams("radius");

                        hv_CircleTup.Append(row1);
                        hv_CircleTup.Append(column1);
                        hv_CircleTup.Append(radius);

                        ho_Circle.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                        HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "CircleRegion.tup");
                }

                //GetWindowHandle().SetDraw("margin");
                //GetWindowHandle().SetLineWidth(2);
                //GetWindowHandle().SetColor("magenta");
                //GetWindowHandle().DispObj(ho_RegionUnion);
                //ClearAllObjects();
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveCircleArcRegion(string Modelpath)
        {
            try
            {
                HObject ho_CircleSector;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_CircleSector);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;
                HTuple hv_CircleTup = new HTuple();

                // if (dobj.Handle != (IntPtr)0)
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Circle_Sector)
                    {
                        bSave = true;
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple radius = dobj.GetDrawingObjectParams("radius");
                        HTuple startPhi = dobj.GetDrawingObjectParams("start_angle");
                        HTuple endphi = dobj.GetDrawingObjectParams("end_angle");

                        hv_CircleTup.Append(row1);
                        hv_CircleTup.Append(column1);
                        hv_CircleTup.Append(radius);
                        hv_CircleTup.Append(startPhi);
                        hv_CircleTup.Append(endphi);

                        ho_CircleSector.Dispose();
                        HOperatorSet.GenCircleSector(out ho_CircleSector, row1, column1, radius, startPhi, endphi);
                        HOperatorSet.Union2(ho_RegionUnion, ho_CircleSector, out ho_RegionUnion);
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "CircleArcRegion.hobj");
                    HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "CircleArcRegion.tup");
                }

                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().SetLineWidth(2);
                GetWindowHandle().SetColor("medium slate blue");
                GetWindowHandle().DispObj(ho_RegionUnion);
                ClearAllObjects();
            }
            catch (Exception ex)
            {

            }
        }
        
        public void AddDrawCircle(double row = 200, double column = 200, int radius = 50)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                                                       HDrawingObject.HDrawingObjectType.CIRCLE, row, column, radius);
            circle.SetDrawingObjectParams("color", "magenta");
            AttachDrawObj(circle);
            HOperatorSet.SetDrawingObjectParams(circle, "line_width", 1);
            circle.OnDrag(OnDragDrawingObject);
            circle.OnResize(OnDragDrawingObject);
            m_Dic.Add(circle.ID, Circle);
        }

        public void AddDrawCircleArc(double row = 200, double column = 200, double radius = 50, double startPhi = 0, double endPhi = 3.14)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                                                       HDrawingObject.HDrawingObjectType.CIRCLE_SECTOR, row, column, radius, startPhi, endPhi);
            circle.SetDrawingObjectParams("color", "medium slate blue");
            AttachDrawObj(circle);
            circle.OnDrag(OnDragDrawingObject);
            circle.OnResize(OnDragDrawingObject);

            m_Dic.Add(circle.ID, Circle_Sector);
        }

        public int m_ArcWidth = 60;
        public HDrawingObject m_Circle2 = null;
        public void AddDrawCircleArc2(double row = 400, double column = 400, double radius = 100, double startPhi = 0, double endPhi = 3.14, double width = 60)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE_SECTOR,
                                                                        row, column, radius, startPhi, endPhi);
            circle.SetDrawingObjectParams("color", "magenta");

            m_Circle2 = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE_SECTOR,
                                                            row, column, radius - width, startPhi, endPhi);
            m_Circle2.SetDrawingObjectParams("color", "magenta");

            AttachDrawObj(circle);
            AttachDrawObj(m_Circle2);
            circle.OnDrag(OnDragDrawingObjectArc2);
            circle.OnResize(OnDragDrawingObjectArc2);
            m_Dic.Add(circle.ID, Circle_Sector2);
        }

        public void AddDrawDoubleCircle(double row = 400, double column = 400, double radius = 100, double width = 60)
        {
            HDrawingObject circle = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE,
                                                                        row, column, radius);
            circle.SetDrawingObjectParams("color", "indian red");

            m_Circle2 = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE,
                                                            row, column, radius - width);
            m_Circle2.SetDrawingObjectParams("color", "indian red");

            AttachDrawObj(circle);
            AttachDrawObj(m_Circle2);
            circle.OnDrag(OnDragDrawingDoubleCircle);
            circle.OnResize(OnDragDrawingDoubleCircle);
            m_Dic.Add(circle.ID, Double_Circle);
        }


        public delegate void Del_FindCircleResult();
        public Del_FindCircleResult m_DelFindCircleResult;
        public void FindCircleAct(bool bArc = false)
        {
            try
            {
                HObject ho_MeasureCross1, ho_CircleContours1, ho_MeasureCircleContours1;

                HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
                HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
                HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
                HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
                HTuple hv_StartPhi = null, hv_PhiRange = null;
                HTuple hv_bDisp = null;
                HTuple hv_bFindCircle2D = null;
                HTuple hv_OutCircleRow = new HTuple();
                HTuple hv_OutCircleCol = new HTuple(), hv_OutCircleRadius = new HTuple();
                HTuple hv_ColEnd1 = new HTuple(), hv_AllRow = new HTuple();
                HTuple hv_AllColumn = new HTuple();
                HTuple hv_CircleCenterRow1 = new HTuple(), hv_CircleCenterColumn1 = new HTuple();
                HTuple hv_CircleRadius1 = new HTuple();

                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours1);
                HOperatorSet.GenEmptyObj(out ho_CircleContours1);
                HOperatorSet.GenEmptyObj(out ho_MeasureCross1);

                hv_CircleCenterRow1 = _FindCircleParam.InCircleRow;
                hv_CircleCenterColumn1 = _FindCircleParam.InCircleCol;
                hv_CircleRadius1 = _FindCircleParam.InSearchRadius;
                hv_InMeasureLength1 = _FindCircleParam.InMeasureLength1;
                hv_InMeasureLength2 = _FindCircleParam.InMeasureLength2;
                hv_InMeasureSigma = _FindCircleParam.InMeasureSigma;
                hv_InMeasureThreshold = _FindCircleParam.InMeasureThreshold;
                hv_InMeasureSelect = _FindCircleParam.InMeasureSelect;
                hv_InMeasureTransition = _FindCircleParam.InMeasureTransition;
                hv_InMeasureNumber = _FindCircleParam.InMeasureNumber;
                hv_InMeasureScore = _FindCircleParam.InMeasureScore;


                hv_StartPhi = _FindCircleParam.StartPhi;
                hv_PhiRange = _FindCircleParam.EndPhi;

                hv_bDisp = 0;


                //输出参数 
                ho_MeasureCircleContours1.Dispose();
                ho_MeasureCross1.Dispose();
                ho_CircleContours1.Dispose();

                FindCircle2D(Image, out ho_MeasureCircleContours1, out ho_MeasureCross1,
                       out ho_CircleContours1, hv_CircleCenterRow1, hv_CircleCenterColumn1, hv_CircleRadius1,
                       hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                       hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                       hv_bDisp, out hv_CircleCenterRow1, out hv_CircleCenterColumn1, out hv_CircleRadius1,
                       out hv_bFindCircle2D, hv_StartPhi, hv_PhiRange, bArc);
                
                if (true)
                {
                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetLineWidth(3);
                    GetWindowHandle().SetColor("spring green");
                    GetWindowHandle().DispObj(ho_CircleContours1);

                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().DispObj(ho_MeasureCircleContours1);
                    GetWindowHandle().DispObj(ho_MeasureCross1);

                    HObject ho_Arrow1;
                    gen_arrow_contour_xld(out ho_Arrow1, hv_CircleCenterRow1 + hv_CircleRadius1 - hv_InMeasureLength1, hv_CircleCenterColumn1,
                        hv_CircleCenterRow1 + hv_CircleRadius1 + hv_InMeasureLength1, hv_CircleCenterColumn1, 5, 5);

                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().SetColor("yellow");
                    GetWindowHandle().SetDraw("margin");
                    DispObj(ho_Arrow1);

                    if (hv_CircleCenterRow1 != null && hv_CircleCenterRow1.Length != 0)
                    {
                        _FindCircleParam.ResultRow = hv_CircleCenterRow1;
                        _FindCircleParam.ResultCol = hv_CircleCenterColumn1;
                        _FindCircleParam.ResultRadius = hv_CircleRadius1;

                        HObject ho_Cross;
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_CircleCenterRow1, hv_CircleCenterColumn1, 30, 0);
                        GetWindowHandle().SetColor("spring green");
                        DispObj(ho_Cross);
                    }
                    else
                    {
                        _FindCircleParam.ResultRow = 0;
                        _FindCircleParam.ResultCol = 0;
                        _FindCircleParam.ResultRadius = 0;
                    }
                    if (m_DelFindCircleResult != null)
                    {
                        m_DelFindCircleResult();
                    }
                    Thread.Sleep(1);
                }

            }
            catch (Exception ex)
            {

            }
        }
        public FindCircleParam _findCircleParam = new FindCircleParam();
        public FindCircleParam _FindCircleParam
        {
            get { return _findCircleParam; }
            set { _findCircleParam = value; }
        }
        public class FindCircleParam
        {
            public double InCircleRow { get; set; }
            public double InCircleCol { get; set; }
            public double InSearchRadius { get; set; }
            public double InMeasureLength1 { get; set; }
            public double InMeasureLength2 { get; set; }
            public double InMeasureSigma { get; set; }
            public double InMeasureThreshold { get; set; }
            public string InMeasureSelect { get; set; }
            public string InMeasureTransition { get; set; }
            public int InMeasureNumber { get; set; }
            public double InMeasureScore { get; set; }

            public double StartPhi { get; set; }
            public double EndPhi { get; set; }

            public double ResultRow { get; set; }
            public double ResultCol { get; set; }
            public double ResultRadius { get; set; }

        }

        public void FindCircle2D(HObject ho_Image, out HObject ho_MeasureCircleContours,
            out HObject ho_MeasureCross, out HObject ho_CircleContours, HTuple hv_InCircleRow,
            HTuple hv_InCircleCol, HTuple hv_InCircleRadiu, HTuple hv_InMeasureLength1,
            HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
            HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
            HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_CircleCenterRow, out HTuple hv_CircleCenterColumn,
            out HTuple hv_CircleRadius, out HTuple hv_bFindCircle2D, double hv_StartPhi = 0, double hv_PhiRange = 360, bool bArc = false)
        {
            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyCircleIndex = new HTuple();
            HTuple hv_CircleParameter = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_CircleContours);
            hv_CircleCenterRow = new HTuple();
            hv_CircleCenterColumn = new HTuple();
            hv_CircleRadius = new HTuple();
            hv_bFindCircle2D = new HTuple();
            try
            {
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_CircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_CircleContours);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                if (bArc)
                {
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_InCircleRow,
                        hv_InCircleCol, hv_InCircleRadiu, hv_InMeasureLength1, hv_InMeasureLength2,
                        hv_InMeasureSigma, hv_InMeasureThreshold, (new HTuple("start_phi")).TupleConcat("end_phi"), (new HTuple(hv_StartPhi)).TupleConcat(hv_PhiRange), out hv_MetrologyCircleIndex);
                }
                else
                {
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_InCircleRow,
                        hv_InCircleCol, hv_InCircleRadiu, hv_InMeasureLength1, hv_InMeasureLength2,
                        hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(), new HTuple(), out hv_MetrologyCircleIndex);
                }
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_measures", (int)hv_InMeasureNumber.D);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_interpolation", "nearest_neighbor");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "instances_outside_measure_regions", "true");


                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "all", "result_type", "all_param", out hv_CircleParameter);
                //Extract the parameters for better readability
                hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                    )) - 1, 3);
                hv_CircleCenterRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                hv_CircleCenterColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);

                ho_CircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_CircleContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureCircleContours, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_Row1, hv_Column1, 8,
                    0.785398);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(hv_bDisp) != 0)
                {
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCircleContours, HDevWindowStack.GetActive()
                            );
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCross, HDevWindowStack.GetActive());
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_CircleContours, HDevWindowStack.GetActive());
                    }
                }
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterRow.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterColumn.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindCircle2D = 1;
                }
                else
                {
                    hv_bFindCircle2D = 0;
                }

            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindCircle2D = 0;
            }


            return;
        }

        #endregion

        #region 找线保存显示区域
        public void SaveLineRegion(string Modelpath)
        {
            try
            {
                HObject ho_Line;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Line);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;

                HTuple hv_Line = new HTuple();
                // if (dobj.Handle != (IntPtr)0)
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Line)
                    {
                        bSave = true;
                        HTuple row1 = dobj.GetDrawingObjectParams("row1");
                        HTuple column1 = dobj.GetDrawingObjectParams("column1");
                        HTuple row2 = dobj.GetDrawingObjectParams("row2");
                        HTuple column2 = dobj.GetDrawingObjectParams("column2");

                        hv_Line.Append(row1);
                        hv_Line.Append(column1);
                        hv_Line.Append(row2);
                        hv_Line.Append(column2);

                        ho_Line.Dispose();
                        HOperatorSet.GenRegionLine(out ho_Line, row1, column1, row2, column2);
                        HOperatorSet.Union2(ho_RegionUnion, ho_Line, out ho_RegionUnion);
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "LineRegion.hobj");
                    HOperatorSet.WriteTuple(hv_Line, Modelpath + "LineRegion.tup");

                    ClearAllObjects();
                    ShowLineRegion(Modelpath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveRect2Region(string Modelpath)
        {
            try
            {
                HObject ho_Line;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Line);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;

                HTuple hv_Line = new HTuple();
                // if (dobj.Handle != (IntPtr)0)
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == "RECTANGLE2")
                    {
                        bSave = true;
                        HTuple row1 = dobj.GetDrawingObjectParams("row");
                        HTuple column1 = dobj.GetDrawingObjectParams("column");
                        HTuple phi = dobj.GetDrawingObjectParams("phi");
                        HTuple length1 = dobj.GetDrawingObjectParams("length1");
                        HTuple length2 = dobj.GetDrawingObjectParams("length2");

                        hv_Line.Append(row1);
                        hv_Line.Append(column1);
                        hv_Line.Append(phi);
                        hv_Line.Append(length1);
                        hv_Line.Append(length2);

                        ho_Line.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Line, row1, column1, phi, length1, length2);
                        HOperatorSet.Union2(ho_RegionUnion, ho_Line, out ho_RegionUnion);
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "Rect2Region.hobj");
                    HOperatorSet.WriteTuple(hv_Line, Modelpath + "Rect2Region.tup");

                    ClearAllObjects();
                    ShowLineRegion(Modelpath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowLineRegion(string Modelpath)
        {
            try
            {
                HObject ho_RegionUnion;
                HObject ho_Arrow;

                HOperatorSet.GenEmptyObj(out ho_RegionUnion);
                HOperatorSet.GenEmptyObj(out ho_Arrow);

                string regionPath = Modelpath + "LineRegion.hobj";
                ho_RegionUnion.Dispose();
                HOperatorSet.ReadRegion(out ho_RegionUnion, regionPath);

                string linePath = Modelpath + "LineRegion.tup";
                HTuple hv_Line = new HTuple();
                HOperatorSet.ReadTuple(linePath, out hv_Line);

                ho_Arrow.Dispose();
                gen_arrow_contour_xld(out ho_Arrow, hv_Line[2], hv_Line[3], hv_Line[0], hv_Line[1],
                    20, 20);

                GetWindowHandle().SetLineWidth(1);
                GetWindowHandle().SetColor("red");
                GetWindowHandle().SetDraw("margin");
                GetWindowHandle().DispObj(ho_Arrow);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveXldRegion(string Modelpath)
        {
            try
            {
                try
                {
                    HObject ho_Circle, ho_Rectangle;
                    HObject ho_RegionUnion;
                    HOperatorSet.GenEmptyObj(out ho_Circle);
                    HOperatorSet.GenEmptyObj(out ho_Rectangle);
                    HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                    HObject ho_Contour;
                    HOperatorSet.GenEmptyObj(out ho_Contour);

                    bool bSave = false;
                    foreach (HDrawingObject dobj in drawing_objects)
                    {
                        if (dobj.Handle != null)
                        {
                            string status = "";
                            m_Dic.TryGetValue(dobj.ID, out status);
                            if (status == Circle)
                            {
                                bSave = true;
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple radius = dobj.GetDrawingObjectParams("radius");

                                ho_Circle.Dispose();
                                HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                                HOperatorSet.ConcatObj(ho_RegionUnion, ho_Circle, out ho_RegionUnion);
                            }
                            else if (status == RECT1)
                            {
                                bSave = true;
                                HTuple row1 = dobj.GetDrawingObjectParams("row1");
                                HTuple column1 = dobj.GetDrawingObjectParams("column1");
                                HTuple row2 = dobj.GetDrawingObjectParams("row2");
                                HTuple column2 = dobj.GetDrawingObjectParams("column2");

                                ho_Rectangle.Dispose();
                                HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                                HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            }
                            else if (status == RECT2)
                            {
                                bSave = true;
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple length1 = dobj.GetDrawingObjectParams("length1");
                                HTuple length2 = dobj.GetDrawingObjectParams("length2");
                                HTuple phi = dobj.GetDrawingObjectParams("phi");

                                ho_Rectangle.Dispose();
                                HOperatorSet.GenRectangle2(out ho_Rectangle, row1, column1, phi, length1, length2);
                                HOperatorSet.ConcatObj(ho_RegionUnion, ho_Rectangle, out ho_RegionUnion);
                            } 
                        } 

                       
                    }
                    if (bSave)
                    {
                        HOperatorSet.GenContourRegionXld(ho_RegionUnion, out ho_Contour, "border");
                        HOperatorSet.WriteContourXldArcInfo(ho_Contour, Modelpath + "Contour.hobj");

                        GetWindowHandle().SetLineWidth(2);
                        GetWindowHandle().SetColor("green");
                        GetWindowHandle().SetDraw("margin");
                        GetWindowHandle().DispObj(ho_Contour);
                    }
                    else
                    {
                        HTuple hv_Rows = new HTuple();
                        HTuple hv_Cols = new HTuple();

                        if (m_DrawXldId.Count != 0)
                        {
                            HOperatorSet.GetDrawingObjectParams(m_DrawXldId[0], "row", out hv_Rows);
                            HOperatorSet.GetDrawingObjectParams(m_DrawXldId[0], "column", out hv_Cols);

                            ho_Contour.Dispose();
                            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);

                            ClearAllObjects();

                            GetWindowHandle().SetLineWidth(2);
                            GetWindowHandle().SetColor("green");
                            GetWindowHandle().SetDraw("margin");
                            GetWindowHandle().DispObj(ho_Contour);

                            HOperatorSet.WriteTuple(hv_Rows, Modelpath + "DrawRows");
                            HOperatorSet.WriteTuple(hv_Cols, Modelpath + "DrawCols");

                            HOperatorSet.DetachDrawingObjectFromWindow(GetWindowHandle(), m_DrawXldId[0]);

                            HOperatorSet.WriteContourXldArcInfo(ho_Contour, Modelpath + "Contour.hobj");
                            m_DrawXldId.Clear();
                        }
                    }

                    ClearAllObjects();
                }
                catch (Exception ex)
                {

                }

               
            }
            catch (Exception ex)
            {

            }
        }

        public void ClearXld()
        {
            try
            {
                if (m_DrawXldId.Count != 0)
                {
                    ClearAllObjects();
                    HOperatorSet.DetachDrawingObjectFromWindow(GetWindowHandle(), m_DrawXldId[0]);

                    m_DrawXldId.Clear();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SavePartitionRegion(string Modelpath)
        {
            try
            {
                HObject ho_Rectangle;
                HObject ho_CheckPartionRegion;
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_CheckPartionRegion);

                HDrawingObject dobj = selected_drawing_object;
                bool bSave = false;

                // if (dobj.Handle != (IntPtr)0)
                if (dobj.Handle != null)
                {
                    bSave = true;
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == RECT1)
                    {
                        HTuple row1 = dobj.GetDrawingObjectParams("row1");
                        HTuple column1 = dobj.GetDrawingObjectParams("column1");
                        HTuple row2 = dobj.GetDrawingObjectParams("row2");
                        HTuple column2 = dobj.GetDrawingObjectParams("column2");

                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);
                        HOperatorSet.SmallestRectangle1(ho_Rectangle, out row1, out column1,
                                out row2, out column2);

                        ho_CheckPartionRegion.Dispose();
                        HOperatorSet.PartitionRectangle(ho_Rectangle, out ho_CheckPartionRegion, 200, 200);
                    }
                }
                if (bSave)
                {
                    HOperatorSet.WriteRegion(ho_CheckPartionRegion, Modelpath + "CheckPartionRegion.hobj");

                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("orange");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_CheckPartionRegion);
                }

                ClearAllObjects();

                ho_Rectangle.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public void AddDrawLine(int width = 0)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, 50, 50, 50, 200 + width);

            line.SetDrawingObjectParams("color", "red");
            AttachDrawObj(line);
            HOperatorSet.SetDrawingObjectParams(line, "line_width", 1);

            line.OnDrag(OnDragDrawingObject);
            line.OnResize(OnDragDrawingObject);

            m_Dic.Add(line.ID, Line);
        }

        public void AddDrawLine(double startRow, double startCol, double endRow, double endCol)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
            HDrawingObject.HDrawingObjectType.LINE, startRow, startCol, endRow, endCol);

            line.SetDrawingObjectParams("color", "red");
            AttachDrawObj(line);
            line.OnDrag(OnDragDrawingObject);

            m_Dic.Add(line.ID, Line);
        }

        private void OnDragDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Line)
                    {
                        _FindLineParam.InLineStartRow = dobj.GetDrawingObjectParams("row1");
                        _FindLineParam.InLineStartCol = dobj.GetDrawingObjectParams("column1");
                        _FindLineParam.InLineEndRow = dobj.GetDrawingObjectParams("row2");
                        _FindLineParam.InLineEndCol = dobj.GetDrawingObjectParams("column2");

                        FindLineAct();
                    }
                    else if (status == Circle)
                    {
                        _FindCircleParam.InCircleRow = dobj.GetDrawingObjectParams("row");
                        _FindCircleParam.InCircleCol = dobj.GetDrawingObjectParams("column");
                        _FindCircleParam.InSearchRadius = dobj.GetDrawingObjectParams("radius");

                        FindCircleAct();
                    }
                    else if (status == Circle_Sector)
                    {
                        _FindCircleParam.InCircleRow = dobj.GetDrawingObjectParams("row");
                        _FindCircleParam.InCircleCol = dobj.GetDrawingObjectParams("column");
                        _FindCircleParam.InSearchRadius = dobj.GetDrawingObjectParams("radius");
                        _FindCircleParam.StartPhi = dobj.GetDrawingObjectParams("start_angle");
                        _FindCircleParam.EndPhi = dobj.GetDrawingObjectParams("end_angle");

                        FindCircleAct(true);
                    }
                    else if (status == LineM)
                    {
                        _OneMeasureParam.LineRowStart = dobj.GetDrawingObjectParams("row1");
                        _OneMeasureParam.LineColumnStart = dobj.GetDrawingObjectParams("column1");
                        _OneMeasureParam.LineRowEnd = dobj.GetDrawingObjectParams("row2");
                        _OneMeasureParam.LineColumnEnd = dobj.GetDrawingObjectParams("column2");
                        OneMeasureFunc();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDragDrawingObjectArc2(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Circle_Sector2)
                    {
                        double row = dobj.GetDrawingObjectParams("row");
                        double column = dobj.GetDrawingObjectParams("column");
                        double radius = dobj.GetDrawingObjectParams("radius");
                        double start_angle = dobj.GetDrawingObjectParams("start_angle");
                        double end_angle = dobj.GetDrawingObjectParams("end_angle");

                        m_Circle2.SetDrawingObjectParams("row", row);
                        m_Circle2.SetDrawingObjectParams("column", column);
                        if (radius > m_ArcWidth)
                        {
                            m_Circle2.SetDrawingObjectParams("radius", radius - m_ArcWidth);
                        }
                        else
                        {
                            m_Circle2.SetDrawingObjectParams("radius", 0);
                        }
                        m_Circle2.SetDrawingObjectParams("start_angle", start_angle);
                        m_Circle2.SetDrawingObjectParams("end_angle", end_angle);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDragDrawingDoubleCircle(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                if (dobj.Handle != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Double_Circle)
                    {
                        double row = dobj.GetDrawingObjectParams("row");
                        double column = dobj.GetDrawingObjectParams("column");
                        double radius = dobj.GetDrawingObjectParams("radius");

                        m_Circle2.SetDrawingObjectParams("row", row);
                        m_Circle2.SetDrawingObjectParams("column", column);
                        if (radius > m_ArcWidth)
                        {
                            m_Circle2.SetDrawingObjectParams("radius", radius - m_ArcWidth);
                        }
                        else
                        {
                            m_Circle2.SetDrawingObjectParams("radius", 0);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        HObject ho_MeasureCross1, ho_MeasuredLines1, ho_MeasureLineContours1;
        public void FindLineAct()
        {
            try
            {
                HTuple hv_InMeasureLength1 = null, hv_InMeasureLength2 = null;
                HTuple hv_InMeasureSigma = null, hv_InMeasureThreshold = null;
                HTuple hv_InMeasureSelect = null, hv_InMeasureTransition = null;
                HTuple hv_InMeasureNumber = null, hv_InMeasureScore = null;
                HTuple hv_bDisp = null;
                HTuple hv_bFindLine2D = null;
                HTuple hv_RowBegin1 = new HTuple();
                HTuple hv_ColBegin1 = new HTuple(), hv_RowEnd1 = new HTuple();
                HTuple hv_ColEnd1 = new HTuple(), hv_AllRow = new HTuple();
                HTuple hv_AllColumn = new HTuple();
                HTuple hv_InLineStartRow = new HTuple(), hv_InLineStartCol = new HTuple();
                HTuple hv_InLineEndRow = new HTuple(), hv_InLineEndCol = new HTuple();


                if (ho_MeasureCross1 != null && ho_MeasuredLines1 != null && ho_MeasureLineContours1 != null)
                {
                    ho_MeasureCross1.Dispose();
                    ho_MeasuredLines1.Dispose();
                    ho_MeasureLineContours1.Dispose();
                }


                hv_InLineStartRow = _FindLineParam.InLineStartRow;
                hv_InLineStartCol = _FindLineParam.InLineStartCol;
                hv_InLineEndRow = _FindLineParam.InLineEndRow;
                hv_InLineEndCol = _FindLineParam.InLineEndCol;
                hv_InMeasureLength1 = _FindLineParam.InMeasureLength1;
                hv_InMeasureLength2 = _FindLineParam.InMeasureLength2;
                hv_InMeasureSigma = _FindLineParam.InMeasureSigma;
                hv_InMeasureThreshold = _FindLineParam.InMeasureThreshold;
                hv_InMeasureSelect = _FindLineParam.InMeasureSelect;
                hv_InMeasureTransition = _FindLineParam.InMeasureTransition;
                hv_InMeasureNumber = _FindLineParam.InMeasureNumber;
                hv_InMeasureScore = _FindLineParam.InMeasureScore;


                //输出参数 
                FindLine2D(Image, out ho_MeasureLineContours1, out ho_MeasureCross1, out ho_MeasuredLines1,
                    hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol,
                    hv_InMeasureLength1, hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold,
                    hv_InMeasureSelect, hv_InMeasureTransition, hv_InMeasureNumber, hv_InMeasureScore,
                    hv_bDisp, out hv_RowBegin1, out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1,
                    out hv_AllRow, out hv_AllColumn, out hv_bFindLine2D); 

                if (true)
                {
                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetLineWidth(2);
                    GetWindowHandle().SetColor("yellow");
                    GetWindowHandle().DispObj(ho_MeasuredLines1);

                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().DispObj(ho_MeasureLineContours1);
                    GetWindowHandle().DispObj(ho_MeasureCross1);

                    #region 显示找线的方向箭头 

                    HTuple hv_Angle;
                    HOperatorSet.AngleLx(hv_InLineStartRow, hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, out hv_Angle);

                    HObject ho_Arrow1;
                    gen_arrow_contour_xld(out ho_Arrow1, hv_InMeasureLength1 * -1, 0, hv_InMeasureLength1, 0, 5, 5);

                    HTuple hv_Mat2d;
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, (hv_InLineStartRow + hv_InLineEndRow) / 2, (hv_InLineStartCol + hv_InLineEndCol) / 2,
                        hv_Angle, out hv_Mat2d);
                    HOperatorSet.AffineTransContourXld(ho_Arrow1, out ho_Arrow1, hv_Mat2d);

                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().SetColor("magenta");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_Arrow1);

                    #endregion

                }

            }
            catch (Exception ex)
            {

            }

        }

        public void FindLine2D(HObject ho_Image, out HObject ho_MeasureLineContours, out HObject ho_MeasureCross,
                  out HObject ho_MeasuredLines, HTuple hv_InLineStartRow, HTuple hv_InLineStartCol,
                  HTuple hv_InLineEndRow, HTuple hv_InLineEndCol, HTuple hv_InMeasureLength1,
                  HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                  HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                  HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_RowBegin, out HTuple hv_ColBegin,
                  out HTuple hv_RowEnd, out HTuple hv_ColEnd, out HTuple hv_AllRow, out HTuple hv_AllColumn,
                  out HTuple hv_bFindLine2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyLineIndices = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            hv_AllRow = new HTuple();
            hv_AllColumn = new HTuple();
            hv_bFindLine2D = new HTuple();
            try
            {
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
                //
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_InLineStartRow,
                    hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, hv_InMeasureLength1,
                    hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(),
                    new HTuple(), out hv_MetrologyLineIndices);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_measures", hv_InMeasureNumber);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_interpolation", "bicubic");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_begin", out hv_RowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_begin", out hv_ColBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_end", out hv_RowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_end", out hv_ColEnd);
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureLineContours, hv_MetrologyHandle,
                    "all", "all", out hv_AllRow, out hv_AllColumn);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_AllRow, hv_AllColumn,
                    8, 0.785398);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLines, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                //if (bDisp)
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureLineContours)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureCross)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasuredLines)
                }
                //endif
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_RowBegin.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_ColEnd.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindLine2D = 1;
                }
                else
                {
                    hv_bFindLine2D = 0;
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindLine2D = 0;
            }


            return;
        }

        public class FindLineParam
        {
            public double InLineStartRow { get; set; }
            public double InLineStartCol { get; set; }
            public double InLineEndRow { get; set; }
            public double InLineEndCol { get; set; }
            public int InMeasureLength1 { get; set; }
            public int InMeasureLength2 { get; set; }
            public double InMeasureSigma { get; set; }
            public double InMeasureThreshold { get; set; }
            public string InMeasureSelect { get; set; }
            public string InMeasureTransition { get; set; }
            public int InMeasureNumber { get; set; }
            public double InMeasureScore { get; set; }
        }

        public FindLineParam _findLineParam = new FindLineParam();
        public FindLineParam _FindLineParam
        {
            get { return _findLineParam; }
            set { _findLineParam = value; }
        }


        public OneMeasureParam _oneMeasureParam = new OneMeasureParam();
        public OneMeasureParam _OneMeasureParam
        {
            get { return _oneMeasureParam; }
            set { _oneMeasureParam = value; }
        }

        public class OneMeasureParam
        {
            public int Threshold { get; set; }

            public int RoiWidth { get; set; }

            public double Sigma { get; set; }

            public string Transition { get; set; }

            public string Select { get; set; }

            public double LineRowStart { get; set; }

            public double LineColumnStart { get; set; }

            public double LineRowEnd { get; set; }

            public double LineColumnEnd { get; set; }
        }

        public void OneMeasureFunc()
        {
            try
            {
                OneMeasureParam tModel = _OneMeasureParam;
                // Local control variables 
                HTuple hv_AmplitudeThreshold = null, hv_RoiWidthLen2 = null;
                HTuple hv_LineRowStart_Measure_01_0 = null, hv_LineColumnStart_Measure_01_0 = null;
                HTuple hv_LineRowEnd_Measure_01_0 = null, hv_LineColumnEnd_Measure_01_0 = null;
                HTuple hv_TmpCtrl_Row = null, hv_TmpCtrl_Column = null;
                HTuple hv_TmpCtrl_Dr = null, hv_TmpCtrl_Dc = null, hv_TmpCtrl_Phi = null;
                HTuple hv_TmpCtrl_Len1 = null, hv_TmpCtrl_Len2 = null;
                HTuple hv_MsrHandle_Measure_01_0 = null, hv_Row_Measure_01_0 = null;
                HTuple hv_Column_Measure_01_0 = null, hv_Amplitude_Measure_01_0 = null;
                HTuple hv_Distance_Measure_01_0 = null;
                // Initialize local and output iconic variables  
                //Measure 01: Code generated by Measure 01
                //Measure 01: Prepare measurement
                hv_AmplitudeThreshold = tModel.Threshold;
                hv_RoiWidthLen2 = tModel.RoiWidth;
                HOperatorSet.SetSystem("int_zooming", "true");
                //Measure 01: Coordinates for line Measure 01 [0]
                hv_LineRowStart_Measure_01_0 = tModel.LineRowStart;
                hv_LineColumnStart_Measure_01_0 = tModel.LineColumnStart;
                hv_LineRowEnd_Measure_01_0 = tModel.LineRowEnd;
                hv_LineColumnEnd_Measure_01_0 = tModel.LineColumnEnd;

                //Measure 01: Convert coordinates to rectangle2 type
                hv_TmpCtrl_Row = 0.5 * (hv_LineRowStart_Measure_01_0 + hv_LineRowEnd_Measure_01_0);
                hv_TmpCtrl_Column = 0.5 * (hv_LineColumnStart_Measure_01_0 + hv_LineColumnEnd_Measure_01_0);
                hv_TmpCtrl_Dr = hv_LineRowStart_Measure_01_0 - hv_LineRowEnd_Measure_01_0;
                hv_TmpCtrl_Dc = hv_LineColumnEnd_Measure_01_0 - hv_LineColumnStart_Measure_01_0;
                hv_TmpCtrl_Phi = hv_TmpCtrl_Dr.TupleAtan2(hv_TmpCtrl_Dc);
                hv_TmpCtrl_Len1 = 0.5 * ((((hv_TmpCtrl_Dr * hv_TmpCtrl_Dr) + (hv_TmpCtrl_Dc * hv_TmpCtrl_Dc))).TupleSqrt()
                    );
                hv_TmpCtrl_Len2 = hv_RoiWidthLen2.Clone();
                //Measure 01: Create measure for line Measure 01 [0]
                HTuple hv_Width, hv_Height;
                HOperatorSet.GetImageSize(Image, out hv_Width, out hv_Height);
                //Measure 01: Attention: This assumes all images have the same size!
                HOperatorSet.GenMeasureRectangle2(hv_TmpCtrl_Row, hv_TmpCtrl_Column, hv_TmpCtrl_Phi,
                    hv_TmpCtrl_Len1, hv_TmpCtrl_Len2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure_01_0);
                //Measure 01: ***************************************************************
                //Measure 01: * The code which follows is to be executed once / measurement *
                //Measure 01: ***************************************************************
                //Measure 01: The image is assumed to be made available in the
                //Measure 01: variable last displayed in the graphics window 

                //Measure 01: Execute measureyments
                HOperatorSet.MeasurePos(Image, hv_MsrHandle_Measure_01_0, 1, hv_AmplitudeThreshold,
                    tModel.Transition, tModel.Select, out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0,
                    out hv_Distance_Measure_01_0);
                //Measure 01: Do something with the results
                //Measure 01: Clear measure when done
                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure_01_0);

                if (hv_Row_Measure_01_0 != null)
                {
                    GetWindowHandle().DispObj(Image);

                    HObject ho_Cross;
                    HOperatorSet.GenEmptyObj(out ho_Cross);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row_Measure_01_0, hv_Column_Measure_01_0, 50, 0.785398);
                    GetWindowHandle().SetLineWidth(3);
                    GetWindowHandle().SetColor("green");
                    GetWindowHandle().DispObj(ho_Cross);

                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region 获取深度曲线

        //获取深度曲线
        public List<double>[] GetGray()
        {
            try
            {
                if (m_deepLine == null || m_deepLine.IsDisposed)
                {
                    return null;
                }

                HObject ho_RegionLines;
                HObject ho_Contours;

                // Local control variables 

                HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
                HTuple hv_Column2 = null, hv_Row = null, hv_Col = null;
                HTuple hv_Length = null, hv_RowSelected = null, hv_ColSelected = null;
                HTuple hv_Grayval = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_RegionLines);
                HOperatorSet.GenEmptyObj(out ho_Contours);

                //HOperatorSet.DrawLine(hsmartControl.HalconWindow, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                //HOperatorSet.DrawLineMod(hsmartControl.HalconWindow, 0, 0, 0, 0, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);

                //AddLine();

                // EventWaitHandle events = new EventWaitHandle(true, EventResetMode.ManualReset);
                //events.WaitOne();

                //Stopwatch st = new Stopwatch();
                //st.Start();
                //for(int i = 0; i < 300; i++)
                //{
                //    if(st.ElapsedMilliseconds > 30*1000 || m_DoneEnter)
                //    {
                //        m_DoneEnter = false;
                //        break;
                //    }
                //    Thread.Sleep(100);
                //}



                HDrawingObject dobj = selected_drawing_object;

                if (dobj != null)
                {
                    string status = "";
                    m_Dic.TryGetValue(dobj.ID, out status);
                    if (status == Line)
                    {
                        hv_Row1 = dobj.GetDrawingObjectParams("row1");
                        hv_Column1 = dobj.GetDrawingObjectParams("column1");
                        hv_Row2 = dobj.GetDrawingObjectParams("row2");
                        hv_Column2 = dobj.GetDrawingObjectParams("column2");
                    }
                    else
                    {
                        return null;
                    }
                }


                ho_RegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row1, hv_Column1, hv_Row2,
                    hv_Column2);


                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionLines, out ho_Contours, "border");

                HOperatorSet.GetContourXld(ho_Contours, out hv_Row, out hv_Col);
                HOperatorSet.TupleLength(hv_Row, out hv_Length);

                HOperatorSet.TupleSelectRange(hv_Row, 0, hv_Length / 2, out hv_RowSelected);
                HOperatorSet.TupleSelectRange(hv_Col, 0, hv_Length / 2, out hv_ColSelected);


                HOperatorSet.GetGrayval(Image, hv_RowSelected, hv_ColSelected,
                    out hv_Grayval);

                //GetWindowHandle().SetColor("red");
                //GetWindowHandle().DispLine(hv_Row1, hv_Column1, hv_Row2, hv_Column2);

                ho_RegionLines.Dispose();
                ho_Contours.Dispose();

                List<double>[] listDArr = new List<double>[3];
                listDArr[0] = hv_RowSelected.DArr.ToList();
                listDArr[1] = hv_ColSelected.DArr.ToList();
                //listDArr[2] = hv_Grayval.LArr.ToList();
                List<double> listD = new List<double>();

                bool bint = hv_Grayval.TupleIsIntElem();
                bool breal = hv_Grayval.TupleIsRealElem();
                if(bint)
                { 
                    List<int> listL = hv_Grayval.ToIArr().ToList();
                    foreach (var item in listL)
                    {
                        listD.Add((double)item);
                    }
                }
                else if(breal)
                {
                    List<long> listL = hv_Grayval.ToLArr().ToList();
                    foreach (var item in listL)
                    {
                        listD.Add((double)item);
                    }
                }

                listDArr[2] = listD;

                if (m_deepLine != null)
                {
                    m_deepLine.RefershData(listDArr[1], listDArr[2], "深度曲线", true);
                }
                //ClearAllObjects();
                return listDArr;
            }
            catch (HalconException ex)
            {
                return null;
            }

        }
        #endregion
        
        #region 显示全屏图像 显示像素信息
        public static FullScreenView view;
        public static bool m_bShow = true;
        private void FullScreenDisplay()
        {
            HObject ho_DumpImage;
            HOperatorSet.GenEmptyObj(out ho_DumpImage);
            ho_DumpImage.Dispose();
            HOperatorSet.DumpWindowImage(out ho_DumpImage, GetWindowHandle());

            m_bShow = false;
            view = new FullScreenView(Image, DisplayObj, m_strValue);
            view.ShowDialog();
        }

        private void CloseScreenDisplay()
        {
            if (view != null)
            {
                view.Close();
            }
        }

        public static bool bCtrlDown = false;
        DisplayValue m_Displayview;
        private void hsmartControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (Image != null && Image.IsInitialized() && m_bShow)
                    {
                        FullScreenDisplay();
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    CloseScreenDisplay();
                }
                else if (e.KeyCode == Keys.ControlKey)
                {
                    bCtrlDown = true;
                    if (m_Displayview == null || m_Displayview.IsDisposed)
                    {
                        m_Displayview = new DisplayValue(positionY.ToString("0.0"), positionX.ToString("0.0"), grayValue);
                        m_Displayview.StartPosition = FormStartPosition.Manual;
                        m_Displayview.Location = Control.MousePosition;
                        m_Displayview.Show();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void hsmartControl_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.ControlKey)
                {
                    bCtrlDown = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void DisplayValue(bool bDisplay)
        {
            if (!bCtrlDown)
            {
                return;
            }
            if (bDisplay)
            {
                if (m_Displayview == null || m_Displayview.IsDisposed)
                {
                    m_Displayview = new DisplayValue(positionY.ToString("0.0"), positionX.ToString("0.0"), grayValue);
                    m_Displayview.StartPosition = FormStartPosition.Manual;
                    m_Displayview.Location = Control.MousePosition;
                    m_Displayview.Show();
                }
            }
            else
            {
                if (m_Displayview != null)
                {
                    m_Displayview.Close();
                    m_Displayview.Dispose();
                    m_Displayview = null;
                }
            }

        }

        #endregion

        #region 通用的函数 3D显示
        
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
                 HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_M = null, hv_N = null, hv_Red = null;
            HTuple hv_Green = null, hv_Blue = null, hv_RowI1Part = null;
            HTuple hv_ColumnI1Part = null, hv_RowI2Part = null, hv_ColumnI2Part = null;
            HTuple hv_RowIWin = null, hv_ColumnIWin = null, hv_WidthWin = null;
            HTuple hv_HeightWin = null, hv_I = null, hv_RowI = new HTuple();
            HTuple hv_ColumnI = new HTuple(), hv_StringI = new HTuple();
            HTuple hv_MaxAscent = new HTuple(), hv_MaxDescent = new HTuple();
            HTuple hv_MaxWidth = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRowI = new HTuple();
            HTuple hv_FactorColumnI = new HTuple(), hv_UseShadow = new HTuple();
            HTuple hv_ShadowColor = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            if ((int)(new HTuple(hv_Box_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Box_COPY_INP_TMP = "false";
            }
            //
            //
            //Check conditions
            //
            hv_M = (new HTuple(hv_Row_COPY_INP_TMP.TupleLength())) * (new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                ));
            hv_N = new HTuple(hv_Row_COPY_INP_TMP.TupleLength());
            if ((int)((new HTuple(hv_M.TupleEqual(0))).TupleOr(new HTuple(hv_String_COPY_INP_TMP.TupleEqual(
                new HTuple())))) != 0)
            {

                return;
            }
            if ((int)(new HTuple(hv_M.TupleNotEqual(1))) != 0)
            {
                //Multiple positions
                //
                //Expand single parameters
                if ((int)(new HTuple((new HTuple(hv_Row_COPY_INP_TMP.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    hv_N = new HTuple(hv_Column_COPY_INP_TMP.TupleLength());
                    HOperatorSet.TupleGenConst(hv_N, hv_Row_COPY_INP_TMP, out hv_Row_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(1))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_N, hv_Column_COPY_INP_TMP, out hv_Column_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                    )).TupleNotEqual(new HTuple(hv_Row_COPY_INP_TMP.TupleLength())))) != 0)
                {
                    throw new HalconException("Number of elements in Row and Column does not match.");
                }
                if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_N, hv_String_COPY_INP_TMP, out hv_String_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )).TupleNotEqual(hv_N))) != 0)
                {
                    throw new HalconException("Number of elements in Strings does not match number of positions.");
                }
                //
            }
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_RowI1Part, out hv_ColumnI1Part,
                out hv_RowI2Part, out hv_ColumnI2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowIWin, out hv_ColumnIWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 0, hv_WidthWin - 0);
            //
            //Loop over all positions
            HTuple end_val89 = hv_N - 1;
            HTuple step_val89 = 1;
            for (hv_I = 0; hv_I.Continue(end_val89, step_val89); hv_I = hv_I.TupleAdd(step_val89))
            {
                hv_RowI = hv_Row_COPY_INP_TMP.TupleSelect(hv_I);
                hv_ColumnI = hv_Column_COPY_INP_TMP.TupleSelect(hv_I);
                //Allow multiple strings for a single position.
                if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
                {
                    hv_StringI = hv_String_COPY_INP_TMP.Clone();
                }
                else
                {
                    //In case of multiple positions, only single strings
                    //are allowed per position.
                    //For line breaks, use \n in this case.
                    hv_StringI = hv_String_COPY_INP_TMP.TupleSelect(hv_I);
                }
                //Default settings
                //-1 is mapped to 12.
                if ((int)(new HTuple(hv_RowI.TupleEqual(-1))) != 0)
                {
                    hv_RowI = 12;
                }
                if ((int)(new HTuple(hv_ColumnI.TupleEqual(-1))) != 0)
                {
                    hv_ColumnI = 12;
                }
                //
                //Split string into one string per line.
                hv_StringI = ((("" + hv_StringI) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_RowI.Clone();
                    hv_C1 = hv_ColumnI.Clone();
                }
                else
                {
                    //Transform image to window coordinates.
                    hv_FactorRowI = (1.0 * hv_HeightWin) / ((hv_RowI2Part - hv_RowI1Part) + 1);
                    hv_FactorColumnI = (1.0 * hv_WidthWin) / ((hv_ColumnI2Part - hv_ColumnI1Part) + 1);
                    hv_R1 = (((hv_RowI - hv_RowI1Part) + 0.5) * hv_FactorRowI) - 0.5;
                    hv_C1 = (((hv_ColumnI - hv_ColumnI1Part) + 0.5) * hv_FactorColumnI) - 0.5;
                }
                //
                //Display text box depending on text size.
                hv_UseShadow = 1;
                hv_ShadowColor = "gray";
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
                {
                    if (hv_Box_COPY_INP_TMP == null)
                        hv_Box_COPY_INP_TMP = new HTuple();
                    hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                    hv_ShadowColor = "#f28d26";
                }
                if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                    1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                    {
                        //Use default ShadowColor set above
                    }
                    else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                        "false"))) != 0)
                    {
                        hv_UseShadow = 0;
                    }
                    else
                    {
                        hv_ShadowColor = hv_Box_COPY_INP_TMP.TupleSelect(1);
                        //Valid color?
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                                1));
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            hv_Exception = new HTuple("Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)");
                            throw new HalconException(hv_Exception);
                        }
                    }
                }
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
                {
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            0));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = new HTuple("Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)");
                        throw new HalconException(hv_Exception);
                    }
                    //Calculate box extents
                    hv_StringI = (" " + hv_StringI) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_StringI.TupleSelect(hv_Index),
                            out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_StringI.TupleLength()));
                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //Display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    //Set shadow color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                    if ((int)(hv_UseShadow) != 0)
                    {
                        HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1,
                            hv_C2 + 1);
                    }
                    //Set box color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    //Set color
                    if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
                    {
                        //Wiht a single text position, each text line
                        //may get a different color.
                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                            )));
                    }
                    else
                    {
                        //With multiple text positions, each position
                        //gets a single color for all text lines.
                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_I % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                            )));
                    }
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        //Valid color?
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            hv_Exception = ((("Wrong value of control parameter Color[" + (hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                                )))) + "] == '") + hv_CurrentColor) + "' (must be a valid color string)";
                            throw new HalconException(hv_Exception);
                        }
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    //Finally display text
                    hv_RowI = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_RowI, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_StringI.TupleSelect(hv_Index));
                }
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_RowI1Part, hv_ColumnI1Part, hv_RowI2Part,
                hv_ColumnI2Part);

            return;
        }

        public static void dev_display_shape_matching_results(HTuple hv_ModelID, HTuple hv_Color,
                 HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC,
                 HTuple hv_Model, out HObject ho_ContoursAffinTrans)
        {
            // Local iconic variables 

            HObject ho_ModelContours = null;
            HObject ho_OutObj = null;

            // Local control variables 

            HTuple hv_NumMatches = null, hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = hv_Model.Clone();
            HTuple hv_ScaleC_COPY_INP_TMP = hv_ScaleC.Clone();
            HTuple hv_ScaleR_COPY_INP_TMP = hv_ScaleR.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_OutObj);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            try
            {
                //This procedure displays the results of Shape-Based Matching.
                //
                hv_NumMatches = new HTuple(hv_Row.TupleLength());
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_ScaleR_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleR_COPY_INP_TMP, out hv_ScaleR_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_ScaleC_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleC_COPY_INP_TMP, out hv_ScaleC_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out hv_Model_COPY_INP_TMP);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID.TupleSelect(
                            hv_Index), 1);
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                hv_Index % (new HTuple(hv_Color.TupleLength()))));
                        }
                        HTuple end_val18 = hv_NumMatches - 1;
                        HTuple step_val18 = 1;
                        for (hv_Match = 0; hv_Match.Continue(end_val18, step_val18); hv_Match = hv_Match.TupleAdd(step_val18))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR_COPY_INP_TMP.TupleSelect(
                                    hv_Match), hv_ScaleC_COPY_INP_TMP.TupleSelect(hv_Match), 0, 0,
                                    out hv_HomMat2DScale);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                    hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                ho_OutObj.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_OutObj,
                                    hv_HomMat2DTranslate);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ContoursAffinTrans, HDevWindowStack.GetActive()
                                        );
                                }
                                HOperatorSet.ConcatObj(ho_ContoursAffinTrans, ho_OutObj, out ho_ContoursAffinTrans);
                            }
                        }
                    }
                }
                ho_ModelContours.Dispose();
                ho_OutObj.Dispose();
                //ho_ContoursAffinTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContours.Dispose();
                ho_OutObj.Dispose();
                //ho_ContoursAffinTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }
        
        public static void select_mask_obj(HObject ho_Objects, out HObject ho_SelectedObjects,
            HTuple hv_Mask)
        {

            // Local control variables 

            HTuple hv_Number = null, hv_AllNumbers = null;
            HTuple hv_Indices = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectedObjects);
            //select_mask_obj selects one or more single elements of the object array
            //Objects and returns them in SelectedObjects.
            //The elements of Mask determine if the corresponding elements of Objects are selected.
            //If the value is greater than 0, the corresponding element is selected.
            //
            //Check number of elements
            HOperatorSet.CountObj(ho_Objects, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleNotEqual(new HTuple(hv_Mask.TupleLength()
                )))) != 0)
            {
                throw new HalconException("Number of elements in Objects and Mask do not match.");
            }
            //
            //Check type of mask elements
            hv_AllNumbers = new HTuple((((((hv_Mask.TupleIsRealElem())).TupleSum()) + (((hv_Mask.TupleIsIntElem()
                )).TupleSum()))).TupleEqual(new HTuple(hv_Mask.TupleLength())));
            if ((int)((new HTuple(hv_AllNumbers.TupleNot())).TupleAnd(new HTuple(hv_Mask.TupleNotEqual(
                new HTuple())))) != 0)
            {
                throw new HalconException("Invalid type: Elements of Mask must be integer or real numbers.");
            }
            //
            //Use select_mask for tuples to generate a list of object indices.
            hv_Indices = (HTuple.TupleGenSequence(1, new HTuple(hv_Mask.TupleLength()), 1)).TupleSelectMask(
                hv_Mask);
            ho_SelectedObjects.Dispose();
            HOperatorSet.SelectObj(ho_Objects, out ho_SelectedObjects, hv_Indices);

            return;
        }

        public static void get_rectangle2_points(HTuple hv_CenterY, HTuple hv_CenterX, HTuple hv_Phi,
            HTuple hv_Len1, HTuple hv_Len2, out HTuple hv_CornerY, out HTuple hv_CornerX,
            out HTuple hv_LineCenterY, out HTuple hv_LineCenterX)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_RowT = null, hv_ColT = null, hv_Cos = null;
            HTuple hv_Sin = null;
            // Initialize local and output iconic variables 
            //矩形端点坐标变量、边中心坐标变量初始化
            hv_CornerX = new HTuple();
            hv_CornerY = new HTuple();
            hv_LineCenterX = new HTuple();
            hv_LineCenterY = new HTuple();

            //临时变量初始化
            hv_RowT = 0;
            hv_ColT = 0;

            //判断仿射矩形是否有效
            if ((int)((new HTuple(hv_Len1.TupleLessEqual(0))).TupleOr(new HTuple(hv_Len2.TupleLessEqual(
                0)))) != 0)
            {

                return;
            }

            //计算仿射矩形角度的正弦值、余弦值
            HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
            HOperatorSet.TupleSin(hv_Phi, out hv_Sin);

            //矩形第一个端点坐标
            hv_ColT = (hv_CenterX - (hv_Len1 * hv_Cos)) - (hv_Len2 * hv_Sin);
            hv_RowT = hv_CenterY - (((-hv_Len1) * hv_Sin) + (hv_Len2 * hv_Cos));
            hv_CornerY = hv_CornerY.TupleConcat(hv_RowT);
            hv_CornerX = hv_CornerX.TupleConcat(hv_ColT);

            //矩形第二个端点坐标
            hv_ColT = (hv_CenterX + (hv_Len1 * hv_Cos)) - (hv_Len2 * hv_Sin);
            hv_RowT = hv_CenterY - ((hv_Len1 * hv_Sin) + (hv_Len2 * hv_Cos));
            hv_CornerY = hv_CornerY.TupleConcat(hv_RowT);
            hv_CornerX = hv_CornerX.TupleConcat(hv_ColT);

            //矩形第三个端点坐标
            hv_ColT = (hv_CenterX + (hv_Len1 * hv_Cos)) + (hv_Len2 * hv_Sin);
            hv_RowT = hv_CenterY - ((hv_Len1 * hv_Sin) - (hv_Len2 * hv_Cos));
            hv_CornerY = hv_CornerY.TupleConcat(hv_RowT);
            hv_CornerX = hv_CornerX.TupleConcat(hv_ColT);

            //矩形第四个端点坐标
            hv_ColT = (hv_CenterX - (hv_Len1 * hv_Cos)) + (hv_Len2 * hv_Sin);
            hv_RowT = hv_CenterY - (((-hv_Len1) * hv_Sin) - (hv_Len2 * hv_Cos));
            hv_CornerY = hv_CornerY.TupleConcat(hv_RowT);
            hv_CornerX = hv_CornerX.TupleConcat(hv_ColT);

            //矩形第一条边中心坐标
            if (hv_LineCenterY == null)
                hv_LineCenterY = new HTuple();
            hv_LineCenterY[0] = ((hv_CornerY.TupleSelect(0)) + (hv_CornerY.TupleSelect(1))) * 0.5;
            if (hv_LineCenterX == null)
                hv_LineCenterX = new HTuple();
            hv_LineCenterX[0] = ((hv_CornerX.TupleSelect(0)) + (hv_CornerX.TupleSelect(1))) * 0.5;

            //矩形第二条边中心坐标
            if (hv_LineCenterY == null)
                hv_LineCenterY = new HTuple();
            hv_LineCenterY[1] = ((hv_CornerY.TupleSelect(1)) + (hv_CornerY.TupleSelect(2))) * 0.5;
            if (hv_LineCenterX == null)
                hv_LineCenterX = new HTuple();
            hv_LineCenterX[1] = ((hv_CornerX.TupleSelect(1)) + (hv_CornerX.TupleSelect(2))) * 0.5;

            //矩形第三条边中心坐标
            if (hv_LineCenterY == null)
                hv_LineCenterY = new HTuple();
            hv_LineCenterY[2] = ((hv_CornerY.TupleSelect(3)) + (hv_CornerY.TupleSelect(2))) * 0.5;
            if (hv_LineCenterX == null)
                hv_LineCenterX = new HTuple();
            hv_LineCenterX[2] = ((hv_CornerX.TupleSelect(3)) + (hv_CornerX.TupleSelect(2))) * 0.5;

            //矩形第四边中心坐标
            if (hv_LineCenterY == null)
                hv_LineCenterY = new HTuple();
            hv_LineCenterY[3] = ((hv_CornerY.TupleSelect(3)) + (hv_CornerY.TupleSelect(0))) * 0.5;
            if (hv_LineCenterX == null)
                hv_LineCenterX = new HTuple();
            hv_LineCenterX[3] = ((hv_CornerX.TupleSelect(3)) + (hv_CornerX.TupleSelect(0))) * 0.5;

            //返回

            return;
        }

        //3D Display
        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Image == null || !Image.IsInitialized())
                {
                    return;
                }

                m_b3DView = true;
                Diplay3DView view = new Diplay3DView(Image);
                view.Show();
            }
            catch (Exception ex)
            {
            }
        }
        private void dToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem strip = sender as ToolStripMenuItem;
                //this.SuspendLayout();

                if (Image == null || !Image.IsInitialized())
                {
                    return;
                }

                if (strip.Checked)
                {
                    //hsmartControl.HalconWindow.SetPaint(((new HTuple("3d_plot")).TupleConcat("shaded")).TupleConcat(1));
                    //hsmartControl.HalconWindow.SetWindowParam("display_grid", "false");
                    //hsmartControl.HalconWindow.SetLut("change3");

                    //hsmartControl.HalconWindow.SetLut("change3");
                    //hsmartControl.HalconWindow.SetPaint("3d_plot"); 
                    //hsmartControl.HalconWindow.ClearWindow();
                    //hsmartControl.HalconWindow.DispImage(Image);

                    m_b3DView = true;
                    Diplay3DView view = new Diplay3DView(Image);
                    view.Show();
                }
                else
                {
                    m_b3DView = false;
                    hsmartControl.HalconWindow.SetPaint("default");
                    hsmartControl.HalconWindow.SetLut("default");

                    DispImage(Image);
                }
            }
            catch (Exception ex)
            {

            }
        }
        DeepLineView m_deepLine;
        public void deepLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDrawingObject line = HDrawingObject.CreateDrawingObject(
              HDrawingObject.HDrawingObjectType.LINE, hv_firstX, hv_firstY, hv_firstX, hv_firstY - 100);
            line.SetDrawingObjectParams("color", "gold");
            AttachDrawObj(line);
            m_Dic.Add(line.ID, Line);

            if (m_deepLine == null || m_deepLine.IsDisposed)
            {
                m_deepLine = new DeepLineView();
            }
            List<double>[] listvalue = GetGray();

            m_deepLine.TopMost = true;
            m_deepLine.Show();
        }

        public void interactive_3d_plot22(HObject ho_HeightField, HTuple hv_WindowHandle,
                HTuple hv_Mode, HTuple hv_GenParamName, HTuple hv_GenParamValue)
        {

            HSystem sys = new HSystem();

            // Local iconic variables 

            // Local control variables 

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
            HOperatorSet.SetColored(hv_WindowHandle, 12);
            hv_Button = new HTuple();
            hv_ButtonDown = 0;
            while ((int)(1) != 0)
            {
                Application.DoEvents();
                // (dev_)set_check ("~give_error")
                //try
                //{
                //    HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row, out hv_Column, out hv_Button);
                //}
                //catch (HalconException e)
                //{
                //    int error = e.GetErrorCode();
                //    if (error < 0)
                //        throw e;
                //    continue;
                //}
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
                    try
                    {
                        HOperatorSet.SetColored(hv_WindowHandle, 12);
                    }
                    catch (HalconException e)
                    {
                        int error = e.GetErrorCode();
                        if (error < 0)
                            throw e;
                    }
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
                            break;
                        }
                        hv_ButtonDown = 1;
                    }
                    hv_lastCol = hv_Column.Clone();
                    hv_lastRow = hv_Row.Clone();

                }
                //disp_image can not be used because it discards all channels but
                //the first, hence the texture mode would not work.
                //HOperatorSet.DispObj(ho_HeightField, hv_WindowHandle);
                hsmartControl.HalconWindow.DispObj(ho_HeightField);
                break;
            }
            HOperatorSet.SetWindowParam(hv_WindowHandle, "interactive_plot", "false");
            HOperatorSet.SetPaint(hv_WindowHandle, hv_PreviousPlotMode);

            return;
        }

        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
                HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            //This procedure generates arrow shaped XLD contours,
            //pointing from (Row1, Column1) to (Row2, Column2).
            //If starting and end point are identical, a contour consisting
            //of a single point is returned.
            //
            //input parameteres:
            //Row1, Column1: Coordinates of the arrows' starting points
            //Row2, Column2: Coordinates of the arrows' end points
            //HeadLength, HeadWidth: Size of the arrow heads in pixels
            //
            //output parameter:
            //Arrow: The resulting XLD contour
            //
            //The input tuples Row1, Column1, Row2, and Column2 have to be of
            //the same length.
            //HeadLength and HeadWidth either have to be of the same length as
            //Row1, Column1, Row2, and Column2 or have to be a single element.
            //If one of the above restrictions is violated, an error will occur.
            //
            //
            //Init
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            return;
        }

        #endregion

        #region 显示局部图形

        //局部图形做直方图处理显示
        public void ShowPartPic()
        {
            try
            {
                HObject ho_Rectangle;
                HObject ho_ImageReduced, ho_ImageEquHisto;
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_ImageEquHisto);
                foreach (HDrawingObject dobj in drawing_objects)
                {
                    if (dobj.Handle != null)
                    {
                        string status = "";
                        m_Dic.TryGetValue(dobj.ID, out status);
                        if (status == RECT1)
                        {
                            HTuple row1 = dobj.GetDrawingObjectParams("row1");
                            HTuple column1 = dobj.GetDrawingObjectParams("column1");
                            HTuple row2 = dobj.GetDrawingObjectParams("row2");
                            HTuple column2 = dobj.GetDrawingObjectParams("column2");

                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, row1, column1, row2, column2);

                            HOperatorSet.ReduceDomain(Image, ho_Rectangle, out ho_ImageReduced);
                            HOperatorSet.EquHistoImage(ho_ImageReduced, out ho_ImageEquHisto);
                            hsmartControl.HalconWindow.DispImage(new HImage(ho_ImageEquHisto));
                        }
                    }
                }

                ho_ImageReduced.Dispose();
                ho_Rectangle.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void showPartViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPartPic();
        }

        #endregion

        #region 创建ROI事件
        public void SaveROIRegion(string strType, string Modelpath)
        {
            try
            {
                HObject ho_Rect2, ho_Circle, ho_CircleSector2, ho_CircleSector;
                HObject ho_RegionUnion;
                HOperatorSet.GenEmptyObj(out ho_Rect2);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_CircleSector);
                HOperatorSet.GenEmptyObj(out ho_CircleSector2);
                HOperatorSet.GenEmptyObj(out ho_RegionUnion);

                HTuple hv_CircleTup = new HTuple();
                if (strType != "Xld")
                {
                    foreach (var dobj in drawing_objects)
                    {
                        // if (dobj.Handle != (IntPtr)0)
                        if (dobj.Handle != null)
                        {
                            string status = "";
                            m_Dic.TryGetValue(dobj.ID, out status);
                            if (status == Circle_Sector2 && dobj.ID != m_Circle2.ID)
                            {
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple radius = dobj.GetDrawingObjectParams("radius");
                                HTuple startPhi = dobj.GetDrawingObjectParams("start_angle");
                                HTuple endphi = dobj.GetDrawingObjectParams("end_angle");

                                hv_CircleTup.Append(row1);
                                hv_CircleTup.Append(column1);
                                hv_CircleTup.Append(radius);
                                hv_CircleTup.Append(startPhi);
                                hv_CircleTup.Append(endphi);

                                ho_CircleSector.Dispose();
                                HOperatorSet.GenCircleSector(out ho_CircleSector, row1, column1, radius, startPhi, endphi);

                                HTuple radius2 = m_Circle2.GetDrawingObjectParams("radius");
                                ho_CircleSector2.Dispose();
                                HOperatorSet.GenCircleSector(out ho_CircleSector2, row1, column1, radius2, startPhi, endphi);

                                HOperatorSet.Difference(ho_CircleSector, ho_CircleSector2, out ho_RegionUnion);

                                hv_CircleTup.Append(radius - radius2);
                                 
                                HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "CircleArc2Region.tup");
                                break;
                            }
                            else if (status == RECT2)
                            {
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple phi = dobj.GetDrawingObjectParams("phi");
                                HTuple length1 = dobj.GetDrawingObjectParams("length1");
                                HTuple length2 = dobj.GetDrawingObjectParams("length2");

                                hv_CircleTup.Append(row1);
                                hv_CircleTup.Append(column1);
                                hv_CircleTup.Append(phi);
                                hv_CircleTup.Append(length1);
                                hv_CircleTup.Append(length2);

                                ho_Rect2.Dispose();
                                HOperatorSet.GenRectangle2(out ho_Rect2, row1, column1, phi, length1, length2);
                                HOperatorSet.Union2(ho_RegionUnion, ho_Rect2, out ho_RegionUnion); 
                                HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "Rect2Region.tup");
                            }
                            else if (status == Circle)
                            {
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple radius = dobj.GetDrawingObjectParams("radius");

                                hv_CircleTup.Append(row1);
                                hv_CircleTup.Append(column1);
                                hv_CircleTup.Append(radius);

                                ho_Circle.Dispose();
                                HOperatorSet.GenCircle(out ho_Circle, row1, column1, radius);
                                HOperatorSet.Union2(ho_RegionUnion, ho_Circle, out ho_RegionUnion); 
                                HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "CircleRegion.tup");
                            }
                            else if (status == Double_Circle && dobj.ID != m_Circle2.ID)
                            {
                                HTuple row1 = dobj.GetDrawingObjectParams("row");
                                HTuple column1 = dobj.GetDrawingObjectParams("column");
                                HTuple radius = dobj.GetDrawingObjectParams("radius");

                                hv_CircleTup.Append(row1);
                                hv_CircleTup.Append(column1);
                                hv_CircleTup.Append(radius);

                                ho_CircleSector.Dispose();
                                HOperatorSet.GenCircle(out ho_CircleSector, row1, column1, radius);

                                HTuple radius2 = m_Circle2.GetDrawingObjectParams("radius");
                                ho_CircleSector2.Dispose();
                                HOperatorSet.GenCircle(out ho_CircleSector2, row1, column1, radius2);

                                HOperatorSet.Difference(ho_CircleSector, ho_CircleSector2, out ho_RegionUnion);

                                hv_CircleTup.Append(radius - radius2);
                                
                                HOperatorSet.WriteTuple(hv_CircleTup, Modelpath + "DoubleCircleRegion.tup");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (m_DrawXldId.Count != 0)
                    {
                        HObject ho_Contour;
                        HOperatorSet.GenEmptyObj(out ho_Contour);
                        HTuple hv_Rows = new HTuple();
                        HTuple hv_Cols = new HTuple();

                        HOperatorSet.GetDrawingObjectParams(m_DrawXldId[0], "row", out hv_Rows);
                        HOperatorSet.GetDrawingObjectParams(m_DrawXldId[0], "column", out hv_Cols);

                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                        HOperatorSet.GenRegionContourXld(ho_Contour, out ho_RegionUnion, "margin");

                        HOperatorSet.WriteTuple(hv_Rows, Modelpath + "ManualRegionRow.tup");
                        HOperatorSet.WriteTuple(hv_Cols, Modelpath + "ManualRegionColumn.tup");
                        HOperatorSet.WriteRegion(ho_RegionUnion, Modelpath + "ManualRegion.hobj");
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 绘制涂抹 显示匹配信息
        //绘制涂抹
        public void DaubRegion(List<string> listValue, int size)
        {
            HSystem sys = new HSystem();
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            // Local iconic variables 

            HObject ho_Image, ho_Rectangle, ho_UnionObj;
            HObject ho_Circle = null, ho_RegionBorder = null, ho_Circle2;
            // Local control variables 

            HTuple hv_WindowHandle1 = null;
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_UnionObj);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_Circle2);

            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple(), hv_Row2 = new HTuple(), hv_Col2 = new HTuple();

            hv_WindowHandle1 = GetWindowHandle();

            ho_Image = Image;

            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

            ho_UnionObj.Dispose();
            HOperatorSet.GenEmptyObj(out ho_UnionObj);
            string maskPath = listValue[0] + "MaskRegion.hobj";
            if (File.Exists(maskPath))
            {
                HOperatorSet.ReadRegion(out ho_UnionObj, maskPath);
            }

            hsmartControl.HMoveContent = false;
            hsmartControl.HDoubleClickToFitContent = false;
            GetWindowHandle().SetLineWidth(1);

            while (!m_bRightDown)
            {
                Application.DoEvents();
                hv_Row = positionY;
                hv_Column = positionX;

                //check if mouse cursor is over window
                if ((int)((new HTuple(hv_Row.TupleGreaterEqual(0))).TupleAnd(new HTuple(hv_Column.TupleGreaterEqual(
                    0)))) != 0)
                {
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, size);
                    if (m_bMouseDown)
                    {
                        //HObject ExpTmpOutVar_0;
                        //HOperatorSet.ConcatObj(ho_UnionObj, ho_Circle, out ExpTmpOutVar_0);
                        //ho_UnionObj.Dispose();
                        //ho_UnionObj = ExpTmpOutVar_0;

                        HOperatorSet.Union2(ho_UnionObj, ho_Circle, out ho_UnionObj);
                    }

                    HOperatorSet.SetSystem("flush_graphic", "false");


                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    if (Contour != null)
                    {
                        GetWindowHandle().DispObj(Contour);
                    }

                    GetWindowHandle().SetColor("#f0e68c80");
                    GetWindowHandle().SetDraw("fill");
                    GetWindowHandle().DispObj(ho_UnionObj);

                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_Circle);

                    HOperatorSet.SetSystem("flush_graphic", "true");
                    HOperatorSet.WriteRegion(ho_UnionObj, maskPath);
                }
                else
                {
                    HOperatorSet.SetSystem("flush_graphic", "false");
                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    if (Contour != null)
                    {
                        GetWindowHandle().DispObj(Contour);
                    }

                    GetWindowHandle().SetColor("#f0e68c80");
                    GetWindowHandle().SetDraw("fill");
                    GetWindowHandle().DispObj(ho_UnionObj);
                    HOperatorSet.SetSystem("flush_graphic", "true");
                }

                Thread.Sleep(10);
            }

            ho_Rectangle.Dispose();
            //ho_UnionObj.Dispose();
            ho_Circle.Dispose();
            ho_RegionBorder.Dispose();

            hsmartControl.HMoveContent = true;
            hsmartControl.HDoubleClickToFitContent = true;
        }

        //绘制涂抹
        public void DaubRegion2(List<string> listValue, int size)
        {
            HSystem sys = new HSystem();
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            // Local iconic variables 

            HObject ho_Image, ho_Rectangle, ho_UnionObj;
            HObject ho_Circle = null, ho_RegionBorder = null, ho_Circle2;
            // Local control variables 

            HTuple hv_WindowHandle1 = null;
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_UnionObj);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_RegionBorder);
            HOperatorSet.GenEmptyObj(out ho_Circle2);

            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple(), hv_Row2 = new HTuple(), hv_Col2 = new HTuple();

            hv_WindowHandle1 = GetWindowHandle();

            ho_Image = Image;

            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

            ho_UnionObj.Dispose();
            HOperatorSet.GenEmptyObj(out ho_UnionObj);
            string maskPath = listValue[0] + "MaskRegion.hobj";
            if (File.Exists(maskPath))
            {
                HOperatorSet.ReadRegion(out ho_UnionObj, maskPath);
            }

            hsmartControl.HMoveContent = false;
            hsmartControl.HDoubleClickToFitContent = false;

            HTuple hv_button = 0;
            while (hv_button != 4)
            {
                Application.DoEvents();

                //HOperatorSet.GetMbutton(GetWindowHandle(), out hv_Row, out hv_Column, out hv_button);

                HOperatorSet.GetMposition(GetWindowHandle(), out hv_Row, out hv_Column,
                   out hv_button);

                //check if mouse cursor is over window
                if ((int)((new HTuple(hv_Row.TupleGreaterEqual(0))).TupleAnd(new HTuple(hv_Column.TupleGreaterEqual(
                    0)))) != 0)
                {
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, size);
                    if (m_bMouseDown)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_UnionObj, ho_Circle, out ExpTmpOutVar_0);
                        ho_UnionObj.Dispose();
                        ho_UnionObj = ExpTmpOutVar_0;
                    }

                    HOperatorSet.SetSystem("flush_graphic", "false");

                    GetWindowHandle().SetLineWidth(1);

                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    if (Contour != null)
                    {
                        GetWindowHandle().DispObj(Contour);
                    }

                    GetWindowHandle().SetColor("#f0e68c80");
                    GetWindowHandle().SetDraw("fill");
                    GetWindowHandle().DispObj(ho_UnionObj);

                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    GetWindowHandle().DispObj(ho_Circle);

                    HOperatorSet.SetSystem("flush_graphic", "true");
                    HOperatorSet.WriteRegion(ho_UnionObj, maskPath);
                }
                else
                {
                    HOperatorSet.SetSystem("flush_graphic", "false");
                    GetWindowHandle().SetLineWidth(1);
                    GetWindowHandle().DispObj(Image);
                    GetWindowHandle().SetColor("red");
                    GetWindowHandle().SetDraw("margin");
                    if (Contour != null)
                    {
                        GetWindowHandle().DispObj(Contour);
                    }

                    GetWindowHandle().SetColor("#f0e68c80");
                    GetWindowHandle().SetDraw("fill");
                    GetWindowHandle().DispObj(ho_UnionObj);
                    HOperatorSet.SetSystem("flush_graphic", "true");
                }

                Thread.Sleep(5);
            }

            ho_Rectangle.Dispose();
            //ho_UnionObj.Dispose();
            ho_Circle.Dispose();
            ho_RegionBorder.Dispose();

            hsmartControl.HMoveContent = true;
            hsmartControl.HDoubleClickToFitContent = true;
        }

        //涂抹擦除
        public void ClearDaubRegion(string regionPath, int size)
        {
            try
            {
                HSystem sys = new HSystem();
                // Stack for temporary objects 
                HObject[] OTemp = new HObject[20];
                // Local iconic variables 

                HObject ho_Image, ho_Rectangle, ho_UnionObj;
                HObject ho_Circle = null, ho_RegionBorder = null;
                HObject ho_DuabRegion;
                // Local control variables 

                HTuple hv_WindowHandle1 = null;
                HTuple hv_Width = null, hv_Height = null, hv_Button = null;
                HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_DuabRegion);
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_UnionObj);
                HOperatorSet.GenEmptyObj(out ho_Circle);
                HOperatorSet.GenEmptyObj(out ho_RegionBorder);

                hv_WindowHandle1 = GetWindowHandle();

                ho_Image = Image;

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                ho_UnionObj.Dispose();
                HOperatorSet.GenEmptyObj(out ho_UnionObj);
                hv_Button = 0;

                string maskPath = regionPath + "MaskRegion.hobj";
                if (File.Exists(maskPath))
                {
                    HOperatorSet.ReadRegion(out ho_DuabRegion, maskPath);
                }

                hsmartControl.HMoveContent = false;
                hsmartControl.HDoubleClickToFitContent = false;
                while (!m_bRightDown)
                {
                    Application.DoEvents();
                    hv_Row = positionY;
                    hv_Column = positionX;
                    if ((int)((new HTuple(hv_Row.TupleGreaterEqual(0))).TupleAnd(new HTuple(hv_Column.TupleGreaterEqual(
                        0)))) != 0)
                    {
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, size);

                        if (m_bMouseDown)
                        {
                            HOperatorSet.Difference(ho_DuabRegion, ho_Circle, out ho_DuabRegion);
                        }

                        HOperatorSet.SetSystem("flush_graphic", "false");
                        GetWindowHandle().DispObj(Image);
                        //GetWindowHandle().SetColor("#bebebe80");
                        GetWindowHandle().SetColor("red");
                        GetWindowHandle().SetDraw("margin");
                        GetWindowHandle().DispObj(ho_Circle);
                        if (Contour != null)
                        {
                            GetWindowHandle().DispObj(Contour);
                        }
                        GetWindowHandle().SetColor("#f0e68c80");
                        GetWindowHandle().SetDraw("fill");
                        GetWindowHandle().DispObj(ho_DuabRegion);
                        HOperatorSet.SetSystem("flush_graphic", "true");


                        HOperatorSet.WriteRegion(ho_DuabRegion, maskPath);
                    }
                    else
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        GetWindowHandle().DispObj(Image);
                        GetWindowHandle().SetColor("red");
                        GetWindowHandle().SetDraw("margin");
                        if (Contour != null)
                        {
                            GetWindowHandle().DispObj(Contour);
                        }
                        GetWindowHandle().SetColor("#f0e68c80");
                        GetWindowHandle().SetDraw("fill");
                        GetWindowHandle().DispObj(ho_DuabRegion);
                        HOperatorSet.SetSystem("flush_graphic", "true");
                    }

                    Thread.Sleep(15);
                }

                ho_Rectangle.Dispose();
                //ho_UnionObj.Dispose();
                ho_Circle.Dispose();
                ho_RegionBorder.Dispose();

                hsmartControl.HMoveContent = true;
                hsmartControl.HDoubleClickToFitContent = true;
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 显示匹配信息到图形
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="angle"></param>
        /// <param name="score"></param>
        public void ShowMatch(double row, double col, double angle, double score)
        {
            try
            {
                HObject ho_Arrow1 = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Arrow1);
                HObject ho_Arrow2 = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Arrow2);
                HObject ho_ConcatObj = new HObject();
                HOperatorSet.GenEmptyObj(out ho_ConcatObj);

                int width = hv_imageWidth / 30;
                int height = hv_imageWidth / 100;

                ho_Arrow1.Dispose();
                gen_arrow_contour_xld(out ho_Arrow1, 0, 0, 0, width, height, height);
                ho_Arrow2.Dispose();
                gen_arrow_contour_xld(out ho_Arrow2, 0, 0, width, 0, height, height);
                 

                ho_ConcatObj.Dispose();
                HOperatorSet.ConcatObj(ho_Arrow1, ho_Arrow2, out ho_ConcatObj);

                HTuple hv_Mat2d;
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row, col, angle, out hv_Mat2d);
                HOperatorSet.AffineTransContourXld(ho_ConcatObj, out ho_ConcatObj, hv_Mat2d);

                GetWindowHandle().SetColor("cyan");
                GetWindowHandle().DispObj(ho_ConcatObj);

                HTuple hv_row1, hv_col1, hv_row2, hv_col2;
                HOperatorSet.AffineTransPoint2d(hv_Mat2d, 0, width, out hv_row1, out hv_col1); 
                HOperatorSet.AffineTransPoint2d(hv_Mat2d, width, 0, out hv_row2, out hv_col2);

                disp_message(GetWindowHandle(), "x", "window", hv_row1 - 3, hv_col1 + 5, "cyan", "false");
                disp_message(GetWindowHandle(), "y", "window", hv_row2, hv_col2 + 1, "cyan", "false"); 

                ho_Arrow1.Dispose();
                ho_Arrow2.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        void XY(HTuple hv_row, HTuple hv_col, HTuple hv_Phi)
        {
            HObject ho_Arrow, ho_Arrow1;
            HTuple hv_Phi1 = null;
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            hv_Phi1 = hv_Phi + ((new HTuple(180)).TupleRad());
            ho_Arrow.Dispose();
            gen_arrow_contour_xld(out ho_Arrow, hv_row + ((((hv_Phi1 + ((new HTuple(-90)).TupleRad()
                ))).TupleSin()) * 0), hv_col - ((((hv_Phi1 + ((new HTuple(90)).TupleRad()))).TupleCos()
                ) * 0), hv_row + ((((hv_Phi1 + ((new HTuple(-90)).TupleRad()))).TupleSin()) * hv_imageHeight / 20),
                hv_col - ((((hv_Phi1 + ((new HTuple(-90)).TupleRad()))).TupleCos()) * hv_imageHeight / 20), hv_imageHeight / 60, hv_imageHeight / 60);
            ho_Arrow1.Dispose();
            gen_arrow_contour_xld(out ho_Arrow1, hv_row + ((hv_Phi1.TupleSin()) * 0), hv_col - ((hv_Phi1.TupleCos()
                ) * 0), hv_row + ((hv_Phi1.TupleSin()) * hv_imageHeight / 20), hv_col - ((hv_Phi1.TupleCos()) * hv_imageHeight / 20), hv_imageHeight / 60, hv_imageHeight / 60);
            HOperatorSet.SetLineWidth(GetWindowHandle(), 1);
            HOperatorSet.SetColor(GetWindowHandle(), "red");
            HOperatorSet.DispObj(ho_Arrow, GetWindowHandle());
            HOperatorSet.DispObj(ho_Arrow1, GetWindowHandle());
            disp_message(GetWindowHandle(), "Y", "image", (hv_row + ((((hv_Phi1 + ((new HTuple(-90)).TupleRad()
                ))).TupleSin()) * hv_imageHeight / 20)) - 7, (hv_col - ((((hv_Phi1 + ((new HTuple(-90)).TupleRad()
                ))).TupleCos()) * hv_imageHeight / 20)) + 7, "red", "false");
            disp_message(GetWindowHandle(), "X", "image", (hv_row + ((hv_Phi1.TupleSin()
                 ) * 50)) + 10, (hv_col - ((hv_Phi1.TupleCos()) * hv_imageHeight / 20)) - 10, "red", "false");

            ho_Arrow.Dispose();
            ho_Arrow1.Dispose();
            return;
        }

        public void ShowMatch(double[] row, double[] col, double[] angle, double score)
        {
            try
            {
                HObject ho_Arrow1 = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Arrow1);
                HObject ho_Arrow2 = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Arrow2);
                HObject ho_ConcatObj = new HObject();
                HOperatorSet.GenEmptyObj(out ho_ConcatObj);

                int width = hv_imageWidth / 40;
                int height = hv_imageWidth / 100;

                ho_Arrow1.Dispose();
                gen_arrow_contour_xld(out ho_Arrow1, 0, 0, 0, width, height, height);
                ho_Arrow2.Dispose();
                gen_arrow_contour_xld(out ho_Arrow2, 0, 0, width, 0, height, height);

                ho_ConcatObj.Dispose();
                HOperatorSet.ConcatObj(ho_Arrow1, ho_Arrow2, out ho_Arrow1);

                for (int i = 0; i < row.Count(); i++)
                {
                    HTuple hv_Mat2d;
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, row[i], col[i], angle[i], out hv_Mat2d);
                    HOperatorSet.AffineTransContourXld(ho_Arrow1, out ho_ConcatObj, hv_Mat2d);
                    
                    GetWindowHandle().SetColor("cyan");
                    GetWindowHandle().DispObj(ho_ConcatObj);
                     

                    HTuple hv_row1, hv_col1, hv_row2, hv_col2;
                    HOperatorSet.AffineTransPoint2d(hv_Mat2d, 0, width, out hv_row1, out hv_col1);
                    HOperatorSet.AffineTransPoint2d(hv_Mat2d, width, 0, out hv_row2, out hv_col2);

                    if(row.Count() < 10)
                    {
                        disp_message(GetWindowHandle(), "x", "window", hv_row1-3, hv_col1 + 3, "cyan", "false");
                        disp_message(GetWindowHandle(), "y", "window", hv_row2, hv_col2 + 3, "cyan", "false");
                    } 
                }

                //disp_message(GetWindowHandle(), score, "window", row, col, "red", "false");

                ho_Arrow1.Dispose();
                ho_Arrow2.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 读取图片显示伪彩色图 拆分图像
        [DllImport("COpenCvColor.dll", EntryPoint = "LoadMyImage", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LoadMyImage(string filename);

        [DllImport("COpenCvColor.dll", EntryPoint = "TransImage2")]
        public static extern IntPtr TransImage2(long width, long height, string type, IntPtr pointData, out IntPtr ptr1, out IntPtr ptr2, out IntPtr ptr3, int index);

        [DllImport("COpenCvColor.dll", EntryPoint = "TransImage")]
        public static extern bool TransImage(long width, long height, string type, IntPtr pointData, out IntPtr ptr1, out IntPtr ptr2, out IntPtr ptr3, int index);

        [DllImport("COpenCvColor.dll", EntryPoint = "TransImage3")]
        public static extern bool TransImage3(long width, long height, string type, IntPtr pointData, out IntPtr ptr1, out IntPtr ptr2, out IntPtr ptr3, int index);


        [DllImport("COpenCvColor.dll", EntryPoint = "Get3Image")]
        public static extern bool Get3Image(long width, long height, string type, IntPtr pointData, out IntPtr ptr1, out IntPtr ptr2, out IntPtr ptr3);

        //Color Image
        private void colorImageToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((null == Image) || (!Image.IsInitialized()) || Image.Key == (IntPtr)0)
                {
                    return;
                }

                HObject ho_ImageRed, ho_ImageGreen;
                HObject ho_ImageBlue, ho_MultiChannelImage, ho_Image1, ho_Image2;
                HObject ho_Image3, ho_ImageGray;
                // Initialize local and output iconic variables  
                HOperatorSet.GenEmptyObj(out ho_ImageRed);
                HOperatorSet.GenEmptyObj(out ho_ImageGreen);
                HOperatorSet.GenEmptyObj(out ho_ImageBlue);
                HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);
                HOperatorSet.GenEmptyObj(out ho_Image1);
                HOperatorSet.GenEmptyObj(out ho_Image2);
                HOperatorSet.GenEmptyObj(out ho_Image3);
                HOperatorSet.GenEmptyObj(out ho_ImageGray);

                //HTuple pointer1, type1, width1, height1;
                //HOperatorSet.GetImagePointer1(Image, out pointer1, out type1, out width1, out height1);
                //IntPtr intpr12, intpr22, intpr32;
                //Get3Image(width1, height1, type1, pointer1, out intpr12, out intpr22, out intpr32);
                //HObject ho_Imagetest;
                //HOperatorSet.GenEmptyObj(out ho_Imagetest);
                //ho_Imagetest.Dispose();
                //HOperatorSet.GenImage1(out ho_Imagetest, "byte", width1, height1 / 3, (HTuple)intpr12);
                //DisplayImage(new HImage(ho_Imagetest));
                //return;

                if (colorImageToolStripMenuItem.Checked)
                {
                    if (false)
                    {
                        ho_ImageRed.Dispose(); ho_ImageGreen.Dispose(); ho_ImageBlue.Dispose();
                        HOperatorSet.TransToRgb(Image, Image, Image, out ho_ImageRed, out ho_ImageGreen, out ho_ImageBlue, "hls");
                        ho_MultiChannelImage.Dispose();
                        HOperatorSet.Compose3(ho_ImageRed, ho_ImageGreen, ho_ImageBlue, out ho_MultiChannelImage);

                        DisplayImageNo(new HImage(ho_MultiChannelImage));
                    }

                    //利用OpenCV来实现伪彩色图
                    //HOperatorSet.WriteImage(Image, "png", 0, "grayPng.png");
                    //if(LoadMyImage("GrayPng.png"))
                    //{
                    //    HOperatorSet.ReadImage(out ho_MultiChannelImage, "colorMap.png");
                    //    DisplayImage(new HImage(ho_MultiChannelImage));
                    //}

                    HTuple pointer, type, width, height;
                    IntPtr intpr1, intpr2, intpr3;
                    HOperatorSet.GetImagePointer1(Image, out pointer, out type, out width, out height);

                    if (type == "uint2")
                    {
                        IntPtr intpr = TransImage2(width, height, type, pointer, out intpr1, out intpr2, out intpr3, 0);

                        HObject ho_Image;
                        HOperatorSet.GenEmptyObj(out ho_Image);
                        ho_Image.Dispose();
                        HOperatorSet.GenImage3(out ho_Image, "byte", width, height, (HTuple)intpr1, (HTuple)intpr2, (HTuple)intpr3);
                        DisplayImageNo(new HImage(ho_Image));
                    }
                    else if (type == "real")
                    {
                        HObject ho_Image;
                        HOperatorSet.GenEmptyObj(out ho_Image);
                        ho_Image.Dispose();
                        HOperatorSet.ScaleImage(Image, out ho_Image, 833.333, 32768);
                        //拉升图像 
                        HOperatorSet.ScaleImage(ho_Image, out ho_Image, 1, -30000);
                        HOperatorSet.ConvertImageType(ho_Image, out ho_Image, "uint2");
                        HOperatorSet.GetImagePointer1(ho_Image, out pointer, out type, out width, out height);

                        TransImage2(width, height, type, pointer, out intpr1, out intpr2, out intpr3, 0);
                        HOperatorSet.GenImage3(out ho_Image, "byte", width, height, (HTuple)intpr1, (HTuple)intpr2, (HTuple)intpr3);
                        DisplayImageNo(new HImage(ho_Image));
                    }
                    else
                    {
                        bool intpr = TransImage(width, height, type, pointer, out intpr1, out intpr2, out intpr3, 0);

                        HObject ho_Image;
                        HOperatorSet.GenEmptyObj(out ho_Image);
                        ho_Image.Dispose();
                        HOperatorSet.GenImage3(out ho_Image, "byte", width, height, (HTuple)intpr1, (HTuple)intpr2, (HTuple)intpr3);
                        DisplayImageNo(new HImage(ho_Image));
                    }

                    // hsmartControl.HalconWindow.DispImage(new HImage(ho_Image));
                }
                else
                {
                    //ho_Image1.Dispose(); ho_Image2.Dispose(); ho_Image3.Dispose();
                    //HOperatorSet.Decompose3(ho_MultiChannelImage, out ho_Image1, out ho_Image2, out ho_Image3
                    //    );

                    //ho_ImageGray.Dispose();
                    //HOperatorSet.Rgb3ToGray(ho_ImageRed, ho_ImageGreen, ho_ImageBlue, out ho_ImageGray
                    //    );

                    FitImageToWindow(Image, null);
                }

                ho_ImageRed.Dispose();
                ho_ImageGreen.Dispose();
                ho_ImageBlue.Dispose();
                ho_MultiChannelImage.Dispose();
                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                ho_ImageGray.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void jETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(0);
        }

        private void colorHotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(1);
        }

        private void colorHSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(2);
        }
        
        private void colorOCEANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(3);
        } 

        private void colorPINKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(4);
        }

        private void colorRAINBOWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(5);
        }

        private void colorBONEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(6);
        }

        private void colorCOOLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetColor(7);
        }
         
        private void SetColor(int index)
        {
            try
            {
                if ((null == Image) || (!Image.IsInitialized()) || Image.Key == (IntPtr)0)
                {
                    return;
                }

                //利用OpenCV来实现伪彩色图 
                HTuple pointer, type, width, height;
                HOperatorSet.GetImagePointer1(Image, out pointer, out type, out width, out height);

                IntPtr intpr1, intpr2, intpr3;
                if (type == "uint2")
                {
                    IntPtr intpr = TransImage2(width, height, type, pointer, out intpr1, out intpr2, out intpr3, index);
                }
                else if (type == "real")
                {
                    HObject ho_Image2;
                    HOperatorSet.GenEmptyObj(out ho_Image2);
                    ho_Image2.Dispose();
                    HOperatorSet.ScaleImage(Image, out ho_Image2, 833.333, 32768);
                    //拉升图像 
                    HOperatorSet.ScaleImage(ho_Image2, out ho_Image2, 1, -30000);
                    HOperatorSet.ConvertImageType(ho_Image2, out ho_Image2, "uint2");
                    HOperatorSet.GetImagePointer1(ho_Image2, out pointer, out type, out width, out height);

                    TransImage2(width, height, type, pointer, out intpr1, out intpr2, out intpr3, index);
                }
                else
                {
                    bool intpr = TransImage(width, height, type, pointer, out intpr1, out intpr2, out intpr3, index);
                }

                HObject ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Image);
                ho_Image.Dispose();
                HOperatorSet.GenImage3(out ho_Image, "byte", width, height, (HTuple)intpr1, (HTuple)intpr2, (HTuple)intpr3);
                DisplayImageNo(new HImage(ho_Image));
            }
            catch (Exception ex)
            {

            }
        }

        public void Get3Image(HObject ho_Image, out HObject image1, out HObject image2, out HObject image3)
        {
            HTuple pointer1, type1, width1, height1;
            HOperatorSet.GetImagePointer1(ho_Image, out pointer1, out type1, out width1, out height1);
            IntPtr intpr12, intpr22, intpr32;
            Get3Image(width1, height1, type1, pointer1, out intpr12, out intpr22, out intpr32);
            
            HOperatorSet.GenImage1(out image1, "byte", width1, height1 / 3, (HTuple)intpr12);
            HOperatorSet.GenImage1(out image2, "byte", width1, height1 / 3, (HTuple)intpr22);
            HOperatorSet.GenImage1(out image3, "byte", width1, height1 / 3, (HTuple)intpr32);
        }
        #endregion

    }

}
