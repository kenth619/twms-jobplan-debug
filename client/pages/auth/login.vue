<script setup lang="ts">
import { useForm, Field } from 'vee-validate'
import * as yup from 'yup'

definePageMeta({
    layout: 'auth',
})

interface LoginResponse {
    message: string
    token: string
    employee: EmployeeWithRoles
}

const toast = useToast()
const route = useRoute()
const authStore = useAuthStore()

const loading = ref(false)
const loginError = ref('')

const schema = yup.object({
    username: yup.string().required('Username is required'),
    password: yup.string().required('Password is required'),
})

const { handleSubmit } = useForm({
    validationSchema: schema,
    initialValues: {
        username: '',
        password: '',
    },
})

const onSubmit = handleSubmit(async (values) => {
    loginError.value = ''
    loading.value = true

    try {
        const response = await $api<LoginResponse>('/api/auth/login', {
            method: 'POST',
            body: {
                username: values.username,
                password: values.password,
            },
        })

        const parsed = await loginResponseSchema.validate(response)

        await authStore.login(parsed.token, parsed.employee)

        const redirectUrl = route.query.redirectUrl as string
        navigateTo(redirectUrl || '/')
    }
    catch {
        loginError.value = 'Invalid username or password'
        toast.add({
            severity: 'error',
            summary: 'Login Failed',
            detail: 'Invalid username or password',
            life: 5000,
        })
    }
    finally {
        loading.value = false
    }
})

const showInactivityLogoutModal = ref(route.query.inactiveLogout === 'true')

onMounted(async () => {
    if (route.query.registrationSuccess || route.query.confirmedEmail || route.query.inactiveLogout) {
        await navigateTo({ query: { ...route.query, registrationSuccess: undefined, confirmedEmail: undefined, inactiveLogout: undefined }, replace: true })
    }
})
</script>

<template>
    <div class="flex flex-grow h-full">
        <div class="m-auto w-full max-w-md p-8 mt-[4.5rem] bg-white dark:bg-gray-800 rounded-lg shadow-lg">
            <div class="text-center mb-8">
                <h1 class="text-2xl font-bold text-gray-800 dark:text-white mb-2">
                    T&TEC Log In
                </h1>
                <p class="text-gray-600 dark:text-gray-300">
                    Same as computer login
                </p>
            </div>

            <form @submit="onSubmit">
                <div class="space-y-2 mb-4">
                    <label
                        for="username"
                        class="block text-sm font-medium text-gray-700 dark:text-gray-300"
                    >
                        Username
                    </label>
                    <Field
                        v-slot="{ field, errorMessage }"
                        name="username"
                    >
                        <InputText
                            id="username"
                            v-bind="field"
                            fluid
                            :invalid="!!errorMessage"
                            placeholder="Enter your username"
                            autocomplete="username"
                        />
                        <ErrorMessage :error-message="errorMessage" />
                    </Field>
                </div>

                <div class="space-y-2 mb-4">
                    <label
                        for="password"
                        class="block text-sm font-medium text-gray-700 dark:text-gray-300"
                    >
                        Password
                    </label>
                    <Field
                        v-slot="{ field, errorMessage }"
                        name="password"
                    >
                        <Password
                            id="password"
                            v-bind="field"
                            :invalid="!!errorMessage"
                            fluid
                            toggle-mask
                            :feedback="false"
                            placeholder="Enter your password"
                            autocomplete="current-password"
                        />
                        <ErrorMessage :error-message="errorMessage" />
                    </Field>
                </div>

                <Message
                    v-if="loginError"
                    severity="error"
                    class="w-full"
                >
                    {{ loginError }}
                </Message>

                <Button
                    type="submit"
                    :loading="loading"
                    class="w-full mt-4"
                    label="Sign In"
                />
            </form>
        </div>

        <Dialog
            v-model:visible="showInactivityLogoutModal"
            modal
            header="Session Expired"
            class="w-[90vw] xl:max-w-[30rem]"
        >
            <p>
                You have been automatically logged out due to inactivity.
            </p>
            <template #footer>
                <Button
                    label="OK"
                    severity="primary"
                    @click="showInactivityLogoutModal = false"
                />
            </template>
        </Dialog>
    </div>
</template>
