using IotHubCommunication.Messages.Core.ClientMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Messages.ClientMessages
{
    public class SetDigitalPin : ClientMessage
    {
        /// <summary>
        /// Digital pin number to set
        /// </summary>
        public int PinNumber { get; set; }

        /// <summary>
        /// Pin state
        /// True = High
        /// False = Low
        /// </summary>
        public bool PinState { get; set; }
    }
}
