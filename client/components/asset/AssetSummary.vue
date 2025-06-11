<template>
  <div class="bg-white dark:bg-neutral-800 rounded-md border border-blue-500 dark:border-blue-800 p-4 mb-6 shadow-xl">
    <h2 class="text-xl font-semibold mb-4 dark:text-white">Asset Summary</h2>
    <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
      <!-- LEFT COLUMN -->
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Asset ID</label>
          <InputText v-model="localModel.assetId" placeholder="Auto-generated Asset ID" class="w-full" readonly />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Asset Classification</label>
          <InputText v-model="localModel.assetClassification" placeholder="Enter asset classification" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Serial Number</label>
          <InputText v-model="localModel.serialNumber" placeholder="Enter serial number" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Manufacturer</label>
          <InputText v-model="localModel.manufacturer" placeholder="Enter manufacturer" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Maintenance Interval</label>
          <Select v-model="localModel.maintenanceInterval" :options="genericOptions" option-label="label"
            option-value="value" show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Ring</label>
          <Select v-model="localModel.ring" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Substation Name</label>
          <Select v-model="localModel.substationName" :options="genericOptions" option-label="label"
            option-value="value" show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Zone</label>
          <Select v-model="localModel.zone" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Segment Point</label>
          <Select v-model="localModel.segmentPoint" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>

        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">From AutoReclosure</label>
          <Select v-model="localModel.fromAutoReclosure" :options="fromAutoReclosureOptions" option-label="label"
            option-value="value" show-clear class="w-full" />
        </div>

        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Assigned GIS ID</label>
          <InputText v-model="localModel.gisId" placeholder="Assigned GIS ID" class="w-full" readonly />
        </div>
      </div>
      <!-- RIGHT COLUMN -->
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Asset Status</label>
          <div class="flex items-center space-x-3">
            <ToggleSwitch v-model="localModel.assetStatus" />
            <span class="text-sm dark:text-white">
              {{ localModel.assetStatus ? 'Active' : 'Inactive' }}
            </span>
          </div>
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Description</label>
          <InputText v-model="localModel.description" placeholder="Enter asset description" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Model Number</label>
          <InputText v-model="localModel.modelNumber" placeholder="Enter model number" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Owning Dept.</label>
          <Select v-model="localModel.owningDept" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">TX Circuit Name</label>
          <Select v-model="localModel.txCircuitName" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Feeder Name</label>
          <Select v-model="localModel.feederName" :options="genericOptions" option-label="label" option-value="value"
            show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">Segment Type (HV/LV)</label>
          <Select v-model="localModel.segmentType" :options="segmentTypeOptions" option-label="label"
            option-value="value" show-clear class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-2 dark:text-white">To AutoReclosure</label>
          <Select v-model="localModel.toAutoReclosure" :options="toAutoReclosureOptions" option-label="label"
            option-value="value" show-clear class="w-full" />
        </div>
      </div>
    </div>
    <!-- FULL WIDTH FIELD -->
    <div class="mt-8">
      <label class="block text-sm font-medium mb-2 dark:text-white">Street Name / Label</label>
      <Textarea v-model="localModel.streetName" placeholder="Enter street name/label" rows="3" class="w-full" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, watch } from 'vue'
import InputText from 'primevue/inputtext'
import Textarea from 'primevue/textarea'
import Select from 'primevue/select'
import ToggleSwitch from 'primevue/toggleswitch'

const props = defineProps(['modelValue'])
const emit = defineEmits(['update:modelValue'])

// Generic dropdown data (replace with real data as needed)
const genericOptions = [
  { label: 'Data 1', value: 'Data 1' },
  { label: 'Data 2', value: 'Data 2' },
  { label: 'Data 3', value: 'Data 3' }
]

const segmentTypeOptions = [
  { label: 'HV', value: 'HV' },
  { label: 'LV', value: 'LV' }
]

// From AutoReclosure options
const fromAutoReclosureOptions = [
  { label: 'From ABS', value: 'From ABS' },
  { label: 'From Section Fuse', value: 'From Section Fuse' },
  { label: 'From T/F', value: 'From T/F' },
  { label: 'From Pole No.', value: 'From Pole No.' },
  { label: 'From GIS No.', value: 'From GIS No.' }
]

// To AutoReclosure dropdown options
const toAutoReclosureOptions = [
  { label: 'To ABS', value: 'To ABS' },
  { label: 'To Section Fuse', value: 'To Section Fuse' },
  { label: 'To T/F', value: 'To T/F' },
  { label: 'To Pole No.', value: 'To Pole No.' },
  { label: 'To GIS No.', value: 'To GIS No.' }
]

const localModel = ref({
  assetId: null,
  assetClassification: '',
  serialNumber: '',
  manufacturer: '',
  maintenanceInterval: null,
  ring: null,
  substationName: null,
  zone: null,
  segmentPoint: null,
  fromAutoReclosure: null,
  gisId: '',
  assetStatus: true,
  description: '',
  modelNumber: '',
  owningDept: null,
  txCircuitName: null,
  feederName: null,
  segmentType: null,
  toAutoReclosure: null,
  streetName: ''
})

watch(() => props.modelValue, (val) => {
  if (val) localModel.value = { ...localModel.value, ...val }
}, { immediate: true, deep: true })

watch(localModel, (val) => {
  emit('update:modelValue', val)
}, { deep: true })
</script>
