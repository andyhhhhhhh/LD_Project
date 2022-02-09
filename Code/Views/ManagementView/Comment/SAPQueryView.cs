using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Collections;
using ServiceController;
//using ADOX;

namespace ManagementView.Comment
{
    public partial class SAPQueryView : Form
    {
        public static string str_SDataPath = Application.StartupPath + "\\SData";

        public struct struct_PData
        {
            public string ScanTime;     //掃描時間
            public string ScanRCode;    //掃描二維碼
            public string PepNO;        //工號
            public string TaxNO;        //稅別
            public string PartNO;       //料號
            public string FctName;      //工廠
            public string BatchNum;     //批次
            public string ProductName;     //品名
            public string DataOfM;        //製造日期
            public string DataOfTog;      //收料日期
            public string ErrorOfSAP;     //SAP返回錯誤信息
            public string ReMark;         //備註//該項預留
        }


        public struct_PData struct_pCSVDA;

        public ArrayList sysRowAL; //行链表,CSV文件的每一行就是一个链
        public string sysFileName; //文件名
        public DataTable sysCsvDT = new DataTable();
        public Encoding sysEncoding; //编码
        //public DataTable sysTable = new DataTable();
        public bool sysIsFirst = true;
        //public List<ArrayList> sysArry = new List<ArrayList>();
        public int sysiLength = 12;    ////数据库每行的数据列数


        public SAPQueryView()
        {
            InitializeComponent();
        }
        private void SAPQueryView_Load(object sender, EventArgs e)
        {
            struct_pCSVDA.ScanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff:");     //掃描時間
            struct_pCSVDA.ScanRCode = "########03C107000028#####B0241201104601115##########400017036200010000#######20200904###GP2#######60";    //掃描二維碼
            struct_pCSVDA.PepNO = "152573";        //工號
            struct_pCSVDA.TaxNO = "A";        //稅別
            struct_pCSVDA.PartNO = "05A300000029875541V5669";       //料號
            struct_pCSVDA.FctName = "2021";      //工廠
            struct_pCSVDA.BatchNum = "A52022000V";     //批次
            struct_pCSVDA.ProductName = "TR ZXTP722MATA PNP S4L3.35KLJHJI5687";     //品名
            struct_pCSVDA.DataOfM = DateTime.Now.ToString("yyyy/MM/dd");        //製造日期
            struct_pCSVDA.DataOfTog = DateTime.Now.ToString("yyyy/MM/dd");     //收料日期
            struct_pCSVDA.ErrorOfSAP = "二維碼中的製造日期至收貨日的天數大於最小剩餘儲存期限!";     //SAP返回錯誤信息
            struct_pCSVDA.ReMark = "OK";         //備註//該項預留
        }
          
        //SAP查询成功记录
        private void bt_QueryInfor_Click(object sender, EventArgs e)
        {
            bt_RePrintLab.Enabled = false;

            string strCurParkDate = dTimePk_ParkData.Text;  //格式为： 20210412

            textBox_PartNO.Text = "";
            textBox_PartBatch.Text = "";
            textBox_PartName.Text = "";

            textBox_MDate.Text = "";
            textBox_ReceDate.Text = "";


            ReadCSVData_OK(struct_pCSVDA, strCurParkDate);
        }

        //SAP查询失败记录
        private void bt_QueryInfor_Fail_Click(object sender, EventArgs e)
        {


            string strCurParkDate = dateTimePicker_Fail.Text;  //格式为： 20210412



            string sPath = "";
            //bool bTitle = false;
            if (!Directory.Exists(str_SDataPath))  //Application.StartupPath + "\\SData";
            {
                Directory.CreateDirectory(str_SDataPath);
            }
            //sPath = str_SDataPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_Data.csv";

            sPath = str_SDataPath + "\\" + strCurParkDate + "_Data.csv";

            //////将原有的旧数据清掉               
            dataGridView_Fail.Rows.Clear();


            if (!File.Exists(sPath))
            {


                return;
            }



            sysFileName = sPath;
            sysRowAL = new ArrayList();
            sysEncoding = Encoding.Default;

            LoadCsvFileofNG();





        }

        private void bt_Clear_Click(object sender, EventArgs e)
        {
            textBox_ScanRCode.Text = "";
        }

        private void bt_RePrintLab_Click(object sender, EventArgs e)
        {
            try
            {
                string strPartNum = textBox_PartNO.Text;

                string strBatch = "N";
                object batch = textBox_PartBatch.Text;
                if (batch != null && batch.ToString() != "")
                {
                    strBatch = batch.ToString();
                }

                string strPartName = "N";
                string part1 = "N";
                string part2 = "N";
                object partName = textBox_PartName.Text;
                if (partName != null)
                {
                    strPartName = partName.ToString();
                    if (strPartName.Length > 20)
                    {
                        part1 = strPartName.Substring(0, 19);
                        part2 = strPartName.Substring(19);
                    }
                }

                string strManuDate = textBox_MDate.Text;
                string strNowDate = textBox_ReceDate.Text; 
                 

                int iCount = 1;
                if (!string.IsNullOrEmpty(comBox_PrintedNum.Text))
                {
                    iCount = Int32.Parse(comBox_PrintedNum.Text);
                }

                for (int i = 0; i < iCount; i++)
                {
                    ZebraPicControl.drawFinalLable("ZDesigner 110Xi4 600 dpi", strPartNum, strBatch, part1, part2, strManuDate, strNowDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";



        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
        }


        //视觉扫描记录
        private void bt_QueryInfor_CCD_Click(object sender, EventArgs e)
        {

            string strCurParkDate = dateTimePicker_CCD.Text;  //格式为： 20210412



            string sPath = "";
            //bool bTitle = false;
            if (!Directory.Exists(str_SDataPath))  //Application.StartupPath + "\\SData";
            {
                Directory.CreateDirectory(str_SDataPath);
            }
            //sPath = str_SDataPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_Data.csv";

            sPath = str_SDataPath + "\\" + strCurParkDate + "_Data.csv";

            //////将原有的旧数据清掉               
            dataGridView_CCD.Rows.Clear();


            if (!File.Exists(sPath))
            {


                return;
            }



            sysFileName = sPath;
            sysRowAL = new ArrayList();
            sysEncoding = Encoding.Default;

            LoadCsvFileofCCD();
        }
        
        private void ReadCSVData_OK(struct_PData struct_PDA, string inCurParkDate)
        {


            string sPath = "";
            //bool bTitle = false;
            if (!Directory.Exists(str_SDataPath))  //Application.StartupPath + "\\SData";
            {
                Directory.CreateDirectory(str_SDataPath);
            }
            //sPath = str_SDataPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_Data.csv";

            sPath = str_SDataPath + "\\" + inCurParkDate + "_Data.csv";

            //////将原有的旧数据清掉               
            dataGView_QueryResults.Rows.Clear();


            if (!File.Exists(sPath))
            {
                return;
            }

            try
            {
                sysFileName = sPath;

                CsvStreamReader_OK(sysFileName);


                ////开始写入
                ////ScanTime;     //掃描時間
                //// ScanRCode;    //掃描二維碼
                //// PepNO;        //工號
                ////TaxNO;        //稅別
                ////PartNO;       //料號
                //// FctName;      //工廠
                //// BatchNum;     //批次
                //// ProductName;     //品名
                //// DataOfM;        //製造日期
                //// DataOfTog;      //收料日期
                //// ErrorOfSAP;     //SAP返回錯誤信息
                //// ReMark;         //備註//該項預留
                //struct_PDA.ScanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff:"); //掃描時間
                //struct_PDA.DataOfM = DateTime.Now.ToString("yyyy/MM/dd");        //製造日期
                //struct_PDA.DataOfTog = DateTime.Now.ToString("yyyy/MM/dd");      //收料日期

                ////準備寫入的數據
                //sss = struct_PDA.ScanTime + "," + struct_PDA.ScanRCode + "," + struct_PDA.PepNO + "," +
                //      struct_PDA.TaxNO + "," + struct_PDA.PartNO + "," + struct_PDA.FctName + "," +
                //      struct_PDA.BatchNum + "," + struct_PDA.ProductName + "," + struct_PDA.DataOfM + "," +
                //      struct_PDA.DataOfTog + "," + struct_PDA.ErrorOfSAP + "," + struct_PDA.ReMark + "\r\n";


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "WriteData文件错误");

            }
        }

        private void bt_WriteCSVData_Click(object sender, EventArgs e)
        {
            WriteCSVData(struct_pCSVDA);
        }
         
        public static void WriteCSVData(struct_PData struct_PDA)
        {
            string sPath = "";
            bool bTitle = false;
            if (!Directory.Exists(str_SDataPath))  //Application.StartupPath + "\\SData";
            {
                Directory.CreateDirectory(str_SDataPath);
            }
            sPath = str_SDataPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_Data.csv";
            try
            {

                string sss, ttt;
                sss = "";
                ttt = "";

                if (!File.Exists(sPath))
                {
                    bTitle = true;
                }

                FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                if (bTitle)
                {



                    ttt = "掃描時間,掃描二維碼,工號,稅別,料號,工廠,批次,品名,製造日期,收料日期,SAP返回錯誤信息,備註\r\n";

                    sw.Write(ttt);
                    bTitle = false;
                }

                //开始写入
                //ScanTime;     //掃描時間
                // ScanRCode;    //掃描二維碼
                // PepNO;        //工號
                //TaxNO;        //稅別
                //PartNO;       //料號
                // FctName;      //工廠
                // BatchNum;     //批次
                // ProductName;     //品名
                // DataOfM;        //製造日期
                // DataOfTog;      //收料日期
                // ErrorOfSAP;     //SAP返回錯誤信息
                // ReMark;         //備註//該項預留
                //struct_PDA.ScanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff:"); //掃描時間
                //struct_PDA.DataOfM = DateTime.Now.ToString("yyyy/MM/dd");        //製造日期
                //struct_PDA.DataOfTog = DateTime.Now.ToString("yyyy/MM/dd");      //收料日期

                //準備寫入的數據
                sss = struct_PDA.ScanTime + "," + struct_PDA.ScanRCode + "," + struct_PDA.PepNO + "," +
                      struct_PDA.TaxNO + "," + struct_PDA.PartNO + "," + struct_PDA.FctName + "," +
                      struct_PDA.BatchNum + "," + struct_PDA.ProductName + "," + struct_PDA.DataOfM + "," +
                      struct_PDA.DataOfTog + "," + struct_PDA.ErrorOfSAP + "," + struct_PDA.ReMark + "\r\n";

                sw.Write(sss);


                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
                ;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "WriteData文件错误");

            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public void CsvStreamReader_OK(string fileName)
        {
            sysRowAL = new ArrayList();
            sysFileName = fileName;
            sysEncoding = Encoding.Default;

            LoadCsvFileofOK();




        }
         
        /// <summary>
        /// 载入CSV文件
        /// </summary>
        private void LoadCsvFileofCCD()
        {
            //对数据的有效性进行验证

            if (sysFileName == null)
            {
                throw new Exception("请指定要载入的CSV文件名");
            }
            else if (!File.Exists(sysFileName))
            {
                throw new Exception("指定的CSV文件不存在");
            }
            else
            {
            }
            if (sysEncoding == null)
            {
                sysEncoding = Encoding.Default;
            }
            DataTable dt = new DataTable();
            StreamReader sr = new StreamReader(sysFileName, sysEncoding);
            string csvDataLine = "";
            string fileDataLine = "";

            fileDataLine = sr.ReadLine();

            //开始写入新的数据
            while (true)
            {
                fileDataLine = "";

                fileDataLine = sr.ReadLine();
                if (fileDataLine == null)
                {
                    break;
                }
                if (csvDataLine == "")
                {
                    csvDataLine = fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLineTo_CCD_DataGView(csvDataLine);

                    csvDataLine = "";
                }
            }
            sr.Close();
            //数据行出现奇数个引号
            if (csvDataLine.Length > 0)
            {
                throw new Exception("CSV文件的格式有错误");
            }
        }


        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLineTo_CCD_DataGView(string newDataLine)
        {
            int iRows = 0;


            //DataRow Row = sysCsvDT.NewRow();
            //ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            //bool oddStartQuota = false; //是否以奇数个引号开始
            string strCellData = "";

            //sysiLength = 12; //数据库每行共12列数据


            iRows = dataGridView_CCD.Rows.Add(); //添加一行
            for (int i = 0; i < sysiLength; i++)
            {
                if (i < dataArray.Length)
                {
                    if (i - 1 <= 0)    //扫描时间 //扫描二维码
                    {
                        strCellData = dataArray[i];
                        dataGridView_CCD.Rows[iRows].Cells[i].Value = strCellData;
                    }



                }


            }




        }
         
        /// <summary>
        /// 载入CSV文件
        /// </summary>
        private void LoadCsvFileofNG()
        {
            //对数据的有效性进行验证

            if (sysFileName == null)
            {
                throw new Exception("请指定要载入的CSV文件名");
            }
            else if (!File.Exists(sysFileName))
            {
                throw new Exception("指定的CSV文件不存在");
            }
            else
            {
            }
            if (sysEncoding == null)
            {
                sysEncoding = Encoding.Default;
            }
            DataTable dt = new DataTable();
            StreamReader sr = new StreamReader(sysFileName, sysEncoding);
            string csvDataLine = "";
            string fileDataLine = "";

            fileDataLine = sr.ReadLine();

            List<string> listLine = new List<string>();
            while (true)
            {
                string strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                listLine.Add(strLine);
            }
            listLine.Reverse();

            if (listLine.Count > 0)
            {
                foreach (var item in listLine)
                {
                    fileDataLine = item;
                    if (csvDataLine == "")
                    {
                        csvDataLine = fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                    }
                    else
                    {
                        csvDataLine += "\r\n" + fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                    }
                    //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                    if (!IfOddQuota(csvDataLine))
                    {
                        AddNewDataLineTo_NG_DataGView(csvDataLine);

                        csvDataLine = "";
                    }
                }
            }

            //开始写入新的数据
            while (false)
            {
                fileDataLine = "";

                fileDataLine = sr.ReadLine();
                if (fileDataLine == null)
                {
                    break;
                }
                if (csvDataLine == "")
                {
                    csvDataLine = fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLineTo_NG_DataGView(csvDataLine);

                    csvDataLine = "";
                }
            }
            sr.Close();
            //数据行出现奇数个引号
            if (csvDataLine.Length > 0)
            {
                throw new Exception("CSV文件的格式有错误");
            }
        }


        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLineTo_NG_DataGView(string newDataLine)
        {
            int iRows = 0;


            //DataRow Row = sysCsvDT.NewRow();
            //ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            //bool oddStartQuota = false; //是否以奇数个引号开始
            string strCellData = "";

            //sysiLength = 12; //数据库每行共12列数据

            string strTemp = dataArray[dataArray.Length - 1];
            strTemp = strTemp.Trim();
            strTemp = strTemp.ToUpper();

            if (strTemp == "NG")
            {
                iRows = dataGridView_Fail.Rows.Add(); //添加一行
                for (int i = 0; i < sysiLength; i++)
                {
                    if (i < dataArray.Length)
                    {
                        if (i - 3 <= 0)    //扫描时间 //扫描二维码 //工号 //税别
                        {
                            strCellData = dataArray[i];
                            dataGridView_Fail.Rows[iRows].Cells[i].Value = strCellData;
                        }
                        else if (i - 10 == 0)  //SAP返回信息
                        {
                            strCellData = dataArray[i];
                            dataGridView_Fail.Rows[iRows].Cells[4].Value = strCellData;
                        }



                    }


                }

            }


        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public void CsvStreamReader(string fileName, Encoding encoding)
        {
            sysRowAL = new ArrayList();
            sysFileName = fileName;
            sysEncoding = encoding;
            LoadCsvFileofOK();
        }

        /// <summary>
        /// 载入CSV文件
        /// </summary>
        private void LoadCsvFileofOK()
        {
            //对数据的有效性进行验证

            if (sysFileName == null)
            {
                throw new Exception("请指定要载入的CSV文件名");
            }
            else if (!File.Exists(sysFileName))
            {
                throw new Exception("指定的CSV文件不存在");
            }
            else
            {
            }
            if (sysEncoding == null)
            {
                sysEncoding = Encoding.Default;
            }
            DataTable dt = new DataTable();
            StreamReader sr = new StreamReader(sysFileName, sysEncoding);
            string csvDataLine = "";
            string fileDataLine = "";

            fileDataLine = sr.ReadLine();

            ////////将原有的旧数据清掉
            //if (fileDataLine != "")
            //{
            //    dataGView_QueryResults.Rows.Clear();
            //}
             

            List<string> listLine = new List<string>();
            while(true)
            {
                string strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                listLine.Add(strLine);
            }
            listLine.Reverse();

            if (listLine.Count > 0)
            {
                foreach (var item in listLine)
                {
                    fileDataLine = item;
                    if (csvDataLine == "")
                    {
                        csvDataLine = fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                    }
                    else
                    {
                        csvDataLine += "\r\n" + fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                    }
                    //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                    if (!IfOddQuota(csvDataLine))
                    {
                        AddNewDataLineTo_OK_DataGView(csvDataLine);

                        csvDataLine = "";
                    }
                }
            }

          

            //开始写入新的数据
            while (false)
            {
                fileDataLine = "";

                fileDataLine = sr.ReadLine();
                if (fileDataLine == null)
                {
                    break;
                }
                if (csvDataLine == "")
                {
                    csvDataLine = fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;//GetDeleteQuotaDataLine(fileDataLine);
                }
                //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLineTo_OK_DataGView(csvDataLine);

                    csvDataLine = "";
                }
            }
            sr.Close();
            //数据行出现奇数个引号
            if (csvDataLine.Length > 0)
            {
                throw new Exception("CSV文件的格式有错误");
            }
        }


        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLineTo_OK_DataGView(string newDataLine)
        {
            int iRows = 0;

            //DataRow Row = sysCsvDT.NewRow();
            //ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            //bool oddStartQuota = false; //是否以奇数个引号开始
            string strCellData = "";

            //sysiLength = 12; //数据库每行共12列数据
            string strTemp = dataArray[dataArray.Length - 1];
            strTemp = strTemp.Trim();
            strTemp = strTemp.ToUpper();

            if (strTemp == "OK")
            {
                iRows = dataGView_QueryResults.Rows.Add(); //添加一行
                for (int i = 0; i < sysiLength; i++)
                {
                    if (i < dataArray.Length)
                    {
                        if (i - 9 <= 0)    //扫描时间 //扫描二维码 //工号 //税别
                        {
                            strCellData = dataArray[i];
                            dataGView_QueryResults.Rows[iRows].Cells[i].Value = strCellData;
                        }


                    }


                }

            }





        }
        

        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLine(string newDataLine)
        {
            int Column = 0;
            DataRow Row = sysCsvDT.NewRow();
            ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            bool oddStartQuota = false; //是否以奇数个引号开始
            string cellData = "";


            for (int j = 0; j < dataArray.Length; j++)
            {
                if (sysIsFirst)
                {
                    DataColumn dc = new DataColumn(dataArray[j]);
                    sysCsvDT.Columns.Add(dc);
                }
                else
                {
                    if (oddStartQuota)
                    {
                        //因为前面用逗号分割,所以要加上逗号
                        cellData += "," + dataArray[j];
                        //是否以奇数个引号结尾
                        if (IfOddEndQuota(dataArray[j]))
                        {

                            Row[Column] = GetHandleData(cellData);
                            Column++;
                            oddStartQuota = false;
                            continue;
                        }
                    }
                    else
                    {
                        //是否以奇数个引号开始
                        if (IfOddStartQuota(dataArray[j]))
                        {
                            //是否以奇数个引号结尾,不能是一个双引号,并且不是奇数个引号

                            if (IfOddEndQuota(dataArray[j]) && dataArray[j].Length > 2 && !IfOddQuota(dataArray[j]))
                            {

                                Row[Column] = GetHandleData(dataArray[j]);
                                Column++;
                                oddStartQuota = false;
                                continue;
                            }
                            else
                            {
                                oddStartQuota = true;
                                cellData = dataArray[j];
                                continue;
                            }
                        }
                        else
                        {
                            Row[Column] = GetHandleData(dataArray[j]);
                            Column++;
                        }
                    }

                }
            }

            if (!sysIsFirst)
            {
                sysCsvDT.Rows.Add(Row);
            }
            sysIsFirst = false;
            if (oddStartQuota)
            {
                throw new Exception("数据格式有问题");
            }

        }
         
        /// <summary>
        /// 去掉格子的首尾引号，把双引号变成单引号

        /// </summary>
        /// <param name="fileCellData"></param>
        /// <returns></returns>
        private string GetHandleData(string fileCellData)
        {
            if (fileCellData == "")
            {
                return "";
            }
            if (IfOddStartQuota(fileCellData))
            {
                if (IfOddEndQuota(fileCellData))
                {
                    return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
                }
                else
                {
                    throw new Exception("数据引号无法匹配" + fileCellData);
                }
            }
            else
            {
                //考虑形如"" """" """"""
                if (fileCellData.Length > 2 && fileCellData[0] == '\"')
                {
                    fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
                }
            }

            return fileCellData;


        }


        /// <summary>
        /// 判断是否以奇数个引号开始

        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddStartQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataCell.Length; i++)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }


        /// <summary>
        /// 判断是否以奇数个引号结尾
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddEndQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = dataCell.Length - 1; i >= 0; i--)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// 判断字符串是否包含奇数个引号
        /// </summary>
        /// <param name="dataLine">数据行</param>
        /// <returns>为奇数时，返回为真；否则返回为假</returns>
        private bool IfOddQuota(string dataLine)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataLine.Length; i++)
            {
                if (dataLine[i] == '\"')
                {
                    quotaCount++;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        //private static void WriteAccess(struct_PData struct_PDA)
        //{
        //    string sPath = "D:\\AOI\\DataBase";
        //    string strPath = "";
        //    string ConnectionString = "";
        //    string strSql = "";
        //    try
        //    {
        //        if (!Directory.Exists(sPath))
        //        {
        //            Directory.CreateDirectory(sPath);
        //        }
        //        strPath = sPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".accdb";
        //        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".accdb";
        //        if (!File.Exists(strPath))
        //        {
        //            ADOX.Catalog catalog = new Catalog();



        //            catalog.Create(ConnectionString);
        //            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog.ActiveConnection);
        //            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog);
        //            OleDbConnection conn = new OleDbConnection(ConnectionString);
        //            conn.Open();
        //            strSql = "create table data(ID AUTOINCREMENT PRIMARY KEY,时间 DATETIME,位置 TEXT,结果 TEXT,工位1测试时间 DATETIME,[工位1不良项/等级] TEXT,工位1结果 TEXT,工位2测试时间 DATETIME,[工位2不良项/等级] TEXT,工位2结果 TEXT,工位3测试时间 DATETIME,[工位3不良项/等级] TEXT,工位3结果 TEXT,工位4测试时间 DATETIME,[工位4不良项/等级] TEXT,工位4结果 TEXT);";
        //            OleDbCommand cmd = new OleDbCommand(strSql, conn);
        //            cmd.ExecuteNonQuery();
        //            //strSql = "insert into data(时间,位置,结果,工位1测试时间,[工位1不良项/等级],工位1结果,工位2测试时间,[工位2不良项/等级],工位2结果,工位3测试时间,[工位3不良项/等级],工位3结果,工位4测试时间,[工位4不良项/等级],工位4结果)" +
        //            //         "values('" + pd.id + "','" + pd.location + "','" + pd.result + "','" + pd.Sation1TestTime + "','" + pd.Sation1NonItem + "','" + pd.Sation1locationResult + "','" + pd.Sation2TestTime + "','" + pd.Sation2NonItem + "','" + pd.Sation2locationResult +
        //            //         "','" + pd.Sation3TestTime + "','" + pd.Sation3NonItem + "','" + pd.Sation3locationResult + "','" + pd.Sation4TestTime + "','" + pd.Sation4NonItem + "','" + pd.Sation4locationResult + "')";
        //            cmd = new OleDbCommand(strSql, conn);
        //            cmd.ExecuteNonQuery();
        //            conn.Close();
        //        }
        //        else
        //        {
        //            OleDbConnection conn = new OleDbConnection(ConnectionString);
        //            conn.Open();
        //            //strSql = "insert into data(时间,位置,结果,工位1测试时间,[工位1不良项/等级],工位1结果,工位2测试时间,[工位2不良项/等级],工位2结果,工位3测试时间,[工位3不良项/等级],工位3结果,工位4测试时间,[工位4不良项/等级],工位4结果)" +
        //            //    "values('" + pd.id + "','" + pd.location + "','" + pd.result + "','" + pd.Sation1TestTime + "','" + pd.Sation1NonItem + "','" + pd.Sation1locationResult + "','" + pd.Sation2TestTime + "','" + pd.Sation2NonItem + "','" + pd.Sation2locationResult +
        //            //     "','" + pd.Sation3TestTime + "','" + pd.Sation3NonItem + "','" + pd.Sation3locationResult + "','" + pd.Sation4TestTime + "','" + pd.Sation4NonItem + "','" + pd.Sation4locationResult + "')";
        //            OleDbCommand cmd = new OleDbCommand(strSql, conn);
        //            cmd.ExecuteNonQuery();
        //            conn.Close();
        //        }

        //    }
        //    catch
        //    {

        //    }

        //}

        private void dataGView_QueryResults_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void dataGView_QueryResults_SelectionChanged(object sender, EventArgs e)
        {
             

        }

        private void dataGView_QueryResults_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dataGView_QueryResults.CurrentRow.Index; //获取选中行的行号
            if (index >= 0)
            {
                textBox_PartNO.Text = dataGView_QueryResults.Rows[index].Cells[4].Value.ToString();
                textBox_PartBatch.Text = dataGView_QueryResults.Rows[index].Cells[6].Value.ToString();
                textBox_PartName.Text = dataGView_QueryResults.Rows[index].Cells[7].Value.ToString();

                textBox_MDate.Text = dataGView_QueryResults.Rows[index].Cells[8].Value.ToString();
                textBox_ReceDate.Text = dataGView_QueryResults.Rows[index].Cells[9].Value.ToString();

                bt_RePrintLab.Enabled = true;
            }

            //MessageBox.Show(index.ToString());
        }
    }
}
