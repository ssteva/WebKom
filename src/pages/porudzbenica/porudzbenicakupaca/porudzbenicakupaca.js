import {Endpoint} from 'aurelia-api';
import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {DialogController, DialogService} from 'aurelia-dialog';
import {DataCache} from 'helper/datacache';
import 'kendo/js/kendo.combobox';
import * as toastr from 'toastr';

@inject(AuthService, DataCache, DialogService, Endpoint.of())
export class Porudzbenica {
  roles = ["Komercijalista", "Supervizor", "Administrator"];
  porudzbenica = null;
  skladista = [];
  odeljenja = [];
  constructor(authService, dc, dialogService, repo) {
    this.authService = authService;
    this.repo = repo;
    this.dialogService = dialogService;
    this.dc = dc;
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
          this.repo.post('Subjekat/ListaMestaIsporukeCombo', o.data)
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
          this.repo.post('Subjekat/ListaKupacaCombo', o.data)
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
      this.repo.findOne("Porudzbenica/PorudzbenicaStavka?id=" + params.id + "&vrsta=PORK"),
      this.dc.getSkladista(),
      this.dc.getOdeljenja()
    ];

    return Promise.all(promises)
      .then(res => {
        this.porudzbenica = res[0];
        this.porudzbenicastavka = res[1];
        this.skladista = res[2];
        this.odeljenja = res[3]
      })
      .catch(err => toastr.error(err.statusText));

  }
  onMestoIsporukeSelect (e){
    let dataItem = this.cboMestoIsporuke.dataItem(e.item);
    if(!this.porudzbenica.kupac.id){
      if(dataItem.platilac){
        this.porudzbenica.kupac = dataItem.platilac;
      }
    }
  }
  onKupacSelect (e){
    let dataItem = this.cboMestoIsporuke.dataItem(e.item);
    if(dataItem.id){
      
    }
  }
}
