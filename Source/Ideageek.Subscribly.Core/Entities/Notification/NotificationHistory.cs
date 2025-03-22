namespace Ideageek.Subscribly.Core.Entities.Notification
{
    public class NotificationHistory
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public NotificationStatus Status { get; set; }
    }
}