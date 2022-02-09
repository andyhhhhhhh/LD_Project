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
using XMLController;
using GlobalCore;
using DevComponents.DotNetBar;

namespace ManagementView._3DViews
{
    public partial class GlobalVariableView : UserControl
    {
        public GlobalVariableView()
        {
            InitializeComponent();
        }

        private void GlobalVariableView_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        #region 添加变量按钮
        private void btnAddDouble_Click(object sender, EventArgs e)
        {
            SequenceModel sequece = XmlControl.sequenceModelNew;
            string strName = SetName(sequece);

            int count = sequece.VariableSetModels.Count;

            sequece.VariableSetModels.Add(new VariableSetModel()
            {
                Id = count + 1,
                Name = strName,
                variableType = VariableType.Double,
                Expression = "",
                Description = "",
                TestResult = ""
            });

            UpdateData();
        }

        private void btnAddBool_Click(object sender, EventArgs e)
        {
            SequenceModel sequece = XmlControl.sequenceModelNew;
            string strName = SetName(sequece);

            int count = sequece.VariableSetModels.Count;

            sequece.VariableSetModels.Add(new VariableSetModel()
            {
                Id = count + 1,
                Name = strName,
                variableType = VariableType.Bool,
                Expression = "",
                Description = "",
                TestResult = ""
            });

            UpdateData();
        }

        private void btnAddString_Click(object sender, EventArgs e)
        {
            SequenceModel sequece = XmlControl.sequenceModelNew;
            string strName = SetName(sequece);

            int count = sequece.VariableSetModels.Count;

            sequece.VariableSetModels.Add(new VariableSetModel()
            {
                Id = count + 1,
                Name = strName,
                variableType = VariableType.String,
                Expression = "",
                Description = "",
                TestResult = ""
            });

            UpdateData();
        }

        private void btnAddObject_Click(object sender, EventArgs e)
        {
            SequenceModel sequece = XmlControl.sequenceModelNew;
            string strName = SetName(sequece);

            int count = sequece.VariableSetModels.Count;

            sequece.VariableSetModels.Add(new VariableSetModel()
            {
                Id = count + 1,
                Name = strName,
                variableType = VariableType.Object,
                Expression = "",
                Description = "",
                TestResult = ""
            });

            UpdateData();
        }

        private void btnAddInt_Click(object sender, EventArgs e)
        {
            SequenceModel sequece = XmlControl.sequenceModelNew;
            string strName = SetName(sequece);

            int count = sequece.VariableSetModels.Count;

            sequece.VariableSetModels.Add(new VariableSetModel()
            {
                Id = count + 1,
                Name = strName,
                variableType = VariableType.Int,
                Expression = "",
                Description = "",
                TestResult = ""
            });

            UpdateData();
        }
        #endregion

        private void UpdateData()
        {
            try
            {
                SequenceModel sequence = XmlControl.sequenceModelNew;
                dataGridView1.DataSource = null;

                if (sequence.VariableSetModels != null && sequence.VariableSetModels.Count != 0)
                {
                    dataGridView1.DataSource = sequence.VariableSetModels;
                } 
                
                
                //dataGridView1.Rows.Clear();
                //dataGridView1.Columns.Clear();
                //dataGridView1.Columns.Add("Id", "序号");
                //dataGridView1.Columns.Add("Name", "名称");
                //dataGridView1.Columns.Add("Type", "类型");
                //dataGridView1.Columns.Add("Express", "表达式");
                //dataGridView1.Columns.Add("Description", "描述");
                //dataGridView1.Columns.Add("Value", "值"); 

                if (dataGridView1.Columns != null && dataGridView1.Columns.Count >= 7)
                {
                    dataGridView1.Columns[0].ReadOnly = true;
                    dataGridView1.Columns[2].ReadOnly = true;
                    dataGridView1.Columns[6].Visible = false;
                    //dataGridView1.Columns[7].Visible = false;

                    dataGridView1.Columns[0].HeaderText = "序号";
                    dataGridView1.Columns[1].HeaderText = "名称";
                    dataGridView1.Columns[2].HeaderText = "类型";
                    dataGridView1.Columns[3].HeaderText = "表达式";
                    dataGridView1.Columns[4].HeaderText = "描述";
                    dataGridView1.Columns[5].HeaderText = "值";

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;                     
                }
            }
            catch(Exception ex)
            {

            }
        }

        #region 上移 下移 删除
       
        private void btnDownMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBoxEx.Show(this, "未选中行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SequenceModel sequence = XmlControl.sequenceModelNew;

                int iselect = dataGridView1.SelectedRows[0].Index;
                DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
                if (rows.Count > 1)
                {
                    MessageBoxEx.Show("一次只能移动一个数据");
                    return;
                }

                if (iselect == dataGridView1.Rows.Count - 1)
                {
                    return;
                }

                VariableSetModel selectModel = dataGridView1.SelectedRows[0].DataBoundItem as VariableSetModel;
                VariableSetModel currentModel = sequence.VariableSetModels.FirstOrDefault(x => x.Id == selectModel.Id);

                VariableSetModel nextModel = sequence.VariableSetModels.FirstOrDefault(x => x.Id == currentModel.Id + 1);
                if (nextModel != null)
                {
                    nextModel.Id -= 1;
                }

                currentModel.Id += 1;

                sequence.VariableSetModels = sequence.VariableSetModels.OrderBy(x => x.Id).ToList();
                //XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());

                UpdateData();

                dataGridView1.Rows[iselect + 1].Selected = true;
                if (dataGridView1.Rows[iselect + 1].Visible == true)
                {
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[iselect + 1].Cells[2];
                }

            }
            catch (Exception ex)
            {

            }
        }
                
        private void btnUpMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBoxEx.Show(this, "未选中行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SequenceModel sequence = XmlControl.sequenceModelNew;

                int iselect = dataGridView1.SelectedRows[0].Index;
                DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
                if (rows.Count > 1)
                {
                    MessageBoxEx.Show("一次只能移动一个数据");
                    return;
                }

                if (iselect == 0)
                {
                    return;
                }

                VariableSetModel selectModel = dataGridView1.SelectedRows[0].DataBoundItem as VariableSetModel;
                VariableSetModel currentModel = sequence.VariableSetModels.FirstOrDefault(x => x.Id == selectModel.Id);

                VariableSetModel nextModel = sequence.VariableSetModels.FirstOrDefault(x => x.Id == currentModel.Id - 1);
                if (nextModel != null)
                {
                    nextModel.Id += 1;
                }

                currentModel.Id -= 1;

                sequence.VariableSetModels = sequence.VariableSetModels.OrderBy(x => x.Id).ToList();
                //XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());

                UpdateData();


                dataGridView1.Rows[iselect - 1].Selected = true;
                if (dataGridView1.Rows[iselect - 1].Visible == true)
                {
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[iselect - 1].Cells[2];
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBoxEx.Show(this, "未选中行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Global.UserName == Global.OperatorName)
                {
                    MessageBoxEx.Show(this, "操作员无权限!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int iselect = dataGridView1.SelectedRows[0].Index;
                SequenceModel sequence = XmlControl.sequenceModelNew;


                List<VariableSetModel> listModel = new List<VariableSetModel>();
                string strName = "";
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    VariableSetModel tModel = dataGridView1.SelectedRows[i].DataBoundItem as VariableSetModel;
                    listModel.Add(tModel);
                    strName += string.Format("【{0}】", tModel.Name);
                }

                string strLog = string.Format("确认删除【{0}】?", strName);
                DialogResult result = MessageBoxEx.Show(strLog, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    MessageBoxEx.Show("取消操作！");
                    return;
                }

                foreach (var item in listModel)
                {
                    VariableSetModel currentModel = sequence.VariableSetModels.FirstOrDefault(x => x.Id == item.Id);
                    sequence.VariableSetModels.Remove(currentModel);
                }


                int index = 1;
                foreach (var item in sequence.VariableSetModels)
                {
                    item.Id = index++;
                }
                sequence.VariableSetModels = sequence.VariableSetModels.OrderBy(x => x.Id).ToList(); 

                UpdateData(); 

                if (dataGridView1.Rows.Count >= iselect + 1)
                {
                    dataGridView1.Rows[iselect].Selected = true;
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[iselect].Cells[2];
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private string SetName(SequenceModel sequence)
        {
            int index = -1;
            int ivalue = 1;
            string strName = "Value";
            index = sequence.VariableSetModels.FindIndex(x => x.Name == strName + ivalue.ToString());
            while (index != -1)
            {
                ivalue++;
                index = sequence.VariableSetModels.FindIndex(x => x.Name == strName + ivalue.ToString());
            }


            strName += ivalue.ToString();

            return strName;
        }

        #region dataGridView1 事件处理
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 3)
                {
                    return;
                }
                MessageBoxEx.Show("1");
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                return;
                if (e.ColumnIndex != 3)
                {
                    return;
                }
                //ShowExpressView view = new ShowExpressView();
                //view.ExpressValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //view.ShowDialog();

                //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = view.ExpressValue;
            }
            catch (Exception ex)
            {

            }
        }

        string m_BeginValue = "";
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1)
                {
                    return;
                }

                if(e.ColumnIndex == 1 || e.ColumnIndex == 5)
                {
                    m_BeginValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1)
                {
                    return;
                }

                string str = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }

                if(e.ColumnIndex == 1)
                {
                    if ((XmlControl.sequenceModelNew.VariableSetModels.FindAll(x => x.Name == str).Count) != 1)
                    {
                        MessageBoxEx.Show(this, "已存在此名称，请更改!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginValue;
                    }
                }
                else if(e.ColumnIndex == 5)
                {
                    string strType = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if(strType == "Double" || strType == "Int")
                    {
                        double dresult;
                        if(!Double.TryParse(str, out dresult))
                        {
                            MessageBoxEx.Show("输入数字类型有误，请重新输入!"); 
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginValue;
                        }
                    }
                    else if(strType == "Bool")
                    {
                        bool bresult; 
                        if (!bool.TryParse(str, out bresult))
                        {
                            MessageBoxEx.Show("输入Bool类型有误，请重新输入!");
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginValue;
                        }
                    }
                } 

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        
    }
}
