using EkiHire.Core.Domain.Entities.Auditing;

namespace EkiHire.Core.Domain.Entities
{
    public class LocalGovernmentArea : FullAuditedEntity<long>
    {
        public int SerialNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string SenDistrict { get; set; }
        public string SenDistrictCode { get; set; }
        public string Shape_Length { get; set; }
        public string Shape_Area { get; set; }
    }

    public class State
    {
        public string Country { get; set; }
        public string Name { get; set; }
    }

    public class LGAData
    {

        public long Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string SenateDistrict { get; set; }
        public string SenateDistrictCode { get; set; }
    }

}
