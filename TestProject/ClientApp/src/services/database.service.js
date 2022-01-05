import axios from "axios";
import { CURRENT_DOMAIN } from "../utils/domain";
import authHeader from "./auth-header";
const API_URL = CURRENT_DOMAIN + "/Database/";

class DatabaseService {

    getBackups() {
        return axios.get(API_URL + "backups", { headers: authHeader() });
    }

    createBackup() {
        return axios.post(API_URL + "createBackup", { }, { headers: authHeader() });
    }

    deleteBackup(backupName) {
        return axios.delete(API_URL + "deleteBackup/" + backupName, { headers: authHeader() });
    }

    restore(backupName) {
        return axios.put(API_URL + "restore/" + backupName, { }, { headers: authHeader() });
    }
}

export default new DatabaseService();
