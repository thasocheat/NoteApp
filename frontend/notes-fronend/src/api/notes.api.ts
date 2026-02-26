import api from './api'
import type { Note, NotesQuery } from '@/types'

// Notes API functions
export const notesApi = {
    // Get all notes with optional query parameters
    getAll: (params?: NotesQuery) =>
        api.get<Note[]>('/notes',),

    // Get a single note by ID
    getById: (id: string) =>
        api.get<Note>(`/notes/${id}`),

    // Create a new note
    create: (data: { title: string; content?: string }) =>
        api.post<Note>('/notes', data),

    // Update an existing note by ID
    update: (id: string, data: { title: string; content?: string }) =>
        api.put<Note>(`/notes/${id}`, data),

    // Delete a note by ID
    delete: (id: string) =>
        api.delete(`/notes/${id}`)
}