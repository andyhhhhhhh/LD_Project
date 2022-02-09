using BaseModels;
using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCollection.Services
{

    public class BaseService<T> where T : BaseEntity, new()
    {
        public bool RemoveHoldingEntityInContext<TEntity>(DbService db, TEntity entity) where TEntity : BaseEntity, new()
        {
            var objContext = ((IObjectContextAdapter)db).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        } 
    }
}
