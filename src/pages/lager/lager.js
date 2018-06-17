import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {DialogController, DialogService} from 'aurelia-dialog';
import {DataCache} from 'helper/datacache';
import {Common} from 'helper/common';
import {computedFrom, observable} from 'aurelia-framework';
import 'kendo/js/kendo.combobox';
import 'kendo/js/kendo.datepicker';
import 'kendo/js/kendo.grid';
import * as toastr from 'toastr';

@inject(Common, Endpoint.of())
export class Lager{
  constructor(common, repo){
    this.common = common;
    this.repo = repo;
    this.datasource = new kendo.data.DataSource({
      pageSize: 50,
      batch: false,
      transport: {
          read: (o)=> {
              this.repo.post('Lager/PregledGrid', o.data)
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
          total: "total"
      }
      
  });
  }
}
