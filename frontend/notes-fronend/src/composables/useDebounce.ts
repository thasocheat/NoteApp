import { ref } from 'vue'

// Utility function to create a debounced version of a function
export function useDebounce<T>(fn: (val: T) => void, delay = 300) {
    // Ref to hold the timer ID
  const timer = ref<ReturnType<typeof setTimeout> | null>(null)

    // Return a new function that will debounce the original function
  return (val: T) => {
    // If there's an existing timer, clear it
    if (timer.value) clearTimeout(timer.value)
    // Set a new timer to call the original function after the specified delay
    timer.value = setTimeout(() => fn(val), delay)
  }
}
