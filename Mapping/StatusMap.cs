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
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Assigned();
      Map(x=>x.Vrsta);
      Map(x=>x.SkraceniNaziv);
      Map(x=>x.Naziv);
    }
  }
}
