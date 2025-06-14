// version: 1.4.0
const path = require('path');
const { execFile } = require('child_process');
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const ip = require('ip');
const { checkPort } = require('get-port-please');

const proxyAppPort = process.env.PORT || 3000;
const proxyAppEnableWebsocket = false;

const dotnetAppName = 'api';
const dotnetAppExecutableName = 'api';
const dotnetAppProtocol = process.env.DOTNET_PROTOCOL || 'http';
const dotnetAppHost = ip.address();
const dotnetAppPort = process.env.DOTNET_PORT || 5001;
const dotnetAppUrl = `${dotnetAppProtocol}://${dotnetAppHost}:${dotnetAppPort}`;
const dotnetAppFolderPath = process.env.DOTNET_ASSETS_PATH || './publish';
const dotnetAppContentRootPath = path.resolve(__dirname, dotnetAppFolderPath);
const dotnetAppExecutablePath = path.resolve(
    __dirname,
    `${dotnetAppFolderPath}/${process.env.DOTNET_EXECUTABLE_NAME || dotnetAppExecutableName}`
);
const dotnetAppProxyUrl = '/api';
const dotnetAppProxyRedirectUrl = '/api/';
const dotnetAppProxyPathRewriteConfig = { '^/api': '/' };

let dotnetAppProcess = null;

const logTimeStamp = () => new Date().toJSON();
const log = (message) => console.log(`[${logTimeStamp()}] ${message}`);
const logError = (message) => console.logError(`[${logTimeStamp()} ERR] ${message}`);
const startDotnetApp = () => new Promise((resolve, _) => {
    let dotnetAppProcessStarted = false;

    log(`Starting app '${dotnetAppName}' on port ${dotnetAppPort}...`);

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
            log(`Process for app '${dotnetAppName}' spawned...`)
        );
        dotnetAppProcess.on('close', (dotnetAppProcessCode) =>
            log(`Process for app '${dotnetAppName}' exited with code: ${dotnetAppProcessCode}`)
        );
        dotnetAppProcess.stdout.on('data', (dotnetAppProcessStdout) => {
            log(dotnetAppProcessStdout);

            if (dotnetAppProcessStarted || !(dotnetAppProcessStdout + '').includes('Now listening on:')) {
                return;
            }

            dotnetAppProcessStarted = true;
            resolve(true);
        });
        dotnetAppProcess.stderr.on('data', (dotnetAppProcessError) =>
            logError(dotnetAppProcessError)
        );
    } catch (error) {
        logError(`Error starting process for app '${dotnetAppName}' on port ${dotnetAppPort} with message: ${error?.message}`);

        return resolve(false);
    }
});
const stopDotnetApp = () => {
    if (!dotnetAppProcess) {
        log(`The process for app '${dotnetAppName}' is not initialized...`);
        return;
    }

    log(`Sending termination signal to process of app '${dotnetAppName}'...`);

    try {
        dotnetAppProcess.kill();
        log(`Process of app '${dotnetAppName}' received termination signal successfully`);
    } catch (error) {
        logError(
            `An error occurred while sending termination signal to process of app '${dotnetAppName}' with message: ${error?.message}`
        );
    }
};

(async () => {
    const proxyApp = express();

    const isProxyAppRequestedPortAvailable = !!(await checkPort(proxyAppPort));

    if (!isProxyAppRequestedPortAvailable) {
        logError(`Port ${proxyAppPort} requested by proxy app is already in use...`);
        return;
    }

    const isDotnetAppRequestedPortAvailable = !!(await checkPort(dotnetAppPort, dotnetAppHost));

    if (!isDotnetAppRequestedPortAvailable) {
        log(
            `Port ${dotnetAppPort} requested by app '${dotnetAppName}' is already in use, this might indicate that app '${dotnetAppName}' is already started...`
        );
    } else {
        const initDotnetApp = await startDotnetApp();

        if (!initDotnetApp) {
            logError(`Error starting app '${dotnetAppName}' on port ${dotnetAppPort}`);
        } else {
            log(`App '${dotnetAppName}' on port ${dotnetAppPort} is ready!`);
        }
    }

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

    log(`Starting the proxy app for app '${dotnetAppName}'...`);

    proxyApp.listen(proxyAppPort, (proxyAppError) => {
        if (proxyAppError) {
            logError(`Error starting proxy app on port: ${proxyAppPort} with message: ${proxyAppError?.message}`);

            stopDotnetApp();

            return;
        }

        log(`Proxy app started on port: ${proxyAppPort}`);
    });
})();
