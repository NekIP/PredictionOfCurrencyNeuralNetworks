<template>
    <div class="custom-table">
        <div class="table" v-on:scroll="scroll">
            <table>
                <tr>
                    <th>№</th>
                    <template v-for="fieldName in fieldsNames">
                        <th>{{ fieldName }}</th>
                    </template>
                    <th></th>
                    <th></th>
                </tr>
                <tr v-for="item in itemsOnCurrentPage">
                    <td>{{ item.id }}</td>
                    <td v-for="value in item.values">
                        <!--<p v-show="!item.update">{{ value }}</p>
                        <input v-show="item.update" v-model="value" />-->
                        <p>{{ value }}</p>
                    </td>
                    <td>
                        <i class="fa fa-pencil" aria-hidden="true"
                           role="button" v-on:click="updateEntry(item)" v-show="!item.update"></i>
                        <i class="fa fa-pencil updated" aria-hidden="true"
                           role="button" v-on:click="updateEntry(item)" v-show="item.update"></i>
                    </td>
                    <td><i class="fa fa-trash" v-on:click="removeEntry(item.id)" role="button"></i></td>
                </tr>
            </table>
        </div>
        <div class="page">
            <i class="fa fa-arrow-circle-left" role="button" v-on:click="movePage(-1)" v-show="leftAvailable()"></i>
            <input type="number" v-model="currentPage"> | {{ countPages }}
            <i class="fa fa-arrow-circle-right" role="button" v-on:click="movePage(1)" v-show="rightAvailable()"></i>
        </div>
    </div>
</template>
<script src="../js/components/custom-table.js"></script>
<style lang="scss" src="../css/components/custom-table.scss"></style>