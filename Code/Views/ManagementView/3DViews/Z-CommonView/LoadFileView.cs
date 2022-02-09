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
    public partial class LoadFileView : UserControl
    {
        //输出文件路径
        private string filePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                if (!string.IsNullOrEmpty(filePath))
                {
                    txtFilePath.Text = filePath;
                }
            }
        }


        private string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                if (!string.IsNullOrEmpty(fileName))
                {
                    lblFileName.Text = fileName;
                }
            }
        }

        //文件格式过滤
        private string fileFilter;
        public string FileFilter
        {
            get
            {
                return fileFilter;
            }
            set
            {
                fileFilter = value;
            }
        }

        public LoadFileView()
        {
            InitializeComponent();            
        }

        private void LoadFileView_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            try
            { 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = ""; 
                openFileDialog.Filter = FileFilter != "" ? FileFilter : "png文件|*.png|jpeg文件|*.jpg|bmp文件|*.bmp|all|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                FilePath = openFileDialog.FileName;
            }
            catch (Exception ex)
            {                

            }
        }
    }
}
