using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceController
{
    public class ZebraPicControl
    {
        public static string printALLPic()
        {
            Image image = new Image();
            Bitmap img = image.CreatImage("料號 ：A1234567890\n批次 ：3456789056789\n" +
                                          "品名 ：FERRITE BEAD 4690\n            345678\n制造日期 ：2017-03-09\n收料日期 ：2017-06-17", new Font("新細明體", 26));
            img.Save("E:\\All.bmp", ImageFormat.Bmp);

            string imgCode = image.ConvertImageToCode(img);

            var t = ((img.Size.Width / 8 + ((img.Size.Width % 8 == 0) ? 0 : 1)) * img.Size.Height).ToString();
            var w = (img.Size.Width / 8 + ((img.Size.Width % 8 == 0) ? 0 : 1)).ToString();
            string zpl = string.Format("~DGR:imgName.GRF,{0},{1},{2}", t, w, imgCode);

            return zpl;
        }

        public static string PrinterZPLCCommand(string str, string code2D)
        {
            //return $"^XA^{str}^XZ";
           // return $"^XA^SZ2^JMA^MMP^MCY^PMN^PW1215^JZY^LH0,0^LRN^XZ^XA^FO800,100^BQ,2,10^FDQA,##0123456789ABCDEF#####12345^FS{str}^XZ";
            return $"^XA^FO900,100^BQ,2,10^FDQA,{code2D}^FS{str}^XZ";
        }

        public static void CitizenPrint()
        {
            try
            {
                string zpl = default(string);
                string imgCode1 = default(string);
                string imgCode2 = default(string);

                string PrinterMATNR = "07A00000003L";
                string PrinterCHARG = "B217308010";
                string PrinterMAKTX = "FERRITE BEAD 470ΩN";
                string PrinterMAKTX2 = "/A 0402";
                string PrinterHSDAT = "2017-03-08";
                string PrinterCURRENTDAT = string.Format(DateTime.Now.ToString("yyyy-MM-dd"));

                BitmapHelper bitmapHelper = new BitmapHelper();

                Bitmap img1 = bitmapHelper.CreatImage($"料號 ：{ PrinterMATNR}\n批次 ：{ PrinterCHARG}", new Font("PMingLiU", 50));
                img1.Save("E:\\Top.bmp", ImageFormat.Bmp);
                imgCode1 = bitmapHelper.ConvertImageToCode(img1);
                var t = ((img1.Size.Width / 8 + ((img1.Size.Width % 8 == 0) ? 0 : 1)) * img1.Size.Height).ToString();
                var w = (img1.Size.Width / 8 + ((img1.Size.Width % 8 == 0) ? 0 : 1)).ToString();
                zpl = $"~DGR:Top.GRF,{t},{w},{imgCode1}^FO140,280^XGR:Top.GRF,1,1^FS ";

                Bitmap img2 = bitmapHelper.CreatImage($"品名 ：{PrinterMAKTX}\n              {PrinterMAKTX2}\n制造日期 ：{PrinterHSDAT}\n收料日期 ：{PrinterCURRENTDAT}", new Font("PMingLiU", 36));
                img2.Save("E:\\Bottom.bmp", ImageFormat.Bmp);
                imgCode2 = bitmapHelper.ConvertImageToCode(img2);
                t = ((img2.Size.Width / 8 + ((img2.Size.Width % 8 == 0) ? 0 : 1)) * img2.Size.Height).ToString();
                w = (img2.Size.Width / 8 + ((img2.Size.Width % 8 == 0) ? 0 : 1)).ToString();
                zpl += $"~DGR:Bottom.GRF,{t},{w},{imgCode2}^FO140,480^XGR:Bottom.GRF,1,1^FS "; 

                string szString = PrinterZPLCCommand(zpl, "123");
                if (!string.IsNullOrEmpty(szString))
                {
                    //PrinterHelper.SendStringToPrinter("Citizen CL-S631Z", szString);
                    MessageBox.Show("打印机任务已传");
                }
                else
                {
                    throw new Exception("打印机错误");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public static void drawFinalLable(string printName, string partNumber, string batch, string partname1, string partname2, string manuDate, string nowDate = "")
        {
            var outBitmap = new Bitmap("E:\\make\\final\\Resource.png", false);

            Image image = new Image();
            string imgInfo1 = $"07A00000003L";
            int length = partNumber.Length > 12 ? 45 : 48;
            Bitmap img1 = image.CreatImage(partNumber.Trim(), new Font("Arial", length, FontStyle.Bold)); 

            string imgInfo2 = $"B217308010";
            Bitmap img2 = image.CreatImage(batch, new Font("Arial", 48, FontStyle.Bold)); 


            string imgInfo3 = $"FERRITE BEAD 470ΩN";
            Bitmap img3 = image.CreatImage(partname1, new Font("Arial", 26, FontStyle.Regular)); 

            string imgInfo4 = $"/A 0402";
            Bitmap img4 = image.CreatImage(partname2, new Font("Arial", 26, FontStyle.Regular)); 

            string imgInfo5 = $"2017-03-08";
            Bitmap img5 = image.CreatImage(manuDate, new Font("Arial", 32, FontStyle.Bold)); 

            string imgInfo6 = nowDate == "" ? $"{DateTime.Now.Date:yyyy-MM-dd}" : nowDate; 
            Bitmap img6 = image.CreatImage(imgInfo6, new Font("Arial", 32, FontStyle.Bold)); 

            DrawPic(img1, 218, 0, 12, 7, outBitmap);
            DrawPic(img2, 218, 93, 10, 7, outBitmap);

            DrawPic(img3, 164, 195, 7, 7, outBitmap);
            DrawPic(img4, 164, 280, 7, 7, outBitmap);

            DrawPic(img5, 260, 355, 7, 7, outBitmap);
            DrawPic(img6, 260, 432, 7, 7, outBitmap);

            //bitmapSave(outBitmap, "E:\\make\\final\\Lable.bmp");
            string strCode2D = string.Format("######{0}{1}", partNumber, batch);
         
            string zpl = Convert(outBitmap);
            string szString = PrinterZPLCCommand(zpl, strCode2D);
            if (!string.IsNullOrEmpty(szString))
            {
                PrinterHelper.SendStringToPrinter(printName, szString);
                //MessageBox.Show("打印机任务已传");
            }
            else
            {
                throw new Exception("打印机错误");
            }

            //outBitmap.Dispose(); 
        }
        
        public static  string Convert(Bitmap bm/*, string s_FilePath*/)

        {
            int b = 0;

            long n = 0;

            long clr;

            StringBuilder sb = new StringBuilder();

            sb.Append("~DGR:ZLOGO.GRF,");

            //Bitmap bm;

            int w = ((bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1)) * bm.Size.Height);

            int h = (bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1));



            sb.Append(w.ToString().PadLeft(5, '0') + "," + h.ToString().PadLeft(3, '0') + ",\n");

            using (Bitmap bmp = new Bitmap(bm.Size.Width, bm.Size.Height))

            {
                for (int y = 0; y < bm.Size.Height; y++)

                {
                    for (int x = 0; x < bm.Size.Width; x++)

                    {
                        b = b * 2;

                        clr = bm.GetPixel(x, y).ToArgb();

                        string s = clr.ToString("X");



                        if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)

                        {
                            bmp.SetPixel(x, y, bm.GetPixel(x, y));

                            b++;

                        }

                        n++;

                        if (x == (bm.Size.Width - 1))

                        {
                            if (n < 8)

                            {
                                b = b * (2 ^ (8 - (int)n));



                                sb.Append(b.ToString("X").PadLeft(2, '0'));

                                b = 0;

                                n = 0;

                            }

                        }

                        if (n >= 8)

                        {
                            sb.Append(b.ToString("X").PadLeft(2, '0'));

                            b = 0;

                            n = 0;

                        }

                    }

                }

                sb.Append("^FO300,540^XGR:ZLOGO.GRF,2,2^FS");

            }

            return sb.ToString();



        }

        //带位置
        public string Convert(string s_FilePath, int xtop, int ytop)

        {
            int b = 0;

            long n = 0;

            long clr;

            StringBuilder sb = new StringBuilder();

            sb.Append("~DGR:ZLOGO.GRF,");

            Bitmap bm = new Bitmap(s_FilePath);

            int w = ((bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1)) * bm.Size.Height);

            int h = (bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1));



            sb.Append(w.ToString().PadLeft(5, '0') + "," + h.ToString().PadLeft(3, '0') + ",\n");

            using (Bitmap bmp = new Bitmap(bm.Size.Width, bm.Size.Height))

            {
                for (int y = 0; y < bm.Size.Height; y++)

                {
                    for (int x = 0; x < bm.Size.Width; x++)

                    {
                        b = b * 2;

                        clr = bm.GetPixel(x, y).ToArgb();

                        string s = clr.ToString("X");



                        if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)

                        {
                            bmp.SetPixel(x, y, bm.GetPixel(x, y));

                            b++;

                        }

                        n++;

                        if (x == (bm.Size.Width - 1))

                        {
                            if (n < 8)

                            {
                                b = b * (2 ^ (8 - (int)n));



                                sb.Append(b.ToString("X").PadLeft(2, '0'));

                                b = 0;

                                n = 0;

                            }

                        }

                        if (n >= 8)

                        {
                            sb.Append(b.ToString("X").PadLeft(2, '0'));

                            b = 0;

                            n = 0;

                        }

                    }

                }

                sb.Append(string.Format("^FO{0},{1}^XGR:ZLOGO.GRF,2,2^FS", xtop, ytop));

            }

            return sb.ToString();



        }

        public static void bitmapSave(Bitmap outBitmap, string path)
        {
            //判断文件的存在
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();//创建该文件
            }
            outBitmap.Save(path);
        }

        public static void DrawPic(Bitmap sourceBitmap, int widthOffset, int heightOffset, int trimSourceTopHeight, int trimSourceBottomHeight, Bitmap outBitmap)
        {
            for (int i = 0; i <= sourceBitmap.Width - 1; i++)
            {
                for (int j = trimSourceTopHeight; j <= sourceBitmap.Height - 1 - trimSourceTopHeight - trimSourceBottomHeight; j++)
                {
                    Color c;
                    try
                    {
                        c = sourceBitmap.GetPixel(i, j);
                    }
                    catch (Exception exp)
                    {
                        c = Color.White;
                    }
                    outBitmap.SetPixel(i + widthOffset, j + heightOffset, c);
                }
            }
        }
        
    }
}
