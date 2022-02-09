using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCvController
{
    public class OpenCVTest
    {

        /// <summary>
        /// ROI分割
        /// </summary>
        public void TestROI()
        {
            //原图
            Mat src = new Mat(@"D:\demo\Image\1.jpg", ImreadModes.Color);
            Mat dst = new Mat();
            //图像中指定的区域
            Rect roi = new Rect(576, 153, 600, 600);
            //将指定区域复制给新的图像
            Mat Roi = new Mat(src, roi);
            Cv2.ImShow("src", src);
            Cv2.ImShow("roi", Roi);
            Cv2.WaitKey();
        }
    }


    class CVSImage
    {
        private string path;
        /// <summary>
        /// 图像文件的全路径
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        private int width;
        /// <summary>
        /// 图像的宽
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        private int height;
        /// <summary>
        /// 图像的高
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        private long size;
        /// <summary>
        /// 图像的尺寸 size = width * height
        /// </summary>
        public long Size
        {
            get
            {
                return size;
            }
        }
        
        private Mat srcImg;

        public CVSImage()
        {
            path = "";
            width = 0;
            height = 0;
            size = 0;
            srcImg = new Mat();
        }

        public CVSImage(string imagePath, int readMode = 1)
        {
            path = imagePath;
            srcImg = Cv2.ImRead(path, (ImreadModes)readMode);
            width = srcImg.Width;
            height = srcImg.Height;
            size = width * height;
        }

        /// <summary>
        /// 预览图像
        /// </summary>
        /// <param name="winName">窗口名</param>
        /// <param name="windowType">0-Normal(可调窗口尺寸),1-Autosize(图像原始大小)</param>
        /// <param name="showTime">窗口停留时间（0则一直等待输入）</param>
        /// <returns>异常返回-1，正常返回1</returns>
        public int showImage(string winName, int windowType = 1, int showTime = 0)
        {
            if (srcImg == null || showTime < 0)
            {
                return -1;
            }
            WindowMode mode = (WindowMode)windowType;
            Cv2.NamedWindow(winName, mode);
            Cv2.ImShow(winName, srcImg);
            Cv2.WaitKey(showTime);
            return 0;
        }
         
        static void Main(string[] args)
        {
            CVSImage cvImage = new CVSImage("D:\\Asuna.jpg");
            Console.WriteLine("image width: {0}", cvImage.Width);
            Console.WriteLine("image height: {0}", cvImage.Height);
            cvImage.showImage("Asuna", 0);
        }
        
    }


}
