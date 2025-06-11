<template>
  <div class="">
    <h1 class="text-2xl font-bold mb-6 dark:text-white">Add New Asset</h1>
    <AssetSummary v-model="assetSummary" />
    <TechnicalSpecification v-model="technicalSpecs" />
    <InspectionFrequency v-model="inspectionFrequencies" />
    <PerformanceMatrix v-model="performanceMatrix" />
    <div class="mt-8 flex justify-end gap-4">
      <Button label="Cancel" severity="secondary" outlined />
      <Button label="Save Asset" @click="submitAsset" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import AssetSummary from '~/components/asset/AssetSummary.vue'
import TechnicalSpecification from '~/components/asset/TechnicalSpecification.vue'
import InspectionFrequency from '~/components/asset/InspectionFrequency.vue'
import PerformanceMatrix from '~/components/asset/PerformanceMatrix.vue'
import Button from 'primevue/button'

const assetSummary = ref({})
const technicalSpecs = ref([])
const inspectionFrequencies = ref([])
const performanceMatrix = ref({})

// CRUD: Submit Asset
function submitAsset() {
  // Implement Later
  alert('Submit Add Asset: Implement Later')
}
</script>
