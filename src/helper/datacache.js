import {inject} from 'aurelia-framework'
import {Endpoint} from 'aurelia-api';
//import {EntityManager} from 'aurelia-orm';
@inject(Endpoint.of())
export class DataCache {
  constructor(repo) {
    //this.em = em;
    this.repo = repo;
    console.log('Data cache constructor');
  }
  getParametri() {
    var promise = new Promise((resolve, reject) => {
      if (!this.parametri) {
        this.repo.find('Parametar')
          .then(result => {
            this.parametri = result;
            toastr.info('Učitani parametri ' + result.length);
            resolve(this.parametri);
          })
          .catch(err => reject(err));
      } else {
        resolve(this.parametri);
      }
    });
    return promise;
  }
  getParametar(naziv) {
    var promise = new Promise((resolve, reject) => {
      this.repo.find('Parametar?naziv=' + naziv)
        .then(result => {
          this.parametri = result;
          //toastr.info('Učitani parametri ' + result.length);
          resolve(this.parametri);
        })
        .catch(err => reject(err));
    });
    return promise;
  }
  getSkladista() {
    var promise = new Promise((resolve, reject) => {
		if (!this.skladista) {
		  this.repo.find('Subjekat/Skladista')
			.then(result => {
			  this.skladista = result;
			  resolve(this.skladista);
			})
			.catch(err => reject(err));
		} else {
		  resolve(this.skladista);
		}
	  });
	  return promise;
  }
  getOdeljenja() {
    var promise = new Promise((resolve, reject) => {
		if (!this.odeljenja) {
		  this.repo.find('Subjekat/Odeljenja')
			.then(result => {
			  this.odeljenja = result;
			  resolve(this.odeljenja);
			})
			.catch(err => reject(err));
		} else {
		  resolve(this.odeljenja);
		}
	  });
	  return promise;
  }
  getMestaIsporuke() {
    var promise = new Promise((resolve, reject) => {
		if (!this.mestaIsporuke) {
		  this.repo.find('Subjekat/MestaIsporuke')
			.then(result => {
			  this.skladista = result;
			  resolve(this.mestaIsporuke);
			})
			.catch(err => reject(err));
		} else {
		  resolve(this.mestaIsporuke);
		}
	  });
	  return promise;
  }
}
