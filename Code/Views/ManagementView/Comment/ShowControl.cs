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
    public partial class ShowControl : Form
    { 
        public ShowControl(UserControl userControl, string strName)
        {
            InitializeComponent();

            this.Width = userControl.Width + 5;
            this.Height = userControl.Height + 25;

            CommHelper.LayoutChildFillView(panelView, userControl); 

            this.Text = strName;
            
            if(!GlobalCore.Global.IsFullScreen)
            {
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width + 2,
                 Screen.PrimaryScreen.WorkingArea.Height + 2);
            }
        }

        private void ShowControl_Load(object sender, EventArgs e)
        {
          
        }
    }
}
