using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class ContactUsRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class ContactUsResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
        public DateTimeOffset When { get; set; }

    }

    public class ContactUsResponseList
    {
        public List<ContactUsResponse> ContactUsDetails { get; set; }
        public long Total { get; set; }
        public long Pages { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
    }
}
