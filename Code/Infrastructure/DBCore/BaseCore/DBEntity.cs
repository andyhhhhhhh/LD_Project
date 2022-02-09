using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBCore
{
    /// <summary>
    /// DB数据保存映射关系
    /// </summary>
    public class DBEntity : BaseEntity
    { 
        public bool IsPrimainKey { get; set; }
        public string Value { get; set; }

        public BaseEntity Child { get; set; }

        public List<BaseEntity> Children { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
        
        /// <summary>
        /// 字节长度
        /// </summary>
        public int MaxLength { get; set; }

        public Type GetType(string type)
        {
            Type innerType = null;
            switch (type.ToLower())
            {
                case "string":
                    innerType = typeof(String);
                    break;
                case "int":
                    innerType = typeof(Int32);
                    break;
                case "double":
                    innerType = typeof(Double);
                    break;
                case "float":
                    innerType = typeof(float);
                    break;
                case "bool":
                    innerType = typeof(Boolean);
                    break;
            }
            return innerType;
        }
    }
}
