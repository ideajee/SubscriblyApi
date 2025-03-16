using Ideageek.Subscribly.Services.Dtos.Authorization;
using Ideageek.Subscribly.Core.Entities.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ideageek.Subscribly.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IPasswordHasher<AspNetUser> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager, SignInManager<AspNetUser> signInManager, IConfiguration configuration, IPasswordHasher<AspNetUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new AspNetUser
            {
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if(model.UserName == "admin")
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new AspNetRole { Name = "Admin" });

                var roleAssignResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleAssignResult.Succeeded)
                    return BadRequest("Failed to assign 'User' role.");
            }
            else
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new AspNetRole { Name = "User" });

                var roleAssignResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleAssignResult.Succeeded)
                    return BadRequest("Failed to assign 'User' role.");
            }

            return Ok(new { Message = "User registered successfully" });
        }

        /// <summary>
        /// User login with JWT token response.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) return Unauthorized(new { Message = "Invalid credentials" });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded) return Unauthorized(new { Message = "Invalid credentials" });

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, Message = "Login successful" });
        }

        private string GenerateJwtToken(AspNetUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Roles = "User")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(new { user.Id, user.UserName, user.Email });
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out" });
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new { Message = "User not found" });

            return Ok(user);
        }

        /// <summary>
        /// Get user by username.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("user/by-username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { Message = "User not found" });

            return Ok(user);
        }
    }
}