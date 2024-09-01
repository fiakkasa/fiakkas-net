let _darkModePreferenceQuery;
let _fullScreenLoaderTimeout;

const setColorModePreference = (isDark) => {
    try {
        let mode = isDark ? 'dark' : 'light';
        document.body.setAttribute('data-bs-theme', mode);
    } catch {
    }
};

window.addEventListener('load', () => {
    _darkModePreferenceQuery = window.matchMedia('(prefers-color-scheme: dark)');
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
    setColorModePreference(_darkModePreferenceQuery.matches);
    _darkModePreferenceQuery.addEventListener("change", e => setColorModePreference(e.matches));
    transitionFullScreenLoader().finally(removeFullScreenLoader);
});
window.addEventListener('unload', () => {
    try {
        _darkModePreferenceQuery && _darkModePreferenceQuery.removeEventListener('change', e => setColorModePreference(e.matches));
    } catch {
    }
    try {
        _fullScreenLoaderTimeout && clearTimeout(_fullScreenLoaderTimeout);
    } catch {
    }
});