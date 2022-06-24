using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Notes
{
    public class CreateNoteRequest
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public string Tags { get; set; }
        public class Validator : AbstractValidator<CreateNoteRequest>
        {
            public Validator()
            {
                RuleFor(model => model.Title).NotNull().WithMessage("Please enter the title of the note")
                    .NotEmpty().WithMessage("Please enter the title of the note")
                    .MaximumLength(100).WithMessage("The title of the note cannot contain more than 100 characters");
                RuleFor(model => model.Description).NotNull().WithMessage("Please enter the description of the note")
                   .NotEmpty().WithMessage("Please enter the description of the note")
                   .MaximumLength(2000).WithMessage("The description of the note cannot contain more than 2000 characters");
                RuleFor(model => model.Tags).NotNull().WithMessage("Please enter the tags for the note")
                   .NotEmpty().WithMessage("Please enter the tags for the note")
                   .MaximumLength(100).WithMessage("The tags of the note cannot contain more than 1000 characters");
            }
        }
    }
}
