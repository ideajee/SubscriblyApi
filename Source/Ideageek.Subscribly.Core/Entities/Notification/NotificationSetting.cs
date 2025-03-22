using System.ComponentModel.DataAnnotations;

namespace Ideageek.Subscribly.Core.Entities.Notification
{
    public class NotificationSetting : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}