using DevComponents.DotNetBar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView
{
    public class CommHelper
    {
        public static int GetComboxSelectIndex(ComboBox cmb, string currentValue)
        {
            int index = -1;
            IEnumerator iEnumerator = cmb.Items.GetEnumerator();
            while (iEnumerator.MoveNext())
            {
                index++;
                string value = iEnumerator.Current.ToString();
                if (string.Equals(value, currentValue))
                {
                    break;
                }
            }
            return index;
        }
        /// <summary>
        /// 添加页面到主界面并清除界面上已经存在的控件
        /// </summary>
        /// <param name="parentView"></param>
        /// <param name="childView"></param>
        /// <param name="clear">为true时，清除界面上已经存在的控件</param>
        public static void LayoutChildFillView(Control parentView, UserControl childView, bool clearParent)
        {
            try
            {
                if (parentView.Controls != null && childView != null)
                {
                    if (clearParent)
                    {
                        parentView.Controls.Clear();
                    }
                    parentView.Controls.Add(childView);
                    Control[] resultView = parentView.Controls.Find(childView.Name, true);
                    if (resultView.Length > 0)
                    {
                        resultView[0].Dock = DockStyle.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        /// <summary>
        /// 添加页面到主界面
        /// </summary>
        /// <param name="parentView">主界面上的Panel控件</param>
        /// <param name="childView">子界面详细界面控件</param>
        public static void LayoutChildFillView(Control parentView, UserControl childView)
        {
            try
            {
                if (parentView.Controls != null)
                {
                    parentView.Controls.Add(childView);
                    Control[] resultView = parentView.Controls.Find(childView.Name, true);
                    if (resultView.Length > 0)
                    {
                        resultView[0].Dock = DockStyle.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }
        /// <summary>
        /// 添加页面到主界面
        /// </summary>
        /// <param name="parentView">主界面上的Panel控件</param>
        /// <param name="childView">子界面详细界面控件</param>
        /// <param name="style">DockStyle</param>
        public static void LayoutChildFillView(Control parentView, UserControl childView, DockStyle style)
        {
            if (parentView.Controls != null)
            {
                parentView.Controls.Add(childView);
                Control[] resultView = parentView.Controls.Find(childView.Name, true);
                if (resultView.Length > 0)
                {
                    resultView[0].Dock = style;
                }
            }
        }

        /// <summary>
        /// 验证Text控件内容是否为空
        /// </summary>
        /// <param name="control"></param>
        public static bool ValidateControlNull(Control control)
        {
            if (control is TextBox)
            {
                if (string.IsNullOrEmpty(((TextBox)control).Text))
                {
                    MessageBoxEx.Show(((TextBox)control).Name + "不可为空！");
                    return false;
                }
            }
            else if (control is ComboBox)
            {
                if (string.IsNullOrEmpty(((ComboBox)control).Text))
                {
                    MessageBoxEx.Show("不可为空！");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateControlNull(params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (!ValidateControlNull(control))
                {
                    return false;
                }
            }
            return true;
        }
        public static void ClearControlToEmpty(params Control[] controls)
        {
            foreach (var control in controls)
            {
                ClearControlToEmpty(control);
            }
        }
        public static void ClearControlToEmpty(Control control)
        {
            if (control is TextBox)
            {
                ((TextBox)control).Text = "";
            }
            else if (control is ComboBox)
            {
                ((ComboBox)control).SelectedIndex = 0;
            }
        }
        public static void SetTextToTextBox(string text, params TextBox[] textBoxs)
        {
            foreach (var textBox in textBoxs)
            {
                textBox.Text = text;
            }
        }

        /// <summary>
        /// 按控件传入的顺序给Tab键进行升序排列
        /// </summary>
        /// <param name="controls"></param>
        public static void SortControlTabIndex(params Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].TabIndex = i + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static bool GetButtonStauts(Button btn)
        {
            bool status = true;
            if (btn != null)
            {
                //标准状态下(Standard)，点击为OFF操作
                if (btn.FlatStyle != FlatStyle.Standard)
                {
                    status = false;
                    btn.FlatStyle = FlatStyle.Standard;
                }
                else
                {
                    btn.FlatStyle = FlatStyle.Flat;
                }
            }
            else
            {
                throw new ArgumentException("无效参数，传入的不是Button对象");
            }
            return status;
        }


        public static int ToInt32(string str)
        {
            try
            {
                int value = 0;
                if (!string.IsNullOrEmpty(str))
                    value = Convert.ToInt32(str);
                return value;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return 0;
            }
        }
        public static double ToDouble(string str)
        {
            try
            {
                double value = 0;
                if (!string.IsNullOrEmpty(str))
                    value = Convert.ToDouble(str);
                return value;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return 0;
            }
        }
    }
}
