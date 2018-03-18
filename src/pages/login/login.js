import {AuthService} from 'aurelia-authentication';
import { Aurelia, inject, computedFrom } from 'aurelia-framework';
import { Endpoint } from 'aurelia-api';
import {activationStrategy} from 'aurelia-router';
import 'bootstrap';

@inject(Aurelia, AuthService, Endpoint.of())
export class Login {
    isLoading;

    username = "";

    password = "";

    constructor(Aurelia, authService, repo) {
        this.app = Aurelia;
        this.authService = authService;
        this.providers = [];
        this.isLoading = false;
        this.username = "";
        this.password = "";
        this.repo = repo;
    }

    determineActivationStrategy() {
        return activationStrategy.replace; //replace the viewmodel with a new instance
        // or activationStrategy.invokeLifecycle to invoke router lifecycle methods on the existing VM
        // or activationStrategy.noChange to explicitly use the default behavior
    }


    attached() {
        
    }

    detached() {

    }
    // make a getter to get the authentication status.
    // use computedFrom to avoid dirty checking
    //@computedFrom('authService.authenticated')
    //get authenticated() {
    //    return this.authService.authenticated;
    //}

    // use authService.login(credentialsObject) to login to your auth server
    login() {
        if (this.isLoading === true) return;
        this.isLoading = true;
        var credentialsObject = { username: this.username, password: this.password };
        this.authService.login(credentialsObject)
            .then(result => {
                if (result && result.authenticated) {
                    this.app.setRoot('app').then(() => this.isLoading = false);
                    //this.isLoading = false;
                } else {
                    toastr.error('Pogrešna lozinka i/ili korisničkom ime');
                    this.isLoading = false;
                }
            })
            .catch(err => {
                console.log(err);
                this.isLoading = false;
            });
    };

    // use authService.logout to delete stored tokens
    // if you are using JWTs, authService.logout() will be called automatically,
    // when the token expires. The expiredRedirect setting in your authConfig
    // will determine the redirection option
    logout() {
        this.authService.logout();
    }


    detached() {
        this.username = "";
        this.password = "";
    }
}
