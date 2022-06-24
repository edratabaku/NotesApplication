using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.ViewModels.Notes
{
    public class NoteViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
