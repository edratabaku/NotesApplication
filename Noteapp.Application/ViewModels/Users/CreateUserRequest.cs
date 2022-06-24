using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Users
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string RoleId { get; set; }
        public class Validator : AbstractValidator<CreateUserRequest>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotNull().WithMessage("The first name field is a required field.")
                    .NotEmpty().WithMessage("The first name field is a required field.")
                    .MaximumLength(100).WithMessage("The first name cannot contain more than 100 characters.");
                RuleFor(x => x.LastName).NotNull().WithMessage("The last name field is a required field.")
                    .NotEmpty().WithMessage("The last name field is a required field.")
                    .MaximumLength(100).WithMessage("The last name field cannot contain more than 100 characters.");
                RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address.")
                    .NotNull().WithMessage("The email field is a required field.")
                    .NotEmpty().WithMessage("The email field is a required field.");
                RuleFor(x => x.Password).NotNull().WithMessage("The password field is a required field.")
                    .NotEmpty().WithMessage("The password field is a required field.")
                    .MinimumLength(8).WithMessage("The password must contain at least 8 characters");
                RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Password and Confirm Password must match.");
            }
        }
    }
}
