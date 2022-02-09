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
using HalconDotNet;
using HalconView;

namespace ManagementView._3DViews
{    
    /// <summary>
    /// 被继承的子窗口
    /// </summary>
    public partial class UnitSetting : UserControl
    {
        //HSmartWindow m_hSmartWindow;
        /// <summary>
        /// 窗口的类型与 FeatureType名称一致
        /// </summary>
        public virtual string UnitType { get; set; }
                
        /// <summary>
        /// 窗口的描述
        /// </summary>
        public virtual string UnitDesp { get; set; }
                
        public CheckFeatureModel featureModel;
        /// <summary>
        /// 模块的内容
        /// </summary>
        public virtual CheckFeatureModel FeatureModel
        {
            get
            {
                return featureModel;
            }
            set
            {
                featureModel = value;

                cmbLayOut_UnitSetting.SelectedIndex = featureModel.LayOut;
                cmbAlaramGoTo_UnitSeting.Text = featureModel.AlarmGoTo;
            }
        }
        
        public UnitSetting()
        {
            InitializeComponent();
            txtDescription_UnitSetting.SkinTxt.TextChanged += txtDescription_SkinTxt_TextChanged;
        }

        string m_initDesc = "";//模块描述
        private void UnitSetting_Load(object sender, EventArgs e)
        {
            try
            {
                InitAlarmGoTo();
                txtDescription_UnitSetting.Text = FeatureModel.Description;
                m_initDesc = txtDescription_UnitSetting.Text;
            }
            catch (Exception ex)
            {
                
            }
        }

        //点击取消事件
        public static event EventHandler<object> CanelEvent;
        public void OnCanelEvent(object e)
        {
            EventHandler<object> handler = CanelEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_initDesc != FeatureModel.Description && !string.IsNullOrEmpty(FeatureModel.Description))//如果是描述有更改则去刷新界面
            {
                OnCanelEvent(null);
            }
        }
        
        //点击确认事件
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Action_Confirm();
        }
        
        //点击执行事件
        private void btnRun_Click(object sender, EventArgs e)
        {
            Action_Run();
        }

        /// <summary>
        /// 点击确认按钮执行的虚函数
        /// </summary>
        public virtual void Action_Confirm()
        {

        }

        /// <summary>
        /// 点击执行按钮执行的虚函数
        /// </summary>
        public virtual void Action_Run()
        {

        }
        
        /// <summary>
        /// 设置按钮的text是新增还是修改
        /// </summary>
        /// <param name="bAdd">是否是新增</param>
        public virtual void SetConfirmBtn(bool bAdd = true)
        {
            try
            {
                btnConfirm_UnitSetting.Text = bAdd ? "新增":"修改";
                if(bAdd)
                {
                    btnCancel_UnitSetting.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
                
        /// <summary>
        /// 隐藏结果输出控件
        /// </summary>
        public virtual void HideOutPut()
        {
            try
            {
                groupOutPut_UnitSetting.Visible = false;
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 隐藏图像控件
        /// </summary>
        public virtual void HideImageWindow()
        {
            try
            {
                panelView_UnitSetting.Visible = false;
                this.Width -= 278;
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("HideImageWindow", ex.Message);
            }
        } 

        public delegate void DelRunAction(SingleSequenceModel sequence, CheckFeatureModel item, HSmartWindow hWindow = null);
        /// <summary>
        /// 执行按钮的执行委托
        /// </summary>
        public static DelRunAction m_DelRunAction;

        /// <summary>
        /// 显示结果
        /// </summary>
        /// <param name="strResult">界面上显示的内容</param>
        public virtual void ShowResult(string strResult)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    txtOutPut_UnitSetting.Text = strResult;
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        /// <summary>
        /// 把显示图像窗口放到panel
        /// </summary>
        /// <param name="childView">图像窗口</param>
        public void AddImageWindow(UserControl childView)
        {
            try
            { 
                CommHelper.LayoutChildFillView(panelView_UnitSetting, childView);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("AddImageWindow", ex.Message);
            }
        }

        /// <summary>
        /// 设置图像Layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLayOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FeatureModel.LayOut = Int32.Parse(cmbLayOut_UnitSetting.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 设置报警跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAlaramGoTo_UnitSeting_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                FeatureModel.AlarmGoTo = cmbAlaramGoTo_UnitSeting.Text;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置模块的描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDescription_SkinTxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FeatureModel.Description = txtDescription_UnitSetting.Text;
            }
            catch (Exception ex)
            {

            }
        }

        public bool _isWindowVisible = true;
        /// <summary>
        /// 是否显示图像控件
        /// </summary>
        public bool IsWindowVisible
        {
            get
            {
                return _isWindowVisible;
            }
            set
            {
                _isWindowVisible = value;
                panelView_UnitSetting.Visible = value;
            }
        }

        /// <summary>
        /// 窗体大小变化时对应改变图像显示的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitSetting_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                int width = this.Size.Width;
                if(width > 1038)
                {
                    panelView_UnitSetting.Width += width - 1038;
                }
                else if(width == 1038)
                {
                    panelView_UnitSetting.Width = 378;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        /// <summary>
        /// 初始化报警跳转项
        /// </summary>
        private void InitAlarmGoTo()
        {
            try
            {
                ChildSequenceModel childModel = XMLController.XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == XMLController.XmlControl.sequenceSingle.BaseSeqName);
                if (childModel == null)
                {
                    return;
                }

                cmbAlaramGoTo_UnitSeting.Items.Clear();
                List<string> listStr = new List<string>();
                listStr.Add("");
                List<CheckFeatureModel> listModel = childModel.GetCheckFeatureList();
                foreach (var item in listModel)
                {
                    if(item.Index > FeatureModel.Index)
                    {
                        listStr.Add(item.Name);
                    }
                }

                cmbAlaramGoTo_UnitSeting.Items.AddRange(listStr.ToArray()); 

                cmbAlaramGoTo_UnitSeting.Text = featureModel.AlarmGoTo;
            }
            catch (Exception ex)
            {

            }
        }
         
    }
}
