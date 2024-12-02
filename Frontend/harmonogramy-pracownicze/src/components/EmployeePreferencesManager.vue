<template>
  <div class="employee-preferences-manager container mt-5 p-4 bg-light shadow rounded">
    <h2 class="text-center mb-4 custom-header">Employee Preferences Manager</h2>

    <!-- Select Date -->
    <div class="mb-4">
      <div class="icons mb-3">
        <i class="fa-solid fa-calendar-days"></i>
        <label for="selectedDate" class="form-label fw-bold">Select Date:</label>
      </div>
      <input type="date" v-model="selectedDate" @change="loadPreferencesForDate" class="form-control shadow-sm" />
    </div>

    <!-- Table of Preferences -->
    <div v-if="selectedDate && employeePreferences.length > 0">
      <div class="card shadow">
        <div class="card-header bg-gold text-white fw-bold">
          Preferences for {{ selectedDate }}
        </div>
        <div class="card-body p-0">
          <table class="table table-hover table-striped mb-0">
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
                <button
                  :class="[
                                            'btn',
                                            preference.confirmed ? 'btn-success' : 'btn-light',
                                            'btn-sm'
                                        ]"
                  @click="toggleConfirmation(preference.employeeId)"
                >
                  <i class="fas" :class="preference.confirmed ? 'fa-check-circle' : 'fa-circle'"></i>
                  {{ preference.confirmed ? 'Confirmed' : 'Confirm' }}
                </button>
              </td>
              <td>
                <button class="btn bg-gold btn-sm me-2" @click="editPreference(preference)">
                  <i class="fas fa-edit"></i> Edit
                </button>
                <button class="btn btn-danger btn-sm" @click="clearWorkday(preference.employeeId)">
                  <i class="fas fa-trash-alt"></i> Clear
                </button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <p v-else-if="selectedDate" class="text-center text-muted">
      No preferences submitted for this date.
    </p>

    <!-- Form -->
    <div class="preference-form mt-4">
      <div class="card shadow">
        <div class="card-header bg-black text-white fw-bold">
          {{ isEditing ? 'Edit' : 'Create' }} Preference
        </div>
        <div class="card-body">
          <form @submit.prevent="savePreference">
            <div class="mb-3">
              <label for="employeeName" class="form-label">Employee:</label>
              <input type="text" v-model="preferenceForm.employeeName" required class="form-control shadow-sm" />
            </div>

            <div class="row">
              <div class="col-md-6 mb-3">
                <label for="startTime" class="form-label">Start Time:</label>
                <input type="time" v-model="preferenceForm.startTime" required class="form-control shadow-sm" />
              </div>

              <div class="col-md-6 mb-3">
                <label for="endTime" class="form-label">End Time:</label>
                <input type="time" v-model="preferenceForm.endTime" required class="form-control shadow-sm" />
              </div>
            </div>

            <div class="d-flex justify-content-between">
              <button type="submit" class="btn bg-gold shadow-sm bigger-button">
                <i class="fas fa-save"></i> {{ isEditing ? 'Save Changes' : 'Create Preference' }}
              </button>
              <button type="button" @click="cancelEdit" class="btn btn-outline-dark shadow-sm bigger-button">
                <i class="fas fa-times"></i> Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <!-- Submit Confirmations -->
    <div class="mt-4 text-center">
      <button @click="submitConfirmations" :disabled="!selectedDate" class="btn btn-success shadow-sm bigger-button me-2">
        <i class="fas fa-check"></i> Submit
      </button>
      <button @click="manageUsers" class="btn btn-outline-success shadow-sm bigger-button">
        <i class="fas fa-users"></i> Manage Users
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
            isEditing: false,
            preferenceForm: {
                employeeName: '',
                startTime: '',
                endTime: '',
                employeeId: null,
            },
            employeePreferences: [
                // Pre-populating with mock data for December
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Alice', startTime: '08:00', endTime: '16:00', confirmed: false, date: '2024-12-01' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Bob', startTime: '09:00', endTime: '17:00', confirmed: false, date: '2024-12-02' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Charlie', startTime: '08:30', endTime: '16:30', confirmed: false, date: '2024-12-03' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Alice', startTime: '10:00', endTime: '18:00', confirmed: false, date: '2024-12-04' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Bob', startTime: '08:00', endTime: '16:00', confirmed: false, date: '2024-12-05' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Charlie', startTime: '09:00', endTime: '17:00', confirmed: false, date: '2024-12-06' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Alice', startTime: '08:00', endTime: '16:00', confirmed: false, date: '2024-12-07' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Bob', startTime: '09:00', endTime: '17:00', confirmed: false, date: '2024-12-08' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Charlie', startTime: '08:30', endTime: '16:30', confirmed: false, date: '2024-12-09' },
                { employeeId: 'f7bb9991-f4d0-4c61-a365-66698621243e', employeeName: 'Alice', startTime: '10:00', endTime: '18:00', confirmed: false, date: '2024-12-10' },
                // Add more days in December as needed
            ],
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
    margin: 0 auto;
    text-align: center;
}

.card.shadow {
  width: 100%;
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
    width: 110px;
}

button.confirmed {
    background-color: #4caf50;
    color: white;
}

.bigger-button {
  width: 180px;
}

.bg-black {
  background: #212529;
  color: #fff;
}

.bg-gold {
  background: #776212;
  color: #fff;
}

.bg-gold:hover {
  background-color: #493c0c;
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
