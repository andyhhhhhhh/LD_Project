using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmController
{
    public class SerializeHelper
    {
        public static bool TrySerialize(object obj)
        {
            try
            {
                using (Stream objectStream = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(objectStream, obj);
                }
                return true;
            }
            catch (OutOfMemoryException ex)
            {
                throw new Exception("内存不足,请检查设置");
            }
            catch (Exception ex)
            {
                //throw new Exception("对象未完全初始化,请检查设置");
            }
            return false;
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="filePath">序列化保存路径</param>
        /// <param name="obj">序列化对象</param>
        public static void SerializeObjectFile(string filePath, object obj)//序列化
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, obj);
            }

        }

        public static void SerializeObjectMemory(string name, object obj)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(ms, obj);
                //Env.Current.Project.SetData(name, ms);
            }
        }

        public static object DeserializeObjectMemory(string name)
        {
            try
            {
                //using (System.IO.Stream ms = Env.Current.Project[name])
                //{
                //    if (ms == null || ms.Length == 0)
                //        return null;
                //    BinaryFormatter b = new BinaryFormatter();
                //    return b.Deserialize(ms);
                //}

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>反序列化后的对象</returns>
        public static object DeserializeObjectFile(string filePath)
        {
            object obj = new object();
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                {
                    BinaryFormatter b = new BinaryFormatter();
                    obj = b.Deserialize(fileStream);
                }
            }
            catch (Exception ex)
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 深度拷贝对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <returns></returns>
        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }
        /// <summary>
        /// 深度拷贝对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Clone(object obj)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, obj);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream);
            }
        }
    }

    public class SerializeObjectToString
    {
        //将Object类型对象(注：必须是可序列化的对象)转换为二进制序列字符串
        public static string SerializeObject(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();
                //result = Encoding.UTF8.GetString(byt, 0, byt.Length);
                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            return result;
        }
        //将二进制序列字符串转换为Object类型对象
        public static object DeserializeObject(string str)
        {
            IFormatter formatter = new BinaryFormatter();
            //byte[] byt = Encoding.UTF8.GetBytes(str);
            byte[] byt = Convert.FromBase64String(str);
            object obj = null;
            using (Stream stream = new MemoryStream(byt, 0, byt.Length))
            {
                obj = formatter.Deserialize(stream);
            }
            return obj;
        }
    }
}
