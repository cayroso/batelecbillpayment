'use strict';

const path = require('path');
const webpack = require('webpack');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const { VueLoaderPlugin } = require('vue-loader');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = (env = {}) => ({
    mode: env.prod ? "production" : "development",    
    resolve: {
        alias: {
            //'vue': '@vue/runtime-dom'
            'vue': 'vue/dist/vue.esm-bundler.js',
            //'vue$': 'vue/dist/vue.esm.js',
            //'bootstrap-vue$': 'bootstrap-vue/src/index.js'
        }
    },
    entry: {
        'administrator': './ClientApp/Administrator/main.js',
        'consumer': './ClientApp/Consumer/main.js',
        //'member': './ClientApp/Member/main.js',
        //'customer': './ClientApp/Customer/main.js',
    },
    output: {
        filename: '[name]-[contenthash].js',
        //chunkFilename: 'chunk-[name].[contenthash]-bundle.js',
        chunkFilename: 'x-[contenthash]-bundle.js',
        path: path.resolve(__dirname, 'wwwroot/app'),
        publicPath: '/app/'
    },

    mode: process.env.NODE_ENV,

    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules\/(?!bootstrap-vue\/src\/)/,
                use: {
                    loader: 'babel-loader',
                }
            },
            {
                test: /\.vue$/,
                use: 'vue-loader'
            },
            {
                test: /\.(html)$/,
                use: ['file-loader?name=[name]-[hash].[ext]', 'extract-loader', 'html-loader']
            },

            {
                test: /\.css$/,
                use: [
                    //{ loader: 'style-loader' },
                    
                    {
                        loader: MiniCssExtractPlugin.loader,
                        options: { esModule: !env.prod }
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            import: true
                        }
                    },
                ]
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                use: ['file-loader']
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                loader: 'url-loader', //?limit=30000&name=[name]-[hash].[ext]',
                options: {
                    publicPath: './',
                    limit: 30000,
                    name: '[name]-[hash].[ext]'

                }
            },

        ]
    },
    plugins: [
        // Clean dist folder.
        new CleanWebpackPlugin({
            "verbose": true // Write logs to console.
        }),
        new VueLoaderPlugin(),
        new MiniCssExtractPlugin({
            filename: '[name]-[contenthash].css',
            //chunkFilename: '[name].[contenthash]-bundle.css'
            chunkFilename: 'x-[contenthash]-bundle.css'
        }),
        //new webpack.ProvidePlugin({
        //    $: "jquery",
        //    jQuery: "jquery"
        //}),
        // Ignore all locale files of moment.js
        //new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new webpack.WatchIgnorePlugin({ paths: [/^\.\/locale$/, /moment$/] }),
        new BundleAnalyzerPlugin(),
        // avoid publishing when compilation failed.
        new webpack.NoEmitOnErrorsPlugin(),
        new HtmlWebpackPlugin({
            inject: false,
            scriptLoading: 'defer',
            chunks: ['administrator'],
            filename: path.resolve(__dirname, "Areas/Administrator/Pages/_Shared/_Layout.cshtml"),
            template: path.resolve(__dirname, "Areas/Administrator/Pages/_Shared/_Layout_Template.cshtml")
        }),
        new HtmlWebpackPlugin({
            inject: false,
            scriptLoading: 'defer',
            chunks: ['consumer'],
            filename: path.resolve(__dirname, "Areas/Consumer/Pages/_Shared/_Layout.cshtml"),
            template: path.resolve(__dirname, "Areas/Consumer/Pages/_Shared/_Layout_Template.cshtml")
        }),
        //new HtmlWebpackPlugin({
        //    inject: false,
        //    scriptLoading: 'defer',
        //    chunks: ['member'],
        //    filename: path.resolve(__dirname, "Areas/Member/Pages/_Shared/_Layout.cshtml"),
        //    template: path.resolve(__dirname, "Areas/Member/Pages/_Shared/_Layout_Template.cshtml")
        //}),
        //new HtmlWebpackPlugin({
        //    inject: false,
        //    scriptLoading: 'defer',
        //    chunks: ['admin'],
        //    filename: path.resolve(__dirname, "Areas/Admin/Pages/_Shared/_Layout.cshtml"),
        //    template: path.resolve(__dirname, "Areas/Admin/Pages/_Shared/_Layout_Template.cshtml")
        //}),
        //new HtmlWebpackPlugin({
        //    inject: false,
        //    scriptLoading: 'defer',
        //    chunks: ['member'],
        //    filename: path.resolve(__dirname, "Areas/Member/Pages/_Shared/_Layout.cshtml"),
        //    template: path.resolve(__dirname, "Areas/Member/Pages/_Shared/_Layout_Template.cshtml")
        //}),
        //new HtmlWebpackPlugin({
        //    inject: false,
        //    scriptLoading: 'defer',
        //    chunks: ['organization'],
        //    filename: path.resolve(__dirname, "Areas/Organization/Pages/_Shared/_Layout.cshtml"),
        //    template: path.resolve(__dirname, "Areas/Organization/Pages/_Shared/_Layout_Template.cshtml")
        //}),
    ],

    optimization: {
        minimize: process.env.NODE_ENV === 'development' ? false : true,
        moduleIds: 'deterministic',
        //runtimeChunk: 'single',
        minimizer: [
            new OptimizeCSSAssetsPlugin({}),
            new TerserPlugin(),
        ],
        splitChunks: {
            chunks: 'all',
            maxInitialRequests: Infinity,
            minSize: 0,
            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    //name(module) {
                    //    // get the name. E.g. node_modules/packageName/not/this/part.js
                    //    // or node_modules/packageName
                    //    const packageName = module.context.match(/[\\/]node_modules[\\/](.*?)([\\/]|$)/)[1];

                    //    // npm package names are URL-safe, but some servers don't like @ symbols
                    //    return `vendor.${packageName.replace('@', '')}`;
                    //},
                    //name(module, chunks, cacheGroupKey) {
                    //    const moduleFileName = module
                    //        .identifier()
                    //        .split('/')
                    //        .reduceRight((item) => item);
                    //    const allChunksNames = chunks.map((item) => item.name).join('~');
                    //    return `vendor-${allChunksNames}-${moduleFileName}`;
                    //},
                },
            },
        }
    }
});