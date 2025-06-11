<template>
    <div>
        <div class="flex justify-between mb-2">
            <h1 class="text-2xl font-bold mb-4 dark:text-white">
                Add New Job Plan
            </h1>

            <div class="flex gap-3">
                <Button @click="showCancelDialog = true" severity="secondary" outlined>
                    <Icon name="material-symbols:cancel" size="1.5rem" />
                    Cancel
                </Button>
                <Button @click="submitJobPlan" :loading="isSubmitting" :disabled="!canSubmit">
                    <Icon name="material-symbols:save" size="1.5rem" />
                    Submit Job Plan
                </Button>
            </div>
        </div>

        <JobPlanSummary 
            v-model="jobPlanData" 
            @validation-change="onJobPlanValidationChange" 
            ref="jobPlanSummaryRef"
            mode="add"
        />

        <Tasks 
            v-model="jobPlanTasks" 
            @validation-change="onTasksValidationChange"
            ref="tasksRef"
            mode="add"
        />

        <div class="flex justify-end gap-3 mb-4">
            <Button @click="showCancelDialog = true" severity="secondary" outlined>
                <Icon name="material-symbols:cancel" size="1.5rem" />Cancel
            </Button>
            
            <ConfirmDialog 
                v-model:visible="showCancelDialog" 
                title="Cancel Job Plan"
                message="Are you sure you want to cancel? All unsaved changes will be lost."
                confirm-label="Cancel Job Plan" 
                cancel-label="Stay" 
                @confirm="confirmCancel"
                @cancel="showCancelDialog = false" 
            />
            
            <Button @click="submitJobPlan" :loading="isSubmitting" :disabled="!canSubmit">
                <Icon name="material-symbols:save" size="1.5rem" />
                Submit Job Plan
            </Button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, computed } from 'vue'
import { useToast } from 'primevue/usetoast'
import Button from 'primevue/button'
import JobPlanSummary from '~/components/jobplan/JobPlanSummary.vue'
import Tasks from '~/components/jobplan/Tasks.vue'
import { jobPlansAPIService } from '~/services/jobPlansAPIService'
import ConfirmDialog from '~/components/modal/ConfirmDialog.vue'

const toast = useToast()
const jobPlanSummaryRef = ref()
const tasksRef = ref()
const isSubmitting = ref(false)
const showCancelDialog = ref(false)

const jobPlanData = ref({
    jobPlanId: null,
    jobPlanStatus: true,
    jobPlanShortDesc: '',
    jobPlanLongDesc: '',
    assetClassId: null,
    assetClassShortDesc: '',
    assetClassLongDesc: ''
})

const jobPlanTasks = ref<Array<{ taskNo: number; description: string }>>([])
const isJobPlanValid = ref(false)
const jobPlanErrors = ref<string[]>([])
const isTasksValid = ref(true)
const tasksErrors = ref<string[]>([])

const canSubmit = computed(() => isJobPlanValid.value && isTasksValid.value && !isSubmitting.value)
const allValidationErrors = computed(() => [...jobPlanErrors.value, ...tasksErrors.value])

const onJobPlanValidationChange = (isValid: boolean, errors: string[]) => {
    isJobPlanValid.value = isValid
    jobPlanErrors.value = errors
}

const onTasksValidationChange = (isValid: boolean, errors: string[]) => {
    isTasksValid.value = isValid
    tasksErrors.value = errors
}

function validateJobPlan(): string[] {
    const errors: string[] = []
    
    if (!isJobPlanValid.value) errors.push(...jobPlanErrors.value)
    if (!isTasksValid.value) errors.push(...tasksErrors.value)
    if (jobPlanTasks.value.length === 0) errors.push('At least one task is required')
    
    // Check for duplicate task numbers
    const taskNumbers = jobPlanTasks.value.map(task => task.taskNo)
    if (new Set(taskNumbers).size !== taskNumbers.length) {
        errors.push('Duplicate task numbers found')
    }
    
    return errors
}

async function submitJobPlan() {
    if (isSubmitting.value) return

    try {
        isSubmitting.value = true
        const validationErrors = validateJobPlan()
        
        if (validationErrors.length > 0) {
            toast.add({
                severity: 'error',
                summary: 'Validation Error',
                detail: validationErrors.join('\n'),
                life: 5000
            })
            return
        }

        toast.add({
            severity: 'info',
            summary: 'Saving',
            detail: 'Saving job plan...',
            life: 3000
        })

        const jobPlanSubmitData = {
            jobPlanStatus: jobPlanData.value.jobPlanStatus,
            jobPlanShortDesc: jobPlanData.value.jobPlanShortDesc.trim(),
            jobPlanLongDesc: jobPlanData.value.jobPlanLongDesc?.trim() || '',
            assetClassId: jobPlanData.value.assetClassId!,
            assetClassShortDesc: jobPlanData.value.assetClassLongDesc,
            jobPlanLines: jobPlanTasks.value.map(task => ({
                jobPlanLineNo: task.taskNo,
                jobPlanLineDesc: task.description.trim()
            }))
        }

        console.log('Job Plan Submit Data:', jobPlanSubmitData)

        const response = await jobPlansAPIService.createJobPlan(jobPlanSubmitData)
        console.log('Job Plan Response:', response)

        if (response.success && response.data) {
            toast.add({
                severity: 'success',
                summary: 'Success',
                detail: `Job Plan saved successfully!`,
                life: 4000
            })
            //resetForm()
            await navigateTo('/jobplan/')
        } else {
            throw new Error(response.error || 'Failed to save job plan')
        }
    } catch (error: any) {
        let errorMessage = 'Failed to save job plan'
        if (error.response?.status === 409) {
            errorMessage = 'Job Plan ID already exists'
        } else if (error.message) {
            errorMessage = error.message
        }
        
        toast.add({
            severity: 'error',
            summary: 'Save Failed',
            detail: errorMessage,
            life: 6000
        })
    } finally {
        isSubmitting.value = false
    }
}

function resetForm() {
    jobPlanData.value = {
        jobPlanId: null,
        jobPlanStatus: true,
        jobPlanShortDesc: '',
        jobPlanLongDesc: '',
        assetClassId: null,
        assetClassShortDesc: '',
        assetClassLongDesc: ''
    }
    jobPlanTasks.value = []
    isJobPlanValid.value = false
    jobPlanErrors.value = []
    isTasksValid.value = true
    tasksErrors.value = []
}

async function confirmCancel() {
    showCancelDialog.value = false
    await navigateTo('/jobplan')
}

defineExpose({
    submitJobPlan,
    resetForm,
    validateJobPlan,
    canSubmit
})
</script>
