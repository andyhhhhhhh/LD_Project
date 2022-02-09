using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;
using System.Reflection;

namespace EngineController.CsEngine
{
    public partial class CsEditView : UserControl
    {
        private ScintillaNET.Scintilla scEdit;
        private const string key0 = "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield ToString HObject HOperatorSet HTuple";
        private const string key1 = "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET List VisionToolBase VsToolGroup OperateResult IsOk IsSuccess Content GroupInfo ToolType Explains Engine Sen ";

        private string key2 = ""; 

        private string funKey = string.Empty;
        private string halKey = string.Empty;

        public Dictionary<string, string> CsMethodNameDic = new Dictionary<string, string>();

        private FindReplace MyFindReplace;
        public CsEditView()
        {
            InitializeComponent();
            // CREATE CONTROL
            scEdit = new Scintilla();
            // INITIAL VIEW CONFIG
            scEdit.WrapMode = WrapMode.None;
            scEdit.IndentationGuides = IndentView.LookBoth;
            scEdit.ViewWhitespace = WhitespaceMode.VisibleAlways;

            funKey = GetScriptFunString();
            halKey = GetHALCONString();

            key2 = key1 + GetFunNameString();
            GetScriptDllString();

            Init();
            this.Controls.Add(scEdit);

            // BASIC CONFIG
            scEdit.Dock = DockStyle.Fill; 

            //查找替换
            MyFindReplace = new FindReplace();
            MyFindReplace.Scintilla = scEdit;
            MyFindReplace.KeyPressed += MyFindReplace_KeyPressed;
            //incrementalSearcher1.FindReplace = MyFindReplace;

        }

        private string getFunName(string name)
        {
            int index1 = name.IndexOf(' ');
            int index2 = name.IndexOf('(');
            return name.Substring(index1 + 1, index2 - index1 - 1);
        }

        public string GetFunNameString()
        {

            List<string> strList = new List<string>();

            foreach (Type t in Assembly.LoadFrom(Application.StartupPath + "/EngineController.dll").GetTypes())
            {
                //只添加指定命名空间的类
                if (t.FullName.Contains("ToolScriptFun"))
                {
                    MethodInfo[] methods = t.GetMethods();
                    foreach (MethodInfo m in methods)
                    {
                        if (m.IsStatic && m.IsPublic)
                        {
                            string name = m.ToString();
                            strList.Add(getFunName(name));
                        }
                    }
                }
            }

            strList.Add("fun");
            strList = strList.Distinct<string>().ToList();
            StringBuilder str = new StringBuilder();
            foreach (var item in strList)
            {
                str.Append(item);
                str.Append(" ");
            }

            return str.ToString().Trim();
        } 

        public string GetScriptFunString()
        {
            List<string> strList = new List<string>();
            //获取当前程序集的所有类
            foreach (Type t in Assembly.LoadFrom(Application.StartupPath + "/EngineController.dll").GetTypes())
            {
                if (t.FullName.Contains("ToolScriptFun"))
                {
                    MethodInfo[] methods = t.GetMethods();
                    foreach (MethodInfo m in methods)
                    {
                        if (m.IsStatic && m.IsPublic)
                        {
                            foreach (Attribute attr in Attribute.GetCustomAttributes(m))
                            {
                                // Check for the AnimalType attribute.
                                if (attr.GetType() == typeof(ScriptAttribute))
                                {
                                    ScriptAttribute sa = (ScriptAttribute)attr;
                                    string funName = sa.Name + "(" + sa.Parameter + ");" + "//" + sa.Explains;
                                    strList.Add(funName);
                                }
                            }
                        }
                    }
                }
            }

            strList.Sort();
            //strList = strList.Distinct<string>().ToList();
            StringBuilder str = new StringBuilder();
            foreach (var item in strList)
            {
                str.Append(item);
                str.Append(" ");
            }
            return str.ToString().Trim();
        } 

        public string GetHALCONString()
        {
            List<string> strList = new List<string>(); 
            //获取当前程序集的所有类
            foreach (Type t in Assembly.LoadFrom(Application.StartupPath + "/halcondotnet.dll").GetTypes())
            {
                if (t.FullName.Contains("HOperatorSet"))
                {
                    MethodInfo[] methods = t.GetMethods();
                    foreach (MethodInfo m in methods)
                    { 
                        if (!m.ToString().Contains("<"))
                        {
                            strList.Add(m.ToString().Substring(m.ToString().IndexOf(" ") + 1).Replace(" ", "").Replace("HalconDotNet.", "") + ";");
                        }
                    }
                } 
            }

            strList.Sort();
            //strList = strList.Distinct<string>().ToList();
            StringBuilder str = new StringBuilder();
            foreach (var item in strList)
            {
                str.Append(item);
                str.Append(" ");
            } 

            return str.ToString().Trim();
        }

        public void GetScriptDllString()
        {
            List<string> strList = new List<string>();
            List<string> key3List = new List<string>();

            string strPath = Application.StartupPath + "//ScriptDll";
            List<string> listFile = Directory.GetFiles(strPath).ToList().FindAll(x => x.Contains(".dll"));
            foreach (var file in listFile)
            {
                //获取当前程序集的所有类
                string nameSpaceName = "";
                List<string> listClass = new List<string>();
                foreach (Type t in Assembly.LoadFrom(file).GetTypes())
                {
                    bool bAdd = false;
                    MethodInfo[] methods = t.GetMethods();
                    foreach (MethodInfo m in methods)
                    {
                        if (m.IsStatic && m.IsPublic && !m.ToString().Contains("<"))
                        {
                            nameSpaceName = t.FullName.Trim().Substring(0, t.FullName.IndexOf('.'));

                            string className = t.FullName.Trim().Substring(t.FullName.IndexOf('.') + 1);
                            if(!listClass.Contains(className))
                            {
                                listClass.Add(className);
                                strList = new List<string>();
                            } 

                            strList.Add(m.ToString().Substring(m.ToString().IndexOf(" ") + 1).Replace(" ", "").Replace("System.", "") + ";");

                            key3List.Add(getFunName(m.ToString()));
                            bAdd = true;
                        }
                    }

                    //把类中的方法先加进去
                    if(bAdd)
                    {
                        strList.Sort();
                        StringBuilder str = new StringBuilder();
                        foreach (var item in strList)
                        {
                            str.Append(item);
                            str.Append(" ");
                        } 
                        CsMethodNameDic.Add(listClass.Last(), str.ToString().Trim());
                    } 
                }

                //把所有的类加进去
                if(listClass.Count > 0)
                {
                    listClass.Sort();
                    StringBuilder str2 = new StringBuilder();
                    foreach (var item in listClass)
                    {
                        str2.Append(item);
                        str2.Append(" ");
                    }
                    CsMethodNameDic.Add(nameSpaceName, str2.ToString().Trim());
                }

                //把函数名加进去
                if(key3List.Count > 0)
                {
                    key3List.Sort();
                    StringBuilder str3 = new StringBuilder();
                    str3.Append(" ");
                    foreach (var item in key3List)
                    {
                        str3.Append(item);
                        str3.Append(" ");

                        key2 += str3;
                    }                    
                }
            }
        }

        private void Init()
        {
            scEdit.TextChanged += (this.OnTextChanged);
            scEdit.CharAdded += TextArea_CharAdded;

            // STYLING
            InitColors();
            InitSyntaxColoring();

            // NUMBER MARGIN
            InitNumberMargin();

            // BOOKMARK MARGIN
            InitBookmarkMargin();

            // CODE FOLDING MARGIN
            InitCodeFolding();

            // DRAG DROP
            InitDragDropFile();
        }

        private void CsEditView_Load(object sender, EventArgs e)
        {
            // INIT HOTKEYS
            InitHotkeys(this.ParentForm);
        }

        public void SetKeyword(string word)
        {
            if (word != null && word.Length > 0)
            {
                key2 = key1 + word;
                //scEdit.SetKeywords(1, key2);
                //设置自动完成字符串
            }
        }

        private string methodKey = string.Empty;
        private void TextArea_CharAdded(object sender, CharAddedEventArgs e)
        {
            var currentPos = scEdit.CurrentPosition;
            var wordStartPos = scEdit.WordStartPosition(currentPos, true);
            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0)
            {
                methodKey = scEdit.Text.Substring(wordStartPos, lenEntered);
            }
            else
            {
                //在代码中间新增的时候也需要
                if (e.Char == '.')
                {
                    string lineText = scEdit.Lines[scEdit.CurrentLine].Text.Trim('\n').Trim('\r').TrimEnd('.');

                    if (lineText.LastIndexOf('.') != -1)
                    {
                        methodKey = lineText.Substring(lineText.LastIndexOf('.') + 1);
                    }
                    else
                    {
                        methodKey = lineText.Substring(0);
                    }
                }
            }

            if (e.Char == '.' && methodKey.Contains("fun"))
            {
                if (!scEdit.AutoCActive)
                {
                    scEdit.AutoCShow(lenEntered, funKey);
                }
            }
            else if (e.Char == '.' && methodKey.Contains("HOperatorSet"))
            {
                if (!scEdit.AutoCActive)
                {
                    scEdit.AutoCShow(lenEntered, halKey);
                }
            }
            else if(e.Char == '.' && CsMethodNameDic.ContainsKey(methodKey))
            {
                if (!scEdit.AutoCActive)
                {
                    scEdit.AutoCShow(lenEntered, CsMethodNameDic[methodKey]);
                }
            }
        }

        private void InitColors()
        {

            scEdit.SetSelectionBackColor(true, IntToColor(0x114D9C));
            scEdit.CaretForeColor = IntToColor(0xF8F8FF);
        }

        private void InitHotkeys(Form form)
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(form, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(form, OpenFindDialog, Keys.F, true, false, true);
            HotKeyManager.AddHotKey(form, OpenFindDialog, Keys.R, true);
            HotKeyManager.AddHotKey(form, OpenReplaceDialog, Keys.H, true);
            HotKeyManager.AddHotKey(form, Uppercase, Keys.U, true);
            HotKeyManager.AddHotKey(form, Lowercase, Keys.L, true);
            HotKeyManager.AddHotKey(form, ZoomIn, Keys.Oemplus, true);
            HotKeyManager.AddHotKey(form, ZoomOut, Keys.OemMinus, true);
            HotKeyManager.AddHotKey(form, ZoomDefault, Keys.D0, true);
            HotKeyManager.AddHotKey(form, CloseSearch, Keys.Escape);
            HotKeyManager.AddHotKey(form, OpenGoToDialog, Keys.G, true);

            // remove conflicting hotkeys from scintilla
            scEdit.ClearCmdKey(Keys.Control | Keys.F);
            scEdit.ClearCmdKey(Keys.Control | Keys.R);
            scEdit.ClearCmdKey(Keys.Control | Keys.H);
            scEdit.ClearCmdKey(Keys.Control | Keys.L);
            scEdit.ClearCmdKey(Keys.Control | Keys.U);
            scEdit.ClearCmdKey(Keys.Control | Keys.G); 
        }

        private void InitSyntaxColoring()
        {
            // Configure the default style
            scEdit.StyleResetDefault();
            scEdit.Styles[Style.Default].Font = "Consolas";
            scEdit.Styles[Style.Default].Size = 10;
            scEdit.Styles[Style.Default].BackColor = IntToColor(0x212121);
            scEdit.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            scEdit.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            scEdit.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            scEdit.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            scEdit.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            scEdit.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            scEdit.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            scEdit.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            scEdit.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            scEdit.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            scEdit.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            scEdit.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            scEdit.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            scEdit.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            scEdit.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            scEdit.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            scEdit.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            scEdit.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            scEdit.Lexer = Lexer.Cpp;

            scEdit.SetKeywords(0, key0);
            scEdit.SetKeywords(1, key2); 
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
        }

        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void InitNumberMargin()
        {

            scEdit.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            scEdit.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            scEdit.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            scEdit.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = scEdit.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            scEdit.MarginClick += TextArea_MarginClick;
        }

        private void InitBookmarkMargin()
        {

            //TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

            var margin = scEdit.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = scEdit.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(IntToColor(0xFF003B));
            marker.SetForeColor(IntToColor(0x000000));
            marker.SetAlpha(100);

        }

        private void InitCodeFolding()
        {

            scEdit.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            scEdit.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            scEdit.SetProperty("fold", "1");
            scEdit.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scEdit.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            scEdit.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            scEdit.Margins[FOLDING_MARGIN].Sensitive = true;
            scEdit.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scEdit.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                scEdit.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            scEdit.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            scEdit.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            scEdit.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            scEdit.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scEdit.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            scEdit.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scEdit.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scEdit.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                var line = scEdit.Lines[scEdit.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }

        #endregion

        #region Drag & Drop File

        public void InitDragDropFile()
        {

            scEdit.AllowDrop = true;
            scEdit.DragEnter += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            scEdit.DragDrop += delegate (object sender, DragEventArgs e)
            {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        string path = a.GetValue(0).ToString();

                        LoadDataFromFile(path);

                    }
                }
            };

        }

        private void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                scEdit.Text = File.ReadAllText(path);
            }
        }

        #endregion

        #region Uppercase / Lowercase

        private void Lowercase()
        {

            // save the selection
            int start = scEdit.SelectionStart;
            int end = scEdit.SelectionEnd;

            // modify the selected text
            scEdit.ReplaceSelection(scEdit.GetTextRange(start, end - start).ToLower());

            // preserve the original selection
            scEdit.SetSelection(start, end);
        }

        private void Uppercase()
        {

            // save the selection
            int start = scEdit.SelectionStart;
            int end = scEdit.SelectionEnd;

            // modify the selected text
            scEdit.ReplaceSelection(scEdit.GetTextRange(start, end - start).ToUpper());

            // preserve the original selection
            scEdit.SetSelection(start, end);
        }

        #endregion

        #region Indent / Outdent

        private void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        private void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        private void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            scEdit.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

        #endregion

        #region Zoom

        private void ZoomIn()
        {
            scEdit.ZoomIn();
        }

        private void ZoomOut()
        {
            scEdit.ZoomOut();
        }

        private void ZoomDefault()
        {
            scEdit.Zoom = 0;
        }


        #endregion

        #region Quick Search Bar

        bool SearchIsOpen = false;

        public Scintilla ScEdit
        {
            get
            {
                return scEdit;
            }
        }
        public string ScriptText
        {
            get
            {
                return scEdit.Text;
            }
            set
            {
                scEdit.Text = value;
            }
        }
        
        /// <summary>
        /// 获取控件中光标位置
        /// </summary>
        public int SelectStart
        {
            get
            {
                return scEdit.SelectionStart;
            }
        }
        private void OpenSearch()
        {

            SearchManager.SearchBox = TxtSearch;
            SearchManager.TextArea = scEdit;

            if (!SearchIsOpen)
            {
                SearchIsOpen = true;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = true;
                    TxtSearch.Text = SearchManager.LastSearch;
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate ()
                {
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
        }
        private void CloseSearch()
        {
            if (SearchIsOpen)
            {
                SearchIsOpen = false;
                InvokeIfNeeded(delegate ()
                {
                    PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            CloseSearch();
        }

        private void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(false, false);
        }
        private void BtnNextSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(true, false);
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true, true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true, false);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false, false);
            }
        }

        #endregion

        #region Find & Replace Dialog

        private void OpenFindDialog()
        {
            MyFindReplace.ShowFind();
        }

        private void OpenReplaceDialog()
        {
            MyFindReplace.ShowReplace();
        }

        private void OpenGoToDialog()
        { 
            GoTo MyGoTo = new GoTo(scEdit);
            MyGoTo.ShowGoToDialog();
        }

        private void MyFindReplace_KeyPressed(object sender, KeyEventArgs e)
        {
            genericScintilla_KeyDown(sender, e);
        }
         
        /// <summary>
		/// Key down event for each Scintilla. Tie each Scintilla to this event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void genericScintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.F)
            {
                MyFindReplace.ShowFind();
                e.SuppressKeyPress = true;
            }
            else if (e.Shift && e.KeyCode == Keys.F3)
            {
                MyFindReplace.Window.FindPrevious();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                MyFindReplace.Window.FindNext();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.H)
            {
                MyFindReplace.ShowReplace();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                MyFindReplace.ShowIncrementalSearch();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                GoTo MyGoTo = new GoTo((Scintilla)sender);
                MyGoTo.ShowGoToDialog();
                e.SuppressKeyPress = true;
            }
        }

        #endregion

        #region Utils

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        public void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
        
        #endregion
      
    }
}
