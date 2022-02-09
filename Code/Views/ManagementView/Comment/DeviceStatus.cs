using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Comment
{
    public partial class DeviceStatus : UserControl
    {
        public DeviceStatus()
        {
            InitializeComponent();
        }

        private void DeviceStatus_Load(object sender, EventArgs e)
        {

        }

        private string _DeviceName;
        public string DeviceName
        {
            get { return _DeviceName; }
            set
            {
                _DeviceName = value;
                if(!string.IsNullOrEmpty(_DeviceName))
                {
                    lblDeviceName.Text = value;
                }
            }
        }

        private EnumDeviceType _EnumDevice;
        public EnumDeviceType EnumDevice
        {
            get { return _EnumDevice; }
            set
            {
                _EnumDevice = value;
                switch (_EnumDevice)
                {
                    case EnumDeviceType.TCP:
                        lblDevice.BackgroundImage = Properties.Resources.TCP_1;
                        break;
                    case EnumDeviceType.COM:
                        lblDevice.BackgroundImage = Properties.Resources.COM_1;
                        break;
                    case EnumDeviceType.PLC:
                        lblDevice.BackgroundImage = Properties.Resources.PLC_1;
                        break;
                    case EnumDeviceType.FTP:
                        lblDevice.BackgroundImage = Properties.Resources.FTP_1;
                        break;
                    default:
                        break;
                }
            }
        }

        private bool _ConnectStatus;
        public bool ConnectStatus
        {
            get { return _ConnectStatus; }
            set
            {
                _ConnectStatus = value;
                lblDevice.BackColor = _ConnectStatus ? Color.Lime : Color.LightGray;
                lblDeviceName.BackColor = _ConnectStatus ? Color.Lime : Color.LightGray; 
            }
        }
    }

    public enum EnumDeviceType
    {
        TCP,
        COM,
        PLC,
        FTP
    }
}
