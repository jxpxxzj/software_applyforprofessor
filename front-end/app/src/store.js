import Settings from './settings.js';
export default {
    modules: {
        settings: Settings
    },
    state: {
        count: 0
    },
    mutations: {
        increment (state) {
            state.count++;
        },
        test (state) {
            console.log(state.settings.enableNotification);
        }
    }
};
