using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class SoftButtonCapabilities: Capabilities
    {
        public static Regex CapRegex = new Regex(@"\[softbutton=(\w+) mask=(\w+)\]", RegexOptions.IgnoreCase);

        public int ButtonID { get; set;  }
        public int Mask { get; set; }
    }
}
