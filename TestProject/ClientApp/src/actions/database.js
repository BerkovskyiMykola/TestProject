import EventBus from "../common/EventBus";
import { CREATE_BACKUP_ERROR, CREATE_BACKUP_SUCCESS, DELETE_BACKUP_ERROR, DELETE_BACKUP_SUCCESS, GET_BACKUPS, RESTORE_ERROR, RESTORE_SUCCESS } from "../constants/database";
import databaseService from "../services/database.service";
import { toast } from "react-toastify";

export const getBackups = (t) => (dispatch) => {
    return databaseService.getBackups().then(
        (responce) => {
            dispatch({
                type: GET_BACKUPS,
                payload: { backups: responce.data }
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

export const restore = (backupName, t) => (dispatch) => {
    return databaseService.restore(backupName).then(
        (responce) => {
            dispatch({
                type: RESTORE_SUCCESS
            });

            toast.success(t("RestorSuccess"));
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }

            dispatch({
                type: RESTORE_ERROR
            });
        }
    )
}

export const createBackup = (t) => (dispatch) => {
    return databaseService.createBackup().then(
        (responce) => {
            dispatch({
                type: CREATE_BACKUP_SUCCESS,
                payload: { backupName: responce.data }
            });

            toast.success(t("CreateSuccess"));
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }
            else {
                toast.error(t("Error"));
            }

            dispatch({
                type: CREATE_BACKUP_ERROR
            });
        }
    )
}

export const deleteBackup = (backupName, t) => (dispatch) => {
    return databaseService.deleteBackup(backupName).then(
        (responce) => {
            dispatch({
                type: DELETE_BACKUP_SUCCESS,
                payload: { backupName }
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
                type: DELETE_BACKUP_ERROR
            });
        }
    )
}