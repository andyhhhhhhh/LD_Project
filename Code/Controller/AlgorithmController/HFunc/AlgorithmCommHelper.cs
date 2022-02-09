using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmController
{
    public class AlgorithmCommHelper
    {
        // Open a new graphics window that preserves the aspect ratio of the given image. 
        public static void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
                         HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
        { 
            // Local iconic variables 

            // Local control variables 

            HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
            HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_ResizeFactor = null, hv_ImageWidth = null, hv_ImageHeight = null;
            HTuple hv_TempWidth = null, hv_TempHeight = null, hv_WindowWidth = null;
            HTuple hv_WindowHeight = null;
            // Initialize local and output iconic variables 
            //This procedure opens a new graphics window and adjusts the size
            //such that it fits into the limits specified by WidthLimit
            //and HeightLimit, but also maintains the correct image aspect ratio.
            //
            //If it is impossible to match the minimum and maximum extent requirements
            //at the same time (f.e. if the image is very long but narrow),
            //the maximum value gets a higher priority,
            //
            //Parse input tuple WidthLimit
            if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
            {
                hv_MinWidth = 500;
                hv_MaxWidth = 800;
            }
            else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinWidth = 0;
                hv_MaxWidth = hv_WidthLimit.Clone();
            }
            else
            {
                hv_MinWidth = hv_WidthLimit.TupleSelect(0);
                hv_MaxWidth = hv_WidthLimit.TupleSelect(1);
            }
            //Parse input tuple HeightLimit
            if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(0))).TupleOr(
                new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
            {
                hv_MinHeight = 400;
                hv_MaxHeight = 600;
            }
            else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
                1))) != 0)
            {
                hv_MinHeight = 0;
                hv_MaxHeight = hv_HeightLimit.Clone();
            }
            else
            {
                hv_MinHeight = hv_HeightLimit.TupleSelect(0);
                hv_MaxHeight = hv_HeightLimit.TupleSelect(1);
            }
            //
            //Test, if window size has to be changed.
            hv_ResizeFactor = 1;
            HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
            //First, expand window to the minimum extents (if necessary).
            if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
                hv_ImageHeight)))) != 0)
            {
                hv_ResizeFactor = (((((hv_MinWidth.TupleReal()) / hv_ImageWidth)).TupleConcat(
                    (hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax();
            }
            hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
            //Then, shrink window to maximum extents (if necessary).
            if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
                hv_TempHeight)))) != 0)
            {
                hv_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()) / hv_TempWidth)).TupleConcat(
                    (hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin());
            }
            hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
            hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
            //Resize window
            HOperatorSet.SetWindowAttr("background_color", "black");
            HOperatorSet.OpenWindow(hv_Row, hv_Column, hv_WindowWidth, hv_WindowHeight, 0, "", "", out hv_WindowHandle);
            HDevWindowStack.Push(hv_WindowHandle);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.SetPart(HDevWindowStack.GetActive(), 0, 0, hv_ImageHeight - 1, hv_ImageWidth - 1);
            }

            return;
        }

        // This procedure writes a text message. 
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
                         HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_M = null, hv_N = null, hv_Red = null;
            HTuple hv_Green = null, hv_Blue = null, hv_RowI1Part = null;
            HTuple hv_ColumnI1Part = null, hv_RowI2Part = null, hv_ColumnI2Part = null;
            HTuple hv_RowIWin = null, hv_ColumnIWin = null, hv_WidthWin = null;
            HTuple hv_HeightWin = null, hv_I = null, hv_RowI = new HTuple();
            HTuple hv_ColumnI = new HTuple(), hv_StringI = new HTuple();
            HTuple hv_MaxAscent = new HTuple(), hv_MaxDescent = new HTuple();
            HTuple hv_MaxWidth = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRowI = new HTuple();
            HTuple hv_FactorColumnI = new HTuple(), hv_UseShadow = new HTuple();
            HTuple hv_ShadowColor = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            if ((int)(new HTuple(hv_Box_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Box_COPY_INP_TMP = "false";
            }
            //
            //
            //Check conditions
            //
            hv_M = (new HTuple(hv_Row_COPY_INP_TMP.TupleLength())) * (new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                ));
            hv_N = new HTuple(hv_Row_COPY_INP_TMP.TupleLength());
            if ((int)((new HTuple(hv_M.TupleEqual(0))).TupleOr(new HTuple(hv_String_COPY_INP_TMP.TupleEqual(
                new HTuple())))) != 0)
            {

                return;
            }
            if ((int)(new HTuple(hv_M.TupleNotEqual(1))) != 0)
            {
                //Multiple positions
                //
                //Expand single parameters
                if ((int)(new HTuple((new HTuple(hv_Row_COPY_INP_TMP.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    hv_N = new HTuple(hv_Column_COPY_INP_TMP.TupleLength());
                    HOperatorSet.TupleGenConst(hv_N, hv_Row_COPY_INP_TMP, out hv_Row_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(1))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_N, hv_Column_COPY_INP_TMP, out hv_Column_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
                    )).TupleNotEqual(new HTuple(hv_Row_COPY_INP_TMP.TupleLength())))) != 0)
                {
                    throw new HalconException("Number of elements in Row and Column does not match.");
                }
                if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_N, hv_String_COPY_INP_TMP, out hv_String_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )).TupleNotEqual(hv_N))) != 0)
                {
                    throw new HalconException("Number of elements in Strings does not match number of positions.");
                }
                //
            }
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_RowI1Part, out hv_ColumnI1Part,
                out hv_RowI2Part, out hv_ColumnI2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowIWin, out hv_ColumnIWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //Loop over all positions
            HTuple end_val89 = hv_N - 1;
            HTuple step_val89 = 1;
            for (hv_I = 0; hv_I.Continue(end_val89, step_val89); hv_I = hv_I.TupleAdd(step_val89))
            {
                hv_RowI = hv_Row_COPY_INP_TMP.TupleSelect(hv_I);
                hv_ColumnI = hv_Column_COPY_INP_TMP.TupleSelect(hv_I);
                //Allow multiple strings for a single position.
                if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
                {
                    hv_StringI = hv_String_COPY_INP_TMP.Clone();
                }
                else
                {
                    //In case of multiple positions, only single strings
                    //are allowed per position.
                    //For line breaks, use \n in this case.
                    hv_StringI = hv_String_COPY_INP_TMP.TupleSelect(hv_I);
                }
                //Default settings
                //-1 is mapped to 12.
                if ((int)(new HTuple(hv_RowI.TupleEqual(-1))) != 0)
                {
                    hv_RowI = 12;
                }
                if ((int)(new HTuple(hv_ColumnI.TupleEqual(-1))) != 0)
                {
                    hv_ColumnI = 12;
                }
                //
                //Split string into one string per line.
                hv_StringI = ((("" + hv_StringI) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_RowI.Clone();
                    hv_C1 = hv_ColumnI.Clone();
                }
                else
                {
                    //Transform image to window coordinates.
                    hv_FactorRowI = (1.0 * hv_HeightWin) / ((hv_RowI2Part - hv_RowI1Part) + 1);
                    hv_FactorColumnI = (1.0 * hv_WidthWin) / ((hv_ColumnI2Part - hv_ColumnI1Part) + 1);
                    hv_R1 = (((hv_RowI - hv_RowI1Part) + 0.5) * hv_FactorRowI) - 0.5;
                    hv_C1 = (((hv_ColumnI - hv_ColumnI1Part) + 0.5) * hv_FactorColumnI) - 0.5;
                }
                //
                //Display text box depending on text size.
                hv_UseShadow = 1;
                hv_ShadowColor = "gray";
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
                {
                    if (hv_Box_COPY_INP_TMP == null)
                        hv_Box_COPY_INP_TMP = new HTuple();
                    hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                    hv_ShadowColor = "#f28d26";
                }
                if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                    1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                    {
                        //Use default ShadowColor set above
                    }
                    else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                        "false"))) != 0)
                    {
                        hv_UseShadow = 0;
                    }
                    else
                    {
                        hv_ShadowColor = hv_Box_COPY_INP_TMP.TupleSelect(1);
                        //Valid color?
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                                1));
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            hv_Exception = new HTuple("Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)");
                            throw new HalconException(hv_Exception);
                        }
                    }
                }
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
                {
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            0));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = new HTuple("Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)");
                        throw new HalconException(hv_Exception);
                    }
                    //Calculate box extents
                    hv_StringI = (" " + hv_StringI) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_StringI.TupleSelect(hv_Index),
                            out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_StringI.TupleLength()));
                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //Display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    //Set shadow color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                    if ((int)(hv_UseShadow) != 0)
                    {
                        HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1,
                            hv_C2 + 1);
                    }
                    //Set box color
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    //Set color
                    if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
                    {
                        //Wiht a single text position, each text line
                        //may get a different color.
                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                            )));
                    }
                    else
                    {
                        //With multiple text positions, each position
                        //gets a single color for all text lines.
                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_I % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                            )));
                    }
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        //Valid color?
                        try
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            hv_Exception = ((("Wrong value of control parameter Color[" + (hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                                )))) + "] == '") + hv_CurrentColor) + "' (must be a valid color string)";
                            throw new HalconException(hv_Exception);
                        }
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    //Finally display text
                    hv_RowI = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_RowI, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_StringI.TupleSelect(hv_Index));
                }
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_RowI1Part, hv_ColumnI1Part, hv_RowI2Part,
                hv_ColumnI2Part);

            return;
        }

        // Get all image files under the given path 
        public static void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options,
                          out HTuple hv_ImageFiles)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_HalconImages = null, hv_OS = null;
            HTuple hv_Directories = null, hv_Index = null, hv_Length = null;
            HTuple hv_NetworkDrive = null, hv_Substring = new HTuple();
            HTuple hv_FileExists = new HTuple(), hv_AllFiles = new HTuple();
            HTuple hv_i = new HTuple(), hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = hv_Extensions.Clone();
            HTuple hv_ImageDirectory_COPY_INP_TMP = hv_ImageDirectory.Clone();

            // Initialize local and output iconic variables 
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //Input parameters:
            //ImageDirectory: as the name says
            //   If a tuple of directories is given, only the images in the first
            //   existing directory are returned.
            //   If a local directory is not found, the directory is searched
            //   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
            //   %HALCONROOT%/images is used instead.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //Output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            if ((int)(new HTuple(hv_ImageDirectory_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                hv_ImageDirectory_COPY_INP_TMP = ".";
            }
            HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(";");
            }
            else
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(":");
            }
            hv_Directories = hv_ImageDirectory_COPY_INP_TMP.Clone();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_Directories = hv_Directories.TupleConcat(((hv_HalconImages.TupleSelect(hv_Index)) + "/") + hv_ImageDirectory_COPY_INP_TMP);
            }
            HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
            HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_NetworkDrive);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                        )).TupleGreater(1))) != 0)
                    {
                        HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1, out hv_Substring);
                        if ((int)((new HTuple(hv_Substring.TupleEqual("//"))).TupleOr(new HTuple(hv_Substring.TupleEqual(
                            "\\\\")))) != 0)
                        {
                            if (hv_NetworkDrive == null)
                                hv_NetworkDrive = new HTuple();
                            hv_NetworkDrive[hv_Index] = 1;
                        }
                    }
                }
            }
            hv_ImageFiles = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                        hv_Options), out hv_AllFiles);
                    hv_ImageFiles = new HTuple();
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_i = (int)hv_i + 1)
                    {
                        HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                            hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                        hv_ImageFiles = hv_ImageFiles.TupleConcat(hv_Selection);
                    }
                    HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("\\\\")).TupleConcat(
                        "replace_all"), "/", out hv_ImageFiles);
                    if ((int)(hv_NetworkDrive.TupleSelect(hv_Index)) != 0)
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                        hv_ImageFiles = "/" + hv_ImageFiles;
                    }
                    else
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                    }

                    return;
                }
            }

            return;
        }
         
        // Display the results of Shape-Based Matching. 
        public static void dev_display_shape_matching_results(HTuple hv_ModelID, HTuple hv_Color,
                          HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC,
                          HTuple hv_Model, out HObject ho_ContoursAffinTrans)
        { 
            // Local iconic variables 

            HObject ho_ModelContours = null ;
            HObject ho_OutObj = null;

            // Local control variables 

            HTuple hv_NumMatches = null, hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = hv_Model.Clone();
            HTuple hv_ScaleC_COPY_INP_TMP = hv_ScaleC.Clone();
            HTuple hv_ScaleR_COPY_INP_TMP = hv_ScaleR.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_OutObj);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            try
            {
                //This procedure displays the results of Shape-Based Matching.
                //
                hv_NumMatches = new HTuple(hv_Row.TupleLength());
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_ScaleR_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleR_COPY_INP_TMP, out hv_ScaleR_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_ScaleC_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleC_COPY_INP_TMP, out hv_ScaleC_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out hv_Model_COPY_INP_TMP);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID.TupleSelect(
                            hv_Index), 1);
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                hv_Index % (new HTuple(hv_Color.TupleLength()))));
                        }
                        HTuple end_val18 = hv_NumMatches - 1;
                        HTuple step_val18 = 1; 
                        for (hv_Match = 0; hv_Match.Continue(end_val18, step_val18); hv_Match = hv_Match.TupleAdd(step_val18))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR_COPY_INP_TMP.TupleSelect(
                                    hv_Match), hv_ScaleC_COPY_INP_TMP.TupleSelect(hv_Match), 0, 0,
                                    out hv_HomMat2DScale);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                    hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                ho_OutObj.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_OutObj,
                                    hv_HomMat2DTranslate);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ContoursAffinTrans, HDevWindowStack.GetActive()
                                        );
                                }
                                HOperatorSet.ConcatObj(ho_ContoursAffinTrans, ho_OutObj, out ho_ContoursAffinTrans);
                            }
                        }
                    }
                }
                ho_ModelContours.Dispose();
                ho_OutObj.Dispose();
                //ho_ContoursAffinTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContours.Dispose();
                ho_OutObj.Dispose();
                //ho_ContoursAffinTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void dev_display_ncc_matching_results(HTuple hv_ModelID, HTuple hv_Color,
                         HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_Model, out HObject ho_OutObj)
        {



            // Local iconic variables 

            HObject ho_ModelRegion = null, ho_ModelContours = null;
            HObject ho_ContoursAffinTrans = null, ho_Cross = null;

            // Local control variables 

            HTuple hv_NumMatches = null, hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DRotate = new HTuple(), hv_HomMat2DTranslate = new HTuple();
            HTuple hv_RowTrans = new HTuple(), hv_ColTrans = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = hv_Model.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelRegion);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_OutObj);
            //This procedure displays the results of Correlation-Based Matching.
            //
            hv_NumMatches = new HTuple(hv_Row.TupleLength());
            if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(1))) != 0)
                {
                    HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out hv_Model_COPY_INP_TMP);
                }
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    ho_ModelRegion.Dispose();
                    HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID.TupleSelect(
                        hv_Index));
                    ho_ModelContours.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_ModelRegion, out ho_ModelContours, "border_holes");
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                            hv_Index % (new HTuple(hv_Color.TupleLength()))));
                    }
                    HTuple end_val13 = hv_NumMatches - 1;
                    HTuple step_val13 = 1;
                    for (hv_Match = 0; hv_Match.Continue(end_val13, step_val13); hv_Match = hv_Match.TupleAdd(step_val13))
                    {
                        if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                            hv_Match)))) != 0)
                        {
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                            HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angle.TupleSelect(
                                hv_Match), 0, 0, out hv_HomMat2DRotate);
                            HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                            ho_ContoursAffinTrans.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                                hv_HomMat2DTranslate);
                            if (HDevWindowStack.IsOpen())
                            {
                                HOperatorSet.DispObj(ho_ContoursAffinTrans, HDevWindowStack.GetActive()
                                    );
                            }
                            HOperatorSet.AffineTransPixel(hv_HomMat2DTranslate, 0, 0, out hv_RowTrans,
                                out hv_ColTrans);
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowTrans, hv_ColTrans,
                                6, hv_Angle.TupleSelect(hv_Match));
                            if (HDevWindowStack.IsOpen())
                            {
                                HOperatorSet.DispObj(ho_Cross, HDevWindowStack.GetActive());
                            }
                            
                            HOperatorSet.ConcatObj(ho_OutObj, ho_ContoursAffinTrans, out ho_OutObj);
                        }
                    }
                }
            }
            ho_ModelRegion.Dispose();
            ho_ModelContours.Dispose();
            ho_ContoursAffinTrans.Dispose();
            ho_Cross.Dispose();

            return;
        }
        
        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
                         HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            //This procedure generates arrow shaped XLD contours,
            //pointing from (Row1, Column1) to (Row2, Column2).
            //If starting and end point are identical, a contour consisting
            //of a single point is returned.
            //
            //input parameteres:
            //Row1, Column1: Coordinates of the arrows' starting points
            //Row2, Column2: Coordinates of the arrows' end points
            //HeadLength, HeadWidth: Size of the arrow heads in pixels
            //
            //output parameter:
            //Arrow: The resulting XLD contour
            //
            //The input tuples Row1, Column1, Row2, and Column2 have to be of
            //the same length.
            //HeadLength and HeadWidth either have to be of the same length as
            //Row1, Column1, Row2, and Column2 or have to be a single element.
            //If one of the above restrictions is violated, an error will occur.
            //
            //
            //Init
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            return;
        }

        public static void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min, HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageSelected = null, ho_SelectedChannel = null;
            HObject ho_LowerRegion = null, ho_UpperRegion = null, ho_ImageSelectedScaled = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_NumImages = null;
            HTuple hv_ImageIndex = null, hv_Channels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_MinGray = new HTuple();
            HTuple hv_MaxGray = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageSelectedScaled);
            //Convenience procedure to scale the gray values of the
            //input image Image from the interval [Min,Max]
            //to the interval [0,255] (default).
            //Gray values < 0 or > 255 (after scaling) are clipped.
            //
            //If the image shall be scaled to an interval different from [0,255],
            //this can be achieved by passing tuples with 2 values [From, To]
            //as Min and Max.
            //Example:
            //scale_image_range(Image:ImageScaled:[100,50],[200,250])
            //maps the gray values of Image from the interval [100,200] to [50,250].
            //All other gray values will be clipped.
            //
            //input parameters:
            //Image: the input image
            //Min: the minimum gray value which will be mapped to 0
            //     If a tuple with two values is given, the first value will
            //     be mapped to the second value.
            //Max: The maximum gray value which will be mapped to 255
            //     If a tuple with two values is given, the first value will
            //     be mapped to the second value.
            //
            //Output parameter:
            //ImageScale: the resulting scaled image.
            //
            if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_LowerLimit = hv_Min_COPY_INP_TMP.TupleSelect(1);
                hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_LowerLimit = 0.0;
            }
            if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_UpperLimit = hv_Max_COPY_INP_TMP.TupleSelect(1);
                hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_UpperLimit = 255.0;
            }
            //
            //Calculate scaling parameters.
            hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
            hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
            //
            //Scale image.
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult, hv_Add);
                ho_Image_COPY_INP_TMP.Dispose();
                ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
            }
            //
            //Clip gray values if necessary.
            //This must be done for each image and channel separately.
            ho_ImageScaled.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.CountObj(ho_Image_COPY_INP_TMP, out hv_NumImages);
            HTuple end_val49 = hv_NumImages;
            HTuple step_val49 = 1;
            for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val49, step_val49); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val49))
            {
                ho_ImageSelected.Dispose();
                HOperatorSet.SelectObj(ho_Image_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                HOperatorSet.CountChannels(ho_ImageSelected, out hv_Channels);
                HTuple end_val52 = hv_Channels;
                HTuple step_val52 = 1;
                for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val52, step_val52); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val52))
                {
                    ho_SelectedChannel.Dispose();
                    HOperatorSet.AccessChannel(ho_ImageSelected, out ho_SelectedChannel, hv_ChannelIndex);
                    HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                        out hv_MaxGray, out hv_Range);
                    ho_LowerRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                        hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                    ho_UpperRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                        ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_LowerLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_UpperLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    if ((int)(new HTuple(hv_ChannelIndex.TupleEqual(1))) != 0)
                    {
                        ho_ImageSelectedScaled.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageSelectedScaled, 1,
                            1);
                    }
                    else
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.AppendChannel(ho_ImageSelectedScaled, ho_SelectedChannel,
                                out ExpTmpOutVar_0);
                            ho_ImageSelectedScaled.Dispose();
                            ho_ImageSelectedScaled = ExpTmpOutVar_0;
                        }
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ImageScaled, ho_ImageSelectedScaled, out ExpTmpOutVar_0
                        );
                    ho_ImageScaled.Dispose();
                    ho_ImageScaled = ExpTmpOutVar_0;
                }
            }
            ho_Image_COPY_INP_TMP.Dispose();
            ho_ImageSelected.Dispose();
            ho_SelectedChannel.Dispose();
            ho_LowerRegion.Dispose();
            ho_UpperRegion.Dispose();
            ho_ImageSelectedScaled.Dispose();

            return;
        }

        /// <summary>
        /// 保存Dump结果文件
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_OutDispObj"></param>
        /// <param name="imagePath"></param>
        /// <param name="name"></param>
        /// <param name="bresult"></param>
        private void SaveDumpImage(HObject ho_Image, HObject ho_OutDispObj, string imagePath, string name, bool bresult)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    HTuple hv_ImageWidth, hv_ImageHeight;
                    HTuple hv_WriteWindow;
                    HObject ho_DumpImage;
                    HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
                    HOperatorSet.OpenWindow(0, 0, hv_ImageWidth, hv_ImageHeight, 0, "invisible", "", out hv_WriteWindow);
                    HOperatorSet.SetColored(hv_WriteWindow, 12);
                    HOperatorSet.SetDraw(hv_WriteWindow, "margin");
                    HOperatorSet.SetPart(hv_WriteWindow, 0, 0, hv_ImageHeight, hv_ImageWidth);
                    HOperatorSet.DispObj(ho_Image, hv_WriteWindow);
                    HOperatorSet.DispObj(ho_OutDispObj, hv_WriteWindow);
                    HOperatorSet.DumpWindowImage(out ho_DumpImage, hv_WriteWindow);

                    if (!System.IO.Directory.Exists(imagePath))
                    {
                        System.IO.Directory.CreateDirectory(imagePath);
                    }


                    string strName = "";
                    if (bresult)
                    {
                        strName = imagePath + name + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "_OK.png";
                    }
                    else
                    {
                        strName = imagePath + name + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "_NG.png";
                    }
                    HOperatorSet.WriteImage(ho_DumpImage, "png", 0, strName);
                    HOperatorSet.CloseWindow(hv_WriteWindow);
                }));
                t.Start();

            }
            catch (Exception ex)
            {

            }

        }
        
        public static HObject ConvertHImageFromBitmap(Bitmap bitmap, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite, bitmap.PixelFormat);
                bitmap.UnlockBits(bitmapData);
                HObject ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Image);
                //HOperatorSet.GenImageInterleaved(out ho_Image, bitmapData.Scan0, "bgrx", bitmapData.Width, bitmapData.Height, -1, "byte",
                //    bitmapData.Width, bitmapData.Height, 0, 0, -1, 0);
                HOperatorSet.GenImage1(out ho_Image, "byte", bitmapData.Width, bitmapData.Height, bitmapData.Scan0);
                //bitmap.Dispose();
                return ho_Image;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        public static HObject ConvertHImageFromBitmap2(Bitmap bitmap, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                HObject ho_Image;
                if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    ho_Image = Bitmap2HObjectBpp8(bitmap);
                }
                else if(bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    ho_Image = Bitmap2HObjectBpp24(bitmap);
                }
                else
                {
                    ho_Image = null;
                }
                return ho_Image;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }
         
        public static HObject Bitmap2HObjectBpp24(Bitmap bmp)
        {
            try
            {
                HObject image = new HObject();
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);

                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static HObject Bitmap2HObjectBpp8(Bitmap bmp)
        {
            try
            {
                HObject image = new HObject();
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static void SaveImage(HObject image, string dirName, string fileName = "")
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    string dir = dirName;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    //遍历图片是否超过999张，超过了则删除离当前时间最久的一张 
                    var files = Directory.GetFiles(dir);
                    int ncount = files.Count();
                    if (files.Count() >= 999)
                    {
                        int count = files.Count() - 999;
                        for (int i = 0; i < count; i++)
                        {
                            File.Delete(files[i]);
                        }
                    }

                    if (image != null && image.IsInitialized() && image.Key != (IntPtr)0)
                    {
                        HOperatorSet.WriteImage(image, "jpg", 0, dir + "\\" + fileName);
                    }

                    Thread.Sleep(100);
                }
                catch (HalconException ex)
                {
                }
            }));
            t.Start();
        }

        public static HObject GetDumpImage(HObject ho_Image, HObject ho_OutObj)
        {
            try
            {
                HTuple hv_ImageWidth, hv_ImageHeight;
                HTuple hv_WriteWindow;
                HObject ho_DumpImage;
                HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
                HOperatorSet.OpenWindow(0, 0, hv_ImageWidth, hv_ImageHeight, 0, "invisible", "", out hv_WriteWindow);
                HOperatorSet.SetColored(hv_WriteWindow, 12);
                HOperatorSet.SetDraw(hv_WriteWindow, "margin");
                HOperatorSet.SetPart(hv_WriteWindow, 0, 0, hv_ImageHeight, hv_ImageWidth);
                HOperatorSet.DispObj(ho_Image, hv_WriteWindow);
                HOperatorSet.DispObj(ho_OutObj, hv_WriteWindow);
                HOperatorSet.DumpWindowImage(out ho_DumpImage, hv_WriteWindow);

                HOperatorSet.ClearWindow(hv_WriteWindow);
                return ho_DumpImage;
            }
            catch (HalconException ex)
            {
                return ho_Image;
            }

        }

        //删除超过多少天的文件夹
        public static void DeleteDir(string dirPath, int iday, int maxCapity = 0)
        {
            try
            { 
                Task.Run(new Action(() =>
                {
                    DateTime currenttime = DateTime.Now;
                    DirectoryInfo root = new DirectoryInfo(dirPath);
                    foreach (var di in root.GetDirectories())
                    {
                        DateTime datetime = di.CreationTime;
                        TimeSpan midtime = currenttime - datetime;

                        if (midtime.Days > iday && iday != 0)
                        {
                            try
                            { 
                                di.Delete(true);
                            }
                            catch (Exception ex)
                            {
                                 
                            }
                            try
                            {
                                Directory.Delete(di.FullName);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    //删除文件
                    foreach(var fi in root.GetFiles("*.*"))
                    {
                        DateTime datetime = fi.LastWriteTime;
                        TimeSpan midtime = currenttime - datetime;

                        if (midtime.Days > iday && iday != 0)
                        {
                            fi.Delete();
                        }
                    }

                    //超过最大容量则删除一半
                    if(maxCapity != 0)
                    {
                        long lValue = GetDirectoryLength(dirPath)/1024/1024;
                        if (lValue > maxCapity)
                        {
                            int length = root.GetDirectories().Length/2;
                            for (int i = 0; i < length; i++)
                            {
                                root.GetDirectories()[i].Delete(true);
                            } 
                        }
                    }

                }));
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 获取指定路径的大小
        /// </summary>
        /// <param name="dirPath">路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            long len = 0;
            //判断该路径是否存在（是否为文件夹）
            if (!Directory.Exists(dirPath))
            {
                //查询文件的大小
                len = FileSize(dirPath);
            }
            else
            {
                //定义一个DirectoryInfo对象
                DirectoryInfo di = new DirectoryInfo(dirPath);

                //通过GetFiles方法，获取di目录中的所有文件的大小
                foreach (FileInfo fi in di.GetFiles())
                {
                    len += fi.Length;
                }
                //获取di中所有的文件夹，并存到一个新的对象数组中，以进行递归
                DirectoryInfo[] dis = di.GetDirectories();
                if (dis.Length > 0)
                {
                    for (int i = 0; i < dis.Length; i++)
                    {
                        len += GetDirectoryLength(dis[i].FullName);
                    }
                }
            }
            return len;
        }

        //所给路径中所对应的文件大小
        public static long FileSize(string filePath)
        {
            //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        
        public byte[] ToByte(System.Drawing.Bitmap bitmap)
        {
            try
            {
                //Bitmap bmp = (Bitmap)Image.FromFile(@"E:\\鹏峰贴合\\代码\\BaseSolution\\Transform\\CHalconVision(1031)-模板匹配\\x64\\Debug\\Mark\\test2.bmp");
                //Bitmap bmp = picSnapImage.Image as Bitmap;
                Bitmap bmp = bitmap;
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;
                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] rgbValues = new byte[bytes];
                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                // Unlock the bits.
                bmp.UnlockBits(bmpData);
                //testShowFormC(rgbValues, bytes)
                return rgbValues;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public enum ImageAngle
        {
            角度0,
            角度90,
            角度180,
            角度270
        }

        public static HObject RotateImage(HObject img, ImageAngle imageAngle)
        {
            try
            {
                HObject img1 = null;
                switch (imageAngle)
                {
                    case ImageAngle.角度0:
                        img1 = img;
                        break;
                    case ImageAngle.角度90:
                        HOperatorSet.RotateImage(img, out img1, 90, "constant");
                        img.Dispose();
                        break;
                    case ImageAngle.角度180:
                        HOperatorSet.RotateImage(img, out img1, 180, "constant");
                        img.Dispose();
                        break;
                    case ImageAngle.角度270:
                        HOperatorSet.RotateImage(img, out img1, 270, "constant");
                        img.Dispose();
                        break;
                    default:
                        break;
                }
                return img1;
            }
            catch(Exception ex)
            {
                return img;
            }
        }

        //镜像图像 strMirror : row column
        public static HObject MirrorImage(HObject img, string strMirror)
        {
            try
            {
                HObject img1 = null;
                HOperatorSet.MirrorImage(img, out img1, strMirror);
                img.Dispose();
                return img1;
            }
            catch (Exception ex)
            {
                return img;
            }
            
        }

        public HObject RotateImage(HObject himage, bool bFirst)
        {
            HTuple phi = bFirst ? 90 : 270;
            HObject ExpTmpOutVar_0 = null;
            HOperatorSet.RotateImage(himage, out ExpTmpOutVar_0, phi, "constant");
            himage.Dispose();
            return ExpTmpOutVar_0;
        }

        //找圆
        public static void FindCircle2D(HObject ho_Image, out HObject ho_MeasureCircleContours,
                          out HObject ho_MeasureCross, out HObject ho_CircleContours, HTuple hv_InCircleRow,
                          HTuple hv_InCircleCol, HTuple hv_InCircleRadiu, HTuple hv_InMeasureLength1,
                          HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                          HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                          HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_CircleCenterRow, out HTuple hv_CircleCenterColumn,
                          out HTuple hv_CircleRadius, out HTuple hv_bFindCircle2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyCircleIndex = new HTuple();
            HTuple hv_CircleParameter = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_CircleContours);
            hv_CircleCenterRow = new HTuple();
            hv_CircleCenterColumn = new HTuple();
            hv_CircleRadius = new HTuple();
            hv_bFindCircle2D = new HTuple();
            try
            {
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_CircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_CircleContours);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_InCircleRow,
                    hv_InCircleCol, hv_InCircleRadiu, hv_InMeasureLength1, hv_InMeasureLength2,
                    hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(), new HTuple(), out hv_MetrologyCircleIndex);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_measures", (int)hv_InMeasureNumber.D);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_interpolation", "nearest_neighbor");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "all", "result_type", "all_param", out hv_CircleParameter);
                //Extract the parameters for better readability
                hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                    )) - 1, 3);
                hv_CircleCenterRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                hv_CircleCenterColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);

                ho_CircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_CircleContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureCircleContours, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_Row1, hv_Column1, 26,
                    0.785398);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(hv_bDisp) != 0)
                {
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCircleContours, HDevWindowStack.GetActive()
                            );
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCross, HDevWindowStack.GetActive());
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_CircleContours, HDevWindowStack.GetActive());
                    }
                }
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterRow.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterColumn.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindCircle2D = 1;
                }
                else
                {
                    hv_bFindCircle2D = 0;
                }

            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindCircle2D = 0;
            }


            return;
        }


        //找圆弧
        public static void FindCircleArc2D(HObject ho_Image, out HObject ho_MeasureCircleContours,
                          out HObject ho_MeasureCross, out HObject ho_CircleContours, HTuple hv_InCircleRow,
                          HTuple hv_InCircleCol, HTuple hv_InCircleRadiu, HTuple hv_InMeasureLength1,
                          HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                          HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                          HTuple hv_InMeasureScore, HTuple hv_StartPhi, HTuple hv_EndPhi, HTuple hv_bDisp, out HTuple hv_CircleCenterRow,
                          out HTuple hv_CircleCenterColumn, out HTuple hv_CircleRadius, out HTuple hv_bFindCircle2D)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyCircleIndex = new HTuple();
            HTuple hv_CircleParameter = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_CircleContours);
            hv_CircleCenterRow = new HTuple();
            hv_CircleCenterColumn = new HTuple();
            hv_CircleRadius = new HTuple();
            hv_bFindCircle2D = new HTuple();
            try
            {
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCircleContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_CircleContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_CircleContours);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height); 
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_InCircleRow,
                      hv_InCircleCol, hv_InCircleRadiu, hv_InMeasureLength1, hv_InMeasureLength2,
                      hv_InMeasureSigma, hv_InMeasureThreshold, (new HTuple("start_phi")).TupleConcat("end_phi"), (new HTuple(hv_StartPhi)).TupleConcat(hv_EndPhi), out hv_MetrologyCircleIndex);

                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "num_measures", (int)hv_InMeasureNumber.D);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "measure_interpolation", "nearest_neighbor");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "instances_outside_measure_regions", "true"); 

                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyCircleIndex,
                    "all", "result_type", "all_param", out hv_CircleParameter);
                //Extract the parameters for better readability
                hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                    )) - 1, 3);
                hv_CircleCenterRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                hv_CircleCenterColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);

                ho_CircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_CircleContours, hv_MetrologyHandle,
                    "all", "all", 1.5);
                ho_MeasureCircleContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureCircleContours, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_Row1, hv_Column1, 26,
                    0.785398);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(hv_bDisp) != 0)
                {
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCircleContours, HDevWindowStack.GetActive()
                            );
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_MeasureCross, HDevWindowStack.GetActive());
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_CircleContours, HDevWindowStack.GetActive());
                    }
                }
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterRow.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_CircleCenterColumn.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindCircle2D = 1;
                }
                else
                {
                    hv_bFindCircle2D = 0;
                }

            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindCircle2D = 0;
            }


            return;
        }

        //找线
        public static void FindLine2D(HObject ho_Image, out HObject ho_MeasureLineContours, out HObject ho_MeasureCross,
                          out HObject ho_MeasuredLines, HTuple hv_InLineStartRow, HTuple hv_InLineStartCol,
                          HTuple hv_InLineEndRow, HTuple hv_InLineEndCol, HTuple hv_InMeasureLength1,
                          HTuple hv_InMeasureLength2, HTuple hv_InMeasureSigma, HTuple hv_InMeasureThreshold,
                          HTuple hv_InMeasureSelect, HTuple hv_InMeasureTransition, HTuple hv_InMeasureNumber,
                          HTuple hv_InMeasureScore, HTuple hv_bDisp, out HTuple hv_RowBegin, out HTuple hv_ColBegin,
                          out HTuple hv_RowEnd, out HTuple hv_ColEnd, out HTuple hv_AllRow, out HTuple hv_AllColumn,
                          out HTuple hv_bFindLine2D)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyLineIndices = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
            HOperatorSet.GenEmptyObj(out ho_MeasureCross);
            HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            hv_AllRow = new HTuple();
            hv_AllColumn = new HTuple();
            hv_bFindLine2D = new HTuple();
            try
            {
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureLineContours);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasureCross);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
                //
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_InLineStartRow,
                    hv_InLineStartCol, hv_InLineEndRow, hv_InLineEndCol, hv_InMeasureLength1,
                    hv_InMeasureLength2, hv_InMeasureSigma, hv_InMeasureThreshold, new HTuple(),
                    new HTuple(), out hv_MetrologyLineIndices);
                //设置参数
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_instances", 1);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_select", hv_InMeasureSelect);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_transition", hv_InMeasureTransition);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "num_measures", hv_InMeasureNumber);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "min_score", hv_InMeasureScore);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "measure_interpolation", "bicubic");
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "instances_outside_measure_regions", "true");
                //测量获取结果
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_begin", out hv_RowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_begin", out hv_ColBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "row_end", out hv_RowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyLineIndices,
                    "all", "result_type", "column_end", out hv_ColEnd);
                ho_MeasureLineContours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_MeasureLineContours, hv_MetrologyHandle,
                    "all", "all", out hv_AllRow, out hv_AllColumn);
                ho_MeasureCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_MeasureCross, hv_AllRow, hv_AllColumn,
                    3, 0.785398);
                ho_MeasuredLines.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLines, hv_MetrologyHandle,
                    "all", "all", 1.5);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                //if (bDisp)
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureLineContours)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasureCross)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (MeasuredLines)
                }
                //endif
                if ((int)((new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_RowBegin.TupleLength()
                    )))).TupleAnd(new HTuple((new HTuple(1)).TupleEqual(new HTuple(hv_ColEnd.TupleLength()
                    ))))) != 0)
                {
                    hv_bFindLine2D = 1;
                }
                else
                {
                    hv_bFindLine2D = 0;
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_bFindLine2D = 0;
            }


            return;
        }

        public static void FindRecPointCenter(out HObject ho_ProXLDTrans, out HObject ho_Cross1,
                          HTuple hv_GetLineStartRow, HTuple hv_GetLineStartCol, HTuple hv_GetLineEndRow,
                          HTuple hv_GetLineEndCol, out HTuple hv_InterRowOut, out HTuple hv_InterColumnOut,
                          out HTuple hv_CenterRow, out HTuple hv_CenterColumn, out HTuple hv_CenterPhi,
                          out HTuple hv_bRun)
        {
            HObject ho_Cross = null, ho_Contour = null, ho_Region = null;
            
            HTuple hv_Index3 = new HTuple(), hv_InterRow0 = new HTuple();
            HTuple hv_InterColumn0 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Row5 = new HTuple(), hv_Column5 = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ProXLDTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Region);
            hv_InterRowOut = new HTuple();
            hv_InterColumnOut = new HTuple();
            hv_CenterRow = new HTuple();
            hv_CenterColumn = new HTuple();
            hv_CenterPhi = new HTuple();
            hv_bRun = new HTuple();
            try
            { 
                hv_InterColumnOut = new HTuple(); 
                hv_InterRowOut = new HTuple();
                try
                {
                    for (hv_Index3 = 0; (int)hv_Index3 <= 3; hv_Index3 = (int)hv_Index3 + 1)
                    {
                        if ((int)(new HTuple(hv_Index3.TupleEqual(3))) != 0)
                        {
                            HOperatorSet.IntersectionLines(hv_GetLineStartRow.TupleSelect(hv_Index3),
                                hv_GetLineStartCol.TupleSelect(hv_Index3), hv_GetLineEndRow.TupleSelect(
                                hv_Index3), hv_GetLineEndCol.TupleSelect(hv_Index3), hv_GetLineStartRow.TupleSelect(
                                0), hv_GetLineStartCol.TupleSelect(0), hv_GetLineEndRow.TupleSelect(
                                0), hv_GetLineEndCol.TupleSelect(0), out hv_InterRow0, out hv_InterColumn0,
                                out hv_IsOverlapping);
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_InterRow0, hv_InterColumn0, 60, 0);
                        }
                        else
                        {
                            HOperatorSet.IntersectionLines(hv_GetLineStartRow.TupleSelect(hv_Index3),
                                hv_GetLineStartCol.TupleSelect(hv_Index3), hv_GetLineEndRow.TupleSelect(
                                hv_Index3), hv_GetLineEndCol.TupleSelect(hv_Index3), hv_GetLineStartRow.TupleSelect(
                                hv_Index3 + 1), hv_GetLineStartCol.TupleSelect(hv_Index3 + 1), hv_GetLineEndRow.TupleSelect(
                                hv_Index3 + 1), hv_GetLineEndCol.TupleSelect(hv_Index3 + 1), out hv_InterRow0,
                                out hv_InterColumn0, out hv_IsOverlapping);
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_InterRow0, hv_InterColumn0, 60, 0);
                        }

                        //以第二条线的角度为基准
                        if(hv_Index3 == 1)
                        {
                            HOperatorSet.AngleLx(hv_GetLineStartRow.TupleSelect(hv_Index3),
                                hv_GetLineStartCol.TupleSelect(hv_Index3), hv_GetLineEndRow.TupleSelect(
                                hv_Index3), hv_GetLineEndCol.TupleSelect(hv_Index3), out hv_CenterPhi); 
                            HOperatorSet.TupleDeg(hv_CenterPhi, out hv_CenterPhi);//新增弧度转为角度
                        }

                        HTuple ExpTmpLocalVar_InterRowOut = hv_InterRowOut.TupleConcat(hv_InterRow0);
                        hv_InterRowOut = ExpTmpLocalVar_InterRowOut;
                        HTuple ExpTmpLocalVar_InterColumnOut = hv_InterColumnOut.TupleConcat(hv_InterColumn0);
                        hv_InterColumnOut = ExpTmpLocalVar_InterColumnOut;
                    }

                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_InterRowOut.TupleConcat(
                        hv_InterRowOut.TupleSelect(0)), hv_InterColumnOut.TupleConcat(hv_InterColumnOut.TupleSelect(0)));

                    ho_ProXLDTrans.Dispose();
                    HOperatorSet.ShapeTransXld(ho_Contour, out ho_ProXLDTrans, "rectangle2");
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_ProXLDTrans, out ho_Region, "filled");
                    HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row4, out hv_Column4);
                    HOperatorSet.AreaCenterPointsXld(ho_ProXLDTrans, out hv_Area1, out hv_Row5, out hv_Column5);
                    HTuple hv_Phi;
                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_CenterRow, out hv_CenterColumn,
                       out hv_Phi, out hv_Length1, out hv_Length2);
                    ho_Cross1.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_CenterRow, hv_CenterColumn, 500, 0);
                    hv_bRun = 1;
                }
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception); 
                    hv_bRun = 0;
                }
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Region.Dispose();
                 
                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Region.Dispose(); 

                throw HDevExpDefaultException;
            }
        }

        //治具示教时算法
        public static void FindRecPointCenter_2(out HObject ho_ProXLDTrans, out HObject ho_Cross1,
                          HTuple hv_GetLineStartRow, HTuple hv_GetLineStartCol, HTuple hv_GetLineEndRow,
                          HTuple hv_GetLineEndCol, out HTuple hv_InterRowOut, out HTuple hv_InterColumnOut,
                          out HTuple hv_CenterRow, out HTuple hv_CenterColumn, out HTuple hv_CenterPhi,
                          out HTuple hv_bRun)
        {
            HObject ho_Cross = null, ho_Contour = null, ho_Region = null;

            HTuple hv_Index3 = new HTuple(), hv_InterRow0 = new HTuple();
            HTuple hv_InterColumn0 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Row5 = new HTuple(), hv_Column5 = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ProXLDTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Region);
            hv_InterRowOut = new HTuple();
            hv_InterColumnOut = new HTuple();
            hv_CenterRow = new HTuple();
            hv_CenterColumn = new HTuple();
            hv_CenterPhi = new HTuple();
            hv_bRun = new HTuple();
            try
            {
                hv_InterColumnOut = new HTuple();
                hv_InterRowOut = new HTuple();
                try
                {
                    for (hv_Index3 = 0; (int)hv_Index3 <= 3; hv_Index3 = (int)hv_Index3 + 1)
                    {
                        if ((int)(new HTuple(hv_Index3.TupleEqual(3))) != 0)
                        {
                            HOperatorSet.IntersectionLines(hv_GetLineStartRow.TupleSelect(hv_Index3),
                                hv_GetLineStartCol.TupleSelect(hv_Index3), hv_GetLineEndRow.TupleSelect(
                                hv_Index3), hv_GetLineEndCol.TupleSelect(hv_Index3), hv_GetLineStartRow.TupleSelect(
                                0), hv_GetLineStartCol.TupleSelect(0), hv_GetLineEndRow.TupleSelect(
                                0), hv_GetLineEndCol.TupleSelect(0), out hv_InterRow0, out hv_InterColumn0,
                                out hv_IsOverlapping);
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_InterRow0, hv_InterColumn0, 60, 0);
                        }
                        else
                        {
                            HOperatorSet.IntersectionLines(hv_GetLineStartRow.TupleSelect(hv_Index3),
                                hv_GetLineStartCol.TupleSelect(hv_Index3), hv_GetLineEndRow.TupleSelect(
                                hv_Index3), hv_GetLineEndCol.TupleSelect(hv_Index3), hv_GetLineStartRow.TupleSelect(
                                hv_Index3 + 1), hv_GetLineStartCol.TupleSelect(hv_Index3 + 1), hv_GetLineEndRow.TupleSelect(
                                hv_Index3 + 1), hv_GetLineEndCol.TupleSelect(hv_Index3 + 1), out hv_InterRow0,
                                out hv_InterColumn0, out hv_IsOverlapping);
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_InterRow0, hv_InterColumn0, 60, 0);
                        } 

                        HTuple ExpTmpLocalVar_InterRowOut = hv_InterRowOut.TupleConcat(hv_InterRow0);
                        hv_InterRowOut = ExpTmpLocalVar_InterRowOut;
                        HTuple ExpTmpLocalVar_InterColumnOut = hv_InterColumnOut.TupleConcat(hv_InterColumn0);
                        hv_InterColumnOut = ExpTmpLocalVar_InterColumnOut;
                    }

                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_InterRowOut.TupleConcat(
                        hv_InterRowOut.TupleSelect(0)), hv_InterColumnOut.TupleConcat(hv_InterColumnOut.TupleSelect(0)));

                    ho_ProXLDTrans.Dispose();
                    HOperatorSet.ShapeTransXld(ho_Contour, out ho_ProXLDTrans, "rectangle2");
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_ProXLDTrans, out ho_Region, "filled");
                    HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row4, out hv_Column4);
                    HOperatorSet.AreaCenterPointsXld(ho_ProXLDTrans, out hv_Area1, out hv_Row5, out hv_Column5);
                    HTuple hv_Phi;
                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_CenterRow, out hv_CenterColumn,
                       out hv_Phi, out hv_Length1, out hv_Length2);
                    HOperatorSet.TupleDeg(hv_Phi, out hv_CenterPhi);//新增弧度转为角度 

                    ho_Cross1.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_CenterRow, hv_CenterColumn, 500, 0);
                    hv_bRun = 1;
                }
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_bRun = 0;
                }
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Region.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_Region.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void GetZmap(HObject ho_SourceImage, HTuple hv_XResolution, HTuple hv_YResolution, HTuple hv_ZResolution,
                          out HObject ho_Z1, HTuple hv_thruseMin, HTuple hv_thruseMax, out HTuple hv_ERROR)
        {
            // Local iconic variables 

            HObject ho_ImageZoom = null, ho_ImageMedian = null;
            HObject ho_Region4 = null, ho_SelectImage = null, ho_X1 = null;
            HObject ho_Y1 = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Realpart3Dobject = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Z1);
            HOperatorSet.GenEmptyObj(out ho_ImageZoom);
            HOperatorSet.GenEmptyObj(out ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out ho_Region4);
            HOperatorSet.GenEmptyObj(out ho_SelectImage);
            HOperatorSet.GenEmptyObj(out ho_X1);
            HOperatorSet.GenEmptyObj(out ho_Y1);
            hv_ERROR = new HTuple();
            try
            {
                try
                {
                    HOperatorSet.GetImageSize(ho_SourceImage, out hv_Width, out hv_Height);
                    //保证图像的XY比例相等
                    ho_ImageZoom.Dispose();
                    HOperatorSet.ZoomImageSize(ho_SourceImage, out ho_ImageZoom, hv_Width * 1.00,
                        hv_Height * 1, "nearest_neighbor");
                    //对面平滑处理
                    ho_ImageMedian.Dispose();
                    HOperatorSet.MedianImage(ho_ImageZoom, out ho_ImageMedian, "circle", 1, "mirrored");


                    ho_Region4.Dispose();
                    HOperatorSet.Threshold(ho_ImageZoom, out ho_Region4, hv_thruseMin, hv_thruseMax);
                    ho_SelectImage.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageZoom, ho_Region4, out ho_SelectImage);
                    //设置图像的XY比例并生成3D模型
                    hv_XResolution = 0.019;
                    hv_YResolution = 0.019;
                    hv_ZResolution = 0.0016;

                    ConvertZmapImageTo3DObject(ho_SelectImage, hv_XResolution, hv_YResolution,
                        hv_ZResolution, out hv_Realpart3Dobject);


                    //3D模型重新生成Zmap图，拉升缩放，成像
                    ho_X1.Dispose(); ho_Y1.Dispose(); ho_Z1.Dispose();
                    HOperatorSet.ObjectModel3dToXyz(out ho_X1, out ho_Y1, out ho_Z1, hv_Realpart3Dobject,
                        "from_xyz_map", new HTuple(), new HTuple());
                    hv_ERROR = 0;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ERROR = 1;
                }
                ho_ImageZoom.Dispose();
                ho_ImageMedian.Dispose();
                ho_Region4.Dispose();
                ho_SelectImage.Dispose();
                ho_X1.Dispose();
                ho_Y1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoom.Dispose();
                ho_ImageMedian.Dispose();
                ho_Region4.Dispose();
                ho_SelectImage.Dispose();
                ho_X1.Dispose();
                ho_Y1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void ConvertZmapImageTo3DObject(HObject ho_ZMapImage, HTuple hv_XResolution,
                         HTuple hv_YResolution, HTuple hv_ZResolution, out HTuple hv_Object3DModule)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_PointCloudX, ho_ImageYVal, ho_PointCloudY;
            HObject ho_Region1, ho_ImageReduced, ho_ImageMeasureReal;
            HObject ho_PointCloudZ, ho_Region, ho_Union, ho_ImageReduced_PointCloudZ;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_zMap_Width = null;
            HTuple hv_zMap_Height = null, hv_Grayval = null, hv_hv_offset = null;
            HTuple hv_hvZ = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_PointCloudX);
            HOperatorSet.GenEmptyObj(out ho_ImageYVal);
            HOperatorSet.GenEmptyObj(out ho_PointCloudY);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageMeasureReal);
            HOperatorSet.GenEmptyObj(out ho_PointCloudZ);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_Union);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced_PointCloudZ);
            try
            {
                //***********
                HOperatorSet.GetImageSize(ho_ZMapImage, out hv_Width, out hv_Height);
                //XResolution := 0.019
                //YResolution := 0.019
                //ZResolution := 0.0016
                ho_PointCloudX.Dispose();
                HOperatorSet.GenImageSurfaceFirstOrder(out ho_PointCloudX, "real", hv_XResolution,
                    0, 0, hv_Height / 2, hv_Width / 2, hv_Width, hv_Height);
                ho_ImageYVal.Dispose();
                HOperatorSet.GenImageSurfaceFirstOrder(out ho_ImageYVal, "real", 0, hv_YResolution,
                    0, hv_Height / 2, hv_Width / 2, hv_Width, hv_Height);
                //gen_image_surface_first_order (PointCloudX, 'real', XResolution, 0, 0, 0, 0, Width, Height)
                //gen_image_surface_first_order (ImageYVal, 'real', 0, YResolution, 0, 0, 0, Width, Height)
                ho_PointCloudY.Dispose();
                HOperatorSet.ScaleImage(ho_ImageYVal, out ho_PointCloudY, 1, 0);

                //gen_rectangle1 (Rectangle, 0, 0, Height-1, Width-1)
                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ZMapImage, out ho_Region1, 20000, 65535);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ZMapImage, ho_Region1, out ho_ImageReduced);
                ho_ImageMeasureReal.Dispose();
                HOperatorSet.ConvertImageType(ho_ImageReduced, out ho_ImageMeasureReal, "real");
                HOperatorSet.GetImageSize(ho_ImageMeasureReal, out hv_zMap_Width, out hv_zMap_Height);
                ho_PointCloudZ.Dispose();
                HOperatorSet.GenImageConst(out ho_PointCloudZ, "real", hv_zMap_Width, hv_zMap_Height);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageMeasureReal, out ho_Region, 20000, 9999999);
                HOperatorSet.GetRegionPoints(ho_Region, out hv_zMap_Height, out hv_zMap_Width);
                HOperatorSet.GetGrayval(ho_ImageMeasureReal, hv_zMap_Height, hv_zMap_Width,
                    out hv_Grayval);
                HOperatorSet.TuplePow(2, 15, out hv_hv_offset);
                hv_hvZ = (hv_Grayval - hv_hv_offset) * hv_ZResolution;
                HOperatorSet.SetGrayval(ho_PointCloudZ, hv_zMap_Height, hv_zMap_Width, hv_hvZ);
                ho_Union.Dispose();
                HOperatorSet.Threshold(ho_PointCloudZ, out ho_Union, (new HTuple(-15)).TupleConcat(
                    0.0001), (new HTuple(-0.0001)).TupleConcat(15));
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Union1(ho_Union, out ExpTmpOutVar_0);
                    ho_Union.Dispose();
                    ho_Union = ExpTmpOutVar_0;
                }
                ho_ImageReduced_PointCloudZ.Dispose();
                HOperatorSet.ReduceDomain(ho_PointCloudZ, ho_Union, out ho_ImageReduced_PointCloudZ
                    );
                HOperatorSet.XyzToObjectModel3d(ho_PointCloudX, ho_PointCloudY, ho_ImageReduced_PointCloudZ,
                    out hv_Object3DModule);
                ho_PointCloudX.Dispose();
                ho_ImageYVal.Dispose();
                ho_PointCloudY.Dispose();
                ho_Region1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMeasureReal.Dispose();
                ho_PointCloudZ.Dispose();
                ho_Region.Dispose();
                ho_Union.Dispose();
                ho_ImageReduced_PointCloudZ.Dispose();

                return;
                //***********
                ho_PointCloudX.Dispose();
                ho_ImageYVal.Dispose();
                ho_PointCloudY.Dispose();
                ho_Region1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMeasureReal.Dispose();
                ho_PointCloudZ.Dispose();
                ho_Region.Dispose();
                ho_Union.Dispose();
                ho_ImageReduced_PointCloudZ.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_PointCloudX.Dispose();
                ho_ImageYVal.Dispose();
                ho_PointCloudY.Dispose();
                ho_Region1.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMeasureReal.Dispose();
                ho_PointCloudZ.Dispose();
                ho_Region.Dispose();
                ho_Union.Dispose();
                ho_ImageReduced_PointCloudZ.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public static void ScaleImageMap(HObject ho_Image, out HObject ho_ScaleImageMap, HTuple hv_GrayValueMin,
                          HTuple hv_GrayValueMax)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Mult = null, hv_Add = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ScaleImageMap);
            hv_Mult = 255.0 / (hv_GrayValueMax - hv_GrayValueMin);
            hv_Add = (-hv_Mult) * hv_GrayValueMin;
            ho_ScaleImageMap.Dispose();
            HOperatorSet.ScaleImage(ho_Image, out ho_ScaleImageMap, hv_Mult, hv_Add);

            return;
        }

        public static void FindDataMatrixECC200(HObject ho_Image, out HObject ho_SymbolXLDs,
                          HTuple hv_DataCodeHandleHigh, out HTuple hv_DataMatrixECC200String, out HTuple hv_Error)
        {




            // Local iconic variables 

            HObject ho_ImageEmphasize1 = null, ho_ImageClosing2 = null;

            // Local control variables 

            HTuple hv_ResultHandles = new HTuple(), hv_Number8 = new HTuple();
            HTuple hv_Number7 = new HTuple(), hv_Factor = new HTuple();
            HTuple hv_MaskWH = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            HOperatorSet.GenEmptyObj(out ho_ImageEmphasize1);
            HOperatorSet.GenEmptyObj(out ho_ImageClosing2);
            hv_DataMatrixECC200String = new HTuple();
            hv_Error = new HTuple();
            try
            {
                try
                {
                    hv_DataMatrixECC200String = new HTuple();
                    ho_SymbolXLDs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
                    ho_SymbolXLDs.Dispose();
                    HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandleHigh,
                        new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DataMatrixECC200String);
                    HOperatorSet.CountObj(ho_SymbolXLDs, out hv_Number8);
                    hv_Number7 = 0;
                    if ((int)(new HTuple(hv_Number8.TupleEqual(0))) != 0)
                    {
                        for (hv_Factor = 1; (int)hv_Factor <= 7; hv_Factor = (int)hv_Factor + 1)
                        {
                            for (hv_MaskWH = 3; (int)hv_MaskWH <= 7; hv_MaskWH = (int)hv_MaskWH + 1)
                            {
                                ho_ImageEmphasize1.Dispose();
                                HOperatorSet.Emphasize(ho_Image, out ho_ImageEmphasize1, hv_MaskWH,
                                    hv_MaskWH, hv_Factor);
                                ho_ImageClosing2.Dispose();
                                HOperatorSet.GrayClosingRect(ho_ImageEmphasize1, out ho_ImageClosing2,
                                    3, 3);
                                ho_SymbolXLDs.Dispose();
                                HOperatorSet.FindDataCode2d(ho_ImageClosing2, out ho_SymbolXLDs, hv_DataCodeHandleHigh,
                                    new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DataMatrixECC200String);
                                HOperatorSet.CountObj(ho_SymbolXLDs, out hv_Number7);
                                if ((int)(new HTuple(hv_Number7.TupleGreater(0))) != 0)
                                {
                                    break;
                                }
                            }
                            if ((int)(new HTuple(hv_Number7.TupleGreater(0))) != 0)
                            {
                                break;
                            }
                        }
                    }
                    if ((int)(new HTuple(hv_Number7.TupleEqual(0))) != 0)
                    {
                        hv_Error = 1;
                    }
                    else
                    {
                        hv_Error = 0;
                    }

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Error = 1;
                }
                ho_ImageEmphasize1.Dispose();
                ho_ImageClosing2.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageEmphasize1.Dispose();
                ho_ImageClosing2.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void CreateDataMatrixECC200(out HTuple hv_DataCodeHandleHigh)
        {

            // Initialize local and output iconic variables 
            //创建读码类型
            //standard_recognition  标准模式
            //enhanced_recognition  加强模式
            //maximum_recognition   最强模式
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", "default_parameters",
                "maximum_recognition", out hv_DataCodeHandleHigh);
            //**set_data_code_2d_param算子的参数解析
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "contrast_tolerance",
                "high");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "timeout", 200);
            //**对比容差
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "contrast_tolerance",
                "any");
            //**偏转
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "slant_max", 0.5235);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "finder_pattern_tolerance",
                "any");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_grid", "any");
            //码粒个数设置
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "symbol_size_min", 16);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "symbol_size_max", 20);
            //码粒像素设置
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_size_min", 3);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_size_max", 10);
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "small_modules_robustness",
                "high");
            //码粒间距
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_gap_min", "no");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_gap_max", "big");
            //鲁棒性
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "finder_pattern_tolerance",
                "any");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "contrast_tolerance",
                "any");
            HOperatorSet.SetDataCode2dParam(hv_DataCodeHandleHigh, "module_grid", "any");

            return;
        }
         
        public static double MeasureCalibFunc(HObject ho_Image, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
                            HTuple hv_Column2, HTuple hv_RealDis, HTuple hv_MaxThreshold, out HObject ho_SortedRegions)
        {
            // Local iconic variables 
            HObject ho_Rectangle, ho_ImageReduced;
            HObject ho_Region, ho_ConnectedRegions;

            // Local control variables 

            HTuple hv_Area = null, hv_Row = null;
            HTuple hv_Column = null, hv_length = null, hv_baseDistance = null;
            HTuple hv_DistanceArr = null, hv_Sum = null, hv_Index = null;
            HTuple hv_Distance = new HTuple(), hv_count = null, hv_MeanDis = null;
            HTuple hv_Scale = null;
            // Initialize local and output iconic variables  
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
            }

            //hv_RealDis = 5;
            //hv_MaxThreshold = 120;

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);

            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, hv_MaxThreshold);

            HOperatorSet.FillUp(ho_Region, out ho_Region);

            HOperatorSet.ClosingCircle(ho_Region, out ho_Region, 20);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_ConnectedRegions, "area", "and", 300, 9999999);

            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "character",
                "true", "row");

            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);

            HObject ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_Cross);
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 50, 0);
            HOperatorSet.ConcatObj(ho_SortedRegions, ho_Cross, out ho_SortedRegions);

            ho_Cross.Dispose();

            hv_length = new HTuple(hv_Row.TupleLength());
            hv_baseDistance = 0;
            hv_DistanceArr = new HTuple();
            hv_Sum = 0;
            HTuple end_val23 = hv_length - 2;
            HTuple step_val23 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val23, step_val23); hv_Index = hv_Index.TupleAdd(step_val23))
            {
                if ((int)(new HTuple(hv_Index.TupleEqual(0))) != 0)
                {
                    HOperatorSet.DistancePp(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0),
                        hv_Row.TupleSelect(1), hv_Column.TupleSelect(1), out hv_baseDistance);
                    hv_Sum = hv_Sum + hv_baseDistance;
                    hv_DistanceArr = hv_DistanceArr.TupleConcat(hv_baseDistance);
                }
                else
                {
                    HOperatorSet.DistancePp(hv_Row.TupleSelect(hv_Index), hv_Column.TupleSelect(
                        hv_Index), hv_Row.TupleSelect(hv_Index + 1), hv_Column.TupleSelect(hv_Index + 1),
                        out hv_Distance);
                    if ((int)(new HTuple(((hv_Distance - hv_baseDistance)).TupleLess(15))) != 0)
                    {
                        hv_DistanceArr = hv_DistanceArr.TupleConcat(hv_Distance);
                        hv_Sum = hv_Sum + hv_Distance;
                    }
                }
            }

            hv_count = new HTuple(hv_DistanceArr.TupleLength());
            hv_MeanDis = hv_Sum / hv_count;

            hv_Scale = hv_RealDis / hv_MeanDis;
            //ho_Image.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            //ho_SortedRegions.Dispose();

            return Math.Round(hv_Scale.D, 9);
        }

    }
}
