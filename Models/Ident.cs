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
public class Ident : Entitet
{
    public virtual int Id { get; set; }
    public virtual string Sifra { get; set; }
    public virtual string Naziv { get; set; }        
    public virtual string Primarna { get; set; }
    public virtual string Sekundarna { get; set; }
    public virtual string Barkod { get; set; }
    public virtual string Jm { get; set; }
    public virtual decimal Koleta { get; set; }
    public virtual int PoreskaStopa { get; set; }
    public virtual string PoreskaOznaka { get; set; }
    public virtual decimal CenaBezPdv { get; set; }
    public virtual decimal Cena { get; set; }
}
