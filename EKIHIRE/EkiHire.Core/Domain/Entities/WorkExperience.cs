using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.Entities
{
    public class WorkExperience : FullAuditedEntity
    {
        [DataType(DataType.Text)] public virtual string CompanyName { get; set; }
        [DataType(DataType.Text)] public virtual string Position { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual bool? TillNow { get; set; }
    }
}
