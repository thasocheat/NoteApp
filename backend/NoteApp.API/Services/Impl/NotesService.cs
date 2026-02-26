using NoteApp.API.DTOs;
using NoteApp.API.Models;
using NoteApp.API.Repositories;

namespace NoteApp.API.Services.Impl;


// Implementation of the notes service
// This class provides the logic for managing notes,
// including creating, updating, retrieving, and deleting notes
public class NotesService : INotesService
{
    private readonly INoteRepository _noteRepository;

    // Constructor that takes a note repository as a dependency
    public NotesService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    // Method to get all notes for a user, with optional search and sorting parameters
    public async Task<IEnumerable<NoteResponse>> GetAllAsync(Guid userId, NoteQueryParams query)
    {
        // Retrieve notes from the repository based on the user ID and query parameters
        var notes = await _noteRepository.GetAllByUserAsync(userId, query.Search, query.SortBy);
        return notes.Select(MapToResponse);
    }

    // Method to get a note by its ID and user ID
    public async Task<NoteResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        // Retrieve the note from the repository based on the note ID
        // and user ID to ensure the note belongs to the user
        var note = await _noteRepository.GetByIdAsync(id, userId);
        return note is null ? null : MapToResponse(note);
    }

    // Method to create a new note for a user
    public async Task<NoteResponse> CreateAsync(Guid userId, NoteCreateRequest request)
    {
        // Create a new note object based on the request data
        var note = new Note
        {
            UserId = userId,
            Title = request.Title.Trim(),
            Content = request.Content?.Trim()
        };

        // Save the note to the repository
        // and return the created note as a response
        note.Id = await _noteRepository.CreateAsync(note);
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = note.CreatedAt;

        return MapToResponse(note);
    }

    // Method to update an existing note by its ID and user ID
    public async Task<(bool Success, NoteResponse? Note)> UpdateAsync(Guid id, Guid userId, NoteUpdateRequest request)
    {
        // Retrieve the existing note from the repository
        var existing = await _noteRepository.GetByIdAsync(id, userId);

        // Check if the note exists and belongs to the user
        if (existing is null) return (false, null);

        // Update the note's properties based on the request data
        existing.Title = request.Title.Trim();
        existing.Content = request.Content?.Trim();

        // Save the updated note to the repository
        var updated = await _noteRepository.UpdateAsync(existing);
        if (!updated) return (false, null);

        // Update the note's updated timestamp
        existing.UpdatedAt = DateTime.UtcNow;
        return (true, MapToResponse(existing));
    }


    // Method to delete a note by its ID and user ID
    public async Task<bool> DeleteAsync(Guid id, Guid userId)
        => await _noteRepository.DeleteAsync(id, userId);


    // method to note response object
    private NoteResponse MapToResponse(Note note) => new()
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content,
        CreatedAt = note.CreatedAt,
        UpdatedAt = note.UpdatedAt
    };
}
