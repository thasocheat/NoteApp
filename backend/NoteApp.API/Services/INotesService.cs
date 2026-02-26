using NoteApp.API.DTOs;
using NoteApp.API.Models;

namespace NoteApp.API.Services;

// Interface for authentication service
// This interface defines the contract for authentication-related operations
// It includes methods for user registration, login, and token generation
public interface INotesService
{
    // Method to get all notes for a user, with optional search and sorting parameters
    Task<IEnumerable<NoteResponse>> GetAllAsync(Guid userId, NoteQueryParams query);

    // Method to get a note by its ID and user ID
    Task<NoteResponse?> GetByIdAsync(Guid id, Guid userId);

    // Method to create a new note for a user
    Task<NoteResponse> CreateAsync(Guid userId, NoteCreateRequest request);

    // Method to update an existing note by its ID and user ID
    Task<(bool Success, NoteResponse? Note)> UpdateAsync(Guid id, Guid userId, NoteUpdateRequest request);

    // Method to delete a note by its ID and user ID
    Task<bool> DeleteAsync(Guid id, Guid userId);
}