using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UserSetController
{
    public class MultiLanguage
    {
        private static List<Control> m_listControl = new List<Control>();
        private static List<ToolStripMenuItem> m_listTool = new List<ToolStripMenuItem>();

        private static string GetLangString(string Key, string langtype, string FilePath)
        {
            try
            {
                string filename;
                switch (langtype)
                {
                    case "cn": filename = "UserSetController.zh-cn.resources"; break;
                    case "en": filename = "UserSetController.en-us.resources"; break;
                    default: filename = "UserSetController.zh-cn.resources"; break;
                }

                System.Resources.ResourceReader reader = new System.Resources.ResourceReader(FilePath + filename);

                string resourcetype;
                byte[] resourcedata;
                string result = string.Empty;

                try
                {
                    reader.GetResourceData(Key, out resourcetype, out resourcedata);
                    //去掉第一个字节，无用
                    byte[] arr = new byte[resourcedata.Length - 1];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = resourcedata[i + 1];
                    }
                    result = System.Text.Encoding.UTF8.GetString(arr);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                finally
                {
                    reader.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static void LoadAllControl(Control.ControlCollection ctls)
        {
            foreach (Control con in ctls)
            {
                if (con.Controls.Count > 0)
                {
                    m_listControl.Add(con);
                    LoadAllControl(con.Controls);
                }
                m_listControl.Add(con);
            }
        }

        private static void LoadAllToolItem(Control.ControlCollection ctls)
        {
            foreach (Control con in ctls)
            {
                if (con is MenuStrip)
                {
                    MenuStrip menu = con as MenuStrip; ;
                    foreach (ToolStripMenuItem con2 in menu.Items)
                    {
                        m_listTool.Add(con2);
                        foreach (ToolStripMenuItem item in con2.DropDownItems)
                        {
                            m_listTool.Add(item);
                        }
                    }
                }
            }
        } 

        public static void SetLanguage(Control.ControlCollection ctls, string lang)
        {
            m_listControl.Clear();
            m_listTool.Clear();
            LoadAllControl(ctls);
            LoadAllToolItem(ctls);


            foreach (var item in m_listControl)
            {
                string str = MultiLanguage.GetLangString(item.Name, lang, "./");
                if (str != "")
                {
                    item.Text = str;
                }
            }

            foreach (var item in m_listTool)
            {
                string str = MultiLanguage.GetLangString(item.Name, lang, "./");
                if (str != "")
                {
                    item.Text = str;
                }
            }
        }

    }

}
