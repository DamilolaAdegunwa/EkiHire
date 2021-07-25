using EkiHire.Core.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace EkiHire.Core.Domain.Entities
{
    public class ErrorCode:Entity<long>
    {
        [DataType(DataType.Text)] public virtual string Code { get; set; }
        [DataType(DataType.Text)] public virtual string Message { get; set; }
        [DataType(DataType.Text)] public virtual string Description { get; set; }
    }
}
