import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi } from '@/api/auth.api'
import type { User } from '@/types'


// Auth store using Pinia
export const useAuthStore = defineStore('auth', () => {
    // State
    const user = ref<User | null>(null)
    const loading = ref(false)
    const error = ref<string | null>(null)


    // Computed property to check if user is authenticated
    const isAuthenticated = computed(() => !!user.value)

    // Register a new user
    async function register(username: string, email: string, password: string){
        loading.value = true
        error.value = null
        // Call the API to register the user
        try{
          const {data} = await authApi.register({username, email, password})
          user.value = data
        }catch(e: any){
          error.value = e.response?.data?.message || 'Registration failed.'
          throw e
        }finally{
          loading.value = false
        }
    }

    // Login a user
    async function login(email: string, password: string) {
      loading.value = true
      error.value = null
      try {
        const { data } = await authApi.login({ email, password })
        user.value = data
      } catch (e: any) {
        error.value = e.response?.data?.message ?? 'Login failed.'
        throw e
      } finally {
        loading.value = false
      }
    }



    // Clear error
    function clearError(){
        error.value = null
    }







  // return the state and actions to be used in components
  return {
    user,
    loading,
    error,
    isAuthenticated,
    clearError,
    register,
    login,
  }
})