﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using webkom.Models;

namespace webkom.Mapping
{
  public class IdentMap : EntitetMap<Ident>
  {
    public IdentMap()
    {
         base.MapSubClass();
    }
    protected override void MapSubClass()
    {
      Table("tIdent");
      Id(x => x.Id).UnsavedValue(0).GeneratedBy.Assigned();
      Map(x=>x.Sifra).Unique();
      Map(x=>x.Naziv);
      Map(x=>x.Primarna).CustomSqlType("varchar(16)");
      Map(x=>x.Sekundarna).CustomSqlType("varchar(16)");
      Map(x => x.Barkod).CustomSqlType("varchar(20)");
      Map(x=>x.Jm).CustomSqlType("varchar(3)");
      Map(x => x.Koleta).CustomSqlType("decimal(19,6)");
      Map(x => x.PoreskaStopa);
      Map(x => x.PoreskaOznaka).CustomSqlType("char(2)");
      Map(x => x.CenaBezPdv).CustomSqlType("float");
      Map(x => x.Cena).CustomSqlType("decimal(12,2)");
    }
  }
}
