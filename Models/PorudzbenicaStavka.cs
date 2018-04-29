using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks;

// Redni broj
// Ident
// Naziv
// kolicina
// koleta
// jedinicia mere
// rabat1
// rabat2
// rabat3
// rabat(prizuje ukupni rabat-kaskadni rabat prva tri rabata)
// cena
// pdv
// pdv stopa
// odelenje
// za placanje

namespace webkom.Models
{
    public class PorudzbenicaStavka : Entitet
    {
        public virtual int Id { get; set; }
        public virtual Porudzbenica Porudzbenica { get; set; }
        public virtual int Rbr { get; set; }
        public virtual Ident Ident { get; set; }
        public virtual int Poruceno { get; set; }
        public virtual int Otprema { get; set; }
        public virtual int Primljeno { get; set; }
        public virtual decimal Koleta { get; set; }
        public virtual string Jm { get; set; }
        public virtual int PoreskaStopa { get; set; }
        public virtual string PoreskaOznaka { get; set; }
        public virtual decimal Rabat1 { get; set; }
        public virtual decimal Rabat2 { get; set; }
        public virtual decimal Rabat3 { get; set; }
        public virtual decimal Rabat { get; set; }
        public virtual decimal CenaBezPdv { get; set; }
        public virtual decimal Cena { get; set; }
        public virtual decimal KonacnaCena { get; set; }
    }
}



//Redni broj
// Ident
// Naziv
// kolicina
// koleta
// jedinicia mere
// rabat1
// rabat2
// rabat3
// rabat(prizuje ukupni rabat-kaskadni rabat prva tri rabata)
// cena
// pdv
// pdv stopa
// odelenje
// za placanje
