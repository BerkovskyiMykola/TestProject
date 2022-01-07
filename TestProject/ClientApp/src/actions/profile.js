import EventBus from "../common/EventBus";
import { SET_MESSAGE } from "../constants/message";
import { EDIT_PROFILE_ERROR, EDIT_PROFILE_SUCCESS, GET_PROFILE_INFO_ERROR, GET_PROFILE_INFO_SUCCESS } from "../constants/profile";
import profileService from "../services/profile.service";
import { history } from "../utils/history";
import { toast } from "react-toastify";

export const getProfile = () => (dispatch) => {
    return profileService.getProfile().then(
        (responce) => {
            dispatch({
                type: GET_PROFILE_INFO_SUCCESS,
                payload: responce.data
            });

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                history.push("/404");
            }
            dispatch({
                type: GET_PROFILE_INFO_ERROR,
            });

            return Promise.reject();
        }
    )
}

export const editProfile = (lastname, firstname, t) => (dispatch) => {
    return profileService.editProfile(lastname, firstname).then(
        (responce) => {
            dispatch({
                type: EDIT_PROFILE_SUCCESS,
                payload: { lastname, firstname }
            });

            toast.success(t("EditSuccess"));

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }

            dispatch({
                type: EDIT_PROFILE_ERROR
            });

            const message = error.response.data.title || error.response.data;

            dispatch({
                type: SET_MESSAGE,
                payload: message,
            });

            return Promise.reject();
        }
    )
}