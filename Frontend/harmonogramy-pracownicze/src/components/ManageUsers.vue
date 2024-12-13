<template>
  <div class="manage-users-page container mt-4">
    <div class="card shadow-lg p-4">
      <h2 class="text-center mb-4 custom-header">Manage Users</h2>

      <!-- User List Table -->
      <div v-if="users.length > 0">
        <table class="table table-bordered table-striped">
          <thead class="table-dark">
          <tr>
            <th>Name</th>
            <th>Last Name</th>
            <th>Role</th>
            <th>Action</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>{{ user.name }}</td>
            <td>{{ user.lastname }}</td>
            <td>{{ user.role }}</td>
            <td>
              <button class="btn btn-danger btn-sm" @click="deleteUser(user.id)">
                Delete
              </button>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
      <p v-else class="text-center">No users found.</p>
    </div>
  </div>
</template>

<script>
export default {
    data() {
        return {
            users: [
              { id: 'f7bb9991-f4d0-4c61-a365-66698621243s', name: 'Admin', lastname: 'Test', role: 'SuperAdmin' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621244a', name: 'Piotr', lastname: 'Jagła', role: 'Admin' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621245a', name: 'Jan', lastname: 'Paweł', role: 'Admin' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621246a', name: 'Jr', lastname: 'Tolkien', role: 'Admin' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621247e', name: 'Marcin', lastname: 'King', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621248e', name: 'Jan', lastname: 'Keys', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621249e', name: 'Alice', lastname: 'Keys', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621250e', name: 'Mamo', lastname: 'Zedong', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621251e', name: 'Coconut', lastname: 'Channel', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621252e', name: 'The', lastname: 'Beatle', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621253e', name: 'Piotro', lastname: 'Picasso', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621254e', name: 'Michał', lastname: 'Jordan', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621255e', name: 'Brat', lastname: 'Wright', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621256e', name: 'Księżna', lastname: 'Diana', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621257e', name: 'Isaac', lastname: 'Oldton', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621258e', name: 'Daria', lastname: 'Skłodowska-Curie', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621259e', name: 'Nikola', lastname: 'Kopernik', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621260e', name: 'Johann', lastname: 'Schlechtenberg', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621261e', name: 'Julian', lastname: 'Cezar', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621262e', name: 'Albert', lastname: 'Steinein', role: 'Worker' },
              { id: 'f7bb9991-f4d0-4c61-a365-66698621263e', name: 'Grota', lastname: 'Thunberg', role: 'Worker' },

            ], // Will store the list of users fetched from the API
            roles: [], // Will store the list of roles fetched from the API
        };
    },
    computed: {
        // Computed property to get the currently authenticated user
        user() {
            return this.$store.getters.getUser;
        },
        // Computed property to get the token from Vuex store or wherever it's stored
        accessToken() {
            return this.$store.getters.getAccessToken; // Assuming the token is in Vuex store under 'getAuthToken'
        },
    },
    methods: {
        // Method to fetch the list of users from the API
        async loadUsers() {
            console.log("TOKEN: ", this.accessToken)
            try {
                const response = await fetch('http://localhost:8080/api/User/ListUsers', {
                    headers: {
                        'Authorization': `Bearer ${this.accessToken}`, // Include token in the headers
                    },
                });
                if (!response.ok) {
                    throw new Error('Failed to load users');
                }
                this.users = await response.json();
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        },

        // Method to fetch the list of roles from the API
        async loadRoles() {
            try {
                const response = await fetch('http://localhost:8080/api/User/ListRoles', {
                    headers: {
                        'Authorization': `Bearer ${this.accessToken}`, // Include token in the headers
                    },
                });
                if (!response.ok) {
                    throw new Error('Failed to load roles');
                }
                this.roles = await response.json();
            } catch (error) {
                console.error('Error fetching roles:', error);
            }
        },

        deleteUser(userId) {
          // Find the user to be deleted and remove it from the list
          console.log(userId);
          this.users = this.users.filter(user => user.id !== userId);
        },

        // Method to delete a user based on their user ID
        // async deleteUser(userId) {
        //     try {
        //         const response = await fetch(`http://localhost:8080/api/User/DeleteUser/${userId}`, {
        //             method: 'DELETE',
        //             headers: {
        //                 'Authorization': `Bearer ${this.accessToken}`, // Include token in the headers
        //             },
        //         });
        //
        //         if (!response.ok) {
        //             throw new Error('Failed to delete user');
        //         }
        //
        //         // Remove the deleted user from the list
        //         this.users = this.users.filter(user => user.id !== userId);
        //     } catch (error) {
        //         console.error('Error deleting user:', error);
        //     }
        // },

        // Method to update the role of a user
        async updateUserRole(user) {
            try {
                const updatedUser = { ...user, role: user.selectedRole };
                const response = await fetch('http://localhost:8080/api/User/ModifyUser', {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${this.accessToken}`, // Include token in the headers
                    },
                    body: JSON.stringify(updatedUser),
                });

                if (!response.ok) {
                    throw new Error('Failed to update user role');
                }

                console.log('User role updated successfully');
            } catch (error) {
                console.error('Error updating user role:', error);
            }
        },
    },
    mounted() {
        // Fetch users and roles when the component is mounted
        this.loadUsers();
        this.loadRoles();
    },
};
</script>

<style scoped>
.manage-users-page {
    max-width: 850px;
    margin: 0 auto;
}

.card {
  width: 100%;
}

.table th, .table td {
    text-align: center;
}

button {
    cursor: pointer;
}
</style>
