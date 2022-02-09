using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace EngineController.VBAEngine
{
    public class VbaFunction
    { 
        [VBScriptAttribute("Show", "\"字符串内容\"", "弹窗显示字符串")]
        public static void Show(string s)
        {
            MessageBox.Show(s);
        }

        [VBScriptAttribute("GetValue", "\"字符串内容\"", "获取变量值")]
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

        [VBScriptAttribute("GetInt", "\"字符串内容\"", "获取Int值")]
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

        [VBScriptAttribute("GetString", "\"字符串内容\"", "获取String值")]
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

        [VBScriptAttribute("GetBoolean", "\"字符串内容\"", "获取Bool值")]
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

        [VBScriptAttribute("GetDouble", "\"字符串内容\"", "获取Double值")]
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

        [VBScriptAttribute("SetValue", "\"字符串内容\",设置的值", "赋值给变量")]
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

    [AttributeUsage(AttributeTargets.Method)]
    public class VBScriptAttribute : Attribute
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public string Explains { get; set; }
        ///相关属性 字段  
        public VBScriptAttribute()
        {
            //构造方法  
        }

        public VBScriptAttribute(string name, string parameter, string explains)
        {
            Name = name;
            Parameter = parameter;
            Explains = explains;

        }
    }

}
