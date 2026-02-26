<template>
  <Teleport to="body">
    <div class="confirm-backdrop" @click.self="$emit('cancel')">
      <Transition name="pop" appear>
        <div class="confirm-box">
          <div class="confirm-icon-wrap">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z"/>
            </svg>
          </div>
          <h3 class="confirm-title">{{ title }}</h3>
          <p class="confirm-message" v-html="message" />
          <div class="confirm-actions">
            <button @click="$emit('cancel')" class="confirm-cancel-btn">Cancel</button>
            <button @click="$emit('confirm')" class="confirm-delete-btn">{{ confirmLabel ?? 'Confirm' }}</button>
          </div>
        </div>
      </Transition>
    </div>
  </Teleport>
</template>

<script setup lang="ts">

// Import global styles
import '@/assets/ConfirmDialog.css'

defineProps<{ title: string; message: string; confirmLabel?: string }>()
defineEmits<{ confirm: []; cancel: [] }>()
</script>




