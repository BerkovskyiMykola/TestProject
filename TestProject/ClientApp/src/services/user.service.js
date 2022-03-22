import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";
import authHeader from "./auth-header";

const API_URL = CURRENT_DOMAIN + "/Users/";

class UserService {
    getUsers() {
        return axios.get(API_URL + "all", { headers: authHeader() });
    }

    createUser(lastname, firstname, role, email, password) {
        return axios.post(API_URL + "create", { lastname, firstname, email, password, role }, { headers: authHeader() });
    }

    deleteUser(id) {
        return axios.delete(API_URL + "delete/" + id, { headers: authHeader() });
    }

    editUser(userId, lastname, firstname, role) {
        return axios.put(API_URL + "edit/" + userId, { userId, lastname, firstname, role }, { headers: authHeader() });
    }
}

export default new UserService();