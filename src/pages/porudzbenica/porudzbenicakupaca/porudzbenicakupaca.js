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

@inject(AuthService, DataCache, Common, DialogService, Endpoint.of())
export class Porudzbenica {
  
  porudzbenica = null;
  porudzbenicastavka = null;
  skladista = [];
  odeljenja = [];
  constructor(authService, dc, common, dialogService, repo) {
    this.authService = authService;
    this.repo = repo;
    this.dialogService = dialogService;
    this.dc = dc;
    this.common = common;
    this.roles = this.common.roles;
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
          let filter = "";
          if(o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0){
            filter = o.data.filter.filters[0].value;
          }
          this.repo.find('ident/ListaCombo?filter='+filter)
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
          this.porudzbenicastavka.edit = true;
          this.porudzbenica.stavke.push(this.porudzbenicastavka);
        }
        
        this.skladista = res[2];
        this.odeljenja = res[3];

      })
      .catch(err => toastr.error(err.statusText));
  }
  afterAttached(){
    this.refresh(this.grid);
  }
  refresh(g){
    if(this.porudzbenica){
      this.dsGrid = new kendo.data.DataSource({
        data: this.porudzbenica.stavke,
        pageSize: 50
      });
      g.setDataSource(this.dsGrid);
    }
  }
  onMestoIsporukeSelect (e){
    let dataItem = this.cboMestoIsporuke.dataItem(e.item);
    if(!this.porudzbenica.kupac){
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
  onIdentSelect(e, porsta){
    let dataItem = e.sender.dataItem(e.item);
    if(dataItem){
      porsta.jm = dataItem.jm;
      porsta.koleta = dataItem.koleta;
    }
  }
  onIdentOpen (e, dis){
    e.sender.dataSource.read()
    //this.cboKupac.refresh();
  }

  kalkulacijaCene(e, porudzbenicastavka){
    try {
      let cena = porudzbenicastavka.cena;
      if(!porudzbenicastavka.rabat1){
        porudzbenicastavka.konacnaCena = cena;
      }
      
      let cena1 =  cena * (1 - porudzbenicastavka.rabat1 / 100);
      
      if(!porudzbenicastavka.rabat2){
        porudzbenicastavka.konacnaCena = cena1;
      }
      let cena2 = cena1 *  (1 - porudzbenicastavka.rabat2 / 100);

      if(!porudzbenicastavka.rabat3){
        porudzbenicastavka.konacnaCena = cena2;
      }
      let cena3 = cena2 * (1 - porudzbenicastavka.rabat3 / 100);
      porudzbenicastavka.konacnaCena = cena3;

    } catch (error) {
      console.log(error);
    }
  }
  potvrdi(obj, e) {

  }
  izmeniInline(obj, e) {
    obj.edit = true;
    this.grid.refresh(this.grid);
  }

  otkazi(obj, e) {
    this.grid.dataSource.read();
    this.proba = "Novo";
  }
  snimi(obj, e) {
    if (confirm("Da li želite da snimite izmene?")) {
      this.repo.post('Porudzbenica', this.porudzbenica)
        .then(res => {
          toastr.success("Uspešno snimljeno");
          this.grid.dataSource.read();
        })
        .error(err => toastr.error(err.statusText));
    }
  }
}
