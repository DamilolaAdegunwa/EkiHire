using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class AccountDTO : UserDTO
    {
        public string RoleName { get; set; }
        public long RoleId { get; set; }
    }
}
