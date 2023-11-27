<template>
  <v-card elevation="5" rounded="5" class="mx-auto my-12" max-width="374">
    <v-card-item>
      <v-card
        :color="titleColor"
        text-center
        :title="product.name"
        :subtitle="product.category"
      >
      </v-card>
    </v-card-item>
    <v-card-item>
      <v-layout align-center>
      <v-img
        :width="300"
        aspect-ratio="16/9"
        cover
        src="https://cdn.vuetifyjs.com/images/parallax/material.jpg"
      ></v-img>
      </v-layout>
    </v-card-item>

    <v-card-item>
      <v-chip-group
        active-class="deep-purple accent-4 white--text"
        column
      >
        <div v-for="item in product.tags.tag" :key="item">
          <div class="mx-4">
            <v-chip>{{ item }}</v-chip>
          </div>
        </div>
      </v-chip-group>
    </v-card-item>
    <v-divider thickness="2"></v-divider>
    <v-container>
      <v-row>
        <v-col align-self="start">
          <v-btn color="red">Rs.{{ product.mrp }}</v-btn>
        </v-col>
        <v-spacer></v-spacer>
        <v-col align-self="end">
          <v-btn color="red-accent-1">Rs.{{ product.discountPrice }}</v-btn>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script setup lang="ts">
import { IProduct } from "@/types/UserTypes";
import { computed } from "vue";

const props = defineProps<{
  product: IProduct;
}>();

const titleColor = computed(() => {
  switch (props.product.category) {
    case "CANVAS":
      return "blue";

    case "CLOCK":
      return "indigo";

    case "PENDANT":
      return "teal";
    default:
      return "secondary";
  }
});
</script>
