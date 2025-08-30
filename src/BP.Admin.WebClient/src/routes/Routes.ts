import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "../views/Dashboard.vue";
import Home from "../views/Home.vue";

const routes = [
    { path: "/", name: "Home", component: Home },
    { path: "/d", name: "Dashboard", component:Dashboard },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;