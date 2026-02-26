using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.API.DTOs;
using NoteApp.API.Services;
using System.Security.Claims;

namespace NoteApp.API.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    // Constructor to inject the notes service
    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    // Private property to get the current user's ID from the JWT claims
    private Guid CurrentUserId => 
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException("User ID claim not found"));

    // GET: api/notes
    // This endpoint retrieves all notes for the authenticated user
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] NoteQueryParams query)
    {
        var notes = await _notesService.GetAllAsync(CurrentUserId, query);
        return Ok(notes);
    }

    // GET: api/notes/{id}
    // This endpoint retrieves a specific note by its ID for the authenticated user
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var note = await _notesService.GetByIdAsync(id, CurrentUserId);
        return note is null ? NotFound() : Ok(note);
    }

    // POST: api/notes
    // This endpoint creates a new note for the authenticated user
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NoteCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var note = await _notesService.CreateAsync(CurrentUserId, request);
        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    // PUT: api/notes/{id}
    // This endpoint updates an existing note by its ID for the authenticated user
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] NoteUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (success, note) = await _notesService.UpdateAsync(id, CurrentUserId, request);
        return success ? Ok(note) : NotFound();
    }

    // DELETE: api/notes/{id}
    // This endpoint deletes a specific note by its ID for the authenticated user
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _notesService.DeleteAsync(id, CurrentUserId);
        return deleted ? NoContent() : NotFound();
    }

    
}