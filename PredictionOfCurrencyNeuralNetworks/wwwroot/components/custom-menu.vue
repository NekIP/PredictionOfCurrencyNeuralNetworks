<template>
    <div class="custom-menu">
        <transition name="slide-fade">
            <div class="panel" v-show="show">
                <div class="menu-close">
                    <a class="menu col-md-1" href="#" role="button" v-on:click="showHide"><i class="fa fa-close" aria-hidden="true"></i></a>
                </div>
                <div class="menu-items">
                    <div class="item">
                        <a class="ref" href="/">Главная</a>
                    </div>
                    <div class="item">
                        <a class="ref" href="/DataManager">Менеджер данных</a>
                    </div>
                    <div class="item">
                        <a class="ref" href="/DataAnalysis">Анализ данных</a>
                    </div>
                </div>
            </div>
        </transition>
        <div class="background" v-show="show" v-on:click="showHide"></div>
        <div class="menu-activator" v-show="!show">
            <a class="menu col-md-1" href="#" role="button" v-on:click="showHide"><i class="fa fa-bars" aria-hidden="true"></i></a>
        </div> 
</div>
</template>
<script src="../js/components/custom-menu.js"></script>
<style lang="scss" src="../css/components/custom-menu.scss"></style>
