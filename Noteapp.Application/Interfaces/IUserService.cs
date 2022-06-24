using Noteapp.Application.Results;
using Noteapp.Application.ViewModels.Users;
using Noteapp.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IUserService" />.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns></returns>
        Task<List<UserViewModel>> GetUsers();
        /// <summary>
        /// Gets the details of a user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel> Details(Guid id);
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Result> Create(CreateUserRequest model);
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Result> Update(UpdateUserRequest model);
        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> Delete(Guid id);
        /// <summary>
        /// Updates the profile of the currently logged in user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Result> UpdateProfile(UpdateProfileRequest model);
        /// <summary>
        /// Returns the identifier of the current user.
        /// </summary>
        /// <returns></returns>
        Guid GetCurrentUserId();
        /// <summary>
        /// Returns the name of the current user's role.
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserRole();
    }
}
