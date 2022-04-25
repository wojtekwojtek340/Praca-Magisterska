using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Commands
{
    public class SetDigitalPin
    {
        public int PinNumber { get; set; }

        public bool PinState { get; set; }
    }
}
