using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Win32
{
    enum DirectOutputResult:int
    {
        S_OK = 0,
        E_PageNotActive = unchecked((int)0xFF040001),
        E_InvalidArgument = unchecked((int)0x80070057),
        E_NotImplemented = unchecked((int)0x80004001),
        E_OutOFmemory = unchecked((int)0x8007000E),
        E_Handle = unchecked((int)0x80070006L),
    }

    public enum DirectOutputFlags:int
    {
        IsActive = 1,
    }

    public static class DirectOutput
    {
        public delegate void DirectOutput_EnumerateCallback(IntPtr hDevice, IntPtr pvParam);
        public delegate void DirectOutput_Device_Callback(IntPtr hDevice, bool Added, IntPtr pvParam);
        public delegate void DirectOutput_Page_Callback(IntPtr hDevice, uint dwPage, bool Added, IntPtr pvParam);
        public delegate void DirectOutput_SoftButton_Callback(IntPtr hDevice, uint dwButtons, IntPtr pvParam);


        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_AddPage(IntPtr hDevice, int dwPage, int dwFlags);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_RemovePage(IntPtr hDevice, int dwPage);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_Initialize(String wszAppName);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_Deinitialize();

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern void DirectOutput_Enumerate(DirectOutput_EnumerateCallback pfnCallback, IntPtr pvParam);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_GetDeviceType(IntPtr hDevice, out Guid pGuid);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_RegisterPageCallback(IntPtr hDevice, DirectOutput_Page_Callback pfnCallback, IntPtr pvParam);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_RegisterDeviceCallback(DirectOutput_Device_Callback pfnCallback, IntPtr pvParam);

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_RegisterSoftButtonCallback(IntPtr hDevice, DirectOutput_SoftButton_Callback pfnCallback, IntPtr pvParam);


        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]

        ///This call reads all the data from the file and sends this directly to the device, so the format of the data is 
        ///device specific. Refer to the device data sheet for more details on the image data format.
        ///Unless specified by the device data sheet you cannot use this function to directly send image files to the device.
        private static extern int DirectOutput_SetImageFromFile(IntPtr hDevice, int dwPage, int dwIndex, int cchFilename, String wszFilename);
        public static int DirectOutput_SetImageFromFile(IntPtr hDevice, int dwPage, int dwIndex, String fileName)
        {
            return DirectOutput_SetImageFromFile(hDevice, dwPage, dwIndex, fileName.Length, fileName);
        }

        [DllImport("DirectOutput.dll", CharSet = CharSet.Unicode)]
        public static extern int DirectOutput_SetImage(IntPtr hDevice, int dwPage, int dwIndex, int cbValue,  IntPtr pBits);

        public static int DirectOutput_SetImage(IntPtr hDevice, int dwPage, int dwIndex, int cbValue, byte[] pBits)
        {
            IntPtr ptr = Marshal.AllocHGlobal(pBits.Length);
            Marshal.Copy(pBits, 0, ptr, pBits.Length);
            int result = DirectOutput_SetImage(hDevice, dwPage, dwIndex, cbValue, ptr);
            Marshal.FreeHGlobal(ptr);
            return result;
        }
    }
}
