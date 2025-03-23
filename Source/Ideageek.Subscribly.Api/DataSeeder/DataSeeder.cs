using Dapper;
using Ideageek.Subscribly.Core.Entities.Authorization;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Ideageek.Subscribly.Api.DataSeeder
{
    public class DataSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(IServiceProvider serviceProvider, ILogger<DataSeeder> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            using var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            try
            {
                await SeedRolesAsync(dbConnection);
                await SeedUsersAsync(dbConnection);
                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error seeding data: {ex.Message}");
            }
        }

        private async Task SeedRolesAsync(IDbConnection dbConnection)
        {
            string[] roles = { "SuperAdmin", "Admin", "User" };

            foreach (var role in roles)
            {
                var existingRole = await dbConnection.QueryFirstOrDefaultAsync<string>(
                    "SELECT Name FROM AspNetRoles WHERE Name = @Name", new { Name = role });

                if (existingRole == null)
                {
                    await dbConnection.ExecuteAsync("INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (@Id, @Name, @NormalizedName)",
                        new
                        {
                            Id = Guid.NewGuid(),
                            Name = role,
                            NormalizedName = role.ToUpper()
                        });
                    _logger.LogInformation($"Role '{role}' inserted.");
                }
            }
        }

        private async Task SeedUsersAsync(IDbConnection dbConnection)
        {
            var users = new[]
            {
            new { Name = "SuperAdminUser", UserName = "12345678", Email = "superadmin@example.com", Role = "SuperAdmin", IsAdmin=false },
            new { Name = "AdminUser", UserName = "+923343487595", Email = "admin@example.com", Role = "Admin", IsAdmin=true },
            new { Name = "RegularUser", UserName = "+923458076081", Email = "user@example.com", Role = "User", IsAdmin=false }
            };

            foreach (var user in users)
            {
                var existingUser = await dbConnection.QueryFirstOrDefaultAsync<string>(
                    "SELECT Email FROM AspNetUsers WHERE UserName = @UserName", new { UserName = user.UserName });

                if (existingUser == null)
                {
                    var userId = Guid.NewGuid();
                    var passwordHash = HashPassword("Ideageek123");

                    await dbConnection.ExecuteAsync(
                    "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, IsAdmin, DuePayments,CurrentPayments,TotalPayments) " +
                    "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @IsAdmin, @DuePayments, @CurrentPayments, @TotalPayments)",
                    new AspNetUser()
                    {
                        Id = userId,
                        UserName = user.UserName,
                        NormalizedUserName = user.UserName.ToUpper(),
                        Email = user.Email,
                        NormalizedEmail = user.Email.ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = passwordHash,
                        IsAdmin = user.IsAdmin,
                        DuePayments = 0,
                        CurrentPayments = 0,
                        TotalPayments = 0,
                    });

                    await dbConnection.ExecuteAsync(
                        "INSERT INTO AspNetUserRoles (UserId, RoleId) " +
                        "VALUES (@UserId, (SELECT Id FROM AspNetRoles WHERE Name = @Role))",
                        new { UserId = userId, Role = user.Role });

                    _logger.LogInformation($"User '{user.Email}' created and assigned role '{user.Role}'.");
                }
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }
    }
}