using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class PassordResetDTO
    {
        public string UserName { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }


    public class ChangePassordDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ProfileImageDTO
    {
        public string profileImageString { get; set; }
    }
}
