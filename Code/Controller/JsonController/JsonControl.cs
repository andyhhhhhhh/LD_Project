using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonController
{
    public class JsonControl
    {
        private Systemparameters _SystemPara;

        public Systemparameters SystemPara
        {
            get { return _SystemPara = ParseJsonFileAction(); }

            set { _SystemPara = value; }
        }

        public void SaveConfigurationClassToJsonFile()
        {
            try
            {
                string path = Application.StartupPath + "\\" + "SoftwareConfig" + "\\Parameters.cfg";

                //Task task = new Task(() =>
                //{
                    List<string> mParameter = new List<string>();
                    try
                    {
                        string json0 = JsonConvert.SerializeObject(_SystemPara);
                        mParameter.Add(json0);
                    }
                    catch (Exception ce)
                    {
                        Debug.WriteLine(ce.Message);
                    }
                   
                    WriteStringToJson(mParameter);
                //});
                //task.Start();
                Debug.WriteLine("Save File to Json Complete!");
            }
            catch (Exception ce)
            {
                Debug.WriteLine("Save File Error:\n" + ce.Message);
              
            }
        }

        /// <summary>
        /// Write Json object string to File
        /// </summary>
        /// <param name="value">json string</param>
        private void WriteStringToJson(List<string> value)
        {
            string path = Application.StartupPath + "\\" + "SoftwareConfig" + "\\Parameters.json";
            FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(f, System.Text.Encoding.Default);
            for (int i = 0; i < value.Count; i++)
            {
                sw.WriteLine(value[i]);
            }
            sw.Flush();
            sw.Close();
            f.Close();
        }

        public Systemparameters ParseJsonFileAction()
        {
            try
            {
                //log.Info("Start to Parse Json file");
                var strings = ReadTxtToJson(Application.StartupPath + "\\" + "SoftwareConfig" + "\\Parameters.json");
                
                Systemparameters a = JsonConvert.DeserializeObject<Systemparameters>(strings[0]);
                return a;
                
            }
            catch (Exception ce)
            {
                Debug.WriteLine("Parse Jason File failed!" + ce.Message);
                return new Systemparameters();
            }
        }

        private List<string> ReadTxtToJson(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            List<String> lines = new List<string>();
            string tempString = string.Empty;
            while ((tempString = sr.ReadLine()) != null)
            {
                lines.Add(tempString.ToString());
            }
            sr.Close();
            return lines;
        }

        public static bool ParsePost(string jsonText, ref int error,  ref string msg)
        {
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                error = Int32.Parse(jo["Error"].ToString());
                msg = jo["Message"].ToString();

                return error == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ParseJson_IW(double minWave, double maxWave, string jsonText, ref string result, ref string type, ref List<string> listLamda, ref List<string> listStrength)
        {
            try
            {
                listLamda = new List<string>();
                listStrength = new List<string>();

                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                result = jo["Result"].ToString();
                type = jo["Type"].ToString();
                var zone_en = jo["Values"].ToArray();
                for (int i = 0; i < zone_en.Length; i++)
                {
                    string str1 = zone_en[i]["Lamda"].ToString();
                    if(Double.Parse(str1) > maxWave || Double.Parse(str1) < minWave)
                    {
                        continue;
                    }
                    str1 = Double.Parse(str1).ToString("f1");
                    listLamda.Add(str1);
                    string strLength = zone_en[i]["Strength"].ToString();
                    strLength = Double.Parse(strLength).ToString("f0");
                    listStrength.Add(strLength);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 
        }

        public static bool ParseJson_LIV(string jsonText, ref string result, ref string type, ref List<string> listI_Amp, ref List<string> listV_Volt,
                                            ref List<string> listP_Watt, ref List<string> listTE_P_Watt, ref List<string> listTM_P_Watt)
        {
            try
            { 

                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                result = jo["Result"].ToString();
                type = jo["Type"].ToString();
                var zone_en = jo["Values"].ToArray();
                for (int i = 0; i < zone_en.Length; i++)
                {
                    string iamp = zone_en[i]["I_Amp"].ToString();
                    iamp = Double.Parse(iamp).ToString("f1"); 
                    listI_Amp.Add(iamp);
                    string vvlot = zone_en[i]["V_Volt"].ToString();
                    vvlot = Double.Parse(vvlot).ToString("f2");
                    listV_Volt.Add(vvlot);
                    string pwatt = zone_en[i]["P_Watt"].ToString();
                    pwatt = Double.Parse(pwatt).ToString("f1"); 
                    listP_Watt.Add(pwatt);
                    string tepwatt = zone_en[i]["TE_P_Watt"].ToString();
                    tepwatt = Double.Parse(tepwatt).ToString("f4");
                    listTE_P_Watt.Add(tepwatt);
                    string tmpwatt = zone_en[i]["TM_P_Watt"].ToString();
                    tmpwatt = Double.Parse(tmpwatt).ToString("f4");
                    listTM_P_Watt.Add(tmpwatt);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ParseJson_Char(string jsonText, ref string result, ref string type, ref string ffp_FW95_X, ref string ffp_FW95_Y,ref string ffp_FWHM_X, ref string ffp_FWHM_Y, ref string ffp_Iop,
            ref string piv_Iop, ref string piv_Ith, ref string piv_Plarization_Average, ref string piv_Polarization_Iop, ref string piv_Pop, ref string piv_Rs, ref string piv_SE, ref string piv_SE_Max, 
            ref string piv_VIop, ref string piv_Von, ref string piv_WPE_Iop, ref string piv_TM_P, ref string piv_TE_P, ref string spectrum_Center, ref string spectrum_FW95, ref string spectrum_FWHM, ref string spect_rum_Iop_Spect, ref string spectrum_Peak, ref string threshold_watt)
        {
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                result = jo["Result"].ToString();
                type = jo["Type"].ToString();
                var ffp = jo["ffp"];
                ffp_FW95_X = Double.Parse(ffp["FW95_X"].ToString()).ToString("f2");
                ffp_FW95_Y = Double.Parse(ffp["FW95_Y"].ToString()).ToString("f2");
                ffp_FWHM_X = Double.Parse(ffp["FWHM_X"].ToString()).ToString("f2");
                ffp_FWHM_Y = Double.Parse(ffp["FWHM_Y"].ToString()).ToString("f2");
                ffp_Iop = Double.Parse(ffp["Iop"].ToString()).ToString("f2");

                var piv = jo["piv"];
                piv_Iop = Double.Parse(piv["Iop"].ToString()).ToString("f2");
                piv_Ith = Double.Parse(piv["Ith"].ToString()).ToString("f2");
                piv_Plarization_Average = Double.Parse(piv["Polarization_Average"].ToString()).ToString("f1");
                piv_Polarization_Iop = Double.Parse(piv["Polarization_Iop"].ToString()).ToString("f2");
                piv_Pop = Double.Parse(piv["Pop"].ToString()).ToString("f2");
                piv_Rs = Double.Parse(piv["Rs"].ToString()).ToString("f2");
                piv_SE = Double.Parse(piv["SE"].ToString()).ToString("f2");
                piv_SE_Max = Double.Parse(piv["SE_Max"].ToString()).ToString("f2");
                piv_VIop = Double.Parse(piv["VIop"].ToString()).ToString("f2");
                piv_Von = Double.Parse(piv["Von"].ToString()).ToString("f2");
                piv_WPE_Iop = Double.Parse(piv["WPE_Iop"].ToString()).ToString("f2");
                piv_TM_P = Double.Parse(piv["TM_P_Watt"].ToString()).ToString("f2");
                piv_TE_P = Double.Parse(piv["TE_P_Watt"].ToString()).ToString("f2");
                threshold_watt = Double.Parse(piv["Threshold_Watt"].ToString()).ToString("f2");

                var spectrum = jo["spectrum"];
                spectrum_Center = Double.Parse(spectrum["Center"].ToString()).ToString("f2");
                spectrum_FW95 = Double.Parse(spectrum["FW95"].ToString()).ToString("f2");
                spectrum_FWHM = Double.Parse(spectrum["FWHM"].ToString()).ToString("f2");
                spect_rum_Iop_Spect = Double.Parse(spectrum["Iop"].ToString()).ToString("f2");
                spectrum_Peak = Double.Parse(spectrum["Peak"].ToString()).ToString("f1");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ParseParam(string name, string jsonText, ref string MaxCurrent, ref string NormalCurrent, ref string StartVoltage, ref string StepCurrent, ref string StepWaitTimeMs, ref string TEconst, ref string TEsale,
            ref string TMconst, ref string TMscale, ref string LamdaOffset, ref string WattCheckCur, ref string WattCheckValue)
        {
            try
            { 
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonText);
                var joName = jo[name];
                MaxCurrent = joName["Logic/MaxCurrent"].ToString();
                NormalCurrent = joName["Logic/NormalCurrent"].ToString();
                StartVoltage = joName["Logic/StartVoltage"].ToString();
                StepCurrent = joName["Logic/StepCurrent"].ToString();
                StepWaitTimeMs = joName["Logic/StepWaitTimeMs"].ToString();
                TEconst = joName["Logic/TEconst"].ToString();
                TEsale = joName["Logic/TEscale"].ToString();
                TMconst = joName["Logic/TMconst"].ToString();
                TMscale = joName["Logic/TMscale"].ToString();
                LamdaOffset = joName["Logic/LamdaOffset"].ToString();
                WattCheckCur = joName["Logic/WattCheckCur"].ToString();
                WattCheckValue = joName["Logic/WattCheckValue"].ToString();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
