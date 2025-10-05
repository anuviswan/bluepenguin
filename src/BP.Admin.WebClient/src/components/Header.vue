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
          <!-- Products Dropdown -->
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
              <!-- Router links styled like Bootstrap items -->
              <li>
                <router-link
                    to="/d/p/create"
                    class="dropdown-item"
                >
                  Create Product
                </router-link>
              </li>

              <li>
                <router-link
                    to="/dashboard/p/list"
                    class="dropdown-item"
                >
                  View Products
                </router-link>
              </li>

              <li><hr class="dropdown-divider" /></li>

              <li>
                <router-link
                    to="/dashboard/p/stats"
                    class="dropdown-item"
                >
                  Product Stats
                </router-link>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useUserStore } from "../stores/userStore.ts";
import { Colors, type ColorKey, type ColorValue } from "../types/Colors.ts";
import { onMounted } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const userStore = useUserStore();

const getColorKeyByValue = (value: ColorValue): ColorKey => {
  const key = (Object.keys(Colors) as ColorKey[]).find(
      (k) => Colors[k] === value
  );
  if (!key) throw new Error(`Value ${value} not found in Colors`);
  return key;
};

onMounted(() => {
  if (!userStore.loggedInUser.token) {
    console.log("User not logged in, redirecting to home");
    router.push({ name: "Home" });
  }
});

const menuFontColor = Colors[getColorKeyByValue(Colors.PrimaryDark)];
</script>
