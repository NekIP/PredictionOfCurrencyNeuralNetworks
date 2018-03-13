import menu from "./custom-menu.vue"
import dataWindow from "./data-window.vue"

(function () {
    Vue.component('data-window', dataWindow);
    Vue.component('custom-menu', menu); 
})();