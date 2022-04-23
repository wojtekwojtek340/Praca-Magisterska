using IotHubCommunication.Messages.Core.ClientMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Messages.ClientMessages
{
    internal class EnableSection : ClientMessage
    {
        public int SectionNumber { get; set; }
    }
}
