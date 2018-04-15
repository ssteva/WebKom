import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {DialogController, DialogService} from 'aurelia-dialog';
import {DataCache} from 'helper/datacache';
import {Common} from 'helper/common';
import 'kendo/js/kendo.combobox';
import 'kendo/js/kendo.datepicker';
import 'kendo/js/kendo.grid';
import * as toastr from 'toastr';

@inject(AuthService, DataCache, Common, DialogService, Endpoint.of())
export class Porudzbenica {
  roles = ["Komercijalista", "Supervizor", "Administrator"];
  porudzbenica = null;
  skladista = [];
  odeljenja = [];
  constructor(authService, dc, common, dialogService, repo) {
    this.authService = authService;
    this.repo = repo;
    this.dialogService = dialogService;
    this.dc = dc;
    this.common = common;
    let payload = this.authService.getTokenPayload();
    if (payload) {
      this.korisnik = payload.unique_name;
      this.role = payload.role;
    }
    this.dsMestoIsporuke = new kendo.data.DataSource({
      pageSize: 10,
      batch: false,
      transport: {
        read: (o) => {
          let filter = "";
          if(o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0){
            filter = o.data.filter.filters[0].value;
          }
          var id = this.porudzbenica.kupac ? this.porudzbenica.kupac.id : null;
          this.repo.find('Subjekat/ListaKupacaComboSp?filter='+ filter + "&id=" + id  )
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
    this.dsKupac = new kendo.data.DataSource({
      pageSize: 10,
      batch: false,
      transport: {
        read: (o) => {
          let filter = "";
          if(o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0){
            filter = o.data.filter.filters[0].value;
          }
          this.repo.find('Subjekat/ListaKupacaComboSp?filter='+ filter)
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
    this.dsGrid = new kendo.data.DataSource({
      data: [], //this.porudzbenica.stavke,
      // schema: {
      //   model: {
      //     fields: {
      //       ProductName: { type: 'string' },
      //       UnitPrice: { type: 'number' },
      //       UnitsInStock: { type: 'number' },
      //       Discontinued: { type: 'boolean' }
      //     }
      //   }
      // },
      pageSize: 50
    });
    this.dsIdent = new kendo.data.DataSource({
      pageSize: 10,
      batch: false,
      transport: {
        read: (o) => {
          this.repo.post('ident/ListaCombo', o.data)
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

  activate(params, routeData) {
    var promises = [
      this.repo.findOne("Porudzbenica/?id=" + params.id + "&vrsta=PORK"),
      this.repo.findOne("Porudzbenica/PorudzbenicaStavka?id=0&vrsta=PORK"),
      this.dc.getSkladista(),
      this.dc.getOdeljenja()
    ];

    return Promise.all(promises)
      .then(res => {
        this.porudzbenica = res[0];
        this.porudzbenicastavka = res[1];
        if(params.id==="0"){
          this.porudzbenica.stavke.push(this.porudzbenicastavka);
        }
        
        this.skladista = res[2];
        this.odeljenja = res[3];

      })
      .catch(err => toastr.error(err.statusText));
  }
  afterAttached(){
    this.refresh();
  }
  refresh(){
    this.grid.dataSource.data(this.porudzbenica.stavke);
  }
  onMestoIsporukeSelect (e){
    let dataItem = this.cboMestoIsporuke.dataItem(e.item);
    if(!this.porudzbenica.kupac.id){
      if(dataItem && dataItem.platilac){
        this.porudzbenica.kupac = dataItem.platilac;
      }
    }
  }
  onMestoIsporukeFiltering(e){
    // if (e.filter) {
    //   var value = e.filter.value;
    //   var newFilter = {
    //     logic: "or",
    //       filters: [
    //           { field: "naziv", operator: "contains", value: value },
    //           { field: "mesto.naziv", operator: "contains", value: value }
    //       ]
    //   }
    //   if(this.porudzbenica.kupac.id){
    //     newFilter.filters.push({ field: "platilac.id", operator: "equals", value: this.porudzbenica.kupac.id })
    //   }
    //   e.sender.dataSource.filter(newFilter);
    //   e.preventDefault();
    // }
    // e.preventDefault();
  }
  onMestoIsporukeOpen (e, dis){
    this.cboMestoIsporuke.dataSource.read()
    //this.cboMestoIsporuke.refresh();
  }
  onKupacSelect (e){
    let dataItem = this.cboKupac.dataItem(e.item);
    if(dataItem && dataItem.id){
      this.porudzbenica.danaZaPlacanje = dataItem.danaZaPlacanje;
    }else{
      this.porudzbenica.kupac = null;
    }
  }
  onKupacOpen (e, dis){
    this.cboKupac.dataSource.read()
    //this.cboKupac.refresh();
  }
  onKupacFiltering(e){
    // if (e.filter) {
    //   var value = e.filter.value;
    //   var newFilter = {
    //       filters: [
    //           { field: "naziv", operator: "contains", value: value },
    //           { field: "mesto.naziv", operator: "contains", value: value }
    //       ],
    //       logic: "or"
    //   }
    //   e.sender.dataSource.filter(newFilter);
    //   e.preventDefault();
    // }
    // e.preventDefault();
  }
}
