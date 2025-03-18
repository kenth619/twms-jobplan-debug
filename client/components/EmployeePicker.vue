<script setup>
import { ref } from 'vue'

const emit = defineEmits(['update:modelValue', 'employee-selected'])
const props = defineProps({
    modelValue: {
        type: Object,
        default: null,
    },
})

const selectedEmployee = ref(props.modelValue)
const filteredEmployees = ref([])
const loading = ref(false)
const allEmployees = ref([])

// Watch for changes to the selected employee and emit them
watch(selectedEmployee, (newValue) => {
    emit('update:modelValue', newValue)
})

// Load all employees on component mount
onMounted(async () => {
    try {
        loading.value = true
        const response = await $api('/api/employees')
        allEmployees.value = response.data
    }
    catch (error) {
        console.error('Error loading employees:', error)
    }
    finally {
        loading.value = false
    }
})

// Filter employees based on search query
const searchEmployees = (event) => {
    loading.value = true

    setTimeout(() => {
        if (!event.query.trim().length) {
            filteredEmployees.value = [...allEmployees.value]
        }
        else {
            const searchTerm = event.query.toLowerCase()
            filteredEmployees.value = allEmployees.value.filter((employee) => {
                return (
                    employee.firstName.toLowerCase().includes(searchTerm)
                    || employee.lastName.toLowerCase().includes(searchTerm)
                    || employee.employeeNumber.toLowerCase().includes(searchTerm)
                    || employee.department.toLowerCase().includes(searchTerm)
                    || employee.fullName.toLowerCase().includes(searchTerm)
                )
            })
        }
        loading.value = false
    }, 250)
}

// Handle employee selection
const onEmployeeSelect = (event) => {
    emit('employee-selected', event.value)
}
</script>

<template>
    <div>
        <AutoComplete
            v-model="selectedEmployee"
            :suggestions="filteredEmployees"
            option-label="label"
            placeholder="Search for an employee"
            :loading="loading"
            class="w-full"
            @complete="searchEmployees"
            @item-select="onEmployeeSelect"
        >
            <template #empty>
                <div class="p-2">
                    No employees found
                </div>
            </template>
        </AutoComplete>
    </div>
</template>
