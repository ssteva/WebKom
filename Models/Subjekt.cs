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
    public virtual bool Kupac { get; set; }
    public virtual bool Dobavljac { get; set; }
    public virtual bool Skladiste { get; set; }
    public virtual bool Odeljenje { get; set; }
    public virtual string Valuta { get; set; }
    public virtual string Paritet { get; set; }
    public virtual Korisnik OdgovornoLice { get; set; }
    public virtual Subjekt Platilac { get; set; }


}
