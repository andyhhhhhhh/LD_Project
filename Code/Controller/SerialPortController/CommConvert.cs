using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortController
{
    public  class CommConvert
    {
        public static string GetHexStr(string strData1)
        {
            byte[] data1 = Encoding.ASCII.GetBytes(strData1); // 将“字符或数字”字符串转换成byte数组

            string strResult1 = "";
            // 将 byte 数组中的每一个元素都转换成 16进制 字符串。
            for (int i = 0; i < data1.Length; i++)
            {
                strResult1 += data1[i].ToString("X2"); 
            } 
            return strResult1;
        }

        public static byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
     
        public static byte[] GetHexBytes(string str)
        {
            string str2 = GetHexString(str);
            byte[] datat = GetByteData(str2);
            return datat;
        }

        private static bool ishex(char x)
        {
            bool re = false;
            if ((x <= '9') && (x >= '0'))
            {
                re = true;
            }
            else if ((x <= 'F') && (x >= 'A'))
            {
                re = true;
            }
            else if ((x <= 'f') && (x >= 'a'))
            {
                re = true;
            }

            return re;
        }
        private static byte[] GetByteData(string s)
        {
            byte[] data = new byte[s.Length / 2];
            for (int i = 0; i < s.Length / 2; i++)
            {
                if (s[i * 2] <= '9')
                {
                    data[i] = (byte)((s[i * 2] - '0') * 16);
                }
                else if (s[i * 2] <= 'f' && s[i * 2] >= 'a')
                {
                    data[i] = (byte)((s[i * 2] - 'a' + 10) * 16);
                }
                else if (s[i * 2] <= 'F' && s[i * 2] >= 'A')
                {
                    data[i] = (byte)((s[i * 2] - 'A' + 10) * 16);
                }

                if (s[i * 2 + 1] <= '9')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - '0')));
                }
                else if (s[i * 2 + 1] <= 'f' && s[i * 2 + 1] >= 'a')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - 'a' + 10)));
                }
                else if (s[i * 2 + 1] <= 'F' && s[i * 2 + 1] >= 'A')
                {
                    data[i] = (byte)(data[i] + (byte)((s[i * 2 + 1] - 'A' + 10)));
                }
            }
            return data;
        }
        private static string GetHexString(string str)
        {
            int len = str.Length;
            string datarev = "";
            int i = 0;
            for (i = 0; i < (len) / 3; i++)
            {
                if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (str[3 * i + 2] == ' '))
                {
                    datarev = datarev + str[3 * i] + str[3 * i + 1];
                }
                else if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (3 * i + 2 == len))
                {
                    datarev = datarev + str[3 * i] + str[3 * i + 1];
                }
            }
            if (len - i * 3 == 2)
            {
                if ((ishex(str[len - 1])) && (ishex(str[len - 2])))
                {
                    datarev = datarev + str[len - 2] + str[len - 1];
                }
            }
            return datarev;
        }
        private static bool ishexstring(string strl)
        {
            string del = "  ";
            string str = strl.Trim(del.ToCharArray());
            int len = str.Length;
            bool re = false;
            for (int i = 0; i < (len) / 3; i++)
            {
                if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (str[3 * i + 2] == ' '))
                {
                    re = true;
                }
                else if ((ishex(str[3 * i])) && (ishex(str[3 * i + 1])) && (3 * i + 2 == len))
                {
                    re = true;
                }
                else
                {
                    re = false;
                }
            }
            return re;
        }
    }
}
