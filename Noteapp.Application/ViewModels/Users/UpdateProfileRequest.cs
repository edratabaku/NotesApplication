using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Users
{
    public class UpdateProfileRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public class Validator : AbstractValidator<UpdateProfileRequest>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("The first name cannot contain more than 100 characters.");
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("The last name field cannot contain more than 100 characters.");
                RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address.");
                RuleFor(x => x.Password).MinimumLength(8).WithMessage("The password must contain at least 8 characters");
                RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Password and Confirm Password must match.");
            }
        }
    }
}
