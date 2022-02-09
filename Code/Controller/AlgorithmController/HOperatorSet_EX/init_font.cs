//
//  File generated by HDevelop for HALCON/DOTNET (C#) Version 12.0
//


using System;
using HalconDotNet;
namespace HOperatorSet_EX
{

    public partial class HOperatorSet_Ex
    {
        // Short Description: set font independent of OS (currently only Courier supported) 
        public static void init_font(HTuple hv_WindowHandle, HTuple hv_Font, HTuple hv_Size)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_FontNameWin = new HTuple();
            HTuple hv_FontNameUnix = new HTuple(), hv_DefaultFontSize = null;
            HTuple hv_FontSizes = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)(new HTuple(hv_Font.TupleEqual("courier"))) != 0)
            {
                hv_FontNameWin = "Courier New";
                hv_FontNameUnix = "courier-bold-r-normal";
            }
            else
            {
                hv_FontNameWin = "Courier New";
                hv_FontNameUnix = "courier-bold-r-normal";
            }
            hv_DefaultFontSize = 12;
            hv_FontSizes = new HTuple();
            hv_FontSizes = hv_FontSizes.TupleConcat(hv_DefaultFontSize);
            hv_FontSizes = hv_FontSizes.TupleConcat(12);
            hv_FontSizes = hv_FontSizes.TupleConcat(14);
            hv_FontSizes = hv_FontSizes.TupleConcat(18);
            hv_FontSizes = hv_FontSizes.TupleConcat(24);
            //set font on Windows systems
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                if ((int)((new HTuple(hv_Size.TupleGreater(0))).TupleAnd(new HTuple(hv_Size.TupleLess(
                    5)))) != 0)
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((("-" + hv_FontNameWin) + "-") + (hv_FontSizes.TupleSelect(
                        hv_Size))) + "-*-*-*-*-1-");
                }
                else
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((("-" + hv_FontNameWin) + "-") + (hv_FontSizes.TupleSelect(
                        0))) + "-*-*-*-*-1-");
                }
            }
            else
            {
                //set font for UNIX systems
                if ((int)((new HTuple(hv_Size.TupleGreater(0))).TupleAnd(new HTuple(hv_Size.TupleLess(
                    5)))) != 0)
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((("-*-" + hv_FontNameUnix) + "-*-") + (hv_FontSizes.TupleSelect(
                        hv_Size))) + "-*-*-*-*-*-iso8859-1");
                }
                else
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((("-*-" + hv_FontNameUnix) + "-*-") + (hv_FontSizes.TupleSelect(
                        0))) + "-*-*-*-*-*-iso8859-1");
                }
            }

            return;
        }

    }
}
