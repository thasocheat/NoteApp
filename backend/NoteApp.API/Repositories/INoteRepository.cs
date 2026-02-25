using NoteApp.API.Models;

namespace NoteApp.API.Repositories;

// Interface for note repository
// This interface defines the contract for note-related data operations
// It includes methods for retrieving notes by user ID, retrieving a note by its ID, creating
// updating, and deleting notes
// It talks to the database and performs operations related to note data
public interface INoteRepository
{
    // Method to get all notes by user ID, with optional search and sorting parameters
    Task<IEnumerable<Note>> GetAllByUserAsync(Guid userId, string? search, string sortBy);

    // Method to get a note by its ID
    Task<Note?> GetByIdAsync(Guid id, Guid userId);

    // Method to create a new note
    Task<Guid> CreateAsync(Note note);

    // Method to update an existing note
    Task<bool> UpdateAsync(Note note);

    // Method to delete a note by its ID and user ID
    Task<bool> DeleteAsync(Guid id, Guid userId);
}