using FIP.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Components
{
    public class Device
    {
        internal IntPtr DeviceID { get; set; }
        DeviceCapabilities Capabilities { get; set; }
        public Device(IntPtr deviceID, DeviceCapabilities cap)
        {
            this.DeviceID = deviceID;
            this.Capabilities = cap;

            this.Capabilities.ForEach((cp) =>
            {
                if(cp.CapabilityName == typeof(ImageCapabilities).Name)
                {
                    var i = new ImageComponent(this, cp as ImageCapabilities);
                    AddComponent(i);
                }
            });
        }
        public List<DeviceComponent> ComponentList  { get; private set;  } = new List<DeviceComponent>();

        public List<T> GetDevices<T>()
            where T : DeviceComponent
        {
            return this.ComponentList.Where((c) => c is T).Select((c) => c as T).ToList();
        }

        public void AddComponent(DeviceComponent component)
        {
            this.ComponentList.Add(component);
        }
    }
}
