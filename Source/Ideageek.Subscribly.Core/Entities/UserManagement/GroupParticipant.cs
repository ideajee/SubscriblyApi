using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class GroupParticipant : BaseEntity
    {
        [Required]
        public Guid GroupExpenseId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ShareAmount { get; set; }
    }
}