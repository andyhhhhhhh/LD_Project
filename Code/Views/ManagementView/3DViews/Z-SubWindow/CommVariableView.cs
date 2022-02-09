using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace ManagementView._3DViews
{
    public partial class CommVariableView : UserControl
    {
        private string outPutValue = "";
        public string OutPutValue
        {
            get
            {
                return outPutValue;
            }
            set
            {
                outPutValue = value;
            }
        }

        private string outPutType = "";
        public string OutPutType
        {
            get
            {
                return outPutType;
            }
            set
            {
                outPutType = value;
            }
        }

        public string m_valueType = "";
        public string m_modelType = "";
        public bool m_bShowParam = true;
        public CommVariableView(bool bShow = true)
        {
            InitializeComponent();
            tableLayoutPanel1.Enabled = bShow;
        }

        private void CommVariableView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTabIndex = 0;

                SingleSequenceModel sequence = XmlControl.sequenceSingle;
                var childModel = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == sequence.BaseSeqName);

                List<CheckFeatureModel> listFeature = new List<CheckFeatureModel>();
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listFeature.AddRange(item.CheckFeatureModels);
                }

                if (m_modelType != "")
                {
                    listBoxModel.DataSource = listFeature.FindAll(x => x.featureType.ToString() == m_modelType);
                }
                else
                {
                    if (m_valueType != "")//筛选ValueType类型
                    {
                        List<CheckFeatureModel> listCheckModel = new List<CheckFeatureModel>();
                        foreach (var checkModel in listFeature)
                        {
                            PropertyInfo[] listpro = null;
                            listpro = XmlControl.GetPropertyInfo(checkModel);
                            if (listpro == null)
                            {
                                continue;
                            }

                            if (m_valueType == "Double" || m_valueType == "Int32")
                            {
                                if (listpro.FirstOrDefault(x => x.PropertyType.Name == "Double") != null ||
                                     listpro.FirstOrDefault(x => x.PropertyType.Name == "Int32") != null ||
                                     listpro.FirstOrDefault(x => x.PropertyType.Name == "Double[]") != null)
                                {
                                    listCheckModel.Add(checkModel);
                                }
                            }
                            else
                            {
                                if (listpro.FirstOrDefault(x => x.PropertyType.Name == m_valueType) != null)
                                {
                                    listCheckModel.Add(checkModel);
                                }
                            }
                        }
                        if (listCheckModel.Count() > 0)
                        {
                            listBoxModel.DataSource = listCheckModel;
                        }
                    }
                    else
                    {
                        listBoxModel.DataSource = listFeature;
                    } 
                }

                listBoxModel.DisplayMember = "Name";  

                //初始化全局变量
                InitTreeView();

                //初始化局部变量
                InitLocal();

                //初始化数组
                InitArray();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("CommVariableView_Load", ex.Message);
            }
        }

        private void listBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!m_bShowParam)
                {
                    return;
                }
                CheckFeatureModel checkModel = listBoxModel.SelectedItem as CheckFeatureModel;
                ShowItemResult(checkModel.Name, checkModel);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("listBoxModel_SelectedIndexChanged", ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                {
                    if (dataGridView1.SelectedCells == null || dataGridView1.SelectedCells.Count == 0)
                    {
                        return;
                    }

                    int index = dataGridView1.SelectedCells[0].RowIndex;
                    ShowValue selectModel = dataGridView1.Rows[index].DataBoundItem as ShowValue;
                    OutPutValue = selectModel.Value;
                    OutPutType = selectModel.ValueType;

                    //CheckFeatureModel checkModel = listBoxModel.SelectedItem as CheckFeatureModel;
                    //OutPutValue = checkModel.Name;
                }
                else
                {
                    ShowValue selectModel = dataGridView1.SelectedRows[0].DataBoundItem as ShowValue;
                    OutPutValue = selectModel.Value;
                    OutPutType = selectModel.ValueType;
                }
                
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnConfirm_Click", ex.Message);
            }
        }
        
        private void ShowItemResult(string name, CheckFeatureModel checkModel)
        {
            SingleSequenceModel sequence = XmlControl.sequenceSingle;
            List<ShowValue> listValue = new List<ShowValue>();
            PropertyInfo[] listpro = null;

            listpro = XmlControl.GetPropertyInfo(checkModel);

            if (listpro != null)
            {
                foreach (var item in listpro)
                {
                    if (m_valueType == "Double" || m_valueType == "Int32")
                    {
                        if (item.PropertyType.Name == "Double" || item.PropertyType.Name == "Int32" || item.PropertyType.Name == "Double[]")
                        {
                            ShowValue showvalue = new ShowValue();
                            showvalue.ValueType = item.PropertyType.Name;
                            showvalue.Name = item.Name;
                            showvalue.Value = name + "." + item.Name;

                            //新增描述显示
                            Object[] obs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                            if (obs != null && obs.Length != 0)
                            {
                                DescriptionAttribute des = obs[0] as DescriptionAttribute;
                                showvalue.Description = des.Description;
                            }

                            listValue.Add(showvalue);
                        }
                    }
                    else
                    {
                        if (m_valueType == "" || m_valueType == item.PropertyType.Name)
                        {
                            ShowValue showvalue = new ShowValue();
                            showvalue.ValueType = item.PropertyType.Name;
                            showvalue.Name = item.Name;
                            showvalue.Value = name + "." + item.Name;

                            //新增描述显示
                            Object[] obs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                            if(obs != null && obs.Length != 0)
                            {
                                DescriptionAttribute des = obs[0] as DescriptionAttribute;
                                showvalue.Description = des.Description;
                            }

                            listValue.Add(showvalue);
                        }
                    }

                }
            }

            ShowData(listValue);
        }

        private void ShowData(List<ShowValue> listValue)
        {
            try
            { 
                dataGridView1.DataSource = null;
                if(listValue != null && listValue.Count > 0)
                {
                    dataGridView1.DataSource = listValue; 
                } 

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("ShowData", ex.Message);
            }
        }

        public class ShowValue
        {
            public string Name
            {
                get; set;
            }
            public string ValueType
            {
                get; set;
            }
            public string Value
            {
                get; set;
            }
            public string Description
            {
                get; set;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConfirm.PerformClick();
        } 

        #region 全局变量

        public void InitTreeView()
        {
            try
            {
                treeViewAllTools.Nodes.Clear();

                int imageIndex = 0;
                foreach (var item in Enum.GetNames(typeof(VariableType)).ToList())
                {
                    TreeNode nodeSetting = new TreeNode();
                    nodeSetting.Name = item;
                    nodeSetting.Text = item + "变量";
                    nodeSetting.ImageIndex = imageIndex;
                    nodeSetting.SelectedImageIndex = 5;
                    imageIndex++;
                    treeViewAllTools.Nodes.Add(nodeSetting);
                }

                treeViewAllTools.ExpandAll();
                if (treeViewAllTools.Nodes.Count > 0)
                {
                    treeViewAllTools.SelectedNode = treeViewAllTools.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("InitTreeView", ex.Message);
            }
        }

        private void treeViewAllTools_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                dataGridView2.DataSource = null;
                TreeNode node = treeViewAllTools.SelectedNode;
                string strName = node.Name;
                VariableType varType = (VariableType)Enum.Parse(typeof(VariableType), strName);
                var listItem = XmlControl.sequenceModelNew.VariableSetModels.FindAll(x => x.variableType == varType);

                dataGridView2.DataSource = listItem;
                UpdateData();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("treeViewAllTools_AfterSelect", ex.Message);
            }
        }

        private void UpdateData()
        {
            try
            {
                if (dataGridView2.Columns != null && dataGridView2.Columns.Count > 6)
                {
                    dataGridView2.Columns[0].ReadOnly = true;
                    dataGridView2.Columns[2].ReadOnly = true;
                    dataGridView2.Columns[6].Visible = false;

                    dataGridView2.Columns[0].HeaderText = "序号";
                    dataGridView2.Columns[1].HeaderText = "名称";
                    dataGridView2.Columns[2].HeaderText = "类型";
                    dataGridView2.Columns[3].HeaderText = "表达式";
                    dataGridView2.Columns[4].HeaderText = "描述";
                    dataGridView2.Columns[5].HeaderText = "值";

                    dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("UpdateData", ex.Message);
            }
        }

        private void btnConfirm2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows == null || dataGridView2.SelectedRows.Count == 0)
                {
                    if(dataGridView2.SelectedCells == null || dataGridView2.SelectedCells.Count == 0)
                    {
                        return; 
                    }

                    int index = dataGridView2.SelectedCells[0].RowIndex;
                    VariableSetModel selectModel = dataGridView2.Rows[index].DataBoundItem as VariableSetModel;
                    OutPutValue = "Global." + selectModel.Name;
                    OutPutType = selectModel.variableType.ToString();
                }
                else
                {
                    VariableSetModel selectModel = dataGridView2.SelectedRows[0].DataBoundItem as VariableSetModel;
                    OutPutValue = "Global." + selectModel.Name;
                    OutPutType = selectModel.variableType.ToString();
                } 
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnConfirm2_Click", ex.Message);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConfirm2.PerformClick();
        }
          
        #endregion

        #region 局部变量

        private void InitLocal()
        {
            try
            {
                SingleSequenceModel sequence = XmlControl.sequenceSingle;
                var childModel = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == sequence.BaseSeqName); 
                List<CheckFeatureModel> listFeature = new List<CheckFeatureModel>();
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listFeature.AddRange(item.CheckFeatureModels);
                }

                List<LocalVariableModel> listVar = new List<LocalVariableModel>();
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listVar.AddRange(item.LocalVariableModels);
                }

                var Listcheckmodel = listFeature.FindAll(x => x.featureType == FeatureType.局部变量).ToList();
                List<LocalVariableModel> listVarModel = new List<LocalVariableModel>();

                foreach (var item in Listcheckmodel)
                {
                    var varModel = listVar.FirstOrDefault(x => x.Name == item.Name);
                    listVarModel.Add(varModel);
                }

                listBox1.DataSource = listVarModel;
                listBox1.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("InitLocal", ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LocalVariableModel localModel = listBox1.SelectedItem as LocalVariableModel;
                dataGridView3.DataSource = null;
                if (localModel.ListVariable != null && localModel.ListVariable.Count > 0)
                {
                    dataGridView3.DataSource = localModel.ListVariable;
                }

                if (dataGridView3.Columns != null && dataGridView3.Columns.Count > 6)
                { 
                    dataGridView3.Columns[0].ReadOnly = true;
                    dataGridView3.Columns[2].ReadOnly = true;
                    dataGridView3.Columns[6].Visible = false;

                    dataGridView3.Columns[0].HeaderText = "序号";
                    dataGridView3.Columns[1].HeaderText = "名称";
                    dataGridView3.Columns[2].HeaderText = "类型";
                    dataGridView3.Columns[3].HeaderText = "表达式";
                    dataGridView3.Columns[4].HeaderText = "描述";
                    dataGridView3.Columns[5].HeaderText = "值";

                    dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("listBox1_SelectedIndexChanged", ex.Message);
            }
        }

        private void btnConfirm3_Click(object sender, EventArgs e)
        {
            try
            {
                LocalVariableModel localModel = listBox1.SelectedItem as LocalVariableModel;

                if (dataGridView3.SelectedRows == null || dataGridView3.SelectedRows.Count == 0)
                {
                    if (dataGridView3.SelectedCells == null || dataGridView3.SelectedCells.Count == 0)
                    {
                        return;
                    }

                    int index = dataGridView3.SelectedCells[0].RowIndex;
                    LocalVariableModel.Variable selectModel = dataGridView3.Rows[index].DataBoundItem as LocalVariableModel.Variable;
                    OutPutValue = "Local." + localModel.Name + "." + selectModel.Name;
                    OutPutType = selectModel.variableType.ToString();
                }
                else
                {
                    LocalVariableModel.Variable selectModel = dataGridView3.SelectedRows[0].DataBoundItem as LocalVariableModel.Variable;
                    OutPutValue = "Local." + localModel.Name + "." + selectModel.Name;
                    OutPutType = selectModel.variableType.ToString();
                }
                 
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnConfirm3_Click", ex.Message);
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConfirm3.PerformClick(); 
        }
        
        #endregion

        #region 数组
        private void InitArray()
        {
            try
            {
                SingleSequenceModel sequence = XmlControl.sequenceSingle; 
                var childModel = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == sequence.BaseSeqName);
                List<CheckFeatureModel> listFeature = new List<CheckFeatureModel>();
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listFeature.AddRange(item.CheckFeatureModels);
                }

                List<LocalArrayModel> listArr = new List<LocalArrayModel>();
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listArr.AddRange(item.LocalArrayModels);
                }

                var Listcheckmodel = listFeature.FindAll(x => x.featureType == FeatureType.数组定义).ToList();
                List<LocalArrayModel> listVarModel = new List<LocalArrayModel>();

                foreach (var item in Listcheckmodel)
                {
                    var varModel = listArr.FirstOrDefault(x => x.Name == item.Name);
                    listVarModel.Add(varModel);
                }

                listArray.DataSource = listVarModel;
                listArray.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("InitArray", ex.Message);
            }
        }

        private void listArray_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LocalArrayModel localModel = listArray.SelectedItem as LocalArrayModel;
                dataGridView4.DataSource = null;
                if (localModel.ListArray != null && localModel.ListArray.Count > 0)
                {
                    dataGridView4.DataSource = localModel.ListArray;
                }

                if (dataGridView4.Columns != null && dataGridView4.Columns.Count >= 6)
                {
                    dataGridView4.Columns[0].ReadOnly = true;
                    dataGridView4.Columns[2].ReadOnly = true;
                    //dataGridView4.Columns[6].Visible = false;

                    dataGridView4.Columns[0].HeaderText = "序号";
                    dataGridView4.Columns[1].HeaderText = "名称";
                    dataGridView4.Columns[2].HeaderText = "类型";
                    dataGridView4.Columns[3].HeaderText = "表达式";
                    dataGridView4.Columns[4].HeaderText = "描述";
                    dataGridView4.Columns[5].HeaderText = "值";

                    dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView4.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("listArray_SelectedIndexChanged", ex.Message);
            }
        }

        private void btnConfrimArr_Click(object sender, EventArgs e)
        {
            try
            {
                LocalArrayModel localModel = listArray.SelectedItem as LocalArrayModel;
                if (dataGridView4.SelectedRows == null || dataGridView4.SelectedRows.Count == 0)
                {
                    if (dataGridView4.SelectedCells == null || dataGridView4.SelectedCells.Count == 0)
                    {
                        return;
                    }

                    int index = dataGridView4.SelectedCells[0].RowIndex;
                    LocalArrayModel.ArrayVar selectModel = dataGridView4.Rows[index].DataBoundItem as LocalArrayModel.ArrayVar;
                    OutPutValue = "Array." + localModel.Name + "." + selectModel.Name;
                    OutPutType = selectModel.arrayType.ToString();
                }
                else
                {
                    LocalArrayModel.ArrayVar selectModel = dataGridView4.SelectedRows[0].DataBoundItem as LocalArrayModel.ArrayVar;
                    OutPutValue = "Array." + localModel.Name + "." + selectModel.Name;
                    OutPutType = selectModel.arrayType.ToString();
                }

            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnConfrimArr_Click", ex.Message);
            }
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConfrimArr.PerformClick();
        }
        #endregion

    }
}
