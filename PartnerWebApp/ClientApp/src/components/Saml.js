import React, { Component } from 'react';
//import { parse } from 'url';
//import { axios } from 'axios';

const querystring = require('query-string');
const axios = require('axios').default;
//import { URLSearchParams } from 'url';
//import { URLSearchParams } from 'react-router-dom';
//import { useSearchParams } from 'react-router-dom';


let searchParams; //= useSearchParams();
//let samlrequest, relaystate;

//function useQS() {
//    [searchParams] = useSearchParams();
//}
let samlrequest, relaystate;

export class Saml extends Component {

    static displayName = Saml.name;

    constructor(props) {
        super(props);
        this.state = { samlresponse: null, validationStatus: false, relaystate: null }
        //useQS();
        //[searchParams] = useSearchParams();
    }

    

    componentDidMount() {
        //const queryParams = new URLSearchParams(window.location.search);
        const queryParams = querystring.parse(window.location.search);
        //[searchParams] = useSearchParams();
        //const len = queryParams.query.length;
        samlrequest = queryParams.SAMLRequest;  //.get('SAMLRequest');
        relaystate = queryParams.RelayState; //.get('RelayState');
        console.log("SAML request is " + samlrequest);
        console.log("RelayState is " + relaystate);
        this.setState({ relaystate: relaystate });
        this.validateSamlAssertion();
    }

    componentDidUpdate() {
        const samlForm = document.getElementById("SAMLForm");
        if (samlForm) {
            document.getElementById("SAMLForm").submit();
        }
    }

    async validateSamlAssertion() {

        const queryParams = querystring.parse(window.location.search);
        console.log("SAML request is " + queryParams.SAMLRequest);
        console.log("RelayState is " + queryParams.RelayState);
        const url = 'saml/process?SAMLRequest=' + samlrequest + "&relayState=" + relaystate;
        const response = fetch(url);
        const data = await (await response).json();
        this.setState({ samlresponse: data.samlresponse, validationstatus: data.validationstatus });

        var formBody = [];
        var details = {
            'SAMLResponse': data.samlresponse,
            'RelayState': this.state['relaystate']
        }

        for (var property in details) {
            var encodedKey = encodeURIComponent(property);
            var encodedValue = encodeURIComponent(details[property]);
            formBody.push(encodedKey + "=" + encodedValue);
        }

        formBody.join("&");

        const options = {
            method: 'POST',
            crossDomain: true,
            withCredentials: true,
            url: 'https://idpartner.mcafee.com/login/callback?connection=saml-testsp',
            headers: {
                "Content-Type": "x-www-form-urlencoded",
                "Access-Control-Allow-Origin": "http://localhost:44464",
                "Access-Control-Allow-Credential": "true"
            },
            data: formBody
        };

        //const respToken = await axios.request(options);
        //console.log("Response is : " + respToken);
        //axios. post('https://idpartner.mcafee.com/login/callback?connection=saml-testsp',)

        //const auth0Resp = fetch('https://idpartner.mcafee.com/login/callback?connection=saml-testsp', {
        //    method: "POST",
        //    keepalive: true,
        //    headers: [
        //        ["Content-Type", "x-www-form-urlencoded"]
        //    ],
        //    body: formBody
        //});
        //await auth0Resp;
    }

    static samlForm(mysamlresponse, myrelaystate) {
        console.log(mysamlresponse);
        return (
            <form name="SAMLForm" id="SAMLForm" method="POST" action="https://idpartner.mcafee.com/login/callback?connection=saml-testsp">
                <input name="SAMLResponse" id="SAMLResponse" type="hidden" value={mysamlresponse} />
                <input name="RelayState" id="RelayState" type="hidden" value={myrelaystate} />
            </form>
            )
    }
    render() {
        //let contents = this.state.validationStatus ?
        //    <p><em>Loading .... </em></p> :
        //    Saml.samlForm()

        let contents = <p> <em> LOADING .... </em> </p>
        if (this.state.validationstatus === true) {
            console.log("WOw success...");
            contents = Saml.samlForm(this.state.samlresponse, this.state.relaystate);
        }

        return (
        <div>
                {contents}
                </div>);
    }
}