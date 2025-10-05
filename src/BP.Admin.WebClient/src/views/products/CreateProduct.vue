<script setup lang="ts">
import {onMounted, ref} from "vue";
import  categoryService from "../../apiservice/CategoryService.ts"
import type {Category, Feature, Material, Collection, KeyValuePair} from "../../types/ProductTypes.ts";
import materialService from "../../apiservice/MaterialService.ts";
import featureService from "../../apiservice/FeatureService.ts";
import collectionService from "../../apiservice/CollectionService.ts";

const form = ref({
  name: "",
  category: "",
  material:"",
  price: 0,
  features: [],
  collection:'',
  yearCode:25
});

const isSubmitting = ref(false);
const successMessage = ref("");
const availableCategories = ref<Category[]>([]);
const availableMaterials = ref<Material[]>([]);
const availableFeatures = ref<Feature[]>([]);
const availableCollections = ref<Collection[]>([]);
const availableYearCode = ref<KeyValuePair[]>();

onMounted(async () => {
  await getCategories();
  await getMaterials();
  await getFeatures();
  await getCollections();
  getYearList();
})

const getYearList = () => {
  const startId = 22;
  availableYearCode.value =  Array.from({ length: 20 }, (_, i) => ({
    Id: String(startId + i),
    Name: String(2000 + startId + i)
  }));
};

const getCollections = async () => {
  availableCollections.value = await collectionService.getFeatures();
  console.log(availableCollections.value);
}

const getFeatures = async () => {
  availableFeatures.value = await featureService.getFeatures();
  console.log(availableFeatures.value);
}

const getCategories = async () => {
  availableCategories.value = await categoryService.getCategories();
  console.log(availableCategories.value);
}

const getMaterials = async() =>{
  availableMaterials.value = await materialService.getMaterials();
  console.log(availableMaterials.value);
}

const handleSubmit = async () => {
  try {
    isSubmitting.value = true;
    successMessage.value = "";
    await new Promise((r) => setTimeout(r, 800));
    successMessage.value = "Product created successfully!";
    form.value = {
      name: "",
      category: "",
      material:"",
      price: 0,
      features: [],
      collection: '',
      yearCode:25
    };
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
            <option v-for="c in availableCategories" :key="c.Id" :value="c.Id">{{c.Name}}</option>
          </select>
        </div>


        <!-- Row: Material -->
        <div class="form-row">
          <label for="material" class="label-brutal">Material</label>
          <select id="material" v-model="form.material" class="nb-input" required>
            <option disabled value="">Select Material</option>
            <option v-for="m in availableMaterials" :key="m.Id" :value="m.Id">{{m.Name}}</option>
          </select>
        </div>


        <!-- Row: Features -->
        <div class="form-row">
          <label class="label-brutal">Features</label>

          <div class="checkbox-grid-3">
            <label
                v-for="c in availableFeatures"
                :key="c.Id"
                class="checkbox-item"
            >
              <input
                  type="checkbox"
                  class="nb-checkbox"
                  :value="c.Id"
                  v-model="form.features"
              />
              <span>{{ c.Name }}</span>
            </label>
          </div>
        </div>

        <!-- Row: Collection -->
        <div class="form-row">
          <label for="collection" class="label-brutal">Collection</label>
          <select id="collection" v-model="form.collection" class="nb-input" required>
            <option disabled value="">Select Collection</option>
            <option v-for="m in availableMaterials" :key="m.Id" :value="m.Id">{{m.Name}}</option>
          </select>
        </div>


        <!-- Row: Year -->
        <div class="form-row">
          <label for="yearCode" class="label-brutal">Collection</label>
          <select id="yearCode" v-model="form.yearCode" class="nb-input" required>
            <option disabled value="">Select Year</option>
            <option v-for="m in availableYearCode" :key="m.Id" :value="m.Id">{{m.Name}}</option>
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

.checkbox-grid-3 {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 2rem 3rem; /* more whitespace between checkboxes */
  padding: 0.5rem 0;
}

/* label wrapping the checkbox and text */
.checkbox-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  cursor: pointer;
  user-select: none;
}



</style>
