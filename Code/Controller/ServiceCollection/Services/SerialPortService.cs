using BaseController.Services;
using SerialPortModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ServiceCollection.Services;
using System.Data.Entity;
using System.Data.Entity.Validation;
using LinqKit;

namespace ServiceCollection
{
    public class SerialPortService : BaseService<SerialPortParamModel>, IBaseService<SerialPortParamModel>
    {
        public bool Delete(List<SerialPortParamModel> entities)
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
                            db.SerialPorts.Attach(entity);
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

        public bool Delete(SerialPortParamModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    //手动级联删除
                    if (db.Entry(entity).State == EntityState.Detached)
                    {
                        db.SerialPorts.Attach(entity);
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

        public SerialPortParamModel Find(Expression<Func<SerialPortParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.SerialPorts.AsExpandable().FirstOrDefault(condition);
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

        public SerialPortParamModel Find(long id)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.SerialPorts.FirstOrDefault(x => x.Id == id);
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

        public SerialPortParamModel FindByName(string likeName)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.SerialPorts.FirstOrDefault(x => x.Name.Contains(likeName));
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

        public List<SerialPortParamModel> QueryAll()
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.SerialPorts.ToList();
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

        public bool SaveOrUpdate(List<SerialPortParamModel> entities)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    foreach (var entity in entities)
                    {

                        if (entity.Id != 0)
                        {
                            var serialPort = db.SerialPorts.Find(entity.Id);
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
                            db.SerialPorts.Add(entity);
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

        public bool SaveOrUpdate(SerialPortParamModel entity)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    if (entity.Id != 0)
                    {
                        var serialPort = db.SerialPorts.Find(entity.Id);
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
                        db.SerialPorts.Add(entity);
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

        public List<SerialPortParamModel> Where(Expression<Func<SerialPortParamModel, bool>> condition)
        {
            try
            {
                using (DbService db = new DbService(GlobalCore.Global.Product))
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.SerialPorts.AsExpandable().Where(condition).ToList();
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
