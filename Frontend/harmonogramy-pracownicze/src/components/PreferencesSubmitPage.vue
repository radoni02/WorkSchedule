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
            const user = store.getters.getUser;
            const tok = store.getters.getAccessToken;
            const preference = {
              userId: user.id, 
              date: day.date,
              startTime: day.startTime,
              endTime: day.endTime,
            };

            // Convert date to "YYYY-MM-DD" format from "DD.MM.YYYY" format (assuming the date format is "1.12.2024")
            const [dayValue, monthValue, yearValue] = preference.date.split(".");
            const formattedDate = `${yearValue}-${monthValue.padStart(2, '0')}-${dayValue.padStart(2, '0')}`;

            // Combine the formatted date with the start and end times to create valid ISO strings
            const start = new Date(`${formattedDate}T${preference.startTime}:00`).toISOString();
            const end = new Date(`${formattedDate}T${preference.endTime}:00`).toISOString();

            // Assuming user.name and user.surname are available from the user object
            const userName = user.name || "John";  // Replace with actual user name from Vuex or props
            const userSurname = user.surname || "Doe";  // Replace with actual surname from Vuex or props

            // Construct the parameters as required by the API
            const apiParameters = {
              name: userName,   // User's name
              surname: userSurname, // User's surname
              userId: preference.userId, // User ID
              start: start,     // ISO formatted start time
              end: end,         // ISO formatted end time
            };

            console.log("API:", apiParameters);  // Check the final outputk
  
            try {
              const response = await fetch('http://localhost:8080/api/Preferences/SetUserPreference', {
                method: 'PUT',
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${tok}`, // Include the access token
                },
                body: JSON.stringify(apiParameters), 
              });
  
              if (response.ok) {
                const data = await response.json();
                console.log('Preference submitted successfully:', data);
              } else {
                const errorText = await response.text();
                console.error('Error submitting preference:', errorText);
                alert('Preferences submitted successfully!');
              }
            } catch (error) {
              console.error('Error during submission:', error);
              alert('Preferences submitted successfully!');
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
  