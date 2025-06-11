import { format } from 'date-fns'

export function formatDate(date: Date | string | null | undefined) {
    if (!date)
        return 'N/A'
    const dateObj = typeof date === 'string' ? new Date(date) : date
    return format(dateObj, 'yyyy-MM-dd hh:mm a')
}

export function formatDateOnly(x: Date | string | number) {
    const dateObj = typeof x === 'string' ? new Date(x) : x
    return format(dateObj, 'yyyy-MM-dd')
}

export function formatDateTime(x: Date | string | number) {
    const dateObj = typeof x === 'string' ? new Date(x) : x
    return format(dateObj, 'yyyy-MM-dd hh:mm a')
}

export function formatTime(x: Date | string | number) {
    const dateObj = typeof x === 'string' ? new Date(x) : x
    return format(dateObj, 'hh:mm a')
}

export const statusSeverity = (status: string) => {
    switch (status) {
        case 'initial': return 'info'
        case 'sent-for-approval': return 'warning'
        case 'approved': return 'success'
        case 'in-progress': return 'info'
        case 'all-emails-sent': return 'success'
        case 'closed': return 'success'
        case 'discarded': return 'danger'
        default: return 'info'
    }
}

export const statusLabel = (status: string) => {
    switch (status) {
        case 'initial': return 'Initial'
        case 'sent-for-approval': return 'Pending Approval'
        case 'approved': return 'Approved'
        case 'in-progress': return 'In Progress'
        case 'all-emails-sent': return 'Emails Sent'
        case 'closed': return 'Closed'
        case 'discarded': return 'Discarded'
        default: return status
    }
}
