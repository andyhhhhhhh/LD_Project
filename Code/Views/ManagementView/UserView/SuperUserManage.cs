using DevComponents.DotNetBar;
using GlobalCore;
using HalconDotNet;
using JsonController;
using ServiceCollection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserSetModel;

namespace ManagementView.UserView
{
    public partial class SuperUserManage : Form
    {
        JsonControl m_jsonControl = new JsonControl();

        Systemparameters m_systemParam = new Systemparameters();
        public SuperUserManage()
        {
            InitializeComponent(); 
        }

        private void SuperUserManage_Load(object sender, EventArgs e)
        {
            try
            {
                m_systemParam = m_jsonControl.ParseJsonFileAction();

                txtDebugValue.Text = m_systemParam.MgPassword;
                txtOperValue.Text = m_systemParam.OpPassword;
                txtEngValue.Text = m_systemParam.EnPassword; 
            }
            catch (Exception ex)
            {

            }

        }
        
        private void chkDisplayOp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisplayOp.Checked)
            {
                txtOperValue.PasswordChar = '\0';
            }
            else
            {
                txtOperValue.PasswordChar = '*';
            }
        }

        private void chkDisplayEng_CheckedChanged(object sender, EventArgs e)
        {
            if (Global.UserName != Global.EngineerName)
            {
                return;
            }
            if (chkDisplayEng.Checked)
            {
                txtEngValue.PasswordChar = '\0';
            }
            else
            {
                txtEngValue.PasswordChar = '*';
            }
        }

        private void chkDisplayDeb_CheckedChanged(object sender, EventArgs e)
        {
            if(Global.UserName == Global.OperatorName)
            {
                return;
            }
            if (chkDisplayDeb.Checked)
            {
                txtDebugValue.PasswordChar = '\0';
            }
            else
            {
                txtDebugValue.PasswordChar = '*';
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.UserName == Global.OperatorName)
                {
                    MessageBoxEx.Show("操作员无权限!!");
                    return;
                }

                m_systemParam.MgPassword = txtDebugValue.Text;
                m_systemParam.OpPassword = txtOperValue.Text;
                if (Global.UserName == Global.EngineerName)
                {
                    m_systemParam.EnPassword = txtEngValue.Text;
                }

                m_jsonControl.SystemPara = m_systemParam;
                m_jsonControl.SaveConfigurationClassToJsonFile();

                MessageBoxEx.Show("修改成功");
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
    }
}
