using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.Administration
{
    public class Expense : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Icon { get; set; }
    }
}