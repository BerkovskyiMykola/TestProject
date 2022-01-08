import { CREATE_USER_SUCCESS, DELETE_USER_SUCCESS, GET_USERS, GET_ROLES, EDIT_USER_SUCCESS } from "../constants/user";

const initialState = {
    users: [],
    roles: []
};

export default function user(state = initialState, action) {
    const { type, payload } = action;

    switch (type) {
        case GET_USERS:
            return {
                ...state,
                users: payload.users
            }
        case GET_ROLES:
            return {
                ...state,
                roles: payload.roles
            }
        case CREATE_USER_SUCCESS:
            return {
                ...state,
                users: [...state.users, payload.user]
            }
        case DELETE_USER_SUCCESS:
            return {
                ...state,
                users: state.users.filter(x => x.id !== payload.id)
            }
        case EDIT_USER_SUCCESS:
            return {
                ...state,
                users: state.users.map(user => {
                    if (user.id === payload.userId)
                        return {
                            ...user,
                            lastname: payload.lastname,
                            firstname: payload.firstname,
                            roleId: payload.roleId,
                            role: payload.role,
                        }
                    return user;
                })
            }
        default:
            return state;
    }
}