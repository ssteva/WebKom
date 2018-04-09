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
  public class StatusMap : EntitetMap<Status>
  {
    public StatusMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tStatus");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x=>x.Oznaka).CustomSqlType("varchar(5)");
      Map(x=>x.Vrsta).CustomSqlType("varchar(20)");
      Map(x=>x.Naziv);
    }
  }
}
