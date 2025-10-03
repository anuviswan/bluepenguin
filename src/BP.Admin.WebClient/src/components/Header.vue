<template>
  <nav
      class="navbar navbar-expand-lg navbar-dark fixed-top py-3 border-bottom border-3"
      style="border-color: var(--bs-black)"
  >
    <div class="container-fluid">
      <!-- Brand -->
      <a class="navbar-brand" href="#" :style="{ color: menuFontColor }">Blue Penguin</a>

      <!-- Toggler for mobile -->
      <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <!-- Collapsible nav -->
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav mb-2 mb-lg-0">
          <li class="nav-item dropdown">
            <a
                class="nav-link dropdown-toggle"
                href="#"
                role="button"
                data-bs-toggle="dropdown"
                aria-expanded="false"
                :style="{ color: menuFontColor }"
            >
              Products
            </a>
            <ul class="dropdown-menu">
              <li ><a class="dropdown-item" href="#"  :style="{
        '--hover-bg': menuFontColor
      }">Create</a></li>
              <li><a class="dropdown-item" href="#">Another action</a></li>
              <li><hr class="dropdown-divider" /></li>
              <li><a class="dropdown-item" href="#">Something else here</a></li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>



<script setup lang="ts">
import {useUserStore} from "../stores/userStore.ts";
import { type ColorKey, type ColorValue, Colors } from "../types/Colors.ts";
import {onMounted} from "vue";
import {useRouter} from "vue-router";

const router  = useRouter();
const userStore = useUserStore();


const getColorKeyByValue = (value: ColorValue): ColorKey => {
  const key = (Object.keys(Colors) as ColorKey[]).find(
      (k) => Colors[k] === value,
  );
  if (!key) throw new Error(`Value ${value} not found in Colors`);
  return key;
};

onMounted(() => {
  if(!userStore.loggedInUser.token){
    console.log("User not logged in, redirecting to home")
    router.push({ name: "Home" });
  }
})
const menuFontColor = Colors[getColorKeyByValue(Colors.PrimaryDark)];
console.log(menuFontColor)
</script>

<style scoped></style>
