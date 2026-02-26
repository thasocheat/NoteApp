// Types for the NoteApp frontend
export interface User{
    id: string
    username: string
    email: string
}

// Note interface
export interface Note{
    id: string
    title: string
    content: string
    createdAt: string
    updatedAt: string
}

// Api Error interface
export interface ApiError{
    message: string
}

// Notes query parameters for filtering and sorting
export interface NotesQuery{
    search?: string
    sortBy?: 'created_desc' | 'created_asc' | 'title_desc' | 'title_asc'
}