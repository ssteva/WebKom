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
  public class PorudzbenicaStavkaMap : EntitetMap<PorudzbenicaStavka>
  {
    public PorudzbenicaStavkaMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tPorudzbenicaStavka");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x=>x.Rbr);
      Map(x=>x.Kolicina);
      Map(x=>x.Koleta);
      Map(x=>x.Jm).CustomSqlType("varchar(3)");
      Map(x=>x.Rabat1);
      Map(x=>x.Rabat2);
      Map(x=>x.Rabat3);
      Map(x=>x.Rabat).Not.Update().Not.Insert();
    }
  }
}
