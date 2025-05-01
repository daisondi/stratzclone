// clientapp/src/setupProxy.js
const { legacyCreateProxyMiddleware } = require('http-proxy-middleware');
module.exports = function(app) {
  app.use(
    '/api',
    legacyCreateProxyMiddleware({ target: 'https://localhost:7065', changeOrigin: true, secure: false, logLevel: 'debug' })
  );
};
