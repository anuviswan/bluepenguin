<template>
    <v-btn elevation="2" 
        rounded="2" 
        size="x-large"
        color="primary">{{ msg}}</v-btn>
</template>

<script setup lang="ts">
const props = defineProps<{
    msg: String
}>();
</script>