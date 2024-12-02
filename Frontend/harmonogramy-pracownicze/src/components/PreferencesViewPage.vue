<template>
  <div class="preferences-view-page container mt-5 p-4 shadow rounded">
    <h2 class="text-center mb-4 custom-header">Employee Preferences Overview</h2>

    <div class="date-picker mb-4">
      <div class="icons mb-3">
        <i class="fa-solid fa-calendar-days"></i>
        <label for="selectedDate" class="form-label fw-bold">Choose a Date:</label>
      </div>
      <input
        type="date"
        v-model="selectedDate"
        @change="loadPreferencesForDate"
        class="form-control border-primary"
      />
    </div>

    <div v-if="selectedDate && employeePreferences.length > 0" class="preferences-table">
      <h3 class="mb-4 text-secondary">Preferences Details</h3>
      <table class="table table-bordered table-striped align-middle">
        <thead class="table-dark text-center">
        <tr>
          <th>Employee</th>
          <th>Start Time</th>
          <th>End Time</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="preference in employeePreferences" :key="preference.employeeId" class="text-center">
          <td>{{ preference.name }}</td>
          <td>{{ preference.start }}</td>
          <td>{{ preference.end }}</td>
        </tr>
        </tbody>
      </table>
    </div>

    <p v-else-if="selectedDate" class="text-center text-muted fs-5">No preferences submitted for this date.</p>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useStore } from 'vuex';

export default {
  name: 'PreferencesViewPage',
  setup() {
    const store = useStore();  // Accessing the store with useStore() in setup
    const selectedDate = ref('');
    const employeePreferences = ref([]);
    const loading = ref(false);

    // Method to fetch preferences based on the selected date
    const loadPreferencesForDate = async () => {
      if (!selectedDate.value) return;

      loading.value = true;

      try {
        // Build the URL with the selected date, optional 'end' date, and user ID
        let url = `http://localhost:8080/api/Preferences/GetPreferences/${selectedDate.value}`;

        // Access user info from the store
        //const user = store.getters.getUser;  // Get current user from Vuex store

        // API request with query parameters
        const response = await fetch(url, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${store.getters.getAccessToken}`, // Use token from Vuex store
          },
          //body: JSON.stringify({userID: user.id})
        });

        if (response.ok) {
          const data = await response.json();
          employeePreferences.value = data;  // Set preferences data to the employeePreferences ref
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

<style scoped>
.preferences-view-page {
  max-width: 900px;
  margin: 0 auto;
  background: #fff;
  border-radius: 15px;
  padding: 25px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.date-picker label {
  font-weight: bold;
}

.table {
  margin-top: 20px;
}

.table th,
.table td {
  text-align: center;
  padding: 10px;
}

.table-bordered {
  border-radius: 10px;
  overflow: hidden;
}

.text-muted {
  font-style: italic;
  color: rgb(128, 128, 128);
}

h2 {
  font-weight: bold;
}
</style>
