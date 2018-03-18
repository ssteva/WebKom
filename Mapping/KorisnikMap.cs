using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using webkom.Models;

namespace webkom.Mapping
{
  public class KorisnikMap : EntitetMap<Korisnik>
  {
    public KorisnikMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tKorisnik");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x=>x.PanteonId);
      Map(x=>x.KorisnickoIme).Unique();
      Map(x=>x.Ime);
      Map(x=>x.Prezime);
      Map(x=>x.Naziv);
      Map(x => x.Email);
      Map(x=>x.Lozinka).CustomSqlType("binary(64)");
      Map(x => x.So);
      Map(x=>x.Uloga);
      Map(x=>x.Aktivan);

      //References(x => x.Meni).Cascade.None();
    }
  }
}
