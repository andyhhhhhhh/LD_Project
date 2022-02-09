using AlgorithmController;
using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AlgorithmController.EncryptionHelper;

namespace GenRegisterView
{
    public partial class Form1 : Form
    {
        private string encryptComputer = string.Empty; 
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string computer = ComputerInfo.GetComputerInfo();
            encryptComputer = new EncryptionHelper().EncryptString(computer);
            txtInfoKey.Text = encryptComputer; 
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenRegister_Click(object sender, EventArgs e)
        {
            try
            {
                EncryptionHelper help = new EncryptionHelper(EncryptionKeyEnum.KeyB);
                string md5String = help.GetMD5String(txtInfoKey.Text);

                //添加时间限制
                string strDay = "";
                if (chkIsLimitTime.Checked)
                {
                    strDay = numDays.Value.ToString("000");
                }

                string registInfo = help.EncryptString(md5String, strDay);
                txtRegisterKey.Text = registInfo;

                if(CheckRegist())
                {
                    MessageBoxEx.Show("生成注册码成功~");
                }
                else
                {
                    MessageBoxEx.Show("生成注册码失败~");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool CheckRegist()
        {
            EncryptionHelper helper = new EncryptionHelper();
            string md5key = helper.GetMD5String(txtInfoKey.Text);
            return CheckRegistData(md5key);
        }


        private bool CheckRegistData(string key)
        {
            string info = txtRegisterKey.Text;
            //增加使用时间限制 20210115
            bool bJudge = false;
            bJudge = info.Length > 80 ? true : false;
            int days = 0;
            if (bJudge)
            {
                string strDay = info.Substring(80);
                bool breturn = Int32.TryParse(strDay, out days);
                if (breturn)
                {
                    info = info.Substring(0, 80);
                }
            }

            var helper = new EncryptionHelper(EncryptionHelper.EncryptionKeyEnum.KeyB);
            string registData = helper.DecryptString(info);
            if (key == registData)
            { 
                return true;
            }
            else
            { 
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
