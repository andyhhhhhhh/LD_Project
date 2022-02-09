 
using ServiceCollection.Services;
using System.Data.Entity;
using System.Data.Entity.Validation;
using LinqKit;
using BaseController.Services;
using ServiceCollection;
using SocketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions; 
namespace ServiceCollection
{
    public class SocketService : BaseService<SocketParamModel>, IBaseService<SocketParamModel>
    {
        public bool Delete(List<SocketParamModel> entities)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    foreach (var entity in entities)
                    {
                        //手动级联删除
                        if (db.Entry(entity).State == EntityState.Detached)
                        {
                            db.Sockets.Attach(entity);
                        }
                        db.Entry(entity).State = EntityState.Deleted;
                    }
                    bool result = db.SaveChanges() >= 0 ? true : false;
                    return result;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(SocketParamModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    //手动级联删除
                    if (db.Entry(entity).State == EntityState.Detached)
                    {
                        db.Sockets.Attach(entity);
                    }
                    db.Entry(entity).State = EntityState.Deleted;
                    bool result = db.SaveChanges() >= 0 ? true : false;
                    return result;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SocketParamModel Find(Expression<Func<SocketParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Sockets.AsExpandable().FirstOrDefault(condition);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SocketParamModel Find(long id)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Sockets.FirstOrDefault(x => x.Id == id);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SocketParamModel FindByName(string likeName)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Sockets.FirstOrDefault(x => x.Name.Contains(likeName));
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocketParamModel> QueryAll()
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Sockets.ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveOrUpdate(List<SocketParamModel> entities)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    foreach (var entity in entities)
                    {

                        if (entity.Id != 0)
                        {
                            var sockets = db.Sockets.Find(entity.Id);
                            if (sockets != null)
                            {
                                // 更新  
                                RemoveHoldingEntityInContext(db, entity);
                                db.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            //新增
                            db.Sockets.Add(entity);
                        }
                        entity.IsNewTag = false;
                    }
                    bool result = db.SaveChanges() >= 0 ? true : false;
                    return result;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveOrUpdate(SocketParamModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    if (entity.Id != 0)
                    {
                        var sockets = db.Sockets.Find(entity.Id);
                        if (sockets != null)
                        {
                            // 更新  
                            RemoveHoldingEntityInContext(db, entity);
                            db.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        //新增
                        db.Sockets.Add(entity);
                    }
                    bool result = db.SaveChanges() >= 0 ? true : false;
                    if (result)
                    {
                        entity.IsNewTag = false;
                    }
                    return result;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocketParamModel> Where(Expression<Func<SocketParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Sockets.AsExpandable().Where(condition).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (DbEntityValidationResult dbValidationResult in dbEx.EntityValidationErrors)
                {
                    foreach (var dbValidationError in dbValidationResult.ValidationErrors)
                    {
                        errorMessage += dbValidationError.ErrorMessage + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
