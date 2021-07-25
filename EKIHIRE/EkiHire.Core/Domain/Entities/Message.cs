using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class Message
    {

        [Key]
        public long Id { get; set; }
        [Required]
        public long SenderId { get; set; }
        [Required]
        public long RecipientId { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTimeOffset When { get; set; }
        [NotMapped]
        public virtual User Sender { get; set; }
        [NotMapped]
        public virtual User Recipient { get; set; }
        public Message()
        {
            When = DateTimeOffset.Now;
        }
    }
}
