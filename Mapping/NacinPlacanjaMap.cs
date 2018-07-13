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
  public class NacinPlacanjaMap : EntitetMap<NacinPlacanja>
  {
    public NacinPlacanjaMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tNacinPlacanja");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x=>x.Sifra);
      Map(x=>x.Naziv);
    }
  }
}
