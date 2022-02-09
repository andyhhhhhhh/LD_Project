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

namespace ManagementView.Comment
{
    public partial class ItemButtonView : UserControl
    {
        //设置属性 FeatureType
        private string _FeatureName = "";
        public string FeatureName
        {
            get { return _FeatureName; }
            set
            {
                _FeatureName = value;
                this.btnItem.Text = value;
                skinToolTip1.SetToolTip(btnItem, value);

                var bitmap = GetBitmap(_FeatureName);
                if (bitmap != null)
                {
                    buttonItem1.Image = bitmap;
                }
            }
        }

        private Image _ImageValue;
        public Image ImageValue
        {
            get { return _ImageValue; }
            set
            {
                _ImageValue = value;
                if(_ImageValue != null)
                {
                    buttonItem1.Image = value;
                }
            }
        }

        public string StrStyle
        {
            set
            {
                eDotNetBarStyle eStyle = (eDotNetBarStyle)Enum.Parse(typeof(eDotNetBarStyle), value); 
                btnItem.Style = eStyle;
            }
        }

        public ItemButtonView()
        {
            InitializeComponent();
        }
        
        Point btpt;
        private void btnItem_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnItem.DoDragDrop(FeatureName, DragDropEffects.Move);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取图像
        /// </summary>
        public static Bitmap GetBitmap(string name)
        {
            try
            {
                object obj = Properties.Resources.ResourceManager.GetObject(name, Properties.Resources.Culture);
                return (Bitmap)obj;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }


    }
}
