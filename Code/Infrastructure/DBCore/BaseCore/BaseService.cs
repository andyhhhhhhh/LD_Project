using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Infrastructure.DBCore.BaseCore;
using System.Collections; 

namespace Infrastructure.DBCore
{
    public class SQLService<T> where T : BaseEntity, new()
    {
        protected string AssembleCreatTableSQL(T entity)
        {
            try
            {
                //CREATE TABLE COMPANY(ID INT PRIMARY KEY ,NAME TEXT ,ADDRESS CHAR(50),SALARY REAL)
                string tableName = GetClassName(entity);
                StringBuilder sqlStr = new StringBuilder("CREATE table IF NOT EXISTS " + tableName + " (");

                List<DBEntity> dbEntities = GetDBEntityInfo(entity);
                for (int i = 0; i < dbEntities.Count; i++)
                {
                    DBEntity dbEntity = dbEntities[i];
                    string field = GetField(dbEntity);
                    string type = GetDbType(dbEntity.Type);
                    string length = "(" + dbEntity.MaxLength + ")";
                    if (!string.Equals(type, "varchar"))
                    {
                        length = "";
                    }
                    if (dbEntity.IsPrimainKey)
                    {
                        sqlStr.AppendFormat("{0} INTEGER PRIMARY KEY AUTOINCREMENT {1}, ", field, length);
                    }
                    else
                    {
                        sqlStr.AppendFormat("{0} {1}{2}, ", field, type, length);
                    }
                }
                sqlStr = sqlStr.Remove(sqlStr.Length - 2, 2);
                sqlStr.Append(");");
                string sql = sqlStr.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleDropTableSQL(T entity)
        {
            try
            {
                //DROP TABLE database_name.table_name;
                string tableName = GetClassName(entity);
                StringBuilder sqlStr = new StringBuilder("DROP table IF EXISTS " + tableName);
                string sql = sqlStr.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleInsertSQL(T entity)
        {
            try
            {
                //insert into processlist VALUES ((SELECT max(Id) FROM processlist)+1
                string tableName = GetClassName(entity);
                StringBuilder sqlFields = new StringBuilder("INSERT into " + tableName + "(");
                StringBuilder sqlValuesStr = new StringBuilder(" values (");

                List<DBEntity> dbEntities = GetDBEntityInfo(entity);
                for (int i = 0; i < dbEntities.Count; i++)
                {
                    DBEntity dbEntity = dbEntities[i];
                    string field = GetField(dbEntity);
                    StringBuilder sqlSb = GetSqlValue(dbEntity);
                    if (IsIdKey(field))
                    {
                        continue;
                    }
                    if (IsBaseEntity(dbEntity))
                    {
                        string dbValue = GetDbEntityValue(dbEntity);
                        if (!string.IsNullOrEmpty(dbValue) && !dbValue.Equals("0"))
                        {
                            sqlFields.AppendFormat("{0}, ", field);
                            sqlValuesStr.AppendFormat("{0}, ", sqlSb);
                        }
                    }
                    else
                    {
                        sqlFields.AppendFormat("{0}, ", field);
                        sqlValuesStr.AppendFormat("{0}, ", sqlSb);
                    }
                }
                sqlFields.Append("Id)");
                sqlValuesStr.AppendFormat("NULL)", tableName);
                string sql = sqlFields.Append(sqlValuesStr).ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleDeleteSQL(T entity)
        {
            try
            {
                //DROP TABLE database_name.table_name;
                string tableName = GetClassName(entity);
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.AppendFormat("delete from {0} where Id = {1}", tableName, entity.Id);
                string sql = sqlStr.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleDeleteAllSQL(T entity, string predicate)
        {
            try
            {
                //DROP TABLE database_name.table_name;
                string tableName = GetClassName(entity);
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.AppendFormat("delete from {0} ", tableName);
                if (!string.IsNullOrEmpty(predicate))
                {
                    sqlStr.Append(predicate);
                }
                string sql = sqlStr.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleCascadeDeleteSQL(T entity, Type type)
        {
            try
            {
                //DROP TABLE database_name.table_name;
                string tableName = type.Name;
                StringBuilder sqlStr = new StringBuilder();
                List<DBEntity> children;
                BaseEntity childrenEntity;
                GetDbChildrenEntity(entity, type, out children, out childrenEntity);
                sqlStr.AppendFormat("delete from {0} where Id = {1}", tableName, entity.Id);
                string sql = sqlStr.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleSelectSQL(T entity)
        {
            try
            {
                //SELECT column1, column2....columnN FROM  table_name;
                string tableName = GetClassName(entity);
                StringBuilder sqlFields = new StringBuilder("SELECT ");
                List<DBEntity> dbEntities = GetDBEntityInfo(entity);
                for (int i = 0; i < dbEntities.Count; i++)
                {
                    DBEntity dbEntity = dbEntities[i];
                    string field = GetField(dbEntity);
                    if (i + 1 == dbEntities.Count)
                        sqlFields.AppendFormat("{0} ", field);
                    else
                        sqlFields.AppendFormat("{0}, ", field);
                }
                sqlFields.AppendFormat(" FROM {0}", tableName);
                string sql = sqlFields.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleCascadeSelectSQL(Type type)
        {
            try
            {
                //SELECT column1, column2....columnN FROM  table_name;
                string tableName = type.Name;
                StringBuilder sqlFields = new StringBuilder("SELECT ");
                List<DBEntity> dbEntities = GetDBEntityInfo(type);
                for (int i = 0; i < dbEntities.Count; i++)
                {
                    DBEntity dbEntity = dbEntities[i];
                    string field = GetField(dbEntity);
                    if (i + 1 == dbEntities.Count)
                        sqlFields.AppendFormat("{0} ", field);
                    else
                        sqlFields.AppendFormat("{0}, ", field);
                }
                sqlFields.AppendFormat(" FROM {0}", tableName);
                string sql = sqlFields.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleUpdateSQL(T entity)
        {
            try
            {
                //UPDATE table_name SET column1 = value1, column2 = value2...., columnN = valueN WHERE[condition];
                string tableName = GetClassName(entity);
                StringBuilder sqlFields = new StringBuilder("UPDATE " + tableName + " SET ");

                List<DBEntity> dbEntities = GetDBEntityInfo(entity);
                for (int i = 0; i < dbEntities.Count; i++)
                {
                    DBEntity dbEntity = dbEntities[i];
                    string field = GetField(dbEntity);
                    if (field.ToUpper().Equals("ID"))
                    {
                        continue;
                    }
                    StringBuilder value = GetSqlValue(dbEntity);
                    sqlFields.AppendFormat("{0} = {1}, ", field, value);
                }
                sqlFields = sqlFields.Remove(sqlFields.Length - 2, 2);
                sqlFields.AppendFormat(" WHERE Id = {0}", entity.Id);
                string sql = sqlFields.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string AssembleCascadeUpdateSQL(T entity, Type type)
        {
            try
            {
                //UPDATE table_name SET column1 = value1, column2 = value2...., columnN = valueN WHERE[condition];
                string sql = "";
                string tableName = type.Name;
                StringBuilder sqlFields = new StringBuilder("UPDATE " + tableName + " SET ");
                //获得CascadeChild
                List<DBEntity> children;
                BaseEntity childEntity;
                GetDbChildrenEntity(entity, type, out children, out childEntity);
                if (childEntity == null)
                {
                    //如果子对象为空，则不级联保存
                    sql = string.Empty;
                    return sql;
                }

                for (int i = 0; i < children.Count; i++)
                {
                    DBEntity dbEntity = children[i];
                    string field = GetField(dbEntity);
                    if (field.ToUpper().Equals("ID"))
                    {
                        continue;
                    }
                    StringBuilder value = GetSqlValue(dbEntity);
                    sqlFields.AppendFormat("{0} = {1}, ", field, value);
                }
                sqlFields = sqlFields.Remove(sqlFields.Length - 2, 2);
                sqlFields.AppendFormat(" WHERE Id = {0}", childEntity.Id);
                sql = sqlFields.ToString();
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string GetDbType(string type)
        {
            string dbType;
            switch (type)
            {
                case ("DBNull"):
                case ("Char"):
                case ("SByte"):
                case ("UInt16"):
                case ("UInt32"):
                case ("UInt64"):
                case ("Byte[]"):
                    throw new SystemException("不支持的类型转换，无法保存到本地，请在序列化时声明[DB(IsSerilize = false)]，或保存为其他类型");

                case ("Null"):
                    dbType = DBGlobal.NULL;
                    break;

                case ("String"):
                case ("String[]"):
                    dbType = DBGlobal.VARCHAR;
                    break;

                case ("Int32"):
                case ("Int64"):
                case ("BaseEntity"):
                case ("IList`1"):
                case ("List`1"):
                    dbType = DBGlobal.INT;
                    break;

                case ("Boolean"):
                    dbType = DBGlobal.BOOLEAN;
                    break;

                case ("DateTime"):
                    dbType = DBGlobal.DATETIME;
                    break;

                case ("Double"):
                    dbType = DBGlobal.DOUBLE;
                    break;

                case ("Decimal"):
                    dbType = DBGlobal.DECIMAL;
                    break;

                case ("Guid"):
                    dbType = DBGlobal.Text;
                    break;
                default:
                    throw new SystemException("不支持的类型转换，无法保存到本地，请在序列化时声明[DB(IsSerilize = false)]，或保存为其他类型");

            }
            return dbType;
        }

        protected int GenPropertyLength(PropertyInfo property)
        {
            IEnumerable<Attribute> atts = property.GetCustomAttributes();
            if (atts.Count() > 0)
            {
                DBAttribute att = atts.ToList()[0] as DBAttribute;
                if (att.CharLength > 0)
                {
                    return att.CharLength;
                }
            }
            return 0;
        }

        protected string GetPropertyBaseType(PropertyInfo property)
        {
            if (IsEnumProperty(property))
            {
                return DBGlobal.TypeString.Name;
            }
            else if (IsBaseEntityProperty(property))
            {
                return DBGlobal.TypeBaseEntity.Name;
            }
            else
            {
                return property.PropertyType.Name;
            }
        }

        protected Type GetPropertyType(PropertyInfo property)
        {
            return property.PropertyType;
        }

        protected bool GenPrimainKey(PropertyInfo property)
        {
            IEnumerable<Attribute> atts = property.GetCustomAttributes();
            if (atts.Count() > 0)
            {
                DBAttribute att = atts.ToList()[0] as DBAttribute;
                if (att.IsPrimainKey)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        protected bool IsSerilizeInfo(PropertyInfo property)
        {
            IEnumerable<Attribute> atts = property.GetCustomAttributes();
            if (atts.Count() > 0)
            {
                DBAttribute att = atts.ToList()[0] as DBAttribute;
                if (att != null && !att.IsSerilize)
                {
                    return false;
                }
            }
            return true;
        }

        protected bool IsEnumProperty(PropertyInfo property)
        {
            if (property.PropertyType.BaseType == DBGlobal.TypeEnum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool IsListType(Type type)
        {
            return DBGlobal.TypeObjectList.Equals(type);
        }

        protected bool IsChildKey(PropertyInfo property, T entity)
        {
            IEnumerable<Attribute> atts = property.GetCustomAttributes();
            if (atts.Count() > 0)
            {

                DBAttribute att = atts.ToList()[0] as DBAttribute;
                if (att != null)
                {
                    if (entity.GetType().Name != att.IsChildKey)
                        return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        protected string GetField(PropertyInfo property)
        {
            string field;
            if (IsBaseEntityProperty(property))
            {
                field = property.Name + "_Id";
            }
            else
            {
                string name = property.Name;
                //数据库关键字冲突
                switch (name.ToLower())
                {
                    case "select":
                    case "set":
                    case "delete":
                    case "from":
                    case "where":
                    case "on":
                    case "index":
                        name += "_alias";
                        break;
                }
                field = name;
            }

            return field;
        }

        protected bool IsBaseEntityProperty(PropertyInfo property)
        {
            if (IsBaseEntity(property.PropertyType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool IsBaseEntityListProperty(PropertyInfo property)
        {
            if (IsBaseEntityList(property.PropertyType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool IsStringArrayProperty(PropertyInfo property)
        {
            return property.PropertyType == DBGlobal.TypeStringArray;
        }

        protected bool IsBooleanProperty(PropertyInfo property)
        {
            return property.PropertyType == DBGlobal.TypeBoolean;
        }

        protected Type GetBaseEntityProperty(PropertyInfo property)
        {
            if (property.PropertyType.BaseType.BaseType == DBGlobal.TypeBaseEntity)
            {
                return property.PropertyType.BaseType.BaseType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前类名，用于生成表名称或者文件名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected string GetClassName(BaseEntity entity)
        {
            try
            {
                Type type = entity.GetType();
                return type.Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string GetClassFullName(BaseEntity entity)
        {
            try
            {
                Type type = entity.GetType();
                return type.AssemblyQualifiedName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取实体内属性字符串，用于自动生成字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<DBEntity> GetDBEntityInfo(BaseEntity entity)
        {
            try
            {
                List<DBEntity> propertiesInfo = new List<DBEntity>();
                Type type = entity.GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DBEntity dbEntity = new DBEntity();
                    if (!IsSerilizeInfo(property))
                    {
                        continue;
                    }
                    if (IsBaseEntityListProperty(property)) 
                    {
                        continue;
                    } 
                    else if (IsBaseEntityProperty(property))

                    {
                        continue;
                    }


                    //此处如果和关键字冲突需要重命名
                    dbEntity.Name = GetField(property); 
                    dbEntity.IsPrimainKey = GenPrimainKey(property);
                    dbEntity.MaxLength = GenPropertyLength(property);
                    dbEntity.Type = GetPropertyBaseType(property);
                    object obj = property.GetValue(entity);

                    if (obj != null)
                    {
                        if (IsStringArrayProperty(property))
                            dbEntity.Value = GetStringArrayValue(obj);
                        else if (IsBooleanProperty(property))
                            dbEntity.Value = obj.ToString().ToLower().Equals("true") ? "1" : "0";
                        else
                            dbEntity.Value = obj.ToString();
                        string value = dbEntity.Value;
                        if (!string.IsNullOrEmpty(value) && value.Contains("\"") || value.Contains("\'"))
                            throw new InvalidOperationException("SQL注入错误，包含\"或\'的值");
                    }
                    else
                    {
                        dbEntity.Value = null;
                    }
                    propertiesInfo.Add(dbEntity);
                }
                return propertiesInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DBEntity> GetDBEntityInfo(Type type)
        {
            try
            {
                object instance = type.Assembly.CreateInstance(type.FullName);
                List<DBEntity> propertiesInfo = new List<DBEntity>();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DBEntity dbEntity = new DBEntity();
                    if (!IsSerilizeInfo(property))
                    {
                        continue;
                    }
                    dbEntity.Name = GetField(property);
                    dbEntity.IsPrimainKey = GenPrimainKey(property);
                    dbEntity.MaxLength = GenPropertyLength(property);
                    dbEntity.Type = GetPropertyBaseType(property);
                    object obj = property.GetValue(instance);
                    if (obj != null)
                        dbEntity.Value = obj.ToString();
                    else
                        dbEntity.Value = null;
                    propertiesInfo.Add(dbEntity);
                }
                return propertiesInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsIdKey(string field)
        {
            return field.ToUpper().Equals("ID");
        }

        private string GetField(DBEntity dbEntity)
        {
            string field;
            if (dbEntity.Type == DBGlobal.BASEENTITY)
            {
                field = dbEntity.Name + "_Id";
            }
            else if (dbEntity.Type == DBGlobal.BASEENTITYLIST || dbEntity.Type == DBGlobal.BASEENTITYILIST)
            {
                field = dbEntity.Name + "_Id";
            }
            else
            {
                field = dbEntity.Name;
            }

            return field;
        }
        private bool IsBaseEntityList(DBEntity dbEntity)
        {
            return dbEntity.Type.ToUpper() == DBGlobal.BASEENTITYLIST;
        }
        private StringBuilder GetSqlValue(DBEntity dbEntity)
        {
            string type = GetDbType(dbEntity.Type);
            string value;
            value = GetDbEntityValue(dbEntity);
            StringBuilder sqlValuesStr = new StringBuilder();
            switch (type.ToUpper())
            {
                case DBGlobal.STRING:
                case DBGlobal.DECIMAL:
                case DBGlobal.DOUBLE:
                case DBGlobal.INT:
                case DBGlobal.BOOLEAN:
                    sqlValuesStr.AppendFormat("{0} ", value);
                    break;
                case DBGlobal.VARCHAR:
                case DBGlobal.Text:
                case DBGlobal.DATETIME:
                    sqlValuesStr.AppendFormat("'{0}' ", value);
                    break;
            }
            return sqlValuesStr;
        }
        private void GetDbChildrenEntity(T entity, Type type, out List<DBEntity> children, out BaseEntity childEntity)
        {
            try
            {
                children = new List<DBEntity>();
                childEntity = null;
                List<DBEntity> dbParent = GetDBEntityInfo(entity);
                foreach (var item in dbParent)
                {
                    if (IsBaseEntity(item))
                    {
                        if (item.Value != null)
                        {
                            if (item.Value.Equals(type.FullName))
                            {
                                childEntity = item.Child;
                                if (childEntity != null)
                                {
                                    children.AddRange(GetDBEntityInfo(childEntity));
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetDbEntityValue(DBEntity dbEntity)
        {
            string value;
            if (!IsBaseEntity(dbEntity))
            {
                value = dbEntity.Value;
            }
            else
            {
                if (dbEntity.Child != null)
                {
                    value = dbEntity.Child.Id.ToString();
                }
                else
                {
                    //更新Child_id时，不存在则为0
                    value = "0";
                }
            }

            return value;
        }
        private string GetStringArrayValue(object obj)
        {
            string strResult = "";
            string[] strArray = new string[] { };
            if (obj != null && (strArray = (obj as string[])) != null)
            {
                foreach (var str in strArray)
                {
                    strResult += str + ",";
                }
            }
            return strResult;
        }
        private bool IsBaseEntity(DBEntity dbEntity)
        {
            return dbEntity.Type.ToUpper() == DBGlobal.BASEENTITY;
        }
        private bool IsBaseEntity(Type type)
        {
            if (type == DBGlobal.TypeBaseEntity)
            {
                return true;
            }
            if (type.BaseType == null)
            {
                return false;
            }
            else
            {
                return IsBaseEntity(type.BaseType);
            }
        }
        private bool IsBaseEntityList(Type type)
        {
            if (type.Name == DBGlobal.TypeBaseEntityList.Name || type.Name == DBGlobal.TypeBaseEntityIList.Name)
            {
                return true;
            }
            if (type.BaseType == null)
            {
                return false;
            }
            else
            {
                return IsBaseEntityList(type.BaseType);
            }
        }
    }
}
