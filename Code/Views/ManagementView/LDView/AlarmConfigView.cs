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
using System.IO;

namespace ManagementView
{
    /// <summary>
    /// 报警配置界面
    /// </summary>
    public partial class AlarmConfigView : UserControl
    {
        public AlarmConfigView()
        {
            InitializeComponent();
        }

        private void AlarmConfigView_Load(object sender, EventArgs e)
        {
            UpDateData();
        }

        /// <summary>
        /// 更新报警信息
        /// </summary>
        private void UpDateData()
        {
            try
            {
                var listAlarm = XMLController.XmlControl.sequenceModelNew.alarmConfigModel.ListAlarm;

                int i = 0;
                foreach (var item in listAlarm)
                {
                    dataAlarm.Rows.Add();
                    dataAlarm.Rows[i].Cells[0].Value = item.AlarmID;
                    dataAlarm.Rows[i].Cells[1].Value = item.AlarmInfo;
                    i++;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<AlarmValue> listAlarm = new List<AlarmValue>();
                int count = dataAlarm.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    object str = dataAlarm.Rows[i].Cells[0].Value;
                    if (str != null)
                    {
                        int id;
                        bool bparse = Int32.TryParse(str.ToString(), out id); 
                        if(!bparse)
                        {
                            continue;
                        }
                        str = dataAlarm.Rows[i].Cells[1].Value;
                        string strValue = "";
                        if(str != null)
                        { 
                            strValue = str.ToString();
                        }
                        listAlarm.Add(new AlarmValue()
                        {
                            AlarmID = id,
                            AlarmInfo = strValue,
                        });
                    }
                }
                XMLController.XmlControl.sequenceModelNew.alarmConfigModel.ListAlarm = listAlarm;

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void 插入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataAlarm.SelectedRows.Count == 0)
                {
                    return;
                }

                int index = dataAlarm.SelectedRows[0].Index; 

                dataAlarm.Rows.Insert(index, new DataGridViewRow());
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int count = dataAlarm.SelectedRows.Count;
                if(count == 0)
                {
                    int length = dataAlarm.SelectedCells.Count;

                    for (int i = 0; i < length; i++)
                    {
                        dataAlarm.SelectedCells[i].Value = "";
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    dataAlarm.SelectedRows[i].Cells[0].Value = "";
                    dataAlarm.SelectedRows[i].Cells[1].Value = "";
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if(folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string strPath = folderDialog.SelectedPath + "//报警配置.csv";

                    string strTitle = "报警ID,报警信息";
                    string strData = "";
                    foreach (var item in XMLController.XmlControl.sequenceModelNew.alarmConfigModel.ListAlarm)
                    {
                        strData += item.AlarmID + "," + item.AlarmInfo + "\r\n";
                    }

                    OutPutCSV(strPath, strTitle, strData); 
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                if(openDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openDialog.FileName;
                    InPutCSV(path);
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        string m_BeginStr = "";
        private void dataAlarm_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 || e.ColumnIndex != 0)
                {
                    return;
                }

                string str = dataAlarm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    m_BeginStr = "";
                }
                else
                {
                    m_BeginStr = str;
                } 
            }
            catch (Exception ex)
            {

            }
        }

        private void dataAlarm_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            { 
                if (e.RowIndex == -1 || e.ColumnIndex != 0)
                {
                    return;
                }

                string str = dataAlarm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }

                int id;
                bool bresult = Int32.TryParse(str, out id);
                if (!bresult)
                {
                    MessageBox.Show("请输入数字类型!");
                    dataAlarm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginStr;
                    return;
                }

                int count = dataAlarm.Rows.Count;
                List<int> listId = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    object obj = dataAlarm.Rows[i].Cells[0].Value;
                    if(obj != null && e.RowIndex != i)
                    {
                        int alarmId;
                        bresult = Int32.TryParse(obj.ToString(), out alarmId);
                        if(bresult)
                        {
                            listId.Add(alarmId);
                        }
                    }
                }


                int index = listId.FindIndex(x=>x == id);
                if (index != -1)
                {
                    MessageBox.Show("已存在此ID!");
                    dataAlarm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = m_BeginStr;
                    return;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 导出数据到CSV
        /// </summary>
        /// <param name="filePath">csv路径</param>
        /// <param name="strTitle">数据头</param>
        /// <param name="dataStr">数据</param>
        public void OutPutCSV(string filePath, string strTitle, string dataStr)
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
                fileWriter.Write(strTitle + "\r\n");//时间字段名

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

        /// <summary>
        /// 导入CSV数据
        /// </summary>
        /// <param name="filePath">csv路径</param>
        public void InPutCSV(string filePath)
        {
            try
            { 
                List<AlarmValue> listAlarm = new List<AlarmValue>();
                using (var sr = new System.IO.StreamReader(filePath, Encoding.Default))
                { 
                    while (!sr.EndOfStream)
                    { 
                        string strline = sr.ReadLine();
                        if (string.IsNullOrEmpty(strline))
                        {
                            continue;
                        }

                        string[] strArr = strline.Split(',');
                        if (strArr.Length < 1)
                        {
                            continue;
                        }

                        if (strArr.Length >= 2)
                        {
                            int id;
                            bool bresult = Int32.TryParse(strArr[0], out id);
                            if (bresult)
                            {
                                listAlarm.Add(new AlarmValue()
                                {
                                    AlarmID = id,
                                    AlarmInfo = strArr[1]
                                });
                            }
                        }
                    }
                }   

                XMLController.XmlControl.sequenceModelNew.alarmConfigModel.ListAlarm = listAlarm;
                dataAlarm.Rows.Clear();

                UpDateData();

                MessageBox.Show("导入成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }
    }
}
