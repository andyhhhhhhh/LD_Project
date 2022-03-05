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

namespace ManagementView
{
    /// <summary>
    /// 测试结果类型的按钮
    /// </summary>
    public partial class BoxBtnView : UserControl
    {
        public BoxBtnView()
        {
            InitializeComponent();
        }

        private EnumLDResult _enumCosResult;
        /// <summary>
        /// 按钮代表的结果类型
        /// </summary>
        public EnumLDResult enumCosResult
        {
            get { return _enumCosResult; }
            set
            {
                _enumCosResult = value;
                SetBtnText();
            }
        } 

        private void BoxBtnView_Load(object sender, EventArgs e)
        {
            try
            {
                SetBtnText();
            }
            catch (Exception ex)
            {

            }
        }

        private void SetBtnText()
        {
            try
            {
                if(InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        switch (enumCosResult)
                        {
                            case EnumLDResult.Enum_Pass:
                                btnBoxType.Text = "测试OK";
                                btnBoxType.BackColor = Color.DarkSeaGreen;
                                break;
                            case EnumLDResult.Enum_Fail:
                                btnBoxType.Text = "测试NG";
                                btnBoxType.BackColor = Color.LightPink;
                                break;
                            case EnumLDResult.Enum_SeemNG:
                                btnBoxType.Text = "疑似NG";
                                btnBoxType.BackColor = Color.Gold;
                                break;

                            default:
                                btnBoxType.Text = "测试OK";
                                btnBoxType.BackColor = Color.DarkSeaGreen;
                                break;
                        }
                    }));
                }
                else
                {
                    switch (enumCosResult)
                    {
                        case EnumLDResult.Enum_Pass:
                            btnBoxType.Text = "测试OK";
                            btnBoxType.BackColor = Color.DarkSeaGreen;
                            break;
                        case EnumLDResult.Enum_Fail:
                            btnBoxType.Text = "测试NG";
                            btnBoxType.BackColor = Color.LightPink;
                            break;
                        case EnumLDResult.Enum_SeemNG:
                            btnBoxType.Text = "疑似NG";
                            btnBoxType.BackColor = Color.Gold;
                            break;

                        default:
                            btnBoxType.Text = "测试OK";
                            btnBoxType.BackColor = Color.DarkSeaGreen;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                enumCosResult = EnumLDResult.Enum_Pass;
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            try
            {
                enumCosResult = EnumLDResult.Enum_Fail;
            }
            catch (Exception ex)
            {

            }
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            try
            {
                enumCosResult = EnumLDResult.Enum_SeemNG;
            }
            catch (Exception ex)
            {

            }
        }
         

        /// <summary>
        /// 设置发散角不良或者976nm芯片Btn是否可见
        /// </summary>
        /// <param name="bshow">是否显示</param>
        /// <param name="bshow">是否显示</param>
        public void SetBtnVisible(bool bshow, bool b976nm)
        {
            try
            { 
                buttonItem1.Visible = !b976nm; 
            }
            catch (Exception ex)
            {
                 
            }
        }
        
    }
}
