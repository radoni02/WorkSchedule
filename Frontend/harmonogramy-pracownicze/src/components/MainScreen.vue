<template>
  <div class="main-page">
    <div class="content-wrapper">
      <h1 class="title">Welcome to Employee Schedules</h1>
      <p class="subtitle">Choose an option to get started</p>

      <div class="button-group">
        <button @click="goToLogin" class="btn btn-primary custom-btn">
          Go to Login Page
        </button>

        <button @click="goToRegister" class="btn btn-primary custom-btn">
          Go to Register Page
        </button>
      </div>
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

<style scoped>
/* Cały ekran */
.main-page {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: url('/main.jpg') no-repeat center center;
  background-size: cover;
  font-family: 'Arial', sans-serif;
}

/* Główna ramka */
.content-wrapper {
  text-align: center;
  padding: 2rem 3rem;
  background: rgba(255, 255, 255, 0.9); /* Transparentne tło dla kontrastu */
  border-radius: 12px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  width: 70%;
}

/* Tytuł */
.title {
  font-size: 2.5rem;
  font-weight: bold;
  color: #333;
  margin-bottom: 4rem;
}

/* Podtytuł */
.subtitle {
  font-size: 1.25rem;
  color: #666;
  margin-bottom: 2rem;
}

.button-group {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.custom-btn {
  padding: 0.75rem 1.5rem;
  font-size: 1.1rem;
  font-weight: bold;
  border-radius: 8px;
  transition: all 0.3s ease-in-out;
}

.custom-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}

.btn-primary {
  background-color: #776212;
  color: white;
  border: none;
  width: 40%;
}

.btn-primary:hover {
  background-color: #493c0c;
}
</style>
