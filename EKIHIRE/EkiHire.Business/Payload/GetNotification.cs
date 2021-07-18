using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Business.Payload
{
    public class GetNotificationResponse
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public NotificationType? NotificationType { get; set; }
        public long? RecipientId { get; set; }
        public string RecipientImagePath { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string RecipientUserName { get; set; }
        public bool? Delivered { get; set; }
        public bool? IsBroadCast { get; set; }
    }
}
