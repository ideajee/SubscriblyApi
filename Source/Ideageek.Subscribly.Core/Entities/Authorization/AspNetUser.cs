namespace Ideageek.Subscribly.Core.Entities.Authorization
{
    public class AspNetUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string NormalizedUserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NormalizedEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public bool IsAdmin { get; set; }
        public int DuePayments { get; set; }
        public int CurrentPayments { get; set; }
        public int TotalPayments { get; set; }
        //public string SecurityStamp { get; set; }
        //public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        //public bool TwoFactorEnabled { get; set; } = false;
        //public DateTimeOffset? LockoutEnd { get; set; }
        //public bool LockoutEnabled { get; set; } = true;
        //public int AccessFailedCount { get; set; } = 0;
    }
}