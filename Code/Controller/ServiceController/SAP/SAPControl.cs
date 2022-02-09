using SAP.Middleware.Connector;
using SequenceTestModel;
//using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceController
{
    public class SAPControl
    {
        public void SAPLogin(string systemName, string address, string systemNum, string userName, string password, string port, string lang,
           out string strStatus, out string strBatch, out string strReturn)
        {
            // rfc配置
            RfcConfigParameters argsP = new RfcConfigParameters();
            argsP.Add(RfcConfigParameters.Name, systemName);
            argsP.Add(RfcConfigParameters.AppServerHost, address);
            argsP.Add(RfcConfigParameters.SystemNumber, systemNum);
            // argsP.Add(RfcConfigParameters.SystemID, "QS7");
            argsP.Add(RfcConfigParameters.User, userName);
            argsP.Add(RfcConfigParameters.Password, password);
            argsP.Add(RfcConfigParameters.Client, port);
            argsP.Add(RfcConfigParameters.Language, lang);
            //argsP.Add(RfcConfigParameters.PoolSize, "5");
            //argsP.Add(RfcConfigParameters.MaxPoolSize, "10");
            //argsP.Add(RfcConfigParameters.IdleTimeout, "60");

            //获取rfc配置
            RfcDestination sapConfig = RfcDestinationManager.GetDestination(argsP); //NCO3.0如果framework不是2.0此处会报错，跟系统64还32无关
            RfcRepository rfcRepository = sapConfig.Repository;

            //调用
            IRfcFunction myfun = rfcRepository.CreateFunction("Z_MM_GAOYI_RECIEVE_GOODS_01"); //调用函数名 ZRFC_MARA_INFO
            myfun.SetValue("LT_CODE2D", "########03C207000019#####B0395201029001523##########400016443600004000#######20201001##CGP2#####1330"); //设置参数
            myfun.SetValue("LT_JOBNUM", "021058"); //设置参数
            myfun.SetValue("LT_TAX", "A"); //设置参数                                                                       
            myfun.Invoke(sapConfig); //执行函数

            //IRfcTable rfcTable = invoke.GetTable("ZIMPSTXS00330"); //获取内表
            //IRfcTable rfcTable = myfun.GetTable("ZMMT062"); //获取内表
            //string message = rfcTable.GetValue("MESSAGE").ToString();

            //另外一种方式
            strStatus = myfun.GetValue("LT_STATUS1").ToString();
            strBatch = myfun.GetValue("LT_BATCH1").ToString();
            strReturn = myfun.GetValue("LT_RETURN").ToString();

            sapConfig = null;
            myfun = null;
        }

        public void SAPQuery(out string strStatus, out string strBatch, out string strReturn)
        {
            strStatus = "";
            strBatch = "";
            strReturn = "";
        }
        
        public void SAPTest(SAPControlModel tModel, string strQRCode, string jobNum, string taxAB, out string strStatus, out string strBatch, out string strReturn)
        {
            // rfc配置
            RfcConfigParameters argsP = new RfcConfigParameters();
            argsP.Add(RfcConfigParameters.Name, tModel.SystemName);
            argsP.Add(RfcConfigParameters.AppServerHost, tModel.Address);
            argsP.Add(RfcConfigParameters.SystemNumber, tModel.SystemNum); 
            argsP.Add(RfcConfigParameters.User, tModel.UserName);
            argsP.Add(RfcConfigParameters.Password, tModel.Password);
            argsP.Add(RfcConfigParameters.Client, tModel.Port);
            argsP.Add(RfcConfigParameters.Language, tModel.Lang); 

            //获取rfc配置
            RfcDestination sapConfig = RfcDestinationManager.GetDestination(argsP); //NCO3.0如果framework不是2.0此处会报错，跟系统64还32无关
            RfcRepository rfcRepository = sapConfig.Repository;

            //调用
            IRfcFunction myfun = rfcRepository.CreateFunction("Z_MM_GAOYI_RECIEVE_GOODS_01"); //调用函数名 ZRFC_MARA_INFO
            myfun.SetValue("LT_CODE2D", strQRCode); //设置参数
            myfun.SetValue("LT_JOBNUM", jobNum); //设置参数
            myfun.SetValue("LT_TAX", taxAB); //设置参数                                            
            myfun.Invoke(sapConfig); //执行函数 

            //另外一种方式
            strStatus = myfun.GetValue("LT_STATUS1").ToString();
            strBatch = myfun.GetValue("LT_BATCH1").ToString();
            strReturn = myfun.GetValue("LT_RETURN").ToString();

            sapConfig = null;
            myfun = null;
        }
        
    }
}
