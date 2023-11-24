<template>
   <v-card
    
    class="mx-auto my-12"
    max-width="374"
  >

    <!-- <v-img
      height="250"
      src="https://cdn.vuetifyjs.com/images/cards/cooking.png"
    ></v-img> -->

    <v-card-title>{{product.name}}</v-card-title>

    <v-card-text>

      <div class="my-4 text-subtitle-1">
        {{product.category}} , {{product.collection}}
      </div>

      <div>Small plates, salads & sandwiches - an intimate setting with 12 indoor seats plus patio seating.</div>
    </v-card-text>

    <v-divider class="mx-4"></v-divider>
    <v-card-subtitle>MRP Rs.354</v-card-subtitle>
    <v-card-subtitle>Discount Price Rs.354</v-card-subtitle>
    <v-divider class="mx-4"></v-divider>
    <v-card-text>
      <v-chip-group
        v-model="selection"
        active-class="deep-purple accent-4 white--text"
        column
      >
        <v-chip>Vintage</v-chip>

        <v-chip>Heart</v-chip>

        <v-chip>Antique</v-chip>

        <v-chip>Gold</v-chip>
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
