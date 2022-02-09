using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceCollection;
using UserSetModel;
using GlobalCore;
using UserSetController;
using JsonController;
using DevComponents.DotNetBar;

namespace ManagementView.UserView
{
    public partial class UserLogin : Form
    {
        JsonControl m_jsonControl = new JsonControl();
        
        string m_strtype = "Operator";
        string m_struser = "operator";

        public static event EventHandler<object> UserSetEvent;
        protected void OnUserSetEvent(object e)
        {
            EventHandler<object> handler = UserSetEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public UserLogin()
        {
            InitializeComponent();
            cmbUserSelect.Items.Clear();
            cmbUserSelect.Items.Add("操作员");
            cmbUserSelect.Items.Add("管理员");
            cmbUserSelect.Items.Add("工程师");
            cmbUserSelect.SelectedIndex = 0;
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            //m_listUserModel = m_userService.QueryAll();

            //选择语言
            //string strLang = Global.GetNodeValue("Language");
            //MultiLanguage.SetLanguage(this.Controls, strLang);

            if(Global.UserName == Global.EngineerName)
            {
                cmbUserSelect.SelectedIndex = 2;
            }
            else if (Global.UserName == Global.DebugerName)
            { 
                cmbUserSelect.SelectedIndex = 1;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strUser = txtUser.Text;
            string strPass = txtPassword.sText;
            bool bLoginSuccess = false;

            if (strUser == "operator" && strPass == m_jsonControl.SystemPara.OpPassword)
            {
                bLoginSuccess = true;
            }
            else if(strUser == "debugger" && strPass == m_jsonControl.SystemPara.MgPassword)
            {
                bLoginSuccess = true;
            }
            else if(strUser == "engineer" && strPass == m_jsonControl.SystemPara.EnPassword)
            {
                bLoginSuccess = true;
            }

            if (bLoginSuccess)
            {
                MessageBoxEx.Show("登录成功");
                Global.UserName = txtUser.Text;
                OnUserSetEvent(cmbUserSelect.Text);
                this.Close();
            }
            else
            { 
                MessageBoxEx.Show("密码错误，请重新输入...", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cmbUserSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cmbUserSelect.SelectedIndex)
            {
                case 0:
                    m_strtype = "Operator";
                    m_struser = "operator";
                    break;
                case 1:
                    m_strtype = "Debugger";
                    m_struser = "debugger";
                    break;
                case 2:
                    m_strtype = "Engineer";
                    m_struser = "engineer";
                    break;
                default:
                    break;
            } 

            txtUser.Text = m_struser;
            //txtPassword.Text = "1";
            txtPassword.sText = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
