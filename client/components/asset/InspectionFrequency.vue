<template>
  <br />
  <div class="bg-white dark:bg-neutral-800 rounded-md border border-blue-500 dark:border-blue-800 p-4 mb-6 shadow-xl">
    <h2 class="text-2xl font-semibold mb-6 flex items-center space-x-2">
      <span>Inspection Frequency Settings</span>
    </h2>
    <form @submit.prevent="saveFrequencies" class="space-y-6">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-5">
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Level 1 Ground Patrol</label>
          <InputText v-model="frequencies.level1" class="w-full" placeholder="e.g. Every 6 months" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Level 2 Ground Patrol</label>
          <InputText v-model="frequencies.level2" class="w-full" placeholder="e.g. Annually" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Level 3 Ground Patrol</label>
          <InputText v-model="frequencies.level3" class="w-full" placeholder="e.g. Every 2 years" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Aerial Inspection</label>
          <InputText v-model="frequencies.aerial" class="w-full" placeholder="e.g. Annually" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Thermographic</label>
          <InputText v-model="frequencies.thermo" class="w-full" placeholder="e.g. Quarterly" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Ultrasonic</label>
          <InputText v-model="frequencies.ultrasonic" class="w-full" placeholder="e.g. Bi-annually" />
        </div>
        <div class="md:col-span-2">
          <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Vegetation Patrol</label>
          <InputText v-model="frequencies.vegetation" class="w-full" placeholder="e.g. Every 4 months" />
        </div>
      </div>
    </form>
    <Toast />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import Toast from 'primevue/toast'
import { useToast } from 'primevue/usetoast'

const toast = useToast()
const isSaving = ref(false)

const defaultFrequencies = {
  level1: '',
  level2: '',
  level3: '',
  aerial: '',
  thermo: '',
  ultrasonic: '',
  vegetation: ''
}

const frequencies = ref({ ...defaultFrequencies })

function saveFrequencies() {
  isSaving.value = true
  setTimeout(() => {
    isSaving.value = false
    toast.add({
      severity: 'success',
      summary: 'Saved',
      detail: 'Inspection frequencies have been saved.',
      life: 2000
    })
    // Implement your API save here
  }, 1000)
}

function resetForm() {
  frequencies.value = { ...defaultFrequencies }
  toast.add({
    severity: 'info',
    summary: 'Reset',
    detail: 'Form has been reset.',
    life: 1500
  })
}
</script>
