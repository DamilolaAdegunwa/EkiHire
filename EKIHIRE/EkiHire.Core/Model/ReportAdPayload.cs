using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class ReportAdPayload
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public long AdId { get; set; }
        public long Id { get; set; }
        public long? ReporterId { get; set; }
        public DateTimeOffset When { get; set; }
    }

    public class GetReportsResponse
    {
        public List<ReportAdPayload> Reports { get; set; }
        public long Total { get; set; }
        public long Pages { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
    }
}
