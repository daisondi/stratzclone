const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',                      // only proxy /api/*
    createProxyMiddleware({
      target: 'http://localhost:5179',  // <-- match your HTTP endpoint
      changeOrigin: true,
    })
  );
};
