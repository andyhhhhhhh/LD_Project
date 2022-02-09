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
    public partial class ELog : UserControl
    {
        public ELog()
        {
            InitializeComponent();
        }

        public delegate LogView Del_ELog();
        public static Del_ELog m_DelELog;

        private void ELog_Load(object sender, EventArgs e)
        {
            try
            {
                object logview = m_DelELog();
                CommHelper.LayoutChildFillView(panelEx1, (UserControl)logview);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
