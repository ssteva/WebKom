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
  public class NacinProdajeMap : EntitetMap<NacinProdaje>
  {
    public NacinProdajeMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tNacinProdaje");
      Id(x => x.Id).UnsavedValue("").GeneratedBy.Assigned();
      Map(x=>x.Naziv);
    }
  }
}
