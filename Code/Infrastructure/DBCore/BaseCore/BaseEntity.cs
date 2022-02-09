using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.DBCore
{
    public class BaseEntity
    {
        /// <summary>
        /// 默认主键
        /// </summary>
        [DB(IsPrimainKey = true)]
        public /*virtual*/ Int64 Id { get; set; }
        public /*virtual*/ string Name { get; set; }


        /// <summary>
        /// 树节点是否为新的节点，用于界面上标注该节点是否为插入的新增节点
        /// </summary>
        public bool IsNewTag { get; set; }

        public BaseEntity()
        {
            IsNewTag = false;
        }
    }
}
