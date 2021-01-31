using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
