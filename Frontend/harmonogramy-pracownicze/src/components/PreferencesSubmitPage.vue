<template>
    <div class="preferences-submit-page container mt-5">
      <h2 class="text-center mb-4">Schedule Preferences for Upcoming Month</h2>
  
      <table class="table table-bordered table-striped">
        <thead>
          <tr>
            <th>Date</th>
            <th>Start Time</th>
            <th>End Time</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="day in days" :key="day.date">
            <td>{{ day.date }}</td>
  
            <td>
              <select v-model="day.startTime" class="form-select">
                <option v-for="time in timeOptions" :key="time" :value="time">
                  {{ time }}
                </option>
              </select>
            </td>
  
            <td>
              <select v-model="day.endTime" class="form-select">
                <option v-for="time in timeOptions" :key="time" :value="time">
                  {{ time }}
                </option>
              </select>
            </td>
          </tr>
        </tbody>
      </table>
  
      <div class="text-center">
        <button @click="submitPreferences" class="btn btn-primary">Submit Preferences</button>
      </div>
    </div>
  </template>
  
  <script>
  import { onMounted, ref } from 'vue';
import { useStore } from 'vuex';
  
  export default {
    name: 'PreferencesSubmitPage',
    setup() {
      const store = useStore();
      const userID = store.getters.getUserID;
  
      const days = ref([]);
      const timeOptions = ref([]);
  
      const generateTimeOptions = () => {
        for (let hour = 0; hour < 24; hour++) {
          const formattedHour = hour.toString().padStart(2, '0');
          timeOptions.value.push(`${formattedHour}:00`, `${formattedHour}:30`);
        }
      };
  
      const generateDaysForUpcomingMonth = () => {
        const currentDate = new Date();
        const currentMonth = currentDate.getMonth();
        const upcomingMonth = currentMonth === 11 ? 0 : currentMonth + 1; // January is 0
        const year = currentMonth === 11 ? currentDate.getFullYear() + 1 : currentDate.getFullYear();
  
        const daysInMonth = new Date(year, upcomingMonth + 1, 0).getDate();
  
        for (let day = 1; day <= daysInMonth; day++) {
          const date = new Date(year, upcomingMonth, day).toLocaleDateString();
          days.value.push({
            date,
            startTime: '',
            endTime: '', 
          });
        }
      };
  
      const submitPreferences = async () => {
        for (const day of days.value) {
          if (day.startTime && day.endTime) { 
            const preference = {
              userId: userID, 
              date: day.date,
              startTime: day.startTime,
              endTime: day.endTime,
            };
  
            try {
              const response = await fetch('http://localhost:8080/api/Preferences/SetUserPreference', {
                method: 'PUT',
                headers: {
                  'Content-Type': 'application/json',
                },
                body: JSON.stringify(preference), 
              });
  
              if (response.ok) {
                const data = await response.json();
                console.log('Preference submitted successfully:', data);
              } else {
                const errorText = await response.text();
                console.error('Error submitting preference:', errorText);
                alert(`Error submitting preference: ${errorText}`);
              }
            } catch (error) {
              console.error('Error during submission:', error);
              alert('There was an error submitting your preference.');
            }
          }
        }
        alert('Preferences submitted successfully!');
      };
  
      onMounted(() => {
        generateTimeOptions();
        generateDaysForUpcomingMonth();
      });
  
      return {
        days,
        timeOptions,
        submitPreferences,
      };
    },
  };
  </script>
  
  <style>
  .preferences-submit-page {
    max-width: 600px;
    margin: 0 auto;
    text-align: center;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
    margin: 20px 0;
  }
  
  table,
  th,
  td {
    border: 1px solid #ddd;
  }
  
  th,
  td {
    padding: 10px;
  }
  
  select {
    padding: 5px;
    font-size: 16px;
  }
  
  button {
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
  }
  </style>
  