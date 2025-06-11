<template>
    <div class="bg-white dark:bg-neutral-800 rounded-md border border-blue-500 dark:border-blue-800 p-4 mb-6 shadow-xl">
        <h2 class="text-xl font-semibold mb-4 dark:text-white">
            {{ mode === 'add' ? 'Add Job Plan' : 'Edit Job Plan' }}
        </h2>

        <div class="grid grid-cols-2 gap-6">
            <!-- Left Column -->
            <div class="space-y-4">
                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">Job Plan ID *</label>
                    <InputNumber :model-value="modelValue.jobPlanId" placeholder="Auto-generated Job Plan ID"
                        :use-grouping="false" class="w-full" :class="{ 'p-invalid': !isJobPlanIdValid }"
                        :readonly="mode === 'add'" />
                    <small v-if="!isJobPlanIdValid" class="text-red-500 text-xs mt-1 block">
                        Job Plan ID is required
                    </small>
                </div>

                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">
                        Job Plan Short Description *
                    </label>
                    <Textarea :model-value="modelValue.jobPlanShortDesc || ''"
                        placeholder="Enter job plan short description" rows="3" class="w-full"
                        :class="{ 'p-invalid': !isShortDescValid }"
                        @update:model-value="updateField('jobPlanShortDesc', $event)" />
                    <small v-if="!isShortDescValid" class="text-red-500 text-xs mt-1 block">
                        Job Plan Short Description is required
                    </small>
                </div>

                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">
                        Asset Classification *
                    </label>
                    <Select :model-value="modelValue.assetClassId" placeholder="Select Asset Classification"
                        :options="assetClassificationOptions" option-label="assetClassShortDesc"
                        option-value="assetClassId" show-clear class="w-full"
                        :class="{ 'p-invalid': !isAssetClassValid }" :loading="isLoadingAssetClasses"
                        @update:model-value="onAssetClassChange">
                        <template #option="{ option }">
                            <div class="flex flex-col">
                                <span class="font-medium">{{ option.assetClassShortDesc }}</span>
                                <span class="text-sm text-gray-500 dark:text-gray-400">{{ option.assetClassLongDesc
                                }}</span>
                            </div>
                        </template>
                        <template #empty>
                            <div class="p-3 text-center">
                                <span v-if="isLoadingAssetClasses">Loading asset classifications...</span>
                                <span v-else>No asset classifications available</span>
                            </div>
                        </template>
                    </Select>
                    <small v-if="!isAssetClassValid" class="text-red-500 text-xs mt-1 block">
                        Asset Classification is required
                    </small>
                </div>
            </div>

            <!-- Right Column -->
            <div class="space-y-4">
                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">Job Plan Status</label>
                    <div class="flex items-center space-x-3">
                        <ToggleSwitch :model-value="modelValue.jobPlanStatus"
                            @update:model-value="updateField('jobPlanStatus', $event)" />
                        <span class="text-sm dark:text-white">
                            {{ modelValue.jobPlanStatus ? 'Active' : 'Inactive' }}
                        </span>
                    </div>
                </div>

                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">
                        Job Plan Long Description
                    </label>
                    <Textarea :model-value="modelValue.jobPlanLongDesc || ''"
                        placeholder="Enter job plan long description (optional)" rows="3" class="w-full"
                        @update:model-value="updateField('jobPlanLongDesc', $event)" />
                </div>

                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">Asset Class Description</label>
                    <Textarea :model-value="selectedAssetClassDisplay"
                        placeholder="Selected asset classification will appear here" rows="3"
                        class="w-full bg-gray-50 dark:bg-gray-700" readonly />
                </div>
            </div>
        </div>
    </div>
</template>


<script lang="ts" setup>
import { ref, computed, onMounted, watch } from 'vue'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Select from 'primevue/select'
import ToggleSwitch from 'primevue/toggleswitch'
import { useToast } from 'primevue/usetoast'
import { assetClassAPIService, type AssetClass } from '~/services/assetClassAPIService'
import { jobPlansAPIService } from '~/services/jobPlansAPIService'


interface JobPlanSummaryData {
    jobPlanId: number | null
    jobPlanStatus: boolean
    jobPlanShortDesc: string
    jobPlanLongDesc: string
    assetClassId: number | null
    assetClassShortDesc: string
    assetClassLongDesc?: string
}

const props = defineProps<{
    modelValue: JobPlanSummaryData
    mode: 'add' | 'edit'
}>()

const emit = defineEmits<{
    'update:modelValue': [value: JobPlanSummaryData]
    'validation-change': [isValid: boolean, errors: string[]]
}>()

const toast = useToast()
const isLoadingAssetClasses = ref(false)
const assetClassificationOptions = ref<AssetClass[]>([])

const isJobPlanIdValid = computed(() => {
    return props.modelValue.jobPlanId !== null && props.modelValue.jobPlanId !== undefined && props.modelValue.jobPlanId > 0
})

const isShortDescValid = computed(() => {
    return props.modelValue.jobPlanShortDesc?.trim().length > 0
})

const isAssetClassValid = computed(() => {
    return props.modelValue.assetClassId !== null
        //&& props.modelValue.assetClassShortDesc?.trim().length > 0
})

const validationErrors = computed(() => {
    const errors: string[] = []
    if (!isJobPlanIdValid.value) errors.push('Job Plan ID is required')
    //if (!isShortDescValid.value) errors.push('jobPlanShortDesc is required')
    //if (!isAssetClassValid.value) errors.push('assetClassShortDesc is required')
    return errors
})

const isValid = computed(() => validationErrors.value.length === 0)

watch([isValid, validationErrors], ([valid, errors]) => {
    emit('validation-change', valid, errors)
})

const selectedAssetClassDisplay = computed(() => {
    if (!props.modelValue.assetClassId) return 'No asset class selected'

    const shortDesc = props.modelValue.assetClassShortDesc || ''
    const longDesc = props.modelValue.assetClassLongDesc || ''

    if (shortDesc && longDesc) return `${shortDesc}\n\n${longDesc}`
    return shortDesc || longDesc || 'Asset class details not available'
})

function updateField(field: keyof JobPlanSummaryData, value: any) {
    let processedValue = value

    if (field === 'jobPlanShortDesc' || field === 'jobPlanLongDesc') {
        processedValue = value || ''
    } else if (field === 'jobPlanId') {
        processedValue = value || null
    } else if (field === 'jobPlanStatus') {
        processedValue = Boolean(value)
    }

    emit('update:modelValue', {
        ...props.modelValue,
        [field]: processedValue
    })
}

function onAssetClassChange(assetClassId: number | null) {
    if (!assetClassId) {
        emit('update:modelValue', {
            ...props.modelValue,
            assetClassId: null,
            assetClassShortDesc: '',
            assetClassLongDesc: ''
        })
        return
    }

    const selectedAssetClass = assetClassificationOptions.value.find(ac => ac.assetClassId === assetClassId)
    if (selectedAssetClass) {
        emit('update:modelValue', {
            ...props.modelValue,
            assetClassId,
            //assetClassShortDesc: selectedAssetClass.assetClassShortDesc,
            assetClassLongDesc: selectedAssetClass.assetClassLongDesc || ''
        })
    } else {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Selected asset class not found',
            life: 3000
        })
    }
}

async function fetchNextJobPlanId() {
    const response = await jobPlansAPIService.getNextJobPlanId()

    if (response.success) {
        emit('update:modelValue', {
            ...props.modelValue,
            jobPlanId: response.data
        })
    } else {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: response.error || 'Failed to generate Job Plan ID',
            life: 5000
        })
    }
}

async function loadAssetClasses() {
    try {
        isLoadingAssetClasses.value = true
        const response = await assetClassAPIService.getActiveAssetClasses()

        if (response.success && response.data) {
            assetClassificationOptions.value = response.data

            // Populate asset class details if ID exists but descriptions are missing
            if (props.modelValue.assetClassId && !props.modelValue.assetClassShortDesc) {
                const selectedAssetClass = response.data.find(ac => ac.assetClassId === props.modelValue.assetClassId)
                if (selectedAssetClass) {
                    emit('update:modelValue', {
                        ...props.modelValue,
                        assetClassShortDesc: selectedAssetClass.assetClassShortDesc,
                        assetClassLongDesc: selectedAssetClass.assetClassLongDesc || ''
                    })
                }
            }
        } else {
            toast.add({
                severity: 'error',
                summary: 'Error',
                detail: 'Failed to load asset classifications',
                life: 5000
            })
        }
    } catch {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load asset classifications',
            life: 5000
        })
    } finally {
        isLoadingAssetClasses.value = false
    }
}

function validateForm() {
    emit('validation-change', isValid.value, validationErrors.value)
    return isValid.value
}

onMounted(async () => {
    await loadAssetClasses()
    // Only fetch next Job Plan ID if in "add" mode
    if (props.mode === 'add') {
        await fetchNextJobPlanId()
    }
    validateForm()
})

defineExpose({
    loadAssetClasses,
    validateForm,
    isValid,
    validationErrors
})
</script>

<style scoped>
.p-invalid {
    border-color: #ef4444 !important;
    box-shadow: 0 0 0 0.2rem rgba(239, 68, 68, 0.2) !important;
}

.dark .p-invalid {
    border-color: #f87171 !important;
    box-shadow: 0 0 0 0.2rem rgba(248, 113, 113, 0.2) !important;
}

.text-red-500 {
    display: block;
    margin-top: 0.25rem;
    font-size: 0.75rem;
    line-height: 1rem;
}
</style>
