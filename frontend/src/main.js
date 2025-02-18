import { createApp } from 'vue';
import App from './App.vue';
import router from './router/router';

const app = createApp(App);

/*
fetch('http://localhost:5000/api/authentication/sso-login', {
        method: 'GET',
        credentials: 'include', // sends cookies and windows login information
        mode: 'cors',
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('SSO-Anmeldung fehlgeschlagen');
        }
        console.log(response.json());
        return response.json();
    })
    .then(data => {
        console.log("data: " + data);
        localStorage.setItem('token', data.token);
    })
    .catch(error => {
        console.error('Fehler beim SSO-Login: ', error);
        alert('Anmeldung fehlgeschlagen. Bitte überprüfen Sie Ihre Berechtigungen.');
    });
*/

app.use(router).mount('#app');