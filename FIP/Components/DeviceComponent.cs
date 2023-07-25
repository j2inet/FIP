using FIP.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Components
{
    public class DeviceComponent
    {
        internal int PageCount { get; set;  } = 0;
        internal Device parent { get; set; }

        public int AddPage( int page, DirectOutputFlags flags)
        {
            var result = DirectOutput.DirectOutput_AddPage(parent.DeviceID, page, (int)flags);
            if(result == (int)DirectOutputResult.S_OK)
            {
                this.PageCount++;
            }
            return result;
        }

        public int RemovePage(int page)
        {
            var result = DirectOutput.DirectOutput_RemovePage(parent.DeviceID, page);
            if(result == (int)DirectOutputResult.S_OK)
            {
                this.PageCount--;
            }
            return result;
        }
    }
}
