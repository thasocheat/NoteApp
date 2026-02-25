namespace NoteApp.API.Models;

// Note model representing a note created by a user
public class Note
{
    // properties
    public Guid Id { get; set; } // Unique identifier for the note
    public Guid UserId { get; set; } // Foreign key to the User model
    public string Title { get; set; } = string.Empty; // Title of the note
    public string? Content { get; set; } // Content of the note (optional)
    public DateTime CreatedAt { get; set; } // Timestamp when the note was created
    public DateTime UpdatedAt { get; set; } // Timestamp when the note was last updated

    
}