using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class FriendExpense : BaseEntity
    {
        [Required]
        public Guid FriendId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public Guid ExpenseId { get; set; }
        [Required]
        public Guid CurrencyId { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}