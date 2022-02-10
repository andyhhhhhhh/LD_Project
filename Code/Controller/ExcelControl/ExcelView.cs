
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
        OleExcel m_ole = new OleExcel();
        ExcelEdit m_excel = new ExcelEdit();

        DataSet m_data = new DataSet();
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

                //m_excel.Open(txtPath.Text); 
                m_data = m_ole.getData(txtPath.Text);
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
                //DataSet来读取Excel
                DataTable dt = m_data.Tables[0];//假设dt是由"SELECT C1,C2,C3 FROM T1"查询出来的结果
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string strWafer = dt.Rows[i]["Wafer"].ToString();
                //    if (strWafer == txtWafer.Text)//查询条件
                //    {
                //        //进行操作
                //    }
                //}
                //但这种做法用一两次还好说，用多了就累了。那有没有更好的方法呢？就是dt.Select()，上面的操作可以改成这样：
                string strSel = string.Format("Wafer='{0}'", txtWafer.Text);
                DataRow[] drArr = dt.Select(strSel);//查询(如果Select内无条件，就是查询所有的数据)

                List<Bar> listBar = new List<Bar>();
                for (int i = 0; i < drArr.Length; i++)
                {
                    Bar bar = new Bar();
                    bar.barId = Int32.Parse(drArr[i].ItemArray[2].ToString());

                    List<Product> listProduct = new List<Product>();
                    for (int j = 1; j <= 54; j++)
                    {
                        Product product = new Product();
                        product.id = j;
                        string ocr = bar.barId.ToString().PadLeft(2, '0') + j.ToString().PadLeft(2, '0');
                        product.productOcr = Int32.Parse(ocr);
                        var reclimer = drArr[i].ItemArray[j + 2];
                        if(reclimer == null)
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


                txtValue.Text = "";
                for (int i = 0; i < listBar.Count; i++)
                {
                    string str = ""; 
                    str += listBar[i].barId + ","; 

                    int proLength = listBar[i].product.Length;
                    for (int j = 0; j < proLength; j++)
                    {
                        str += listBar[i].product[j].productOcr + ",";
                    }

                    txtValue.AppendText(str + Environment.NewLine);
                }

                return;

                

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
