const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = app => {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'https://localhost:7065',
      secure: false,         // accept self-signed cert
      changeOrigin: true,
      logLevel: 'debug'
    })
  );
};
