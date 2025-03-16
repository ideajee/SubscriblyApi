using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class Friend : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FriendName { get; set; }
        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}