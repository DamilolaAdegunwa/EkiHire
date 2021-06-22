﻿using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EkiHire.Core.Domain.Extensions;

namespace EkiHire.Core.Domain.Entities
{
    public class PreviousWorkExperience  : FullAuditedEntity
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? TillNow { get; set; }

        public static implicit operator PreviousWorkExperience(PreviousWorkExperienceDTO model)
        {
            try
            {
                if(model == null)
                {
                    return null;
                }
                PreviousWorkExperience response = new PreviousWorkExperience
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
