using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class UserSubscription : BaseEntity
    {
        [Required]
        public Guid AdminUserId { get; set; }
        [Required]
        public Guid SubscriptionId { get; set; }
        [Required]
        public string ProfileNameOrEmail { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}