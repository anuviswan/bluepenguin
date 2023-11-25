<template>
   <v-card
   prepend-icon="mdi-account"
   :title = product.name
   :subtitle = product.category
    class="mx-auto my-12"
    max-width="374"
  >

    <!-- <v-img
      height="250"
      src="https://cdn.vuetifyjs.com/images/cards/cooking.png"
    ></v-img> -->


    <v-card-text>

      <div class="my-4 text-subtitle-1">
        {{product.category}} , {{product.collection}}
      </div>

      <div>{{product.MRP}}</div>
    </v-card-text>

    <v-divider class="mx-4"></v-divider>
    <v-card-subtitle>MRP Rs.{{product.mrp}}</v-card-subtitle>
    <v-card-subtitle>Discount Price Rs.{{product.discountPrice}}</v-card-subtitle>
    <v-divider class="mx-4"></v-divider>
    <v-card-text>
      <v-chip-group
        v-model="selection"
        active-class="deep-purple accent-4 white--text"
        column
      >
      <div v-for="item in product.tags.Tag" :key="item.name">
          <div  class="mx-4">
            <v-chip>{{item}}</v-chip>
          </div>
        </div>
      </v-chip-group>
    </v-card-text>

    <v-card-actions>
      <v-btn
        color="deep-purple lighten-2"
        text
        
      >
        Details
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { IProduct } from "@/types/UserTypes";

const props = defineProps<{
    product : IProduct
}>();

</script>
