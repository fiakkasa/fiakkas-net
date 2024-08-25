let themeModeSwitcherInterval;
let fullScreenLoaderTimeout;
window.addEventListener('load', () => {
    const isHidden = elem => {
        const styles = window.getComputedStyle(elem);
        return styles.display === 'none' || styles.visibility === 'hidden';
    };
    const pivotElement = document.querySelector('.prefers-light');
    const setMode = () => {
        try {
            let mode = isHidden(pivotElement) ? 'dark' : 'light';
            document.body.setAttribute('data-bs-theme', mode);
        } catch {
        }
    };
    const transitionFullScreenLoader = () =>
        new Promise((resolve, _) => {
            const delay = 600;

            try {
                const fullScreenLoaderElement = document.querySelector('.full-screen-loader');
                fullScreenLoaderElement.style.transitionDuration = `${delay}ms`;
                fullScreenLoaderElement.classList.add('done');
            } catch {
            }

            fullScreenLoaderTimeout = setTimeout(() => resolve(true), delay - 1);
        });
    const removeFullScreenLoader = async () => {
        try {
            document.querySelector('.full-screen-loader').remove();
        } catch {
        }
    }
    const intervalDelay = 30000;

    setMode();
    themeModeSwitcherInterval = setInterval(setMode, intervalDelay);
    transitionFullScreenLoader().finally(removeFullScreenLoader);
});
window.addEventListener('unload', () => {
    try {
        themeModeSwitcherInterval && clearInterval(themeModeSwitcherInterval);
    } catch {
    }
    try {
        fullScreenLoaderTimeout && clearTimeout(fullScreenLoaderTimeout);
    } catch {
    }
});