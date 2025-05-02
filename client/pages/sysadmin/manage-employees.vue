<script lang="ts" setup>
const { has } = useRoles()
const isAuthorized = computed(() => has('system-administrator') || has('superuser'))

const toast = useToast()
const queryClient = useQueryClient()

const selectedEmployee = ref<EmployeeDataHCM | null>(null)
const employeeSearchTerm = ref('')
const currentPage = ref(1)
const rowsPerPage = ref(10)
const confirmDialog = ref(false)
const confirmAction = ref<(() => void) | null>(null)
const confirmMessage = ref('')
const selectedDepartment = ref<Department | null>(null)
const selectedDepartmentRole = ref<DepartmentRole | null>(null)

const { data: employees, isLoading: loadingEmployees } = useQuery({
    queryKey: ['employees-list'],
    queryFn: async () => {
        const response = await $api<EmployeeDataHCM[]>('/api/employeeroles')
        return response || []
    },
    enabled: isAuthorized,
})

const { data: selectedEmployeeWithRoles, isLoading: loadingSelectedEmployee } = useQuery({
    queryKey: ['employee-with-roles'],
    queryFn: async () => {
        if (!selectedEmployee.value?.employeeNumber) return null
        return await $api<EmployeeWithRoles>(`/api/employeeroles/${selectedEmployee.value.employeeNumber}`)
    },
    enabled: computed(() => isAuthorized.value && !!selectedEmployee.value?.employeeNumber),
})

const { data: systemRoles } = useQuery({
    queryKey: ['system-roles'],
    queryFn: async () => {
        const response = await $api<SystemRole[]>('/api/roles/system')
        return response || []
    },
    enabled: isAuthorized,
})

const { data: departmentRoles } = useQuery({
    queryKey: ['department-roles'],
    queryFn: async () => {
        const response = await $api<DepartmentRole[]>('/api/roles/department')
        return response || []
    },
    enabled: isAuthorized,
})

const { data: departments } = useQuery({
    queryKey: ['departments'],
    queryFn: async () => {
        const response = await $api<Department[]>('/api/departments')
        return response || []
    },
    enabled: isAuthorized,
})

const filteredEmployees = computed(() => {
    if (!employees.value) return []

    if (!employeeSearchTerm.value) return employees.value

    const searchTerm = employeeSearchTerm.value.toLowerCase()
    return employees.value.filter(emp =>
        emp.fullName.toLowerCase().includes(searchTerm) || emp.employeeNumber.toLowerCase().includes(searchTerm) || emp.department.toLowerCase().includes(searchTerm),
    )
})

const paginatedEmployees = computed(() => {
    const start = (currentPage.value - 1) * rowsPerPage.value
    const end = start + rowsPerPage.value
    return filteredEmployees.value.slice(start, end)
})

const assignSystemRoleMutation = useMutation({
    mutationFn: async ({ employeeNumber, roleKey }: { employeeNumber: string, roleKey: string }) => {
        return await $api(`/api/employeeroles/${employeeNumber}/system-roles`, {
            method: 'POST',
            body: { roleKey },
        })
    },
    onSuccess: () => {
        toast.add({ severity: 'success', summary: 'Success', detail: 'System role assigned successfully', life: 3000 })
        queryClient.refetchQueries({ queryKey: ['employee-with-roles'] })
    },
    onError: (error) => {
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to assign system role: ${error.message}`, life: 3000 })
    },
})

const removeSystemRoleMutation = useMutation({
    mutationFn: async ({ employeeNumber, roleKey }: { employeeNumber: string, roleKey: string }) => {
        return await $api(`/api/employeeroles/${employeeNumber}/system-roles/${roleKey}`, {
            method: 'DELETE',
        })
    },
    onSuccess: () => {
        toast.add({ severity: 'success', summary: 'Success', detail: 'System role removed successfully', life: 3000 })
        queryClient.refetchQueries({ queryKey: ['employee-with-roles'] })
    },
    onError: (error) => {
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to remove system role: ${error.message}`, life: 3000 })
    },
})

const assignDepartmentRoleMutation = useMutation({
    mutationFn: async ({ employeeNumber, roleKey, departmentCode }: { employeeNumber: string, roleKey: string, departmentCode: string }) => {
        return await $api(`/api/employeeroles/${employeeNumber}/department-roles`, {
            method: 'POST',
            body: { roleKey, departmentCode },
        })
    },
    onSuccess: () => {
        toast.add({ severity: 'success', summary: 'Success', detail: 'Department role assigned successfully', life: 3000 })
        queryClient.refetchQueries({ queryKey: ['employee-with-roles'] })
    },
    onError: (error) => {
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to assign department role: ${error.message}`, life: 3000 })
    },
})

const removeDepartmentRoleMutation = useMutation({
    mutationFn: async ({ employeeNumber, roleKey, departmentCode }: { employeeNumber: string, roleKey: string, departmentCode: string }) => {
        return await $api(`/api/employeeroles/${employeeNumber}/department-roles?departmentCode=${departmentCode}&roleKey=${roleKey}`, {
            method: 'DELETE',
        })
    },
    onSuccess: () => {
        toast.add({ severity: 'success', summary: 'Success', detail: 'Department role removed successfully', life: 3000 })
        queryClient.refetchQueries({ queryKey: ['employee-with-roles'] })
    },
    onError: (error) => {
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to remove department role: ${error.message}`, life: 3000 })
    },
})

const selectEmployee = (employee: EmployeeDataHCM) => {
    selectedEmployee.value = employee
    selectedDepartment.value = null
    selectedDepartmentRole.value = null
    queryClient.refetchQueries({ queryKey: ['employee-with-roles'] })
}

const hasSystemRole = (employee: EmployeeWithRoles | null, roleKey: string) => {
    if (!employee || !employee.systemRoles) return false
    return employee.systemRoles.some(role => role.key === roleKey)
}

const hasDepartmentRole = (employee: EmployeeWithRoles | null, departmentCode: string, roleKey: string) => {
    if (!employee || !employee.departmentRoleMapping) return false
    return employee.departmentRoleMapping.some(
        (mapping) => {
            if (departmentCode === 'S200000') {
                console.log(mapping)
                console.log(mapping.departmentCode)
                console.log(departmentCode)
                console.log(mapping.role?.key)
                console.log(roleKey)
            }
            return mapping.departmentCode === departmentCode && mapping.role.key === roleKey
        },
    )
}

const confirmAssignSystemRole = (role: SystemRole) => {
    if (!selectedEmployeeWithRoles.value) return

    confirmMessage.value = `Are you sure you want to assign the ${role.name} role to ${selectedEmployeeWithRoles.value.fullName}?`
    confirmAction.value = () => assignSystemRoleMutation.mutate({
        employeeNumber: selectedEmployeeWithRoles.value!.employeeNumber,
        roleKey: role.key,
    })
    confirmDialog.value = true
}

const confirmRemoveSystemRole = (role: SystemRole) => {
    if (!selectedEmployeeWithRoles.value) return

    confirmMessage.value = `Are you sure you want to remove the ${role.name} role from ${selectedEmployeeWithRoles.value.fullName}?`
    confirmAction.value = () => removeSystemRoleMutation.mutate({
        employeeNumber: selectedEmployeeWithRoles.value!.employeeNumber,
        roleKey: role.key,
    })
    confirmDialog.value = true
}

const confirmAssignDepartmentRole = (department: Department, role: DepartmentRole) => {
    if (!selectedEmployeeWithRoles.value) return

    confirmMessage.value = `Are you sure you want to assign the ${role.name} role for ${department.name} to ${selectedEmployeeWithRoles.value.fullName}?`
    confirmAction.value = () => assignDepartmentRoleMutation.mutate({
        employeeNumber: selectedEmployeeWithRoles.value!.employeeNumber,
        roleKey: role.key,
        departmentCode: department.code,
    })
    confirmDialog.value = true
}

const confirmRemoveDepartmentRole = (department: Department, role: DepartmentRole) => {
    if (!selectedEmployeeWithRoles.value) return

    confirmMessage.value = `Are you sure you want to remove the ${role.name} role for ${department.name} from ${selectedEmployeeWithRoles.value.fullName}?`
    confirmAction.value = () => removeDepartmentRoleMutation.mutate({
        employeeNumber: selectedEmployeeWithRoles.value!.employeeNumber,
        roleKey: role.key,
        departmentCode: department.code,
    })
    confirmDialog.value = true
}

const executeConfirmedAction = () => {
    if (confirmAction.value) {
        confirmAction.value()
        confirmDialog.value = false
        confirmAction.value = null
    }
}

const cancelConfirmedAction = () => {
    confirmDialog.value = false
    confirmAction.value = null
}
</script>

<template>
    <div>
        <h1 class="text-2xl font-bold mb-6">
            Role Management
        </h1>

        <div
            v-if="!isAuthorized"
            class="p-4 bg-red-100 text-red-700 rounded"
        >
            You do not have permission to access this page.
        </div>

        <div
            v-else
            class="grid grid-cols-1 lg:grid-cols-3 gap-6"
        >
            <!-- Employee List Panel -->
            <div class="lg:col-span-1 bg-white dark:bg-neutral-800 rounded-lg shadow p-4">
                <h2 class="text-xl font-semibold mb-4">
                    Employees
                </h2>

                <!-- Search -->
                <div class="mb-4">
                    <InputText
                        v-model="employeeSearchTerm"
                        placeholder="Search employees..."
                        class="w-full"
                    />
                </div>

                <!-- Employee List -->
                <div class="mb-4 max-h-[600px] overflow-y-auto">
                    <div
                        v-if="loadingEmployees"
                        class="flex justify-center p-4"
                    >
                        <ProgressSpinner style="width: 50px; height: 50px" />
                    </div>

                    <div
                        v-else-if="paginatedEmployees.length === 0"
                        class="p-4 text-center text-gray-500"
                    >
                        No employees found.
                    </div>

                    <div v-else>
                        <div
                            v-for="employee in paginatedEmployees"
                            :key="employee.employeeNumber"
                            class="p-3 mb-2 rounded cursor-pointer hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors"
                            :class="{ 'bg-blue-50 dark:bg-blue-900': selectedEmployee && selectedEmployee.employeeNumber === employee.employeeNumber }"
                            @click="selectEmployee(employee)"
                        >
                            <div class="font-medium">
                                {{ employee.fullName }}
                            </div>
                            <div class="text-sm text-gray-600 dark:text-gray-400">
                                {{ employee.employeeNumber }} - {{ employee.department }}
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Pagination -->
                <div class="flex justify-center">
                    <Paginator
                        :first="(currentPage - 1) * rowsPerPage"
                        :rows="rowsPerPage"
                        :total-records="filteredEmployees.length"
                        @page="(e) => currentPage = e.page + 1"
                    />
                </div>
            </div>

            <!-- Role Management Panel -->
            <div class="lg:col-span-2 bg-white dark:bg-neutral-800 rounded-lg shadow p-4">
                <div
                    v-if="!selectedEmployee"
                    class="p-8 text-center text-gray-500 dark:text-gray-400"
                >
                    Select an employee to manage their roles.
                </div>

                <div
                    v-else-if="loadingSelectedEmployee"
                    class="p-8 text-center"
                >
                    <ProgressSpinner style="width: 50px; height: 50px" />
                    <div class="mt-4 text-gray-500 dark:text-gray-400">
                        Loading employee roles...
                    </div>
                </div>

                <div
                    v-else-if="selectedEmployeeWithRoles"
                >
                    <h2 class="text-xl font-semibold mb-4">
                        Manage Roles: {{ selectedEmployeeWithRoles.fullName }}
                    </h2>

                    <!-- System Roles -->
                    <div class="mb-6">
                        <h3 class="text-lg font-medium mb-3">
                            System Roles
                        </h3>
                        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div
                                v-for="role in systemRoles"
                                :key="role.key"
                                class="p-3 border dark:border-neutral-600 rounded flex justify-between items-center"
                            >
                                <span>{{ role.name }}</span>
                                <div>
                                    <Button
                                        v-if="hasSystemRole(selectedEmployeeWithRoles, role.key)"
                                        icon="pi pi-times"
                                        severity="danger"
                                        @click="confirmRemoveSystemRole(role)"
                                    />
                                    <Button
                                        v-else
                                        icon="pi pi-plus"
                                        severity="success"
                                        @click="confirmAssignSystemRole(role)"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Department Roles -->
                    <div>
                        <h3 class="text-lg font-medium mb-3">
                            Department Roles
                        </h3>
                        <div
                            v-if="!departments || departments.length === 0"
                            class="p-4 text-center text-gray-500 dark:text-gray-400"
                        >
                            No departments available.
                        </div>
                        <div v-else>
                            <div class="mb-4">
                                <h4 class="text-md font-medium mb-2">
                                    Current Department Roles
                                </h4>
                                <DataTable
                                    v-if="selectedEmployeeWithRoles.departmentRoleMapping && selectedEmployeeWithRoles.departmentRoleMapping.length > 0"
                                    :value="selectedEmployeeWithRoles.departmentRoleMapping"
                                    striped-rows
                                >
                                    <Column
                                        field="departmentCode"
                                        header="Department"
                                    >
                                        <template #body="slotProps">
                                            {{ departments.find(d => d.code === slotProps.data.departmentCode)?.name || slotProps.data.departmentCode }}
                                        </template>
                                    </Column>
                                    <Column
                                        field="role.name"
                                        header="Role"
                                    />
                                    <Column
                                        header="Actions"
                                        style="width: 100px"
                                    >
                                        <template #body="slotProps">
                                            <Button
                                                icon="pi pi-times"
                                                severity="danger"
                                                size="small"
                                                @click="confirmRemoveDepartmentRole(
                                                    departments.find(d => d.code === slotProps.data.departmentCode) || { code: slotProps.data.departmentCode, name: slotProps.data.departmentCode },
                                                    slotProps.data.role,
                                                )"
                                            />
                                        </template>
                                    </Column>
                                </DataTable>
                                <div
                                    v-else
                                    class="p-3 text-center text-gray-500 dark:text-gray-400 border dark:border-neutral-600 rounded"
                                >
                                    No department roles assigned.
                                </div>
                            </div>

                            <div>
                                <h4 class="text-md font-medium mb-2">
                                    Assign New Department Role
                                </h4>
                                <div class="p-3 border dark:border-neutral-600 rounded">
                                    <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-3">
                                        <div>
                                            <label class="block mb-1 text-sm">Department</label>
                                            <Select
                                                v-model="selectedDepartment"
                                                :options="departments"
                                                option-label="name"
                                                placeholder="Select Department"
                                                class="w-full"
                                            />
                                        </div>
                                        <div>
                                            <label class="block mb-1 text-sm">Role</label>
                                            <Select
                                                v-model="selectedDepartmentRole"
                                                :options="departmentRoles"
                                                option-label="name"
                                                placeholder="Select Role"
                                                class="w-full"
                                            />
                                        </div>
                                        <div class="flex items-end">
                                            <Button
                                                label="Assign Role"
                                                icon="pi pi-plus"
                                                severity="success"
                                                :disabled="!selectedDepartment || !selectedDepartmentRole || hasDepartmentRole(selectedEmployeeWithRoles, selectedDepartment.code, selectedDepartmentRole.key)"
                                                class="w-full"
                                                @click="confirmAssignDepartmentRole(selectedDepartment!, selectedDepartmentRole!)"
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Confirmation Dialog -->
        <Dialog
            v-model:visible="confirmDialog"
            header="Confirm Action"
            :style="{ width: '450px' }"
            :modal="true"
        >
            <div class="p-4">
                <p>{{ confirmMessage }}</p>
            </div>
            <template #footer>
                <Button
                    label="No"
                    icon="pi pi-times"
                    class="p-button-text"
                    @click="cancelConfirmedAction"
                />
                <Button
                    label="Yes"
                    icon="pi pi-check"
                    class="p-button-text"
                    @click="executeConfirmedAction"
                />
            </template>
        </Dialog>
    </div>
</template>
