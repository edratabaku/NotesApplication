using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Noteapp.Application.Interfaces;
using Noteapp.Application.Results;
using Noteapp.Application.ViewModels.Notes;
using Noteapp.Domain.Interfaces;
using Noteapp.Domain.Models.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Services
{
    /// <summary>
    /// Defines the structure of the <see cref="NoteService"/> class.
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IUserService _userService;
        private readonly ILogger<NoteService> _logger;
        public NoteService(IRepository<Note> noteRepository, IUserService userService, ILogger<NoteService> logger)
        {
            _noteRepository = noteRepository;
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// Implements the <see cref="GetNotes(Guid?)"/> method of interface.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<NoteViewModel>> GetNotes(Guid? userId = null)
        {
            try
            {
                var currentUserRole = _userService.GetCurrentUserRole();
                if (currentUserRole != "Admin")
                {
                    var currentUserId = _userService.GetCurrentUserId();
                    if (currentUserId != userId)
                    {
                        return null;
                    }
                }
                return await _noteRepository.TableNoTracking.Include(x => x.CreatedBy).Where(x => !x.IsDeleted && (userId == null || x.CreatedById == userId)).Select(x => new NoteViewModel
                {
                    Id = x.Id,
                    CreatedById = x.CreatedById.GetValueOrDefault(),
                    Description = x.Description,
                    Created = x.CreatedOnUtc,
                    CreatedBy = x.CreatedBy.FullName,
                    Tags = x.Tags,
                    Title = x.Title
                }).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Implements the <see cref="GetNoteById(Guid)"/> method of interface.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public async Task<NoteViewModel> GetNoteById(Guid noteId)
        {
            try
            {
                var currentUserRole = _userService.GetCurrentUserRole();
                NoteViewModel model = new NoteViewModel();
                var note = await _noteRepository.TableNoTracking.Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == noteId && !x.IsDeleted);
                if (note == null) return null;
                if (currentUserRole != "Admin")
                {
                    var currentUserId = _userService.GetCurrentUserId();
                    if(currentUserId != note.CreatedById)
                    {
                        return null;
                    }
                }
                model.Created = note.CreatedOnUtc;
                model.CreatedBy = note.CreatedBy.FullName;
                model.Description = note.Description;
                model.Tags = note.Tags;
                model.Title = note.Title;
                model.Id = note.Id;
                model.CreatedById = note.CreatedById.GetValueOrDefault();
                return model;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Implements the <see cref="Create(CreateNoteRequest)"/> method of interface.
        /// </summary>
        /// <param name="noteRequest"></param>
        /// <returns></returns>
        public async Task<Result> Create(CreateNoteRequest noteRequest)
        {
            try
            {
                Note note = new Note();
                note.Id = new Guid();
                note.IsActive = true;
                note.Title = noteRequest.Title;
                note.CreatedOnUtc = DateTime.UtcNow;
                note.Description = noteRequest.Description;
                note.CreatedById = _userService.GetCurrentUserId();
                note.IsDeleted = false;
                note.Tags = noteRequest.Tags;
                await _noteRepository.InsertAsync(note);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Could not create note.");
            }
        }
        /// <summary>
        /// Implements the <see cref="Update(UpdateNoteRequest)"/> method of interface.
        /// </summary>
        /// <param name="noteRequest"></param>
        /// <returns></returns>
        public async Task<Result> Update(UpdateNoteRequest noteRequest)
        {
            try
            {
                var note = _noteRepository.TableNoTracking.FirstOrDefault(x => x.Id == noteRequest.Id);
                if (note == null) return Result.Fail("This note does not exist.");
                var currentUserRole = _userService.GetCurrentUserRole();
                if (currentUserRole != "Admin")
                {
                    var currentUserId = _userService.GetCurrentUserId();
                    if (currentUserId != note.CreatedById)
                    {
                        return Result.Fail("Unauthorized");
                    }
                }
                if (noteRequest.Title != null)
                {
                    note.Title = noteRequest.Title;
                }
                if(noteRequest.Description != null)
                {
                    note.Description = noteRequest.Description;
                }
                if(noteRequest.Tags != null)
                {
                    note.Tags = noteRequest.Tags;
                }
                note.UpdatedOnUtc = DateTime.UtcNow;
                note.UpdatedById = _userService.GetCurrentUserId();
                await _noteRepository.UpdateAsync(note);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Could not update note.");
            }
        }
        /// <summary>
        /// Implements the <see cref="Delete(Guid)"/> method of interface.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> Delete(Guid id)
        {
            try
            {
                var note = _noteRepository.TableNoTracking.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                if (note == null) return Result.Fail("Could not find note.");
                var currentUserRole = _userService.GetCurrentUserRole();
                if (currentUserRole != "Admin")
                {
                    var currentUserId = _userService.GetCurrentUserId();
                    if (currentUserId != note.CreatedById)
                    {
                        return Result.Fail("Unauthorized");
                    }
                }
                note.IsDeleted = true;
                await _noteRepository.UpdateAsync(note);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Fail("Could not delete note.");
            }
        }
    }
}
