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
using ManagementView.Comment;

namespace ManagementView.MotorView
{
    public partial class IoView : UserControl
    {
        public string m_CardPath = Application.StartupPath + "//Sequence//Card//";
        public ControlCardModel m_CardModel = new ControlCardModel();
        public IoView()
        {
            InitializeComponent();
        }

        List<IOModel> m_InModels = new List<IOModel>();
        List<IOModel> m_OutModels = new List<IOModel>();
        List<RelatIoModel> m_RelatIoModels = new List<RelatIoModel>();
        private void IoView_Load(object sender, EventArgs e)
        {
            try
            {
                m_CardPath += "Card.dsr";
                m_CardModel = XmlControl.controlCardModel;
                if (m_CardModel == null)
                {
                    m_CardModel = new ControlCardModel();
                }

                m_InModels = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输入 && !string.IsNullOrEmpty(x.Name));
                m_OutModels = m_CardModel.IOModels.FindAll(x => x.enumIo == EnumIO.通用输出 && !string.IsNullOrEmpty(x.Name));
                m_RelatIoModels = m_CardModel.RelatIoModels;

                InitInPutView();
                InitOutPutView();
                InitRelatView();

                superTabControl1.SelectedTabIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        InPutIoView[] m_InPutViewArr;
        private void InitInPutView()
        {
            try
            {
                if (m_InModels == null || m_InModels.Count == 0)
                {
                    return;
                }

                int posX = panelInPut.Location.X + 10;
                int posY = panelInPut.Location.Y + 10;

                int length = m_InModels.Count;
                m_InPutViewArr = new InPutIoView[length];
                for (int i = 0; i < length; i++)
                {
                    m_InPutViewArr[i] = new InPutIoView();

                    int pHeight = panelInPut.Height - 50; 
                     
                    int height = m_InPutViewArr[i].Height;
                    int width = m_InPutViewArr[i].Width;

                    int count = pHeight / (height + 10);

                    int ix = i / count;

                    int iy = i;
                    if(i >= count)
                    {
                        iy = i - count*ix;
                    }

                    m_InPutViewArr[i].Location = new Point(posX + ix*(width + 10), posY + iy * (height + 10));
                    m_InPutViewArr[i].IoModel = m_InModels[i];

                    panelInPut.Controls.Add(m_InPutViewArr[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        OutPutIoView[] m_OutPutViewArr;
        private void InitOutPutView()
        {
            try
            {
                if (m_OutModels == null || m_OutModels.Count == 0)
                {
                    return;
                }

                int posX = panelOutPut.Location.X + 10;
                int posY = panelOutPut.Location.Y + 10;

                int length = m_OutModels.Count;
                m_OutPutViewArr = new OutPutIoView[length];
                for (int i = 0; i < length; i++)
                {
                    m_OutPutViewArr[i] = new OutPutIoView();

                    int pHeight = panelOutPut.Height - 50;

                    int height = m_OutPutViewArr[i].Height;
                    int width = m_OutPutViewArr[i].Width;

                    int count = pHeight / (height + 10);

                    int ix = i / count;

                    int iy = i;
                    if (i >= count)
                    {
                        iy = i - count * ix;
                    }

                    m_OutPutViewArr[i].Location = new Point(posX + ix * (width + 10), posY + iy * (height + 10));
                    m_OutPutViewArr[i].IoModel = m_OutModels[i];

                    panelOutPut.Controls.Add(m_OutPutViewArr[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        OutInPutView[] m_OutInPutViewArr;
        private void InitRelatView()
        {
            try
            {
                if (m_RelatIoModels == null || m_RelatIoModels.Count == 0)
                {
                    return;
                }

                int posX = panelInOutPut.Location.X + 10;
                int posY = panelInOutPut.Location.Y + 10;

                int length = m_RelatIoModels.Count;
                m_OutInPutViewArr = new OutInPutView[length];
                for (int i = 0; i < length; i++)
                {
                    m_OutInPutViewArr[i] = new OutInPutView();

                    int pHeight = panelInOutPut.Height - 50;

                    int height = m_OutInPutViewArr[i].Height;
                    int width = m_OutInPutViewArr[i].Width;

                    int count = pHeight / (height + 10);

                    int ix = i / count;

                    int iy = i;
                    if (i >= count)
                    {
                        iy = i - count * ix;
                    }

                    m_OutInPutViewArr[i].Location = new Point(posX + ix * (width + 10), posY + iy * (height + 10));
                    m_OutInPutViewArr[i].OutIoModel_1 = m_RelatIoModels[i].OutIoModel;
                    m_OutInPutViewArr[i].InIoModel_1 = m_RelatIoModels[i].InIoModel1;
                    m_OutInPutViewArr[i].InIoModel_2 = m_RelatIoModels[i].InIoModel2;

                    if(m_RelatIoModels[i].InIoModel1.Name == m_RelatIoModels[i].InIoModel2.Name)
                    {
                        m_OutInPutViewArr[i].SetVisible();
                    }

                    panelInOutPut.Controls.Add(m_OutInPutViewArr[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                int length = 0;
                if (m_InPutViewArr != null)
                { 
                    length = m_InPutViewArr.Count();
                }
                 
                int length2 = 0;
                if (m_OutInPutViewArr != null)
                {
                    length2 = m_OutInPutViewArr.Count();
                } 

                int length3 = 0;
                if (m_OutPutViewArr != null)
                {
                    length3 = m_OutPutViewArr.Count();
                }

                if (btnMonitor.Text == "开始监控")
                {
                    btnMonitor.Text = "停止监控";
                   
                    for (int i = 0; i < length; i++)
                    {
                        m_InPutViewArr[i].Monitor = true;
                    }

                    for (int i = 0; i < length3; i++)
                    {
                        m_OutPutViewArr[i].Monitor = true;
                    }

                    for (int i = 0; i < length2; i++)
                    {
                        m_OutInPutViewArr[i].Monitor = true;
                    }
                }
                else if (btnMonitor.Text == "停止监控")
                {
                    btnMonitor.Text = "开始监控";

                    for (int i = 0; i < length; i++)
                    {
                        m_InPutViewArr[i].Monitor = false;
                    }

                    for (int i = 0; i < length3; i++)
                    {
                        m_OutPutViewArr[i].Monitor = false;
                    }

                    for (int i = 0; i < length2; i++)
                    {
                        m_OutInPutViewArr[i].Monitor = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIOConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ShowControl view = new ShowControl(new IOConfigView(), "Io配置");
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
