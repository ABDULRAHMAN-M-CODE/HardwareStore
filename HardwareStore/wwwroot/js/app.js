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
let dialog;
window.onload = () => {
    console.log("URL:", window.location.href);
    console.log("Element:", document.getElementById("formFileLg"));
    const formFileLg = document.getElementById("formFileLg");
    formFileLg.addEventListener("change", importFile);
    dialog = document.getElementById("dialog");
    const closeButton = document.getElementById("close");
    closeButton.addEventListener("click", () => __awaiter(void 0, void 0, void 0, function* () {
        dialog.close();
        yield ShowSpecificPageProducts(1);
    }));
};
function importFile() {
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
            dialog.showModal();
            //Show a button that enables the user to show the paginated imported products; attach a call-back to the button. use bootstrap styling.
            //Or use  the close button instead.
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
function ShowSpecificPageProducts(PageNumber) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const response = yield fetch("/Admin/GetProducts", {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ PageNumber: PageNumber }),
            });
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }
            const specificPageProducts = yield response.json();
            let productsGrid = document.getElementById("productsContainer");
            // Source - https://stackoverflow.com/a/65413839
            // Posted by Mason Freed, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-10-23, License - CC BY-SA 4.0
            productsGrid.replaceChildren();
            let currentRow = document.createElement("div");
            currentRow.classList.add("row");
            let priceString;
            specificPageProducts.forEach((product) => __awaiter(this, void 0, void 0, function* () {
                const newColumn = document.createElement("div");
                newColumn.classList.add("col");
                const nameElement = document.createElement("p");
                nameElement.classList.add("text-center");
                var nameText = document.createTextNode(product.name);
                nameElement.appendChild(nameText);
                const descriptionElement = document.createElement("p");
                descriptionElement.classList.add("text-center");
                var descriptionText = document.createTextNode(product.description);
                descriptionElement.appendChild(descriptionText);
                const priceElement = document.createElement("p");
                priceElement.classList.add("text-center");
                priceString = product.price.toString();
                var priceText = document.createTextNode(priceString);
                priceElement.appendChild(priceText);
                newColumn.appendChild(nameElement);
                newColumn.appendChild(descriptionElement);
                newColumn.appendChild(priceElement);
                currentRow.appendChild(newColumn);
                if (product.id % 4 === 0) {
                    productsGrid.appendChild(currentRow);
                    currentRow = document.createElement("div");
                    currentRow.classList.add("row");
                }
            }));
        }
        catch (error) {
            console.error(error.message);
        }
        finally {
        }
    });
}
//# sourceMappingURL=app.js.map