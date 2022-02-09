using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBCore.BaseCore
{
    internal class DBGlobal
    {
        public const string STRING = "STRING";
        public const string DECIMAL = "DECIMAL";
        public const string DOUBLE = "DOUBLE";
        public const string INT = "INT";
        public const string VARCHAR = "VARCHAR";
        public const string NULL = "NULL";
        public const string Text = "Text";
        public const string BOOLEAN = "BOOLEAN";
        public const string DATETIME = "DATETIME"; 
        /// <summary>
        /// 该属性类型是实体对象
        /// </summary>
        public const string BASEENTITY = "BASEENTITY";
        public static string BASEENTITYLIST = typeof(List<BaseEntity>).Name;
        public static string BASEENTITYILIST = typeof(IList<BaseEntity>).Name;

        #region 设置已存在的类型对象，减少反射次数
        public static Type TypeString = typeof(string);
        public static Type TypeBoolean = typeof(bool);
        public static Type TypeStringArray = typeof(string[]);
        public static Type TypeInt = typeof(int);
        public static Type TypeBaseEntity = typeof(BaseEntity);
        public static Type TypeBaseEntityList = typeof(List<BaseEntity>);
        public static Type TypeBaseEntityIList = typeof(IList<BaseEntity>);
        public static Type TypeEnum = typeof(Enum);
        public static Type TypeObjectList = typeof(List<object>);
       
        #endregion
    }
}
