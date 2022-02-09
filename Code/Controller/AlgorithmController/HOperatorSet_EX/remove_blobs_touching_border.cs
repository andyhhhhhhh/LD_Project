//
//  File generated by HDevelop for HALCON/DOTNET (C#) Version 12.0
//


using System;
using HalconDotNet;
namespace HOperatorSet_EX
{

    public partial class HOperatorSet_Ex
    {
        // Short Description: 滤除与检测区域接触的blob区域，得到新的blob区域 
        public static void remove_blobs_touching_border(HObject ho_Image, HObject ho_ROIRegion,
            HObject ho_BlobRegions, out HObject ho_NewBlobRegions, out HTuple hv_ErrorCode)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ROIRegionTemp = null, ho_RegionErosion = null;
            HObject ho_RegionDifference = null, ho_ObjectSelected = null;
            HObject ho_RegionIntersection = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_i = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Number1 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_NewBlobRegions);
            HOperatorSet.GenEmptyObj(out ho_ROIRegionTemp);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            hv_ErrorCode = new HTuple();
            try
            {
                hv_ErrorCode = 0;
                //确保检测区域在图像内
                ho_ROIRegionTemp.Dispose();
                HOperatorSet.Intersection(ho_ROIRegion, ho_Image, out ho_ROIRegionTemp);
                //检测区域缩小
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_ROIRegionTemp, out ho_RegionErosion, 2.5);
                //获取检测区域的边界回型区域
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_ROIRegionTemp, ho_RegionErosion, out ho_RegionDifference
                    );
                //创建空的新Blob区域
                ho_NewBlobRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_NewBlobRegions);
                //去除与检测区域接触的blob区域
                //原理：blob区域与检测区域接触，那么必定与检测区域的边界回型区域有交集，有交集，把该区域去掉
                //无交集，把该blob区域保存在新的blob区域
                HOperatorSet.CountObj(ho_BlobRegions, out hv_Number);
                HTuple end_val14 = hv_Number;
                HTuple step_val14 = 1;
                for (hv_i = 1; hv_i.Continue(end_val14, step_val14); hv_i = hv_i.TupleAdd(step_val14))
                {
                    //选择一个blob区域
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_BlobRegions, out ho_ObjectSelected, hv_i);
                    //求交集，通过交集是否有效，来判断该blob区域是否为与边界接触，不接触，则保存
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_RegionDifference, ho_ObjectSelected, out ho_RegionIntersection
                        );
                    hv_Area = 0;
                    HOperatorSet.AreaCenter(ho_RegionIntersection, out hv_Area, out hv_Row, out hv_Column);
                    if ((int)(new HTuple(hv_Area.TupleGreater(0))) != 0)
                    {

                    }
                    else
                    {
                        HOperatorSet.CountObj(ho_NewBlobRegions, out hv_Number1);
                        if ((int)(new HTuple(hv_Number1.TupleEqual(0))) != 0)
                        {
                            ho_NewBlobRegions.Dispose();
                            HOperatorSet.CopyObj(ho_ObjectSelected, out ho_NewBlobRegions, 1, 1);
                        }
                        else
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_NewBlobRegions, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_NewBlobRegions.Dispose();
                                ho_NewBlobRegions = ExpTmpOutVar_0;
                            }
                        }
                    }

                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_ErrorCode = -1;
                //创建空的新Blob区域
                ho_NewBlobRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_NewBlobRegions);
            }
            ho_ROIRegionTemp.Dispose();
            ho_RegionErosion.Dispose();
            ho_RegionDifference.Dispose();
            ho_ObjectSelected.Dispose();
            ho_RegionIntersection.Dispose();

            return;
        }

    }
}