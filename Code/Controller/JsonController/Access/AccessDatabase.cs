using ADOX;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
//using Excel= Microsoft.Office.Interop.Excel; //ZH200714


namespace JsonController
{
    /// <summary>
    /// 类，用于Access數據庫访问的类。
    /// </summary>
    public class AccessDatabase
    {
        /// <summary>
        /// 保护变量，数据库连接。
        /// </summary>
        protected OleDbConnection Connection;

        /// <summary>
        /// Access操作異常日誌文件名稱
        /// </summary>
        protected string ErrorLogTitle = "Access數據庫操作異常";

        /// <summary>
        /// 保护变量，数据库连接字符串。
        /// </summary>
        protected String ConnectionString;

        /// <summary>
        /// 操作數據庫時候的鎖。
        /// </summary>
        private readonly object _o;


        /// <summary>
        /// 构造函数
        /// </summary>
        public AccessDatabase()
        {
            //ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}\\Database\\AccessDatabase.mdb;Persist Security Info=False;Jet OLEDB:Database Password=yoyo.2008", Application.StartupPath);
            //ConnectionString = SystemConfig.Instance().AccessConnectingString;

            ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\Data\\AccessDatabase.mdb;Persist Security Info=False;", Application.StartupPath);
            _o = new object();
        }

        public void Create()
        {
            //创建数据库文件 
            string path = string.Format("{0}\\Data\\AccessDatabase.mdb", Application.StartupPath);
            //数据库文件不存在则创建
            if (!File.Exists(path))
            {
                ADOX.Catalog catalog = new ADOX.Catalog();
                catalog.Create(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\Data\\AccessDatabase.mdb;", Application.StartupPath));
            }
        }

        public void AddTable()
        {
            OleDbConnection con = new OleDbConnection(ConnectionString);
            con.Open();

            //检查表是否存在
            bool bresult = false;
            DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" }); 
            if (schemaTable != null)
            {
                for (Int32 row = 0; row < schemaTable.Rows.Count; row++)
                {
                    string col_name = schemaTable.Rows[row]["TABLE_NAME"].ToString();
                    if (col_name == "SystemUser")
                    {
                        bresult = true;
                        break;
                    }
                }
            } 


            if (!bresult)
            {
                //创建表
                string strSql = "create table SystemUser(ID int identity(1, 1) not null, USERNAME char(10) not null, PWD char(18) not null, ACCESSLEVEL char(15) not null)";
                OleDbCommand cmd = new OleDbCommand(strSql, con);
                cmd.ExecuteNonQuery();
            }
            
            con.Close();
        }

        /// <summary>
        /// 析构函数，释放非托管资源
        /// </summary>
        ~AccessDatabase()
        {
            try
            {
                if (Connection != null)
                    Connection.Close();
            }
            catch (Exception e)
            {
                //UserPrompt.ShowException("關閉數據庫連接失敗，系統異常信息" + e.Message, ErrorLogTitle);
            }
            try
            {
                Dispose();
            }
            catch (Exception ex)
            {
                //UserPrompt.ShowException("釋放數據庫失敗，系統異常信息" + ex.Message, ErrorLogTitle);
            }
        }

        /// <summary>
        /// 保护方法，打开数据库连接。
        /// </summary>
        protected void Open()
        {
            if (Connection == null)
            {
                try
                {
                    Connection = new OleDbConnection(ConnectionString);
                }
                catch (Exception e)
                {
                    //UserPrompt.ShowException("打開數據庫連接失敗，系統異常信息：" + e.Message, ErrorLogTitle);
                }
            }
            if (Connection != null)
                if (Connection.State.Equals(ConnectionState.Closed))
                {
                    try
                    {
                        Connection.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("打開數據庫失敗，系統異常信息：" + e.Message);
                    }
                }
        }

        /// <summary>
        /// 公有方法，关闭数据库连接。
        /// </summary>
        public void Close()
        {
            try
            {
                if (Connection != null)
                    Connection.Close();
            }
            catch (Exception e)
            {
                //UserPrompt.ShowException("關閉數據庫連接失敗，系統異常信息：" + e.Message, ErrorLogTitle);
            }
        }

        /// <summary>
        /// 公有方法，释放资源。
        /// </summary>
        public void Dispose()
        {
            // 确保连接被关闭
            try
            {
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
            }
            catch (Exception e)
            {
                //UserPrompt.ShowException("釋放數據庫連接失敗，系統異常信息：" + e.Message, ErrorLogTitle);
            }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个SqlDataReader （调用后主意调用OleDbDataReader.Close()）。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>OleDbDataReader</returns>
        public OleDbDataReader GetDataReader(string sqlString)
        {
            lock (_o)
            {
                Open();
                try
                {
                    var cmd = new OleDbCommand(sqlString, Connection);
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("GetDataReader失败，SqlString={0},系统异常信息：{1}", sqlString, e.Message));
                    return null;
                }
            }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataSet。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sqlString)
        {
            lock (_o)
            {
                var dataset = new DataSet();
                Open();
                try
                {
                    var adapter = new OleDbDataAdapter(sqlString, Connection);
                    adapter.Fill(dataset);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("GetDataSet失败，SqlString={0},系统异常信息：{1}", sqlString, e.Message));
                }
                finally
                {
                    Close();
                }
                return dataset;
            }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataSet。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <param name="tableName">表格名稱</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sqlString, string tableName)
        {
            lock (_o)
            {
                var dataset = new DataSet();
                Open();
                try
                {
                    var adapter = new OleDbDataAdapter(sqlString, Connection);
                    adapter.Fill(dataset, tableName);
                }
                catch (Exception e)
                {
                    //UserPrompt.ShowException(
                    //string.Format("GetDataSet失败，SqlString={0},系统异常信息：{1}", sqlString, e.Message), ErrorLogTitle);
                }
                finally
                {
                    Close();
                }
                return dataset;
            }
        }
        /// <summary>
        /// 公有方法，获取数据，返回一个DataTable。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sqlString)
        {
            DataSet dataset = GetDataSet(sqlString);
            dataset.CaseSensitive = false;
            if (dataset.Tables.Count > 0)
            {
                return dataset.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataRow。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>DataRow</returns>
        public DataRow GetDataRow(string sqlString)
        {
            DataSet dataset = GetDataSet(sqlString);
            dataset.CaseSensitive = false;
            if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
            {
                return dataset.Tables[0].Rows[0];
            }
            return null;
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个字符串。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>string</returns>
        public string GetDataString(string sqlString)
        {
            DataSet dataset = GetDataSet(sqlString);
            dataset.CaseSensitive = false;
            if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
            {
                return dataset.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
            }
            return "";
        }

        /// <summary>
        /// 公有方法，执行Sql语句。
        /// </summary>
        /// <param name="sqlString">Sql语句</param>
        /// <returns>对Update、Insert、Delete为影响到的行数，其他情况为-1</returns>
        public int ExecuteSql(string sqlString)
        {
            lock (_o)
            {
                int count;
                Open();
                try
                {
                    var cmd = new OleDbCommand(sqlString, Connection);
                    count = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //UserPrompt.ShowException(
                    //string.Format("ExecuteSQL失败，SqlString={0},系统异常信息：{1}", sqlString, e.Message);
                    count = -1;
                }
                finally
                {
                    Close();
                }
                return count;
            }
        }

        /// <summary>
        /// 公有方法，执行一组Sql语句。
        /// </summary>
        /// <param name="sqlStrings">Sql语句组</param>
        /// <returns>是否成功</returns>
        public bool ExecuteSQL(string[] sqlStrings)
        {
            lock (_o)
            {
                bool success = true;
                Open();
                var cmd = new OleDbCommand();
                OleDbTransaction trans = Connection.BeginTransaction();
                cmd.Connection = Connection;
                cmd.Transaction = trans;

                int i = 0;
                try
                {
                    foreach (string str in sqlStrings)
                    {
                        cmd.CommandText = str;
                        cmd.ExecuteNonQuery();
                        i++;
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    //UserPrompt.ShowException(
                    //string.Format("ExecuteSQL失败，SqlString={0},系统异常信息：{1}", sqlStrings[i], e.Message), ErrorLogTitle);
                    success = false;
                    trans.Rollback();
                }
                finally
                {
                    Close();
                }
                return success;
            }
        }

        /// <summary>
        /// 公有方法，在一个数据表中插入一条记录。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">哈西表，键值为字段名，值为字段值</param>
        /// <returns>是否成功</returns>
        public bool Insert(string tableName, Hashtable cols)
        {
            int count = 0;

            if (cols.Count <= 0)
            {
                return true;
            }

            string fields = " (";
            string values = " Values(";
            foreach (DictionaryEntry item in cols)
            {
                if (count != 0)
                {
                    fields += ",";
                    values += ",";
                }
                fields += item.Key;
                values += item.Value.ToString();
                count++;
            }
            fields += ")";
            values += ")";

            string sqlString = string.Format("Insert into {0}{1}{2}", tableName, fields, values);

            string[] sqls = { sqlString };
            return ExecuteSQL(sqls);
        }
        
        /// <summary>
        /// 公有方法，更新一个数据表。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">哈西表，键值为字段名，值为字段值</param>
        /// <param name="where">Where子句</param>
        /// <returns>是否成功</returns>
        public bool Update(string tableName, Hashtable cols, string where)
        {
            int count = 0;
            if (cols.Count <= 0)
            {
                return true;
            }
            string fields = " ";
            foreach (DictionaryEntry item in cols)
            {
                if (count != 0)
                {
                    fields += ",";
                }
                fields += item.Key;
                fields += "=";
                fields += item.Value.ToString();
                count++;
            }
            fields += " ";

            string sqlString = string.Format("Update {0} Set {1}{2}", tableName, fields, where);

            string[] sqls = { sqlString };
            return ExecuteSQL(sqls);
        }

        /// <summary>
        /// 判斷表格是否存在
        /// </summary>
        /// <param name="tableName">表格名稱</param>
        /// <returns>是否存在</returns>
        public bool CheckTableExisting(string tableName)
        {
            lock (_o)
            {
                Open();
                DataTable schemaTable;
                try
                {
                    schemaTable = Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                                                                 new object[] { null, null, null, "TABLE" });
                }
                catch (Exception)
                {
                    Close();
                    return false;
                }
                finally
                {
                    Close();
                }
                if (schemaTable == null)
                {
                    return false;
                }


                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    if (tableName == schemaTable.Rows[i][2].ToString())
                    {
                        return true;
                    }
                }


                return false;

                //catch (Exception)
                //{
                //    return false;
                //}
                //finally
                //{
                //    Close();
                //}
            }
        }
        
        /// <summary>
        /// 将DataTable中的数据导出到Excel //ZH0704
        /// </summary>
        /// <param name="tmpDataTable"></param>
        /// <param name="savePath"></param>
        //private void DataTabletoExcel(System.Data.DataTable tmpDataTable, string savePath)
        //{

        //    if (tmpDataTable == null)

        //        return;

        //    int rowNum = tmpDataTable.Rows.Count;//需要导出的数据的行数

        //    int columnNum = tmpDataTable.Columns.Count;//需要导出的数据的列数

        //    int rowIndex = 1;//起始行为第二行

        //    int columnIndex = 0;//起始列为第一列

        //    Excel.Range range;//Excel的格式设置

        //    System.Reflection.Missing miss = System.Reflection.Missing.Value;



        //    Excel.Application xlApp = new Excel.Application();

        //    xlApp.DisplayAlerts = true;// 在程序执行过程中使出现的警告框显示

        //    xlApp.SheetsInNewWorkbook = 1;


        //    Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);

        //    foreach (DataColumn dc in tmpDataTable.Columns)             //将datatable的列名导入excel表的第一行
        //    {
        //        columnIndex++;
        //        xlApp.Cells[rowIndex, columnIndex] = dc.ColumnName;
        //    }

        //    //将数据写入到Excel表中

        //    for (int i = 0; i < rowNum; i++)
        //    {

        //        rowIndex++;

        //        columnIndex = 0;

        //        for (int j = 0; j < columnNum; j++)

        //        {//按行写入数据

        //            columnIndex++;

        //            range = (Microsoft.Office.Interop.Excel.Range)xlApp.Cells[rowIndex, columnIndex];

        //            range.NumberFormatLocal = "@";//写入到表中的数据格式以文本形式存在

        //            xlApp.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();

        //        }

        //    }

        //    //数据保存

        //    xlBook.SaveAs(savePath, miss, miss, miss, miss, miss, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);

        //    xlBook.Close(false, miss, miss);

        //    xlApp.Quit();

        //}

    }
}

