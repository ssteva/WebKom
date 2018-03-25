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
  public class MestoMap : EntitetMap<Mesto>
  {
    public MestoMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tMesto");
      Id(x => x.Id).UnsavedValue("").GeneratedBy.Assigned();
      Map(x=>x.Naziv);
      Map(x=>x.Region);
    }
  }
}
