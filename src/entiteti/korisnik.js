import {Entity, validatedResource, association, type} from 'aurelia-orm';
import {ensure} from 'aurelia-validation';

@validatedResource('korisnik')
export class KorisnikEntity extends Entity {
    //@ensure(it => it.isNotEmpty().hasLengthBetween(3, 20))
    id=0;
	panteonId=0;
	korisnickoIme="";
    ime = "";
    prezime = "";
    email="";
    uloga="";
    obrisan = false;
	aktivan = true;
}


