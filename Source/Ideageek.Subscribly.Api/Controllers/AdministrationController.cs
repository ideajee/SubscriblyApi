using Ideageek.Subscribly.Core.Entities.Administration;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Services.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ideageek.Subscribly.Api.Controllers
{
    [Authorize]
    [Route("api/administration")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly ISubscriptionService _SubscriptionService;
        private readonly IAuthHelper _authHelper;
        public AdministrationController(ISubscriptionService SubscriptionService, IConfiguration configuration, IAuthHelper authHelper)
        {
            _SubscriptionService = SubscriptionService;
            _authHelper = authHelper;
        }

        #region Subscription
        [HttpGet("GetSubscriptionById")]
        public async Task<IActionResult> GetSubscriptionById(Guid id)
        {
            return Ok(await _SubscriptionService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _SubscriptionService.GetAll());
        }

        [HttpPost("GetAllSubscriptions")]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");

            return Ok(await _SubscriptionService.GetAllSubscriptionsById(userId.Value));
        }

        [HttpPost("AddSubscription")]
        public async Task<IActionResult> AddSubscription(AddSubscription request)
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");
            try
            {
                return Ok(await _SubscriptionService.Add(request, userId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateSubscription")]
        public async Task<IActionResult> UpdateSubscription(UpdateSubscription request)
        {
            return Ok(await _SubscriptionService.Update(request));
        }
        #endregion
    }
}