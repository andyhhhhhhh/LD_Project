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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AlgorithmController.EncryptionHelper;

namespace ManagementView.EncyptView
{
    public partial class EncyptForm : Form
    {
        private string encryptComputer = string.Empty;
        private bool isRegist = false;
        private const int timeCount = 10*60; 
        public EncyptForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            string computer = ComputerInfo.GetComputerInfo();
            encryptComputer = new EncryptionHelper().EncryptString(computer);
            txtInfoKey.Text = encryptComputer;

            if (CheckRegist() == true)
            {
                lbRegistInfo.Text = "已注册";
            }
            else
            {
                lbRegistInfo.Text = "待注册，运行十分钟后关闭";
                RegistFileHelper.WriteComputerInfoFile(encryptComputer);
                TryRunForm();
            }
        }
        /// <summary> ///
        private void TryRunForm()
        {
            Thread threadClose = new Thread(CloseForm);
            threadClose.IsBackground = true;
            threadClose.Start();
        }
        private bool CheckRegist()
        {
            EncryptionHelper helper = new EncryptionHelper();
            string md5key = helper.GetMD5String(encryptComputer);
            return CheckRegistData(md5key);
        }
        private bool CheckRegistData(string key)
        {
            if (RegistFileHelper.ExistRegistInfofile() == false)
            {
                isRegist = false;
                return false;
            }
            else
            {
                string info = RegistFileHelper.ReadRegistFile();

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
                    isRegist = true;
                    //增加使用时间限制 20210115
                    if (bJudge)
                    {
                        DateTime currenttime = DateTime.Now;
                        DateTime datetime = File.GetCreationTime("Regist.key");
                        TimeSpan midtime = currenttime - datetime;

                        lblUseTime.Text = string.Format("剩余使用期限:{0}天", days - midtime.Days);
                        if(days - midtime.Days <= 0)
                        {
                            isRegist = false;
                        }
                    }

                    return isRegist;
                }
                else
                {
                    isRegist = false;
                    return isRegist;
                }
            }
        }
        private void CloseForm()
        {
            int count = 0;
            while (count < timeCount && isRegist == false)
            {
                if (isRegist == true)
                {
                    return;
                }
                Thread.Sleep(1 * 1000);
                count++;
            }
            if (isRegist == true)
            {
                return;
            }
            else
            {
                this.Close();
            }
        }
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (lbRegistInfo.Text == "已注册")
            {
                MessageBoxEx.Show("已经注册～");
                return;
            }

            if (!string.IsNullOrEmpty(txtRegisterKey.Text))
            {
                RegistFileHelper.RegistInfofile = GlobalCore.Global.CurrentPath + "//Regist.key";
                RegistFileHelper.WriteRegistFile(txtRegisterKey.Text);
                RegistFileHelper.RegistInfofile = "C://Regist.key";
                RegistFileHelper.WriteRegistFile(txtRegisterKey.Text);
            }
            else
            {
                //string fileName = string.Empty;
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //if (openFileDialog.ShowDialog() == DialogResult.OK)
                //{
                //    fileName = openFileDialog.FileName;
                //}
                //else
                //{
                //    return;
                //}
                //string localFileName = string.Concat(Global.CurrentPath, Path.DirectorySeparatorChar, RegistFileHelper.RegistInfofile);
                //if (fileName != localFileName)
                //    File.Copy(fileName, localFileName, true);
            }

            if (CheckRegist() == true)
            {
                lbRegistInfo.Text = "已注册";
                MessageBoxEx.Show("注册成功，请重启软件～");
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBoxEx.Show("注册失败～");
            }

        }

        private void btnOpenRegister_Click(object sender, EventArgs e)
        {
            ResigterForm form = new ResigterForm();
            form.Show();
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
                string registInfo = help.EncryptString(md5String);
                txtRegisterKey.Text = registInfo; 
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
