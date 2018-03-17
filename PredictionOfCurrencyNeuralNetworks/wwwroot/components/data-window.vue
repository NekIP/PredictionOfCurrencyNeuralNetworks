<template>
    <div class="data-window">
        <div class="name" v-on:click="showHide" role="button">
            <div class="text">{{ name }}</div>
            <div class="arrow">
                <i class="fa fa-arrow-circle-down" v-show="!show"></i>
                <i class="fa fa-arrow-circle-up" v-show="show"></i>
            </div>
        </div>
        <div class="information" v-show="show">
            <spinner v-bind:show="!wasInit"></spinner>
            <div class="meta" v-show="wasInit">
                <div class="additional">
                    <input class="additional-item" type="date" v-model="entry.date"/>
                    <input class="additional-item" type="time" v-model="entry.time"/>
                    <input class="additional-item" type="number" v-model="entry.value"/>
                    <i class="fa fa-plus-circle add-button additional-item" 
                       role="button" v-on:click="addEntry" title="Добавить данные слева"></i>
                    <i class="fa fa-pie-chart add-button additional-item" 
                       role="button" v-on:click="showHideGraphic" title="Построить график"></i>
                    <i class="fa fa-database add-button additional-item" 
                       role="button" v-on:click="prepareData" 
                       title="Загрузить все недостающие данные с удаленного источника(Finam.ru либо же из файла)"></i>
                </div>
                <custom-table v-bind:data="items" code="code" v-bind:fieldsNames="['Дата', 'Значение']" v-bind:removeEntry="removeEntry"></custom-table>
            </div>
            <div v-bind:id="code + '-graphic'" class="graphic" v-show="graphic" v-bind:style="{ width: graphicWidth() + 'px' }"></div>
        </div>
    </div>
</template>
<script src="../js/components/data-window.js"></script>
<style lang="scss" src="../css/components/data-window.scss"></style>