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
using System.IO;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using GlobalCore;

namespace ManagementView.MotorView
{
    public partial class CardView : UserControl
    {
        public string m_CardPath = Application.StartupPath + "//Sequence//Card//";
        public ControlCardModel m_CardModel = new ControlCardModel();
        AxisParamModel m_selAxisModel = null;

        ComboBoxEx m_comboBox1 = new ComboBoxEx();
        ComboBoxEx m_comboBox2 = new ComboBoxEx();
        public CardView()
        {
            InitializeComponent();
        }

        private void CardView_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(m_CardPath))
                {
                    Directory.CreateDirectory(m_CardPath);
                }
                m_CardPath += "Card.dsr";
                m_CardModel = XmlControl.controlCardModel;
                if (m_CardModel == null)
                {
                    m_CardModel = new ControlCardModel();
                }

                InitTree();
                InitDataIO();
                InitStation();
                InitStationCmbox();

                this.dataIO.Controls.Add(m_comboBox1);
                this.dataIO.Controls.Add(m_comboBox2);
                m_comboBox1.Visible = false;
                m_comboBox2.Visible = false;

                m_comboBox1.DataSource = Enum.GetNames(typeof(EnumIO));
                m_comboBox2.DataSource = Enum.GetNames(typeof(EnumIOType));

                cmbhomeType.DataSource = Enum.GetNames(typeof(EnumHomeType));
                cmblimitType.DataSource = Enum.GetNames(typeof(EnumLimitType));
                cmbmotorType.DataSource = Enum.GetNames(typeof(EnumMotorType));
                
                OutPuttoolStripMenuItem1.Enabled = Global.UserName == Global.EngineerName;
                InPuttoolStripMenuItem2.Enabled = Global.UserName == Global.EngineerName;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 控制卡配置
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CardModel tModel = new CardModel();
                tModel.AxisNum = Int32.Parse(txtAxisNum.Text);
                tModel.IoNum = Int32.Parse(txtIONum.Text);
                tModel.Id = Int32.Parse(txtCardIndex.Text);
                tModel.Name = "Card: " + tModel.Id.ToString();

                AxisParamModel[] axisModel = new AxisParamModel[tModel.AxisNum];
                for (int i = 0; i < tModel.AxisNum; i++)
                {
                    axisModel[i] = new AxisParamModel();
                    axisModel[i].Id = i;
                    axisModel[i].Name = "Axis:" + i.ToString();
                    axisModel[i].axisIndex = (ushort)i;
                    axisModel[i].cardIndex = (ushort)tModel.Id;
                }
                tModel.AxisParamModels.AddRange(axisModel);

                var cardModels = m_CardModel.CardModels;
                if (cardModels != null)
                {
                    if (cardModels.FindIndex(x => x.Id == tModel.Id) != -1)
                    {
                        MessageBoxEx.Show("已存在此Id的控制卡，请修改!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                m_CardModel.CardModels.Add(tModel);

                InitTree();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var node = advTree1.SelectedNode;
                if (node.Parent == null)
                {
                    int Id = Int32.Parse(node.Name);
                    var card = m_CardModel.CardModels.FirstOrDefault(x => x.Id == Id);
                    var dialogResult = MessageBoxEx.Show("是否删除控制卡?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        m_CardModel.CardModels.Remove(card);
                    }
                }
                else
                {
                    if (node.Text.Contains("Axis"))
                    {
                        MessageBoxEx.Show("轴不能手动删除!!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int Id = Int32.Parse(node.Name);
                        var extIo = m_selectCard.ExtIoModels.FirstOrDefault(x => x.Id == Id);
                        m_selectCard.ExtIoModels.Remove(extIo);
                    }
                }

                InitTree();
            }
            catch (Exception ex)
            {

            }
        }

        private void 添加扩展卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExtIoModel tModel = new ExtIoModel();
                tModel.IoNum = Int32.Parse(txtExtIONum.Text);

                if (m_selectCard == null)
                {
                    return;
                }

                int num = m_selectCard.ExtIoModels.Count();
                tModel.Id = num;
                tModel.Name = "ExtIo:" + num.ToString();
                m_selectCard.ExtIoModels.Add(tModel);

                InitTree();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 配置操作
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateAxis();
                UpdateStation();
                XmlControl.SaveToXml(m_CardPath, m_CardModel, typeof(ControlCardModel));
                XmlControl.controlCardModel = m_CardModel;

                MessageBoxEx.Show("保存成功!!");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存之后再刷新工站中的轴参数
        /// </summary>
        private void UpdateStation()
        {
            try
            {
                foreach (var item in m_CardModel.StationModels)
                {
                    if(item.Axis_X != null)
                    {
                        var card = m_CardModel.CardModels.FirstOrDefault(x => x.Id == item.Axis_X.cardIndex);
                        var axis = card.AxisParamModels.FirstOrDefault(x => x.axisIndex == item.Axis_X.axisIndex);
                        item.Axis_X = axis;
                    }
                    if (item.Axis_Y != null)
                    {
                        var card = m_CardModel.CardModels.FirstOrDefault(x => x.Id == item.Axis_Y.cardIndex);
                        var axis = card.AxisParamModels.FirstOrDefault(x => x.axisIndex == item.Axis_Y.axisIndex);
                        item.Axis_Y = axis;
                    }
                    if (item.Axis_Z != null)
                    {
                        var card = m_CardModel.CardModels.FirstOrDefault(x => x.Id == item.Axis_Z.cardIndex);
                        var axis = card.AxisParamModels.FirstOrDefault(x => x.axisIndex == item.Axis_Z.axisIndex);
                        item.Axis_Z = axis;
                    }
                    if (item.Axis_U != null)
                    {
                        var card = m_CardModel.CardModels.FirstOrDefault(x => x.Id == item.Axis_U.cardIndex);
                        var axis = card.AxisParamModels.FirstOrDefault(x => x.axisIndex == item.Axis_U.axisIndex);
                        item.Axis_U = axis;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitTree();
            InitIO();
        }
        #endregion

        /// <summary>
        /// 初始化显示在树形控件中
        /// </summary>
        private void InitTree()
        {
            try
            {
                advTree1.BeginUpdate();
                advTree1.View = eView.Tree;
                advTree1.Nodes.Clear();

                foreach (var item in m_CardModel.CardModels)
                {
                    Node node = new Node();
                    node.Name = item.Id.ToString();
                    node.Text = item.Name;
                    node.ImageIndex = 0;
                    

                    foreach (var item2 in item.AxisParamModels)
                    {
                        Node cNode = new Node();
                        cNode.Name = item2.Id.ToString();
                        cNode.Text = item2.Name + " " + item2.aliasName;
                        node.Nodes.Add(cNode);
                        cNode.ImageIndex = 1;
                    }

                    foreach (var item3 in item.ExtIoModels)
                    {
                        Node chNode = new Node();
                        chNode.Name = item3.Id.ToString();
                        chNode.Text = item3.Name;
                        node.Nodes.Add(chNode);
                        chNode.ImageIndex = 2;
                    }

                    advTree1.Nodes.Add(node);
                }
                 
                advTree1.ExpandAll();
                advTree1.EndUpdate(); 
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据配置的卡和扩展IO来初始化IO
        /// </summary>
        private void InitIO()
        {
            try
            {
                var IoModels = m_CardModel.IOModels;

                List<IOModel> listModel = new List<IOModel>();
                foreach (var item in m_CardModel.CardModels)
                {
                    //先添加控制卡自带的IO
                    int num = item.IoNum;
                    IOModel[] IOModelArr = new IOModel[num * 2];
                    for (int i = 0; i < num * 2; i++)
                    {
                        if (i < num)
                        {
                            var model = IoModels.FirstOrDefault(x => x.cardIndex == item.Id && x.extIndex == 0 && x.index == i && x.enumIo == EnumIO.通用输入);
                            if (model == null)
                            {
                                IOModelArr[i] = new IOModel()
                                {
                                    cardIndex = item.Id,
                                    extIndex = 0,
                                    index = i,
                                    enumIo = EnumIO.通用输入,
                                    enumIoType = EnumIOType.通用IO,
                                    bReverse = false,
                                };
                            }
                            else
                            {
                                IOModelArr[i] = model;
                            }
                        }
                        else
                        {
                            var model = IoModels.FirstOrDefault(x => x.cardIndex == item.Id && x.extIndex == 0 && x.index == i - num && x.enumIo == EnumIO.通用输出);
                            if (model == null)
                            {
                                IOModelArr[i] = new IOModel()
                                {
                                    cardIndex = item.Id,
                                    extIndex = 0,
                                    index = i - num,
                                    enumIo = EnumIO.通用输出,
                                    enumIoType = EnumIOType.通用IO,
                                    bReverse = false,
                                };
                            }
                            else
                            {
                                IOModelArr[i] = model;
                            }
                        }
                    }
                    listModel.AddRange(IOModelArr);

                    //再添加扩展卡上的IO
                    foreach (var item2 in item.ExtIoModels)
                    {
                        int extnum = item2.IoNum;
                        IOModel[] ExtIOArr = new IOModel[extnum * 2];
                        for (int i = 0; i < extnum * 2; i++)
                        {
                            if (i < extnum)
                            {
                                var model = IoModels.FirstOrDefault(x => x.cardIndex == item.Id && x.extIndex == item2.Id + 1 && x.index == i && x.enumIo == EnumIO.通用输入);
                                if (model == null)
                                {
                                    ExtIOArr[i] = new IOModel()
                                    {
                                        cardIndex = item.Id,
                                        extIndex = item2.Id + 1,
                                        index = i,
                                        enumIo = EnumIO.通用输入,
                                        enumIoType = EnumIOType.通用IO,
                                        bReverse = false,
                                    };
                                }
                                else
                                {
                                    ExtIOArr[i] = model;
                                }
                            }
                            else
                            {
                                var model = IoModels.FirstOrDefault(x => x.cardIndex == item.Id && x.extIndex == item2.Id + 1 && x.index == i - extnum && x.enumIo == EnumIO.通用输出);
                                if (model == null)
                                {
                                    ExtIOArr[i] = new IOModel()
                                    {
                                        cardIndex = item.Id,
                                        extIndex = item2.Id + 1,
                                        index = i - extnum,
                                        enumIo = EnumIO.通用输出,
                                        enumIoType = EnumIOType.通用IO,
                                        bReverse = false,
                                    };
                                }
                                else
                                {
                                    ExtIOArr[i] = model;
                                }
                            }

                        }
                        listModel.AddRange(ExtIOArr);
                    }
                }

                int id = 0;
                foreach (var item in listModel)
                {
                    item.Id = id;
                    id++;
                }
                m_CardModel.IOModels = listModel;

                InitDataIO();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化DataGridView中IO信息
        /// </summary>
        private void InitDataIO()
        {
            try
            {
                if(m_CardModel.IOModels != null && m_CardModel.IOModels.Count > 0)
                {
                    dataIO.DataSource = m_CardModel.IOModels;
                    dataIO.Columns[0].ReadOnly = true;
                    dataIO.Columns[1].ReadOnly = true;
                    dataIO.Columns[2].ReadOnly = true;
                    dataIO.Columns[3].ReadOnly = true;

                    if(GlobalCore.Global.UserName == GlobalCore.Global.OperatorName)
                    {
                        dataIO.Columns[7].ReadOnly = true; 
                    }
                }

                cmbCheckIO.Items.Clear();
                cmbCheckIO.Items.Add("");
                foreach (var item in m_CardModel.IOModels)
                {
                    if(!string.IsNullOrEmpty(item.Name))
                    { 
                        cmbCheckIO.Items.Add(item.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        CardModel m_selectCard = null;
        private void advTree1_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            try
            {
                int Id = 0;
                var node = e.Node;
                if (node.Parent != null)
                {
                    node = node.Parent;
                }
                Id = Int32.Parse(node.Name);
                m_selectCard = m_CardModel.CardModels.FirstOrDefault(x => x.Id == Id);

                node = e.Node;
                if (node.Parent != null)
                {
                    if (node.Text.Contains("Axis"))
                    {
                        m_selAxisModel = m_selectCard.AxisParamModels.FirstOrDefault(x => x.Id == Int32.Parse(node.Name));
                        if (m_selAxisModel != null)
                        {
                            Show(m_selAxisModel);
                        }
                    }
                    else
                    {
                        ExtIoModel emodel = m_selectCard.ExtIoModels.FirstOrDefault(x => x.Id == Int32.Parse(node.Name));
                        Show(emodel);
                    }
                }

                Show(m_selectCard);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAxis()
        {
            try
            {
                if(m_selAxisModel == null)
                {
                    return;
                }

                m_selAxisModel.aliasName = txtAxisAlias.Text;
                m_selAxisModel.motorType = (EnumMotorType)Enum.Parse(typeof(EnumMotorType), cmbmotorType.Text);
                m_selAxisModel.limitType = (EnumLimitType)Enum.Parse(typeof(EnumLimitType), cmblimitType.Text);
                m_selAxisModel.homeType = (EnumHomeType)Enum.Parse(typeof(EnumHomeType), cmbhomeType.Text);
                m_selAxisModel.limitN = long.Parse(txtlimitN.Text);
                m_selAxisModel.limitP = long.Parse(txtlimitP.Text);
                m_selAxisModel.homeSecondVel = double.Parse(txthomeSecondVel.Text);
                m_selAxisModel.stepvalue = double.Parse(txtstepvalue.Text);
                m_selAxisModel.maxVel = double.Parse(txtmaxVel.Text);
                m_selAxisModel.maxAcc = double.Parse(txtmaxAcc.Text);
                m_selAxisModel.maxDec = double.Parse(txtmaxDec.Text);
                m_selAxisModel.maxAacc = double.Parse(txtAAcc.Text);
                m_selAxisModel.homeVel = double.Parse(txthomeVel.Text);
                m_selAxisModel.iInHomeOffset = long.Parse(txtiInHomeOffset.Text);
                m_selAxisModel.homeIo = UInt32.Parse(txtHomeIO.Text);
                m_selAxisModel.InPlaceOffSet = double.Parse(txtinplaceOffSet.Text);
                m_selAxisModel.homeMode = Int32.Parse(txtHomeMode.Text);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void Show(AxisParamModel model)
        {
            try
            {
                txtAxisAlias.Text = model.aliasName;
                cmbmotorType.Text = model.motorType.ToString();
                cmblimitType.Text = model.limitType.ToString();
                cmbhomeType.Text = model.homeType.ToString();
                txtlimitN.Text = model.limitN.ToString();
                txtlimitP.Text = model.limitP.ToString();
                txthomeSecondVel.Text = model.homeSecondVel.ToString();
                txtstepvalue.Text = model.stepvalue.ToString();
                txtmaxVel.Text = model.maxVel.ToString();
                txtmaxAcc.Text = model.maxAcc.ToString();
                txtmaxDec.Text = model.maxDec.ToString();
                txtAAcc.Text = model.maxAacc.ToString();
                txthomeVel.Text = model.homeVel.ToString();
                txtiInHomeOffset.Text = model.iInHomeOffset.ToString();
                txtHomeIO.Text = model.homeIo.ToString();
                txtinplaceOffSet.Text = model.InPlaceOffSet.ToString();
                txtHomeMode.Text = model.homeMode.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Show(ExtIoModel model)
        {
            try
            {
                txtExtIONum.Text = model.IoNum.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Show(CardModel model)
        {
            try
            {
                txtCardIndex.Text = model.Id.ToString();
                txtAxisNum.Text = model.AxisNum.ToString();
                txtIONum.Text = model.IoNum.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 在datagridview中实现点击显示comboBox供选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataIO_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 3 && e.ColumnIndex != 4)
                {
                    return;
                }

                DataGridViewCell cell = dataIO.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle rect = this.dataIO.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);

                if (e.ColumnIndex == 3)
                {
                    m_comboBox1.Location = rect.Location;
                    m_comboBox1.Size = rect.Size;
                    m_comboBox1.Visible = true;
                    m_comboBox1.Text = cell.Value.ToString();
                }
                else if (e.ColumnIndex == 4)
                {
                    m_comboBox2.Location = rect.Location;
                    m_comboBox2.Size = rect.Size;
                    m_comboBox2.Visible = true;
                    m_comboBox2.Text = cell.Value.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataIO_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 3 && e.ColumnIndex != 4)
                {
                    return;
                }

                DataGridViewCell cell = dataIO.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (e.ColumnIndex == 3)
                {
                    cell.Value = m_comboBox1.Text;
                    m_comboBox1.Visible = false;
                }
                else if (e.ColumnIndex == 4)
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
         
        string m_BeginValue = "";
        private void dataIO_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.ColumnIndex == 7)
                {
                    var str = dataIO.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (str != null)
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

        private void dataIO_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //如果不是更改名字则退出
                if (e.RowIndex == -1 || e.ColumnIndex != 7)
                {
                    return;
                }

                var str = dataIO.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (str == null)
                {
                    return;
                }
                string strvalue = str.ToString();

                if (e.ColumnIndex == 7)
                {
                    if ((m_CardModel.IOModels.FindAll(x => x.Name == strvalue).Count) != 1)
                    {
                        MessageBoxEx.Show(this, "已存在此名称，请更改!!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataIO.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginValue;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        #region 工站操作
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                StationModel model = new StationModel();
                model.Id = Int32.Parse(txtStationId.Text);
                model.Name = txtStationName.Text;
                model.AxisNum = Int32.Parse(cmbStationAxisNum.Text);
                if(string.IsNullOrEmpty(cmbCheckIO.Text))
                { 
                    model.CheckIOModel = null;
                }
                else
                {
                    IOModel ioModel = m_CardModel.IOModels.FirstOrDefault(x => x.Name == cmbCheckIO.Text);
                    model.CheckIOModel = ioModel;
                }

                if(m_CardModel.StationModels.FindIndex(x=>x.Name == model.Name || x.Id == model.Id) != -1)
                {
                    MessageBoxEx.Show("已存在此Id或Name的工站，请修改!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PointModel[] pointModelArr = new PointModel[200];
                for (int i = 0; i < 200; i++)
                {
                    pointModelArr[i] = new PointModel()
                    {
                        Id = i,
                        StationName = model.Name
                    };
                }

                model.PointModels = pointModelArr.ToList();
                
                m_CardModel.StationModels.Add(model);
                m_CardModel.StationModels = m_CardModel.StationModels.OrderBy(x => x.Id).ToList();
                InitStation();
                AxisVisible(model.AxisNum);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                int num = dataStation.SelectedRows.Count;
                if(num != 1)
                {
                    return;
                }
                var selectModel = dataStation.SelectedRows[0].DataBoundItem as StationModel;

                selectModel.Axis_X = GetAxisModel(cmbCardX, cmbAxisX);

                if (selectModel.AxisNum > 1)
                {
                    selectModel.Axis_Y = GetAxisModel(cmbCardY, cmbAxisY);
                }
                else
                {
                    selectModel.Axis_Y = null;
                }

                if(selectModel.AxisNum > 2)
                {
                    selectModel.Axis_Z = GetAxisModel(cmbCardZ, cmbAxisZ);
                }
                else
                {
                    selectModel.Axis_Z = null;
                }

                if(selectModel.AxisNum > 3)
                {
                    selectModel.Axis_U = GetAxisModel(cmbCardU, cmbAxisU);
                }
                else
                {
                    selectModel.Axis_U = null;
                }

                if (string.IsNullOrEmpty(cmbCheckIO.Text))
                {
                    selectModel.CheckIOModel = null;
                }
                else
                {
                    IOModel ioModel = m_CardModel.IOModels.FirstOrDefault(x => x.Name == cmbCheckIO.Text);
                    selectModel.CheckIOModel = ioModel;
                }

                List<StationModel> listModel = m_CardModel.StationModels.FindAll(x => x.Id != selectModel.Id || x.Name != selectModel.Name);
                if (listModel.FindIndex(x => x.Name == txtStationName.Text || x.Id == Int32.Parse(txtStationId.Text)) != -1)
                {
                    MessageBoxEx.Show("已存在此Id或Name的工站，请修改!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectModel.Name = txtStationName.Text;
                selectModel.AxisNum = Int32.Parse(cmbStationAxisNum.Text);
                selectModel.Id = Int32.Parse(txtStationId.Text);

                foreach (var item in selectModel.PointModels)
                {
                    item.StationName = selectModel.Name;
                }

                MessageBox.Show("修改成功");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据信息获取轴信息
        /// </summary>
        /// <param name="cmbCard">卡号选择</param>
        /// <param name="cmbAxis">轴信息选择</param>
        /// <returns></returns>
        private AxisParamModel GetAxisModel(ComboBoxEx cmbCard, ComboBoxEx cmbAxis)
        {
            try
            {
                int cardId = Int32.Parse(cmbCard.Text);
                var cardModel = m_CardModel.CardModels.FirstOrDefault(x => x.Id == cardId);
                string value = cmbAxis.Text;
                int index = value.IndexOf("_");
                if (index != -1)
                {
                    string name = value.Substring(0, index);
                    var axisModel = cardModel.AxisParamModels.FirstOrDefault(x => x.Name == name);

                    return axisModel;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataStation.SelectedRows.Count == 0)
                {
                    return;
                }
                var selectModel = dataStation.SelectedRows[0].DataBoundItem as StationModel;

                var dialogResult = MessageBoxEx.Show("是否删除工站?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    m_CardModel.StationModels.Remove(selectModel);
                    InitStation();
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化工站的datagridview
        /// </summary>
        private void InitStation()
        {
            try
            {
                if(m_CardModel.StationModels != null && m_CardModel.StationModels.Count > 0)
                {
                    dataStation.DataSource = null;
                    dataStation.DataSource = m_CardModel.StationModels; 

                    dataStation.Columns["Axis_X"].Visible = false;
                    dataStation.Columns["Axis_Y"].Visible = false;
                    dataStation.Columns["Axis_Z"].Visible = false;
                    dataStation.Columns["Axis_U"].Visible = false;

                    dataStation.Columns["CheckIOModel"].Visible = false;
                    dataStation.Columns["PercentSpeed"].Visible = false;

                    //dataStation.Columns["AxisNum"].HeaderText = "轴数量";
                }
                else
                {
                    dataStation.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataStation_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if(dataStation.SelectedRows.Count == 0)
                {
                    return;
                }
                var selectModel = dataStation.SelectedRows[0].DataBoundItem as StationModel;

                AxisVisible(selectModel.AxisNum);

                ShowStation(selectModel);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据工站配置轴个数实现控件隐藏
        /// </summary>
        /// <param name="num"></param>
        private void AxisVisible(int num)
        {
            try
            {
                switch(num)
                {
                    case 1:
                        lblAxis1.Visible = true;
                        lblCard1.Visible = true;
                        lblAxis2.Visible = false;
                        lblCard2.Visible = false;

                        lblX.Visible = true;
                        lblY.Visible = false;
                        lblZ.Visible = false;
                        lblU.Visible = false;

                        cmbAxisX.Visible = true;
                        cmbAxisY.Visible = false;
                        cmbAxisZ.Visible = false;
                        cmbAxisU.Visible = false;
                        
                        cmbCardX.Visible = true;
                        cmbCardY.Visible = false;
                        cmbCardZ.Visible = false;
                        cmbCardU.Visible = false;
                        break;
                    case 2:
                        lblAxis1.Visible = true;
                        lblCard1.Visible = true;
                        lblAxis2.Visible = false;
                        lblCard2.Visible = false;

                        lblX.Visible = true;
                        lblY.Visible = true;
                        lblZ.Visible = false;
                        lblU.Visible = false;

                        cmbAxisX.Visible = true;
                        cmbAxisY.Visible = true;
                        cmbAxisZ.Visible = false;
                        cmbAxisU.Visible = false;

                        cmbCardX.Visible = true;
                        cmbCardY.Visible = true;
                        cmbCardZ.Visible = false;
                        cmbCardU.Visible = false;
                        break;
                    case 3:
                        lblAxis1.Visible = true;
                        lblCard1.Visible = true;
                        lblAxis2.Visible = false;
                        lblCard2.Visible = false;

                        lblX.Visible = true;
                        lblY.Visible = true;
                        lblZ.Visible = true;
                        lblU.Visible = false;

                        cmbAxisX.Visible = true;
                        cmbAxisY.Visible = true;
                        cmbAxisZ.Visible = true;
                        cmbAxisU.Visible = false;

                        cmbCardX.Visible = true;
                        cmbCardY.Visible = true;
                        cmbCardZ.Visible = true;
                        cmbCardU.Visible = false;
                        break;
                    case 4:
                        lblAxis1.Visible = true;
                        lblCard1.Visible = true;
                        lblAxis2.Visible = true;
                        lblCard2.Visible = true;

                        lblX.Visible = true;
                        lblY.Visible = true;
                        lblZ.Visible = true;
                        lblU.Visible = true;

                        cmbAxisX.Visible = true;
                        cmbAxisY.Visible = true;
                        cmbAxisZ.Visible = true;
                        cmbAxisU.Visible = true;

                        cmbCardX.Visible = true;
                        cmbCardY.Visible = true;
                        cmbCardZ.Visible = true;
                        cmbCardU.Visible = true;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 显示工站信息到对应的控件中
        /// </summary>
        /// <param name="model">输入的工站Model</param>
        private void ShowStation(StationModel model)
        {
            try
            {
                txtStationId.Text = model.Id.ToString();
                txtStationName.Text = model.Name.ToString();
                cmbStationAxisNum.Text = model.AxisNum.ToString();

                if(model.Axis_X != null)
                {
                    cmbCardX.Text = model.Axis_X.cardIndex.ToString();
                    cmbAxisX.Text = model.Axis_X.Name + "_" +  model.Axis_X.aliasName;
                }
                else
                {
                    cmbAxisX.Text = "";
                }

                if (model.AxisNum > 1 && model.Axis_Y != null)
                {
                    cmbCardY.Text = model.Axis_Y.cardIndex.ToString();
                    cmbAxisY.Text = model.Axis_Y.Name + "_" + model.Axis_Y.aliasName;
                }
                else
                {
                    cmbAxisY.Text = "";
                }

                if(model.AxisNum > 2 && model.Axis_Z != null)
                {
                    cmbCardZ.Text = model.Axis_Z.cardIndex.ToString();
                    cmbAxisZ.Text = model.Axis_Z.Name + "_" + model.Axis_Z.aliasName;
                }
                else
                {
                    cmbAxisZ.Text = "";
                }

                if(model.AxisNum > 3 && model.Axis_U != null)
                {
                    cmbCardU.Text = model.Axis_U.cardIndex.ToString();
                    cmbAxisU.Text = model.Axis_U.Name + "_" + model.Axis_U.aliasName;
                }
                else
                {
                    cmbAxisU.Text = "";
                }

                if(model.CheckIOModel == null || model.CheckIOModel.Name == null)
                {
                    cmbCheckIO.Text = "";
                }
                else
                { 
                    cmbCheckIO.Text = model.CheckIOModel.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化控制卡的comboBox
        /// </summary>
        private void InitStationCmbox()
        {
            try
            {
                int cardNum = m_CardModel.CardModels.Count;
                for (int i = 0; i < cardNum; i++)
                {
                    cmbCardX.Items.Add(i);
                    cmbCardY.Items.Add(i);
                    cmbCardZ.Items.Add(i);
                    cmbCardU.Items.Add(i);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var comboBox = sender as ComboBoxEx;
                var selectCard = m_CardModel.CardModels.FirstOrDefault(x => x.Id == Int32.Parse(comboBox.Text));
                if (comboBox.Name == "cmbCardX")
                {
                    cmbAxisX.Items.Clear();
                    foreach (var item in selectCard.AxisParamModels)
                    {
                        cmbAxisX.Items.Add(item.Name + "_" + item.aliasName);
                    } 
                }
                else if(comboBox.Name == "cmbCardY")
                {
                    cmbAxisY.Items.Clear();
                    foreach (var item in selectCard.AxisParamModels)
                    {
                        cmbAxisY.Items.Add(item.Name + "_" + item.aliasName);
                    }
                }
                else if(comboBox.Name == "cmbCardZ")
                {
                    cmbAxisZ.Items.Clear();
                    foreach (var item in selectCard.AxisParamModels)
                    {
                        cmbAxisZ.Items.Add(item.Name + "_" + item.aliasName);
                    }
                }
                else if(comboBox.Name == "cmbCardU")
                {
                    cmbAxisU.Items.Clear();
                    foreach (var item in selectCard.AxisParamModels)
                    {
                        cmbAxisU.Items.Add(item.Name + "_" + item.aliasName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 导入导出数据

        //导出数据
        private void OutPuttoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string strPath = folderDialog.SelectedPath + "//" + "IO.csv";

                    DataSet ds = GetDataSetFromDataGridView(dataIO, "IO");
                    Export2CSV(ds, "IO", true, strPath);

                    MessageBoxEx.Show("导出成功到文件：" + strPath);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //导入数据
        private void InPuttoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    List<IOModel> listIo = new List<IOModel>();
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
                            if (strArr.Length < 8 || strArr[0].Contains("Id"))
                            {
                                continue;
                            }

                            IOModel ioModel = new IOModel();

                            ioModel.Id = int.Parse(strArr[0]);
                            ioModel.Name = strArr[1];
                            ioModel.cardIndex = Int32.Parse(strArr[2]);
                            ioModel.extIndex = Int32.Parse(strArr[3]);
                            ioModel.index = Int32.Parse(strArr[4]);
                            ioModel.enumIo = (EnumIO)Enum.Parse(typeof(EnumIO), strArr[5]);
                            ioModel.enumIoType = (EnumIOType)Enum.Parse(typeof(EnumIOType), strArr[6]);
                            ioModel.bReverse = bool.Parse(strArr[7]);

                            listIo.Add(ioModel);
                            
                        }
                    }

                    m_CardModel.IOModels = listIo;

                    MessageBoxEx.Show("导入成功");
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        //DataGridView数据转成DataSet
        public static DataSet GetDataSetFromDataGridView(DataGridView datagridview, string name)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //为了把Id与Name显示在最开头
            int index = 6;
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

        //数据保存到CSV
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

        //将指定的数据集中指定的表转换成CSV字符串
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
                    csvStr += column.ColumnName + ",";
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
                    csvStr += row[column].ToString() + ",";
                }
                csvStr = csvStr.Remove(csvStr.LastIndexOf(","), 1);
                csvStr += "\n";
            }
            return csvStr;
        }

        #endregion
    }
}
