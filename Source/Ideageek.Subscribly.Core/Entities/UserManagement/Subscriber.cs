using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class Subscriber : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
    }
    public class  GetAllSubscriber
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int DuePayments { get; set; }
        public int CurrentPayments { get; set; }
        public int TotalPayments { get; set; }
    }
    public class AddSubscriber
    {
        public Guid? UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Guid SubscriptionId { get; set; }
        public string PasswordHash { get; set; }
        public int DuePayments { get; set; }
    }
}