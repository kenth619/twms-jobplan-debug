import { defineStore } from 'pinia'

export const useWorkOrderHeaderStore = defineStore('myWorkorderheaderStore', () => {
    const workOrderHeader = ref<WorkOrderHeader>({
        requestedBy: '',
        area: '',
        section: '',
        associatedWO: 0,
        jobType: '',
        jobTypeSubCategory: '',
        priority: '',
        sourceDocumentType: '',
        sourceDocumentNumber: '',
        accountNo: '',
        costCentre: '',
        jobNumber: '',
    })

    async function submitWorkOrderHeader() {
        console.log('workOrderHeader', workOrderHeader.value)
        $api('/api/workorder/create', {
            method: 'POST',
            body: workOrderHeader.value.requestedBy,
        })
    }

    return {
        workOrderHeader,
        submitWorkOrderHeader,
    }
})
