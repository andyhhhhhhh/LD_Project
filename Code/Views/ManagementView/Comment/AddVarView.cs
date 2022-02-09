using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SequenceTestModel;

namespace ManagementView.Comment
{
    public partial class AddVarView : UserControl
    {
        public AddVarView()
        {
            InitializeComponent();
        }

        private void AddVarView_Load(object sender, EventArgs e)
        {

        }

        private List<string> _ListLink = new List<string>();
        public List<string> ListLink
        {
            get
            {
                return _ListLink;
            }
            set
            {
                _ListLink = value;
                RefreshData();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ListLink.Clear();
                RefreshData();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnUpMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listVar.SelectedItems.Count == 0)
                {
                    return;
                }

                int count = ListLink.Count;
                var selectItem = listVar.SelectedItems[0] as string;

                int index = ListLink.IndexOf(selectItem);
                if (index == 0)
                {
                    return;
                }

                ListLink.Remove(selectItem);
                ListLink.Insert(index - 1, selectItem);
  
                RefreshData();

                listVar.SelectedIndex = index - 1; 
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDownMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listVar.SelectedItems.Count == 0)
                {
                    return;
                }
                int count = ListLink.Count;
                var selectItem = listVar.SelectedItems[0] as string;

                int index = ListLink.IndexOf(selectItem);
                if (index == count - 1)
                {
                    return;
                }

                ListLink.Insert(index + 2, selectItem);
                ListLink.Remove(selectItem);
                
                RefreshData();

                listVar.SelectedIndex = index + 1; 
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAddVar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(linkVarForm.LinkText))
                    return;

                if(ListLink == null)
                {
                    ListLink = new List<string>();
                }
                int count = ListLink.Count;

                ListLink.Add(linkVarForm.LinkText); 
                RefreshData();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (listVar.SelectedItems.Count == 0)
                {
                    return;
                }

                var selectItem = listVar.SelectedItems[0] as string;
                ListLink.Remove(selectItem);

                if (ListLink.Count == 0)
                {
                    return;
                } 

                RefreshData();
            }
            catch (Exception ex)
            {

            }
        }

        private void RefreshData()
        {
            try
            { 
                if(ListLink != null)
                {
                    listVar.Items.Clear();
                    foreach (var item in ListLink)
                    {
                        listVar.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
