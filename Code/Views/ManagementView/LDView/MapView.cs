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
    public partial class MapView : UserControl
    {
        public delegate bool Del_AddMap(int index, string path, string waferName, string productType, bool bcheck);
        public Del_AddMap m_DelAddMap;

        public delegate void Del_DeleteMap(int index);
        public Del_DeleteMap m_DelDeleteMap; 

        public delegate bool Del_ChangeRow();
        public Del_ChangeRow m_DelChangeRow;

        public MapView()
        {
            InitializeComponent();
        }

        private void MapView_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void UpdateData()
        {
            try
            {
                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                if(mapModel == null)
                {
                    mapModel = new MapModel();
                }
                var listMap = mapModel.ListMap;
                if(listMap != null && listMap.Count > 0)
                {
                    dataMapView.DataSource = null;
                    dataMapView.DataSource = listMap;
                }
                else
                {
                    dataMapView.DataSource = null;
                }

                dataMapView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataMapView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataMapView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dataMapView.Columns[0].HeaderText = "序号";
                dataMapView.Columns[1].HeaderText = "Wafer是否有效";
                dataMapView.Columns[2].HeaderText = "Wafer号";
                dataMapView.Columns[3].HeaderText = "路径";
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnCheckWafer_Click(object sender, EventArgs e)
        {
            try
            {
                bool bresult = m_DelAddMap(cmbIndex.SelectedIndex + 1, loadMapPath.FilePath, txtWaferNo.Text, cmbProduct.Text, false);
                if (!bresult)
                {
                    MessageBox.Show("Map表未包含Wafer号!!");
                    return;
                }
                else
                {
                    MessageBox.Show("Map表正确!!");
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                if(mapModel.ListMap == null)
                {
                    mapModel.ListMap = new List<MapModel.SetMap>();
                }
                 
                bool bresult = m_DelAddMap(cmbIndex.SelectedIndex + 1, loadMapPath.FilePath, txtWaferNo.Text, cmbProduct.Text, false);
                if(!bresult)
                {
                    MessageBox.Show("请检查Map表是否正确!!");
                    return;
                }

                int index = mapModel.ListMap.FindIndex(x => x.Index == cmbIndex.SelectedIndex + 1);
                if (index == -1)
                {
                    mapModel.ListMap.Add(new MapModel.SetMap()
                    {
                        Index = cmbIndex.SelectedIndex + 1,
                        WaferNo = txtWaferNo.Text,
                        WaferPath = loadMapPath.FilePath,
                        IsEffective = true,
                    });
                }
                else
                {
                    var setMap = mapModel.ListMap.FirstOrDefault(x => x.Index == cmbIndex.SelectedIndex + 1);
                    {
                        setMap.Index = cmbIndex.SelectedIndex + 1;
                        setMap.WaferNo = txtWaferNo.Text;
                        setMap.WaferPath = loadMapPath.FilePath;
                        setMap.IsEffective = true;
                    }
                } 

                UpdateData();

            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var selModel = dataMapView.SelectedRows[0].DataBoundItem as MapModel.SetMap;
                
                if(selModel == null)
                {
                    return;
                }

                int index = dataMapView.SelectedRows[0].Index; 

                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                mapModel.ListMap.Remove(selModel);

                m_DelDeleteMap(selModel.Index);

                UpdateData();

                if (index > 0)
                {
                    dataMapView.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 手动换排
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeRow_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("是否确认要手动换排?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    MessageBox.Show("用户取消");
                    return;
                }

                bool bvalue = m_DelChangeRow();
                MessageBox.Show("手动换排" + (bvalue ? "成功" : "失败"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void dataMapView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var selModel = dataMapView.SelectedRows[0].DataBoundItem as MapModel.SetMap;

                if (selModel == null)
                {
                    return;
                }

                txtWaferNo.Text = selModel.WaferNo;
                cmbIndex.SelectedIndex = selModel.Index - 1;
                loadMapPath.FilePath = selModel.WaferPath;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnMapTest_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelController.Form1 view = new ExcelController.Form1();
                view.Show();
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void RefreshData()
        {
            try
            {
                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                BeginInvoke(new Action(() =>
                {
                    txtAlreadyCount.Text = mapModel.GetCount.ToString();
                    txtCurrentOcr.Text = mapModel.CurrentOcr;
                    txtGetCount.Text = mapModel.BarCount.ToString();
                    txtSetBar.Text = mapModel.BarSet;
                    txtCurrentRow.Text = mapModel.CurrentRow.ToString();
                    UpdateData();
                }));

            }
            catch (Exception ex)
            {

            }
        }
    }
}
