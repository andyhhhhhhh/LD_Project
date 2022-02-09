using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SequenceTestModel;

namespace ManagementView
{
    public partial class ParamConfigView : UserControl
    {
        public static event EventHandler<object> ConfirmEvent;
        protected void OnConfirmEvent(object e)
        {
            EventHandler<object> handler = ConfirmEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public ParamConfigView()
        {
            InitializeComponent();
        }

        private void ParamConfigView_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateData();
                SetUserEnable();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var selModel = cmbProduct.SelectedItem as ParamRangeModel;
                if (selModel == null)
                {
                    return;
                }

                RefreshData(selModel);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var rangeModel = cmbProduct.SelectedItem as ParamRangeModel;
                if (rangeModel == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("名称为空");
                    return;
                }

                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.paramRangeModels.FindAll(x => x.Name != rangeModel.Name).ToList();

                if (listModel.FindIndex(x => x.Name == txtName.Text) != -1)
                {
                    MessageBox.Show("已存在名称" + txtName.Text);
                    return;
                }


                rangeModel.Name = txtName.Text;
                
                rangeModel.OcrNum = numOcrNum.Value;

                OnConfirmEvent(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshData(ParamRangeModel rangeModel)
        {
            try
            {
                txtName.Text = rangeModel.Name;
                
                numOcrNum.Value = rangeModel.OcrNum;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 新增产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.paramRangeModels;
                if(listModel == null)
                {
                    listModel = new List<ParamRangeModel>();
                }

                if(string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("名称为空");
                    return;
                }

                if(listModel.FindIndex(x=>x.Name == txtName.Text) != -1)
                {
                    MessageBox.Show("已存在名称" + txtName.Text);
                    return;
                }               

                ParamRangeModel rangeModel = new ParamRangeModel();
                rangeModel.Name = txtName.Text;
                rangeModel.OcrNum = numOcrNum.Value;

                listModel.Add(rangeModel);

                MessageBox.Show(string.Format("新建产品[{0}]成功!", txtName.Text));
                UpdateData();
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("确认删除产品" + txtName.Text, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.paramRangeModels;
                var selModel = cmbProduct.SelectedItem as ParamRangeModel;
                if (selModel == null)
                {
                    return;
                }

                listModel.Remove(selModel);
                UpdateData();
                MessageBox.Show("删除产品成功");
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 产品切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selModel = cmbProduct.SelectedItem as ParamRangeModel;
                if(selModel == null)
                {
                    return;
                }

                RefreshData(selModel);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void UpdateData()
        {
            try
            {
                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.paramRangeModels;
                if (listModel == null)
                {
                    listModel = new List<ParamRangeModel>();
                    cmbProduct.DataSource = null;
                }
                else
                {
                    cmbProduct.DataSource = null;
                    cmbProduct.DataSource = listModel;
                    cmbProduct.DisplayMember = "Name";
                }

                if(listModel.FirstOrDefault(x=>x.Name == XMLController.XmlControl.sequenceModelNew.ProductInfo) == null)
                {
                    return;
                }

                cmbProduct.Text = XMLController.XmlControl.sequenceModelNew.ProductInfo;
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void SetUserEnable()
        {
            try
            {
                bool bEnable = GlobalCore.Global.UserName != GlobalCore.Global.OperatorName;
                btnNewProduct.Enabled = bEnable;
                btnDelProduct.Enabled = bEnable;
                btnSave.Enabled = bEnable;
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
