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
using GlobalCore;

namespace ManagementView.MotorView
{
    public partial class AxisOffSetView : UserControl
    {
        private StationModel m_stationModel;
        public AxisOffSetView(StationModel stationModel)
        {
            InitializeComponent();
            m_stationModel = stationModel;
        }

        private void AxisOffSetView_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateData(m_stationModel);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 更新界面数据
        /// </summary>
        /// <param name="model"></param>
        private void UpdateData(StationModel model)
        {
            dataView.DataSource = model.PointModels;
            //dataPoint.Update();
            dataView.Refresh(); 

            dataView.Columns["Pos_Y"].Visible = model.AxisNum > 1;
            txtOffSetY.Enabled = model.AxisNum > 1;
            dataView.Columns["Pos_Z"].Visible = model.AxisNum > 2;
            txtOffSetZ.Enabled = model.AxisNum > 2;
            dataView.Columns["Pos_U"].Visible = model.AxisNum > 3;
            txtOffSetU.Enabled = model.AxisNum > 3;
            
            dataView.Columns["pos_X_Min"].Visible = false;
            dataView.Columns["pos_X_Max"].Visible = false;
            dataView.Columns["pos_Y_Min"].Visible = false;
            dataView.Columns["pos_Y_Max"].Visible = false;
            dataView.Columns["pos_Z_Min"].Visible = false;
            dataView.Columns["pos_Z_Max"].Visible = false;
            dataView.Columns["pos_U_Min"].Visible = false;
            dataView.Columns["pos_U_Max"].Visible = false;

            dataView.Columns["Pos_X"].HeaderText = "X";
            dataView.Columns["Pos_Y"].HeaderText = "Y";
            dataView.Columns["Pos_Z"].HeaderText = "Z";
            dataView.Columns["Pos_U"].HeaderText = "U";
            
            dataView.Columns["StationName"].HeaderText = "工站";
            dataView.Columns["StationName"].ReadOnly = true;
            

            dataView.Columns["Pos_X"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataView.Columns["Pos_Y"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataView.Columns["Pos_Z"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataView.Columns["Pos_U"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataView.Columns["StationName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            //操作员不能修改名称
            if (Global.UserName == Global.OperatorName)
            {
                dataView.Columns["Pos_X"].ReadOnly = true;
                dataView.Columns["Pos_Y"].ReadOnly = true;
                dataView.Columns["Pos_Z"].ReadOnly = true;
                dataView.Columns["Pos_U"].ReadOnly = true;
            }
            else
            {
                dataView.Columns["Pos_X"].ReadOnly = false;
                dataView.Columns["Pos_Y"].ReadOnly = false;
                dataView.Columns["Pos_Z"].ReadOnly = false;
                dataView.Columns["Pos_U"].ReadOnly = false;
            }

            dataView.Columns[14].ReadOnly = true;
            //ID不能修改
            dataView.Columns[13].ReadOnly = true;
            
        }


        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                string str = string.Format("确认修改序号 {0}-{1} 偏移量X:{2} Y:{3} Z:{4} U:{5} ??", txtStartIndex.Text, txtEndIndex.Text, txtOffSetX.Text, txtOffSetY.Text,
                    txtOffSetZ.Text, txtOffSetU.Text);
                var result = MessageBox.Show(str, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.No)
                {
                    return;
                }

                foreach (var item in m_stationModel.PointModels.FindAll(x => x.Id >= Int32.Parse(txtStartIndex.Text)
                && x.Id <= Int32.Parse(txtEndIndex.Text)))
                {
                    if(string.IsNullOrEmpty(item.Name))
                    {
                        continue;
                    }

                    if(m_stationModel.Axis_X != null)
                    {
                        item.Pos_X += double.Parse(txtOffSetX.Text);
                    }

                    if (m_stationModel.Axis_Y != null)
                    {
                        item.Pos_Y += double.Parse(txtOffSetY.Text);
                    }

                    if (m_stationModel.Axis_Z != null)
                    {
                        item.Pos_Z += double.Parse(txtOffSetZ.Text);
                    }

                    if (m_stationModel.Axis_U != null)
                    {
                        item.Pos_U += double.Parse(txtOffSetU.Text);
                    }
                }

                UpdateData(m_stationModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
