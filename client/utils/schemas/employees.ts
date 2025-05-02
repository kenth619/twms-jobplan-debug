import * as yup from 'yup'

export const validSystemRoles = [
    { key: 'none', name: 'None' },
    { key: 'viewer', name: 'Viewer' },
    { key: 'editor', name: 'Editor' },
    { key: 'manager', name: 'HR Manager' },
    { key: 'administrator', name: 'Administrator' },
    { key: 'system-administrator', name: 'System Administrator' },
    { key: 'superuser', name: 'Superuser' },
] as const
export const validDepartmentRoles = [] as const

export const departmentSchema = yup.object({
    code: yup.string().required(),
    name: yup.string().required(),
})

export const systemRoleSchema = yup.object({
    key: yup.string().oneOf(['none', 'viewer', 'editor', 'manager', 'administrator', 'system-administrator', 'superuser']).required(),
    name: yup.string().required(),
})

export const departmentRoleSchema = yup.object({
    key: yup.string().oneOf(['none']).required(),
    name: yup.string().required(),
})

export const departmentRoleMappingEntrySchema = yup.object({
    departmentCode: yup.string().required(),
    role: departmentRoleSchema.required(),
})

export const employeeDataHCMSchema = yup.object({
    employeeNumber: yup.string().required().label('Employee Number'),
    username: yup.string().required().label('Username'),
    firstName: yup.string().required().label('First Name'),
    lastName: yup.string().required().label('Last Name'),
    fullName: yup.string().required().label('Full Name'),
    email: yup.string().optional().label('Email'),
    department: yup.string().required().label('Department'),
    division: yup.string().required().label('Division'),
    employeeStatus: yup.string().required().label('Employee Status'),
    jobName: yup.string().required().label('Job Title'),
    grade: yup.string().required().label('Grade'),
})

export const employeeWithRolesSchema = employeeDataHCMSchema.concat(yup.object({
    label: yup.string().required().label('Label'),
    systemRoles: yup.array().of(systemRoleSchema).required().default([]),
    departmentRoleMapping: yup.array().of(departmentRoleMappingEntrySchema).required().default([]),
}))

export const loginResponseSchema = yup.object({
    message: yup.string().required(),
    token: yup.string().required(),
    employee: employeeWithRolesSchema.required(),
})

export type EmployeeWithRoles = yup.InferType<typeof employeeWithRolesSchema>
export type EmployeeDataHCM = yup.InferType<typeof employeeDataHCMSchema>
export type SystemRole = yup.InferType<typeof systemRoleSchema>
export type Department = yup.InferType<typeof departmentSchema>
export type DepartmentRole = yup.InferType<typeof departmentRoleSchema>
export type DepartmentRoleMappingEntry = yup.InferType<typeof departmentRoleMappingEntrySchema>
export type LoginResponse = yup.InferType<typeof loginResponseSchema>
