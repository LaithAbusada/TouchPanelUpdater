using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovo_TP4_Updater
{
    public class ConnectedDeviceInfo // Renamed from DeviceInfo to ConnectedDeviceInfo
    {
        public string IPAddress { get; set; }
        public string Port { get; set; }

        public override string ToString()
        {
            return $"{IPAddress}:{Port}";
        }
    }
}
