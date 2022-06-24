using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels
{
    public class AuthenticationSettings
    {
        /// <summary>
        /// The default value used for authentication scheme.
        /// </summary>
        public static string AuthenticationScheme { get; set; } = "Authentication";

        /// <summary>
        /// The issuer that should be used for any claims that are created.
        /// </summary>
        public static string ClaimsIssuer { get; set; } = "Noteapp";
    }
}
