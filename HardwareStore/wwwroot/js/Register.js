"use strict";
window.onload = () => {
    document.querySelector("#registerForm").addEventListener("input", () => {
        var _a;
        (_a = document.querySelector("#registrationErrors")) === null || _a === void 0 ? void 0 : _a.remove();
    }, { once: true });
};
//# sourceMappingURL=Register.js.map