using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;
using GlobalCore;

namespace ManagementView.EditView
{
    public partial class EProductSel : UserControl
    {
        public EProductSel()
        {
            InitializeComponent();
        }

        bool bFirst = false;
        private void EProductSel_Load(object sender, EventArgs e)
        {
            try
            {
                var listProduct = XmlControl.listProductSelModel;

                if (listProduct != null && listProduct.Count > 0)
                {
                    cmbProduct.Items.Clear();
                    foreach (var item in listProduct)
                    {
                        cmbProduct.Items.Add(item.Name);
                    }
                }

                cmbProduct.Text = Global.ProductInfo;

                var selectProduct = listProduct.FirstOrDefault(x => x.Name == cmbProduct.Text); 

                bFirst = true;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bFirst)
            {
                return;
            }
            var listProduct = XmlControl.listProductSelModel;
            var selectProduct = listProduct.FirstOrDefault(x => x.Name == cmbProduct.Text);
            if (selectProduct != null)
            { 
                Global.ProductInfo = selectProduct.Name;
                XmlControl.sequenceModelNew.ProductInfo = Global.ProductInfo;
            }
        }

    }
}
