const _darkModePreferenceQuery = window.matchMedia('(prefers-color-scheme: dark)');
let _fullScreenLoaderTimeout;

const setColorModePreference = e => {
    try {
        let mode = !!e?.matches ? 'dark' : 'light';

        if (document.body.getAttribute('data-bs-theme') === mode) {
            return;
        }

        document.body.setAttribute('data-bs-theme', mode);
    } catch {
    }
};

window.autoSetColorModePreference = () => setColorModePreference(_darkModePreferenceQuery);

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
    setColorModePreference(_darkModePreferenceQuery);
    _darkModePreferenceQuery.addEventListener('change', setColorModePreference);
    transitionFullScreenLoader().finally(removeFullScreenLoader);
});
window.addEventListener('unload', () => {
    try {
        _darkModePreferenceQuery?.removeEventListener('change', setColorModePreference);
    } catch {
    }
    try {
        _fullScreenLoaderTimeout && clearTimeout(_fullScreenLoaderTimeout);
    } catch {
    }
});