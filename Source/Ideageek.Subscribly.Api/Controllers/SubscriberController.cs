using Ideageek.Subscribly.Core.Entities.Authorization;
using Ideageek.Subscribly.Core.Entities.UserManagement;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Services.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ideageek.Subscribly.Api.Controllers
{
    [Route("api/administration")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IPasswordHasher<AspNetUser> _passwordHasher;
        private readonly ISubscriberService _subscriberService;
        private readonly IAuthHelper _authHelper;
        public SubscriberController(UserManager<AspNetUser> userManager, IPasswordHasher<AspNetUser> passwordHasher, ISubscriberService subscriberService,IAuthHelper authHelper)
        {
            _userManager = userManager;
            _subscriberService = subscriberService;
            _authHelper = authHelper;
            _passwordHasher = passwordHasher;
        }
        [Authorize(Roles = "User")]
        [HttpGet("GetSubscriberById")]
        public async Task<IActionResult> GetSubscriberById(Guid subscriberId)
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");

            return Ok(await _subscriberService.GetSubscriberById(userId.Value, subscriberId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllSubscribersByAdminAndSubscriptionId")]
        public async Task<IActionResult> GetAllSubscribersByAdminAndSubscriptionId(Guid adminId, Guid subscriptionId)
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");

            return Ok(await _subscriberService.GetAllSubscribersByAdminAndSubscriptionId(adminId, subscriptionId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddSubscriber")]
        public async Task<IActionResult> AddSubscriber([FromBody] AddSubscriber request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userId = _authHelper.GetUserId();
                if (userId == null)
                    return Unauthorized("Invalid User Id. Please validate token!");

                var user = new AspNetUser
                {
                    UserName = request.UserName,
                    NormalizedUserName = request.UserName.ToUpper(),
                    Email = request.Email,
                    NormalizedEmail = request.Email.ToUpper(),
                    DuePayments = request.DuePayments,
                    CurrentPayments = 0,
                    TotalPayments = request.DuePayments,
                    EmailConfirmed = true,
                    IsAdmin = false
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, "123456");
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                var roleAssignResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleAssignResult.Succeeded)
                    return BadRequest("Failed to assign 'User' role.");

                var subscriberId = (await _userManager.FindByNameAsync(user.UserName)).Id;
                return Ok(await _subscriberService.Add(subscriberId, userId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("RemoveSubscriberFromSubscription")]
        public async Task<IActionResult> RemoveSubscriberFromSubscription(Guid subscriberId, Guid subscriptionId)
        {
            return Ok(await _subscriberService.RemoveSubscriberFromSubscription(subscriberId, subscriptionId));
        }
    }
}