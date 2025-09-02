<template>
  <div class="d-flex align-items-center justify-content-center vh-50 bg-light">
    <div class="card neubrutalism-card p-4" style="width: 22rem;">
      <!-- Header -->
      <h1 class="text-center fw-bold mb-4">Login</h1>

      <!-- Form -->
      <form @submit.prevent="handleLogin">
        <!-- Email -->
        <div class="mb-3">
          <label for="email" class="form-label fw-bold">Email</label>
          <input
              type="email"
              id="email"
              v-model="email"
              required
              class="form-control neubrutalism-input"
          />
        </div>

        <!-- Password -->
        <div class="mb-4">
          <label for="password" class="form-label fw-bold">Password</label>
          <input
              type="password"
              id="password"
              v-model="password"
              required
              class="form-control neubrutalism-input"
          />
        </div>

        <!-- Button -->
        <button
            type="submit"
            class="btn btn-success w-100 fw-bold neubrutalism-btn"
        >
          Login
        </button>
      </form>

      <!-- Footer -->
      <p class="text-center mt-4">
        Don’t have an account?
        <a href="#" class="fw-bold text-decoration-underline text-primary">
          Sign up
        </a>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";

const email = ref("");
const password = ref("");

const handleLogin = () => {
  console.log("Email:", email.value);
  console.log("Password:", password.value);
  alert("Logged in (fake)!");
};
</script>

<style>
/* --- Neubrutalism style overrides --- */
.neubrutalism-card {
  border: 4px solid #000;
  border-radius: 1rem;
  box-shadow: 8px 8px 0px #000;
}

.neubrutalism-input {
  border: 3px solid #000 !important;
  border-radius: 0.75rem;
  box-shadow: 4px 4px 0px #000;
}

.neubrutalism-btn.btn {
  background-color: var(--action-button) !important;
  border: 3px solid var(--bs-black) !important;
  border-radius: 0.75rem !important;
  box-shadow: 4px 4px 0px var(--bs-black) !important;
  color: var(--bs-black) !important;
  transition: transform 0.15s, box-shadow 0.15s;
}

.neubrutalism-btn.btn:hover {
  transform: translate(3px, 3px);
  box-shadow: none !important;
}
</style>
