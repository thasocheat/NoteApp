<template>
  <div class="notes-page">

    <!-- Navbar -->
    <nav class="notes-nav">
      <!-- brand + controls -->
    <div class="nav-row1">

      <!-- Brand -->
      <div class="nav-brand">
        <img src="@/assets/notes.png" alt="NoteApp Logo" class="nav-brand-img" />
        <span class="nav-brand-text">NoteApp</span>
      </div>

      <!-- Controls -->
      <div class="nav-controls">

        <!-- Sort dropdown -->
        <div class="dropdown-wrap">

          <button @click.stop="sortOpen = !sortOpen; userOpen = false" class="ctrl-btn" :class="{ 'ctrl-btn--active': sortOpen}">
            <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4h13M3 8h9m-9 4h9m5-4v12m0 0l-4-4m4 4l4-4"/>
            </svg>
            <span class="ctrl-btn-label">{{ activeSortLabel }}</span>
             <svg class="ctrl-chevron" :class="{ 'ctrl-chevron--up': sortOpen }" xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M19 9l-7 7-7-7"/>
              </svg>
          </button>

          <!-- Sort options dropdown -->
           <Transition name="drop">
              <div v-if="sortOpen" class="dropdown-panel">
                <button v-for="opt in sortOptions" :key="opt.value"
                  @click.stop="selectSort(opt.value)"
                  :class="['dropdown-item', notesStore.sortBy === opt.value ? 'dropdown-item--active' : '']">
                  <span>{{ opt.label }}</span>
                  <svg v-if="notesStore.sortBy === opt.value" class="dropdown-check" xmlns="http://www.w3.org/2000/svg" width="13" height="13" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414L8.414 15l-4.121-4.121a1 1 0 011.414-1.414L8.414 12.172l7.879-7.879a1 1 0 011.414 0z" clip-rule="evenodd" />
                  </svg>
                </button>
              </div>
            </Transition>

        </div>

        <!-- Note count -->
        <div class="nav-badge">
          <span class="nav-badge-num">{{ notesStore.filteredNotes.length }} </span>
          <span class="nav-badge-label"> {{ notesStore.filteredNotes.length === 1 ? 'note' : 'notes' }}</span>
        </div>

        <!-- User profile -->
        <div class="dropdown-wrap">

          <button @click.stop="userOpen = !userOpen; sortOpen = false" class="user-avatar" title="User Profile">
            {{ userInitial }}
          </button>

          <!-- User dropdown panel -->
          <Transition name="drop">
            <div v-if="userOpen" class="dropdown-panel">
              <div class="user-info">
                <div class="user-info-avatar">{{ userInitial }}</div>
                <div>
                  <p class="user-info-name">{{ authStore.user?.username }}</p>
                  <p class="user-info-email">{{ authStore.user?.email }}</p>
                </div>
              </div>
              <div class="dropdown-divider" />
              <button @click="handleLogout" class="dropdown-item dropdown-item--danger">
                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a2 2 0 01-2 2H5a2 2 0 01-2-2V7a2 2 0 012-2h6a2 2 0 012 2v1"/>
                </svg>
                Sign out
              </button>
            </div>
          </Transition>

        </div>

      </div>

    </div>

    <!-- Search bar -->
    <div class="nav-row2">
      <div class="nav-search-wrap">
        <svg class="nav-search-icon" xmlns="http://www.w3.org/2000/svg" width="15" height="15" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-4.35-4.35M17 11A6 6 0 115 11a6 6 0 0112 0z" />
        </svg>
        <input
          :value="notesStore.search"
          @input="onSearch"
          type="text"
          placeholder="Search your notes…"
          class="nav-search-input"
        />
        <button v-if="notesStore.search" @click="clearSearch" class="nav-search-clear" aria-label="Clear">
          <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
          </svg>
        </button>
      </div>
    </div>

  </nav>

  <!-- Content -->
  <main class="notes-content" @click="closeDropdowns">

    <!-- Loading -->
    <div v-if="notesStore.loading" class="state-center">
      <svg class="state-spinner" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
      </svg>
      <span class="state-text">Loading your notes…</span>
    </div>

    <!-- Error -->
    <div v-else-if="notesStore.error" class="state-center">
      <div class="state-emoji">⚠️</div>
      <p class="state-text">{{ notesStore.error }}</p>
      <button @click="notesStore.fetchNotes()" class="retry-btn">Try again</button>
    </div>

    <!-- Notes grid (alway shown includes Add card) -->
     <div>

      <!-- Grid header -->
      <div class="grid-header" v-if="notesStore.filteredNotes.length > 0 || notesStore.search">
        <h2 class="grid-header-title">
          {{ notesStore.search ? `Results for "${notesStore.search}"` : 'All Notes' }}
        </h2>
        <span class="grid-header-badge">{{ notesStore.filteredNotes.length }} {{ notesStore.filteredNotes.length === 1 ? 'note' : 'notes' }}</span>
      </div>
      
      <!-- Notes grid -->
      <div class="notes-grid">
        <!-- Add New Note card — always first, hidden when searching -->
        <button v-if="!notesStore.search" @click="openCreate" class="add-card">
            <div class="add-card-icon">
              <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.75" d="M12 4v16m8-8H4"/>
              </svg>
            </div>
            <span class="add-card-label">New Note</span>
          </button>

          <!-- Note cards -->
          <NoteCard
            v-for="note in notesStore.filteredNotes"
            :key="note.id"
            :note="note"
            @edit="openEdit"
            @delete="openDelete"
          />
      </div>

     </div>

  </main>

  <!-- Note Modal -->
  <NoteModal
    v-if="showModal"
    :note="editingNote"
    @close="closeModal"
    @saved="handleSaved"
  />

  <!-- Confirm delete -->
  <ConfirmDialog
    v-if="deletingNote"
    title="Delete Note"
    :message="`Are you sure you want to delete &quot;${deletingNote.title}&quot;? This cannot be undone.`"
    confirm-label="Delete"
    @confirm="confirmDelete"
    @cancel="deletingNote = null"
  />




  <!-- Toast -->
  <Transition name="toast">
    <div v-if="toast" :class="['notes-toast', `notes-toast--${toast.type}`]">
      <svg v-if="toast.type === 'success'" xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 20 20" fill="currentColor">
        <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414L8.414 15l-4.121-4.121a1 1 0 011.414-1.414L8.414 12.172l7.879-7.879a1 1 0 011.414 0z" clip-rule="evenodd" />
      </svg>
      {{ toast.message }}
    </div>
  </Transition>

  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/useAuthStore'
import { useNotesStore } from '@/stores/useNotesStore'
import { useDebounce } from '@/composables/useDebounce'

import NoteCard from '@/components/notes/NoteCard.vue'
import NoteModal from '@/components/notes/NoteModal.vue'
import ConfirmDialog from '@/components/ui/ConfirmDialog.vue'

import type { Note } from '@/types'

// Import global styles
import '@/assets/NoteView.css'

// define stores and router
const authStore = useAuthStore()
const notesStore = useNotesStore()
const router = useRouter()


// Create/edit note modal state
const showModal = ref(false)
const editingNote = ref<Note | null>(null)
const deletingNote = ref<Note | null>(null)
// Dropdown states
const sortOpen     = ref(false)
const userOpen     = ref(false)


// toast
const toast = ref<{ message: string; type: 'success' | 'error' } | null>(null)
let toastTimer: ReturnType<typeof setTimeout> | null = null

function showToast(message: string, type: 'success' | 'error' = 'success') {
  // Set toast message and type
  if (toastTimer) clearTimeout(toastTimer)
  // Set toast message and type
  toast.value = { message, type }
  toastTimer = setTimeout(() => (toast.value = null), 3000)
}


// user initial for avatar
const userInitial = computed(() => (authStore.user?.username?.[0] ?? '?').toUpperCase())


// sort options
const sortOptions = [
  { value: 'created_desc', label: 'Newest first' },
  { value: 'created_asc', label: 'Oldest first' },
  { value: 'updated_desc', label: 'Recently updated' },
  { value: 'updated_asc', label: 'Least recently updated' },
]

// Active sort label
const activeSortLabel = computed(
  () => sortOptions.find(o => o.value === notesStore.sortBy)?.label ?? 'Sort'
)



// search input handler
const debouncedSearch = useDebounce((val: string) => notesStore.setSearch(val), 300)
function onSearch(e: Event){debouncedSearch((e.target as HTMLInputElement).value)}
// Clear search input
function clearSearch(){notesStore.setSearch('')}


// function sort
// to close dropdowns when clicking outside and select sort option
function closeDropdowns()    { sortOpen.value = false; userOpen.value = false }

// select sort option
function selectSort(v: string){notesStore.setSortBy(v as any); sortOpen.value = false }


// Open create note modal
function openCreate(){
  editingNote.value = null
  showModal.value = true
}

// Close modal and reset editing note
function closeModal(){
  editingNote.value = null
  showModal.value = false
}

// Open edit note modal with selected note
function openEdit(n: Note){
  editingNote.value = n
  showModal.value = true
}

// Open delete confirmation dialog
function openDelete(n: Note){
  deletingNote.value = n
}

// Handel save note
function handleSaved(action: 'created' | 'updated'){
  closeModal()
  showToast(`Note ${action === 'created' ? 'created' : 'updated'} successfully!`)
}


// Confirm delete note
async function confirmDelete(){
  // safety check
  if (!deletingNote.value) return
  try {
    const title = deletingNote.value.title
    await notesStore.deleteNote(deletingNote.value.id)
    showToast(`"${title}" deleted successfully!`)
  } catch (e) {
    showToast('Failed to delete note.', 'error')
  } finally {
    deletingNote.value = null
  }
}


// Logout function
async function handleLogout(){
  await authStore.logout()
  router.push('/auth')
}

// fetch notes and set up reactivity on mount
// to ensure notes are loaded when user visits the page and react to changes in the store
onMounted(() => notesStore.fetchNotes())
</script>
