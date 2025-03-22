using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class Payment : BaseEntity
    {
        public Guid UserSubscriptionId { get; set; }
        public int Amount { get; set; }
        public DateOnly PaymentMonth { get; set; }
        public bool IsDued { get; set; }
        public DateTime? PaidOn { get; set; }
        public bool IsPaid { get; set; }
    }
    public class AddPayment
    {
        [Required]
        public Guid UserSubscriptionId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public DateOnly PaymentMonth { get; set; }
        [Required]
        public bool IsDued { get; set; }
        public DateTime? PaidOn { get; set; }
        [Required]
        public bool IsPaid { get; set; }
    }
}