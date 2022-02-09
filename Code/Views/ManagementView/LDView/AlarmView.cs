using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView
{
    /// <summary>
    /// 报警统计界面
    /// </summary>
    public partial class AlarmView : UserControl
    {
        public AlarmView()
        {
            InitializeComponent();
        }

        private void AlarmView_Load(object sender, EventArgs e)
        {

        }

        public void AddAlarm(int alarmID, string strAlarm)
        {
            try
            {
                BeginInvoke(new Action(() =>
                { 
                    if (dataAlarmView.Columns.Count == 0)
                    {
                        dataAlarmView.Columns.Add("ID", "报警ID");
                        dataAlarmView.Columns.Add("Time", "时间");
                        dataAlarmView.Columns.Add("Alarm", "报警描述");
                    }
                    int index = dataAlarmView.Rows.Add();

                    dataAlarmView.Rows[index].Cells[0].Value = alarmID.ToString();
                    dataAlarmView.Rows[index].Cells[1].Value = DateTime.Now.ToString();
                    dataAlarmView.Rows[index].Cells[2].Value = strAlarm;

                    dataAlarmView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataAlarmView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                })); 
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
