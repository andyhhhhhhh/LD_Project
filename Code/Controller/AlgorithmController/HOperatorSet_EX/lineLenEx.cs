using System;
using HalconDotNet;
namespace HOperatorSet_EX
{
      public partial class HOperatorSet_Ex

    {
       /// <summary>
       /// 扩展直线长度
       /// </summary>
       /// <param name="hv_Row1"></param>
       /// <param name="hv_Column1"></param>
       /// <param name="hv_Row2"></param>
       /// <param name="hv_Column2"></param>
       /// <param name="hv_lineLenEx"></param>
       /// <param name="hv_RowEx1"></param>
       /// <param name="hv_ColEx1"></param>
       /// <param name="hv_RowEx2"></param>
       /// <param name="hv_ColEx2"></param>
      public static void lineLenEx(HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2,
      HTuple hv_lineLenEx, out HTuple hv_RowEx1, out HTuple hv_ColEx1, out HTuple hv_RowEx2,
      out HTuple hv_ColEx2)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_Angle = null;
            // Initialize local and output iconic variables 
            HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Angle);
            hv_RowEx1 = hv_Row1 + ((hv_Angle.TupleSin()) * hv_lineLenEx);
            hv_ColEx1 = hv_Column1 - ((hv_Angle.TupleCos()) * hv_lineLenEx);
            hv_RowEx2 = hv_Row2 - ((hv_Angle.TupleSin()) * hv_lineLenEx);
            hv_ColEx2 = hv_Column2 + ((hv_Angle.TupleCos()) * hv_lineLenEx);
            return;
        }

  
    }
}
