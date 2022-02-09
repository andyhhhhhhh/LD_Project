using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ManagementView._3DViews.CommonView
{
    public partial class ShowManyPic : UserControl
    {
        private List<string> m_listImagePath = new List<string>();
        private string[] m_ArrPath = new string[5];
        public string ImageFile = "";
        public delegate void GetImageFile();
        public GetImageFile m_GetImageFile;

        private string imagePath = "";
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }
        public ShowManyPic()
        {
            InitializeComponent();
        }

        private void ShowManyPic_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void RefeshView()
        {
            try
            {
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    m_listImagePath = Directory.GetFiles(ImagePath, "*.jpg").Union(Directory.GetFiles(ImagePath, "*.png"))
                    .Union(Directory.GetFiles(ImagePath, "*.bmp")).ToArray().ToList();
                }

                int iCount = m_listImagePath.Count;
                if (iCount < 5)
                {
                    skinHScrollBar1.Maximum = 0;
                    skinHScrollBar1.MiddleButtonLengthPercentage = 99;
                }
                else
                {
                    skinHScrollBar1.Maximum = iCount / 5 - 1;
                    skinHScrollBar1.MiddleButtonLengthPercentage = 100 / (skinHScrollBar1.Maximum + 1);
                }

                //Thread t = new Thread(new ThreadStart(() =>
                //{
                Task.Run(new Action(() =>
                {
                    if (m_listImagePath.Count > 0)
                    {
                        pictureBox1.BackgroundImage = Image.FromFile(m_listImagePath[0]);
                        m_ArrPath[0] = m_listImagePath[0];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                                {
                                    toolTip1.SetToolTip(pictureBox1, m_ArrPath[0]);
                                }));

                        }
                    }
                }));
                Task.Run(new Action(() =>
                {
                    if (m_listImagePath.Count > 1)
                    {
                        pictureBox2.BackgroundImage = Image.FromFile(m_listImagePath[1]);
                        m_ArrPath[1] = m_listImagePath[1];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                        {
                            toolTip1.SetToolTip(pictureBox2, m_ArrPath[1]);
                        }));
                        }
                    }
                }));
                Task.Run(new Action(() =>
                {
                    if (m_listImagePath.Count > 2)
                    {
                        pictureBox3.BackgroundImage = Image.FromFile(m_listImagePath[2]);
                        m_ArrPath[2] = m_listImagePath[2];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                toolTip1.SetToolTip(pictureBox3, m_ArrPath[2]);
                            }));
                        }
                    }
                }));
                Task.Run(new Action(() =>
                {
                    if (m_listImagePath.Count > 3)
                    {
                        pictureBox4.BackgroundImage = Image.FromFile(m_listImagePath[3]);
                        m_ArrPath[3] = m_listImagePath[3];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {

                                toolTip1.SetToolTip(pictureBox4, m_ArrPath[3]);
                            }));
                        }
                    }
                }));
                Task.Run(new Action(() =>
                {
                    if (m_listImagePath.Count > 4)
                    {
                        pictureBox5.BackgroundImage = Image.FromFile(m_listImagePath[4]);
                        m_ArrPath[4] = m_listImagePath[4];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                toolTip1.SetToolTip(pictureBox5, m_ArrPath[4]);
                            }));
                        }
                    }
                }));
            }
            catch (Exception ex)
            {

            }

        }

        private void skinHScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            int value = skinHScrollBar1.Value;
            RefreshData(value);
        }

        private void RefreshData(int value)
        {
            Task.Run(new Action(() =>
            {
                lock (this)
                {
                    if (m_listImagePath.Count > value * 5 + 0)
                    {
                        pictureBox1.BackgroundImage = Image.FromFile(m_listImagePath[value * 5 + 0]);
                        m_ArrPath[0] = m_listImagePath[value * 5 + 0];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                toolTip1.SetToolTip(pictureBox1, m_ArrPath[0]);
                            }));
                        }

                    }
                }
            }));

            Task.Run(new Action(() =>
            {

                lock (this)
                {
                    if (m_listImagePath.Count > value * 5 + 1)
                    {
                        pictureBox2.BackgroundImage = Image.FromFile(m_listImagePath[value * 5 + 1]);
                        m_ArrPath[1] = m_listImagePath[value * 5 + 1];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                        {
                            toolTip1.SetToolTip(pictureBox2, m_ArrPath[1]);
                        }));
                        }
                    }
                }
            }));

            Task.Run(new Action(() =>
            {
                lock (this)
                {
                    if (m_listImagePath.Count > value * 5 + 2)
                    {
                        pictureBox3.BackgroundImage = Image.FromFile(m_listImagePath[value * 5 + 2]);
                        m_ArrPath[2] = m_listImagePath[value * 5 + 2];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                        {
                            toolTip1.SetToolTip(pictureBox3, m_ArrPath[2]);
                        }));
                        }
                    }
                }
            }));

            Task.Run(new Action(() =>
            {
                lock (this)
                {
                    if (m_listImagePath.Count > value * 5 + 3)
                    {
                        pictureBox4.BackgroundImage = Image.FromFile(m_listImagePath[value * 5 + 3]);
                        m_ArrPath[3] = m_listImagePath[value * 5 + 3];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                        {
                            toolTip1.SetToolTip(pictureBox4, m_ArrPath[3]);
                        }));
                        }
                    }
                }
            }));
            Task.Run(new Action(() =>
            {
                lock (this)
                {
                    if (m_listImagePath.Count > value * 5 + 4)
                    {
                        pictureBox5.BackgroundImage = Image.FromFile(m_listImagePath[value * 5 + 4]);
                        m_ArrPath[4] = m_listImagePath[value * 5 + 4];
                        if (InvokeRequired && this.IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                        {
                            toolTip1.SetToolTip(pictureBox5, m_ArrPath[4]);
                        }));
                        }
                    }
                }
            })); 
        }

        #region Mouse Double Click Event
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox == pictureBox1)
            {
                ImageFile = m_ArrPath[0];
            }
            else if (pictureBox == pictureBox2)
            {
                ImageFile = m_ArrPath[1];
            }
            else if (pictureBox == pictureBox3)
            {
                ImageFile = m_ArrPath[2];
            }
            else if (pictureBox == pictureBox4)
            {
                ImageFile = m_ArrPath[3];
            }
            else if (pictureBox == pictureBox5)
            {
                ImageFile = m_ArrPath[4];
            }

            if (m_GetImageFile != null)
            {
                m_GetImageFile();
            }
        }
        #endregion

    }
}
