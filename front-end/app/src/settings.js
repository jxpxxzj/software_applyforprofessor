export default {
    state: {
        enableNotification: false, // show system notification when some actions are completed
        downloadPath: '', // where file will be put
        $storage: null,
        $electron: null
    },
    mutations: {
        // save these into state so that Vuex.Settings could access them
        initStore (state, value) {
            state.$storage = value.storage;
            state.$electron = value.electron;
        },
        initSettings (state) {
            state.$storage.has('userSettings', (error, hasKey) => {
                if (error) { // use default and fallback settings
                    state.enableNotification = false;
                    state.downloadPath = state.$electron.remote.app.getPath('downloads');
                }
                if (hasKey) {
                    state.$storage.get('userSettings', (error, data) => {
                        if (error) {
                            console.error(error);
                        }
                        state.enableNotification = data.enableNotification;
                        state.downloadPath = data.downloadPath;
                    });
                }
            });
        },
        // jsons are in %Appdata%\Roaming\Webdisk\storage
        saveSettings (state) {
            state.$storage.set('userSettings', {
                enableNotification: state.enableNotification,
                downloadPath: state.downloadPath
            }, (error) => {
                if (error) {
                    console.error(error);
                };
            });
        },
        setEnableNotification (state, value) {
            state.enableNotification = value;
        },
        setDownloadPath (state, value) {
            state.downloadPath = value;
        }
    }
};
