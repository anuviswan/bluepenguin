<template>
  <v-container class="fill-height">
    <v-responsive class="align-center text-center fill-height">
      <v-row class="d-flex align-center justify-center">
        <v-flex v-for="image in images" :key="image.num">
          <v-card color="gray" dark width="150px">
            <v-card-text>{{ image.name }}</v-card-text>
            <v-card-media :src="image.url" height="200px"></v-card-media>
            <v-card-actions>
              <v-btn icon><v-icon>arrow_left</v-icon></v-btn>
              <v-spacer></v-spacer>
              <v-tooltip bottom>
                <v-btn icon flat color="red"><v-icon>favorite</v-icon></v-btn>
              </v-tooltip>
              <v-spacer></v-spacer>
              <v-btn icon><v-icon>arrow_right</v-icon></v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>
      </v-row>
    </v-responsive>
  </v-container>
</template>

<script setup lang="ts">
interface IProduct {
  name: string;
  url: string;
  category: string;
  collection: string;
}

const images: IProduct[] = [...Array(10).keys()].map(
  (v, i) =>
    <IProduct>{
      name: "Demo " + i,
      url: `https://picsum.photos/300/400?image=${i + 1}`,
      category: "Category " + i,
      collection: "Collection " + i,
    }
);
</script>
