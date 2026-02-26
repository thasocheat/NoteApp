<template>

  <Teleport to="body">
    <div class="modal-backdrop" @click.self="$emit('close')">

      <Transition name="modal" appear>
        <div class="modal-box">

          <!-- Header -->
          <div class="modal-header">
            <h2 class="modal-title">{{ isEditing ? 'Edit Note' : 'New Note' }}</h2>
            <button @click="$emit('close')" class="modal-close-btn" aria-label="Close">
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
              </svg>
            </button>
          </div>

          <!-- Form -->
          <form @submit.prevent="handleSubmit" class="modal-form">

            <!-- Error -->
            <div v-if="error" class="modal-error">{{ error }}</div>

            <!-- Title -->
            <div class="modal-field">
              <label class="modal-label">Title <span class="modal-required">*</span></label>
              <input
                v-model="form.title" type="text" required maxlength="255" autofocus
                placeholder="Note title…" class="modal-input"
              />
              <p class="modal-char-count">{{ form.title.length }}/255</p>
            </div>

            <!-- Content -->
            <div class="modal-field modal-field--grow">
              <label class="modal-label">Content</label>
              <textarea
                v-model="form.content" rows="8"
                placeholder="Write your note here…" class="modal-textarea"
              />
            </div>

            <!-- Footer -->
            <div class="modal-footer">
              <button type="button" @click="$emit('close')" class="modal-cancel-btn">Cancel</button>
              <button type="submit" :disabled="saving || !form.title.trim()" class="modal-submit-btn">
                <svg v-if="saving" class="modal-spinner" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
                </svg>
                {{ saving ? 'Saving…' : isEditing ? 'Save Changes' : 'Create Note' }}
              </button>
            </div>

          </form>
        </div>
      </Transition>

    </div>
  </Teleport>

</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { useNotesStore } from '@/stores/useNotesStore'
import type { Note } from '@/types'

// Import global styles
import '@/assets/NoteModal.css'

// Import global styles
const props = defineProps<{ note: Note | null }>()
const emit = defineEmits<{ close: []; saved: [action: 'created' | 'updated'] }>()

// Local state
const notesStore = useNotesStore()
const saving = ref(false)
const error = ref<string | null>(null)
const isEditing = computed(() => !!props.note)

// Initialize form with note data if editing, or empty if creating new
const form = reactive({
  title: props.note?.title ?? '',
  content: props.note?.content ?? '',
})

// Handle form submission for creating/updating a note
async function handleSubmit() {
  if (!form.title.trim()) return
  saving.value = true
  error.value = null
    // If editing, call updateNote with the note ID; if creating, call createNote
  try {
    if (isEditing.value && props.note) {
      await notesStore.updateNote(props.note.id, form.title.trim(), form.content || undefined)
      emit('saved', 'updated')
    } else {
      await notesStore.createNote(form.title.trim(), form.content || undefined)
      emit('saved', 'created')
    }
  } catch (e: any) {
    error.value = e.response?.data?.message ?? 'Failed to save note.'
  } finally {
    saving.value = false
  }
}
</script>