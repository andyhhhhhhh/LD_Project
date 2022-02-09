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
using DevComponents.DotNetBar;

namespace ManagementView.MotorView
{
    public partial class IOConfigView : UserControl
    { 
        public ControlCardModel m_CardModel = new ControlCardModel();


        List<IOModel> m_InModels = new List<IOModel>();
        List<IOModel> m_OutModels = new List<IOModel>(); 
        public IOConfigView()
        {
            InitializeComponent();
        }

        private void IOConfigView_Load(object sender, EventArgs e)
        {
            try
            { 
                m_CardModel = XmlControl.controlCardModel;
                
                m_InModels = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输入 && !string.IsNullOrEmpty(x.Name));
                m_OutModels = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输出 && !string.IsNullOrEmpty(x.Name));

                cmbIOType.DataSource = Enum.GetNames(typeof(EnumMonitorIO));
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbIOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EnumMonitorIO enumMonitor = (EnumMonitorIO)Enum.Parse(typeof(EnumMonitorIO), cmbIOType.Text);
                switch(enumMonitor)
                {
                    case EnumMonitorIO.通用输入:
                        cmbIOName.DataSource = m_InModels;
                        cmbIOName.DisplayMember = "Name";
                        groupRelate.Visible = false;
                        break;
                    case EnumMonitorIO.通用输出:
                        cmbIOName.DataSource = m_OutModels;
                        cmbIOName.DisplayMember = "Name";
                        groupRelate.Visible = false;
                        break;

                    case EnumMonitorIO.输入输出关联:
                        cmbIOName.DataSource = m_OutModels;
                        cmbIOName.DisplayMember = "Name";

                        var inModel1 = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输入 && !string.IsNullOrEmpty(x.Name));
                        cmbInIO_1.DataSource = inModel1;
                        cmbInIO_1.DisplayMember = "Name";

                        var inModel2 = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输入 && !string.IsNullOrEmpty(x.Name));
                        cmbInIO_2.DataSource = inModel2;
                        cmbInIO_2.DisplayMember = "Name";

                        groupRelate.Visible = true;
                        InitList();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EnumMonitorIO enumMonitor = (EnumMonitorIO)Enum.Parse(typeof(EnumMonitorIO), cmbIOType.Text);
                switch (enumMonitor)
                {
                    case EnumMonitorIO.通用输入: 
                        break;
                    case EnumMonitorIO.通用输出:
                        break;

                    case EnumMonitorIO.输入输出关联:
                        IOModel outModel = cmbIOName.SelectedItem as IOModel;
                        IOModel InModel_1 = cmbInIO_1.SelectedItem as IOModel;
                        IOModel InModel_2 = cmbInIO_2.SelectedItem as IOModel;
                        RelatIoModel relatIO = new RelatIoModel()
                        {
                            Name = outModel.Name,
                            OutIoModel = outModel,
                            InIoModel1 = InModel_1,
                            InIoModel2 = InModel_2
                        };
                        
                        if(m_CardModel.RelatIoModels.FindIndex(x=>x.Name == outModel.Name) != -1)
                        {
                            MessageBoxEx.Show("新增输出名称已存在!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        m_CardModel.RelatIoModels.Add(relatIO);

                        InitList();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                EnumMonitorIO enumMonitor = (EnumMonitorIO)Enum.Parse(typeof(EnumMonitorIO), cmbIOType.Text);
                switch (enumMonitor)
                {
                    case EnumMonitorIO.通用输入:
                        break;
                    case EnumMonitorIO.通用输出:
                        break;

                    case EnumMonitorIO.输入输出关联:
                        RelatIoModel model = listBox1.SelectedItem as RelatIoModel;
                        m_CardModel.RelatIoModels.Remove(model);

                        InitList();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitList()
        {
            try
            {
                if(m_CardModel.RelatIoModels != null && m_CardModel.RelatIoModels.Count > 0)
                {
                    listBox1.DataSource = null;
                    listBox1.DataSource = m_CardModel.RelatIoModels;
                    listBox1.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RelatIoModel model = listBox1.SelectedItem as RelatIoModel;
                cmbIOName.Text = model.OutIoModel.Name;
                cmbInIO_1.Text = model.InIoModel1.Name;
                cmbInIO_2.Text = model.InIoModel2.Name;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
