import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";
import authHeader from "./auth-header";

const API_URL = CURRENT_DOMAIN + "/Users/";

class UserService {
    getUsers() {
        return axios.get(API_URL + "all", { headers: authHeader() });
    }

    getRoles() {
        return axios.get(CURRENT_DOMAIN + "/Roles/all", { headers: authHeader() });
    }

    createUser(lastname, firstname, roleId, email, password) {
        return axios.post(API_URL + "create", { lastname, firstname, email, password, roleId }, { headers: authHeader() });
    }

    deleteUser(id) {
        return axios.delete(API_URL + "delete/" + id, { headers: authHeader() });
    }

    editUser(userId, lastname, firstname, roleId) {
        return axios.put(API_URL + "edit/" + userId, { userId, lastname, firstname, roleId }, { headers: authHeader() });
    }
}

export default new UserService();