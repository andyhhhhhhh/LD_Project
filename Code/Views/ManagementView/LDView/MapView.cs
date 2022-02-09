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
        public MapView()
        {
            InitializeComponent();
        }

        private void MapView_Load(object sender, EventArgs e)
        {
            UpdateData();
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
                int index = mapModel.ListMap.FindIndex(x => x.Index == cmbIndex.SelectedIndex + 1);
                if(index == -1)
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

                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                mapModel.ListMap.Remove(selModel);

                UpdateData();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            try
            {
                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                MapModel mapModel = ldModel.mapModel;
                txtAlreadyCount.Text = mapModel.GetCount.ToString();
                txtCurrentOcr.Text = mapModel.CurrentOcr;
                txtGetCount.Text = mapModel.BarCount.ToString();
                txtSetBar.Text = mapModel.BarSet;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnChangeRow_Click(object sender, EventArgs e)
        {

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
    }
}
