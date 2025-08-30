<script setup lang="ts">
import type {ColorKey} from "../types/Colors.ts";
import { Colors } from "../types/Colors.ts";

interface Props {
  title:string,
  color:ColorKey,
  fontColor:ColorKey,
}

const props = defineProps<Props>();
const backgroundColor = Colors[props.color];
const fontForeColor = Colors[props.fontColor];
</script>

<template>
  <div>
    <a href="#">
  <div
      class="tile d-flex flex-column justify-content-center align-items-center"
      :style="{ background: backgroundColor}"
  >

    <h3 class="tile-title text-center" :style="{color:fontForeColor}">{{ title }}</h3>

  </div>
    </a>
  </div>
</template>



<style scoped>
.tile {
  width: 100%;              /* desired width */
  aspect-ratio: 1 / 1;       /* square */
  border: 3px solid #000;
  box-shadow: 6px 6px 0 #000;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between; /* distribute title & content */
  align-items: center;
  font-family: 'Space Grotesk', sans-serif;
  overflow: hidden;          /* prevent overflow */
  text-align: center;
}
.tile:hover {
  transform: translate(-2px, -2px);
  box-shadow: 8px 8px 0px #000;
}

.tile-title {
  font-weight: bold;
  text-transform: uppercase;
  margin: 0;
  font-size: clamp(0.8rem, 2vw, 1.5rem);
  word-wrap: break-word;      /* wrap long words */
}

.tile-content {
  font-size: 1rem;
  font-weight: 400;
  color: #111;
}
</style>
