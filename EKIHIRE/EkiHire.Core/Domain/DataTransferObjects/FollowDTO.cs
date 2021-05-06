using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities.Auditing;
namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class FollowDTO : EntityDTO<long>
    {
        #region follow
        //[ForeignKey("FollowerId")]
        public virtual long? FollowerId { get; set; }
        public virtual User Follower { get; set; }
        //[ForeignKey("FollowingId")]
        public virtual long? FollowingId { get; set; }
        public virtual User Following { get; set; }
        #endregion

        public static implicit operator FollowDTO(Follow model)
        {
            try
            {
                if (model != null)
                {
                    FollowDTO response = new FollowDTO
                    {
                        FollowerId = model.FollowerId,
                        Follower = model.Follower,
                        FollowingId = model.FollowingId,
                        Following = model.Following,
                    };
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
