import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { notesApi } from '@/api/notes.api'
import type { Note, NotesQuery } from '@/types'


// Notes store using Pinia
export const useNotesStore = defineStore('notes', () => {

    // State
    const notes = ref<Note[]>([])

    // Loading and error states
    const loading = ref(false)
    const error = ref<string | null>(null)

    const search = ref('')
    // Sort
    const sortBy = ref<NotesQuery['sortBy']>('created_desc')

    // Filtered notes
    const filteredNotes = computed(() => {
        // Apply search filter
        const q = search.value.trim().toLowerCase()
        // check if search query is empty
        if(!q) return notes.value
        // Filter notes by title or content
        return notes.value.filter(
            (n) => n.title.toLowerCase().includes(q) || n.content?.toLowerCase().includes(q)
        )
    })


    // Fetch notes with optional query parameters
    async function fetchNotes(){
        loading.value = true
        error.value = null
        // Call the API to fetch notes
        try{
          const {data} = await notesApi.getAll({sortBy: sortBy.value})
          notes.value = data
        }catch(e: any){
            error.value = e.response?.data?.message || 'Failed to fetch notes.'
        }finally{
            loading.value = false
        }

    }



    function setSearch(q: string) {search.value = q}
    function setSortBy(s: NotesQuery['sortBy']) {
        sortBy.value = s
        fetchNotes()
    }

    // Return the store's state and actions
    return {
        notes,
        loading,
        error,
        sortBy,
        search,
        filteredNotes,
        fetchNotes,
        setSearch,
        setSortBy,
    }
})