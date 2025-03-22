using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
    }
}
