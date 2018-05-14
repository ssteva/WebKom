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
Number.prototype.formatMoney = function(c, d, t){
  var n = this, 
      c = isNaN(c = Math.abs(c)) ? 2 : c, 
      d = d == undefined ? "." : d, 
      t = t == undefined ? "," : t, 
      s = n < 0 ? "-" : "", 
      i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))), 
      j = (j = i.length) > 3 ? j % 3 : 0;
  return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
