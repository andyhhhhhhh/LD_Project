using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternDll
{
    /// <summary>
    /// 程序使用脚本调用的外部方法
    /// </summary> 
    [Category()]
    public class ExternClass
    {
        public void TestFunc()
        {
            UsingPython py = new UsingPython();
            bool bReturn = py.ExcutePython();
            //MessageBox.Show("TestFunc");
        }

        public static void TestStaticFunc()
        {
            MessageBox.Show("TestStaticFunc");
        }

        public static void ShowBox(string msg, string caption)
        {
            MessageBox.Show(msg, caption);
        }

        /// <summary>
        /// 调用Python中的方法
        /// </summary>
        public class UsingPython
        {
            private ScriptRuntime pyRuntime = null;
            private dynamic obj = null;
            public UsingPython()
            {
                string serverpath = AppDomain.CurrentDomain.BaseDirectory + "py//Sum.py";//所引用python路径
                pyRuntime = Python.CreateRuntime();

                ////第一种方法
                //ScriptEngine Engine = pyRuntime.GetEngine("python");
                //ScriptScope pyScope = Engine.CreateScope(); //Python.ImportModule(Engine, "random");
                //obj = Engine.ExecuteFile(serverpath, pyScope); 

                //第二种方法
                obj = pyRuntime.UseFile(serverpath);                 
            }

            public bool ExcutePython()
            {
                try
                {
                    if (null != obj)
                    {
                        int value =  obj.NumFunc();//调用py中的方法
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }


    public class ExternClass2
    { 
        public static void TestClass2()
        {
            MessageBox.Show("TestStaticFunc");
        }

        public static void ShowBox2(string msg, string caption)
        {
            MessageBox.Show(msg, caption);
        }
    }
}
