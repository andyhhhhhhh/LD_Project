using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonController
{
    public class PanelToBmp
    {
        ///   将容器内容转成图片导出,这里的controller就是this
        /// </summary>
        public static void OutTheControllerToPic(Control control)
        {

            Bitmap bitmap = new Bitmap(control.Width, control.Height);
            DrawToBitmap(control, bitmap, new Rectangle(0, 0, control.Width, control.Height));
            bool isSave = true;
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "图片保存";
            saveImageDialog.Filter = @"png|*.png|jpeg|*.jpg|bmp|*.bmp|gif|*.gif";
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveImageDialog.FileName.ToString();
                if (fileName != "" && fileName != null)
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();
                    System.Drawing.Imaging.ImageFormat imgformat = null;
                    if (fileExtName != "")
                    {
                        switch (fileExtName)
                        {
                            case "jpg":
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case "gif":
                                imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            case "png":
                                imgformat = System.Drawing.Imaging.ImageFormat.Png;
                                break;
                            default:
                                MessageBox.Show("只能存取为: jpg,bmp,gif,png 格式");
                                isSave = false;
                                break;
                        }
                    }
                    //默认保存为JPG格式  
                    if (imgformat == null)
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }
                    if (isSave)
                    {
                        try
                        {
                            bitmap.Save(fileName, imgformat);
                            MessageBox.Show("图片已经成功保存!");
                        }
                        catch
                        {
                            MessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 导出图片从控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="fileName">文件名</param>
        public static void OutTheControllerToPic(Control control, string fileName)
        {
            Bitmap bitmap = new Bitmap(control.Width, control.Height);
            DrawToBitmap(control, bitmap, new Rectangle(0, 0, control.Width, control.Height)); 
            System.Drawing.Imaging.ImageFormat imgformat = System.Drawing.Imaging.ImageFormat.Jpeg; 
            bitmap.Save(fileName, imgformat);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);
        /// <summary>
        /// 支持呈现到指定的位图。
        /// </summary>
        public static Bitmap DrawToBitmap(Control control, Bitmap bitmap, Rectangle targetBounds)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            if (((targetBounds.Width <= 0) || (targetBounds.Height <= 0)) || ((targetBounds.X < 0) || (targetBounds.Y < 0)))
            {
                throw new ArgumentException("targetBounds");
            }
            Bitmap image = new Bitmap(control.Width, control.Height, bitmap.PixelFormat);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                IntPtr hdc = graphics.GetHdc();
                SendMessage(new HandleRef(control, control.Handle), 0x317, hdc, (IntPtr)30);
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    IntPtr handle = graphics2.GetHdc();
                    BitBlt(new HandleRef(graphics2, handle), 0, 0, control.Width, control.Height, new HandleRef(graphics, hdc), targetBounds.X, targetBounds.Y, 0xcc0020);
                    graphics2.ReleaseHdcInternal(handle);
                }
                graphics.ReleaseHdcInternal(hdc);
            }
            return image;
        }
    }
}

