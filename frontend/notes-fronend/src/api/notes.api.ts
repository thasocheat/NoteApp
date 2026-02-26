import api from './api'
import type { Note, NotesQuery } from '@/types'

// Notes API functions
export const notesApi = {
    // Get all notes with optional query parameters
    getAll: (params?: NotesQuery) =>
        api.get<Note[]>('/notes',)
}