//Sifra
// naziv
// adresa
// posta
// drzava
// pib
// maticni broj
// telefon
// e mail
// odgovorna osoba
// tip subjekta
// kontakt osoba
//platilac
using webkom.Models;
public class Subjekt : Entitet
{
    public virtual int Id { get; set; }
    public virtual string Sifra { get; set; }
    public virtual string Naziv { get; set; }
    public virtual string Adresa { get; set; }
    public virtual string Ptt { get; set; }
    public virtual string Drzava { get; set; }
    public virtual string Pib { get; set; }
    public virtual string MaticniBroj { get; set; }
    public virtual string Payer { get; set; }
    public virtual bool Buyer { get; set; }
    public virtual bool Supplier { get; set; }
    public virtual bool Warehouse { get; set; }
    public virtual bool Dept { get; set; }
    public virtual Korisnik OdgovornoLice { get; set; }


}
