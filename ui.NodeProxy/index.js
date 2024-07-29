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

  const dotnetApiFolderPath = process.env.DOTNET_UI_FOLDER_PATH || './publish';
  const contentRootPath = path.resolve(__dirname, dotnetApiFolderPath);
  const uiExecutablePath = path.resolve(__dirname, `${dotnetApiFolderPath}/${process.env.DOTNET_UI_EXECUTABLE_NAME || 'ui'}`);

  const dotnetPortIsAvailable = !!(await checkPort(dotnetPort, dotnetHost));

  let startDotnet = new Promise((resolve, _) => {
    if (!dotnetPortIsAvailable) {
      console.log('The port requested by the .NET UI is already in use');
      resolve(true);
      return;
    }

    console.log('Starting the .NET UI..');
    let dotnetInit = false;

    const uiProcess = execFile(
      uiExecutablePath,
      ['--urls', dotnetUrl, '--contentroot', contentRootPath], //
      (err, stdout, stderr) => {
        if (err) {
          console.error(`Error starting .NET UI with message: ${err?.message}`);
          return;
        }

        console.log(`.NET UI started: ${stdout}`);
      }
    );

    uiProcess.on('close', (code) => {
      console.log(`.NET UI exited with code: ${code}`);

      if (dotnetInit) return;

      resolve(false);
      dotnetInit = true;
    });
    uiProcess.stdout.on('data', (text) => {
      console.log(text);

      if (dotnetInit || !(text + '').includes('Application started.')) return;

      resolve(true);
      dotnetInit = true;
    });
    uiProcess.stderr.on('data', (text) => {
      console.error(text);

      if (dotnetInit) return;

      resolve(false);
      dotnetInit = true;
    });
  });

  if (!(await startDotnet)) {
    console.error('Error starting the .NET UI');
    return;
  }

  console.log('The .NET UI is ready');

  const uiProxy = createProxyMiddleware({
    target: dotnetUrl,
    changeOrigin: true,
    ws: true,
    logger: console,
    pathRewrite: {
      '^/ui': '/'
    },
  });

  app.set('strict routing', true);
  app.get('/ui', (req, res) => res.redirect(301, '/ui/'));
  app.use(uiProxy);

  console.log('Starting the Proxy server..');

  app.listen(port, (err) => {
    if (err) {
      console.error(`Error starting proxy server on port: ${port} with message: ${err}`);
      return;
    }

    console.log(`Proxy server started on port: ${port}`);
  });
})();
