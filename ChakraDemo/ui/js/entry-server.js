import {createApp} from './app';

import renderVueComponentToString from 'vue-server-renderer/basic';

const isDev = process.env.NODE_ENV !== 'production';

export var closure = context => {
    return new Promise((resolve, reject) => {
        try {

            const {app, store} = createApp();
            
            store.replaceState(context);

            renderVueComponentToString(app, (err, res) => {
                if (err !== null) {
                    reject(err);
                } else {
                    resolve(res);
                }
            });
        } catch (err) {
            reject(err);
        }
    });
};