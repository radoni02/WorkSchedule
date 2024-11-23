<template>
  <div class="preferences-view-page container mt-4">
    <h2 class="text-center mb-4">View Employee Preferences</h2>

    <div class="mb-4">
      <label for="selectedDate" class="form-label">Select Date:</label>
      <input
        type="date"
        v-model="selectedDate"
        @change="loadPreferencesForDate"
        class="form-control"
      />
    </div>

    <div v-if="selectedDate && employeePreferences.length > 0">
      <h3 class="mb-3">Employee Preferences</h3>
      <table class="table table-bordered table-striped">
        <thead class="table-dark">
          <tr>
            <th>Employee</th>
            <th>Start Time</th>
            <th>End Time</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="preference in employeePreferences" :key="preference.employeeId">
            <td>{{ preference.employeeName }}</td>
            <td>{{ preference.startTime }}</td>
            <td>{{ preference.endTime }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <p v-else-if="selectedDate" class="text-center">No preferences submitted for this date.</p>
  </div>
</template>

<script>
import { ref } from 'vue';

export default {
  name: 'PreferencesViewPage',
  computed: {
    user() {
      return this.$store.getters.getUser;
    },
  },
  setup() {
    const selectedDate = ref('');
    const employeePreferences = ref([]);
    const loading = ref(false);

    const loadPreferencesForDate = async () => {
      if (!selectedDate.value) return;

      loading.value = true;

      try {
        // API request to get preferences
        const response = await fetch(
          `http://localhost:8080/api/Preferences/GetPreferences/${selectedDate.value}&userID=${this.user().userID}`,
          {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
            },
          }
        );

        if (response.ok) {
          const data = await response.json();
          employeePreferences.value = data; 
        } else {
          console.error('Error fetching preferences:', await response.text());
          alert('Error fetching preferences.');
        }
      } catch (error) {
        console.error('Error:', error);
        alert('An error occurred while fetching preferences.');
      } finally {
        loading.value = false;
      }
    };

    return {
      selectedDate,
      employeePreferences,
      loadPreferencesForDate,
      loading,
    };
  },
};
</script>

<style>
.preferences-view-page {
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
