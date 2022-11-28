import React, { Component } from 'react';
import { useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { Login } from './components/Login';
import './custom.css';


const getToken = function() {
    const tokenString = sessionStorage.getItem('token');
    const userToken = JSON.parse(tokenString);
    return userToken?.token
}

const token = getToken();

export default class App extends Component {
    static displayName = App.name;

    render() {
        //const [token, setToken] = useState();
        if (!token) {
            return <Login />
        }
        else {
            return (
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} onEnter={this.getToken } />;
                        })}
                    </Routes>
                </Layout>
            );
        }
  }
}