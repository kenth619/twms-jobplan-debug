import { defineStore } from 'pinia'

export const useWorkorderLineStore = defineStore('workorderlineStore', () => {
    const workOrderLine = ref<WorkOrderLine>({
        WorkOrderLineId: 0,
        AssetID: '',
        AssetDescription: '',
        AssetClassification: '',
        Ring: '',
        TXCircuitName: '',
        SubstationName: '',
        FeederName: '',
        Zone: '',
        StreetName: '',
        SegmentFrom: '',
        SegmentTo: '',
        Remarks: '',
        JobPlanID: '',
        JobPlanDescription: '',
        WorkPlanSummary: '',
    })

    const workOrderLineList = ref<WorkOrderLine[]>([])

    async function submitWorkOrderLine(line: WorkOrderLine) {
        workOrderLineList.value.push(line)
        console.log('Work Order Line submitted:', workOrderLineList.value)
    }

    return {
        workOrderLine,
        workOrderLineList,
        submitWorkOrderLine,
    }
})
