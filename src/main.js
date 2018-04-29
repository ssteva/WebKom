import environment from './environment';
import authConfig from './config/auth';
import * as entiteti from './config/entities';
import {AuthService} from 'aurelia-authentication';
import {EventAggregator} from 'aurelia-event-aggregator';



export function configure(aurelia) {
  let ea = aurelia.container.get(EventAggregator);
  aurelia.use
    .standardConfiguration()
    .feature('resources')
    .plugin('aurelia-api',
    config => {
      config
        .registerEndpoint('auth',
          configure => {
              configure.withBaseUrl('/');
          })
        .registerEndpoint('lokal',
          configure => {
              configure.withBaseUrl('api/');
              configure.withDefaults({
                  //credentials: 'same-origin',
                  headers: {
                      'Accept': 'application/json',
                      //'Content-Type': "application/json",
                      'X-Requested-With': 'Fetch'
                  }
              })
              configure.withInterceptor({
                  request(request) {
                      ea.publish('loader', true);
                      return request;
                      // you can return a modified Request, or you can short-circuit the request by returning a Response
                  },
                  response(response) {
                      ea.publish('loader', false);
                      return response; // you can return a modified Response
                      //return response.json().then(Promise.reject.bind(Promise));

                  }
              });
          })
        .setDefaultEndpoint('lokal')
  })  
  .plugin('aurelia-orm',
    builder => {
      builder.registerEntities(entiteti);
  })
  .plugin('aurelia-mousetrap', config => {
    // Example keymap
    config.set('keymap', {
      "alt+n": "KS_SEARCH",
      "n": "KS_NEW"
    });
  })
  .plugin('aurelia-kendoui-bridge', kendo => {
    kendo.kendoGrid()
        .kendoTemplateSupport()
        .kendoComboBox()
        //.kendoBarcode()
        .kendoContextMenu()
        .kendoDatePicker()
        .kendoNumericTextBox()
        .kendoDateTimePicker()
        .kendoAutoComplete()
        .kendoDropDownList()
  })
  .plugin('aurelia-validation')
  .plugin('aurelia-after-attached-plugin')
  .plugin('aurelia-dialog', config => {
    config.useDefaults();
    config.settings.lock = true;
    config.settings.centerHorizontalOnly = false;
    config.settings.startingZIndex = 5;
    config.settings.keyboard = true;
  })
  .plugin('aurelia-authentication',
    baseConfig => {
        baseConfig.configure(authConfig);
    });


  if (environment.debug) {
    aurelia.use.developmentLogging();
  }

  if (environment.testing) {
    aurelia.use.plugin('aurelia-testing');
  }

   //aurelia.start().then(() => aurelia.setRoot());
  aurelia.start().then(a => {
        
    // moment().locale('sr');
    // console.log(moment().format());
    //localStorage.setItem("putanja", window.location.href);
    let authService = aurelia.container.get(AuthService);
    //console.log(authService);
    //let payload = authService.getTokenPayload();
    let auth = authService.isAuthenticated();
    //console.log( 'a: ' + auth);
    if (auth === true) {  
        authService.updateToken()
            .then(result => {
                if (result === true) {
                    a.setRoot('app', document.body);
                } else {
                    a.setRoot('pages/login/login');    
                }
            })
            .error(err => {
                console.log(err);
                //console.log('login');
                a.setRoot('pages/login/login');
            })
            .catch(err => {
                console.log(err);
                //console.log('login');
                a.setRoot('pages/login/login');
            });
    } else {
        
        a.setRoot('pages/login/login');
    }
    
    //let rootComponent = authService.authenticated ? 'app' : 'login';
    //a.setRoot(rootComponent);

  });
}
