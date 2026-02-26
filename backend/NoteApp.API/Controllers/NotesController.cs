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

    
}