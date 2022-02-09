using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementView._3DViews;

namespace ManagementView.Comment
{
    public partial class EmbedAppView : UserControl
    {
        public EmbedAppView()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                appContainer1.AppFilename = loadFileView1.FilePath;
                appContainer1.Start();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("btnStart_Click", ex.Message);
            }
        }
    }
}
