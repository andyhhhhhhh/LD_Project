using System;
using System.Data;
using System.Data.SqlClient;

namespace JsonController
{
    public class GetSafeData
    {
        #region DataRow

        /// <summary>
        /// 從一行中，安全得到列colname中的值：值為字串類型
        /// </summary>
        /// <param name="row">數據行對象</param>
        /// <param name="colname">列名</param>
        /// <returns>如果值存在，返回；否則 ，返回System.String.Empty</returns>
        public static string ValidateDataRowS(DataRow row, string colname)
        {
            return row[colname] != DBNull.Value ? row[colname].ToString() : String.Empty;
        }

        /// <summary>
        /// 從一行中，安全得到列colname中的值：值為整數類型
        /// </summary>
        /// <param name="row">數據行對象</param>
        /// <param name="colname">列名</param>
        /// <returns>如果值存在，返回；否則 ，返回System.Int32.MinValue</returns>
        public static int ValidateDataRowN(DataRow row, string colname)
        {
            return row[colname] != DBNull.Value ? Convert.ToInt32(row[colname]) : Int32.MinValue;
        }

        /// <summary>
        /// 從一行中，安全得到列colname中的值：值為浮點類型
        /// </summary>
        /// <param name="row">數據行對象</param>
        /// <param name="colname">列名</param>
        /// <returns>如果值存在，返回；否則 ，返回Double.MinValue</returns>
        public static double ValidateDataRowF(DataRow row, string colname)
        {
            return row[colname] != DBNull.Value ? Convert.ToDouble(row[colname]) : Double.MinValue;
        }

        /// <summary>
        /// 從一行中，安全得到列colname中的值：值為時間類型
        /// </summary>
        /// <param name="row">數據行對象</param>
        /// <param name="colname">列名</param>
        /// <returns>如果值存在，返回；否則 ，返回System.DateTime.MinValue;</returns>
        public static DateTime ValidateDataRowT(DataRow row, string colname)
        {
            return row[colname] != DBNull.Value ? Convert.ToDateTime(row[colname]) : DateTime.MinValue;
        }

        #endregion DataRow

        #region DataReader

        /// <summary>
        /// 從SqlDataReader中安全獲取數據
        /// </summary>
        /// <param name="reader">數據讀取器SqlDataReader</param>
        /// <param name="colname">列名</param>
        /// <returns>列中的字串數據，如果為空，則返回System.String.Empty</returns>
        public static string ValidateDataReaderS(SqlDataReader reader, string colname)
        {
            return reader.GetValue(reader.GetOrdinal(colname)) != DBNull.Value
                       ? reader.GetString(reader.GetOrdinal(colname))
                       : String.Empty;
        }

        /// <summary>
        /// 從SqlDataReader中安全獲取數據
        /// </summary>
        /// <param name="reader">數據讀取器SqlDataReader</param>
        /// <param name="colname">列名</param>
        /// <returns>列中的值為整形，如果為空，則返回System.Int32.MinValue</returns>
        public static int ValidateDataReaderN(SqlDataReader reader, string colname)
        {
            return reader.GetValue(reader.GetOrdinal(colname)) != DBNull.Value
                       ? reader.GetInt32(reader.GetOrdinal(colname))
                       : Int32.MinValue;
        }

        /// <summary>
        /// 從SqlDataReader中安全獲取數據
        /// </summary>
        /// <param name="reader">數據讀取器SqlDataReader</param>
        /// <param name="colname">列名</param>
        /// <returns>列中的值為Double型，如果為空，則返回System.Double.MinValue</returns>
        public static double ValidateDataReaderF(SqlDataReader reader, string colname)
        {
            return reader.GetValue(reader.GetOrdinal(colname)) != DBNull.Value
                       ? reader.GetDouble(reader.GetOrdinal(colname))
                       : Double.MinValue;
        }

        /// <summary>
        /// 從SqlDataReader中安全獲取數據
        /// </summary>
        /// <param name="reader">數據讀取器SqlDataReader</param>
        /// <param name="colname">列名</param>
        /// <returns>列中的值為DateTime，如果為空，則返回System.DateTime.MinValue</returns>
        public static DateTime ValidateDataReaderT(SqlDataReader reader, string colname)
        {
            return reader.GetValue(reader.GetOrdinal(colname)) != DBNull.Value
                       ? reader.GetDateTime(reader.GetOrdinal(colname))
                       : DateTime.MinValue;
        }

        #endregion DataReader
    }
}
