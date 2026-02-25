using System.ComponentModel.DataAnnotations;

namespace NoteApp.API.DTOs;

// DTOs for note-related operations
// These classes are used to transfer data between
// the client and the server for note management purposes
public class NoteCreateRequest
{
    // properties and validation attributes for creating a note
    [Required, MinLength(1), MaxLength(255)]
    public string Title { get; set; } = string.Empty; // Title of the note

    public string? Content { get; set; }

}

// DTO for updating a note
public class NoteUpdateRequest
{
    // properties and validation attributes for updating a note
    [Required, MinLength(1), MaxLength(255)]
    public string Title { get; set; } = string.Empty; // Updated title of the note
    
    public string? Content { get; set; } // Updated content of the note
}

// DTO for note response
public class NoteResponse
{
    // properties
    public Guid Id { get; set; } // Unique identifier for the note
    public string Title { get; set; } = string.Empty; // Title of the note
    public string? Content { get; set; } // Content of the note
    public DateTime CreatedAt { get; set; } // Timestamp when the note was created
    public DateTime UpdatedAt { get; set; } // Timestamp when the note was last updated
}

// DTO for note query parameters
public class NoteQueryParams
{
    // properties for search and sorting filters
    public string? Search { get; set; } // Search term for filtering notes by title or content
    public string? SortBy { get; set; } = "CreatedAt"; // Field to sort by (created_asc, created_desc, title_asc, title_desc)
}