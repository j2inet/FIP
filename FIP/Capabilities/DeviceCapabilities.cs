using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class DeviceCapabilities: List<Capabilities>
    {
        public Guid DeviceTypeGuid { get; set; }

        public static IEnumerable<DeviceCapabilities> Load(String parentFolder)
        {
            var guidRegex = new Regex(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?.dat$");
            var parentDirectory = (new FileInfo(parentFolder)).Directory;
            var files = parentDirectory.GetFiles("*.dat", SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                if(!guidRegex.IsMatch(f.Name))
                {
                    continue;
                }
                var capabilities = new DeviceCapabilities();
                string IDString = f.Name.Split(new char[] { '.' })[0];
                capabilities.DeviceTypeGuid = Guid.Parse(IDString);
                var capString = File.ReadAllText(f.FullName);
                var capPartList = capString.Split(new char[]{'\r','\n' }).Where(x=>!String.IsNullOrEmpty(x));
                foreach (var capPart in capPartList)
                {
                    
                    var imageMatch = ImageCapabilities.CapRegex.Match(capPart);
                    if(imageMatch.Success)
                    {
                        var image = new ImageCapabilities();
                        image.Width = int.Parse(imageMatch.Groups[1].Value);
                        image.Height = int.Parse(imageMatch.Groups[2].Value);
                        image.BPP = int.Parse(imageMatch.Groups[3].Value);
                        capabilities.Add(image);
                        continue;
                    }

                    var softButtonMatch = SoftButtonCapabilities.CapRegex.Match(capPart);
                    if(softButtonMatch.Success)
                    {
                        var softButton = new SoftButtonCapabilities();
                        softButton.ButtonID = Convert.ToInt32(softButtonMatch.Groups[1].Value, 16);
                        softButton.Mask = Convert.ToInt32(softButtonMatch.Groups[2].Value, 16);
                        capabilities.Add(softButton);
                        continue;
                    }

                    var ledMatch = LedCapabilities.CapRegex.Match(capPart);
                    if(ledMatch.Success)
                    {
                        var led = new LedCapabilities();
                        led.ID = Convert.ToInt32(ledMatch.Groups[1].Value, 16);
                        led.Command = Convert.ToInt32(ledMatch.Groups[2].Value, 16);
                        led.Index = Convert.ToInt32(ledMatch.Groups[3].Value, 16);
                        led.MaxValue = Convert.ToInt32(ledMatch.Groups[4].Value, 16);
                        capabilities.Add(led);
                        continue;
                    }
                    var stringMatch = StringCapabilities.CapRegex.Match(capPart);
                    if(stringMatch.Success)
                    {
                      var str = new StringCapabilities();
                        str.ID = Convert.ToInt32(stringMatch.Groups[1].Value, 16);
                        str.ClearCommand = Convert.ToInt32(stringMatch.Groups[2].Value, 16);
                        str.SetCommand = Convert.ToInt32(stringMatch.Groups[3].Value, 16);
                        str.MaxLength = Convert.ToInt32(stringMatch.Groups[4].Value, 16);
                        str.MarqueeTime = Convert.ToInt32(stringMatch.Groups[5].Value, 16);
                        capabilities.Add(str);
                        continue;
                    }

                    var brightnessMatch = BrightnessCapabilities.CapRegex.Match(capPart);
                    if(brightnessMatch.Success)
                    {
                        var brightness = new BrightnessCapabilities();
                        brightness.ID = Convert.ToInt32(brightnessMatch.Groups[1].Value, 16);
                        brightness.Command = Convert.ToInt32(brightnessMatch.Groups[2].Value, 16);
                        brightness.MaxValue = Convert.ToInt32(brightnessMatch.Groups[3].Value, 16);
                        capabilities.Add(brightness);
                        continue;
                    }

                    var shiftMatch = ShiftButtonCapabilities.CapRegex.Match(capPart);
                    if(shiftMatch.Success)
                    {
                        var shift = new ShiftButtonCapabilities();
                        shift.ID = Convert.ToInt32(shiftMatch.Groups[1].Value, 16);
                        capabilities.Add(shift);
                        continue;
                    }
                }


                yield return capabilities;
            }
        }
    }
}
