
using EkiHire.Core.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace EkiHire.Core.Messaging.Email
{
    public class SmtpConfig : ISettings
    {
        public bool EnableSSl { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string From { get; set; }
        public string SenderDisplayName { get; set; }
        public bool UseDefaultCredentials { get; set; }
        //
        public string Host { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public ICredentialsByHost Credentials { get; set; }
        public SmtpDeliveryFormat DeliveryFormat { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public string PickupDirectoryLocation { get; set; }
        public ServicePoint ServicePoint { get; }
        public string TargetName { get; set; }
        public int Timeout { get; set; }
    }
}