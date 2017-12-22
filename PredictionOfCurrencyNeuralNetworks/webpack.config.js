const path = require('path');

module.exports = {
  entry: './wwwroot/app.js',
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    filename: 'build.js'
  }
};