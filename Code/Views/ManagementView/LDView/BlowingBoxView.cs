using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using SequenceTestModel;

namespace ManagementView
{
    /// <summary>
    /// 下料料盘类型界面
    /// </summary>
    public partial class BlowingBoxView : UserControl
    {
        BoxBtnView[] m_boxViewArr;
        PanelEx[] m_panelArr;
        public BlowingBoxView()
        {
            InitializeComponent();
        }

        private void BlowingBoxView_Load(object sender, EventArgs e)
        {
            try
            {
                m_panelArr = new PanelEx[16] { panelEx1, panelEx2, panelEx3, panelEx4, panelEx5, panelEx6,
                    panelEx7, panelEx8, panelEx9, panelEx10, panelEx11, panelEx12, panelEx13, panelEx14, panelEx15, panelEx16 };
                m_boxViewArr = new BoxBtnView[16];
                for (int i = 0; i < m_boxViewArr.Length; i++)
                {
                    m_boxViewArr[i] = new BoxBtnView();
                    CommHelper.LayoutChildFillView(m_panelArr[i], m_boxViewArr[i]);
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 设置发散角不良Btn是否可见
        /// </summary>
        /// <param name="bshow">发散角是否显示</param>
        /// <param name="b976nm">是否是976nm芯片</param>
        public void SetBtnVisible(bool bshow, bool b976nm)
        {
            try
            {
                for (int i = 0; i < m_boxViewArr.Length; i++)
                {
                    m_boxViewArr[i].SetBtnVisible(bshow, b976nm);
                } 
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 设置结果List到按钮
        /// </summary>
        /// <param name="listCosResult">输入list结果</param>
        public void SetBlowingList(List<EnumLDResult> listCosResult)
        {
            try
            {
                if(listCosResult != null && listCosResult.Count > 0)
                {
                    for (int i = 0; i < m_boxViewArr.Length; i++)
                    {
                        m_boxViewArr[i].enumCosResult = listCosResult[i];
                    }
                }
                else
                {
                    MessageBoxEx.Show("未配置具体结果排列!!");
                }
            }
            catch (Exception ex)
            {
                 
            }
        } 

        /// <summary>
        /// 获取按钮的结果List
        /// </summary>
        /// <returns>返回结果list</returns>
        public List<EnumLDResult> GetBlowingList()
        {
            try
            {
                List<EnumLDResult> listCosResult = new List<EnumLDResult>();
                for (int i = 0; i < m_boxViewArr.Length; i++)
                {
                    listCosResult.Add(m_boxViewArr[i].enumCosResult);
                }

                return listCosResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}
