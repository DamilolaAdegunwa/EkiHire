using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities
{
    public class Profile : FullAuditedEntity
    {
		public string ProfilePicturePath {get; set;}
		public string FirstName {get; set;}
		
    }
}
