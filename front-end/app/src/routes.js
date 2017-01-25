import Index from './pages/index.vue';
import NotFound from './pages/notFound.vue';
import Page from './pages/page.vue';
import Login from './pages/login.vue';
import Home from './pages/home.vue';

export default [
    {
        path: '/',
        name: 'index',
        component: Index
    },
    {
        path: '/page',
        name: 'page',
        component: Page
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
    },
    {
        path: '*',
        name: 'notFound',
        component: NotFound
    }
];
