let timeSwitcherInterval;
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
    const intervalDelay = 30000;

    setMode();
    timeSwitcherInterval = setInterval(setMode, intervalDelay);
});
window.addEventListener('unload', () => {
    try {
        timeSwitcherInterval && clearInterval(timeSwitcherInterval);
    } catch {
    }
});