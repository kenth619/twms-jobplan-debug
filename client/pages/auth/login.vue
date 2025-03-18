<script setup lang="ts">
import { Form, type FormSubmitEvent } from '@primevue/forms'
import { yupResolver } from '@primevue/forms/resolvers/yup'
import * as yup from 'yup'

interface LoginResponse {
    message: string
    token: string
    employee: {
        username: string
        employeeNumber: string
        fullName: string
        department: string
    }
}

const toast = useToast()
const authStore = useAuthStore()

const loading = ref(false)

const schema = yup.object({
    username: yup.string().required('Username is required'),
    password: yup.string().required('Password is required'),
})

const formRef = ref()
const loginError = ref('')

const resolver = yupResolver(schema)

const initialValues = {
    username: '',
    password: '',
}

const route = useRoute()

const onSubmit = async (event: FormSubmitEvent) => {
    if (!event.valid) return

    loginError.value = ''
    loading.value = true

    try {
        const response = await $api<LoginResponse>('/api/auth/login', {
            method: 'POST',
            body: {
                username: event.values.username,
                password: event.values.password,
            },
        })

        authStore.setToken(response.token)

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
}
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

            <Form
                ref="formRef"
                v-slot="$form"
                :initial-values="initialValues"
                :resolver="resolver"
                @submit="onSubmit"
            >
                <div class="space-y-2 mb-4">
                    <label
                        for="username"
                        class="block text-sm font-medium text-gray-700 dark:text-gray-300"
                    >
                        Username
                    </label>
                    <InputText
                        id="username"
                        name="username"
                        fluid
                        :class="{ 'p-invalid': $form.username?.invalid }"
                        placeholder="Enter your username"
                        autocomplete="username"
                    />
                    <small
                        v-if="$form.username?.invalid"
                        class="p-error"
                    >
                        {{ $form.username.error.message }}
                    </small>
                </div>

                <div class="space-y-2 mb-4">
                    <label
                        for="password"
                        class="block text-sm font-medium text-gray-700 dark:text-gray-300"
                    >
                        Password
                    </label>
                    <Password
                        id="password"
                        name="password"
                        :class="{ 'p-invalid': $form.password?.invalid }"
                        fluid
                        toggle-mask
                        :feedback="false"
                        placeholder="Enter your password"
                        autocomplete="current-password"
                    />
                    <small
                        v-if="$form.password?.invalid"
                        class="p-error"
                    >
                        {{ $form.password.error.message }}
                    </small>
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
            </Form>
        </div>
    </div>
</template>
