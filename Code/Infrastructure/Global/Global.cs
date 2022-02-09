using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GlobalCore
{
    public class Global
    {
        /// <summary>
        /// 执行是否中断
        /// </summary>
        public static bool Break = false;
        /// <summary>
        /// 暂停状态
        /// </summary>
        public static bool Pause = false;
        /// <summary>
        /// 运行状态
        /// </summary>
        public static bool Run = false;
        /// <summary>
        /// 停止状态
        /// </summary>
        public static bool Stop = false;

        /// <summary>
        /// 程序启动状态
        /// </summary>
        public static bool Frame_Start = true;
        
        /// <summary>
        /// 产品参数
        /// </summary>
        public static string ProductInfo = "";
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string Product = "localhost";

        /// <summary>
        /// 是否界面设计模式
        /// </summary>
        public static bool IsDesginMode = false;

        /// <summary>
        /// 操作员
        /// </summary>
        public static string OperatorName = "operator";
        /// <summary>
        /// 工程师
        /// </summary>
        public static string EngineerName = "engineer";
        /// <summary>
        /// 调试员
        /// </summary>
        public static string DebugerName = "debugger";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName = "engineer";

        /// <summary>
        /// 当前Log的缓冲区
        /// </summary>
        public static string LogBuffer = "";
        
        /// <summary>
        /// 保存的图片路径
        /// </summary>
        public static string SaveImagePath = "D://CameraImage";          

        /// <summary>
        /// 是否启用小键盘
        /// </summary>
        public static bool EnableOsk = false;

        /// <summary>
        /// 是否启用运行编辑界面
        /// </summary>
        public static bool EnableRunView = false;

        /// <summary>
        /// 流程中的项是否隐藏 如执行片段 循环开始下的测试项
        /// </summary>
        public static bool IsProVisible = true;

        /// <summary>
        /// 2D相机是否实时显示
        /// </summary>
        public static bool IsRealDisplay = false;

        /// <summary>
        /// 是否全屏显示
        /// </summary>
        public static bool IsFullScreen = false;

        /// <summary>
        /// 检测急停
        /// </summary>
        public static bool IsEmergency = false;

        /// <summary>
        /// 配置显示的项目
        /// </summary>
        public static string ConfigPath = CurrentPath + "//Config//configitem.dat";

        public static string CurrentPath = Application.StartupPath;

        /// <summary>
        /// 保存数据路径
        /// </summary>
        public static string DataPath = "D://CSVData//";

        /// <summary>
        /// Sequence所在路径
        /// </summary>
        public static string SequencePath = "";

        /// <summary>
        /// 软件使用天数
        /// </summary>
        public static int UseTime = 10;//软件使用天数

        /// <summary>
        /// Sequence对应的配置文件路径
        /// </summary>
        public static string Model3DPath = @"D:\Documents\Dr3DVision\Model\";

        /// <summary>
        /// 默认的配置文件路径
        /// </summary>
        public static string ModelPath = @"D:\Documents\Dr3DVision\Model\";         

        /// <summary>
        /// 镭射的XY方向分辨率
        /// </summary>
        public static double XYResolution = 1.0;

        /// <summary>
        /// 镭射的Z方向分辨率
        /// </summary>
        public static double ZResolution = 1.0;

        public static double ScaleDivider = 4;

        /// <summary>
        /// 执行删除动作时需要删除的文件
        /// </summary>
        public static List<string> DeleteFiles = new List<string>();

        /// <summary>
        /// 回零标志位
        /// </summary>
        public static bool IsAllgohome = false;

        /// <summary>
        /// 是否启动手动保护
        /// </summary>
        public static bool IsHandProtect = true;

        static Global()
        {
            //CreateNewProduct("abc");
            //SaveProductValue("abc");
        }

        public static bool Init()
        {
            int m_Index;
            string path = CurrentPath + @"/config.xml";
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("路径" + path + "不存在");
            }
            xmlDoc.Load(path);

            #region 选择产品

            XmlNode readProductNode = xmlDoc.ChildNodes[1].SelectSingleNode("productValue");
            if (readProductNode == null)
            {
                throw new FieldAccessException("readProductNode不存在");
            }
            else
            {
                Product = Convert.ToString(readProductNode.InnerText);
            } 

            #endregion

            #region 读取图片
            XmlNode readImageNode = xmlDoc.ChildNodes[1].SelectSingleNode("saveImage");
            if (readImageNode == null)
            {
                throw new FieldAccessException("saveImage不存在");
            }
            else
            {
                m_Index = Convert.ToInt32(readImageNode.InnerText);
            }
            switch (m_Index)
            {
                case 1:
                    SaveImageStauts = EnumSaveImage.NotSave;
                    break;
                case 2:
                    SaveImageStauts = EnumSaveImage.SaveNG;
                    break;
                case 3:
                    SaveImageStauts = EnumSaveImage.SaveAll;
                    break;
                default:
                    SaveImageStauts = EnumSaveImage.SaveAll;
                    break;
            }
            #endregion 

            #region 读取运行速度
            XmlNode speedValueNode = xmlDoc.ChildNodes[1].SelectSingleNode("speedValue");
            if (speedValueNode == null)
            {
                throw new FieldAccessException("saveImage不存在");
            }
            else
            {
                MoveSpeedValue = Convert.ToDouble(speedValueNode.InnerText);
            }
            #endregion

         
            return true;
        }

        public static double MoveSpeedValue { get; set; }

        #region 配置文件相关

        public static bool SaveEmptyRunStatus(bool isEmptyRun)
        {
            try
            {
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode readEmptyRunNode = xmlDoc.ChildNodes[1].SelectSingleNode("isEmptyRun");
                if (readEmptyRunNode == null)
                {
                    throw new FieldAccessException("isEmptyRun不存在");
                }
                else
                {
                    if (IsEmptyRun)
                        readEmptyRunNode.InnerText = "0";
                    else
                        readEmptyRunNode.InnerText = "1";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
          
        public static void SaveImageStatus(EnumSaveImage saveType)
        {
            try
            {
                int index = 0;
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode readNode = xmlDoc.ChildNodes[1].SelectSingleNode("saveImage");
                if (readNode == null)
                {
                    throw new FieldAccessException("saveImage不存在");
                }
                else
                {
                    switch (saveType)
                    {
                        case Global.EnumSaveImage.SaveAll:
                            index = 3;
                            break;
                        case Global.EnumSaveImage.SaveNG:
                            index = 2;
                            break;
                        case Global.EnumSaveImage.NotSave:
                            index = 1;
                            break;
                    }
                }
                readNode.InnerText = index.ToString();
                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreateNewProduct(string productName)
        {
            try
            {
                string path = CurrentPath + @"/MainView.exe.config";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode productNode = xmlDoc.ChildNodes[1].SelectSingleNode("connectionStrings");
                if (productNode == null)
                {
                    throw new FieldAccessException("speedValue不存在");
                }
                else
                {
                    XmlElement xmlElement = xmlDoc.CreateElement("add");
                    xmlElement.SetAttribute("name", productName);
                    xmlElement.SetAttribute("connectionString", "data source=Data\\" + productName + ".db");
                    xmlElement.SetAttribute("providerName", "System.Data.SQLite");
                    productNode.AppendChild(xmlElement);
                    xmlDoc.Save(path);

                    //再复制一个种子数据库过去
                    string oldPath = CurrentPath + @"/Data/localhost.db";
                    string newPath = CurrentPath + @"/Data/" + productName + ".db";
                    File.Copy(oldPath, newPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool DelProductValue(string productName)
        {
            {
                try
                {
                    string path = CurrentPath + @"/config.xml";
                    XmlDocument xmlDoc = new XmlDocument();
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("路径" + path + "不存在");
                    }
                    xmlDoc.Load(path);

                    XmlNode productValue = xmlDoc.ChildNodes[1].SelectSingleNode("productValue");
                    if (productValue == null)
                    {
                        throw new FieldAccessException("productValue不存在");
                    }
                    else
                    {
                        xmlDoc.ChildNodes[1].RemoveChild(productValue);
                        xmlDoc.Save(path);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
        }
        public static bool SaveProductValue(string productName)
        {
            try
            {
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode productValue = xmlDoc.ChildNodes[1].SelectSingleNode("productValue");
                if (productValue == null)
                {
                    throw new FieldAccessException("productValue不存在");
                }
                else
                {
                    productValue.InnerText = productName.ToString();
                    xmlDoc.Save(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool SaveColorValue(string productName, string value)
        {
            try
            {
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode productValue = xmlDoc.ChildNodes[1].SelectSingleNode("Color");
                if (productValue == null)
                {
                    //创建一个新的元素. 
                    XmlNode elem = xmlDoc.CreateElement("Color"); 
                    elem.InnerText = value;
                    xmlDoc.ChildNodes[1].AppendChild(elem);
                    xmlDoc.Save(path);
                }
                else
                {
                    productValue.InnerText = value.ToString();
                    xmlDoc.Save(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static string GetColorValue(string nodename)
        {
            try
            {
                string iValue = "";
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode readNode = xmlDoc.ChildNodes[1].SelectSingleNode(nodename);
                if (readNode == null)
                {
                    return "";
                }
                else
                {
                    iValue = readNode.InnerText;
                }

                return iValue;
            }
            catch (Exception ex)
            {
                return "";
            }
          
        }
        public static List<string> GetProductValues()
        {
            List<string> names = new List<string>();
            try
            {
                string path = CurrentPath + @"/MainView.exe.config";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode productNode = xmlDoc.ChildNodes[1].SelectSingleNode("connectionStrings");
                if (productNode == null)
                {
                    throw new FieldAccessException("speedValue不存在");
                }
                else
                {
                    foreach (XmlNode item in productNode.ChildNodes)
                    {
                        names.Add(item.Attributes["name"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return names;
        }

        public static bool SaveNodeValue(string nodename, string value)
        {
            try
            {
                string path = CurrentPath + @"/config.xml";
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("路径" + path + "不存在");
                }
                xmlDoc.Load(path);

                XmlNode nodeValue = xmlDoc.ChildNodes[1].SelectSingleNode(nodename);
                if (nodeValue == null)
                {
                    throw new FieldAccessException(string.Format("{0}不存在", nodename));
                }
                else
                {
                    nodeValue.InnerText = value.ToString();
                    xmlDoc.Save(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public static string GetStrNodeValue(string node)
        {
            string sValue = "";
            string path = CurrentPath + @"/config.xml";
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("路径" + path + "不存在");
            }
            xmlDoc.Load(path);

            XmlNode readNode = xmlDoc.ChildNodes[1].SelectSingleNode(node);
            if (readNode == null)
            {
                throw new FieldAccessException(string.Format("{0}不存在", node));
            }
            else
            {
                sValue = readNode.InnerText;
            }

            return sValue;
        }

        public static string GetNodeValue(string nodename)
        {
            string iValue = "";
            string path = CurrentPath + @"/config.xml";
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("路径" + path + "不存在");
            }
            xmlDoc.Load(path);

            XmlNode readNode = xmlDoc.ChildNodes[1].SelectSingleNode(nodename);
            if (readNode == null)
            {
                throw new FieldAccessException(string.Format("{0}不存在", nodename));
            }
            else
            {
                iValue = readNode.InnerText;
            }

            return iValue;
        }

        #endregion 

        /// <summary>
        /// 根据编号设置系统状态(运行，暂停，停止)
        /// </summary>
        /// <param name="statusIndex"></param>
        public static void SetSystemStauts(EnumSystemRunStatus statusIndex)
        {
            if (statusIndex == EnumSystemRunStatus.Run)
            {
                Global.Run = true;
                Global.Stop = false;
                Global.Pause = false;
            }
            else if (statusIndex == EnumSystemRunStatus.Stop)
            {
                Global.Run = false;
                Global.Stop = true;
                Global.Pause = false;
            }
            else if (statusIndex == EnumSystemRunStatus.Pause)
            {
                //Global.Run = false;
                //Global.Stop = false;
                Global.Pause = true;
            }
        }
         
        /// <summary>
        /// 空跑
        /// </summary>
        public static bool IsEmptyRun { get; set; } 
         
        public static List<T> CopyList<T>(List<T> list)
        {
            List<T> tList = new List<T>();
            foreach (var item in list)
            {
                tList.Add(item);
            }
            return tList;
        }

        public static List<string> ReadConfig(string path)
        {
            try
            {
                //先读取是否存在
                List<string> list = new List<string>();
                if (File.Exists(path))
                {
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader re = new StreamReader(fs, Encoding.UTF8);
                    re.BaseStream.Seek(0, SeekOrigin.Begin);
                    string tmp = "";
                    while (tmp != null)
                    {
                        tmp = re.ReadLine();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            list.Add(tmp);
                        }
                    }

                    re.Close();
                    fs.Close();
                }

                return list;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public static EnumSaveImage SaveImageStauts { get; set; }

        /// <summary>
        /// 保存图片枚举
        /// </summary>
        public enum EnumSaveImage
        {
            SaveAll,
            SaveNG,
            NotSave
        }

        /// <summary>
        /// 系统运行状态枚举
        /// </summary>
        public enum EnumSystemRunStatus
        {
            Stop,
            Run,
            Pause
        }

        /// <summary>
        /// 执行按钮枚举
        /// </summary>
        public enum EnumEButtonRun
        {
            运行,
            暂停,
            停止,
            恢复,
            编辑,
            产品配置,
            SAP信息,
            相机实时,
        }

        /// <summary>
        /// 执行流程枚举
        /// </summary>
        public enum EnumEProcess
        {
            流程1,
            流程2,
            流程3,
            流程4,
            流程5,
            流程6,
            流程7,
            流程8,
            流程9,
            流程10,
            流程11,
            流程12,
            流程13,
            流程14,
            流程15,
            流程16,
            流程17,
            流程18,
            流程19,
            流程20, 
            流程21,
            流程22,
            流程23,
            流程24,
            流程25,
            流程26,
            流程27,
            流程28,
            流程29,
            流程30,
        }

    }
}
