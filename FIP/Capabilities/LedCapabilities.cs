using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class LedCapabilities:Capabilities
    {
        public static Regex CapRegex = new Regex(@"\[ledcaps=0x([0-9a-f]+) command=0x([0-9a-f]+) index=0x([0-9a-f]+) maxvalue=0x([0-9a-f]+)\]", RegexOptions.IgnoreCase);
        public int ID { get; set;  }
        public int Command { get; set;  }
        public int Index { get; set;  }
        public int MaxValue { get; set;  }
    }
}
