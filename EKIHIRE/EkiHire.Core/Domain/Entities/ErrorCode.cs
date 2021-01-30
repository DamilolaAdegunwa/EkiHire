using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Common;

namespace EkiHire.Core.Domain.Entities
{
   public class ErrorCode:Entity<long>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
