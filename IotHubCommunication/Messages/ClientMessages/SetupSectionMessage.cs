using IotHubCommunication.Data;
using IotHubCommunication.Messages.Core.ClientMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotHubCommunication.Messages.ClientMessages
{
    public class SetupSectionMessage : ClientMessage
    {
        /// <summary>
        /// Section number to set.
        /// </summary>
        public SectionNumbers SectionNumber { get; set; }

        /// <summary>
        /// Pin state
        /// True = High
        /// False = Low
        /// </summary>
        public bool SectionState { get; set; }
    }
}
