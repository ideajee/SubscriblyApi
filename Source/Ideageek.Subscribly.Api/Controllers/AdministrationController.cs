using Ideageek.Subscribly.Core.Entities.Administration;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Services.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ideageek.Subscribly.Api.Controllers
{
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
        [Authorize(Roles = "Admin")]
        [HttpGet("GetSubscriptionById")]
        public IActionResult GetSubscriptionById(Guid id)
        {
            return Ok(_SubscriptionService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_SubscriptionService.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllSubscriptions")]
        public IActionResult GetAllSubscriptions()
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");

            return Ok(_SubscriptionService.GetAllSubscriptionsById(userId.Value));
        }

        [Authorize(Roles = "Admin")]
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
        public IActionResult UpdateSubscription(UpdateSubscription request)
        {
            return Ok(_SubscriptionService.Update(request));
        }
        #endregion
    }
}