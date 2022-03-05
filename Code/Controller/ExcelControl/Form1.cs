using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using Spire.Xls;
using System.Text.RegularExpressions;

namespace ExcelController
{
    public partial class Form1 : Form
    {
        OleExcel mode = new OleExcel();
        public Excel.Application app;
        public Workbooks wbs;
        public Excel.Workbook wb;
        public Worksheets wss;
        public Excel.Worksheet ws;
        public DataSet dataSet = null;
        List<Bar> listBar = new List<Bar>();
        List<Product> listbar = new List<Product>();
        public int bar = 0;
        public string numStr = "";
        public string productType = "";
        public string mapType = "验证质检发货bar明细";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMapPath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                txtMapPath.Text = dialog.FileName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                string WaferNum = txtWaferNum.Text;
                if (dataSet == null && string.IsNullOrEmpty(txtWaferNum.Text))
                {
                    return;
                }
                productType = txtProductType.Text;
                if(txtMapType.Text.Equals("国外"))
                {
                    listBar = mode.GetDataGD(dataSet, productType);
                }
                else
                {
                    listBar = mode.GetData(dataSet, WaferNum, productType);//28W由软件选择产品时传递过来
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnReadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMapPath.Text == null)
                {
                    MessageBox.Show("请选择MAP路径");
                    return;
                }
                if (txtMapType.Text.Equals("国外"))
                {
                    mapType = "P&P Map";
                }
                else
                {
                    mapType = "验证质检发货bar明细";
                }
                string mapPath = txtMapPath.Text;
                DataSet da = mode.ExcelToDS(mapPath, mapType);
                dataSet = da;
                this.dataGridView1.DataSource = da;
                this.dataGridView1.DataMember = "table1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSetBackColor_Click(object sender, EventArgs e)
        {
            try
            {
                string path = txtMapPath.Text;
                int row = Convert.ToInt32(txtBoxRow.Text);
                int col = Convert.ToInt32(txtBoxCol.Text);
                string mapType = txtMapType.Text;
                if(mapType.Equals("国内"))
                { 
                    mode.SetBackClolor(path, row, col, mapType);
                }
                else
                {
                    mode.SetBackClolorGD(path, row, col, mapType);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        } 

        private void btnSetBar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtbar.Text))
                {
                    return;
                }
                bar = Convert.ToInt32(txtbar.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnParseBar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }
                if(txtMapType.Text.Equals("国外"))
                {
                    bar = Convert.ToInt32(mode.ParseBarGD(textBox1.Text));
                    numStr = mode.ParseNumGD(textBox1.Text);
                }
                else
                {
                    bar = Convert.ToInt32(mode.ParseBar(textBox1.Text, productType));
                }                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnGetBarData_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtMapType.Text.Equals("国外"))
                {
                    listbar = mode.GetBarDataGD(listBar, bar.ToString(),numStr.ToString());
                }
                else
                {
                    listbar = mode.GetBarData(listBar, bar.ToString());
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (mapType.Equals("验证质检发货"))
                {
                    for (int i = 0; i < 54; i++)
                    {
                        int row = 0;
                        int col = 0;
                        bool result = false;
                        if (listbar[i].isReclaimer == 1)
                        {
                            //发送OCR给视觉
                            mode.SendOcr(listbar[i].productOcr);
                            MessageBox.Show(listbar[i].productOcr);
                            row = listbar[i].rowOcr;
                            col = listbar[i].colOcr;
                            while (true)
                            {
                                if (!result)
                                {
                                    mode.SetBackClolor(txtMapPath.Text, row, col, mapType);
                                    break;
                                }
                                break;
                            }
                        }
                    }
                }
                //换行，该行取完
                else
                {
                    for (int i = 0; i < 25; i++)
                    {
                        int row = 0;
                        int col = 0;
                        bool result = false;
                        if (listbar[i].isReclaimer == 1)
                        {
                            //发送OCR给视觉
                            mode.SendOcr(listbar[i].productOcr);
                            MessageBox.Show(listbar[i].productOcr);
                            row = listbar[i].rowOcr;
                            col = listbar[i].colOcr;
                            while (true)
                            {
                                if (!result)
                                {
                                    mode.SetBackClolorGD(txtMapPath.Text, row, col, mapType);
                                    break;
                                }
                                break;
                            }
                        }
                    }
                }

                //换行，该行取完
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnUnMerge_Click(object sender, EventArgs e)
        {
            try
            {

                Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
                workbook.LoadFromFile(txtMapPath.Text);
                Spire.Xls.Worksheet sheet = workbook.Worksheets[0];

                CellRange[] range = sheet.MergedCells;
                foreach (CellRange cell in range)
                {
                    cell.UnMerge();
                }
                workbook.SaveToFile(txtMapPath.Text);


                workbook.Dispose();
                sheet.Dispose();
                //System.Diagnostics.Process.Start(txtMapPath.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        
    }
}
