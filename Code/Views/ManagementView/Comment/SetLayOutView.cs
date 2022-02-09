using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class SetLayOutView : Form
    { 
        ComboBoxEx m_comboBox1 = new ComboBoxEx();
        ComboBoxEx m_comboBox2 = new ComboBoxEx();
        private ChildSequenceModel m_childModel = null;
        //确认事件
        public static event EventHandler<object> CanelEvent;
        public void OnCanelEvent(object e)
        {
            EventHandler<object> handler = CanelEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public SetLayOutView(ChildSequenceModel childModel)
        {
            InitializeComponent();
            m_childModel = childModel;
        }

        private void SetLayOutView_Load(object sender, EventArgs e)
        {
            try
            {
                List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
                if(XMLController.XmlControl.sequenceSingle != null)
                { 
                    listModel = XMLController.XmlControl.sequenceSingle.CheckFeatureModels;
                }
                else
                {
                    if(m_childModel == null)
                    {
                        return;
                    }
                    listModel = m_childModel.GetCheckFeatureList();
                }

                if (listModel != null && listModel.Count > 0)
                {
                    dataGridView1.DataSource = listModel;

                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[10].Visible = false; 

                    dataGridView1.Columns[0].ReadOnly = true;
                    dataGridView1.Columns[1].ReadOnly = true;
                    dataGridView1.Columns[11].ReadOnly = true;
                    dataGridView1.Columns[12].ReadOnly = true;

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView1.Columns[12].Width = 130;

                    dataGridView1.Columns[0].HeaderText = "Id";
                    dataGridView1.Columns[1].HeaderText = "名称";
                    dataGridView1.Columns[3].HeaderText = "描述";
                    dataGridView1.Columns[5].HeaderText = "是否启用";
                    dataGridView1.Columns[9].HeaderText = "图像";
                    dataGridView1.Columns[11].HeaderText = "序号";
                    dataGridView1.Columns[12].HeaderText = "报警跳转";
                }

                this.dataGridView1.Controls.Add(m_comboBox1);
                this.dataGridView1.Controls.Add(m_comboBox2);
                m_comboBox1.Visible = false;
                m_comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                m_comboBox2.Visible = false;
                m_comboBox2.Items.AddRange(new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8" }.ToArray());

                m_comboBox1.Font = new System.Drawing.Font("微软雅黑", 10F);
                m_comboBox2.Font = new System.Drawing.Font("微软雅黑", 10F);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void SetLayOutView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                OnCanelEvent(null);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                CheckFeatureModel selModel = dataGridView1.Rows[e.RowIndex].DataBoundItem as CheckFeatureModel;
                if (e.ColumnIndex == 12)
                {
                    m_comboBox1.Items.Clear();
                    m_comboBox1.Items.Add("");
                    ChildSequenceModel childModel = null;
                    if (XMLController.XmlControl.sequenceSingle != null)
                    {
                        childModel = XMLController.XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == XMLController.XmlControl.sequenceSingle.BaseSeqName);
                    }
                    else
                    {
                        childModel = m_childModel;
                    }
                    
                    List<CheckFeatureModel> listModel = childModel.GetCheckFeatureList();
                    foreach (var item in listModel)
                    {
                        if (item.Index > selModel.Index)
                        {
                            m_comboBox1.Items.Add(item.Name);
                        }
                    }
                    m_comboBox1.Text = selModel.AlarmGoTo; 

                    m_comboBox1.Location = rect.Location;
                    m_comboBox1.Size = rect.Size;
                    m_comboBox1.Visible = true;
                }
                else if(e.ColumnIndex == 9)
                {
                    m_comboBox2.Text = selModel.LayOut.ToString();

                    m_comboBox2.Location = rect.Location;
                    m_comboBox2.Size = rect.Size;
                    m_comboBox2.Visible = true;
                }
            }
            catch (Exception ex)
            { 
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 12 && e.ColumnIndex != 9)
                {
                    return;
                }

                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (e.ColumnIndex == 12)
                {
                    cell.Value = m_comboBox1.Text;
                    m_comboBox1.Visible = false;
                } 
                else
                {
                    cell.Value = m_comboBox2.Text;
                    m_comboBox2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
