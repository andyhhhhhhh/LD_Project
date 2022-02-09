using BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortModel
{
    public class SerialPortResultModel : BaseResultModel
    {
        private string m_ReceiveMessage;
        public string ReceiveMessage
        {
            get { return m_ReceiveMessage; }
            set
            {
                m_ReceiveMessage = value; 
                ObjectResult = m_ReceiveMessage;
            }
        }
    }
}
