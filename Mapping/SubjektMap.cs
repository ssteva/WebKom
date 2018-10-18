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
  public class SubjektMap : EntitetMap<Subjekt>
  {
    public SubjektMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tSubjekt");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Assigned();
      Map(x=>x.Sifra).Unique();
      Map(x=>x.Naziv);
      Map(x=>x.Adresa);
      Map(x=>x.Drzava);
      Map(x => x.Pib);
      Map(x => x.MaticniBroj);
      Map(x => x.Kupac);
      Map(x => x.Dobavljac);
      Map(x => x.Skladiste);
      Map(x => x.Odeljenje);
      Map(x => x.DanaZaPlacanje);
      References(x => x.OdgovornoLice).Column("KorisnikId").Cascade.None().Not.Update();
      References(x => x.Valuta).Column("ValutaId").Cascade.None().Not.Update();
      References(x => x.Mesto).Column("MestoId").Cascade.None().Not.Update();
      References(x => x.Platilac).Column("PlatilacId").Cascade.None().Not.Update();
      References(x => x.MestoIsporuke).Column("MestoIsporukeId").Cascade.None().Not.Update();
    }
  }
}
