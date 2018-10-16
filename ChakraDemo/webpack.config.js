'use strict';

const webpack = require('webpack');
const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");

module.exports = function (env) {
    console.log('Env:', env);
    const isProduction = env ? env['prod'] === true : false;

    return {
        context: path.join(__dirname, "./ui"),
        entry: {
            'js/client': ['./js/entry-client.js'],
        },
        mode: isProduction ? 'production' : 'development',
        optimization: {
            minimize: isProduction
        },
        output: {
            path: path.join(__dirname, './wwwroot/dist'),
            publicPath: '/dist/',
            filename: isProduction ? '[name].min.js' : '[name].js'
        },
        module: {
            rules: [
                {
                    test: /\.vue$/,
                    loader: 'vue-loader'
                },
            ],
            
        },
        plugins: [
            // make sure to include the plugin!
            new VueLoaderPlugin()
        ]
    }
};