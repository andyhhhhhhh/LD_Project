using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBCore
{
    public class DBAttribute : Attribute
    {
        public DBAttribute()
        {
            IsSerilize = true;
            IsPrimainKey = false; 
            CharLength = 20;
        }

        /// <summary>
        /// 是否保存到本地（暂时不支持Object，Tuple等复合类型的保存，请在声明实体时赋值为false）
        /// </summary>
        public bool IsSerilize { get; set; }

        public bool IsPrimainKey { get; set; }

        public string IsChildKey { get; set; }

        public int CharLength
        {
            get; set;
        }
    }
}
