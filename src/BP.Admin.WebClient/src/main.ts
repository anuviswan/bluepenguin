import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
// ✅ Bootstrap CSS
import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/custom.css";
import router from "./routes/Routes.ts";


// ✅ Bootstrap JS bundle (for collapse, dropdown, modal, etc.)
import "bootstrap/dist/js/bootstrap.bundle.min.js";
createApp(App).use(router).mount('#app')
