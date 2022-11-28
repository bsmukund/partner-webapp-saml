# partner-web-app-saml

The contents in this repo is developed as an SPA with a React UI and dotnet backend to support it
## React UI:
Contains the basic UI with a login form, home page
 - Login Component
    The login credentials are defined
 - Saml component:
    This acts as a proxy layer receiving the request from McAfee's ID layer
    It will receive two query params in the request:
    a. SAMLRequest - will be a base64 encoded string containing the SAMLRequest definition
    b. RelayState - a random alphanumeric cryptographically generated string. The same relay state value must be passed back to McAfee's ID layer while we form post the SAML from this partner app
    The UI internally calls the backend layer to process the saml request
    The SAML response received from the backend is (form)posted to https://idpartner.mcafee.com/login/callback?connection=connection-name
    where "connection-name" will be shared by McAfee to the partner after the setup

## Dotnet backend supporting
WebAPI project contains 2 main controllers
 - Login Controller
    - POST /api/login/user with request body {"UserName":"", "Password":""}
        validates the request against the value set in the configuration.
        This login controller is for indicative purposes only - depicting a layer of authentication at the partner's side
        Refer to appsettings.json field - AllowedUsers

 - Saml Controller
    - GET /api/saml/process
        This API is responsible to receive the forwarded SAML request from the UI layer that it received from McAfee ID layer (id.mcafee.com)
        It will receive two query params in the request: 
            - SAMLRequest: Contains the request id and issuer details
            - RelayState: The same relay state as received will be given back to the caller
        

## Configuration details for backend
 - PrivateKey: can be left as empty, no more in use
 - PrivateKeyCertificatePath: Path to the pfx/cer/pem cert with private key
 - UserCCID: The user identifier for which SAML SSO is being performed
 - UserAffId: Affiliate ID to which the SSO user belongs to
 - UserCulture: Culture the user should see the landing page in 
 - SAMLRecipient: The callback URL with the connection name to which SAMLResponse must be posted to ex: https://idpartner.mcafee.com/login/callback?connection=saml-testsp
 - SAMLDomain: SAML Domain under which SAML is being generated - can be partner's domain
 - SAMLIssuer: Should be set to https://id.mcafee.com 
 - SAMLPostURL: The callback URL with the connection name to which SAMLResponse must be posted to ex: https://idpartner.mcafee.com/login/callback?connection=saml-testsp
 - SAMLValidPeriod: Time in minutes until which the current SAML request is to be honored

Note: This is an implementation for representative purposes only. Design and approach is to the discretion of the reader.
This repo can be viewed for basic idea and details about SAML processing


