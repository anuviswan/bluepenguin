<script setup lang="ts">
import { ref } from "vue";

const form = ref({
  name: "",
  category: "",
  price: 0,
  description: "",
});

const isSubmitting = ref(false);
const successMessage = ref("");



const handleSubmit = async () => {
  try {
    isSubmitting.value = true;
    successMessage.value = "";
    await new Promise((r) => setTimeout(r, 800));
    successMessage.value = "Product created successfully!";
    form.value = { name: "", category: "", price: 0, description: "" };
  } finally {
    isSubmitting.value = false;
  }
}
</script>





<template>
  <div class="min-h-screen flex items-center justify-center p-10 bg-transparent">
    <div class="w-full max-w-5xl">
      <h1
          class="text-4xl font-extrabold text-black mb-12 uppercase tracking-tight text-center"
      >
        Create Product
      </h1>

      <form @submit.prevent="handleSubmit" class="space-y-10">
        <!-- Row: Product Name -->
        <div class="form-row">
          <label for="name" class="label-brutal">Product Name</label>
          <input
              id="name"
              v-model="form.name"
              type="text"
              placeholder="Ocean Beads Necklace"
              class="nb-input"
              required
          />
        </div>

        <!-- Row: Category -->
        <div class="form-row">
          <label for="category" class="label-brutal">Category</label>
          <select id="category" v-model="form.category" class="nb-input" required>
            <option disabled value="">Select category</option>
            <option value="necklace">Necklace</option>
            <option value="bracelet">Bracelet</option>
            <option value="earring">Earring</option>
            <option value="ring">Ring</option>
          </select>
        </div>

        <!-- Row: Price -->
        <div class="form-row">
          <label for="price" class="label-brutal">Price (INR)</label>
          <input
              id="price"
              v-model.number="form.price"
              type="number"
              min="0"
              step="0.01"
              placeholder="1499"
              class="nb-input"
              required
          />
        </div>

        <!-- Row: Description -->
        <div class="form-row">
          <label for="description" class="label-brutal">Description</label>
          <textarea
              id="description"
              v-model="form.description"
              rows="3"
              placeholder="Write a short description..."
              class="nb-input resize-none"
          ></textarea>
        </div>

        <!-- Submit -->
        <div class="pt-10 text-center">
          <button type="submit" class="nb-button w-60" :disabled="isSubmitting">
            {{ isSubmitting ? "Saving..." : "Create Product" }}
          </button>
        </div>

        <!-- Success -->
        <p
            v-if="successMessage"
            class="text-green-700 mt-6 text-center font-bold uppercase tracking-wide"
        >
          {{ successMessage }}
        </p>
      </form>
    </div>
  </div>
</template>



<style scoped>
/* --- Form structure --- */
.form-row {
  display: grid;
  grid-template-columns: 1fr 2fr;
  align-items: center;
  gap: 2rem;
}

/* add vertical space between rows */
form > .form-row:not(:last-child) {
  margin-bottom: 2.5rem; /* ~40px spacing between each row */
}


</style>
