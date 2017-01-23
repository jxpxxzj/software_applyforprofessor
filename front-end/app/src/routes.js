import Index from './pages/index.vue';
import NotFound from './pages/notFound.vue';
import Page from './pages/page.vue';
import Login from './pages/login.vue';

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
        path: '*',
        name: 'notFound',
        component: NotFound
    }
];
