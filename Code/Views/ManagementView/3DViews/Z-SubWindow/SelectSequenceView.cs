using CCWin;
using GlobalCore;
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
    public partial class SelectSequenceView : Form
    {
        public static event EventHandler<object> SelectEvent;
        public void OnSelectEvent(object e)
        {
            EventHandler<object> handler = SelectEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public SelectSequenceView()
        {
            InitializeComponent();
        }

        public class AppendSequence
        {
            public string Name { get; set; }
            public string strPath { get; set; }
            public bool bSelect { get; set; }
        }

        public class AppendConfig
        {
            public string strPath { get; set; }
            public bool bSelect { get; set; }
        }

        private void SelectSequenceView_Load(object sender, EventArgs e)
        {
            try
            {
                skinTabControl1.SelectedIndex = 0;
                //查找固定路径下的文件
                string strPath = Global.CurrentPath + "\\Sequence";  
                string[] strPathArr = Directory.GetFiles(strPath);

                List<AppendSequence> listSequence = new List<AppendSequence>();

                List<string> listPath = new List<string>();
                foreach (var path in strPathArr)
                {
                    if (path.Contains(".dsr"))
                    {
                        listPath.Add(path);
                        listSequence.Add(new AppendSequence()
                        {
                            Name = path.Replace(path.Substring(0, path.LastIndexOf("\\")), "").Replace("\\", ""),
                            strPath = path,
                            bSelect = path == Global.SequencePath,
                        }); 
                    }
                }

                dataGridView1.DataSource = listSequence;
                
                List<AppendConfig> listConfig = new List<AppendConfig>();

                //检查电脑上是否有D盘
                var driveD = DriveInfo.GetDrives().ToList().Any(x => x.Name.Contains("D:"));
                if (!driveD)
                {
                    Global.ModelPath = @"C:\Documents\Dr3DVision\Model\";
                }

                string[] strArrPath = Directory.GetDirectories(Global.ModelPath);
                foreach (var item in strArrPath)
                {
                    listConfig.Add(new AppendConfig()
                    {
                        strPath = item,
                        bSelect = item == Global.Model3DPath
                    });
                }

                dataGridView2.DataSource = listConfig;

                var names =  Enum.GetNames(typeof(FeatureType));
                listBox1.DataSource = names.ToList();

                listBox2.Items.Clear();
                foreach (var item in Global.ReadConfig(Global.ConfigPath))
                {
                    listBox2.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void 确认修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show(this, "未选中行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var listSeq = dataGridView1.DataSource as List<AppendSequence>;

                var selSequence = dataGridView1.SelectedRows[0].DataBoundItem as AppendSequence;
                selSequence.bSelect = true;

                foreach (var item in listSeq)
                {
                    item.bSelect = item.Name == selSequence.Name;
                }
                
                dataGridView1.Refresh();

                OnSelectEvent(selSequence.strPath);
            }
            catch (Exception ex)
            {
                  
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows == null || dataGridView2.SelectedRows.Count == 0)
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show(this, "未选中行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var listSeq = dataGridView2.DataSource as List<AppendConfig>;

                var selConfig = dataGridView2.SelectedRows[0].DataBoundItem as AppendConfig;
                selConfig.bSelect = true;

                foreach (var item in listSeq)
                {
                    item.bSelect = item.strPath == selConfig.strPath;
                }

                dataGridView2.Refresh();

                XMLController.XmlControl.sequenceModelNew.BasePath = selConfig.strPath;
                GlobalCore.Global.Model3DPath = selConfig.strPath;
            }
            catch (Exception ex)
            {

            }
        }
        
        #region 测试项目选择
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var selName = listBox2.SelectedItem;
                int index = listBox2.SelectedIndex;
                listBox2.Items.Remove(selName);

                if(index == -1)
                {
                    return;
                }
                listBox2.SelectedIndex = index - 1;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var selName = listBox1.SelectedItem;

                if(!listBox2.Items.Contains(selName))
                {
                    listBox2.Items.Add(selName.ToString());
                }
            }
            catch (Exception ex)
            { 

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            { 
                FileStream fs = new FileStream(Global.ConfigPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                StreamReader re = new StreamReader(fs, Encoding.UTF8); 

                List<string> liststr = new List<string>();
                foreach (var item in listBox2.Items)
                {
                    liststr.Add(item.ToString());
                }

                File.WriteAllLines(Global.ConfigPath, liststr);
                 
                fs.Flush(); 
                fs.Close();
                re.Close();

               DevComponents.DotNetBar.MessageBoxEx.Show("保存成功");
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion
        
    }
}
