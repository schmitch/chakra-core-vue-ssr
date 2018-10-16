import VuePlugin from 'rollup-plugin-vue'
import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import nodeResolve from 'rollup-plugin-node-resolve';
import json from 'rollup-plugin-json';

export default {
    input: 'ui/js/entry-server.js',
    plugins: [
        VuePlugin(/* VuePluginOptions */),
        json(),
        resolve({
            include: ['node_modules/**', "node_modules/deep-equal/**"],
        }),
        nodeResolve({
            include: ['node_modules/**', "node_modules/deep-equal/**"],
        }),
        commonjs({
          include: ['node_modules/**', "node_modules/deep-equal/**"],
        }),
    ],
    output: {
        file: 'wwwroot/dist/js/bundle.js',
        format: 'esm'
    }
}
