using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class PostDTO: Post
    {
        public new ICollection<AttachmentDTO> Attachments { get; set; }
        public new UserDTO User { get; set; }
    }
}
