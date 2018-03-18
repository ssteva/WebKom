import routes from "./config/routes";
import { inject } from 'aurelia-framework';
import { AuthenticateStep, AuthService } from 'aurelia-authentication';
import { Endpoint } from 'aurelia-api';
//import { DataCache } from 'helper/datacache';
import * as toastr from 'toastr';
import 'bootstrap';

@inject(AuthService, Endpoint.of())
export class App {

  constructor(authService, repo) {
    this.authService = authService;
    this.repo = repo;
  }

  configureRouter(config, router) {
    config.title = 'Webkom';
    //config.options.pushState = true;
    //config.addPipelineStep('authorize', AuthenticateStep);
    //config.addPipelineStep('authorize', AuthenticateStepRole);
    config.map(this.routes);
    this.router = router;
  }
  activate() {
    return this.repo.find('Meni')
      .then(result => {
        this.routes=result;
      })
      .catch(err => {
        toastr.error(err.statusText);
      });
  }
}
