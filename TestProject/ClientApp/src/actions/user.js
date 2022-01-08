import EventBus from "../common/EventBus";
import { CREATE_USER_SUCCESS, DELETE_USER_SUCCESS, CREATE_USER_ERROR, DELETE_USER_ERROR, GET_ROLES, EDIT_USER_SUCCESS, EDIT_USER_ERROR, GET_USERS } from "../constants/user";
import { SET_MESSAGE } from "../constants/message";
import userService from "../services/user.service"
import { toast } from "react-toastify";

export const getUsers = (t) => (dispatch) => {
    return userService.getUsers().then(
        (responce) => {
            dispatch({
                type: GET_USERS,
                payload: { users: responce.data }
            });

            toast.success(t("LoadSuccess"));
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }
        }
    )
}

export const getRoles = (t) => (dispatch) => {
    return userService.getRoles().then(
        (responce) => {
            dispatch({
                type: GET_ROLES,
                payload: { roles: responce.data }
            });
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }
        }
    )
}

export const createUser = (lastname, firstname, roleId, email, password, t) => (dispatch) => {
    return userService.createUser(lastname, firstname, roleId, email, password).then(
        (responce) => {
            dispatch({
                type: CREATE_USER_SUCCESS,
                payload: { user: responce.data }
            });

            toast.success(t("CreateSuccess"));

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            dispatch({
                type: CREATE_USER_ERROR
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

export const deleteUser = (id, t) => (dispatch) => {
    return userService.deleteUser(id).then(
        (responce) => {
            dispatch({
                type: DELETE_USER_SUCCESS,
                payload: { id }
            });

            toast.success(t("DeleteSuccess"));
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }

            dispatch({
                type: DELETE_USER_ERROR
            });
        }
    )
}

export const editUser = (userId, lastname, firstname, roleId, role, t) => (dispatch) => {
    return userService.editUser(userId, lastname, firstname, roleId).then(
        (responce) => {
            dispatch({
                type: EDIT_USER_SUCCESS,
                payload: { userId, lastname, firstname, roleId, role }
            });

            toast.success(t("EditSuccess"));

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            dispatch({
                type: EDIT_USER_ERROR
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