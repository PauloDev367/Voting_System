import axios from "axios";

const API_URL = "http://localhost:5293/api/v1";

export function login(email, senha) {
    const data = { email: email, senha: senha };
    console.log(data);
    return axios.post(`${API_URL}/auth/login`, data, {
        headers: {
            "Content-Type": 'application/json'
        }
    });
}
