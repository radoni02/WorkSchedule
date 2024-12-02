<template>
  <div class="preferences-view-page container mt-5 p-4 shadow rounded">
    <h2 class="text-center mb-5 custom-header">Employee Preferences for Selected Day</h2>

    <div class="date-picker mb-4">
      <div class="icons mb-3">
        <i class="fa-solid fa-calendar-days"></i>
        <label for="selectedDate" class="form-label fw-bold">Select a Date:</label>
      </div>
      <input
        type="date"
        v-model="selectedDate"
        @change="loadPreferencesForDate"
        class="form-control border-primary"
        placeholder="Choose a date..."
      />
    </div>

    <div v-if="selectedDate && employeePreferences.length > 0" class="preferences-table mt-4">
      <h3 class="mb-4 text-center text-secondary">Submitted Preferences</h3>
      <table class="table table-hover align-middle">
        <thead class="table-primary text-center">
        <tr>
          <th>Employee</th>
          <th>Start Time</th>
          <th>End Time</th>
          <th>Note</th>
          <th>Actions</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="preference in employeePreferences" :key="preference.employeeId" class="text-center">
          <td>{{ preference.employeeName }}</td>
          <td>{{ preference.startTime }}</td>
          <td>{{ preference.endTime }}</td>
          <td>{{ preference.note || 'No notes added' }}</td>
          <td>
            <button
              class="btn btn-outline-info btn-sm"
              @click="selectPreferenceForNote(preference)"
            >
              Edit Note
            </button>
          </td>
        </tr>
        </tbody>
      </table>
    </div>

    <p v-else-if="selectedDate" class="text-center text-muted fs-5">No preferences submitted for this date.</p>

    <div v-if="selectedPreference" class="note-editor mt-5 p-3 border rounded shadow-sm">
      <h3 class="text-primary">Edit Note</h3>
      <form @submit.prevent="saveNote" class="mt-3">
        <div class="mb-3">
          <label for="note" class="form-label fw-bold">Note:</label>
          <textarea
            v-model="selectedPreference.note"
            class="form-control border-primary"
            rows="3"
            placeholder="Enter a note here..."
          ></textarea>
        </div>

        <div class="d-flex justify-content-end">
          <button type="submit" class="btn btn-success me-2">Save</button>
          <button type="button" @click="clearSelectedPreference" class="btn btn-secondary">
            Cancel
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { useStore } from 'vuex';

export default {
    data() {
        return {
            selectedDate: '',
            employeePreferences: [],
            selectedPreference: null,
        };
    },
    methods: {
        async loadPreferencesForDate() {
            if (!this.selectedDate) return;

            const date = this.selectedDate;
            const end = '';

            const store = useStore();
            const userID = store.getters.getUserID;

            try {
                const url = `/api/WorkDay/GetWorkdays/${date}?end=${end}&userID=${userID}`;
                const response = await fetch(url);

                if (!response.ok) {
                    throw new Error('Failed to fetch preferences');
                }

                const data = await response.json();
                this.employeePreferences = data || [];
            } catch (error) {
                console.error('Error fetching preferences:', error);
            }
        },

        selectPreferenceForNote(preference) {
            this.selectedPreference = { ...preference };
        },

        saveNote() {
            const preferenceIndex = this.employeePreferences.findIndex(
                (pref) => pref.employeeId === this.selectedPreference.employeeId
            );

            if (preferenceIndex !== -1) {
                this.employeePreferences[preferenceIndex].note = this.selectedPreference.note;
            }

            this.clearSelectedPreference();
        },

        clearSelectedPreference() {
            this.selectedPreference = null;
        },
    },
};
</script>

<style scoped>
.preferences-view-page {
  max-width: 800px;
  background: #fff;
  border-radius: 15px;
  padding: 30px;
}

.date-picker label {
  font-weight: bold;
}

.table {
  border-radius: 10px;
  overflow: hidden;
}

.table-hover tbody tr:hover {
  background-color: rgba(0, 123, 255, 0.1);
}

.note-editor {
  background-color: #fefefe;
}

.note-editor h3 {
  font-size: 1.5rem;
}

button {
  transition: all 0.3s;
}

button:hover {
  transform: translateY(-2px);
}
</style>
