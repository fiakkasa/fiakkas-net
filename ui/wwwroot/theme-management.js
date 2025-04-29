const _darkModePreferenceQuery = '(prefers-color-scheme: dark)';
const _darkModePreferenceQueryList = window.matchMedia(_darkModePreferenceQuery);
const _themeAttribute = 'data-bs-theme';
const _darkTheme = 'dark';
const _lightTheme = 'light';
const _supportedThemes = {
    dark: _darkTheme,
    light: _lightTheme
};
const _fullScreenLoaderQuerySelector = '.full-screen-loader';
const _fullScreenLoaderTransitionOutClass = 'transition-out';
const _fullScreenLoaderDoneClass = 'done';

const setTheme = (theme) => {
    try {
        const resolvedTheme = _supportedThemes[theme];

        if (!resolvedTheme) {
            return;
        }

        if (document.body.getAttribute(_themeAttribute) === resolvedTheme) {
            return;
        }

        document.body.setAttribute(_themeAttribute, resolvedTheme);
    } catch {
    }
};

const resolveThemePreference = () => {
    try {
        const prefersDarkMode = window.matchMedia(_darkModePreferenceQuery)?.matches === true;

        return prefersDarkMode ? _darkTheme : _lightTheme;
    } catch {
        return _darkTheme;
    }
};

function autoSetTheme() {
    setTheme(resolveThemePreference());
}

function clearFullScreenLoader(delay, duration) {
    const fullScreenLoaderElement = document.querySelector(_fullScreenLoaderQuerySelector);

    delay = ~~delay;
    duration = ~~duration;

    return !fullScreenLoaderElement
        ? new Promise((resolve, _) => resolve(true))
        : new Promise((resolve, _) => {
            fullScreenLoaderElement.style.transitionDelay = `${delay}ms`;
            fullScreenLoaderElement.style.transitionDuration = `${duration}ms`;
            fullScreenLoaderElement.classList.add(_fullScreenLoaderTransitionOutClass);
            setTimeout(() => resolve(true), delay + duration);
        }).finally(() => {
            fullScreenLoaderElement.classList.add(_fullScreenLoaderDoneClass);
            fullScreenLoaderElement.classList.remove(_fullScreenLoaderTransitionOutClass);
            fullScreenLoaderElement.style.transitionDelay = null;
            fullScreenLoaderElement.style.transitionDuration = null;
        });
}

_darkModePreferenceQueryList.addEventListener('change', autoSetTheme);
window.addEventListener('unload', () =>
    _darkModePreferenceQueryList?.removeEventListener('change', autoSetTheme)
);