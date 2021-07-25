using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkiHire.Core.Domain.Entities
{
    public class Follow : FullAuditedEntity
    {
        #region follow
        [ForeignKey("FollowerId")]
        public virtual long? FollowerId { get; set; }
        public virtual User Follower { get; set; }//fan
        [ForeignKey("FollowingId")]
        public virtual long? FollowingId { get; set; }
        public virtual User Following { get; set; }//receipient
        #endregion

        //public static implicit operator Follow(FollowDTO model)
        //{
        //    try
        //    {
        //        if(model != null)
        //        {
        //            Follow response = new Follow
        //            {
        //                FollowerId = model.FollowerId,
        //                Follower = model.Follower,
        //                FollowingId = model.FollowingId,
        //                Following = model.Following,
        //            };
        //            return response;
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
