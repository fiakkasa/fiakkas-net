window.scrollMainToTop = () => {
    try {
        document.querySelector('.main-scroll-anchor').scrollIntoView(true);
    } catch {
    }
};