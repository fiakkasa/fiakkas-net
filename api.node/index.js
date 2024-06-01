const path = require('path');
const { execFile } = require('child_process');
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const ip = require('ip');
const { checkPort } = require('get-port-please');

(async () => {
  const app = express();

  const port = process.env.PORT || 3000;

  const dotnetProtocol = process.env.DOTNET_PROTOCOL || 'http';
  const dotnetHost = ip.address();
  const dotnetPort = process.env.DOTNET_PORT || 5000;
  const dotnetUrl = `${dotnetProtocol}://${dotnetHost}:${dotnetPort}`;

  const dotnetApiFolderPath = process.env.DOTNET_API_FOLDER_PATH || './publish';
  const contentRootPath = path.resolve(__dirname, dotnetApiFolderPath);
  const apiExecutablePath = path.resolve(__dirname, `${dotnetApiFolderPath}/${process.env.DOTNET_API_EXECUTABLE_NAME || 'api'}`);

  const dotnetPortIsAvailable = !!(await checkPort(dotnetPort, dotnetHost));

  let startDotnet = new Promise((resolve, _) => {
    if (!dotnetPortIsAvailable) {
      console.log('The port requested by the .NET API is already in use');
      resolve(true);
      return;
    }

    console.log('Starting the .NET API..');
    let dotnetInit = false;

    const apiProcess = execFile(
      apiExecutablePath,
      ['--urls', dotnetUrl, '--contentroot', contentRootPath], //
      (err, stdout, stderr) => {
        if (err) {
          console.error(`Error starting .NET API with message: ${err?.message}`);
          return;
        }

        console.log(`.NET API started: ${stdout}`);
      }
    );

    apiProcess.on('close', (code) => {
      console.log(`.NET API exited with code: ${code}`);

      if (dotnetInit) return;

      resolve(false);
      dotnetInit = true;
    });
    apiProcess.stdout.on('data', (text) => {
      console.log(text);

      if (dotnetInit || !(text + '').includes('Application started.')) return;

      resolve(true);
      dotnetInit = true;
    });
    apiProcess.stderr.on('data', (text) => {
      console.error(text);

      if (dotnetInit) return;

      resolve(false);
      dotnetInit = true;
    });
  });

  if (!(await startDotnet)) {
    console.error('Error starting the .NET API');
    return;
  }

  console.log('The .NET API is ready');

  const apiProxy = createProxyMiddleware({
    target: dotnetUrl,
    changeOrigin: true,
    logger: console,
    pathRewrite: {
      '^/api': '/'
    },
  });

  app.set('strict routing', true);
  app.get('/api', (req, res) => res.redirect(301, '/api/'));
  app.use(apiProxy);

  console.log('Starting the Proxy server..');

  app.listen(port, (err, stdout, stderr) => {
    if (err) {
      console.error(`Error starting proxy server on port: ${port} with message: ${err}`);
      return;
    }

    console.log(`Proxy server started on port: ${port}`);
  });
})();
