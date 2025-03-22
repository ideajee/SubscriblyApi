using System.ComponentModel;

namespace Ideageek.Subscribly.Core.Entities
{
    public class Enum
    {
    }

    public enum GroupType
    {
        [Description("None")]
        None = 0,
        [Description("Home")]
        Home = 1,
        [Description("Trip")]
        Trip
    }
    public enum SubscriptionStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Inactive")]
        Inactive = 2,
        [Description("Unpaid")]
        Unpaid = 3,
    }
    public enum NotificationStatus
    {
        [Description("Pending")]
        Pending = 0,
        [Description("Delivered")]
        Delivered = 1,
        [Description("Failed")]
        Failed = 2,
    }
}