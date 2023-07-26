using FIP.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FIP.Win32.DirectOutput;
using FIP.Win32;

namespace FIP.Components
{
    public class Device
    {
        internal IntPtr DeviceID { get; set; }
        DeviceCapabilities Capabilities { get; set; }
        DirectOutput_Page_Callback pageCallbackRef;
        DirectOutput_SoftButton_Callback softButtonCallbackRef;
        GCHandle handle;

        List<int> pageList = new List<int>();


        public Device(IntPtr deviceID, DeviceCapabilities cap)
        {
            this.DeviceID = deviceID;
            this.Capabilities = cap;
            handle = GCHandle.Alloc(this);

            this.Capabilities.ForEach((cp) =>
            {
                if(cp.CapabilityName == typeof(ImageCapabilities).Name)
                {
                    var i = new ImageComponent(this, cp as ImageCapabilities);
                    AddComponent(i);
                }
                else if(cp.CapabilityName==typeof(SoftButtonCapabilities).Name)
                {
                    var i = new SoftButtonComponent(this, cp as SoftButtonCapabilities);
                    AddComponent(i);
                }
            });
            DirectOutput.DirectOutput_RegisterPageCallback(deviceID, pageCallbackRef = Device.DirectOutput_PageCallback, (IntPtr)handle);
            DirectOutput.DirectOutput_RegisterSoftButtonCallback(deviceID, softButtonCallbackRef = Device.DirectOutput_SoftButtonCallback, (IntPtr)handle);
        }


        

        public void DirectOutput_PageCallback(IntPtr hDevice, uint dwPage, bool IsActivated)
        {

        }

        public void DirectOutput_SoftButtonCallback( uint dwButtons)
        {
            var softButtons = this.GetDevices<SoftButtonComponent>();
            softButtons.ForEach((sb) =>
            {
                sb.IsPressed = ((dwButtons & sb.Mask) == sb.Mask);
            });
        }

        static void DirectOutput_PageCallback(IntPtr hDevice, uint dwPage, bool bActivated, IntPtr pvParam)
        {
            GCHandle handle = (GCHandle)pvParam;
            var device = handle.Target as Device;
            device.DirectOutput_PageCallback(hDevice, dwPage, bActivated);
        }

        static void DirectOutput_SoftButtonCallback(IntPtr hDevice, uint dwButtons, IntPtr pvParam)
        {
            GCHandle handle = (GCHandle)pvParam;
            var device = handle.Target as Device;
            device.DirectOutput_SoftButtonCallback(dwButtons);
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

        public int AddPage(int page, DirectOutputFlags flags)
        {
            var result = DirectOutput.DirectOutput_AddPage(DeviceID, page, (int)flags);
            if (result == (int)DirectOutputResult.S_OK)
            {
                pageList.Add(page);
            }
            return result;
        }

        public int RemovePage(int page)
        {
            var result = DirectOutput.DirectOutput_RemovePage(DeviceID, page);
            if (result == (int)DirectOutputResult.S_OK)
            {
                pageList.Remove(page);
            }
            return result;
        }
    }
}
