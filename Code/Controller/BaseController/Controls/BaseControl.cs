using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;

namespace BaseController
{
    public class BaseControl
    {
        public bool m_IsInit = false;

    }
    public enum ControlType
    {
        CameraSnap,
        CameraSetParam,
        CameraOpen,
        CameraClose,

        SerialPortOpen,
        SerialPortSend,
        SerialPortReceive,
        SerialPortReceiveStatus,
        SerialPortSendAndReceive,
        SerialPortClose,

        SocketSend,
        SocketReceive,

        MotorSet,
        MotorGet, 
        MotorAnalysis,

        CalibrationMat2d,
        CalibreationCircleCenter,

        AlgorithmInit,
        AlgorithmRun,

        Run
    }
}
