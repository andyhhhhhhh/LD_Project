using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalconView
{
    public partial class DisplayValue : Form
    {
        string m_valueX, m_valueY, m_valueGray;

        private void DisplayValue_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ControlKey)
            {
                HSmartWindow.bCtrlDown = false;
                this.Close();
            }
        }

        public DisplayValue(string valueX, string valueY, string valueGray)
        {
            InitializeComponent();
            m_valueX = valueX;
            m_valueY = valueY;
            m_valueGray = valueGray;
        }
        private void DisplayValue_Load(object sender, EventArgs e)
        {
            if(m_valueGray.Contains(","))
            {
                this.Width = 126;
            }
            else
            {
                this.Width = 82;
            }
            this.Height = 57;
            lblRow.Text = m_valueX;
            lblColumn.Text = m_valueY;
            lblGrayValue.Text = m_valueGray; 
        }

    }
}
