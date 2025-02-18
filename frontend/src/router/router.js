import {createRouter, createWebHistory} from 'vue-router';
import Startseite from '../views/Startseite.vue';
import Verwaltung from '../views/Verwaltung.vue';
import Ersterfassung from '../views/Ersterfassung.vue';
import Falluebersicht from '../views/Falluebersicht.vue';
import Bearbeitungssicht from '../views/Bearbeitungssicht.vue';
import StyleGuide from '../views/StyleGuide.vue';
import Anmelden from '../views/Anmelden.vue';
import Abmelden from '../views/Abmelden.vue';
import Konfiguration from "@/views/Weiserzeichenverwaltung.vue";
import Meldungsimport from "@/views/Meldungsimport.vue";

const routes = [
    {path: '/', component: Startseite},
    {path: '/ordnungswidrigkeiten-verwalten', component: Verwaltung},
    {path: '/ersterfassung', component: Ersterfassung},
    {path: '/falluebersicht', component: Falluebersicht},
    {path: '/styleguide', component: StyleGuide},
    {path: '/weiserzeichen-konfiguration', component: Konfiguration},
    {path: '/meldungen-importieren', component: Meldungsimport},
    {path: '/anmelden', component: Anmelden},
    {path: '/abmelden', component: Abmelden},
    {
        path: '/bearbeiten/:id',
        component: Bearbeitungssicht,
        props: route => {
            const data = route.query.data ? JSON.parse(route.query.data) : {};
            return {id: route.params.id, data};
        }
    },
    {path: '/:notFound(.*)', redirect: '/'},
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// Global navigation guard
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('token');

    // Check if the user is logged in
    if (!token && to.path !== '/anmelden') {
        // Redirect to the login page if the user is not logged in
        next('/anmelden');
    } else if (token && to.path === '/anmelden') {
        // If the user is already logged in, redirect from login page to the start page
        next('/');
    } else {
        // Proceed to the intended route
        next();
    }
});

export default router;
