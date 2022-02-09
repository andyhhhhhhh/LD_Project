using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class OutPutView : UserControl
    {
        public OutPutView()
        {
            InitializeComponent();
        }

        public void SetOutPut(object setValue)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    if (setValue != null)
                    {
                        dataOutPut.DataSource = setValue;

                        int length = dataOutPut.Rows.Count;
                        for (int i = 0; i < length; i++)
                        {
                            if (dataOutPut.Rows[i].Cells[5].Value.ToString() == "OK")
                            {
                                dataOutPut.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                dataOutPut.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                            }

                            //把显示值设为只可读，防止修改会报错
                            int count = dataOutPut.Columns.Count;
                            for (int j = 0; j < count; j++)
                            {
                                dataOutPut.Rows[i].Cells[j].ReadOnly = true;
                            }

                        }                       
                    }
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
