using Noteapp.Domain.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Domain.Models.Note
{
    public class Note : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
    }
}
