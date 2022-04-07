import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";
import authHeader from "./auth-header";

const API_URL = CURRENT_DOMAIN + "/Account/";

class ProfileService {

    getProfile() {
        return axios.get(API_URL + "profileInfo", { headers: authHeader() });
    }

    editProfile(lastname, firstname) {
        return axios.put(API_URL + "edit/profileInfo", { lastname, firstname }, { headers: authHeader() });
    }
}

export default new ProfileService();