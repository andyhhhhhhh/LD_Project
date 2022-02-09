using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.Windows.Forms;
using XMLController;

namespace EngineController.CsEngine
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptAttribute : Attribute
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public string Explains { get; set; }

        ///相关属性 字段  
        public ScriptAttribute()
        {
            //构造方法  
        }

        public ScriptAttribute(string name, string parameter, string explains)
        {
            Name = name;
            Parameter = parameter;
            Explains = explains;

        }
    }

    public class ToolScriptFun
    {
        public delegate void Del_ShowLog(string strLog, bool bError = false);
        public static Del_ShowLog m_DelShowLog; 

        public delegate void Del_ShowObject(int inum, object dispObj);
        public static Del_ShowObject m_DelShowObject;

        [ScriptAttribute("Show", "\"字符串内容,弹窗标题\"", "定义窗体名称弹窗")]
        public static void Show(string s, string title)
        {
            MessageBox.Show(s, title);
        }

        [ScriptAttribute("Show", "\"字符串内容\"", "弹窗显示字符串")]
        public static void Show(string s)
        {
            MessageBox.Show(s);
        }

        [ScriptAttribute("ShowLog", "\"Log内容\"", "显示Log")]
        public static void ShowLog(string strLog, bool bError = false)
        {
            if(m_DelShowLog != null)
            {
                m_DelShowLog(strLog, bError);
            }
        }

        [ScriptAttribute("ShowObject", "窗口编号,输出图像", "图像窗体显示")]
        public static void ShowObject(int inum, object dispObj)
        {
            if (m_DelShowObject != null)
            {
                m_DelShowObject(inum, dispObj);
            }
        }

        [ScriptAttribute("GetValue", "\"字符串内容\"", "获取变量值")]
        public static object GetValue(string str)
        {
            try
            {
                object objValue = XmlControl.GetLinkValue(str);

                return objValue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [ScriptAttribute("GetInt", "\"字符串内容\"", "获取Int值")]
        public static int GetInt(string key)
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

        [ScriptAttribute("GetString", "\"字符串内容\"", "获取String值")]
        public static string GetString(string key)
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

        [ScriptAttribute("GetBoolean", "\"字符串内容\"", "获取Bool值")]
        public static bool GetBoolean(string key)
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

        [ScriptAttribute("GetDouble", "\"字符串内容\"", "获取Double值")]
        public static double GetDouble(string key)
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

        [ScriptAttribute("SetValue", "\"字符串内容\",设置的值", "赋值给变量")]
        public static void SetValue(string key, object value)
        {
            try
            {
                XmlControl.SetLinkValue(key, value);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
