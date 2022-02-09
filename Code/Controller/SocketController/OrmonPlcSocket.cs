using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Net;
using System.Net.Sockets;
 
namespace SocketController
{
    /// <summary>
    /// 欧姆龙PLC网口通讯
    /// </summary>
    public class OrmonPlcSocket
    {
        public bool connected;
        string ip;
        int port;
        byte[] Client = new byte[4];
        byte[] Server = new byte[4];//PLC和本机数据
        byte[] dataLeng = { 0x00, 0x01 };
        TcpClient msender;
        Socket msock;

        #region 构造函数
        /// <summary>
        /// Fins协议Tcp连接的构造函数（PLC的IP地址、PLC端口号、本机节点）
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号，默认9600</param>
        /// <param name="fra">本机节点</param>
        /// <returns>实例化一个Fins协议Tcp连接</returns>
        public OrmonPlcSocket(string ip, int port, byte fra)
        {
            //frame = Convert.ToByte(ip.Substring(ip.LastIndexOf('.')));
            this.ip = ip;
            this.port = port;
            Client[3] = fra;
            connected = false;
        }
        #endregion

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public void disconn()
        {
            if (connected)
            {
                msender.Close();
                connected = false;
            }
        }
        #endregion


        #region 连接PLC
        /// <summary>
        /// 连接PLC
        /// </summary>
        public void conn()
        {
            if (!connected)
            {
                msender = new TcpClient(ip, port);
                msock = msender.Client;

                byte[] Handshake = new byte[20];
                Handshake[0] = 0x46;//F
                Handshake[1] = 0x49;//I
                Handshake[2] = 0x4e;//N
                Handshake[3] = 0x53;//S

                Handshake[4] = 0;
                Handshake[5] = 0;
                Handshake[6] = 0;
                Handshake[7] = 0x0c;//Length长度

                Handshake[8] = 0;
                Handshake[9] = 0;
                Handshake[10] = 0;
                Handshake[11] = 0;//Command

                Handshake[12] = 0;
                Handshake[13] = 0;
                Handshake[14] = 0;
                Handshake[15] = 0;//Error Code

                Handshake[16] = 0;
                Handshake[17] = 0;
                Handshake[18] = 0;
                Handshake[19] = Client[3];//FINS Frame (本机节点)

                msock.Send(Handshake);
                byte[] buffer = new byte[24];
                msock.Receive(buffer, SocketFlags.None);
                byte[] buf = buffer;
                buf[0] = 0x46;//F
                buf[1] = 0x49;//I
                buf[2] = 0x4e;//N
                buf[3] = 0x53;//S
                buf[4] = 0;
                buf[5] = 0;
                buf[6] = 0;
                buf[7] = 20;
                buf[8] = 0;
                buf[9] = 0;
                buf[10] = 0;
                buf[11] = 1;
                buf[12] = 0;
                buf[13] = 0;
                buf[14] = 0;
                buf[15] = 0;
                if (buf == buffer)
                {
                    Client[0] = buffer[16];
                    Client[1] = buffer[17];
                    Client[2] = buffer[18];
                    Client[3] = buffer[19];

                    Server[0] = buffer[20];
                    Server[1] = buffer[21];
                    Server[2] = buffer[22];
                    Server[3] = buffer[23];
                    connected = true;
                }
            }
        }
        #endregion


        #region 读位数据
        /// <summary>
        /// 读取PLC上的位数据（内存地址、读取结果）
        /// </summary>
        /// <param name="memory">PLC的内存地址(区域.地址.位 如：CIO.1.2)</param>
        /// <param name="result">读取结果</param>
        public int readBit(string memory, ref bool result)
        {
            byte[] re = new byte[1] { 0x00 };
            int rev = SendByte(memory, true, true, ref re);
            if (rev == 0) result = Convert.ToBoolean(re[0]);
            return rev;

        }
        #endregion


        #region 写位数据
        /// <summary>
        /// 写入PLC上的位数据（内存地址、写入数据）
        /// </summary>
        /// <param name="memory">PLC的内存地址(区域.地址.位 如：WR.3.0)</param>
        /// <param name="sendBit">写入数据</param>
        public int writeBit(string memory, bool sendBit)
        {
            Byte[] Se = new Byte[1];
            Se[0] = Convert.ToByte(sendBit);
            return SendByte(memory, true, false, ref Se, Se);
        }
        #endregion


        #region 读字数据
        /// <summary>
        /// 读取PLC上的位数据（内存地址、读取结果）
        /// </summary>
        /// <param name="memory">PLC的内存地址(区域.地址.字长度 如：DR.1.2)</param>
        /// <param name="result">读取结果</param>
        public int readWord(string memory, ref byte[] result)
        {
            return SendByte(memory, false, true, ref result);
        }
        #endregion


        #region 写字数据
        /// <summary>
        /// 写入PLC上的位数据（内存地址、写入数据）
        /// </summary>
        /// <param name="memery">PLC的内存地址(区域.地址.字长度 如：WR.10.4)</param>
        /// <param name="sendWord">写入数据</param>
        public int writeWord(string memory, byte[] sendWord)
        {
            return SendByte(memory, false, false, ref sendWord, sendWord);
        }
        #endregion


        #region PLC内存格式转换

        /// <summary>
        /// 地址格式转化
        /// </summary>
        private byte[] addsToByte(string memory, bool isBit)
        {
            string[] AddrParts = memory.Split('.');
            byte[] CH = BitConverter.GetBytes(Convert.ToInt32(AddrParts[1]));
            byte[] Count = BitConverter.GetBytes(Convert.ToInt32(AddrParts[2]));
            byte[] Addrs = new byte[6];
            Addrs[1] = CH[1];
            Addrs[2] = CH[0];

            if (!isBit)   //字处理            
            {
                switch (AddrParts[0])
                {
                    case "CIO":
                        Addrs[0] = 0xB0;
                        break;
                    case "WR":
                        Addrs[0] = 0xB1;
                        break;
                    case "DM":
                        Addrs[0] = 0x82;
                        break;
                    case "HR":
                        Addrs[0] = 0xB2;
                        break;
                    case "TIM":
                        Addrs[0] = 0x89;
                        break;
                    case "AR":
                        Addrs[0] = 0xB3;
                        break;
                    case "CNT":
                        Addrs[0] = 0x89;
                        break;
                    default:
                        Addrs[0] = 0x00;
                        break;
                }
                Addrs[3] = 0x00;
                Addrs[4] = Count[1];
                Addrs[5] = Count[0];//读写字的长度
            }
            else //位处理            
            {
                switch (AddrParts[0])
                {
                    case "CIO":
                        Addrs[0] = 0x30;
                        break;
                    case "WR":
                        Addrs[0] = 0x31;
                        break;
                    case "DM":
                        Addrs[0] = 0x02;
                        break;
                    case "HR":
                        Addrs[0] = 0x32;
                        break;
                    case "TIM":
                        Addrs[0] = 0x09;
                        break;
                    case "AR":
                        Addrs[0] = 0x33;
                        break;
                    case "CNT":
                        Addrs[0] = 0x09;
                        break;
                    default:
                        Addrs[0] = 0x00;
                        break;
                }
                Addrs[3] = Count[0];
                Addrs[4] = 0x00;
                Addrs[5] = 0x01;//每次读写一位
            }


            return Addrs;
        }
        #endregion


        #region 报文处理
        /// <summary>
        /// 报文处理
        /// </summary>
        /// <param name="memory">要读写的内存地址</param>
        /// <param name="isBit">位还是字</param>
        /// <param name="isRead">读还是写</param>
        /// <param name="rev">返回的数据</param>
        /// <param name="datas">写入的数据</param>
        private int SendByte(string memory, bool isBit, bool isRead, ref byte[] rev, byte[] datas = null)
        {
            int dataLength;
            if (datas == null)
                dataLength = 26;
            else
                dataLength = datas.Length + 26;
            byte[] SendByte = new byte[dataLength + 8];
            SendByte[0] = 0x46;//F
            SendByte[1] = 0x49;//I
            SendByte[2] = 0x4e;//N
            SendByte[3] = 0x53;//S
            SendByte[4] = 0;//cmd length
            SendByte[5] = 0;
            SendByte[6] = 0;
            SendByte[7] = Convert.ToByte(dataLength);
            SendByte[8] = 0;//frame command
            SendByte[9] = 0;
            SendByte[10] = 0;
            SendByte[11] = 0x02;
            SendByte[12] = 0;//err
            SendByte[13] = 0;
            SendByte[14] = 0;
            SendByte[15] = 0;
            //command frame header
            SendByte[16] = 0x80;//ICF
            SendByte[17] = 0x00;//RSV
            SendByte[18] = 0x02;//GCT, less than 8 network layers
            SendByte[19] = 0x00;//DNA, local network
            SendByte[20] = Server[3];//DA1
            SendByte[21] = 0x00;//DA2, CPU unit
            SendByte[22] = 0x00;//SNA, local network
            SendByte[23] = Client[3];//SA1
            SendByte[24] = 0x00;//SA2, CPU unit
            SendByte[25] = Convert.ToByte(21);//SID

            SendByte[26] = 0x01;
            if (isRead)
                SendByte[27] = 0x01;
            else
                SendByte[27] = 0x02;

            byte[] head = addsToByte(memory, isBit);
            SendByte[28] = head[0];
            SendByte[29] = head[1];
            SendByte[30] = head[2];
            SendByte[31] = head[3];
            SendByte[32] = head[4];
            SendByte[33] = head[5];

            if (datas != null)
            {
                Array.Copy(datas, 0, SendByte, 34, datas.Length);
            }

            msock.Send(SendByte, SocketFlags.None);
            byte[] buffer = new byte[256];
            msock.Receive(buffer);

            int err = 0;
            if (buffer[0] != SendByte[0] || buffer[1] != SendByte[1] || buffer[2] != SendByte[2] || buffer[3] != SendByte[3])
            {
                err = 1;//the head is err
            }

            if (err == 0 && buffer[11] == 3)
            {
                switch (buffer[15])
                {
                    case 0x01: err = 2; break;//the head is not 'FINS'
                    case 0x02: err = 3; break;//the data length is too long 
                    case 0x03: err = 4; break;//the command is not supported
                }
            }

            if (err == 0 && buffer[28] != 0 && buffer[29] != 0)
            {
                err = 5; //end code err

                switch (buffer[28])
                {
                    case 0x00:
                        if (buffer[29] == 0x01) err = 6;//service canceled
                        break;
                    case 0x01:
                        switch (buffer[29])
                        {
                            case 0x01: err = 7; break; //local node not in network
                            case 0x02: err = 8; break; //token timeout
                            case 0x03: err = 9; break; //retries failed
                            case 0x04: err = 10; break; //too many send frames
                            case 0x05: err = 11; break; //node address range error
                            case 0x06: err = 12; break; //node address duplication
                        }
                        break;
                    case 0x02:
                        switch (buffer[29])
                        {
                            case 0x01: err = 13; break; //destination node not in network
                            case 0x02: err = 14; break; //unit missing
                            case 0x03: err = 15; break; //third node missing
                            case 0x04: err = 16; break; //destination node busy
                            case 0x05: err = 17; break; //response timeout
                        }
                        break;
                    case 0x03:
                        switch (buffer[29])
                        {
                            case 0x01: err = 18; break; //communications controller error
                            case 0x02: err = 19; break; //CPU unit error
                            case 0x03: err = 20; break; //controller error
                            case 0x04: err = 21; break; //unit number error
                        }
                        break;
                    case 0x04:
                        switch (buffer[29])
                        {
                            case 0x01: err = 22; break; //undefined command
                            case 0x02: err = 23; break; //not supported by model/version
                        }
                        break;
                    case 0x05:
                        switch (buffer[29])
                        {
                            case 0x01: err = 24; break; //destination address setting error
                            case 0x02: err = 25; break; //no routing tables
                            case 0x03: err = 26; break; //routing table error
                            case 0x04: err = 27; break; //too many relays
                        }
                        break;
                    case 0x10:
                        switch (buffer[29])
                        {
                            case 0x01: err = 28; break; //command too long
                            case 0x02: err = 29; break; //command too short
                            case 0x03: err = 30; break; //elements/data don't match
                            case 0x04: err = 31; break; //command format error
                            case 0x05: err = 32; break; //header error
                        }
                        break;
                    case 0x11:
                        switch (buffer[29])
                        {
                            case 0x01: err = 33; break; //area classification missing
                            case 0x02: err = 34; break; //access size error
                            case 0x03: err = 35; break; //address range error
                            case 0x04: err = 36; break; //address range exceeded
                            case 0x06: err = 37; break; //program missing
                            case 0x09: err = 38; break; //relational error
                            case 0x0a: err = 39; break; //duplicate data access
                            case 0x0b: err = 40; break; //response too long
                            case 0x0c: err = 41; break; //parameter error
                        }
                        break;
                    case 0x20:
                        switch (buffer[29])
                        {
                            case 0x02: err = 42; break; //protected
                            case 0x03: err = 43; break; //table missing
                            case 0x04: err = 44; break; //data missing
                            case 0x05: err = 45; break; //program missing
                            case 0x06: err = 46; break; //file missing
                            case 0x07: err = 47; break; //data mismatch
                        }
                        break;
                    case 0x21:
                        switch (buffer[29])
                        {
                            case 0x01: err = 48; break; //read-only
                            case 0x02: err = 49; break; //protected , cannot write data link table
                            case 0x03: err = 50; break; //cannot register
                            case 0x05: err = 51; break; //program missing
                            case 0x06: err = 52; break; //file missing
                            case 0x07: err = 53; break; //file name already exists
                            case 0x08: err = 54; break; //cannot change
                        }
                        break;
                    case 0x22:
                        switch (buffer[29])
                        {
                            case 0x01: err = 55; break; //not possible during execution
                            case 0x02: err = 56; break; //not possible while running
                            case 0x03: err = 57; break; //wrong PLC mode
                            case 0x04: err = 58; break; //wrong PLC mode
                            case 0x05: err = 59; break; //wrong PLC mode
                            case 0x06: err = 60; break; //wrong PLC mode
                            case 0x07: err = 61; break; //specified node not polling node
                            case 0x08: err = 62; break; //step cannot be executed
                        }
                        break;
                    case 0x23:
                        switch (buffer[29])
                        {
                            case 0x01: err = 63; break; //file device missing
                            case 0x02: err = 64; break; //memory missing
                            case 0x03: err = 65; break; //clock missing
                        }
                        break;
                    case 0x24:
                        if (buffer[29] == 0x01) err = 66; //table missing
                        break;
                    case 0x25:
                        switch (buffer[29])
                        {
                            case 0x02: err = 67; break; //memory error
                            case 0x03: err = 68; break; //I/O setting error
                            case 0x04: err = 69; break; //too many I/O points
                            case 0x05: err = 70; break; //CPU bus error
                            case 0x06: err = 71; break; //I/O duplication
                            case 0x07: err = 72; break; //CPU bus error
                            case 0x09: err = 73; break; //SYSMAC BUS/2 error
                            case 0x0a: err = 74; break; //CPU bus unit error
                            case 0x0d: err = 75; break; //SYSMAC BUS No. duplication
                            case 0x0f: err = 76; break; //memory error
                            case 0x10: err = 77; break; //SYSMAC BUS terminator missing
                        }
                        break;
                    case 0x26:
                        switch (buffer[29])
                        {
                            case 0x01: err = 78; break; //no protection
                            case 0x02: err = 79; break; //incorrect password
                            case 0x04: err = 80; break; //protected
                            case 0x05: err = 81; break; //service already executing
                            case 0x06: err = 82; break; //service stopped
                            case 0x07: err = 83; break; //no execution right
                            case 0x08: err = 84; break; //settings required before execution
                            case 0x09: err = 85; break; //necessary items not set
                            case 0x0a: err = 86; break; //number already defined
                            case 0x0b: err = 87; break; //error will not clear
                        }
                        break;
                    case 0x30:
                        if (buffer[29] == 0x01) err = 88; //no access right
                        break;
                    case 0x40:
                        if (buffer[29] == 0x01) err = 89;//service aborted
                        break;
                }
            }

            if (err == 0 && isRead) Array.Copy(buffer, 30, rev, 0, rev.Length);

            return err;
        }
        #endregion



    }

} 