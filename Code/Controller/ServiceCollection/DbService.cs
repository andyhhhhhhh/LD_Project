using BaseController.Services; 
using Infrastructure.DBCore;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserSetModel;
using static UserSetModel.UserSetParamModel;

namespace ServiceCollection
{
    public class DbService : DbContext
    {
        public static bool m_IsInit = false;

        #region DbSet
         
        public DbSet<SerialPortModel.SerialPortParamModel> SerialPorts { get; set; }
        public DbSet<SocketModel.SocketParamModel> Sockets { get; set; }
        
        public DbSet<UserSetParamModel> UserSets { get; set; }
        
        public DbSet<CommonSetModel> CommonModels { get; set; }

        #endregion

        public DbService(string databaseName /*= "SqlliteEF6"*/)
            : base(databaseName)
        { 
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                  .Where(type => !String.IsNullOrEmpty(type.Namespace));

            foreach (var type in typesToRegister)
            {
                if (IsEntityTypeConfiguration(type))
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }
            CreatingTableIfNotExist(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private bool IsEntityTypeConfiguration(Type type)
        {
            if (type == null)
            {
                return false;
            }
            else if (type.Name != typeof(EntityTypeConfiguration<>).Name)
            {
                return IsEntityTypeConfiguration(type.BaseType);
            }
            else
            {
                return true;
            }
        }

        private void CreatingTableIfNotExist(DbModelBuilder modelBuilder)
        {
            var model = modelBuilder.Build(Database.Connection);
            ISqlGenerator sqlGenerator = new SqliteSqlGenerator();
            string sqlStr = sqlGenerator.Generate(model.StoreModel);
            using (SQLiteConnection cn = new SQLiteConnection(Database.Connection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    foreach (var item in sqlStr.Split(';'))
                    {
                        string sql = item;
                        if (item.Contains("CREATE TABLE"))
                        {
                            int index = item.IndexOf("CREATE TABLE");
                            sql = item.Insert(index + "CREATE TABLE".Length, " IF NOT EXISTS ");
                        }
                        if (item.Contains("CREATE  INDEX"))
                        {
                            int index = item.IndexOf("CREATE  INDEX");
                            sql = item.Insert(index + "CREATE  INDEX".Length, " IF NOT EXISTS ");
                        }
                        if (!string.IsNullOrEmpty(sql))
                        {
                            //拼sql语句  
                            SQLiteCommand command = new SQLiteCommand(sql, cn);
                            int resultInt = command.ExecuteNonQuery();
                            if (resultInt >= 0)
                            {
                            }
                            else
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
