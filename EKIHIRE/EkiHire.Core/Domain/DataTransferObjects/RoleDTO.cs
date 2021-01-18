using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class RoleDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public string[] Claims { get; set; }
    }
}
