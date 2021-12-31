import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";
import authHeader from "./auth-header";

const API_URL = CURRENT_DOMAIN + "/Users/";

class ProfileService {

    getUser() {
        return axios.get(API_URL + "profile", { headers: authHeader() });
    }

    editUser(lastname, firstname) {
        return axios.put(API_URL + "edit/profile", { lastname, firstname }, { headers: authHeader() });
    }
}

export default new ProfileService();