using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AttachmentDTO : EntityDTO<long>
    {
        public string FilePath { get; set; }
    }
}