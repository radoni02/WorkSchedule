<template>
    <div class="employee-preferences-manager container mt-4">
        <h2 class="text-center mb-4">Employee Preferences for Selected Day</h2>

        <div class="mb-4">
            <label for="selectedDate" class="form-label">Select Date:</label>
            <input type="date" v-model="selectedDate" @change="loadPreferencesForDate" class="form-control" />
        </div>

        <div v-if="selectedDate && employeePreferences.length > 0">
            <table class="table table-bordered table-striped">
                <thead class="table-dark">
                    <tr>
                        <th>Employee</th>
                        <th>Start Time</th>
                        <th>End Time</th>
                        <th>Confirm</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="preference in employeePreferences" :key="preference.employeeId">
                        <td>{{ preference.employeeName }}</td>
                        <td>{{ preference.startTime }}</td>
                        <td>{{ preference.endTime }}</td>
                        <td>
                            <button :class="[ 
                                'btn', 
                                preference.confirmed ? 'btn-success' : 'btn-warning', 
                                'btn-sm' 
                            ]" @click="toggleConfirmation(preference.employeeId)">
                                {{ preference.confirmed ? 'Confirmed' : 'Confirm' }}
                            </button>
                        </td>
                        <td>
                            <button class="btn btn-info btn-sm" @click="editPreference(preference)">
                                Edit
                            </button>
                            <button class="btn btn-danger btn-sm" @click="clearWorkday(preference.employeeId)">
                                Clear
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <p v-else-if="selectedDate" class="text-center">No preferences submitted for this date.</p>

        <div class="preference-form mt-4">
            <h3>{{ isEditing ? 'Edit' : 'Create' }} Preference</h3>
            <form @submit.prevent="savePreference" class="mt-3">
                <div class="mb-3">
                    <label for="employeeName" class="form-label">Employee:</label>
                    <input type="text" v-model="preferenceForm.employeeName" required class="form-control" />
                </div>

                <div class="mb-3">
                    <label for="startTime" class="form-label">Start Time:</label>
                    <input type="time" v-model="preferenceForm.startTime" required class="form-control" />
                </div>

                <div class="mb-3">
                    <label for="endTime" class="form-label">End Time:</label>
                    <input type="time" v-model="preferenceForm.endTime" required class="form-control" />
                </div>

                <div class="d-flex justify-content-between">
                    <button type="submit" class="btn btn-primary">
                        {{ isEditing ? 'Save Changes' : 'Create Preference' }}
                    </button>
                    <button type="button" @click="cancelEdit" class="btn btn-secondary">
                        Cancel
                    </button>
                </div>
            </form>
        </div>

        <div class="mt-4 text-center">
            <button @click="submitConfirmations" :disabled="!selectedDate" class="btn btn-success">
                Submit Confirmations
            </button>
        </div>
        <div class="mt-4 text-center">
            <button @click="manageUsers" class="btn btn-success">
                ManageUsers
            </button>
        </div>
    </div>
</template>

<script>
import { useRouter } from 'vue-router';

export default {
    name: 'EmployeePreferencesManager',
    data() {
        return {
            selectedDate: '',
            employeePreferences: [],
            isEditing: false,
            preferenceForm: {
                employeeName: '',
                startTime: '',
                endTime: '',
                employeeId: null,
            },
        };
    },
    methods: {
        async loadPreferencesForDate() {
            if (!this.selectedDate) return;
            try {
                const response = await fetch(`/api/WorkDay/GetWorkdays/${this.selectedDate}`);
                if (!response.ok) throw new Error('Failed to fetch preferences');
                const data = await response.json();
                this.employeePreferences = data || [];
            } catch (error) {
                console.error('Error fetching preferences:', error);
            }
        },

        async toggleConfirmation(employeeId) {
            const preference = this.employeePreferences.find(p => p.employeeId === employeeId);
            if (preference) {
                preference.confirmed = !preference.confirmed;
                await this.savePreference(preference);
            }
        },

        editPreference(preference) {
            this.preferenceForm = { ...preference };
            this.isEditing = true;
        },

        async savePreference(preference) {
            try {
                const url = '/api/WorkDay/SetUserWorkday';
                const method = 'PUT';
                const body = JSON.stringify({
                    userId: preference.employeeId,
                    startTime: preference.startTime,
                    endTime: preference.endTime,
                    date: this.selectedDate,
                });
                const response = await fetch(url, {
                    method,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body,
                });

                if (!response.ok) throw new Error('Failed to save preference');
                await this.loadPreferencesForDate();
            } catch (error) {
                console.error('Error saving preference:', error);
            }
        },

        resetForm() {
            this.preferenceForm = { employeeName: '', startTime: '', endTime: '', employeeId: null };
            this.isEditing = false;
        },

        manageUsers() {
            let router = useRouter();
            router.push({name: "ManageUsers"})

        },

        cancelEdit() {
            this.resetForm();
        },

        async submitConfirmations() {
            const confirmedPreferences = this.employeePreferences
                .filter(preference => preference.confirmed)
                .map(preference => ({
                    employeeId: preference.employeeId,
                    date: this.selectedDate,
                    startTime: preference.startTime,
                    endTime: preference.endTime,
                }));

            try {
                const url = '/api/WorkDay/TransferUserPreferences';
                const method = 'POST';
                const body = JSON.stringify(confirmedPreferences);

                const response = await fetch(url, {
                    method,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body,
                });

                if (!response.ok) throw new Error('Failed to submit confirmations');
                alert('Confirmed preferences submitted successfully!');
            } catch (error) {
                console.error('Error submitting confirmations:', error);
            }
        },

        async clearWorkday(employeeId) {
            try {
                const url = `/api/WorkDay/ClearUserWorkday/${employeeId}/${this.selectedDate}`;
                const method = 'DELETE';

                const response = await fetch(url, { method });
                if (!response.ok) throw new Error('Failed to clear workday');
                await this.loadPreferencesForDate();
            } catch (error) {
                console.error('Error clearing workday:', error);
            }
        },
    },
};
</script>

<style>
.employee-preferences-manager {
    max-width: 800px;
    margin: 0 auto;
    text-align: center;
}

table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 20px;
}

table,
th,
td {
    border: 1px solid #ddd;
}

th,
td {
    padding: 8px;
    text-align: center;
}

button {
    padding: 5px 10px;
    font-size: 14px;
    cursor: pointer;
}

button.confirmed {
    background-color: #4caf50;
    color: white;
}

.preference-form {
    margin-top: 20px;
    text-align: left;
}

.preference-form label {
    display: block;
    margin-bottom: 5px;
}

.preference-form input {
    margin-bottom: 10px;
}

.preference-form button {
    margin-top: 10px;
}
</style>
