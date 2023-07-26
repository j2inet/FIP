using FIP.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.Components
{

    public class SoftButtonPressedChangedEventArgs:EventArgs
    {
        public bool Ipesed { get; internal set; }        
    }
    public delegate void SoftButtonPressedEvent(object sender, SoftButtonPressedChangedEventArgs e);    
    public class SoftButtonComponent : DeviceComponent
    {
        public int ButtonID { get; set; } = 0;
        public int Mask { get; set; } = 0;

        bool _isPressed = false;
        public bool IsPressed
        {
            get => _isPressed;
            set  {
                if(_isPressed != value)
                {
                    _isPressed= value;
                    OnSoftButtonPressedChanged(new SoftButtonPressedChangedEventArgs());

                }
            }

        }

        public event SoftButtonPressedEvent SoftButtonPressed;

        public SoftButtonComponent(Device parent, SoftButtonCapabilities capabilities):base(parent)
        {
            this.ButtonID = capabilities.ButtonID;
            this.Mask = capabilities.Mask;
        }

        public  void OnSoftButtonPressedChanged(SoftButtonPressedChangedEventArgs e)
        {
            System.Diagnostics.Debug.Write($"Status of button {ButtonID} changed to {IsPressed}.");
            SoftButtonPressed?.Invoke(this, e);
        }

    }
}
