<template>
  <div class="note-card" @click="$emit('edit', note)">
    <div class="note-card-top">
      <h3 class="note-title">{{ note.title }}</h3>
      <div class="note-actions" @click.stop>
        <button @click="$emit('edit', note)" class="note-action-btn note-action-btn--edit" title="Edit">
          <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z"/>
          </svg>
        </button>
        <button @click="$emit('delete', note)" class="note-action-btn note-action-btn--delete" title="Delete">
          <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/>
          </svg>
        </button>
      </div>
    </div>

    <p v-if="note.content" class="note-preview">{{ note.content }}</p>
    <p v-else class="note-empty">No content</p>

    <div class="note-footer">
      <span class="note-date">{{ formatDate(note.createdAt) }}</span>
      <span v-if="isUpdated" class="note-edited">edited</span>
    </div>
  </div>
</template>




<script setup lang="ts">
import { computed } from 'vue'
import type { Note } from '@/types'

// Import global styles
import '@/assets/NoteCard.css'

const props = defineProps<{ note: Note }>()
defineEmits<{ edit: [note: Note]; delete: [note: Note] }>()

const isUpdated = computed(
  () => new Date(props.note.updatedAt).getTime() - new Date(props.note.createdAt).getTime() > 1000
)

function formatDate(iso: string): string {
  const d = new Date(iso)
  const diffDays = Math.floor((Date.now() - d.getTime()) / 86400000)
  if (diffDays === 0) return 'Today'
  if (diffDays === 1) return 'Yesterday'
  if (diffDays < 7)  return `${diffDays}d ago`
  return d.toLocaleDateString(undefined, { month: 'short', day: 'numeric', year: diffDays > 365 ? 'numeric' : undefined })
}
</script>