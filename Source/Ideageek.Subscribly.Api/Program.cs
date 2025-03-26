using Ideageek.MunshiJee.Services.Authorization;
using Ideageek.Subscribly.Api.DataSeeder;
using Ideageek.Subscribly.Core.Entities.Authorization;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Repositories;
using Ideageek.Subscribly.Core.Services.Administration;
using Ideageek.Subscribly.Core.Services.UserManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("SubscriblyConnection")));

//builder.Services.AddIdentityCore<AspNetUser>()
//    .AddSignInManager()
//    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<AspNetUser>()
    .AddRoles<AspNetRole>()
    .AddUserStore<UserStore>()
    .AddRoleStore<RoleStore>()
    .AddSignInManager<SignInManager<AspNetUser>>()
    .AddRoleManager<RoleManager<AspNetRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserStore<AspNetUser>, UserStore>();
builder.Services.AddScoped<IRoleStore<AspNetRole>, RoleStore>();
builder.Services.AddScoped<IPasswordHasher<AspNetUser>, PasswordHasher<AspNetUser>>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IAppHelper, AppHelper>();

//builder.Services.AddScoped<DataSeeder>();


// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    //options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    //options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //ValidateIssuerSigningKey = true,
        //IssuerSigningKey = new SymmetricSecurityKey(key),
        //ValidateIssuer = false,
        //ValidateAudience = false,
        //ValidIssuer = jwtSettings["Issuer"],
        //ValidAudience = jwtSettings["Audience"],
        //ValidateLifetime = true,
        //RoleClaimType = ClaimTypes.Role,
        //ClockSkew = TimeSpan.Zero

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
//});

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
//    await seeder.SeedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
