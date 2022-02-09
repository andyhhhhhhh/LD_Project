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
    /// 显示当前步骤界面
    /// </summary>
    public partial class StepView : UserControl
    {
        int m_stepCount = 1;
        public StepView(int stepCount)
        {
            InitializeComponent();
            m_stepCount = stepCount;
        }

        private void StepView_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < m_stepCount; i++)
                {
                    dataStepView.Rows.Add();
                } 
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 显示当前执行步骤
        /// </summary>
        /// <param name="stepIndex">步骤序号</param>
        /// <param name="strStep">步骤名称</param>
        public void ShowStep(int id, string modelName, int stepIndex, string strStep)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    dataStepView.Rows[id].Cells[0].Value = modelName;
                    dataStepView.Rows[id].Cells[1].Value = stepIndex;
                    dataStepView.Rows[id].Cells[2].Value = strStep;
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
