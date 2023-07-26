using FIP.Capabilities;
using FIP.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FIP.Components
{
    public class ImageComponent: DeviceComponent
    {
        

        public ImageComponent(Device parent, ImageCapabilities capabilities): base(parent)
        {
            this.Capabilities= capabilities;
        }
        
        ImageCapabilities Capabilities;

        public void DisplayImage(String filePath, int page)
        {
            var result = DirectOutput.DirectOutput_SetImageFromFile(parent.DeviceID, page, 0, filePath);
        }
        public void DisplayImage(BitmapImage image , int page)
        {
            try
            {
                var scaledImage = new TransformedBitmap(image, new System.Windows.Media.ScaleTransform(Capabilities.Width / image.Width, -Capabilities.Height / image.Height));
                var sampleImage = new byte[4 * Capabilities.Width * Capabilities.Height];
                var adjustedImage = new byte[3*Capabilities.Width * Capabilities.Height];
                scaledImage.CopyPixels(sampleImage, 4 * Capabilities.Width, 0);
                for(var i=0;i<Capabilities.Width * Capabilities.Height;i++)
                {
                    adjustedImage[i * 3] = sampleImage[i * 4];
                    adjustedImage[i * 3 + 1] = sampleImage[i * 4 + 1];
                    adjustedImage[i * 3 + 2] = sampleImage[i * 4 + 2];
                }
                DirectOutput.DirectOutput_SetImage(parent.DeviceID, page, 0, 3*Capabilities.Width* Capabilities.Height, adjustedImage);
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

    }
}
