<!-- Displays a detailed, read-only view of a selected Job Plan.
Uses route param binding to fetch data via global Pinia store.
Includes sectioned rendering for header info and task breakdown. -->

<template>
    <div>
        <div class="flex justify-between mb-2">
            <h1 class="text-2xl font-bold mb-4 dark:text-white">
                Edit Job Plan
            </h1>

            <div class="flex gap-3">
                <Button @click="showCancelDialog = true" severity="secondary" outlined>
                    <Icon name="material-symbols:cancel" size="1.5rem" />
                    Cancel
                </Button>
                <Button @click="updateJobPlan" :loading="isSubmitting" :disabled="!canSubmit">
                    <Icon name="material-symbols:save" size="1.5rem" />
                    Update Job Plan
                </Button>
            </div>
        </div>

        <!-- Job Plan Summary -->
        <JobPlanSummary v-model="jobPlanData" mode="edit" @validation-change="onJobPlanValidationChange" />

        <!-- Tasks Component -->
        <Tasks v-model="jobPlanTasks" mode="edit" />

        <div class="flex justify-end gap-3 mb-4">
            <Button @click="showCancelDialog = true" severity="secondary" outlined>
                <Icon name="material-symbols:cancel" size="1.5rem" />Cancel
            </Button>

            <!-- Cancel dialog for Edit Job Plan -->
            <ConfirmDialog v-model:visible="showCancelDialog" title="Cancel Job Plan"
                message="Are you sure you want to cancel? All unsaved changes will be lost."
                confirm-label="Cancel Job Plan" cancel-label="Stay" @confirm="confirmCancel"
                @cancel="showCancelDialog = false" />

            <Button @click="updateJobPlan" :loading="isSubmitting" :disabled="!canSubmit">
                <Icon name="material-symbols:save" size="1.5rem" />
                Update Job Plan
            </Button>
        </div>
    </div>
</template>


<script lang="ts" setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useToast } from 'primevue/usetoast'
import Button from 'primevue/button'
import JobPlanSummary from '~/components/jobplan/JobPlanSummary.vue'
import Tasks from '~/components/jobplan/Tasks.vue'
import { jobPlansAPIService } from '~/services/jobPlansAPIService'
import { jobPlansLineAPIService } from '~/services/jobPlansLineAPIService'
import ConfirmDialog from '~/components/modal/ConfirmDialog.vue'

const toast = useToast()
const isSubmitting = ref(false)
const showCancelDialog = ref(false)
const route = useRoute()
const jobPlanId = ref(route.params.id as string)

const isJobPlanValid = ref(false)
const jobPlanErrors = ref<string[]>([])
const isTasksValid = ref(true)

const canSubmit = computed(() => isJobPlanValid.value && isTasksValid.value && !isSubmitting.value)
const onJobPlanValidationChange = (isValid: boolean, errors: string[]) => {
    isJobPlanValid.value = isValid
    jobPlanErrors.value = errors
}

// State for the job plan data
const jobPlanData = ref({
    jobPlanId: null as number | null,
    jobPlanStatus: true,
    jobPlanShortDesc: '',
    jobPlanLongDesc: '',
    assetClassId: null as number | null,
    assetClassShortDesc: '',
    assetClassLongDesc: ''
})

const jobPlanTasks = ref<Array<{ taskNo: number; description: string }>>([])
const isLoadingTasks = ref(false)
const error = ref(null)

const loadJobPlan = async () => {
    try {
        isLoadingTasks.value = true
        error.value = null

        const response = await jobPlansAPIService.getJobPlan(Number(jobPlanId.value))
        //console.log('Job Plan Response:', response)

        if (response.success && response.data) {
            const header = response.data
            jobPlanData.value = {
                jobPlanId: header.jobPlanId,
                jobPlanStatus: header.jobPlanStatus,
                jobPlanShortDesc: header.jobPlanShortDesc,
                jobPlanLongDesc: header.jobPlanLongDesc || '',
                assetClassId: header.assetClassId,
                assetClassShortDesc: header.assetClassShortDesc || '',
                assetClassLongDesc: header.assetClassLongDesc || ''
            }
            await loadTasks() // Load tasks after loading the job plan data
        } else {
            throw new Error('Failed to load job plan data')
        }
    } catch (err: any) {
        error.value = err.message || 'Failed to load job plan'
        toast.add({
            severity: 'error',
            summary: 'Load Failed',
            detail: error.value,
            life: 5000
        })
    } finally {
        isLoadingTasks.value = false
    }
}


const loadTasks = async () => {
    try {
        isLoadingTasks.value = true;
        error.value = null;

        // Use jobPlansLineAPIService to get all job plan lines
        const response = await jobPlansLineAPIService.getJobPlansLinesByJobPlanId(Number(jobPlanId.value));
        //console.log('Tasks Response:', response);

        if (response.success && response.data) {
            // Map the response data to the desired format
            jobPlanTasks.value = response.data.map((task: any) => ({
                taskNo: task.jobPlanLineId,
                description: task.jobPlanLineDesc
            }));
        } else {
            throw new Error(response.error || 'Failed to load tasks');
        }
    } catch (err: any) {
        error.value = err.message || 'Failed to load tasks';
        toast.add({
            severity: 'error',
            summary: 'Load Tasks Failed',
            detail: error.value,
            life: 5000
        });
    } finally {
        isLoadingTasks.value = false;
    }
};


const updateJobPlan = async () => {
    try {
        isSubmitting.value = true;

        const payload = {
            jobPlanId: jobPlanData.value.jobPlanId,
            jobPlanStatus: jobPlanData.value.jobPlanStatus,
            jobPlanShortDesc: jobPlanData.value.jobPlanShortDesc,
            jobPlanLongDesc: jobPlanData.value.jobPlanLongDesc || '',
            assetClassId: Number(jobPlanData.value.assetClassId),
            assetClassShortDesc: jobPlanData.value.assetClassShortDesc || ''
        };
        const response = await jobPlansAPIService.updateJobPlan(Number(jobPlanId.value), payload);
        if (response.success) {
            toast.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Job Plan updated successfully!',
                life: 5000,
            });
            await navigateTo('/jobplan');
        } else {
            throw new Error(response.error || 'Failed to update Job Plan');
        }
    } catch (err: any) {
        error.value = err.message || 'An error occurred while updating the Job Plan';

        // Show error toast
        toast.add({ severity: 'error', summary: 'Update Failed', detail: error.value, life: 5000, });
    } finally {
        isSubmitting.value = false;
    }
};



onMounted(loadJobPlan)
watch(() => route.params.id, (newId: any) => {
    if (newId) {
        jobPlanId.value = newId
        loadJobPlan()
    }
})

async function confirmCancel() {
    showCancelDialog.value = false
    await navigateTo('/jobplan')
}
</script>
