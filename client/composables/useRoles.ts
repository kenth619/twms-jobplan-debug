export default function useRoles() {
    const authStore = useAuthStore()
    const { employeeData } = storeToRefs(authStore)

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
