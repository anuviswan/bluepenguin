import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
// ✅ Bootstrap CSS
import "bootstrap/dist/css/bootstrap.min.css";

// ✅ Bootstrap JS bundle (for collapse, dropdown, modal, etc.)
import "bootstrap/dist/js/bootstrap.bundle.min.js";
createApp(App).mount('#app')
