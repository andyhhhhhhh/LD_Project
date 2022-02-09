//
//  File generated by HDevelop for HALCON/DOTNET (C#) Version 12.0
//


using System;
using HalconDotNet;
namespace HOperatorSet_EX
{

    public partial class HOperatorSet_Ex
    {
        // Chapter: Graphics / Output
        // Short Description: Display the axes of a 3d coordinate system 
        public static void disp_3d_coord_system(HTuple hv_WindowHandle, HTuple hv_CamParam, HTuple hv_Pose,
            HTuple hv_CoordAxesLength)
        {



            // Local iconic variables 

            HObject ho_Arrows;

            // Local control variables 

            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null;
            HTuple hv_OrigCamY = null, hv_OrigCamZ = null, hv_Row0 = null;
            HTuple hv_Column0 = null, hv_X = null, hv_Y = null, hv_Z = null;
            HTuple hv_RowAxX = null, hv_ColumnAxX = null, hv_RowAxY = null;
            HTuple hv_ColumnAxY = null, hv_RowAxZ = null, hv_ColumnAxZ = null;
            HTuple hv_Distance = null, hv_HeadLength = null, hv_Red = null;
            HTuple hv_Green = null, hv_Blue = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrows);
            //This procedure displays a 3D coordinate system.
            //It needs the procedure gen_arrow_contour_xld.
            //
            //Input parameters:
            //WindowHandle: The window where the coordinate system shall be displayed
            //CamParam: The camera paramters
            //Pose: The pose to be displayed
            //CoordAxesLength: The length of the coordinate axes in world coordinates
            //
            //Check, if Pose is a correct pose tuple.
            if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
            {
                ho_Arrows.Dispose();

                return;
            }
            if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(new HTuple(((hv_CamParam.TupleSelect(
                0))).TupleNotEqual(0)))) != 0)
            {
                //For projective cameras:
                //Poses with Z position zero cannot be projected
                //(that would lead to a division by zero error).
                ho_Arrows.Dispose();

                return;
            }
            //Convert to pose to a transformation matrix
            HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
            //Project the world origin into the image
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                out hv_OrigCamY, out hv_OrigCamZ);
            HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                out hv_Row0, out hv_Column0);
            //Project the coordinate axes into the image
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
            //
            //Generate an XLD contour for each axis
            HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                hv_ColumnAxZ), out hv_Distance);
            hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                )).TupleInt();
            ho_Arrows.Dispose();
            gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);
            //
            //Display coordinate system
            HOperatorSet.DispXld(ho_Arrows, hv_WindowHandle);
            //
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(0), hv_Green.TupleSelect(
                0), hv_Blue.TupleSelect(0));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxX + 3, hv_ColumnAxX + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "X");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(1 % (new HTuple(hv_Red.TupleLength()
                ))), hv_Green.TupleSelect(1 % (new HTuple(hv_Green.TupleLength()))), hv_Blue.TupleSelect(
                1 % (new HTuple(hv_Blue.TupleLength()))));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxY + 3, hv_ColumnAxY + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "Y");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(2 % (new HTuple(hv_Red.TupleLength()
                ))), hv_Green.TupleSelect(2 % (new HTuple(hv_Green.TupleLength()))), hv_Blue.TupleSelect(
                2 % (new HTuple(hv_Blue.TupleLength()))));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxZ + 3, hv_ColumnAxZ + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "Z");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            ho_Arrows.Dispose();

            return;
        }
    }
}
