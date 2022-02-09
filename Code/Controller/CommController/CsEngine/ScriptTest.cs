using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EngineController.CsEngine
{
    public class CsScriptEngine
    {
        public static void ScriptRun()
        {
            try
            {
                //函数代码开始
                MessageBox.Show("Test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}