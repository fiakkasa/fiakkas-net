const path = require('path');
const { execFile } = require('child_process');
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const ip = require('ip');
const { checkPort } = require('get-port-please');

const proxyAppPort = process.env.PORT || 3001;
const proxyAppEnableWebsocket = true;

const dotnetAppName = 'ui';
const dotnetAppExecutableName = 'ui';
const dotnetAppProtocol = process.env.DOTNET_PROTOCOL || 'http';
const dotnetAppHost = ip.address();
const dotnetAppPort = process.env.DOTNET_PORT || 5002;
const dotnetAppUrl = `${dotnetAppProtocol}://${dotnetAppHost}:${dotnetAppPort}`;
const dotnetAppFolderPath = process.env.DOTNET_ASSETS_PATH || './publish';
const dotnetAppContentRootPath = path.resolve(__dirname, dotnetAppFolderPath);
const dotnetAppExecutablePath = path.resolve(
    __dirname,
    `${dotnetAppFolderPath}/${process.env.DOTNET_EXECUTABLE_NAME || dotnetAppExecutableName}`
);
const dotnetAppProxyUrl = '/ui';
const dotnetAppProxyRedirectUrl = '/ui/';
const dotnetAppProxyPathRewriteConfig = { '^/ui': '/' };

let dotnetAppProcess = null;

const startDotnetApp = () => new Promise((resolve, _) => {
    let dotnetAppProcessStarted = false;

    console.log(`Starting app '${dotnetAppName}'...`);

    try {
        dotnetAppProcess = execFile(
            dotnetAppExecutablePath,
            ['--urls', dotnetAppUrl, '--contentroot', dotnetAppContentRootPath], //
            (dotnetAppProcessError, _, __) => {
                if (!dotnetAppProcessError) return;

                throw dotnetAppProcessError;
            }
        );
        dotnetAppProcess.on('spawn', () =>
            console.log(`Process for app '${dotnetAppName}' spawned...`)
        );
        dotnetAppProcess.on('close', (dotnetAppProcessCode) =>
            console.log(`Process for app '${dotnetAppName}' exited with code: ${dotnetAppProcessCode}`)
        );
        dotnetAppProcess.stdout.on('data', (dotnetAppProcessStdout) => {
            console.log(dotnetAppProcessStdout);

            if (dotnetAppProcessStarted || !(dotnetAppProcessStdout + '').includes('Now listening on:')) {
                return;
            }

            dotnetAppProcessStarted = true;
            resolve(true);
        });
        dotnetAppProcess.stderr.on('data', (dotnetAppProcessError) =>
            console.error(dotnetAppProcessError)
        );
    } catch (error) {
        console.error(`Error starting process for app '${dotnetAppName}' with message: ${error?.message}`);

        return resolve(false);
    }
});
const stopDotnetApp = () => {
    if (!dotnetAppProcess) {
        console.log(`The process for app '${dotnetAppName}' is not initialized...`);
        return;
    }

    console.log(`Sending termination signal to process of app '${dotnetAppName}'...`);

    try {
        dotnetAppProcess.kill();
        console.log(`Process of app '${dotnetAppName}' received termination signal successfully`);
    } catch (error) {
        console.error(
            `An error occurred while sending termination signal to process of app '${dotnetAppName}' with message: ${error?.message}`
        );
    }
};

(async () => {
    const proxyApp = express();

    const isProxyAppRequestedPortAvailable = !!(await checkPort(proxyAppPort));

    if (!isProxyAppRequestedPortAvailable) {
        console.error(`Port ${proxyAppPort} requested by proxy app for app '${dotnetAppName}' is already in use...`);
        return;
    }

    const isDotnetAppRequestedPortAvailable = !!(await checkPort(dotnetAppPort, dotnetAppHost));
    if (!isDotnetAppRequestedPortAvailable) {
        console.error(`Port ${dotnetAppPort} requested by app '${dotnetAppName}' is already in use...`);
        return;
    }

    if (!(await startDotnetApp())) {
        console.error(`Error starting app '${dotnetAppName}'`);
        return;
    }

    console.log(`App '${dotnetAppName}' is ready!`);

    const proxyMiddleware = createProxyMiddleware({
        target: dotnetAppUrl,
        changeOrigin: true,
        ws: proxyAppEnableWebsocket,
        logger: console,
        pathRewrite: dotnetAppProxyPathRewriteConfig,
    });

    proxyApp.set('strict routing', true);
    proxyApp.get(dotnetAppProxyUrl, (_, response) => response.redirect(301, dotnetAppProxyRedirectUrl));
    proxyApp.use(proxyMiddleware);

    console.log(`Starting the proxy app for app '${dotnetAppName}'...`);

    proxyApp.listen(proxyAppPort, (proxyAppError) => {
        if (proxyAppError) {
            console.error(`Error starting proxy app on port: ${proxyAppPort} with message: ${proxyAppError?.message}`);

            stopDotnetApp();

            return;
        }

        console.log(`Proxy app started on port: ${proxyAppPort}`);
    });
})();
