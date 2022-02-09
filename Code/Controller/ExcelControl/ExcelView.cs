
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelController
{
    public partial class ExcelView : Form
    {
        ExcelEdit m_excel = new ExcelEdit();
        public ExcelView()
        {
            InitializeComponent();
        }

        private void ExcelView_Load(object sender, EventArgs e)
        {
            
        }

        private void btnLoadPath_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "xlsx文件|*.xlsx|all|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                fileName = openFileDialog.FileName;
                txtPath.Text = fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtPath.Text))
                {
                    return;
                }

                 
                m_excel.Open(txtPath.Text);
            }
            catch (Exception ex)
            {
                 txtValue.Text = ex.Message;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                //object list2 = m_excel.GetSheetValue("验证质检发货bar明细", txtWafer.Text); 
                //List<string[]> listvalue = list2 as List<string[]>;
                //txtValue.Text = "";
                //foreach (var item in listvalue)
                //{
                //    string[] strArr = item;
                //    string str = "";
                //    for (int i = 0; i < strArr.Length; i++)
                //    {
                //        str += strArr[i] + ",";
                //    }
                //    txtValue.AppendText(str + Environment.NewLine);
                //}

                Wafer wafer = m_excel.GetMapValue("验证质检发货bar明细", txtWafer.Text);
                int length = wafer.bar.Length;
                txtValue.Text = "";
                for (int i = 0; i < length; i++)
                {
                    string str = "";  
                    str +=  wafer.bar[i].index + ",";
                    str += wafer.waferName + ",";
                    str +=  wafer.bar[i].barId + ",";

                    int proLength = wafer.bar[i].product.Length;
                    for (int j = 0; j < proLength; j++)
                    {
                        str += wafer.bar[i].product[j].productOcr + ",";
                    }

                    txtValue.AppendText(str + Environment.NewLine);
                }
            }
            catch (Exception ex)
            { 
                txtValue.Text = ex.Message;
            }
        }

        private void btnSetColor_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Int32.Parse(txtX.Text);
                int y = Int32.Parse(txtY.Text);
                m_excel.SetCellColor("验证质检发货bar明细", x, y + 2);
            }
            catch (Exception ex)
            { 
                txtValue.Text = ex.Message;
            }
        }

    }
}
