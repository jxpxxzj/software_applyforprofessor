import Login from './pages/login.vue';
import Home from './pages/home.vue';

export default [
    // create a redirect to login for the default page is /
    {
        path: '/',
        redirect: '/login'
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/home',
        name: 'home',
        component: Home
    }
];
