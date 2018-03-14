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
            <div class="spinner" v-show="!wasInit"><i class="fa fa-refresh fa-spin"></i></div>
            <div class="meta" v-show="wasInit">
                <div class="additional">
                    <input class="additional-item" type="date"/>
                    <input class="additional-item" type="time" />
                    <input class="additional-item" type="number"/>
                    <i class="fa fa-plus-circle add-button additional-item" role="button" title="Добавить данные слева"></i>
                    <i class="fa fa-pie-chart add-button additional-item" aria-hidden="true" role="button" title="Построить график"></i>
                    <i class="fa fa-database add-button additional-item" 
                       aria-hidden="true" role="button" 
                       title="Загрузить все недостающие данные с удаленного источника(Finam.ru либо же из файла)"></i>
                    <!--<button class="additional-item">График</button>-->
                    <!--<button class="additional-item">DataProvide</button>-->
                </div>
                <div class="table" v-on:scroll="scroll">
                    <table>
                        <tr>
                            <th>№</th>
                            <th>Дата</th>
                            <th>Значение</th>
                            <th></th>
                            <th></th>
                        </tr>
                        <tr v-for="(item, i) in itemsOnCurrentPage">
                            <td>{{ i }}</td>
                            <td>{{ item.date }}</td>
                            <td>{{ item.value }}</td>
                            <td><i class="fa fa-pencil" aria-hidden="true" role="button"></i></td>
                            <td><i class="fa fa-trash" aria-hidden="true" role="button"></i></td>
                        </tr>
                    </table>
                </div>
                <div class="page">
                    <i class="fa fa-arrow-circle-left" role="button" v-on:click="movePage(-1)" v-show="leftAvailable()"></i> 
                    <input type="number" v-model="currentPage"> | {{ countPages }} 
                    <i class="fa fa-arrow-circle-right" role="button" v-on:click="movePage(1)" v-show="rightAvailable()"></i>
                </div>
            </div>
        </div>
    </div>
</template>
<script src="../js/components/data-window.js"></script>
<style lang="scss" src="../css/components/data-window.scss"></style>