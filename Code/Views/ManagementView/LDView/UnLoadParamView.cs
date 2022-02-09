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
using DevComponents.DotNetBar;

namespace ManagementView
{
    /// <summary>
    /// 下料料盘参数界面
    /// </summary>
    public partial class UnLoadParamView : UserControl
    {
        BlowingBoxView m_blowingView;
        public UnLoadParamView()
        {
            InitializeComponent();
        }

        private void UnLoadParamView_Load(object sender, EventArgs e)
        {
            try
            {
                m_blowingView = new BlowingBoxView();
                CommHelper.LayoutChildFillView(panelBlow, m_blowingView);

                UpdateData();
                eLight1.ComName = "光源串口";
                eLight2.ComName = "光源串口";
                SetUserEnable();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 下料更新配置
        /// </summary>
        public void UpdateData()
        {
            try
            {
                LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                UnLoadTrayModel unLoadTray = cosModel.unLoadTrayModel;
              
                SetBtnEnable();
                if (unLoadTray != null)
                {
                    numProductRowCount.Value = unLoadTray.ProductRowCount;
                    numProductColCount.Value = unLoadTray.ProductColCount;
                    numProductRowDis.Value = unLoadTray.ProductRowDis;
                    numProductColDis.Value = unLoadTray.ProductColDis;
                    numTrayRowDis.Value = unLoadTray.TrayRowDis;
                    numTrayColDis.Value = unLoadTray.TrayColDis;
                    numTrayXOffSet.Value = unLoadTray.TrayXOffSet;
                    numTrayYOffSet.Value = unLoadTray.TrayYOffSet;

                    numPassTrayCurrentNum.Value = unLoadTray.PassModel.TrayCurrentNum;
                    numPassProductCurrentRow.Value = unLoadTray.PassModel.ProductCurrentRow;
                    numPassProductCurrentCol.Value = unLoadTray.PassModel.ProductCurrentCol;

                    numNGTrayCurrentNum.Value = unLoadTray.FailModel.TrayCurrentNum;
                    numFailProductCurrentRow.Value = unLoadTray.FailModel.ProductCurrentRow;
                    numFailProductCurrentCol.Value = unLoadTray.FailModel.ProductCurrentCol;

                    numSeemNGTrayCurrentNum.Value = unLoadTray.SeemNGModel.TrayCurrentNum;
                    numSeemNGCurrentRow.Value = unLoadTray.SeemNGModel.ProductCurrentRow;
                    numSeemNGCurrentCol.Value = unLoadTray.SeemNGModel.ProductCurrentCol;

                    m_blowingView.SetBlowingList(cosModel.listBlowType1);
                }
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
                if(result == DialogResult.No)
                {
                    return;
                }
                LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                UnLoadTrayModel unLoadTray = cosModel.unLoadTrayModel;
                if (unLoadTray == null)
                {
                    unLoadTray = new UnLoadTrayModel();
                }

                unLoadTray.ProductRowCount = numProductRowCount.Value;
                unLoadTray.ProductColCount = numProductColCount.Value;
                unLoadTray.ProductRowDis = numProductRowDis.Value;
                unLoadTray.ProductColDis = numProductColDis.Value;
                unLoadTray.TrayRowDis = numTrayRowDis.Value;
                unLoadTray.TrayColDis = numTrayColDis.Value;
                unLoadTray.TrayXOffSet = numTrayXOffSet.Value;
                unLoadTray.TrayYOffSet = numTrayYOffSet.Value;

                if(unLoadTray.PassModel == null)
                {
                    unLoadTray.PassModel = new UnLoadModel();
                }
                unLoadTray.PassModel.Name = "OK料盘";
                unLoadTray.PassModel.TrayCurrentNum = numPassTrayCurrentNum.Value;
                unLoadTray.PassModel.ProductCurrentRow = numPassProductCurrentRow.Value;
                unLoadTray.PassModel.ProductCurrentCol = numPassProductCurrentCol.Value;

                if (unLoadTray.FailModel == null)
                {
                    unLoadTray.FailModel = new UnLoadModel();
                }
                unLoadTray.FailModel.Name = "NG料盘";
                unLoadTray.FailModel.TrayCurrentNum = numNGTrayCurrentNum.Value;
                unLoadTray.FailModel.ProductCurrentRow = numFailProductCurrentRow.Value;
                unLoadTray.FailModel.ProductCurrentCol = numFailProductCurrentCol.Value;

                 
                if (unLoadTray.SeemNGModel == null)
                {
                    unLoadTray.SeemNGModel = new UnLoadModel();
                }
                unLoadTray.SeemNGModel.Name = "疑似NG料盘";
                unLoadTray.SeemNGModel.TrayCurrentNum = numSeemNGTrayCurrentNum.Value;
                unLoadTray.SeemNGModel.ProductCurrentRow = numSeemNGCurrentRow.Value;
                unLoadTray.SeemNGModel.ProductCurrentCol = numSeemNGCurrentCol.Value;
                
                cosModel.unLoadTrayModel = unLoadTray;

                cosModel.listBlowType1 = m_blowingView.GetBlowingList();

                MessageBoxEx.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// 重置当前序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBoxEx.Show(this, "是否重置所有配置?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
                ResetConfig(); 
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 重置当前配置序号
        /// </summary>
        public void ResetConfig()
        {
            LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;
            UnLoadTrayModel unLoadTray = cosModel.unLoadTrayModel;
            if (unLoadTray != null)
            {
                List<EnumCosResult> listCosResult = new List<EnumCosResult>();

                listCosResult = cosModel.listBlowType1;


                int index = listCosResult.FindIndex(x => x == EnumCosResult.Enum_Pass);
                if (index != -1)
                {
                    unLoadTray.PassModel.TrayCurrentNum = index + 1;
                    unLoadTray.PassModel.ProductCurrentRow = 1;
                    unLoadTray.PassModel.ProductCurrentCol = 1;
                }
                else
                {
                    unLoadTray.PassModel.TrayCurrentNum = 0;
                    unLoadTray.PassModel.ProductCurrentRow = 0;
                    unLoadTray.PassModel.ProductCurrentCol = 0;
                }

                index = listCosResult.FindIndex(x => x == EnumCosResult.Enum_Fail);
                if (index != -1)
                {
                    unLoadTray.FailModel.TrayCurrentNum = index + 1;
                    unLoadTray.FailModel.ProductCurrentRow = 1;
                    unLoadTray.FailModel.ProductCurrentCol = 1;
                }
                
                index = listCosResult.FindIndex(x => x == EnumCosResult.Enum_SeemNG);
                if (index != -1)
                {
                    unLoadTray.SeemNGModel.TrayCurrentNum = index + 1;
                    unLoadTray.SeemNGModel.ProductCurrentRow = 1;
                    unLoadTray.SeemNGModel.ProductCurrentCol = 1;
                }
                else
                {
                    unLoadTray.SeemNGModel.TrayCurrentNum = 0;
                    unLoadTray.SeemNGModel.ProductCurrentRow = 0;
                    unLoadTray.SeemNGModel.ProductCurrentCol = 0;
                }
                
            } 

            UpdateData();
        }

        public void SetUserEnable()
        {
            try
            {
                bool bEnable = GlobalCore.Global.UserName != GlobalCore.Global.OperatorName;
                numProductRowCount.Enabled = bEnable;
                numProductColCount.Enabled = bEnable;
                numProductRowDis.Enabled = bEnable;
                numProductColDis.Enabled = bEnable;
                numTrayXOffSet.Enabled = bEnable;
                numTrayYOffSet.Enabled = bEnable;
                numTrayRowDis.Enabled = bEnable; 
                numTrayColDis.Enabled = bEnable;
            }
            catch (Exception ex)
            {

            }
        }
        
        public void SetBtnEnable()
        {
            try
            {
                LDModel cosModel = XMLController.XmlControl.sequenceModelNew.LDModel;
                UnLoadTrayModel unLoadTray = cosModel.unLoadTrayModel;
                if (unLoadTray == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
    }
}
