<template>
  <div class="login-page container mt-5">
    <h2 class="text-center mb-4">Login</h2>

    <div class="mb-3">
      <label for="email" class="form-label">Email:</label>
      <input
        type="email"
        id="email"
        v-model="email"
        placeholder="Enter your email"
        class="form-control"
      />
    </div>

    <div class="mb-3">
      <label for="password" class="form-label">Password:</label>
      <input
        type="password"
        id="password"
        v-model="password"
        placeholder="Enter your password"
        class="form-control"
      />
    </div>


    <div class="text-center">
      <button @click="handleSubmit" class="btn btn-primary">Submit</button>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  name: 'LoginPage',
  setup() {
    const router = useRouter();

    const email = ref('');
    const password = ref('');

    const handleSubmit = async () => {
      if (!email.value || !password.value) {
        alert('Please enter an email, password, and select a role.');
        return;
      }

      const loginData = {
        email: email.value,
        password: password.value,
      };

      try {

        const loginResponse = await fetch('http://localhost:8080/api/Auth/Login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(loginData),
        });

        if (loginResponse.ok) {
          const loginData = await loginResponse.json();
          console.log('Login successful:', loginData);

          const userInfoResponse = await fetch('http://localhost:8080/api/Auth/GetUserInfo', {
            method: 'GET',
            headers: {
              'Authorization': `Bearer ${loginData.token}`,
            },
          });

          if (userInfoResponse.ok) {
            const userInfo = await userInfoResponse.json();
            console.log('User Info retrieved:', userInfo);
            this.$store.dispatch('updateUser', userInfo);

            if (userInfo.role === 'admin') {
              router.push({ name: 'EmployeePreferencesManager' });
            } else if (userInfo.role === 'employee') {
              router.push({ name: 'EmployeePreferencesView' });
            } else if (userInfo.role === 'viewer') {
              router.push({ name: 'PreferencesViewPage' });
            }
          } else {
            const errorText = await userInfoResponse.text();
            console.error('Failed to get user info:', errorText);
            alert(`Failed to get user info: ${errorText}`);
          }
        } else {
          const errorText = await loginResponse.text();
          console.error('Login failed:', errorText);
          alert(`Login failed: ${errorText}`);
        }
      } catch (error) {
        console.error('Error during login or user info fetch:', error);
        alert('There was an error during login, please try again.');
      }

    };

    return {
      email,
      password,
      handleSubmit,
    };
  },
};
</script>

<style>
.login-page {
  max-width: 300px;
  margin: 0 auto;
  text-align: left;
}

label {
  display: block;
  margin: 10px 0;
  cursor: pointer;
}

input[type="email"],
input[type="password"] {
  width: 100%;
  padding: 8px;
  margin-bottom: 15px;
}

button {
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
}
</style>
