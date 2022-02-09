using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalCore;

namespace ManagementView.EditView
{
    public partial class ELblStatus : UserControl
    {
        public ELblStatus()
        {
            InitializeComponent();
        }
        private void ELblStatus_Load(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if(Global.Run && !Global.Pause)
                {
                    labelX1.Text = "运行";
                    labelX1.BackColor = Color.LimeGreen;
                }
                else if(Global.Pause)
                { 
                    labelX1.Text = "暂停";
                    labelX1.BackColor = Color.LightYellow;
                }
                else if(Global.Stop)
                { 
                    labelX1.Text = "停止";
                    labelX1.BackColor = Color.LightPink;
                } 
            }
            catch (Exception ex)
            {
                 
            }
        } 
      
    }
}
