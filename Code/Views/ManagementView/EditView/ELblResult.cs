using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ManagementView.EditView
{
    public partial class ELblResult : UserControl
    {
        public static ELblResult mainLbl;
        public ELblResult()
        {
            InitializeComponent();
            mainLbl = this;
        } 

        private void ELblResult_Load(object sender, EventArgs e)
        {
            try
            { 

            }
            catch (Exception ex)
            {
                 
            }
        }

        public static string _sResult = "";
        [Browsable(true)]
        [Description("设置的结果(\"OK\"/\"NG\"")]
        public static string sResult
        {
            get { return _sResult; }
            set
            {
                _sResult = value;
                mainLbl.lblResult.Text = _sResult;
                if(_sResult.Contains("OK"))
                {
                    mainLbl.lblResult.ForeColor = Color.Green;
                }
                else
                { 
                    mainLbl.lblResult.ForeColor = Color.Red;
                }
            }
        }
         
    }
}
