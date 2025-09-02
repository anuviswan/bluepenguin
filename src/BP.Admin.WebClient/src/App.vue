<template>
  <div id="app">


    <!-- Routed views appear here -->
    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>


<script setup lang="ts">
</script>

<style scoped>

.main-content {
  padding-top: 70px; /* match your Header height */
}
</style>
