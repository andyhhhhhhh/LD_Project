using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using SequenceTestModel;

namespace ManagementView
{
    /// <summary>
    /// 参数配置界面
    /// </summary>
    public partial class ParamSetView : UserControl
    {
        public static event EventHandler<object> ConfirmEvent;
        protected void OnConfirmEvent(object e)
        {
            EventHandler<object> handler = ConfirmEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public ParamSetView()
        {
            InitializeComponent();
        }

        private void ParamSetView_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateData();

                SetUserEnable();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBoxEx.Show(this, "是否保存当前配置?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
                LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                                
                cosModel.DeviceID = txtDeviceID.sText;
                cosModel.IsShieldVacuum = chkIsShieldVacuum.Checked;
                cosModel.IsShieldClamp = chkIsShieldClasp.Checked;
                cosModel.IsEmptyRun = chkIsEmptyRun.Checked;
                cosModel.IsShieldDoor = chkIsShieldDoor.Checked;
                cosModel.IsShieldDownCamera = chkIsShieldDownCamera.Checked;
                cosModel.IsShieldUnClasp = chkIsShieldUnClamp.Checked;
                cosModel.IsShieldCylinderUp = chkIsShieldCylinderUp.Checked;
                cosModel.IsShieldOCR = chkIsShieldOCR.Checked;
                cosModel.IsShieldMes = chkIsShieldMes.Checked;

                cosModel.SnapTimeOut = Int32.Parse(txtSnapTimeOut.sText);
                cosModel.VacuumTimeOut = Int32.Parse(txtVacuumTimeOut.sText);
                cosModel.VacuumDelayTime = Int32.Parse(txtVacuumDelayTime.sText);
                cosModel.AxisInPlaceTimeOut = Int32.Parse(txtAxisInPlaceTimeOut.sText);
                cosModel.VacuumBreakDelay = Int32.Parse(txtVacuumBreakDelay.sText);
                cosModel.OperatorID = txtOperatorID.sText;
                cosModel.CylinderDelay = Int32.Parse(txtCylinderDelay.sText);
                cosModel.AxisInPlaceDelay = Int32.Parse(txtAxisInPlaceDelay.sText);
                cosModel.ContinueNGCount = Int32.Parse(txtContinueNGCount.sText);

                cosModel.TrayModelInch2 = GetTrayModel();
                OnConfirmEvent(null);
                MessageBoxEx.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 更新信息到界面
        /// </summary>
        public void UpdateData()
        {
            try
            {
                LDModel ldModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                txtDeviceID.sText = ldModel.DeviceID;
                chkIsShieldVacuum.Checked = ldModel.IsShieldVacuum;
                chkIsShieldClasp.Checked = ldModel.IsShieldClamp;
                chkIsEmptyRun.Checked = ldModel.IsEmptyRun;
                chkIsShieldDoor.Checked = ldModel.IsShieldDoor;
                chkIsShieldDownCamera.Checked = ldModel.IsShieldDownCamera;
                chkIsShieldUnClamp.Checked = ldModel.IsShieldUnClasp;
                chkIsShieldCylinderUp.Checked = ldModel.IsShieldCylinderUp;
                chkIsShieldOCR.Checked = ldModel.IsShieldOCR;
                chkIsShieldMes.Checked = ldModel.IsShieldMes;
                
                txtSnapTimeOut.sText = ldModel.SnapTimeOut.ToString();
                txtVacuumTimeOut.sText = ldModel.VacuumTimeOut.ToString();
                txtVacuumDelayTime.sText = ldModel.VacuumDelayTime.ToString();
                txtAxisInPlaceTimeOut.sText = ldModel.AxisInPlaceTimeOut.ToString();
                txtVacuumBreakDelay.sText = ldModel.VacuumBreakDelay.ToString();
                txtOperatorID.sText = ldModel.OperatorID.ToString();
                txtCylinderDelay.sText = ldModel.CylinderDelay.ToString();
                txtAxisInPlaceDelay.sText = ldModel.AxisInPlaceDelay.ToString();
                txtContinueNGCount.sText = ldModel.ContinueNGCount.ToString();

                SetUserEnable();
            }
            catch (Exception ex)
            {
                
            }
        }
               
        /// <summary>
        /// 获取当前料盘配置参数
        /// </summary>
        /// <returns>料盘Model</returns>
        private TrayModel GetTrayModel()
        {
            TrayModel trayModel = new TrayModel();
            try
            {
                return trayModel;
            }
            catch (Exception)
            {
                return trayModel;
            }
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// 重置行列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetConfig();

                OnConfirmEvent(null);

                MessageBoxEx.Show("重置行列成功!!!");
            }
            catch (Exception ex)
            {

            }
        }

        public void ResetConfig()
        {
            try
            {
                LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;

                cosModel.TrayModelInch2.ProductCurrentRow = 1;
                cosModel.TrayModelInch2.ProductCurrentCol = 1;
                cosModel.TrayModelInch2.TrayCurrentRow = 1;
                cosModel.TrayModelInch2.TrayCurrentCol = 1;

                UpdateData();
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void SetUserEnable()
        {
            try
            {
                bool bEnable = GlobalCore.Global.UserName != GlobalCore.Global.OperatorName;
               
                chkIsEmptyRun.Enabled = bEnable;
                chkIsShieldDownCamera.Enabled = bEnable;
                chkIsShieldCylinderUp.Enabled = bEnable;
                chkIsShieldClasp.Enabled = bEnable;
                chkIsShieldUnClamp.Enabled = bEnable;
                chkIsShieldOCR.Enabled = bEnable;
                chkIsShieldDoor.Enabled = bEnable;
                chkIsShieldVacuum.Enabled = bEnable;

                txtSnapTimeOut.Enabled = bEnable;
                txtVacuumDelayTime.Enabled = bEnable;
                txtVacuumBreakDelay.Enabled = bEnable;
                txtAxisInPlaceDelay.Enabled = bEnable;
                txtCylinderDelay.Enabled = bEnable;
                txtVacuumTimeOut.Enabled = bEnable;
                txtCylinderDelay.Enabled = bEnable;
                txtAxisInPlaceTimeOut.Enabled = bEnable;
                txtContinueNGCount.Enabled = bEnable;
            }
            catch (Exception ex)
            {
                 
            }
        }
        
    }
}
