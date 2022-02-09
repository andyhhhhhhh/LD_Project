using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenRegisterView
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "daschendr888888")
            {
                this.DialogResult = DialogResult.OK;
                Form1 form = new Form1();
                form.ShowDialog();
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
