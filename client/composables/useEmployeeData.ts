const emptyEmployee = {
    employeeNumber: '',
    username: '',
    firstName: '',
    lastName: '',
    fullName: '',
    department: '',
    division: '',
    employeeStatus: '',
    label: '',
    systemRoles: [],
    departmentRoleMapping: [],
}

export default function useEmployeeData() {
    const authStore = useAuthStore()
    const { isAuthenticated } = storeToRefs(authStore)

    function getEmployeeDataCached(): EmployeeWithRoles {
        try {
            const cached = localStorage.getItem('employeeData')
            const parsed = JSON.parse(cached ?? '')
            return employeeWithRolesSchema.validateSync(parsed)
        }
        catch {
            return emptyEmployee
        }
    }

    const employeeDataCached = ref(getEmployeeDataCached())

    const { data: employeeData } = useQuery<EmployeeWithRoles>({
        queryKey: ['employee-data'],
        queryFn: async () => {
            try {
                const data = await $api<EmployeeWithRoles>('/api/status')
                const parsed = await employeeWithRolesSchema.validate(data)
                localStorage.setItem('employeeData', JSON.stringify(parsed))
                employeeDataCached.value = parsed
                return parsed
            }
            catch {
                return emptyEmployee
            }
        },
        enabled: isAuthenticated,
        placeholderData: () => employeeDataCached.value,
    })

    const systemRoles = computed<SystemRole[]>(() => employeeData.value?.systemRoles ?? [])
    const departmentRoleMapping = computed<DepartmentRoleMappingEntry[]>(() => employeeData.value?.departmentRoleMapping ?? [])

    const has = (roleKey: string) => {
        return systemRoles.value?.some(role => role.key === roleKey) ?? false
    }

    const hasAll = (roleKeys: string[]) => {
        return roleKeys.every(key => has(key))
    }

    const hasAny = (roleKeys: string[]) => {
        return roleKeys.some(key => has(key))
    }

    const hasInDepartment = (roleKey: string, departmentCode: string) => {
        const departmentRoles = departmentRoleMapping.value.filter(e => e.departmentCode === departmentCode).map(e => e.role)
        return departmentRoles?.some(role => role.key === roleKey) ?? false
    }

    const hasAllInDepartment = (roleKeys: string[], departmentCode: string) => {
        const departmentRoles = departmentRoleMapping.value.filter(e => e.departmentCode === departmentCode).map(e => e.role)
        return roleKeys.every(key => departmentRoles?.some(role => role.key === key) ?? false)
    }

    const hasAnyInDepartment = (roleKeys: string[], departmentCode: string) => {
        const departmentRoles = departmentRoleMapping.value.filter(e => e.departmentCode === departmentCode).map(e => e.role)
        return roleKeys.some(key => departmentRoles?.some(role => role.key === key) ?? false)
    }

    return {
        employeeData,
        systemRoles,
        departmentRoleMapping,
        has,
        hasAll,
        hasAny,
        hasInDepartment,
        hasAllInDepartment,
        hasAnyInDepartment,
    }
}
