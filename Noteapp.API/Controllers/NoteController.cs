using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noteapp.Application.Interfaces;
using Noteapp.Application.ViewModels.Notes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Noteapp.API.Controllers
{
    /// <summary>
    /// Controller that will handle all operations regarding notes.
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteService"></param>
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        /// <summary>
        /// Returns a list of all the notes
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet("Index")]
        [SwaggerOperation("Get a list of all the notes")]
        public async Task<IActionResult> Index()
        {
            var notes = await _noteService.GetNotes();
            return Json(notes);
        }
        /// <summary>
        /// Returns a list of all the notes made by a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("NotesForUser/{userId}")]
        [SwaggerOperation("Get a list of all the notes for a user")]
        public async Task<IActionResult> IndexForUser([FromRoute] Guid userId)
        {
            var notes = await _noteService.GetNotes(userId);
            if (notes == null) return Unauthorized();
            return Json(notes);
        }

        /// <summary>
        /// Returns the details of a single note.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation("Get details of a note")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var details = await _noteService.GetNoteById(id);
            if (details == null) return Unauthorized();
            return Json(details);
        }
        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation("Creates a new note")]
        public async Task<IActionResult> Create([FromForm] CreateNoteRequest model)
        {
            var result = await _noteService.Create(model);
            if (result.Success)
                return Ok();
            return BadRequest(result.Messages);
        }
        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [SwaggerOperation("Updates an existing note")]
        public async Task<IActionResult> Update([FromForm] UpdateNoteRequest model)
        {
            var result = await _noteService.Update(model);
            if (result.Success)
                return Ok();
            if (result.Messages[0].Code == "Unauthorized") return Unauthorized();
            return BadRequest(result.Messages);
        }
        /// <summary>
        /// Deletes an existing note.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerOperation("Deletes an existing note")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _noteService.Delete(id);
            if (result.Success)
                return Ok();
            if (result.Messages[0].Code == "Unauthorized") return Unauthorized();
            return BadRequest(result.Messages);
        }
    }
}
