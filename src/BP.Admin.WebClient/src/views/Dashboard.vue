<script setup lang="ts">
import Header from "../components/Header.vue";
import Tile from "../components/Tile.vue";
import { type ColorKey, type ColorValue, Colors } from "../types/Colors.ts";

interface CategoryType {
  title: string;
  color: ColorKey;
  fontColor: ColorKey;
}
const getColorKeyByValue = (value: ColorValue): ColorKey => {
  const key = (Object.keys(Colors) as ColorKey[]).find(
    (k) => Colors[k] === value,
  );
  if (!key) throw new Error(`Value ${value} not found in Colors`);
  return key;
};

const categoryCollection: CategoryType[] = [
  {
    title: "Pendant",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Bracelet",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Necklace",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Clock",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Earings",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Coaster",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Home Decor",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Bag",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Key Chain",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Preservation",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
  {
    title: "Ring",
    color: getColorKeyByValue(Colors.PrimaryDark),
    fontColor: getColorKeyByValue(Colors.OffWhite),
  },
];
</script>

<template>
  <div>
    <!-- Fixed navbar -->
    <Header />

    <!-- Main container, pushed down -->
    <div class="container mt-5 pt-5">
      <div class="row g-4">
        <!-- 10 square tiles -->
        <div
          class="col-6 col-sm-4 col-md-3 col-lg-2"
          v-for="category in categoryCollection"
          :key="category.title"
        >
          <Tile
            :title="category.title"
            :color="category.color"
            :fontColor="category.fontColor"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
