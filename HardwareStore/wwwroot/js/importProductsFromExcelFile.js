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
document.addEventListener("DOMContentLoaded", () => {
    const formFileLg = document.getElementById("formFileLg");
    formFileLg.addEventListener("change", importProductsFromExcelFile);
});
//W3schools uses `camelCase` for identifier names (variables and functions).
function importProductsFromExcelFile() {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("importFil executed");
        const controller = new AbortController();
        let cancelContainer = document.getElementById("cancelContainer");
        let cancelButton = document.createElement('BUTTON');
        cancelButton.appendChild(document.createTextNode("Cancel"));
        cancelButton.onclick = function () {
            console.log("abort executed");
            controller.abort();
            cancelButton.remove();
        };
        cancelContainer.appendChild(cancelButton);
        const formData = new FormData();
        formData.append("file", this.files[0]);
        try {
            console.log("Try block executed.");
            const response = yield fetch('/Admin/Import', {
                method: 'POST',
                body: formData,
                signal: controller.signal
            });
            if (!response.ok) {
                console.log("response is not ok");
                throw new Error(`Response status: ${response.status}`);
            }
        }
        catch (error) {
            if (error.name == "AbortError") {
                console.log("catch block executed: AbortError");
            }
            console.error(error.message);
        }
        finally {
            cancelButton.remove();
        }
    });
}
//# sourceMappingURL=importProductsFromExcelFile.js.map