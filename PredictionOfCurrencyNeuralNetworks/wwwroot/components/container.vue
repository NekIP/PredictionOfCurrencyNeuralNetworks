<template>
    <div class="container">
        <div class="name" v-on:click="showHide" role="button">
            <div class="text">{{ name }}</div>
            <div class="arrow">
                <i class="fa fa-arrow-circle-down" v-show="!show"></i>
                <i class="fa fa-arrow-circle-up" v-show="show"></i>
            </div>
        </div>
        <div class="information" v-show="show">
            <slot></slot>
        </div>
    </div>
</template>
<script src="../js/components/container.js"></script>
<style lang="scss" src="../css/components/container.scss"></style>