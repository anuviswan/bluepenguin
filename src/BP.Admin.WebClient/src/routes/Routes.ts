import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../views/Dashboard.vue";
import Home from "../views/Home.vue";
import CreateProduct from "../views/products/CreateProduct.vue";

const routes = [
  { path: "/", name: "Home", component: Home },
  { path: "/d", name: "Dashboard", component: Dashboard,
  children: [
      { path: "p/create", name: "CreateProduct", component: CreateProduct },
  ]
  },

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
