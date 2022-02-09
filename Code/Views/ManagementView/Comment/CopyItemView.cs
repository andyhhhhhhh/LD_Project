using DevComponents.DotNetBar;
using SequenceTestModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;

namespace ManagementView.Comment
{
    /// <summary>
    /// 不同流程单元复制项的窗口
    /// </summary>
    public partial class CopyItemView : Form
    {   
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
        public CopyItemView()
        {
            InitializeComponent();
        }

        private void CopyItemView_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedTabIndex = 0;

                cmbSeq.Text = XmlControl.sequenceSingle.Name;
                cmbSeq2.Text = XmlControl.sequenceSingle.Name;

                //var listCopyItem = XmlControl.sequenceModelNew.SingleSequenceModels.FindAll(x => x.Name != cmbSeq.Text).ToList();
                var listCopyItem = XmlControl.sequenceModelNew.ChildSequenceModels;

                cmbCopySeq.DataSource = listCopyItem;
                cmbCopySeq.DisplayMember = "AnotherName";


                txtProjectPath.SkinTxt.TextChanged += txtProjectPath_TextChanged;
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        private void cmbCopySeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChildSequenceModel model = cmbCopySeq.SelectedItem as ChildSequenceModel;

                List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
                foreach (var item in model.SingleSequenceModels)
                {
                    listModel.AddRange(item.CheckFeatureModels);
                }
                cmbStartItem.DataSource = listModel;
                cmbStartItem.DisplayMember = "Name";
            }
            catch (Exception ex)
            { 

            }
        }

        private void cmbStartItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var listModels = cmbStartItem.DataSource as List<CheckFeatureModel>;

                var selectModel = cmbStartItem.SelectedItem as CheckFeatureModel;

                var listEndModel = listModels.FindAll(x => x.Index >= selectModel.Index).ToList();

                cmbEndItem.DataSource = listEndModel;
                cmbEndItem.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ChildSequenceModel copyseq = cmbCopySeq.SelectedItem as ChildSequenceModel; 
                  
                var startModel = cmbStartItem.SelectedItem as CheckFeatureModel;
                var endModel = cmbEndItem.SelectedItem as CheckFeatureModel;

                var copyModels = copyseq.GetCheckFeatureList().FindAll(x => x.Index >= startModel.Index && x.Index <= endModel.Index).ToList();

                int startId = XmlControl.sequenceSingle.CheckFeatureModels.Count;
                int index = startId + 1;
                foreach (var item in copyModels)
                {
                    ChildSequenceModel selChild = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == XmlControl.sequenceSingle.BaseSeqName);
                    CheckFeatureModel addModel = item.Clone();
                    addModel.Id = index++;
                    addModel.Name = addModel.Name.Replace(copyseq.Name, selChild.Name);

                    //如果存在此名称则重新设置
                    if (selChild.GetCheckFeatureList().FindIndex(x=>x.Name == addModel.Name) != -1)
                    {
                        addModel.Name = XmlControl.SetName(XmlControl.sequenceSingle, addModel.featureType.ToString());
                    }

                    //是否设置图像窗口
                    if(chkIsSetLayOut.Checked)
                    {
                        addModel.LayOut = cmbLayOut.SelectedIndex;
                    }
                     
                    XmlControl.sequenceSingle.CheckFeatureModels.Add(addModel);

                    SingleSequenceModel seq = copyseq.SingleSequenceModels.FirstOrDefault(x => x.CheckFeatureModels.Contains(item)); 
                    CopyItem(seq, item, addModel.Name);
                }

                OnCanelEvent(null);
                MessageBoxEx.Show("复制完成!!!");

            }
            catch (Exception ex)
            {
                 
            }
        }

        public void CopyItem(SingleSequenceModel sequence, CheckFeatureModel tModel, string strname)
        {
            try
            {
                //利用反射实现复制项目 
                object baseModel = XmlControl.GetModel(tModel, sequence);
                object listObj = XmlControl.sequenceSingle.GetType().GetProperty(baseModel.GetType().Name + "s").GetValue(XmlControl.sequenceSingle);

                IList ilist = listObj as IList;

                //先实现Clone
                var method = baseModel.GetType().GetMethod("Clone");
                object t = method.Invoke(baseModel, new object[] { });

                //设置名称
                t.GetType().GetProperty("Name").SetValue(t, strname);
                if (!ilist.Contains(t))
                {
                    ilist.Add(t);
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        private void chkIsSetLayOut_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cmbLayOut.Enabled = chkIsSetLayOut.Checked;
            }
            catch (Exception ex)
            {
                 
            }
        }
        

        #region 跨项目复制
        private void btnConfirm2_Click(object sender, EventArgs e)
        {
            try
            {
                ChildSequenceModel copyseq = cmbCopySeq2.SelectedItem as ChildSequenceModel;
                var startModel = cmbStartItem2.SelectedItem as CheckFeatureModel;
                var endModel = cmbEndItem2.SelectedItem as CheckFeatureModel;

                var copyModels = copyseq.GetCheckFeatureList().FindAll(x => x.Index >= startModel.Index && x.Index <= endModel.Index).ToList();

                int startId = XmlControl.sequenceSingle.CheckFeatureModels.Count;
                int index = startId + 1;
                foreach (var item in copyModels)
                {
                    ChildSequenceModel selChild = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == XmlControl.sequenceSingle.BaseSeqName);
                    CheckFeatureModel addModel = item.Clone();
                    addModel.Id = index++;
                    addModel.Name = addModel.Name.Replace(copyseq.Name, selChild.Name);

                    //如果存在此名称则重新设置
                    if (selChild.GetCheckFeatureList().FindIndex(x => x.Name == addModel.Name) != -1)
                    {
                        addModel.Name = XmlControl.SetName(XmlControl.sequenceSingle, addModel.featureType.ToString());
                    }

                    //是否设置图像窗口
                    if (chkIsSetLayOut2.Checked)
                    {
                        addModel.LayOut = cmbLayOut2.SelectedIndex;
                    }

                    XmlControl.sequenceSingle.CheckFeatureModels.Add(addModel);

                    SingleSequenceModel seq = copyseq.SingleSequenceModels.FirstOrDefault(x => x.CheckFeatureModels.Contains(item));
                    CopyItem(seq, item, addModel.Name);
                }


                OnCanelEvent(null);

                MessageBoxEx.Show("复制完成!!!");
            }
            catch (Exception ex)
            {

            }
        }

        private void chkIsSetLayOut2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cmbLayOut2.Enabled = chkIsSetLayOut2.Checked;
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbCopySeq2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChildSequenceModel model = cmbCopySeq2.SelectedItem as ChildSequenceModel;

                List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
                foreach (var item in model.SingleSequenceModels)
                {
                    listModel.AddRange(item.CheckFeatureModels);
                } 

                cmbStartItem2.DataSource = listModel;
                cmbStartItem2.DisplayMember = "Name";
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbStartItem2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var listModels = cmbStartItem2.DataSource as List<CheckFeatureModel>;

                var selectModel = cmbStartItem2.SelectedItem as CheckFeatureModel;

                var listEndModel = listModels.FindAll(x => x.Index >= selectModel.Index).ToList();

                cmbEndItem2.DataSource = listEndModel;
                cmbEndItem2.DisplayMember = "Name";
            }
            catch (Exception ex)
            {

            }
        }

        private void btnGetImagePath_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.InitialDirectory = "d:\\";
                openFileDialog.Filter = "dsr文件|*.dsr|all|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                fileName = openFileDialog.FileName;
                txtProjectPath.Text = fileName;
            }
            catch (Exception ex)
            {
                 
            }

        }

        private void txtProjectPath_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                SequenceModel sequence = new SequenceModel();
                sequence = XmlControl.LoadFromXml(txtProjectPath.Text, sequence.GetType()) as SequenceModel;

                if(sequence == null)
                {
                    return;
                }

                cmbCopySeq2.DataSource = sequence.ChildSequenceModels;
                cmbCopySeq2.DisplayMember = "AnotherName";
            }
            catch (Exception ex)
            {
                 
            }
        }
        #endregion

    }
}
