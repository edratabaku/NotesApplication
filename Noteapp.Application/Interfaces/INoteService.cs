using Noteapp.Application.Results;
using Noteapp.Application.ViewModels.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Interfaces
{
    /// <summary>
    /// Defines the structure of the <see cref="INoteService"/> interface.
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Gets all the notes if the user is admin and notes of a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<NoteViewModel>> GetNotes(Guid? userId = null);
        /// <summary>
        /// Gets the details of a note from its identifier
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        Task<NoteViewModel> GetNoteById(Guid noteId);
        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="noteRequest"></param>
        /// <returns></returns>
        Task<Result> Create(CreateNoteRequest noteRequest);
        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="noteRequest"></param>
        /// <returns></returns>
        Task<Result> Update(UpdateNoteRequest noteRequest);
        /// <summary>
        /// Deletes an existing note.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> Delete(Guid id);
    }
}
