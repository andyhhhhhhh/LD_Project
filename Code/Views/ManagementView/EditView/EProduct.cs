using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalCore;
using XMLController;

namespace ManagementView.EditView
{
    public partial class EProduct : UserControl
    {
        public EProduct()
        {
            InitializeComponent();
        }

        bool bFirst = false;
        private void EComboProduct_Load(object sender, EventArgs e)
        {
            try
            {
                if (Global.IsDesginMode)
                {
                    return;
                }

                var listProduct = XmlControl.listProductModel;

                if (listProduct != null && listProduct.Count > 0)
                {
                    cmbSevenTitle.Items.Clear();
                    foreach (var item in listProduct)
                    {
                        cmbSevenTitle.Items.Add(item.SevenTitle);
                    }
                }

                cmbSevenTitle.Text = Global.ProductInfo;

                var selectProduct = listProduct.FirstOrDefault(x => x.SevenTitle == cmbSevenTitle.Text);
                if (selectProduct != null)
                {
                    txtBoxCode.Text = selectProduct.BoxCode;
                }

                bFirst = true;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void cmbSevenTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Global.IsDesginMode)
                {
                    return;
                }

                if (!bFirst)
                {
                    return;
                }
                var listProduct = XmlControl.sequenceModelNew.ProductInfoModels;
                var selectProduct = listProduct.FirstOrDefault(x => x.SevenTitle == cmbSevenTitle.Text);
                if(selectProduct != null)
                {
                    txtBoxCode.Text = selectProduct.BoxCode; 
                    Global.ProductInfo = selectProduct.SevenTitle;
                    XmlControl.sequenceModelNew.ProductInfo = Global.ProductInfo;
                }
                
            }
            catch (Exception ex)
            {
                 
            }
        }
        
    }
}
