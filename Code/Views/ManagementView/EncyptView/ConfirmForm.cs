using DevComponents.DotNetBar;
using JsonController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.EncyptView
{
    public partial class ConfirmForm : Form
    {
        JsonControl m_jsonControl = new JsonControl();
        public ConfirmForm()
        {
            InitializeComponent();
        } 
       

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == m_jsonControl.SystemPara.SuperPassword)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBoxEx.Show(this, "输入有误...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }
    }
}
