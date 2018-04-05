using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using webkom.Models;

namespace webkom.Mapping
{
    public class ParametarMap :  ClassMap<Parametar>
    {

        protected ParametarMap()
        {
            //SchemaAction.None();
            Table("tParametri");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Klasa);
            Map(x => x.Vrsta);
            Map(x => x.Naziv);
            Map(x => x.Vredpar1);
            Map(x => x.Vredpar2);
            Map(x => x.Vredpar3);
            Map(x => x.Vredpar4);
            Map(x => x.Vredpar5);
            Map(x => x.Vredpar6);
            Map(x => x.Vredpar7);
            Map(x => x.Vredpar8);
        }
    }
}
