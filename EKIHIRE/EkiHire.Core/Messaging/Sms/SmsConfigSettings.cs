using EkiHire.Core.Configuration;

namespace EkiHire.Core.Messaging.Sms
{
    public abstract class SMSConfigSettings : ISettings
    {
        public string Sender { get; set; }
    }
}