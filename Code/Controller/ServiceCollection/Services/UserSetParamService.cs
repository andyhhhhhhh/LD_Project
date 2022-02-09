using BaseController.Services;
using LinqKit;
using ServiceCollection.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserSetModel;

namespace ServiceCollection
{
    public class UserSetParamService : BaseService<UserSetParamModel>, IBaseService<UserSetParamModel>
    {
        public bool Delete(List<UserSetParamModel> entities)
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
                            db.UserSets.Attach(entity);
                        }
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

        public bool Delete(UserSetParamModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    //手动级联删除
                    if (db.Entry(entity).State == EntityState.Detached)
                    {
                        db.UserSets.Attach(entity);
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

        public UserSetParamModel Find(Expression<Func<UserSetParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.UserSets.AsExpandable().FirstOrDefault(condition);
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

        public UserSetParamModel Find(Int64 id)
        {
            using (DbService db = new DbService(GlobalCore.Global.Product))
            {
                try
                {
                    return db.UserSets.FirstOrDefault(x => x.Id == id);
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

        public UserSetParamModel FindByName(string likeName)
        {
            throw new NotImplementedException();
        }

        public List<UserSetParamModel> QueryAll()
        {
            try
            {
               
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.UserSets.ToList();
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

        public bool SaveOrUpdate(List<UserSetParamModel> entities)
        {
            using (DbService db = new DbService(GlobalCore.Global.Product))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool result = true;
                        foreach (var entity in entities)
                        {
                            if (entity.Id != 0)
                            {
                                var userSet = db.UserSets.Find(entity.Id);
                                if (userSet != null)
                                {
                                    // 更新  
                                    RemoveHoldingEntityInContext(db, entity);
                                    db.Entry(entity).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                // 新增
                                db.UserSets.Add(entity);
                            }
                        }
                        result = db.SaveChanges() >= 0 ? true : false;
                        if (result)
                        {
                            transaction.Commit();
                        }
                        return result;
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
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public bool SaveOrUpdate(UserSetParamModel entity)
        {
            using (DbService db = new DbService(GlobalCore.Global.Product))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool result = true;
                        if (entity.Id != 0)
                        {
                            var userSet = db.UserSets.Find(entity.Id);
                            if (userSet != null)
                            {
                                // 更新  
                                RemoveHoldingEntityInContext(db, entity);
                                db.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            // 新增
                            db.UserSets.Add(entity);
                        }
                        result = db.SaveChanges() >= 0 ? true : false;
                        if (result)
                        {
                            transaction.Commit();
                        }
                        return result;
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
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public List<UserSetParamModel> Where(Expression<Func<UserSetParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.UserSets.AsExpandable().Where(condition).ToList();
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
