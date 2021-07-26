using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class GetAdsResponse
    {
        public List<Ad> Ads { get; set; }
        public long Total { get; set; }
        public long Pages { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
    }
    public class GetAdsAd
    {

    }
}
