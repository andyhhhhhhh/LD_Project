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
    public partial class NumInput : UserControl
    {
        private OskView m_oskView;
        public NumInput()
        {
            InitializeComponent();
        }

        private string _sText = "";
        /// <summary>
        /// 输出输入的值
        /// </summary>
        public string sText
        {
            get
            {
                return _sText;
            }
            set
            {
                _sText = value;

                if(string.IsNullOrEmpty(_sText))
                {
                    doubleInput1.Value = 0;
                }
                else
                {
                    double dvalue= 0;
                    bool bParse = Double.TryParse(_sText, out dvalue);
                    if(bParse)
                    {
                        doubleInput1.Value = (double)dvalue;
                    }
                }
            }
        }

        private int _pointNum = 0;
        /// <summary>
        /// 设置小数点位
        /// </summary>
        public int PointNum
        {
            get
            {
                return _pointNum;
            }
            set
            {
                _pointNum = value;
                string str = "0";
                if(value > 0)
                {
                    str = "0.";
                    for (int i = 0; i < value; i++)
                    {
                        str += "0";
                    }
                }
               
                doubleInput1.DisplayFormat = str;
            }
        }

        private bool _bInt = true;
        /// <summary>
        /// 确认是否要输入Int
        /// </summary>
        public bool bInt
        {
            get
            {
                return _bInt;
            }
            set
            {
                _bInt = value;
                if(value)
                {
                    PointNum = 0;
                }
            }
        }

        private double _MinValue = -999999999;
        /// <summary>
        /// 输入最小值
        /// </summary>
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                _MinValue = value;
                doubleInput1.MinValue = value;
            }
        }
        
        private double _MaxValue = 999999999;
        /// <summary>
        /// 输入最大值
        /// </summary>
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                doubleInput1.MaxValue = value;
            }
        }
        
        private void doubleInput1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                sText = doubleInput1.Value.ToString();
                if (!string.IsNullOrEmpty(sText))
                {
                    OnTxtValueEvent(sText);
                }
            }
            catch (Exception ex)
            {
                  
            }
        }

        private void doubleInput1_Enter(object sender, EventArgs e)
        {
         
        }

        private void doubleInput1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalCore.Global.EnableOsk)
                {
                    return;
                }

                if (m_oskView != null && !m_oskView.IsDisposed)
                {
                    m_oskView.Hide();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void doubleInput1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalCore.Global.EnableOsk)
                {
                    return;
                }
                Rectangle rect = new Rectangle();
                rect = Screen.GetWorkingArea(this); 
                if (m_oskView == null || m_oskView.IsDisposed)
                {
                    m_oskView = new OskView(doubleInput1, bInt);
                    m_oskView.InitValue = doubleInput1.Value.ToString();
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }
                    m_oskView.Location = p;
                    m_oskView.ShowDialog();
                }
                else
                {
                    Control ctrl = sender as Control;
                    Point p = this.PointToScreen(new Point(ctrl.Left, ctrl.Bottom));

                    if (rect.Width < p.X + m_oskView.Width)
                    {
                        p.X = rect.Width - m_oskView.Width;
                    }

                    if (rect.Height < p.Y + m_oskView.Height)
                    {
                        p.Y = rect.Height - m_oskView.Height;
                    }
                    m_oskView.Location = p;
                    m_oskView.InitValue = doubleInput1.Value.ToString();
                    m_oskView.ShowDialog();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public event EventHandler<string> TxtValueEvent;
        private void OnTxtValueEvent(string e)
        {
            TxtValueEvent?.Invoke(this, e);
        }
    }

}
