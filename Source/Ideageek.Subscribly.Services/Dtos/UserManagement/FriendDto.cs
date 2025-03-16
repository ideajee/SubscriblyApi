using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Services.Dtos.UserManagement
{
    public class FriendDto
    {
    }
    public class AddFriendDto
    {
        [Required]
        [StringLength(100)]
        public string FriendName { get; set; }
        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
    public class UpdateFriendDto
    {
        [Required]
        public Guid Id { get; set; }
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