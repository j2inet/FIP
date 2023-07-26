using FIP.ViewModels;
using FIP.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FIP.Win32.DirectOutput;

namespace FIP.Components
{
    public abstract class DeviceComponent
    {
        internal Device parent { get; set; }


        public DeviceComponent(Device parent)
        {
            this.parent = parent;
        }


    }
}
