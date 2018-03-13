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
                    <input type="date"/>
                    <input type="time" />
                    <input type="number"/>
                    <i class="fa fa-plus-circle add-button" role="button"></i>
                </div>
                <div class="table" v-on:scroll="scroll">
                    <table>
                        <tr>
                            <th>№</th>
                            <th>Дата</th>
                            <th>Значение</th>
                        </tr>
                        <tr v-for="(item, i) in itemsOnCurrentPage">
                            <td>{{ i }}</td>
                            <td>{{ item.date }}</td>
                            <td>{{ item.value }}</td>
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