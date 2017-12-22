const path = require('path');
let UglifyJSPlugin = require('uglifyjs-webpack-plugin'); // плагин минимизации
let ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: './wwwroot/app.js',
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: 'build.js'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.css$/,
                loaders: ['style-loader', 'css-loader', 'resolve-url-loader']
            },
            {
                test: /\.scss$/,
                //loader: ExtractTextPlugin.extract('style-loader', 'css-loader!resolve-url-loader!sass-loader?sourceMap')
                //loaders: ['style-loader', 'css-loader', 'resolve-url-loader', 'sass-loader?sourceMap'],
                use: [
                    {
                        loader: "style-loader"
                    },
                    {
                        loader: "css-loader"
                    },
                    {
                        loader: "resolve-url-loader"
                    },
                    {
                        loader: "sass-loader?sourceMap",
                        options: {
                            includePaths: [path.resolve(__dirname, 'wwwroot/images'), path.resolve(__dirname, 'wwwroot/dist')]
                        }
                    }
                ]
            },
            {
                test: /\.woff2?$|\.ttf$|\.otf$|\.eot$|\.svg$|\.png|\.jpe?g|\.gif$/,
                use: {
                    loader: "url-loader",
                    options: {
                        name: "[name].[hash].[ext]",
                    },
                },
            }
        ]
    },
    plugins: [
        // new UglifyJSPlugin(), // скрипты будут минимизироваться
        new ExtractTextPlugin('styles.css', {
            allChunks: true
        }),
    ]
};