using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;
using SequenceTestModel;
using DevComponents.DotNetBar;
using GlobalCore;

namespace ManagementView._3DViews
{
    public partial class ProductSelView : UserControl
    {
        public ProductSelView()
        {
            InitializeComponent();
        }

        private void ProductSelView_Load(object sender, EventArgs e)
        {
            UpdateData();

            if(!string.IsNullOrEmpty(Global.ProductInfo))
            {
                lblProduct.Text = Global.ProductInfo;
            }
        }

        private void UpdateData()
        {
            try
            {
                var listProduct = XmlControl.sequenceModelNew.ProductSelModels;

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("Id", "序号");
                dataGridView1.Columns.Add("Name", "名称");
                dataGridView1.Columns.Add("Radius", "半径");
                dataGridView1.Columns.Add("Min", "最小值");
                dataGridView1.Columns.Add("Max", "最大值");
                dataGridView1.Columns.Add("Description", "描述");

                int i = 0;
                foreach (var item in listProduct)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = item.Id;
                    dataGridView1.Rows[i].Cells[1].Value = item.Name;
                    dataGridView1.Rows[i].Cells[2].Value = item.Radius;
                    dataGridView1.Rows[i].Cells[3].Value = item.MinValue;
                    dataGridView1.Rows[i].Cells[4].Value = item.MaxValue;
                    dataGridView1.Rows[i].Cells[5].Value = item.Description;

                    dataGridView1.Rows[i].ReadOnly = true;

                    i++;
                }

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            }
            catch (Exception ex)
            {

            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                string strvalue = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                ProductSelModel tModel = XmlControl.sequenceModelNew.ProductSelModels.FirstOrDefault(x => x.Name == strvalue);
                
                var listProduct = XmlControl.sequenceModelNew.ProductSelModels.FindAll(x=>x.Name != strvalue).ToList();
                if (listProduct.FindIndex(x => x.Name == txtName.Text) != -1)
                {
                    MessageBoxEx.Show("已存在此产品名称");
                    return;
                }

                tModel.Name = txtName.Text;
                tModel.MinValue = Double.Parse(numMinValue.sText);
                tModel.MaxValue = Double.Parse(numMaxValue.sText);
                tModel.Radius = Double.Parse(numRadius.sText);
                tModel.Description = txtDescription.Text;

                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var listProduct = XmlControl.sequenceModelNew.ProductSelModels;

                if (listProduct.FindIndex(x => x.Name == txtName.Text) != -1)
                {
                    MessageBoxEx.Show("已存在此产品名称");
                    return;
                }

                listProduct.Add(new ProductSelModel()
                {
                    Name = txtName.Text,
                    MinValue = Double.Parse(numMinValue.sText),
                    MaxValue = Double.Parse(numMaxValue.sText),
                    Radius = Double.Parse(numRadius.sText),
                    Description = txtDescription.Text
                });

                int id = 0;
                foreach (var item in listProduct)
                {
                    item.Id = id++;
                }

                UpdateData();

                MessageBoxEx.Show("修改完成!!!");
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strvalue = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                ProductSelModel tModel = XmlControl.sequenceModelNew.ProductSelModels.FirstOrDefault(x => x.Name == strvalue);

                var listProduct = XmlControl.sequenceModelNew.ProductSelModels;
                listProduct.Remove(tModel);

                int id = 0;
                foreach (var item in listProduct)
                {
                    item.Id = id++;
                }

                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBoxEx.Show("未选中产品行...");
                    return;
                }

                Global.ProductInfo = txtName.Text;
                XmlControl.sequenceModelNew.ProductInfo = Global.ProductInfo;

                lblProduct.Text = Global.ProductInfo;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string strvalue = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                ProductSelModel tModel = XmlControl.sequenceModelNew.ProductSelModels.FirstOrDefault(x => x.Name == strvalue);

                txtName.Text = tModel.Name;
                numMinValue.sText = tModel.MinValue.ToString();
                numMaxValue.sText = tModel.MaxValue.ToString();
                numRadius.sText = tModel.Radius.ToString();
                txtDescription.Text = tModel.Description;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
