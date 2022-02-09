using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFormDesinger
{
    public partial class HostFrame : Form
    {
        public HostFrame()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.TopLevel)
                ControlPaint.DrawGrid(e.Graphics, this.ClientRectangle, new Size(10, 10), Color.White); //绘制底层网格
            base.OnPaint(e);
        }


    }
}
