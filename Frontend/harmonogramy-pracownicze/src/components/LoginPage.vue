<template>
  <div class="login-page container d-flex justify-content-center align-items-center vh-100">
    <div class="card shadow-lg p-4">
      <h2 class="text-center mb-4 custom-header">Welcome Back</h2>
      <p class="text-center text-muted mb-4">Log in to your account</p>

      <div class="mb-3">
        <label for="email" class="form-label">Email:</label>
        <div class="input-group">
          <input
            type="email"
            id="email"
            v-model="email"
            placeholder="Enter your email"
            class="form-control"
          />
        </div>
      </div>

      <div class="mb-3">
        <label for="password" class="form-label">Password:</label>
        <div class="input-group">
          <input
            type="password"
            id="password"
            v-model="password"
            placeholder="Enter your password"
            class="form-control"
          />
        </div>
      </div>

      <div class="d-flex align-items-center mb-3">
        <div>
          <input class="marr-05" type="checkbox" id="rememberMe" />
          <label for="rememberMe" class="form-check-label">Remember me</label>
        </div>
      </div>

      <div class="text-center">
        <button @click="handleSubmit" class="register-button btn btn-primary w-100">Login</button>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useStore } from 'vuex';

export default {
  name: 'LoginPage',
  setup() {
    const router = useRouter();

    const email = ref('');
    const password = ref('');

    const store = useStore();

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

          const tok = loginData.accessToken;
          store.commit('setAccessToken', tok);

          const userInfoResponse = await fetch('http://localhost:8080/api/Auth/GetUserDetails', {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${tok}`, // Include the access token
            },
          });

          if (userInfoResponse.ok) {
            const userInfo = await userInfoResponse.json();
            console.log('User Info retrieved:', userInfo);
            store.dispatch('updateUser', userInfo);

            if (userInfo.role === 'Admin') {
              // router.push({ name: 'EmployeePreferencesManager' });
              router.push({ name: 'ManageUsers' });
            } else if (userInfo.role === 'SuperAdmin') {
              // router.push({ name: 'ManageUsers' });
              router.push({ name: 'EmployeePreferencesManager' });
            } else if (userInfo.role === 'Worker') {
              router.push({ name: 'PreferencesSubmitPage' });
            } else if (userInfo.role === 'Spectator') {
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
  max-width: 700px;
}

.marr-05 {
  margin-right: 0.5em;
}
</style>
