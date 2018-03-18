<template>
    <div class="data-window">
        <container v-bind:name="name" v-bind:callback="callback">
            <spinner v-bind:show="!wasInit"></spinner>
            <div class="meta" v-show="wasInit">
                <div>Кол-во: <div class="value">{{ info.count }}</div></div>
                <div>Мат. Ожидание: <div class="value">{{ info.expectedValue }}</div></div>
                <div>Дисперсия: <div class="value">{{ info.dispersion }}</div></div>
                <div>Данные с {{ info.from }} по {{ info.to }}</div><br />
                <div class="additional">
                    <input class="additional-item" type="date" v-model="entry.date" />
                    <input class="additional-item" type="time" v-model="entry.time" />
                    <input class="additional-item" type="number" v-model="entry.value" />
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
            <custom-graphic v-bind:code="code" v-bind:data="items" v-bind:graphicWidth="graphicWidth" v-bind:graphic="graphic"></custom-graphic>
        </container>
    </div>
</template>
<script src="../js/components/data-window.js"></script>
<style lang="scss" src="../css/components/data-window.scss"></style>