const path = require('path');

module.exports = {
  configureWebpack: {
    resolve: {
      alias: {
        '@': path.resolve(__dirname, 'src/')
      }
    }
  },
  devServer: {
    proxy: {
      '/api': {
        target: 'http://webengineering.ins.hs-anhalt.de:11032',
        changeOrigin: true
      }
    }
  }
}