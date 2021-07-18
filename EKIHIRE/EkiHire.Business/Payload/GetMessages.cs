using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Business.Payload
{
    public class GetMessagesResponse
    {

        public long MessageId { get; set; }

        public long SenderId { get; set; }
        public string SenderImagePath { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderUserName { get; set; }

        public long RecipientId { get; set; }
        public string RecipientImagePath { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientUserName { get; set; }

        public string Text { get; set; }
        public DateTimeOffset When { get; set; }
    }
}
