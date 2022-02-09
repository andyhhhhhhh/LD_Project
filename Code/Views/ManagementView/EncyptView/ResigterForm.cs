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

namespace ManagementView.EncyptView
{
    public partial class ResigterForm : Form
    {
        public ResigterForm()
        {
            InitializeComponent();
        }
        
        private void buttonX1_Click(object sender, EventArgs e)
        {
            //请验证身份
            ConfirmForm confirm = new ConfirmForm();
            DialogResult result = confirm.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string fileName = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
            }
            else
            {
                return;
            }
            string localFileName = string.Concat(Application.StartupPath, Path.DirectorySeparatorChar, RegistFileHelper.ComputerInfofile);
            if (fileName != localFileName)
                File.Copy(fileName, localFileName, true);
            string computer = RegistFileHelper.ReadComputerInfoFile();
            EncryptionHelper help = new EncryptionHelper(EncryptionKeyEnum.KeyB);
            string md5String = help.GetMD5String(computer);
            string registInfo = help.EncryptString(md5String);
            RegistFileHelper.WriteRegistFile(registInfo);
            MessageBoxEx.Show("注册成功,请重新开启软件.");
            this.Close();
        }
    }
}
