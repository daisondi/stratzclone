// clientapp/src/setupProxy.js
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
  app.use(
    '/api',                                      // proxy only API calls
    createProxyMiddleware({
      target: 'https://localhost:7065',          // ASP.NET Core dev server
      secure: false,                             // accept self-signed TLS cert
      changeOrigin: true,                        // Host header = target
      cookieDomainRewrite: 'localhost',          // let auth cookie stick
      logLevel: 'debug'                          // verbose proxy logs
    })
  );
};
