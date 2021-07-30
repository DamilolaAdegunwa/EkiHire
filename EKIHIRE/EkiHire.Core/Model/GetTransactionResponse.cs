using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class GetTransactionResponse
    {
        public List<Transaction> Transactions { get; set; }
        public long Total { get; set; }
        public long Pages { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
    }
}
