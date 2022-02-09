using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
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
    public partial class OskView : Form
    {
        DoubleInput m_DoubleInput;
        TextBoxX m_textBox;
        
        public OskView(DoubleInput textBox, bool bInt = true)
        {
            InitializeComponent();
            m_textBox = txtValue;

            m_DoubleInput = textBox;
            btnPoint.Enabled = !bInt; 
        } 

        //显示初始值
        public string InitValue
        {
            set
            {
                txtValue.Text = value;
            }
        }
        
        //输入数据
        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_textBox == null)
                {
                    return;
                }

                if(txtValue.SelectedText == txtValue.Text)
                {
                    txtValue.Text = "";
                }

                ButtonX btn = sender as ButtonX;
                switch(btn.Text)
                {
                    case "0":
                        m_textBox.Text += "0";
                        break;
                    case "1":
                        m_textBox.Text += "1";
                        break;
                    case "2":
                        m_textBox.Text +="2";
                        break;
                    case "3":
                        m_textBox.Text +="3";
                        break;
                    case "4":
                        m_textBox.Text +="4";
                        break;
                    case "5":
                        m_textBox.Text +="5";
                        break;
                    case "6":
                        m_textBox.Text +="6";
                        break;
                    case "7":
                        m_textBox.Text +="7";
                        break;
                    case "8":
                        m_textBox.Text +="8";
                        break;
                    case "9":
                        m_textBox.Text +="9";
                        break;
                    case ".":
                        m_textBox.Text +=".";
                        break;
                    case "Clr":
                        m_textBox.Text = "";
                        break;
                    case "OK":
                        double dvalue = 0;
                        bool bParse = Double.TryParse(m_textBox.Text, out dvalue);
                        if (bParse)
                        {
                            m_DoubleInput.Value = (double)dvalue;
                        }
                        else
                        {
                            MessageBoxEx.Show("输入有误...");
                        }
                        this.Hide();
                        break;
                    case "±":
                        if(m_textBox.Text.Contains("-"))
                        {
                            m_textBox.Text = m_textBox.Text.Replace("-", "");
                        }
                        else
                        {
                            m_textBox.Text = m_textBox.Text.Insert(0, "-");
                        }
                        break;
                    case "<-":
                        if(m_textBox.Text.Length > 0)
                        {
                            m_textBox.Text = m_textBox.Text.Remove(m_textBox.Text.Length - 1, 1);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
