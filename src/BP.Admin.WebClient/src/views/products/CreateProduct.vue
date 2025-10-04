<template>
  <div class="create-product-page max-w-2xl mx-auto mt-10 p-6 bg-white shadow-lg rounded-2xl">
    <h2 class="text-2xl font-semibold text-gray-800 mb-6">Create New Product</h2>

    <form @submit.prevent="handleSubmit" class="space-y-5">
      <!-- Product Name -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Product Name</label>
        <input
            v-model="form.name"
            type="text"
            placeholder="e.g., Ocean Beads Necklace"
            class="w-full border rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
            required
        />
      </div>

      <!-- Category -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Category</label>
        <select
            v-model="form.category"
            class="w-full border rounded-lg px-4 py-2 bg-white focus:ring-2 focus:ring-blue-500 focus:outline-none"
        >
          <option disabled value="">Select Category</option>
          <option value="necklace">Necklace</option>
          <option value="bracelet">Bracelet</option>
          <option value="earring">Earring</option>
          <option value="ring">Ring</option>
        </select>
      </div>

      <!-- Price -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Price (INR)</label>
        <input
            v-model.number="form.price"
            type="number"
            min="0"
            step="0.01"
            placeholder="e.g., 1499"
            class="w-full border rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
            required
        />
      </div>

      <!-- Description -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Description</label>
        <textarea
            v-model="form.description"
            placeholder="Write a short description..."
            rows="4"
            class="w-full border rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
        ></textarea>
      </div>

      <!-- Submit -->
      <div class="pt-4">
        <button
            type="submit"
            class="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 rounded-lg transition"
            :disabled="isSubmitting"
        >
          {{ isSubmitting ? 'Creating...' : 'Create Product' }}
        </button>
      </div>
    </form>

    <!-- Success message -->
    <p v-if="successMessage" class="text-green-600 mt-4 text-center">
      {{ successMessage }}
    </p>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'

// reactive form state
const form = ref({
  name: '',
  category: '',
  price: 0,
  description: ''
})

const isSubmitting = ref(false)
const successMessage = ref('')

async function handleSubmit() {
  try {
    isSubmitting.value = true
    successMessage.value = ''

    // Example API call (replace with your real service)
    const response = await fetch('/api/products', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    })

    if (!response.ok) throw new Error('Failed to create product')

    successMessage.value = 'Product created successfully!'
    form.value = { name: '', category: '', price: 0, description: '' }
  } catch (err) {
    alert((err as Error).message)
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
.create-product-page {
  transition: all 0.3s ease;
}
</style>
