using BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSetModel
{ 
    public class CommonSetModel : BaseModel<CommonSetModel>
    {
        /// <summary>
        /// 是否保存结果图片
        /// </summary>
        public bool IsSaveBmp { get; set; }
        //空跑
        public bool IsRunTest { get; set; }
        public bool IsSaveNGBmp { get; set; }

        public bool IsRunStart { get; set; }

        public string BmpSavePath { get; set; }

        public int SaveDays { get; set; }//图片保存天数

        public string IPAddress { get; set; }

        public int Port { get; set; }

        public override CommonSetModel Clone()
        {
            throw new NotImplementedException();
        }
    }
}
