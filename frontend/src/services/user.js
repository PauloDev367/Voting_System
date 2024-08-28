import axios from "axios";

const API_URL = "http://localhost:5293/api/v1";

export function login(email, senha) {
    const data = { email: email, senha: senha };
    return axios.post(`${API_URL}/auth/login`, data, {
        headers: {
            "Content-Type": 'application/json'
        }
    });
}
export function getUserInfo() {
    const token = window.localStorage.getItem("token");
    return axios.get(`${API_URL}/auth/me`, {
        headers: {
            "Content-Type": 'application/json',
            "Authorization": 'Bearer ' + token
        }
    });
}
