<template>
    <div class="preferences-view-page container mt-4">
        <h2 class="text-center mb-4">Employee Preferences for Selected Day</h2>

        <div class="mb-4">
            <label for="selectedDate" class="form-label">Select Date:</label>
            <input type="date" v-model="selectedDate" @change="loadPreferencesForDate" class="form-control" />
        </div>

        <div v-if="selectedDate && employeePreferences.length > 0">
            <h3 class="mb-3">Existing Preferences</h3>
            <table class="table table-bordered table-striped">
                <thead class="table-dark">
                    <tr>
                        <th>Employee</th>
                        <th>Start Time</th>
                        <th>End Time</th>
                        <th>Note</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="preference in employeePreferences" :key="preference.employeeId">
                        <td>{{ preference.employeeName }}</td>
                        <td>{{ preference.startTime }}</td>
                        <td>{{ preference.endTime }}</td>
                        <td>{{ preference.note }}</td>
                        <td>
                            <button class="btn btn-info btn-sm" @click="selectPreferenceForNote(preference)">
                                Edit Note
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <p v-else-if="selectedDate" class="text-center">No preferences submitted for this date.</p>

        <div v-if="selectedPreference" class="mt-4">
            <h3>Edit Note for Preference</h3>
            <form @submit.prevent="saveNote" class="mt-3">
                <div class="mb-3">
                    <label for="note" class="form-label">Note:</label>
                    <textarea v-model="selectedPreference.note" class="form-control" rows="3"
                        placeholder="Add or update a note for this preference..."></textarea>
                </div>

                <div class="d-flex justify-content-between">
                    <button type="submit" class="btn btn-primary">
                        Save Note
                    </button>
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
    max-width: 900px;
    margin: 0 auto;
}
</style>
