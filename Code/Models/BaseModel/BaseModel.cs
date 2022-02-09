using Infrastructure.DBCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModels
{
    public abstract class BaseModel<T> : BaseEntity where T : BaseEntity
    {

        /// <summary>
        /// 比较两个对象的数据是否一致（只对比数据库映射属性，不对比主键）
        /// </summary>
        /// <param name="compare"></param> 
        /// <returns></returns>
        //public abstract bool IsEuqalEntity(T compare);
        public abstract T Clone();
    }
}
