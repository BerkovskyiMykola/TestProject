import { combineReducers } from "redux";
import auth from "./auth";
import message from "./message";
import profile from "./profile";
import database from "./database";

export default combineReducers({
    auth,
    message,
    profile,
    database
});