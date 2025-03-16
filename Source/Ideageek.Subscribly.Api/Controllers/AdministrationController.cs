using Ideageek.Subscribly.Services.Administration;
using Ideageek.Subscribly.Services.Dtos.UserManagement;
using Ideageek.Subscribly.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ideageek.Subscribly.Api.Controllers
{
    [Route("api/administration")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly IConfiguration _configuration;
        private readonly IAuthHelper _authHelper;
        public AdministrationController(IFriendService friendService, IConfiguration configuration, IAuthHelper authHelper)
        {
            _friendService = friendService;
            _configuration = configuration;
            _authHelper = authHelper;
        }

        #region Friend
        [Authorize(Roles = "User")]
        [HttpGet("GetFriendById")]
        public IActionResult GetFriendById(Guid id)
        {
            return Ok(_friendService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_friendService.GetAll());
        }

        [Authorize(Roles = "User")]
        [HttpPost("GetAllFriends")]
        public IActionResult GetAllFriends()
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");

            return Ok(_friendService.GetAllFriendsById(userId.Value));
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddFriend")]
        public async Task<IActionResult> AddFriend(AddFriendDto request)
        {
            var userId = _authHelper.GetUserId();
            if (userId == null)
                return Unauthorized("Invalid User Id. Please validate token!");
            try
            {
                return Ok(await _friendService.Add(request, userId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("UpdateFriend")]
        public IActionResult UpdateFriend(UpdateFriendDto request)
        {
            return Ok(_friendService.Update(request));
        }
        #endregion
    }
}