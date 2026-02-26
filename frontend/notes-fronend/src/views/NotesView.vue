<template>
  <div class="notes-page">

    <!-- Navbar -->
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

  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/useAuthStore'
import { useNotesStore } from '@/stores/useNotesStore'



const authStore = useAuthStore()
const notesStore = useNotesStore()
const router = useRouter()



// Dropdown states
const sortOpen     = ref(false)
const userOpen     = ref(false)




// Import global styles
import '@/assets/NoteView.css'


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




// function sort
function closeDropdowns()    { sortOpen.value = false; userOpen.value = false }
function selectSort(v: string){notesStore.setSortBy(v as any); sortOpen.value = false }





// Logout function
async function handleLogout(){
  await authStore.logout()
  router.push('/auth')
}
</script>
