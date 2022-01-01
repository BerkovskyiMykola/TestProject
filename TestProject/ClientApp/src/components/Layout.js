import React from 'react';
import { Switch } from 'react-router-dom';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout = (props) => {

    return (
        <div>
            <NavMenu />
            <Container className="mt-4">
                <Switch>
                    {props.children}
                </Switch>
            </Container>
        </div>
    );
}

export default Layout;