using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webkom.Controllers.api
{
    public static class Helper
    {
        public static int RedniBroj(ISession _session, string tip)
        {
            var query = _session.CreateSQLQuery("exec RedniBroj :tip");
            query.SetCacheMode(CacheMode.Ignore);
            query.SetParameter("tip", tip);
            //query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(int)));
            var result = query.UniqueResult<int>();
            return result;
        }
    }
}
