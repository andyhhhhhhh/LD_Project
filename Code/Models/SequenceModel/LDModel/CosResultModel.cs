using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    public class CosResultModel
    {

    }

    /// <summary>
    /// IW测试结果
    /// </summary>
    public class IWResultModel
    {
        /// <summary>
        /// 测试结果 Testing--测试中 OK--测试完成 IDLE--测试未开始
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 测试类型 iw
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 输出结果Lamda
        /// </summary>
        public List<string> listLamda { get; set; }
        /// <summary>
        /// 输出结果Strength
        /// </summary>
        public List<string> listStrength { get; set; }

        /// <summary>
        /// FWHM nm
        /// </summary>
        public string FWHM_Nm { get; set; }
        /// <summary>
        /// 峰值波长
        /// </summary>
        public string PeekWave { get; set; }
        /// <summary>
        /// 中心波长
        /// </summary>
        public string CenterWave { get; set; }
        /// <summary>
        /// 95%波长
        /// </summary>
        public string Percent95FWHM { get; set; }
    }

    /// <summary>
    /// LIV测试结果
    /// </summary>
    public class LIVResultModel
    {
        /// <summary>
        /// 测试结果 Testing--测试中 OK--测试完成 IDLE--测试未开始
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 测试类型 iw
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 输出结果I_Amp
        /// </summary>
        public List<string> listI_Amp { get; set; }
        /// <summary>
        /// 输出结果V_Volt 
        /// </summary>
        public List<string> listV_Volt { get; set; }
        /// <summary>
        /// 输出结果P_Watt 
        /// </summary>
        public List<string> listP_Watt { get; set; }
        /// <summary>
        /// 输出结果TE_P_Watt 
        /// </summary>
        public List<string> listTE_P_Watt { get; set; }
        /// <summary>
        /// 输出结果TM_P_Watt 
        /// </summary>
        public List<string> listTM_P_Watt { get; set; }

        /// <summary>
        /// LD电流
        /// </summary>
        public string LDAmp { get; set; }
        /// <summary>
        /// LD电压
        /// </summary>
        public string LDVolt { get; set; }
        /// <summary>
        /// 偏振度
        /// </summary>
        public string PolarDegree { get; set; }
        /// <summary>
        /// LD总功率
        /// </summary>
        public string LDPower { get; set; } 
        /// <summary>
        /// PDA电压_TE
        /// </summary>
        public string PDAVolt_TE { get; set; } 
        /// <summary>
        /// PDA电压_TM
        /// </summary>
        public string PDAVolt_TM { get; set; }
        /// <summary>
        /// LD功率_TE
        /// </summary>
        public string LDPower_TE { get; set; }
        /// <summary>
        /// LD功率_TM
        /// </summary>
        public string LDPower_TM { get; set; }
        /// <summary>
        /// 阈值电流
        /// </summary>
        public string Ith { get; set; }

    }

    /// <summary>
    /// 发散角结果
    /// </summary>
    public class DiverAngResultModel
    {
        /// <summary>
        /// FWHM deg
        /// </summary>
        public string FwhmDeg { get; set; }
        /// <summary>
        /// %86 deg
        /// </summary>
        public string Percent86Deg { get; set; }
        /// <summary>
        /// %95 deg
        /// </summary>
        public string Percent95Deg { get; set; }
        /// <summary>
        /// Angle deg
        /// </summary>
        public string AngleDeg { get; set; }
    }

    /// <summary>
    /// 特征结果
    /// </summary>
    public class CharResultModel
    {
        public string ffp_FW95_X { get; set; }
        public string ffp_FW95_Y { get; set; }
        public string ffp_FWHM_X { get; set; }
        public string ffp_FWHM_Y { get; set; }
        public string ffp_Iop { get; set; }
        public string piv_Iop { get; set; }
        public string piv_Ith { get; set; }
        public string piv_Plarization_Average { get; set; }
        public string piv_Polarization_Iop { get; set; }
        public string piv_Pop { get; set; }
        public string piv_Rs { get; set; }
        public string piv_SE { get; set; }
        public string piv_SE_Max { get; set; }
        public string piv_VIop { get; set; }
        public string piv_Von { get; set; }
        public string piv_WPE_Iop { get; set; }
        public string spectrum_Center { get; set; }
        public string spectrum_FW95 { get; set; }
        public string spectrum_FWHM { get; set; }
        public string spect_rum_Iop_Spect { get; set; }
        public string spectrum_Peak { get; set; }

        public string threshold_watt { get; set; }

    }
}
