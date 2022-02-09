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
    public partial class EnteringQRView : UserControl
    {
        public EnteringQRView()
        {
            InitializeComponent();
        }

        private void EnteringQRView_Load(object sender, EventArgs e)
        {
            InitData();
            UpdateData();
            SetUserEnable();
        }

        private void InitData()
        {
            try
            {
                for (int i = 1; i < 25; i++)
                {
                    cmbIndex.Items.Add(i.ToString());
                }
            }
            catch (Exception ex)
            {
                 
            }
        } 

        public void UpdateData()
        {
            try
            {
                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.enterQRModels;
                if (listModel == null || listModel.Count == 0)
                {
                    return;
                }

                int index = 1;
                foreach (var item in listModel)
                {
                    item.Index = index;
                    index++;
                }

                dataQR.DataSource = null;
                dataQR.DataSource = listModel;

                dataQR.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataQR.Columns[0].HeaderText = "序号";
                dataQR.Columns[1].HeaderText = "一号二维码";
                dataQR.Columns[2].HeaderText = "位数";
                dataQR.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataQR.Columns[3].HeaderText = "二号二维码";
                dataQR.Columns[4].HeaderText = "位数";
                dataQR.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 新增一行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewQR_Click(object sender, EventArgs e)
        {
            try
            {
                var listModel = XMLController.XmlControl.sequenceModelNew.LDModel.enterQRModels;
                if(listModel == null)
                {
                    listModel = new List<SequenceTestModel.EnterQRModel>(); 
                }

                if(listModel.FindIndex(x=>x.QR1 == txtQR_1.Text) != -1 && !string.IsNullOrEmpty(txtQR_1.Text))
                {
                    MessageBox.Show("已存在一号二维码:" + txtQR_1.Text);
                    return;
                }

                if (listModel.FindIndex(x => x.QR2 == txtQR_2.Text) != -1 && !string.IsNullOrEmpty(txtQR_2.Text))
                {
                    MessageBox.Show("已存在二号二维码:" + txtQR_2.Text);
                    return;
                }

                if (listModel.Count > 23)
                {
                    MessageBox.Show("已经到最大值!");
                    return;
                }

                if(txtQR_1.Text.Length != numQR1Num.Value)
                {
                    MessageBox.Show(string.Format("一号二维码位数{0}不等于{1} ", txtQR_1.Text.Length, numQR1Num.Value));
                    return;
                }
                if (txtQR_2.Text.Length != numQR2Num.Value)
                {
                    MessageBox.Show(string.Format("二号二维码位数{0}不等于{1} ", txtQR_2.Text.Length, numQR2Num.Value));
                    return;
                }

                listModel.Add(new SequenceTestModel.EnterQRModel()
                {
                    Index = Int32.Parse(cmbIndex.Text),
                    QR1 = txtQR_1.Text,
                    QR2 = txtQR_2.Text,
                    QR1Num = numQR1Num.Value,
                    QR2Num = numQR2Num.Value,
                });

                UpdateData();
                if(cmbIndex.SelectedIndex < 24)
                {
                    cmbIndex.SelectedIndex += 1;
                }
            }
            catch (Exception ex)
            {
                 
            }
        } 

        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataQR.SelectedRows.Count == 0)
                {
                    MessageBox.Show("未选中行");
                    return;
                }
                var selModel = dataQR.SelectedRows[0].DataBoundItem as EnterQRModel;
                if (selModel == null)
                {
                    return;
                }

                XMLController.XmlControl.sequenceModelNew.LDModel.enterQRModels.Remove(selModel);
                UpdateData();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 更新当前行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataQR.SelectedRows.Count == 0)
                {
                    MessageBox.Show("未选中行");
                    return;
                }
                var selModel = dataQR.SelectedRows[0].DataBoundItem as EnterQRModel;
                if(selModel == null)
                {
                    return;
                }

                if (txtQR_1.Text.Length != numQR1Num.Value)
                {
                    MessageBox.Show(string.Format("一号二维码位数{0}不等于{1} ", txtQR_1.Text.Length, numQR1Num.Value));
                    return;
                }
                if (txtQR_2.Text.Length != numQR2Num.Value)
                {
                    MessageBox.Show(string.Format("二号二维码位数{0}不等于{1} ", txtQR_2.Text.Length, numQR2Num.Value));
                    return;
                }

                selModel.QR1 = txtQR_1.Text;
                selModel.QR2 = txtQR_2.Text;
                selModel.QR1Num = numQR1Num.Value;
                selModel.QR2Num = numQR2Num.Value;

                dataQR.Refresh();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            { 
                XMLController.XmlControl.sequenceModelNew.LDModel.enterQRModels.Clear(); 
                dataQR.DataSource = null;
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataQR_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataQR.SelectedRows.Count == 0)
                {
                    return;
                }
                var selModel = dataQR.SelectedRows[0].DataBoundItem as EnterQRModel;
                if (selModel == null)
                {
                    return;
                }

                cmbIndex.Text = selModel.Index.ToString();
                txtQR_1.Text = selModel.QR1;
                txtQR_2.Text = selModel.QR2;
                numQR1Num.Value = selModel.QR1Num;
                numQR2Num.Value = selModel.QR2Num;
                SetUserEnable();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 序号切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Int32.Parse(cmbIndex.Text);
                if(dataQR.RowCount >= index)
                {
                    dataQR.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 双击清空控件数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQR_1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                txtQR_1.Clear();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 双击清空控件数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQR_2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                txtQR_2.Clear();
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
                numQR1Num.Enabled = bEnable;
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
