"use strict";
// Source - https://stackoverflow.com/a/11550299
// Posted by Jason Hazel
// Retrieved 2026-10-27, License - CC BY-SA 3.0
window.addEventListener("load", (event) => {
    sessionStorage.setItem("hideShowProductsAnchor", "false");
});
const productsPageNavigators = document.querySelectorAll("a.productsPageNavigator");
const showProductsAnchor = document.getElementById("showProductsAnchor");
productsPageNavigators.forEach((navigator) => {
    navigator.addEventListener("click", function (e) {
        sessionStorage.setItem("hideShowProductsAnchor", "true");
    }, false);
});
// Restore the hidden state after the page reloads
if (sessionStorage.getItem("hideShowProductsAnchor") === "true") {
    showProductsAnchor.hidden = true;
}
else {
    showProductsAnchor.hidden = false;
}
//# sourceMappingURL=hideShowProductsButton.js.map