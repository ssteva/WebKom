using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using webkom.Mapping;
using webkom.Models;

namespace webkom.Mapping
{
  public class ValutaMap : EntitetMap<Valuta>
  {
    public ValutaMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tValuta");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x=>x.Oznaka).CustomSqlType("varchar(3)");
      Map(x=>x.Naziv);
    }
  }
}
