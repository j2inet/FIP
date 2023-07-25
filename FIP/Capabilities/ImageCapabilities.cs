using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class ImageCapabilities: Capabilities
    {
        public static Regex CapRegex = new Regex(@"\[imagecaps wi[dt]{2}h=(\d+) height=(\d+) bpp=(\d+)\]", RegexOptions.IgnoreCase);
        public int Width;
        public int Height;
        public int BPP;
    }
}
