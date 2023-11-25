<template>
  <v-card elevation="2" rounded="5"  class="mx-auto my-12" max-width="374">
    <v-card-item>
      <v-card
        :color="titleColor"
        text-center
        :title="product.name"
        :subtitle="product.category"
      >
      </v-card>
    </v-card-item>

    <v-card-text>
      <v-chip-group
        v-model="selection"
        active-class="deep-purple accent-4 white--text"
        column
      >
        <div v-for="item in product.tags.Tag" :key="item.name">
          <div class="mx-4">
            <v-chip>{{ item }}</v-chip>
          </div>
        </div>
      </v-chip-group>
    </v-card-text>
    <v-divider thickness="2"></v-divider>
    <v-card-subtitle>MRP Rs.{{ product.mrp }}</v-card-subtitle>
    <v-card-subtitle>Discount Price Rs.{{ product.discountPrice }}</v-card-subtitle>
  </v-card>
</template>

<script setup lang="ts">
import { IProduct } from "@/types/UserTypes";
import {computed} from "vue"

const props = defineProps<{
  product: IProduct;
}>();

const titleColor = computed(()=> {
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
})

</script>
