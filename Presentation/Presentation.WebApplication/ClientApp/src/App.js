import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Login, Register, ConfirmAccount, ForgotPassword, ResetPassword } from './components/account';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/login' component={Login} />
                <Route path='/register' component={Register} />
                <Route path='/confirm-account' component={ConfirmAccount} />
                <Route path='/forgot-password' component={ForgotPassword} />
                <Route path='/reset-password' component={ResetPassword} />
            </Layout>
        );
    }
}
