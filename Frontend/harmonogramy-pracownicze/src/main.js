import 'bootstrap/dist/css/bootstrap.min.css'

import { createApp } from 'vue'
import App from './App.vue'
import store from './store'

import { createRouter, createWebHistory } from 'vue-router'
import EmployeePreferencesManager from './components/EmployeePreferencesManager.vue'
import EmployeePreferencesView from './components/EmployeePreferencesView.vue'
import LoginPage from './components/LoginPage.vue'
import MainScreen from './components/MainScreen.vue'
import ManageUsers from './components/ManageUsers.vue'
import PreferencesSubmitPage from './components/PreferencesSubmitPage.vue'
import PreferencesViewPage from './components/PreferencesViewPage.vue'
import RegisterPage from './components/RegisterPage.vue'

const routes = [
    {
        path: '/login',
        name: 'LoginPage',
        component: LoginPage
    },
    {
        path: '/register',
        name: 'RegisterPage',
        component: RegisterPage
    },
    {
        path: '/preferencesSubmit',
        name: 'PreferencesSubmitPage',
        component: PreferencesSubmitPage 
    },
    {
        path: '/preferencesManager',
        name: 'EmployeePreferencesManager',
        component: EmployeePreferencesManager 
    },
    {
        path: '/preferencesView',
        name: 'PreferencesViewPage',
        component: PreferencesViewPage
    },
    {
        path: '/employeePreferencesView',
        name: 'EmployeePreferencesView',
        component: EmployeePreferencesView
    },
    {
        path: '/manageUsers',
        name: 'ManageUsers',
        component: ManageUsers
    },
    {
        path: '/',
        name: 'MainScreen',
        component: MainScreen 
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

const app = createApp(App)
app.use(router);
app.use(store)
app.mount('#app')
