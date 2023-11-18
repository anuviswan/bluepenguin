<template>
  <v-container class="fill-height">
    <v-responsive class="align-center text-center fill-height">
      
      <v-row class="d-flex align-center justify-center">
        <v-col cols="auto">
          <SquareButton msg="Product List"></SquareButton>
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
import SquareButton from './controls/SquareButton.vue';

  //
</script>
