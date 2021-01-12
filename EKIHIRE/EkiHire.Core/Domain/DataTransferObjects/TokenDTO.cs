using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
