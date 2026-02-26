<template>
  <div class="auth-page">

    <!-- Left branding panel (desktop) -->
     <div class="auth-brand">
      <div class="auth-brand-inner">
        <img src="@/assets/notes.png" alt="NotesApp Logo" class="auth-brand-logo" />
        <h1 class="auth-brand-title">Your ideas,<br/>Your notes</h1>
        <p class="auth-brand-desc">Your personal note-taking companion</p>
        <div class="auth-feature-grid">
          <div
            v-for="f in features"
            :key="f.name"
            class="auth-feature-card"
          >
            <div class="auth-feature-icon-wrap">
              <img :src="f.icon" :alt="f.name" />
            </div>

            <div class="auth-feature-content">
              <h3 class="auth-feature-name">{{ f.name }}</h3>
              <p class="auth-feature-desc">{{ f.desc }}</p>
            </div>

          </div>
        </div>
      </div>
    </div>


    <!-- Right form panel -->
     <div class="auth-from-panel">

      <!-- Mobile logo -->
      <div class="auth-mobile-logo">
        <img src="@/assets/notes.png" alt="NotesApp Logo" class="auth-mobile-logo-img" />
        <span class="auth-mobile-logo-title">NotesApp</span>
      </div>

      <!-- Card form -->
      <div class="auth-card">

        <!-- Tab switcher -->
        <div class="auth-tabs">
          <button
            v-for="tab in tabs"
            :key="tab.key" @click="switchTab(tab.key)"
            :class="['auth-tab-btn', activeTab === tab.key ? 'auth-tab-btn--active' : '']"
          >
            {{ tab.label }}
          </button>
        </div>

        <!-- Error banner -->
        <Transition name="slide-down">
          <div v-if="authStore.error" class="auth-error">
            {{ authStore.error }}
          </div>
        </Transition>

        <!-- Login and Register form -->
        <Transition name="fade-slide" mode="out-in">

          <!-- Login -->
          <form v-if="activeTab == 'login'" key="login" @submit.prevent="handleLogin">

            <!-- Header -->
            <div class="auth-form-header">
              <h2 class="auth-form-title">Welcome Back</h2>
              <p class="auth-form-desc">Login to access your notes</p>
            </div>

            <!-- Fields -->
            <div class="auth-field">
              <label class="auth-label">Email address</label>
              <input v-model="loginForm.email" type="email" required autocomplete="email"
                placeholder="you@example.com" class="auth-input" />
            </div>

            <div class="auth-field">
              <label for="password" class="auth-label">Password</label>
              <input v-model="loginForm.password" type="password" required autocomplete="current-password"
                placeholder="••••••••" class="auth-input" />
            </div>

            <!-- Button login -->
            <button type="submit" :disabled="authStore.loading" class="auth-submit-btn">
              <svg v-if="authStore.loading" class="auth-spinner" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
              </svg>
              {{ authStore.loading ? 'Signing in…' : 'Sign In' }}
            </button>
            <p class="auth-switch">
              Don't have an account?
              <button type="button" @click="switchTab('register')" class="auth-switch-link">Register</button>
            </p>

          </form>

          <!-- Register -->
          <form v-else key="register" @submit.prevent="handleRegister">

            <!-- Header -->
            <div class="auth-form-header">
              <h2 class="auth-form-title">Create Account</h2>
              <p class="auth-form-desc">Register to get started</p>
            </div>

            <!-- Fields -->
            <div class="auth-field">
              <label class="auth-label">Username</label>
              <input v-model="registerForm.username" type="text" required autocomplete="username"
                placeholder="yourname" class="auth-input" />
            </div>

            <div class="auth-field">
              <label class="auth-label">Email address</label>
              <input v-model="registerForm.email" type="email" required autocomplete="email"
                placeholder="you@example.com" class="auth-input" />
            </div>

            <div class="auth-field">
              <label class="auth-label">Password</label>
              <input v-model="registerForm.password" type="password" required autocomplete="new-password"
                placeholder="Min. 6 characters" class="auth-input" />
            </div>

            <!-- password strength bar -->
            <div class="auth-strength-bar">
              <div v-for="i in 4" :key="i" :class="['auth-strength-seg', strengthClass(i)]" />
            </div>

            <!-- Button register -->
            <button type="submit" :disabled="authStore.loading" class="auth-submit-btn">
              <svg v-if="authStore.loading" class="auth-spinner" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
              </svg>
              {{ authStore.loading ? 'Creating account…' : 'Create Account' }}
            </button>
            <p class="auth-switch">
              Already have an account?
              <button type="button" @click="switchTab('login')" class="auth-switch-link">Sign in</button>
            </p>

          </form>
        </Transition>

      </div>
     </div>
  </div>
</template>



<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/useAuthStore'


import searchIcon from '@/assets/search.png'
import syncIcon from '@/assets/sync.png'
import secureIcon from '@/assets/secure.png'
import responsiveIcon from '@/assets/responsive.png'

// define router and auth store
const router = useRouter()
const authStore = useAuthStore()

// Import global styles
import '@/assets/AuthView.css'


// Image name and icon
const features = [
  {name: 'Smart Search', icon: searchIcon, desc: 'Find your notes instantly'},
  {name: 'Real-time Sync', icon: syncIcon, desc: 'Always up to date'},
  {name: 'Secure', icon: secureIcon, desc: 'Encrypted & private'},
  {name: 'Responsive', icon: responsiveIcon, desc: 'Works on any device'},
]

// tab switcher state
const tabs = [
  { key: 'login', label: 'Login' },
  { key: 'register', label: 'Register' },
] as const

const activeTab = ref<'login' | 'register'>('login')
const loginForm    = reactive({ email: '', password: '' })
const registerForm = reactive({
  username: '',
  email: '',
  password: '',
})


// switch between login and register
function switchTab(tab: 'login' | 'register'){
  authStore.clearError()
  activeTab.value = tab
}

// handle login
async function handleLogin() {
  authStore.clearError()
  try {
    await authStore.login(loginForm.email, loginForm.password)
    router.push('/notes')
  } catch (e) {
    console.error(e)
  }
}


// handle register
async function handleRegister(){
  authStore.clearError()
  try{
    await authStore.register(registerForm.username, registerForm.email, registerForm.password)
    router.push('/notes')
  }catch(e){
    console.log(e)
  }
}




// compute password strength (0-4)
const passwordStrength = computed(() => {
  // strength criteria: length >= 6, has uppercase, has number, has special char
  const p = registerForm.password
  if (!p) return 0
  let s = 0
  if (p.length >= 6) s++
  if (/[A-Z]/.test(p)) s++
  if (/[0-9]/.test(p)) s++
  if (/[^A-Za-z0-9]/.test(p)) s++
  return s
})

// return css class for strength bar segment
function strengthClass(level: number) {

  // if password is empty or doesn't meet strength level, return empty (gray)
  if (!registerForm.password) return ''
  if (passwordStrength.value < level) return ''
  if (passwordStrength.value <= 2) return 'auth-strength-seg--weak'
  if (passwordStrength.value === 3) return 'auth-strength-seg--ok'
  return 'auth-strength-seg--strong'
}

</script>
