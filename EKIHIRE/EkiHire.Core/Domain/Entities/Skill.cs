using EkiHire.Core.Domain.Entities.Auditing;
using System;

namespace EkiHire.Core.Domain.Entities
{
    public class Skill: FullAuditedEntity
    {
        public string Name { get; set; }

        //public static implicit operator Skill(SkillDTO model)
        //{
        //    try
        //    {
        //        if (model == null)
        //        {
        //            return null;
        //        }
        //        Skill response = new Skill
        //        {
        //            Name = model.Name,
        //            Id = model.Id,
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
    
}
