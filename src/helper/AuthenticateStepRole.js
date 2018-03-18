import { RedirectToRoute } from 'aurelia-router';
export class AuthenticateStepRole {
    run(navigationInstruction, next) {
        if (navigationInstruction.getAllInstructions().some(i => i.config.settings.roles.indexOf('admin') !== -1)) {
            var isAdmin = /* insert magic here */false;
            if (!isAdmin) {
                return next.cancel(new RedirectToRoute('login'));
            }
        }
        return next();
    }
}