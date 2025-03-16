using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class GroupMember : BaseEntity
    {
        [Required]
        public Guid GroupId { get; set; }
        [Required]
        public Guid FriendId { get; set; }
    }
}