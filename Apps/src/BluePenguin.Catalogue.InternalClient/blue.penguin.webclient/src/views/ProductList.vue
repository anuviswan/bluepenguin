<template>
  <v-container class="fill-height" fluid grid-list-xl grid-list-md>
    <v-layout wrap justify-space-around>
      <v-row class="d-flex align-center justify-center">
        <div v-for="item in products" :key="item.name">
          <div  class="mx-4">
            <ProductCard :product=item />
          </div>
        </div>
      </v-row>
    </v-layout>
  </v-container>
</template>

<script setup lang="ts">
import {IProduct} from "@/types/UserTypes";
import ProductCard from "@/components/controls/ProductCard.vue";
const products: IProduct[] = [...Array(10).keys()].map(
  (v, i) =>
    <IProduct>{
      name: "Demo " + i,
      url: `https://picsum.photos/300/400?image=${i + 1}`,
      category: "Category " + i,
      collection: "Collection " + i,
    }
);
</script>

<style scoped>
.cards{
  margin: 15;
  padding: 15;
}
</style>
