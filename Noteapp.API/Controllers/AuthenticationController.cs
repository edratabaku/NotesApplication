using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Noteapp.Application.Interfaces;
using Noteapp.Application.Results.Messages;
using Noteapp.Application.ViewModels.Users;
using Noteapp.Domain.Models.User;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.API.Controllers
{
    /// <summary>
    /// Controller that will handle all operations regarding authentication.
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Registers a new user to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [SwaggerOperation("Register a new user")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("Email address is taken.");
            ApplicationUser user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                IsDeleted = false,
                CreatedOnUtc = DateTime.Now,
                IsActive = true
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest("Could not create user. Please make sure the password contains at least 8 characters, an uppercase and lowercase letter, a number and a special character.");
            var role = await roleManager.FindByNameAsync("Admin");
            if (role != null)
            {
                await userManager.AddToRoleAsync(user, role.Name);
            }
            return Ok();
        }
        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [SwaggerOperation("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new Message("An accound with this email address does not exist."));
            else
            {
                if (user.IsDeleted)
                {
                    return Unauthorized("User no longer exists.");
                }
                else if(user.IsActive == false)
                {
                    return Unauthorized("User is no longer active.");
                }
                if (await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                    var x = _configuration.GetSection("JWT")["SecretKey"];

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(x));

                    var token = new JwtSecurityToken(

                    expires: DateTime.Now.AddDays(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    return Unauthorized(new Message("Incorrect password."));
                }
            }

        }

    }
}
