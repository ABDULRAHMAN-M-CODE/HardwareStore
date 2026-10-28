document.addEventListener("DOMContentLoaded", () => {
        const formFileLg = document.getElementById("formFileLg") as HTMLInputElement;
        formFileLg.addEventListener("change", importProductsFromExcelFile);
});

//W3schools uses `camelCase` for identifier names (variables and functions).
async function importProductsFromExcelFile(this: HTMLInputElement) {
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
    cancelContainer!.appendChild(cancelButton);
    const formData = new FormData();
   
    formData.append("file", this.files![0]);


    try {
        console.log("Try block executed.")
        const response = await fetch('/Admin/Import', {
            method: 'POST',
            body: formData,
            signal: controller.signal

        })
        if (!response.ok) {
            console.log("response is not ok")
            throw new Error(`Response status: ${response.status}`);

        }
        
    } catch (error:any) {



        if (error.name == "AbortError") {

            console.log("catch block executed: AbortError")


        }


        console.error(error.message);

    } finally {
        cancelButton.remove();
    }

}
