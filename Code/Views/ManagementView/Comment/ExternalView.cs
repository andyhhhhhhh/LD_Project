﻿using System;
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
    public partial class ExternalView : UserControl
    {
        public ExternalView()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            appContainer1.AppFilename = @"E:\A-Platform\Local-Platform\Inspect3D\Inspect3D\Bin\MainView3D.exe";
            appContainer1.Start();
        }

    }
}
