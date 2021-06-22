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

namespace EkiHire.Core.Domain.Entities
{
    public class Skill: FullAuditedEntity
    {
        public string Name { get; set; }
        public static implicit operator Skill(SkillDTO model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                Skill response = new Skill
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
