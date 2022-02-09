using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLController;
using HalconDotNet;
using VisionController;
using SequenceTestModel;
using System.IO;
using GlobalCore;
using HalconView;
using AlgorithmController;
using CameraContorller;

namespace ManagementView
{
    /// <summary>
    /// 算法窗口
    /// </summary>
    public partial class AlgorithmView : UserControl
    { 
        HObject m_HoImage = new HObject();
        HTuple m_ModelID = new HTuple();
        HTuple m_ModelRow = new HTuple(), m_ModelColumn = new HTuple(), m_ModelAngle = new HTuple(), m_ModelScore = new HTuple();
        HTuple m_CenterRow = null, m_CenterColumn = null, m_CenterPhi = null;
        bool m_bDrawCreate = false;
        bool m_bDrawOcr = false;
        bool m_bDrawFind = false;
        List<HDrawingObject> m_DrawLine4List = new List<HDrawingObject>();
        HObject m_HoSearchRegion = new HObject();
        HObject m_HoOCRRegion = new HObject();
        HObject m_ImageRotateText = new HObject();
        SequenceModel m_SequenceModel = null;
        AlgorithmControl m_AlgorithmControl = null; 
        private HDrawingObject m_DrawModelRec1 = new HDrawingObject();
        private HDrawingObject m_DrawSearchRec1 = new HDrawingObject();
        private HDrawingObject m_DrawOCRRec1 = new HDrawingObject();   
        private OpenFileDialog OpenFileDialogImage = new OpenFileDialog();
        private HTuple m_DrawLineStartRow = new HTuple(), m_DrawLineStartCol = new HTuple(), m_DrawLineEndRow = new HTuple(), m_DrawLineEndCol = new HTuple();

        HSmartWindow m_hsmartWindow1 = new HSmartWindow();
        HSmartWindow m_hsmartWindow2 = new HSmartWindow();

        string m_strPath = "";

        AlgorithmModel m_algorithmModel;

        public AlgorithmView()
        {
            InitializeComponent(); 
            m_AlgorithmControl = new AlgorithmControl(); 
        }

        private void AlgorithmView_Load(object sender, EventArgs e)
        {
            CommHelper.LayoutChildFillView(panelView1, m_hsmartWindow1);
            CommHelper.LayoutChildFillView(panelView2, m_hsmartWindow2);
            InitParam();
        }

        private void InitParam()
        {
            try
            {
                m_SequenceModel = XmlControl.sequenceModelNew;

                foreach (var item in m_SequenceModel.Camera2DSetModels)
                {
                    cmbCamera.Items.Add(item.Name);
                }
                cmbCamera.Items.Add("下料相机");
                cmbCamera.Items.Add("治具相机");
                cmbCamera.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitData(AlgorithmModel algorithmModel)
        {
            if(algorithmModel == null)
            {
                return;
            }
            int nIndex = 0;
            dataGridView1.Rows.Clear();
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配起始角度";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.AngleStartFind;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配结束角度";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.AngleExtentFind;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配分数";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.MinScoreFind;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配数量";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.NumMatchesFind;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配等级";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.NumLevelsFind;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板匹配兼容";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.Greediness;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "模板对比度";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.Contrast;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线长度";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureLength1;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线宽度";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureLength2;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线平滑";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureSigma;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线阈值";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureThreshold;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线数量";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureNumber;
            nIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线分数";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index].Value = algorithmModel.InMeasureScore;

            nIndex = dataGridView1.Rows.Add();
            DataGridViewComboBoxCell comboxcell = new DataGridViewComboBoxCell();
            comboxcell.Items.Add("positive");
            comboxcell.Items.Add("negative");
            comboxcell.Value = algorithmModel.InMeasureTransition;
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线极性";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index] = comboxcell;

            nIndex = dataGridView1.Rows.Add();
            DataGridViewComboBoxCell comboxcell2 = new DataGridViewComboBoxCell();
            comboxcell2.Items.Add("first");
            comboxcell2.Items.Add("last");
            comboxcell2.Value = algorithmModel.InMeasureSelect;
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value = "寻找边线排序";
            dataGridView1.Rows[nIndex].Cells[dataGridView1.Columns["ParamValueBoxCol"].Index] = comboxcell2;
        }

        private void btn_SaveParam_Click(object sender, EventArgs e)
        {
            try
            {
                m_algorithmModel.HFileParamPath = m_strPath;
                m_algorithmModel.strOCRPath = m_strPath + "Lpr_ocr.bin";
               // m_algorithmModel.strOCRPath = m_strPath + "mvb_ocr.bin";

                if (m_DrawLine4List.Count == 4)
                {
                    int index = 0;

                    List<HTuple> Line4Param = new List<HTuple>();
                    m_DrawLineStartRow = new HTuple();
                    m_DrawLineStartCol = new HTuple();
                    m_DrawLineEndRow = new HTuple();
                    m_DrawLineEndCol = new HTuple();

                    foreach (HDrawingObject hdraw in m_DrawLine4List)
                    {
                        HTuple paramName;
                        string[] paramlist = { "row2", "column2", "row1", "column1" };
                        paramName = paramlist;
                        HTuple param = hdraw.GetDrawingObjectParams(paramName);
                        Line4Param.Add(param);
                        m_DrawLineStartRow.Append(Line4Param[index][0]);
                        m_DrawLineStartCol.Append(Line4Param[index][1]);
                        m_DrawLineEndRow.Append(Line4Param[index][2]);
                        m_DrawLineEndCol.Append(Line4Param[index][3]);
                        index++;
                    }

                    HOperatorSet.WriteTuple(m_DrawLineStartRow, m_strPath + "\\DrawLineStartRow");
                    HOperatorSet.WriteTuple(m_DrawLineStartCol, m_strPath + "\\DrawLineStartCol");
                    HOperatorSet.WriteTuple(m_DrawLineEndRow, m_strPath + "\\DrawLineEndRow");
                    HOperatorSet.WriteTuple(m_DrawLineEndCol, m_strPath + "\\DrawLineEndCol");
                }

                XmlControl.SaveToXml(Global.SequencePath, XmlControl.sequenceModelNew, typeof(SequenceModel));
                AddLog("保存参数成功!");
            }
            catch (Exception ex)
            {
                AddLog("保存参数异常！" + ex.Message);
            }
        }

        private void btn_DrawModelRegion_Click(object sender, EventArgs e)
        {
            //绘制模板区域
            if (!m_bDrawCreate)
            {
                m_bDrawCreate = true;
                DrawRec1Objects(ref m_DrawModelRec1, "green");
                AddLog("已经添加模板区域。");
            }
            else
            {
                AddLog("已经添加模板区域，不需要再增加。");
            }
        }
        
        private void DrawRec1Objects(ref HDrawingObject drawobj, string strColor)
        {
            //绘制模板区域
            try
            {
                m_hsmartWindow1.AddRect(ref drawobj, 1000, 1000, 2000, 2000, strColor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btn_DrawSearchRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_DrawSearchRegion.Text == "绘制搜索区域")
                {
                    //绘制模板搜索区域
                    if (!m_bDrawFind)
                    {
                        m_bDrawFind = true;
                        DrawRec1Objects(ref m_DrawSearchRec1, "red");
                        AddLog("已经绘制完成模板搜索区域。" + "");
                        btn_DrawSearchRegion.Text = "绘制搜索区域完成";
                        btn_DrawSearchRegion.FlatStyle = FlatStyle.Flat;
                    }
                    else
                    {
                        AddLog("已经绘制完成模板搜索区域，不需要再增加！" + "");
                    }
                }
                else if (btn_DrawSearchRegion.Text == "绘制搜索区域完成")
                {
                    GetDrawRec1Param(m_DrawSearchRec1, out m_HoSearchRegion);
                    HOperatorSet.WriteRegion(m_HoSearchRegion, m_strPath + "\\SearchRegion.hobj");

                    m_hsmartWindow1.ClearAllObjects();
                    HOperatorSet.ReadRegion(out m_HoSearchRegion, m_strPath + "\\SearchRegion.hobj");
                    m_hsmartWindow1.GetWindowHandle().SetColor("red");
                    m_hsmartWindow1.GetWindowHandle().SetDraw("margin");
                    m_hsmartWindow1.GetWindowHandle().DispObj(m_HoSearchRegion);

                    btn_DrawSearchRegion.Text = "绘制搜索区域";
                    btn_DrawSearchRegion.FlatStyle = FlatStyle.Standard;

                    MessageBox.Show("绘制搜索区域保存成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
         
        private void btn_DrawLine4_Click(object sender, EventArgs e)
        {
            int iLine4 = m_DrawLine4List.Count;
            if (iLine4 > 3)
            {
                AddLog("已经绘制完成四条边线，不需要再增加！" + "");
                return;
            }
            string path_startRow = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineStartRow";
            string path_startCol = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineStartCol";
            string path_endRow = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineEndRow";
            string path_endCol = Global.Model3DPath + "//" + cmbCamera.Text + "//DrawLineEndCol";
            if(File.Exists(path_startRow) && File.Exists(path_startCol) && File.Exists(path_endRow) && File.Exists(path_endCol))
            {
                HTuple hv_startRow, hv_startCol, hv_endRow, hv_endCol;
                HOperatorSet.ReadTuple(path_startRow, out hv_startRow);
                HOperatorSet.ReadTuple(path_startCol, out hv_startCol);
                HOperatorSet.ReadTuple(path_endRow, out hv_endRow);
                HOperatorSet.ReadTuple(path_endCol, out hv_endCol);

                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(hv_endRow[0], hv_endCol[0], hv_startRow[0], hv_startCol[0]));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(hv_endRow[1], hv_endCol[1], hv_startRow[1], hv_startCol[1]));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(hv_endRow[2], hv_endCol[2], hv_startRow[2], hv_startCol[2]));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(hv_endRow[3], hv_endCol[3], hv_startRow[3], hv_startCol[3])); 
            }
            else
            {
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(1337, 1820, 2679, 1873));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(1252, 3554, 1347, 1831));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(2563, 3628, 1263, 3554));
                m_DrawLine4List.Add(m_hsmartWindow1.AddLine(2701, 1884, 2613, 3626)); 
            }          

            AddLog("增加绘制边线" + iLine4.ToString() + "成功！" + "");
        }

        private void btn_ReadImage_Click(object sender, EventArgs e)
        {
            Stream inputStream = null;
            this.OpenFileDialogImage.Filter = "All files (*.*)|*.*";
            if (this.OpenFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((inputStream = this.OpenFileDialogImage.OpenFile()) != null)
                    {
                        String strImageFile = this.OpenFileDialogImage.FileName;
                        this.OpenFileDialogImage.InitialDirectory = strImageFile;
                        HOperatorSet.ReadImage(out m_HoImage, strImageFile);
                        m_hsmartWindow1.FitImageToWindow(new HImage(m_HoImage), null);
                        AddLog("读入图像成功。");
                    }
                }
                catch (Exception ex)
                {
                    AddLog("打开图像失败!" + ex.Message + "");
                }
            }
        }

        private void btn_AutoRun_Click(object sender, EventArgs e)
        {
            try
            {
                TestAlgorithm();
            }
            catch (Exception ex)
            {
                AddLog("自动测试定位异常！" + ex.Message + "");
            }
        }

        private void btnCycleRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkIsPathTest.Checked)
                {
                    string strPath = loadPath.FolderPath;  
                    var listFile = Directory.GetFiles(strPath, "*.jpg").Union(Directory.GetFiles(strPath, "*.png")).Union(Directory.GetFiles(strPath, "*.bmp")).ToArray().ToList();

                    foreach (var file in listFile)
                    {
                        HOperatorSet.ReadImage(out m_HoImage, file);
                        TestAlgorithm();
                        var result = MessageBox.Show("是否继续测试?", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.Cancel)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void TestAlgorithm()
        { 
            AlgorithmResultModel resultModel = AlgorithmFunc(m_HoImage, m_algorithmModel);
            if (resultModel.bFindCenter)
            {
                m_hsmartWindow1.FitImageToWindow(m_HoImage, null);
                m_hsmartWindow1.GetWindowHandle().SetColor("red");
                m_hsmartWindow1.GetWindowHandle().SetDraw("margin");

                m_hsmartWindow1.GetWindowHandle().DispObj(resultModel.ProXLDTrans);
                m_hsmartWindow1.GetWindowHandle().DispObj(resultModel.CenterCross);
                m_hsmartWindow1.GetWindowHandle().DispObj(resultModel.ObjectResult as HObject);
                HTuple hv_imageWidth = null, hv_imageHeight = null;
                HOperatorSet.GetImageSize(resultModel.ImageRotateText, out hv_imageWidth, out hv_imageHeight);

                m_hsmartWindow2.FitImageToWindow(resultModel.ImageRotateText, null);

                HOperatorSet.WriteImage(resultModel.ImageRotateText, "jpg", 0, "OCR");
                AddLog("自动测试定位成功。" + "CenterRow:" + resultModel.CenterRow.TupleString(".3f") +
                                                         "  CenterColumn:" + resultModel.CenterColumn.TupleString(".3f") +
                                                         "  CenterPhi:" + resultModel.CenterPhi.TupleString(".3f") + "");
                AddLog("OCR字符读取：" + resultModel.strOCR + "。");
            }
            else
            {
                m_hsmartWindow1.FitImageToWindow(m_HoImage, null);
                m_hsmartWindow1.GetWindowHandle().SetColor("red");
                m_hsmartWindow1.GetWindowHandle().SetDraw("margin");

                m_hsmartWindow1.GetWindowHandle().SetLineWidth(2);

                m_hsmartWindow1.GetWindowHandle().DispObj(resultModel.ObjectResult as HObject);

                AddLog("自动测试定位失败！" + "");
            }

            if(!string.IsNullOrEmpty(resultModel.ErrorMessage))
            {
                AddLog(resultModel.ErrorMessage);
            }
        }
        
        private void btn_ReadOCRModel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDlg = null;
            try
            {
                fileDlg = new System.Windows.Forms.OpenFileDialog();
                fileDlg.Filter = @"model(*.bin)|*.bin||";
                fileDlg.RestoreDirectory = true;

                if (fileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //m_AlgorithmControl.m_stCNNOCRToolObj.BasicParam.LoadModel(fileDlg.FileName);
                    m_algorithmModel.strOCRPath = fileDlg.FileName;
                    AddLog("加载OCR模板成功 " + fileDlg.FileName + "。");
                }
                fileDlg.Dispose();
            }
            catch (System.Exception ex)
            {
                AddLog("加载OCR模板异常 " + fileDlg.FileName + ": " + ex.Message + "");
            }
            finally
            {
                if (null != fileDlg)
                {
                    fileDlg.Dispose();
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Type gettype = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType();
            ////dataGridView1 里面 DataGridViewComboBoxCell 类型
            if (dataGridView1.CurrentCell.GetType().FullName.Contains("DataGridViewComboBoxCell"))
            {
                if (13 == e.RowIndex)
                {
                    m_algorithmModel.InMeasureTransition = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
                if (14 == e.RowIndex)
                {
                    m_algorithmModel.InMeasureSelect = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
            }
            //dataGridView1 里面 DataGridViewTextBoxCell 类型
            if (dataGridView1.CurrentCell.GetType().FullName.Contains("DataGridViewTextBoxCell"))
            {
                double dParamValue = 0.0;
                String strName = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["ParamNameBoxCol"].Index].Value.ToString();
                Double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dParamValue);
                switch (strName)
                {
                    case "模板匹配起始角度":
                        m_algorithmModel.AngleStartFind = dParamValue;
                        break;
                    case "模板匹配结束角度":
                        m_algorithmModel.AngleExtentFind = dParamValue;
                        break;
                    case "模板匹配分数":
                        m_algorithmModel.MinScoreFind = dParamValue;
                        break;
                    case "模板匹配数量":
                        m_algorithmModel.NumMatchesFind = (int)dParamValue;
                        break;
                    case "模板匹配等级":
                        m_algorithmModel.NumLevelsFind = (int)dParamValue;
                        break;
                    case "模板匹配兼容":
                        m_algorithmModel.Greediness = dParamValue;
                        break;
                    case "模板对比度":
                        m_algorithmModel.Contrast = (int)dParamValue;
                        break;
                    case "寻找边线长度":
                        m_algorithmModel.InMeasureLength1 = dParamValue;
                        break;
                    case "寻找边线宽度":
                        m_algorithmModel.InMeasureLength2 = dParamValue;
                        break;
                    case "寻找边线平滑":
                        m_algorithmModel.InMeasureSigma = dParamValue;
                        break;
                    case "寻找边线阈值":
                        m_algorithmModel.InMeasureThreshold = (int)dParamValue;
                        break;
                    case "寻找边线数量":
                        m_algorithmModel.InMeasureNumber = (int)dParamValue;
                        break;
                    case "寻找边线分数":
                        m_algorithmModel.InMeasureScore = dParamValue;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 拍照Func
        /// </summary>
        public Func<Camera2DSetModel, CameraResultModel> m_FuncCameraSnap;
        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cmbCamera.SelectedIndex;
                if(index > 1)
                {
                    index = 0;
                }
                Camera2DSetModel tModel = XMLController.XmlControl.sequenceModelNew.Camera2DSetModels.FirstOrDefault(x => x.Id == index); 
                CameraResultModel resultModel = m_FuncCameraSnap(tModel);
                m_HoImage = resultModel.Image as HObject;

                if (resultModel.RunResult)
                {
                    m_hsmartWindow1.FitImageToWindow(m_HoImage, null);
                }
                else
                {
                    AddLog(resultModel.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
        }

        private void cmbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_strPath = Global.Model3DPath + "//" + cmbCamera.Text + "//";
                m_algorithmModel = m_SequenceModel.algorithmModels.FirstOrDefault(x => x.Name == cmbCamera.Text);

                if (m_algorithmModel == null)
                {
                    m_algorithmModel = new AlgorithmModel();
                    m_algorithmModel.Name = cmbCamera.Text;
                    m_SequenceModel.algorithmModels.Add(m_algorithmModel);
                }

                InitData(m_algorithmModel);

            }
            catch (Exception ex)
            {
                 
            }
        }
                  
        private void btn_CleanReDraw_Click(object sender, EventArgs e)
        {
            m_bDrawCreate = false;
            m_bDrawFind = false;
            m_bDrawOcr = false; 
            m_hsmartWindow1.ClearAllObjects();
            m_DrawLine4List = null;
            m_DrawLine4List = new List<HDrawingObject>();
            AddLog("清除所有绘制内容！" + "");
        }

        private void btn_DrawOCRRegion_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_DrawOCRRegion.Text == "绘制字符区域")
                {
                    //绘制OCR区域
                    if (!m_bDrawOcr)
                    {
                        m_bDrawOcr = true;
                        DrawRec1Objects(ref m_DrawOCRRec1, "yellow");
                        AddLog("已经添加OCR区域。" + "");
                        btn_DrawOCRRegion.Text = "绘制字符区域完成";
                        btn_DrawOCRRegion.FlatStyle = FlatStyle.Flat;
                    }
                    else
                    {
                        AddLog("已经绘制OCR区域，不需要再增加！" + "");
                    }
                }
                else if (btn_DrawOCRRegion.Text == "绘制字符区域完成")
                {
                    GetDrawRec1Param(m_DrawOCRRec1, out m_HoOCRRegion);
                    HOperatorSet.WriteRegion(m_HoOCRRegion, m_strPath + "\\OCRRegion.hobj");


                    m_hsmartWindow1.ClearAllObjects();
                    HOperatorSet.ReadRegion(out m_HoOCRRegion, m_strPath + "\\OCRRegion.hobj");
                    m_hsmartWindow1.GetWindowHandle().SetColor("red");
                    m_hsmartWindow1.GetWindowHandle().SetDraw("margin");
                    m_hsmartWindow1.GetWindowHandle().DispObj(m_HoOCRRegion);

                    btn_DrawOCRRegion.Text = "绘制字符区域";
                    btn_DrawOCRRegion.FlatStyle = FlatStyle.Standard;

                    MessageBox.Show("绘制字符区域保存成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void GetDrawRec1Param(HDrawingObject hdrawobj, out HObject ho_Rectangle)
        {
            HTuple paramName;
            string[] paramlist = { "row1", "column1", "row2", "column2" };
            paramName = paramlist;
            HTuple param = hdrawobj.GetDrawingObjectParams(paramName);
            HOperatorSet.GenRectangle1(out ho_Rectangle, param[0], param[1], param[2], param[3]);
        }

        private void btn_CreateModel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!m_bDrawCreate)
                {
                    AddLog("请绘制创建模板区域！" + "");
                    return;
                }
                if (m_HoImage == null)
                {
                    AddLog("请读取图像！" + "");
                    return;
                }
                HObject ho_Rectangle = null, ho_ImageMedian = null;
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_ImageMedian);
                ho_Rectangle.Dispose();
                GetDrawRec1Param(m_DrawModelRec1, out ho_Rectangle);

                //创建模板前图像预处理
                //输入参数
                HTuple hv_NumLevels = new HTuple(), hv_AngleStart = new HTuple();
                HTuple hv_AngleExtent = new HTuple(), hv_AngleStep = new HTuple();
                HTuple hv_Optimization = new HTuple(), hv_Metric = new HTuple();
                HTuple hv_Contrast = new HTuple(), hv_MinContrast = new HTuple();
                HTuple hv_bCreateModel = new HTuple(); 
                hv_NumLevels = "auto"; 
                hv_AngleStart = 0; 
                hv_AngleExtent = 6.28319; 
                hv_AngleStep = "auto"; 
                hv_Optimization = "auto"; 
                hv_Metric = "use_polarity";
                hv_Contrast = m_algorithmModel.Contrast;
                hv_MinContrast = "auto";

                HOperatorSet.TupleRad(m_algorithmModel.AngleStartFind, out hv_AngleStart);
                HOperatorSet.TupleRad(m_algorithmModel.AngleExtentFind, out hv_AngleExtent);

                //输出
                ho_ImageMedian.Dispose();
                HObject ho_Contour;
                m_AlgorithmControl.CreateShapeModel(m_HoImage, ho_Rectangle, out ho_ImageMedian, hv_NumLevels,
                    hv_AngleStart, hv_AngleExtent, hv_AngleStep, hv_Optimization, hv_Metric,
                    hv_Contrast, hv_MinContrast, out m_ModelID, out m_ModelRow, out m_ModelColumn,
                    out m_ModelAngle, out m_ModelScore, out hv_bCreateModel, out ho_Contour); 
                if (hv_bCreateModel)
                {
                    AddLog("创建模板成功。" + "");
                    m_hsmartWindow1.GetWindowHandle().SetColor("red");
                    m_hsmartWindow1.GetWindowHandle().SetLineWidth(1);
                    m_hsmartWindow1.GetWindowHandle().DispObj(ho_Contour);
                    m_algorithmModel.ModelRow = m_ModelRow;
                    m_algorithmModel.ModelColumn = m_ModelColumn;
                    m_algorithmModel.ModelAngle = m_ModelAngle;

                    if (m_ModelID != null && m_ModelID.Length != 0)
                    {
                        if(!Directory.Exists(m_strPath))
                        {
                            Directory.CreateDirectory(m_strPath);
                        }
                        HOperatorSet.WriteShapeModel(m_ModelID, m_strPath + "\\Model.shm");
                    }
                }
                else
                {
                    AddLog("创建模板失败！" + "");
                }
            }
            catch (Exception ex)
            {
                AddLog("创建模板异常！" + ex.Message + "");
            }
        }

        private void btnFindModel_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_ModelID, hv_ModelRow, hv_ModelColumn, hv_ModelAngle, hv_ModelScore, hv_AngleStart, hv_AngleExtent;
                HObject ho_Contour;

                HOperatorSet.ReadShapeModel(m_strPath + "\\Model.shm", out hv_ModelID);

                HOperatorSet.TupleRad(m_algorithmModel.AngleStartFind, out hv_AngleStart);
                HOperatorSet.TupleRad(m_algorithmModel.AngleExtentFind, out hv_AngleExtent);
                HTuple hv_level = new HTuple(6).TupleConcat(new HTuple(-3));

                HOperatorSet.FindShapeModel(m_HoImage, hv_ModelID, hv_AngleStart, hv_AngleExtent, 0.5, 1, 0.5,
                    "least_squares", hv_level, 0.9, out hv_ModelRow, out hv_ModelColumn, out hv_ModelAngle,
                    out hv_ModelScore);

                AlgorithmCommHelper.dev_display_shape_matching_results(hv_ModelID, "red", hv_ModelRow, hv_ModelColumn,
                     hv_ModelAngle, 1, 1, 0, out ho_Contour);

                HOperatorSet.ClearShapeModel(hv_ModelID);

                m_hsmartWindow1.FitImageToWindow(m_HoImage, ho_Contour);
            }
            catch (Exception ex)
            {

            }
        }
         
        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="ho_Image">输入图片</param>
        /// <param name="algorithmModel">算法参数</param>
        /// <returns></returns>
        public AlgorithmResultModel AlgorithmFunc(HObject ho_Image, AlgorithmModel algorithmModel)
        {
            algorithmModel.Image = ho_Image;
            AlgorithmResultModel resultModel = m_AlgorithmControl.Run(algorithmModel, AlgorithmControlType.AlgorithmRun) as AlgorithmResultModel;
             
            return resultModel;
        }

        private void AddLog(string strLog)
        {
            logView1.LogMessage(strLog);
        }
    }
}


