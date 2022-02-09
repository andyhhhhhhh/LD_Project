using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using ManagementView;
using ManagementView.EditView;

namespace MyFormDesinger
{
    class ControlHelper
    {
        public static Control CreateControl(string ctrlName, string path)
        {
            try
            {
                Control ctrl = null;
                switch (ctrlName)
                {
                    case "EButton":
                        ctrl = new EButton();
                        break;
                    case "EButtonPro":
                        ctrl = new EButtonPro();
                        break;
                    case "EData":
                        ctrl = new EDataOutput();
                        break;
                    case "EHWindow":
                        ctrl = new EHWindow();
                        break;
                    case "ELblResult":
                        ctrl = new ELblResult();
                        break;
                    case "ELblStatus":
                        ctrl = new ELblStatus();
                        break;
                    case "ELog":
                        ctrl = new ELog();
                        break;
                    case "ETextBox":
                        ctrl = new ETextBox();
                        break;
                    case "ESetText":
                        ctrl = new ESetText();
                        break;
                    case "EProduct":
                        ctrl = new EProductSel();
                        break;
                    case "EItemResult":
                        ctrl = new EItemResult();
                        break;
                    case "EError":
                        ctrl = new EErrorItem();
                        break;
                    case "ECheck":
                        ctrl = new ECheck();
                        break;
                    case "ELight":
                        ctrl = new ELight();
                        break;
                    case "EGroup":
                        ctrl = new EGroupBox();
                        break;
                    case "ECombo":
                        ctrl = new ECombox();
                        break;
                    default: //其他
                        string[] strs = path.Split('/');
                        if (strs.Length == 2)
                        {
                            Assembly controlAsm = Assembly.LoadFile(strs[1]);
                            Type controlType = controlAsm.GetType(strs[0]);
                            ctrl = (Control)Activator.CreateInstance(controlType);
                        }
                        break;
                }
                return ctrl;

            }
            catch (Exception ex) //创建失败
            {
                return new Control();
            }	
        }
    }
}
