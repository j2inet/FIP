using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public  class BrightnessCapabilities:Capabilities
    {
        public static System.Text.RegularExpressions.Regex CapRegex = new System.Text.RegularExpressions.Regex(@"\[brightnesscaps=0x([0-9a-f]+) command=0x([0-9a-f]+) maxvalue=0x([0-9a-f]+)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        public int ID { get; set;  }
        public int Command { get; set;  }        
        public int MaxValue { get; set;  }
    }
}
