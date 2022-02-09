using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class TextInput : UserControl
    {
        private AOSKView m_oskView;
        private string _sText = "";
        /// <summary>
        /// 输出输入的值
        /// </summary>
        public string sText
        {
            get
            {
                return _sText;
            }
            set
            {
                _sText = value;

                txtValue.Text = value;
                txtValue.Refresh();
            }
        }

        private bool _IsPassword;
        public bool  IsPassword
        {
            get { return _IsPassword; }
            set
            {
                _IsPassword = value;
                if (value == true)
                { 
                    txtValue.PasswordChar = '*';
                }
                else
                {
                    txtValue.PasswordChar = new char();
                }
            }
        }

        private bool _IsMultiLine;

        public bool IsMultiLine
        {
            get { return _IsMultiLine; }
            set
            {
                txtValue.Multiline = value;
                _IsMultiLine = value;
            }
        }

        public TextInput()
        {
            InitializeComponent();
        }

        private void txtValue_Enter(object sender, EventArgs e)
        {
           
        }

        private void txtValue_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalCore.Global.EnableOsk)
                {
                    return;
                }

                if (m_oskView != null && !m_oskView.IsDisposed)
                {
                    m_oskView.Hide();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sText = txtValue.Text;
                OnTxtValueEvent(sText);
            }
            catch (Exception ex)
            {

            }
        }

        private void txtValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalCore.Global.EnableOsk)
                {
                    return;
                }
                Rectangle rect = new Rectangle();
                rect = Screen.GetWorkingArea(this); 
                if (m_oskView == null || m_oskView.IsDisposed)
                {
                    m_oskView = new AOSKView(txtValue);
                    m_oskView.InitValue = txtValue.Text;
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }


                    m_oskView.Location = p;
                    m_oskView.ShowDialog();
                }
                else
                {
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }
                    m_oskView.Location = p;
                    m_oskView.InitValue = txtValue.Text;
                    m_oskView.ShowDialog();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public event EventHandler<string> TxtValueEvent;
        private void OnTxtValueEvent(string e)
        {
            TxtValueEvent?.Invoke(this, e);
        }

        private void TextInput_BackColorChanged(object sender, EventArgs e)
        {
            try
            { 
                txtValue.BackColor = this.BackColor;
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
