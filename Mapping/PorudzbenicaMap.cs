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
            //.Map(x => x.Vrsta).CustomSqlType("varchar(20)");
            //Map(x => x.Sifra).CustomSqlType("varchar(20)");
            Map(x => x.Tip).CustomSqlType("varchar(20)");
            Map(x => x.Broj);
            Map(x => x.BrojPanteon).CustomSqlType("varchar(50)");
            Map(x => x.Datum).CustomSqlType("date");
            Map(x => x.DatumVazenja).CustomSqlType("date");
            Map(x => x.DatumIsporuke).CustomSqlType("date");
            Map(x => x.DanaZaPlacanje);
            Map(x => x.Kurs).CustomType("decimal(12,4)");
            Map(x => x.Dokument1);
            Map(x => x.Dokument2);
            Map(x => x.DatumDokument1).CustomSqlType("date");
            Map(x => x.DatumDokument2).CustomSqlType("date");
            Map(x => x.StatusPanteon);
            Map(x => x.Uid);
            Map(x => x.Napomena).CustomSqlType("nvarchar(1000)");
            //References(x => x.Platilac).Column("PlatilacId").Cascade.None().Not.Update();
            References(x => x.Valuta).Column("ValutaId").Cascade.None().Not.Update().LazyLoad();
            References(x => x.NacinDostave).Column("NacinDostaveId").Cascade.None().Not.Update();
            References(x => x.NacinPlacanja).Column("NacinPlacanjaId").Cascade.None().Not.Update();
            References(x => x.Kupac).Column("KupacId").Cascade.None().Not.Update();
            References(x => x.Skladiste).Column("SkladisteId").Cascade.None().Not.Update();
            References(x => x.Odeljenje).Column("OdeljenjeId").Cascade.None().Not.Update();
            References(x => x.MestoIsporuke).Column("MestoIsporukeId").Cascade.None().Not.Update();
            References(x => x.Region).Column("MestoId").Cascade.None();
            References(x => x.Referent).Column("KorisnikId").Cascade.None().Not.Update();
            References(x => x.Status).Cascade.None();
            HasMany(x => x.Stavke)
              .Inverse()
              .AsSet()
              .Cascade.None();
        }
    }
}
