import { CREATE_BACKUP_SUCCESS, DELETE_BACKUP_SUCCESS, GET_BACKUPS } from "../constants/database";

const initialState = {
    backups: []
};

export default function database(state = initialState, action) {
    const { type, payload } = action;

    switch (type) {
        case GET_BACKUPS:
            return {
                backups: payload.backups
            }
        case CREATE_BACKUP_SUCCESS:
            return {
                backups: [...state.backups, payload.backupName]
            }
        case DELETE_BACKUP_SUCCESS:
            return {
                backups: state.backups.filter(x => x.backupName !== payload.backupName)
            }
        default:
            return state;
    }
}