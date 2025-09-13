<template>
  <nav
    class="navbar navbar-expand-lg navbar-dark fixed-top py-3 border-bottom border-3"
    style="border-color: var(--bs-black)"
  >
    <div class="container-fluid">
      <!-- Brand -->
      <a class="navbar-brand" href="#">Blue Penguin</a>
    </div>
  </nav>
</template>

<script setup lang="ts">
import {useUserStore} from "../stores/userStore.ts";
import {onMounted} from "vue";
import {useRouter} from "vue-router";

const router  = useRouter();
const userStore = useUserStore();

onMounted(() => {
  if(!userStore.loggedInUser.token){
    console.log("User not logged in, redirecting to home")
    router.push({ name: "Home" });
  }
})

</script>

<style scoped></style>
