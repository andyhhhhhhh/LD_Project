using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionController
{
    public class LHFOcr
    {
        #region 参数定义
        public string[] strArrayOCR = new string[10];
        public List<int> m_iVectOCR = new List<int>();
        public int[] dArrayOCR_1 = new int[10];
        public int[] dArrayOCR_2 = new int[10];
        public int[] dArrayOCR_3 = new int[10];
        public int[] dArrayOCR_4 = new int[10];
        public int[] dArrayOCR_5 = new int[10];
        public int[] dArrayOCR_6 = new int[10];
        public int[] dArrayOCR_7 = new int[10];
        public int[] dArrayOCR_8 = new int[10];
        public int[] dArrayOCR_9 = new int[10];
        public int[] dArrayOCR_10 = new int[10];
        public string strOCR1;
        public string strOCR2;
        public string strOCR3;
        public string strOCR4;
        public string strOCR5;
        public string strOCR6;
        public string strOCR7;
        public string strOCR8;
        public string strOCR9;
        public string strOCR10;
        public List<int> iVectOCR1 = new List<int>();
        public List<int> iVectOCR2 = new List<int>();
        public List<int> iVectOCR3 = new List<int>();
        public List<int> iVectOCR4 = new List<int>();
        public List<int> iVectOCR5 = new List<int>();
        public List<int> iVectOCR6 = new List<int>();
        public List<int> iVectOCR7 = new List<int>();
        public List<int> iVectOCR8 = new List<int>();
        public List<int> iVectOCR9 = new List<int>();
        public List<int> iVectOCR10 = new List<int>();
        string[] strDisArr = new string[10];
        string reciveStr;
        #endregion

        public bool StrToIntList()
        {
            m_iVectOCR.Clear();
            string strGetArrayValue = null;
            int iStrToInt = 0;
            bool bsa = StrToArray();
            if (!bsa)
            {
                return false;
            }
            int iArrayLen = strArrayOCR.Length;
            if (10 != iArrayLen)
            {
                return false;
            }
            for (int index = 0; index < iArrayLen; index++)
            {
                strGetArrayValue = strArrayOCR[index];
                iStrToInt = int.Parse(strGetArrayValue);
                m_iVectOCR.Add(iStrToInt);
                if (2147483647 == iStrToInt)
                {
                    return false;
                }
            }
            return true;
        }
        public bool StrToArray()
        {
            //strArrayOCR.Initialize();
            if (!strOCR1.Equals("") && strOCR1.Length != 0)
            {
                strArrayOCR[0] = strOCR1;
            }
            else
            {
                return false;
            }
            if (!strOCR2.Equals("") && strOCR2.Length != 0)
            {
                strArrayOCR[1] = strOCR2;
            }
            else
            {
                return false;
            }
            if (!strOCR3.Equals("") && strOCR3.Length != 0)
            {
                strArrayOCR[2] = strOCR3;
            }
            else
            {
                return false;
            }
            if (!strOCR4.Equals("") && strOCR4.Length != 0)
            {
                strArrayOCR[3] = strOCR4;
            }
            else
            {
                return false;
            }
            if (!strOCR5.Equals("") && strOCR5.Length != 0)
            {
                strArrayOCR[4] = strOCR5;
            }
            else
            {
                return false;
            }
            if (!strOCR6.Equals("") && strOCR6.Length != 0)
            {
                strArrayOCR[5] = strOCR6;
            }
            else
            {
                return false;
            }
            if (!strOCR7.Equals("") && strOCR7.Length != 0)
            {
                strArrayOCR[6] = strOCR7;
            }
            else
            {
                return false;
            }
            if (!strOCR8.Equals("") && strOCR8.Length != 0)
            {
                strArrayOCR[7] = strOCR8;
            }
            else
            {
                return false;
            }
            if (!strOCR9.Equals("") && strOCR9.Length != 0)
            {
                strArrayOCR[8] = strOCR9;
            }
            else
            {
                return false;
            }
            if (!strOCR10.Equals("") && strOCR10.Length != 0)
            {
                strArrayOCR[9] = strOCR10;
            }
            else
            {
                return false;
            }
            return true;
        }
        public bool SortIntVect()
        {
            int add = 0;
            int ivalue = 0;
            int ivalue1 = 0;
            int ilistNum = m_iVectOCR.Count;
            if (10 != ilistNum)
            {
                return false;
            }
            iVectOCR1 = new List<int>();
            iVectOCR2 = new List<int>();
            iVectOCR3 = new List<int>();
            iVectOCR4 = new List<int>();
            iVectOCR5 = new List<int>();
            iVectOCR6 = new List<int>();
            iVectOCR7 = new List<int>();
            iVectOCR8 = new List<int>();
            iVectOCR9 = new List<int>();
            iVectOCR10 = new List<int>(); 
            for (int index = 0; index < ilistNum; index++)
            {
                add = 0;
                ivalue = m_iVectOCR[index];
                //正常物料推算后面产品排序
                switch (index)
                {
                    case 0:
                        if (ivalue > 0)
                        {//推算
                            iVectOCR1.Insert(0,ivalue);
                            for (int i = 1; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {//有物料的才推算，包括0
                                    iVectOCR1.Insert(i, ivalue + add);
                                }
                                else
                                {//没有物料的，仍然为没有物料-1
                                    iVectOCR1.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {//当前物料OCR为0，不进行推算，只要有料的全为0，无料的仍然为-1				
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR1.Insert(i, 0);

                                }
                                else
                                {
                                    iVectOCR1.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {//缺料的，全部为缺料
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR1.Insert(i, -1);
                            }
                        }

                        break;
                    case 1:
                        if (ivalue > 0)
                        {
                            iVectOCR2.Insert(0, ivalue - 1);
                            iVectOCR2.Insert(1, ivalue);
                            for (int i = 2; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR2.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR2.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR2.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR2.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR2.Insert(i, -1);
                            }
                        }

                        break;
                    case 2:
                        if (ivalue > 0)
                        {
                            iVectOCR3.Insert(0, ivalue - 2);
                            iVectOCR3.Insert(1, ivalue - 1);
                            iVectOCR3.Insert(2, ivalue);
                            for (int i = 3; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR3.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR3.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR3.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR3.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR3.Insert(i, -1);
                            }
                        }

                        break;
                    case 3:
                        if (ivalue > 0)
                        {
                            iVectOCR4.Insert(0, ivalue - 3);
                            iVectOCR4.Insert(1, ivalue - 2);
                            iVectOCR4.Insert(2, ivalue - 1);
                            iVectOCR4.Insert(3, ivalue);
                            for (int i = 4; i< 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR4.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR4.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR4.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR4.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR4.Insert(i, -1);
                            }
                        }

                        break;
                    case 4:
                        if (ivalue > 0)
                        {
                            iVectOCR5.Insert(0, ivalue - 4);
                            iVectOCR5.Insert(1, ivalue - 3);
                            iVectOCR5.Insert(2, ivalue - 2);
                            iVectOCR5.Insert(3, ivalue - 1);
                            iVectOCR5.Insert(4, ivalue);
                            for (int i = 5; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR5.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR5.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR5.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR5.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR5.Insert(i, -1);
                            }
                        }

                        break;
                    case 5:
                        if (ivalue > 0)
                        {
                            iVectOCR6.Insert(0, ivalue - 5);
                            iVectOCR6.Insert(1, ivalue - 4);
                            iVectOCR6.Insert(2, ivalue - 3);
                            iVectOCR6.Insert(3, ivalue - 2);
                            iVectOCR6.Insert(4, ivalue - 1);
                            iVectOCR6.Insert(5, ivalue);
                            for (int i = 6; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR6.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR6.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR6.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR6.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR6.Insert(i, -1);
                            }
                        }

                        break;
                    case 6:
                        if (ivalue > 0)
                        {
                            iVectOCR7.Insert(0, ivalue - 6);
                            iVectOCR7.Insert(1, ivalue - 5);
                            iVectOCR7.Insert(2, ivalue - 4);
                            iVectOCR7.Insert(3, ivalue - 3);
                            iVectOCR7.Insert(4, ivalue - 2);
                            iVectOCR7.Insert(5, ivalue - 1);
                            iVectOCR7.Insert(6, ivalue);
                            for (int i = 7; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR7.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR7.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR7.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR7.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR7.Insert(i, -1);
                            }
                        }

                        break;
                    case 7:
                        if (ivalue > 0)
                        {
                            iVectOCR8.Insert(0, ivalue - 7);
                            iVectOCR8.Insert(1, ivalue - 6);
                            iVectOCR8.Insert(2, ivalue - 5);
                            iVectOCR8.Insert(3, ivalue - 4);
                            iVectOCR8.Insert(4, ivalue - 3);
                            iVectOCR8.Insert(5, ivalue - 2);
                            iVectOCR8.Insert(6, ivalue - 1);
                            iVectOCR8.Insert(7, ivalue);
                            for (int i = 8; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR8.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR8.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR8.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR8.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR8.Insert(i, -1);
                            }
                        }

                        break;
                    case 8:
                        if (ivalue > 0)
                        {
                            iVectOCR9.Insert(0, ivalue - 8);
                            iVectOCR9.Insert(1, ivalue - 7);
                            iVectOCR9.Insert(2, ivalue - 6);
                            iVectOCR9.Insert(3, ivalue - 5);
                            iVectOCR9.Insert(4, ivalue - 4);
                            iVectOCR9.Insert(5, ivalue - 3);
                            iVectOCR9.Insert(6, ivalue - 2);
                            iVectOCR9.Insert(7, ivalue - 1);
                            iVectOCR9.Insert(8, ivalue);
                            for (int i = 9; i < 10; i++)
                            {
                                add++;
                                if (m_iVectOCR[i] > -1)
                                {
                                    iVectOCR9.Insert(i, ivalue + add);
                                }
                                else
                                {
                                    iVectOCR9.Insert(i, -1);
                                }
                            }
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR9.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR9.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR9.Insert(i, -1);
                            }
                        }

                        break;
                    case 9:
                        if (ivalue > 0)
                        {
                            iVectOCR10.Insert(0, ivalue - 9);
                            iVectOCR10.Insert(1, ivalue - 8);
                            iVectOCR10.Insert(2, ivalue - 7);
                            iVectOCR10.Insert(3, ivalue - 6);
                            iVectOCR10.Insert(4, ivalue - 5);
                            iVectOCR10.Insert(5, ivalue - 4);
                            iVectOCR10.Insert(6, ivalue - 3);
                            iVectOCR10.Insert(7, ivalue - 2);
                            iVectOCR10.Insert(8, ivalue - 1);
                            iVectOCR10.Insert(9, ivalue);
                        }
                        else if (ivalue == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                ivalue1 = m_iVectOCR[i];
                                if (ivalue1 > -1)
                                {
                                    iVectOCR10.Insert(i, 0);
                                }
                                else
                                {
                                    iVectOCR10.Insert(i, -1);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                iVectOCR10.Insert(i, -1);
                            }
                        }

                        break;

                    default:
                        return false;
                        break;
                }

            }

            return true;
        }
        public bool DeductionVect( ref List<int> vecNewDeduct)
        {
            List<List<int>> listvector = new List<List<int>>();
            //List<int> vctori;
            listvector.Add(iVectOCR1);
            listvector.Add(iVectOCR2);
            listvector.Add(iVectOCR3);
            listvector.Add(iVectOCR4);
            listvector.Add(iVectOCR5);
            listvector.Add(iVectOCR6);
            listvector.Add(iVectOCR7);
            listvector.Add(iVectOCR8);
            listvector.Add(iVectOCR9);
            listvector.Add(iVectOCR10);

            List<int> OutListVectorIndex = new List<int>();
            GetListValue(4, listvector, ref OutListVectorIndex);
            int iii = OutListVectorIndex[5];

            List<int> vecRow1 = new List<int>();
            List<int> vecRow2 = new List<int>();
            List<int> vecRow3 = new List<int>();
            List<int> vecRow4 = new List<int>();
            List<int> vecRow5 = new List<int>();
            List<int> vecRow6 = new List<int>();
            List<int> vecRow7 = new List<int>();
            List<int> vecRow8 = new List<int>();
            List<int> vecRow9 = new List<int>();
            List<int> vecRow10 = new List<int>();


            for (int i = 0; i < 10; i++)
            {
                GetListValue(i, listvector, ref OutListVectorIndex);
                vecRow1.Add(OutListVectorIndex[0]);
                vecRow2.Add(OutListVectorIndex[1]);
                vecRow3.Add(OutListVectorIndex[2]);
                vecRow4.Add(OutListVectorIndex[3]);
                vecRow5.Add(OutListVectorIndex[4]);
                vecRow6.Add(OutListVectorIndex[5]);
                vecRow7.Add(OutListVectorIndex[6]);
                vecRow8.Add(OutListVectorIndex[7]);
                vecRow9.Add(OutListVectorIndex[8]);
                vecRow10.Add(OutListVectorIndex[9]);

            }

            int irow1 = 0;
            int irow1_1 = 0;
            int irow1add = 0;
            int irowsize = 0;
            int maxNumber = 0;
            int position = 0;
            int iRealValue = 0;

            vecNewDeduct.Clear();
            List<int> vecRow1Value = new List<int>();
            List<int> vecRow1ValueNum = new List<int>();
            List<int> vecRow2Value = new List<int>();
            List<int> vecRow2ValueNum = new List<int>();
            List<int> vecRow3Value = new List<int>();
            List<int> vecRow3ValueNum = new List<int>();
            List<int> vecRow4Value = new List<int>();
            List<int> vecRow4ValueNum = new List<int>();
            List<int> vecRow5Value = new List<int>();
            List<int> vecRow5ValueNum = new List<int>();
            List<int> vecRow6Value = new List<int>();
            List<int> vecRow6ValueNum = new List<int>();
            List<int> vecRow7Value = new List<int>();
            List<int> vecRow7ValueNum = new List<int>();
            List<int> vecRow8Value = new List<int>();
            List<int> vecRow8ValueNum = new List<int>();
            List<int> vecRow9Value = new List<int>();
            List<int> vecRow9ValueNum = new List<int>();
            List<int> vecRow10Value = new List<int>();
            List<int> vecRow10ValueNum = new List<int>();
            // 1
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow1[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow1[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow1Value.Add(irow1);
                    vecRow1ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow1Value.Count();
            if (irowsize == 0)
            {
                vecRow1Value.Add(-1);
                vecRow1ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow1ValueNum);
            position = GetPositionOfMax(vecRow1ValueNum, maxNumber);
            iRealValue = vecRow1Value[position];
            vecNewDeduct.Add(iRealValue);
            // 2
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow2[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow2[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow2Value.Add(irow1);
                    vecRow2ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow2Value.Count();
            if (irowsize == 0)
            {
                vecRow2Value.Add(-1);
                vecRow2ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow2ValueNum);
            position = GetPositionOfMax(vecRow2ValueNum, maxNumber);
            iRealValue = vecRow2Value[position];
            vecNewDeduct.Add(iRealValue);
            // 3
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow3[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow3[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow3Value.Add(irow1);
                    vecRow3ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow3Value.Count();
            if (irowsize == 0)
            {
                vecRow3Value.Add(-1);
                vecRow3ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow3ValueNum);
            position = GetPositionOfMax(vecRow3ValueNum, maxNumber);
            iRealValue = vecRow3Value[position];
            vecNewDeduct.Add(iRealValue);
            // 4
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow4[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow4[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow4Value.Add(irow1);
                    vecRow4ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow4Value.Count();
            if (irowsize == 0)
            {
                vecRow4Value.Add(-1);
                vecRow4ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow4ValueNum);
            position = GetPositionOfMax(vecRow4ValueNum, maxNumber);
            iRealValue = vecRow4Value[position];
            vecNewDeduct.Add(iRealValue);

            // 5
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow5[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow5[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow5Value.Add(irow1);
                    vecRow5ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow5Value.Count();
            if (irowsize == 0)
            {
                vecRow5Value.Add(-1);
                vecRow5ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow5ValueNum);
            position = GetPositionOfMax(vecRow5ValueNum, maxNumber);
            iRealValue = vecRow5Value[position];
            vecNewDeduct.Add(iRealValue);

            // 6
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow6[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow6[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow6Value.Add(irow1);
                    vecRow6ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow6Value.Count();
            if (irowsize == 0)
            {
                vecRow6Value.Add(-1);
                vecRow6ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow6ValueNum);
            position = GetPositionOfMax(vecRow6ValueNum, maxNumber);
            iRealValue = vecRow6Value[position];
            vecNewDeduct.Add(iRealValue);

            // 7
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow7[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow7[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow7Value.Add(irow1);
                    vecRow7ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow7Value.Count();
            if (irowsize == 0)
            {
                vecRow7Value.Add(-1);
                vecRow7ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow7ValueNum);
            position = GetPositionOfMax(vecRow7ValueNum, maxNumber);
            iRealValue = vecRow7Value[position];
            vecNewDeduct.Add(iRealValue);
            // 8
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow8[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow8[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow8Value.Add(irow1);
                    vecRow8ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow8Value.Count();
            if (irowsize == 0)
            {
                vecRow8Value.Add(-1);
                vecRow8ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow8ValueNum);
            position = GetPositionOfMax(vecRow8ValueNum, maxNumber);
            iRealValue = vecRow8Value[position];
            vecNewDeduct.Add(iRealValue);
            // 9
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow9[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow9[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow9Value.Add(irow1);
                    vecRow9ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow9Value.Count();
            if (irowsize == 0)
            {
                vecRow9Value.Add(-1);
                vecRow9ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow9ValueNum);
            position = GetPositionOfMax(vecRow9ValueNum, maxNumber);
            iRealValue = vecRow9Value[position];
            vecNewDeduct.Add(iRealValue);
            // 10
            for (int i = 0; i < 10; i++)
            {
                irow1add = 0;
                irow1 = vecRow10[i];
                if (irow1 > 0)
                {//OCR读取有数据时，进行推算
                    for (int m = i; m < 10; m++)
                    {
                        irow1_1 = vecRow9[m];
                        if (irow1 == irow1_1)
                        {
                            irow1add++;
                        }
                    }
                    vecRow10Value.Add(irow1);
                    vecRow10ValueNum.Add(irow1add);
                }
            }
            //当一行数据中全部为0不能读取物料OCR，或者为-1缺料时，无法推算
            irowsize = vecRow10Value.Count();
            if (irowsize == 0)
            {
                vecRow10Value.Add(-1);
                vecRow10ValueNum.Add(0);
            }
            //选择最大可能性数值，赋值给新的推算vector
            maxNumber = FindMax(vecRow10ValueNum);
            position = GetPositionOfMax(vecRow10ValueNum, maxNumber);
            iRealValue = vecRow10Value[position];
            vecNewDeduct.Add(iRealValue);

            return true;
        }
        public bool GetDeductionVect(ref List<int> vecNewDeduct)
        {
            try
            {
                StrToIntList();
                SortIntVect();
                DeductionVect(ref vecNewDeduct);
                return true;
            }
            catch (Exception e){
                return false;
            }
            }
        public bool GetListValue(int index, List<List<int>> listVector, ref List<int> outListVectorIndex)
        {
            int iAdd = 0;
            for (int i = 0; i < listVector.Count; i++)
            {
                outListVectorIndex = listVector[i];
                if (iAdd == index)
                {
                    return true;
                }
                iAdd++;
            }

            return false;
        }
        public int FindMax(List<int> vec)
        {
            int max = -999;
            for (int i = 0; i < vec.Count(); i++)
            {
                if (max < vec[i]) max = vec[i];
            }
            return max;
        }
        public int GetPositionOfMax(List<int> vec, int max)
        {
            //double distance = find(vec.begin(), vec.end(), max);
            int distance = vec.Max();
            int index = vec.IndexOf(distance);
            return index;
            //return distance - vec[0];
        }
        public void SetOrigin(string str, string barStr, ref string[] strOcrArr)
        {
            // TODO: 在此添加控件通知处理程序代码
            reciveStr = str;
            string[] vecstr = SplitCString(reciveStr, ';');
            int length = vecstr.Count();
            strOcrArr = new string[length];
            string[] octTempArr = new string[length];

            for (int i = 0; i < length; i++)
            {
                string str2 = vecstr[i];
                int index = str2.IndexOf(',');
                string strDis = str2.Substring(0, index);
                string strOcr = str2.Substring(index + 1, str2.Length - strDis.Length - 1);
                strDisArr[i] = strDis;
                if (strOcr != "-1" && strOcr != "0")
                {
                    string productNum = "0";
                    if (strOcr.Length == 4) productNum = strOcr.Substring(2,strOcr.Length - 2);
                    if (strOcr.Length == 5) productNum = strOcr.Substring(3,strOcr.Length - 2);
                    strOcr = barStr + productNum;
                }

                octTempArr[i] = strOcr;
            }
            //更新数据
            if (reciveStr != null && reciveStr != "" && reciveStr.Length > 0)
            {
                GetData(strDisArr, octTempArr, ref strOcrArr);
                for (int i = 0; i < strDisArr.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            strOCR1 = strOcrArr[i];
                            break;
                        case 1:
                            strOCR2 = strOcrArr[i];
                            break;
                        case 2:
                            strOCR3 = strOcrArr[i];
                            break;
                        case 3:
                            strOCR4 = strOcrArr[i];
                            break;
                        case 4:
                            strOCR5 = strOcrArr[i];
                            break;
                        case 5:
                            strOCR6 = strOcrArr[i];
                            break;
                        case 6:
                            strOCR7 = strOcrArr[i];
                            break;
                        case 7:
                            strOCR8 = strOcrArr[i];
                            break;
                        case 8:
                            strOCR9 = strOcrArr[i];
                            break;
                        case 9:
                            strOCR10 = strOcrArr[i];
                            break;
                    }
                }
            }
        }
        public void GetData(string[] strArr, string[] strInOcr, ref string[] strOcrArr)
        {
            List<string> strArr2 = new List<string>();
            List<string> strArrOcr2 = new List<string>();
            strArr2.Clear();
            strArrOcr2.Clear();
            int size = strArr.Length;
            for (int i = 0; i < size - 1 ; i++)
            {
                //获取相邻两个值得差值
                int value1 = int.Parse(strArr[i]);
                int value2 = int.Parse(strArr[i + 1]);
                int offset = (value2 - value1) / 1050;
                strArr2.Add(strArr[i]);
                //OCR
                strArrOcr2.Add(strInOcr[i]);
                if (value2 != 0)
                {
                    for (int j = 0; j < System.Math.Abs(offset); j++)
                    {
                        if (strArr2.Count < 10)
                        {
                            strArr2.Add("0");
                            strArrOcr2.Add("-1");
    }
                    }
                }
                if (i == size - 2)
                {
                    //if (strArr2.Length < 10)
                    //{
                    int index = i + 1;
                        strArr2.Add(strArr[index]);
                        strArrOcr2.Add(strInOcr[index]);
                    //}
                }
            }

            strArr = strArr2.ToArray();
            strOcrArr = strArrOcr2.ToArray();
        }
        public string[] SplitCString(string strSource, char ch)
        {
            string strTmp;
            string[] vecString = new string[10];
            int n = -1;
            n = strSource.IndexOf(ch);
            int index = 0;
            while (n != -1)
            {
                strTmp = strSource.Substring(0,n);
                vecString[index] = strTmp;
                strSource = strSource.Substring(n + 1, strSource.Length - n - 1);
                n = strSource.IndexOf(ch);
                index++;
            }
            return vecString;
        }

    }
}
