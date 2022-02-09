using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceController
{
    /// <summary>
    /// 字典帮组类
    /// 特点：对字典的增删改查很方便
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DictionaryHelper<T>
    {
        public Dictionary<string, T> dicList;

        public DictionaryHelper()
        {
            dicList = new Dictionary<string, T>();
            dicList.Clear();
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        public void AddItem(string key, T t)
        {
            if (!dicList.ContainsKey(key))
            {
                dicList.Add(key, t);
            }
        }

        /// <summary>
        /// 根据键删除元素
        /// </summary>
        public void RemoveByKey(string key)
        {
            if (dicList.ContainsKey(key))
            {
                dicList.Remove(key);
            }
        }

        /// <summary>
        /// 根据值删除元素
        /// </summary>
        public void RemoveByVal(T t)
        {
            if (dicList.ContainsValue(t))
            {
                //Dictionary的元素类型为KeyValuePair
                foreach (KeyValuePair<string, T> entry in dicList)
                {
                    if (entry.Value.Equals(t))
                    {
                        dicList.Remove(entry.Key);
                    }
                }
            }
        }

        /// <summary>
        /// 跟据键修改值
        /// </summary>
        public void SetValue(string key, T t)
        {
            if (dicList.ContainsKey(key))
            {
                dicList[key] = t;
            }
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        public Dictionary<string, T> List
        {
            get
            {
                return dicList;
            }
        }

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        public T GetValue(string key)
        {
            if (!dicList.ContainsKey(key))
            {
                //返回T类型的默认值
                return default(T);
            }

            return dicList[key];
        }

        /// <summary>
        /// 获取值集合
        /// </summary>
        public Dictionary<string, T>.ValueCollection GetValues()
        {
            return dicList.Values;
        }

        /// <summary>
        /// 获取键集合
        /// </summary>
        public Dictionary<string, T>.KeyCollection GetKeys()
        {
            return dicList.Keys;
        }

        /// <summary>
        /// 获取集合的长度
        /// </summary>
        public int Count
        {
            get
            {
                return dicList.Count;
            }
        }

        /// <summary>
        /// 清除集合
        /// </summary>
        public void Clear()
        {
            dicList.Clear();
        }


    }
}
