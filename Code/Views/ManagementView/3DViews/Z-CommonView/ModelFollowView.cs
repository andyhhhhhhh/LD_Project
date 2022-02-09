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

namespace ManagementView._3DViews.CommonView
{
    public partial class ModelFollowView : UserControl
    {
        private string modelForm;
        public string ModelForm
        {
            get
            {
                return modelForm;
            }
            set
            {
                modelForm = value;
                cmbModelForm.Text = value;
            } 
        }

        private bool isModelForm;
        public bool IsModelForm
        {
            get
            {
                return isModelForm;
            }
            set
            {
                isModelForm = value;
                chkIsModelFollow.Checked = value;
                cmbModelForm.Enabled = chkIsModelFollow.Checked;
            }

        }

        public ModelFollowView()
        {
            InitializeComponent();
        }

        private void ModelFollowView_Load(object sender, EventArgs e)
        {
            try
            {
                List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
                ChildSequenceModel childModel = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == XmlControl.sequenceSingle.BaseSeqName);
                foreach (var item in childModel.SingleSequenceModels)
                {
                    listModel.AddRange(item.CheckFeatureModels);
                } 
                var listModel2 = listModel.FindAll(x => x.featureType == FeatureType.特征匹配 ||
                                                                     x.featureType == FeatureType.灰度匹配 ||
                                                                     x.featureType == FeatureType.轮廓匹配 ||
                                                                     x.featureType == FeatureType.交点匹配 ||
                                                                     x.featureType == FeatureType.直线匹配 ||
                                                                     x.featureType == FeatureType.手绘模板).ToList();
                cmbModelForm.DataSource = listModel2;
                cmbModelForm.DisplayMember = "Name";

                //cmbModelForm.Text = ModelForm;
                //chkIsModelFollow.Checked = IsModelForm;
                //cmbModelForm.Enabled = chkIsModelFollow.Checked;

                //if(ModelForm == "")
                //{
                //    cmbModelForm.SelectedIndex = -1;
                //}
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void cmbModelForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModelForm = cmbModelForm.Text;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void chkIsModelFollow_CheckedChanged(object sender, EventArgs e)
        {
            IsModelForm = chkIsModelFollow.Checked;
            cmbModelForm.Enabled = chkIsModelFollow.Checked;
            btnDisplayPic.Visible = chkIsModelFollow.Checked;
        }

        //定义委托
        public delegate void DisplayModelPicDel(object sender, EventArgs e);
        //定义事件
        public event DisplayModelPicDel DisplayModelPicEvent;
        private void btnDisplayPic_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayModelPicEvent(cmbModelForm.Text, null);
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
