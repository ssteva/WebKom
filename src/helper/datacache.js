import {inject} from 'aurelia-framework'
import {Endpoint} from 'aurelia-api';

@inject(EntityManager, Endpoint.of())
export class DataCache {
    constructor(em, repo) {
        this.em = em;
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
            this.repo.find('Parametar?naziv=' + naziv )
                .then(result => {
                    this.parametri = result;
                    //toastr.info('Učitani parametri ' + result.length);
                    resolve(this.parametri);
                })
                .catch(err => reject(err));
        });
        return promise;
    }
    
}
