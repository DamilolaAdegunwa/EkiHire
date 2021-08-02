using EkiHire.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Model
{
    public class AdFeedbackRequest
    {
        #region AdFeedback

        public virtual long? AdId { get; set; }
        public virtual bool? Like { get; set; }
        public virtual string Review { get; set; }
        public virtual Rating? Rating { get; set; }
        #endregion

    }
}
