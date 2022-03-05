using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelController
{
    public class OleExcel
    {
        public DataSet getData(string path)
        { 
            //判断文件后缀 
            string fileSuffix = System.IO.Path.GetExtension(path);
            if (string.IsNullOrEmpty(fileSuffix))
                return null;

            using (DataSet ds = new DataSet())
            {
                //判断Excel文件是2003版本还是2007版本
                string connString = "";
                //if (fileSuffix == ".xls")
                //    connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                //else
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

                //读取文件
                string sql_select = " SELECT * FROM [验证质检发货bar明细$]";

                using (OleDbConnection conn = new OleDbConnection(connString))
                using (OleDbDataAdapter cmd = new OleDbDataAdapter(sql_select, conn))
                {
                    conn.Open();
                    cmd.Fill(ds);
                }

                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds;
            }
        }


    }
}
