import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { shallowEqual, useSelector } from "react-redux";

export const PrivateRoute = ({ component: Component, roles, ...rest }) => {

    const { user } = useSelector(state => ({
        user: state.auth.user
    }), shallowEqual)

    return <Route {...rest} render={props => {

        if (!user) {
            return <Redirect to='/login' />
        }

        if (roles && roles.indexOf(user.role) === -1) {
            return <Redirect to='/' />
        }

        return <Component {...props} />
    }} />
}