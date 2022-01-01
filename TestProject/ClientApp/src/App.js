import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Route } from 'react-router';
import { history } from './utils/history';
import { logout } from "./actions/auth";
import { clearMessage } from "./actions/message";
import { Router } from 'react-router-dom';
import EventBus from "./common/EventBus";
import Layout from './components/Layout';
import Home from './components/Home';

import './App.css'

const App = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        history.listen((location) => {
            dispatch(clearMessage());
        });

        EventBus.on("logout", () => {
            dispatch(logout());
        });

        return () => { EventBus.remove("logout"); }
    }, [dispatch])

    return (
        <Router history={history}>
            <Layout>
                <Route exact path='/' component={Home} />
            </Layout>
        </Router>
    );
}

export default App;