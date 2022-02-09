using HalconDotNet;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserController
{
    public interface ISensor
    {
        event EventHandler<ImageClass> CompletedAcqEvent;
        void OnCompletedAcqEvent(ImageClass e);
        event EventHandler<string> MessageEvent;
        bool InitialSensor( SnapImageModel snapModel, string strIp, int Port);
        Task AcqusitionFromSensor(bool isRun, uint numberofprofile, int index);
        void CloseSensor();
        void ClearObject();
        object GetScanImage(int boardIndex);
        object GetSensor();
        void StopAcquistion(int index);
    }

    public class ImageClass
    {
        public int index { get; set; }
        public HObject Image { get; set; }
    }
}
