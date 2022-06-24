using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Noteapp.Application.Interfaces;
using Noteapp.Application.Results;
using Noteapp.Application.ViewModels.Users;
using Noteapp.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Services
{
    /// <summary>
    /// Defines the structure of the <see cref="UserService"/> class.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor httpContext, ILogger<UserService> logger)
        {
            _userManager = userManager; 
            _roleManager = roleManager; 
            _httpContext = httpContext;
            _logger = logger;
        }
        /// <summary>
        /// Implements the <see cref="GetUsers"/> method of interface.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserViewModel>> GetUsers()
        {
            try
            {
                var users = _userManager.Users.Where(x => !x.IsDeleted);
                List<UserViewModel> model = new List<UserViewModel>();
                foreach (var user in users)
                {
                    UserViewModel modelItem = new UserViewModel();
                    modelItem.Id = user.Id;
                    modelItem.Email = user.Email;
                    modelItem.FirstName = user.FirstName;
                    modelItem.LastName = user.LastName;
                    modelItem.CreatedOn = user.CreatedOnUtc.ToShortDateString();
                    modelItem.UpdatedOn = user.UpdatedOnUtc == null ? "" : user.UpdatedOnUtc.GetValueOrDefault().ToShortDateString();
                    var userRoles = await _userManager.GetRolesAsync(user);
                    modelItem.RoleName = userRoles.FirstOrDefault();
                    var roleObj = await _roleManager.FindByNameAsync(modelItem.RoleName);
                    modelItem.RoleId = roleObj.Id;
                    model.Add(modelItem);
                }
                return model;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Implements the <see cref="Details(Guid)"/> method of interface.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserViewModel> Details(Guid id)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
                if (user == null)
                    return null;
                var currentUserRole = GetCurrentUserRole();
                if (currentUserRole != "Admin")
                {
                    var currentUserId = GetCurrentUserId();
                    if (currentUserId != id)
                    {
                        return null;
                    }
                }
                UserViewModel modelItem = new UserViewModel();
                modelItem.Id = user.Id;
                modelItem.Email = user.Email;
                modelItem.FirstName = user.FirstName;
                modelItem.LastName = user.LastName;
                modelItem.CreatedOn = user.CreatedOnUtc.ToShortDateString();
                modelItem.UpdatedOn = user.UpdatedOnUtc == null ? "" : user.UpdatedOnUtc.GetValueOrDefault().ToShortDateString();
                var userRoles = await _userManager.GetRolesAsync(user);
                modelItem.RoleName = userRoles.FirstOrDefault();
                var roleObj = await _roleManager.FindByNameAsync(modelItem.RoleName);
                modelItem.RoleId = roleObj.Id;
                return modelItem;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Implements the <see cref="GetCurrentUserId"/> method of interface.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Guid GetCurrentUserId()
        {
            var email = string.Empty;
            if (_httpContext.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                email = identity.FindFirst(ClaimTypes.Name).Value;
                var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == email);
                return currentUser.Id;
            }
            else
            {
                _logger.LogInformation("No users are logged in");
                throw new ArgumentNullException("No users are logged in");

            }
        }
        /// <summary>
        /// Implements the <see cref="GetCurrentUserRole"/> method of interface.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetCurrentUserRole()
        {
            var role = string.Empty;
            if (_httpContext.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                role = identity.FindFirst(ClaimTypes.Role).Value;
                return role;
            }
            else
            {
                _logger.LogInformation("No users are logged in");
                throw new ArgumentNullException("No users are logged in");

            }
        }
        /// <summary>
        /// Implements the <see cref="Create(CreateUserRequest)"/> method of interface.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result> Create(CreateUserRequest model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return Result.Fail("Email address is taken.");
                ApplicationUser user = new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    IsDeleted = false,
                    CreatedOnUtc = DateTime.Now,
                    IsActive = true,
                    CreatedById = GetCurrentUserId()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join(',',result.Errors?.Select(x => x.Description)));
                    return Result.Fail("Could not create user. Please make sure the password contains at least 8 characters, an uppercase and lowercase letter, a number and a special character.");

                }
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                return Result.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Something went wrong.");
            }
        }
        /// <summary>
        /// Implements the <see cref="Update(UpdateUserRequest)"/> method of interface.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result> Update(UpdateUserRequest model)
        {
            try
            {
                var validationResult = await ValidateUserUpdate(model.Email);
                if (validationResult.Success)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (!String.IsNullOrEmpty(model.FirstName))
                        user.FirstName = model.FirstName;
                    if (!String.IsNullOrEmpty(model.LastName))
                        user.LastName = model.LastName;
                    if (!String.IsNullOrEmpty(model.Email))
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                    }
                    user.UpdatedOnUtc = DateTime.Now;
                    user.UpdatedById = GetCurrentUserId();
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                        return Result.Fail("Could not update profile.");
                    }
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    var userIsInRole = await _userManager.IsInRoleAsync(user, model.RoleId);
                    if (!userIsInRole)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        result = await _userManager.RemoveFromRolesAsync(user, userRoles.ToList());
                        if (!result.Succeeded)
                        {
                            _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                            return Result.Fail("Something went wrong.");

                        }
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                        {
                            _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                            return Result.Fail("Could not set new user role.");

                        }
                    }
                    if (!String.IsNullOrEmpty(model.OldPassword) && !String.IsNullOrEmpty(model.Password))
                    {
                        result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                        if (!result.Succeeded)
                        {
                            _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                            return Result.Fail("Could not set new user password. Please make sure your old password is correct and that your new password contains at least 8 characters, an uppercase and lowercase letter, a number and a special character.");
                        }

                    }
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail(string.Join(',', validationResult.Messages.Select(x => x.Text)));
                }
            }
           catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Something went wrong.");
            }
        }
        /// <summary>
        /// Implements the <see cref="Delete(Guid)"/> method of interface.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> Delete(Guid id)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
                if (user == null)
                    return Result.Fail("Could not find user.");
                user.IsDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Result.Ok();
                _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                return Result.Fail("Something went wrong.");
            }
           catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Something went wrong.");
            }
        }
        /// <summary>
        /// Implements the <see cref="UpdateProfile(UpdateProfileRequest)"/> method of interface.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result> UpdateProfile(UpdateProfileRequest model)
        {
            try
            {
                var validationResult = await ValidateUserUpdate(model.Email);
                if (validationResult.Success)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var currentUserRole = GetCurrentUserRole();
                    if (currentUserRole != "Admin")
                    {
                        var currentUserId = GetCurrentUserId();
                        if (currentUserId != user.Id)
                        {
                            return Result.Fail("Unauthorized");
                        }
                    }
                    if (!String.IsNullOrEmpty(model.FirstName))
                        user.FirstName = model.FirstName;
                    if (!String.IsNullOrEmpty(model.LastName))
                        user.LastName = model.LastName;
                    if (!String.IsNullOrEmpty(model.Email))
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                    }
                    user.UpdatedOnUtc = DateTime.Now;
                    user.UpdatedById = GetCurrentUserId();
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                        return Result.Fail("Could not update profile.");

                    }
                    if (!String.IsNullOrEmpty(model.OldPassword) && !String.IsNullOrEmpty(model.Password))
                    {
                        result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                        if (!result.Succeeded)
                        {
                            _logger.LogError(string.Join(',', result.Errors?.Select(x => x.Description)));
                            return Result.Fail("Could not set new user password. Please make sure your old password is correct and that your new password contains at least 8 characters, an uppercase and lowercase letter, a number and a special character.");
                        }

                    }
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail(string.Join(',', validationResult.Messages.Select(x => x.Text)));
                }
            }
           catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Something went wrong.");
            }
            
        }
        /// <summary>
        /// Implements the <see cref="ValidateUserUpdate(string)"/> method of interface.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<Result> ValidateUserUpdate(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }
            var userExists = _userManager.Users.Any(x => x.Email == email && x.Id != user.Id);
            if (userExists)
                return Result.Fail("Another user with the same email address already exists");
            return Result.Ok();
        }
    }
}
