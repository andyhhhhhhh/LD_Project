using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ServiceController
{

    class Image
    {
        public Image()
        {
        }

        public Bitmap CreatBitmapImagePara(DictionaryHelper<Font> dic)
        {

            var txt = new RichTextBox();
            /*           int count = 0;
                       foreach (var item in dic.dicList)
                       {
                           txt.Font = dic.GetValue(item.Key);
                           txt.AppendText(item.Key);
                           count++;
                           if (count == 10) { break; }
                       }
            */
            Bitmap image =null;
            int[,] intLoc = new int[,]
            {
                {1, 10,     1, 10,    1,  6,  6,    1, 11,    1, 11},
                {1,  1,    10, 10,   20, 20, 30,   40, 40,   50, 50}
            };
            Console.WriteLine(intLoc);
            int count = 0;
            foreach (var item in dic.dicList)
            {

                txt.Text = item.Key;
                txt.Font = dic.GetValue(item.Key);
                if(image==null) image = new Bitmap(txt.PreferredSize.Width, (txt.PreferredHeight - 5) * txt.Lines.Length + 5);
                var g = Graphics.FromImage(image);
                var b = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                    Color.Black, Color.Black, 1.2f, true);
                g.Clear(System.Drawing.Color.White);
                g.DrawString(item.Key, dic.GetValue(item.Key), b, intLoc[0, 0], intLoc[1, 0]);
                count++;
                if (count == 1) { break; }
            }
            return image;
        }


        public Bitmap CreatImage(string data, Font font)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var txt = new RichTextBox();
            txt.Text = data;
            txt.Font = font;
            //txt.Rtf.Replace(@"\pard", @"\pard\sl700\slmult1");

            var image = new Bitmap(txt.PreferredSize.Width, (txt.PreferredHeight - 5) * txt.Lines.Length + 5);
            var g = Graphics.FromImage(image);
            var b = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.Black, Color.Black, 1.2f, true);
            g.Clear(System.Drawing.Color.White);
            g.DrawString(data, font, b, 1, 1);
            return image;
        }

        public string ConvertImageToCode(Bitmap img)
        {
            var sb = new StringBuilder();
            long clr = 0, n = 0;
            int b = 0;
            for (int i = 0; i < img.Size.Height; i++)
            {
                for (int j = 0; j < img.Size.Width; j++)
                {
                    b = b * 2;
                    clr = img.GetPixel(j, i).ToArgb();
                    string s = clr.ToString("X");

                    if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)
                    {
                        b++;
                    }
                    n++;
                    if (j == (img.Size.Width - 1))
                    {
                        if (n < 8)
                        {
                            b = b * (2 ^ (8 - (int) n));

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
                sb.Append(System.Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
