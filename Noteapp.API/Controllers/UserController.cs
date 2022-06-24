using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noteapp.Application.Interfaces;
using Noteapp.Application.ViewModels.Users;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Noteapp.API.Controllers
{
    /// <summary>
    /// Controller that will handle all operations regarding users.
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService; 
        }
        /// <summary>
        /// Returns a list of all the users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet("Index")]
        [SwaggerOperation("Get a list of all the users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();
            return Json(users);
        }
        /// <summary>
        /// Returns the details of a single user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation("Get details of a user")]
        public async Task<IActionResult> Details([FromRoute]Guid id)
        {
            var details = await _userService.Details(id);
            return Json(details);
        }
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [SwaggerOperation("Creates a new user")]
        public async Task<IActionResult> Create([FromForm]CreateUserRequest model)
        {
            var result = await _userService.Create(model);
            if (result.Success)
                return Ok();
            return BadRequest(result.Messages);
        }
        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Update")]
        [SwaggerOperation("Updates an existing user")]
        public async Task<IActionResult> Update([FromForm]UpdateUserRequest model)
        {
            var result = await _userService.Update(model);
            if (result.Success)
                return Ok();
            return BadRequest(result.Messages);
        }
        /// <summary>
        /// Updates the profile of the currently signed in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateProfile")]
        [SwaggerOperation("Updates an existing user")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest model)
        {
            var result = await _userService.UpdateProfile(model);
            if (result.Success)
                return Ok();
            return BadRequest(result.Messages);
        }
        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        [SwaggerOperation("Deletes an existing user")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var result = await _userService.Delete(id);
            if (result.Success)
                return Ok();
            return BadRequest(result.Messages);
        }
    }
}
