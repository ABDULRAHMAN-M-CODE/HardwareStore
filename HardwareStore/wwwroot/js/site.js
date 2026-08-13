// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const submitUserData = async () => {
    debugger;
    const emailOrPhoneNumber = document.getElementById("email-or-phoneNumber");
    const password = document.getElementById("password");

    const payload = {
        EmailOrPhoneNumber: emailOrPhoneNumber.value,
        Password: password.value
    }

    
    try {
        fetch("/Home/OnPostAsync", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
         }).then(response => {
             debugger;
             console.log(response)
         });
        
    } catch (error) {

        alert("Check your internet connection.");
        console.log("catch block executed:\n", error)

    }
}

    



