using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ServiceController
{
    /*条码打印命令说明
    ^XA                           //条码打印指令开始
    ^MD30                         //设置色带颜色的深度, 取值范围从-30到30
    ^LH60,10                      //设置条码纸的边距
    ^FO20,10                      //设置条码左上角的位置
    ^ACN,18,10                    //设置字体
    ^BY1.4,3,50                   //设置条码样式。1.4是条码的缩放级别，3是条码中粗细柱的比例, 50是条码高度
    ^BC,,Y,N                      //打印code128的指令
    ^FD12345678^FS                //设置要打印的内容, ^FD是要打印的条码内容^FS表示换行
    ^XZ                           //条码打印指令结束
    */                            //上面的指令会打印12345678的CODE128的条码
    public class ZebraControl
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, out OVERLAPPED lpOverlapped);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        private int iHandle;
        public bool Open(string lpFileName = "LPT1:")
        {
            iHandle = CreateFile(lpFileName, (uint)FileAccess.ReadWrite, 0, 0, (int)FileMode.Open, 0, 0);
            if (iHandle != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Write(string Mystring)
        {
            if (iHandle != -1)
            {
                int i;
                OVERLAPPED x;
                byte[] mybyte = System.Text.Encoding.Default.GetBytes(Mystring);
                return WriteFile(iHandle, mybyte, mybyte.Length, out i, out x);
            }
            else
            {
                throw new Exception("LPT1端口未打开!");
            }
        }
        public bool Close()
        {
            return CloseHandle(iHandle);
        }  
    }
}
