﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace webkom.Models
{
    public class Porudzbenica : Entitet
    {
        public Porudzbenica()
        {
            Stavke = new HashSet<PorudzbenicaStavka>();
        }
        public virtual int Id { get; set; }
        //public virtual string Vrsta { get; set; }
        //public virtual string Sifra { get; set; }
        public virtual string Tip { get; set; }
        public virtual int Broj { get; set; }

        public virtual string BrojPanteon { get; set; }
        public virtual DateTime Datum { get; set; }
        public virtual DateTime DatumVazenja { get; set; }
        public virtual DateTime? DatumIsporuke { get; set; }
        public virtual int DanaZaPlacanje { get; set; }

        public virtual decimal Kurs { get; set; }
        public virtual string Dokument1 { get; set; }
        public virtual string Dokument2 { get; set; }
        public virtual string KontaktOsobaKupca { get; set; }
        public virtual string KontaktOsobaMestaIsporuke { get; set; }
        public virtual string Napomena { get; set; }
        public virtual int StatusPanteon { get; set; }
        public virtual Guid? Uid { get; set; }
        public virtual DateTime? DatumDokument1 { get; set; }
        public virtual DateTime? DatumDokument2 { get; set; }
        //public virtual Subjekt Platilac {get;set;}
        public virtual Subjekt Kupac { get; set; }
        public virtual Valuta Valuta { get; set; }
        public virtual NacinDostave NacinDostave { get; set; }
        public virtual NacinPlacanja NacinPlacanja { get; set; }
        public virtual Subjekt Skladiste { get; set; }
        public virtual Subjekt MestoIsporuke { get; set; }
        public virtual Subjekt Odeljenje { get; set; }
        public virtual Mesto Region { get; set; }
        public virtual Status Status { get; set; }
        public virtual Korisnik Referent { get; set; }
        public virtual ICollection<PorudzbenicaStavka> Stavke { get; set; }

    }
}


