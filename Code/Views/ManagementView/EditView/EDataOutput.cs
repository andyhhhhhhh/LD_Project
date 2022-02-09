using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementView.Comment;

namespace ManagementView.EditView
{
    public partial class EDataOutput : UserControl
    {
        public static EDataOutput EOutput;
        public EDataOutput()
        {
            InitializeComponent();
            EOutput = this;
        }

        private void EDataOutput_Load(object sender, EventArgs e)
        {
            try
            {
                m_DelEDataOutPut(outPutView1);
            }
            catch (Exception ex)
            {

            }
        }

        public delegate void Del_EDataOutput(OutPutView outPutView);
        public static Del_EDataOutput m_DelEDataOutPut;

    }
}
