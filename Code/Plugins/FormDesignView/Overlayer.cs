using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagementView;
using System.Reflection;
using ManagementView.EditView;

namespace MyFormDesinger
{
    public partial class Overlayer : UserControl
    {
        #region 参数定义

        //被遮罩的控件容器，通过Overlayer操作该容器（以及其中的子控件）
        HostFrame _themainhost;
        //被操作控件（容器）周围的方框
        Recter _recter = new Recter();

        //当前被操作控件
        Control _currentCtrl = null;

        //按下Ctrl键时选中的多个控件
        List<Control> _listCtrl = new List<Control>();
        //按下Ctrl键时显示的多个Recter
        List<Recter> _listRecter = new List<Recter>();

        Point _firstPoint = new Point();
        bool _mouseDown = false;
        DragType _dragType = DragType.None;

        #endregion

        public Overlayer(HostFrame themainhost)
        {
            _themainhost = themainhost;  //默认被操作的是控件容器
            //Rectangle r = _themainhost.Bounds;
            //r = _themainhost.Parent.RectangleToScreen(r);
            //r = this.RectangleToClient(r);
            //_recter.Rect = r;
            //_recter.IsForm = true;

            MyFormDesign.m_DeleteItem += DeleteItem;
            MyFormDesign.AlignEvent += M_MyFormDesign_AlignEvent;
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams para = base.CreateParams;
                para.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT 透明支持
                return para;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) //不画背景
        {
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_recter != null) //绘制被操作控件周围的方框
            {
                _recter.Draw(e.Graphics);
            }

            if(_listRecter != null && _listRecter.Count > 0)
            {
                foreach (var item in _listRecter)
                {
                    item.Draw(e.Graphics);
                }
            }
            base.OnPaint(e);
        }

        #region 代理所有用户操作
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_mouseDown) //鼠标形状
            {
                DragType dt = _recter.GetMouseDragType(e.Location);
                switch (dt)
                {
                    case DragType.Top:
                        {
                            Cursor = Cursors.SizeNS;
                            break;
                        }
                    case DragType.RightTop:
                        {
                            Cursor = Cursors.SizeNESW;
                            break;
                        }
                    case DragType.RightBottom:
                        {
                            Cursor = Cursors.SizeNWSE;
                            break;
                        }
                    case DragType.Right:
                        {
                            Cursor = Cursors.SizeWE;
                            break;
                        }
                    case DragType.LeftTop:
                        {
                            Cursor = Cursors.SizeNWSE;
                            break;
                        }
                    case DragType.LeftBottom:
                        {
                            Cursor = Cursors.SizeNESW;
                            break;
                        }
                    case DragType.Left:
                        {
                            Cursor = Cursors.SizeWE;
                            break;
                        }
                    case DragType.Center:
                        {
                            Cursor = Cursors.SizeAll;
                            break;
                        }
                    case DragType.Bottom:
                        {
                            Cursor = Cursors.SizeNS;
                            break;
                        }
                    default:
                        {
                            Cursor = Cursors.Default;
                            break;
                        }
                }
            }
            else
            {
                switch (_dragType) //改变方框位置大小
                {
                    case DragType.Top:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X, _recter.Rect.Y + delta.Y, _recter.Rect.Width, _recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.RightTop:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X, _recter.Rect.Y + delta.Y, _recter.Rect.Width + delta.X, _recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;                       
                            break;
                        }
                    case DragType.RightBottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X, _recter.Rect.Y, _recter.Rect.Width + delta.X, _recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Right:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X, _recter.Rect.Y, _recter.Rect.Width + delta.X, _recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.LeftTop:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X + delta.X, _recter.Rect.Y + delta.Y, _recter.Rect.Width + delta.X * (-1), _recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;                     
                            break;
                        }
                    case DragType.LeftBottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X + delta.X, _recter.Rect.Y, _recter.Rect.Width + delta.X * (-1), _recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;                    
                            break;
                        }
                    case DragType.Left:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X + delta.X, _recter.Rect.Y, _recter.Rect.Width + delta.X * (-1), _recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Center:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X + delta.X, _recter.Rect.Y + delta.Y, _recter.Rect.Width, _recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Bottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            _recter.Rect = new Rectangle(_recter.Rect.X, _recter.Rect.Y, _recter.Rect.Width, _recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
     
            if (_mouseDown)
            {
                Invalidate2(false);
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //左键
            {
                bool flag = false;
                foreach (Control c in _themainhost.Controls) //遍历控件容器 看是否选中其中某一控件
                {
                    Rectangle r = c.Bounds;
                    r = _themainhost.RectangleToScreen(r);
                    r = this.RectangleToClient(r);
                    Rectangle rr = r;
                    rr.Inflate(5, 5);

                    Point location = new Point()
                    {
                        X = e.Location.X - 2,
                        Y = e.Location.Y,
                    };
                    if (rr.Contains(location))
                    {
                        //按下Ctrl键显示的多个Recter
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            Recter rect = new Recter();
                            rect.Rect = r;
                            rect.IsForm = false;
                            _listRecter.Add(rect);
                        }

                        _recter.Rect = r;
                        _currentCtrl = c;
                         
                        (Parent.Parent.Parent.Parent.Parent as MyFormDesign).propertyGrid1.SelectedObject = c; //选中控件
                        //(Parent.Parent as NewFormDesigner).propertyGrid1.SelectedObject = c; //选中控件
                        _recter.IsForm = false;
                        flag = true;
                        Invalidate2(false);
                        break;
                    }
                }
                if (!flag) //没有控件被选中，判断是否选中控件容器
                {
                    Rectangle r = _themainhost.Bounds;
                    r = Parent.RectangleToScreen(r);
                    r = this.RectangleToClient(r);
                    if (r.Contains(e.Location))
                    {
                        _recter.Rect = r;
                        _recter.IsForm = true;
                        _currentCtrl = null;
                        (Parent.Parent.Parent.Parent.Parent as MyFormDesign).propertyGrid1.SelectedObject = _themainhost; //选中控件容器
                       // (Parent.Parent as NewFormDesigner).propertyGrid1.SelectedObject = _themainhost; //选中控件容器
                        Invalidate2(false);
                    }
                }
                DragType dt = _recter.GetMouseDragType(e.Location);  //判断是否可以进行鼠标操作
                if (dt != DragType.None)
                {
                    _mouseDown = true;
                    _firstPoint = e.Location;
                    _dragType = dt;
                }


                //按下Ctrl键以便操作多个控件Align
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (!_listCtrl.Contains(_currentCtrl) && _currentCtrl != _themainhost)
                    {
                        _listCtrl.Add(_currentCtrl);
                    }                    
                }

            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //左键弹起
            {
                _firstPoint = new Point();
                _mouseDown = false;
                _dragType = DragType.None;
                Invalidate2(true);
            }
            base.OnMouseUp(e);
        }
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Copy;
            base.OnDragEnter(drgevent);
        }
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            try
            {
                string[] strs = (string[])drgevent.Data.GetData(typeof(string[])); //获取拖拽数据
                Control ctrl = ControlHelper.CreateControl(strs[1], strs[0]); //实例化控件

                ctrl.Location = _themainhost.PointToClient(new Point(drgevent.X, drgevent.Y)); //屏幕坐标转换成控件容器坐标
                if (!(ctrl is DateTimePicker))
                {
                    ctrl.Text = strs[1];
                }
                _themainhost.Controls.Add(ctrl);
                ctrl.BringToFront();
                _currentCtrl = ctrl;

                (Parent.Parent.Parent.Parent.Parent as MyFormDesign).propertyGrid1.SelectedObject = ctrl; //选中控件
                //(Parent.Parent as NewFormDesigner).propertyGrid1.SelectedObject = ctrl; //选中控件
                SetCtrlBrowsable(ctrl);//设置控件属性显示

               //将控件容器坐标转换为Overlayer坐标
               Rectangle r = _themainhost.RectangleToScreen(ctrl.Bounds);
                r = this.RectangleToClient(r);

                _recter.Rect = r;
                _recter.IsForm = false;
                Invalidate2(false);
            }
            catch
            {

            }
            base.OnDragDrop(drgevent);
        } 
        protected override bool ProcessDialogKey(Keys keyData)
        {
            try
            {
                if (_currentCtrl == null)
                {
                    return true;
                }
                
                switch (keyData)
                {
                    case Keys.Left:
                        _currentCtrl.Left -= 1;
                        SetRectPos();
                        break;
                    case Keys.Right:
                        _currentCtrl.Left += 1;
                        SetRectPos();
                        break;
                    case Keys.Up:
                        _currentCtrl.Top -= 1;
                        SetRectPos();
                        break;
                    case Keys.Down:
                        _currentCtrl.Top += 1;
                        SetRectPos();
                        break;
                    case Keys.Delete:
                        _themainhost.Controls.Remove(_currentCtrl);
                        break;
                        
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.ControlKey)
                {
                    _listCtrl.Clear();
                    _listRecter.Clear();
                    Invalidate2(false);
                }
            }
            catch (Exception ex)
            {

            }


            base.OnKeyUp(e);    
        }
        #endregion

        /// <summary>
        /// 上下左右移动控件时Rect也跟随
        /// </summary>
        private void SetRectPos()
        {
            try
            { 
                Rectangle r = _currentCtrl.Bounds;
                r = _themainhost.RectangleToScreen(r);
                r = this.RectangleToClient(r);
                _recter.Rect = r;
                _recter.IsForm = false;
                Invalidate2(false);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void Invalidate2(bool mouseUp)
        {
            Invalidate();
            if (Parent != null) //更新父控件
            {
                Rectangle rc = new Rectangle(this.Location, this.Size);
                Parent.Invalidate(rc, true);	
            }
            if (mouseUp) //鼠标弹起 更新底层控件
            {
                if (_currentCtrl != null) //更新底层控件的位置、大小
                {
                    Rectangle r = _recter.Rect;
                    r = this.RectangleToScreen(r);
                    r = _themainhost.RectangleToClient(r);

                    _currentCtrl.SetBounds(r.Left, r.Top, r.Width, r.Height);
                }
                else //更新控件容器大小
                {
                    Rectangle r = _recter.Rect;
                    r = this.RectangleToScreen(r);
                    r = Parent.RectangleToClient(r);
                    _themainhost.SetBounds(r.Left, r.Top, r.Width, r.Height);
                }
            }
        }
    
        //设置控件的公共属性设置成不可见
        void SetPropertyVisibility(object obj, string propertyName)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);

            bool bVisible = false;
            if (propertyName == "Name" || propertyName == "Width" || propertyName == "Height"
               || propertyName == "Left" || propertyName == "Top")
            {
                bVisible = true;
            }
            fld.SetValue(attrs[type], bVisible);
        }

        private void SetCtrlBrowsable(Control ctrl)
        {
            try
            {
                PropertyInfo[] infos = ctrl.GetType().GetProperties();
                if (ctrl is EButton || ctrl is EButtonPro || ctrl is EErrorItem)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "EText" || item.Name == "EBackColor" || item.Name == "SText" || item.Name == "FontSize")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name); 
                    }
                }
                else if(ctrl is EDataOutput)
                { 
                    foreach (var item in infos)
                    { 
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EHWindow || ctrl is EGroupBox)
                { 
                    foreach (var item in infos)
                    {
                        if (item.Name == "LayoutWindow" || item.Name == "LText")
                        {
                            continue;
                        } 
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELblResult)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "sResult")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELblStatus)
                {
                    foreach (var item in infos)
                    { 
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELog)
                {
                    foreach (var item in infos)
                    { 
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ETextBox || ctrl is ESetText || ctrl is ECheck)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue" || item.Name == "TextLength")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is HostFrame)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EProduct || ctrl is EProductSel)
                {
                    foreach (var item in infos)
                    {
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is EItemResult)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ELight)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "ComName" || item.Name == "OpenText" || item.Name == "CloseText")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
                else if (ctrl is ECombox)
                {
                    foreach (var item in infos)
                    {
                        if (item.Name == "LText" || item.Name == "LinkValue" || item.Name == "ListValue")
                        {
                            continue;
                        }
                        SetPropertyVisibility(ctrl, item.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        
        public void DeleteItem()
        {
            try
            {
                if (_currentCtrl == null)
                {
                    return;
                }
                _themainhost.Controls.Remove(_currentCtrl);
            }
            catch (Exception ex)
            {
                 
            }
        }

        //设置选中的控件对齐
        private void M_MyFormDesign_AlignEvent(object sender, object e)
        {
            try
            {
                int count = _listCtrl.Count;
                if(count < 2)
                {
                    return;
                }

                Control firstCtrl = _listCtrl[0];
                List<Control> listContrl = _listCtrl.FindAll(x => x != firstCtrl).ToList();

                int index = (Int32)e;
                switch(index)
                {
                    case 1://左对齐
                        foreach (var item in listContrl)
                        {
                            item.Left = firstCtrl.Left;
                        }
                        break;

                    case 2://上对齐 
                        foreach (var item in listContrl)
                        {
                            item.Top = firstCtrl.Top;
                        }
                        break;

                    case 3://水平间距相等 
                        if (count < 3)
                        {
                            return;
                        }

                        int allWidth = _listCtrl[count - 1].Right - _listCtrl[0].Left;
                        int ctrlWidth = firstCtrl.Width;
                        foreach (var item in listContrl)
                        {
                            ctrlWidth += item.Width;
                        }
                        int width = (allWidth - ctrlWidth) / (count - 1);

                        Control preControl = firstCtrl;
                        foreach (var item in listContrl)
                        {
                            item.Left = preControl.Right + width;
                            preControl = item;
                        }
                        break;

                    case 4://垂直间距相等
                        if (count < 3)
                        {
                            return;
                        }

                        int allHeight = _listCtrl[count - 1].Bottom - _listCtrl[0].Top;
                        int ctrlHeight = firstCtrl.Height;
                        foreach (var item in listContrl)
                        {
                            ctrlHeight += item.Height;
                        }
                        int height = (allHeight - ctrlHeight) / (count - 1);

                        Control bfControl = firstCtrl;
                        foreach (var item in listContrl)
                        {
                            item.Top = bfControl.Bottom + height;
                            bfControl = item;
                        } 
                        break;

                    case 5://Size相等 
                        foreach (var item in listContrl)
                        {
                            item.Size = firstCtrl.Size;
                        }
                        break;

                    case 6://右对齐 
                        foreach (var item in listContrl)
                        {
                            item.Left = firstCtrl.Right - item.Width;
                        }
                        break;

                    case 7://下对齐 
                        foreach (var item in listContrl)
                        {
                            item.Top = firstCtrl.Bottom - item.Height;
                        }
                        break;

                    case 8://宽度方向相同 
                        foreach (var item in listContrl)
                        {
                            item.Width = firstCtrl.Width;
                        }
                        break;

                    case 9://高度方向相同 
                        foreach (var item in listContrl)
                        {
                            item.Height = firstCtrl.Height;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
