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
  public class MeniMap : ClassMap<Meni>
  {


    public MeniMap()
    {
      Table("tMeni");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x => x.Rbr);
      Map(x => x.RouteString);
      Map(x => x.Name);
      Map(x => x.ModuleId);
      Map(x => x.Href);
      Map(x => x.Title);
      Map(x => x.Nav);
      Map(x => x.Auth);
      References(x=>x.Master).Cascade.None();
      References(x=>x.Settings).Cascade.All();
      HasMany(x=>x.Podmeni).KeyColumn("NadMeniId").Cascade.None();
    }
  }
}

