using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Runtime.InteropServices;
using HalconView;

namespace ManagementView
{
    public partial class TrigSnapView : Form
    {
        public HTuple hv_AcqHandle = null;//定义相机句柄
        public HObject img = null;//定义图像变量
        HalconAPI.HFramegrabberCallback delegateCallback;//定义一个委托

        HSmartWindow m_hSmartWindow = new HSmartWindow();
        public TrigSnapView()
        {
            InitializeComponent();
        }

        private void TrigSnapView_Load(object sender, EventArgs e)//窗体加载的时候委托绑定及开启相机等操作
        {
            try
            { 
                CommHelper.LayoutChildFillView(panel1, m_hSmartWindow);
                //给委托绑定
                delegateCallback = MyCallbackFunction;
                //开启相机
                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive",
                 -1, "default", -1, "false", "default", "c42f90fa3e47_GEV_MVCA05010GM",
                  0, -1, out hv_AcqHandle);
                //下面开启硬触发
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSource", "Line0");
                //下面设置连续采集，上升沿触发，曝光模式等
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "AcquisitionMode", "Continuous");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerSelector", "FrameBurstStart");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerActivation", "RisingEdge");
                //HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureMode", "Timed");
                //设置曝光时间
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", 5000.0);
                //下面为设置用不超时
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "grab_timeout", 1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int test = 1;//随便定义的一个变量，后面会取其地址带入回调函数的user_context

        public int MyCallbackFunction(IntPtr handle, IntPtr context, IntPtr user_context)//回调函数
        {
            try
            {
                HOperatorSet.GrabImage(out img, hv_AcqHandle);

                m_hSmartWindow.DisplayImage(new HImage(img));
                
                return 0;
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);//显示错误
                return -1;
            }

        }

        private void btnReg_Click(object sender, EventArgs e)//这里注册回调
        {
            try
            {
                IntPtr ptr = Marshal.GetFunctionPointerForDelegate(delegateCallback);//取回调函数的地址
                IntPtr ptr1 = GCHandle.Alloc(test, GCHandleType.Pinned).AddrOfPinnedObject();//取test变量的地址

                //因为大华的相机不支持"transfer_end"等模式，只能用 "LineStatus"作演示
                HOperatorSet.SetFramegrabberCallback(hv_AcqHandle, "LineStatus", ptr, ptr1);//注册回调函数
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetPara_Click(object sender, EventArgs e)//返回支持available_callback_types的列表参数
        {
            HTuple Value = null;
            //下面返回支持回调的列表
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "available_callback_types", out Value);
            int l = Value.Length;
            for (long i = 0; i < l; i++)
            {
                this.listBox1.Items.Add(Value[i].S);
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)//关闭相机
        {
            if (hv_AcqHandle == null) return;
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
        }

        //-------以下是循环方式采集图像--------------------------------------
        private bool exitloop = false;//推出循环命令
        private void btnLoop_Click(object sender, EventArgs e)//采用循环的方式去取图
        {
            exitloop = false;
            btnExitLoop.Focus();
            System.Threading.Thread.Sleep(100);
            while (exitloop == false)
            {
                Application.DoEvents();
                try
                {
                    HOperatorSet.GrabImage(out img, hv_AcqHandle);
                    m_hSmartWindow.DisplayImage(new HImage(img)); 
                    Application.DoEvents();
                }
                catch (Exception)
                {
                    Application.DoEvents();
                    continue;
                    //throw;
                }
            }
        }

        private void btnExitLoop_Click(object sender, EventArgs e)//退出循环
        {
            exitloop = true;
        }
         
    }
}
