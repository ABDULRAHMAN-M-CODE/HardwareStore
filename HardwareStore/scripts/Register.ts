window.onload = () => {

    document.querySelector("#registerForm")!.addEventListener("input", () => {
        document.querySelector("#registrationErrors")?.remove();
    }, { once: true });

}
