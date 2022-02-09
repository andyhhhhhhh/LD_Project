using SequenceTestModel;
using SerialPortModel;
using ServiceCollection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortController
{

    public  struct PlcParam
    {
        public string rAbsMove;
        public int dMoveXPosition;
        public int dMoveYPosition;
        //public int dSpeed;
        public string rAbsMoveFinish;
    
        public int dXCurPosition;
        public int dYCurPosition;    
    }

    public class PanasonicPLCCommand
    {
        public enum EMPlcSerialPortType
        {
            emDefalut,
        }
        public enum EMPointType
        {
            emBit,
            emDTBit,
            emDoubleWord,
        }

        public STPlcSettingsInfo m_PlcSettingsInfo = null;
        //private SerialPort m_SerialPort = null;
        SerialPortControl m_SerialPortControl = new SerialPortControl();
        ComModel m_SerialPortModel = null;
        public  List<STPlcPoint> m_WarningPointList = null;
        private Mutex m_PortMutex = new Mutex();
        public  PlcParam m_PlcParam = new PlcParam();

        private bool m_bReadPortExcaption = false;
        private bool m_bWritePortExcaption = false;

        public String m_PlcInitPath = "";
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern int ReadIni(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public class STPlcSettingsInfo
        {
            public EMPlcSerialPortType emPlcSerialPortType = EMPlcSerialPortType.emDefalut;
            public int nSerialPortIndex = 0;
            public int nBoundRate = 9600;
            public int nParity = 1;
            public int nDataBits = 8;
            public int nStopBits = 1;

            public List<STPlcPoint> stPointList = null;
        }
        public class STPlcPoint
        {
            public EMPlcSerialPortType emPlcSerialPortType = EMPlcSerialPortType.emDefalut;
            public EMPointType emPointType = EMPointType.emBit;
            public String strAddrType = "";
            public String strAddrCode = "";
            public String strDesc = "";

            public STPlcPoint(EMPlcSerialPortType emPlcSerialPortType, EMPointType emPointType, String strAddrType, String strAddrCode, String strDesc)
            {
                this.emPlcSerialPortType = emPlcSerialPortType;
                this.emPointType = emPointType;
                this.strAddrType = strAddrType;
                this.strAddrCode = strAddrCode;
                this.strDesc = strDesc;
            }
        }

        public bool Init(string comPort)
        {
            if(!OpenAllSerailPort(comPort))
            {
                return false;
            }
            
            Thread.Sleep(100);

            return true;
        }

        public void Close()
        {
            if(null != m_SerialPortControl)
            {
                m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortClose);
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType">X点位，Y点位，R点位</param>
        /// <param name="strCode">寄存器(如30，10C)</param>
        /// <param name="eMPointType"></param>
        /// <param name="on">点位ON/OFF</param>
        public bool WriteBit(string strType, string strCode, EMPointType eMPointType, bool on)
        {
            STPlcPoint stPlcPoint = new STPlcPoint(EMPlcSerialPortType.emDefalut, eMPointType, strType, strCode, "");
            UInt32 nData = 0;
            if (on)
            {
                nData = 1;
            }

            if (null != stPlcPoint)
            {
                bool result = WriteSinglePlcPoint(stPlcPoint, nData);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool WriteRBit(string strCode, bool on)
        {
            return WriteBit("R", strCode, EMPointType.emBit, on);
        }
        public bool WriteBit(string strType, string strCode, EMPointType eMPointType, string data)
        {
            STPlcPoint stPlcPoint = new STPlcPoint(EMPlcSerialPortType.emDefalut, eMPointType, strType, strCode, "");

            if (null != stPlcPoint)
            {
                bool result = WriteSinglePlcPoint(stPlcPoint, data);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ReadBit(string strType, string strCode, EMPointType eMPointType, out UInt32 nData)
        {
            STPlcPoint stPlcPoint = new STPlcPoint(EMPlcSerialPortType.emDefalut, eMPointType, strType, strCode, "");
            nData = 0;

            if (null != stPlcPoint)
            {
                bool result = ReadSinglePlcPoint(stPlcPoint, out  nData);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

    
        }

        public bool ReadRBit(string strCode, out UInt32 nData)
        {
            return ReadBit("R", strCode, EMPointType.emBit, out nData);
        }

        public bool WriteDTFloat( string strStartCode, float dStartData)
        {
            string strStartData = dStartData.ToString();

            string strWriteCmd = GetWriteDTFloatCmd("D",strStartCode,strStartData);

            if (null == strWriteCmd)
            {
                return false;
            }

            //SerialPort stSerialPort = GetSerialPort(EMPlcSerialPortType.emDefalut);
            //if (null == stSerialPort)
            //{
            //    return false;
            //}

            //if (!stSerialPort.IsOpen)
            //{
            //    return false;
            //}

            //Byte[] Buffer = null;
            //int nLen = 0;
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                //SwitchStringsToByte(strWriteCmd, out Buffer, out nLen);
                //stSerialPort.Write(Buffer, 0, nLen);
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if(!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            if (strReceived.Length < 4)
            {
                return false;
            }

            if (strReceived[3].Equals('!'))
            {
                return false;
            }

            if (!strReceived[3].Equals('$'))
            {
                return false;
            }

            return true;
        }

        public bool WriteDTMultiFloat(string strStartCode, float[] dWriteData)
        {
            
            string strWriteCmd = GetWriteDTMultiFloatCmd("D", strStartCode, dWriteData);

            if (null == strWriteCmd)
            {
                return false;
            }

            
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }


            if (strReceived[3].Equals('!'))
            {
                return false;
            }

            if (!strReceived[3].Equals('$'))
            {
                return false;
            }

            return true;
        }

        public bool WriteDTMultiShort(string strStartCode, short[] sWriteData)
        {
            
            
            string strWriteCmd = GetWriteDTMultiShortCmd("D", strStartCode, sWriteData);

            if (null == strWriteCmd)
            {
                return false;
            }

            //SerialPort stSerialPort = GetSerialPort(EMPlcSerialPortType.emDefalut);
            //if (null == stSerialPort)
            //{
            //    return false;
            //}

            //if (!stSerialPort.IsOpen)
            //{
            //    return false;
            //}

            //Byte[] Buffer = null;
            //int nLen = 0;
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                //SwitchStringsToByte(strWriteCmd, out Buffer, out nLen);
                //stSerialPort.Write(Buffer, 0, nLen);
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            if (strReceived.Length < 4)
            {
                return false;
            }

            if (strReceived[3].Equals('!'))
            {
                return false;
            }

            if (!strReceived[3].Equals('$'))
            {
                return false;
            }

            return true;
        }


        public bool ReadDTFloat(string strStartCode,out float fData)
        {
            fData = 0;

            string strWriteCmd = GetReadDTFloatCmd("D",strStartCode);

            if (null == strWriteCmd)
            {
                return false;
            }

            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            bool bError = false;

            if(!ParserReceiveFloat(strReceived, out bError, out fData))
            {
                return false;
            }
            

            return true;
        }

        public bool ReadDTFloatXY(string strStartCode, out float fXData,out float fYData)
        {
            fXData = 0;
            fYData = 0;

            string strWriteCmd = GetReadDTMultiFloatCmd("D", strStartCode,2);

            if (null == strWriteCmd)
            {
                return false;
            }

            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                //SwitchStringsToByte(strWriteCmd, out Buffer, out nLen);
                //stSerialPort.Write(Buffer, 0, nLen);
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            bool bError = false;

            if (!ParserReceiveMultiFloat(strReceived, out bError, out fXData,out fYData))
            {
                return false;
            }


            return true;
        }

        public bool WriteDTShort(string strStartCode, short sStartData)
        {
            string strStartData = sStartData.ToString();

            string strWriteCmd = GetWriteDTShortCmd("D", strStartCode, strStartData);

            if (null == strWriteCmd)
            {
                return false;
            }

            //SerialPort stSerialPort = GetSerialPort(EMPlcSerialPortType.emDefalut);
            //if (null == stSerialPort)
            //{
            //    return false;
            //}

            //if (!stSerialPort.IsOpen)
            //{
            //    return false;
            //}

            //Byte[] Buffer = null;
            //int nLen = 0;
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                //SwitchStringsToByte(strWriteCmd, out Buffer, out nLen);
                //stSerialPort.Write(Buffer, 0, nLen);
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            if(strReceived.Length < 4)
            {
                return false;
            }

            if (strReceived[3].Equals('!'))
            {
                return false;
            }

            if (!strReceived[3].Equals('$'))
            {
                return false;
            }

            return true;
        }

        public bool ReadDTShort(string strStartCode, out short fData)
        {
            fData = 0;

            string strWriteCmd = GetReadDTShortCmd("D", strStartCode);

            if (null == strWriteCmd)
            {
                return false;
            }


            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();

                return false;
            }

            bool bError = false;

            if (!ParserReceiveShort(strReceived, out bError, out fData))
            {
                return false;
            }


            return true;
        }

        public string GenBitCmd(string strType, string strCode, string on, bool isWrite)
        {
            if (isWrite)
            {
                return GetWriteCmd(strType, strCode, on);
            }
            else
            {
                EMPointType type = EMPointType.emBit;
                //if (strType.Contains("D"))
                //{
                //    type = EMPointType.emDTBit;
                //}
                STPlcPoint stPlcPoint = new STPlcPoint(EMPlcSerialPortType.emDefalut, type, strType, strCode, "");
                //STPlcPoint[] stPlcPoints = { stPlcPoint };

                string strReadCmd;

                GenReadCmd(stPlcPoint, out strReadCmd);

                return strReadCmd;
            }
        }
        public string GetWriteCmd(string strType, string strCode, string on)
        {
            string strWriteCmd = "";
            byte[] Buffer;
            int nLen = 0;
            EMPointType type = EMPointType.emBit;
            if (strCode.Contains("D"))
            {
                type = EMPointType.emDTBit;
            }
            STPlcPoint stPlcPoint = new STPlcPoint(EMPlcSerialPortType.emDefalut, type, strType, strCode, "");
            UInt32 nData = 0;
            if (on.Equals("1"))
            {
                nData = 1;
            }

            if (null != stPlcPoint)
            {
                GenWriteCmd(stPlcPoint, nData, out strWriteCmd);
                SwitchStringsToByte(strWriteCmd, out Buffer, out nLen);
                strWriteCmd = Encoding.ASCII.GetString(Buffer);
            }
            return strWriteCmd;
        }

        public string GetReadCmd(string strType, string strCode)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";


            string cmdType = "RCS";
            {
                int nCodeLenth = strCode.Length;
                if (4 >= nCodeLenth)
                {
                    strAddr = strCode;
                    for (int j = 0; j < (4 - nCodeLenth); j++)
                    {
                        strAddr = "0" + strAddr;
                    }

                    strReadCmd += strType + strAddr;
                }
            }
            strReadCmd = strPre + cmdType + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }
        public string GetReadMultiCmd(STPlcPoint[] stPoint)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;
            if (null == stPoint)
            {
                return "";
            }
            int pointNum = stPoint.Length;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";


            string cmdType = "RCP";
            for (int i = 0; i < pointNum; i++)
            {
                if (stPoint[0].emPointType == EMPointType.emDTBit)
                {
                    //cmdType = "RD";
                    //int nCodeLenth = stPoint[i].strAddrCode.Length;
                    //if (4 >= nCodeLenth)
                    //{
                    //    strAddr = stPoint[i].strAddrCode;
                    //    for (int j = 0; j < (5 - nCodeLenth); j++)
                    //    {
                    //        strAddr = "0" + strAddr;
                    //    }

                    //    strReadCmd += stPoint[i].strAddrType + strAddr;
                    //}
                    //strReadCmd = strPre + cmdType + strReadCmd + strTail;
                }
                else
                {
                    int nCodeLenth = stPoint[i].strAddrCode.Length;
                    if (4 >= nCodeLenth)
                    {
                        strAddr = stPoint[i].strAddrCode;
                        for (int j = 0; j < (4 - nCodeLenth); j++)
                        {
                            strAddr = "0" + strAddr;
                        }

                        strReadCmd += stPoint[i].strAddrType + strAddr;
                    }
                }
            }
            strReadCmd = strPre + cmdType + pointNum + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        //:%01$RD 4006 0000 0000 BCC  -  高低位相反 4006->0640
        public string GetReadDTShortCmd(string strType, string strStartCode)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            //String strEndAddr = "";

            string dType = "D";
            string cmdType = "RD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCmd += strAddr;
            }
            //////
            //int nEndCode = Convert.ToInt16(strStartCode) + 1;

            //strEndCode = Convert.ToString(nEndCode);
            //nCodeLenth = strEndCode.Length;
            //if (5 >= nCodeLenth)
            //{
            //    strEndAddr = strEndCode;
            //    for (int j = 0; j < (5 - nCodeLenth); j++)
            //    {
            //        strEndAddr = "0" + strEndAddr;
            //    }

            //    strReadCmd += strEndAddr;
            //}

            strReadCmd = strPre + cmdType + dType + strReadCmd + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        public string GetReadDTFloatCmd(string strType, string strStartCode)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";

            string dType = "D";
            string cmdType = "RD";

            string strEndCode = "";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCmd += strAddr;
            }
            //////
            int nEndCode = Convert.ToInt16(strStartCode) + 1;

            strEndCode = Convert.ToString(nEndCode);
            nCodeLenth = strEndCode.Length;
            if (5 >= nCodeLenth)
            {
                strEndAddr = strEndCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strEndAddr = "0" + strEndAddr;
                }

                strReadCmd += strEndAddr;
            }

            strReadCmd = strPre + cmdType + dType + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        public string GetReadDTMultiFloatCmd(string strType, string strStartCode,int nReadNum)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";

            string dType = "D";
            string cmdType = "RD";

             

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCmd += strAddr;
            }
            //////
            int num = 2 * nReadNum - 1;
            
           
            int nEndCode = Convert.ToInt16(strStartCode) + num;

            string strEndCode = Convert.ToString(nEndCode);
            nCodeLenth = strEndCode.Length;
            if (5 >= nCodeLenth)
            {
                strEndAddr = strEndCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strEndAddr = "0" + strEndAddr;
                }

                strReadCmd += strEndAddr;
            }
            
            
            strReadCmd = strPre + cmdType + dType + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }
        /// <summary>
        /// 写入一个D地址（寄存器）值
        /// </summary>
        /// <param name="strType">点位D</param>
        /// <param name="strStartCode">地址</param>
        /// <param name="strStartData">内容必须有4位，并高低位相反</param>
        /// <returns></returns>
        public string GetWriteDTShortCmd(string strType, string strStartCode, string strStartData)
        {
            byte[] Buffer;
            string strReadCode = "";
           
            string strReadCmd = "";
            string strReadData = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            
           

            string dType = "D";
            string cmdType = "WD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCode += strAddr;
            }

            
            strReadData = ShortTOstr(Convert.ToInt16(strStartData));
            
            strReadCmd = strPre + cmdType + dType + strReadCode + strReadCode + strReadData + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        public string GetWriteDTFloatCmd(string strType, string strStartCode, string strStartData)
        {
            byte[] Buffer;
            string strReadCode = "";
            string strReadEndCode = "";
            string strReadCmd = "";
            string strReadData = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";
            String strData = "";

            string dType = "D";
            string cmdType = "WD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCode += strAddr;
            }

            ////
            int nEndCode = Convert.ToInt16(strStartCode) + 1;

            string strEndCode = Convert.ToString(nEndCode);

            nCodeLenth = strEndCode.Length;
            if (5 >= nCodeLenth)
            {
                strEndAddr = strEndCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strEndAddr = "0" + strEndAddr;
                }

                strReadEndCode += strEndAddr;
            }


            strReadData = FloatTOstr(Convert.ToSingle(strStartData));
            //strReadData = "D2040000";

            strReadCmd = strPre + cmdType + dType + strReadCode + strReadEndCode + strReadData + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        public string GetWriteDTMultiFloatCmd(string strType, string strStartCode, float[] fWriteData)
        {
            byte[] Buffer;
            string strReadCode = "";
            
            string strReadCmd = "";
            string strWriteData = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";
            

            string dType = "D";
            string cmdType = "WD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCode += strAddr;
            }

            ////
            int nWriteLen = fWriteData.Length;

            int nWriteNum = 2 * nWriteLen - 1;

           
                int nEndCode = Convert.ToInt16(strStartCode) + nWriteNum;

                string strEndCode = Convert.ToString(nEndCode);

                nCodeLenth = strEndCode.Length;
                if (5 >= nCodeLenth)
                {
                    strEndAddr = strEndCode;
                    for (int j = 0; j < (5 - nCodeLenth); j++)
                    {
                        strEndAddr = "0" + strEndAddr;
                    }

                    strReadCode += strEndAddr;
                }
            

            for(int i=0;i<nWriteLen;i++)
            {
                strWriteData += FloatTOstr(fWriteData[i]);
            }
                      
            strReadCmd = strPre + cmdType + dType + strReadCode  + strWriteData + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        public string GetWriteDTMultiShortCmd(string strType, string strStartCode, short[] sWriteData)
        {
            byte[] Buffer;
            string strReadCode = "";

            string strReadCmd = "";
            string strWriteData = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";


            string dType = "D";
            string cmdType = "WD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCode += strAddr;
            }

            ////
            int nWriteLen = sWriteData.Length;

            int nWriteNum = 2 * nWriteLen - 1;


            int nEndCode = Convert.ToInt16(strStartCode) + nWriteNum;

            string strEndCode = Convert.ToString(nEndCode);

            nCodeLenth = strEndCode.Length;
            if (5 >= nCodeLenth)
            {
                strEndAddr = strEndCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strEndAddr = "0" + strEndAddr;
                }

                strReadCode += strEndAddr;
            }


            for (int i = 0; i < nWriteLen; i++)
            {
                strWriteData += ShortTOstr(sWriteData[i]);
            }
        

            strReadCmd = strPre + cmdType + dType + strReadCode  + strWriteData + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }


        public string GetReadCrowdCmd(string strType, string strStartCode, string strEndCode)
        {
            byte[] Buffer;
            string strReadCmd = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";


            string cmdType = "RCC";
            int nCodeLenth = strStartCode.Length;
            if (4 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (4 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCmd += strAddr;
            }
            nCodeLenth = strEndCode.Length;
            if (4 >= nCodeLenth)
            {
                strAddr = strEndCode;
                for (int j = 0; j < (4 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCmd += strAddr;
            }
            strReadCmd = strPre + cmdType + strType + strReadCmd + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        private bool FindPlcPoint(String strType, String strCode, out STPlcPoint stPlcPoint)
        {
            stPlcPoint = null;

            if (null != m_PlcSettingsInfo)
            {
                if (null != m_PlcSettingsInfo.stPointList)
                {
                    int nCnt = m_PlcSettingsInfo.stPointList.Count;
                    for (int i = 0; i < nCnt; i++)
                    {
                        if (null == m_PlcSettingsInfo.stPointList[i])
                        {
                            continue;
                        }

                        if (m_PlcSettingsInfo.stPointList[i].strAddrType.Equals(strType)
                         && m_PlcSettingsInfo.stPointList[i].strAddrCode.Equals(strCode))
                        {
                            stPlcPoint = m_PlcSettingsInfo.stPointList[i];
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        private bool OpenAllSerailPort(string comPort)
        {
            //SerialPortService m_SerialPortParamService = new SerialPortService();
            //m_SerialPortModel = m_SerialPortParamService.QueryAll().FirstOrDefault(x => x.Name == comPort);
            //m_SerialPortModel = m_SerialPortParamService.QueryAll().FirstOrDefault(x => x.ComPortName == comPort);
            //m_SerialPort = new SerialPort(m_SerialPortModel.ComPortName, m_SerialPortModel.BaudRate, m_SerialPortModel.Parity, m_SerialPortModel.DataBits, m_SerialPortModel.StopBits);

            m_SerialPortModel = XMLController.XmlControl.sequenceModelNew.ComModels.FirstOrDefault(x => x.Name == comPort);

            m_SerialPortControl.SetFormatRec(true);
            if(!m_SerialPortControl.Init(m_SerialPortModel))
            {
                return false;
            }
            //m_SerialPort = SerialPortControl.GetSerialPort();

            return true;
        }

        private bool ReadSinglePlcPoint(STPlcPoint stPoint, out UInt32 nData)
        {
            m_bReadPortExcaption = false;
            nData = 0;
            if (null == stPoint)
            {
                return false;
            }


            String strReadCmd = null;
            if (!GenReadCmd(stPoint, out strReadCmd))
            {
                return false;
            }

            if (null == strReadCmd)
            {
                return false;
            }

       
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
               
                m_SerialPortModel.SendContent = strReadCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bReadPortExcaption = true;
                m_PortMutex.ReleaseMutex();
            }

            String strClearedString = "";
            ClearString(strReceived, out strClearedString);

            bool bError = false;
            if (!ParserReceiveBit(strClearedString, out bError, out nData))
            {
                return false;
            }

            if (bError)
            {
                return false;
            }

            return true;
        }
        private bool WriteSinglePlcPoint(STPlcPoint stPoint, UInt32 nData)
        {
            m_bWritePortExcaption = false;
            if (null == stPoint)
            {
                return false;
            }

            String strWriteCmd = null;
            if (!GenWriteCmd(stPoint, nData, out strWriteCmd))
            {
                return false;
            }

            if (null == strWriteCmd)
            {
                return false;
            }


            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();
            }

            String strClearedString = "";
            ClearString(strReceived, out strClearedString);

            bool bError = false;
            if (!ParserReceiveBit(strClearedString, out bError, out nData))
            {
                return false;
            }

            if (bError)
            {
                return false;
            }

            return true;
        }
        private bool WriteSinglePlcPoint(STPlcPoint stPoint, string nData)
        {
            m_bWritePortExcaption = false;
            if (null == stPoint)
            {
                return false;
            }

            String strWriteCmd = null;
            if (!GenWriteCmd(stPoint, nData, out strWriteCmd))
            {
                return false;
            }

            if (null == strWriteCmd)
            {
                return false;
            }

           
            String strReceived = "";
            try
            {
                m_PortMutex.WaitOne();
                
                m_SerialPortModel.SendContent = strWriteCmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSend);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                Thread.Sleep(20);

                resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortReceive);
                if (!resultModel.RunResult)
                {
                    m_PortMutex.ReleaseMutex();
                    return false;
                }

                strReceived = resultModel.ObjectResult.ToString();
                m_PortMutex.ReleaseMutex();
            }
            catch
            {
                m_bWritePortExcaption = true;
                m_PortMutex.ReleaseMutex();
            }

            String strClearedString = "";
            ClearString(strReceived, out strClearedString);

            bool bError = false;
            UInt32 outData;
            if (!ParserReceiveBit(strClearedString, out bError, out outData))
            {
                return false;
            }

            if (bError)
            {
                return false;
            }

            return true;
        }
        //private SerialPort GetSerialPort(EMPlcSerialPortType emSerialPortType)
        //{
        //    SerialPort stSerialPort = null;
        //    switch (emSerialPortType)
        //    {

        //        case EMPlcSerialPortType.emDefalut:
        //        default:
        //            //stSerialPort = m_SerialPort;
        //            break;
        //    }

        //    return stSerialPort;
        //}
        private bool GenWriteCmd(STPlcPoint stPoint, UInt32 nData, out String strWriteCmd)
        {
            strWriteCmd = null;
            if (null == stPoint)
            {
                return false;
            }

            bool bOk = false;
            String strPre = "%01#";
            String strTail = "**\r";
            switch (stPoint.emPointType)
            {
                case EMPointType.emBit:
                    {
                        String strAddr = "";
                        if (4 >= stPoint.strAddrCode.Length)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (4 - stPoint.strAddrCode.Length); i++)
                            {
                                strAddr = "0" + strAddr;
                            }
                            if (0 == nData)
                            {
                                strWriteCmd = strPre + "WCS" + stPoint.strAddrType + strAddr + "0" + strTail;
                            }
                            else
                            {
                                strWriteCmd = strPre + "WCS" + stPoint.strAddrType + strAddr + "1" + strTail;
                            }

                            bOk = true;
                        }
                    }
                    break;

                case EMPointType.emDTBit:
                    {
                        String strAddr = "";
                        if (4 >= stPoint.strAddrCode.Length)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (5 - stPoint.strAddrCode.Length); i++)
                            {
                                strAddr = "0" + strAddr;
                            }
                            if (0 == nData)
                            {
                                strWriteCmd = strPre + "WD" + stPoint.strAddrType + strAddr + "0" + strTail;
                            }
                            else
                            {
                                strWriteCmd = strPre + "WD" + stPoint.strAddrType + strAddr + "1" + strTail;
                            }

                            bOk = true;
                        }
                    }
                    break;

                case EMPointType.emDoubleWord:
                    {
                        String strBeginAddr = "";
                        if (5 >= stPoint.strAddrCode.Length)
                        {
                            strBeginAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (5 - stPoint.strAddrCode.Length); i++)
                            {
                                strBeginAddr = "0" + strBeginAddr;
                            }

                            UInt32 uNewData = ((UInt32)((nData & 0xFF) << 8)) + ((UInt32)((nData & 0xFF00) >> 8));
                            String strNewData = Convert.ToString(uNewData, 16);
                            for (int i = 0; i < (4 - stPoint.strAddrCode.Length); i++)
                            {
                                strNewData = "0" + strNewData;
                            }

                            strWriteCmd = strPre + "WD" + "D" + strBeginAddr + strBeginAddr + strNewData + strTail;
                            bOk = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            return bOk;
        }
        private bool GenWriteCmd(STPlcPoint stPoint, string nData, out String strWriteCmd)
        {
            strWriteCmd = null;
            if (null == stPoint)
            {
                return false;
            }

            bool bOk = false;
            String strPre = "%01#";
            String strTail = "**\r";
            switch (stPoint.emPointType)
            {
                case EMPointType.emBit:
                    {
                        String strAddr = "";
                        if (4 >= stPoint.strAddrCode.Length)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (4 - stPoint.strAddrCode.Length); i++)
                            {
                                strAddr = "0" + strAddr;
                            }
                            if ("0" == nData)
                            {
                                strWriteCmd = strPre + "WCS" + stPoint.strAddrType + strAddr + "0" + strTail;
                            }
                            else
                            {
                                strWriteCmd = strPre + "WCS" + stPoint.strAddrType + strAddr + "1" + strTail;
                            }

                            bOk = true;
                        }
                    }
                    break;

                case EMPointType.emDTBit:
                    {
                        String strAddr = "";
                        if (4 >= stPoint.strAddrCode.Length)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (5 - stPoint.strAddrCode.Length); i++)
                            {
                                strAddr = "0" + strAddr;
                            }
                            strWriteCmd = strPre + "WD" + stPoint.strAddrType + strAddr + nData + strTail;

                            bOk = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            return bOk;
        }

        private bool GenReadCmd(STPlcPoint stPoint, out String strReadCmd)
        {
            strReadCmd = null;
            if (null == stPoint)
            {
                return false;
            }

            bool bOk = false;
            String strPre = "%01#";
            String strTail = "**\r";
            switch (stPoint.emPointType)
            {
                case EMPointType.emBit:
                    {
                        String strAddr = "";
                        int nCodeLenth = stPoint.strAddrCode.Length;
                        if (4 >= nCodeLenth)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (4 - nCodeLenth); i++)
                            {
                                strAddr = "0" + strAddr;
                            }

                            strReadCmd = strPre + "RCS" + stPoint.strAddrType + strAddr + strTail;
                            bOk = true;
                        }
                    }
                    break;

                case EMPointType.emDTBit:
                    {
                        String strAddr = "";
                        int nCodeLenth = stPoint.strAddrCode.Length;
                        if (4 >= nCodeLenth)
                        {
                            strAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (4 - nCodeLenth); i++)
                            {
                                strAddr = "0" + strAddr;
                            }

                            strReadCmd = strPre + "RD" + stPoint.strAddrType + strAddr + strTail;
                            bOk = true;
                        }
                    }
                    break;

                case EMPointType.emDoubleWord:
                    {
                        String strBeginAddr = "";
                        if (5 >= stPoint.strAddrCode.Length)
                        {
                            strBeginAddr = stPoint.strAddrCode;
                            for (int i = 0; i < (5 - stPoint.strAddrCode.Length); i++)
                            {
                                strBeginAddr = "0" + strBeginAddr;
                            }

                            strReadCmd = strPre + "RD" + stPoint.strAddrType + strBeginAddr + strBeginAddr + strTail;

                            bOk = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            return bOk;
        }

        private bool SwitchStringsToByte(String strCmd, out Byte[] Buffer, out int nLen)
        {
            Buffer = null;
            nLen = 0;
            if (null == strCmd)
            {
                return false;
            }

            nLen = strCmd.Length;
            Buffer = new Byte[nLen];
            for (int i = 0; i < nLen; i++)
            {
                Buffer[i] = (Byte)strCmd[i];
            }

            BccCommand(ref Buffer, nLen - 3);

            return true;
        }
        private void ClearString(String strReceive, out String strClearedString)
        {
            strClearedString = "";
            if (null == strReceive)
            {
                return;
            }

            int nCnt = strReceive.Length;
            for (int i = 0; i < nCnt; i++)
            {
                if (strReceive[i].Equals('?'))
                {
                    continue;
                }

                strClearedString += strReceive[i];
            }

            return;
        }
        public bool ParserReceiveBit(String strReadCmd, out bool bError, out UInt32 uData)
        {
            bError = false;
            uData = 0;

            if (null == strReadCmd)
            {
                return false;
            }

            int nLength = strReadCmd.Length;
            if (6 >= nLength)
            {
                return false;
            }

            if (strReadCmd[3].Equals('!'))
            {
                bError = true;
                return false;
            }

            if (!strReadCmd[3].Equals('$'))
            {
                return false;
            }

            if (strReadCmd[4].Equals('R') && (strReadCmd[5].Equals('C')))
            {
                if (7 >= nLength)
                {
                    return false;
                }

                /*
                 * 单点返回。
                */
                if (strReadCmd[6].Equals('0'))
                {
                    uData = 0;
                }
                else
                {
                    uData = 1;
                }
            }
            //else if ((strReadCmd[4].Equals('R')) && (strReadCmd[5].Equals('D')))
            //{
            //    if (10 >= nLength)
            //    {
            //        return false;
            //    }

            //    /*
            //     * 数据返回。
            //    */
            //    try
            //    {
            //        String strData = strReadCmd[8].ToString() + strReadCmd[9].ToString() + strReadCmd[6].ToString() + strReadCmd[7].ToString();
            //        uData = UInt32.Parse(strData, NumberStyles.HexNumber);
            //    }
            //    catch
            //    {
            //        uData = 0;
            //    }
            //}
            else
            {
                uData = 0;
            }

            return true;
        }

        public bool ParserReceiveFloat(String strReadCmd, out bool bError, out float fData)
        {
            bError = false;
            fData = 0;

            if (null == strReadCmd)
            {
                return false;
            }

            int nLength = strReadCmd.Length;
            if (6 >= nLength)
            {
                return false;
            }

            if (strReadCmd[3].Equals('!'))
            {
                bError = true;
                return false;
            }

            if (!strReadCmd[3].Equals('$'))
            {
                return false;
            }

            else if ((strReadCmd[4].Equals('R')) && (strReadCmd[5].Equals('D')))
            {
                //if (10 >= nLength)
                //{
                //    return false;
                //}

                try
                {

                    byte[] value = new byte[]
                    {
                      Convert.ToByte("0x" + strReadCmd[6]  + strReadCmd[7],  16),
                      Convert.ToByte("0x" + strReadCmd[8]  + strReadCmd[9],  16),
                      Convert.ToByte("0x" + strReadCmd[10] + strReadCmd[11], 16),
                      Convert.ToByte("0x" + strReadCmd[12] + strReadCmd[13], 16)
                    };

                    fData = BitConverter.ToSingle(value, 0);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool ParserReceiveMultiFloat(String strReadCmd, out bool bError, out float fXData,out float fYData)
        {
            bError = false;
            fXData = 0;
            fYData = 0;

            if (null == strReadCmd)
            {
                return false;
            }

            int nLength = strReadCmd.Length;
            if (6 >= nLength)
            {
                return false;
            }

            if (strReadCmd[3].Equals('!'))
            {
                bError = true;
                return false;
            }

            if (!strReadCmd[3].Equals('$'))
            {
                return false;
            }

            else if ((strReadCmd[4].Equals('R')) && (strReadCmd[5].Equals('D')))
            {
                
                try
                {

                    byte[] valueX = new byte[]
                    {
                      Convert.ToByte("0x" + strReadCmd[6]  + strReadCmd[7],  16),
                      Convert.ToByte("0x" + strReadCmd[8]  + strReadCmd[9],  16),
                      Convert.ToByte("0x" + strReadCmd[10] + strReadCmd[11], 16),
                      Convert.ToByte("0x" + strReadCmd[12] + strReadCmd[13], 16)
                    };

                    fXData = BitConverter.ToSingle(valueX, 0);

                    byte[] valueY = new byte[]
                    {
                      Convert.ToByte("0x" + strReadCmd[14]  + strReadCmd[15],  16),
                      Convert.ToByte("0x" + strReadCmd[16]  + strReadCmd[17],  16),
                      Convert.ToByte("0x" + strReadCmd[18]  + strReadCmd[19],   16),
                      Convert.ToByte("0x" + strReadCmd[20]  + strReadCmd[21],   16)
                    };

                    fXData = BitConverter.ToSingle(valueY, 0);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool ParserReceiveShort(String strReadCmd, out bool bError, out short sData)
        {
            bError = false;
            sData = 0;

            if (null == strReadCmd)
            {
                return false;
            }

            int nLength = strReadCmd.Length;
            if (6 >= nLength)
            {
                return false;
            }

            if (strReadCmd[3].Equals('!'))
            {
                bError = true;
                return false;
            }

            if (!strReadCmd[3].Equals('$'))
            {
                return false;
            }

            else if ((strReadCmd[4].Equals('R')) && (strReadCmd[5].Equals('D')))
            {
                //if (10 >= nLength)
                //{
                //    return false;
                //}

                try
                {

                    byte[] value = new byte[]
                    {
                      Convert.ToByte("0x" + strReadCmd[6] + strReadCmd[7], 16),
                      Convert.ToByte("0x" + strReadCmd[8] + strReadCmd[9], 16),
                    };

                    sData =  BitConverter.ToInt16(value, 0);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private void BccCommand(ref Byte[] pCommand, int nDataLen)
        {
            UInt16 bcc = 0;

            for (int i = 0; i < nDataLen; i++)
            {
                bcc ^= pCommand[i];
            }

            String strBcc = Convert.ToString(bcc, 16);
            strBcc = strBcc.ToUpper();

            if (1 >= strBcc.Length)
            {
                pCommand[nDataLen++] = (Byte)'*';
                pCommand[nDataLen] = (Byte)'*';
            }
            else if (2 >= strBcc.Length)
            {
                pCommand[nDataLen++] = (Byte)strBcc[0];
                pCommand[nDataLen] = (Byte)strBcc[1];
            }
        }

        ///<summary >浮点转换字符串</summary>
        private string FloatTOstr(float floats)
        {
            if (floats == 0f)
            {
                return "00000000";//00 00 00 00
            }
            byte[] bytes = BitConverter.GetBytes(floats);
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]   /*{0:X} {1:X} {2:X} {3:X}*/
			{
                bytes[0],
                bytes[1],
                bytes[2],
                bytes[3]
            });
        }
        ///<summary >short转换字符串</summary>
        private string ShortTOstr(short shorts)
        {
            if (shorts == 0)
            {
                return "0000";//00 00
            }
            byte[] bytes = BitConverter.GetBytes(shorts);
            return string.Format("{0:X2}{1:X2}", bytes[0], bytes[1]); //{0:X} {1:X}
        }
        public bool ExeGoto(float fXPos,float fYPos,int nSpeed)
        {
            if(!WriteDTFloat(m_PlcParam.dMoveXPosition.ToString(), fXPos))
            {
                return false;
            }

            if (!WriteDTFloat(m_PlcParam.dMoveYPosition.ToString(), fYPos))
            {
                return false;
            }

            //if (!WriteDTFloat(m_PlcParam.dSpeed.ToString(), nSpeed))
            //{
            //    return false;
            //}

            if(!WriteRBit(m_PlcParam.rAbsMove.ToString(), true))
            {
                return false;
            }


            long dwOldTime = DateTime.Now.Ticks / 10000;
            long dwCurTime = dwOldTime;

            bool bError = false;

            while (true)
            {
                dwCurTime = DateTime.Now.Ticks /10000;
                if (dwCurTime - dwOldTime > 5000)
                {
                    bError = true;
                    break;
                }

                Thread.Sleep(20);

                UInt32 nOut = 0;

                if( !ReadRBit(m_PlcParam.rAbsMoveFinish.ToString(), out nOut) )
                {
                    bError = true;
                    break;
                }

                if(1 == nOut)
                {
                    break;
                }


            }

            if (!WriteRBit(m_PlcParam.rAbsMove.ToString(), false))
            {
                return false;
            }

            if(bError)
            {
                return false;
            }

            return true;
        }

        public bool GetCurPos(out float fXPos,out float fYPos)
        {
            fXPos = 0;
            fYPos = 0;


            //if( !ReadDTFloat(m_PlcParam.dXCurPosition.ToString(),out fXPos) )
            //{
            //    return false;
            //}

            //if ( !ReadDTFloat(m_PlcParam.dYCurPosition.ToString(), out fYPos) )
            //{
            //    return false;
            //}

            if (!ReadDTFloatXY(m_PlcParam.dXCurPosition.ToString(), out fXPos,out fYPos))
            {
                return false;
            }

            return true;
        }

        public bool Step(float fXStep, float fYStep,int nSpeed)
        {
            float fXPos = 0;
            float fYPos = 0;

            if( !GetCurPos(out fXPos,out fYPos) )
            {
                return false;
            }

            fXPos += fXStep;
            fYPos += fYStep;

            if( !ExeGoto( fXPos,fYPos,nSpeed) )
            {
                return false;
            }
            
            return true;
        }
        
        public string GetWriteDTCmd(string strType, string strStartCode, string strStartData)  //strType:1."D" 2."I"
        {
            byte[] Buffer;
            string strReadCode = "";
            string strReadEndCode = "";
            string strReadCmd = "";
            string strReadData = "";
            int nLen;

            String strPre = "%01#";
            String strTail = "**\r";

            String strAddr = "";
            String strEndAddr = "";
            String strData = "";

            string dType = "D";
            string cmdType = "WD";

            int nCodeLenth = strStartCode.Length;
            if (5 >= nCodeLenth)
            {
                strAddr = strStartCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strAddr = "0" + strAddr;
                }

                strReadCode += strAddr;
            }

            ////
            int nEndCode = Convert.ToInt16(strStartCode) + 1;

            string strEndCode = Convert.ToString(nEndCode);

            nCodeLenth = strEndCode.Length;
            if (5 >= nCodeLenth)
            {
                strEndAddr = strEndCode;
                for (int j = 0; j < (5 - nCodeLenth); j++)
                {
                    strEndAddr = "0" + strEndAddr;
                }

                strReadEndCode += strEndAddr;
            }

            if ("D" == strType)
            {
                strReadData = FloatTOstr(Convert.ToSingle(strStartData));
            }
            else if ("I" == strType)
            {
                strReadData = IntTOstr(Convert.ToInt32(strStartData));
            }
            else
            {
                return "";
            }


            strReadCmd = strPre + cmdType + dType + strReadCode + strReadEndCode + strReadData + strTail;

            SwitchStringsToByte(strReadCmd, out Buffer, out nLen);
            strReadCmd = Encoding.ASCII.GetString(Buffer);

            return strReadCmd;
        }

        ///<summary >int转换字符串</summary>
        private string IntTOstr(int int_0)
        {
            if (int_0 == 0)
            {
                return "00000000";//00 00 00 00
            }
            byte[] bytes = BitConverter.GetBytes(int_0);
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]  /*{0:X} {1:X} {2:X} {3:X}*/
            {
                bytes[0],
                bytes[1],
                bytes[2],
                bytes[3]
            });
        }

        public bool ParserReceiveInt(String strReadCmd, out bool bError, out Int32 iData)
        {
            bError = false;
            iData = 0;

            if (null == strReadCmd)
            {
                return false;
            }

            int nLength = strReadCmd.Length;
            if (6 >= nLength)
            {
                return false;
            }

            if (strReadCmd[3].Equals('!'))
            {
                bError = true;
                return false;
            }

            if (!strReadCmd[3].Equals('$'))
            {
                return false;
            }

            else if ((strReadCmd[4].Equals('R')) && (strReadCmd[5].Equals('D')))
            {

                try
                {

                    byte[] value = new byte[]
                    {
                      Convert.ToByte("0x" + strReadCmd[6]  + strReadCmd[7],  16),
                      Convert.ToByte("0x" + strReadCmd[8]  + strReadCmd[9],  16),
                      Convert.ToByte("0x" + strReadCmd[10] + strReadCmd[11], 16),
                      Convert.ToByte("0x" + strReadCmd[12] + strReadCmd[13], 16)
                    };

                    iData = BitConverter.ToInt32(value, 0);
                }
                catch
                {
                    return false;
                }
            }
            else if ((strReadCmd[4].Equals('W')) && (strReadCmd[5].Equals('D')))
            {
                iData = Convert.ToInt16(strReadCmd.Substring(6, 2));
                return true;
            }
            else
            {
                return false;
            }

            return true;
        }


        public bool WriteDTInt(string strStartCode, int sStartData)
        {
            try
            {
                m_PortMutex.WaitOne();
                string cmd = GetWriteDTCmd("I", strStartCode, sStartData.ToString());
                m_SerialPortModel.SendContent = cmd;
                var resultModel = m_SerialPortControl.Run(m_SerialPortModel, BaseController.ControlType.SerialPortSendAndReceive) as SerialPortResultModel;

                bool bError = false;
                Int32 value = 0;
                bool bRef = ParserReceiveInt(resultModel.ReceiveMessage.ToString(), out bError, out value);

                m_PortMutex.ReleaseMutex();
                return bRef;
            }
            catch (Exception ex)
            { 
                m_PortMutex.ReleaseMutex();

                return false;
            }
        }

    }
}
