<script setup lang="ts">
import '@/assets/css/LoginScreen.css'
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

const showPassword = ref(false)

function togglePassword() {
    const passwordField = document.getElementById('password-field') as HTMLInputElement
    showPassword.value = !showPassword.value
    passwordField.type = showPassword.value ? 'text' : 'password'
}


const showInactivityLogoutModal = ref(route.query.inactiveLogout === 'true')

onMounted(async () => {
    if (route.query.registrationSuccess || route.query.confirmedEmail || route.query.inactiveLogout) {
        await navigateTo({ query: { ...route.query, registrationSuccess: undefined, confirmedEmail: undefined, inactiveLogout: undefined }, replace: true })
    }
})
</script>


<template>
    <div class="login-page-wrapper">
        <!-- Header -->
        <header class="login-header d-flex align-items-center px-4 py-2">
            <div class="d-inline-flex align-items-center logo-title-wrapper" style="margin-left: 250px;">
                <img src="/images/logo.png" alt="Logo" class="login-logo" />
                <h2 class="app-title mb-0 ml-2">TWMS</h2>
            </div>
        </header>



        <!-- Main Login Form Section -->
        <div class="login-background">
            <form id="login-form" @submit.prevent="onSubmit">
                <h4 class="text-center pb-3">LOG IN</h4>

                <div class="form-group">
                    <label for="username">Username</label>
                    <Field id="username" name="username" type="text" class="form-control"
                        placeholder="Enter your TTEC username (Same as Computer Login)" />
                    <ErrorMessage name="username" class="text-danger text-sm" />
                </div>

                <div class="form-group">
                    <label for="password">Password</label>
                    <div class="password-container">
                        <Field id="password-field" name="password" type="password" class="form-control"
                            placeholder="Please enter your password" />
                        <span class="password-toggle" @click="togglePassword">
                            <i class="fas" :class="showPassword ? 'fa-eye-slash' : 'fa-eye'"></i>
                        </span>
                    </div>
                    <ErrorMessage name="password" class="text-danger text-sm" />
                </div>

                <div class="form-group pt-2">
                    <Button type="submit" :loading="loading" class="w-full mt-4" label="Sign In" />
                    <!-- <Button type="submit" label="Log In" :loading="loading" class="login-button" /> -->
                </div>

                <Message v-if="loginError" severity="error" :closable="false" class="mt-3">
                    {{ loginError }}
                </Message>
            </form>
        </div>

        <!-- Footer -->
        <footer class="login-footer text-center">
            <p>© {{ new Date().getFullYear() }} Trinidad and Tobago Electricity Commission. All Rights Reserved.</p>
        </footer>
    </div>
</template>
