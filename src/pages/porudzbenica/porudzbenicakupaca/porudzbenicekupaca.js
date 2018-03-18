import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {EntityManager} from 'aurelia-orm';
import 'kendo/js/kendo.grid';
import 'kendo/js/kendo.dropdownlist';
import * as toastr from 'toastr';

@inject(AuthService, EntityManager, DialogService, Endpoint.of())
export class Korisnici {
  roles = ["Komercijalista", "Supervizor", "Administrator"];
  pageable = {
    messages: {
      display: "{0} - {1} of {2} redova", //{0} is the index of the first record on the page, {1} - index of the last record on the page, {2} is the total amount of records
      empty: "Broj redova za prikaz",
      page: "Strana",
      of: "od {0}", //{0} is total amount of pages
      itemsPerPage: "redova po strani",
      first: "Idi na prvu stranu",
      previous: "Idi na poslednju stranu",
      next: "Idi na sledeću stranu",
      last: "Idi na poslednju stranu",
      refresh: "Osveži"
    },
    refresh: true,
    pageSizes: true,
    buttonCount: 5
  };
  filterobj = {
    messages: {
      info: "Prikaži podatke koji", // sets the text on top of the Filter menu
      filter: "Filter", // sets the text for the "Filter" button
      clear: "Poništi", // sets the text for the "Clear" button

      // when filtering boolean numbers
      isTrue: "custom is true", // sets the text for "isTrue" radio button
      isFalse: "custom is false", // sets the text for "isFalse" radio button

      //changes the text of the "And" and "Or" of the Filter menu
      and: "CustomAnd",
      or: "CustomOr"
    },
    //mode: "row",
    extra: false,
    operators: {
      string: {
        contains: "Sadrže"
      }
    }
  }

  constructor(authService, em, dialogService, repo) {
    this.authService = authService;
    this.repo = repo;
    this.repoKorisnik = em.getRepository('korisnik');
    this.dialogService = dialogService;
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
  
  
}
