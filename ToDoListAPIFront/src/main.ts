import './assets/main.css'
import './assets/homeView.css';

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config'
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primeicons/primeicons.css';
import Tooltip from 'primevue/tooltip';
const app = createApp(App)

app.directive('tooltip', Tooltip);
app.use(PrimeVue)
app.use(router)
app.mount('#app')
