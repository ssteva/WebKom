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
    public class PorudzbenicaMap : EntitetMap<Porudzbenica>
    {
        public PorudzbenicaMap()
        {
            base.MapSubClass();
        }
        protected override void MapSubClass()
        {
            Table("tPorudzbenica");
            Id(x => x.Id).UnsavedValue(0).GeneratedBy.Identity();
            Map(x => x.Vrsta).CustomSqlType("varchar(20)");
            Map(x => x.Broj);
            Map(x => x.Datum).CustomSqlType("date");
            Map(x => x.DatumVazenja).CustomSqlType("date");
            Map(x => x.DatumIsporuke).CustomSqlType("date");
            Map(x => x.Kurs).CustomType("decimal(12,4)");
            Map(x => x.Dokument1);
            Map(x => x.Dokument2);
            Map(x => x.DatumDokument1).CustomSqlType("date");
            Map(x => x.DatumDokument2).CustomSqlType("date");
            References(x => x.Valuta).Cascade.None().Not.Update();
            References(x => x.Kupac).Column("KupacId").Cascade.None().Not.Update();
            References(x => x.Platilac).Column("PlatilacId").Cascade.None().Not.Update();
            //References(x => x.Skladiste).Column("SkladisteId").Cascade.None().Not.Update();
            References(x => x.MestoIsporuke).Column("MestoIsporukeId").Cascade.None().Not.Update();
            References(x => x.Region).Cascade.None().Not.Update();
            References(x => x.Referent).Cascade.None().Not.Update();
            HasMany(x => x.Stavke)
              .Inverse()
              .AsSet()
              .Cascade.All();
        }
    }
}
