using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XMLController
{
    public class CommonFunction
    {
        public int GetInt(string key)
        {
            try
            {
                object e = XmlControl.GetLinkValue(key); 
                double d = Double.Parse(e.ToString());
                return (int)d;
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

    public class CalcFunc
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

                    strExpress = strExpress.Replace(func, "(" + e.ToString() + ")");

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

                    strExpress = strExpress.Replace(func, "(" + e.ToString() + ")");

                    index = strExpress.IndexOf("GetInt");
                }

                return strExpress;
            }
            catch (Exception ex)
            {
                return "";
            } 
        }
        public static string GetResult(string exp)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Result").Expression = exp;
                dt.Rows.Add(dt.NewRow());

                var result = dt.Rows[0]["Result"];

                //Console.WriteLine("{0} = {1}", exp, result);

                dt.Clear();
                dt.Dispose();
                dt = null;

                return result.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //根据表达式开始计算
        public static string GetResult_pre(string str)
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

    /// <summary>
    /// 串口格式转换
    /// </summary>
    public class CommConvert
    {
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
