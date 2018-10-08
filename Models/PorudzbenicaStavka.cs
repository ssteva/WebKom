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
        //public virtual int Otprema { get; set; }
        public virtual decimal Koleta { get; set; }
        public virtual string Jm { get; set; }
        public virtual int PoreskaStopa { get; set; }
        public virtual string PoreskaOznaka { get; set; }
        public virtual decimal Rabat1 { get; set; }
        public virtual decimal Rabat2 { get; set; }
        public virtual decimal Rabat3 { get; set; }

        public virtual decimal CenaBezPdv { get; set; }
        public virtual decimal Cena { get; set; }
        public virtual decimal KonacnaCena { get; set; }
        public virtual decimal Vrednost
        {
            get
            {
                try
                {
                    return Poruceno * KonacnaCena;
                }
                catch
                {
                    return 0;
                }
            }
            set { }
        }
        public virtual decimal Placanje
        {
            get
            {
                try
                {
                    return Vrednost * (100 + PoreskaStopa) / 100;
                }
                catch
                {
                    return 0;
                }
            }
            set { }
        }
        public virtual decimal Pdv
        {
            get
            {
                try
                {
                    return Placanje - Vrednost;
                }
                catch
                {
                    return 0;
                }
            }
            set { }
        }
        public virtual decimal Rabat
        {
            get
            {
                try
                {
                    var rabat = 0m;
                    decimal um1 = 0;
                    decimal um2 = 0;
                    decimal um3 = 0;
                    if (Rabat1 != 0)
                    {
                        um1 = 1 - (Rabat1 / 100m);
                    }
                    if (Rabat2 != 0)
                    {
                        um2 = 1 - (Rabat2 / 100m);
                    }
                    if (Rabat3 != 0)
                    {
                        um3 = 1 - (Rabat3 / 100m);
                    }

                    if (um1 != 0 && um2 != 0 && um3 != 0)
                        rabat = (1m - (um1 * um2 * um3)) * 100m;
                    if (um1 != 0 && um2 == 0 && um3 == 0)
                        rabat = (1 - (um1)) * 100m;
                    if (um1 != 0 && um2 != 0 && um3 == 0)
                        rabat = (1 - (um1 * um2)) * 100m;
                    if (um1 == 0 && um2 != 0 && um3 != 0)
                        rabat = (1 - (um2 * um3)) * 100m;
                    if (um1 != 0 && um2 == 0 && um3 != 0)
                        rabat = (1 - (um1 * um3)) * 100;
                    if (um1 == 0 && um2 == 0 && um3 != 0)
                        rabat = (1 - (um3)) * 100;
                    if (um1 == 0 && um2 != 0 && um3 == 0)
                        rabat = (1 - (um2)) * 100;
                    return rabat;
                }
                catch
                {
                    return 0;
                }
            }
            set { }
        }
        public virtual Subjekt Odeljenje { get; set; }
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
