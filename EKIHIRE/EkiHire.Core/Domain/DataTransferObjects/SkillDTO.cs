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
    public class SkillDTO : EntityDTO<long>
    {
        public string Name { get; set; }
        public static implicit operator SkillDTO(Skill model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                SkillDTO response = new SkillDTO
                {
                    Name = model.Name,
                    Id = model.Id,
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
