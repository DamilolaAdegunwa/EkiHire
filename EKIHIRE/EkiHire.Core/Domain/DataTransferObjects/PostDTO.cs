using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class PostDTO: Post
    {
        public new ICollection<Attachment> Attachments { get; set; }
        public new User User { get; set; }
    }
}
