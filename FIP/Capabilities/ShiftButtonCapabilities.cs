using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public  class ShiftButtonCapabilities:Capabilities
    {
        public static System.Text.RegularExpressions.Regex CapRegex = new System.Text.RegularExpressions.Regex(@"\[shiftbutton=0x([0-9a-f]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        public int ID { get; set;  }

    }
}
