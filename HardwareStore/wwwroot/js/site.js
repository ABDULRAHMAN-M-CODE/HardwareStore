console.log("Script Executed")
document.getElementById("formFileLg").addEventListener("change", importFile);



async function importFile() {
    console.log("importFil executed")
    const controller = new AbortController();

    let cancelContainer = document.getElementById("cancelContainer");
    let cancelButton = document.createElement('BUTTON');
    cancelButton.appendChild(document.createTextNode("Cancel"));
    cancelButton.onclick = function () {
        console.log("abort executed")
        controller.abort()
        cancelButton.remove();
    }
    cancelContainer.appendChild(cancelButton);
    const formData = new FormData();
    formData.append("file", this.files[0]);


    try {
        console.log("Try block executed.")
        response = await fetch('/Admin/Import', {
            method: 'POST',
            body: formData,
            signal: controller.signal

        })
        if (!response.ok) {
            console.log("response is not ok")
            throw new Error(`Response status: ${response.status}`);
            window.location.href = "/Admin/ImportError";
            return;
        }

       

        try {
            const response = await fetch("/Admin/GetProducts", {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ PageNumber: 3}),
            });

            if (!response.ok) {
                console.log(`Line 50 :${response.message} `);
                throw new Error(`Response status: ${response.status}`);
                
            }


            const specificPageProducts = await response.json();

            let productsGrid = document.getElementById("productsContainer");

            // Source - https://stackoverflow.com/a/65413839
            // Posted by Mason Freed, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-10-23, License - CC BY-SA 4.0
            productsGrid.replaceChildren();

            let currentRow = document.createElement("div");
            currentRow.classList.add("row");
            specificPageProducts.forEach(async (product) => {




                const newColumn = document.createElement("div");
                newColumn.classList.add("col");

                const nameElement = document.createElement("p");
                nameElement.classList.add("text-center");
                var nameText = document.createTextNode(product.name);
                nameElement.appendChild(nameText);

                const descriptionElement = document.createElement("p");
                descriptionElement.classList.add("text-center");
                var descriptionText = document.createTextNode(product.description);
                descriptionElement.appendChild(descriptionText)

                const priceElement = document.createElement("p");
                priceElement.classList.add("text-center");
                var priceText = document.createTextNode(product.price);
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


            });

        }catch (error) {

            console.error(error.message);

        }







    }catch (error) {



        if (error.name == "AbortError") {

            console.log("catch block executed: AbortError")


        }


        console.error(error.message);

    }finally {
        cancelButton.remove();
    }

  

}