<script setup lang="ts">
import Header from "../components/Header.vue";

</script>

<template>
  <div>
    <!-- Main content -->
    <main class="flex-1 p-6">
      <Header />
      <RouterView />
    </main>
  </div>
</template>

<style scoped></style>
