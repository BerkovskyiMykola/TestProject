import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";

const API_URL = CURRENT_DOMAIN + "/Auth/";

class AuthService {

    login(email, password) {
        return axios
            .post(API_URL + "login", { email, password })
            .then((response) => {
                if (response.data.token) {
                    sessionStorage.setItem("user", JSON.stringify(response.data));
                }

                return response.data;
            });
    }

    logout() {
        sessionStorage.removeItem("user");
    }

    register(lastname, firstname, email, password) {
        return axios.post(API_URL + "register", {
            lastname,
            firstname,
            email,
            password
        }).then((response) => {
            if (response.data.token) {
                sessionStorage.setItem("user", JSON.stringify(response.data));
            }

            return response.data;
        });
    }
}

export default new AuthService();