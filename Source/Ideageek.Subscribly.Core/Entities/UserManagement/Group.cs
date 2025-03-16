using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.UserManagement
{
    public class Group : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }
        [Required]
        public GroupType GroupType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(100)]
        public string ShareableLink { get; set; }
    }
}