using Noteapp.Application.Results;
using Noteapp.Application.ViewModels;
using Noteapp.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noteapp.Application.Interfaces
{
    /// <summary>
    /// Authentication and authorization management service.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="user">User to authenticate.</param>user
        /// <param name="rememberMe">Value whether to persist the cookie.</param>user
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Result> SignInAsync(
            ApplicationUser user,
            bool rememberMe,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sign out.
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// Gets currently authenticated user.
        /// </summary>
        Task<AuthenticatedUser?> GetAuthenticatedUserAsync();

        /// <summary>
        /// Gets currently authenticated user or the default guest.
        /// </summary>
        Task<AuthenticatedUser> GetAuthenticatedUserOrGuestAsync();
    }
}
