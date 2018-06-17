import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {EntityManager} from 'aurelia-orm';
import {Common} from 'helper/common';
import {Router} from 'aurelia-router';
import {DataCache} from 'helper/datacache';
import 'kendo/js/kendo.grid';
import 'kendo/js/kendo.dropdownlist';
import * as toastr from 'toastr';

@inject(AuthService, Common, Endpoint.of(), Router, DataCache)
export class PorudzbeniceKupaca {

  constructor(authService, common, repo, router, dc) {
    this.authService = authService;
    this.repo = repo;
    this.common = common;
    this.router = router;
    this.dc = dc;
    let payload = this.authService.getTokenPayload();
    if (payload) {
      this.korisnik = payload.unique_name;
      this.role = payload.role;
    };
    this.statusFilter =
    {
        extra : false,
        ui: (element) => {
            element.kendoDropDownList({
                dataTextField: "naziv",
                dataValueField: "id",
                dataSource: this.statusi
            });
        },
        operators: {
            string: {
                contains: "Sadrže"
            },
            number: {
                eq: "je jednak"
            }
        }
    };
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
                datum: { type: 'date' },
                datumVazenja: { type: 'date' }
            }
        }
    }
    });
  }

  activate(params, routeData) {
    var promises = [
      this.dc.getStatusiPork()
    ];

    return Promise.all(promises)
      .then(res => {
        this.statusi = res[0];
      })
      .catch(err => toastr.error(err.statusText));
  }

  detailInit(e) {
    let detailRow = e.detailRow;

    detailRow.find('.tabstrip').kendoTabStrip({
        animation: {
            open: { effects: 'fadeIn' }
        }
    });

    detailRow.find('.stavke').kendoGrid({
        dataSource: {
            pageSize: 10,
            batch: false,
            transport: {
                read: (o)=> {
                    this.repo.post('Dokument/StavkePregledGrid', o.data)    
                    //this.lokalep.post('Zamena/PregledGrid', o.data)                    
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
            },
            filter: { field: 'Dokument.Id', operator: 'eq', value: e.data.id }
        },
        //    pageSize: 10,
        //    batch: false,
        //    transport: {
        //        read: (o)=> {
        //            this.repo.post('Dokument/StavkePregledGrid', o.data)                    
        //                .then(result => {
        //                    o.success(result);
        //                })
        //                .catch(err => {
        //                    console.log(err.statusText);
        //                });
        //        }
        //    },
        //    serverPaging: true,
        //    serverSorting: true,
        //    serverFiltering: true,
        //    filter: { field: 'Dokument.Id', operator: 'eq', value: e.data.id }
        //},
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
          { field: 'rbr', title: 'Redni broj', width: '70px' },
          { field: 'rezervniDeo.sifra', title: 'Šifra', width: '70px' },
          { field: 'rezervniDeo.naziv', title: 'Naziv', width: '140px' },
          { field: 'kolicina', title: 'Količina', width: '70px' },
          { field: 'cena', title: 'Cena', width: '70px' }
        ]
    });
  }
  izmena(obj, e){
    this.router.navigateToRoute("porudzbenicakupaca", { id: obj.id });
    
  }
  
}
