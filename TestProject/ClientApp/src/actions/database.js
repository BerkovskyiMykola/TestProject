import EventBus from "../common/EventBus";
import { CREATE_BACKUP_ERROR, CREATE_BACKUP_SUCCESS, DELETE_BACKUP_ERROR, DELETE_BACKUP_SUCCESS, GET_BACKUPS, RESTORE_ERROR, RESTORE_SUCCESS } from "../constants/database";
import databaseService from "../services/database.service";

export const getBackups = () => (dispatch) => {
    return databaseService.getBackups().then(
        (responce) => {
            dispatch({
                type: GET_BACKUPS,
                payload: { backups: responce.data }
            });

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            return Promise.reject();
        }
    )
}

export const restore = (backupName) => (dispatch) => {
    return databaseService.restore(backupName).then(
        (responce) => {
            dispatch({
                type: RESTORE_SUCCESS
            });

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            dispatch({
                type: RESTORE_ERROR
            });

            return Promise.reject();
        }
    )
}

export const createBackup = () => (dispatch) => {
    return databaseService.createBackup().then(
        (responce) => {
            dispatch({
                type: CREATE_BACKUP_SUCCESS,
                payload: { backupName: responce.data }
            });

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            dispatch({
                type: CREATE_BACKUP_ERROR
            });

            return Promise.reject();
        }
    )
}

export const deleteBackup = (backupName) => (dispatch) => {
    return databaseService.deleteBackup(backupName).then(
        (responce) => {
            dispatch({
                type: DELETE_BACKUP_SUCCESS,
                payload: { backupName }
            });

            return Promise.resolve();
        },
        (error) => {
            if (error.response && error.response.status === 401) {
                EventBus.dispatch("logout");
            }

            dispatch({
                type: DELETE_BACKUP_ERROR
            });

            return Promise.reject();
        }
    )
}