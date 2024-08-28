export function verifyIfIsLogged(to, from, next) {
    const token = localStorage.getItem("token");
    if (token == null) {
        alert("É preciso fazer login para acessar essa rota")
        next("/");
        return;
    }
}

export function getUserRole() {
    const user = JSON.parse(window.localStorage.getItem("user"));
    return user.role;
}