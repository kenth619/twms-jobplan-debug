export default function useEmployeeLists() {
    const { data: activeEmployees, isPending } = useQuery<EmployeeDataHCM[]>({
        queryKey: ['employees', 'recipients'],
        queryFn: async () => {
            const activeEmployees = await $api<EmployeeDataHCM[]>('/api/employees/active')
            return activeEmployees
        },
    })

    const allRecipients = computed<LetterRecipient[]>(() => {
        const data = [] as LetterRecipient[]

        if (activeEmployees.value) {
            for (const emp of activeEmployees.value) {
                if (emp.email) {
                    data.push({
                        employeeName: emp.fullName,
                        employeeNumber: emp.employeeNumber,
                        employeeEmail: emp.email,
                    })
                }
            }
        }

        return data
    })

    return {
        activeEmployees,
        allRecipients,
        isPending,
    }
}
