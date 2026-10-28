"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
function isThereProductsInDb() {
    return __awaiter(this, void 0, void 0, function* () {
        const url = '/Admin/IsThereProductsInDb';
        try {
            const response = yield fetch(url);
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }
            const isThereProductsInDb = yield response.json();
            return isThereProductsInDb;
        }
        catch (error) {
            console.error(error);
            return false;
        }
    });
}
function Main() {
    return __awaiter(this, void 0, void 0, function* () {
        var _a;
        var thereAreProductsInDb = yield isThereProductsInDb();
        // Source - https://stackoverflow.com/a/50269969
        // Posted by Ted
        // Retrieved 2026-10-26, License - CC BY-SA 4.0
        if (!thereAreProductsInDb) {
            console.log("There are no products in the database.");
            const productsContainer = document.getElementById("products-container");
            productsContainer.innerHTML = '';
            var notification = document.createElement("p");
            notification.innerText = "There are no products in the database yet,please import products in order to show them here.";
            notification.className = "alert alert-info text-center mx-auto col-12 col-md-8 col-lg-6 p-3 p-md-4";
            notification.id = 'noProductsAvailableNotification';
            notification.setAttribute("role", "alert");
            productsContainer.appendChild(notification);
            sessionStorage.setItem("hideShowProductsAnchor", "true");
            document.getElementById("showProductsAnchor").hidden = true;
            (_a = document.getElementById("productsNavigator")) === null || _a === void 0 ? void 0 : _a.remove();
            return;
        }
        document.getElementById("noProductsAvailableNotification").innerText = "click the button to show the products";
        sessionStorage.setItem("hideShowProductsAnchor", "false");
        document.getElementById("showProductsAnchor").hidden = false;
        console.log("There are products in the database!");
    });
}
console.log("`isThereProductsInDb` script was executed");
/**Period checking, otherwise the browser will not be aware of manual products deletion using SSMS*/
/**
 *
 * Note : SSMS is an abbreviation for Sql Server management studio.
 */
let isRunning = false;
setInterval(() => __awaiter(void 0, void 0, void 0, function* () {
    if (isRunning)
        return;
    isRunning = true;
    try {
        yield Main();
    }
    finally {
        isRunning = false;
    }
}), 1000);
//# sourceMappingURL=isThereProductsInDb.js.map