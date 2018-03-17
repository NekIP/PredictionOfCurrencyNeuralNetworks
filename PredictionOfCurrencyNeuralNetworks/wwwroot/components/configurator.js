import customMenu from "./custom-menu.vue"
import dataWindow from "./data-window.vue"
import customTable from "./custom-table.vue"

(function () {
    Vue.component('data-window', dataWindow);
    Vue.component('custom-menu', customMenu); 
    Vue.component('custom-table', customTable); 
})();