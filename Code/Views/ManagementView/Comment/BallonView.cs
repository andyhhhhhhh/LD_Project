using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class BallonView : DevComponents.DotNetBar.Balloon
    {
        public BallonView()
        {
            InitializeComponent();

            Rectangle r = Screen.GetWorkingArea(this);
            this.Location = new Point(r.Right - this.Width, r.Bottom - this.Height);
            this.AutoClose = true;
            this.AutoCloseTimeOut = 5;
            this.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.RightToLeft;
            this.AlertAnimationDuration = 500;
        }

        private void BallonView_Load(object sender, EventArgs e)
        {  
        }

        public void ShowInfo(string strTitle, string strInfo)
        {
            try
            {
                labelX1.Text = strTitle;
                labelX2.Text = strInfo;
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
