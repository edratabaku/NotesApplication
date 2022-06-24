using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Notes
{
    public class UpdateNoteRequest : CreateNoteRequest
    {
        public Guid Id { get; set; }
        public new class Validator : AbstractValidator<UpdateNoteRequest>
        {
            public Validator()
            {
                RuleFor(model => model.Title)
                    .MaximumLength(100).WithMessage("The title of the note cannot contain more than 100 characters");
                RuleFor(model => model.Description)
                   .MaximumLength(2000).WithMessage("The description of the note cannot contain more than 2000 characters");
                RuleFor(model => model.Tags)
                   .MaximumLength(100).WithMessage("The tags of the note cannot contain more than 1000 characters");
            }
        }
    }
}
