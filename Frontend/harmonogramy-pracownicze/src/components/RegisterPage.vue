<template>
  <div class="register-page container d-flex justify-content-center align-items-center vh-100">
    <div class="card shadow-lg p-4">
      <h2 class="text-center mb-4 custom-header">Create an Account</h2>
      <p class="text-center text-muted mb-4">Join us today and explore the platform</p>

      <div class="mb-3">
        <div class="icons mb-3">
          <i class="fa-solid fa-user"></i>
          <label for="firstName" class="form-label">First Name:</label>
        </div>
        <div class="input-group">
          <input
            type="text"
            id="firstName"
            v-model="firstName"
            placeholder="Enter your first name"
            class="form-control"
          />
        </div>
      </div>

      <div class="mb-3">
        <div class="icons mb-3">
          <i class="fa-solid fa-user"></i>
          <label for="lastName" class="form-label">Last Name:</label>
        </div>
        <div class="input-group">
          <input
            type="text"
            id="lastName"
            v-model="lastName"
            placeholder="Enter your last name"
            class="form-control"
          />
        </div>
      </div>

      <div class="mb-3">
        <div class="icons mb-3">
          <i class="fa-solid fa-envelope"></i>
          <label for="email" class="form-label">Email:</label>
        </div>
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
        <div class="icons mb-3">
          <i class="fa-solid fa-lock"></i>
          <label for="password" class="form-label">Password:</label>
        </div>
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

      <h5 class="text-center mb-3 custom-header">Select Your Role:</h5>
      <div class="ml-4 mb-3">
        <div class="form-check">
          <input
            type="radio"
            id="admin"
            name="role"
            value="admin"
            v-model="selectedRole"
            class="form-check-input"
          />
          <label for="admin" class="form-check-label">Admin</label>
        </div>
        <div class="form-check">
          <input
            type="radio"
            id="employee"
            name="role"
            value="employee"
            v-model="selectedRole"
            class="form-check-input"
          />
          <label for="employee" class="form-check-label">Employee</label>
        </div>
        <div class="form-check">
          <input
            type="radio"
            id="viewer"
            name="role"
            value="viewer"
            v-model="selectedRole"
            class="form-check-input"
          />
          <label for="viewer" class="form-check-label">Viewer</label>
        </div>
      </div>

      <div class="text-center">
        <button @click="handleRegister" class="register-button btn btn-primary w-100">Register</button>
      </div>

      <p class="text-center text-muted mt-4">
        Already have an account?
        <a href="#" class="login-link text-decoration-none">Log in here</a>
      </p>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useStore } from 'vuex';

export default {
    name: 'RegisterPage',
    setup() {
        const router = useRouter();

        const firstName = ref('');
        const lastName = ref('');
        const email = ref('');
        const password = ref('');
        const selectedRole = ref('');

        const store = useStore();

      const handleRegister = async () => {
        if (!firstName.value || !lastName.value || !email.value || !password.value || !selectedRole.value) {
          alert('Please fill out all fields and select a role.');
          return;
        }


        let hashTable = {};
        //hashTable["SuperAdmin"] = 0;
        hashTable["admin"] = 0;
        hashTable["viewer"] = 2;
        hashTable["employee"] = 3;

        const registerData = {
          name: firstName.value,
          lastname: lastName.value,
          email: email.value,
          password: password.value,
          passwordConfirmation: password.value,
          rola: hashTable[selectedRole.value], // Convert to integer for role
        };
        console.log("Dane rejestracij: ", registerData)

        try {
          const response = await fetch('http://localhost:8080/api/Auth/rejstacja', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify(registerData),
          });

          if (response.ok) {
            const data = await response.json();
            console.log('Registration successful:', data);

            // Store access token in Vuex
            const tok = data.accessToken;
            store.commit('setAccessToken', tok);

            // Fetch user details using the retrieved access token
            const userDetailsResponse = await fetch('http://localhost:8080/api/Auth/GetUserDetails', {
              method: 'GET',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${tok}`, // Include the access token
              },
            });

            if (userDetailsResponse.ok) {
              const userDetails = await userDetailsResponse.json();
              console.log('User details retrieved:', userDetails);

              // Store user details in Vuex
              store.commit('setUser', userDetails);

              // Role-based navigation
              if (selectedRole.value === 'admin') {
                router.push({ name: 'EmployeePreferencesManager' });
              } else if (selectedRole.value === 'employee') {
                router.push({ name: 'PreferencesSubmitPage' });
              } else {
                router.push({ name: 'PreferencesViewPage' });
              }
            } else {
              const errorDetails = await userDetailsResponse.json();
              alert(`Error fetching user details: ${errorDetails.message || 'Please try again.'}`);
            }
          } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || 'Something went wrong. Please try again.'}`);
          }
        } catch (error) {
          console.error('Error during registration or fetching user details:', error);
          alert('Error during registration or fetching user details. Please try again.');
        }

      };

        return {
            firstName,
            lastName,
            email,
            password,
            selectedRole,
            handleRegister,
        };
    },
};
</script>

<style>
html, body {
  height: 100%;
  margin: 0;
  padding: 0;
}

body {
  background: url('/main.jpg') no-repeat center center;
  background-size: cover;
  font-family: 'Roboto', sans-serif;
}

.register-page {
  max-width: 450px;
}

.card {
  width: 450px;
  border: none;
  border-radius: 10px;
  background-color: rgba(255, 255, 255, 0.9);
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
}

input {
  border: none;
  border-radius: 5px;
  box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.1);
}

input:focus {
  outline: none;
  box-shadow: inset 0 0 5px #776212;
}

.register-button {
  background-color: #776212;
  color: white;
  border: none;
}

.register-button:hover {
  background-color: #493c0c;
  color: white;
}

.login-link {
  color: #776212;
}

.login-link:hover {
  color: #493c0c;
  text-decoration: underline;
}

button {
  border: none;
  border-radius: 5px;
}

button:hover {
  background-color: #493c0c;
  color: #fff;
}

.custom-header {
  color: #776212;
  font-weight: bold;
}

a {
  font-size: 14px;
}

a:hover {
  text-decoration: underline;
}

.icons {
  display: flex;
  align-items: center;
  gap: 8px;
}

.icons i {
  font-size: 18px;
  color: #776212;
}

.icons label {
  margin: 0;
}
</style>
