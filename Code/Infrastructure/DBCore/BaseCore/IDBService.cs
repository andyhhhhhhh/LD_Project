using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBCore
{
    public interface IDBService<T>
    {
        bool Create(T entity);
        bool Drop(T entity); 
        /// <summary>
        /// 保存实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Save(T entity);
        bool Save(List<T> entities);
        /// <summary>
        /// 默认删除主键的对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
        bool Delete(string sqlStr);
        bool Delete(List<T> entities); 
        /// <summary>
        /// 默认查询所有对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        List<T> Query();
        bool Update(T entity);
        bool Update(List<T> entities);
        bool SaveOrUpdate(T entity);
        bool SaveOrUpdate(List<T> entities);
    }
}
