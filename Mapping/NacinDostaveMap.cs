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
  public class NacinDostaveMap : EntitetMap<NacinDostave>
  {
    public NacinDostaveMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tNacinDostave");
      Id(x => x.Id).UnsavedValue("").GeneratedBy.Assigned();
      Map(x=>x.Naziv);
    }
  }
}
