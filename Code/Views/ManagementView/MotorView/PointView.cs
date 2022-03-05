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
using System.IO;
using XMLController;
using DevComponents.DotNetBar;
using MotionController;
using GlobalCore;
using System.Threading;
using ManagementView.Comment;

namespace ManagementView.MotorView
{
    public partial class PointView : UserControl
    {
        public string m_CardPath = Application.StartupPath + "//Sequence//Card//";
        public ControlCardModel m_CardModel = new ControlCardModel();
        public StationModel m_StationModel = new StationModel();
        IMotorControl m_MotroContorl = MotorInstance.GetInstance();
        bool m_bMointor = true;

        AxisMonitor[] m_axisMonitor = new AxisMonitor[4];
        
        public PointView()
        {
            InitializeComponent();
        }

        private void PointView_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(m_CardPath))
                {
                    Directory.CreateDirectory(m_CardPath);
                }
                m_CardPath += "Card.dsr";
                m_CardModel = XmlControl.controlCardModel;
                if(m_CardModel == null)
                {
                    m_CardModel = (ControlCardModel)XmlControl.LoadFromXml(m_CardPath, typeof(ControlCardModel));
                    if (m_CardModel == null)
                    {
                        m_CardModel = new ControlCardModel();
                    }
                }

                this.ParentForm.FormClosing += ParentForm_FormClosing;

                cmbStation.DataSource = m_CardModel.StationModels;
                cmbStation.DisplayMember = "Name";

                cmbAxisSpeed.DataSource = Enum.GetNames(typeof(AxisSpeed)); 

                EnableControl();

                for (int i = 0; i < m_axisMonitor.Count(); i++)
                {
                    m_axisMonitor[i] = new AxisMonitor();
                    m_axisMonitor[i].Index = i;
                    m_axisMonitor[i].m_DelAxisServo += SingleAxisServo;
                }
                CommHelper.LayoutChildFillView(panel_X, m_axisMonitor[0]);
                CommHelper.LayoutChildFillView(panel_Y, m_axisMonitor[1]);
                CommHelper.LayoutChildFillView(panel_Z, m_axisMonitor[2]);
                CommHelper.LayoutChildFillView(panel_U, m_axisMonitor[3]); 

                GetAxisPosThead();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置运动过程中不能操作轴
        /// </summary>
        private void EnableControl()
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    groupHome.Enabled = !Global.Run || Global.Pause || Global.Stop;
                    groupStep.Enabled = !Global.Run || Global.Pause || Global.Stop;
                }));
            }
            catch (Exception ex)
            {

            }
        }
        
        /// <summary>
        /// 工站选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_StationModel = cmbStation.SelectedItem as StationModel; 

                UpdateData(m_StationModel);

                panel_Y.Visible = m_StationModel.AxisNum > 1;
                panel_Z.Visible = m_StationModel.AxisNum > 2;
                panel_U.Visible = m_StationModel.AxisNum > 3;

                slider1_ValueChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新界面数据
        /// </summary>
        /// <param name="model"></param>
        private void UpdateData(StationModel model)
        {
            dataPoint.DataSource = model.PointModels;
            //dataPoint.Update();
            dataPoint.Refresh();
            RefreshBth(model);

            dataPoint.Columns["Pos_Y"].Visible = model.AxisNum > 1;
            dataPoint.Columns["Pos_Z"].Visible = model.AxisNum > 2;
            dataPoint.Columns["Pos_U"].Visible = model.AxisNum > 3;
            dataPoint.Columns["pos_Y_Min"].Visible = model.AxisNum > 1;
            dataPoint.Columns["pos_Y_Max"].Visible = model.AxisNum > 1;
            dataPoint.Columns["pos_Z_Min"].Visible = model.AxisNum > 3;
            dataPoint.Columns["pos_Z_Max"].Visible = model.AxisNum > 3;
            dataPoint.Columns["pos_U_Min"].Visible = model.AxisNum > 3;
            dataPoint.Columns["pos_U_Max"].Visible = model.AxisNum > 3;
            
            dataPoint.Columns["Pos_X"].HeaderText = "X";
            dataPoint.Columns["Pos_Y"].HeaderText = "Y";
            dataPoint.Columns["Pos_Z"].HeaderText = "Z";
            dataPoint.Columns["Pos_U"].HeaderText = "U";

            dataPoint.Columns["pos_X_Min"].HeaderText = "X下限";
            dataPoint.Columns["pos_X_Max"].HeaderText = "X上限";
            dataPoint.Columns["pos_Y_Min"].HeaderText = "Y下限";
            dataPoint.Columns["pos_Y_Max"].HeaderText = "Y上限";
            dataPoint.Columns["pos_Z_Min"].HeaderText = "Z下限";
            dataPoint.Columns["pos_Z_Max"].HeaderText = "Z上限";
            dataPoint.Columns["pos_U_Min"].HeaderText = "U下限";
            dataPoint.Columns["pos_U_Max"].HeaderText = "U上限";
            dataPoint.Columns["StationName"].HeaderText = "工站";
            dataPoint.Columns["StationName"].ReadOnly = true;

            //dataPoint.Columns["pos_X_Min"].Width =  50;
            //dataPoint.Columns["pos_X_Max"].Width = 50;
            //dataPoint.Columns["pos_Y_Min"].Width = 50;
            //dataPoint.Columns["pos_Y_Max"].Width = 60;
            //dataPoint.Columns["pos_Z_Min"].Width = 60;
            //dataPoint.Columns["pos_Z_Max"].Width = 60;
            //dataPoint.Columns["pos_U_Min"].Width = 60;
            //dataPoint.Columns["pos_U_Max"].Width = 60;

            dataPoint.Columns["Pos_X"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["Pos_Y"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["Pos_Z"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["Pos_U"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataPoint.Columns["pos_X_Min"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_X_Max"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_Y_Min"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_Y_Max"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_Z_Min"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_Z_Max"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_U_Min"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["pos_U_Max"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataPoint.Columns["StationName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //操作员不能修改名称
            if (Global.UserName == Global.OperatorName)
            {
                dataPoint.Columns["Pos_X"].ReadOnly = true;
                dataPoint.Columns["Pos_Y"].ReadOnly = true;
                dataPoint.Columns["Pos_Z"].ReadOnly = true;
                dataPoint.Columns["Pos_U"].ReadOnly = true;
            }
            else
            {
                dataPoint.Columns["Pos_X"].ReadOnly = false;
                dataPoint.Columns["Pos_Y"].ReadOnly = false;
                dataPoint.Columns["Pos_Z"].ReadOnly = false;
                dataPoint.Columns["Pos_U"].ReadOnly = false;
            }

            dataPoint.Columns[14].ReadOnly = Global.UserName != Global.EngineerName;
            //ID不能修改
            dataPoint.Columns[13].ReadOnly = true;

            toolInPutData.Enabled = Global.UserName == Global.EngineerName;
            toolOutPutData.Enabled = Global.UserName == Global.EngineerName;
            设置偏移ToolStripMenuItem.Enabled = Global.UserName == Global.EngineerName;
            清空数据ToolStripMenuItem.Enabled = Global.UserName == Global.EngineerName;
            DeletetoolStripMenuItem1.Enabled = Global.UserName == Global.EngineerName;

            slider1.Value = Int32.Parse((m_StationModel.PercentSpeed * 100).ToString());
        }

        /// <summary>
        /// 刷新按钮状态
        /// </summary>
        /// <param name="model"></param>
        private void RefreshBth(StationModel model)
        {
            try
            {
                btnY_Neg.Visible = model.AxisNum > 1;
                btnZ_Neg.Visible = model.AxisNum > 2;
                btnU_Neg.Visible = model.AxisNum > 3;
                btnY_Pos.Visible = model.AxisNum > 1;
                btnZ_Pos.Visible = model.AxisNum > 2;
                btnU_Pos.Visible = model.AxisNum > 3;

                btnY_Home.Visible = model.AxisNum > 1;
                btnZ_Home.Visible = model.AxisNum > 2;
                btnU_Home.Visible = model.AxisNum > 3;
                 
                txtY_Distance.Visible = model.AxisNum > 1;
                txtZ_Distance.Visible = model.AxisNum > 2;
                txtU_Distance.Visible = model.AxisNum > 3;

                txtY_Pos.Visible = model.AxisNum > 1;
                txtZ_Pos.Visible = model.AxisNum > 2;
                txtU_Pos.Visible = model.AxisNum > 3;

                lblY_Dis.Visible = model.AxisNum > 1;
                lblZ_Dis.Visible = model.AxisNum > 2;
                lblU_Dis.Visible = model.AxisNum > 3;

                lblY_Pos.Visible = model.AxisNum > 1;
                lblZ_Pos.Visible = model.AxisNum > 2;
                lblU_Pos.Visible = model.AxisNum > 3;

                if(model.Axis_X != null)
                {
                    btnX_Pos.Text = model.Axis_X.aliasName + "+";
                    btnX_Neg.Text = model.Axis_X.aliasName + "-";
                }
                if (model.Axis_Y != null)
                {
                    btnY_Pos.Text = model.Axis_Y.aliasName + "+";
                    btnY_Neg.Text = model.Axis_Y.aliasName + "-";
                }
                if (model.Axis_Z != null)
                {
                    btnZ_Pos.Text = model.Axis_Z.aliasName + "+";
                    btnZ_Neg.Text = model.Axis_Z.aliasName + "-";
                }
                if (model.Axis_U != null)
                {
                    btnU_Pos.Text = model.Axis_U.aliasName + "+";
                    btnU_Neg.Text = model.Axis_U.aliasName + "-";
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        #region 右键操作菜单
        private void 示教ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataPoint.SelectedRows.Count != 1)
                {
                    return;
                } 

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;

                var result = MessageBoxEx.Show(this, string.Format("确定示教点位[{0}]?", selModel.Name), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                dataPoint.SelectedRows[0].Cells["Pos_X"].Value = Double.Parse(txtX_Pos.Text);
                dataPoint.SelectedRows[0].Cells["Pos_Y"].Value = Double.Parse(txtY_Pos.Text);
                dataPoint.SelectedRows[0].Cells["Pos_Z"].Value = Double.Parse(txtZ_Pos.Text);
                dataPoint.SelectedRows[0].Cells["Pos_U"].Value = Double.Parse(txtU_Pos.Text);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GotoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                SingleAxisMove(false);

                if (dataPoint.SelectedRows.Count == 0)
                {
                    return;
                }

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;

                var result = MessageBoxEx.Show(this, string.Format("确定走点位[{0}]?", selModel.Name), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                if(!JudgeSafe())
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(selModel, MotorControlType.AxisMoveNotWait);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SingleAxisMove(true);

                if (dataPoint.SelectedRows.Count == 0)
                {
                    return;
                } 

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;

                var result = MessageBoxEx.Show(this, string.Format("确定走点位[{0}]?", selModel.Name), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                if (!JudgeSafe())
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(selModel, MotorControlType.AxisMove);
                if(!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SingleAxisMove(bool bwait)
        {
            try
            {
                if (dataPoint.SelectedCells.Count != 1)
                {
                    return;
                }

                int rowIndex = dataPoint.SelectedCells[0].RowIndex;
                int colIndex = dataPoint.SelectedCells[0].ColumnIndex;
                AxisParamModel axis = null;
                switch (colIndex)
                {
                    case 0:
                        axis = m_StationModel.Axis_X;
                        break;
                    case 1:
                        axis = m_StationModel.Axis_Y;
                        break;
                    case 2:
                        axis = m_StationModel.Axis_Z;
                        break;
                    case 3:
                        axis = m_StationModel.Axis_U;
                        break;
                    default:
                        break;
                }

                if(axis == null)
                {
                    return;
                }

                axis.pos = double.Parse(dataPoint.SelectedCells[0].Value.ToString());

                var result = MessageBoxEx.Show(this, string.Format("[{0}]确定走位置[{1}]?", axis.aliasName, axis.pos), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                if (!JudgeSafe())
                {
                    return;
                }

                if(bwait)
                {
                    var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisMove);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(resultModel.ErrorMessage);
                    }
                }
                else
                {
                    var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisMoveNotWait);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(resultModel.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        PointModel m_CopyModel = null;
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataPoint.SelectedRows.Count != 1)
                {
                    return;
                }
                m_CopyModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_CopyModel == null)
                {
                    return;
                }

                if (dataPoint.SelectedRows.Count != 1)
                {
                    return;
                }
                var model = dataPoint.SelectedRows[0].DataBoundItem as PointModel;
                model = m_CopyModel.Clone();

                var stationModel = cmbStation.SelectedItem as StationModel;

                int index = 1;
                string name = m_CopyModel.Name + "_" + index.ToString();
                int findIndex = stationModel.PointModels.FindIndex(x => x.Name == name);
                while(findIndex != -1)
                {
                    index++;
                    name = m_CopyModel.Name + "_" + index.ToString();
                    findIndex = stationModel.PointModels.FindIndex(x => x.Name == name);
                    if(findIndex == -1)
                    {
                        break;
                    }
                }
                model.Name = name;

                dataPoint.SelectedRows[0].Cells[14].Value = name;

                dataPoint.SelectedRows[0].Cells["Pos_X"].Value = model.Pos_X;
                dataPoint.SelectedRows[0].Cells["Pos_Y"].Value = model.Pos_Y;
                dataPoint.SelectedRows[0].Cells["Pos_Z"].Value = model.Pos_Z;
                dataPoint.SelectedRows[0].Cells["Pos_U"].Value = model.Pos_U;

                dataPoint.SelectedRows[0].Cells["pos_X_Min"].Value = model.pos_X_Min;
                dataPoint.SelectedRows[0].Cells["pos_Y_Min"].Value = model.pos_Y_Min;
                dataPoint.SelectedRows[0].Cells["pos_Z_Min"].Value = model.pos_Z_Min;
                dataPoint.SelectedRows[0].Cells["pos_U_Min"].Value = model.pos_U_Min;
                dataPoint.SelectedRows[0].Cells["pos_X_Max"].Value = model.pos_X_Max;
                dataPoint.SelectedRows[0].Cells["pos_Y_Max"].Value = model.pos_Y_Max;
                dataPoint.SelectedRows[0].Cells["pos_Z_Max"].Value = model.pos_Z_Max;
                dataPoint.SelectedRows[0].Cells["pos_U_Max"].Value = model.pos_U_Max;

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// <summary>
        /// 删除当前行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletetoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataPoint.SelectedRows.Count != 1)
                {
                    return;
                }

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;
                if (!string.IsNullOrEmpty(selModel.Name))
                {
                    return;
                }

                m_StationModel.PointModels.Remove(selModel);

                int id = 0;
                foreach (var item in m_StationModel.PointModels)
                {
                    item.Id = id;
                    id++;
                }

                m_StationModel.PointModels.Add(new PointModel()
                {
                    Id = m_StationModel.PointModels.Count,
                    StationName = selModel.StationName
                });

                UpdateData(m_StationModel);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataPoint.SelectedRows.Count != 1)
                {
                    return;
                }

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;
                var result = MessageBoxEx.Show(this, string.Format("确定在点位[{0}]下方插入位置?", selModel.Name), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                PointModel pointModel = new PointModel()
                {
                    Id = selModel.Id + 1,
                    StationName = selModel.StationName
                };

                foreach (var item in m_StationModel.PointModels)
                {
                    if(item.Id > selModel.Id)
                    {
                        item.Id++;
                    }
                }
                m_StationModel.PointModels.Insert(selModel.Id + 1, pointModel);
                if(m_StationModel.PointModels.Last().Name == null)
                { 
                    m_StationModel.PointModels.Remove(m_StationModel.PointModels.Last());
                }

                UpdateData(m_StationModel);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataPoint.SelectedRows.Count != 1)
                {
                    return;
                }

                var selModel = dataPoint.SelectedRows[0].DataBoundItem as PointModel;

                var result = MessageBoxEx.Show(this, string.Format("确定清空点位[{0}]?", selModel.Name), "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBoxEx.Show("取消操作!");
                    return;
                }

                dataPoint.SelectedRows[0].Cells[14].Value = "";

                dataPoint.SelectedRows[0].Cells["Pos_X"].Value = 0;
                dataPoint.SelectedRows[0].Cells["Pos_Y"].Value = 0;
                dataPoint.SelectedRows[0].Cells["Pos_Z"].Value = 0;
                dataPoint.SelectedRows[0].Cells["Pos_U"].Value = 0;

                dataPoint.SelectedRows[0].Cells["pos_X_Min"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_Y_Min"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_Z_Min"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_U_Min"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_X_Max"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_Y_Max"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_Z_Max"].Value = 0;
                dataPoint.SelectedRows[0].Cells["pos_U_Max"].Value = 0;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XmlControl.SaveToXml(m_CardPath, m_CardModel, typeof(ControlCardModel)); 
                XmlControl.controlCardModel = m_CardModel;

                MessageBoxEx.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void pointContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                pointContextMenuStrip.Enabled = !Global.Run || Global.Pause || Global.Stop;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 运动距离设置
        private void radio_Low_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtX_Distance.Text = "10";
                txtY_Distance.Text = "10";
                txtZ_Distance.Text = "10";
                txtU_Distance.Text = "10";

                txtX_Distance.Enabled = true;
                txtY_Distance.Enabled = true;
                txtZ_Distance.Enabled = true;
                txtU_Distance.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void radio_High_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtX_Distance.Text = "1000";
                txtY_Distance.Text = "1000";
                txtZ_Distance.Text = "1000";
                txtU_Distance.Text = "1000";

                txtX_Distance.Enabled = true;
                txtY_Distance.Enabled = true;
                txtZ_Distance.Enabled = true;
                txtU_Distance.Enabled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void radio_Mid_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtX_Distance.Text = "100";
                txtY_Distance.Text = "100";
                txtZ_Distance.Text = "100";
                txtU_Distance.Text = "100";

                txtX_Distance.Enabled = true;
                txtY_Distance.Enabled = true;
                txtZ_Distance.Enabled = true;
                txtU_Distance.Enabled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void radio_Continue_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtX_Distance.Text = "";
                txtY_Distance.Text = "";
                txtZ_Distance.Text = "";
                txtU_Distance.Text = "";

                txtX_Distance.Enabled = false;
                txtY_Distance.Enabled = false;
                txtZ_Distance.Enabled = false;
                txtU_Distance.Enabled = false;
            }
            catch (Exception ex)
            {

            }
        }

        public enum AxisSpeed
        {
            低速,
            中速,
            高速
        }
        #endregion
        
        #region 回原 
        private void btnX_Home_Click(object sender, EventArgs e)
        {
            try
            {
                if (!JudgeSafe())
                {
                    return;
                }

                //工站X回零
                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_X, MotorControlType.AxisGoHome);
                if (resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零成功", m_StationModel.Axis_X.Name));
                }
                else
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零失败", m_StationModel.Axis_X.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnY_Home_Click(object sender, EventArgs e)
        {
            try
            {
                if (!JudgeSafe())
                {
                    return;
                }

                //工站Y回零
                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Y, MotorControlType.AxisGoHome);
                if (resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零成功", m_StationModel.Axis_Y.Name));
                }
                else
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零失败", m_StationModel.Axis_Y.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnZ_Home_Click(object sender, EventArgs e)
        {
            try
            {
                //工站Z回零
                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Z, MotorControlType.AxisGoHome);
                if (resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零成功", m_StationModel.Axis_Z.Name));
                }
                else
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零失败", m_StationModel.Axis_Z.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnU_Home_Click(object sender, EventArgs e)
        {
            try
            {
                //工站U回零
                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_U, MotorControlType.AxisGoHome);
                if (resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零成功", m_StationModel.Axis_U.Name));
                }
                else
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零失败", m_StationModel.Axis_U.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStation_Home_Click(object sender, EventArgs e)
        {
            try
            {
                if (!JudgeSafe())
                {
                    return;
                }

                //工站回零
                var resultModel = m_MotroContorl.Run(m_StationModel, MotorControlType.AxisGoHome);
                if(resultModel.RunResult)
                {
                    MessageBoxEx.Show(string.Format("【{0}】回零成功", m_StationModel.Name));
                }
                else
                { 
                    MessageBoxEx.Show(string.Format("【{0}】回零失败", m_StationModel.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 步进Jog运动
        private void btnX_Neg_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtX_Distance.Text, m_StationModel.Axis_X, false); 
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnX_Pos_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtX_Distance.Text, m_StationModel.Axis_X, true);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnY_Neg_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtY_Distance.Text, m_StationModel.Axis_Y, false);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnY_Pos_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtY_Distance.Text, m_StationModel.Axis_Y, true);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnZ_Neg_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtZ_Distance.Text, m_StationModel.Axis_Z, false);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnZ_Pos_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtZ_Distance.Text, m_StationModel.Axis_Z, true);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnU_Neg_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtU_Distance.Text, m_StationModel.Axis_U, false);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnU_Pos_Click(object sender, EventArgs e)
        {
            try
            {
                MoveRel(txtU_Distance.Text, m_StationModel.Axis_U, true);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void MoveRel(string strDis, AxisParamModel axis, bool dir)
        {
            if (radio_Continue.Checked)
            {
                return;
            }
            double offset = Double.Parse(strDis);
            if(offset >= 10)
            {
                var result = MessageBoxEx.Show(this, string.Format("注意，请确认是否移动距离{0}mm", offset), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if(result == DialogResult.No)
                {
                    MessageBoxEx.Show("用户取消");
                    return;
                }
            }

            axis.vel = m_StationModel.PercentSpeed * axis.maxVel;

            axis.relPos = dir ? offset : -offset;
            var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisMoveRel);
            if (!resultModel.RunResult)
            {
                MessageBoxEx.Show("运动失败:" + resultModel.ErrorMessage);
            }
        }
        #endregion

        #region 步进连续运动
        private void btnX_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            try
            { 
                MoveJog(m_StationModel.Axis_X, 1);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnX_Neg_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                } 
                 
                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_X, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnX_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_X, 0);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnX_Pos_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_X, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnY_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_Y, 0);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnY_Pos_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Y, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnY_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_Y, 1);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnY_Neg_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Y, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnZ_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_Z, 1);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnZ_Neg_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Z, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnZ_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_Z, 0);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnZ_Pos_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Z, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnU_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            try
            { 
                MoveJog(m_StationModel.Axis_U, 1);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnU_Neg_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_U, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void btnU_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MoveJog(m_StationModel.Axis_U, 0); 
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        private void btnU_Pos_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }

                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_U, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void MoveJog(AxisParamModel axis, int dir)
        {
            try
            {
                if (!radio_Continue.Checked)
                {
                    return;
                }
                axis.dir = dir;
                axis.vel = axis.maxVel * m_StationModel.PercentSpeed;
                var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisMoveJog);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show(resultModel.ErrorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        #endregion

        string m_BeginValue = "";
        private void dataPoint_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.ColumnIndex == 14)
                {
                    var str = dataPoint.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if(str != null)
                    {
                        m_BeginValue = str.ToString();
                    }
                    else
                    {
                        m_BeginValue = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataPoint_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1 || e.ColumnIndex != 14)
                {
                    return;
                }

                var str = dataPoint.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (str == null)
                {
                    return;
                }
                string strvalue = str.ToString();

                if (e.ColumnIndex == 14)
                {
                    if ((m_StationModel.PointModels.FindAll(x => x.Name == strvalue).Count) != 1)
                    {
                        MessageBoxEx.Show(this, "已存在此名称，请更改!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataPoint.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginValue;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置整体运行速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                lblSpeed.Text = slider1.Value.ToString() + "%";

                m_StationModel.PercentSpeed = (double) slider1.Value / 100;  
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAxisSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                double percent = 0.5;
                if(cmbAxisSpeed.SelectedIndex == 0)
                {
                    percent = 30;
                }
                else if (cmbAxisSpeed.SelectedIndex == 1)
                { 
                    percent = 60;
                }
                else if (cmbAxisSpeed.SelectedIndex == 2)
                { 
                    percent = 90;
                }

                //设置X速度
                if (m_StationModel.Axis_X == null)
                {
                    return;
                }
                m_StationModel.Axis_X.vel = (percent / 100) * m_StationModel.Axis_X.maxVel;

                //设置Y速度
                if (m_StationModel.Axis_Y == null)
                {
                    return;
                }
                m_StationModel.Axis_Y.vel = (percent / 100) * m_StationModel.Axis_Y.maxVel;

                //设置Z速度
                if (m_StationModel.Axis_Z == null)
                {
                    return;
                }
                m_StationModel.Axis_Z.vel = (percent / 100) * m_StationModel.Axis_Z.maxVel;

                //设置U速度
                if (m_StationModel.Axis_U == null)
                {
                    return;
                }
                m_StationModel.Axis_U.vel = (percent / 100) * m_StationModel.Axis_U.maxVel;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取轴位置线程
        /// </summary>
        private void GetAxisPosThead()
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        while (m_bMointor)
                        { 
                            EnableControl();

                            //获取X位置
                            if (m_StationModel.Axis_X != null)
                            {
                                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_X, MotorControlType.AxisStatus);
                                if (resultModel.RunResult)
                                {
                                    AxisStatus status = resultModel.ObjectResult as AxisStatus;
                                    if (status != null)
                                    {
                                        BeginInvoke(new Action(() =>
                                        {
                                            txtX_Pos.Text = (status.Acs / m_StationModel.Axis_X.stepvalue).ToString("0.000"); 
                                            SetStatus(status);
                                        }));
                                        m_axisMonitor[0].SetStatus(status);
                                    }
                                    else
                                    {
                                        BeginInvoke(new Action(() =>
                                        {
                                            txtX_Pos.Text = "NONE";
                                        }));
                                    }
                                }
                            }

                            //获取Y位置
                            if (m_StationModel.Axis_Y != null)
                            {
                                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Y, MotorControlType.AxisStatus);
                                if (resultModel.RunResult)
                                {
                                    AxisStatus status = resultModel.ObjectResult as AxisStatus;
                                    if (status != null)
                                    {
                                        txtY_Pos.Invoke(new Action(() =>
                                        {
                                            txtY_Pos.Text = (status.Acs / m_StationModel.Axis_Y.stepvalue).ToString("0.000"); 
                                            SetStatus(status);
                                        }));
                                        m_axisMonitor[1].SetStatus(status);
                                    }
                                    else
                                    {
                                        BeginInvoke(new Action(() =>
                                        {
                                            txtY_Pos.Text = "NONE";
                                        }));
                                    }
                                }
                            }

                            //获取Z位置
                            if (m_StationModel.Axis_Z != null)
                            {
                                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_Z, MotorControlType.AxisStatus);
                                if (resultModel.RunResult)
                                {
                                    AxisStatus status = resultModel.ObjectResult as AxisStatus;
                                    if (status != null)
                                    {
                                        txtZ_Pos.Invoke(new Action(() =>
                                        {
                                            txtZ_Pos.Text = (status.Acs / m_StationModel.Axis_Z.stepvalue).ToString("0.000");
                                            SetStatus(status);
                                        }));
                                        m_axisMonitor[2].SetStatus(status);
                                    }
                                    else
                                    {
                                        BeginInvoke(new Action(() =>
                                        {
                                            txtZ_Pos.Text = "NONE";
                                        }));
                                    }
                                }
                            }

                            //获取Z位置
                            if (m_StationModel.Axis_U != null)
                            {
                                var resultModel = m_MotroContorl.Run(m_StationModel.Axis_U, MotorControlType.AxisStatus);
                                if (resultModel.RunResult)
                                {
                                    AxisStatus status = resultModel.ObjectResult as AxisStatus;
                                    if (status != null)
                                    {
                                        txtU_Pos.Invoke(new Action(() =>
                                        {
                                            txtU_Pos.Text = (status.Acs / m_StationModel.Axis_U.stepvalue).ToString("0.000");
                                            SetStatus(status);
                                        }));
                                        m_axisMonitor[3].SetStatus(status);
                                    }
                                    else
                                    {
                                        BeginInvoke(new Action(() =>
                                        {
                                            txtU_Pos.Text = "NONE";
                                        }));
                                    }
                                }
                            }

                            Thread.Sleep(200);
                        }
                    }
                    catch (Exception)
                    { 
                    }  
                }));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        /// <summary>
        /// 窗口关闭执行动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_bMointor = false;
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {

            }
        }

        //使能
        private void btnEnable_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_MotroContorl.Run(m_StationModel, MotorControlType.AxisEnable);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show("使能失败");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //断使能
        private void btnDisable_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_MotroContorl.Run(m_StationModel, MotorControlType.AxisDisable);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show("断使能失败");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //停止运动
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_MotroContorl.Run(m_StationModel, MotorControlType.AxisStop);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show("停止失败");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //轴复位
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var resultModel = m_MotroContorl.Run(m_StationModel, MotorControlType.AxisReset);
                if (!resultModel.RunResult)
                {
                    MessageBoxEx.Show("复位失败");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        } 
     
        //设置工站状态
        private void SetStatus(AxisStatus status)
        {
            try
            {
                lblStatus.Text = status.inited ? "初始化" : "未初始化";
                if (!status.inited)
                {
                    return;
                }

                lblStatus.Text = status.warning ? "报警" : "未报警";
                if(status.warning)
                {
                    return;
                }

                lblStatus.Text = status.enabled ? "使能" : "未使能";
                if(!status.enabled)
                {
                    return;
                }

                if(!status.homed)
                {
                    lblStatus.Text = "未回零"; 
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        
        private void 设置偏移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new AxisOffSetView(m_StationModel), "偏移设置");
                view.ShowDialog();

                UpdateData(m_StationModel);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 单轴使能委托
        /// </summary>
        /// <param name="index">轴Index</param>
        /// <param name="bservo">true-使能 false-禁能</param>
        private void SingleAxisServo(int index, bool bservo)
        {
            try
            {
                AxisParamModel axis = new AxisParamModel();
                switch (index)
                {
                    case 0:
                        axis = m_StationModel.Axis_X;
                        break;
                    case 1:
                        axis = m_StationModel.Axis_Y;
                        break;
                    case 2:
                        axis = m_StationModel.Axis_Z;
                        break;
                    case 3:
                        axis = m_StationModel.Axis_U;
                        break;

                    default:
                        axis = m_StationModel.Axis_X;
                        break;
                }

                if (bservo)
                {
                    var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisEnable);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(axis.aliasName + " 使能失败!");
                    }
                }
                else
                {
                    var resultModel = m_MotroContorl.Run(axis, MotorControlType.AxisDisable);
                    if (!resultModel.RunResult)
                    {
                        MessageBoxEx.Show(axis.aliasName + " 断使能失败!");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        #region 判断位置安全
        /// <summary>
        /// 判断位置安全
        /// </summary>
        /// <returns></returns>
        private bool JudgeSafe()
        {
            try
            {
                bool isJudgeSafe = cmbStation.Text.Contains(MotionParam.Station_PrePare);//是否需要判断Z轴  
                if (isJudgeSafe)
                {
                    if (!JudgeUpZSafe())
                    {
                        MessageBoxEx.Show("顶针模组Z轴未在安全位!");
                        return false;
                    }
                }

                isJudgeSafe = cmbStation.Text.Contains(MotionParam.Station_Camera);//是否需要判断Z轴    
                if (isJudgeSafe)
                {
                    if (!JudgeLoadXSafe())
                    {
                        MessageBoxEx.Show("上料模组X未在安全位!");
                        return false;
                    }
                }

                isJudgeSafe = cmbStation.Text.Contains(MotionParam.Station_LoadX);//是否需要判断Z轴    
                if (isJudgeSafe)
                {
                    if (!JudgeCameraSafe())
                    {
                        MessageBoxEx.Show("双目相机未在安全位!");
                        return false;
                    }

                    if (!JudgeUnLoadXSafe())
                    {
                        MessageBoxEx.Show("下料模组X未在安全位!");
                        return false;
                    }

                    if (!JudgeLoadZSafe())
                    {
                        MessageBoxEx.Show("上料模组Z未在安全位!");
                        return false;
                    }
                }

                isJudgeSafe = cmbStation.Text.Contains(MotionParam.Station_UnLoad);//是否需要判断Z轴    
                if (isJudgeSafe)
                {
                    if (!JudgeLoadXSafe(false))
                    {
                        MessageBoxEx.Show("上料模组X未在安全位!");
                        return false;
                    }

                    if (!JudgeUnLoadZSafe())
                    {
                        MessageBoxEx.Show("下料模组Z未在安全位!");
                        return false;
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断顶升模组是否在安全位
        /// </summary>
        /// <returns></returns>
        private bool JudgeUpZSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue > pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断上料模组X是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeLoadXSafe(bool bLeft = true)
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_LoadX);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (bLeft)
                {
                    if (dvalue < pointModel.Pos_X - 0.5)
                    {
                        return false;
                    }
                }
                else
                {
                    if (dvalue > pointModel.Pos_X + 0.5)
                    {
                        return false;
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断双目相机是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeCameraSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Camera);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue > pointModel.Pos_X + 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断下料模组X是否在安全位置
        /// </summary>
        /// <returns></returns>
        public bool JudgeUnLoadXSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());
                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断下料模组Z是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeUnLoadZSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断上料模组Z是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeLoadZSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_LoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
        
        #region 导入导出数据
        //导出数据
        private void toolOutPutData_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string strPath = folderDialog.SelectedPath + "//" + m_StationModel.Name + ".csv";

                    DataSet ds = GetDataSetFromDataGridView(dataPoint, m_StationModel.Name);
                    Export2CSV(ds, m_StationModel.Name, true, strPath);

                    MessageBoxEx.Show("导出成功到文件：" + strPath);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //导入数据
        private void toolInPutData_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<PointModel> listPoint = new List<PointModel>();
                    string filePath = openFileDialog.FileName;

                    using (var sr = new StreamReader(filePath, Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            string strline = sr.ReadLine();
                            if (string.IsNullOrEmpty(strline))
                            {
                                continue;
                            }

                            string[] strArr = strline.Split(',');
                            if (strArr.Length < 15 || strArr[2].Contains("X"))
                            {
                                continue;
                            }

                            PointModel pointModel = new PointModel();

                            pointModel.Id = int.Parse(strArr[0]);
                            pointModel.Name = strArr[1];
                            pointModel.Pos_X = double.Parse(strArr[2]);
                            pointModel.Pos_Y = double.Parse(strArr[3]);
                            pointModel.Pos_Z = double.Parse(strArr[4]);
                            pointModel.Pos_U = double.Parse(strArr[5]);
                            pointModel.pos_X_Min = double.Parse(strArr[6]);
                            pointModel.pos_X_Max = double.Parse(strArr[7]);
                            pointModel.pos_Y_Min = double.Parse(strArr[8]);
                            pointModel.pos_Y_Max = double.Parse(strArr[9]);
                            pointModel.pos_Z_Min = double.Parse(strArr[10]);
                            pointModel.pos_Z_Max = double.Parse(strArr[11]);
                            pointModel.pos_U_Min = double.Parse(strArr[12]);
                            pointModel.pos_U_Max = double.Parse(strArr[13]);
                            pointModel.StationName = strArr[14];

                            listPoint.Add(pointModel);
                        }
                    }

                    m_StationModel.PointModels = listPoint;
                    UpdateData(m_StationModel);

                    MessageBoxEx.Show("导入成功");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        public static DataSet GetDataSetFromDataGridView(DataGridView datagridview, string name)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //为了把Id与Name显示在最开头
            int index = 13;
            for (int j = index; j < datagridview.Columns.Count; j++)
            {
                dt.Columns.Add(datagridview.Columns[j].HeaderCell.Value.ToString());
            }
            for (int j = 0; j < index; j++)
            {
                dt.Columns.Add(datagridview.Columns[j].HeaderCell.Value.ToString());
            }

            for (int j = 0; j < datagridview.Rows.Count; j++)
            {
                DataRow dr = dt.NewRow();
                //先把不是ID和Name的赋值
                for (int i = 0; i < datagridview.Columns.Count - 2; i++)
                {
                    if (datagridview.Rows[j].Cells[i].Value != null)
                    {
                        dr[i + 2] = datagridview.Rows[j].Cells[i].Value.ToString();
                    }
                    else
                    {
                        dr[i + 2] = "";
                    }
                }
                //再把ID与Name赋值
                for (int i = index; i < datagridview.Columns.Count; i++)
                {
                    if (datagridview.Rows[j].Cells[i].Value != null)
                    {
                        dr[i - index] = datagridview.Rows[j].Cells[i].Value.ToString();
                    }
                    else
                    {
                        dr[i - index] = "";
                    }
                }

                dt.Rows.Add(dr);
            }
            dt.TableName = name;
            ds.Tables.Add(dt);

            return ds;
        }

        //将DataSet转换成CSV文件
        public static void Export2CSV(DataSet ds, string tableName, bool containColumName, string fileName)
        {
            string csvStr = ConverDataSet2CSV(ds, tableName, containColumName);
            if (csvStr == "")
                return;

            OutPutCSV(fileName, csvStr);
        }

        public static void OutPutCSV(string filePath, string dataStr)
        {
            try
            {
                //目录不存在则创建
                string path = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                StreamWriter fileWriter;
                fileWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312"));

                if ("" != dataStr)
                {
                    fileWriter.Write(dataStr);
                }

                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 将指定的数据集中指定的表转换成CSV字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static string ConverDataSet2CSV(DataSet ds, string tableName, bool containColumName)
        {
            //首先判断数据集中是否包含指定的表
            if (ds == null || !ds.Tables.Contains(tableName))
            {
                MessageBox.Show("指定的数据集为空或不包含要写出的数据表！", "系统提示：", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "";
            }
            string csvStr = "";
            //下面写出数据
            DataTable tb = ds.Tables[tableName];

            //第一步：写出列名
            if (containColumName)
            {
                foreach (DataColumn column in tb.Columns)
                {
                    csvStr += column.ColumnName+ ",";
                }
                //去掉最后一个","
                csvStr = csvStr.Remove(csvStr.LastIndexOf(","), 1);
                csvStr += "\n";
            }

            //第二步：写出数据
            foreach (DataRow row in tb.Rows)
            {
                foreach (DataColumn column in tb.Columns)
                {
                    csvStr += row[column].ToString()+ ",";
                }
                csvStr = csvStr.Remove(csvStr.LastIndexOf(","), 1);
                csvStr += "\n";
            }
            return csvStr;
        }

        #endregion

    }
}
