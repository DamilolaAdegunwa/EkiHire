using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities.Enums
{
    public enum AdsStatus
    {
        ACTIVE = 0,
        INREVIEW = 1,
        DECLINED = 2
    }
    public enum AdClass
    {
        Normal = 0,
        Pro = 1,
        Premium = 2
    }
    public enum Rating
    {
        BAD = 1,
        POOR = 2,
        FAIR = 3,
        GOOD = 4,
        EXCELLENT = 5
    }
}
