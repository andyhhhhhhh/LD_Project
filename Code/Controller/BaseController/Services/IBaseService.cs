using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseController.Services
{
    public interface IBaseService<T>
    {
        //bool Save(T entity);
        //bool Save(List<T> entities);
        /// <summary>
        /// 默认删除主键的对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
        bool Delete(List<T> entities);
        /// <summary>
        /// 默认查询所有对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        List<T> QueryAll();
        List<T> Where(Expression<Func<T, bool>> condition);
        T Find(Int64 id);
        T FindByName(string likeName);
        T Find(Expression<Func<T, bool>> condition);
        //bool Update(T entity);
        //bool Update(List<T> entities);
        /// <summary>
        /// 保存实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveOrUpdate(T entity);
        bool SaveOrUpdate(List<T> entities);
        //bool Create(T entity);
        //bool Drop(T entity);
    }
}
