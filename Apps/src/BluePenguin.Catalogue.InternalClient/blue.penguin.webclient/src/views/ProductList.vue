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
import {  ref , onMounted } from "vue";
import {IProduct} from "@/types/UserTypes";
import ProductCard from "@/components/controls/ProductCard.vue"
import {productsApiService} from "@/apiservice/ProductApiService"


const getAllProducts = async () => await productsApiService.getAllProducts();
const products = ref<IGetAllProductsResponse>();
onMounted(async () =>{
  products.value = await getAllProducts();
});
</script>

<style scoped>
.cards{
  margin: 15;
  padding: 15;
}
</style>
