import React from 'react';
import { Switch } from 'react-router-dom';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer, Zoom } from 'react-toastify';

const Layout = (props) => {

    return (
        <div>
            <NavMenu />
            <Container className="mt-4">
                <Switch>
                    {props.children}
                </Switch>
            </Container>
            <ToastContainer position="bottom-right" toastClassName="dark-toast" transition={Zoom} theme="dark" />
        </div>
    );
}

export default Layout;