import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {EntityManager} from 'aurelia-orm';
import {DialogService} from 'aurelia-dialog';
import {EditKorisnik} from './editkorisnik'
import 'kendo/js/kendo.grid';
import 'kendo/js/kendo.dropdownlist';
import * as toastr from 'toastr';
import {Common} from 'helper/common';

@inject(AuthService, EntityManager, DialogService, Endpoint.of(), Common)
export class Korisnici {
  roles = ["Komercijalista", "Supervizor", "Administrator"];

  constructor(authService, em, dialogService, repo, common) {
    this.authService = authService;
    this.repo = repo;
    this.repoKorisnik = em.getRepository('korisnik');
    this.dialogService = dialogService;
    this.common = common;
    let payload = this.authService.getTokenPayload();
    if (payload) {
      this.korisnik = payload.unique_name;
      this.role = payload.role;
    }
    this.datasource = new kendo.data.DataSource({
      pageSize: 10,
      batch: false,
      transport: {
        read: (o) => {
          this.repo.find('Korisnik/ListaKorisnika', o.data)
            .then(result => {
              o.success(result);
            })
            .catch(err => {
              console.log(err.statusText);
            });
        }
      },
      serverPaging: true,
      serverSorting: true,
      serverFiltering: true,
    });
  }
  izmeni(obj, e) {
    // obj.edit = true;
    // this.grid.refresh();
    let objm = {
      naslov: `${obj.ime} ${obj.prezime}`,
      roles: this.roles,
      korisnik: obj,
      repo: this.repo
    };

    this.dialogService.open({viewModel: EditKorisnik,model: objm})
      .whenClosed(response => {
        console.log(response);
        if (!response.wasCancelled) {
          console.log('not cancelled');
          this.datasource.read();
      } else {
        console.log('cancelled');
      }
    });
  }
  izmeniInline(obj, e) {
    obj.edit = true;
    this.grid.refresh();
  }
  potvrdi(obj, e) {
    if (confirm("Da li želite da snimite izmene?")) {
      this.repo.post('Korisnik', obj)
        .then(res => {
          toastr.success("Uspešno snimljeno");
          this.grid.dataSource.read();
        })
        .error(err => toastr.error(err.statusText));
    }
  }
  otkazi(obj, e) {
    this.grid.dataSource.read();
  }
  noviKorisnik() {
    if (this.role !== 'Administrator') return;
    let obj = {
      naslov: "Novi korisnik",
      roles: this.roles,
      korisnik: this.repoKorisnik.getNewEntity(),
      repo: this.repo
    };

    this.dialogService.open({viewModel: EditKorisnik,model: obj})
      .then(response => {
        console.log(response);
        if (!response.wasCancelled) {
          console.log('not cancelled');
          this.datasource.read();
      } else {
        console.log('cancelled');
      }
      console.log(response.output);
    });
  }
}
