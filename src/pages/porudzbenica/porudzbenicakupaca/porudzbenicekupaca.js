import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {EntityManager} from 'aurelia-orm';
import {Common} from 'helper/common';
import {Router} from 'aurelia-router';
import {DataCache} from 'helper/datacache';
import 'kendo/js/kendo.grid';
import 'kendo/js/kendo.dropdownlist';
import 'kendo/js/kendo.tabstrip';
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
    var indeks =  e.masterRow.index(".k-master-row") + 1;
    detailRow.find('.stavke').kendoGrid({
        dataSource: {
            pageSize: 10,
            batch: false,
            transport: {
                read: (o)=> {
                    this.repo.post('Porudzbenica/StavkePregledGrid', o.data)    
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
            filter: { field: 'Porudzbenica.Id', operator: 'eq', value: e.data.id },
            aggregate: [ { field: "rbr", aggregate: "count" },
            { field: "poruceno", aggregate: "sum"}],
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
          { field: 'rbr', title: 'Redni broj', width: '70px', footerTemplate: "Broj stavki: #=count#" },
          { field: 'ident.sifra', title: 'Ident', width: '120px' },
          { field: 'ident.naziv', title: 'Naziv', width: '70px' },
          { field: 'poruceno', title: 'Poručeno', width: '70px', footerTemplate: "Ukupno poručeno: #=sum#" },
          { field: 'konacnaCena', title: 'Konačna cena', width: '70px' },
          { /*field: "Total",*/ title: "Vrednost", width: '80px', template: "<span class='totalSpan'>#= poruceno * konacnaCena #</span>", footerTemplate: function(e,a,b){
            var zbir = 0;
            //var stavke = $('.stavke').data('kendoGrid').dataSource.view()

            //var detailsGridForRow = $(e.masterRow).siblings('.k-detail-row').find('.k-grid').data('kendoGrid').dataSource.view();
            var stavke = $("#grid tr:nth-child("  + indeks +  ")").siblings('.k-detail-row').find('.k-grid').data('kendoGrid').dataSource.view();
            stavke.forEach(stavka=>{
              zbir += stavka.konacnaCena * stavka.poruceno;
            })
              return 'Ukupna vrednost: ' + zbir;
          } }
        ]
    });
  }
  izmena(obj, e){
    this.router.navigateToRoute("porudzbenicakupaca", { id: obj.id });
    
  }
  
}
