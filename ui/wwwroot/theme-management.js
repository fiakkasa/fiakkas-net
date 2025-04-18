const _darkModePreferenceQuery = '(prefers-color-scheme: dark)';
const _darkModePreferenceQueryList = window.matchMedia(_darkModePreferenceQuery);
let _fullScreenLoaderTimeout;
const _supportedThemes = {
    dark: 'dark',
    light: 'light'
};

const prefersDarkMode = () => window.matchMedia(_darkModePreferenceQuery)?.matches === true;

const setTheme = (theme) => {
    try {
        const resolvedTheme = _supportedThemes[theme];

        if (!resolvedTheme) {
            return;
        }

        if (document.body.getAttribute('data-bs-theme') === resolvedTheme) {
            return;
        }

        document.body.setAttribute('data-bs-theme', resolvedTheme);
    } catch {
    }
};

const resolveThemePreference = () => {
    const darkMode = 'dark';
    const lightMode = 'light';

    try {
        return prefersDarkMode() ? darkMode : lightMode;
    } catch {
        return darkMode;
    }
};

window.autoSetTheme = () => setTheme(resolveThemePreference());

window.addEventListener('load', () => {
    const transitionFullScreenLoader = () =>
        new Promise((resolve, _) => {
            const delay = 1000;
            try {
                const fullScreenLoaderElement = document.querySelector('.full-screen-loader');
                fullScreenLoaderElement.style.transitionDuration = `${delay}ms`;
                fullScreenLoaderElement.style.transitionDelay = `${~~Math.round(delay * .334)}ms`;
                fullScreenLoaderElement.classList.add('done');
            } catch {
            }

            _fullScreenLoaderTimeout = setTimeout(() => resolve(true), delay - 1);
        });
    const removeFullScreenLoader = async () => {
        try {
            document.querySelector('.full-screen-loader').remove();
        } catch {
        }
    }
    setTheme(resolveThemePreference());
    _darkModePreferenceQueryList.addEventListener('change', window.autoSetTheme);
    transitionFullScreenLoader().finally(removeFullScreenLoader);
});
window.addEventListener('unload', () => {
    try {
        _darkModePreferenceQueryList?.removeEventListener('change', window.autoSetTheme);
    } catch {
    }
    try {
        _fullScreenLoaderTimeout && clearTimeout(_fullScreenLoaderTimeout);
    } catch {
    }
});