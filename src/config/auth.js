export default {
    // Which endpoint (in `./app.js`) to use for auth requests.
    endpoint: 'auth',

    // Which endpoints to patch with the JWT (Authorization header).
    configureEndpoints: ['auth', 'lokal'],

    // The base url used for all authentication related requests, including provider.url below.
    // This appends to the httpClient/endpoint base url (in `./app.js`), it does not override it.
    //baseUrl: 'auth',

    // The API endpoint to which login requests are sent
    loginUrl: 'token',

    // The API endpoint to which signup requests are sent
    //signupUrl: '/signup',

    // Redirect to this url after successfully logging in
    loginRedirect: '#/home',

    // The API endpoint used in profile requests (inc. `find/get` and `update`)
    profileUrl: 'token/Profil',
    logoutRedirect: '/#',
    //tokenName: 'token',
    //accessTokenProp: 'access_token',


    // If the property defined by `accessTokenProp` is an object:
    // ------------------------------------------------------------

    //This is the property from which to get the token `{ "accessTokenProp": { "accessTokenName" : '...' } }`
    accessTokenName: 'access_token',
    refreshTokenName: 'refresh_token',

    refreshTokenUrl: 'token/RefreshToken',

    useRefreshToken: true
};
