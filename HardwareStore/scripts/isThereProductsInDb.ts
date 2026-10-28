async function isThereProductsInDb(): Promise<boolean> {
    const url = '/Admin/IsThereProductsInDb';
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const isThereProductsInDb = await response.json();
        return isThereProductsInDb;

    } catch (error: any) {
        console.error(error);
        return false;
    }
}

async function Main() {


    var thereAreProductsInDb = await isThereProductsInDb();
    // Source - https://stackoverflow.com/a/50269969
    // Posted by Ted
    // Retrieved 2026-10-26, License - CC BY-SA 4.0

    if (!thereAreProductsInDb) {

        console.log("There are no products in the database.");

        const productsContainer = document.getElementById("products-container");
        productsContainer!.innerHTML = '';

        var notification = document.createElement("p");
        notification.innerText = "There are no products in the database yet,please import products in order to show them here."
        notification.className = "alert alert-info text-center mx-auto col-12 col-md-8 col-lg-6 p-3 p-md-4";
        notification.id='noProductsAvailableNotification'
        notification.setAttribute("role", "alert");
        productsContainer!.appendChild(notification);
        sessionStorage.setItem("hideShowProductsAnchor", "true");
        document.getElementById("showProductsAnchor")!.hidden = true;
        document.getElementById("productsNavigator")?.remove();

        return

    }

        
    document.getElementById("noProductsAvailableNotification")!.innerText = "click the button to show the products";

    sessionStorage.setItem("hideShowProductsAnchor", "false");
    document.getElementById("showProductsAnchor")!.hidden = false;
    console.log("There are products in the database!");

        

    

    

}

console.log("`isThereProductsInDb` script was executed");


/**Period checking, otherwise the browser will not be aware of manual products deletion using SSMS*/
/**
 * 
 * Note : SSMS is an abbreviation for Sql Server management studio.
 */
let isRunning = false;
setInterval(async () => {
    if (isRunning) return;

    isRunning = true;
    try {
        await Main();
    } finally {
        isRunning = false;
    }
}, 1000);

