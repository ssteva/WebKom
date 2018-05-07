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
import {EventAggregator} from 'aurelia-event-aggregator';
import 'jquery.dataTables';

@inject(AuthService, DataCache, Common, DialogService, Endpoint.of(), EventAggregator)
export class Porudzbenica {
  
  porudzbenica = null;
  porudzbenicastavka = null;
  skladista = [];
  odeljenja = [];
  constructor(authService, dc, common, dialogService, repo, eventAggregator) {
    this.authService = authService;
    this.repo = repo;
    this.dialogService = dialogService;
    this.dc = dc;
    this.common = common;
    this.roles = this.common.roles;
    this.eventAggregator = eventAggregator;
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
          this.repo.find('Subjekat/ListaKupacaComboSp?filter=' + filter + "&id=" + id)
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
          this.repo.find('Ident/ListaCombo?filter='+filter)
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
    this.dsOdeljenje = new kendo.data.DataSource({
      pageSize: 10,
      batch: false,
      transport: {
        read: (o) => {
          let filter = "";
          if(o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0){
            filter = o.data.filter.filters[0].value;
          }
          this.repo.find('Subjekat/ListaOdeljenjaComboSp?filter='+ filter)
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
        this.porudzbenicastavkaprazna = res[1];
        if(params.id==="0"){
          this.novaStavka();
        }
        
        this.skladista = res[2];
        this.odeljenja = res[3];

      })
      .catch(err => toastr.error(err.statusText));
  }
  afterAttached(){
    //this.setGridDataSource(this.grid);
    // $(this.tblstavke).DataTable({
    //   paging: false,
    //   scrollY: 400,
    //   info: false,
    //   searching: false
    // });
    $('[data-toggle="tooltip"]').tooltip();
    //wire focus of all numerictextbox widgets on the page
    $("input[type=numeber]").on("focus", function () {
      var input = $(this);
          clearTimeout(input.data("selectTimeId")); //stop started time out if any

          var selectTimeId = setTimeout(function()  {
              input.select();
              // To make this work on iOS, too, replace the above line with the following one. Discussed in https://stackoverflow.com/q/3272089
              // input[0].setSelectionRange(0, 9999);
          });

          input.data("selectTimeId", selectTimeId);
      }).blur(function(e) {
          clearTimeout($(this).data("selectTimeId")); //stop started timeout
      });
    this.subscription = this.eventAggregator.subscribe('loader', payload => {
      this.loading = payload;
  });
  }
  // setGridDataSource(g){
  //   if(this.porudzbenica){
  //     this.dsGrid = new kendo.data.DataSource({
  //       data: this.porudzbenica.stavke,
  //       pageSize: 50
  //     });
  //     g.setDataSource(this.dsGrid);
  //   }
  // }
  novaStavka(){
    this.porudzbenicastavka =  JSON.parse(JSON.stringify(this.porudzbenicastavkaprazna));
    this.porudzbenicastavka.edit = true;
    this.porudzbenicastavka.rbr = this.porudzbenica.stavke.length + 1;
    if(this.porudzbenica.odeljenje)
      this.porudzbenicastavka.odeljenje = this.porudzbenica.odeljenje;
    this.kalkulacijaCene(this.porudzbenicastavka, this.porudzbenicastavka.rabat1,this.porudzbenicastavka.rabat2, this.porudzbenicastavka.rabat3)
    this.porudzbenica.stavke.push(this.porudzbenicastavka);
    $('[data-toggle="tooltip"]').tooltip();
  }
  srediStavke(){

  }
  onMestoIsporukeSelect (e){
    let dataItem = this.cboMestoIsporuke.dataItem(e.item);
    if(dataItem){
      this.porudzbenica.mestoIsporuke = dataItem;
    }
    if(!this.porudzbenica.kupac || !this.porudzbenica.kupac.id){
      if(dataItem && dataItem.platilac){
        this.porudzbenica.kupac = dataItem.platilac;
      }
    }
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
  onKupacChange (e, dis){
    //let dataItem = this.cboKupac.dataItem(e.item);
    if(e.sender.value()===""){
      this.porudzbenica.kupac = null;
    }
    
    //this.cboKupac.refresh();
  }
  onOdeljenjeSelect (e){
    let dataItem = this.cboOdeljenje.dataItem(e.item);
    if(dataItem && dataItem.id){
      this.porudzbenica.stavke.forEach((stavka)=>{
        if(stavka.ident){
          if(stavka.ident.id){
            if(!stavka.odeljenje.id){
              stavka.odeljenje = dataItem;
            }
          }
        }
      })
    }
  }
  // get novaStavkaOk(){
  //   if(!this.grid) return false;
  //   let found = this.grid.dataSource.data().find((element)=>element.edit);
  //   return found;
  // }
  onIdentSelect(e, obj){
    let dataItem = e.sender.dataItem(e.item);
    if(dataItem){
      obj.stavka.jm = dataItem.jm;
      obj.stavka.koleta = dataItem.koleta;
      obj.stavka.poreskaStopa = dataItem.poreskaStopa;
      obj.stavka.poreskaOznaka = dataItem.poreskaOznaka;
      obj.stavka.ident.naziv = dataItem.naziv;
      if(!obj.stavka.odeljenje.id && this.porudzbenica.odeljenje.id) obj.stavka.odeljenje = this.porudzbenica.odeljenje;
      
      if (obj.stavka.rbr===this.porudzbenica.stavke.length) this.novaStavka();
      
      this.repo.find("Ident/GetPrice?id=" + dataItem.id)
        .then(res=>{
          obj.stavka.cena = res;
          obj.stavka.konacnaCena = res;
          obj.stavka.vrednost = obj.stavka.konacnaCena * obj.stavka.poruceno;
          //if(this.txtPoruceno) this.txtPoruceno.focus();
          let widget =  kendo.widgetInstance($("#numPoruceno" + obj.stavka.rbr), kendo.ui);
          if(widget) widget.focus();
        })
        .error(err => toastr.error(err.statusText));
    }
  }
  onIdentOpen (e, dis){
    e.sender.dataSource.read()
    this.cboKupac.refresh();
  }
  onIdentChange(e, dis){
    let dataItem = e.sender.dataItem(e.item);
    if(!dataItem){

    }
  }
  onRabatChange(obj, e){
    let porudzbenicastavka = obj.stavka;
    let rabat1 = porudzbenicastavka.rabat1;
    let rabat2 = porudzbenicastavka.rabat2;
    let rabat3 = porudzbenicastavka.rabat3;
    switch(e.sender.element[0].id){
      case "numRabat1": rabat1 = e.sender.value(); break;
      case "numRabat2": rabat2 = e.sender.value(); break;
      case "numRabat3": rabat3 = e.sender.value(); break;
    }
    this.kalkulacijaCene(porudzbenicastavka, rabat1, rabat2, rabat3);
    this.kalkulacijaRabata(porudzbenicastavka, rabat1, rabat2, rabat3);
  }
  onPorucenoChange(obj,e){
    let porudzbenicastavka = obj.stavka;
    let kolicina = e.sender.value();
    porudzbenicastavka.vrednost = porudzbenicastavka.konacnaCena * kolicina;
  }
  kalkulacijaCene(porudzbenicastavka, rabat1, rabat2, rabat3){
    try {
      let cena = porudzbenicastavka.cena;
      if (!rabat1) {
        porudzbenicastavka.konacnaCena = cena;
      }

      let cena1 = cena * (1 - rabat1 / 100);

      if (!rabat2 && cena1) {
        porudzbenicastavka.konacnaCena = cena1;
      }
      let cena2 = cena1 * (1 - rabat2 / 100);

      if (!rabat3 && cena2) {
        porudzbenicastavka.konacnaCena = cena2;
      }
      let cena3 = cena2 * (1 - rabat3 / 100);

      if (cena3) {
        porudzbenicastavka.konacnaCena = cena3;
      }
      porudzbenicastavka.vrednost = porudzbenicastavka.konacnaCena * porudzbenicastavka.poruceno;
    } catch (error) {
      console.log(error);
    }
  }
  kalkulacijaRabata(porudzbenicastavka,rabat1, rabat2, rabat3){
    try {
      let rabat = 0
      if (rabat1) {
        rabat = rabat + rabat1;
      }
      if (rabat2) {
        rabat = rabat + rabat2;
      }
      if (rabat3) {
        rabat = rabat + rabat3;
      }
      porudzbenicastavka.rabat = rabat;
    } catch (error) {
      console.log(error);
    }
  }
  potvrdi(obj, e) {
    obj.stavka.edit = false;
    $('[data-toggle="tooltip"]').tooltip('hide');
  }
  izmeni(obj, e) {
    obj.stavka.edit = true;
    $('[data-toggle="tooltip"]').tooltip('hide');
  }

  obrisi(obj, e) {
    let id  = null;
    if(obj.stavka.ident){
      if(obj.stavka.ident.id)
        id = obj.stavka.ident.id;
    }
      
    if((!id) && (this.porudzbenica.stavke.length === obj.stavka.rbr)) return;
      
    var index=this.porudzbenica.stavke.map((x)=>{ return x.rbr; }).indexOf(obj.stavka.rbr);
    if (confirm(`Da li želite da obrišete stavku ${index + 1}?`)) {
      if(index !== -1 && obj.stavka.id===0){
        this.porudzbenica.stavke.splice(index,1);
        this.porudzbenica.stavke.forEach((element, index)=>element.rbr=index+1); 
      }
    }
    $('[data-toggle="tooltip"]').tooltip('hide');
  }


  snimi(obj, e) {
    if(this.porudzbenica.stavke.length===1 && (!this.porudzbenica.stavke[0].ident && !(this.porudzbenica.stavke[0].ident !==null && !this.porudzbenica.stavke[0].ident.id))){
      toastr.error("Porudžbenica nema stavke");
      return;
    }
    if(!this.porudzbenica.kupac){
      toastr.error("Kupac nije odabran");
      return;
    }
    if(!this.porudzbenica.kupac.id){
      toastr.error("Kupac nije odabran");
      return;
    }
    if(!this.porudzbenica.mestoIsporuke){
      toastr.error("Mesto isporuke nije odabrano");
      return;
    }
    if(!this.porudzbenica.mestoIsporuke.id){
      toastr.error("Mesto isporuke nije odabrano");
      return;
    }
    if(this.porudzbenica.odeljenje){
      if(!this.porudzbenica.odeljenje){
        this.porudzbenica.odeljenje = null;
      }
    }

    let ok = true;
    this.porudzbenica.stavke.forEach((element, index)=>{
      if(((!element.ident && (element.ident && !element.ident.id)) && index + 1 !== this.porudzbenica.stavke.length) || element.poruceno ===0 ){
        ok = false
      }
      if(element.odeljenje){
        if(!element.odeljenje.id){
          element.odeljenje = null;
        }
      }
    });
    if(!ok){
      toastr.error("Stavke porudžbenice nisu ispravne");
      return;
    }
    



    //brisem poslednju stavku
    if(this.porudzbenica.stavke[this.porudzbenica.stavke.length-1].ident){
      if(!this.porudzbenica.stavke[this.porudzbenica.stavke.length-1].ident.id){
        this.porudzbenica.stavke.splice(this.porudzbenica.stavke.length-1, 1);
      }
    }else{
      this.porudzbenica.stavke.splice(this.porudzbenica.stavke.length-1, 1);
    }



    if (confirm("Da li želite da snimite izmene?")) {
      this.repo.post('Porudzbenica', this.porudzbenica)
        .then(res => {
          toastr.success("Uspešno snimljeno");
          this.porudzbenica = res;
        })
        .error(err => toastr.error(err.statusText));
    }
  }
  // proba(){
  //   console.log(1);
  // }
}
