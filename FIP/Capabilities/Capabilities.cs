using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Capabilities
{
    public class Capabilities
    {
        public Capabilities()
        {
            this.CapabilityName = this.GetType().Name;
        }   
        public String CapabilityName { get; set;  }
    }
}
