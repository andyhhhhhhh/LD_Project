using Microsoft.Office.Interop.Excel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelController
{
    public class OleExcel
    {
        public string mFilename;
        public Application app;
        public Workbooks wbs;
        public Microsoft.Office.Interop.Excel.Workbook wb;
        public Worksheets wss;
        public Microsoft.Office.Interop.Excel.Worksheet ws;
        public bool finish = false;
        public string barString = "";
        public string numString = "";
        DataSet ds = null;
        List<Bar> listBar = new List<Bar>();

        public OleExcel()
        {

        }
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public DataSet ExcelToDS(string Path, string MapType)
        {
            //getData(Path);

            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            //string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + Path + ";Extended Properties='Excel 12.0;";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Path + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            ds = null;
            strExcel = "select * from [" + MapType + "$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            conn.Close();
            return ds;
        }
        /// <summary>
        /// dataset格式写入excel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="oldds"></param>
        public void DSToExcel(string Path, DataSet oldds)
        {
            //先得到汇总Excel的DataSet 主要目的是获得Excel在DataSet中的结构
            //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            //string strCon = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" +Path+ ";Extended Properties='Excel 8.0; HDR=Yes; IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件
            string strCon = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + Path + ";Extended Properties='Excel 12.0; HDR=No; IMEX=0'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。//      "IMEX=1 "如果列中的数据类型不一致，使用"IMEX=1"可必免数据类型冲突。 


            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = "select * from [验证质检发货bar明细$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
            //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。
            builder.QuotePrefix = "[";     //获取insert语句中保留字符（起始位置）
            builder.QuoteSuffix = "]"; //获取insert语句中保留字符（结束位置）
            DataSet newds = new DataSet();
            myCommand.Fill(newds, "Table1");
            for (int i = 0; i < oldds.Tables[0].Rows.Count; i++)
            {
                //在这里不能使用ImportRow方法将一行导入到news中，
                //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added
                DataRow nrow = newds.Tables["Table1"].NewRow();
                for (int j = 0; j < oldds.Tables[0].Columns.Count; j++)
                {
                    nrow[j] = oldds.Tables[0].Rows[i][j];

                }
                newds.Tables["Table1"].Rows.Add(nrow);
            }
            myCommand.Update(newds, "Table1");
            myConn.Close();
        }
        /// <summary>
        /// 获取对应wafera数据(国外）
        /// </summary>
        /// <param name="data">总数据</param>
        /// <param name="waferNum">wafer名称</param>
        /// <param name="productType">产品类型</param>
        public List<Bar> GetDataGD(DataSet data, string productType)
        {
            try
            {
                //int barNum = 0;//Bar长度
                int num = 0;
                System.Data.DataTable dt = data.Tables[0];
                DataRow[] drArr = dt.Select();//查询(如果Select内无条件，就是查询所有的数据)  
                //List<Bar> listBar = new List<Bar>();
                int colMax = data.Tables[0].Columns.Count;//最大列数
                int rowMax = data.Tables[0].Rows.Count;//最大行数
                for (int i = 0; i < colMax - 2; i++)
                {
                    Bar bar = new Bar();
                    string rowGroup = "";
                    bar.barId = Int32.Parse(drArr[0].ItemArray[i + 1].ToString());
                    //num = Int32.Parse(drArr[i].ItemArray[0].ToString()); //num为map表中的序号
                    List<Product> listProduct = new List<Product>();
                    for (int j = 1; j <= rowMax - 1; j++)
                    {
                        Product product = new Product();
                        if ((j + 24) % 25 == 0)
                        {
                            rowGroup = drArr[j].ItemArray[colMax - 1].ToString();
                        }
                        product.id = j;
                        product.rowGroup = rowGroup;
                        bar.rowGroup = rowGroup;
                        product.rowOcr = j + 2;
                        product.colOcr = i + 2;
                        num = Int32.Parse(drArr[j].ItemArray[0].ToString());
                        string ocr = bar.barId.ToString().PadLeft(2, '0') + num.ToString().PadLeft(3, '0');
                        product.productOcr = ocr;
                        var reclimer = drArr[j].ItemArray[i + 1];
                        if (reclimer == null)
                        {
                            product.isReclaimer = 0;
                        }
                        else
                        {
                            product.isReclaimer = reclimer.ToString() == "1" ? 1 : 0;
                        }
                        listProduct.Add(product);
                    } 
                    bar.product = listProduct.ToArray();
                    listBar.Add(bar);
                }
                return listBar; 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 获取对应wafera数据(国内）
        /// </summary>
        /// <param name="data">总数据</param>
        /// <param name="waferNum">wafer名称</param>
        /// <param name="productType">产品类型</param>
        public List<Bar> GetData(DataSet data, string waferNum, string productType)
        {
            try
            {
                int barNum = 0;//Bar长度
                int num = 0;
                System.Data.DataTable dt = data.Tables[0];//假设dt是由"SELECT C1,C2,C3 FROM T1"查询出来的结果 
                //但这种做法用一两次还好说，用多了就累了。那有没有更好的方法呢？就是dt.Select()，上面的操作可以改成这样：
                string strSel = string.Format("Wafer='{0}'", waferNum);
                DataRow[] drArr = dt.Select(strSel);//查询(如果Select内无条件，就是查询所有的数据)
                if (productType.Contains("28"))
                {
                    barNum = 3;
                }
                else
                {
                    barNum = 2;
                }
                List<Bar> listBar = new List<Bar>();
                for (int i = 0; i < drArr.Length; i++)
                {
                    Bar bar = new Bar();
                    bar.barId = Int32.Parse(drArr[i].ItemArray[2].ToString());
                    num = Int32.Parse(drArr[i].ItemArray[0].ToString()); //num为map表中的序号
                    List<Product> listProduct = new List<Product>();
                    for (int j = 1; j <= 54; j++)
                    {
                        Product product = new Product();
                        product.id = j;
                        product.rowOcr = num + 1;
                        product.colOcr = j + 3;
                        string ocr = bar.barId.ToString().PadLeft(barNum, '0') + j.ToString().PadLeft(2, '0');
                        product.productOcr = ocr;
                        var reclimer = drArr[i].ItemArray[j + 2];
                        if (reclimer == null)
                        {
                            product.isReclaimer = 0;
                        }
                        else
                        {
                            product.isReclaimer = reclimer.ToString() == "1" ? 1 : 0;
                        }

                        listProduct.Add(product);
                    }

                    bar.product = listProduct.ToArray();
                    listBar.Add(bar);
                }
                return listBar;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 解析num(国外）
        /// </summary>
        /// <param name="barData">获取num条字符串</param> 
        public string ParseNumGD(string barData)
        {
            try
            {
                if (listBar == null)
                {
                    return null;
                }
                //字符串分割 
                string[] numStr = barData.Split(new char[] { ',' });
                string[] num = new string[numStr.Length];
                for (int i = 0; i < numStr.Length; i++)
                {
                    for (int j = 0; j < listBar.Count; j++)
                    {
                        for (int k = 0; k < listBar[j].product.Length; k++)
                        {
                            if (String.Equals(numStr[i], listBar[j].product[k].productOcr))
                            {
                                num[i] = listBar[j].product[k].rowGroup.Trim();
                            }
                        } 
                    } 
                }
                numString = SearchGD(num);
                return numString;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        /// <summary>
        /// 解析bar(国外）
        /// </summary>
        /// <param name="barData">获取bar条字符串</param> 
        public string ParseBarGD(string barData)
        {
            try
            {
                //字符串分割
                int start = 0;
                int leght = 2;
                string[] barStr = barData.Split(new char[] { ',' });
                string[] barNum = new string[barStr.Length];
                for (int i = 0; i < barStr.Length; i++)
                {
                    barNum[i] = barStr[i].Substring(start, leght);
                }
                barString = SearchGD(barNum);
                return barString;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 解析bar
        /// </summary>
        /// <param name="barData">获取bar条字符串</param>
        /// <param name="productType">产品类型</param>
        public string ParseBar(string barData, string productType)
        {
            try
            {
                //字符串分割
                int start = 0;
                int leght = 3;
                string[] barStr = barData.Split(new char[] { ',' });
                string[] barNum = new string[barStr.Length];
                if (productType == "28")
                {
                    leght = 3;
                }
                else
                {
                    leght = 2;
                }
                for (int i = 0; i < barStr.Length; i++)
                {
                    barNum[i] = barStr[i].Substring(start, leght);
                }
                barString = Search(barNum);
                return barString;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        /// <summary>
        /// 求出现次数最多的数为bar
        /// </summary>
        /// <param name="arrs">bar数组</param>
        /// <returns></returns>
        public string SearchGD(string[] arrs)
        {
            int len = arrs.Length;
            int max = 0;  //出现最多的次数
            string num = "";  //当前的数字

            List<string> temps = new List<string>(); //a
            for (int i = 0; i < len; i++)
            {
                if (temps.Contains(arrs[i]))
                {
                    continue;      //排除之前参与过的数字   
                }
                int count = 0;
                for (int j = 0; j < len; j++)
                {
                    if (arrs[i] == arrs[j])
                    {
                        count++;
                    }
                }
                if (count > max)
                {
                    max = count;
                    num = arrs[i];
                }
                temps.Add(arrs[i]); //a
            }
            return num;
        }
        /// <summary>
        /// 求出现次数最多的数为bar
        /// </summary>
        /// <param name="arrs">bar数组</param>
        /// <returns></returns>
        public string Search(string[] arrs)
        {
            int len = arrs.Length;
            int max = 0;  //出现最多的次数
            string num = "";  //当前的数字

            List<string> temps = new List<string>(); //a
            for (int i = 0; i < len; i++)
            {
                if (temps.Contains(arrs[i]))
                {
                    continue;      //排除之前参与过的数字   
                }
                int count = 0;
                for (int j = 0; j < len; j++)
                {
                    if (arrs[i] == arrs[j])
                    {
                        count++;
                    }
                }
                if (count > max)
                {
                    max = count;
                    num = arrs[i];
                }
                temps.Add(arrs[i]); //a
            }
            return num;
        }
        /// <summary>
        /// 获取对应bar的数据(国外）
        /// </summary>
        /// <param name="listBar">wafer对应的数据</param>
        /// <param name="Bar">bar</param>
        /// <returns></returns>
        public List<Product> GetBarDataGD(List<Bar> ListBar, string Bar, string num)
        {
            try
            {
                List<Product> list = new List<Product>();
                int barNum = Convert.ToInt32(Bar);
                for (int i = 0; i < ListBar.Count; i++)
                {
                    for (int j = 0; j < listBar[i].product.Length; j++)
                    {
                        if (ListBar[i].barId == barNum && listBar[i].product[j].rowGroup == num)
                        {
                            //list = listBar[i].product.ToList();
                            Product product = new Product();
                            product.id = listBar[i].product[j].id; 
                            product.rowOcr = listBar[i].product[j].rowOcr; 
                            product.colOcr = listBar[i].product[j].colOcr; 
                            product.productOcr = listBar[i].product[j].productOcr; 
                            product.isReclaimer = listBar[i].product[j].isReclaimer;
                            product.rowGroup = listBar[i].product[j].rowGroup;
                            list.Add(product);
                        }
                        
                    }
                    
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 获取对应bar的数据
        /// </summary>
        /// <param name="listBar">wafer对应的数据</param>
        /// <param name="Bar">bar</param>
        /// <returns></returns>
        public List<Product> GetBarData(List<Bar> listBar, string Bar)
        {
            try
            {
                List<Product> list = new List<Product>();
                int barNum = Convert.ToInt32(Bar);
                for (int i = 0; i < listBar.Count; i++)
                {
                    if (listBar[i].barId == barNum)
                    {
                        list = listBar[i].product.ToList();


                    }
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 发送OCR给视觉
        /// </summary>
        /// <param name="OCR">OCR</param>
        public void SendOcr(string OCR)
        {

        }
        /// <summary>
        /// 设置Map表单元格底色
        /// </summary>
        /// <param name="mapPath">Map表路径</param>
        /// <param name="rows">行</param>
        /// <param name="cols">列</param>
        /// <returns></returns>
        public void SetBackClolor(string mapPath, int rows, int cols, string mapType)
        {
            try
            {
                Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
                workbook.LoadFromFile(mapPath, ExcelVersion.Version2013);
                Spire.Xls.Worksheet worksheet = workbook.Worksheets[mapType];
                worksheet.Range[rows, cols].Style.Color = Color.Green;
                workbook.SaveToFile(mapPath, /*FileFormat.Version2010*/ ExcelVersion.Version2013);
                workbook.Dispose();
                worksheet.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetBackClolorGD(string mapPath, int rows, int cols, string mapType)
        {
            try
            {
                Application app = new Application();
                Workbooks wbks = app.Workbooks;
                _Workbook _wbk = wbks.Add(mapPath);
                Sheets shs = _wbk.Sheets;
                Sheets ExcelSheet = _wbk.Sheets;
                _Worksheet _wsh = (_Worksheet)shs.get_Item(mapType);
                _wsh.Cells[rows, cols].Interior.Color = System.Drawing.Color.FromArgb(0, 255, 0).ToArgb();
                app.Application.DisplayAlerts = false;
                _wbk.SaveAs(mapPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                _wbk.Close();
                wbks.Close();
                app.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    public class Wafer
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        /// <summary>
        /// wafer名称
        /// </summary>
        public string waferName;
        /// <summary>
        /// wafer路径
        /// </summary>
        public string waferPath;
        /// <summary>
        /// 每个wafer中最多bar数
        /// </summary>
        public Bar[] bar;
    };

    public class Bar
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int index;
        /// <summary>
        /// Bar条ID
        /// </summary>
        public int barId;
        /// <summary>
        /// OK数量
        /// </summary>
        public int OKCount;
        /// <summary>
        /// 每个Bar条最多的OCR数
        /// </summary>    
        public Product[] product;
        /// /// <summary>
        /// row分组
        /// </summary>
        public string rowGroup;
        /// <summary>
        /// 数据
        /// </summary>
        public Data[] data;
    };
    public class Product
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        /// <summary>
        /// 对应ID的OCR
        /// </summary>
        public string productOcr;
        /// <summary>
        /// 是否需要取
        /// 0不取，1取
        /// </summary>
        public int isReclaimer;
        /// <summary>
        /// Ocr所在行
        /// </summary>
        public int rowOcr;
        /// <summary>
        /// Ocr所在列
        /// </summary>
        public int colOcr;
        /// <summary>
        /// row分组
        /// </summary>
        public string rowGroup;
    };
    public class Data
    {
        /// <summary>
        /// 序号，第几个
        /// </summary>
        public int id;
        /// <summary>
        /// 对应ID的OCR
        /// </summary>
        public string productOcr;
        /// <summary>
        /// 是否需要取
        /// 0不取，1取
        /// </summary>
        public int isReclaimer;
        /// <summary>
        /// Ocr所在行
        /// </summary>
        public int rowOcr;
        /// <summary>
        /// Ocr所在列
        /// </summary>
        public int colOcr;
        /// <summary>
        /// waferName名称
        /// </summary>
        public string waferName;
        /// <summary>
        /// bar条名称
        /// </summary>
        public int bar;
    };
}
