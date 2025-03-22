using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.Administration
{
    public class Subscription : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Icon { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class AddSubscription
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Icon { get; set; }
        [Required]
        public bool Status { get; set; }
    }
    public class UpdateSubscription
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Icon { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}