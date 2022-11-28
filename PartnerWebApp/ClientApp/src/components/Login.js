import React, { Component } from 'react';
import logo from "./../assets/logo.PNG";
import PropTypes from 'prop-types';
import './Login.css';
import { Home } from './Home';

async function loginUser(credentials) {

    return fetch('login/user', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentials)
    })
    .then(data => data.json()) 
}

export function getToken() {
    const tokenString = sessionStorage.getItem('token');
    const userToken = JSON.parse(tokenString);
    return userToken?.token
}

export function setToken(userToken) {
    sessionStorage.setItem('token', JSON.stringify(userToken));
    console.log("Login successful ..");
    this.props.history.push("/");
}

export class Login extends Component {

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: ''
        };
        //this.setToken = this.setToken.bind(this)
    }

    setUserName(e) {
        e.preventDefault();
        console.log(e.target.value);
        this.setState({
            username: e.target.value
        })
    }

    setPassword(e) {
        e.preventDefault();
        console.log(e.target.value);
        this.setState({
            password: e.target.value
        })
    }

    handleSubmit = async e => {
        //this.setState({
        //    username: e
        //})
        e.preventDefault();
        const { username, password } = this.state;
        const token = await loginUser({
            "UserName": username,
            "Password": password
        });
        //setToken(token);
        var tknResponse = JSON.stringify(token);
        if (token.status == "success") {
            sessionStorage.setItem('token', token.token);
            window.location.reload();
            console.log("Login successful ..");
            this.props.history.push("/");
        }
    }
    /*const [username, setUserName] = useState();
    const [password, setPassword] = useState(); */

    componentDidMount() {
        if (getToken()) {
            this.props.history.push("/")
        }
    }

    render() {
        return (
            <div className="App">
                <form className="form" onSubmit={this.handleSubmit.bind(this)}>
                    <div className="logoContainer">
                        <img src={logo} className="logo" alt="MyCompany" />
                    </div>
                    <div className="head">
                        <h4 className="company">Welcome to MyCompany</h4>
                    </div>
                    <div className="input-group">
                        {/*<label>Email</label>*/}
                        <br />
                        <input type="text" name="email" placeholder="UserId" onChange={this.setUserName.bind(this)} />
                    </div>
                    <div className="input-group">
                        {/*<label htmlFor="password">Password</label>*/}
                        <br />
                        <input type="password" name="password" placeholder="Password" onChange={this.setPassword.bind(this)} />
                    </div>
                    <button type="submit" className="primary">Sign In</button>
                </form>
            </div>
            //<div className="container">
            //    <div className="login">
            //        <div className="head">
            //            <h1 className="company">Welcome to MyCompany</h1>
            //        </div>
            //        <p className="msg"> Please LogIn to Continue </p>
            //        <form onSubmit={this.handleSubmit} className="form">
            //            {/*<label className="lbl">*/}
            //            {/*    <p>Username</p>*/}
            //            <input type="text" className="text" placeholder="Username" onChange={(e) => this.setState({ username: e.target.value })} />
            //            {/*</label><br />*/}
            //            {/*<label className="lbl">*/}
            //            {/*    <p>Password</p>*/}
            //            <input type="password" className="password" placeholder="Password" onChange={(e) => this.setState({ password: e.target.value })} />
            //            {/*</label>*/}
            //            <div>
            //                <br />
            //                <button type="submit" className="btn-login">Submit</button>
            //            </div>
            //        </form>
            //    </div>
            //</div>
        );
    }
}

//Login.PropTypes = {
//    setToken: PropTypes.func.isRequired
//}