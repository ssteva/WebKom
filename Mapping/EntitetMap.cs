using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using webkom.Models;

namespace webkom.Mapping
{
    public class EntitetMap<T> : ClassMap<T > where T : Entitet
    {
        public EntitetMap()
        {
            MapSubClass();
            Map(p => p.UserCreated).CustomSqlType("nvarchar(50)");
            Map(p => p.DateCreated)
                .CustomSqlType("datetime")
                .Not.Nullable()
                .Default("getDate()")
                .Not.Insert()
                .Not.Update();
            Map(p => p.UserModified).CustomSqlType("nvarchar(50)");
            Map(p => p.DateModified).CustomSqlType("datetime").Default("getDate()");
            Map(p => p.Obrisan).Default("0");
            Map(p => p.Zakljucan).Default("0");
        }
        protected virtual void MapSubClass()
        {
        }
    }
}
