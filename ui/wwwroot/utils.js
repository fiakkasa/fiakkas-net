const _mainScrollAnchorQuerySelector = '.main-scroll-anchor';

function scrollMainToTop() {
    try {
        document.querySelector(_mainScrollAnchorQuerySelector).scrollIntoView(true);
    } catch {
    }
}