<template>
    <div class="data-analysis-component">
        <container v-bind:name="systemName" v-bind:callback="info">
            <spinner v-bind:show="!wasInit"></spinner>
            <div v-show="wasInit">
                <div>Средняя ошибка для обучающей выборки: <div class="value">{{ data.meanErrorForLearnSet }}</div></div>
                <div>Средняя ошибка для тестовой выборки: <div class="value">{{ data.meanErrorForTestSet }}</div></div>
                <div>Эффективность для обучающей выборки: <div class="value">{{ data.efficiencyForLearnSet }}</div></div>
                <div>Эффективность (Тейл) для обучающей выборки: <div class="value">{{ data.efficiencyTailForLearnSet }}</div></div>
                <div>Эффективность для тестовой выборки: <div class="value">{{ data.efficiencyForTestSet }}</div></div>
				<div>Эффективность (Тейл) для тестовой выборки: <div class="value">{{ data.efficiencyTailForTestSet }}</div></div>
                <template v-for="description in data.descriptionSystem">
                    <div>{{ description.split('`')[0] }}<div class="value">{{ description.split('`')[1] }}</div></div>
                </template>
                <custom-table v-bind:data="data.learnResult" code="neuralNetData" v-show="wasInit"></custom-table>
                <custom-graphic v-bind:code="systemName + '-learnResult'" v-bind:data="data.learnResult" v-bind:graphicWidth="gWidth"
                    v-bind:graphic="wasInitGraphic"></custom-graphic>
                <custom-graphic v-bind:code="systemName + '-learnResultWithoutInput'" v-bind:data="data.learnResultWithoutInput" v-bind:graphicWidth="gWidth"
                    v-bind:graphic="wasInitGraphic"></custom-graphic>
                <custom-graphic v-bind:code="systemName + '-learnResultErrors'" v-bind:data="data.learnResultErrors" v-bind:graphicWidth="gWidth"
                    v-bind:graphic="wasInitGraphic"></custom-graphic>
				<custom-table v-bind:data="data.testResult" code="neuralNetTestData" v-show="wasInit"></custom-table>
				<custom-graphic v-bind:code="systemName + '-testResult'" v-bind:data="data.testResult" v-bind:graphicWidth="gWidth"
								v-bind:graphic="wasInitGraphic"></custom-graphic>
				<custom-graphic v-bind:code="systemName + '-testResultWithoutInput'" v-bind:data="data.testResultWithoutInput" v-bind:graphicWidth="gWidth"
								v-bind:graphic="wasInitGraphic"></custom-graphic>
                <custom-graphic v-bind:code="systemName + '-inputData'" v-bind:data="data.inputData" v-bind:graphicWidth="gWidth"
                    v-bind:graphic="wasInitGraphic"></custom-graphic>
            </div>
        </container>
    </div>
</template>
<script src="../js/components/data-analysis-component.js"></script>
<style lang="scss" src="../css/components/data-analysis-component.scss"></style>