<template>
    <div
        class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-6 shadow-xl">
        <!-- Header -->
        <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-semibold dark:text-white">
                {{ mode === 'add' ? 'Add Tasks' : 'Edit Tasks' }}
            </h2>
            <Button @click="showAddTaskDialog" severity="success">
                <Icon name="material-symbols:add" size="1.5rem" />
                Add Task
            </Button>
        </div>

        <!-- Data Table -->
        <DataTable :value="modelValue" class="mt-4" :paginator="true" :rows="10" :rowsPerPageOptions="[5, 10, 20]"
            tableStyle="min-width: 50rem">
            <template #empty>
                <div class="text-center py-4">
                    <p class="text-gray-500 dark:text-gray-400">
                        {{ mode === 'add' ? 'No tasks added yet' : 'No tasks found for this job plan' }}
                    </p>
                </div>
            </template>

            <Column field="taskNo" header="Task No" sortable style="width: 15%">
                <template #body="slotProps">
                    <span class="font-medium">{{ slotProps.data.taskNo }}</span>
                </template>
            </Column>

            <Column field="description" header="Task Description" sortable style="width: 70%">
                <template #body="slotProps">
                    <div class="max-w-md">
                        <p class="text-sm">{{ slotProps.data.description }}</p>
                    </div>
                </template>
            </Column>

            <Column header="Actions" style="width: 15%">
                <template #body="slotProps">
                    <div class="flex gap-2">
                        <Button @click="showEditTaskDialog(slotProps.data)" severity="info" size="small" outlined>
                            <Icon name="material-symbols:edit" size="1rem" />
                        </Button>
                        <Button @click="showDeleteConfirmation(slotProps.index)" severity="danger" size="small"
                            outlined>
                            <Icon name="material-symbols:delete" size="1rem" />
                        </Button>
                    </div>
                </template>
            </Column>
        </DataTable>

        <!-- Add/Edit Task Dialog -->
        <Dialog v-model:visible="showTaskDialog" modal :header="dialogTitle" :style="{ width: '450px' }"
            :closable="true">
            <div class="space-y-4">
                <div>
                    <label class="block text-sm font-medium mb-2 dark:text-white">Task Description</label>
                    <Textarea v-model="taskDescription" placeholder="Enter task description" rows="3" class="w-full"
                        :invalid="!isTaskDescriptionValid && showValidationError" />
                    <small v-if="!isTaskDescriptionValid && showValidationError" class="text-red-500">
                        Task description is required
                    </small>
                </div>
            </div>

            <template #footer>
                <div class="flex justify-end gap-2">
                    <Button label="Cancel" severity="secondary" outlined @click="cancelTaskDialog" />
                    <Button :label="isTaskEditing ? 'Update Task' : 'Add Task'" @click="confirmTaskDialog" />
                </div>
            </template>
        </Dialog>

        <!-- Delete Task Confirmation Dialog -->
        <ConfirmDialog v-model:visible="showDeleteDialog" title="Delete Task"
            message="Are you sure you want to delete this task? This action cannot be undone." icon-type="error"
            confirm-label="Delete" cancel-label="Cancel" confirm-severity="danger" @confirm="handleDeleteConfirm"
            @cancel="cancelDelete" />
    </div>
</template>

<script lang="ts" setup>
import { ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import Textarea from 'primevue/textarea'
import { useToast } from 'primevue/usetoast'
import ConfirmDialog from '~/components/modal/ConfirmDialog.vue'
import { jobPlansLineAPIService, type CreateJobPlansLineRequest, type UpdateJobPlansLineRequest } from '~/services/jobPlansLineAPIService'

// Types
interface Task {
    taskNo: number
    description: string
}

// Props
const props = defineProps<{
    modelValue: Task[]
    mode: 'add' | 'edit'
}>()

// Emits
const emit = defineEmits<{
    'update:modelValue': [value: Task[]]
}>()

// State
const route = useRoute()
const toast = useToast()
const jobPlanId = ref(Number(route.params.id))
const showTaskDialog = ref(false)
const showDeleteDialog = ref(false)
const showValidationError = ref(false)
const taskDescription = ref('')
const isTaskEditing = ref(false)
const editingTaskIndex = ref<number | null>(null)
const deletingTaskIndex = ref<number | null>(null)

// Computed Properties
const isTaskDescriptionValid = computed(() => taskDescription.value.trim().length > 0)
const dialogTitle = computed(() => (isTaskEditing.value ? 'Edit Task' : 'Add New Task'))

// Methods

/** Fetch tasks from API */
async function fetchTasks() {
    if (props.mode !== 'edit') return

    try {
        const response = await jobPlansLineAPIService.getJobPlansLinesByJobPlanId(jobPlanId.value)
        if (response.success && Array.isArray(response.data)) {
            const tasks = response.data.map(task => ({
                //taskNo: task.jobPlanLineNo,
                taskNo: task.jobPlanLineId,
                description: task.jobPlanLineDesc,
            }))
            emit('update:modelValue', tasks)
        }
        //else {
        //throw new Error(response.error || 'Failed to fetch tasks')
        //}
    } catch (error: any) {
        toast.add({ severity: 'error', summary: 'Error', detail: error.message, life: 3000, })
    }
}

/** Show Add Task Dialog */
function showAddTaskDialog() {
    resetTaskDialog()
    showTaskDialog.value = true
}

/** Show Edit Task Dialog */
function showEditTaskDialog(task: Task) {
    resetTaskDialog()
    taskDescription.value = task.description
    editingTaskIndex.value = props.modelValue.findIndex(t => t.taskNo === task.taskNo)
    isTaskEditing.value = true
    showTaskDialog.value = true
}

/** Confirm Task Dialog (Add/Edit) */
async function confirmTaskDialog() {
    if (!isTaskDescriptionValid.value) {
        showValidationError.value = true
        return
    }

    try {
        const isAddMode = props.mode === 'add'
        if (isAddMode) {
            isTaskEditing.value ? updateTaskUIOnly() : createTaskUIOnly()
        } else {
            if (isTaskEditing.value) {
                updateTaskUIOnly()
                await updateTaskAPIOnly()
            } else {
                //createTaskUIOnly()
                await createTaskAPIOnly()
                fetchTasks()
            }
        }
    } catch (error: any) {
        toast.add({ severity: 'error', summary: 'Error', detail: error.message, life: 3000, })
    } finally {
        resetTaskDialog()
    }
}

/** Create Task: Update UI Only */
function createTaskUIOnly() {
    const newTask = {
        taskNo: props.modelValue.length + 1,
        description: taskDescription.value.trim(),
    }
    emit('update:modelValue', [...props.modelValue, newTask])
    toast.add({ severity: 'success', summary: 'Success', detail: 'Task added successfully', life: 3000 })
}

/** Update Task: Update UI Only */
function updateTaskUIOnly() {
    const updatedTask = {
        ...props.modelValue[editingTaskIndex.value!],
        description: taskDescription.value.trim(),
    }
    const updatedTasks = [...props.modelValue]
    updatedTasks[editingTaskIndex.value!] = updatedTask
    emit('update:modelValue', updatedTasks)
    toast.add({ severity: 'success', summary: 'Success', detail: 'Task updated successfully', life: 3000 })
}

/** Create Task: Call API Only */
async function createTaskAPIOnly() {
    const payload: CreateJobPlansLineRequest = {
        jobPlanId: jobPlanId.value,
        jobPlanLineNo: props.modelValue.length + 1,
        jobPlanLineDesc: taskDescription.value.trim(),
    }
    const response = await jobPlansLineAPIService.createJobPlansLine(payload)
    if (!response.success) throw new Error(response.error || 'Failed to create task')
}

/** Update Task: Call API Only */
async function updateTaskAPIOnly() {
    const payload: UpdateJobPlansLineRequest = {
        jobPlanLineId: props.modelValue[editingTaskIndex.value!].taskNo,
        jobPlanId: jobPlanId.value,
        jobPlanLineNo: props.modelValue[editingTaskIndex.value!].taskNo,
        jobPlanLineDesc: taskDescription.value.trim(),
    }
    const response = await jobPlansLineAPIService.updateJobPlansLine(payload.jobPlanLineId, payload)
    if (!response.success) throw new Error(response.error || 'Failed to update task')
}

/** Show Delete Confirmation */
function showDeleteConfirmation(index: number) {
    deletingTaskIndex.value = index
    showDeleteDialog.value = true
}

/** Handle Delete Confirmation */
async function handleDeleteConfirm() {
    try {
        if (deletingTaskIndex.value !== null) {
            // Delete a single task
            const taskToDelete = props.modelValue[deletingTaskIndex.value!];

            if (props.mode === 'add') {
                // Delete the row from UI only
                const updatedTasks = [...props.modelValue];
                updatedTasks.splice(deletingTaskIndex.value!, 1);
                emit('update:modelValue', updatedTasks);
            } else {
                // Delete the row from UI and call API
                const response = await jobPlansLineAPIService.deleteJobPlansLine(taskToDelete.taskNo);
                if (!response.success) throw new Error(response.error);

                const updatedTasks = [...props.modelValue];
                updatedTasks.splice(deletingTaskIndex.value!, 1);
                emit('update:modelValue', updatedTasks);
            }

            toast.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Task deleted successfully',
                life: 3000,
            });
        }
    } catch (error: any) {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message || 'Failed to delete task',
            life: 3000,
        });
    } finally {
        resetDeleteDialog();
    }
}

/** Cancel Delete */
function cancelDelete() {
    resetDeleteDialog()
}

/** Cancel Task Dialog */
function cancelTaskDialog() {
    resetTaskDialog()
}

/** Reset Task Dialog */
function resetTaskDialog() {
    taskDescription.value = ''
    editingTaskIndex.value = null
    showValidationError.value = false
    showTaskDialog.value = false
    isTaskEditing.value = false
}

/** Reset Delete Dialog */
function resetDeleteDialog() {
    deletingTaskIndex.value = null
    showDeleteDialog.value = false
}

// Fetch tasks on component mount (only in edit mode)
if (props.mode === 'edit') fetchTasks()
</script>
