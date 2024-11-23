<template>
    <div class="manage-users-page container mt-4">
        <h2 class="text-center mb-4">Manage Users</h2>

        <!-- User List Table -->
        <div v-if="users.length > 0">
            <h3 class="mb-3">Users List</h3>
            <table class="table table-bordered table-striped">
                <thead class="table-dark">
                    <tr>
                        <th>Username</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="user in users" :key="user.id">
                        <td>{{ user.username }}</td>
                        <td>{{ user.email }}</td>
                        <td>
                            <select v-model="user.selectedRole" @change="updateUserRole(user)" class="form-control">
                                <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
                            </select>
                        </td>
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
</template>

<script>

export default {
    data() {
        return {
            users: [], 
            roles: [], 
        };
    },
    computed: {
        user() {
            return this.$store.getters.getUser;
        },
    },
    methods: {
        async loadUsers() {
            try {
                const response = await fetch('/api/User/ListUsers');
                if (!response.ok) {
                    throw new Error('Failed to load users');
                }
                this.users = await response.json();
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        },

        async loadRoles() {
            try {
                const response = await fetch('/api/User/ListRoles');
                if (!response.ok) {
                    throw new Error('Failed to load roles');
                }
                this.roles = await response.json();
            } catch (error) {
                console.error('Error fetching roles:', error);
            }
        },

        async deleteUser(userId) {
            try {
                const response = await fetch(`/api/User/DeleteUser/${userId}`, {
                    method: 'DELETE',
                });

                if (!response.ok) {
                    throw new Error('Failed to delete user');
                }

                this.users = this.users.filter(user => user.id !== userId);
            } catch (error) {
                console.error('Error deleting user:', error);
            }
        },

        async updateUserRole(user) {
            try {
                const updatedUser = { ...user, role: user.selectedRole };
                const response = await fetch('/api/User/ModifyUser', {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
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
        this.loadUsers();
        this.loadRoles();
    },
};
</script>

<style scoped>
.manage-users-page {
    max-width: 900px;
    margin: 0 auto;
}

.table th, .table td {
    text-align: center;
}

button {
    cursor: pointer;
}
</style>
