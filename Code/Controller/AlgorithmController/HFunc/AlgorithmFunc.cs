using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmController
{
    public class AlgorithmFunc
    {
        //中值滤波
        public static HImage MedianRun(HImage img, int Radius)
        {
            try
            {
                HImage image = img.MedianImage("circle", Radius, "mirrored");
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //均值滤波
        public static HImage MeanRun(HImage img, int mask)
        {
            try
            {
                HImage image = img.MeanImage(mask, mask);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //高斯滤波
        public static HImage GaussRun(HImage img, int size)
        {
            try
            {
                HImage image = img.GaussFilter(size);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sobel滤波
        public static HImage SobelAmp(HImage img, int size)
        {
            try
            {
                HImage image = img.SobelAmp("sum_abs", size);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Gamma校正
        public static HImage GammaFilter(HImage img)
        {
            try
            {
                HImage image = img.GammaImage(0.416667, 0.099, 0.018, (double)255, "true");
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //灰度腐蚀
        public static HImage GrayErosion(HImage img, int MaskHeight, int MaskWidth)
        {
            try
            {
                HImage image = img.GrayErosionRect(MaskHeight, MaskWidth);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //灰度膨胀
        public static HImage GrayDilation(HImage img, int MaskHeight, int MaskWidth)
        {
            try
            {
                HImage image = img.GrayDilationRect(MaskHeight, MaskWidth);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //图片翻转 model: row column
        public static HImage MirrorImage(HImage img, string mode)
        {
            try
            {
                HImage image = img.MirrorImage(mode);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //直方图
        public static HImage EquHistoImage(HImage img)
        {
            try
            {
                HImage image = img.EquHistoImage();
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //旋转图片
        public static HImage RotateImage(HImage img, double phi)
        {
            try
            {
                HImage image = img.RotateImage(phi, "constant");
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //阈值分割
        public static HImage ThresholdImage(HImage img, double minGray, double MaxGray)
        {
            try
            {
                HRegion ho_Region = img.Threshold(minGray, MaxGray);
                HImage image = img.ReduceDomain(ho_Region);
                ho_Region.Dispose();
                img.Dispose();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //均值二值化
        public static HImage BinaryThresholdImage(HImage img, string strParam)
        {
            try
            {
                HTuple hv_usedThreshold = new HTuple();
                HRegion ho_Region = img.BinaryThreshold("max_separability", strParam, out hv_usedThreshold);
                HImage image = img.ReduceDomain(ho_Region);
                ho_Region.Dispose();
                img.Dispose();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        } 
        
        //动态阈值分割
        public static HImage DynThresholdImage(HImage orignImage, HImage img, double offset, string lightOrDark)
        {
            try
            {
                HTuple hv_usedThreshold = new HTuple();  
                HRegion ho_Region = orignImage.DynThreshold(img, offset, lightOrDark);
                HImage image = img.ReduceDomain(ho_Region);
                ho_Region.Dispose();
                img.Dispose();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //图像反转
        public static HImage InverseImage(HImage img)
        {
            try
            {
                HImage image = img.InvertImage();
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static double GrayFeatureImage(HImage img, string strParam)
        {
            try
            {
                HTuple hv_Width, hv_Height;
                img.GetImageSize(out hv_Width, out hv_Height);

                HObject ho_Region;
                HOperatorSet.GenEmptyObj(out ho_Region);
                ho_Region.Dispose();
                HOperatorSet.GenRectangle1(out ho_Region, 0, 0, hv_Height, hv_Width);

                HTuple hv_Value;
                HOperatorSet.GrayFeatures(ho_Region, img, strParam, out hv_Value);
                ho_Region.Dispose();
                return Math.Round(hv_Value.D, 0);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //ScalueImage
        public static HImage ScaleImageMax(HImage img)
        {
            try
            {
                HImage image = img.ScaleImageMax();
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //彩色转灰度
        public static HImage RGBToGrayImage(HImage img)
        {
            try
            {
                HImage image = img.Rgb1ToGray();
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        //提取单通道
        public static HImage DeCompose3Image(HImage img, string strParam)
        {
            try
            {
                HImage ho_ImageR, ho_ImageG, ho_ImageB;
                ho_ImageR = img.Decompose3(out ho_ImageG, out ho_ImageB);

                img.Dispose();
                if (strParam.Contains("H") || strParam.Contains("S") || strParam.Contains("V"))
                {
                    HImage ho_ImageH, ho_ImageS, ho_ImageV;
                    ho_ImageH = ho_ImageR.TransFromRgb(ho_ImageG, ho_ImageB, out ho_ImageS, out ho_ImageV, "hsv");

                    ho_ImageR.Dispose();ho_ImageG.Dispose();ho_ImageB.Dispose();
                    if (strParam.Contains("H"))
                    {
                        ho_ImageS.Dispose();
                        ho_ImageV.Dispose();
                        return ho_ImageH;
                    }
                    else if (strParam.Contains("S"))
                    {
                        ho_ImageH.Dispose();
                        ho_ImageV.Dispose();
                        return ho_ImageS;
                    }
                    else if (strParam.Contains("V"))
                    {
                        ho_ImageH.Dispose();
                        ho_ImageS.Dispose();
                        return ho_ImageV;
                    }
                    else
                    {
                        ho_ImageS.Dispose();
                        ho_ImageV.Dispose();
                        return ho_ImageH;
                    }
                }
                else
                {
                    if (strParam.Contains("R"))
                    {
                        ho_ImageG.Dispose();
                        ho_ImageB.Dispose();
                        return ho_ImageR;
                    }
                    else if (strParam.Contains("G"))
                    {
                        ho_ImageR.Dispose();
                        ho_ImageB.Dispose();
                        return ho_ImageG;
                    }
                    else if (strParam.Contains("B"))
                    {
                        ho_ImageR.Dispose();
                        ho_ImageG.Dispose();
                        return ho_ImageB;
                    }
                    else
                    {
                        ho_ImageG.Dispose();
                        ho_ImageB.Dispose();
                        return ho_ImageR;
                    }
                }               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //灰度转彩色
        public static HImage GrayToRGBImage(HImage img)
        {
            try
            {
                HObject ho_ImageRed, ho_ImageGreen;
                HObject ho_ImageBlue, ho_MultiChannelImage; 
                // Initialize local and output iconic variables  
                HOperatorSet.GenEmptyObj(out ho_ImageRed);
                HOperatorSet.GenEmptyObj(out ho_ImageGreen);
                HOperatorSet.GenEmptyObj(out ho_ImageBlue);
                HOperatorSet.GenEmptyObj(out ho_MultiChannelImage);  

                ho_ImageRed.Dispose(); ho_ImageGreen.Dispose(); ho_ImageBlue.Dispose();
                HOperatorSet.TransToRgb(img, img, img, out ho_ImageRed, out ho_ImageGreen,
                    out ho_ImageBlue, "hls");
                ho_MultiChannelImage.Dispose();
                HOperatorSet.Compose3(ho_ImageRed, ho_ImageGreen, ho_ImageBlue, out ho_MultiChannelImage);

                ho_ImageRed.Dispose();
                ho_ImageGreen.Dispose();
                ho_ImageBlue.Dispose();

                return new HImage(ho_MultiChannelImage);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //图像锐化
        public static HImage EmphasizeImage(HImage img, int maskWidth, int maskHeight, double factor)
        {
            try
            {
                HImage image = img.Emphasize(maskWidth, maskHeight, factor);
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //亮度调节
        public static HImage ScaleImage(HImage img, double mult, double add)
        {
            try
            {
                HImage image = img.ScaleImage(mult, add); 
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //改变尺寸
        public static HImage ZoomImage(HImage img, int Width, int Height)
        {
            try
            {
                HImage image = img.ZoomImageSize(Width, Height, "constant");
                img.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //字符旋转
        public static HImage TextLineImage(HImage img, int charHeight)
        {
            try
            {
                HTuple hv_OrientationAngle;
                HOperatorSet.TextLineOrientation(img, img, charHeight, (new HTuple(-90)).TupleRad(), (new HTuple(90)).TupleRad(), out hv_OrientationAngle); 
                HImage image = img.RotateImage(((-hv_OrientationAngle) / ((new HTuple(180)).TupleRad())) * 180, "constant");
                img.Dispose(); 
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
