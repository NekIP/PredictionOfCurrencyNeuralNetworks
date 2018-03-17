<template>
    <div class="custom-graphic">
        <div v-bind:id="code + '-graphic'" class="graphic" v-show="graphic" v-bind:style="{ width: graphicWidth() + 'px' }"></div>
    </div>
</template>
<script src="../js/components/custom-graphic.js"></script>
<style lang="scss" src="../css/components/custom-graphic.scss"></style>