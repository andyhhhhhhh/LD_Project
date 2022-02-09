using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XMLController;

namespace EngineController
{
    public class CommonFunction2
    {
        public int GetInt(string key)
        {
            try
            {
                object e = XmlControl.GetLinkValue(key);
                return Convert.ToInt32(e.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetString(string key)
        {
            try
            {
                object e = XmlControl.GetLinkValue(key);
                return (string)e;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool GetBoolean(string key)
        {
            try
            {
                object e = XmlControl.GetLinkValue(key);
                return bool.Parse(e.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public double GetDouble(string key)
        {
            try
            {
                object e = XmlControl.GetLinkValue(key);
                return Convert.ToDouble(e.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class RunFunc
    {
        public static object Run(string methodName, object[] paras)
        {
            //反射获取 命名空间+类名
            //传递参数 
            var t = typeof(CommonFunction);
            object obj = Activator.CreateInstance(t);

            try
            {
                //直接调用
                MethodInfo method = t.GetMethod(methodName);
                object o = method.Invoke(obj, paras);
                return o;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class CalcFunc2
    {
        //把GetInt 和GetDouble 替换成对应的值
        public static string GetValue(string strExpress)
        {
            try
            {
                int index = strExpress.IndexOf("GetDouble");
                while (index != -1)
                {
                    //先获取参数
                    string str = strExpress.Substring(index);
                    int iStart = str.IndexOf("(");
                    int iEnd = str.IndexOf(")");
                    string param = str.Substring(iStart + 1, iEnd - iStart - 1);

                    //再置换函数
                    string func = str.Substring(0, iEnd + 1);


                    object[] paras = new object[] { param };
                    object e = RunFunc.Run("GetDouble", paras);

                    strExpress = strExpress.Replace(func, e.ToString());

                    index = strExpress.IndexOf("GetDouble");
                }

                index = strExpress.IndexOf("GetInt");
                while (index != -1)
                {
                    //先获取参数
                    //先获取参数
                    string str = strExpress.Substring(index);
                    int iStart = str.IndexOf("(");
                    int iEnd = str.IndexOf(")");
                    string param = str.Substring(iStart + 1, iEnd - iStart - 1);

                    //再置换函数
                    string func = str.Substring(0, iEnd + 1);

                    object[] paras = new object[] { param };
                    object e = RunFunc.Run("GetInt", paras);

                    strExpress = strExpress.Replace(func, e.ToString());

                    index = strExpress.IndexOf("GetInt");
                }

                return strExpress;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        //根据表达式开始计算
        public static string GetResult(string str)
        {
            str = str + "+";
            char[] expArry = str.ToCharArray();
            string operations = "()+-*/";
            Hashtable signs = new Hashtable();
            string[] numArry = str.Split('(', ')', '+', '-', '*', '/'); //分割字符串，提取表达式中的数字
            //定义操作符的优先级并存入哈希表
            signs.Add("(", 0);
            signs.Add(")", 0);
            signs.Add("+", 1);
            signs.Add("-", 1);
            signs.Add("*", 2);
            signs.Add("/", 2);
            string[] resultArry = new string[99];
            int resultIndex = 0;
            int numIndex = 0;
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (operations.Contains(expArry[i]))
                {
                    //遇到符号直接存入resultArry
                    resultArry[resultIndex] = expArry[i].ToString();
                    resultIndex++;
                }
                else if (operations.Contains(expArry[i + 1]))
                {
                    //连续非符号字符，从numArry取出一个元素放入resultArry
                    while (numArry[numIndex] == "")
                    //去除numArry中的空元素
                    {
                        numIndex++;
                    }
                    resultArry[resultIndex] = numArry[numIndex];
                    numIndex++;
                    resultIndex++;
                }
            }
            Stack<char> sign_stack = new Stack<char>();//定义符号栈
            Stack<string> num_stack = new Stack<string>();//定义操作数栈
            char calc_sign;
            try
            {
                for (int i = 0; i < resultIndex + 1; i++)
                {
                    if (i == resultIndex)
                    {
                        if (sign_stack.Count != 0)
                        {
                            calc_sign = sign_stack.Pop();
                            Calc(ref sign_stack, ref num_stack, ref resultArry[i], ref calc_sign);
                            while (num_stack.Count() > 1)
                            //计算到栈中只剩一个操作数为止
                            {
                                calc_sign = sign_stack.Pop();
                                Calc(ref sign_stack, ref num_stack, ref resultArry[i], ref calc_sign);
                            }
                        }
                    }
                    else if (operations.Contains(resultArry[i]))
                    {
                        if (resultArry[i] != "(")
                        {
                            if (sign_stack.Count() == 0 || Convert.ToInt32(signs[resultArry[i]]) > Convert.ToInt32(signs[sign_stack.Peek().ToString()]))
                            //第一个操作符或者当前操作符优先级大于符号栈栈顶元素时，当前操作符入栈
                            {
                                sign_stack.Push(resultArry[i].ToCharArray()[0]);
                            }
                            else
                            {
                                //否则，符号栈出栈操作符，数字栈出栈两个操作数进行运算
                                calc_sign = sign_stack.Pop();
                                Calc(ref sign_stack, ref num_stack, ref resultArry[i], ref calc_sign);
                            }
                        }
                        else
                        {
                            sign_stack.Push(resultArry[i].ToCharArray()[0]);
                        }
                    }
                    else
                    {
                        num_stack.Push(resultArry[i]);
                    }
                }
                //结果出栈
                return num_stack.Pop();
            }
            catch (Exception e)
            {
                //MessageBox.Show("表达式格式不正确，请检查！ " + e.Message);
                return null;
            }
        }
        public static string CalculateResult(string num1, string num2, char sign)
        {
            double result = 0;
            double oper_num1 = Convert.ToDouble(num1);
            double oper_num2 = Convert.ToDouble(num2);
            if (sign.ToString() == "+")
            {
                result = oper_num1 + oper_num2;
            }
            if (sign.ToString() == "-")
            {
                result = oper_num1 - oper_num2;
            }
            if (sign.ToString() == "*")
            {
                result = oper_num1 * oper_num2;
            }
            if (sign.ToString() == "/")
            {
                if (oper_num2 != 0)
                    result = oper_num1 / oper_num2;
                else
                {

                }
                //MessageBox.Show("表达式中有除0操作，请检查！");
            }
            return result.ToString();
        }
        public static void Calc(ref Stack<char> sign, ref Stack<string> num, ref string str, ref char calc_sign)
        {
            string num1 = "";
            string num2 = "";
            if (str == ")")
            {
                while (calc_sign != '(')
                {
                    num2 = num.Pop();
                    num1 = num.Pop();
                    num.Push(CalculateResult(num1, num2, calc_sign));
                    calc_sign = sign.Pop();
                }
            }
            else
            {
                num2 = num.Pop();
                num1 = num.Pop();
                num.Push(CalculateResult(num1, num2, calc_sign));
                if (str != null && "()+-*/".Contains(str))
                {
                    sign.Push(str.ToCharArray()[0]);
                }
            }

        }
    }

    public class CommonFuncClass
    {
        //单例模式
        private static CommonFuncClass _singleton;

        public static CommonFuncClass Instance
        {
            get
            {
                if (_singleton == null)
                {
                    //多线程处理可以用这个来处理
                    Interlocked.CompareExchange(ref _singleton, new CommonFuncClass(), null);
                }

                return _singleton;
            }
        }

        //利用容器来调用方法
        public void TestDel()
        {
            var handlers = new Dictionary<string, Func<string, int>>();
            handlers["get"] = OnGet;
            int str = handlers["get"]("");
            int str2 = handlers["get"].Invoke("");

            var handlers2 = new Dictionary<string, Action>();
            handlers2["post"] = OnPost;
            handlers2["post"]();
        }

        private int OnGet(string str)
        {
            return 1;
        }

        private void OnPost()
        {

        }

        //利用委托来调用方法
        public string Exec(Func<string> method)
        {
            return method();
        }

        public string Get()
        {
            return "";
        }

        private void TestExec()
        {
            //调用
            Exec(Get);
        }

        //将16进制字符串转换为字符串
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        /// <summary>
        /// 根据双字节发送，需要两个寄存器，超过65535  tempC地址低位:5000  tempC1地址高位:5001
        /// </summary>
        /// <param name="sendContent">发送的数据</param>
        /// <param name="tempC">前面的地址发送数据</param>
        /// <param name="tempC1">后面的地址发送数据</param>
        public static void SetBitTrans(int sendContent, ref int tempC, ref int tempC1)
        {
            tempC = sendContent & 0xffff;
            tempC1 = sendContent >> 16;
        }

        /// <summary>
        /// 获取两个地址的寄存器 超过65535  tempC:低位 5000 tempC1:高位 5001
        /// </summary>
        /// <param name="tempC">前面的地址</param>
        /// <param name="tempC1">后面的地址</param>
        /// <returns></returns>
        public static int GetBitTrans(int tempC, int tempC1)
        {
            tempC = (tempC1 << 16) + tempC;

            return tempC;
        }
    }

    /// <summary>
    /// CRC校验
    /// </summary>
    public class CRC
    {

        #region  CRC16
        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }
        #endregion

        #region  ToCRC16
        public static string ToCRC16(string content)
        {
            return ToCRC16(content, Encoding.UTF8);
        }

        public static string ToCRC16(string content, bool isReverse)
        {
            return ToCRC16(content, Encoding.UTF8, isReverse);
        }

        public static string ToCRC16(string content, Encoding encoding)
        {
            return ByteToString(CRC16(encoding.GetBytes(content)), true);
        }

        public static string ToCRC16(string content, Encoding encoding, bool isReverse)
        {
            return ByteToString(CRC16(encoding.GetBytes(content)), isReverse);
        }

        public static string ToCRC16(byte[] data)
        {
            return ByteToString(CRC16(data), true);
        }

        public static string ToCRC16(byte[] data, bool isReverse)
        {
            return ByteToString(CRC16(data), isReverse);
        }
        #endregion

        #region  ToModbusCRC16
        public static string ToModbusCRC16(string s)
        {
            return ToModbusCRC16(s, true);
        }

        public static string ToModbusCRC16(string s, bool isReverse, bool space = false)
        {
            string cmd = ByteToString(CRC16(StringToHexByte(s)), isReverse);
            if (space)
            {
                //两个字符之间加空格
                cmd = cmd.Insert(0, " ");
                cmd = cmd.Insert(3, " ");
            }
            return cmd;
        }

        public static string ToModbusCRC16(byte[] data)
        {
            return ToModbusCRC16(data, true);
        }

        public static string ToModbusCRC16(byte[] data, bool isReverse)
        {
            return ByteToString(CRC16(data), isReverse);
        }
        #endregion

        #region  ByteToString
        public static string ByteToString(byte[] arr, bool isReverse)
        {
            try
            {
                byte hi = arr[0], lo = arr[1];
                return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0');
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string ByteToString(byte[] arr)
        {
            try
            {
                return ByteToString(arr, true);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  StringToHexString
        public static string StringToHexString(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                s.Append(c.ToString("X4"));
            }
            return s.ToString();
        }
        #endregion

        #region  StringToHexByte
        private static string ConvertChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c <= 0 || c >= 127)
                {
                    s.Append(c.ToString("X4"));
                }
                else
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }

        private static string FilterChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c > 0 && c < 127)
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str)
        {
            return StringToHexByte(str, false);
        }

        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str, bool isFilterChinese)
        {
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

            //清除所有空格
            hex = hex.Replace(" ", "");
            //若字符个数为奇数，补一个0
            hex += hex.Length % 2 != 0 ? "0" : "";

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0, c = result.Length; i < c; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }
        #endregion

    }

    //替换Switch更优雅的写法
    public class MyValueProcessor
    {
        private readonly Dictionary<short, Func<decimal, decimal>> _dic;
        public MyValueProcessor()
        {
            _dic = new Dictionary<short, Func<decimal, decimal>>
            {
                {0, m => m * (decimal)0.5},
                {1, m => m * (decimal)0.6},
                {2, m => m * (decimal)0.7},
                {3, m => m * (decimal)0.8},
                {4, m => m * (decimal)0.9}
            };
        }
        public decimal DaZhe(short policy, decimal orginPrice)
        {
            if (_dic.ContainsKey(policy))
            {
                return _dic[policy].Invoke(orginPrice);
            }
            return orginPrice / 2;
        }
    }

    public class TaskTest
    {
        //使用Task处理
        public async void CameraFolder()
        {
            CancellationTokenSource tokenSource;
            CancellationToken token;
            ManualResetEvent resetEvent = new ManualResetEvent(false);

            Task task = new Task(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    // 初始化为true时执行WaitOne不阻塞
                    resetEvent.WaitOne();
                    //GrabOne();
                    // 模拟等待100ms
                    await Task.Delay(100);
                }

            }, token);
            task.Start();

            await Task.Run(() =>
            {

            });
        }
    }

}
