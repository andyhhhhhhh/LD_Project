using BaseController;
using BaseModels;
using Infrastructure.DBCore;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraContorller
{
    public interface ICameraControl
    {
        bool Init(object parameter);
        BaseResultModel Run(Camera2DSetModel controlModel, CameraControlType controlType);

        event EventHandler<object> GetImageByTrigger;
        event EventHandler<object> GetImageBySoft;
        event EventHandler<object> GetImageByContinue;
    }

    public enum CameraControlType
    {
        CameraDiscover,
        CameraGetImageByTirgger,
        CameraSetParam,
        CameraSetTriggerMode,
        /// <summary>
        /// 设置为触发拍照，不需要进行Snap操作
        /// </summary>
        CameraOpenByTirgger,
        CameraClose,
        CloseCamera,
        CameraOpenBySoft,
        CameraStartSnapBySoft,
        CameraGetImageBySoft,
        CameraStopGrab,
        //连续拍照 
        CameraOpenContinue,
        CameraStartContinue,
        CameraGetImageByContinue
    }

    public class CameraResultModel : BaseResultModel
    {
        public CameraResultModel()
        {

        }
        public object Image { get; set; }

        public object DispObj { get; set; }

        //public byte[] ImageBuf { get; set; }

        public int IndexResult { get; set; }//区别是哪个相机拍照

        public string ResultLabel { get; set; }//显示结果在界面
    }
}
