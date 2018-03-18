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
      Map(x=>x.Ptt);
      Map(x=>x.Adresa);
      Map(x=>x.Drzava);
      Map(x => x.Pib);
      Map(x => x.MaticniBroj);
      Map(x => x.Payer).CustomSqlType("nvarchar(30)");
      Map(x => x.Buyer);
      Map(x => x.Supplier);
      Map(x => x.Warehouse);
      Map(x => x.Dept);
      References(x => x.OdgovornoLice).Cascade.None();
    }
  }
}
