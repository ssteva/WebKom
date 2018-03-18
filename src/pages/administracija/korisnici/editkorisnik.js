import {inject} from 'aurelia-framework';
import {DialogController, DialogService} from 'aurelia-dialog';
import {AuthService} from 'aurelia-authentication';
import * as toastr from 'toastr';

@inject(DialogController,  DialogService, AuthService)
export class EditKorisnik{
    constructor(dialogController, dialogService, authService) {
        this.dialogController = dialogController;
        this.dialogService = dialogService;
        // this.authService = authService;
        // let payload = this.authService.getTokenPayload();
        // this.korisnik = payload.unique_name;
        // if (payload)
        //     this.role = payload.role;
    }
    activate(obj) {
        this.naslov = obj.naslov;
        this.korisnik = obj.korisnik;
        this.roles = obj.roles;
        this.repo = obj.repo;
        //if (!this.korisnik) {
        //    this.korisnik = this.repoKorisnik.getPopulatedEntity(korisnik);
        //} else {
        //    this.korisnik = this.repoKorisnik.getNewEntity();
        //}
        //console.log(this.korisnik);
    }

    obrisan(dis) {
        //console.log(dis.korisnik.obrisan);
        this.korisnik.obrisan = !this.korisnik.obrisan;
    }
    cancel() {
        this.dialogController.cancel();
    }
    resetLozinke() {
        if (confirm('Da li želite da se resetujete loziku za korisnika ' + this.korisnik.email + '?')) {
                this.repo.post('Korisnik/ResetLozinke', this.korisnik)
                    .then(result => {
                        toastr.info('Nova lozinka je ' + this.korisnik.korisnickoIme);
                    });
        };
    }
    potvrdi() {
        if (confirm("Da li želite da snimite izmene?")) {
          this.repo.post('Korisnik', this.korisnik)
            .then(res => {
              toastr.success("Uspešno snimljeno");
              this.dialogController.ok();
            })
            .error(err => toastr.error(err.statusText));
        }
    }

}
