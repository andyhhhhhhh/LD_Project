using AlgorithmController;
using DevComponents.DotNetBar;
using JsonController;
using Microsoft.Win32;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView._3DViews
{
    public partial class SetView : Form
    {
        JsonControl m_jsonControl = new JsonControl();
        Systemparameters m_systemParam = new Systemparameters();

        EnumCard m_CardEnum;

        public static event EventHandler<object> ConfirmEvent;
        protected void OnConfirmEvent(object e)
        {
            EventHandler<object> handler = ConfirmEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public SetView()
        {
            InitializeComponent();
        }

        private void SetView_Load(object sender, EventArgs e)
        {
            try
            {
                var listModel =  XMLController.XmlControl.sequenceModelNew.ChildSequenceModels;
                List<string> listStr = new List<string>();
                foreach (var item in listModel)
                {
                    cmbMainProcess.Items.Add(item.AnotherName);
                    cmbResetProcess.Items.Add(item.AnotherName);
                }

                m_systemParam = m_jsonControl.ParseJsonFileAction();
                
                txtFileDay.Text = m_systemParam.SaveDays.ToString();
                txtPath.Text = m_systemParam.SavePath;
                txtMaxCapaity.Text = m_systemParam.MaxCapity.ToString();
                chkMinToPallet.Checked = m_systemParam.MinToPallet;
                chkOpenAutoRun.Checked = m_systemParam.OpenAutonRun;
                chkOpenMany.Checked = m_systemParam.OpenMany;
                chkEnableOsk.Checked = m_systemParam.EnableOsk;
                chkEnableRunView.Checked = m_systemParam.EnableRunView;
                chkRunOtherSoft.Checked = m_systemParam.IsRunOtherSoft;
                txtSoftAddr.Text = m_systemParam.OtherSoftAddr;
                chkIsSaveImg.Checked = m_systemParam.IsSaveImg;
                chkIsPrintLog.Checked = m_systemParam.IsPrintLog;
                chkIsInitDevice.Checked = m_systemParam.IsInitDevice;
                chkIsItemVisible.Checked = m_systemParam.IsItemVisible;
                chkIsProVisible.Checked = m_systemParam.IsProVisible;
                chkIsRealDisplay.Checked = m_systemParam.IsRealDisplay;
                chkIsShowCross.Checked = m_systemParam.IsShowCross;
                chkIsStopShowMsg.Checked = m_systemParam.IsStopShowMsg;
                cmbResetProcess.Text = m_systemParam.ResetProcess;
                cmbMainProcess.Text = m_systemParam.MainProcess;
                txtImagePath.Text = m_systemParam.ImagePath;
                chkIsSaveNGImage.Checked = m_systemParam.IsSaveNGImg;
                chkIsFullScreen.Checked = m_systemParam.IsFullScreen;

                if(!string.IsNullOrEmpty(m_systemParam.MotionCard))
                {
                    m_CardEnum = (EnumCard)Enum.Parse(typeof(EnumCard), m_systemParam.MotionCard);
                    SetMotion();
                }

                txtViewName.Text = GlobalCore.Global.GetNodeValue("ViewName");

                listBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            { 

            }
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                m_systemParam.SaveDays = Int32.Parse(txtFileDay.Text);
                m_systemParam.SavePath = txtPath.Text;
                m_systemParam.MaxCapity = Int32.Parse(txtMaxCapaity.Text);
                m_systemParam.OpenAutonRun = chkOpenAutoRun.Checked;
                m_systemParam.OpenMany = chkOpenMany.Checked;
                m_systemParam.MinToPallet = chkMinToPallet.Checked;
                m_systemParam.EnableOsk = chkEnableOsk.Checked;
                m_systemParam.EnableRunView = chkEnableRunView.Checked;
                m_systemParam.IsRunOtherSoft = chkRunOtherSoft.Checked;
                m_systemParam.OtherSoftAddr = txtSoftAddr.Text;
                m_systemParam.IsPrintLog = chkIsPrintLog.Checked;
                m_systemParam.IsInitDevice = chkIsInitDevice.Checked;
                m_systemParam.IsItemVisible = chkIsItemVisible.Checked;
                m_systemParam.IsProVisible = chkIsProVisible.Checked;
                m_systemParam.IsRealDisplay = chkIsRealDisplay.Checked;
                m_systemParam.IsSaveImg = chkIsSaveImg.Checked;
                m_systemParam.IsShowCross = chkIsShowCross.Checked;
                m_systemParam.IsStopShowMsg = chkIsStopShowMsg.Checked;
                m_systemParam.MainProcess = cmbMainProcess.Text;
                m_systemParam.ResetProcess = cmbResetProcess.Text;
                m_systemParam.MotionCard = m_CardEnum.ToString();
                m_systemParam.IsSaveNGImg = chkIsSaveNGImage.Checked;
                m_systemParam.IsFullScreen = chkIsFullScreen.Checked;
                m_systemParam.ImagePath = txtImagePath.Text;

                m_jsonControl.SystemPara = m_systemParam;
                m_jsonControl.SaveConfigurationClassToJsonFile();
 

                Fun_AutoStart(m_systemParam.OpenAutonRun);

                OnConfirmEvent(null);

                GlobalCore.Global.SaveNodeValue("ViewName", txtViewName.Text);
                MessageBoxEx.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 开机自启
        /// </summary>
        public static void Fun_AutoStart(bool isAutoRun = true)
        {
            try
            {
                //string path = Application.ExecutablePath;
                string path = Application.StartupPath + "\\start.bat";
                if(!File.Exists(path))
                {
                    return;
                }
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    rk2.SetValue("MainView", string.Format("\"{0}\"", path)); //rk2.DeleteValue("OIMSServer", false);
                else
                    rk2.DeleteValue("MainView", false);
                rk2.Close();
                rk.Close();
            }
            catch
            {
                MessageBoxEx.Show("开机自动启动服务注册被拒绝!请确认有系统管理员权限!");
            }
        } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtPath.Text = foldPath;
            }
        }

        private void btnSelectSeqence_Click(object sender, EventArgs e)
        {
            try
            {
                SelectSequenceView view = new SelectSequenceView();
                view.Show();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnSelectSeqence_Click", ex.Message);    
            }
        }

        private void btnLoadAddr_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "";
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                //openFileDialog.InitialDirectory = "d:\\";
                openFileDialog.Filter = "exe文件|*.exe";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                fileName = openFileDialog.FileName;
                txtSoftAddr.Text = fileName;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void chkRunOtherSoft_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblSoftAddr.Enabled = chkRunOtherSoft.Checked;
                txtSoftAddr.Enabled = chkRunOtherSoft.Checked;
                btnLoadAddr.Enabled = chkRunOtherSoft.Checked;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnDelImage_Click(object sender, EventArgs e)
        {
            AlgorithmCommHelper.DeleteDir(m_systemParam.SavePath, m_systemParam.SaveDays);
            MessageBoxEx.Show("删除完成");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                switch(index)
                {
                    case 0:
                        panel_BaseSet.Visible = true;
                        panel_BaseSet.Dock = DockStyle.Top;
                        panel_LogSet.Visible = false;
                        panel_LogSet.Dock = DockStyle.None;
                        panel_OtherSet.Visible = false;
                        panel_OtherSet.Dock = DockStyle.None;
                        panel_BaseSet.Height = 388;
                        break;
                    case 1:
                        panel_BaseSet.Visible = false;
                        panel_BaseSet.Dock = DockStyle.None;
                        panel_LogSet.Visible = true;
                        panel_LogSet.Dock = DockStyle.Top;
                        panel_OtherSet.Visible = false;
                        panel_OtherSet.Dock = DockStyle.None;
                        panel_LogSet.Height = 388;
                        break;
                    case 2:
                        panel_BaseSet.Visible = false;
                        panel_BaseSet.Dock = DockStyle.None;
                        panel_LogSet.Visible = false;
                        panel_LogSet.Dock = DockStyle.None;
                        panel_OtherSet.Visible = true;
                        panel_OtherSet.Dock = DockStyle.Top;
                        panel_OtherSet.Height = 388;
                        break;

                    default:
                        panel_BaseSet.Visible = true;
                        panel_BaseSet.Dock = DockStyle.Top;
                        panel_LogSet.Visible = false;
                        panel_LogSet.Dock = DockStyle.None;
                        panel_OtherSet.Visible = false;
                        panel_OtherSet.Dock = DockStyle.None;
                        panel_BaseSet.Height = 388;
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        private void btnLoadImagePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtImagePath.Text = foldPath;
            }
        }

        #region 运动控制卡选择  
        private void radioZMotion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioZMotion.Checked)
            {
                m_CardEnum = EnumCard.正运动;
            }
        }

        private void radioSoftPLC_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSoftPLC.Checked)
            {
                m_CardEnum = EnumCard.软PLC;
            }
        }

        private void radioVirtual_CheckedChanged(object sender, EventArgs e)
        {
            if (radioVirtual.Checked)
            {
                m_CardEnum = EnumCard.虚拟卡;
            }
        }

        private void SetMotion()
        {
            try
            {
                switch (m_CardEnum)
                { 
                    case EnumCard.正运动:
                        radioZMotion.Checked = true;
                        break;
                    case EnumCard.软PLC:
                        radioSoftPLC.Checked = true;
                        break;
                    case EnumCard.虚拟卡:
                        radioVirtual.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

       
    }
}
