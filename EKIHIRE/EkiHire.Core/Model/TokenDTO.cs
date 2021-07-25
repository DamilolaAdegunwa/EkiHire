using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Model
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
