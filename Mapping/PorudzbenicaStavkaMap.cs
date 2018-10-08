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
            Map(x => x.Rbr);
            Map(x => x.Poruceno);
            //Map(x => x.Otprema);
            Map(x => x.Jm).CustomSqlType("varchar(3)");
            Map(x => x.Koleta).CustomSqlType("decimal(19,6)");
            Map(x => x.PoreskaStopa);
            Map(x => x.PoreskaOznaka).CustomSqlType("char(2)");
            Map(x => x.Rabat1).CustomSqlType("numeric(8,2)");
            Map(x => x.Rabat2).CustomSqlType("numeric(8,2)");
            Map(x => x.Rabat3).CustomSqlType("numeric(8,2)");
            Map(x => x.Rabat).Not.Update().Not.Insert();
            Map(x => x.CenaBezPdv).CustomSqlType("float");
            Map(x => x.Cena).CustomSqlType("decimal(12,2)");
            Map(x => x.KonacnaCena).CustomSqlType("decimal(12,2)");
            References(x => x.Porudzbenica).Column("PorudzbenicaId").Cascade.None();
            References(x => x.Ident).Column("IdentId").Cascade.None();
            References(x => x.Odeljenje).Column("OdeljenjeId").Cascade.None();
        }
    }
}
