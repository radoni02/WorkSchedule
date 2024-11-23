<template>
  <div class="register-page container mt-5">
    <h2 class="text-center mb-4">Register</h2>

    <div class="mb-3">
      <label for="firstName" class="form-label">First Name:</label>
      <input
        type="text"
        id="firstName"
        v-model="firstName"
        placeholder="Enter your first name"
        class="form-control"
      />
    </div>

    <div class="mb-3">
      <label for="lastName" class="form-label">Last Name:</label>
      <input
        type="text"
        id="lastName"
        v-model="lastName"
        placeholder="Enter your last name"
        class="form-control"
      />
    </div>

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

    <h3 class="mb-3">Select Your Role:</h3>

    <div class="mb-3">
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
      <button @click="handleRegister" class="btn btn-primary">Register</button>
    </div>
  </div>
</template>


<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
    name: 'RegisterPage',
    setup() {
        const router = useRouter();
        
        const firstName = ref('');
        const lastName = ref('');
        const email = ref('');
        const password = ref('');
        const selectedRole = ref('');

        const handleRegister = async () => {
            if (!firstName.value || !lastName.value || !email.value || !password.value || !selectedRole.value) {
                alert('Please fill out all fields and select a role.');
                return;
            }

            let hashTable = {};
            hashTable["admin"] = 0;
            hashTable["employee"] = 2;
            hashTable["viewer"] = 1;

            const registerData = {
                name: firstName.value,
                lastname: lastName.value,
                email: email.value,
                password: password.value,
                passwordConfirmation: password.value,
                rola: hashTable[selectedRole.value], // Convert to integer for role
            };

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
                    if (selectedRole.value === 'admin') {
                        router.push({ name: 'EmployeePreferencesManager' });
                    } else if (selectedRole.value === 'employee') {
                        router.push({ name: 'PreferencesSubmitPage' });
                    } else {
                        router.push({ name: 'PreferencesViewPage' });
                    }
                } else {
                    const errorData = await response.json();
                    alert(`Error: ${errorData.message || 'Something went wrong. Please try again.'}`);
                }
            } catch (error) {
                console.error('Error during registration:', error);
                alert('Error during registration. Please try again.');
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
.register-page {
    max-width: 300px;
    margin: 0 auto;
    text-align: left;
}

label {
    display: block;
    margin: 10px 0;
    cursor: pointer;
}

input[type="text"],
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
