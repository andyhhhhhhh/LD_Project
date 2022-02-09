using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFormDesinger
{
    /// <summary>
    /// 设计面板
    /// </summary>
    public partial class DesignerControl : UserControl
    {
        public HostFrame _hostFrame; //所有控件的容器
        Overlayer _overlayer;  //遮罩层

        public DesignerControl()
        {
            InitializeComponent();
            _hostFrame = new HostFrame();
            _hostFrame.TopLevel = false;  //当做子控件添加到设计面板
            _hostFrame.Location = new Point(5, 5);
            _hostFrame.Text = "";
            Controls.Add(_hostFrame);  
            _hostFrame.Show();

            _overlayer = new Overlayer(_hostFrame);
            _overlayer.Location = new Point(0, 0);
            _overlayer.Dock = DockStyle.Fill;
            Controls.Add(_overlayer);
            _overlayer.Show();  
            _overlayer.BringToFront();  //将其设置Z轴最上，接受所有的用户操作，底层的其他控件无法接受用户输入。各位可以注释这条试一下，底层的其他控件（窗体）就能接受鼠标点击

            Application.AddMessageFilter(new MessageFilter(_hostFrame, this)); // 过滤控件容器中所有子控件的WM_PAINT消息 减少重绘操作
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
