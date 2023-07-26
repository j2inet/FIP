using FIP.Capabilities;
using FIP.Components;
using FIP.Win32;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static FIP.Win32.DirectOutput;

namespace FIP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        List<DeviceCapabilities> capabilityList;

        GCHandle handle;

        DirectoryInfo ContentDirectory;
        BitmapImage displayImage;
        String imagePath;

        //If we don't maintain a reference to the delegate, it gets garbage
        //collected and we get an exception when the native code calls it.        
        DirectOutput_EnumerateCallback enumerateCallbackRef;
        DirectOutput_Device_Callback deviceCallbackRef;
        
        public MainViewModel() 
        { 
            DirectOutputPath = GetDirectOutputPath();

            ContentDirectory = new DirectoryInfo(Settings.Default.ContentFolder);
            if (!ContentDirectory.Exists)
            {
                ContentDirectory.Create();
            }

            LoadContent();

            capabilityList = DeviceCapabilities.Load(DirectOutputPath).ToList();

            if(DirectOutputPath.Length != null)
            {
                var path = System.IO.Path.GetDirectoryName(DirectOutputPath);
                var resultSet = SetDllDirectory(path);
                var result = DirectOutput.DirectOutput_Initialize("FIP");
                handle = GCHandle.Alloc(this);
                DirectOutput.DirectOutput_Enumerate(enumerateCallbackRef = DirectOutput_EnumerateCallback, ((IntPtr)handle) );
                DirectOutput.DirectOutput_RegisterDeviceCallback(deviceCallbackRef = DirectOutput_DeviceCallback, (IntPtr)handle);
                
                handle.Free();


            }
        }


        void LoadContent()
        {
            var fileList = ContentDirectory.GetFiles();
            foreach(var file in fileList)
            {
                if(file.Extension == ".png" || file.Extension == ".jpg")
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(file.FullName);
                    image.EndInit();
                    displayImage = image;
                    imagePath = file.FullName;
                    break;
                }
            }
        }

        ~MainViewModel()
        {
            
        }

        String _directOutputPath;
        public String DirectOutputPath
        {
            get
            {
                return _directOutputPath;
            }
            set
            {
                SetValueIfChanged(() => DirectOutputPath, () => _directOutputPath, value);
            }
        }


        public void AddDevice(IntPtr hDevice)
        {
            DirectOutput.DirectOutput_GetDeviceType(hDevice, out Guid deviceGuid);
            
        }


        static void DirectOutput_DeviceCallback(IntPtr hDevice, bool Added, IntPtr pvParam)
        {
            GCHandle handle = (GCHandle)pvParam;
            var vm = handle.Target as MainViewModel;            
        }

        static void DirectOutput_EnumerateCallback(IntPtr hDevice, IntPtr pvParam)
        {
            GCHandle handle = (GCHandle)pvParam;
            var vm = handle.Target as MainViewModel;
            vm.EnumerateCallback(hDevice);
        }


        void EnumerateCallback(IntPtr hDevice)
        {
            DirectOutput.DirectOutput_GetDeviceType(hDevice, out Guid deviceGuid);
            var deviceCaps = capabilityList.FirstOrDefault(d => d.DeviceTypeGuid == deviceGuid);
            if(deviceCaps!= null)
            {
                var device = new Device(hDevice, deviceCaps);
                device.AddPage(1, DirectOutputFlags.IsActive);
                var imageComponent = device.GetDevices<ImageComponent>().FirstOrDefault();
                if(imageComponent != null)
                {                    
                    imageComponent.DisplayImage(displayImage, 1);
                    //imageComponent.DisplayImage(imagePath);
                }
            }
        }

        String GetDirectOutputPath()
        {
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey key = hklm.OpenSubKey(@"SOFTWARE\Saitek\DirectOutput"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("DirectOutput_Saitek");
                        if (o != null)
                        {
                            return o.ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}
