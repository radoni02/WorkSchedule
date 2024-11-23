<template>
    <div class="container mt-5">
        <h1 class="text-center mb-4">Main Page</h1>

        <div class="d-flex justify-content-center gap-3">
            <button @click="goToLogin" class="btn btn-primary btn-lg">
                Go to Login Page
            </button>

            <button @click="goToRegister" class="btn btn-secondary btn-lg">
                Go to Register Page
            </button>
        </div>
    </div>
</template>

<script>
import { useRouter } from 'vue-router';

export default {
    name: 'MainScreen',
    setup() {
        const router = useRouter();

        const goToLogin = () => {
            router.push({name:'LoginPage'})
        }

        const goToRegister = () => {
            router.push({name:'RegisterPage'})
        }

        return {
            goToLogin,
            goToRegister,
        }
    },
    methods: {
    }
}
</script>

<style>
</style>