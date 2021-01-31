using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class CommentDTO: Comment
    {
        public UserDTO User { get; set; }
    }
}
