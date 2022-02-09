using BaseController.Services;
using LinqKit;
using ProcessModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCollection.Services
{
    public class PlatformSetService : BaseService<PlatformSetModel>, IBaseService<PlatformSetModel>
    {

        public bool Delete(List<PlatformSetModel> entities)
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
                            db.PlatformSets.Attach(entity);
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

        public bool Delete(PlatformSetModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    //手动级联删除
                    if (db.Entry(entity).State == EntityState.Detached)
                    {
                        db.PlatformSets.Attach(entity);
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

        public PlatformSetModel Find(Expression<Func<PlatformSetModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.PlatformSets.AsExpandable().FirstOrDefault(condition);
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

        public PlatformSetModel Find(long id)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.PlatformSets.FirstOrDefault(x => x.Id == id);
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

        public PlatformSetModel FindByName(string likeName)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.PlatformSets.FirstOrDefault(x => x.Name.Contains(likeName));
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

        public List<PlatformSetModel> QueryAll()
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    //db.Configuration.LazyLoadingEnabled = false;
                    //return db.PlatformSets.ToList();

                    List<PlatformSetModel> platformList = new List<PlatformSetModel>();
                    //查询Processes时级联查询
                    platformList = db.PlatformSets 
                        .Include(o => o.CameraModel1).DefaultIfEmpty() 
                        .Include(o => o.CameraModel2).DefaultIfEmpty()
                        .OrderBy(x => x.Id).AsNoTracking().ToList();
                    platformList.Remove(null);
                    return platformList;
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

        public bool SaveOrUpdate(List<PlatformSetModel> entities)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    foreach (var entity in entities)
                    {

                        if (entity.Id != 0)
                        {
                            var serialPort = db.PlatformSets.Find(entity.Id);
                            if (entity.CameraModel1 != null)
                                db.Entry(entity.CameraModel1).State = EntityState.Modified;
                            if (entity.CameraModel2 != null)
                                db.Entry(entity.CameraModel2).State = EntityState.Modified;

                            if (serialPort != null)
                            {
                                // 更新  
                                RemoveHoldingEntityInContext(db, entity);
                                db.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            //新增
                            db.PlatformSets.Add(entity);
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

        public bool SaveOrUpdate(PlatformSetModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    if (entity.Id != 0)
                    {
                        var serialPort = db.PlatformSets.Find(entity.Id);
                        if (serialPort != null)
                        {
                            if (entity.CameraModel1 != null)
                                db.Entry(entity.CameraModel1).State = EntityState.Modified;
                            if (entity.CameraModel2 != null)
                                db.Entry(entity.CameraModel2).State = EntityState.Modified;

                            // 更新  
                            RemoveHoldingEntityInContext(db, entity);
                            db.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        //新增
                        if (entity.CameraModel1 != null)
                        db.Entry(entity.CameraModel1).State = EntityState.Unchanged;
                        if (entity.CameraModel2 != null)
                            db.Entry(entity.CameraModel2).State = EntityState.Unchanged;
                        db.PlatformSets.Add(entity);
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

        public List<PlatformSetModel> Where(Expression<Func<PlatformSetModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.PlatformSets.AsExpandable().Where(condition).ToList();
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
