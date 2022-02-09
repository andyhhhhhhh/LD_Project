using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace ExcelController
{
    /// <SUMMARY>
    /// Microsoft.Office.Interop.ExcelEdit 的摘要说明
    /// </SUMMARY>
    public class ExcelEdit
    {
        public string mFilename;
        public Application app;
        public Workbooks wbs;
        public Workbook wb;
        public Worksheets wss;
        public Worksheet ws;
        public ExcelEdit()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public void Create()//创建一个Microsoft.Office.Interop.Excel对象
        {
            app = new Application();
            wbs = app.Workbooks;
            wb = wbs.Add(true);
        }

        public void Open(string FileName)//打开一个Microsoft.Office.Interop.Excel文件
        {
            app = new Application();
            wbs = app.Workbooks; 
            //wb = wbs.Add(FileName);
            //wb = wbs.Open(FileName, 0, true, 5,"", "", true, XlPlatform.xlWindows, "t", false, false, 0, true,Type.Missing,Type.Missing);
            wb = wbs.Open(FileName,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,XlPlatform.xlWindows,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
            mFilename = FileName;
        }

        //获取一个工作表
        public Worksheet GetSheet(string SheetName)
        {
            Worksheet s = (Worksheet)wb.Worksheets[SheetName];
            return s;
        }

        //添加一个工作表
        public Worksheet AddSheet(string SheetName)
        {
            Worksheet s = (Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        public void DelSheet(string SheetName)//删除一个工作表
        {
            ((Worksheet)wb.Worksheets[SheetName]).Delete();
        }

        public Worksheet ReNameSheet(string OldSheetName, string NewSheetName)//重命名一个工作表一
        {
            Worksheet s = (Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }

        public Worksheet ReNameSheet(Worksheet Sheet, string NewSheetName)//重命名一个工作表二
        {
            Sheet.Name = NewSheetName;
            return Sheet;
        }

        //ws：要设值的工作表     X行Y列     value   值
        public void SetCellValue(Worksheet ws, int x, int y, object value)
        {
            ws.Cells[x, y] = value;
        }

        //ws：要设值的工作表的名称 X行Y列 value 值
        public void SetCellValue(string ws, int x, int y, object value)
        {
            GetSheet(ws).Cells[x, y] = value;
        }

        public object GetSheetValue(string SheetName, string strWafer = "A5945-12")
        {
            //取得总记录行数   (包括标题列)
            ws = GetSheet(SheetName);
            int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数
            int columnsint = ws.UsedRange.Cells.Columns.Count;//得到列数

            //取得数据范围区域 (不包括标题列) 
            Range rng1 = ws.Cells.get_Range("B1", "B" + rowsint);   //item 
            object[,] arryItem = (object[,])rng1.Value;
            string[] arry = new string[rowsint - 1];

            List<string[]> listAdd = new List<string[]>();
            int index = 0;
            for (int i = 1; i <= rowsint - 1; i++)
            {
                arry[i-1] = arryItem[i, 1].ToString();

                if (arry[i-1] == strWafer)
                {
                    //获取特定行的数据
                    List<string> liststr = new List<string>();
                    liststr.Add(strWafer);

                    for (int j = 3; j < columnsint; j++)
                    {
                        string value = ""; 
                        object value1 = ws.Cells[i, j].Value2;

                        if (value1 == null)
                        {
                            value = "0";
                        }
                        else
                        {
                            value = value1.ToString();
                        }
                        liststr.Add(value);
                    }

                    listAdd.Add(liststr.ToArray());
                    index++;
                }
            }

            return listAdd;
        }

        //设置一个单元格的属性 字体，大小，颜色，对齐方式
        public void SetCellProperty(Worksheet ws, int Startx, int Starty, int Endx, int Endy, int size, string name, Constants color, Constants HorizontalAlignment)
        {
            name = "宋体";
            size = 12;
            color = Constants.xlAutomatic;
            HorizontalAlignment = Constants.xlRight;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Constants color, Constants HorizontalAlignment)
        {
            //name = "宋体";
            //size = 12;
            //color = Constants.xlAutomatic;
            //HorizontalAlignment = Constants.xlRight;

            Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;

            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }


        //合并单元格
        public void UniteCells(Worksheet ws, int x1, int y1, int x2, int y2)
        {
            ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
        }

        //合并单元格
        public void UniteCells(string ws, int x1, int y1, int x2, int y2)
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing); 
        } 

        //将内存中数据表格插入到Microsoft.Office.Interop.Excel指定工作表的指定位置 为在使用模板时控制格式时使用一
        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[startX + i, j + startY] = dt.Rows[i][j].ToString();
                }
            } 
        }

        //将内存中数据表格插入到Microsoft.Office.Interop.Excel指定工作表的指定位置二
        public void InsertTable(System.Data.DataTable dt, Worksheet ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ws.Cells[startX + i, j + startY] = dt.Rows[i][j];
                }
            }
        }

        //将内存中数据表格添加到Microsoft.Office.Interop.Excel指定工作表的指定位置一
        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[i + startX, j + startY] = dt.Rows[i][j];
                }
            }
        }

        //将内存中数据表格添加到Microsoft.Office.Interop.Excel指定工作表的指定位置二
        public void AddTable(System.Data.DataTable dt, Worksheet ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ws.Cells[i + startX, j + startY] = dt.Rows[i][j];
                }
            }
        }

        //插入图片操作一
        public void InsertPictures(string Filename, string ws)
        {
            GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            //后面的数字表示位置
        }

        public void InsertActiveChart(Microsoft.Office.Interop.Excel.XlChartType ChartType, string ws, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Microsoft.Office.Interop.Excel.XlRowCol ChartDataType)
        //插入图表操作
        {
            ChartDataType = Microsoft.Office.Interop.Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(ws).get_Range(GetSheet(ws).Cells[DataSourcesX1, DataSourcesY1], GetSheet(ws).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(XlChartLocation.xlLocationAsObject, ws);
            }
        }

        //保存文档
        public bool Save()
        {
            if (mFilename == "")
            {
                return false;
            }
            else
            {
                try
                {
                    wb.Save();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        //文档另存为
        public bool SaveAs(object FileName)
        {
            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        //关闭一个Microsoft.Office.Interop.Excel对象，销毁对象
        public void Close()
        {
            //wb.Save();
            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }

        /// <summary>
        /// 设置单元格底色
        /// </summary>
        /// <param name="wsn">表名</param>
        /// <param name="Startx">行</param>
        /// <param name="Starty">列</param>
        public void SetCellColor(string SheetName, int Startx, int Starty)
        {
            Worksheet ws = GetSheet(SheetName);
            Range titleRange = ws.Range[ws.Cells[Startx, Starty], ws.Cells[Startx, Starty]];
            titleRange.Interior.Color = Color.Green;//设置颜色
            SaveAs(@"F:\3.xlsx");
            //Save();
            //Close();
        }

        /// <summary>
        /// 获取Map对应值
        /// </summary>
        /// <param name="SheetName">表明</param>
        /// <param name="strWafer">Wafer值</param>
        /// <returns></returns>
        public Wafer GetMapValue(string SheetName, string strWafer = "A5945-12")
        { 
            Wafer wafer = new Wafer();

            //取得总记录行数   (包括标题列)
            ws = GetSheet(SheetName);
            int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数
            int columnsint = ws.UsedRange.Cells.Columns.Count;//得到列数

            //取得数据范围区域 (不包括标题列) 
            Range rng1 = ws.Cells.get_Range("B1", "B" + rowsint);   //item 
            object[,] arryItem = (object[,])rng1.Value;
            string[] arry = new string[rowsint - 1];
             
            List<Bar> listbar = new List<Bar>(); 
            for (int i = 1; i <= rowsint - 1; i++)
            {
                arry[i - 1] = arryItem[i, 1].ToString();

                if (arry[i - 1].ToUpper() == strWafer.ToUpper())
                {
                    //获取特定行的数据
                    wafer.waferName = strWafer;
                    Bar bar = new Bar();
                    bar.index = i;
                    List<Product> listpro = new List<Product>();
                    for (int j = 3; j < columnsint; j++)
                    {
                        Product product = new Product();
                        string value = "";
                        object value1 = ws.Cells[i, j].Value2;
                        if (value1 == null)
                        {
                            value = "0";
                        }
                        else
                        {
                            value = value1.ToString();
                        }

                        //获取 bar
                        if (j == 3)
                        {
                            bar.barId = Int32.Parse(value);
                            continue;
                        }

                        product.id = j - 2;
                        product.productOcr = Int32.Parse(value);
                        listpro.Add(product); 
                    }

                    bar.product = listpro.ToArray();
                    listbar.Add(bar);
                }
            }

            wafer.bar = listbar.ToArray(); 

            return wafer;
        }
    }


    public class MaterialMap
    {
        public Wafer[] Wafers { get; set; }
    }

    public class Wafer
    {
        public int id;
        public string waferName;
        public string waferPath;
        public Bar[] bar;//每个wafer中最多bar数
    };

    public class Bar
    {
        public int index;
        public int barId;
        public int OKCount;
        public Product[] product;
    };

    public class Product
    {
        public int id;
        public int productOcr;
        public int isReclaimer;
    };
}