using BaseController.Services;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserSetModel;

namespace ServiceCollection.Services
{
    public class CommonSetService : BaseService<CommonSetModel>, IBaseService<CommonSetModel>
    {
        public bool Delete(List<CommonSetModel> entities)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    foreach (var entity in db.CommonModels)
                    {
                        RemoveHoldingEntityInContext(db, entity);
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

        public bool Delete(CommonSetModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    RemoveHoldingEntityInContext(db, entity);
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

        public CommonSetModel Find(Expression<Func<CommonSetModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.CommonModels.AsExpandable().FirstOrDefault(condition);
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

        public CommonSetModel Find(Int64 id)
        {
            using (DbService db = new DbService(GlobalCore.Global.Product))
            {
                try
                {
                    return db.CommonModels.FirstOrDefault(x => x.Id == id);
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

        public CommonSetModel FindByName(string likeName)
        {
            throw new NotImplementedException();
        }

        public List<CommonSetModel> QueryAll()
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    return db.CommonModels.ToList();
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

        public bool SaveOrUpdate(List<CommonSetModel> entities)
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
                                var camera = db.CommonModels.Find(entity.Id);
                                if (camera != null)
                                {
                                    // 更新  
                                    RemoveHoldingEntityInContext(db, entity);
                                    db.Entry(entity).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                // 新增
                                db.CommonModels.Add(entity);
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

        public bool SaveOrUpdate(CommonSetModel entity)
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
                            var camera = db.CommonModels.Find(entity.Id);
                            if (camera != null)
                            {
                                // 更新  
                                RemoveHoldingEntityInContext(db, entity);
                                db.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            // 新增
                            db.CommonModels.Add(entity);
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

        public List<CommonSetModel> Where(Expression<Func<CommonSetModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.CommonModels.AsExpandable().Where(condition).ToList();
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
