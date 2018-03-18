<template>
    <div class="custom-graphic">
        <div v-show="graphic">
            <input class="range" type="range" min="0" max="100" step="0.2" v-model="left"/>
            <input class="range" type="range" min="-100" max="0" step="0.2" v-model="right" />
        </div>
        <div v-bind:id="code + '-graphic'" class="graphic" v-show="graphic" v-bind:style="{ width: graphicWidth ? graphicWidth() + 'px' : '100%' }" v-on:wheel="wheel"></div>
    </div>
</template>
<script src="../js/components/custom-graphic.js"></script>
<style lang="scss" src="../css/components/custom-graphic.scss"></style>