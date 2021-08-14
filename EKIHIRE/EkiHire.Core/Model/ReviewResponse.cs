using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EkiHire.Core.Domain.Entities.Enums;
namespace EkiHire.Core.Model
{
    public class ReviewResponse
    {
        #region review response
        public long? UserId { get; set; }
        //public virtual User User { get; set; }
        public long? AdId { get; set; }
        //public virtual Ad Ad { get; set; }
        public bool? Like { get; set; }
        [DataType(DataType.Text)]
        public string Review { get; set; }
        public Rating? Rating { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        #endregion
    }
}
