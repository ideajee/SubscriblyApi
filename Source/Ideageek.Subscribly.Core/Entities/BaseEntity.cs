using Ideageek.Subscribly.Core.Entities.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedOn = DateTime.Now;
            this.Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
