﻿using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EkiHire.Core.Domain.Entities
{
    public class Report : FullAuditedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
