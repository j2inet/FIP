using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    class SoftButton: Capabilities
    {
        public static Regex CapRegex = new Regex(@"\[softbutton=(\w+) mask=(\w+)\]", RegexOptions.IgnoreCase);

        public int ID { get; set;  }
        public int Mask { get; set; }
    }
}
