using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.EditView
{
    public partial class EGroupBox : UserControl
    {
        public EGroupBox()
        {
            InitializeComponent();
        }

        private string _LText;
        [Browsable(true)]
        [Description("名称")] 
        public string LText
        {
            get { return _LText; }
            set
            {
                _LText = value;
                if (_LText != null)
                {
                    groupBox1.Text = value;
                }
            }
        }
    }
}
