using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView
{
    /// <summary>
    /// 产量显示界面
    /// </summary>
    public partial class CountView : UserControl
    {

        public event EventHandler<object> ClearEvent;
        /// <summary>
        /// 清空数据事件
        /// </summary>
        /// <param name="e">/param>
        protected void OnClearEvent(object e)
        {
            EventHandler<object> handler = ClearEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public CountView()
        {
            InitializeComponent();
        }

        private void CountView_Load(object sender, EventArgs e)
        {
            try
            {
                SequenceTestModel.SequenceModel sequence = XMLController.XmlControl.sequenceModelNew;  
                SetUserEnable();

                for (int i = 0; i < 11; i++)
                {
                    dataCountView.Rows.Add();
                    dataCountView.Rows[i].Height = 28;
                }

                dataCountView.Rows[0].Cells[0].Value = "上料数量";
                dataCountView.Rows[0].Cells[1].Value = sequence.productDataModel.LoadCount.ToString();

                dataCountView.Rows[1].Cells[0].Value = "下料数量";
                dataCountView.Rows[1].Cells[1].Value = sequence.productDataModel.UnLoadCount.ToString();

                dataCountView.Rows[2].Cells[0].Value = "良品数量";
                dataCountView.Rows[2].Cells[1].Value = sequence.productDataModel.OKCount.ToString();

                dataCountView.Rows[3].Cells[0].Value = "扫码不良数量";
                dataCountView.Rows[3].Cells[1].Value = sequence.productDataModel.ScanNGCount.ToString();

                dataCountView.Rows[4].Cells[0].Value = "疑似不良数量";
                dataCountView.Rows[4].Cells[1].Value = sequence.productDataModel.SeemNGCount.ToString();

                dataCountView.Rows[5].Cells[0].Value = "成品不良数量";
                dataCountView.Rows[5].Cells[1].Value = sequence.productDataModel.NGCount.ToString();

                dataCountView.Rows[6].Cells[0].Value = "良率";
                dataCountView.Rows[6].Cells[1].Value = sequence.productDataModel.OKPercent.ToString();

                dataCountView.Rows[7].Cells[0].Value = "测试时间";
                dataCountView.Rows[7].Cells[1].Value = GetTime(sequence.productDataModel.CostTime);

                dataCountView.Rows[8].Cells[0].Value = "暂停时间";
                dataCountView.Rows[8].Cells[1].Value = GetTime(sequence.productDataModel.PauseTime);

                dataCountView.Rows[9].Cells[0].Value = "报警时间";
                dataCountView.Rows[9].Cells[1].Value = GetTime(sequence.productDataModel.WarnTime);

                dataCountView.Rows[10].Cells[0].Value = "CT";
                dataCountView.Rows[10].Cells[1].Value = sequence.productDataModel.CT.ToString("0.00");
            }
            catch (Exception ex)
            {
                 
            }
        }

        private string GetTime(double count)
        {
            try
            {
                int miao = (int)count % 60;//获取秒钟
                int fenzhong = (int)((count - miao) / 60) % 60;//获取分钟
                int xiaoshi = (int)(count - miao) / 3600;//获取小时
                
                string str = string.Format("{0}:{1}:{2}", xiaoshi.ToString("D2"), fenzhong.ToString("D2"), miao.ToString("D2")); 

                return str;
            }
            catch (Exception ex)
            {
                return count.ToString();
            }
        }

        public void SetUserEnable()
        {
            try
            {
                bool bEnable = GlobalCore.Global.UserName != GlobalCore.Global.OperatorName;
               // btnClear.Enabled = bEnable;
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 获取测试的数据
        /// </summary>
        /// <param name="count">生产总数</param>
        /// <param name="okcount">OK数量</param>
        /// <param name="ngcount">NG数量</param>
        /// <param name="costtime">消耗时间</param>
        /// <param name="pausetime">暂停时间</param>
        /// <param name="warntime">报警时间</param>
        public void GetTestData(int loadcount, int unloadcount, int okcount, int ngcount, int scanngcount, int seemngcount, double costtime, double pausetime, double warntime)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    SequenceTestModel.SequenceModel sequence = XMLController.XmlControl.sequenceModelNew; 
                    if(sequence == null)
                    {
                        return;
                    }

                    //当测试数据有增加才计算CT
                    double ct = 0;
                    double okpercent = 0;
                    if (loadcount != 0 /*&& loadcount != sequence.productDataModel.LoadCount*/)
                    {
                        ct = costtime / loadcount;
                        sequence.productDataModel.CT = ct;

                        okpercent = (double)okcount / loadcount;
                        sequence.productDataModel.OKPercent = Math.Round(okpercent, 2);
                    }                     

                    sequence.productDataModel.LoadCount = loadcount;
                    sequence.productDataModel.UnLoadCount = unloadcount;
                    sequence.productDataModel.OKCount = okcount;
                    sequence.productDataModel.NGCount = ngcount;
                    sequence.productDataModel.ScanNGCount = scanngcount;
                    sequence.productDataModel.SeemNGCount = seemngcount;
                    sequence.productDataModel.CostTime = costtime;
                    sequence.productDataModel.PauseTime = pausetime;
                    sequence.productDataModel.WarnTime = warntime;

                    dataCountView.Rows[0].Cells[1].Value = sequence.productDataModel.LoadCount.ToString();
                    dataCountView.Rows[1].Cells[1].Value = sequence.productDataModel.UnLoadCount.ToString();
                    dataCountView.Rows[2].Cells[1].Value = sequence.productDataModel.OKCount.ToString();
                    dataCountView.Rows[3].Cells[1].Value = sequence.productDataModel.ScanNGCount.ToString();
                    dataCountView.Rows[4].Cells[1].Value = sequence.productDataModel.SeemNGCount.ToString();
                    dataCountView.Rows[5].Cells[1].Value = sequence.productDataModel.NGCount.ToString();
                    dataCountView.Rows[6].Cells[1].Value = sequence.productDataModel.OKPercent.ToString();
                    dataCountView.Rows[7].Cells[1].Value = GetTime(sequence.productDataModel.CostTime);
                    dataCountView.Rows[8].Cells[1].Value = GetTime(sequence.productDataModel.PauseTime);
                    dataCountView.Rows[9].Cells[1].Value = GetTime(sequence.productDataModel.WarnTime);
                    dataCountView.Rows[10].Cells[1].Value = sequence.productDataModel.CT.ToString("0.00");
                }));
            }
            catch (Exception ex)
            {
                
            }
        }

        private void 清除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SequenceTestModel.SequenceModel sequence = XMLController.XmlControl.sequenceModelNew;
            sequence.productDataModel.LoadCount = 0;
            sequence.productDataModel.UnLoadCount = 0;
            sequence.productDataModel.OKCount = 0;
            sequence.productDataModel.ScanNGCount = 0;
            sequence.productDataModel.SeemNGCount = 0;
            sequence.productDataModel.NGCount = 0;
            sequence.productDataModel.OKPercent = 0;
            sequence.productDataModel.CostTime = 0;
            sequence.productDataModel.PauseTime = 0;
            sequence.productDataModel.WarnTime = 0;
            sequence.productDataModel.CT = 0;


            dataCountView.Rows[0].Cells[1].Value = sequence.productDataModel.LoadCount.ToString();
            dataCountView.Rows[1].Cells[1].Value = sequence.productDataModel.UnLoadCount.ToString();
            dataCountView.Rows[2].Cells[1].Value = sequence.productDataModel.OKCount.ToString();
            dataCountView.Rows[3].Cells[1].Value = sequence.productDataModel.ScanNGCount.ToString();
            dataCountView.Rows[4].Cells[1].Value = sequence.productDataModel.SeemNGCount.ToString();
            dataCountView.Rows[5].Cells[1].Value = sequence.productDataModel.NGCount.ToString();
            dataCountView.Rows[6].Cells[1].Value = sequence.productDataModel.OKPercent.ToString();
            dataCountView.Rows[7].Cells[1].Value = "00:00:00";
            dataCountView.Rows[8].Cells[1].Value = "00:00:00";
            dataCountView.Rows[9].Cells[1].Value = "00:00:00";
            dataCountView.Rows[10].Cells[1].Value = sequence.productDataModel.CT.ToString("0.00");

            OnClearEvent(null);
        }

    }
}
