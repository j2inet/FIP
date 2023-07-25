using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class StringCapabilities:Capabilities
    {
        public static Regex CapRegex = new Regex(@"\[stringcaps=0x([0-9a-f]+) clearcommand=0x([0-9a-f]+) setcommand=0x([0-9a-f]+) maxlength=0x([0-9a-f]+) marqueetime=0x([0-9a-f]+)\]", RegexOptions.IgnoreCase);
        public int ID { get; set;  }
        public int ClearCommand { get; set;  }
        public int SetCommand { get; set;  }
        public int MaxLength { get; set;  }
        public int MarqueeTime { get; set;  }

    }
}
