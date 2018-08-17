using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Transform;
using webkom.Helper;
using webkom.Helper.Kendo;
using webkom.Models;
using webkom.Models.DTO;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;
using ISession = NHibernate.ISession;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webkom.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class LagerController : Controller
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public LagerController(ISession session, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LagerController>();
            _session = session;
        }

        // [HttpGet]
        // public ActionResult Get()
        // {

        //   var upit = _session.QueryOver<Meni>();
        //   upit.OrderBy(m=>m.Rbr);
        //   var lista = upit.List<Meni>();

        //   return Ok(lista);
        // }
        [HttpPost]
        [Route("[Action]")]
        public KendoResult<Lager> PregledGrid([FromBody]KendoRequest kr)
        {
            var query = _session.CreateSQLQuery("exec Lager :count, :take, :skip, :sortfield, :sortdir, :skladiste, :ident");
            var rows = _session.CreateSQLQuery("exec Lager :count, :take, :skip, :sortfield, :sortdir, :skladiste, :ident");
            query.SetParameter("count", false, NHibernateUtil.Boolean);
            query.SetParameter("take", kr.Take, NHibernateUtil.Int32);
            query.SetParameter("skip", kr.Skip, NHibernateUtil.Int32);
            query.SetParameter("sortfield", "Ident", NHibernateUtil.String);
            query.SetParameter("sortdir", "ASC", NHibernateUtil.String);
            query.SetParameter("skladiste", null, NHibernateUtil.String);
            query.SetParameter("ident", null, NHibernateUtil.String);

            rows.SetParameter("count", true, NHibernateUtil.Boolean);
            rows.SetParameter("take", null, NHibernateUtil.Int32);
            rows.SetParameter("skip", null, NHibernateUtil.Int32);
            rows.SetParameter("sortfield", null, NHibernateUtil.String);
            rows.SetParameter("sortdir", null, NHibernateUtil.String);
            rows.SetParameter("skladiste", null, NHibernateUtil.String);
            rows.SetParameter("ident", null, NHibernateUtil.String);

            if (kr.Filter != null && kr.Filter.Filters.Any())
            {
                foreach (FilterDescription filter in kr.Filter.Filters)
                {
                    query.SetParameter(filter.Field, filter.Value);
                    rows.SetParameter(filter.Field, filter.Value);
                }
            }

            if (kr.Sort != null && kr.Sort.Count > 0)
            {
                foreach (Sort sort in kr.Sort)
                {
                    rows.SetParameter("sortfield", sort.Field);
                    rows.SetParameter("sortdir", sort.Dir);
                }
            }

            query.Future<Lager>();
            rows.Future<int>();
            var redova = rows.UniqueResult() as int? ?? 0;

            query.ExecuteUpdate();
            query.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Lager)));
            var lista = query.List<Lager>();

            var res = new KendoResult<Lager>
            {
                Data = lista,
                Total = redova
            };
            return res;

        }
    }
}
