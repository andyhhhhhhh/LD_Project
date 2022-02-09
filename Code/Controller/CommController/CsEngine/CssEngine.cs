using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.IO;

namespace EngineController.CsEngine
{
    public class CssEngine
    {
        private int startCodeLineCount = 0;
        public CssEngine()
        {
            Init();
        }

        /// <summary>
        /// 编译器参数
        /// </summary>
        private CompilerParameters compilerParameters = new CompilerParameters();
        /// <summary>
        /// 引用的名称列表
        /// </summary>
        public StringCollection ReferencedAssemblies
        {
            get
            {
                return compilerParameters.ReferencedAssemblies;
            }
        }

        private StringCollection csCompilerUsing = new StringCollection();
        /// <summary>
        /// 编译器使用的名称空间导入
        /// </summary>
        public StringCollection CsCompilerUsing
        {
            get
            {
                return csCompilerUsing;
            }
        }

        /// <summary>
        /// 编译脚本生成的程序集
        /// </summary>
        private System.Reflection.Assembly myAssembly = null;
        /// <summary>
        /// 所有缓存的程序集
        /// </summary>
        private static Hashtable myAssemblies = new Hashtable();

        private string strCompilerOutput = null;

        /// <summary>
        /// 编译器输出文本
        /// </summary>
        public string CompilerOutput
        {
            get
            {
                return strCompilerOutput;
            }
        }

        private MethodInfo methodInfo = null;

        /// <summary>
        /// 初始化脚本引擎
        /// </summary>
        private void Init()
        {
            this.csCompilerUsing.Add("System.Net.Sockets");
            this.csCompilerUsing.Add("System.Net");
            this.csCompilerUsing.Add("System");
            this.csCompilerUsing.Add("Microsoft.CSharp");
            this.csCompilerUsing.Add("System.Collections");
            this.csCompilerUsing.Add("System.Drawing");
            this.csCompilerUsing.Add("System.Linq");
            this.csCompilerUsing.Add("System.Windows.Forms");
            this.csCompilerUsing.Add("HalconDotNet");
            this.csCompilerUsing.Add("System.Text.RegularExpressions");
            this.CsCompilerUsing.Add("System.Threading");
            this.csCompilerUsing.Add("EngineController.CsEngine");
            this.csCompilerUsing.Add("EngineController.VBAEngine");
            this.csCompilerUsing.Add("fun = EngineController.CsEngine.ToolScriptFun");
            this.csCompilerUsing.Add("SequenceTestModel");

            this.compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            this.compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            this.compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            this.compilerParameters.ReferencedAssemblies.Add("halcondotnet.dll"); 
            this.compilerParameters.ReferencedAssemblies.Add("EngineController.dll");
            this.compilerParameters.ReferencedAssemblies.Add("SequenceTestModel.dll");


            string strPath = Application.StartupPath + "//ScriptDll";
            List<string> listFile =  Directory.GetFiles(strPath).ToList().FindAll(x => x.Contains(".dll"));
            foreach (var item in listFile)
            {
                string fileName = Path.GetFileName(item);
                this.compilerParameters.ReferencedAssemblies.Add(string.Format("ScriptDll//{0}", fileName));
            }
        }

        public void AddUsing(string name)
        {
            this.csCompilerUsing.Add(name);
        }

        public void AddDll(string name)
        {
            this.compilerParameters.ReferencedAssemblies.Add(name);
        }
        
        /// <summary>
        /// 安全的简单的执行脚本方法
        /// </summary>
        public bool ExecuteSub(object[] param)
        {
           return Execute(param, false);
        }

        /// <summary>
        /// 执行脚本方法
        /// </summary>
        /// <param name="Parameters">参数</param>
        /// <param name="ThrowException">若发生错误是否触发异常</param>
        /// <returns>执行结果</returns>
        public bool Execute(object[] Parameters, bool ThrowException)
        {
            // 若发生错误则不抛出异常，安静的退出
            try
            {
                // 执行脚本方法
                object result = methodInfo.Invoke(null, Parameters);
                // 返回脚本方法返回值
                return (bool)result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 编译脚本
        /// </summary>
        /// <returns>编译是否成功</returns>
        public bool Compile(Script2Model model)
        {
            string s = GenScript(model);

            //MessageBox.Show(s);
            myAssembly = (System.Reflection.Assembly)myAssemblies[s];
            if (myAssembly == null)
            {
                // 设置编译参数
                this.compilerParameters.GenerateExecutable = false;
                this.compilerParameters.GenerateInMemory = true;
                this.compilerParameters.IncludeDebugInformation = false;
                this.compilerParameters.OutputAssembly = model.ModelName;

                //提供对C#代码生成器和代码编译器的实例的访问
                CSharpCodeProvider provider = new CSharpCodeProvider();
                // 这段代码用于微软.NET2.0或更高版本
                CompilerResults result = provider.CompileAssemblyFromSource(this.compilerParameters, s);

                // 获得编译器控制台输出文本
                System.Text.StringBuilder myOutput = new System.Text.StringBuilder();
                foreach (string line in result.Output)
                {
                    myOutput.Append("\r\n" + line);
                }
                this.strCompilerOutput = myOutput.ToString();
                // 输出编译结果
                if (this.strCompilerOutput.Length > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("VBAScript Compile result" + strCompilerOutput);
                    //////////////////////
                    //string strErrorMsg = "检测出 " + result.Errors.Count.ToString() + " 个错误:";
                    string strErrorMsg = string.Empty;
                    int count = 0;
                    for (int x = 0; x < result.Errors.Count; x++)
                    {
                        if (result.Errors[x].IsWarning)
                            continue;
                        strErrorMsg = strErrorMsg + Environment.NewLine +
                                    "第 " + (result.Errors[x].Line - (startCodeLineCount - 1)).ToString() + " 行:  " +
                                     result.Errors[x].ErrorText;
                        count++;
                    }
                    strErrorMsg = "检测出 " + count.ToString() + " 个错误:" + strErrorMsg;
                    strCompilerOutput = strErrorMsg;
                }
                provider.Dispose();
                if (result.Errors.HasErrors == false)
                {
                    // 若没有发生编译错误则获得编译所得的程序集
                    this.myAssembly = result.CompiledAssembly;
                }
                if (myAssembly != null)
                {
                    // 将程序集缓存到程序集缓存区中
                    myAssemblies[s] = myAssembly;
                }
            }

            if (this.myAssembly != null)
            {
                // 检索脚本中定义的类型
                Type ModuleType = myAssembly.GetType(model.NameSpace + "." + model.ModelName);
                if (ModuleType != null)
                {
                    //System.Reflection.MethodInfo[] ms = ModuleType.GetMethods( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic| System.Reflection.BindingFlags.Static);
                    methodInfo = ModuleType.GetMethod("ScriptRun");
                    if (methodInfo != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 生成脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private string GenScript(Script2Model model)
        {
            System.Text.StringBuilder mySource = new System.Text.StringBuilder();
            foreach (string import in this.csCompilerUsing)
            {
                //添加引用
                mySource.Append("\r\nusing " + import + ";");
            }
            //添加命名空间
            mySource.Append("\r\nnamespace " + model.NameSpace);
            mySource.Append("\r\n{");
            mySource.Append("\r\npublic class " + model.ModelName);
            mySource.Append("\r\n   {");
            mySource.Append("\r\n    public static bool ScriptRun(" + model.FunParameters + ")");
            mySource.Append("\r\n       {");
            mySource.Append("\r\n             try");
            mySource.Append("\r\n             {");
            mySource.Append("\r\n              //函数代码开始");
            string datTmp = mySource.ToString();
            startCodeLineCount = datTmp.Split('\r').Length + 1;
            mySource.Append("\r\n              " + model.ScriptText);
            mySource.Append("\r\n             return true;");
            mySource.Append("\r\n             }");
            mySource.Append("\r\n             catch (Exception ex)");
            mySource.Append("\r\n             {");
            mySource.Append("\r\n             fun.ShowLog(\"ScriptRun Error \" + ex.Message, true);");
            mySource.Append("\r\n             return false;");
            mySource.Append("\r\n             }");
            mySource.Append("\r\n       }");
            mySource.Append("\r\n   }");
            mySource.Append("\r\n}");
            return mySource.ToString();
        }

    }

    public class Script2Model
    {
        public string ScriptText { get; set; }
        public string NameSpace { get; set; }
        public string ModelName { get; set; }
        public string FunParameters { get; set; }

        // public object[] Parameters { get; set; }
    }

}
