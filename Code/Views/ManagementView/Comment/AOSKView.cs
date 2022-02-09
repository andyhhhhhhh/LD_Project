using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
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
    public partial class AOSKView : Form
    {
        private List<ButtonX> m_ListButX = new List<ButtonX>();

        TextBox m_TextBoxX;
        public AOSKView(TextBox textBoxX)
        {
            InitializeComponent();
            m_ListButX.AddRange(new List<ButtonX>()
            {
                btn_A,btn_B,btn_C,btn_D,btn_E,btn_F,btn_G,btn_H,btn_I,btn_J,btn_K,
                btn_L,btn_M,btn_N,btn_O,btn_P,btn_Q,btn_R,btn_S,btn_T,btn_U,btn_V,
                btn_W,btn_X,btn_Y,btn_Z
            });

            m_TextBoxX = textBoxX;
        } 

        //显示初始值
        public string InitValue
        {
            set
            {
                txtValue.Text = value;
            }
        }

        private void btn_Cap_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsToUpper = true;
                if(btn_Cap.BackColor == Color.Transparent)
                {
                    btn_Cap.BackColor = Color.LightSkyBlue;
                    btn_Cap.ColorTable = eButtonColor.Orange;
                    IsToUpper = false;
                }
                else
                {
                    btn_Cap.BackColor = Color.Transparent;
                    btn_Cap.ColorTable = eButtonColor.OrangeWithBackground;
                }

                foreach (var btn in m_ListButX)
                {
                    btn.Text = IsToUpper ? btn.Text.ToUpper() : btn.Text.ToLower();
                }

                btn_Plus.Text = IsToUpper ? "+" : "=";
                btn_Jian.Text = IsToUpper ? "-" : "_";
                btn_Dou.Text = IsToUpper ? "," : "<";
                btnJu.Text = IsToUpper ? "." : ">";
                btnFen.Text = IsToUpper ? ";" : ":";
                btnMao.Text = IsToUpper ? "'" : "\"";
                btnKuo1.Text = IsToUpper ? "[" : "{";
                btnKuo2.Text = IsToUpper ? "]" : "}";
                btnXie.Text = IsToUpper ? "/" : "?";
                btnFanXie.Text = IsToUpper ? "\\" : "|";

                btn1.Text = IsToUpper ? "1" : "!";
                btn2.Text = IsToUpper ? "2" : "@";
                btn3.Text = IsToUpper ? "3" : "#";
                btn4.Text = IsToUpper ? "4" : "$";
                btn5.Text = IsToUpper ? "5" : "%";
                btn6.Text = IsToUpper ? "6" : "^";
                btn7.Text = IsToUpper ? "7" : "~";
                btn8.Text = IsToUpper ? "8" : "*";
                btn9.Text = IsToUpper ? "9" : "(";
                btn0.Text = IsToUpper ? "0" : ")"; 
            }
            catch (Exception ex)
            {
                 
            }
        } 

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonX btn = sender as ButtonX;
                txtValue.AppendText(btn.Text);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            txtValue.AppendText(" ");
        }

        private void btn_Esc_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtValue.Clear();
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if(txtValue.Text.Length == 0)
            {
                return;
            }
            txtValue.Text = txtValue.Text.Remove(txtValue.Text.Length - 1, 1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_TextBoxX.Text = txtValue.Text;
            this.Hide(); 
        }

        private void txtValue_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
