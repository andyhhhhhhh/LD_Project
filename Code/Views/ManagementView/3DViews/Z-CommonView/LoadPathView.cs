using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView._3DViews.CommonView
{
    public partial class LoadPathView : UserControl
    { 
        //输出文件路径
        private string folderPath;
        public string FolderPath
        {
            get
            {
                return folderPath;
            }
            set
            {
                folderPath = value;
                if(!string.IsNullOrEmpty(folderPath))
                {
                    txtFolderPath.Text = folderPath;
                }
            }
        }

        private string folderName;
        public string FolderName
        {
            get
            {
                return folderName;
            }
            set
            {
                folderName = value;
                if (!string.IsNullOrEmpty(folderName))
                {
                    lblPath.Text = folderName;
                }
            }
        }

        public LoadPathView()
        {
            InitializeComponent();
            txtFolderPath.SkinTxt.TextChanged += SkinTxt_TextChanged;
        }

        private void SkinTxt_TextChanged(object sender, EventArgs e)
        {
            FolderPath = txtFolderPath.Text;
        }

        private void btnLoadFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.Description = "选择目录";

                if (folder.ShowDialog() == DialogResult.OK)
                {
                    string sPath = folder.SelectedPath;
                    txtFolderPath.Text = sPath;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        

    }
}
