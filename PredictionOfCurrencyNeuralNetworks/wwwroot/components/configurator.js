import customMenu from "./custom-menu.vue"
import dataWindow from "./data-window.vue"
import customTable from "./custom-table.vue"
import spinner from "./spinner.vue"
import customGraphic from "./custom-graphic.vue"
import container from "./container.vue"
import dataAnylysisComponent from "./data-analysis-component.vue"

(function () {
    Vue.component('data-window', dataWindow);
    Vue.component('custom-menu', customMenu); 
    Vue.component('custom-table', customTable); 
    Vue.component('spinner', spinner); 
    Vue.component('custom-graphic', customGraphic); 
    Vue.component('container', container); 
    Vue.component('data-analysis-component', dataAnylysisComponent); 
})();