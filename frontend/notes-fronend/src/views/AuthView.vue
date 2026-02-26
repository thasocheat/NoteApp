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

        </Transition>

        <!-- Login form -->
        <Transition name="fade-slide" mode="out-in">
          <form v-if="activeTab == 'login'" key="login">

            <!-- Header -->
            <div class="auth-form-header">
              <h2 class="auth-form-title">Welcome Back</h2>
              <p class="auth-form-desc">Login to access your notes</p>
            </div>

            <!-- Fields -->
            <div class="auth-field">
              <label class="auth-label">Email address</label>
              <input type="email" placeholder="you@example.com" class="auth-input" />
            </div>

            <div class="auth-field">
              <label for="password" class="auth-label">Password</label>
              <input type="password" placeholder="••••••••" class="auth-input" />
            </div>

            <!-- Button login -->
            <button type="submit" class="auth-submit-btn">Login</button>
            <p class="auth-switch">
              Don't have an account?
              <button @click="switchTab('register')">Register</button>
            </p>

          </form>
        </Transition>

      </div>


     </div>
    
  </div>
</template>



<script setup lang="ts">
import { ref, reactive, computed } from 'vue'


import searchIcon from '@/assets/search.png'
import syncIcon from '@/assets/sync.png'
import secureIcon from '@/assets/secure.png'
import responsiveIcon from '@/assets/responsive.png'

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


// switch between login and register
function switchTab(tab: 'login' | 'register'){
  activeTab.value = tab
}

</script>
