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
  public class MeniSettingsMap : ClassMap<MeniSettings>
  {
    
    public MeniSettingsMap()
    {
      Table("tMeniSettings");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
      Map(x => x.Ikona);      
      Map(x => x.RolesString);
      //References(x => x.Meni).Cascade.None();
    }
  }
}
