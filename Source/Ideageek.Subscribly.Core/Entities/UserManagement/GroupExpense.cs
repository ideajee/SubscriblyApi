using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class GroupExpense : BaseEntity
    {
        [Required]
        public Guid GroupId { get; set; }
        public Guid PaidBy { get; set; } // Nullable since it is NULL in the DB
        [Required]
        public int Amount { get; set; }
        [Required]
        public Guid CurrencyId { get; set; }
        [Required]
        public Guid ExpenseId { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}