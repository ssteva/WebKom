import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {EntityManager} from 'aurelia-orm';
import {Common} from 'helper/common';
import 'kendo/js/kendo.grid';
import 'kendo/js/kendo.dropdownlist';
import * as toastr from 'toastr';

@inject(AuthService, Common, Endpoint.of())
export class PorudzbeniceKupaca {

  constructor(authService, common, repo) {
    this.authService = authService;
    this.repo = repo;
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
          this.repo.post('Porudzbenica/PregledGrid', o.data)
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
      schema: {
        data: "data",
        total: "total",
        model: {
            id: "id",
            fields: {
                datum: { type: 'date' }
            }
        }
    }
    });
  }
  detailInit(e) {
    
  }
  
  
}
