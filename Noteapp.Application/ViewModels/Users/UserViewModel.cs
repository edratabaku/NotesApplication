using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;   
        public string Email { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string RoleName { get; set; }
        public Guid RoleId { get; set; }

    }
}
