<template>
  <v-container class="fill-height">
    <v-responsive class="align-center text-center fill-height">
      
      <v-row class="d-flex align-center justify-center">
        <v-col cols="auto">
          <SquareButton msg="Product List" @click="GoToProductList"></SquareButton>
        </v-col>

        <v-col cols="auto">
          <SquareButton msg="Search By Name"></SquareButton>
        </v-col>

        <v-col cols="auto">
          <SquareButton msg="Search By Code"></SquareButton>
        </v-col>
      </v-row>
    </v-responsive>
  </v-container>
</template>

<script lang="ts" setup>
import SquareButton from '@/components/controls/SquareButton.vue'
import router from "@/router";

const GoToProductList = () => {
  console.log("product list")
  router.push("/productlist");
} 
  //
</script>
``