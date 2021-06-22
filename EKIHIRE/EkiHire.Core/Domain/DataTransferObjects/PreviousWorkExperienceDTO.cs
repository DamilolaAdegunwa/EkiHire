using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class PreviousWorkExperienceDTO : EntityDTO<long>
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? TillNow { get; set; }

        public static implicit operator PreviousWorkExperienceDTO(PreviousWorkExperience model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                PreviousWorkExperienceDTO response = new PreviousWorkExperienceDTO
                {
                    CompanyName = model.CompanyName,
                    Position = model.Position,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TillNow = model.TillNow,
                    Id = model.Id
                };
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
