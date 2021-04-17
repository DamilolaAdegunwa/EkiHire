using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.Entities
{
    public class CartItem: FullAuditedEntity
    {
        public Ad Ad { get; set; }
    }
}
